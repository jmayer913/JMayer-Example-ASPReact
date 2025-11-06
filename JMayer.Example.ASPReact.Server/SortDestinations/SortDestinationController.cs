using JMayer.Web.Mvc.Controller.Api;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JMayer.Example.ASPReact.Server.SortDestinations;

#warning Should I use the NoAction attribute over returning a MethodNotAllowed status?

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
    public override Task<IActionResult> CreateAsync([FromBody] SortDestination dataObject)
    {
        return Task.FromResult((IActionResult)StatusCode((int)HttpStatusCode.MethodNotAllowed));
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the deletion of sort destinations. The example will auto generate some default sort destinations
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    public override Task<IActionResult> DeleteAsync(long integerID)
    {
        return Task.FromResult((IActionResult)StatusCode((int)HttpStatusCode.MethodNotAllowed));
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the deletion of sort destinations. The example will auto generate some default sort destinations
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    public override Task<IActionResult> DeleteAsync(string stringID)
    {
        return Task.FromResult((IActionResult)StatusCode((int)HttpStatusCode.MethodNotAllowed));
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the updating of sort destinations. The example will auto generate some default sort destinations
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    public override Task<IActionResult> UpdateAsync([FromBody] SortDestination dataObject)
    {
        return Task.FromResult((IActionResult)StatusCode((int)HttpStatusCode.MethodNotAllowed));
    }
}
