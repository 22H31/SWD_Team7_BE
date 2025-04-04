﻿using System.Linq;
using AutoMapper;
using BE_Team7.Dtos.FeedBack;
using BE_Team7.Dtos.Product;
using BE_Team7.Dtos.ProductVariant;
using BE_Team7.Helpers;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static StackExchange.Redis.Role;

namespace BE_Team7.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PagedResult<Product>> GetProductsAsync(ProductQuery productQuery)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.ProductAvatarImages)
                .Include(p => p.Feedbacks)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(productQuery.Name))
            {
                products = products.Where(p => p.ProductName.Contains(productQuery.Name));
            }
            var totalCount = await products.CountAsync();
            var skipNumber = (productQuery.PageNumber - 1) * productQuery.PageSize;
            var pagedProducts = await products.Skip(skipNumber).Take(productQuery.PageSize).ToListAsync();

            return new PagedResult<Product>
            {
                Items = pagedProducts,
                TotalCount = totalCount
            };
        }


        public async Task<ApiResponse<Product>> CreateProductAsyns(Product product)
        {
            // Kiểm tra Category có tồn tại không
            var productExists = await _context.Products.AnyAsync(c => c.ProductName == product.ProductName);
            if (productExists)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Tên sản phẩm đã tồn tại",
                    Data = null
                };
            }
            // Kiểm tra Category có tồn tại không
            var categoryExists = await _context.Category.AnyAsync(c => c.CategoryId == product.CategoryId);
            if (!categoryExists)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Danh mục không tồn tại.",
                    Data = null
                };
            }
            // Kiểm tra Brand có tồn tại không
            var brandExists = await _context.Brand.AnyAsync(b => b.BrandId == product.BrandId);
            if (!brandExists)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Thương hiệu không tồn tại.",
                    Data = null
                };
            }
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Tên sản phẩm không được để trống.",
                    Data = null
                };
            }
            _context.Products.Add(product);
            int changes = await _context.SaveChangesAsync();
            if (changes == 0)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Lỗi khi lưu dữ liệu vào database.",
                    Data = null
                };
            }
            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Tạo sản phẩm thành công.",
                Data = product
            };
        }

        public async Task<ProductDetailDto?> GetProductById(Guid productId)
        {
            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductAvatarImages)
                .Include(p => p.ProductImages)
                .Include(p => p.Variants)
                .Include(p => p.Feedbacks)
                    .ThenInclude(f => f.User)
                        .ThenInclude(u => u.AvatarImages)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null) return null;

            var productDto = new ProductDetailDto
            { 
                CategoryId = product.CategoryId,
                SoldQuantity = product.SoldQuantity,
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                BrandName = product.Brand!.BrandName,
                CategoryName = product.Category!.CategoryName,
                ImageUrl = product.ProductImages
                .OrderByDescending(img => img.ProductImageCreatedAt)
                .Take(5)
                .ToDictionary(
                    img => $"img{product.ProductImages.ToList().IndexOf(img) + 1}",
                    img => img.ImageUrl ?? ""),
                AvatarImageUrl = product.ProductAvatarImages.FirstOrDefault()?.ImageUrl,
                Variants = product.Variants.Select(v => new ProductVariantDto
                {
                    VariantId = v.VariantId,
                    Volume = v.Volume,
                    SkinType = v.SkinType,
                    Price = v.Price,
                    StockQuantity = v.StockQuantity,
                    MainIngredients = v.MainIngredients,
                    FullIngredients  = v.FullIngredients
                }).ToList(),
                Feedbacks = product.Feedbacks.Select(f => new FeedbackDto
                {
                    FeedbackId = f.FeedbackId,
                    UserName = f.User?.Name,
                    UserAvatarUrl = f.User?.AvatarImages?
                        .OrderByDescending(a => a.AvatarImageCreatedAt)
                        .FirstOrDefault()?.ImageUrl,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    CreatedAt = f.CreatedAt.ToString("HH:mm dd/MM/yyyy")
                }).ToList(),
                AverageRating = product.Feedbacks.Any() ? product.Feedbacks.Average(f => f.Rating) : 0,
                TotalFeedback = product.Feedbacks.Count,
                Describe = JsonConvert.DeserializeObject<DescriptionDto>(product.Description),
                Specifications = JsonConvert.DeserializeObject<SpecificationDto>(product.Specification),
                UseManual = JsonConvert.DeserializeObject<UseManualDto>(product.UseManual)
            };
            return productDto;
        }


        public async Task<ApiResponse<Product>> UpdateProductById(Guid id, UpdateProductRequestDto productDtoForUpdate)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (productModel == null)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Sản phẩm không tồn tại.",
                    Data = null
                };
            }

            if (productModel.CategoryId != productDtoForUpdate.CategoryId)
            {
                var categoryExists = await _context.Category.AnyAsync(c => c.CategoryId == productDtoForUpdate.CategoryId);
                if (!categoryExists)
                {
                    return new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "Danh mục không tồn tại.",
                        Data = null
                    };
                }
                productModel.CategoryId = productDtoForUpdate.CategoryId;
            }
            if (productModel.BrandId != productDtoForUpdate.BrandId)
            {
                var brandExists = await _context.Brand.AnyAsync(c => c.BrandId == productDtoForUpdate.BrandId);
                if (!brandExists)
                {
                    return new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "Brand không tồn tại.",
                        Data = null
                    };
                }
                productModel.BrandId = productDtoForUpdate.BrandId;
            }

            _mapper.Map(productDtoForUpdate, productModel);
            _context.Products.Update(productModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Cập nhật sản phẩm thành công.",
                Data = productModel
            };
        }

        public async Task<ApiResponse<Product>> DeleteProductById(Guid productId)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (productModel == null)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Sản phẩm không tồn tại.",
                    Data = null
                };
            }
            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Xóa sản phẩm thành công.",
                Data = productModel
            };
        }
        public async Task<ApiResponse<Product>> CreateProductImgAsync(Guid productId, string publicId, string absoluteUrl)
        {

            var productImg = new ProductImage()
            {
                ImageUrl = absoluteUrl,
                ImageId = publicId,
                ProductId = productId,
                ProductImageCreatedAt = DateTime.UtcNow
            };
            _context.ProductImage.Add(productImg);
            await _context.SaveChangesAsync();

            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Upload thành công.",
                Data = null,
            };
        }

        public async Task<ApiResponse<Product>> CreateProductAvartarImgAsync(Guid productId, string publicId, string absoluteUrl)
        {
            var productImg = new ProductAvatarImage()
            {
                ImageUrl = absoluteUrl,
                ImageId = publicId,
                ProductId = productId,
                ProductAvatarImageCreatedAt = DateTime.UtcNow,
            };
            _context.productAvatarImage.Add(productImg);
            await _context.SaveChangesAsync();

            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Upload thành công.",
                Data = null,
            };
        }
        public async Task<PagedResult<Product>> GetProductsByCategoryTitleIdAsync(Guid categoryTitleId, ProductQuery productQuery)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ThenInclude(c => c.CategoryTitle) // Include cả CategoryTitle để lọc
                .Include(p => p.Variants)
                .Include(p => p.ProductAvatarImages)
                .Include(p => p.Feedbacks)
                .Where(p => p.Category.CategoryTitleId == categoryTitleId) // Lọc theo CategoryTitleId
                .AsQueryable();

            return await PaginateAsync(products, productQuery);
        }
        public async Task<PagedResult<Product>> GetProductsByCategoryAsync(Guid categoryId, ProductQuery productQuery)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.ProductAvatarImages)
                .Include(p => p.Feedbacks)
                .Where(p => p.CategoryId == categoryId)
                .AsQueryable();

            return await PaginateAsync(products, productQuery);
        }

        public async Task<PagedResult<Product>> GetProductsByBrandAsync(Guid brandId, ProductQuery productQuery)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.ProductAvatarImages)
                .Include(p => p.Feedbacks)
                .Where(p => p.BrandId == brandId)
                .AsQueryable();

            return await PaginateAsync(products, productQuery);
        }

        public async Task<PagedResult<Product>> GetRecentProductsAsync(ProductQuery productQuery)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.ProductAvatarImages)
                .Include(p => p.Feedbacks)
                .Where(p => p.CreatedAt >= DateTime.UtcNow.AddHours(-24))
                .OrderByDescending(p => p.CreatedAt)
                .AsQueryable();

            return await PaginateAsync(products, productQuery);
        }

        public async Task<PagedResult<Product>> GetTopSellingProductsAsync(ProductQuery productQuery)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.ProductAvatarImages)
                .Include(p => p.Feedbacks)
                .OrderByDescending(p => p.SoldQuantity)
                .AsQueryable();

            return await PaginateAsync(products, productQuery, maxLimit: 20);
        }

        private async Task<PagedResult<Product>> PaginateAsync(IQueryable<Product> query, ProductQuery productQuery, int? maxLimit = null)
        {
            var totalCount = await query.CountAsync();
            var pageSize = maxLimit.HasValue ? Math.Min(productQuery.PageSize, maxLimit.Value) : productQuery.PageSize;
            var skipNumber = (productQuery.PageNumber - 1) * pageSize;
            var pagedProducts = await query.Skip(skipNumber).Take(pageSize).ToListAsync();

            return new PagedResult<Product>
            {
                Items = pagedProducts,
                TotalCount = totalCount
            };
        }
    }
}