using System.Reflection;
using WorkerService_FE;
using WorkerService_FE_Entities.Repository;
using WorkerService_FE_Entities.Repository.Interfaces;
using WorkerService_FE_Request.Repository;
using WorkerService_FE_Request.Repository.Interfaces;
using WorkerService_FE_Response.Repository;
using WorkerService_FE_Response.Repository.Interfaces;
using WorkerService_FE_SL.Repository;
using WorkerService_FE_SL.Repository.Interfaces;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        services.AddHostedService<Worker>();
//    })
//    .Build();

//await host.RunAsync();


IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        // Configure the Windows Service Name.
        options.ServiceName = "MGSFE.Demo";
    })
    .ConfigureServices(services =>
    {
        // Register the primary worker service.
        services.AddHostedService<Worker>();
        //services.AddSingleton<IServiceLayerRepository, ServiceLayerRepository>();

        //services.AddSingleton<ISAPRepository, SAPRepository>(); //ISlRepository
        //services.AddSingleton<ISlRepository, SlRepository>();
        //services.AddSingleton<IAfipRepository, AfipRepository>();
        //services.AddSingleton<ILoggingRepository, LoggingRepository>();
        services.AddSingleton<IRequestRepository, RequestRepository>();
        services.AddSingleton<IRequestNCRepository, RequestNCRepository>();
        services.AddSingleton<IResponseRepository, ResponseRepository>();
        services.AddSingleton<IOperationRepository, OperationRepository>();
        services.AddSingleton<IServicioRepository, ServicioRepository>();
        services.AddSingleton<ILogRepository, LogRepository>();

        //services.AddHttpClient();
        services.AddHttpClient("reportService", c =>
        {
            //c.BaseAddress = new Uri("https://localhost:44316");
            c.BaseAddress = new Uri("http://localhost:9001");


        });


        //var configuration = new ConfigurationBuilder()
        //.SetBasePath(Directory.GetCurrentDirectory())
        //.AddJsonFile("appsettings.json")
        //.Build();

        var configuration = new ConfigurationBuilder()
        .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
        .AddJsonFile("appsettings.json");

        services.AddSingleton(configuration);



        // Register other services here...
    })
    .Build();

// Run the application.
await host.RunAsync();
