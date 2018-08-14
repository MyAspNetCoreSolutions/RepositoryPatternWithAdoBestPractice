using DataAccessLayer;
using DataAccessLayer.Contracts;
using DataAccessLayer.Repository;
using Domain.Model.Entities;
using lotfi.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotfi.Services.Services
{
    public class UserServices : IUserServices
    {
        private IConnectionFactory _connectionFactory;
        private DbContext _context;
        private UserRepository _userRepo;
        public UserServices()
        {
            _connectionFactory = ConnectionHelper.GetConnection();
            _context = new DbContext(_connectionFactory);
            _userRepo = new UserRepository(_context);
        }

        public IList<User> GetUsers()
        {
            return _userRepo.GetUsers();
        }

        public User Login(string id, string password)
        {
            return _userRepo.LoginUser(id, password);
        }

        public User RegisterUser(User user)
        {
            return _userRepo.CreateUser(user);
        }

        public bool UserNameExists(string username, string email)
        {
            var user = _userRepo.GetUserByUserNameOrEmail(username, email);
            return !(user != null && user.UserID > 0);
        }
    }
}
