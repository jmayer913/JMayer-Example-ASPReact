using JMayer.Data.HTTP.DataLayer;
using JMayer.Example.ASPReact.Server.Airlines;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace TestProject.Test.WebRequest.Airlines;

/// <summary>
/// The class manages tests for airlines using both the http client and server.
/// </summary>
/// <remarks>
/// The example web server creates default data objects and the unit tests
/// uses this already existing data.
/// </remarks>
public class AirlineUnitTest : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// The factory for the web application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// The dependency injection constructor.
    /// </summary>
    /// <param name="factory">The factory for the web application.</param>
    public AirlineUnitTest(WebApplicationFactory<Program> factory) => _factory = factory;

    /// <summary>
    /// The method verifies the HTTP data layer can request an airline to be created by the server and the server can successfully process the request.
    /// </summary>
    /// <param name="name">The name of the airline.</param>
    /// <param name="description">The description for the airline.</param>
    /// <param name="iata">The IATA code assigned to the airline by the IATA Organization.</param>
    /// <param name="icao">The ICAO code assigned to the airline by the International Aviation Organization.</param>
    /// <param name="numberCode">The number code assigned to the airline by the IATA Organization.</param>
    /// <returns>A Task object for the async.</returns>
    //[Theory]
    //public async Task VerifyAddAirline(string name, string description, string iata, string icao, string numberCode)
    //{
    //    HttpClient httpClient = _factory.CreateClient();
    //    HTTP.DataLayer.Airlines.AirlineDataLayer dataLayer = new(httpClient);

    //    Airline airline = new()
    //    {
    //        Description = description,
    //        IATA = iata,
    //        ICAO = icao,
    //        Name = name,
    //        NumberCode = numberCode,
    //    };
    //    OperationResult operationResult = await dataLayer.CreateAsync(airline);

    //    Assert.True(operationResult.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.
    //    Assert.IsType<Airline>(operationResult.DataObject); //An airline must have been returned.
    //    //Assert.True(new AirlineEqualityComparer(true, true, true).Equals((Airline)operationResult.DataObject, airline), "The data object sent should be the same as the data object returned."); //Original and return must be equal.
    //}

    /// <summary>
    /// The method verifies the server will return a failure if the airline ICAO already exists when adding a new airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddDuplicateAirlineICAOFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        HTTP.DataLayer.Airlines.AirlineDataLayer dataLayer = new(httpClient);

        Airline airline = new()
        {
            IATA = "ZC",
            ICAO = "ZZC",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Duplicate ICAO Test 1",
        };
        OperationResult operationResult = await dataLayer.CreateAsync(airline);

        if (!operationResult.IsSuccessStatusCode)
        {
            Assert.Fail("Failed to create the first airline.");
            return;
        }

        airline = new()
        {
            IATA = "ZC",
            ICAO = "ZZC",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Duplicate ICAO Test New",
        };
        operationResult = await dataLayer.CreateAsync(airline);

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No asset was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //A validation error was returned.
        Assert.NotNull(operationResult.ServerSideValidationResult);
        Assert.Single(operationResult.ServerSideValidationResult.Errors);

        //The correct error was returned.
        Assert.Contains("ICAO must be unique", operationResult.ServerSideValidationResult.Errors[0].ErrorMessage);
        Assert.Equal(nameof(Airline.ICAO), operationResult.ServerSideValidationResult.Errors[0].PropertyName);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline name already exists when adding a new airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddDuplicateAirlineNameFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        HTTP.DataLayer.Airlines.AirlineDataLayer dataLayer = new(httpClient);

        Airline airline = new()
        {
            IATA = "ZA",
            ICAO = "ZZA",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Duplicate Name Test",
        };
        OperationResult operationResult = await dataLayer.CreateAsync(airline);

        if (!operationResult.IsSuccessStatusCode)
        {
            Assert.Fail("Failed to create the first airline.");
            return;
        }

        airline = new()
        {
            IATA = "ZB",
            ICAO = "ZZB",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Duplicate Name Test",
        };
        operationResult = await dataLayer.CreateAsync(airline);

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No asset was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //A validation error was returned.
        Assert.NotNull(operationResult.ServerSideValidationResult);
        Assert.Single(operationResult.ServerSideValidationResult.Errors);

        //The correct error was returned.
        Assert.Contains("name already exists", operationResult.ServerSideValidationResult.Errors[0].ErrorMessage);
        Assert.Equal(nameof(Airline.Name), operationResult.ServerSideValidationResult.Errors[0].PropertyName);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline number code already exists when adding a new airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddDuplicateAirlineNumberCodeFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        HTTP.DataLayer.Airlines.AirlineDataLayer dataLayer = new(httpClient);

        Airline airline = new()
        {
            IATA = "ZD",
            ICAO = "ZZD",
            NumberCode = "999",
            Name = "Duplicate Number Code Test Test 1",
        };
        OperationResult operationResult = await dataLayer.CreateAsync(airline);

        if (!operationResult.IsSuccessStatusCode)
        {
            Assert.Fail("Failed to create the first airline.");
            return;
        }

        airline = new()
        {
            IATA = "ZE",
            ICAO = "ZZE",
            NumberCode = "999",
            Name = "Duplicate Number Code Test New",
        };
        operationResult = await dataLayer.CreateAsync(airline);

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No asset was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //A validation error was returned.
        Assert.NotNull(operationResult.ServerSideValidationResult);
        Assert.Single(operationResult.ServerSideValidationResult.Errors);

        //The correct error was returned.
        Assert.Contains("number code must be unique", operationResult.ServerSideValidationResult.Errors[0].ErrorMessage);
        Assert.Equal(nameof(Airline.NumberCode), operationResult.ServerSideValidationResult.Errors[0].PropertyName);
    }
}
