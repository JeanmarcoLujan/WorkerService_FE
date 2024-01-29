using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.Document
{
    public class Product
    {
        [XmlAttribute("SupplierSKU")]
        public string SupplierSKU { get; set; }

        [XmlAttribute("Item")]
        public string Item { get; set; }

        [XmlAttribute("Qty")]
        public Decimal Qty { get; set; }

        [XmlAttribute("MU")]
        public string MU { get; set; }

        [XmlAttribute("CU")]
        public int CU { get; set; }

        [XmlAttribute("UP")]
        public Decimal UP { get; set; }

        [XmlAttribute("Total")]
        public Decimal Total { get; set; }

        [XmlAttribute("NetAmount")]
        public Decimal NetAmount { get; set; }

        [XmlAttribute("SysLineType")]
        public string SysLineType { get; set; }

        [XmlElement("Taxes")]
        public Taxes Taxes { get; set; }

        [XmlElement("PublicAdministration")]
        public PublicAdministration PublicAdministration { get; set; }
    }
}
