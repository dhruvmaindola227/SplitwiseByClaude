using SplitwiseByClaude.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitwiseByClaude.Services
{
    public interface IUserService
    {
        public void AddUser(string name, string email, string phoneNumber);
        public List<User> GetAllUsers();
        public User? GetUserByEmail(string email);
    }
}
