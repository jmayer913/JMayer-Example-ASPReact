import { useState } from 'react';
import { useError } from '../components/errorDialog/ErrorProvider.jsx';

//Defines the initial airline object.
const initialAirline = {
    name: '',
    description: '',
    iata: '',
    icao: '',
    numberCode: '',
    sortDestinationID: 0,
    sortDestinationName: '',
};
export default initialAirline;

//The function returns a hook to interact with the server for the airlines.
export function useAirlineDataLayer() {
    const { showError } = useError();
    const [airlines, setAirlines] = useState([]);
    const [addAirlineSuccess, setAddAirlineSuccess] = useState(false);
    const [addAirlineValidationProblemDetails, setAddAirlineValidationProblemDetails] = useState(null);
    const [deleteAirlineSuccess, setDeleteAirlineSuccess] = useState(false);
    const [updateAirlineSuccess, setUpdateAirlineSuccess] = useState(false);
    const [updateAirlineValidationProblemDetails, setUpdateAirlineValidationProblemDetails] = useState(null);

    //The function adds an airline to the server.
    //@param {object} airline The airline to add.
    const addAirline = (airline) => {
        clearStates();

        fetch('api/Airline', {
            method: 'POST',
            body: JSON.stringify(airline),
            headers: {
                "Content-Type": "application/json",
            },
        })
            .then(response => {
                if (response.ok) {
                    setAddAirlineSuccess(true);
                }
                else if (response.status === 400) {
                    response.json().then(validationProblemDetails => setAddAirlineValidationProblemDetails(validationProblemDetails));
                }
                else if (response.status === 500) {
                    response.json().then(problemDetails => showError(problemDetails.detail));
                }
                else {
                    showError('Failed to create the airline because of an error on the server.');
                }
            })
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //The function clears the states before an operation.
    const clearStates = () => {
        setAddAirlineSuccess(false);
        setAddAirlineValidationProblemDetails(null);
        setDeleteAirlineSuccess(false);
        setUpdateAirlineSuccess(false);
        setUpdateAirlineValidationProblemDetails(null);
    };

    //The function deletes an airline from the server.
    //@param {object} airline The airline to delete.
    const deleteAirline = (airline) => {
        clearStates();

        fetch('/api/Airline/' + airline.integer64ID, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    setDeleteAirlineSuccess(true);
                }
                else if (response.status === 500) {
                    response.json().then(problemDetails => showError(problemDetails.detail));
                }
                else {
                    showError('Failed to delete the airline because of an error on the server.');
                }
            })
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //The function retrieves the airlines from the server.
    const getAirlines = () => {
        fetch('api/Airline/All')
            .then(response => response.json())
            .then(json => setAirlines(json))
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //The function updates an airline on the server.
    //@param {object} airline The airline to update.
    const updateAirline = (airline) => {
        clearStates();

        fetch('api/Airline', {
            method: 'PUT',
            body: JSON.stringify(airline),
            headers: {
                "Content-Type": "application/json",
            },
        })
            .then(response => {
                if (response.ok) {
                    setUpdateAirlineSuccess(true);
                }
                else if (response.status === 400) {
                    response.json().then(validationProblemDetails => setUpdateAirlineValidationProblemDetails(validationProblemDetails));
                }
                else if (response.status == 409 || response.status === 500) {
                    response.json().then(problemDetails => showError(problemDetails.detail));
                }
                else {
                    showError('Failed to update the airline because of an error on the server.');
                }
            })
            .catch(error => showError('Failed to communicate with the server.'));
    };

    return {
        airlines,
        getAirlines,
        addAirline,
        addAirlineValidationProblemDetails,
        addAirlineSuccess,
        deleteAirline,
        deleteAirlineSuccess,
        updateAirline,
        updateAirlineValidationProblemDetails,
        updateAirlineSuccess
    };
};