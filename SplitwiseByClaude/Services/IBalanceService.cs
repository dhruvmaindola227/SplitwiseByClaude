
using SplitwiseByClaude.Entities;

namespace SplitwiseByClaude.Services
{
    public interface IBalanceService
    {
        public Dictionary<string, int> GetBalanceForUser(string userEmail);
        public Dictionary<string, Dictionary<string, int>> GetAllBalances();
        public void UpdateBalances(Expense expense);
        public void UpdateUsersBalanceInfo(string userEmail);
    }
}
