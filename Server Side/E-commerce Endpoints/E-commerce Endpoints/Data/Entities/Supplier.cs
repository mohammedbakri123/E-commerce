using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class Supplier
{
    [Key]
    [Column("Supplier_id")]
    public int SupplierId { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [Column("Company_name")]
    [StringLength(200)]
    public string CompanyName { get; set; } = null!;

    [InverseProperty("Supplier")]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
