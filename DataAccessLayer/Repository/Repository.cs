using DataAccessLayer.Extentions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Repository
{
    public abstract class Repository<T> where T : new()
    {
        DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        protected DbContext Context { get { return _context; } }

        protected IEnumerable<T> Tolist(IDbCommand command)
        {
            using (var record=command.ExecuteReader())
            {
                List<T> items = new List<T>();
                while (record.Read())
                {
                    items.Add(Map<T>(record));
                }
                return items;
            }

        }

        private T Map<T>(IDataRecord record)
        {
            var objectT = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
                    property.SetValue(objectT, record[property.Name]);
            }
            return objectT;
        }

    }
}
