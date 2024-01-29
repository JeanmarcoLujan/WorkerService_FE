using WorkerService_FE_Request.Repository.Interfaces;
using WorkerService_FE_Response.Repository.Interfaces;

namespace WorkerService_FE
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRequestRepository _requestRepository;
        private readonly IResponseRepository _responseRepository;

        public Worker(ILogger<Worker> logger, IRequestRepository requestRepository, IResponseRepository responseRepository)
        {
            _logger = logger;
            _requestRepository = requestRepository;
            _responseRepository = responseRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);


                _requestRepository.GetDocumentSAP();

                _requestRepository.SendDocuement();


                //_requestRepository.SendDocuement();
                //_responseRepository.GetResult();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}