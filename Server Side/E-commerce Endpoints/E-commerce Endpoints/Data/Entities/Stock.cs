using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class Stock
{
    [Key]
    [Column("Stock_id")]
    public int StockId { get; set; }

    [Column("Variant_id")]
    public int VariantId { get; set; }

    public int EntranceQuantity { get; set; }

    public int CurrentQuantity { get; set; }

    [Column(TypeName = "date")]
    public DateTime? EntranceDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ExpireDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? CostPrice { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? SellPrice { get; set; }

    [Column("Is_Done")]
    public bool? IsDone { get; set; }

    [Column("supplier_id")]
    public int? SupplierId { get; set; }

    [InverseProperty("Stocks")]
    [ForeignKey("SupplierId")]
    public virtual Supplier? Supplier { get; set; }

    [ForeignKey("VariantId")]
    [InverseProperty("Stocks")]
    public virtual Variant Variant { get; set; } = null!;
}
