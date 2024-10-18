using JMayer.Example.ASPReact.Server;
using JMayer.Example.ASPReact.Server.Airlines;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

#region Setup Database, Data Layers & Logging

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

FlightScheduleExampleBuilder flightScheduleExampleBuilder = new();
flightScheduleExampleBuilder.Build();

//Add the data layers. Because the example data needs to be built before registration and the data
//layers are memory based, the middleware is not creating the data layers which is awkard but on a
//normal website, the middleware would handle this.
builder.Services.AddSingleton<IAirlineDataLayer, AirlineDataLayer>(factory => (AirlineDataLayer)flightScheduleExampleBuilder.AirlineDataLayer);

#endregion

#region Setup Services

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

#endregion

var app = builder.Build();

#region Setup App

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("ReactPolicy");

app.MapControllers();
app.MapFallbackToFile("/index.html");

#endregion

app.Run();

//Used to expose the launching of the web application to xunit using WebApplicationFactory.
public partial class Program { }
