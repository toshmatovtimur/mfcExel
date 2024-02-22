using System.Collections.Generic;


namespace exel_for_mfc;

public partial class PayAmount
{
    public int Id { get; set; }

    public decimal? Pay { get; set; }

    public int? HidingPay { get; set; }

    public string? Mkr { get; set; }

    public string? Ulica { get; set; }

    public string? Kvartira { get; set; }

    public virtual ICollection<Registry> Registries { get; set; } = new List<Registry>();
}
