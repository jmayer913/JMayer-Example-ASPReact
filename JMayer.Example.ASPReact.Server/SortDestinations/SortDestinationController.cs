using JMayer.Web.Mvc.Controller.Api;
using Microsoft.AspNetCore.Mvc;

namespace JMayer.Example.ASPReact.Server.SortDestinations;

/// <summary>
/// The class manages HTTP requests for CRUD operations associated with a sort destination in a database.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SortDestinationController : StandardCRUDController<SortDestination, ISortDestinationDataLayer>
{
    /// <inheritdoc/>
    public SortDestinationController(ISortDestinationDataLayer dataLayer, ILogger<SortDestinationController> logger) : base(dataLayer, logger) { }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the creation of new sort destinations. The example will auto generate some default sort destinations
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> CreateAsync([FromBody] SortDestination dataObject)
    {
        return base.CreateAsync(dataObject);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the deletion of sort destinations. The example will auto generate some default sort destinations
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> DeleteAsync(long id)
    {
        return base.DeleteAsync(id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the deletion of sort destinations. The example will auto generate some default sort destinations
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> DeleteAsync(string id)
    {
        return base.DeleteAsync(id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the updating of sort destinations. The example will auto generate some default sort destinations
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> UpdateAsync([FromBody] SortDestination dataObject)
    {
        return base.UpdateAsync(dataObject);
    }
}
