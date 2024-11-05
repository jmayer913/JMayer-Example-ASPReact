using JMayer.Data.Database.DataLayer;
using JMayer.Web.Mvc.Controller;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JMayer.Example.ASPReact.Server.Gates;

/// <summary>
/// The class manages HTTP requests for CRUD operations associated with a gate in a database.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GateController : UserEditableController<Gate, IGateDataLayer>
{
    /// <inheritdoc/>
    public GateController(IUserEditableDataLayer<Gate> dataLayer, ILogger logger) : base(dataLayer, logger) { }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the creation of new gates. The example will auto generate some default gates
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    public override Task<IActionResult> CreateAsync([FromBody] Gate dataObject)
    {
        return Task.FromResult((IActionResult)StatusCode((int)HttpStatusCode.MethodNotAllowed));
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the deletion of gates. The example will auto generate some default gates
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    public override Task<IActionResult> DeleteAsync(long integerID)
    {
        return Task.FromResult((IActionResult)StatusCode((int)HttpStatusCode.MethodNotAllowed));
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the deletion of gates. The example will auto generate some default gates
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    public override Task<IActionResult> DeleteAsync(string stringID)
    {
        return Task.FromResult((IActionResult)StatusCode((int)HttpStatusCode.MethodNotAllowed));
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to prevent the updating of gates. The example will auto generate some default gates
    /// and the client side will only retrieve them but not edit them.
    /// </remarks>
    public override Task<IActionResult> UpdateAsync([FromBody] Gate dataObject)
    {
        return Task.FromResult((IActionResult)StatusCode((int)HttpStatusCode.MethodNotAllowed));
    }
}
