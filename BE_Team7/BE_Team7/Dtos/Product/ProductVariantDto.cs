﻿namespace BE_Team7.Dtos.Product
{
    public class ProductVariantDto
    {
        public int Volume { get; set; }
        public string SkinType { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string MainIngredients { get; set; }
        public string FullIngredients { get; set; }
    }
}
