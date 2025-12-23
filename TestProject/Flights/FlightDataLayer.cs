using JMayer.Data.HTTP.DataLayer;
using JMayer.Example.ASPReact.Server.Flights;

namespace TestProject.Flights;

/// <summary>
/// The class manages CRUD interactions with a remote server for a flight.
/// </summary>
internal class FlightDataLayer : StandardCRUDDataLayer<Flight>
{
    /// <inheritdoc/>
    public FlightDataLayer(HttpClient httpClient) : base(httpClient) { }
}
