
namespace exel_for_mfc.SupportClass
{
    internal class PayClass
    {
        public int? Id { get; set; }
        public decimal? Pay { get; set; }
        public int? PayCount{ get; set; }

        public PayClass(int id, decimal? pay, int? payCount)
        {
            Id = id;
            Pay = pay;
            PayCount = payCount;
        }
    }
}