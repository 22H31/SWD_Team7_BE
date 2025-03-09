using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;
using BE_Team7.Models;
using Microsoft.AspNetCore.Identity;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IAuthRepository
    {
        Task<User?> ValidateUserAsync(string username, string password);
        public Task<IList<string>> GetRolesAsync(User user);
        public Task<string> LoginAsync(User appUser, string pwd);
        public Task<string> ChangePasswordUSerAsync(ChangePassword changePassword, ClaimsPrincipal user);
        public Task<string> ConfirmEmailAsync(string email);
        public Task<string> ForgotPasswordAsync(string email);
        public Task<string> NewPasswordAsync(string email, string token);


    }
}