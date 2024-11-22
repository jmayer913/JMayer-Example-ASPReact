using JMayer.Example.ASPReact.Server.Flights;

namespace TestProject.Flights;

/// <summary>
/// The class manages testing the codeshare equality comparer.
/// </summary>
public class CodeShareEqualityComparerUnitTest
{
    /// <summary>
    /// The constant for the airline ID.
    /// </summary>
    public const long AirlineID = 1;

    /// <summary>
    /// The constant for the flight number.
    /// </summary>
    public const string FlightNumber = "1234B";

    /// <summary>
    /// The method verifies equality failure when the AirlineID property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureAirlineID()
    {
        CodeShare codeshare1 = new()
        {
            AirlineID = AirlineID,
        };
        CodeShare codeshare2 = new();

        Assert.False(new CodeShareEqualityComparer().Equals(codeshare1, codeshare2));
    }

    /// <summary>
    /// The method verifies equality failure when two nulls are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureBothNull() => Assert.False(new CodeShareEqualityComparer().Equals(null, null));

    /// <summary>
    /// The method verifies equality failure when the FlightNumber property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureFlightNumber()
    {
        CodeShare codeshare1 = new()
        {
            FlightNumber = FlightNumber,
        };
        CodeShare codeshare2 = new();

        Assert.False(new CodeShareEqualityComparer().Equals(codeshare1, codeshare2));
    }

    /// <summary>
    /// The method verifies equality failure when an object and null are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureOneIsNull()
    {
        CodeShare codeshare = new()
        {
            AirlineID = AirlineID,
            FlightNumber = FlightNumber,
        };

        Assert.False(new CodeShareEqualityComparer().Equals(codeshare, null));
        Assert.False(new CodeShareEqualityComparer().Equals(null, codeshare));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared.
    /// </summary>
    [Fact]
    public void VerifySuccess()
    {
        CodeShare codeshare1 = new()
        {
            AirlineID = AirlineID,
            FlightNumber = FlightNumber,
        };
        CodeShare codeshare2 = new(codeshare1);

        Assert.True(new CodeShareEqualityComparer().Equals(codeshare1, codeshare2));
    }
}
