using Adgangskontroll.SENTRAL.InputHandlers;
using Adgangskontroll.SENTRAL.Models;
using Adgangskontroll.SENTRAL.Repository;

namespace Adgangskontroll.SENTRAL
{
    public class Program
    {
        static void Main(string[] args)
        {

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
                        // Opprett bruker instans og lagre til database
                        BrukerInput brukerInput = new BrukerInput();
                        Bruker nyBruker = brukerInput.GetBrukerInput();

                        try
                        {
                            BrukerRepository brukerRepository = new BrukerRepository();
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
                        Console.WriteLine("Ikke implementert enda");
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
