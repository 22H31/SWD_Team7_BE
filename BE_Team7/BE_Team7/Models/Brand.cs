﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BrandId { get; set; }
        public required string BrandName { get; set; }
        public required string brandDescription { get; set; }
        public virtual ICollection<BrandAvartarImage> BrandAvartarImage { get; set; } = new List<BrandAvartarImage>();
    }
}
