using JMayer.Data.Data;
using System.ComponentModel.DataAnnotations;

namespace JMayer.Example.ASPReact.Server.SortDestinations;

/// <summary>
/// The class represents a sort destination in the baggage handling system.
/// </summary>
public class SortDestination : DataObject
{
    /// <inheritdoc/>
    /// <remarks>Overridden to add Required data annotation.</remarks>
    [Required]
    public override string? Name { get => base.Name; set => base.Name = value; }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public SortDestination() { }

    /// <summary>
    /// The copy constructor.
    /// </summary>
    /// <param name="copy">The copy.</param>
    public SortDestination(SortDestination copy) => MapProperties(copy);
}
