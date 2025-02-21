using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using BE_Team7.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;
        public AccountRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
    }
}