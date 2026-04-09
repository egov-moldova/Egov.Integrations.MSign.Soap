var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSystemCertificate(builder.Configuration.GetSection("Certificate"));
builder.Services.AddMSignSoapClient(builder.Configuration.GetSection("MSignSoap"));
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();