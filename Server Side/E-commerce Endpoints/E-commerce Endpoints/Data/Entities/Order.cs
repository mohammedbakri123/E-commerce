using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class Order
{
    [Key]
    [Column("Order_id")]
    public int OrderId { get; set; }

    [Column("User_id")]
    public int UserId { get; set; }

    [Column("Order_date", TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalAmount { get; set; }

    [Column("Payment_method")]
    [StringLength(50)]
    public string? PaymentMethod { get; set; }

    [Column("DeliveryInfo_id")]
    public int? DeliveryInfoId { get; set; }

    [Column("Cart_id")]
    public int CartId { get; set; }

    [ForeignKey("DeliveryInfoId")]
    [InverseProperty("Orders")]
    public virtual DeliveryInfo? DeliveryInfo { get; set; }

    [ForeignKey("CartId")]
    public virtual Cart Cart { get; set; } = null;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
