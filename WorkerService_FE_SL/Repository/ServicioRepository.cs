using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkerService_FE_Entities.ServiceLayer;
using WorkerService_FE_SL.Repository.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkerService_FE_SL.Repository
{
    public class ServicioRepository : IServicioRepository
    {
        private readonly IOperationRepository _operationRepository;
        public ServicioRepository(IOperationRepository operationRepository)
        {
              _operationRepository = operationRepository;
        }

        public Task<string> SLLogin()
        {
            throw new NotImplementedException();
        }

        public async Task<GeneralResponseDTO> UpdateInfo(InfoRequest infoRequest)
        {
            string route = @"https://saphaargendemo:50000/b1s/v2/" + infoRequest.Route;

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
                //_logService.Log("Procesar la informacion SL :" + documentResult.Message);
            }


            return documentResult;
        }
    }
}
