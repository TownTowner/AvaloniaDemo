using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvaloniaSix.Entities;

public class Setting : EntityBase
{
    public bool SkipNoActionFiles { get; set; }

    public bool AllowDuplicateEntries { get; set; }

    [MaxLength(100000)]
    public List<string> LocationPaths { get; set; } = [];

    [MaxLength(100)]
    public string SolidWorksHost { get; set; } = "";

    [MaxLength(100)]
    public string PdmeVaultName { get; set; } = "";

    [MaxLength(100)]
    public string PdmeUsername { get; set; } = "";

    [MaxLength(100)]
    public string PdmePassword { get; set; } = "";

    [MaxLength(100000)]
    public List<string> DrawingTemplatePaths { get; set; } = [];
}
