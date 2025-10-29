using System;
using System.ComponentModel.DataAnnotations;

namespace AvaloniaSix.Entities;

public class EntityBase
{
    [Key]
    [MaxLength(100)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
}
