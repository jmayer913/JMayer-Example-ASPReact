using JMayer.Data.Data;

namespace JMayer.Example.ASPReact.Server.Gates;

/// <summary>
/// The class represents a gate in an airport.
/// </summary>
public class Gate : UserEditableDataObject
{
    /// <summary>
    /// The default constructor.
    /// </summary>
    public Gate() { }

    /// <summary>
    /// The copy constructor.
    /// </summary>
    /// <param name="copy">The copy.</param>
    public Gate(Gate copy) => MapProperties(copy);
}