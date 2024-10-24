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
    public class BrukerRepository
    {
        private readonly DbConnection _db;

        public BrukerRepository()
        {
            _db = new DbConnection();
        }

        public void OpprettBruker(Bruker bruker)
        {
            using (var dbTilkobling = _db.GetConnection())
            {
                dbTilkobling.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = dbTilkobling;
                    cmd.CommandText = "INSERT INTO Bruker (Fornavn, Etternavn, Epost, KortID, PIN) " +
                                      "VALUES (@Fornavn, @Etternavn, @Epost, @KortID, @PIN)";

                    // Mappe parametere for å forhindre SQL-injeksjon
                    cmd.Parameters.AddWithValue("Fornavn", bruker.Fornavn);
                    cmd.Parameters.AddWithValue("Etternavn", bruker.Etternavn);
                    cmd.Parameters.AddWithValue("Epost", bruker.Epost);
                    cmd.Parameters.AddWithValue("KortID", bruker.KortID);
                    cmd.Parameters.AddWithValue("PIN", bruker.PIN);

                    // Kjør SQL-spørring
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void OppdaterBruker(Bruker bruker)
        {
            using (var dbTilkobling = _db.GetConnection())
            {
                dbTilkobling.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = dbTilkobling;

                    // Oppdatert verdier basert på bruker input (forandret verdier eller ikke)
                    string oppdaterSpørring = "UPDATE Bruker SET ";
                    bool erFørsteFelt = true;   // sjekk hvilket felt i databasen er blitt oppdatert for riktig SQL-spørring

                    if (!string.IsNullOrEmpty(bruker.Fornavn))
                    {
                        oppdaterSpørring += "Fornavn = @Fornavn";
                        erFørsteFelt = false;
                    }

                    if (!string.IsNullOrEmpty(bruker.Etternavn))
                    {
                        oppdaterSpørring += (erFørsteFelt ? "" : ", ") + "Etternavn = @Etternavn";
                        erFørsteFelt = false;
                    }

                    if (!string.IsNullOrEmpty(bruker.Epost))
                    {
                        oppdaterSpørring += (erFørsteFelt ? "" : ", ") + "Epost = @Epost";
                        erFørsteFelt = false;
                    }

                    if (!string.IsNullOrEmpty(bruker.KortID))
                    {
                        oppdaterSpørring += (erFørsteFelt ? "" : ", ") + "KortID = @KortID";
                        erFørsteFelt = false;
                    }

                    if (!string.IsNullOrEmpty(bruker.PIN))
                    {
                        oppdaterSpørring += (erFørsteFelt ? "" : ", ") + "PIN = @PIN";
                    }

                    // Oppdater SQL spørring
                    oppdaterSpørring += " WHERE BrukerID = @BrukerID";

                    // Sett kommandotekst med SQL-spørring
                    cmd.CommandText = oppdaterSpørring;

                    // Mappe parametere
                    if (!string.IsNullOrEmpty(bruker.Fornavn))
                        cmd.Parameters.AddWithValue("Fornavn", bruker.Fornavn);
                    if (!string.IsNullOrEmpty(bruker.Etternavn))
                        cmd.Parameters.AddWithValue("Etternavn", bruker.Etternavn);
                    if (!string.IsNullOrEmpty(bruker.Epost))
                        cmd.Parameters.AddWithValue("Epost", bruker.Epost);
                    if (!string.IsNullOrEmpty(bruker.KortID))
                        cmd.Parameters.AddWithValue("KortID", bruker.KortID);
                    if (!string.IsNullOrEmpty(bruker.PIN))
                        cmd.Parameters.AddWithValue("PIN", bruker.PIN);

                    cmd.Parameters.AddWithValue("BrukerID", bruker.BrukerID);  // Oppdaterer brukerdata basert på oppgitt brukerID

                    // Kjør SQL-spørring
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SlettBruker(int brukerID)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM Bruker WHERE BrukerID = @BrukerID";

                    // Add parameter to prevent SQL injection
                    cmd.Parameters.AddWithValue("BrukerID", brukerID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Bruker med BrukerID {brukerID} ble slettet.");
                    }
                    else
                    {
                        Console.WriteLine($"Ingen bruker funnet med BrukerID {brukerID}.");
                    }
                }
            }
        }

        public Bruker FinnBrukerEtterID(int brukerID)
        {
            Bruker bruker = null;

            using (var dbTilkobling = _db.GetConnection())
            {
                dbTilkobling.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = dbTilkobling;

                    // SQL-spørring for å finne bruker
                    cmd.CommandText = "SELECT BrukerID, Fornavn, Etternavn, Epost, KortID, PIN FROM Bruker WHERE BrukerID = @BrukerID";
                    cmd.Parameters.AddWithValue("BrukerID", brukerID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())  // Dersom bruker eksisterer, få alle verdier 
                        {
                            bruker = new Bruker
                            {
                                BrukerID = reader.GetInt32(0),
                                Fornavn = reader.GetString(1),
                                Etternavn = reader.GetString(2),
                                Epost = reader.GetString(3),
                                KortID = reader.GetString(4),
                                PIN = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return bruker;
        }

        public bool SjekkOmEpostEksisterer(string epost)
        {
            using (var dbTilkobling = _db.GetConnection())
            {
                dbTilkobling.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = dbTilkobling;

                    // SQL-spørring for å finne epost
                    cmd.CommandText = "SELECT COUNT(*) FROM Bruker WHERE Epost = @Epost";
                    cmd.Parameters.AddWithValue("Epost", epost);

                    object resultat = cmd.ExecuteScalar();

                    // Håndter null-resultater og konverter til int
                    int teller = resultat != null ? Convert.ToInt32(resultat) : 0;

                    // om teller er > 0, bruker med samme epost eksisterer
                    return teller > 0;  
                }
            }
        }

        public bool SjekkOmKortIDEksisterer(string kortId)
        {
            using (var dbTilkobling = _db.GetConnection())
            {
                dbTilkobling.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = dbTilkobling;

                    // SQL-spørring for å finne kort ID
                    cmd.CommandText = "SELECT COUNT(*) FROM Bruker WHERE KortID = @KortID";
                    cmd.Parameters.AddWithValue("KortID", kortId);

                    object resultat = cmd.ExecuteScalar();

                    // Håndter null-resultater og konverter til int
                    int teller = resultat != null ? Convert.ToInt32(resultat) : 0;

                    // om teller er > 0, , bruker med samme kort ID eksisterer
                    return teller > 0;  
                }
            }
        }
    }
}