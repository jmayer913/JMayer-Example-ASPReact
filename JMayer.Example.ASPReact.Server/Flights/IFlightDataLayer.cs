using JMayer.Data.Database.DataLayer;

namespace JMayer.Example.ASPReact.Server.Flights;

/// <summary>
/// The interface for interacting with a flight collection in a database using CRUD operations.
/// </summary>
public interface IFlightDataLayer : IUserEditableDataLayer<Flight>
{
}
