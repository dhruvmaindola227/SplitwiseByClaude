using SplitwiseByClaude.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitwiseByClaude.Services
{
    public class SplitService : ISplitService
    {
        public void CalculateSplits(Expense expense, Dictionary<string, int>? userNameVsValue)
        {
            ISplitStrategy splitStrategy = SplitStrategyFactory.GetStrategy(expense.SplitType);
            
            splitStrategy.PopulateSplitBalances(expense, userNameVsValue);
        }
    }
}
