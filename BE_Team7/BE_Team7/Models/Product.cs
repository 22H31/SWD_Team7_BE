using BE_Team7.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;
    public Guid BrandId { get; set; }
    public virtual Brand Brand { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    // Danh sách biến thể sản phẩm
    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    // Danh sách hình ảnh sản phẩm
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    // Danh sách Feedbacks
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    // Mô tả chi tiết sản phẩm (Lưu dưới dạng JSON)
    [Column(TypeName = "nvarchar(max)")]
    public string Description { get; set; } = "{}";
    // Thông số kỹ thuật sản phẩm (Lưu dưới dạng JSON)
    [Column(TypeName = "nvarchar(max)")]
    public string Specification { get; set; } = "{}";
    // Hướng dẫn sử dụng (Lưu dưới dạng JSON)
    [Column(TypeName = "nvarchar(max)")]
    public string UseManual { get; set; } = "{}";
}
