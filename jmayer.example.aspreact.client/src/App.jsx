//import { useEffect, useState } from 'react';
import { Button } from 'primereact/button';
import 'primereact/resources/themes/lara-light-indigo/theme.css';
import 'primeflex/primeflex.css';
import './App.css';

function App() {
    //const [forecasts, setForecasts] = useState();

    //useEffect(() => {
    //    populateWeatherData();
    //}, []);

    //const contents = forecasts === undefined
    //    ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
    //    : <table className="table table-striped" aria-labelledby="tableLabel">
    //        <thead>
    //            <tr>
    //                <th>Date</th>
    //                <th>Temp. (C)</th>
    //                <th>Temp. (F)</th>
    //                <th>Summary</th>
    //            </tr>
    //        </thead>
    //        <tbody>
    //            {forecasts.map(forecast =>
    //                <tr key={forecast.date}>
    //                    <td>{forecast.date}</td>
    //                    <td>{forecast.temperatureC}</td>
    //                    <td>{forecast.temperatureF}</td>
    //                    <td>{forecast.summary}</td>
    //                </tr>
    //            )}
    //        </tbody>
    //    </table>;

    const contents = <p><em>Under Construction</em></p>;

    return (
        <div>
            <h1 id="tableLabel">Example Project</h1>
            {contents}
            <div className="card flex flex-wrap justify-content-center gap-3">
                <Button label="Primary" />
                <Button label="Secondary" severity="secondary" />
                <Button label="Success" severity="success" />
                <Button label="Info" severity="info" />
                <Button label="Warning" severity="warning" />
                <Button label="Help" severity="help" />
                <Button label="Danger" severity="danger" />
            </div>
        </div>
    );
    
    //async function populateWeatherData() {
    //    const response = await fetch('weatherforecast');

    //    if (response.ok) {
    //        const data = await response.json();
    //        setForecasts(data);
    //    }
    //}
}

export default App;