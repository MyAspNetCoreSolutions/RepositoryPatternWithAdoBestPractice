using DataAccessLayer.Extentions;
using Domain.Model.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class UserRepository : Repository<User>
    {
        private DbContext _context;

        public UserRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public IList<User> GetUsers()
        {
            using (var command=_context.CreateCommand())
            {
                command.CommandText = "exec [dbo].[uspGetUsers]";

                return Tolist(command).ToList();
            }
        }

        public User CreateUser(User user)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "uspSignUp";

                command.Parameters.Add(command.CreateParameter("@pFirstName", user.FirstName));
                command.Parameters.Add(command.CreateParameter("@pLastName",  user.LastName));
                command.Parameters.Add(command.CreateParameter("@pUserName",  user.UserName));
                command.Parameters.Add(command.CreateParameter("@pPassword",  user.Password));
                command.Parameters.Add(command.CreateParameter("@pEmail",     user.Email));

                return Tolist(command).FirstOrDefault();
            }
        }

        public User LoginUser(string username,string password)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "uspSignIn";

                command.Parameters.Add(command.CreateParameter("@pUserName", username));
                command.Parameters.Add(command.CreateParameter("@pPassword", password));

                return Tolist(command).FirstOrDefault();
            }
        }

        public User GetUserByUserNameOrEmail(string userName,string email)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "uspGetUserByUsernameOrEmail";

                command.Parameters.Add(command.CreateParameter("@pUserName",userName));
                command.Parameters.Add(command.CreateParameter("@pEmail",email));

                return Tolist(command).FirstOrDefault();
            }
        }
    }
}
