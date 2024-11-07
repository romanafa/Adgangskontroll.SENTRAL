using Adgangskontroll.SENTRAL.Models;

namespace Adgangskontroll.SENTRAL.InputHandlers
{
    public class BrukerInput
    {
        public Bruker HentOpprettetBrukerInput()
        {
            Bruker nyBruker = new Bruker();

            nyBruker.Fornavn = HentBrukerInput("Skriv inn fornavn:", null, input => ErGyldigStringInput(input, 50), true);
            nyBruker.Etternavn = HentBrukerInput("Skriv inn etternavn:", null, input => ErGyldigStringInput(input, 50), true);
            nyBruker.Epost = HentBrukerInput("Skriv inn e-post:", null, input => ErGyldigEpostInput(input, 150), true);
            nyBruker.KortID = HentBrukerInput("Skriv inn kort ID (4 siffer):", null, input => ErGyldigSifferInput(input, 4), true);
            nyBruker.PIN = HentBrukerInput("Skriv inn PIN (4 siffer):", null, input => ErGyldigSifferInput(input, 4), true);

            return nyBruker;
        }

        public Bruker HentOppdatertBrukerInput(int brukerID)
        {
            Bruker bruker = new Bruker();
            bruker.BrukerID = brukerID;

            bruker.Fornavn = HentBrukerInput("Skriv inn nytt fornavn, eller trykk Enter for å beholde eksisterende fornavn:", bruker.Fornavn, input => ErGyldigStringInput(input, 50), false);
            bruker.Etternavn = HentBrukerInput("Skriv inn nytt etternavn, eller trykk Enter for å beholde eksisterende etternavn:", bruker.Etternavn, input => ErGyldigStringInput(input, 50), false);
            bruker.Epost = HentBrukerInput("Skriv inn ny e-post, eller trykk Enter for å beholde eksisterende e-post:", bruker.Epost, input => ErGyldigEpostInput(input, 150), false);
            bruker.KortID = HentBrukerInput("Skriv inn ny kort ID (4 siffer), eller trykk Enter for å beholde eksisterende kort ID:", bruker.KortID, input => ErGyldigSifferInput(input, 4), false);
            bruker.PIN = HentBrukerInput("Skriv inn ny PIN (4 siffer), eller trykk Enter for å beholde eksisterende PIN:", bruker.PIN, input => ErGyldigSifferInput(input, 4), false);

            return bruker;
        }

        // Metode for å sjekke om epost er gyldig med '@' og '.'
        private bool ErGyldigEpost(string epost)
        {
            // Sjekk at epost inneholder kun ett '@'
            int alfaKroell = epost.Split('@').Length - 1;
            if (alfaKroell != 1) return false;

            // Sjekk at epost har tekst foran og etter '@'
            string[] parts = epost.Split('@');
            if (parts.Length != 2 || parts[0].Length == 0 || parts[1].Length == 0) return false;

            // Sjekk at eposten inneholder minst et punktum '.'
            if (!parts[1].Contains(".") || parts[1].EndsWith(".")) return false;

            return true;
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

        // Sjekk om input for brukerID er gyldig
        public int ErGyldigIntInput()
        {
            int resultat;
            while (!int.TryParse(Console.ReadLine(), out resultat))
            {
                Console.WriteLine("Ugyldig input. Vennligst skriv inn et gyldig tall.");
            }
            return resultat;
        }

        private string HentBrukerInput(string prompt, string lagretVerdi, Func<string, bool> validerInput, bool erIkkeNull)
        {
            string input;

            do
            {
                Console.WriteLine(prompt);
                // Dersom man oppdaterer en eksisterende bruker, så kan man trykke enter for å beholde verdi fra før
                // erIkkeNull == false  -->  kan være null input, gjelder kun brukeroppdatering
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) && !erIkkeNull)
                {
                    // Returner lagret verdi fra databasen hvis brukeren trykker på Enter uten å skrive inn ny verdi
                    return lagretVerdi;
                }
                else if (validerInput(input))
                {
                    // Returner validert input
                    return input;
                }
                Console.WriteLine("Ugyldig input. Vennligst prøv igjen");
            } while (erIkkeNull || !validerInput(input));

            return input;
        }

        private bool ErGyldigStringInput(string input, int maksLengde)
        {
            return input.Length <= maksLengde;
        }

        private bool ErGyldigEpostInput(string input, int maksLengde)
        {
            return input.Length <= maksLengde && ErGyldigEpost(input);
        }

        private bool ErGyldigSifferInput(string input, int lengde)
        {
            return input.Length == lengde && ErAlleTegnSiffer(input);
        }
    }
}
