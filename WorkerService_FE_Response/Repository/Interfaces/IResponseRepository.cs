using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService_FE_Entities.Response;

namespace WorkerService_FE_Response.Repository.Interfaces
{
    public interface IResponseRepository
    {
        public ResultReponse GetResult();
    }
}
