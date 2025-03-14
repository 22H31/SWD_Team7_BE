﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;
using api.Services;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using api.Mappers;
using BE_Team7.Models;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Sevices;
using AutoMapper;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        public readonly IEmailService _emailService;
        public readonly IAuthRepository _authService;
        private readonly IAccountRepository _accountRepository;
        private readonly SignInManager<User> _signinManager;
        private readonly IAuthRepository _authRepo;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<User> userManager, ITokenService tokenService, IEmailService emailService
        , SignInManager<User> signInManager, IAuthRepository authService, IAccountRepository accountRepository, IAuthRepository authRepo, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _signinManager = signInManager;
            _authService = authService;
            _accountRepository = accountRepository;
            _authRepo = authRepo;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var user = await _authRepo.ValidateUserAsync(loginDto.Username, loginDto.Password);
                if (user == null) return Unauthorized("Invalid username or password");
                if (!user.EmailConfirmed) return Unauthorized("Please confirm your email before logging in");
                var result = await _authService.LoginAsync(user, loginDto.Password);
                if (result == null) return Unauthorized("Invalid username or password");
                var roles = await _authRepo.GetRolesAsync(user);
                var token = _tokenService.CreateToken(user, roles);
                var userLoginDto = _mapper.Map<LoginResponseDto>(user);
                userLoginDto.IsLogedIn = true;
                userLoginDto.JwtToken = token;
                userLoginDto.Roles = roles;
                return Ok(userLoginDto);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("confirm-email")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ConfirmEmail(string email)
        {

            var result = await _authService.ConfirmEmailAsync(email);
            if (result == "Invalid email") return BadRequest("Cannot confirm your email");
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    NormalizedUserName = registerDto.UserName.ToUpper(),
                    NormalizedEmail = registerDto.Email.ToUpper(),
                    EmailConfirmed = false,
                    PhoneNumber = registerDto.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    Name = registerDto.Name,
                    CreatedAt = DateTime.UtcNow,
                };

                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);
                if (!createdUser.Succeeded)
                {
                    return StatusCode(500, createdUser.Errors);
                }

                // Kiểm tra role
                string roleName = "USER";
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!createRoleResult.Succeeded)
                    {
                        return StatusCode(500, createRoleResult.Errors);
                    }
                }

                // Gán role
                var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine($"Lỗi khi gán role: {error.Description}");
                    }
                    return StatusCode(500, roleResult.Errors);
                }

                // Gửi email xác nhận
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { email = user.Email }, Request.Scheme);
                var emailContent = $"Please confirm your account by clicking <a href=\"{confirmationLink}\">here</a>";
                await _emailService.SendEmailAsync(user.Email, "Confirm your email", emailContent);

                return Ok("Registration successful. Please check your email to confirm your account.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserForChangePasswordDto passwordDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var tokenReset = await _authService.ForgotPasswordAsync(passwordDto.Email);
                if (tokenReset == null) return BadRequest("Invalid Email");
                // Tạo liên kết xác nhận email và token
                var confirmationLink = Url.Action("NewPassWord", "Account", new { email = passwordDto.Email, token = tokenReset }, Request.Scheme);
                var emailContent = $"Please confirm your account by clicking <a href=\"{confirmationLink}\">here</a>";
                await _emailService.SendEmailAsync(passwordDto.Email, "Create new password", emailContent);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            return Ok("Please check your email to create a new password");
        }

        [HttpGet("new-password")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> NewPassword(string email, string token)
        {
            var result = await _authService.NewPasswordAsync(email, token);
            if (result == null) return BadRequest("Failed to create new password");
            await _emailService.SendEmailAsync(email, "Your new password", $"Your password has been reset. Your new password is: {result}");
            return Ok("Password has been reset successfully.");
        }
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.ChangePasswordUSerAsync(changePassword, User);

            if (result == "Password changed successfully.")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAccountAll()
        {
            var accounts = await _accountRepository.GetAllAsync();
            if (accounts.Count() == 0)
                return BadRequest("There are no accounts available to retrieve information from the server.");
            var accountDto = accounts.Select(a => a.ToAccountDto());
            return Ok(accountDto);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Không có token" });
            }

            // Vô hiệu hóa token nhưng không lưu vào database
            TokenRevocationService.RevokeToken(token);

            return Ok(new { message = "Đăng xuất thành công, token đã bị vô hiệu hóa" });
        }

    }
}