using SplitwiseByClaude.Services;
using static SplitwiseByClaude.Entities.Enum;

namespace SplitwiseByClaude
{
    public class SplitwiseApplication
    {
        private readonly IUserService _userService;
        private readonly IExpenseService _expenseService;
        private readonly IBalanceService _balanceService;
        public SplitwiseApplication(IUserService userService, IExpenseService expenseService, IBalanceService balanceService)
        {
            _expenseService = expenseService;
            _userService = userService;
            _balanceService = balanceService;
        }
        public void AddExpense(string description, int amount, DateTime dateIncurred, string paidByEmail, SplitType splitType, List<string> splitBetweenEmails, Dictionary<string, int>? userEmailVsValue)
        {
            _expenseService.AddExpense(description, amount, dateIncurred, paidByEmail, splitType, splitBetweenEmails, userEmailVsValue);
            Console.WriteLine("Expense added successfully.");
        }

        public void AddUser(string name, string email, string phoneNumber)
        {
            _userService.AddUser(name, email, phoneNumber);
            Console.WriteLine("User added successfully.");
        }

        public void GetAllUsers()
        {
            Console.WriteLine("Users\n");
            foreach (var user in _userService.GetAllUsers())
            {
                Console.WriteLine($"Name: {user.Name}, Email: {user.Email}, PhoneNumber: {user.PhoneNumber}");
            }
        }

        public void GetAllExpenses()
        {
            Console.WriteLine("Expenses\n");
            foreach (var expense in _expenseService.GetAllExpenses())
            {
                Console.WriteLine($"Description: {expense.Description}, Amount: {expense.Amount}, DateIncurred: {expense.DateIncurred}, PaidBy: {expense.PaidByUserEmail}, SplitType: {expense.SplitType}");
            }
        }

        public void GetAllExpensesForUser(string userEmail)
        {
            Console.WriteLine($"Expenses for user: {userEmail}\n");
            var paidExpenses = _expenseService.GetAllExpensesForEmail(userEmail);
            Console.WriteLine("All Expenses:");
            foreach (var expense in paidExpenses)
            {
                Console.WriteLine($"Description: {expense.Description}, Amount: {expense.Amount}, AmounOwed: {expense.UserEmailVsSplit[userEmail].Amount}, DateIncurred: {expense.DateIncurred}, SplitType: {expense.SplitType}");
            }
        }

        public void GetOwedExpensesForUser(string userEmail)
        {
            Console.WriteLine($"Owed Expenses for user: {userEmail}\n");
            var owedExpenses = _expenseService.GetOwedExpensesForEmail(userEmail);
            Console.WriteLine("Owed Expenses:");
            foreach (var expense in owedExpenses)
            {
                Console.WriteLine($"Description: {expense.Description}, Amount: {expense.Amount}, AmounOwed: {expense.UserEmailVsSplit[userEmail].Amount}, DateIncurred: {expense.DateIncurred}, SplitType: {expense.SplitType}");
            }
        }

        public void GetPaidExpensesForUser(string userEmail)
        {
            Console.WriteLine($"Paid Expenses for user: {userEmail}\n");
            var paidExpenses = _expenseService.GetPaidExpensesForEmail(userEmail);
            Console.WriteLine("Paid Expenses:");
            foreach (var expense in paidExpenses)
            {
                Console.WriteLine($"Description: {expense.Description}, Amount: {expense.Amount}, AmounOwed: {expense.UserEmailVsSplit[userEmail].Amount}, DateIncurred: {expense.DateIncurred}, SplitType: {expense.SplitType}");
            }
        }

        public void GetAllBalances()
        {
            Console.WriteLine("Balances:\n");
            var balances = _balanceService.GetAllBalances();
            foreach (var lender in balances)
            {
                foreach (var borrower in lender.Value)
                {
                    Console.WriteLine($"{borrower.Key} owes {lender.Key}: {borrower.Value}");
                }
            }
        }

        public void GetBalanceForUser(string userEmail)
        {
            Console.WriteLine($"Balances for user: {userEmail}\n");
            var balances = _balanceService.GetBalanceForUser(userEmail);
            foreach (var entry in balances)
            {
                if (entry.Value > 0)
                    Console.WriteLine($"{entry.Key} owes {userEmail}: {entry.Value}");
                else
                    Console.WriteLine($"{userEmail} owes {entry.Key}: {Math.Abs(entry.Value)}");
            }
        }
    }
}
