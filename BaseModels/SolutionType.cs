using System.Collections.Generic;

namespace exel_for_mfc;

public partial class SolutionType
{
    public int Id { get; set; }

    public string? SolutionName { get; set; }
    public string? Login { get; set; }
    public string? Passwords { get; set; }
    public string? Rolle { get; set; }

    public int? HidingSol { get; set; }

    public virtual ICollection<Registry> Registries { get; set; } = new List<Registry>();
}
