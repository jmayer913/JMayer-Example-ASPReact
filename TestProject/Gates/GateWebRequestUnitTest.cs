using JMayer.Data.Data;
using JMayer.Data.HTTP.DataLayer;
using JMayer.Example.ASPReact.Server.Gates;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace TestProject.Gates;

/// <summary>
/// The class manages tests for gates using both the http client and server.
/// </summary>
/// <remarks>
/// The example web server creates default data objects and the unit tests
/// uses this already existing data.
/// </remarks>
public class GateWebRequestUnitTest : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// The factory for the web application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// The constant for the default gate name.
    /// </summary>
    private const string DefaultGateName = "A Gate";

    /// <summary>
    /// The dependency injection constructor.
    /// </summary>
    /// <param name="factory">The factory for the web application.</param>
    public GateWebRequestUnitTest(WebApplicationFactory<Program> factory) => _factory = factory;

    /// <summary>
    /// The method verifies the HTTP data layer is not allowed to request a gate to be created by the server
    /// </summary>
    [Fact]
    public async Task VerifyAddGateNotAllowed()
    {
        HttpClient httpClient = _factory.CreateClient();
        GateDataLyaer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Gate() {  Name = DefaultGateName });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No gate or server side validation result was returned.
        Assert.Null(operationResult.DataObject);
        Assert.Null(operationResult.ServerSideValidationResult);

        //A method not allowed status was returned.
        Assert.Equal(HttpStatusCode.MethodNotAllowed, operationResult.StatusCode);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request the count from the server and the server can successfully process the request.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task VerifyCountGates()
    {
        HttpClient httpClient = _factory.CreateClient();
        GateDataLyaer dataLayer = new(httpClient);

        long count = await dataLayer.CountAsync();
        Assert.True(count > 0);
    }

    /// <summary>
    /// The method verifies the HTTP data layer is not allowed to request a gate to be deleted by the server
    /// </summary>
    [Fact]
    public async Task VerifyDeleteGateNotAllowed()
    {
        HttpClient httpClient = _factory.CreateClient();
        GateDataLyaer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.DeleteAsync(new Gate() { Name = DefaultGateName });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No gate or server side validation result was returned.
        Assert.Null(operationResult.DataObject);
        Assert.Null(operationResult.ServerSideValidationResult);

        //A method not allowed status was returned.
        Assert.Equal(HttpStatusCode.MethodNotAllowed, operationResult.StatusCode);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request all gates from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetAllGates()
    {
        HttpClient httpClient = _factory.CreateClient();
        GateDataLyaer dataLayer = new(httpClient);

        List<Gate>? gates = await dataLayer.GetAllAsync();

        //Gates must have been returned.
        Assert.NotNull(gates);
        Assert.NotEmpty(gates);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request all gates as list views from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetAllListViewGates()
    {
        HttpClient httpClient = _factory.CreateClient();
        GateDataLyaer dataLayer = new(httpClient);

        List<ListView>? gates = await dataLayer.GetAllListViewAsync();

        //List view gates must have been returned.
        Assert.NotNull(gates);
        Assert.NotEmpty(gates);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request the first gate from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetSingleGate()
    {
        HttpClient httpClient = _factory.CreateClient();
        GateDataLyaer dataLayer = new(httpClient);

        Gate? gate = await dataLayer.GetSingleAsync();
        Assert.NotNull(gate);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request a gate from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetSingleGateWithId()
    {
        HttpClient httpClient = _factory.CreateClient();
        GateDataLyaer dataLayer = new(httpClient);

        Gate? gate = await dataLayer.GetSingleAsync(1);
        Assert.NotNull(gate);
    }

    /// <summary>
    /// The method verifies the HTTP data layer is not allowed to request a gate to be updated by the server
    /// </summary>
    [Fact]
    public async Task VerifyUpdateGateNotAllowed()
    {
        HttpClient httpClient = _factory.CreateClient();
        GateDataLyaer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.UpdateAsync(new Gate() { Name = DefaultGateName });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No gate or server side validation result was returned.
        Assert.Null(operationResult.DataObject);
        Assert.Null(operationResult.ServerSideValidationResult);

        //A method not allowed status was returned.
        Assert.Equal(HttpStatusCode.MethodNotAllowed, operationResult.StatusCode);
    }
}
