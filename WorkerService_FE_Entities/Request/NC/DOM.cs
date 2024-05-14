using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.NC
{
    public class DOM
    {
        [XmlAttribute("TipoIngreso")]
        public string TipoIngreso { get; set; }

        //[XmlAttribute("TipoPago")]
        //public string TipoPago { get; set; }

        [XmlAttribute("LinesPerPrintedPage")]
        public string LinesPerPrintedPage { get; set; }

        [XmlAttribute("CodigoModificacion")]
        public string CodigoModificacion { get; set; }

        [XmlAttribute("IndicadorNotaCredito")]
        public string IndicadorNotaCredito { get; set; }
    }
}
