
namespace exel_for_mfc.SupportClass
{
    class SolutionClass
    {
        public int Id { get; set; }
        public string? SolutionName { get; set; }

        public int? SolCount { get; set; }

        public SolutionClass(int id, string? sol, int? solCount)
        {
            Id = id;
            SolutionName = sol;
            SolCount = solCount;
        }
    }
}
