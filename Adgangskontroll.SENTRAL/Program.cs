using Adgangskontroll.SENTRAL.InputHandlers;
using Adgangskontroll.SENTRAL.Models;
using Adgangskontroll.SENTRAL.Repository;

namespace Adgangskontroll.SENTRAL
{
    public class Program
    {
        static void Main(string[] args)
        {
            BrukerRepository brukerRepository = new BrukerRepository();
            BrukerInput brukerInput = new BrukerInput();

            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Velg en av instillingne i menyen.");
                Console.WriteLine("1. Legg til bruker");
                Console.WriteLine("2. Rediger bruker");
                Console.WriteLine("3. Slett bruker");
                Console.WriteLine("4. Kortleser administrasjon");
                Console.WriteLine("5. Kortleser forespørsel");
                Console.WriteLine("6. Se rapport meny");
                Console.WriteLine("7. Avslutt programmet");
                int menyValg;
                try
                {
                    menyValg = Convert.ToInt16(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Tast inn et heltall for menyvalg");
                    continue; // Ugyldig input, returner til starten
                }
                switch (menyValg)
                {
                    case 1:
                        Bruker nyBruker = brukerInput.HentOpprettetBrukerInput();

                        // Sjekk om epost finnes i databasen
                        if (brukerRepository.SjekkOmEpostEksisterer(nyBruker.Epost))
                        {
                            Console.WriteLine("En bruker med denne e-posten finnes allerede. Vennligst prøv på nytt med en annen e-post.");
                            break;  
                        }

                        // Sjekk om kort ID finnes i databasen
                        if (brukerRepository.SjekkOmKortIDEksisterer(nyBruker.KortID))
                        {
                            Console.WriteLine("En bruker med denne kort ID finnes allerede. Vennligst prøv på nytt med en annen kort ID.");
                            break;  
                        }

                        try
                        {
                            brukerRepository.OpprettBruker(nyBruker);
                            Console.WriteLine("Bruker ble lagt til i databasen.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                        }
                            
                        break;
                    case 2:
                        // Rediger bruker
                        while (true)
                        {
                            // Spør om bruker ID og valider om den er gyldig, og sjekk om bruker eksisterer
                            Console.WriteLine("Skriv inn BrukerID på brukeren som skal oppdateres:");

                            // Sjekk om input er gyldig
                            int brukerID = brukerInput.ErGyldigIntInput();

                            // Sjekk om bruker med oppgitt bruker ID finnes i databasen
                            Bruker eksisterendeBruker = brukerRepository.FinnBrukerEtterID(brukerID);

                            if (eksisterendeBruker == null)
                            {
                                // Dersom brukeren ikke finnes, spør om ID igjen
                                Console.WriteLine($"Ingen bruker funnet med BrukerID {brukerID}. Vennligst prøv igjen.");
                            }
                            else
                            {
                                Bruker oppdatertBruker = brukerInput.HentOppdatertBrukerInput(brukerID);

                                // Oppdater brukerdata i databasen
                                brukerRepository.OppdaterBruker(oppdatertBruker);
                                Console.WriteLine("Brukerdata ble oppdatert i databasen.");
                                break; 
                            }
                        }
                    break;

                    case 3:
                        // Slett bruker
                        Console.WriteLine("Ikke implementert enda");

                        break;

                    case 4:
                        // Kortleser administrasjon
                        Console.WriteLine("Ikke implementert enda");
                        break;

                    case 5:
                        // Kortleser forespørsel
                        Console.WriteLine("Ikke implementert enda");
                        break;

                    case 6:
                        // Rapportmeny 
                        Console.WriteLine("Ikke implementert enda");
                        break;

                    case 7:
                        // Avslutt programmet
                        Console.WriteLine("Avslutter programmet...");
                        Environment.Exit(0);
                        break;

                    default:
                        // Dersom bruker velger ugyldig tall
                        Console.WriteLine("Ugyldig valg, prøv igjen.");
                        break;
                }

                // Spør brukeren om hen vil tilbake til oppstartsmeny
                if (loop)
                {
                    Console.WriteLine("Vil du gå tilbake til hovedmenyen? Tast 'ja' eller 'nei'");
                    string brukerValg = Console.ReadLine();
                    if (brukerValg.ToLower() == "nei")
                    {
                        loop = false;
                    }
                }
            }
            Console.WriteLine("Programmet er avsluttet.");
        }
    }
}
