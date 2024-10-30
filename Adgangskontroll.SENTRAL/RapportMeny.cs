using Adgangskontroll.SENTRAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        GenererBrukerData();
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

        private void GenererBrukerData()
        {
            
        }
    }
}
