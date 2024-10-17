import React, { useState, useEffect } from 'react'
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Toolbar } from 'primereact/toolbar';
import AirlineAddEditDialog from './AirlineAddEditDialog.jsx';
import AirlineDeleteConfirmDialog from './AirlineDeleteConfirmDialog.jsx'

//The airline page. Users can manage the airlines.
export default function AirlinePage() {
    //Used when adding a new airline.
    let emptyAirline = {
        name: '',
        description: '',
        iata: '',
        icao: '',
        numberCode: '',
    };

    const [airlines, setAirlines] = useState([]);
    const [airline, setAirline] = useState(emptyAirline);
    const [dataTableSelectedAirline, setDataTableSelectedAirline] = useState(null);
    const [addEditDialogVisible, setAddEditDialogVisible] = useState(false);
    const [deleteConfirmDialogVisible, setDeleteConfirmDialogVisible] = useState(false);
    const [newRecord, setNewRecord] = useState(false);

    //Load the airlines when the component mounts.
    useEffect(() => {
        refreshAirlines();
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
    const openAddEditDialog = (airline) => {
        if (airline === null) {
            airline = { ...emptyAirline }
            setNewRecord(true);
        }
        else {
            setNewRecord(false);
        }

        setAirline(airline);
        setAddEditDialogVisible(true);
    };

    //Opens the delete confirmation dialog for the
    //user to confirm or cancel the deletion.
    const openDeleteConfirmDialog = (airline) => {
        setAirline(airline);
        setDeleteConfirmDialogVisible(true);
    };

    //Refreshes the airlines.
    const refreshAirlines = () => {
        fetch("/api/Airline/All")
            .then(response => response.json())
            .then(json => setAirlines(json))
            .catch(error => {
                //TO DO: Add error handling.
            });
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
            <Card title="Airlines">
                <Toolbar className="mb-4" left={leftToolbarTemplate} />

                <DataTable value={airlines} filterDisplay="row" reorderableColumns stripedRows tableStyle={{ minWidth: '50rem' }}
                        paginator rows={10} rowsPerPageOptions={[10, 25, 50]}
                        selectionMode="single" dataKey="integer64ID" selection={dataTableSelectedAirline} onSelectionChange={(e) => setDataTableSelectedAirline(e.value)}
                        removableSort sortField="name" sortOrder={1}>
                    <Column body={actionBodyTemple} exportable={false} style={{ minWidth: '12rem' }} />
                    <Column field="name" header="Name" filter sortable />
                    <Column field="iata" header="IATA" filter sortable />
                    <Column field="icao" header="ICAO" filter sortable />
                    <Column field="numberCode" header="Number Code" filter sortable />
                </DataTable>
            </Card>

            <AirlineAddEditDialog newRecord={newRecord} airline={airline} setAirline={setAirline} refreshAirlines={refreshAirlines} visible={addEditDialogVisible} hide={hideAddEditDialog} />
            <AirlineDeleteConfirmDialog airline={airline} refreshAirlines={refreshAirlines} visible={deleteConfirmDialogVisible} hide={hideDeleteConfirmDialog} />
        </>
    );
}