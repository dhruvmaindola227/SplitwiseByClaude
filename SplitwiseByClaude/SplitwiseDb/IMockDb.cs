using SplitwiseByClaude.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitwiseByClaude.SplitwiseDb
{
    public interface IMockDb
    {
        public List<User> GetUsers();
        public List<Expense> GetExpenses();
        public void AddEntity<T>(T entity) where T : class;
        public int[,] GetBalancesData();
        public Dictionary<string, int> GetUserEmailToIndexMap();
        public void UpdateBalancesData(int[,] balancesData, Dictionary<string, int> userEmailToIndex);
    }
}
