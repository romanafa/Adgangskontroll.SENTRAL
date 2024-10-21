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
                Console.WriteLine("3. Kortleser");
                Console.WriteLine("4. Avslutt programmet");
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
                        // Kortleser program - mer kommer
                        Console.WriteLine("Ikke implementert enda");

                        break;

                    case 4:
                        // Avslutt programmet
                        Console.WriteLine("Avslutter programmet...");
                        loop = false;
                        break;

                    default:
                        // Dersom bruker velger ugyldig tall
                        Console.WriteLine("Ugyldig valg, prøv igjen.");
                        break;
                }

                // Ask if the user wants to return to the main menu or exit
                if (loop) // Only ask if loop hasn't been set to false
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
