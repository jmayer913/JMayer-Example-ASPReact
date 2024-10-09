import React, { useState, useEffect } from 'react'
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Toolbar } from 'primereact/toolbar';
import AirlineDeleteConfirmDialog from './AirlineDeleteConfirmDialog.jsx'

//The airline page. Users can manage the airlines.
export default function AirlinePage() {
    const [airlines, setAirlines] = useState([]);
    const [selectedAirline, setSelectedAirline] = useState(null);
    const [deleteConfirmDialogVisible, setDeleteConfirmDialogVisible] = useState(false);

    useEffect(() => {
        let ignore = false;

        fetch("/api/Airline/All")
            .then(response => response.json())
            .then(json => {
                if (!ignore) {
                    setAirlines(json)
                }
            })
            .catch(error => {
                //TO DO: Add error handling.
            });

        return () => {
            ignore = true;
        }
    }, []);

    //Hide the delete confirmation dialog.
    const hideDeleteConfirmDialog = () => {
        setDeleteConfirmDialogVisible(false);
    };

    //Opens the delete confirmation dialog for the
    //user to confirm or cancel the deletion.
    const openDeleteConfirmDialog = (airline) => {
        setSelectedAirline(airline);
        setDeleteConfirmDialogVisible(true);
    };

    const leftToolbarTemplate = () => {
        return (
            <div className="flex flex-wrap gap-2">
                <Button icon="pi pi-plus" rounded />
            </div>
        );
    };

    const actionBodyTemple = (rowData) => {
        return (
            <React.Fragment>
                <Button icon="pi pi-trash" className="mr-2" rounded text onClick={() => openDeleteConfirmDialog(rowData)} />
                <Button icon="pi pi-pencil" text />
            </React.Fragment>
        )
    }

    return (
        <>
            <Card title="Airlines">
                <Toolbar className="mb-4" left={leftToolbarTemplate} />

                <DataTable value={airlines} filterDisplay="row" reorderableColumns stripedRows tableStyle={{ minWidth: '50rem' }}
                        paginator rows={10} rowsPerPageOptions={[10, 25, 50]}
                        selectionMode="single" dataKey="integer64ID" selection={selectedAirline} onSelectionChange={(e) => setSelectedAirline(e.value)}
                        removableSort sortField="name" sortOrder={1}>
                    <Column body={actionBodyTemple} exportable={false} style={{ minWidth: '12rem' }} />
                    <Column field="name" header="Name" filter sortable />
                    <Column field="iata" header="IATA" filter sortable />
                    <Column field="icao" header="ICAO" filter sortable />
                    <Column field="numberCode" header="Number Code" filter sortable />
                </DataTable>
            </Card>

            <AirlineDeleteConfirmDialog airline={selectedAirline} visible={deleteConfirmDialogVisible} hide={hideDeleteConfirmDialog} />
        </>
    );
}