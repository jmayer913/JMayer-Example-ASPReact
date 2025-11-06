using JMayer.Data.Database.DataLayer;

namespace JMayer.Example.ASPReact.Server.Gates;

/// <summary>
/// The interface for interacting with a gate collection in a database using CRUD operations.
/// </summary>
public interface IGateDataLayer : IStandardCRUDDataLayer<Gate>
{
}
