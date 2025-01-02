import React, { useState, useEffect } from 'react'
import { Calendar } from 'primereact/calendar';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { useAirlineDataLayer } from '../../datalayers/AirlineDataLayer.jsx';
import { useFlightDataLayer } from '../../datalayers/FlightDataLayer.jsx';
import { useGateDataLayer } from '../../datalayers/GateDataLayer.jsx';
import { useSortDestinationDataLayer } from '../../datalayers/SortDestinationDataLayer.jsx';

//Used to add or update a flight.
//@param {object} props The properties accepted by the component.
//@param {bool} props.newRecord Indicates if the flight object is a new record or not.
//@param {object} props.flight The flight to add or update.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function FlightAddEditDialog({ newRecord, flight, setFlight, visible, hide }) {
    const { airlines, getAirlines } = useAirlineDataLayer();
    const { addFlight, addFlightServerSideResult, addFlightSuccess, updateFlight, updateFlightServerSideResult, updateFlightSuccess } = useFlightDataLayer();
    const { gates, getGates } = useGateDataLayer();
    const { sortDestinations, getSortDestinations } = useSortDestinationDataLayer();
    const [airlineValidationError, setAirlineValidationError] = useState('');
    const [gateValidationError, setGateValidationError] = useState('');
    const [destinationValidationError, setDestinationValidationError] = useState('');
    const [flightNumberValidationError, setFlightNumberValidationError] = useState('');
    const [sortDestinationValidationError, setSortDestinationValidationError] = useState('');

    //When editing a flight, the server will return only the time which
    //javascript will represent as a string so the time string will need 
    //to be converted to a date for the Calendar control.
    if (typeof flight.departTime === 'string') {
        const options = { year: 'numeric', month: 'numeric', day: 'numeric' };
        setFlight({
            ...flight,
            departTime: new Date(Date.parse(new Date().toLocaleDateString('en-US', options) + " " + flight.departTime))
        });
    }

    //Load the airlines, gates & sort destinations when the component mounts.
    useEffect(() => {
        getAirlines();
        getGates();
        getSortDestinations();
    }, []);

    //Handle state changes based on add/update operations.
    useEffect(() => {
        //Hide the dialog on a successful add or update.
        if (addFlightSuccess || updateFlightSuccess) {
            hide();
        }

        //Handle displays server side errors.
        if (addFlightServerSideResult !== null) {
            processServerSideValidationResult(addFlightServerSideResult);
        }
        else if (updateFlightServerSideResult !== null) {
            processServerSideValidationResult(updateFlightServerSideResult);
        }

    }, [addFlightServerSideResult, addFlightSuccess, updateFlightServerSideResult, updateFlightSuccess]);

    //The function clears the validation and closes the dialog.
    const closeDialog = () => {
        setAirlineValidationError('');
        setDestinationValidationError('');
        setGateValidationError('');
        setFlightNumberValidationError('');
        setSortDestinationValidationError('');
        hide();
    };

    //The funciton returns if validation passed or failed.
    const isValid = () => {
        const airlinePass = validateAirline();
        const destinationPass = validateDestination();
        const gatePass = validateGate();
        const flightNumberPass = validateFlightNumber();
        const sortDestinaitonPass = validateSortDestination();

        return airlinePass && destinationPass && gatePass && flightNumberPass && sortDestinaitonPass;
    };

    //The function processes the server side validation result and sets any validation errors.
    //@param {object} serverSideValidationResult What the server found wrong with the user input.
    const processServerSideValidationResult = (serverSideValidationResult) => {
        for (const error of serverSideValidationResult.errors) {
            switch (error.propertyName) {
                case 'Destination':
                    setDestinationValidationError(error.errorMessage);
                    break;
                case 'FlightNumber':
                    setFlightNumberValidationError(error.errorMessage);
                    break;
            }
        }
    };

    //The function updates the airline field with the value selected by the user.
    //@param {object} airline The airline the user selected.
    const setAirline = (airline) => {
        setFlight({
            ...flight,
            airlineID: airline.integer64ID,
            airlineIATACode: airline.iata,
            name: airline.iata + flight.flightNumber
        });
    };

    //The function updates the depart time field with the value entered by the user.
    //@param {date} departTime The depart time the user entered.
    const setDepartTime = (departTime) => {
        setFlight({
            ...flight,
            departTime: departTime
        });
    };

    //The function updates the destination field with the value entered by the user.
    //@param {string} destination The destination the user entered.
    const setDestination = (destination) => {
        setFlight({
            ...flight,
            destination: destination.toUpperCase()
        });
    };

    //The function updates the flight number field with the value entered by the user.
    //@param {string} flightNumber The flight number the user entered.
    const setFlightNumber = (flightNumber) => {
        setFlight({
            ...flight,
            flightNumber: flightNumber.toUpperCase(),
            name: flight.airlineIATACode + flightNumber.toUpperCase()
        });
    };

    //The function updates the gate field with the value selected by the user.
    //@param {object} gate The flight number the user selected.
    const setGate = (gate) => {
        setFlight({
            ...flight,
            gateID: gate.integer64ID,
            gateName: gate.name,
        });
    };

    //The function updates the sort destination field with the value selected by the user.
    //@param {sortDestination} sortDestination The sort desstination the user selected.
    const setSortDestination = (sortDestination) => {
        setFlight({
            ...flight,
            sortDestinationID: sortDestination.integer64ID,
            sortDestinationName: sortDestination.name
        });
    };

    //The function returns if the flight's airline field passed validation.
    const validateAirline = () => {
        let error = '';

        if (flight.airlineID === 0) {
            error = 'The airline is required.';
        }

        setAirlineValidationError(error);

        return !error;
    };

    //The function returns if the flight's destination field passed validation.
    const validateDestination = () => {
        const pattern = /^[A-Z]{3}$/;
        let error = '';

        if (!flight.destination) {
            error = 'The destination is required.';
        }
        else if (!pattern.test(flight.destination)) {
            error = 'The city must be 3 capital letters.';
        }

        setDestinationValidationError(error);

        return !error;
    };

    //The function returns if the flight's gate field passed validation.
    const validateGate = () => {
        let error = '';

        if (flight.gateID === 0) {
            error = 'The gate is required.';
        }

        setGateValidationError(error);

        return !error;
    };

    //The function returns if the flight's number field passed validation.
    const validateFlightNumber = () => {
        const pattern = /^([0-9]{4}|([0-9]{4}[A-Z]{1}))$/;
        let error = '';

        if (!flight.flightNumber) {
            error = 'The flight number is required.';
        }
        else if (!pattern.test(flight.flightNumber)) {
            error = 'The flight number must be 4 digits or 4 digits and a capital letter.';
        }

        setFlightNumberValidationError(error);

        return !error;
    };

    //The function returns if the flight's sort destination field passed validation.
    const validateSortDestination = () => {
        let error = '';

        if (flight.sortDestinationID === 0) {
            error = 'The sort destination is required.';
        }

        setSortDestinationValidationError(error);

        return !error;
    };

    //The footer for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="Cancel" icon="pi pi-times" outlined onClick={closeDialog} />
            <Button label="Save" icon="pi pi-check" onClick={() => isValid() && (newRecord ? addFlight(flight) : updateFlight(flight))} />
        </React.Fragment>
    );

    return (
        <>
            <Dialog breakpoints={{ '960px': '75vw', '641px': '90vw' }} className="p-fluid" footer={footer} header={newRecord ? 'Add Flight' : 'Update Flight' } modal style={{ width: '32rem' }} visible={visible} onHide={closeDialog}>
                <div className="field">
                    <label htmlFor="gate" className="font-bold">Gate</label>
                    <Dropdown id="gate" value={gates.find((element) => element.integer64ID == flight.gateID)} options={gates} optionLabel="name" filter placeholder="Select a gate for the flight" onBlur={(e) => validateGate()} onChange={(e) => setGate(e.value)} />
                    {gateValidationError && <small className="p-error">{gateValidationError}</small>}
                </div>
                <div className="field">
                    <label htmlFor="airline" className="font-bold">Airline</label>
                    <Dropdown id="airline" value={airlines.find((element) => element.integer64ID == flight.airlineID)} options={airlines} optionLabel="name" filter placeholder="Select an airline for the flight" onBlur={(e) => validateAirline()} onChange={(e) => setAirline(e.value)} />
                    {airlineValidationError && <small className="p-error">{airlineValidationError}</small>}
                </div>
                <div className="field">
                    <label htmlFor="flight-number" className="font-bold">Flight Number</label>
                    <InputText id="flight-number" value={flight.flightNumber} maxLength="5" keyfilter="alphanum" placeholder="Enter the flight number" onBlur={(e) => validateFlightNumber()} onChange={(e) => setFlightNumber(e.target.value)} />
                    {flightNumberValidationError && <small className="p-error">{flightNumberValidationError}</small>}
                </div>
                <div className="field">
                    <label htmlFor="destination" className="font-bold">Destination</label>
                    <InputText id="destination" value={flight.destination} maxLength="3" keyfilter="alpha" placeholder="Enter the destination for the flight" onBlur={(e) => validateDestination()} onChange={(e) => setDestination(e.target.value)} />
                    {destinationValidationError && <small className="p-error">{destinationValidationError}</small>}
                </div>
                <div className="field">
                    <label htmlFor="depart-time" className="font-bold">Depart Time</label>
                    <Calendar id="depart-time" value={flight.departTime} onChange={(e) => setDepartTime(e.value)} timeOnly />
                </div>
                <div className="field">
                    <label htmlFor="sort-destination" className="font-bold">Sort Destination</label>
                    <Dropdown id="sort-destination" value={sortDestinations.find((element) => element.integer64ID == flight.sortDestinationID)} options={sortDestinations} optionLabel="name" filter placeholder="Select a sort destination for the flight's baggage" onBlur={(e) => validateSortDestination()} onChange={(e) => setSortDestination(e.value)} />
                    {sortDestinationValidationError && <small className="p-error">{sortDestinationValidationError}</small>}
                </div>
            </Dialog>
        </>
    );
}

