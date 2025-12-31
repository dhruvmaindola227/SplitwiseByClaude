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
            throw new NotImplementedException();
        }

        public Dictionary<string, int> GetBalanceForUser(string userEmail)
        {
            var usersBalanceInfoMatrix = _mockDb.GetBalancesData();
            var userVsIndexDict = _mockDb.GetUserEmailToIndexMap();
            // dhruv -> 0
            // 
            return new();
        }

        public void UpdateBalances(Expense expense)
        {
            var usersBalanceInfoMatrix = _mockDb.GetBalancesData();
            var userVsIndexDict = _mockDb.GetUserEmailToIndexMap();
            var paidByIndex = userVsIndexDict[expense.PaidByUserEmail];
            var newMatrix = new int[usersBalanceInfoMatrix.Length, usersBalanceInfoMatrix.Length];
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
            var matrixSize = usersBalanceInfoMatrix.Length;
            int newCount = matrixSize + 1;
            var newMatrix = new int[newCount, newCount];
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
