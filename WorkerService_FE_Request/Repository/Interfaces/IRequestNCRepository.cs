﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_FE_Request.Repository.Interfaces
{
    public interface IRequestNCRepository
    {
        public void SendDocuementNC(string token);
        public int GetDocumentSAPNC();
    }
}