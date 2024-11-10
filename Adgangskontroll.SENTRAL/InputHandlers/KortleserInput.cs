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

        public KortleserModel HentKortleserInput()
        {
            KortleserModel nyKortleser = new KortleserModel();

            Console.WriteLine("Skriv inn kortleser nummer (4 siffer):");
            nyKortleser.KortleserNummer = ValiderKortleserNummer();

            Console.WriteLine("Skriv inn kortleser plassering (f.eks. A102, B502, B958, osv.):");
            nyKortleser.KortleserPlassering = ValiderKortleserPlassering();

            return nyKortleser;
        }

        public KortleserModel HentOppdatertKortleserInput(int kortleserID)
        {
            KortleserModel kortleser = new KortleserModel();
            kortleser.KortleserID = kortleserID;

            // Valider input
            Console.WriteLine("Skriv inn nytt kortleser nummer (4 siffer), eller trykk Enter for å beholde eksisterende nummer:");
            kortleser.KortleserNummer = ValiderKortleserNummer();
           

            // Valider input
            Console.WriteLine("Skriv inn ny plassering (maks 10 tegn), eller trykk Enter for å beholde eksisterende plassering:");
            kortleser.KortleserPlassering = ValiderKortleserPlassering();


            return kortleser;
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

        private string HentKortleserInput(
            string promptMessage,
            string existingValue,
            Func<string, bool> validate,
            bool isRequired = false)
        {
            string input;
            do
            {
                Console.WriteLine(promptMessage);
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    return existingValue;
                }
                else if (validate(input))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Ugyldig input. Vennligst prøv igjen.");
                }
            } while (true);
        }
    }
}
