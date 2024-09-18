using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService_FE_Entities.Repository.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkerService_FE_Entities.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly IConfiguration _configuration;
        public LogRepository(IConfiguration configuration)
        {
                _configuration = configuration;
        }
        public void Log(string message, int flujo)
        {
            string route = "";

            switch (flujo)
            {
                case 1:
                    route = _configuration["Log:RouteRequest"];
                    break;
                case 2:
                    route = _configuration["Log:RouteResponse"];
                    break;
                default:
                    route = _configuration["Log:RouteLog"];
                    break;
            }
             

            if (!Directory.Exists(route))
            {
                Directory.CreateDirectory(route);
            }

            // Creamos el archivo de log correspondiente al día de hoy
            string logFilePath = Path.Combine(route+"\\", "MGS_FE "+ $"{DateTime.Today:yyyyMMdd}.log");
            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                // Escribimos el mensaje en el archivo de log
                sw.WriteLine($"Service MGS FE - [{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
            }
        }
    }
}
