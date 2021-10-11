using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferMarktScraper.Core
{
    public class DbConfig
    {
        public string Database_Name { get; set; }
        public string Connection_String { get; set; }
        public string Teams_Collection_Name { get; set; }
        public string Players_Collection_Name { get; set; }
        public string Market_Values_Collection_Name { get; set; }
        public string Performances_Collection_Name { get; set; }
    }
}
