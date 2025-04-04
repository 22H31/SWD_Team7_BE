﻿using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using BE_Team7;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using api.Interfaces;
using api.Service;
using api.Services;
using BE_Team7.Interfaces.Repository.Contracts;
using api.Repository;
using BE_Team7.Repository;
using BE_Team7.Sevices;
using BE_Team7.Dtos;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using BE_Team7.Interfaces.Service.Contracts;
using GarageManagementAPI.Service;
using BE_Team7.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:5173", "http://localhost:5174", "http://localhost:5175", "http://localhost:5176", "http://localhost:3002") // Thêm domain của frontend
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});// Cấu hình Swagger
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Cấu hình Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Cấu hình Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAlll", policy => policy.RequireRole("User", "Admin", "Staff", "StaffSale"));
    options.AddPolicy("RequireUser", policy => policy.RequireRole("User"));
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireAdminOrStaff", policy => policy.RequireRole("Admin", "Staff"));
    options.AddPolicy("RequireStaffSaleOrAdmin", policy => policy.RequireRole("StaffSale", "Admin"));
    options.AddPolicy("RequireStaffSaleOrStaff", policy => policy.RequireRole("Staff", "StaffSale"));
    options.AddPolicy("RequireStaff", policy => policy.RequireRole("Staff"));
    options.AddPolicy("RequireStaffSale", policy => policy.RequireRole("StaffSale"));
    options.AddPolicy("RequireAlllStaff", policy => policy.RequireRole( "Admin", "Staff", "StaffSale"));
});

// Cấu hình Authentication + JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
            ),
        };

        options.Events = new JwtBearerEvents
        {
            // Xử lý khi xác thực token thất bại
            OnAuthenticationFailed = context =>
            {
                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("{\"message\": \"Token expired\"}");
                }

                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"message\": \"Authentication failed\"}");
            }
        };
    }
);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Cấu hình DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Cấu hình Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddSingleton<Cloudinary>(provider =>
{
    var config = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
});

// Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký các Repository & Service
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<BE_Team7.Interfaces.Repository.Contracts.ICartItemRepository, BE_Team7.Repository.CartItemRepository>();
builder.Services.AddScoped<ICategoryTitleRepository, CategoryTitleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISkinTestQuestionRepository, SkinTestQuestionRepository>();
builder.Services.AddScoped<ISkinTestAnswersRepository, SkinTestAnswersRepository>();
builder.Services.AddScoped<ISkinTestResultRepository, SkinTestResultRepository>();
builder.Services.AddScoped<IVnPayService, PaymentService>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IShippingInfoRepository, ShippingInfoRepository>();

var app = builder.Build();

// Middleware kiểm tra Token
app.UseMiddleware<TokenValidationMiddlewareService>();

// ✅ Thêm Middleware CORS trước Authentication
app.UseCors("AllowAll");

// Xử lý lỗi 403
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 403)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"message\": \"Bạn không có quyền truy cập vào tài nguyên này.\"}");
    }
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
