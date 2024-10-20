using Adgangskontroll.SENTRAL.Models;
using Adgangskontroll.SENTRAL.Repository;

namespace Adgangskontroll.SENTRAL
{
    public class Program
    {
        static void Main(string[] args)
        {
            BrukerRepository brukerRep = new BrukerRepository();

            // Opprett ny bruker objekt
            Bruker nyBruker = new Bruker
            {
                BrukerID = 2,
                Etternavn = "Hansen",
                Fornavn = "Ola",
                Epost = "ola@example.com",
                KortID = "2345",  // CHAR(4)
                PIN = "5678"      // CHAR(4)
                // GyldigFra og GyldigTil bruker default verdier som er satt i Bruker modell
            };

            // Sjekk om all input er gyldig 
            if (nyBruker.IsValid())
            {
                // Lagre ny bruker til databasen
                brukerRep.OpprettBruker(nyBruker);
                Console.WriteLine("Bruker ble lagt til i databasen.");
            }
            else
            {
                Console.WriteLine("Brukerdata er ikke gyldig.");
            }
        }
    }
}
