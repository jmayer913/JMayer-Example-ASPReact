import { useState, useEffect } from 'react'
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';

//The airline page. Users can manage the airlines.
export default function AirlinePage() {
    const [airlines, setAirlines] = useState([]);

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

    return (
        <Card title="Airlines">
            <DataTable value={airlines} filterDisplay="row" reorderableColumns stripedRows tableStyle={{ minWidth: '50rem' }}
                    paginator rows={10} rowsPerPageOptions={[10, 25, 50]}
                    removableSort sortField="name" sortOrder={1}>
                <Column field="name" header="Name" filter sortable />
                <Column field="iata" header="IATA" filter sortable />
                <Column field="icao" header="ICAO" filter sortable />
                <Column field="numberCode" header="Number Code" filter sortable />
            </DataTable>
        </Card>
    );
}