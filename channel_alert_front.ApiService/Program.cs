using channel_alert_front.ApiService.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfigure(builder);

WebApplication app = builder.Build();

app.UseConfigure();

app.Run();