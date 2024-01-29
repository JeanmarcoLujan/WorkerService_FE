using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_FE_Request.Repository.Interfaces
{
    public interface IRequestRepository
    {
        public void SendDocuement();
        public void GetDocumentSAP();
    }
}
