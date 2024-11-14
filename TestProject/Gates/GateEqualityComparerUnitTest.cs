using JMayer.Example.ASPReact.Server.Gates;

namespace TestProject.Gates;

/// <summary>
/// The class manages testing the gate equality comparer.
/// </summary>
public class GateEqualityComparerUnitTest
{
    /// <summary>
    /// The constant for the airline ID.
    /// </summary>
    private const long AirlineID = 2;

    /// <summary>
    /// The constant for the description.
    /// </summary>
    private const string Description = "A Description";

    /// <summary>
    /// The constant for the ID.
    /// </summary>
    private const long ID = 1;

    /// <summary>
    /// The constant for the name.
    /// </summary>
    private const string Name = "A Name";

    /// <summary>
    /// The method verifies equality failure when the AirlineID property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureAirlineID()
    {
        Gate gate1 = new()
        {
            AirlineID = AirlineID,
        };
        Gate gate2 = new();

        Assert.False(new GateEqualityComparer().Equals(gate1, gate2));
    }

    /// <summary>
    /// The method verifies equality failure when two nulls are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureBothNull() => Assert.False(new GateEqualityComparer().Equals(null, null));

    /// <summary>
    /// The method verifies equality failure when the Description property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureDescription()
    {
        Gate gate1 = new()
        {
            Description = Description,
        };
        Gate gate2 = new();

        Assert.False(new GateEqualityComparer().Equals(gate1, gate2));
    }

    /// <summary>
    /// The method verifies equality failure when the ID property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureID()
    {
        Gate gate1 = new()
        {
            Integer64ID = ID,
        };
        Gate gate2 = new();

        Assert.False(new GateEqualityComparer().Equals(gate1, gate2));
    }

    /// <summary>
    /// The method verifies equality failure when the Name property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureName()
    {
        Gate gate1 = new()
        {
            Name = Name,
        };
        Gate gate2 = new();

        Assert.False(new GateEqualityComparer().Equals(gate1, gate2));
    }

    /// <summary>
    /// The method verifies equality failure when an object and null are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureOneIsNull()
    {
        Gate gate = new()
        {
            AirlineID = AirlineID,
            CreatedOn = DateTime.Now,
            Description = Description,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };

        Assert.False(new GateEqualityComparer().Equals(gate, null));
        Assert.False(new GateEqualityComparer().Equals(null, gate));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared.
    /// </summary>
    [Fact]
    public void VerifySuccess()
    {
        Gate gate1 = new()
        {
            AirlineID = AirlineID,
            CreatedOn = DateTime.Now,
            Description = Description,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        Gate gate2 = new(gate1);

        Assert.True(new GateEqualityComparer().Equals(gate1, gate2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the LastEditedOn property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeCreatedOn()
    {
        Gate gate1 = new()
        {
            AirlineID = AirlineID,
            CreatedOn = DateTime.Now,
            Description = Description,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        Gate gate2 = new(gate1)
        {
            CreatedOn = DateTime.MinValue,
        };

        Assert.True(new GateEqualityComparer(true, false, false).Equals(gate1, gate2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the Integer64ID property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeID()
    {
        Gate gate1 = new()
        {
            AirlineID = AirlineID,
            CreatedOn = DateTime.Now,
            Description = Description,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        Gate gate2 = new(gate1)
        {
            Integer64ID = ID + 1,
        };

        Assert.True(new GateEqualityComparer(false, true, false).Equals(gate1, gate2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the LastEditedOn property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeLastEditedOn()
    {
        Gate gate1 = new()
        {
            AirlineID = AirlineID,
            CreatedOn = DateTime.Now,
            Description = Description,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        Gate gate2 = new(gate1)
        {
            LastEditedOn = DateTime.MinValue,
        };

        Assert.True(new GateEqualityComparer(false, false, true).Equals(gate1, gate2));
    }
}
