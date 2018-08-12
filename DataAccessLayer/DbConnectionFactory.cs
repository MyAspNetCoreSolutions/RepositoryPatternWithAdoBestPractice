using DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    class DbConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _dbProvider;
        private readonly string _conectionString;
        private readonly string _name;
        public DbConnectionFactory(string connectionName)
        {
            if (connectionName==null)
            {
                throw new ArgumentNullException("ConnectionName");
            }

            var conStr = ConfigurationManager.ConnectionStrings[connectionName];
            if (conStr==null)
            {
                throw new ConfigurationErrorsException(
                    string.Format("Failed to find connection string named '{0}' in app/web.config.",
                                    connectionName));
            }

            _name = conStr.ProviderName;
            _dbProvider = DbProviderFactories.GetFactory(conStr.ProviderName);
            _conectionString = conStr.ConnectionString;

        }
        public IDbConnection Create()
        {
            var conection = _dbProvider.CreateConnection();
            if (conection==null)
            {
                throw new ConfigurationErrorsException(string.Format("Failed to create a connection using the connection string named '{0}' in app/web.config.", _name));
            }
            conection.ConnectionString = _conectionString;
            conection.Open();
            return conection;
        }
    }
}
