using System;

namespace E_commerce_Endpoints.DTO.Stock.Response
{
    public class StockDTO
    {
        public int StockId { get; set; }
        public int VariantId { get; set; }
        public string? VariantName { get; set; }
        public int EntranceQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public DateTime? EntranceDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SellPrice { get; set; }
        public int? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public bool IsDone { get; set; }
    }
}
