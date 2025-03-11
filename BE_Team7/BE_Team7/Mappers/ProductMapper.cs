using BE_Team7.Models;
using AutoMapper;
using BE_Team7.Dtos.Product;
using Newtonsoft.Json;
using BE_Team7.Dtos.ProductVariant;

namespace BE_Team7.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            // Map từ Product → ProductDto
            CreateMap<Product, ProductDetailDto>()
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ProductImages
            .OrderByDescending(img => img.ProductImageCreatedAt) // Sắp xếp theo ngày tạo giảm dần
            .Take(5) // Lấy 5 ảnh mới nhất
            .ToDictionary(img => $"img{src.ProductImages.ToList().IndexOf(img) + 1}", img => img.ImageUrl)))
            .ForMember(dest => dest.AvatarImageUrl, opt => opt.MapFrom(src =>
            src.ProductAvatarImages != null && src.ProductAvatarImages.Any() ? src.ProductAvatarImages.First().ImageUrl: null))
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
            .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Feedbacks))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                src.Feedbacks.Any() ? src.Feedbacks.Average(f => f.Rating) : 0))
            .ForMember(dest => dest.TotalFeedback, opt => opt.MapFrom(src => src.Feedbacks.Count))
            .ForMember(dest => dest.Describe, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<DescriptionDto>(src.Description)))
            .ForMember(dest => dest.Specifications, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<SpecificationDto>(src.Specification)))
            .ForMember(dest => dest.UseManual, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<UseManualDto>(src.UseManual)));

            CreateMap<ProductVariant, ProductVariantDto>();
            CreateMap<Feedback, FeedbackDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("HH:mm dd/MM/yyyy")));
            // Map từ Product → CreateProductRequestDto      
            CreateMap<CreateProductRequestDto, Product>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Describe)))
                .ForMember(dest => dest.Specification, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Specifications)))
                .ForMember(dest => dest.UseManual, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.UseManual)))
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.Variants, opt => opt.Ignore());

            CreateMap<ProductVariantDto, ProductVariant>();
            // Map update
            CreateMap<UpdateProductRequestDto, Product>()
            .ForMember(dest => dest.Description,opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Describe ?? new DescriptionDto())))
            .ForMember(dest => dest.Specification, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Specifications ?? new SpecificationDto())))
            .ForMember(dest => dest.UseManual,opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.UseManual ?? new UseManualDto())))
            .ForMember(dest => dest.ProductId, opt => opt.Ignore());
        }
    }
}
