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
            KortleserRepository kortleserRepository = new KortleserRepository();
            KortleserInput kortleserInput = new KortleserInput(kortleserRepository);
            BrukerInput brukerInput = new BrukerInput();
            RapportMeny rapportMeny = new RapportMeny(brukerRepository);

            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Velg en av instillingne i menyen.");
                Console.WriteLine("1. Legg til bruker");
                Console.WriteLine("2. Rediger bruker");
                Console.WriteLine("3. Slett bruker");
                Console.WriteLine("4. Kortleser administrasjon");
                Console.WriteLine("5. Oppdater kortleser");
                Console.WriteLine("6. Slett kortleser");
                Console.WriteLine("7. Se rapport meny");
                Console.WriteLine("8. Avslutt programmet");
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
                        while (true)
                        {
                            // Slett bruker
                            Console.WriteLine("Skriv inn BrukerID til brukeren du ønsker å slette:");
                            // Sjekk om input er gyldig
                            int brukerIDslett = brukerInput.ErGyldigIntInput();
                            // Sjekk om bruker med oppgitt bruker ID finnes i databasen
                            Bruker eksisterendeBruker = brukerRepository.FinnBrukerEtterID(brukerIDslett);

                            if (eksisterendeBruker == null)
                            {
                                // Dersom brukeren ikke finnes, spør om ID igjen
                                Console.WriteLine($"Ingen bruker funnet med BrukerID {brukerIDslett}. Vennligst prøv igjen.");
                            }
                            else
                            {
                                // Slett brukerdata fra databasen
                                brukerRepository.SlettBruker(brukerIDslett);
                                Console.WriteLine("Brukerdata ble slettet.");
                                break;
                            }
                        }
                        break;

                    case 4:
                        // Kortleser administrasjon - ny kortleser
                        Kortleser nyKortleser = kortleserInput.HentKortleserInput();
                        try
                        {
                            kortleserRepository.OpprettKortleser(nyKortleser);
                            Console.WriteLine("Kortleser ble lagt til i databasen.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"En feil oppstod: {ex.Message}");
                        }
                        break;

                    case 5:
                        // Oppdater kortleser
                        Console.WriteLine("Ikke implementert enda");
                        break;

                    case 6:
                        // Slett kortleser
                        while (true)
                        {
                            Console.WriteLine("Skriv inn ID på kortleser som skal slettes:");
                            int kortleserId = brukerInput.ErGyldigIntInput();  // Valider om bruker skrevet inn integer

                            // Finn kortleser etter ID
                            Kortleser kortleser = kortleserRepository.FinnKortleserEtterId(kortleserId);
                            if (kortleser == null)
                            {
                                // Dersom kortleseren ikke finnes, spør om ID igjen
                                Console.WriteLine($"Ingen kortleser funnet med KortleserID {kortleserId}. Vennligst prøv igjen.");
                            }
                            else
                            {
                                // Slett kortleser fra databasen
                                kortleserRepository.SlettKortleser(kortleserId);
                                Console.WriteLine("Kortleser ble slettet.");
                                break;
                            }
                        }

                        break;
                    case 7:
                        // Rapportmeny 
                        rapportMeny.StartRapportMeny();
                        break;

                    case 8:
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
                    Console.WriteLine("Vil du gå tilbake til hovedmenyen? Tast 'ja' eller noe annet for å avslutte programmet");
                    string brukerValg = Console.ReadLine();
                    if (brukerValg.ToLower() != "ja")
                    {
                        loop = false;
                    }
                }
            }
            Console.WriteLine("Programmet er avsluttet.");
        }
    }
}
