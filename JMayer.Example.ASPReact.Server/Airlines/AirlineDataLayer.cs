using JMayer.Data.Database.DataLayer.MemoryStorage;

namespace JMayer.Example.ASPReact.Server.Airlines;

/// <summary>
/// The class manages CRUD interactions with the database for an airline.
/// </summary>
public class AirlineDataLayer : UserEditableDataLayer<Airline>, IAirlineDataLayer
{
}
