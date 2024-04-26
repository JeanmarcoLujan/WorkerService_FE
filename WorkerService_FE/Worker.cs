using WorkerService_FE_Entities.ServiceLayer;
using WorkerService_FE_Entities.ServiceLayer.Document;
using WorkerService_FE_Request.Repository.Interfaces;
using WorkerService_FE_Response.Repository.Interfaces;
using WorkerService_FE_SL.Repository.Interfaces;

namespace WorkerService_FE
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestNCRepository _requestNCRepository;
        private readonly IRequestNDRepository _requestNDRepository;
        private readonly IResponseRepository _responseRepository;
        private readonly IServicioRepository _servicioRepository;
        private readonly IConfiguration _configuration;

        public Worker(
            ILogger<Worker> logger, 
            IRequestRepository requestRepository,
            IRequestNCRepository requestNCRepository,
            IRequestNDRepository requestNDRepository,
        IResponseRepository responseRepository, 
            IServicioRepository servicioRepository, 
            IConfiguration configuration)
        {
            _logger = logger;
            _requestRepository = requestRepository;
            _requestNCRepository = requestNCRepository;
            _requestNDRepository = requestNDRepository;
            _responseRepository = responseRepository;
            _servicioRepository = servicioRepository;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                string token = await _servicioRepository.SLLogin();

                if (token != "")
                {


                    //FACTURA DE DEUDORES
                    //int docs = _requestRepository.GetDocumentSAP();
                    //if (docs > 0)
                    //    _requestRepository.SendDocuement(token);



                    ////NOTAS DE CREDITO
                    //int docsNC = _requestNCRepository.GetDocumentSAPNC();
                    //if (docsNC > 0)
                    //    _requestNCRepository.SendDocuementNC(token);

                    ////NOTAS DE DEBITO
                    int docsND = _requestNDRepository.GetDocumentSAPND();
                    if (docsND > 0)
                        _requestNDRepository.SendDocuementND(token);




                    foreach (NCFResponse item in _responseRepository.GetDocumentSeguimientoSAP())
                    {
                        _responseRepository.GetResult(item, token);
                        //var asdf = item.ToString();
                    }


                }



                await Task.Delay(1000* int.Parse(_configuration["Servicio:Intervalo"].ToString()) , stoppingToken);
            }
        }
    }
}