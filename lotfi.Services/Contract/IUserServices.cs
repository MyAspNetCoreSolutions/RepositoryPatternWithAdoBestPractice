using Domain.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotfi.Services.Contract
{
    public interface IUserServices
    {
        IList<User> GetUsers();
        
        User RegisterUser(User user);
        
        User Login(string id, string password);
        
        bool UserNameExists(string username, string email);
    }
}
