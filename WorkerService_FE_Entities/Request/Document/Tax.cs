using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.Document
{
    public class Tax
    {
        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlAttribute("Rate")]
        public Decimal Rate { get; set; }

        [XmlAttribute("Base")]
        public Decimal Base { get; set; }

        [XmlAttribute("Amount")]
        public Decimal Amount { get; set; }
    }
}
