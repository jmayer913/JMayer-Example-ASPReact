using JMayer.Data.Data;
using System.ComponentModel.DataAnnotations;

namespace JMayer.Example.ASPReact.Server.Airlines;

/// <summary>
/// The class represents an airline and its codes.
/// </summary>
public class Airline : UserEditableDataObject
{
    /// <summary>
    /// The property gets/sets the IATA code assigned by the IATA organization.
    /// </summary>
    [Required]
    [RegularExpression("^[A-Z0-9]{2}$", ErrorMessage = "The IATA must be 2 alphanumeric characters; letters must be capitalized.")]
    public string IATA { get;set; } = string.Empty;

    /// <summary>
    /// The property gets/sets the ICAO code assigned by the International Aviation Organization.
    /// </summary>
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "The ICAO must be 3 capital letters.")]
    public string ICAO { get; set; } = string.Empty;

    /// <summary>
    /// The property gets/sets the number code assigned by the IATA organization.
    /// </summary>
    [Required]
    [RegularExpression("^\\d{3}$", ErrorMessage = "The number code must be 3 digits.")]
    public string NumberCode { get; set; } = ZeroNumberCode;

    /// <summary>
    /// The constant for the zero number code.
    /// </summary>
    public const string ZeroNumberCode = "000";

    /// <summary>
    /// The default constructor.
    /// </summary>
    public Airline() { }

    /// <summary>
    /// The copy constructor.
    /// </summary>
    /// <param name="copy">The copy.</param>
    public Airline(Airline copy) => MapProperties(copy);

    /// <inheritdoc/>
    public override void MapProperties(DataObject dataObject)
    {
        base.MapProperties(dataObject);

        if (dataObject is Airline airline)
        {
            IATA = airline.IATA;
            ICAO = airline.ICAO;
            NumberCode = airline.NumberCode;
        }
    }
}
