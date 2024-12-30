import React, { useState } from 'react'
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import ErrorDialog from '../errorDialog/ErrorDialog.jsx'

//Used to delete an airline; user must confirm first.
//@param {object} props The properties accepted by the component.
//@param {object} props.airline The airline to be deleted on user confirmation.
//@param {function} props.refreshAirlines Used to refresh the airlines in the data table in the parent component.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function AirlineDeleteConfirmDialog({ airline, refreshAirlines, visible, hide }) {
    const [errorDialogVisible, setErrorDialogVisible] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');

    //Send a request asking the server to delete a specific airline from the database.
    //This is done only after the user has confirmed the deletion.
    const deleteAirline = () => {
        fetch('/api/Airline/' + airline.integer64ID, { method: 'DELETE' })
            .then(response => {
                if (response.ok) {
                    hide();
                    refreshAirlines();
                }
                else {
                    openErrorDialog('Failed to delete the airline because of an error on the server.');
                }
            })
            .catch(error => {
                openErrorDialog('Failed to communicate with the server.');
            });
    };

    //Hides the error dialog.
    const hideErrorDialog = () => {
        setErrorDialogVisible(false);
    };

    //Opens the error dialog.
    //@param {string} error The error to display to the user.
    const openErrorDialog = (error) => {
        setErrorMessage(error);
        setErrorDialogVisible(true);
    };

    //Define the footer for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="No" icon="pi pi-times" outlined onClick={hide} />
            <Button label="Yes" icon="pi pi-check" severity="danger" onClick={deleteAirline} />
        </React.Fragment>
    );

    return (
        <>
            <Dialog breakpoints={{ '960px': '75vw', '641px': '90vw' }} footer={footer} header="Confirm Deletion" modal style={{ width: '32rem' }} visible={visible} onHide={hide}>
                <div className="confirmation-content">
                    <i className="pi pi-exclamation-triangle mr-3" style={{ fontSize: '2rem' }} />
                    {airline && <span>Are you sure you want to delete <b>{airline.name}</b>? Flights for this airline will be deleted too.</span>}
                </div>
            </Dialog>

            <ErrorDialog errorMessage={errorMessage} visible={errorDialogVisible} hide={hideErrorDialog} />
        </>
    );
}