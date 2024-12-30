import React from 'react'
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { useError } from './ErrorProvider';

//Displays an error message to the user.
export default function ErrorDialog() {
    const { errorMessage, errorVisible, hideError } = useError();

    //Define the footer for the dialog.
    const footer = (
        <React.Fragment>
            <Button label="Ok" outlined onClick={hideError} />
        </React.Fragment>
    );

    return (
        <Dialog breakpoints={{ '960px': '75vw', '641px': '90vw' }} footer={footer} header="Error" modal style={{ width: '32rem' }} visible={errorVisible} onHide={hideError}>
            <div className="confirmation-content">
                <i className="pi pi-exclamation-triangle mr-3" style={{ fontSize: '2rem' }} />
                <span>{errorMessage}</span>
            </div>
        </Dialog>
    );
}