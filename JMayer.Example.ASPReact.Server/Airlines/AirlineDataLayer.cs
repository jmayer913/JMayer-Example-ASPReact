using JMayer.Data.Database.DataLayer.MemoryStorage;
using JMayer.Example.ASPReact.Server.SortDestinations;
using System.ComponentModel.DataAnnotations;

namespace JMayer.Example.ASPReact.Server.Airlines;

/// <summary>
/// The class manages CRUD interactions with the database for an airline.
/// </summary>
public class AirlineDataLayer : StandardCRUDDataLayer<Airline>, IAirlineDataLayer
{
    /// <summary>
    /// The property gets/sets the data layer for accessing the sort desintation data.
    /// </summary>
    private readonly ISortDestinationDataLayer _sortDestinationDataLayer;

    /// <summary>
    /// The dependency injection constructor.
    /// </summary>
    /// <param name="sortDestinationDataLayer"></param>
    public AirlineDataLayer(ISortDestinationDataLayer sortDestinationDataLayer)
    {
        _sortDestinationDataLayer = sortDestinationDataLayer;
        IsUniqueNameRequired = true;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to check the ICAO is unique and the number code is unique (expect for 000).
    /// </remarks>
    public override async Task<List<ValidationResult>> ValidateAsync(Airline dataObject, CancellationToken cancellationToken = default)
    {
        List<ValidationResult> validationResults = await base.ValidateAsync(dataObject, cancellationToken);

        if (dataObject.ICAO is not null && await ExistAsync(obj => obj.Integer64ID != dataObject.Integer64ID && obj.ICAO == dataObject.ICAO, cancellationToken) is true)
        {
            validationResults.Add(new ValidationResult("The ICAO must be unique.", [nameof(Airline.ICAO)]));
        }

        if (await _sortDestinationDataLayer.ExistAsync(obj => obj.Integer64ID == dataObject.SortDestinationID, cancellationToken) is false)
        {
            validationResults.Add(new ValidationResult($"The {dataObject.SortDestinationID} sort destination was not found in the data store.", [nameof(Airline.SortDestinationID)]));
        }

        if (dataObject.NumberCode is not Airline.ZeroNumberCode && await ExistAsync(obj => obj.Integer64ID != dataObject.Integer64ID && obj.NumberCode == dataObject.NumberCode, cancellationToken) is true)
        {
            validationResults.Add(new ValidationResult("The number code must be unique unless the code is 000.", [nameof(Airline.NumberCode)]));
        }

        return validationResults;
    }
}
