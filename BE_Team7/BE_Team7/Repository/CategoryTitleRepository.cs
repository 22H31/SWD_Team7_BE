using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.CategoryTitle;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace BE_Team7.Repository
{
    public class CategoryTitleRepository : ICategoryTitleRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CategoryTitleRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CategoryTitle>> CreateCategoryTitleAsync(CategoryTitle categoryTitle)
        {
            var categoryTitleModel = await _context.CategoryTitle.FirstOrDefaultAsync(x => x.CategoryTitleName == categoryTitle.CategoryTitleName);
            if (categoryTitleModel != null)
            {
                return new ApiResponse<CategoryTitle>
                {
                    Success = false,
                    Message = "Brand này đã tồn tại.",
                    Data = null
                };
            }
            _context.CategoryTitle.Add(categoryTitle);
            await _context.SaveChangesAsync();
            return new ApiResponse<CategoryTitle>
            {
                Success = true,
                Message = "Tạo sản phẩm thành công.",
                Data = categoryTitleModel
            };
        }

        public async Task<ApiResponse<CategoryTitle>> DeleteCategoryTitleAsync(Guid categoryTitleId)
        {
            var categoryTitleModel = await _context.CategoryTitle.FirstOrDefaultAsync(x => x.CategoryTitleId == categoryTitleId);
            if (categoryTitleModel == null)
            {
                return new ApiResponse<CategoryTitle>   
                {
                    Success = false,
                    Message = "Category title không tồn tại",
                    Data = null
                };
            }
            _context.CategoryTitle.Remove(categoryTitleModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<CategoryTitle>
            {
                Success = true,
                Message = "Xóa category title thành công",
                Data = categoryTitleModel
            };
        }

        public async Task<List<CategoryTitle>> GetCategoryAsync()
        {
            var categoryTitle = _context.CategoryTitle.AsQueryable();
            return await categoryTitle.ToListAsync();
        }

        public async Task<CategoryTitle?> GetCategoryTitleById(Guid categoryTitleId)
        {
            return await _context.CategoryTitle.FirstOrDefaultAsync(i => i.CategoryTitleId == categoryTitleId);
        }

        public async Task<ApiResponse<CategoryTitle>> UpdateCategoryTitleAsync(Guid categoryTitleId, UpdateCategoryTitleRequestDto updateCategoryTitleRequestDto)
        {
            var categoryTitleModel = await _context.CategoryTitle.FirstOrDefaultAsync(x => x.CategoryTitleId == categoryTitleId);
            if (categoryTitleModel == null)
            {
                return new ApiResponse<CategoryTitle>
                {
                    Success = false,
                    Message = "Category title không tồn tại.",
                    Data = null
                };
            }
            _mapper.Map(updateCategoryTitleRequestDto, categoryTitleModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<CategoryTitle>
            {
                Success = true,
                Message = "Cập nhật category title thành công.",
                Data = categoryTitleModel
            };
        }
    }
}
