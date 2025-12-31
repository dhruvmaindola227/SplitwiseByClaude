using SplitwiseByClaude.Entities;
using SplitwiseByClaude.SplitwiseDb;
using static SplitwiseByClaude.Entities.Enum;

namespace SplitwiseByClaude.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IMockDb _mockDb;
        private readonly ISplitService _splitService;
        private readonly IBalanceService _balanceService;
        public ExpenseService(IMockDb mockDb, ISplitService splitService, IBalanceService balanceService)
        {
            _mockDb = mockDb;
            _splitService = splitService;
            _balanceService = balanceService;
        }
        public void AddExpense(string description, int amount, DateTime dateIncurred, string paidByEmail, SplitType splitType, List<string> splitBetweenEmails, Dictionary<string, int>? userEmailVsValue)
        {
            // validations
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be null or empty.");

            var isValidEmail = CommonUtility.IsValidEmail(paidByEmail);
            if (!isValidEmail)
                throw new ArgumentException("Invalid email format.");

            if (splitBetweenEmails is null || splitBetweenEmails.Count == 0)
                throw new ArgumentException("splitBetweenEmails cannot be null or empty.");

            HashSet<string> invalidMails = new(StringComparer.OrdinalIgnoreCase);
            foreach (var email in splitBetweenEmails)
            {
                isValidEmail = CommonUtility.IsValidEmail(email);
                if (!isValidEmail)
                    invalidMails.Add(email);
            }
            if (invalidMails.Count > 0)
                throw new ArgumentException($"Invalid emails format: {string.Join(", ", invalidMails)}");

            var currExpense = new Expense
            {
                Description = description,
                Amount = amount,
                DateIncurred = dateIncurred,
                PaidByUserEmail = paidByEmail,
                SplitType = splitType,
            };

            // Initialize UserEmailVsSplit with provided participants
            Dictionary<string, Split> initialSplits = new(StringComparer.OrdinalIgnoreCase);
            foreach (var email in splitBetweenEmails)
            {
                initialSplits[email] = new Split { UserEmail = email };
            }
            currExpense.UserEmailVsSplit = initialSplits;

            _splitService.CalculateSplits(currExpense, userEmailVsValue);

            _balanceService.UpdateBalances(currExpense);

            _mockDb.AddEntity(currExpense);
        }

        public List<Expense> GetAllExpenses()
        {
            return _mockDb.GetExpenses();
        }

        public List<Expense> GetAllExpensesForEmail(string userEmail)
        {
            var isValidEmail = CommonUtility.IsValidEmail(userEmail);
            if (!isValidEmail)
                throw new ArgumentException("Invalid email format.");

            return _mockDb.GetExpenses().Where(u => u.UserEmailVsSplit.ContainsKey(userEmail)).ToList();
        }

        public List<Expense> GetOwedExpensesForEmail(string userEmail)
        {
            var isValidEmail = CommonUtility.IsValidEmail(userEmail);
            if (!isValidEmail)
                throw new ArgumentException("Invalid email format.");

            return _mockDb.GetExpenses().Where(u => u.UserEmailVsSplit.ContainsKey(userEmail) && !u.PaidByUserEmail.Equals(userEmail, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Expense> GetPaidExpensesForEmail(string userEmail)
        {
            var isValidEmail = CommonUtility.IsValidEmail(userEmail);
            if (!isValidEmail)
                throw new ArgumentException("Invalid email format.");

            return _mockDb.GetExpenses().Where(u => u.PaidByUserEmail.Equals(userEmail, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
