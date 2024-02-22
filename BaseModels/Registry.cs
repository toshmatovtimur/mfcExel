using System;

namespace exel_for_mfc;

public partial class Registry
{
    public int Id { get; set; }

    public int? ApplicantFk { get; set; }

    public string? SerialAndNumberSert { get; set; }

    public DateTime? DateGetSert { get; set; }

    public int? PayAmountFk { get; set; }

    public int? SolutionFk { get; set; }

    public string? DateAndNumbSolutionSert { get; set; }

    public string? Comment { get; set; }

    public string? Trek { get; set; }

    public DateTime? MailingDate { get; set; }

    public DateTime? dateOfTheApp { get; set; }

    public virtual Applicant? ApplicantFkNavigation { get; set; }

    public virtual PayAmount? PayAmountFkNavigation { get; set; }

    public virtual SolutionType? SolutionFkNavigation { get; set; }
}
