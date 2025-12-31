using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SplitwiseByClaude.Services;
using SplitwiseByClaude.SplitwiseDb;
using static SplitwiseByClaude.Entities.Enum;

namespace SplitwiseByClaude
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddSingleton<IMockDb, MockDb>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IExpenseService, ExpenseService>();
            builder.Services.AddSingleton<ISplitService, SplitService>();
            builder.Services.AddSingleton<IBalanceService, BalanceService>();
            builder.Services.AddSingleton<SplitwiseApplication>();
            var app = builder.Build();
            Console.WriteLine("Splitwise application");
            var splitwiseApp = app.Services.GetRequiredService<SplitwiseApplication>();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Add user");
                Console.WriteLine("2. Add expense");
                Console.WriteLine("3. List all users");
                Console.WriteLine("4. List all expenses");
                Console.WriteLine("5. List all expenses for a user");
                Console.WriteLine("6. List owed expenses for a user");
                Console.WriteLine("7. List paid expenses for a user");
                Console.WriteLine("0. Exit");
                Console.Write("Option: ");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Write("Name: ");
                        var name = Console.ReadLine() ?? string.Empty;
                        Console.Write("Email: ");
                        var email = Console.ReadLine() ?? string.Empty;
                        Console.Write("Phone: ");
                        var phone = Console.ReadLine() ?? string.Empty;
                        try
                        {
                            splitwiseApp.AddUser(name, email, phone);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case "2":
                        try
                        {
                            Console.Write("Description: ");
                            var description = Console.ReadLine() ?? string.Empty;
                            Console.Write("Amount (integer): ");
                            var amountText = Console.ReadLine() ?? "0";
                            int amount = int.Parse(amountText);
                            Console.Write("Date (yyyy-MM-dd) or leave empty for today: ");
                            var dateText = Console.ReadLine();
                            DateTime date = string.IsNullOrWhiteSpace(dateText) ? DateTime.Today : DateTime.Parse(dateText);
                            Console.Write("Paid by (email): ");
                            var paidBy = Console.ReadLine() ?? string.Empty;
                            Console.WriteLine("Split type (1=Equal, 2=Exact, 3=Percentage): ");
                            var st = Console.ReadLine();
                            SplitType splitType = SplitType.EqualSplit;
                            switch (st)
                            {
                                case "1": splitType = SplitType.EqualSplit; break;
                                case "2": splitType = SplitType.ExactSplit; break;
                                case "3": splitType = SplitType.PercentageSplit; break;
                                default: Console.WriteLine("Invalid split type selected, using EqualSplit"); break;
                            }

                            Console.WriteLine("Enter participant emails separated by comma:");
                            var participantsRaw = Console.ReadLine() ?? string.Empty;
                            var participants = participantsRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

                            Dictionary<string, int>? userEmailVsValue = null;
                            if (splitType == SplitType.ExactSplit || splitType == SplitType.PercentageSplit)
                            {
                                userEmailVsValue = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                                foreach (var p in participants)
                                {
                                    Console.Write($"Enter value for {p} ({(splitType == SplitType.ExactSplit ? "amount" : "percentage")}): ");
                                    var valText = Console.ReadLine() ?? "0";
                                    if (!int.TryParse(valText, out var val))
                                    {
                                        val = 0;
                                    }
                                    userEmailVsValue[p] = val;
                                }
                            }

                            splitwiseApp.AddExpense(description, amount, date, paidBy, splitType, participants, userEmailVsValue);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case "3":
                        splitwiseApp.GetAllUsers();
                        break;
                    case "4":
                        splitwiseApp.GetAllExpenses();
                        break;
                    case "5":
                        Console.Write("User email: ");
                        var ue = Console.ReadLine() ?? string.Empty;
                        try
                        {
                            splitwiseApp.GetAllExpensesForUser(ue);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case "6":
                        Console.Write("User email: ");
                        var ue2 = Console.ReadLine() ?? string.Empty;
                        try
                        {
                            splitwiseApp.GetOwedExpensesForUser(ue2);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case "7":
                        Console.Write("User email: ");
                        var ue3 = Console.ReadLine() ?? string.Empty;
                        try
                        {
                            splitwiseApp.GetPaidExpensesForUser(ue3);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }

            // don't run the host loop
            // app.Run();
        }
    }
}
