using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.Document
{
    public class GeneralData
    {
        [XmlAttribute("Ref")]
        public string Ref { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlAttribute("Date")]
        public string Date { get; set; }

        [XmlAttribute("Currency")]
        public string Currency { get; set; }

        [XmlAttribute("ExchangeRate")]
        public Decimal ExchangeRate { get; set; }

        [XmlAttribute("TaxIncluded")]
        public bool TaxIncluded { get; set; }

        [XmlAttribute("NCF")]
        public string NCF { get; set; }

        [XmlAttribute("NCFExpirationDate")]
        public string NCFExpirationDate { get; set; }

        public PublicAdministration PublicAdministration { get; set; }
    }
}
