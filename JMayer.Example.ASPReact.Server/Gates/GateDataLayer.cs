using JMayer.Data.Database.DataLayer.MemoryStorage;

namespace JMayer.Example.ASPReact.Server.Gates;

/// <summary>
/// The class manages CRUD interactions with the database for a gate.
/// </summary>
public class GateDataLayer : UserEditableDataLayer<Gate>, IGateDataLayer
{
}
