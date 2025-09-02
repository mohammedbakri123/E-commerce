using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class SubCategory
{
    [Key]
    [Column("Subcategory_id")]
    public int SubcategoryId { get; set; }

    [Column("Category_id")]
    public int CategoryId { get; set; }

    [Column("Subcategory_name")]
    [StringLength(100)]
    public string SubcategoryName { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("SubCategories")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Subcategory")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
