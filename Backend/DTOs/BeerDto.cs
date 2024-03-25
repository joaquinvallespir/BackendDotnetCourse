﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DTOs
{
    public class BeerDto
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public decimal Alcohol {  get; set; }
    }
}
