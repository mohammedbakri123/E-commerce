using System;

namespace E_commerce_Endpoints.DTO.Offer.Response
{
    public class OfferDTO
    {
        public int OfferId { get; set; }
        public int VariantId { get; set; }
        public string? VariantName { get; set; }
        public decimal OfferPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    }
}
