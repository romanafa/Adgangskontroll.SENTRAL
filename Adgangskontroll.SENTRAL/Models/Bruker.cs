using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adgangskontroll.SENTRAL.Models
{
    public class Bruker
    {
        public int BrukerID { get; set; }
        public string Etternavn { get; set; }
        public string Fornavn { get; set; }
        public string Epost { get; set; }
        public string KortID { get; set; }  // CHAR(4)
        public string PIN { get; set; }     // CHAR(4)
        public DateTime GyldigFra { get; set; } = DateTime.Now;
        public DateTime GyldigTil { get; set; } = DateTime.MaxValue;  // Default lang fra i dag

        
        // Sjekk om input er gyldig
        public bool IsValid()
        {
            bool valid = true;

            if (Etternavn == null || Etternavn.Length > 50)
            {
                Console.WriteLine("Feil: Etternavn kan ikke være tomt eller lengre enn 50 tegn.");
                valid = false;
            }
            if (Fornavn == null || Fornavn.Length > 50)
            {
                Console.WriteLine("Feil: Fornavn kan ikke være tomt eller lengre enn 50 tegn.");
                valid = false;
            }
            if (Epost == null || Epost.Length > 150)
            {
                Console.WriteLine("Feil: Epost kan ikke være tomt eller lengre enn 150 tegn.");
                valid = false;
            }
            if (KortID == null || KortID.Length != 4)
            {
                Console.WriteLine("Feil: KortID må være nøyaktig 4 siffer.");
                valid = false;
            }
            if (PIN == null || PIN.Length != 4)
            {
                Console.WriteLine("Feil: PIN må være nøyaktig 4 siffer.");
                valid = false;
            }

            return valid;
        }
    }
}
