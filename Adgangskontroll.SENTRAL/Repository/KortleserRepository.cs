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

        public void OpprettKortleser(KortleserModel kortleser)
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

        public void SlettKortleser(int kortleserID)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM Kortleser WHERE KortleserID = @KortleserID";
                    cmd.Parameters.AddWithValue("KortleserID", kortleserID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if(rowsAffected > 0)
                    {
                        Console.WriteLine($"Kortleser med KortleserID {kortleserID} ble slettet.");
                    }
                    else
                    {
                        Console.WriteLine($"Ingen kortleser funnet med KortleserID {kortleserID}.");
                    }
                }
            }
        }

        public KortleserModel FinnKortleserEtterId(int kortleserID)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM Kortleser WHERE KortleserID = @KortleserID";
                    cmd.Parameters.AddWithValue("KortleserID", kortleserID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new KortleserModel
                            {
                                KortleserID = reader.GetInt32(reader.GetOrdinal("KortleserID")),
                                KortleserNummer = reader.GetString(reader.GetOrdinal("KortleserNummer")),
                                KortleserPlassering = reader.GetString(reader.GetOrdinal("KortleserPlassering"))
                            };
                        }
                    }
                }
            }
            return null; // Returnerer null om ingenting er funnet
        }
    }
}
