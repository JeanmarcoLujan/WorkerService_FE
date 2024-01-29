using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_FE_SL.Repository.Interfaces
{
    public interface IOperationRepository
    {
        public HttpWebResponse PostLogin();
        public HttpWebResponse PostInfo(string obj, string document, string route);
        public HttpWebResponse UpdInfo(string obj, string document, string route);
    }
}
