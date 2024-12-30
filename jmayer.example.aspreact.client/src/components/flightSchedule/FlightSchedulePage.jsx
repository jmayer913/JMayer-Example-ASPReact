import React, { useState, useEffect } from 'react'
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Toolbar } from 'primereact/toolbar';
import { useError } from '../errorDialog/ErrorProvider.jsx';
import ErrorDialog from '../errorDialog/ErrorDialog.jsx';

//TO DO: I need to figure out if the dataTableSelectedFlight and selection options are needed to edit/delete a flight.

//The flight schedule page. Users can manage active flights for today.
export default function FlightSchedulePage() {
    //Used when adding a new flight.
    let emptyFlight = {
        airlineID: 0,
        codeShares: [],
        description: '',
        departTime: '00:00',
        destination: '',
        flightNumber: '',
        gateID: 0,
        gateName: '',
        name: '',
        sortDestinationID: 0,
        sortDestinationName: '',
    };

    const [flights, setFlights] = useState([]);
    const [flight, setFlight] = useState(emptyFlight);
    const [dataTableSelectedFlight, setDataTableSelectedFlight] = useState(null);
    const [addEditDialogVisible, setAddEditDialogVisible] = useState(false);
    const [deleteConfirmDialogVisible, setDeleteConfirmDialogVisible] = useState(false);
    const [newRecord, setNewRecord] = useState(false);
    const { showError } = useError();

    //Load the flights when the component mounts.
    useEffect(() => {
        refreshFlights();
    }, []);

    //Hide the add/edit dialog.
    const hideAddEditDialog = () => {
        setAddEditDialogVisible(false);
    };

    //Hide the delete confirmation dialog.
    const hideDeleteConfirmDialog = () => {
        setDeleteConfirmDialogVisible(false);
    };

    //Opens the add/edit dialog.
    const openAddEditDialog = (flight) => {
        if (flight === null) {
            flight = { ...emptyFlight }
            setNewRecord(true);
        }
        else {
            setNewRecord(false);
        }

        setFlight(flight);
        setAddEditDialogVisible(true);
        //TO DO: Add add/edit dialog.
    };

    //Opens the delete confirmation dialog for the
    //user to confirm or cancel the deletion.
    const openDeleteConfirmDialog = (flight) => {
        setFlight(flight);
        setDeleteConfirmDialogVisible(true);
        //TO DO: Add delete confirm dialog
    };

    //Refreshes the flights.
    const refreshFlights = () => {
        fetch('api/Flight/All')
            .then(response => response.json())
            .then(json => setFlights(json))
            .catch(error => showError('Failed to communicate with the server.'));
    };

    //Define the add button for the toolbar.
    const leftToolbarTemplate = () => {
        return (
            <div className="flex flex-wrap gap-2">
                <Button icon="pi pi-plus" rounded onClick={() => openAddEditDialog(null)} />
            </div>
        );
    };

    //Define the delete/edit actions for the action column.
    const actionBodyTemple = (rowData) => {
        return (
            <React.Fragment>
                <Button icon="pi pi-trash" className="mr-2" rounded text onClick={() => openDeleteConfirmDialog(rowData)} />
                <Button icon="pi pi-pencil" text onClick={() => openAddEditDialog(rowData)} />
            </React.Fragment>
        )
    }

    return (
        <>
            <Card title="Flight Schedule">
                <Toolbar className="mb-4" left={leftToolbarTemplate} />

                <DataTable value={flights} filterDisplay="row" reorderableColumns stripedRows tableStyle={{ minWidth: '50rem' }}
                        paginator rows={10} rowsPerPageOptions={[10, 25, 50]}
                        selectionMode="single" dataKey="integer64ID" selection={dataTableSelectedFlight} onSelectionChange={(e) => setDataTableSelectedFlight(e.value)}
                        removableSort sortField="name" sortOrder={1}>
                    <Column body={actionBodyTemple} exportable={false} style={{ minWidth: '12rem' }} />
                    <Column field="gateName" header="Gate" filter sortable />
                    <Column field="name" header="Name" filter sortable />
                    <Column field="destination" header="Destination" filter sortable />
                    <Column field="departTime" header="Depart Time" filter sortable />
                    <Column field="sortDestinationName" header="Baggage Sort Destination" filter sortable />
                </DataTable>
            </Card>

            <ErrorDialog />
        </>
    );
}