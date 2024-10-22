import React from 'react'
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';

//Displays an error message to the user.
//@param {object} props The properties accepted by the component.
//@param {object} props.errorMessage The error message to display to the user.
//@param {bool} props.visible Used to control if the dialog is visible or not.
//@param {function} props.hide Used to hide the dialog.
export default function ErrorDialog({ errorMessage, visible, hide }) {
    //Define the footer for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="Ok" outlined onClick={hide} />
        </React.Fragment>
    );

    return (
        <Dialog breakpoints={{ '960px': '75vw', '641px': '90vw' }} footer={footer} header="Error" modal style={{ width: '32rem' }} visible={visible} onHide={hide}>
            <div className="confirmation-content">
                <i className="pi pi-exclamation-triangle mr-3" style={{ fontSize: '2rem' }} />
                <span>{errorMessage}</span>
            </div>
        </Dialog>
    );
}