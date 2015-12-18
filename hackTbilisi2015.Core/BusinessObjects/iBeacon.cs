using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackTbilisi2015.Core.BusinessObjects
{
    public class iBeacon
    {
        public string UUID { get; set; }
        public ushort Major { get; set; }
        public ushort Minor { get; set; }
        public DateTime EventDate { get; set; }
        public string Name { get; set; }
        public string MacAddress { get; set; }
    }
}
