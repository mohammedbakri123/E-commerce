using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Data.Entities;

public partial class Permission
{
    [Key]
    [Column("Permission_id")]
    public int PermissionId { get; set; }

    [Column("Permission_name")]
    [StringLength(100)]
    public string PermissionName { get; set; } = null!;

    [Column("Permission_value")]
    public int PermissionValue { get; set; }
}
