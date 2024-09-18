using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkerService_FE_Entities.Repository.Interfaces;
using WorkerService_FE_Entities.ServiceLayer;
using WorkerService_FE_SL.Repository.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkerService_FE_SL.Repository
{
    public class ServicioRepository : IServicioRepository
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogRepository _logRepository;
        public ServicioRepository(IOperationRepository operationRepository, IConfiguration configuration, ILogRepository logRepositor)
        {
            _operationRepository = operationRepository;
            _configuration = configuration;
            _logRepository = logRepositor;
        }

        public async Task<string> SLLogin()
        {
            B1Session obj = null;
            string rs = "";
            try
            {
                using (var streamReader = new StreamReader(_operationRepository.PostLogin().GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    obj = JsonConvert.DeserializeObject<B1Session>(result);
                    rs = obj.SessionId.ToString();

                }
            }
            catch (Exception e)
            {

                rs = "";
                _logRepository.Log("Error al loguearse en SL :" + e.Message.ToString(), 3);
            }

            return rs;
        }

        public  GeneralResponseDTO UpdateInfo(InfoRequest infoRequest)
        {
            string route = _configuration["Acceso:ServiceLayerUrl"].ToString() + infoRequest.Route;

            GeneralResponseDTO documentResult = new GeneralResponseDTO();
            try
            {



                using (var streamReader = new StreamReader(_operationRepository.UpdInfo(infoRequest.Token, infoRequest.Doc, route).GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    // documentResult = JsonConvert.DeserializeObject<DeliveryRpta>(result);
                    documentResult.Registered = true;
                    documentResult.Message = "Se registro con exito";
                }
            }
            catch (WebException e)
            {
                //ErrorSL errorMessage = null;
                //errorMessage = ;

                documentResult.Registered = false;
                documentResult.Message = "Ocurrio un error";
                _logRepository.Log("Procesar la informacion SL :" + documentResult.Message, 3);
            }


            return documentResult;
        }
    }
}
