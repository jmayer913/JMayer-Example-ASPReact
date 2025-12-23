using JMayer.Data.HTTP.DataLayer;
using JMayer.Example.ASPReact.Server.SortDestinations;

namespace TestProject.SortDestinations;

/// <summary>
/// The class manages CRUD interactions with a remote server for a sort destination.
/// </summary>
internal class SortDestinationDataLayer : StandardCRUDDataLayer<SortDestination>
{
    /// <inheritdoc/>
    public SortDestinationDataLayer(HttpClient httpClient) : base(httpClient) { }
}
