using SplitwiseByClaude.Entities;
using static SplitwiseByClaude.Entities.Enum;

namespace SplitwiseByClaude.Services
{
    public interface IExpenseService
    {
        public void AddExpense(string description, int amount, DateTime dateIncurred, string paidByEmail, SplitType splitType, List<string> splitBetweenEmails, Dictionary<string, int>? userEmailVsValue);
        public List<Expense> GetAllExpenses();
        public List<Expense> GetPaidExpensesForEmail(string userEmail);
        public List<Expense> GetOwedExpensesForEmail(string userEmail);
        public List<Expense> GetAllExpensesForEmail(string userEmail);
    }
}
