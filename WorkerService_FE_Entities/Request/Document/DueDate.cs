using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.Document
{
    public class DueDate
    {
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [XmlAttribute(AttributeName = "Amount")]
        public string Amount { get; set; }

        [XmlAttribute(AttributeName = "PaymentID")]
        public string PaymentID { get; set; }
    }
}
