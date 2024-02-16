using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService_FE_Entities.Response;
using WorkerService_FE_Entities.ServiceLayer.Document;

namespace WorkerService_FE_Response.Repository.Interfaces
{
    public interface IResponseRepository
    {
        public ResultReponse GetResult(NCFResponse nCFResponse, string token);
        public List<NCFResponse> GetDocumentSeguimientoSAP();
    }
}
