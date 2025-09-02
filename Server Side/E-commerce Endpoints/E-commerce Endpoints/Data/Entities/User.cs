using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

[Index("Email", Name = "UQ__Users__A9D1053445500F14", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("User_id")]
    public int UserId { get; set; }

    [Column("User_firstName")]
    [StringLength(100)]
    public string UserFirstName { get; set; } = null!;

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string Password { get; set; } = null!;

    public bool? Status { get; set; }

    [StringLength(50)]
    public string Role { get; set; } = null!;

    [Column("Create_at", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("User")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
