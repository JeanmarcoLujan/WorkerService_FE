using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.Document
{
    public class DueDates
    {
        [XmlElement(ElementName = "DueDate")]
        public DueDate DueDate { get; set; }
    }
}
