using JMayer.Web.Mvc.Controller.Api;
using Microsoft.AspNetCore.Mvc;

namespace JMayer.Example.ASPReact.Server.Gates;

/// <summary>
/// The class manages HTTP requests for CRUD operations associated with a gate in a database.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GateController : StandardCRUDController<Gate, IGateDataLayer>
{
    /// <inheritdoc/>
    public GateController(IGateDataLayer dataLayer, ILogger<GateController> logger) : base(dataLayer, logger) { }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the creation of new gates. The example will auto generate some default gates
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> CreateAsync([FromBody] Gate dataObject)
    {
        return base.CreateAsync(dataObject);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the deletion of gates. The example will auto generate some default gates
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> DeleteAsync(long id)
    {
        return base.DeleteAsync(id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the deletion of gates. The example will auto generate some default gates
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> DeleteAsync(string id)
    {
        return base.DeleteAsync(id);
    }

    /// <inheritdoc/>
    /// <remarks>Overridden to hide the string version of this. The client doesn't use it and swagger complains about a conflict when its exposed.</remarks>
    [NonAction]
    public override Task<IActionResult> GetSingleAsync(string id)
    {
        return base.GetSingleAsync(id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the updating of gates. The example will auto generate some default gates
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> UpdateAsync([FromBody] Gate dataObject)
    {
        return base.UpdateAsync(dataObject);
    }
}
