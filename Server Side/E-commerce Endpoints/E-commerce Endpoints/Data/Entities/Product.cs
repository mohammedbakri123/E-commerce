using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class Product
{
    [Key]
    [Column("Product_id")]
    public int ProductId { get; set; }

    [Column("Product_name")]
    [StringLength(200)]
    public string ProductName { get; set; } = null!;

    [Column("Brand_id")]
    public int? BrandId { get; set; }

    [Column("Subcategory_id")]
    public int? SubcategoryId { get; set; }

    [Column("Created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("Updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("Products")]
    public virtual Brand? Brand { get; set; }

    [ForeignKey("SubcategoryId")]
    [InverseProperty("Products")]
    public virtual SubCategory? Subcategory { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Variant> Variants { get; set; } = new List<Variant>();
}
