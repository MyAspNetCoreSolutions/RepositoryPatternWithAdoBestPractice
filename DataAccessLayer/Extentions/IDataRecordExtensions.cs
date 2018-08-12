using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Extentions
{
    public static class IDataRecordExtensions
    {
        public static bool HasColumn(this IDataRecord record,string columnName)
        {
            for (int i = 0; i < record.FieldCount; i++)
            {
                if (record.GetName(i).Equals(columnName,StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
