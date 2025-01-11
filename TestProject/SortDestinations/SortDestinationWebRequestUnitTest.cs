using JMayer.Data.Data;
using JMayer.Data.HTTP.DataLayer;
using JMayer.Example.ASPReact.Server.SortDestinations;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace TestProject.SortDestinations;

/// <summary>
/// The class manages tests for sort destinations using both the http client and server.
/// </summary>
/// <remarks>
/// The example web server creates default data objects and the unit tests
/// uses this already existing data.
/// </remarks>
public class SortDestinationWebRequestUnitTest : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// The factory for the web application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// The constant for the default sort destination name.
    /// </summary>
    private const string DefaultSortDestinationName = "A Sort Destination";

    /// <summary>
    /// The dependency injection constructor.
    /// </summary>
    /// <param name="factory">The factory for the web application.</param>
    public SortDestinationWebRequestUnitTest(WebApplicationFactory<Program> factory) => _factory = factory;

    /// <summary>
    /// The method verifies the HTTP data layer is not allowed to request a sort destination to be created by the server
    /// </summary>
    [Fact]
    public async Task VerifyAddSortDestinationNotAllowed()
    {
        HttpClient httpClient = _factory.CreateClient();
        SortDestinationDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new SortDestination() { Name = DefaultSortDestinationName });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No sort destination or server side validation result was returned.
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
    public async Task VerifyCountSortDestinations()
    {
        HttpClient httpClient = _factory.CreateClient();
        SortDestinationDataLayer dataLayer = new(httpClient);

        long count = await dataLayer.CountAsync();
        Assert.True(count > 0);
    }

    /// <summary>
    /// The method verifies the HTTP data layer is not allowed to request a sort destination to be deleted by the server
    /// </summary>
    [Fact]
    public async Task VerifyDeleteSortDestinationNotAllowed()
    {
        HttpClient httpClient = _factory.CreateClient();
        SortDestinationDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.DeleteAsync(new SortDestination() { Name = DefaultSortDestinationName });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No sort destination or server side validation result was returned.
        Assert.Null(operationResult.DataObject);
        Assert.Null(operationResult.ServerSideValidationResult);

        //A method not allowed status was returned.
        Assert.Equal(HttpStatusCode.MethodNotAllowed, operationResult.StatusCode);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request all sort destinations from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetAllSortDestinations()
    {
        HttpClient httpClient = _factory.CreateClient();
        SortDestinationDataLayer dataLayer = new(httpClient);

        List<SortDestination>? sortDestinations = await dataLayer.GetAllAsync();

        //Sort destinations must have been returned.
        Assert.NotNull(sortDestinations);
        Assert.NotEmpty(sortDestinations);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request all sort destinations as list views from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetAllListViewSortDestinations()
    {
        HttpClient httpClient = _factory.CreateClient();
        SortDestinationDataLayer dataLayer = new(httpClient);

        List<ListView>? sortDestinations = await dataLayer.GetAllListViewAsync();

        //List view sort destinations must have been returned.
        Assert.NotNull(sortDestinations);
        Assert.NotEmpty(sortDestinations);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request the first sort destination from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetSingleSortDestination()
    {
        HttpClient httpClient = _factory.CreateClient();
        SortDestinationDataLayer dataLayer = new(httpClient);

        SortDestination? sortDestination = await dataLayer.GetSingleAsync();
        Assert.NotNull(sortDestination);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request a sort destination from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetSingleSortDestinationWithId()
    {
        HttpClient httpClient = _factory.CreateClient();
        SortDestinationDataLayer dataLayer = new(httpClient);

        SortDestination? sortDestination = await dataLayer.GetSingleAsync(1);
        Assert.NotNull(sortDestination);
    }

    /// <summary>
    /// The method verifies the HTTP data layer is not allowed to request a sort destination to be updated by the server
    /// </summary>
    [Fact]
    public async Task VerifyUpdateSortDestinationNotAllowed()
    {
        HttpClient httpClient = _factory.CreateClient();
        SortDestinationDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.UpdateAsync(new SortDestination() { Name = DefaultSortDestinationName });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No sort destination or server side validation result was returned.
        Assert.Null(operationResult.DataObject);
        Assert.Null(operationResult.ServerSideValidationResult);

        //A method not allowed status was returned.
        Assert.Equal(HttpStatusCode.MethodNotAllowed, operationResult.StatusCode);
    }
}
