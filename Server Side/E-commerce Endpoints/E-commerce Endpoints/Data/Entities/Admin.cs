using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class Admin
{
    [Key]
    [Column("Admin_id")]
    public int AdminId { get; set; }

    [Column("User_id")]
    public int UserId { get; set; }

    [Column("Admin_lastName")]
    [StringLength(100)]
    public string? AdminLastName { get; set; }

    [Column("Permissions_value")]
    public int? PermissionsValue { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Admins")]
    public virtual User User { get; set; } = null!;

}
