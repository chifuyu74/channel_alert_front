var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder;
//.AddProject<Projects.channel_alert_front_ApiService>("apiservice");

builder.AddProject<Projects.channel_alert_front_Web>("webfrontend");
	//.WithExternalHttpEndpoints()
	//.WithReference(apiService);

builder.Build().Run();
