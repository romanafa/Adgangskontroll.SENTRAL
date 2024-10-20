using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adgangskontroll.SENTRAL.Data
{
    public class DbConnection
    {
        private string _connectionString;

        public DbConnection()
        {
            _connectionString = "Host=ider-database.westeurope.cloudapp.azure.com;Port=5432;Username=;Password=;Database=";
        }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
