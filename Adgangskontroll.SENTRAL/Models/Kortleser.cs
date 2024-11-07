using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adgangskontroll.SENTRAL.Models
{
    public class KortleserModel
    {
        public int KortleserID { get; set; } 
        public string KortleserNummer { get; set; }      // CHAR(4)
        public string KortleserPlassering { get; set; }  
    }
}
