using JMayer.Web.Mvc.Controller;
using Microsoft.AspNetCore.Mvc;

namespace JMayer.Example.ASPReact.Server.Airlines;

/// <summary>
/// The class manages HTTP requests for CRUD operations associated with an airline in a database.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AirlineController : UserEditableController<Airline, IAirlineDataLayer>
{
    /// <inheritdoc/>
    public AirlineController(IAirlineDataLayer dataLayer, ILogger<AirlineController> logger) : base(dataLayer, logger) { }
}
