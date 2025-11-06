using JMayer.Data.Data;
using JMayer.Data.HTTP.DataLayer;
using JMayer.Example.ASPReact.Server.Airlines;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace TestProject.Airlines;

/// <summary>
/// The class manages tests for airlines using both the http client and server.
/// </summary>
/// <remarks>
/// The example web server creates default data objects and the unit tests
/// uses this already existing data.
/// </remarks>
public class AirlineWebRequestUnitTest : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// The factory for the web application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// The constant for a badly formatted IATA code.
    /// </summary>
    private const string BadFormattedIATACode = "ZZZ";

    /// <summary>
    /// The constant for a badly formatted ICAO code.
    /// </summary>
    private const string BadFormattedICAOCode = "ZZZZ";

    /// <summary>
    /// The constant for a badly formatted number code.
    /// </summary>
    private const string BadFormattedNumberCode = "9999";

    /// <summary>
    /// The constant for a default IATA code.
    /// </summary>
    private const string DefaultIATACode = "ZZ";

    /// <summary>
    /// The constant for a default ICAO code.
    /// </summary>
    private const string DefaultICAOCode = "ZZZ";

    /// <summary>
    /// The dependency injection constructor.
    /// </summary>
    /// <param name="factory">The factory for the web application.</param>
    public AirlineWebRequestUnitTest(WebApplicationFactory<Program> factory) => _factory = factory;

    /// <summary>
    /// The method verifies the HTTP data layer can request an airline to be created by the server and the server can successfully process the request.
    /// </summary>
    /// <param name="name">The name of the airline.</param>
    /// <param name="description">The description for the airline.</param>
    /// <param name="iata">The IATA code assigned to the airline by the IATA Organization.</param>
    /// <param name="icao">The ICAO code assigned to the airline by the International Aviation Organization.</param>
    /// <param name="numberCode">The number code assigned to the airline by the IATA Organization.</param>
    /// <returns>A Task object for the async.</returns>
    [Theory]
    [InlineData("Alaska Airlines", "Alaska", "AS", "ASA", "027")]
    [InlineData("Allegiant Air", "Allegiant", "G4", "AAY", "268")]
    [InlineData("Jetblue Airways", "Jetblue", "B6", "JBU", "279")]
    public async Task VerifyAddAirline(string name, string description, string iata, string icao, string numberCode)
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        Airline airline = new()
        {
            Description = description,
            IATA = iata,
            ICAO = icao,
            Name = name,
            NumberCode = numberCode,
        };
        OperationResult operationResult = await dataLayer.CreateAsync(airline);

        Assert.True(operationResult.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.
        Assert.IsType<Airline>(operationResult.DataObject); //An airline must have been returned.
        Assert.True(new AirlineEqualityComparer(true, true, true).Equals((Airline)operationResult.DataObject, airline), "The data object sent should be the same as the data object returned."); //Original and return must be equal.
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline IATA code is not properly formatted when adding a new airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddAirlineBadIATAFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = BadFormattedIATACode,
            ICAO = DefaultICAOCode,
            NumberCode = Airline.ZeroNumberCode,
            Name = "Add Bad IATA Code Test",
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.IATA));
        Assert.Single(operationResult.ValidationErrors[nameof(Airline.IATA)]);
        Assert.Equal("The IATA must be 2 alphanumeric characters; letters must be capitalized.", operationResult.ValidationErrors[nameof(Airline.IATA)][0]);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline ICAO code is not properly formatted when adding a new airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddAirlineBadICAOFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = DefaultIATACode,
            ICAO = BadFormattedICAOCode,
            NumberCode = Airline.ZeroNumberCode,
            Name = "Add Bad ICAO Code Test",
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.ICAO));
        Assert.Single(operationResult.ValidationErrors[nameof(Airline.ICAO)]);
        Assert.Equal("The ICAO must be 3 capital letters.", operationResult.ValidationErrors[nameof(Airline.ICAO)][0]);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline number code is not properly formatted when adding a new airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddAirlineBadNumberCodeFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = DefaultIATACode,
            ICAO = DefaultICAOCode,
            NumberCode = BadFormattedNumberCode,
            Name = "Add Bad Number Code Test",
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.NumberCode));
        Assert.Single(operationResult.ValidationErrors[nameof(Airline.NumberCode)]);
        Assert.Equal("The number code must be 3 digits.", operationResult.ValidationErrors[nameof(Airline.NumberCode)][0]);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline ICAO already exists when adding a new airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddDuplicateAirlineICAOFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZC",
            ICAO = "ZZC",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Add Duplicate ICAO Test 1",
        });
        Assert.True(operationResult.IsSuccessStatusCode, "Failed to create the first airline.");

        operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZC",
            ICAO = "ZZC",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Add Duplicate ICAO Test New",
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.ICAO));
        Assert.Single(operationResult.ValidationErrors[nameof(Airline.ICAO)]);
        Assert.Equal("The ICAO must be unique.", operationResult.ValidationErrors[nameof(Airline.ICAO)][0]);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline name already exists when adding a new airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddDuplicateAirlineNameFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZA",
            ICAO = "ZZA",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Add Duplicate Name Test",
        });
        Assert.True(operationResult.IsSuccessStatusCode, "Failed to create the first airline.");

        operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZB",
            ICAO = "ZZB",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Add Duplicate Name Test",
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.Name));
        Assert.Single(operationResult.ValidationErrors[nameof(Airline.Name)]);
        Assert.Equal("The Add Duplicate Name Test name already exists in the data store.", operationResult.ValidationErrors[nameof(Airline.Name)][0]);
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline number code already exists when adding a new airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyAddDuplicateAirlineNumberCodeFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZD",
            ICAO = "ZZD",
            NumberCode = "999",
            Name = "Add Duplicate Number Code Test 1",
        });
        Assert.True(operationResult.IsSuccessStatusCode, "Failed to create the first airline.");

        operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZE",
            ICAO = "ZZE",
            NumberCode = "999",
            Name = "Add Duplicate Number Code Test New",
        });

        //The operation must have failed.
        Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

        //No airline was returned.
        Assert.Null(operationResult.DataObject);

        //A bad request status was returned.
        Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

        //The correct error was returned.
        Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.NumberCode));
        Assert.Single(operationResult.ValidationErrors[nameof(Airline.NumberCode)]);
        Assert.Equal("The number code must be unique unless the code is 000.", operationResult.ValidationErrors[nameof(Airline.NumberCode)][0]);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request the count from the server and the server can successfully process the request.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task VerifyCountAirines()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        long count = await dataLayer.CountAsync();
        Assert.True(count > 0);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request an airline to be deleted by the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyDeleteAirline()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZF",
            ICAO = "ZZF",
            Name = "Delete Airline Test",
            NumberCode = Airline.ZeroNumberCode,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Airline airline)
        {
            operationResult = await dataLayer.DeleteAsync(airline);
            Assert.True(operationResult.IsSuccessStatusCode);
        }
        else
        {
            Assert.Fail("Failed to create the airline.");
        }
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request all airlines from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetAllAirlines()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        List<Airline>? airlines = await dataLayer.GetAllAsync();

        //Airlines must have been returned.
        Assert.NotNull(airlines);
        Assert.NotEmpty(airlines);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request all airlines as list views from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetAllListViewAirlines()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        List<ListView>? airlines = await dataLayer.GetAllListViewAsync();

        //List view airlines must have been returned.
        Assert.NotNull(airlines);
        Assert.NotEmpty(airlines);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request the first airline from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetSingleAirline()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        Airline? airline = await dataLayer.GetSingleAsync();
        Assert.NotNull(airline);
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request an airline from the server and the server can successfully process the request.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyGetSingleAirlineWithId()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        Airline airline = new()
        {
            IATA = "ZG",
            ICAO = "ZZG",
            Name = "Get Single Airline Test",
            NumberCode = Airline.ZeroNumberCode,
        };
        OperationResult operationResult = await dataLayer.CreateAsync(airline);

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Airline createdAirline)
        {
            Airline? verifyAirline = await dataLayer.GetSingleAsync(createdAirline.Integer64ID);
            Assert.NotNull(verifyAirline);
        }
        else
        {
            Assert.Fail("Failed to create the airline.");
        }
    }

    /// <summary>
    /// The method verifies the HTTP data layer can request an airline to be updated by the server and the server can successfully process the request.
    /// </summary>
    /// <param name="originalName">The original name of the airline.</param>
    /// <param name="newName">The new name of the airline.</param>
    /// <param name="originalDescription">The original description for the airline.</param>
    /// <param name="newDescription">The new description for the airline.</param>
    /// <param name="originalIata">The original IATA code assigned to the airline by the IATA Organization.</param>
    /// <param name="newIata">The new IATA code assigned to the airline by the IATA Organization.</param>
    /// <param name="originalIcao">The original ICAO code assigned to the airline by the International Aviation Organization.</param>
    /// <param name="newIcao">The new ICAO code assigned to the airline by the International Aviation Organization.</param>
    /// <param name="originalNumberCode">The orginal number code assigned to the airline by the IATA Organization.</param>
    /// <param name="newNumberCode">The new number code assigned to the airline by the IATA Organization.</param>
    /// <returns>A Task object for the async.</returns>
    [Theory]
    [InlineData("Air Canada", "Spirit Airlines", "Canada", "Spirit", "AC", "NK", "ACA", "NKS", "014", "487")]
    [InlineData("British Airways", "Silver Airways", "British", "Silver", "BA", "3M", "BAW", "SIL", "125", "449")]
    [InlineData("Flair Airlines", "Frontier Airlines", "Flair", "Frontier", "F8", "F9", "FLE", "FFT", "418", "422")]
    public async Task VerifyUpdateAirline(string originalName, string newName, string originalDescription, string newDescription, string originalIata, string newIata, string originalIcao, string newIcao, string originalNumberCode, string newNumberCode)
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            Description = originalDescription,
            IATA = originalIata,
            ICAO = originalIcao,
            Name = originalName,
            NumberCode = originalNumberCode,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Airline createdAirline)
        {
            Airline updatedAirline = new(createdAirline)
            {
                Description = newDescription,
                IATA = newIata,
                ICAO = newIcao,
                Name = newName,
                NumberCode = newNumberCode,
            };
            operationResult = await dataLayer.UpdateAsync(updatedAirline);

            Assert.True(operationResult.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.
            Assert.IsType<Airline>(operationResult.DataObject); //An airline must have been returned.
            Assert.True(new AirlineEqualityComparer(true, true, true).Equals((Airline)operationResult.DataObject, updatedAirline), "The data object sent should be the same as the data object returned."); //Original and return must be equal.
        }
        else
        {
            Assert.Fail("Failed to create the airline.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline IATA code is not properly formatted when updating an airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateAirlineBadIATAFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZH",
            ICAO = "ZZH",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Update Bad IATA Code Test",
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Airline airline)
        {
            airline.IATA = BadFormattedIATACode;
            operationResult = await dataLayer.UpdateAsync(airline);

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.IATA));
            Assert.Single(operationResult.ValidationErrors[nameof(Airline.IATA)]);
            Assert.Equal("The IATA must be 2 alphanumeric characters; letters must be capitalized.", operationResult.ValidationErrors[nameof(Airline.IATA)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the airline.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline ICAO code is not properly formatted when updating an airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateAirlineBadICAOFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZI",
            ICAO = "ZZI",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Update Bad ICAO Code Test",
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Airline airline)
        {
            airline.ICAO = BadFormattedICAOCode;
            operationResult = await dataLayer.UpdateAsync(airline);

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.ICAO));
            Assert.Single(operationResult.ValidationErrors[nameof(Airline.ICAO)]);
            Assert.Equal("The ICAO must be 3 capital letters.", operationResult.ValidationErrors[nameof(Airline.ICAO)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the airline.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline number code is not properly formatted when updating an airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateAirlineBadNumberCodeFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZJ",
            ICAO = "ZZJ",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Update Bad Number Code Test",
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Airline airline)
        {
            airline.NumberCode = BadFormattedNumberCode;
            operationResult = await dataLayer.UpdateAsync(airline);

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.NumberCode));
            Assert.Single(operationResult.ValidationErrors[nameof(Airline.NumberCode)]);
            Assert.Equal("The number code must be 3 digits.", operationResult.ValidationErrors[nameof(Airline.NumberCode)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the airline.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline being updated is old.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateAirlineOldDataConflict()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZK",
            ICAO = "ZZK",
            Name = "Old Data Airline Test",
            NumberCode = Airline.ZeroNumberCode,
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Airline firstAirline)
        {
            Airline secondAirline = new(firstAirline);

            firstAirline.Description = "A description";

            operationResult = await dataLayer.UpdateAsync(secondAirline);

            if (!operationResult.IsSuccessStatusCode)
            {
                Assert.Fail("Failed to update the second airline.");
                return;
            }

            operationResult = await dataLayer.UpdateAsync(firstAirline);

            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed."); //The operation must have failed.
            Assert.Null(operationResult.DataObject); //No airline was returned.
            Assert.Equal(HttpStatusCode.Conflict, operationResult.StatusCode); //A conflict status was returned.
        }
        else
        {
            Assert.Fail("Failed to create the first airline.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline ICAO already exists when adding updating an airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateDuplicateAirlineICAOFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZM",
            ICAO = "ZZM",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Update Duplicate ICAO Test 1",
        });
        Assert.True(operationResult.IsSuccessStatusCode, "Failed to create the first airline.");

        operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZN",
            ICAO = "ZZN",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Update Duplicate ICAO Test 2",
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Airline airline)
        {
            airline.ICAO = "ZZM";
            operationResult = await dataLayer.UpdateAsync(airline);

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.ICAO));
            Assert.Single(operationResult.ValidationErrors[nameof(Airline.ICAO)]);
            Assert.Equal("The ICAO must be unique.", operationResult.ValidationErrors[nameof(Airline.ICAO)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the second airline.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline name already exists when adding updating an airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateDuplicateAirlineNameFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZO",
            ICAO = "ZZO",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Update Duplicate Name Test 1",
        });
        Assert.True(operationResult.IsSuccessStatusCode, "Failed to create the first airline.");

        operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZP",
            ICAO = "ZZP",
            NumberCode = Airline.ZeroNumberCode,
            Name = "Update Duplicate Name Test 2",
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Airline airline)
        {
            airline.Name = "Update Duplicate Name Test 1";
            operationResult = await dataLayer.UpdateAsync(airline);

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.Name));
            Assert.Single(operationResult.ValidationErrors[nameof(Airline.Name)]);
            Assert.Equal($"The {airline.Name} name already exists in the data store.", operationResult.ValidationErrors[nameof(Airline.Name)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the second airline.");
        }
    }

    /// <summary>
    /// The method verifies the server will return a failure if the airline number code already exists when updating an airline.
    /// </summary>
    /// <returns>A Task object for the async.</returns>
    [Fact]
    public async Task VerifyUpdateDuplicateAirlineNumberCodeFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        AirlineDataLayer dataLayer = new(httpClient);

        OperationResult operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZQ",
            ICAO = "ZZQ",
            NumberCode = "997",
            Name = "Update Duplicate Number Code Test Test 1",
        });
        Assert.True(operationResult.IsSuccessStatusCode, "Failed to create the first airline.");

        operationResult = await dataLayer.CreateAsync(new Airline()
        {
            IATA = "ZR",
            ICAO = "ZZR",
            NumberCode = "998",
            Name = "Update Duplicate Number Code Test 2",
        });

        if (operationResult.IsSuccessStatusCode && operationResult.DataObject is Airline airline)
        {
            airline.NumberCode = "997";
            operationResult = await dataLayer.UpdateAsync(airline);

            //The operation must have failed.
            Assert.False(operationResult.IsSuccessStatusCode, "The operation should have failed.");

            //No airline was returned.
            Assert.Null(operationResult.DataObject);

            //A bad request status was returned.
            Assert.Equal(HttpStatusCode.BadRequest, operationResult.StatusCode);

            //The correct error was returned.
            Assert.Contains(operationResult.ValidationErrors, obj => obj.Key == nameof(Airline.NumberCode));
            Assert.Single(operationResult.ValidationErrors[nameof(Airline.NumberCode)]);
            Assert.Equal("The number code must be unique unless the code is 000.", operationResult.ValidationErrors[nameof(Airline.NumberCode)][0]);
        }
        else
        {
            Assert.Fail("Failed to create the second airline.");
        }
    }
}
