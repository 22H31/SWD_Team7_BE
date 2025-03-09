using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BE_Team7.Models;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7;
using api.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;


namespace api.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signinManager;
        private readonly AppDbContext _context;

        public AuthRepository(UserManager<User> userManager, IConfiguration config, ITokenService tokenService, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _tokenService = tokenService;
            _signinManager = signInManager;
            _context = context;
        }

        public async Task<string> LoginAsync(User appUser, string pwd)
        {
            var result = await _signinManager.CheckPasswordSignInAsync(appUser, pwd, false);
            if (!result.Succeeded) return null;
            await _userManager.UpdateAsync(appUser);
            return "ok";
        }
        public async Task<string> ChangePasswordUSerAsync(ChangePassword changePassword, ClaimsPrincipal user)
        {
            try
            {
                var appUser = await _userManager.FindByEmailAsync(user.FindFirst(ClaimTypes.Email)?.Value);
                if (appUser == null)
                {
                    return "User not found";
                }
                var result = await _userManager.ChangePasswordAsync(appUser, changePassword.CurrentPassword, changePassword.NewPassword);

                if (result.Succeeded)
                {
                    return "Password changed successfully.";
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            return "Wrong password";
        }
        public async Task<string> ConfirmEmailAsync(string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower());
            if (user == null) return "Invalid email";
            user.EmailConfirmed = true;
            await _context.SaveChangesAsync();
            return "Email confirmed successfully";
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<string> NewPasswordAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var newPassWord = GenerateRandomPassword();
            var result = await _userManager.ResetPasswordAsync(user, token, newPassWord);
            if (!result.Succeeded) return null;
            return newPassWord;
        }

        private string GenerateRandomPassword()

        {
            var random = new Random();
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
            var length = 12;
            var password = new string(Enumerable.Range(0, length)
                                                .Select(x => validChars[random.Next(validChars.Length)])
                                                .ToArray());
            return password;
        }


        private ClaimsPrincipal? GetTokenPrincipal(string token)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:SigningKey").Value));

            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = securityKey,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["JWT:Audience"],
            };
            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            // Tìm user theo Username
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return null;

            // Kiểm tra password
            var result = await _signinManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) return null;

            // Trả về user nếu đăng nhập thành công
            return user;
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}