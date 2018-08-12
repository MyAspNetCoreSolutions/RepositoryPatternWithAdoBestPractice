using Domain.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    class UserRepository : Repository<User>
    {
        private DbContext _context;

        public UserRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public IList<User> GetUsers()
        {
            using (var comand=_context.CreateCommand())
            {
                comand.CommandText = "Get Query";
                return this.Tolist(comand).ToList();
            }
        }

        public User CreateUser(User user)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "signup user query";
                command.Parameters.Add(command.CreateParameter("@pFirstName", user.FirstName));
                command.Parameters.Add(command.CreateParameter("@pLastName", user.LastName));
                command.Parameters.Add(command.CreateParameter("@pUserName", user.UserName));
                command.Parameters.Add(command.CreateParameter("@pPassword", user.Password));
                command.Parameters.Add(command.CreateParameter("@pEmail", user.Email));

                return this.Tolist(command).FirstOrDefault();
            }
        }
    }
}
