using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_FE_Request.Repository.Interfaces
{
    public interface IRequestNDRepository
    {
        public void SendDocuementND(string token);
        public int GetDocumentSAPND();
    }
}
