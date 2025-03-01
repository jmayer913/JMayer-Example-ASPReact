﻿using JMayer.Data.Data;
using System.ComponentModel.DataAnnotations;

namespace JMayer.Example.ASPReact.Server.Flights;

/// <summary>
/// The class represents a flight in the flight schedule.
/// </summary>
public class Flight : UserEditableDataObject
{
    /// <summary>
    /// The property gets/sets the IATA code for the airline for the flight.
    /// </summary>
    public string AirlineIATACode { get; set; } = string.Empty;

    /// <summary>
    /// The property gets/sets the id for the airline for the flight.
    /// </summary>
    [Required]
    public long AirlineID { get; set; }

    /// <summary>
    /// The property gets/sets the codeshares for the flight.
    /// </summary>
    public List<CodeShare> CodeShares { get; init; } = [];

    /// <summary>
    /// The property gets/sets when the flight departs.
    /// </summary>
    [Required]
    public TimeSpan DepartTime { get; set; }

    /// <summary>
    /// The property gets/sets the destination for the flight.
    /// </summary>
    [Required]
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "The city must be 3 capital letters.")]
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// The property gets/sets the number associated with the flight.
    /// </summary>
    [Required]
    [RegularExpression("^([0-9]{4}|([0-9]{4}[A-Z]{1}))$", ErrorMessage = "The flight number must be 4 digits or 4 digits and a capital letter.")]
    public string FlightNumber { get; set; } = string.Empty;

    /// <summary>
    /// The property gets/sets the id for the gate assigned to the flight.
    /// </summary>
    [Required]
    public long GateID { get; set; }

    /// <summary>
    /// The property gets/sets the name of the gate assigned to the flight.
    /// </summary>
    public string GateName { get; set; } = string.Empty;

    /// <summary>
    /// The property gets/sets the id for the sort destintion assigned to the flight.
    /// </summary>
    [Required]
    public long SortDestinationID { get; set; }

    /// <summary>
    /// The property gets/sets the name of the sort destination assigned to the flight.
    /// </summary>
    public string SortDestinationName { get; set; } = string.Empty;

    /// <summary>
    /// The default constructor.
    /// </summary>
    public Flight() { }

    /// <summary>
    /// The copy constructor.
    /// </summary>
    /// <param name="copy">The copy.</param>
    public Flight(Flight copy) => MapProperties(copy);

    /// <inheritdoc/>
    public override void MapProperties(DataObject dataObject)
    {
        base.MapProperties(dataObject);

        if (dataObject is Flight flight)
        {
            AirlineIATACode = flight.AirlineIATACode;
            AirlineID = flight.AirlineID;
            CodeShares.Clear();
            DepartTime = flight.DepartTime;
            Destination = flight.Destination;
            FlightNumber = flight.FlightNumber;
            GateID = flight.GateID;
            GateName = flight.GateName;
            SortDestinationID = flight.SortDestinationID;
            SortDestinationName = flight.SortDestinationName;

            foreach (var codeShare in flight.CodeShares)
            {
                CodeShares.Add(new CodeShare(codeShare));
            }
        }
    }
}
