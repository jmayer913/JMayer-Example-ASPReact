using JMayer.Example.ASPReact.Server.Airlines;
using JMayer.Example.ASPReact.Server.Flights;
using JMayer.Example.ASPReact.Server.Gates;
using JMayer.Example.ASPReact.Server.SortDestinations;

namespace JMayer.Example.ASPReact.Server;

/// <summary>
/// The class is used to generate example data for a flight schedule.
/// </summary>
public class FlightScheduleExampleBuilder
{
    /// <summary>
    /// A list of airport codes.
    /// </summary>
    private readonly List<string> _airportCodes = [];

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
    /// The property gets/sets the data layer used to interact with the sort destinations.
    /// </summary>
    public ISortDestinationDataLayer SortDestinationDataLayer { get; init; } = new SortDestinationDataLayer();

    /// <summary>
    /// The default constructor.
    /// </summary>
    public FlightScheduleExampleBuilder()
    {
        FlightDataLayer = new FlightDataLayer(AirlineDataLayer, GateDataLayer, SortDestinationDataLayer);
        GenerateAirportCodes();
    }

    /// <summary>
    /// The method builds the flight schedule example data.
    /// </summary>
    public void Build()
    {
        BuildAirlines();
        BuildGates();
        BuildSortDestinations();
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
        List<SortDestination> sortDestinations = SortDestinationDataLayer.GetAllAsync().Result;

        int flightNumber = 1000;
        int gateIndex = 0;
        int sortDestinationIndex = 0;
        TimeSpan departTime = new(4, 0, 0);
        TimeSpan operationalEnd = new(22, 0, 0);

        while (departTime < operationalEnd)
        {
            foreach (Airline airline in airlines)
            {
                _ = FlightDataLayer.CreateAsync(new Flight()
                {
                    AirlineIATACode = airline.IATA,
                    AirlineID = airline.Integer64ID,
                    CreatedOn = DateTime.Now,
                    Destination = _airportCodes[new Random(DateTime.Now.Millisecond).Next(0, _airportCodes.Count - 1)],
                    DepartTime = departTime,
                    FlightNumber = flightNumber.ToString().PadLeft(4, '0'),
                    GateID = gates[gateIndex].Integer64ID,
                    GateName = gates[gateIndex].Name ?? string.Empty,
                    Name = $"{airline.IATA}{flightNumber.ToString().PadLeft(4, '0')}",
                    SortDestinationID = sortDestinations[sortDestinationIndex].Integer64ID,
                    SortDestinationName = sortDestinations[sortDestinationIndex].Name ?? string.Empty,
                });

                flightNumber++;
                gateIndex++;
                sortDestinationIndex++;
                departTime = departTime.Add(TimeSpan.FromMinutes(10));

                if (gateIndex > gates.Count - 1)
                {
                    gateIndex = 0;
                }

                if (sortDestinationIndex > sortDestinations.Count - 1)
                {
                    sortDestinationIndex = 0;
                }
            }
        }
    }

    /// <summary>
    /// The method builds the gates.
    /// </summary>
    private void BuildGates()
    {
        _ = GateDataLayer.CreateAsync(new Gate()
        {
            Name = "A1",
        });
        _ = GateDataLayer.CreateAsync(new Gate()
        {
            Name = "A2",
        });

        _ = GateDataLayer.CreateAsync(new Gate()
        {
            Name = "B1",
        });
        _ = GateDataLayer.CreateAsync(new Gate()
        {
            Name = "B2",
        });

        _ = GateDataLayer.CreateAsync(new Gate()
        {
            Name = "C1",
        });
        _ = GateDataLayer.CreateAsync(new Gate()
        {
            Name = "C2",
        });
    }

    /// <summary>
    /// The method builds the sort destinations.
    /// </summary>
    private void BuildSortDestinations()
    {
        _ = SortDestinationDataLayer.CreateAsync(new SortDestination()
        {
            Name = "MU1",
        });
        _ = SortDestinationDataLayer.CreateAsync(new SortDestination()
        {
            Name = "MU2",
        });
        _ = SortDestinationDataLayer.CreateAsync(new SortDestination()
        {
            Name = "MU3",
        });
        _ = SortDestinationDataLayer.CreateAsync(new SortDestination()
        {
            Name = "MU4",
        });
    }

    /// <summary>
    /// The method generates all the airport code combinations.
    /// </summary>
    private void GenerateAirportCodes()
    {
        int capitalA = 'A';

        for (int firstChar = 0; firstChar < 26; firstChar++)
        {
            for (int secondChar = 0; secondChar < 26; secondChar++)
            {
                for (int thirdChar = 0; thirdChar < 26; thirdChar++)
                {
                    string airportCode = ((char)(firstChar + capitalA)).ToString() + ((char)(secondChar + capitalA)).ToString() + ((char)(thirdChar + capitalA)).ToString();
                    _airportCodes.Add(airportCode);
                }
            }
        }
    }
}
