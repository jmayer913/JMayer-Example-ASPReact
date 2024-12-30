import React from 'react'
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { useError } from '../errorDialog/ErrorProvider.jsx';
import ErrorDialog from '../errorDialog/ErrorDialog.jsx';

//Used to delete a flight; user must confirm first.
//@param {object} props The properties accepted by the component.
//@param {object} props.flight The flight to be deleted on user confirmation.
//@param {function} props.refreshFlights Used to refresh the flights in the data table in the parent component.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function FlightDeleteConfirmDialog({ flight, refreshFlights, visible, hide }) {
    const { showError } = useError();

    //Send a request asking the server to delete a specific flight from the database.
    //This is done only after the user has confirmed the deletion.
    const deleteFlight = () => {
        fetch('/api/Flight/' + flight.integer64ID, { method: 'DELETE' })
            .then(response => {
                if (response.ok) {
                    hide();
                    refreshFlights();
                }
                else {
                    showError('Failed to delete the flight because of an error on the server.');
                }
            })
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //Define the footer for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="No" icon="pi pi-times" outlined onClick={hide} />
            <Button label="Yes" icon="pi pi-check" severity="danger" onClick={deleteFlight} />
        </React.Fragment>
    );

    return (
        <>
            <Dialog breakpoints={{ '960px': '75vw', '641px': '90vw' }} footer={footer} header="Confirm Deletion" modal style={{ width: '32rem' }} visible={visible} onHide={hide}>
                <div className="confirmation-content">
                    <i className="pi pi-exclamation-triangle mr-3" style={{ fontSize: '2rem' }} />
                    {flight && <span>Are you sure you want to delete <b>{flight.name}</b>?</span>}
                </div>
            </Dialog>

            <ErrorDialog />
        </>
    );
}