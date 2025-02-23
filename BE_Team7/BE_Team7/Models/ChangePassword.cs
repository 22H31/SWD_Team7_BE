using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class ChangePassword
    {
        [Key]
        [Required]
        [DataType(DataType.Password)]
        public required string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.")]
        public required string NewPasswordConfirmation { get; set; }
    }
}