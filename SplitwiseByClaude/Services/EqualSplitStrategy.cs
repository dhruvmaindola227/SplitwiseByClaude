using SplitwiseByClaude.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitwiseByClaude.Services
{
    public class EqualSplitStrategy : ISplitStrategy
    {
        public void PopulateSplitBalances(Expense expense, Dictionary<string, int>? userEmailVsValue)
        {
            // For equal split no userEmailVsValue should be provided
            if (userEmailVsValue is not null && userEmailVsValue.Count > 0)
                throw new ArgumentException("userEmailVsValue should be null or empty for Equal Split strategy.");

            Dictionary<string, Split> userEmailVsSplit = new(StringComparer.OrdinalIgnoreCase);
            int numberOfUsers = expense.UserEmailVsSplit.Count;
            if (numberOfUsers == 0)
                throw new ArgumentException("No users to split the expense between.");

            int equalShare = expense.Amount / numberOfUsers;
            foreach (var userEmail in expense.UserEmailVsSplit.Select(x => x.Key))
            {
                var split = new Split
                {
                    UserEmail = userEmail,
                    Amount = equalShare,
                };
                userEmailVsSplit[userEmail] = split;
            }
            expense.UserEmailVsSplit = userEmailVsSplit;
        }
    }
}
