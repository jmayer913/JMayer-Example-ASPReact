import React, { useState, useEffect } from 'react';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { useAirlineDataLayer } from '../../datalayers/AirlineDataLayer.jsx';
import { useSortDestinationDataLayer } from '../../datalayers/SortDestinationDataLayer.jsx';

//The function returns the dialog for adding or updating an airline.
//@param {object} props The properties accepted by the component.
//@param {bool} props.newRecord Indicates if the airline object is a new record or not.
//@param {object} props.airline The airline to add or update.
//@param {function} props.setAirline Used to update the state of the airline object.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function AirlineAddEditDialog({ newRecord, airline, setAirline, visible, hide }) {
    const [iataValidationError, setIataValidationError] = useState('');
    const [icaoValidationError, setIcaoValidationError] = useState('');
    const [nameValidationError, setNameValidationError] = useState('');
    const [numberCodeValidationError, setNumberCodeValidationError] = useState('');
    const [sortDestinationValidationError, setSortDestinationValidationError] = useState('');
    const { addAirline, addAirlineServerSideResult, addAirlineSuccess, updateAirline, updateAirlineServerSideResult, updateAirlineSuccess } = useAirlineDataLayer();
    const { sortDestinations, getSortDestinations } = useSortDestinationDataLayer();

    //Load the destinations when the component mounts.
    useEffect(() => {
        getSortDestinations();
    }, []);

    //Handle state changes based on add/update operations.
    useEffect(() => {
        //Hide the dialog on a successful add or update.
        if (addAirlineSuccess || updateAirlineSuccess) {
            hide();
        }

        //Handle displays server side errors.
        if (addAirlineServerSideResult !== null) {
            processServerSideValidationResult(addAirlineServerSideResult);
        }
        else if (updateAirlineServerSideResult !== null) {
            processServerSideValidationResult(updateAirlineServerSideResult);
        }

    }, [addAirlineServerSideResult, addAirlineSuccess, updateAirlineServerSideResult, updateAirlineSuccess]);

    //The function clears the validation and closes the dialog.
    const closeDialog = () => {
        setIataValidationError('');
        setIcaoValidationError('');
        setNameValidationError('');
        setNumberCodeValidationError('');
        setSortDestinationValidationError('');
        hide();
    };

    //The funciton returns if validation passed or failed.
    const isValid = () => {
        const iataPass = validateIATA();
        const icoaPass = validateICOA();
        const namePass = validateName();
        const numberCodePass = validateNumberCode();
        const sortDestinationPass = validateSortDestination();

        return iataPass && icoaPass && namePass && numberCodePass && sortDestinationPass;
    };

    //The function processes the server side validation result and sets any validation errors.
    //@param {object} serverSideValidationResult What the server found wrong with the user input.
    const processServerSideValidationResult = (serverSideValidationResult) => {
        for (const error of serverSideValidationResult.errors) {
            switch (error.propertyName) {
                case 'Name':
                    setNameValidationError(error.errorMessage);
                    break;
                case 'IATA':
                    setIataValidationError(error.errorMessage);
                    break;
                case 'ICAO':
                    setIcaoValidationError(error.errorMessage);
                    break;
                case 'NumberCode':
                    setNumberCodeValidationError(error.errorMessage);
                    break;
                case 'SortDestinationID':
                    setSortDestinationValidationError(error.errorMessage);
                    break;
            }
        }
    };

    //The function updates the description field with the value entered by the user.
    //@param {string} value The new description value.
    const setDescription = (value) => {
        setAirline({
            ...airline,
            description: value
        });
    };

    //The function updates the iata field with the value entered by the user.
    //@param {string} value The new iata value.
    const setIATA = (value) => {
        setAirline({
            ...airline,
            iata: value.toUpperCase()
        });
    };

    //The function updates the icao field with the value entered by the user.
    //@param {string} value The new icao value.
    const setICAO = (value) => {
        setAirline({
            ...airline,
            icao: value.toUpperCase()
        });
    };

    //The function updates the name field with the value entered by the user.
    //@param {string} value The new name value.
    const setName = (value) => {
        setAirline({
            ...airline,
            name: value
        });
    };

    //The function updates the number code field with the value entered by the user.
    //@param {string} value The new number code value.
    const setNumberCode = (value) => {
        setAirline({
            ...airline,
            numberCode: value
        });
    };

    //The function updates the sort destination field with the value selected by the user.
    //@param {sortDestination} sortDestination The sort desstination the user selected.
    const setSortDestination = (sortDestination) => {
        setAirline({
            ...airline,
            sortDestinationID: sortDestination.integer64ID,
            sortDestinationName: sortDestination.name
        });
    }

    //The function returns if the airline's IATA field passed validation.
    const validateIATA = () => {
        const pattern = /^[A-Z0-9]{2}$/;
        let error = '';

        if (!airline.iata) {
            error = 'The IATA is required.';
        }
        else if (!pattern.test(airline.iata)) {
            error = 'The IATA must be 2 alphanumeric characters.';
        }

        setIataValidationError(error);

        return !error;
    };

    //The function returns if the airline's ICAO field passed validation.
    const validateICOA = () => {
        const pattern = /^[A-Z]{3}$/;
        let error = '';

        if (!airline.icao) {
            error = 'The ICAO is required.';
        }
        else if (!pattern.test(airline.icao)) {
            error = 'The ICAO must be 3 letters.';
        }

        setIcaoValidationError(error);

        return !error;
    };

    //The function returns if the airline's name field passed validation.
    const validateName = () => {
        let error = '';

        if (!airline.name || !airline.name.trim()) {
            error = 'The name is required.';
        }

        setNameValidationError(error);

        return !error;
    };

    //The function returns if the airline's number code field passed validation.
    const validateNumberCode = () => {
        const pattern = /^[0-9]{3}$/;
        let error = '';

        if (!airline.numberCode) {
            error = 'The number code is required.';
        }
        else if (!pattern.test(airline.numberCode)) {
            error = 'The number code must be 3 digits.';
        }

        setNumberCodeValidationError(error);

        return !error;
    };

    //The function returns if the airline's sort destination field passed validation.
    const validateSortDestination = () => {
        let error = '';

        if (airline.sortDestinationID === 0) {
            error = 'The sort destination is required.';
        }

        setSortDestinationValidationError(error);

        return !error;
    };

    //The footer for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="Cancel" icon="pi pi-times" outlined onClick={closeDialog} />
            <Button label="Save" icon="pi pi-check" onClick={() => isValid() && (newRecord ? addAirline(airline) : updateAirline(airline))} />
        </React.Fragment>
    );

    return (
        <>
            <Dialog breakpoints={{ '960px': '75vw', '641px': '90vw' }} className="p-fluid" footer={footer} header={newRecord ? 'Add Airline' : 'Edit Airline'} modal style={{ width: '32rem' }} visible={visible} onHide={closeDialog}>
                <div className="field">
                    <label htmlFor="name" className="font-bold">Name</label>
                    <InputText id="name" value={airline.name} placeholder="Enter a name for the airline" onBlur={(e) => validateName()} onChange={(e) => setName(e.target.value)} />
                    {nameValidationError && <small className="p-error">{nameValidationError}</small>}
                </div>
                <div className="field">
                    <label htmlFor="description" className="font-bold">Description</label>
                    <InputText id="description" value={airline.description} placeholder="Optionally enter a description for the airline" onChange={(e) => setDescription(e.target.value)} />
                </div>
                <div className="field">
                    <label htmlFor="iata" className="font-bold">IATA</label>
                    <InputText id="iata" value={airline.iata} maxLength="2" keyfilter="alphanum" placeholder="Enter the 2 alphanumeric code" onBlur={(e) => validateIATA()} onChange={(e) => setIATA(e.target.value)} />
                    {iataValidationError && <small className="p-error">{iataValidationError}</small>}
                </div>
                <div className="field">
                    <label htmlFor="icao" className="font-bold">ICAO</label>
                    <InputText id="icao" value={airline.icao} maxLength="3" keyfilter="alpha" placeholder="Enter the 3 letter code" onBlur={(e) => validateICOA()} onChange={(e) => setICAO(e.target.value)} />
                    {icaoValidationError && <small className="p-error">{icaoValidationError}</small>}
                </div>
                <div className="field">
                    <label htmlFor="number-code" className="font-bold">Number Code</label>
                    <InputText id="number-code" value={airline.numberCode} maxLength="3" keyfilter="int" placeholder="Enter the 3 digit code" onBlur={(e) => validateNumberCode()} onChange={(e) => setNumberCode(e.target.value)} />
                    {numberCodeValidationError && <small className="p-error">{numberCodeValidationError}</small>}
                </div>
                <div className="field">
                    <label htmlFor="sort-destination" className="font-bold">Sort Destination</label>
                    <Dropdown id="sort-destination" value={sortDestinations.find((element) => element.integer64ID == airline.sortDestinationID)} options={sortDestinations} optionLabel="name" filter placeholder="Select a default sort destination for the airline's baggage" onBlur={(e) => validateSortDestination()} onChange={(e) => setSortDestination(e.value)} />
                    {sortDestinationValidationError && <small className="p-error">{sortDestinationValidationError}</small>}
                </div>
            </Dialog>
        </>
    );
}