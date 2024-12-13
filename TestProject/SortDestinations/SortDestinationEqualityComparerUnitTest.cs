using JMayer.Example.ASPReact.Server.SortDestinations;

namespace TestProject.SortDestinations;

/// <summary>
/// The class manages testing the sort destination equality comparer.
/// </summary>
public class SortDestinationEqualityComparerUnitTest
{
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
    /// The method verifies equality failure when two nulls are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureBothNull() => Assert.False(new SortDestinationEqualityComparer().Equals(null, null));

    /// <summary>
    /// The method verifies equality failure when the Description property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureDescription()
    {
        SortDestination sortDestination1 = new()
        {
            Description = Description,
        };
        SortDestination sortDestination2 = new();

        Assert.False(new SortDestinationEqualityComparer().Equals(sortDestination1, sortDestination2));
    }

    /// <summary>
    /// The method verifies equality failure when the ID property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureID()
    {
        SortDestination sortDestination1 = new()
        {
            Integer64ID = ID,
        };
        SortDestination sortDestination2 = new();

        Assert.False(new SortDestinationEqualityComparer().Equals(sortDestination1, sortDestination2));
    }

    /// <summary>
    /// The method verifies equality failure when the Name property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureName()
    {
        SortDestination sortDestination1 = new()
        {
            Name = Name,
        };
        SortDestination sortDestination2 = new();

        Assert.False(new SortDestinationEqualityComparer().Equals(sortDestination1, sortDestination2));
    }

    /// <summary>
    /// The method verifies equality failure when an object and null are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureOneIsNull()
    {
        SortDestination sortDestination = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };

        Assert.False(new SortDestinationEqualityComparer().Equals(sortDestination, null));
        Assert.False(new SortDestinationEqualityComparer().Equals(null, sortDestination));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared.
    /// </summary>
    [Fact]
    public void VerifySuccess()
    {
        SortDestination sortDestination1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        SortDestination sortDestination2 = new(sortDestination1);

        Assert.True(new SortDestinationEqualityComparer().Equals(sortDestination1, sortDestination2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the LastEditedOn property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeCreatedOn()
    {
        SortDestination sortDestination1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        SortDestination sortDestination2 = new(sortDestination1)
        {
            CreatedOn = DateTime.MinValue,
        };

        Assert.True(new SortDestinationEqualityComparer(true, false, false).Equals(sortDestination1, sortDestination2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the Integer64ID property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeID()
    {
        SortDestination sortDestination1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        SortDestination sortDestination2 = new(sortDestination1)
        {
            Integer64ID = ID + 1,
        };

        Assert.True(new SortDestinationEqualityComparer(false, true, false).Equals(sortDestination1, sortDestination2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the LastEditedOn property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeLastEditedOn()
    {
        SortDestination sortDestination1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
        };
        SortDestination sortDestination2 = new(sortDestination1)
        {
            LastEditedOn = DateTime.MinValue,
        };

        Assert.True(new SortDestinationEqualityComparer(false, false, true).Equals(sortDestination1, sortDestination2));
    }
}
