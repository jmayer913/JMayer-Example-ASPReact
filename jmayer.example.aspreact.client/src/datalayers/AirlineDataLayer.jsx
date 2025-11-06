import { useState } from 'react';
import { useError } from '../components/errorDialog/ErrorProvider.jsx';
import { shapeValidationResult } from './DataLayerHelper.jsx';

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
    const [addAirlineServerSideResult, setAddAirlineServerSideResult] = useState(null);
    const [deleteAirlineSuccess, setDeleteAirlineSuccess] = useState(false);
    const [updateAirlineSuccess, setUpdateAirlineSuccess] = useState(false);
    const [updateAirlineServerSideResult, setUpdateAirlineServerSideResult] = useState(null);

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
                else if (response.status == 400) {
                    response.json().then(serverSideValidationResult => setAddAirlineServerSideResult(shapeValidationResult(serverSideValidationResult)));
                }
                else {
                    showError('Failed to create the airline because of an error on the server.');
                }
            })
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //The function clears the states before an operation.
    const clearStates = () => {
        setAddAirlineServerSideResult(null);
        setAddAirlineSuccess(false);
        setDeleteAirlineSuccess(false);
        setUpdateAirlineServerSideResult(null);
        setUpdateAirlineSuccess(false);
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
                else if (response.status == 400) {
                    response.json().then(serverSideValidationResult => setUpdateAirlineServerSideResult(shapeValidationResult(serverSideValidationResult)));
                }
                else if (response.status == 409) {
                    showError('The submitted data was detected to be out of date; please refresh and try again.');
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
        addAirlineServerSideResult,
        addAirlineSuccess,
        deleteAirline,
        deleteAirlineSuccess,
        updateAirline,
        updateAirlineServerSideResult,
        updateAirlineSuccess
    };
};