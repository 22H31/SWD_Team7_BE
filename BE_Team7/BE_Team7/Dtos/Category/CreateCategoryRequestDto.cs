﻿namespace BE_Team7.Dtos.Category
{
    public class CreateCategoryRequestDto
    {
        public required Guid CategoryTitleId { get; set; }
        public required string CategoryName { get; set; }
        public required string Description { get; set; }
    }
}
