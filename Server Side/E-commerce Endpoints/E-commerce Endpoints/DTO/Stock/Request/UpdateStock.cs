using System;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Stock.Request
{
    public class UpdateStockDTO
    {
        [Required]
        public int StockId { get; set; }

        [Range(0, int.MaxValue)]
        public int EntranceQuantity { get; set; }

        [Range(0, int.MaxValue)]
        public int CurrentQuantity { get; set; }

        public DateTime? EntranceDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? CostPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SellPrice { get; set; }

        public int? SupplierId { get; set; }
    }
}
