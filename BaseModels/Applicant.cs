using System.Collections.Generic;


namespace exel_for_mfc;

public partial class Applicant
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Middlename { get; set; }

    public string? Lastname { get; set; }

    public int? AreaFk { get; set; }

    public int? LocalityFk { get; set; }

    public string? Adress { get; set; }

    public string? Snils { get; set; }

    public int? PrivilegesFk { get; set; }

    public virtual Area? AreaFkNavigation { get; set; }

    public virtual Locality? LocalityFkNavigation { get; set; }

    public virtual Privilege? PrivilegesFkNavigation { get; set; }

    public virtual ICollection<Registry> Registries { get; set; } = new List<Registry>();
}
