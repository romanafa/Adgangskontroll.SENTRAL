﻿using Adgangskontroll.SENTRAL.Data;
using Adgangskontroll.SENTRAL.Models;
using Npgsql;
using System.Data;

namespace Adgangskontroll.SENTRAL.Repository
{
    public class BrukerRepository
    {
        private readonly DbConnection _db;
        private static DataTable dtgetData = new DataTable(); // DataTable for å lagre data fra database

        public BrukerRepository()
        {
            _db = new DbConnection();
        }

        public static DataTable DtgetData
        {
            get { return dtgetData; }
            set { dtgetData = value; }
        }

        // Metode for å sende og motta informasjon mellom sentral og database
        public DataTable getData(string sql)
        {
            DataTable dt = new DataTable();
            using (var cmd = new NpgsqlCommand())
            {
                NpgsqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
            }
            return dt;
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

        public Bruker FinnBrukerEtterEpost(string epost)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM Bruker WHERE Epost = @Epost";
                    cmd.Parameters.AddWithValue("Epost", epost);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Map database columns to the Bruker object properties
                            return new Bruker
                            {
                                BrukerID = reader.GetInt32(reader.GetOrdinal("BrukerID")),
                                Fornavn = reader.GetString(reader.GetOrdinal("Fornavn")),
                                Etternavn = reader.GetString(reader.GetOrdinal("Etternavn")),
                                Epost = reader.GetString(reader.GetOrdinal("Epost")),
                                KortID = reader.GetString(reader.GetOrdinal("KortID")),
                                PIN = reader.GetString(reader.GetOrdinal("PIN")),
                                GyldigFra = reader.GetDateTime(reader.GetOrdinal("GyldigFra")),
                                GyldigTil = reader.GetDateTime(reader.GetOrdinal("GyldigTil"))
                            };
                        }
                    }
                }
            }

            // Return null if no user is found
            return null;
        }


        // Autentisering tar inn kort_id, pin og kortleser_id.
        // Sjekker om pin matcher kort_id, om kort_id er innenfor gyldighetsperioden
        // og om kort_id har tilgang til kortleser_id.
        // Hvis prossesen feiler returnerer den blankt, om den er er ok returnerer den
        // en rad med bruker og kortleser (den informasjonen som ble sendt inn) spleiset sammen.
        public string Autentisering(string kort_id, string pin, string kortleser_id)
        {
            string suksess;
            string query = ($"select * from tilgangrelasjon join bruker on tilgangrelasjon.tilgang_id = bruker.tilgang_id join kortleser on tilgangrelasjon.seksjon_id = kortleser.seksjon_id where kort_id = '{kort_id}' and pin = '{pin}' and kortleser_id = '{kortleser_id}' and CURRENT_DATE between gyldighet_start and gyldighet_slutt;");
            using (var dbTilkobling = _db.GetConnection())
            {
                using var cmd = new NpgsqlCommand(query, dbTilkobling);

                using NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())  // Kommando for å "lese" om vi får returnert en tabell fra spørringen vi sender
                {
                    suksess = "Godkjent";
                }
                else  // Dersom vi ikke får returnert en tabell fra spørringen
                {
                    suksess = "Ikke godkjent";
                }
            }
            return suksess; // Blir sendt som dataTilKortleser
        }


        // Loggfører og legger inn en rad i loggtabellen
        public DataTable LeggTilLogg(int logg_type, string kortleser_id, string kort_id)
        {

            dtgetData = getData($"insert into logg values ({logg_type}, CURRENT_TIMESTAMP, '{kortleser_id}', '{kort_id}');");
            DataTable dt = dtgetData;

            return dt;
        }

        // Returnerer en tabell med alle rader i kortlesertabellen
        public DataTable VisKortlesere()
        {
            dtgetData = getData("select * from kortleser");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }

        // Returnerer en tabell med alle rader i kortlesertabellen for en valgt seksjon
        public DataTable VisKortleserVedSeksjon(int seksjon_id)
        {
            dtgetData = getData($"select * from kortleser where seksjon_id = {seksjon_id}");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }

        // Returener en tabell med alle rader i brukertabellen
        public DataTable VisBrukere()
        {
            dtgetData = getData("select * from bruker");
            DataTable dt = BrukerRepository.dtgetData;

            return dt;
        }

        // Lister adgangslogg (inkludert forsøk på adgang) utifra kort_id mellom to datoer
        public DataTable VisAdgangsloggForBrukerVedDato(string kort_id, string start, string slutt)
        {
            dtgetData = getData($"select * from logg where kort_id = '{kort_id}' and (logg_type = 0 or logg_type = 1 or logg_type = 2) and logg_tid between '{start}' and '{slutt}'");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }

        // Returnerer alle innpasseringsforsøk for en dør med ikke-godkjent adgang (uansett bruker) mellom to datoer
        public DataTable VisNegativAdgangsloggKortleserVedDato(string kortleser_id, string start, string slutt)
        {
            dtgetData = getData($"select * from logg where kortleser_id = '{kortleser_id}' and logg_type = 1 and logg_tid between '{start}' and '{slutt}'");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }

        // Returnerer alarmer
        public DataTable VisAlarm()
        {
            dtgetData = getData($"select * from logg where (logg_type = 3 or logg_type = 4)");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }

        // Returnerer alarmer ved kort_id
        public DataTable VisAlarmVedBruker(string kort_id)
        {
            dtgetData = getData($"select * from logg where (logg_type = 3 or logg_type = 4) and kort_id = '{kort_id}'");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }

        // Returnerer alarmer ved kortleser_id
        public DataTable VisAlarmVedKortleser(string kortleser_id)
        {
            dtgetData = getData($"select * from logg where (logg_type = 3 or logg_type = 4) and kortleser_id = '{kortleser_id}'");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }

        // Returnerer alarmer mellom to datoer
        public DataTable VisAlarmVedDato(string start, string slutt)
        {
            dtgetData = getData($"select * from logg where (logg_type = 3 or logg_type = 4) and logg_tid between '{start}' and '{slutt}'");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }

        // returnerer alle entries i loggtabellen
        public DataTable VisLogg()
        {
            dtgetData = getData($"select * from logg");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }

        // Returnerer alle entries fra loggtabellen basert på kort_id
        public DataTable VisLoggVedBruker(string kort_id)
        {
            dtgetData = getData($"select * from logg where kort_id = '{kort_id}'");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }

        // Returnerer alle entries fra loggtabellen basert på kortleser_id
        public DataTable VisLoggVedKortleser(string kortleser_id)
        {
            dtgetData = getData($"select * from logg where kortleser_id = '{kortleser_id}'");
            DataTable dt = BrukerRepository.DtgetData;

            return dt;
        }
    }
}