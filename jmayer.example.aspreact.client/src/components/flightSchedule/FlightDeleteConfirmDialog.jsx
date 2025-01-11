import React, { useEffect } from 'react';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { useFlightDataLayer } from '../../datalayers/FlightDataLayer.jsx';

//The function returns the dialog for deleting a flight.
//@param {object} props The properties accepted by the component.
//@param {object} props.flight The flight to be deleted on user confirmation.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function FlightDeleteConfirmDialog({ flight, visible, hide }) {
    const { deleteFlight, deleteFlightSuccess } = useFlightDataLayer();

    useEffect(() => {
        //Hide the dialog on a successful delete.
        if (deleteFlightSuccess) {
            hide();
        }
    }, [deleteFlightSuccess]);

    //The footer content for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="No" icon="pi pi-times" outlined onClick={hide} />
            <Button label="Yes" icon="pi pi-check" severity="danger" onClick={() => deleteFlight(flight)} />
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
        </>
    );
}