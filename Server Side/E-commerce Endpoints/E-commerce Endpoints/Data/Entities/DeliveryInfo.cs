using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

[Table("DeliveryInfo")]
public partial class DeliveryInfo
{
    [Key]
    [Column("DeliveryInfo_id")]
    public int DeliveryInfoId { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(20)]
    public string? PhoneNumber2 { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(255)]
    public string? Street { get; set; }

    [StringLength(255)]
    public string? AddressDetails { get; set; }

    [Column("Created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("DeliveryInfo")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
