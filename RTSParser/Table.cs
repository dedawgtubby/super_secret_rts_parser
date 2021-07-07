using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSParser
{
    class Table
    {
        public List<TableEntry> AllTableEntries {get;set;}
        public struct TableEntry
        {
            int rowCount { get; set; }
             
        }



        public Table()
        {
            
        }

        
    }
}
