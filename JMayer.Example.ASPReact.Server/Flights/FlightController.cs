using JMayer.Web.Mvc.Controller.Api;
using Microsoft.AspNetCore.Mvc;

namespace JMayer.Example.ASPReact.Server.Flights;

/// <summary>
/// The class manages HTTP requests for CRUD operations associated with a flight in a database.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class FlightController : StandardCRUDController<Flight, IFlightDataLayer>
{
    /// <inheritdoc/>
    public FlightController(IFlightDataLayer dataLayer, ILogger<FlightController> logger) : base(dataLayer, logger) { }
}
