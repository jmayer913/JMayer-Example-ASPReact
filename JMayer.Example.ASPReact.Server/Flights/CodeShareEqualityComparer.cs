using System.Diagnostics.CodeAnalysis;

namespace JMayer.Example.ASPReact.Server.Flights;

/// <summary>
/// The class manages comparing two CodeShare objects.
/// </summary>
public class CodeShareEqualityComparer : IEqualityComparer<CodeShare>
{
    /// <inheritdoc/>
    public bool Equals(CodeShare? x, CodeShare? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return x.AirlineID == y.AirlineID
            && x.FlightNumber == y.FlightNumber;
    }

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] CodeShare obj) => obj.GetHashCode();
}
