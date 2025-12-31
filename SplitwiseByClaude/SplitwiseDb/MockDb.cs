using SplitwiseByClaude.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitwiseByClaude.SplitwiseDb
{
    public sealed class MockDb : IMockDb
    {
        private readonly List<User> _users;
        private readonly List<Expense> _expenses;
        private int[,] _balancesData;
        private readonly Dictionary<string, int> _userEmailToIndexMap;
        public MockDb()
        {
            _users = new();
            _expenses = new();
            _balancesData = new int[0,0];
            _userEmailToIndexMap = new(StringComparer.OrdinalIgnoreCase);
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public List<Expense> GetExpenses()
        {
            return _expenses;
        }
        
        public void AddEntity<T>(T entity) where T : class
        {
            if (entity is User user)
                _users.Add(user);
            else if (entity is Expense expense)
                _expenses.Add(expense);
            else throw new InvalidDataException("Invalid entity type");
        }

        public int[,] GetBalancesData()
        {
            return _balancesData;
        }

        public void UpdateBalancesData(int[,] balancesData)
        {
            _balancesData = balancesData;
        }

        public Dictionary<string, int> GetUserEmailToIndexMap()
        {
            return _userEmailToIndexMap;
        }

        public void UpdateBalancesData(int[,] balancesData, Dictionary<string, int> userEmailToIndex)
        {
            
        }
    }
}
