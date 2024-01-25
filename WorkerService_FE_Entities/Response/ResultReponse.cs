using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_FE_Entities.Response
{
    public class ResultReponse
    {
        public DGII DGII { get; set; }
    }

    public class DGII
    {
        public string ReferenciaFactura { get; set; }
        public string ReferenciaNCF { get; set; }
        public string EstatusDGII { get; set; }
        public string MensajeDGII { get; set; }
        public string PDF { get; set; }
        public string QR { get; set; }
        public DateTime FechaFirma { get; set; }
        public string CodigoSeguridad { get; set; }
    }
}
