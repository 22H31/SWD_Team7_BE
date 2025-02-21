﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Dtos.Account
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string SkinType { get; set; }

        public string PhoneNumber { get; set; }
    }
}