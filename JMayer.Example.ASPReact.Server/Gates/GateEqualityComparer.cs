using System.Diagnostics.CodeAnalysis;

namespace JMayer.Example.ASPReact.Server.Gates;

/// <summary>
/// The class manages comparing two Gate objects.
/// </summary>
public class GateEqualityComparer : IEqualityComparer<Gate>
{
    /// <summary>
    /// Excludes the CreatedOn property from the equals check.
    /// </summary>
    private readonly bool _excludeCreatedOn;

    /// <summary>
    /// Excludes the ID property from the equals check.
    /// </summary>
    private readonly bool _excludeID;

    /// <summary>
    /// Excludes the LastEditedOn property from the equals check.
    /// </summary>
    private readonly bool _excludeLastEditedOn;

    /// <summary>
    /// The default constructor.
    /// </summary>
    public GateEqualityComparer() { }

    /// <summary>
    /// The property constructor.
    /// </summary>
    /// <param name="excludeCreatedOn">Excludes the CreatedOn property from the equals check.</param>
    /// <param name="exlucdeID">Excludes the ID property from the equals check.</param>
    /// <param name="excludeLastEditedOn">Excludes the LastEditedOn property from the equals check.</param>
    public GateEqualityComparer(bool excludeCreatedOn, bool excludeID, bool excludeLastEditedOn)
    {
        _excludeCreatedOn = excludeCreatedOn;
        _excludeID = excludeID;
        _excludeLastEditedOn = excludeLastEditedOn;
    }

    /// <inheritdoc/>
    public bool Equals(Gate? x, Gate? y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return x.AirlineID == y.AirlineID
            && (_excludeCreatedOn || x.CreatedOn == y.CreatedOn)
            && x.Description == y.Description
            && (_excludeID || x.Integer64ID == y.Integer64ID)
            && (_excludeLastEditedOn || x.LastEditedBy == y.LastEditedBy)
            && x.Name == y.Name;
    }

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] Gate obj) => obj.GetHashCode();
}
