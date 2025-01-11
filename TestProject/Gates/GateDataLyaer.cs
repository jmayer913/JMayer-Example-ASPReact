using JMayer.Data.HTTP.DataLayer;
using JMayer.Example.ASPReact.Server.Gates;

namespace TestProject.Gates;

/// <summary>
/// The class manages CRUD interactions with a remote server for an airline.
/// </summary>
internal class GateDataLyaer : UserEditableDataLayer<Gate>
{
    /// <inheritdoc/>
    public GateDataLyaer(HttpClient httpClient) : base(httpClient) { }
}
