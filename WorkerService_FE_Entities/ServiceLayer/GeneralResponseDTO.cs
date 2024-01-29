using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_FE_Entities.ServiceLayer
{
    public class GeneralResponseDTO
    {
        public bool Registered { get; set; }
        public string Message { get; set; }
        public string Content { get; set; }

        public GeneralResponseDTO()
        {
            Registered = true;
            Message = "";
            Content = "";
        }
    }
}
