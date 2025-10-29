using System;
using System.ComponentModel.DataAnnotations;

namespace AvaloniaSix.Entities;

public class ActionEntityBase : EntityBase
{
    [MaxLength(200)]
    public string JobName { get; set; } = "";

    [MaxLength(5000)]
    public string Description { get; set; } = "";

    public int SortOrder { get; set; }
}
