﻿using System;
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
    }
}
