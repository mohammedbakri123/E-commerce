using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

[Table("Cart_items")]
public partial class CartItem
{
    [Key]
    [Column("Cart_item_id")]
    public int CartItemId { get; set; }

    [Column("Cart_id")]
    public int CartId { get; set; }

    [Column("Variant_id")]
    public int VariantId { get; set; }

    public int Quantity { get; set; }

    [Column("Price_per_unit", TypeName = "decimal(10, 2)")]
    public decimal PricePerUnit { get; set; }

    [Column(TypeName = "decimal(21, 2)")]
    public decimal? TotalPrice { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart Cart { get; set; } = null!;

    [ForeignKey("VariantId")]
    [InverseProperty("CartItems")]
    public virtual Variant Variant { get; set; } = null!;
}
