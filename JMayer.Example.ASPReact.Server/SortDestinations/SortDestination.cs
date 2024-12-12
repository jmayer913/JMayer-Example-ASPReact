using JMayer.Data.Data;

namespace JMayer.Example.ASPReact.Server.SortDestinations;

/// <summary>
/// The class represents a sort destination in the baggage handling system.
/// </summary>
public class SortDestination : UserEditableDataObject
{
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
