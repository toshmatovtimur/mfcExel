using System.Collections.Generic;

namespace exel_for_mfc;

public partial class Locality
{
    public int Id { get; set; }

    public string? LocalName { get; set; }

    public int? HidingLocal { get; set; }

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
