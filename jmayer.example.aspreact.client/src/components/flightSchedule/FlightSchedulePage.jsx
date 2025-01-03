import React, { useState, useEffect } from 'react'
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Toolbar } from 'primereact/toolbar';
import FlightAddEditDialog from './FlightAddEditDialog.jsx';
import FlightDeleteConfirmDialog from './FlightDeleteConfirmDialog.jsx';
import initialFlight, { useFlightDataLayer } from '../../datalayers/FlightDataLayer.jsx';

//The function returns the page for the flight schedule (flights for today).
export default function FlightSchedulePage() {
    const { flights, getFlights } = useFlightDataLayer();
    const [flight, setFlight] = useState(initialFlight);
    const [addEditDialogVisible, setAddEditDialogVisible] = useState(false);
    const [deleteConfirmDialogVisible, setDeleteConfirmDialogVisible] = useState(false);
    const [newRecord, setNewRecord] = useState(false);

    //Load the flights when the component mounts.
    useEffect(() => {
        getFlights();
    }, []);

    //The function hides the add/edit dialog.
    const hideAddEditDialog = () => {
        setAddEditDialogVisible(false);
        getFlights();
    };

    //The function hides the delete confirmation dialog.
    const hideDeleteConfirmDialog = () => {
        setDeleteConfirmDialogVisible(false);
        getFlights();
    };

    //The function opens the add/edit dialog.
    const openAddEditDialog = (flight) => {
        if (flight === null) {
            flight = { ...initialFlight }
            setNewRecord(true);
        }
        else {
            setNewRecord(false);
        }

        setFlight(flight);
        setAddEditDialogVisible(true);
    };

    //The function opens the delete confirmation dialog for the
    //user to confirm or cancel the deletion.
    const openDeleteConfirmDialog = (flight) => {
        setFlight(flight);
        setDeleteConfirmDialogVisible(true);
    };

    //The content of the toolbar.
    const leftToolbarTemplate = () => {
        return (
            <div className="flex flex-wrap gap-2">
                <Button icon="pi pi-plus" rounded onClick={() => openAddEditDialog(null)} />
            </div>
        );
    };

    //The content for the action column of the data table.
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
                        removableSort sortField="name" sortOrder={1}>
                    <Column body={actionBodyTemple} exportable={false} style={{ minWidth: '12rem' }} />
                    <Column field="gateName" header="Gate" filter sortable />
                    <Column field="name" header="Name" filter sortable />
                    <Column field="destination" header="Destination" filter sortable />
                    <Column field="departTime" header="Depart Time" filter sortable />
                    <Column field="sortDestinationName" header="Baggage Sort Destination" filter sortable />
                </DataTable>
            </Card>

            <FlightAddEditDialog newRecord={newRecord} flight={flight} setFlight={setFlight} visible={addEditDialogVisible} hide={hideAddEditDialog} />
            <FlightDeleteConfirmDialog flight={flight} visible={deleteConfirmDialogVisible} hide={hideDeleteConfirmDialog} />
        </>
    );
}