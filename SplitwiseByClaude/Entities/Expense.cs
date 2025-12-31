using static SplitwiseByClaude.Entities.Enum;

namespace SplitwiseByClaude.Entities
{
    public class Expense
    {
        public int Id { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Amount { get; set; } = default!;
        public DateTime DateIncurred { get; set; } = default!;
        public string PaidByUserEmail { get; set; } = default!;
        public SplitType SplitType { get; set; } = default!;
        public Dictionary<string, Split> UserEmailVsSplit { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }
}