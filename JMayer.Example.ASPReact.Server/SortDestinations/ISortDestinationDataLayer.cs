using JMayer.Data.Database.DataLayer;

namespace JMayer.Example.ASPReact.Server.SortDestinations;

/// <summary>
/// The interface for interacting with a sort destination collection in a database using CRUD operations.
/// </summary>
public interface ISortDestinationDataLayer : IStandardCRUDDataLayer<SortDestination>
{
}
