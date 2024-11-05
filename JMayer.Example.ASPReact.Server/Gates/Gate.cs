using JMayer.Data.Data;

namespace JMayer.Example.ASPReact.Server.Gates;

/// <summary>
/// The class represents a gate in an airport.
/// </summary>
public class Gate : UserEditableDataObject
{
    /// <summary>
    /// The property gets/sets the id of the airline this gate is registered to.
    /// </summary>
    public long AirlineID { get; set; }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public Gate() { }

    /// <summary>
    /// The copy constructor.
    /// </summary>
    /// <param name="copy">The copy.</param>
    public Gate(Gate copy) => MapProperties(copy);

    /// <inheritdoc/>
    public override void MapProperties(DataObject dataObject)
    {
        base.MapProperties(dataObject);

        if (dataObject is Gate gate)
        {
            AirlineID = gate.AirlineID;
        }
    }
}