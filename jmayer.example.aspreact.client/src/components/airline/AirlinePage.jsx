import React, { useState, useEffect } from 'react'
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Toolbar } from 'primereact/toolbar';
import ErrorDialog from '../errorDialog/ErrorDialog.jsx';
import AirlineAddEditDialog from './AirlineAddEditDialog.jsx';
import AirlineDeleteConfirmDialog from './AirlineDeleteConfirmDialog.jsx';
import initialAirline, { useAirlineDataLayer } from '../../datalayers/AirlineDataLayer.jsx';

//TO DO: I need to figure out if the dataTableSelectedAirline and selection options are needed to edit/delete an airline.

//The function returns the page for the airlines.
export default function AirlinePage() {
    const { airlines, getAirlines } = useAirlineDataLayer();
    const [airline, setAirline] = useState(initialAirline);
    const [dataTableSelectedAirline, setDataTableSelectedAirline] = useState(null);
    const [addEditDialogVisible, setAddEditDialogVisible] = useState(false);
    const [deleteConfirmDialogVisible, setDeleteConfirmDialogVisible] = useState(false);
    const [newRecord, setNewRecord] = useState(false);    

    //Load the airlines when the component mounts.
    useEffect(() => {
        getAirlines();
    }, []);

    //The function hides the add/edit dialog.
    const hideAddEditDialog = () => {
        setAddEditDialogVisible(false);
        getAirlines();
    };

    //The function hides the delete confirmation dialog.
    const hideDeleteConfirmDialog = () => {
        setDeleteConfirmDialogVisible(false);
        getAirlines();
    };

    //The function opens the add/edit dialog.
    const openAddEditDialog = (airline) => {
        if (airline === null) {
            airline = { ...initialAirline }
            setNewRecord(true);
        }
        else {
            setNewRecord(false);
        }

        setAirline(airline);
        setAddEditDialogVisible(true);
    };

    //The function opens the delete confirmation dialog for the
    //user to confirm or cancel the deletion.
    const openDeleteConfirmDialog = (airline) => {
        setAirline(airline);
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

    //The delete/edit actions for the action column of the data table.
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

            <AirlineAddEditDialog newRecord={newRecord} airline={airline} setAirline={setAirline} visible={addEditDialogVisible} hide={hideAddEditDialog} />
            <AirlineDeleteConfirmDialog airline={airline} visible={deleteConfirmDialogVisible} hide={hideDeleteConfirmDialog} />
            <ErrorDialog />
        </>
    );
}