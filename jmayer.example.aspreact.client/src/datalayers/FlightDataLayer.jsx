import { useState } from 'react';
import { useError } from '../components/errorDialog/ErrorProvider.jsx';
import { shapeValidationResult } from './DataLayerHelper.jsx';

//Defines the initial flight object.
const initialFlight = {
    airlineID: 0,
    codeShares: [],
    description: '',
    departTime: new Date(new Date().setHours(0, 0, 0, 0)),
    destination: '',
    flightNumber: '',
    gateID: 0,
    gateName: '',
    name: '',
    sortDestinationID: 0,
    sortDestinationName: '',
};
export default initialFlight;

//The function returns a hook to interact with the server for the flights.
export function useFlightDataLayer() {
    const { showError } = useError();
    const [flights, setFlights] = useState([])
    const [addFlightServerSideResult, setAddFlightServerSideResult] = useState(null);
    const [addFlightSuccess, setAddFlightSuccess] = useState(false);
    const [deleteFlightSuccess, setDeleteFlightSuccess] = useState(false);
    const [updateFlightServerSideResult, setUpdateFlightServerSideResult] = useState(null);
    const [updateFlightSuccess, setUpdateFlightSuccess] = useState(false);

    //The function adds a flight to the server.
    //@param {object} flight The flight to add.
    const addFlight = (flight) => {
        clearStates();

        let prepFlight = prepFlightObject(flight);

        fetch('api/Flight', {
            method: 'POST',
            body: JSON.stringify(prepFlight),
            headers: {
                "Content-Type": "application/json",
            },
        })
            .then(response => {
                if (response.ok) {
                    setAddFlightSuccess(true);
                }
                else if (response.status == 400) {
                    response.json().then(serverSideValidationResult => setAddFlightServerSideResult(shapeValidationResult(serverSideValidationResult)));
                }
                else {
                    showError('Failed to create the flight because of an error on the server.');
                }
            })
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //The function clears the states before an operation.
    const clearStates = () => {
        setAddFlightServerSideResult(null);
        setAddFlightSuccess(false);
        setDeleteFlightSuccess(false);
        setUpdateFlightServerSideResult(null);
        setUpdateFlightSuccess(false);
    };

    //The function deletes a flight from the server.
    //@param {object} flight The flight to delete.
    const deleteFlight = (flight) => {
        clearStates();

        fetch('/api/Flight/' + flight.integer64ID, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    setDeleteFlightSuccess(true);
                }
                else {
                    showError('Failed to delete the flight because of an error on the server.');
                }
            })
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //The function retrieves the flights from the server.
    const getFlights = () => {
        fetch('api/Flight/All')
            .then(response => response.json())
            .then(json => setFlights(json))
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //The function prepares the flight object before being sent to the server.
    const prepFlightObject = (flight) => {
        const options = { hour12: false, hour: '2-digit', minute: '2-digit', second: '2-digit' };
        return {
            ...flight,
            //Only send the time to the server because the property on the server side is a C# TimeSpan.
            departTime: flight.departTime.toLocaleTimeString('en-US', options)
        };
    };

    //The function updates a flight to the server.
    //@param {object} flight The flight to update.
    const updateFlight = (flight) => {
        clearStates();

        let prepFlight = prepFlightObject(flight);

        fetch('api/Flight', {
            method: 'PUT',
            body: JSON.stringify(prepFlight),
            headers: {
                "Content-Type": "application/json",
            },
        })
            .then(response => {
                if (response.ok) {
                    setUpdateFlightSuccess(true);
                }
                else if (response.status == 400) {
                    response.json().then(serverSideValidationResult => setUpdateFlightServerSideResult(shapeValidationResult(serverSideValidationResult)));
                }
                else if (response.status == 409) {
                    showError('The submitted data was detected to be out of date; please refresh and try again.');
                }
                else {
                    showError('Failed to update the flight because of an error on the server.');
                }
            })
            .catch(error => showError('Failed to communicate with the server.'));
    };

    return {
        flights,
        getFlights,
        addFlight,
        addFlightServerSideResult,
        addFlightSuccess,
        deleteFlight,
        deleteFlightSuccess,
        updateFlight,
        updateFlightServerSideResult,
        updateFlightSuccess
    };
};