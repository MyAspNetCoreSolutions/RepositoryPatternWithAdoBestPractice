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
        private IDbTransaction transaction;
        private Func<Action<AdoNetUnitOfWork>> removeTransaction1;
        private Func<Action<AdoNetUnitOfWork>> removeTransaction2;
        private Func<Action<AdoNetUnitOfWork>> removeTransaction;
        private object remo;
        private readonly Action<AdoNetUnitOfWork> _commited;
        private readonly Action<AdoNetUnitOfWork> _rolledBack;
        public IDbTransaction Transaction { get; }

        public AdoNetUnitOfWork(IDbTransaction dbTransaction, Action<AdoNetUnitOfWork> rolledBack, Action<AdoNetUnitOfWork> commited)
        {
            Transaction = dbTransaction;
            _transaction = dbTransaction;
            _rolledBack = rolledBack;
            _commited = commited;
        }

        public AdoNetUnitOfWork(IDbTransaction transaction, Func<Action<AdoNetUnitOfWork>> removeTransaction1, Func<Action<AdoNetUnitOfWork>> removeTransaction2)
        {
            this.transaction = transaction;
            this.removeTransaction1 = removeTransaction1;
            this.removeTransaction2 = removeTransaction2;
        }

        public AdoNetUnitOfWork(IDbTransaction transaction, Func<Action<AdoNetUnitOfWork>> removeTransaction, object remo)
        {
            this.transaction = transaction;
            this.removeTransaction = removeTransaction;
            this.remo = remo;
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
                throw new InvalidOperationException("May not call Savechange twice.");
            }
            _transaction.Commit();
            _commited(this);
            _transaction = null;
        }
    }
}
