using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class Offer
{
    [Key]
    [Column("Offer_id")]
    public int OfferId { get; set; }

    [Column("Variant_id")]
    public int VariantId { get; set; }

    [Column("Offer_percentage", TypeName = "decimal(10, 2)")]
    public decimal OfferPercentage { get; set; }

    [Column("Start_date", TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column("End_date", TypeName = "date")]
    public DateTime EndDate { get; set; }

    [ForeignKey("VariantId")]
    [InverseProperty("Offers")]
    public virtual Variant Variant { get; set; } = null!;
}
