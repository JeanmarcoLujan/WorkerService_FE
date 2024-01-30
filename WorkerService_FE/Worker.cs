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
        private readonly IResponseRepository _responseRepository;
        private readonly IServicioRepository _servicioRepository;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IRequestRepository requestRepository, IResponseRepository responseRepository, IServicioRepository servicioRepository, IConfiguration configuration)
        {
            _logger = logger;
            _requestRepository = requestRepository;
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

                    
                    int docs = _requestRepository.GetDocumentSAP();
                    if (docs > 0)
                        _requestRepository.SendDocuement(token);
                }


                //_requestRepository.GetDocumentSAP();

                //_requestRepository.SendDocuement();


                //_requestRepository.SendDocuement();
                //_responseRepository.GetResult();

                await Task.Delay(1000* int.Parse(_configuration["Servicio:Intervalo"].ToString()) , stoppingToken);
            }
        }
    }
}