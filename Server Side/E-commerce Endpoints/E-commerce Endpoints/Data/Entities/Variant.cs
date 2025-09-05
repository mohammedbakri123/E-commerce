using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class Variant
{
    [Key]
    [Column("Variant_id")]
    public int VariantId { get; set; }

    public string? ImagePaths { get; set; }

    [Column("Variant_properties")]
    public string? VariantProperties { get; set; }

    public bool? Status { get; set; }

    [Column("Product_id")]
    public int ProductId { get; set; }

    [InverseProperty("Variant")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("Variant")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [InverseProperty("Variant")]
    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    [ForeignKey("ProductId")]
    [InverseProperty("Variants")]
    public virtual Product Product { get; set; } = null!;

    [InverseProperty("Variant")]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
