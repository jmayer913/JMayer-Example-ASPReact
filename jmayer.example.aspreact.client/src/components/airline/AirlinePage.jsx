import { useState, useEffect } from 'react'
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';

//The airline page. Users can manage the airlines.
export default function AirlinePage() {
    const [airlines, setAirlines] = useState([]);

    useEffect(() => {
        let ignore = false;

        //Need to add error handling.
        //Need to figure out how to not hardcode the base address.
        fetch("https://localhost:7020/api/Airline/All")
            .then(response => response.json())
            .then(json => {
                if (!ignore) {
                    setAirlines(json)
                }
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