using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.Document
{
    public class PublicAdministration
    {
        [XmlElement("DOM")]
        public DOM DOM { get; set; }
    }
}
