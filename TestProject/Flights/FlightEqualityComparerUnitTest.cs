using JMayer.Example.ASPReact.Server.Flights;

namespace TestProject.Flights;

/// <summary>
/// The class manages testing the flight equality comparer.
/// </summary>
public class FlightEqualityComparerUnitTest
{
    /// <summary>
    /// The constant for the airline ID.
    /// </summary>
    public long AirlineID = 1;

    /// <summary>
    /// The constant for the description.
    /// </summary>
    private const string Description = "A Description";

    /// <summary>
    /// The constant for the next destination.
    /// </summary>
    private const string Destination = "AAA";

    /// <summary>
    /// The constant for the flight number.
    /// </summary>
    private const string FlightNumber = "1234A";

    /// <summary>
    /// The constant for the gate ID.
    /// </summary>
    private const long GateID = 2;

    /// <summary>
    /// The constant for the ID.
    /// </summary>
    private const long ID = 1;

    /// <summary>
    /// The constant for the name.
    /// </summary>
    private const string Name = "A Name";

    /// <summary>
    /// The method verifies equality failure when the AirlineID property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureAirlineID()
    {
        Flight flight1 = new()
        {
            AirlineID = AirlineID,
        };
        Flight flight2 = new();

        Assert.False(new FlightEqualityComparer().Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality failure when two nulls are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureBothNull() => Assert.False(new FlightEqualityComparer().Equals(null, null));

    /// <summary>
    /// The method verifies equality failure when the CodeShares property count is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureCodeSharesCount()
    {
        Flight flight1 = new()
        {
            CodeShares = [new CodeShare() { AirlineID = AirlineID, FlightNumber = FlightNumber, }],
        };
        Flight flight2 = new();

        Assert.False(new FlightEqualityComparer().Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality failure when the DepartTime property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureDepartTime()
    {
        Flight flight1 = new()
        {
            DepartTime = DateTime.Now.TimeOfDay,
        };
        Flight flight2 = new();

        Assert.False(new FlightEqualityComparer().Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality failure when the Description property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureDescription()
    {
        Flight flight1 = new()
        {
            Description = Description,
        };
        Flight flight2 = new();

        Assert.False(new FlightEqualityComparer().Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality failure when the Destintion property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureDestination()
    {
        Flight flight1 = new()
        {
            Destination = Destination,
        };
        Flight flight2 = new();

        Assert.False(new FlightEqualityComparer().Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality failure when the FlightNumber property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureFlightNumber()
    {
        Flight flight1 = new()
        {
            FlightNumber = FlightNumber,
        };
        Flight flight2 = new();

        Assert.False(new FlightEqualityComparer().Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality failure when the GateID property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureGateID()
    {
        Flight flight1 = new()
        {
            GateID = GateID,
        };
        Flight flight2 = new();

        Assert.False(new FlightEqualityComparer().Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality failure when the ID property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureID()
    {
        Flight flight1 = new()
        {
            Integer64ID = ID,
        };
        Flight flight2 = new();

        Assert.False(new FlightEqualityComparer().Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality failure when the Name property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureName()
    {
        Flight flight1 = new()
        {
            Name = Name,
        };
        Flight flight2 = new();

        Assert.False(new FlightEqualityComparer().Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality failure when an object and null are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureOneIsNull()
    {
        Flight flight = new()
        {
            AirlineID = AirlineID,
            CodeShares = [new CodeShare() { AirlineID = AirlineID, FlightNumber = FlightNumber, }],
            CreatedOn = DateTime.Now,
            DepartTime = DateTime.Now.TimeOfDay,
            Description = Description,
            Destination = Destination,
            FlightNumber = FlightNumber,
            GateID = GateID,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };

        Assert.False(new FlightEqualityComparer().Equals(flight, null));
        Assert.False(new FlightEqualityComparer().Equals(null, flight));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared.
    /// </summary>
    [Fact]
    public void VerifySuccess()
    {
        Flight flight1 = new()
        {
            AirlineID = AirlineID,
            CodeShares = [new CodeShare() { AirlineID = AirlineID, FlightNumber = FlightNumber, }],
            CreatedOn = DateTime.Now,
            DepartTime = DateTime.Now.TimeOfDay,
            Description = Description,
            Destination = Destination,
            FlightNumber = FlightNumber,
            GateID = GateID,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        Flight flight2 = new(flight1);

        Assert.True(new FlightEqualityComparer().Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the LastEditedOn property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeCreatedOn()
    {
        Flight flight1 = new()
        {
            AirlineID = AirlineID,
            CodeShares = [new CodeShare() { AirlineID = AirlineID, FlightNumber = FlightNumber, }],
            CreatedOn = DateTime.Now,
            DepartTime = DateTime.Now.TimeOfDay,
            Description = Description,
            Destination = Destination,
            FlightNumber = FlightNumber,
            GateID = GateID,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        Flight flight2 = new(flight1)
        {
            CreatedOn = DateTime.MinValue,
        };

        Assert.True(new FlightEqualityComparer(true, false, false).Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the Integer64ID property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeID()
    {
        Flight flight1 = new()
        {
            AirlineID = AirlineID,
            CodeShares = [new CodeShare() { AirlineID = AirlineID, FlightNumber = FlightNumber, }],
            CreatedOn = DateTime.Now,
            DepartTime = DateTime.Now.TimeOfDay,
            Description = Description,
            Destination = Destination,
            FlightNumber = FlightNumber,
            GateID = GateID,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        Flight flight2 = new(flight1)
        {
            Integer64ID = ID + 1,
        };

        Assert.True(new FlightEqualityComparer(false, true, false).Equals(flight1, flight2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the LastEditedOn property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeLastEditedOn()
    {
        Flight flight1 = new()
        {
            AirlineID = AirlineID,
            CodeShares = [new CodeShare() { AirlineID = AirlineID, FlightNumber = FlightNumber, }],
            CreatedOn = DateTime.Now,
            DepartTime = DateTime.Now.TimeOfDay,
            Description = Description,
            Destination = Destination,
            FlightNumber = FlightNumber,
            GateID = GateID,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        Flight flight2 = new(flight1)
        {
            LastEditedOn = DateTime.MinValue,
        };

        Assert.True(new FlightEqualityComparer(false, false, true).Equals(flight1, flight2));
    }
}
