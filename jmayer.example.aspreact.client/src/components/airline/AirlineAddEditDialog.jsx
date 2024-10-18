import React, { useState } from 'react'
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { InputText } from 'primereact/inputtext';

//Used to add or update an airline.
//@param {object} props The properties accepted by the component.
//@param {bool} props.newRecord Indicates if the airline object is a new record or not.
//@param {object} props.airline The airline to add or update.
//@param {function} props.setAirline Used to update the state of the airline object.
//@param {function} props.refreshAirlines Used to refresh the airlines in the data table in the parent component.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function AirlineAddEditDialog({ newRecord, airline, setAirline, refreshAirlines, visible, hide }) {
    const [iataValidationError, setIataValidationError] = useState('');
    const [icaoValidationError, setIcaoValidationError] = useState('');
    const [nameValidationError, setNameValidationError] = useState('');
    const [numberCodeValidationError, setNumberCodeValidationError] = useState('');

    //Send a request asking the server to add the new airline to the database.
    const addAirline = () => {
        if (isValid()) {
            fetch('api/Airline', {
                method: 'POST',
                body: JSON.stringify(airline),
                headers: {
                    "Content-Type": "application/json",
                },
            })
            .then(response => {
                if (response.ok) {
                    closeDialog();
                    refreshAirlines();
                }
                else if (response.status == 400) {
                    response.json().then(serverSideValidationResult => processServerSideValidationResult(serverSideValidationResult));
                }
            })
            .catch(error => {
                //TO DO: Add error handling.
            });
        }
    };

    //Clears the validation and closes the dialog.
    const closeDialog = () => {
        setIataValidationError('');
        setIcaoValidationError('');
        setNameValidationError('');
        setNumberCodeValidationError('');
        hide();
    };

    //Validates all and returns a pass or fail.
    const isValid = () => {
        const iataPass = validateIATA();
        const icoaPass = validateICOA();
        const namePass = validateName();
        const numberCodePass = validateNumberCode();

        return iataPass && icoaPass && namePass && numberCodePass;
    }

    //Processes the server side validation result and sets any validation errors.
    const processServerSideValidationResult = (serverSideValidationResult) => {
        if (Array.isArray(serverSideValidationResult.errors)) {
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
                }
            }
        }
        else {
            if (serverSideValidationResult.errors['Name'] !== undefined) {
                setNameValidationError(serverSideValidationResult.errors['Name'][0]);
            }

            if (serverSideValidationResult.errors['IATA'] !== undefined) {
                setIataValidationError(serverSideValidationResult.errors['IATA'][0]);
            }

            if (serverSideValidationResult.errors['ICAO'] !== undefined) {
                setIcaoValidationError(serverSideValidationResult.errors['ICAO'][0]);
            }

            if (serverSideValidationResult.errors['NumberCode'] !== undefined) {
                setNumberCodeValidationError(serverSideValidationResult.errors['NumberCode'][0]);
            }
        }
    };

    //Updates the description field with the value entered by the user.
    //@param {string} The new description value.
    const setDescription = (value) => {
        let tempAirline = { ...airline };
        tempAirline.description = value;
        setAirline(tempAirline);
    };

    //Updates the iata field with the value entered by the user.
    //@param {string} The new iata value.
    const setIATA = (value) => {
        let tempAirline = { ...airline };
        tempAirline.iata = value.toUpperCase();
        setAirline(tempAirline);
    };

    //Updates the icao field with the value entered by the user.
    //@param {string} The new icao value.
    const setICAO = (value) => {
        let tempAirline = { ...airline };
        tempAirline.icao = value.toUpperCase();
        setAirline(tempAirline);
    };

    //Updates the name field with the value entered by the user.
    //@param {string} The new name value.
    const setName = (value) => {
        let tempAirline = { ...airline };
        tempAirline.name = value;
        setAirline(tempAirline);
    };

    //Updates the number code field with the value entered by the user.
    //@param {string} The new number code value.
    const setNumberCode = (value) => {
        let tempAirline = { ...airline };
        tempAirline.numberCode = value;
        setAirline(tempAirline);
    };

    //Send a request asking the server to update an existing airline in the database.
    const updateAirline = () => {
        if (isValid()) {
            fetch('api/Airline', {
                method: 'PUT',
                body: JSON.stringify(airline),
                headers: {
                    "Content-Type": "application/json",
                },
            })
            .then(response => {
                if (response.ok) {
                    closeDialog();
                    refreshAirlines();
                }
                else if (response.status == 400) {
                    response.json().then(serverSideValidationResult => processServerSideValidationResult(serverSideValidationResult));
                }
                else if (response.status == 409) {
                    //TO DO: Display a conflict message on a conflict.
                }
            })
            .catch(error => {
                //TO DO: Add error handling.
            });
        }
    };

    //Validates the airline's IATA and returns a pass or fail.
    const validateIATA = () => {
        const iataPattern = /^[A-Z0-9]{2}$/;
        let error = '';

        if (!airline.iata) {
            error = 'The IATA is required.';
        }
        else if (!iataPattern.test(airline.iata)) {
            error = 'The IATA must be 2 alphanumeric characters.';
        }

        setIataValidationError(error);

        return !error;
    }

    //Validates the airline's ICAO and returns a pass or fail.
    const validateICOA = () => {
        const icoaPattern = /^[A-Z]{3}$/;
        let error = '';

        if (!airline.icao) {
            error = 'The ICAO is required.';
        }
        else if (!icoaPattern.test(airline.icao)) {
            error = 'The ICAO must be 3 letters.';
        }

        setIcaoValidationError(error);

        return !error;
    }

    //Validates the airline's name and returns a pass or fail.
    const validateName = () => {
        let error = '';

        if (!airline.name || !airline.name.trim()) {
            error = 'The name is required.';
        }

        setNameValidationError(error);

        return !error;
    }

    //Validates the airline's number code and returns a pass or fail.
    const validateNumberCode = () => {
        const numberCodePattern = /^[0-9]{3}$/;
        let error = '';

        if (!airline.numberCode) {
            error = 'The number code is required.';
        }
        else if (!numberCodePattern.test(airline.numberCode)) {
            error = 'The number code must be 3 digits.';
        }

        setNumberCodeValidationError(error);

        return !error;
    }

    //Define the footer for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="Cancel" icon="pi pi-times" outlined onClick={closeDialog} />
            <Button label="Save" icon="pi pi-check" onClick={() => newRecord ? addAirline() : updateAirline()} />
        </React.Fragment>
    );

    return (
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
        </Dialog>
    );
}