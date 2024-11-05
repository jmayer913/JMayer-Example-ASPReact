using JMayer.Example.ASPReact.Server.Airlines;
using JMayer.Example.ASPReact.Server.Flights;
using JMayer.Example.ASPReact.Server.Gates;

namespace JMayer.Example.ASPReact.Server;

/// <summary>
/// The class is used to generate example data for a flight schedule.
/// </summary>
public class FlightScheduleExampleBuilder
{
    /// <summary>
    /// The property gets/sets the data layer used to interact with airlines.
    /// </summary>
    public IAirlineDataLayer AirlineDataLayer { get; init; } = new AirlineDataLayer();

    /// <summary>
    /// The property gets/sets the data layer used to ineract with the flights.
    /// </summary>
    public IFlightDataLayer FlightDataLayer { get; init; }

    /// <summary>
    /// The property gets/sets the data layer used to interact with the gates.
    /// </summary>
    public IGateDataLayer GateDataLayer { get; init; } = new GateDataLayer();

    /// <summary>
    /// The default constructor.
    /// </summary>
    public FlightScheduleExampleBuilder()
    {
        FlightDataLayer = new FlightDataLayer(AirlineDataLayer, GateDataLayer);
    }

    /// <summary>
    /// The method builds the flight schedule example data.
    /// </summary>
    public void Build()
    {
        BuildAirlines();
        BuildGates();
        BuildFlights();
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

    /// <summary>
    /// The method builds the flights for the flight schedule.
    /// </summary>
    private void BuildFlights()
    {
        List<Airline> airlines = AirlineDataLayer.GetAllAsync().Result;
        List<Gate> gates = GateDataLayer.GetAllAsync().Result;

        int flightNumber = 1000;
        TimeSpan departTime = new(4, 0, 0);
        TimeSpan operationalEnd = new(22, 0, 0);

        while (departTime < operationalEnd)
        {
            foreach (Airline airline in airlines)
            {
                List<Gate> airlineGates = gates.Where(obj => obj.AirlineID == airline.Integer64ID).ToList();

                foreach (Gate gate in airlineGates)
                {
                    _ = FlightDataLayer.CreateAsync(new Flight()
                    {
                        AirlineID = airline.Integer64ID,
                        CreatedOn = DateTime.Now,
                        DepartTime = departTime,
                        FlightNumber = flightNumber.ToString().PadLeft(4, '0'),
                        GateID = gate.Integer64ID,
                        Name = $"{airline.IATA}{flightNumber.ToString().PadLeft(4, '0')}",
                        NextDestination = "ZZZ",
                    });

                    flightNumber++;
                    departTime = departTime.Add(TimeSpan.FromMinutes(10));
                }
            }
        }
    }

    /// <summary>
    /// The method builds the gates.
    /// </summary>
    private void BuildGates()
    {
        List<Airline> airlines = AirlineDataLayer.GetAllAsync().Result;

        _ = GateDataLayer.CreateAsync(new Gate()
        {
            AirlineID = airlines[0].Integer64ID,
            Name = "A1",
        });
        _ = GateDataLayer.CreateAsync(new Gate()
        {
            AirlineID = airlines[0].Integer64ID,
            Name = "A2",
        });

        _ = GateDataLayer.CreateAsync(new Gate()
        {
            AirlineID = airlines[1].Integer64ID,
            Name = "B1",
        });
        _ = GateDataLayer.CreateAsync(new Gate()
        {
            AirlineID = airlines[1].Integer64ID,
            Name = "B2",
        });

        _ = GateDataLayer.CreateAsync(new Gate()
        {
            AirlineID = airlines[2].Integer64ID,
            Name = "C1",
        });
        _ = GateDataLayer.CreateAsync(new Gate()
        {
            AirlineID = airlines[2].Integer64ID,
            Name = "C2",
        });
    }
}
