using JMayer.Data.HTTP.DataLayer;
using JMayer.Example.ASPReact.Server.Airlines;

namespace TestProject.Airlines;

/// <summary>
/// The class manages CRUD interactions with a remote server for an airline.
/// </summary>
internal class AirlineDataLayer : UserEditableDataLayer<Airline>
{
    /// <inheritdoc/>
    public AirlineDataLayer(HttpClient httpClient) : base(httpClient) { }
}
