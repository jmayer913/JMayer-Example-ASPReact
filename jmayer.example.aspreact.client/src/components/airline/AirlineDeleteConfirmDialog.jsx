import React, { useEffect } from 'react'
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { useAirlineDataLayer } from '../../datalayers/AirlineDataLayer.jsx';

//The function returns the dialog for deleting an airline.
//@param {object} props The properties accepted by the component.
//@param {object} props.airline The airline to be deleted on user confirmation.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function AirlineDeleteConfirmDialog({ airline, visible, hide }) {
    const { deleteAirline, deleteAirlineSuccess } = useAirlineDataLayer();

    useEffect(() => {
        //Hide the dialog on a successful delete.
        if (deleteAirlineSuccess) {
            hide();
        }
    }, [deleteAirlineSuccess]);

    //The footer for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="No" icon="pi pi-times" outlined onClick={hide} />
            <Button label="Yes" icon="pi pi-check" severity="danger" onClick={() => deleteAirline(airline)} />
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
        </>
    );
}