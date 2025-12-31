using SplitwiseByClaude.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitwiseByClaude.SplitwiseDb
{
    public class MockDb : IMockDb
    {
        private readonly List<User> Users;
        private readonly List<Expense> Expenses;
        public MockDb()
        {
            Users = new();
            Expenses = new();
        }

        public List<User> GetUsers()
        {
            return Users;
        }
        public List<Expense> GetExpenses()
        {
            return Expenses;
        }
        
        public void AddEntity<T>(T entity) where T : class
        {
            if (entity is User user)
                Users.Add(user);
            else if (entity is Expense expense)
                Expenses.Add(expense);
            else throw new InvalidDataException("Invalid entity type");
        }
    }
}
