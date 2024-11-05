using JMayer.Data.Database.DataLayer.MemoryStorage;
using JMayer.Example.ASPReact.Server.Airlines;
using JMayer.Example.ASPReact.Server.Gates;
using System.ComponentModel.DataAnnotations;

namespace JMayer.Example.ASPReact.Server.Flights;

/// <summary>
/// The class manages CRUD interactions with the database for a flight.
/// </summary>
public class FlightDataLayer : UserEditableDataLayer<Flight>, IFlightDataLayer
{
    /// <summary>
    /// Used to access the airline data.
    /// </summary>
    private readonly IAirlineDataLayer _airlineDataLayer;

    /// <summary>
    /// Used to access the gate data.
    /// </summary>
    private readonly IGateDataLayer _gateDataLayer;

    /// <summary>
    /// The dependency injection constructor.
    /// </summary>
    /// <param name="airlineDataLayer">Used to access the airline data.</param>
    /// <param name="gateDataLayer">Used to access the gate data.</param>
    public FlightDataLayer(IAirlineDataLayer airlineDataLayer, IGateDataLayer gateDataLayer)
    {
        _airlineDataLayer = airlineDataLayer;
        _gateDataLayer = gateDataLayer;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to confirm references exists and the flight is unique (airline, flight number & next destination).
    /// </remarks>
    public override async Task<List<ValidationResult>> ValidateAsync(Flight dataObject, CancellationToken cancellationToken = default)
    {
        List<ValidationResult> validationResults = await base.ValidateAsync(dataObject, cancellationToken);

        if (await _airlineDataLayer.ExistAsync(obj => obj.Integer64ID == dataObject.AirlineID, cancellationToken) == false)
        {
            validationResults.Add(new ValidationResult($"The {dataObject.AirlineID} airline was not found in the data store.", [nameof(Flight.AirlineID)]));
        }

        if (await _gateDataLayer.ExistAsync(obj => obj.Integer64ID == dataObject.GateID, cancellationToken) == false)
        {
            validationResults.Add(new ValidationResult($"The {dataObject.GateID} gate was not found in the data store.", [nameof(Flight.GateID)]));
        }

        foreach (var codeShare in dataObject.CodeShares)
        {
            if (await _airlineDataLayer.ExistAsync(obj => obj.Integer64ID == dataObject.AirlineID, cancellationToken) == false)
            {
                validationResults.Add(new ValidationResult($"The {codeShare.AirlineID} airline for the codeshare was not found in the data store.", [nameof(CodeShare.AirlineID)]));
            }
        }

        if (await ExistAsync(obj => obj.Integer64ID != dataObject.Integer64ID && obj.AirlineID == dataObject.AirlineID && obj.FlightNumber == dataObject.FlightNumber && obj.NextDestination == dataObject.NextDestination, cancellationToken) == true)
        {
            validationResults.Add(new ValidationResult("The flight already exists in the active schedule.", [nameof(Flight.FlightNumber)]));
        }

        return validationResults;
    }
}
