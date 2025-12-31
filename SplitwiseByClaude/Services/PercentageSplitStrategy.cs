using SplitwiseByClaude.Entities;

namespace SplitwiseByClaude.Services
{
    public class PercentageSplitStrategy : ISplitStrategy
    {
        public void PopulateSplitBalances(Expense expense, Dictionary<string, int>? userEmailVsValue)
        {
            if (userEmailVsValue is null || userEmailVsValue.Count == 0)
                throw new ArgumentException("userEmailVsValue cannot be null or empty for Percentage Split strategy.");

            Dictionary<string, Split> userEmailVsSplit = new(StringComparer.OrdinalIgnoreCase);
            var isValidPercentage = userEmailVsValue.Values.Sum() == 100;
            if (!isValidPercentage)
                throw new ArgumentException("Total percentage must sum up to 100 for Percentage Split strategy.");

            foreach (var userEmailKvp in userEmailVsValue)
            {
                var split = new Split
                {
                    UserEmail = userEmailKvp.Key,
                    Percentage = userEmailKvp.Value,
                    Amount = (expense.Amount * userEmailKvp.Value) / 100,
                };
                userEmailVsSplit[userEmailKvp.Key] = split;
            }
            expense.UserEmailVsSplit = userEmailVsSplit;
        }
    }
}
