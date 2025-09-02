using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class Favorite
{
    [Key]
    [Column("Favorite_id")]
    public int FavoriteId { get; set; }

    [Column("User_id")]
    public int UserId { get; set; }

    [Column("Variant_id")]
    public int VariantId { get; set; }

    [Column("Create_at", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Favorites")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("VariantId")]
    [InverseProperty("Favorites")]
    public virtual Variant Variant { get; set; } = null!;
}
