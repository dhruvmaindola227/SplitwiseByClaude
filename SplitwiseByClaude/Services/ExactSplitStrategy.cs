using SplitwiseByClaude.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitwiseByClaude.Services
{
    public class ExactSplitStrategy : ISplitStrategy
    {
        public void PopulateSplitBalances(Expense expense, Dictionary<string, int>? userEmailVsValue)
        {
            if (userEmailVsValue is null || userEmailVsValue.Count == 0)
                throw new ArgumentException("userEmailVsValue cannot be null or empty for Exact Split strategy.");

            Dictionary<string, Split> userEmailVsSplit = new(StringComparer.OrdinalIgnoreCase);
            bool isValidExactAmounts = userEmailVsValue.Values.Sum() == expense.Amount;
            if (!isValidExactAmounts)
                throw new ArgumentException("Total exact amounts must sum up to the total expense amount for Exact Split strategy.");
            foreach (var userEmailKvp in userEmailVsValue)
            {
                var split = new Split
                {
                    UserEmail = userEmailKvp.Key,
                    Amount = userEmailKvp.Value,
                };
                userEmailVsSplit[userEmailKvp.Key] = split;
            }
            expense.UserEmailVsSplit = userEmailVsSplit;
        }
    }
}
