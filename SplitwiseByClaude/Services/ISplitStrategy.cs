using SplitwiseByClaude.Entities;

namespace SplitwiseByClaude.Services
{
    public interface ISplitStrategy
    {
        public void PopulateSplitBalances(Expense expense, Dictionary<string, int>? userEmailVsValue);
    }
}
