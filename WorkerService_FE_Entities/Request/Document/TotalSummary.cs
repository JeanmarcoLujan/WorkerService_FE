using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.Document
{
    public class TotalSummary
    {
        [XmlAttribute("SubTotal")]
        public Decimal SubTotal { get; set; }

        [XmlAttribute("Tax")]
        public Decimal Tax { get; set; }

        [XmlAttribute("Total")]
        public Decimal Total { get; set; }

        [XmlElement("PublicAdministration")]
        public PublicAdministration PublicAdministration { get; set; }
    }
}
