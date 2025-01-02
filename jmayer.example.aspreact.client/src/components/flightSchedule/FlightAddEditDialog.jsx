import React, { useState, useEffect } from 'react'
import { Calendar } from 'primereact/calendar';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { useError } from '../errorDialog/ErrorProvider.jsx';
import ErrorDialog from '../errorDialog/ErrorDialog.jsx';

//Used to add or update a flight.
//@param {object} props The properties accepted by the component.
//@param {bool} props.newRecord Indicates if the flight object is a new record or not.
//@param {object} props.flight The flight to add or update.
//@param {function} props.refreshFlights Used to refresh the flights in the data table in the parent component.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function FlightAddEditDialog({ newRecord, flight, setFlight, refreshFlights, visible, hide }) {
    const [airlines, setAirlines] = useState([]);
    const [gates, setGates] = useState([]);
    const [sortDestinations, setSortDestinations] = useState([]);
    const [airlineValidationError, setAirlineValidationError] = useState('');
    const [gateValidationError, setGateValidationError] = useState('');
    const [destinationValidationError, setDestinationValidationError] = useState('');
    const [flightNumberValidationError, setFlightNumberValidationError] = useState('');
    const [sortDestinationValidationError, setSortDestinationValidationError] = useState('');
    const { showError } = useError();

    //When editing a flight, the server will return only the time which
    //javascript will represent as a string so the time string will need 
    //to be converted to a date for the Calendar control.
    if (typeof flight.departTime === 'string') {
        const options = { year: 'numeric', month: 'numeric', day: 'numeric' };
        setFlight({
            ...flight,
            departTime: new Date(Date.parse(new Date().toLocaleDateString('en-US', options) + " " + flight.departTime))
        })
    }

    //Load the airlines, gates & sort destinations when the component mounts.
    useEffect(() => {
        refreshAirlines();
        refreshGates();
        refreshSortDestinations();
    }, []);

    //Send a request asking the server to add the new flight to the database.
    const addFlight = () => {
        if (isValid()) {
            let tempFlight = prepFlightObject();

            fetch('api/Flight', {
                method: 'POST',
                body: JSON.stringify(tempFlight),
                headers: {
                    "Content-Type": "application/json",
                },
            })
                .then(response => {
                    if (response.ok) {
                        closeDialog();
                        refreshFlights();
                    }
                    else if (response.status == 400) {
                        response.json().then(serverSideValidationResult => processServerSideValidationResult(serverSideValidationResult));
                    }
                    else {
                        showError('Failed to create the flight because of an error on the server.');
                    }
                })
                .catch(error => showError('Failed to communicate with the server.'));
        }
    };

    //Clears the validation and closes the dialog.
    const closeDialog = () => {
        setAirlineValidationError('');
        setDestinationValidationError('');
        setGateValidationError('');
        setFlightNumberValidationError('');
        setSortDestinationValidationError('');
        hide();
    };

    //Validates all and returns a pass or fail.
    const isValid = () => {
        const airlinePass = newRecord && validateAirline();
        const destinationPass = validateDestination();
        const gatePass = validateGate();
        const flightNumberPass = newRecord && validateFlightNumber();
        const sortDestinaitonPass = validateSortDestination();

        if (newRecord) {
            return airlinePass && destinationPass && gatePass && flightNumberPass && sortDestinaitonPass;
        }
        else {
            return destinationPass && gatePass && sortDestinaitonPass;
        }
    };

    //Processes the server side validation result and sets any validation errors.
    //@param {object} serverSideValidationResult What the server found wrong with the user input.
    const processServerSideValidationResult = (serverSideValidationResult) => {
        if (Array.isArray(serverSideValidationResult.errors)) {
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
        }
        else {
            if (serverSideValidationResult.errors['Destination'] !== undefined) {
                setDestinationValidationError(serverSideValidationResult.errors['Destination'][0]);
            }

            if (serverSideValidationResult.errors['FlightNumber'] !== undefined) {
                setFlightNumberValidationError(serverSideValidationResult.errors['FlightNumber'][0]);
            }
        }
    };

    //Does data modification before sending the flight object to the server.
    const prepFlightObject = () => {
        const options = { hour12: false, hour: '2-digit', minute: '2-digit', second: '2-digit' };
        return {
            ...flight,
            //Only send the time to the server because the property on the server side is a C# TimeSpan.
            departTime: flight.departTime.toLocaleTimeString('en-US', options)
        };
    };

    //Refreshes the airlines for the dropdown.
    const refreshAirlines = () => {
        fetch('api/Airline/All')
            .then(response => response.json())
            .then(json => setAirlines(json))
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //Refreshes the gates for the dropdown.
    const refreshGates = () => {
        fetch('api/Gate/All')
            .then(response => response.json())
            .then(json => setGates(json))
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //Refreshes the sort destinations for the dropdown.
    const refreshSortDestinations = () => {
        fetch('api/SortDestination/All')
            .then(response => response.json())
            .then(json => setSortDestinations(json))
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //Updates the airline field with the value selected by the user.
    //@param {object} airline The airline the user selected.
    const setAirline = (airline) => {
        setFlight({
            ...flight,
            airlineID: airline.integer64ID,
            airlineIATACode: airline.iata,
            name: airline.iata + flight.flightNumber
        });
    };

    //Updates the depart time field with the value entered by the user.
    //@param {date} departTime The depart time the user entered.
    const setDepartTime = (departTime) => {
        setFlight({
            ...flight,
            departTime: departTime
        });
    };

    //Updates the destination field with the value entered by the user.
    //@param {string} destination The destination the user entered.
    const setDestination = (destination) => {
        setFlight({
            ...flight,
            destination: destination.toUpperCase()
        });
    };

    //Updates the flight number field with the value entered by the user.
    //@param {string} flightNumber The flight number the user entered.
    const setFlightNumber = (flightNumber) => {
        setFlight({
            ...flight,
            flightNumber: flightNumber.toUpperCase(),
            name: flight.airlineIATACode + flightNumber.toUpperCase()
        });
    };

    //Updates the gate field with the value selected by the user.
    //@param {object} gate The flight number the user selected.
    const setGate = (gate) => {
        setFlight({
            ...flight,
            gateID: gate.integer64ID,
            gateName: gate.name,
        });
    };

    //Updates the sort destination field with the value selected by the user.
    //@param {sortDestination} gate The sort desstination the user selected.
    const setSortDestination = (sortDestination) => {
        setFlight({
            ...flight,
            sortDestinationID: sortDestination.integer64ID,
            sortDestinationName: sortDestination.name
        });
    };

    //Send a request asking the server to update an existing flight in the database.
    const updateFlight = () => {
        if (isValid()) {
            let tempFlight = prepFlightObject();

            fetch('api/Flight', {
                method: 'PUT',
                body: JSON.stringify(tempFlight),
                headers: {
                    "Content-Type": "application/json",
                },
            })
                .then(response => {
                    if (response.ok) {
                        closeDialog();
                        refreshFlights();
                    }
                    else if (response.status == 400) {
                        response.json().then(serverSideValidationResult => processServerSideValidationResult(serverSideValidationResult));
                    }
                    else if (response.status == 409) {
                        showError('The submitted data was detected to be out of date; please refresh and try again.');
                    }
                    else {
                        showError('Failed to update the flight because of an error on the server.');
                    }
                })
                .catch(error => showError('Failed to communicate with the server.'));
        }
    };

    //Validates the flight's airline and returns a pass or fail.
    const validateAirline = () => {
        let error = '';

        if (flight.airlineID === 0) {
            error = 'The airline is required.';
        }

        setAirlineValidationError(error);

        return !error;
    };

    //Validates the flight's destination and returns a pass or fail.
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

    //Validates the flight's gate and returns a pass or fail.
    const validateGate = () => {
        let error = '';

        if (flight.gateID === 0) {
            error = 'The gate is required.';
        }

        setGateValidationError(error);

        return !error;
    };

    //Validates the flight's number and returns a pass or fail.
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

    //Validates the flight's sort destination and returns a pass or fail.
    const validateSortDestination = () => {
        let error = '';

        if (flight.sortDestinationID === 0) {
            error = 'The sort destination is required.';
        }

        setSortDestinationValidationError(error);

        return !error;
    };

    //Define the footer for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="Cancel" icon="pi pi-times" outlined onClick={closeDialog} />
            <Button label="Save" icon="pi pi-check" onClick={() => newRecord ? addFlight() : updateFlight()} />
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

            <ErrorDialog />
        </>
    );
}

