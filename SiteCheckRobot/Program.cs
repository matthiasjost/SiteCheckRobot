using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using SiteCheckRobot;

// TODO: use host builder without ASP.NET Core stuff since there's no actual ASP.NET Core functionality in the app
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<SiteCheckBackgroundWorker>();
builder.Services.AddTransient<PeriodicWork>();

var keyVaultUrl = builder.Configuration["keyVaultUrl"];
var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
            
var app = builder.Build();

app.Run();