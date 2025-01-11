import { useState } from 'react';
import { useError } from '../components/errorDialog/ErrorProvider.jsx';

//The function returns a hook to interact with the server for the gates.
export function useGateDataLayer() {
    const [gates, setGates] = useState([]);
    const { showError } = useError();

    //The function retrieves the gates from the server.
    const getGates = () => {
        fetch('api/Gate/All')
            .then(response => response.json())
            .then(json => setGates(json))
            .catch(error => showError('Failed to communicate with the server.'));
    };

    return { gates, getGates };
};