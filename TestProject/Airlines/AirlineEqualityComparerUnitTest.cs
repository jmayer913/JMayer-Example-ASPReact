using JMayer.Example.ASPReact.Server.Airlines;

namespace TestProject.Airlines;

/// <summary>
/// The class manages testing the airline equality comparer.
/// </summary>
public class AirlineEqualityComparerUnitTest
{
    /// <summary>
    /// The constant for the description.
    /// </summary>
    private const string Description = "A Description";

    /// <summary>
    /// The constant for the IATA.
    /// </summary>
    private const string IATA = "ZZ";

    /// <summary>
    /// The constant for the ICAO.
    /// </summary>
    private const string ICAO = "ZZZ";

    /// <summary>
    /// The constant for the ID.
    /// </summary>
    private const long ID = 1;

    /// <summary>
    /// The constant for the name.
    /// </summary>
    private const string Name = "A Name";

    /// <summary>
    /// The constant for the number code.
    /// </summary>
    private const string NumberCode = "999";

    /// <summary>
    /// The constant for the sort destination ID.
    /// </summary>
    private const long SortDestinationID = 2;

    /// <summary>
    /// The method verifies equality failure when two nulls are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureBothNull() => Assert.False(new AirlineEqualityComparer().Equals(null, null));

    /// <summary>
    /// The method verifies equality failure when the Description property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureDescription()
    {
        Airline airline1 = new()
        {
            Description = Description,
        };
        Airline airline2 = new();

        Assert.False(new AirlineEqualityComparer().Equals(airline1, airline2));
    }

    /// <summary>
    /// The method verifies equality failure when the IATA property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureIATA()
    {
        Airline airline1 = new()
        {
            IATA = IATA,
        };
        Airline airline2 = new();

        Assert.False(new AirlineEqualityComparer().Equals(airline1, airline2));
    }

    /// <summary>
    /// The method verifies equality failure when the ICAO property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureICAO()
    {
        Airline airline1 = new()
        {
            ICAO = ICAO,
        };
        Airline airline2 = new();

        Assert.False(new AirlineEqualityComparer().Equals(airline1, airline2));
    }

    /// <summary>
    /// The method verifies equality failure when the ID property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureID()
    {
        Airline airline1 = new()
        {
            Integer64ID = ID,
        };
        Airline airline2 = new();

        Assert.False(new AirlineEqualityComparer().Equals(airline1, airline2));
    }

    /// <summary>
    /// The method verifies equality failure when the Name property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureName()
    {
        Airline airline1 = new()
        {
            Name = Name,
        };
        Airline airline2 = new();

        Assert.False(new AirlineEqualityComparer().Equals(airline1, airline2));
    }

    /// <summary>
    /// The method verifies equality failure when the NumberCode property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureNumberCode()
    {
        Airline airline1 = new()
        {
            NumberCode = NumberCode,
        };
        Airline airline2 = new();

        Assert.False(new AirlineEqualityComparer().Equals(airline1, airline2));
    }

    /// <summary>
    /// The method verifies equality failure when an object and null are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureOneIsNull()
    {
        Airline airline = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            IATA = IATA,
            ICAO = ICAO,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
            NumberCode = NumberCode,
            SortDestinationID = SortDestinationID,
        };

        Assert.False(new AirlineEqualityComparer().Equals(airline, null));
        Assert.False(new AirlineEqualityComparer().Equals(null, airline));
    }

    /// <summary>
    /// The method verifies equality failure when the SortDestinationID property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureSortDestinationID()
    {
        Airline airline1 = new()
        {
            SortDestinationID = SortDestinationID,
        };
        Airline airline2 = new();

        Assert.False(new AirlineEqualityComparer().Equals(airline1, airline2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared.
    /// </summary>
    [Fact]
    public void VerifySuccess()
    {
        Airline airline1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            IATA = IATA,
            ICAO = ICAO,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
            NumberCode = NumberCode,
            SortDestinationID = SortDestinationID,
        };
        Airline airline2 = new(airline1);

        Assert.True(new AirlineEqualityComparer().Equals(airline1, airline2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the LastEditedOn property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeCreatedOn()
    {
        Airline airline1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            IATA = IATA,
            ICAO = ICAO,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
            NumberCode = NumberCode,
            SortDestinationID = SortDestinationID,
        };
        Airline airline2 = new(airline1)
        {
            CreatedOn = DateTime.MinValue,
        };

        Assert.True(new AirlineEqualityComparer(true, false, false).Equals(airline1, airline2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the Integer64ID property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeID()
    {
        Airline airline1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            IATA = IATA,
            ICAO = ICAO,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
            NumberCode = NumberCode,
            SortDestinationID = SortDestinationID,
        };
        Airline airline2 = new(airline1)
        {
            Integer64ID = ID + 1,
        };

        Assert.True(new AirlineEqualityComparer(false, true, false).Equals(airline1, airline2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the LastEditedOn property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeLastEditedOn()
    {
        Airline airline1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            IATA = IATA,
            ICAO = ICAO,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
            NumberCode = NumberCode,
            SortDestinationID = SortDestinationID,
        };
        Airline airline2 = new(airline1)
        {
            LastEditedOn = DateTime.MinValue,
        };

        Assert.True(new AirlineEqualityComparer(false, false, true).Equals(airline1, airline2));
    }
}
