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
    public IAirlineDataLayer AirlineDataLayer { get; init; }

    /// <summary>
    /// The constant for the American Airline IATA code.
    /// </summary>
    private const string AmericanIataCode = "AA";

    /// <summary>
    /// The constant for the Delta IATA code.
    /// </summary>
    private const string DeltaIataCode = "DL";

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
    /// The constant for the Southwest IATA code.
    /// </summary>
    private const string SouthwestIataCode = "WN";

    /// <summary>
    /// The default constructor.
    /// </summary>
    public FlightScheduleExampleBuilder()
    {
        AirlineDataLayer = new AirlineDataLayer(SortDestinationDataLayer);
        FlightDataLayer = new FlightDataLayer(AirlineDataLayer, GateDataLayer, SortDestinationDataLayer);
        GenerateAirportCodes();
    }

    /// <summary>
    /// The method builds the flight schedule example data.
    /// </summary>
    public void Build()
    {
        BuildSortDestinations();
        BuildAirlines();
        BuildGates();
        BuildFlights();
    }

    /// <summary>
    /// The method builds the airlines for the flight schedule.
    /// </summary>
    private void BuildAirlines()
    {
        List<SortDestination> sortDestinations = SortDestinationDataLayer.GetAllAsync().Result;

        _ = AirlineDataLayer.CreateAsync(new Airline()
        {
            IATA = AmericanIataCode,
            ICAO = "AAL",
            Name = "American Airlines",
            NumberCode = "001",
            SortDestinationID = sortDestinations[0].Integer64ID,
            SortDestinationName = sortDestinations[0].Name ?? string.Empty,
        });
        _ = AirlineDataLayer.CreateAsync(new Airline()
        {
            IATA = DeltaIataCode,
            ICAO = "DAL",
            Name = "Delta Air Lines",
            NumberCode = "006",
            SortDestinationID = sortDestinations[2].Integer64ID,
            SortDestinationName = sortDestinations[2].Name ?? string.Empty,
        });
        _ = AirlineDataLayer.CreateAsync(new Airline()
        {
            IATA = SouthwestIataCode,
            ICAO = "SWA",
            Name = "Southwest Airlines",
            NumberCode = "526",
            SortDestinationID = sortDestinations[4].Integer64ID,
            SortDestinationName = sortDestinations[4].Name ?? string.Empty,
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

        bool altAmericanGate = false;
        bool altAmericanSortDestination = false;

        bool altDeltaGate = false;
        bool altDeltaSortDestination = false;

        bool altSouthwestGate = false;
        bool altSouthWestSortDestination = false;

        int flightNumber = 1000;
        TimeSpan departTime = new(4, 0, 0);
        TimeSpan operationalEnd = new(22, 0, 0);

        while (departTime < operationalEnd)
        {
            foreach (Airline airline in airlines)
            {
                Gate gate;
                SortDestination? altSortDestination = null;

                //Determine which gate is used for the airline and if the alternative sort destination is used.
                if (airline.IATA is AmericanIataCode)
                {
                    gate = altAmericanGate ? gates[1] : gates[0];

                    if (altAmericanSortDestination)
                    {
                        altSortDestination = sortDestinations[1];
                    }

                    altAmericanGate = !altAmericanGate;
                    altAmericanSortDestination = !altAmericanSortDestination;
                }
                else if (airline.IATA is DeltaIataCode)
                {
                    gate = altDeltaGate ? gates[3] : gates[2];

                    if (altDeltaSortDestination)
                    {
                        altSortDestination = sortDestinations[3];
                    }

                    altDeltaGate = !altDeltaGate;
                    altDeltaSortDestination = !altDeltaSortDestination;
                }
                else
                {
                    gate = altSouthwestGate ? gates[5] : gates[4];

                    if (altSouthWestSortDestination)
                    {
                        altSortDestination = sortDestinations[5];
                    }

                    altSouthwestGate = !altSouthwestGate;
                    altSouthWestSortDestination = !altSouthWestSortDestination;
                }

                _ = FlightDataLayer.CreateAsync(new Flight()
                {
                    AirlineIATACode = airline.IATA,
                    AirlineID = airline.Integer64ID,
                    CreatedOn = DateTime.Now,
                    Destination = _airportCodes[new Random(DateTime.Now.Millisecond).Next(0, _airportCodes.Count - 1)],
                    DepartTime = departTime,
                    FlightNumber = flightNumber.ToString().PadLeft(4, '0'),
                    GateID = gate.Integer64ID,
                    GateName = gate.Name ?? string.Empty,
                    Name = $"{airline.IATA}{flightNumber.ToString().PadLeft(4, '0')}",
                    SortDestinationID = altSortDestination is not null ? altSortDestination.Integer64ID : airline.SortDestinationID,
                    SortDestinationName = altSortDestination is not null ? altSortDestination.Name ?? string.Empty : airline.SortDestinationName,
                });

                flightNumber++;
                departTime = departTime.Add(TimeSpan.FromMinutes(10));
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
        _ = SortDestinationDataLayer.CreateAsync(new SortDestination()
        {
            Name = "MU5",
        });
        _ = SortDestinationDataLayer.CreateAsync(new SortDestination()
        {
            Name = "MU6",
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
