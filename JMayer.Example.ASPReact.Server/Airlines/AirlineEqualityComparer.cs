using System.Diagnostics.CodeAnalysis;

namespace JMayer.Example.ASPReact.Server.Airlines;

/// <summary>
/// The class manages comparing two Airline objects.
/// </summary>
public class AirlineEqualityComparer : IEqualityComparer<Airline>
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
    public AirlineEqualityComparer() { }

    /// <summary>
    /// The property constructor.
    /// </summary>
    /// <param name="excludeCreatedOn">Excludes the CreatedOn property from the equals check.</param>
    /// <param name="excludeID">Excludes the ID property from the equals check.</param>
    /// <param name="excludeLastEditedOn">Excludes the LastEditedOn property from the equals check.</param>
    public AirlineEqualityComparer(bool excludeCreatedOn, bool excludeID, bool excludeLastEditedOn)
    {
        _excludeCreatedOn = excludeCreatedOn;
        _excludeID = excludeID;
        _excludeLastEditedOn = excludeLastEditedOn;
    }

    /// <inheritdoc/>
    public bool Equals(Airline? x, Airline? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return (_excludeCreatedOn || x.CreatedOn == y.CreatedOn)
            && x.Description == y.Description
            && x.IATA == y.IATA
            && x.ICAO == y.ICAO
            && (_excludeID || x.Integer64ID == y.Integer64ID)
            && (_excludeLastEditedOn || x.LastEditedBy == y.LastEditedBy)
            && x.Name == y.Name
            && x.NumberCode == y.NumberCode;
    }

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] Airline obj) => obj.GetHashCode();
}
