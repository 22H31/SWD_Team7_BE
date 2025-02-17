using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using BE_Team7;

var builder = WebApplication.CreateBuilder(args);

// Đọc ConnectionString từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Thêm dịch vụ DbContext  
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Register the NSwag services
builder.Services.AddOpenApiDocument();

// Thêm Swagger vào DI container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
