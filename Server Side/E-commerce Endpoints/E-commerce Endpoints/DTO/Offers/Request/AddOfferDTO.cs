using System;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Offer.Request
{
    public class AddOfferDTO
    {
        [Required]
        public int VariantId { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal OfferPercentage { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}