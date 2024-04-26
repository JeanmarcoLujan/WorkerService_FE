using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService_FE_Entities.ServiceLayer;

namespace WorkerService_FE_SL.Repository.Interfaces
{
    public interface IServicioRepository
    {
        public Task<string> SLLogin();
        //public Task<GeneralResponseDTO> PostInfo(InfoRequest infoRequest);
        public GeneralResponseDTO UpdateInfo(InfoRequest infoRequest);
    }
}
