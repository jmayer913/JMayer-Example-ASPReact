using JMayer.Example.ASPReact.Server.Airlines;

namespace JMayer.Example.ASPReact.Server;

/// <summary>
/// The class is used to generate example data for a flight schedule.
/// </summary>
public class FlightScheduleExampleBuilder
{
    /// <summary>
    /// The property gets/sets the data layer used to interact with airlines.
    /// </summary>
    public IAirlineDataLayer AirlineDataLayer { get; set; } = new AirlineDataLayer();

    /// <summary>
    /// The method builds the flight schedule example data.
    /// </summary>
    public void Build()
    {
        BuildAirlines();
    }

    /// <summary>
    /// The method builds the airlines for the flight schedule.
    /// </summary>
    private void BuildAirlines()
    {
        _ = AirlineDataLayer.CreateAsync(new Airline()
        {
            IATA = "AA",
            ICAO = "AAL",
            Name = "American Airlines",
            NumberCode = "001",
        });
        _ = AirlineDataLayer.CreateAsync(new Airline()
        {
            IATA = "DL",
            ICAO = "DAL",
            Name = "Delta Air Lines",
            NumberCode = "006",
        });
        _ = AirlineDataLayer.CreateAsync(new Airline()
        {
            IATA = "WN",
            ICAO = "SWA",
            Name = "Southwest Airlines",
            NumberCode = "526",
        });
    }
}
