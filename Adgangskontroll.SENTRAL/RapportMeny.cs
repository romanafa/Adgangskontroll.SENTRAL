using Adgangskontroll.SENTRAL.Models;
using Adgangskontroll.SENTRAL.Repository;
using Adgangskontroll.SENTRAL.TCP_kortleser;

namespace Adgangskontroll.SENTRAL
{
    public class RapportMeny
    {
        private readonly BrukerRepository _brukerRepository;

        public RapportMeny(BrukerRepository brukerRepository)
        {
            _brukerRepository = brukerRepository;
        }

        public void StartRapportMeny()
        {
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Velg en rapport:");
                Console.WriteLine("1. Liste brukerdata på grunnlag av brukernavn");
                Console.WriteLine("2. Liste adgangslogg på grunnlag av brukernavn mellom to datoer");
                Console.WriteLine("3. Liste alle innpasseringsforsøk for en dør med ikke-godkjent adgang mellom to datoer");
                Console.WriteLine("4. Liste alle brukere med over 10 ikke godkjente adgangsforsøk");
                Console.WriteLine("5. Liste av alarmer mellom to datoer");
                Console.WriteLine("6. Tilbake til hovedmeny");

                int rapportValg;
                try
                {
                    rapportValg = Convert.ToInt16(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Tast inn et heltall for menyvalg.");
                    continue;
                }

                switch (rapportValg)
                {
                    case 1:
                        // Liste brukerdata på grunnlag av brukernavn
                        VisBrukerDataEtterEpost();
                        break;
                    case 2:
                        // Liste adgangslogg på grunnlag av brukernavn mellom to datoer
                        Console.WriteLine("Ikke implementert enda"); 
                        break;
                    case 3:
                        // Liste alle innpasseringsforsøk for en dør med ikke-godkjent adgang mellom to datoer 
                        Console.WriteLine("Ikke implementert enda");
                        break;
                    case 4:
                        // Liste alle brukere med over 10 ikke godkjente adgangsforsøk 
                        Console.WriteLine("Ikke implementert enda");
                        break;
                    case 5:
                        // Liste av alarmer mellom to datoer
                        Console.WriteLine("Ikke implementert enda");
                        break;
                    case 6:
                        Console.WriteLine("Tilbake til hovedmeny.");
                        loop = false;
                        break;
                    default:
                        Console.WriteLine("Ugyldig valg, prøv igjen.");
                        break;
                }
            }
        }

        private void VisBrukerDataEtterEpost()
        {
            Console.WriteLine("Skriv inn epost til brukeren du ønsker å søke etter:");
            string epost = Console.ReadLine();

            Bruker bruker = _brukerRepository.FinnBrukerEtterEpost(epost);

            if (bruker != null)
            {
                Console.WriteLine("");
                Console.WriteLine("Bruker funnet:");
                Console.WriteLine($"BrukerID: {bruker.BrukerID}");
                Console.WriteLine($"Fornavn: {bruker.Fornavn}");
                Console.WriteLine($"Etternavn: {bruker.Etternavn}");
                Console.WriteLine($"Epost: {bruker.Epost}");
                Console.WriteLine($"KortID: {bruker.KortID}");
                Console.WriteLine($"PIN: {bruker.PIN}");
                Console.WriteLine($"Gyldig Fra: {bruker.GyldigFra}");
                Console.WriteLine($"Gyldig Til: {bruker.GyldigTil}");
            }
            else
            {
                Console.WriteLine("Ingen bruker funnet med denne e-posten.");
            }
            Console.WriteLine("");
        }

        private void
    }
}
