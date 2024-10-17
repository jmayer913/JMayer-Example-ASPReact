import React from 'react'
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';

//Used to delete an airline; user must confirm first.
//@param {object} props The properties accepted by the component.
//@param {object} props.airline The airline to be deleted on user confirmation.
//@param {function} props.refreshAirlines Used to refresh the airlines in the data table in the parent component.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function AirlineDeleteConfirmDialog({ airline, refreshAirlines, visible, hide }) {
    //Send a request asking the server to delete a specific airline from the database.
    //This is done only after the user has confirmed the deletion.
    const deleteAirline = () => {
        fetch('/api/Airline/' + airline.integer64ID, { method: 'DELETE' })
            .then(() => {
                hide();
                refreshAirlines();
            })
            .catch(error => {
                //TO DO: Add error handling.
            });
    };

    //Define the footer for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="No" icon="pi pi-times" outlined onClick={() => hide()} />
            <Button label="Yes" icon="pi pi-check" severity="danger" onClick={deleteAirline} />
        </React.Fragment>
    );

    return (
        <Dialog breakpoints={{ '960px': '75vw', '641px': '90vw' }} footer={footer} header="Confirm Deletion" modal style={{ width: '32rem' }} visible={visible} onHide={hide}>
            <div className="confirmation-content">
                <i className="pi pi-exclamation-triangle mr-3" style={{ fontSize: '2rem' }} />
                {airline && <span>Are you sure you want to delete <b>{airline.name}</b>?</span>}
            </div>
        </Dialog>
    );
}