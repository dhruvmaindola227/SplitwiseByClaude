using SplitwiseByClaude.Services;
using static SplitwiseByClaude.Entities.Enum;

namespace SplitwiseByClaude
{
    public class SplitwiseApplication
    {
        private readonly IUserService _userService;
        private readonly IExpenseService _expenseService;
        public SplitwiseApplication(IUserService userService, IExpenseService expenseService)
        {
            _expenseService = expenseService;
            _userService = userService;
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
    }
}
