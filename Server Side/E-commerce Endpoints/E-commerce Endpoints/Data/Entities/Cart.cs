using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class Cart
{
    [Key]
    [Column("Cart_id")]
    public int CartId { get; set; }

    [Column("User_id")]
    public int UserId { get; set; }

    [Column("Create_at", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column("Updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public bool isClosed { get; set; }

    [InverseProperty("Cart")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("UserId")]
    [InverseProperty("Carts")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("Cart")]
    public virtual Order? Order { get; set; }
}
