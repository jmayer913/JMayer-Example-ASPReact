import { useState, useEffect } from 'react'
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';

//The flight schedule page. Users can manage active flights.
export default function FlightSchedulePage() {
    const [flights, setFlights] = useState([]);

    useEffect(() => {
        //TO DO: Query Data on Server.
    }, []);

    return (
        <Card title="Flight Schedule">
            <DataTable value={flights} tableStyle={{minWidth: '50rem'}}>
                <Column field="name" header="Name" />
                <Column field="destination" header="Destination" />
                <Column field="departTime" header="Depart Time" />
                <Column field="primarySort" header="Primary Sort" />
                <Column field="secondarySort" header="Secondary Sort" />
            </DataTable>
        </Card>
    );
}