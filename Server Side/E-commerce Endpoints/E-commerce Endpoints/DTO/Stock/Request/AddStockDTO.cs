using System;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Stock.Request
{
    public class AddStockDTO
    {
        [Required]
        public int VariantId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int EntranceQuantity { get; set; }

        [Range(0, int.MaxValue)]
        public int? CurrentQuantity { get; set; } // Optional, defaults to EntranceQuantity

        public DateTime? EntranceDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpireDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? CostPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SellPrice { get; set; }

        public int? SupplierId { get; set; }
    }
}
