using JMayer.Data.Database.DataLayer.MemoryStorage;
using System.ComponentModel.DataAnnotations;

namespace JMayer.Example.ASPReact.Server.Airlines;

/// <summary>
/// The class manages CRUD interactions with the database for an airline.
/// </summary>
public class AirlineDataLayer : StandardCRUDDataLayer<Airline>, IAirlineDataLayer
{
    /// <summary>
    /// The default constructor.
    /// </summary>
    public AirlineDataLayer() => IsUniqueNameRequired = true;

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

        if (dataObject.NumberCode is not Airline.ZeroNumberCode && await ExistAsync(obj => obj.Integer64ID != dataObject.Integer64ID && obj.NumberCode == dataObject.NumberCode, cancellationToken) is true)
        {
            validationResults.Add(new ValidationResult("The number code must be unique unless the code is 000.", [nameof(Airline.NumberCode)]));
        }

        return validationResults;
    }
}
