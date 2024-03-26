using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_FE_Entities.Request
{
    public class ReportRequest
    {
        public string DocEntry { get; set; }
        public string ObjectId { get; set; }
        public string UrlQr { get; set; }
        public string Report { get; set; }
        public string Ruta { get; set; }
    }

    public class ReportReponse
    {
        public bool ValorRecibido { get; set; }
    }
}
