import { useState } from 'react';
import { useError } from '../components/errorDialog/ErrorProvider.jsx';

//The function returns a hook to interact with the server for the sort destinations.
export function useSortDestinationDataLayer() {
    const [sortDestinations, setSortDestinations] = useState([]);
    const { showError } = useError();

    //The function retrieves the sort destinations from the server.
    const getSortDestinations = () => {
        fetch('api/SortDestination/All')
            .then(response => response.json())
            .then(json => setSortDestinations(json))
            .catch(error => showError('Failed to communicate with the server.'));
    };

    return { sortDestinations, getSortDestinations };
};