using JMayer.Data.Data;
using JMayer.Data.HTTP.DataLayer;
using JMayer.Example.ASPReact.Server.Airlines;
using JMayer.Example.ASPReact.Server.Flights;
using JMayer.Example.ASPReact.Server.Gates;
using JMayer.Example.ASPReact.Server.SortDestinations;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TestProject.Gates;

namespace TestProject.Flights;

/// <summary>
/// The class manages tests for flights using both the http client and server.
/// </summary>
/// <remarks>
/// The example web server creates default data objects and the unit tests
/// uses this already existing data.
/// </remarks>
public class FlightWebRequestUnitTest : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// The factory for the web application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// The constant for the airline for the airline cascade delete test.
    /// </summary>
    private const string AirlineCascadeDelete = "ZZ";

    /// <summary>
    /// The constant for a bad airline ID.
    /// </summary>
    private const long BadAirlineID = 99;

    /// <summary>
    /// A constant for a badly formatted destination.
    /// </summary>
    private const string BadFormatttedDestination = "ZZZZ";

    /// <summary>
    /// A constant for a badly formatted flight number.
    /// </summary>
    private const string BadFormatttedFlightNumber = "AS10";

    /// <summary>
    /// The constant for a bad gate ID.
    /// </summary>
    private const long BadGateID = 99;

    /// <summary>
    /// The constant for a bad sort destination ID.
    /// </summary>
    private const long BadSortDestinationID = 99;

    /// <summary>
    /// The constant for the default airline ID.
    /// </summary>
    private const long DefaultAirlineID = 1;

    /// <summary>
    /// The constant for the default airport code.
    /// </summary>
    private const string DefaultAirportCode = "ZZZ";

    /// <summary>
    /// The constant for the default gate ID.
    /// </summary>
    private const long DefaultGateID = 1;

    /// <summary>
    /// The constant for the default sort destination ID.
    /// </summary>
    private const long DefaultSortDestinationID = 1;

    /// <summary>
    /// The constant for the flight number for the airline not found test.
    /// </summary>
    private const string FlightNumberAddTestAirlineNotFound = "9999";

    /// <summary>
    /// The constant for the flight number for the codeshare airline not found test.
    /// </summary>
    private const string FlightNumberAddTestCodeShareAirlineNotFound = "9998";

    /// <summary>
    /// The constant for the flight number for the add duplicate test.
    /// </summary>
    private const string FlightNumberAddTestDuplicate = "9997";

    /// <summary>
    /// The constant for the flight number for the gate not found test.
    /// </summary>
    private const string FlightNumberAddTestGateNotFound = "9996";

    /// <summary>
    /// The constant for the flight number for the sort destination not found test.
    /// </summary>
    private const string FlightNumberAddTestSortDestinationNotFound = "9995";

    /// <summary>
    /// The constant for the flight number for the cascade delete test.
    /// </summary>
    private const string FlightNumberCascadeDelete = "9994";

    /// <summary>
    /// The constant for the flight number for the delete test.
    /// </summary>
    private const string FlightNumberDeleteTest = "9993";

    /// <summary>
    /// The constant for the flight number for the airline not found test.
    /// </summary>
    private const string FlightNumberUpdateTestAirlineNotFound = "9992";

    /// <summary>
    /// The constant for the flight number for the bad destination test.
    /// </summary>
    private const string FlightNumberUpdateTestBadDestination = "9991";

    /// <summary>
    /// The constant for the flight number for the bad flight number test.
    /// </summary>
    private const string FlightNumberUpdateTestBadFlightNumber = "9990";

    /// <summary>
    /// The constant for the flight number for the codeshare airline not found test.
    /// </summary>
    private const string FlightNumberUpdateTestCodeShareAirlineNotFound = "9989";

    /// <summary>
    /// The constant for the duplicate flight number for the update test.
    /// </summary>
    private const string FlightNumberUpdateTestDuplicate1 = "9988";

    /// <summary>
    /// The constant for the duplicate flight number for the update test.
    /// </summary>
    private const string FlightNumberUpdateTestDuplicate2 = "9987";

    /// <summary>
    /// The constant for the flight number for the gate not found test.
    /// </summary>
    private const string FlightNumberUpdateTestGateNotFound = "9986";

    /// <summary>
    /// The constant for the flight number for the sort destination not found test.
    /// </summary>
    private const string FlightNumberUpdateTestSortDestinationNotFound = "9985";

    /// <summary>
    /// The dependency injection constructor.
    /// </summary>
    /// <param name="factory">The factory for the web application.</param>
    public FlightWebRequestUnitTest(WebApplicationFactory<Program> factory) => _factory = factory;

    /// <summary>
    /// The method creates an airline.
    /// </summary>
    /// <param name="iata">The IATA code for the airline.</param>
    /// <returns>An airline or null if it failed to create.</returns>
    private async Task<Airline?> CreateAirlineAsync(string iata)
    {
        HttpClient httpClient = _factory.CreateClient();
        Airlines.AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = iata,
            Name = iata,
            SortDestinationID = DefaultSortDestinationID,
        });

        return operationResult.DataObject as Airline;
    }

    /// <summary>
    /// The method returns a list of CodeShare objects created from the comma separated list.
    /// </summary>
    /// <param name="codeshareCommaSeparatedList">A comma separated list of codeshares; each element will be the airline's IATA concatenated with the flight number used by the airline.</param>
    /// <returns>A list of CodeShare objects.</returns>
    private async Task<List<CodeShare>> CreateCodeSharesAsync(string? codeshareCommaSeparatedList)
    {
        if (codeshareCommaSeparatedList is null)
        {
            return [];
        }

        List<CodeShare> codeshares = [];
        string[] splitCodeShares = codeshareCommaSeparatedList.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var codeshareString in splitCodeShares)
        {
            string airlineIATACode = codeshareString.Substring(0, 2);
            string flightNumber = codeshareString.Substring(2);

            Airline? airline = await GetAirlineByIATAAsync(airlineIATACode);

            if (airline is not null)
            {
                codeshares.Add(new CodeShare()
                {
                    AirlineID = airline.Integer64ID,
                    FlightNumber = flightNumber,
                });
            }
        }

        return codeshares;
    }

    /// <summary>
    /// The method returns the airline.
    /// </summary>
    /// <param name="iata">The IATA to search for.</param>
    /// <returns>The airline or null if not found.</returns>
    private async Task<Airline?> GetAirlineByIATAAsync(string iata)
    {
        HttpClient httpClient = _factory.CreateClient();
        Airlines.AirlineDataLayer dataLayer = new(httpClient);

        List<Airline>? airlines = await dataLayer.GetAllAsync();
        return airlines?.FirstOrDefault(obj => obj.IATA == iata);
    }

    /// <summary>
    /// The method returns the gate.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <returns>The gate or null if not found.</returns>
    private async Task<Gate?> GetGateAsync(string name)
    {
        HttpClient httpClient = _factory.CreateClient();
        GateDataLyaer dataLayer = new(httpClient);

        List<Gate>? gates = await dataLayer.GetAllAsync();
        return gates?.FirstOrDefault(obj => obj.Name == name);
    }

    /// <summary>
    /// The method returns the sort destination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <returns>The sort destination or null if not found.</returns>
    private async Task<SortDestination?> GetSortDestinationAsync(string name)
    {
        HttpClient httpClient = _factory.CreateClient();
        SortDestinations.SortDestinationDataLayer dataLayer = new(httpClient);

        List<SortDestination>? sortDestinations = await dataLayer.GetAllAsync();
        return sortDestinations?.FirstOrDefault(obj => obj.Name == name);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the flight already exists when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddDuplicateFlightFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberAddTestDuplicate,
            GateID = DefaultGateID,
            Name = "Add Duplicate Flight Test 1",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });
        Assert.True(operationResult.IsSuccessStatusCode, "Failed to create the first flight.");

        operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberAddTestDuplicate,
            GateID = DefaultGateID,
            Name = "Add Duplicate Flight Test New",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.FlightNumber));
        Assert.Single(operationResult.ValidationErrors[nameof(Flight.FlightNumber)]);
        Assert.Equal("The flight already exists in the schedule.", operationResult.ValidationErrors[nameof(Flight.FlightNumber)][0]);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request a flight to be created by the server and the server can successfully process the request.
    /// </summary>
    /// <param name="gateName">The name of the gate.</param>
    /// <param name="airlineIATA">The IATA code for the airline.</param>
    /// <param name="flightNumber">The identifier for the flight.</param>
    /// <param name="destination">The destination for the flight.</param>
    /// <param name="codeshareCommaSeparatedList">A comma separated list of codeshares; each element will be the airline's IATA concatenated with the flight number used by the airline.</param>
    /// <param name="sortDestinationName">The name of the sort destination.</param>
    /// <returns>A Task object for the async.</returns>
    [Theory]
    [InlineData("A1", "AA", "3124", "ZZZ", null, "MU1")]
    [InlineData("B1", "DL", "3985", "ZZZ", "AA0235", "MU2")]
    [InlineData("C1", "WN", "3985", "ZZZ", "AA9382,DL8274", "MU3")]
    public async Task VerifyAddFlight(string gateName, string airlineIATA, string flightNumber, string destination, string? codeshareCommaSeparatedList, string sortDestinationName)
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        Gate? gate = await GetGateAsync(gateName);

        if (gate is null)
        {
            Assert.Fail("Failed to retrieve the gate.");
        }
        
        Airline? airline = await GetAirlineByIATAAsync(airlineIATA);

        if (airline is null)
        {
            Assert.Fail("Failed to retrieve the airline.");
        }

        SortDestination? sortDestination = await GetSortDestinationAsync(sortDestinationName);

        if (sortDestination is null)
        {
            Assert.Fail("Failed to retrieve the sort destination.");
        }

        List<CodeShare> codeshares = await CreateCodeSharesAsync(codeshareCommaSeparatedList);

        Flight flight = new()
        {
            AirlineID = airline.Integer64ID,
            CodeShares = codeshares,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = flightNumber,
            GateID = gate.Integer64ID,
            Name = $"{airline.IATA}{flightNumber}",
            Destination = destination,
            SortDestinationID = sortDestination.Integer64ID,
        };
        OperationResult operationResult = await dataLayer.CreateAsync(flight);

        Assert.True(operationResult.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.
        Assert.IsType<Flight>(operationResult.DataObject); //A flight must have been returned.
        Assert.True(new FlightEqualityComparer(true, true, true).Equals((Flight)operationResult.DataObject, flight), "The data object sent should be the same as the data object returned."); //Original and return must be equal.
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline ID doesn't exist when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddFlightAirlineNotFoundFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = BadAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberAddTestAirlineNotFound,
            GateID = DefaultGateID,
            Name = "Add Flight Airline Not Found Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.AirlineID));
        Assert.Single(operationResult.ValidationErrors[nameof(Flight.AirlineID)]);
        Assert.Equal($"The {BadAirlineID} airline was not found in the data store.", operationResult.ValidationErrors[nameof(Flight.AirlineID)][0]);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the flight destination is badly formatted when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddFlightBadDestinationFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = "0000",
            GateID = DefaultGateID,
            Name = "Add Bad Destination Test",
            Destination = BadFormatttedDestination,
            SortDestinationID = DefaultSortDestinationID,
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.Destination));
        Assert.Single(operationResult.ValidationErrors[nameof(Flight.Destination)]);
        Assert.Equal("The city must be 3 capital letters.", operationResult.ValidationErrors[nameof(Flight.Destination)][0]);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the flight number is badly formatted when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddFlightBadFlightNumberFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = BadFormatttedFlightNumber,
            GateID = DefaultGateID,
            Name = "Add Bad Flight Number Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.FlightNumber));
        Assert.Single(operationResult.ValidationErrors[nameof(Flight.FlightNumber)]);
        Assert.Equal("The flight number must be 4 digits or 4 digits and a capital letter.", operationResult.ValidationErrors[nameof(Flight.FlightNumber)][0]);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the codeshare airline ID doesn't exist when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddFlightCodeShareAirlineNotFoundFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            CodeShares = [new CodeShare() { AirlineID = BadAirlineID, FlightNumber = FlightNumberAddTestCodeShareAirlineNotFound }],
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberAddTestCodeShareAirlineNotFound,
            GateID = DefaultGateID,
            Name = "Add Flight CodeShare Airline Not Found Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.AirlineID));
        Assert.Single(operationResult.ValidationErrors[nameof(Flight.AirlineID)]);
        Assert.Equal($"The {BadAirlineID} airline for the codeshare was not found in the data store.", operationResult.ValidationErrors[nameof(Flight.AirlineID)][0]);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the gate ID doesn't exist when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddFlightGateNotFoundFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberAddTestGateNotFound,
            GateID = BadGateID,
            Name = "Add Flight Gate Not Found Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.GateID));
        Assert.Single(operationResult.ValidationErrors[nameof(Flight.GateID)]);
        Assert.Equal($"The {BadGateID} gate was not found in the data store.", operationResult.ValidationErrors[nameof(Flight.GateID)][0]);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the sort destination ID doesn't exist when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddFlightSortDestinationNotFoundFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberAddTestSortDestinationNotFound,
            GateID = DefaultGateID,
            Name = "Add Flight Sort Destination Not Found Test",
            Destination = DefaultAirportCode,
            SortDestinationID = BadSortDestinationID,
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.SortDestinationID));
        Assert.Single(operationResult.ValidationErrors[nameof(Flight.SortDestinationID)]);
        Assert.Equal($"The {BadSortDestinationID} sort destination was not found in the data store.", operationResult.ValidationErrors[nameof(Flight.SortDestinationID)][0]);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request the count from the server and the server can successfully process the request.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task VerifyCountFlights()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        long count = await dataLayer.CountAsync();
        Assert.True(count > 0);
    }

    /// <summary>
    /// The method verifies on the server-side if an airline is deleted, the associated flights are also deleted.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyDeleteAirlineCascade()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        Airline? airline = await CreateAirlineAsync(AirlineCascadeDelete);

        if (airline is null)
        {
            Assert.Fail("Failed to create the airline.");
        }

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberCascadeDelete,
            GateID = DefaultGateID,
            Name = "Delete Airline Cascade Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });
        Assert.True(operationResult.IsSuccessStatusCode, "Failed to create the flight.");

        operationResult = await new Airlines.AirlineDataLayer(httpClient).DeleteAsync(airline);
        Assert.True(operationResult.IsSuccessStatusCode, "Failed to delete the airline.");

        List<Flight>? flights = await dataLayer.GetAllAsync();

        if (flights is null)
        {
            Assert.Fail("Failed to query the flights.");
        }
        
        flights = [.. flights.Where(obj => obj.AirlineID == airline.Integer64ID)];
        Assert.Empty(flights);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request a flight to be deleted by the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyDeleteFlight()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberDeleteTest,
            GateID = DefaultGateID,
            Name = "Delete Flight Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Flight flight)
        {
            operationResult = await dataLayer.DeleteAsync(flight);
            Assert.True(operationResult.IsSuccessStatusCode);
        }
        else
        {
            Assert.Fail("Failed to create the flight.");
        }
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request all flights from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetAllFlights()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);
        List<Flight>? flights = await dataLayer.GetAllAsync();

        //Flights must have been returned.
        Assert.NotNull(flights);
        Assert.NotEmpty(flights);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request all flights as list views from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetAllListViewFlights()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);
        List<ListView>? flights = await dataLayer.GetAllListViewAsync();

        //List view flights must have been returned.
        Assert.NotNull(flights);
        Assert.NotEmpty(flights);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request the first flights from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetSingleFlight()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        Flight? flight = await dataLayer.GetSingleAsync();
        Assert.NotNull(flight);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request a flights from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetSingleFlightWithId()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        Flight? flight = await dataLayer.GetSingleAsync(1);
        Assert.NotNull(flight);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the flight already exists when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateDuplicateFlightFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberUpdateTestDuplicate1,
            GateID = DefaultGateID,
            Name = "Update Duplicate Flight Test 1",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });
        Assert.True(operationResult.IsSuccessStatusCode, "Failed to create the first flight.");

        operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberUpdateTestDuplicate2,
            GateID = DefaultGateID,
            Name = "Update Duplicate Flight Test New",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Flight secondFlight)
        {
            operationResult = await dataLayer.UpdateAsync(new Flight()
            {
                AirlineID = DefaultAirlineID,
                DepartTime = DateTime.Now.TimeOfDay,
                FlightNumber = FlightNumberUpdateTestDuplicate1,
                GateID = DefaultGateID,
                Integer64ID = secondFlight.Integer64ID,
                Name = "Update Duplicate Flight Test New",
                Destination = DefaultAirportCode,
                SortDestinationID = DefaultSortDestinationID,
            });

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.FlightNumber));
            Assert.Single(operationResult.ValidationErrors[nameof(Flight.FlightNumber)]);
            Assert.Equal("The flight already exists in the schedule.", operationResult.ValidationErrors[nameof(Flight.FlightNumber)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the second flight.");
        }
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request a flight to be updated by the server and the server can successfully process the request.
    /// </summary>
    /// <param name="gateName">The name of the gate.</param>
    /// <param name="airlineIATA">The IATA code for the airline.</param>
    /// <param name="flightNumber">The identifier for the flight.</param>
    /// <param name="destination">The destination for the flight.</param>
    /// <param name="codeshareCommaSeparatedList">A comma separated list of codeshares; each element will be the airline's IATA concatenated with the flight number used by the airline.</param>
    /// <param name="sortDestinationName">The name of the sort destination.</param>
    /// <returns>A Task object for the async.</returns>
    [Theory]
    [InlineData("A2", "AA", "4592", "ZZA", null, "MU2")]
    [InlineData("B2", "DL", "4201", "ZZB", "AA5928", "MU3")]
    [InlineData("C2", "WN", "4728", "ZZC", "AA8382,DL7274", "MU4")]
    public async Task VerifyUpdateFlight(string gateName, string airlineIATA, string flightNumber, string destination, string? codeshareCommaSeparatedList, string sortDestinationName)
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        Gate? gate = await GetGateAsync(gateName);

        if (gate is null)
        {
            Assert.Fail("Failed to retrieve the gate.");
        }

        Airline? airline = await GetAirlineByIATAAsync(airlineIATA);

        if (airline is null)
        {
            Assert.Fail("Failed to retrieve the airline.");
        }

        SortDestination? sortDestination = await GetSortDestinationAsync(sortDestinationName);

        if (sortDestination is null)
        {
            Assert.Fail("Failed to retrieve the sort destination.");
        }

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = airline.Integer64ID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = flightNumber,
            GateID = DefaultGateID,
            Name = $"{airline.IATA}{flightNumber}",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Flight createdFlight)
        {
            List<CodeShare> codeshares = await CreateCodeSharesAsync(codeshareCommaSeparatedList);

            Flight flight = new()
            {
                AirlineID = airline.Integer64ID,
                CodeShares = codeshares,
                DepartTime = DateTime.Now.TimeOfDay,
                FlightNumber = flightNumber,
                GateID = gate.Integer64ID,
                Integer64ID = createdFlight.Integer64ID,
                Name = $"{airline.IATA}{flightNumber}",
                Destination = destination,
                SortDestinationID = sortDestination.Integer64ID,
            };
            operationResult = await dataLayer.UpdateAsync(flight);

            Assert.True(operationResult.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.
            Assert.IsType<Flight>(operationResult.DataObject); //A flight must have been returned.
            Assert.True(new FlightEqualityComparer(true, true, true).Equals((Flight)operationResult.DataObject, flight), "The data object sent should be the same as the data object returned."); //Original and return must be equal.
        }
        else
        {
            Assert.Fail("Failed to create the flight.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline ID doesn't exist when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateFlightAirlineNotFoundFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberUpdateTestAirlineNotFound,
            GateID = DefaultGateID,
            Name = "Update Flight Airline Not Found Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Flight createdFlight)
        {
            operationResult = await dataLayer.UpdateAsync(new Flight()
            {
                AirlineID = BadAirlineID,
                DepartTime = DateTime.Now.TimeOfDay,
                FlightNumber = FlightNumberUpdateTestAirlineNotFound,
                GateID = DefaultGateID,
                Integer64ID = createdFlight.Integer64ID,
                Name = "Update Flight Airline Not Found Test",
                Destination = DefaultAirportCode,
                SortDestinationID = DefaultSortDestinationID,
            });

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.AirlineID));
            Assert.Single(operationResult.ValidationErrors[nameof(Flight.AirlineID)]);
            Assert.Equal($"The {BadAirlineID} airline was not found in the data store.", operationResult.ValidationErrors[nameof(Flight.AirlineID)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the flight.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the flight destination is badly formatted when updating a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateFlightBadDestinationFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberUpdateTestBadDestination,
            GateID = DefaultGateID,
            Name = "Update Bad Destination Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Flight flight)
        {
            flight.Destination = BadFormatttedDestination;
            operationResult = await dataLayer.UpdateAsync(flight);

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.Destination));
            Assert.Single(operationResult.ValidationErrors[nameof(Flight.Destination)]);
            Assert.Equal("The city must be 3 capital letters.", operationResult.ValidationErrors[nameof(Flight.Destination)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the flight.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the flight number is badly formatted when updating a flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateFlightBadFlightNumberFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberUpdateTestBadFlightNumber,
            GateID = DefaultGateID,
            Name = "Update Bad Flight Number Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Flight flight)
        {
            flight.FlightNumber = BadFormatttedFlightNumber;
            operationResult = await dataLayer.UpdateAsync(flight);

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.FlightNumber));
            Assert.Single(operationResult.ValidationErrors[nameof(Flight.FlightNumber)]);
            Assert.Equal("The flight number must be 4 digits or 4 digits and a capital letter.", operationResult.ValidationErrors[nameof(Flight.FlightNumber)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the flight.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the codeshare airline ID doesn't exist when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateFlightCodeShareAirlineNotFoundFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberUpdateTestCodeShareAirlineNotFound,
            GateID = DefaultGateID,
            Name = "Update Flight CodeShare Airline Not Found Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Flight createdFlight)
        {
            operationResult = await dataLayer.CreateAsync(new Flight()
            {
                AirlineID = DefaultAirlineID,
                CodeShares = [new CodeShare() { AirlineID = BadAirlineID, FlightNumber = FlightNumberAddTestDuplicate }],
                DepartTime = DateTime.Now.TimeOfDay,
                FlightNumber = FlightNumberUpdateTestCodeShareAirlineNotFound,
                GateID = DefaultGateID,
                Integer64ID = createdFlight.Integer64ID,
                Name = "Update Flight CodeShare Airline Not Found Test",
                Destination = DefaultAirportCode,
                SortDestinationID = DefaultSortDestinationID,
            });

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.AirlineID));
            Assert.Single(operationResult.ValidationErrors[nameof(Flight.AirlineID)]);
            Assert.Equal($"The {BadAirlineID} airline for the codeshare was not found in the data store.", operationResult.ValidationErrors[nameof(Flight.AirlineID)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the flight.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the gate ID doesn't exist when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateFlightGateNotFoundFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberUpdateTestGateNotFound,
            GateID = DefaultGateID,
            Name = "Update Flight Gate Not Found Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Flight createdFlight)
        {
            operationResult = await dataLayer.CreateAsync(new Flight()
            {
                AirlineID = DefaultAirlineID,
                DepartTime = DateTime.Now.TimeOfDay,
                FlightNumber = FlightNumberUpdateTestGateNotFound,
                GateID = BadGateID,
                Integer64ID = createdFlight.Integer64ID,
                Name = "Update Flight Gate Not Found Test",
                Destination = DefaultAirportCode,
                SortDestinationID = DefaultSortDestinationID,
            });

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.GateID));
            Assert.Single(operationResult.ValidationErrors[nameof(Flight.GateID)]);
            Assert.Equal($"The {BadGateID} gate was not found in the data store.", operationResult.ValidationErrors[nameof(Flight.GateID)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the flight.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the sort destination ID doesn't exist when adding a new flight.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateFlightSortDestinationNotFoundFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        FlightDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Flight()
        {
            AirlineID = DefaultAirlineID,
            DepartTime = DateTime.Now.TimeOfDay,
            FlightNumber = FlightNumberUpdateTestSortDestinationNotFound,
            GateID = DefaultGateID,
            Name = "Update Flight Sort Destination Not Found Test",
            Destination = DefaultAirportCode,
            SortDestinationID = DefaultSortDestinationID,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Flight createdFlight)
        {
            operationResult = await dataLayer.CreateAsync(new Flight()
            {
                AirlineID = DefaultAirlineID,
                DepartTime = DateTime.Now.TimeOfDay,
                FlightNumber = FlightNumberUpdateTestSortDestinationNotFound,
                GateID = DefaultGateID,
                Integer64ID = createdFlight.Integer64ID,
                Name = "Update Flight Sort Destination Not Found Test",
                Destination = DefaultAirportCode,
                SortDestinationID = BadSortDestinationID,
            });

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Flight.SortDestinationID));
            Assert.Single(operationResult.ValidationErrors[nameof(Flight.SortDestinationID)]);
            Assert.Equal($"The {BadSortDestinationID} sort destination was not found in the data store.", operationResult.ValidationErrors[nameof(Flight.SortDestinationID)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the flight.");
        }
    }
}
