using AutoMapper;
using BE_Team7.Dtos.Category;
using BE_Team7.Dtos.CategoryTitle;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace BE_Team7.Repository
{
    public class CategoryRepository  : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<Category>> CreateCategoryAsync(Category category)
        {
            var categoryModel = await _context.Category.FirstOrDefaultAsync(x => x.CategoryName == category.CategoryName);
            if (categoryModel != null)
            {
                return new ApiResponse<Category>
                {
                    Success = false,
                    Message = "Category này đã tồn tại.",
                    Data = null
                };
            }
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return new ApiResponse<Category>
            {
                Success = true,
                Message = "Tạo sản phẩm thành công.",
                Data = categoryModel
            };
        }

        public async Task<ApiResponse<Category>> DeleteCategoryAsync(Guid categoryId)
        {
            var categoryModel = await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == categoryId);
            if (categoryModel == null)
            {
                return new ApiResponse<Category>
                {
                    Success = false,
                    Message = "Category không tồn tại.",
                    Data = null
                };
            }
            _context.Category.Remove(categoryModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Category>
            {
                Success = true,
                Message = "Xóa category thành công",
                Data = categoryModel
            };
        }

        public async Task<List<GetCategoryTitleDto>> GetAllCategoriesAsync()
        {
            var category = await _context.CategoryTitle
            .Include(ct => ct.Category)
            .ToListAsync();

            return _mapper.Map<List<GetCategoryTitleDto>>(category);
        }

        public async Task<Category?> GetCategoryById(Guid categoryId)
        {
            return await _context.Category.FirstOrDefaultAsync(i => i.CategoryId == categoryId);
        }

        public async Task<ApiResponse<Category>> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequestDto updateCategoryRequestDto)
        {
            var categoryModel = await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == categoryId);
            if (categoryModel == null)
            {
                return new ApiResponse<Category>
                {
                    Success = false,
                    Message = "Category không tồn tại.",
                    Data = null
                };
            }
            if (categoryModel.CategoryTitleId != updateCategoryRequestDto.CategoryTitleId)
            {
                var categoryExists = await _context.Category.AnyAsync(c => c.CategoryTitleId == updateCategoryRequestDto.CategoryTitleId);
                if (!categoryExists)
                {
                    return new ApiResponse<Category>
                    {
                        Success = false,
                        Message = "Category Title không tồn tại.",
                        Data = null
                    };
                }
                categoryModel.CategoryTitleId = updateCategoryRequestDto.CategoryTitleId;
            }
            _mapper.Map(updateCategoryRequestDto, categoryModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Category>
            {
                Success = true,
                Message = "Cập nhật Category thành công.",
                Data = categoryModel
            };
        }
    }
}
