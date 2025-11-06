using System.Diagnostics.CodeAnalysis;

namespace JMayer.Example.ASPReact.Server.Flights;

/// <summary>
/// The class manages comparing two Flight objects.
/// </summary>
public class FlightEqualityComparer : IEqualityComparer<Flight>
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
    public FlightEqualityComparer() { }

    /// <summary>
    /// The property constructor.
    /// </summary>
    /// <param name="excludeCreatedOn">Excludes the CreatedOn property from the equals check.</param>
    /// <param name="excludeID">Excludes the ID property from the equals check.</param>
    /// <param name="excludeLastEditedOn">Excludes the LastEditedOn property from the equals check.</param>
    public FlightEqualityComparer(bool excludeCreatedOn, bool excludeID, bool excludeLastEditedOn)
    {
        _excludeCreatedOn = excludeCreatedOn;
        _excludeID = excludeID;
        _excludeLastEditedOn = excludeLastEditedOn;
    }

    /// <inheritdoc/>
    public bool Equals(Flight? x, Flight? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        if (x.CodeShares.Count != y.CodeShares.Count)
        {
            return false;
        }
        else
        {
            for (int index = 0; index < x.CodeShares.Count; index++)
            {
                if (new CodeShareEqualityComparer().Equals(x.CodeShares[index], y.CodeShares[index]) == false)
                {
                    return false;
                }
            }
        }

        return x.AirlineID == y.AirlineID
            && (_excludeCreatedOn || x.CreatedOn == y.CreatedOn)
            && x.DepartTime == y.DepartTime
            && x.Description == y.Description
            && x.FlightNumber == y.FlightNumber
            && x.GateID == y.GateID
            && (_excludeID || x.Integer64ID == y.Integer64ID)
            && (_excludeLastEditedOn || x.LastEditedBy == y.LastEditedBy)
            && x.Name == y.Name
            && x.Destination == y.Destination;
    }

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] Flight obj) => obj.GetHashCode();
}
