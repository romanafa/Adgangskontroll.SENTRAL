using Adgangskontroll.SENTRAL.Data;
using Adgangskontroll.SENTRAL.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adgangskontroll.SENTRAL.Repository
{
    public class KortleserRepository
    {
        private readonly DbConnection _db;

        public KortleserRepository()
        {
            _db = new DbConnection();
        }

        public void OpprettKortleser(Kortleser kortleser)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO Kortleser (KortleserNummer, KortleserPlassering) VALUES (@KortleserNummer, @KortleserPlassering)";

                    cmd.Parameters.AddWithValue("KortleserNummer", kortleser.KortleserNummer);
                    cmd.Parameters.AddWithValue("KortleserPlassering", kortleser.KortleserPlassering);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool SjekkOmKortleserNummerEksisterer(string kortleserNummer)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT COUNT(*) FROM Kortleser WHERE KortleserNummer = @KortleserNummer";
                    cmd.Parameters.AddWithValue("KortleserNummer", kortleserNummer);

                    object result = cmd.ExecuteScalar();
                    int count = result != null ? Convert.ToInt32(result) : 0;

                    return count > 0; // returnerer true dersom kortlesernummer finnes
                }
            }
        }
    }
}
