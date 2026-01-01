using SplitwiseByClaude.Entities;
using SplitwiseByClaude.SplitwiseDb;

namespace SplitwiseByClaude.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IMockDb _mockDb;
        public BalanceService(IMockDb mockDb)
        {
            _mockDb = mockDb;
        }
        public Dictionary<string, Dictionary<string, int>> GetAllBalances()
        {
            Dictionary<string, Dictionary<string, int>> lenderVsOwerVsAmountDict = new(StringComparer.OrdinalIgnoreCase);
            var usersBalanceInfoMatrix = _mockDb.GetBalancesData();
            var userVsIndexDict = _mockDb.GetUserEmailToIndexMap();
            Dictionary<int, string> indexVsUserEmailReverseDict = userVsIndexDict.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            for (int i = 0; i < usersBalanceInfoMatrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < usersBalanceInfoMatrix.GetLength(0); j++)
                {
                    int balance = usersBalanceInfoMatrix[i, j] - usersBalanceInfoMatrix[j, i];
                    if (balance != 0)
                    {
                        var lenderIndex = balance > 0 ? i : j;
                        var borrowerIndex = balance < 0 ? i : j;
                        var lenderEmail = indexVsUserEmailReverseDict[lenderIndex];
                        var borrowerEmail = indexVsUserEmailReverseDict[borrowerIndex];
                        if (!lenderVsOwerVsAmountDict.TryGetValue(lenderEmail, out var borrowerVsAmount))
                        {
                            borrowerVsAmount = new(StringComparer.OrdinalIgnoreCase);
                            lenderVsOwerVsAmountDict[lenderEmail] = borrowerVsAmount;
                        }
                        borrowerVsAmount[borrowerEmail] = Math.Abs(balance);
                    }
                }
            }
            return lenderVsOwerVsAmountDict;
        }

        public Dictionary<string, int> GetBalanceForUser(string userEmail)
        {
            Dictionary<string, int> userEmailVsBalance = new(StringComparer.OrdinalIgnoreCase);
            var usersBalanceInfoMatrix = _mockDb.GetBalancesData();
            var userVsIndexDict = _mockDb.GetUserEmailToIndexMap();
            int currUserIndex = userVsIndexDict[userEmail];
            Dictionary<int, string> indexVsUserEmailReverseDict = userVsIndexDict.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            for (int i = 0; i < usersBalanceInfoMatrix.GetLength(0); i++)
            {
                if (i == currUserIndex) continue; // ignore self
                int balance = usersBalanceInfoMatrix[currUserIndex, i] - usersBalanceInfoMatrix[i, currUserIndex];
                if (balance != 0)
                {
                    var otherUserEmail = indexVsUserEmailReverseDict[i];
                    userEmailVsBalance[otherUserEmail] = balance;
                }
                    
            }
            return userEmailVsBalance;
        }

        public void UpdateBalances(Expense expense)
        {
            var usersBalanceInfoMatrix = _mockDb.GetBalancesData();
            var userVsIndexDict = _mockDb.GetUserEmailToIndexMap();
            var paidByIndex = userVsIndexDict[expense.PaidByUserEmail];
            Console.WriteLine($"UpdateBalances -> IndexCount before: {usersBalanceInfoMatrix.GetLength(0)}");
            var newMatrix = new int[usersBalanceInfoMatrix.GetLength(0), usersBalanceInfoMatrix.GetLength(0)];
            Console.WriteLine($"UpdateBalances -> IndexCount after: {usersBalanceInfoMatrix.GetLength(0)}");
            usersBalanceInfoMatrix.Copy2DArray(newMatrix);
            foreach (var split in expense.UserEmailVsSplit.Values)
            {
                if (string.Equals(split.UserEmail, expense.PaidByUserEmail, StringComparison.OrdinalIgnoreCase))
                    continue;
                var owedByIndex = userVsIndexDict[split.UserEmail];
                newMatrix[paidByIndex, owedByIndex] += split.Amount;
            }
            _mockDb.UpdateBalancesData(newMatrix, userVsIndexDict);
            // sam -> dhruv -> 100
            // sam -> 1, dhruv -> 0
            // payee -> i, owee -> j
        }

        public void UpdateUsersBalanceInfo(string userEmail)
        {
            var usersBalanceInfoMatrix = _mockDb.GetBalancesData();
            var userVsIndexDict = _mockDb.GetUserEmailToIndexMap();
            var matrixSize = usersBalanceInfoMatrix.GetLength(0);
            Console.WriteLine($"UpdateUsersBalanceInfo -> IndexCount before: {matrixSize}");
            int newCount = matrixSize + 1;
            var newMatrix = new int[newCount, newCount];
            Console.WriteLine($"UpdateUsersBalanceInfo -> IndexCount after: {newMatrix.GetLength(0)}");
            var userVsIndexLocal = userVsIndexDict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
            userVsIndexLocal.TryAdd(userEmail, matrixSize);
            usersBalanceInfoMatrix.Copy2DArray(newMatrix);
            _mockDb.UpdateBalancesData(newMatrix, userVsIndexLocal);
            //      dhruv      sam      shu
            //dhruv [[0, 0], [0, 1], [0, 2]]
            //sam   [[1, 0], [1, 1], [1, 2]]
            //shu   [[2, 0], [2, 1], [2, 2]]
        }
    }
}
