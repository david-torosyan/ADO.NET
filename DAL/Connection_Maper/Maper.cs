using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Connection_Maper
{
    static public class Maper<T>
    {

        static public T MapDataReaderToEntity(IDataReader reader)
        {
            // Implement the logic to map data reader to your entity
            var entity = Activator.CreateInstance<T>();

            // Assuming columns in the reader match properties in the entity
            foreach (var property in typeof(T).GetProperties())
            {
                var columnName = property.Name; // Assuming column names match property names
                var value = reader[columnName];
                if (value != DBNull.Value)
                {
                    property.SetValue(entity, value);
                }
            }

            return entity;
        }
    }
}
