using SplitwiseByClaude.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitwiseByClaude.Services
{
    public interface ISplitService
    {
        public void CalculateSplits(Expense expense, Dictionary<string, int>? userNameVsValue);
    }
}
