using Adgangskontroll.SENTRAL.InputHandlers;
using Adgangskontroll.SENTRAL.Models;
using Adgangskontroll.SENTRAL.Repository;
using Adgangskontroll.SENTRAL.TCP_kortleser;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adgangskontroll.SENTRAL
{
    public class Program
    {
        static void Main(string[] args)
        {
            BrukerRepository brukerRepository = new BrukerRepository();
            BrukerInput brukerInput = new BrukerInput();
            RapportMeny rapportMeny = new RapportMeny(brukerRepository);

            // Oppretter en instans av TCP-klassen
            TCP tcpClient = new TCP();

            // Starter TCP tilkoblingen
            tcpClient.Start();

            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Velg en av instillingne i menyen.");
                Console.WriteLine("1. Legg til bruker");
                Console.WriteLine("2. Rediger bruker");
                Console.WriteLine("3. Slett bruker");
                Console.WriteLine("4. Tester oppkobling til serveren."); // Kortleser administrasjon
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
                        // Kortleser administrasjon //tester tilkoblingen til kortleser
                        Console.WriteLine("Tester oppkobling til kortleser...");
                        try
                        {
                            // Adresse og port for serveren
                            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

                            // Opprett en TCP-klient sokkel
                            Socket klientSokkel = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                            // Koble til serveren
                            klientSokkel.Connect(serverEP);
                            Console.WriteLine("Koblet til kortleseren.");

                            // Send en testmelding til serveren
                            string melding = "Testmelding";
                            byte[] dataSomBytes = Encoding.ASCII.GetBytes(melding);
                            klientSokkel.Send(dataSomBytes);

                            // Motta svar fra serveren
                            byte[] mottaksBuffer = new byte[1024];
                            int bytesMottatt = klientSokkel.Receive(mottaksBuffer);
                            string svar = Encoding.ASCII.GetString(mottaksBuffer, 0, bytesMottatt);
                            Console.WriteLine("Svar fra serveren: " + svar);

                            // Lukk tilkoblingen
                            klientSokkel.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Feil: " + ex.Message);
                        }

                        Console.WriteLine("Trykk på en tast for å avslutte...");
                        Console.ReadKey();
                        break;

                    case 5:
                        // Kortleser forespørsel
                        Console.WriteLine("Ikke implementert enda");
                        break;

                    case 6:
                        // Rapportmeny 
                        rapportMeny.StartRapportMeny();
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
