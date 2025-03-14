using VolumeFootprint.WorkerService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddBinance();

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IVolumeManager, VolumeManager>();

var host = builder.Build();
host.Run();