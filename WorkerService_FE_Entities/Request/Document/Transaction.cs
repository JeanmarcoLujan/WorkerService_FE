using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.Document
{

    [XmlRoot("Transaction")]
    public class Transaction
    {
        [XmlElement("GeneralData")]
        public GeneralData GeneralData { get; set; }

        [XmlElement("Supplier")]
        public Supplier Supplier { get; set; }

        [XmlElement("Client")]
        public Client Client { get; set; }

        [XmlElement("ProductList")]
        public ProductList ProductList { get; set; }

        [XmlElement("TaxSummary")]
        public TaxSummary TaxSummary { get; set; }

        [XmlElement("DueDates")]
        public DueDates DueDates { get; set; }

        [XmlElement("TotalSummary")]
        public TotalSummary TotalSummary { get; set; }
    }
}
