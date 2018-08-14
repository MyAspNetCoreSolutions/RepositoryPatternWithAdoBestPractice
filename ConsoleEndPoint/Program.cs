using lotfi.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEndPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            UserServices uServ = new UserServices();
            var list = uServ.GetUsers();
            foreach (var item in list)
            {
                Console.WriteLine($" {item.FirstName} {item.LastName}");
            }
            Console.ReadKey();
        }
    }
}
