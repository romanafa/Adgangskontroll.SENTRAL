using Adgangskontroll.SENTRAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adgangskontroll.SENTRAL.InputHandlers
{
    public class BrukerInput
    {
        public Bruker GetBrukerInput()
        {
            Bruker nyBruker = new Bruker();

            // Fornavn input med maks. 50 tegn
            Console.WriteLine("Skriv inn Fornavn:");
            nyBruker.Fornavn = GetValidStringInput(50, "Fornavn");

            // Etternavn input med maks. 50 tegn
            Console.WriteLine("Skriv inn Etternavn:");
            nyBruker.Etternavn = GetValidStringInput(50, "Etternavn");

            // Epost input med validering
            Console.WriteLine("Skriv inn Epost:");
            nyBruker.Epost = GetValidEmailInput(150);

            // KortID input med 4 siffer
            Console.WriteLine("Skriv inn KortID (4 siffer):");
            nyBruker.KortID = GetValidDigitInput(4, "KortID");

            // PIN input med 4 siffer
            Console.WriteLine("Skriv inn PIN (4 siffer:");
            nyBruker.PIN = GetValidDigitInput(4, "PIN");

            return nyBruker;
        }

        // String validering
        private string GetValidStringInput(int maksLengde, string stringVerdi)
        {
            string input;
            do
            {
                input = Console.ReadLine();
                if (input.Length > maksLengde)
                {
                    Console.WriteLine($"{stringVerdi} kan ikke være lengre enn {maksLengde} tegn. Vennligst prøv igjen.");
                }
            } while (input.Length > maksLengde);

            return input;
        }

        // Validering for epost
        private string GetValidEmailInput(int maksLengde)
        {
            string input;
            do
            {
                input = Console.ReadLine();

                // Sjekk om epost ikke er lengre enn 150 tegn
                if (input.Length > maksLengde)
                {
                    Console.WriteLine($"Epost kan ikke være lengre enn {maksLengde} tegn. Vennligst prøv igjen.");
                }
                // Sjekk om epost er gyldig
                else if (!IsValidEmail(input))
                {
                    Console.WriteLine("Ugyldig epost. Vennligst skriv inn en gyldig epostadresse (må inneholde nøyaktig én '@' og et domenenavn).");
                }
            } while (input.Length > maksLengde || !IsValidEmail(input));

            return input;
        }

        // Metode for å sjekke om epost er gyldig med '@' og '.'
        private bool IsValidEmail(string epost)
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

        // Metode for å validere input for KortID og PIN
        private string GetValidDigitInput(int lengde, string stringVerdi)
        {
            string input;
            do
            {
                input = Console.ReadLine();

                // Sjekk om input inneholder kun 4 siffer
                if (input.Length != lengde)
                {
                    Console.WriteLine($"{stringVerdi} må være nøyaktig {lengde} siffer. Vennligst prøv igjen.");
                }
                else if (!IsAllDigits(input))
                {
                    Console.WriteLine($"{stringVerdi} må bare inneholde siffer. Vennligst prøv igjen.");
                }
            } while (input.Length != lengde || !IsAllDigits(input));

            return input;
        }

        // Hjelpemetode for å sjekke om tegn er siffer
        private bool IsAllDigits(string input)
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
