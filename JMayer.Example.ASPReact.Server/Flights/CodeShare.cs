namespace JMayer.Example.ASPReact.Server.Flights;

/// <summary>
/// The class represents a codeshare for a flight.
/// </summary>
/// <remarks>
/// A codeshare means the flight is coshared with other airlines but advertised using their flight number.
/// </remarks>
public class CodeShare
{
    /// <summary>
    /// The property gets/sets the 
    /// </summary>
    public int AirlineID { get; set; }

    /// <summary>
    /// The property gets/sets the flight number for the codeshare.
    /// </summary>
    public string FlightNumber { get; set; } = string.Empty;

    /// <summary>
    /// The default constructor.
    /// </summary>
    public CodeShare() { }

    /// <summary>
    /// The copy constructor.
    /// </summary>
    /// <param name="copy">The copy.</param>
    public CodeShare(CodeShare copy)
    {
        AirlineID = copy.AirlineID;
        FlightNumber = copy.FlightNumber;
    }
}
