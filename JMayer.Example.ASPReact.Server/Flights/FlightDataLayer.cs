using JMayer.Data.Database.DataLayer.MemoryStorage;
using JMayer.Example.ASPReact.Server.Airlines;
using JMayer.Example.ASPReact.Server.Gates;
using JMayer.Example.ASPReact.Server.SortDestinations;
using System.ComponentModel.DataAnnotations;

namespace JMayer.Example.ASPReact.Server.Flights;

/// <summary>
/// The class manages CRUD interactions with the database for a flight.
/// </summary>
public class FlightDataLayer : StandardCRUDDataLayer<Flight>, IFlightDataLayer
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
    /// Used to access the sort desintation data.
    /// </summary>
    private readonly ISortDestinationDataLayer _sortDestinationDataLayer;

    /// <summary>
    /// The dependency injection constructor.
    /// </summary>
    /// <param name="airlineDataLayer">Used to access the airline data.</param>
    /// <param name="gateDataLayer">Used to access the gate data.</param>
    /// <param name="sortDestinationDataLayer">Used to access the sort desintation data.</param>
    public FlightDataLayer(IAirlineDataLayer airlineDataLayer, IGateDataLayer gateDataLayer, ISortDestinationDataLayer sortDestinationDataLayer)
    {
        IsOldDataObjectDetectionEnabled = true;

        _airlineDataLayer = airlineDataLayer;
        _gateDataLayer = gateDataLayer;
        _sortDestinationDataLayer = sortDestinationDataLayer;

        _airlineDataLayer.Deleted += AirlineDataLayer_Deleted;
    }

    /// <summary>
    /// The method deletes any flights associated with the deleted airlines.
    /// </summary>
    /// <param name="sender">The asset data layer.</param>
    /// <param name="e">The arguments which contain the deleted assets.</param>
    private async void AirlineDataLayer_Deleted(object? sender, Data.Database.DataLayer.DeletedEventArgs e)
    {
        foreach (Airline airline in e.DataObjects.Cast<Airline>())
        {
            await DeleteAsync(obj => obj.AirlineID == airline.Integer64ID);
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to confirm references exist and the flight is unique (airline, flight number & next destination).
    /// </remarks>
    public override async Task<List<ValidationResult>> ValidateAsync(Flight dataObject, CancellationToken cancellationToken = default)
    {
        List<ValidationResult> validationResults = await base.ValidateAsync(dataObject, cancellationToken);

        if (await _airlineDataLayer.ExistAsync(obj => obj.Integer64ID == dataObject.AirlineID, cancellationToken) is false)
        {
            validationResults.Add(new ValidationResult($"The {dataObject.AirlineID} airline was not found in the data store.", [nameof(Flight.AirlineID)]));
        }

        if (await _gateDataLayer.ExistAsync(obj => obj.Integer64ID == dataObject.GateID, cancellationToken) is false)
        {
            validationResults.Add(new ValidationResult($"The {dataObject.GateID} gate was not found in the data store.", [nameof(Flight.GateID)]));
        }

        if (await _sortDestinationDataLayer.ExistAsync(obj => obj.Integer64ID == dataObject.SortDestinationID, cancellationToken) is false)
        {
            validationResults.Add(new ValidationResult($"The {dataObject.SortDestinationID} sort destination was not found in the data store.", [nameof(Flight.SortDestinationID)]));
        }

        foreach (var codeShare in dataObject.CodeShares)
        {
            if (await _airlineDataLayer.ExistAsync(obj => obj.Integer64ID == codeShare.AirlineID, cancellationToken) is false)
            {
                validationResults.Add(new ValidationResult($"The {codeShare.AirlineID} airline for the codeshare was not found in the data store.", [nameof(CodeShare.AirlineID)]));
            }
        }

        if (await ExistAsync(obj => obj.Integer64ID != dataObject.Integer64ID && obj.AirlineID == dataObject.AirlineID && obj.FlightNumber == dataObject.FlightNumber && obj.Destination == dataObject.Destination, cancellationToken) is true)
        {
            validationResults.Add(new ValidationResult("The flight already exists in the schedule.", [nameof(Flight.FlightNumber)]));
        }

        return validationResults;
    }
}
