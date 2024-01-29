using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.Document
{
    public class Client
    {
        [XmlAttribute("CIF")]
        public string CIF { get; set; }

        [XmlAttribute("Company")]
        public string Company { get; set; }

        [XmlAttribute("Email")]
        public string Email { get; set; }

        [XmlAttribute("Address")]
        public string Address { get; set; }

        [XmlAttribute("City")]
        public string City { get; set; }

        [XmlAttribute("PC")]
        public string PC { get; set; }

        [XmlAttribute("Province")]
        public string Province { get; set; }

        [XmlAttribute("Country")]
        public string Country { get; set; }
    }
}
