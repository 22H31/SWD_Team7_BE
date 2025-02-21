using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api.Dtos.Account
{
    public class AccountDto
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }

    }
}