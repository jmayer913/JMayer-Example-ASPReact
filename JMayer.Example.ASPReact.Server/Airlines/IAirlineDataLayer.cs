using JMayer.Data.Database.DataLayer;

namespace JMayer.Example.ASPReact.Server.Airlines;

/// <summary>
/// The interface for interacting with an airline collection in a database using CRUD operations.
/// </summary>
public interface IAirlineDataLayer : IUserEditableDataLayer<Airline>
{
}
