using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_FE_Entities.Repository.Interfaces
{
    public interface ILogRepository
    {
        public void Log(string message, int flujo);
    }
}
