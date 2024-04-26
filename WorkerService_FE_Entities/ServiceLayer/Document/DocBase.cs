using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_FE_Entities.ServiceLayer.Document
{
    public class DocBase
    {
        public string U_MGS_FE_Estado { get; set; }
        public string U_MGS_FE_RespEnvio {  get; set; }
        public string U_MGS_FE_EstatusDGII {  get; set; }
        public string U_MGS_FE_MensajeDGII {  get; set; }
        public string U_MGS_FE_PDF {  get; set; }
        public string U_MGS_FE_QR {  get; set; }
        public string U_MGS_FE_FechaFirma {  get; set; }
        public string U_MGS_FE_CodigoSeguridad {  get; set; }
        public string U_MGS_FE_PDF2 {  get; set; }

        public DocBase()
        {
            U_MGS_FE_EstatusDGII = "";
            U_MGS_FE_MensajeDGII = "";
            U_MGS_FE_PDF = "";
            U_MGS_FE_QR = "";
            U_MGS_FE_FechaFirma = "";
            U_MGS_FE_CodigoSeguridad = "";
            U_MGS_FE_PDF2 = "";
        }
    }
}
