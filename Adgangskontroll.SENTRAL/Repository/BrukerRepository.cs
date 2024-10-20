﻿using Adgangskontroll.SENTRAL.Data;
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
            using (var connection = _db.GetConnection())
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO Bruker (BrukerID, Etternavn, Fornavn, Epost, KortID, PIN) " +
                                      "VALUES (@BrukerID, @Etternavn, @Fornavn, @Epost, @KortID, @PIN)";

                    // Map parametere for å forhindre SQL-injeksjon
                    cmd.Parameters.AddWithValue("BrukerID", bruker.BrukerID);
                    cmd.Parameters.AddWithValue("Etternavn", bruker.Etternavn);
                    cmd.Parameters.AddWithValue("Fornavn", bruker.Fornavn);
                    cmd.Parameters.AddWithValue("Epost", bruker.Epost);
                    cmd.Parameters.AddWithValue("KortID", bruker.KortID);
                    cmd.Parameters.AddWithValue("PIN", bruker.PIN);

                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Ny bruker opprettet.");
        }
    }
}
