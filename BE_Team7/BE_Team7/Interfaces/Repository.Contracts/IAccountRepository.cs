using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;
using BE_Team7.Models;

namespace api.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetUserById(string id);

    }
}