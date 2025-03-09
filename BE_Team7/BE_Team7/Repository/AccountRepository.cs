using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using BE_Team7;
using BE_Team7.Models;
using BE_Team7.Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        public AccountRepository(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<User?> GetUserById(string id)
        {
            Console.WriteLine(await _context.User.FirstOrDefaultAsync(p => p.Id.Equals(id)));
            return await _context.User.FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public async Task<ApiResponse<AvatarImage>> UploadAvatarImage(string id, string publicId, string absoluteUrl)
        {

            var avatarImg = new AvatarImage()
            {
                ImageUrl = absoluteUrl,
                ImageId = publicId,
                Id = id
            };
            _context.AvatarImage.Add(avatarImg);
            await _context.SaveChangesAsync();

            return new ApiResponse<AvatarImage>
            {
                Success = true,
                Message = "upload avater thành công.",
                Data = avatarImg,
            };
        }
    }
}