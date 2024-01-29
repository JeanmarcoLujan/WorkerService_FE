using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.Document
{
    public class DOM
    {
        [XmlAttribute("TipoIngreso")]
        public string TipoIngreso { get; set; }

        [XmlAttribute("TipoPago")]
        public string TipoPago { get; set; }

        [XmlAttribute("LinesPerPrintedPage")]
        public int LinesPerPrintedPage { get; set; }

        [XmlAttribute("IndicadorAgenteRetencionoPercepcion")]
        public string IndicadorAgenteRetencionoPercepcion { get; set; }

        [XmlAttribute("MontoITBISRetenido")]
        public string MontoITBISRetenido { get; set; }

        [XmlAttribute("MontoISRRetenido")]
        public string MontoISRRetenido { get; set; }
    }
}
