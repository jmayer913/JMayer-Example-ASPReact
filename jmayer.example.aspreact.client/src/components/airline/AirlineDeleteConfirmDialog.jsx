import React from 'react'
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';

//Used to delete an airline; user must confirm first.
//@param {object} props The properties accepted by the component.
//@param {object} props.airline The airline to be deleted on user confirmation.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function AirlineDeleteConfirmDialog({ airline, visible, hide }) {
    //Sends a request to the server to delete the airline.
    //This is done only after the user has confirmed the deletion.
    const deleteAirline = () => {
        fetch('/api/Airline/' + airline.integer64ID, { method: 'DELETE' })
            .then(() => {
                hide();
                //TO DO: Refresh the data table.
            })
            .catch(error => {
                //TO DO: Add error handling.
            });
    };

    //The footer to be displayed on the dialog.
    const footer = (
        <React.Fragment>
            <Button label="No" icon="pi pi-times" outlined onClick={() => hide()} />
            <Button label="Yes" icon="pi pi-check" severity="danger" onClick={deleteAirline} />
        </React.Fragment>
    );

    return (
        <Dialog breakpoints={{ '960px': '75vw', '641px': '90vw' }} footer={footer} header="Confirm" modal style={{ width: '32rem' }} visible={visible} onHide={() => hide()}>
            <div className="confirmation-content">
                <i className="pi pi-exclamation-triangle mr-3" style={{ fontSize: '2rem' }}>
                    {airline && <span>Are you sure you want to delete <b>{airline.name}</b>?</span>}
                </i>
            </div>
        </Dialog>
    );
}