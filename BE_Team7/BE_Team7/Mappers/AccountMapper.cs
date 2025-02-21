using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;
using BE_Team7.Models;

namespace api.Mappers
{
    public static class AccountMapper
    {
        public static AccountDto ToAccountDto(this User user)
        {
            return new AccountDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

    }
}