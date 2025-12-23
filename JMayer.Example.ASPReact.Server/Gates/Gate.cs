using JMayer.Data.Data;
using System.ComponentModel.DataAnnotations;

namespace JMayer.Example.ASPReact.Server.Gates;

/// <summary>
/// The class represents a gate in an airport.
/// </summary>
public class Gate : DataObject
{
    /// <inheritdoc/>
    /// <remarks>Overridden to add Required data annotation.</remarks>
    [Required]
    public override string? Name { get => base.Name; set => base.Name = value; }

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