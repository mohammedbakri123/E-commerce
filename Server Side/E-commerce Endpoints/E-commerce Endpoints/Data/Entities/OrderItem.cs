using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class OrderItem
{
    [Key]
    [Column("OrderItem_id")]
    public int OrderItemId { get; set; }

    [Column("Order_id")]
    public int OrderId { get; set; }

    [Column("Stock_id")]
    public int StockId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(21, 2)")]
    public decimal? SubTotal { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("StockId")]
    [InverseProperty("OrderItems")]
    public virtual Stock Stock { get; set; } = null!;
}
