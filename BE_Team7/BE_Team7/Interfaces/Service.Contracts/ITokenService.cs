using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using BE_Team7.Models;

namespace api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);  
    }
}