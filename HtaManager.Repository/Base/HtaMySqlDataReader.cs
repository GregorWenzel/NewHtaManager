using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Repository.Base
{
    public class HtaMySqlDataReader
    {
        readonly MySqlDataReader reader;

        public HtaMySqlDataReader(MySqlDataReader reader)
        {
            this.reader = reader;
        }

        public bool Read()
        {
            return reader.Read();
        }

        public int GetInt32(string colName)
        {
            var colIndex = reader.GetOrdinal(colName);
            return !reader.IsDBNull(colIndex) ? reader.GetInt32(colIndex) : default(int);
        }

        public string GetString(string colName)
        {
            var colIndex = reader.GetOrdinal(colName);
            return !reader.IsDBNull(colIndex) ? reader.GetString(colIndex) : string.Empty;
        }
    }
}
