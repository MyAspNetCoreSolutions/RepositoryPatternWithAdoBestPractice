using DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DbContext
    {
        private readonly IDbConnection _connection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();
        private readonly LinkedList<AdoNetUnitOfWork> _unitOfWorks = new LinkedList<AdoNetUnitOfWork>();

        public DbContext(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.Create();
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var transaction = _connection.BeginTransaction();
            var unitOfWork = new AdoNetUnitOfWork(transaction, RemoveTransaction, RemoveTransaction);

            _readerWriterLock.EnterWriteLock();
            _unitOfWorks.AddLast(unitOfWork);
            _readerWriterLock.ExitWriteLock();

            return unitOfWork;
        }

        public IDbCommand CreateCommand()
        {
            var command = _connection.CreateCommand();
            _readerWriterLock.EnterReadLock();
            if (_unitOfWorks.Count>0)
            {
                command.Transaction = _unitOfWorks.First.Value.Transaction;
            }
            _readerWriterLock.ExitReadLock();
            return command;
        }

        private void RemoveTransaction(AdoNetUnitOfWork work)
        {
            _readerWriterLock.EnterWriteLock();
            _unitOfWorks.Remove(work);
            _readerWriterLock.ExitWriteLock();
        }
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
