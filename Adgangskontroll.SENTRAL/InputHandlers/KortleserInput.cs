using Adgangskontroll.SENTRAL.Models;
using Adgangskontroll.SENTRAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adgangskontroll.SENTRAL.InputHandlers
{
    public class KortleserInput
    {
        private readonly KortleserRepository _kortleserRepository;

        public KortleserInput(KortleserRepository kortleserRepository)
        {
            _kortleserRepository = kortleserRepository;
        }

        public Kortleser HentKortleserInput()
        {
            Kortleser nyKortleser = new Kortleser();

            Console.WriteLine("Skriv inn kortleser nummer (4 siffer):");
            nyKortleser.KortleserNummer = ValiderKortleserNummer();

            Console.WriteLine("Skriv inn kortleser plassering (f.eks. A102, B502, B958, osv.):");
            nyKortleser.KortleserPlassering = ValiderKortleserPlassering();

            return nyKortleser;
        }

        private string ValiderKortleserNummer()
        {
            string kortleserNummer;
            bool isUnique = false;

            do
            {
                kortleserNummer = Console.ReadLine();

                // Sjekk om kortleser er nøyaktig 4 siffer
                if (kortleserNummer.Length != 4 || !ErAlleTegnSiffer(kortleserNummer))
                {
                    Console.WriteLine("KortleserNummer må være nøyaktig 4 siffer. Vennligst prøv igjen.");
                    continue;
                }

                // Sjekk om kortleser nummer allerede eksisterer
                isUnique = !_kortleserRepository.SjekkOmKortleserNummerEksisterer(kortleserNummer);
                if (!isUnique)
                {
                    Console.WriteLine("KortleserNummer finnes allerede. Vennligst skriv inn et unikt nummer.");
                }

            } while (kortleserNummer.Length != 4 || !ErAlleTegnSiffer(kortleserNummer) || !isUnique);

            return kortleserNummer;
        }

        private string ValiderKortleserPlassering()
        {
            string plassering;

            do
            {
                plassering = Console.ReadLine();

                // Sjekk om plassering er mindre enn 10 tegn 
                if (plassering.Length > 10)
                {
                    Console.WriteLine("Plassering er skrevet inn i feil format. Vennligst prøv igjen.");
                }

            } while (plassering.Length > 10);

            return plassering;
        }

        // Hjelpemetode for å sjekke om tegn er siffer
        private bool ErAlleTegnSiffer(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
