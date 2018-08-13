using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AdoNetUnitOfWork : IUnitOfWork
    {
        private IDbTransaction _transaction;
        private readonly Action<AdoNetUnitOfWork> _committed;
        private readonly Action<AdoNetUnitOfWork> _rolledBack;

        public IDbTransaction Transaction { get; }

        public AdoNetUnitOfWork(IDbTransaction dbTransaction, Action<AdoNetUnitOfWork> rolledBack, Action<AdoNetUnitOfWork> commited)
        {
            Transaction = dbTransaction;
            _transaction = dbTransaction;
            _rolledBack = rolledBack;
            _committed = commited;
        }


        public void Dispose()
        {
            if (_transaction == null)
            {
                return;
            }
            _transaction.Rollback();
            _transaction.Dispose();
            _rolledBack(this);
            _transaction = null;

        }

        public void SaveChange()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("May not call Save change twice.");
            }
            _transaction.Commit();
            _committed(this);
            _transaction = null;
        }
    }
}
