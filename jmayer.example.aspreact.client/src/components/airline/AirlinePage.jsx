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
            <DataTable value={airlines} tableStyle={{ minWidth: '50rem' }}>
                <Column field="name" header="Name" />
                <Column field="iata" header="IATA" />
                <Column field="icao" header="ICAO" />
                <Column field="numberCode" header="Number Code" />
            </DataTable>
        </Card>
    );
}