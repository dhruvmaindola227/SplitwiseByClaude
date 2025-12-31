using SplitwiseByClaude.Entities;
using SplitwiseByClaude.SplitwiseDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SplitwiseByClaude.Services
{
    public class UserService : IUserService
    {
        private readonly IMockDb _mockDb;
        private readonly IBalanceService _balanceService;
        public UserService(IMockDb mockDb, IBalanceService balanceService)
        {
            _mockDb = mockDb;
            _balanceService = balanceService;
        }
        public void AddUser(string name, string email, string phoneNumber)
        {
            // validate params
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Name, email, and phone number cannot be null or empty.");

            // validate email
           var isValidEmail = CommonUtility.IsValidEmail(email);
            if (!isValidEmail)
                throw new ArgumentException("Invalid email format.");

            // validate user
            User? currentUser = GetUserByEmail(email);
            if (currentUser is not null)
                throw new InvalidOperationException("User with the same email already exists.");

            _balanceService.UpdateUsersBalanceInfo(email);

            _mockDb.AddEntity<User>(new User
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
            });
        }

        public List<User> GetAllUsers()
        {
            _mockDb.GetUsers().Add(new User() { Email = "mukesh@gmail.com", Name = "asd", PhoneNumber = "34342" });
            return _mockDb.GetUsers();
        }

        public User? GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.");

            // validate email
            var isValidEmail = CommonUtility.IsValidEmail(email);
            if (!isValidEmail)
                throw new ArgumentException("Invalid email format.");

            return _mockDb.GetUsers().SingleOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
