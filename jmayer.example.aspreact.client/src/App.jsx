import { useState } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import AirlinePage from './components/airline/AirlinePage.jsx';
import FlightSchedulePage from './components/flightSchedule/FlightSchedulePage.jsx';
import HomePage from './components/home/HomePage.jsx';
import Header from './components/layout/Header.jsx';
import Menu from './components/layout/Menu.jsx';
import NotFoundPage from './components/NotFound/NotFoundPage.jsx';
import 'primereact/resources/themes/lara-dark-indigo/theme.css';
import 'primeflex/primeflex.css';
import 'primeicons/primeicons.css';
import './App.css';

//The main layout of the website & the current page.
function App() {
    const [sideBarVisible, setSideBarVisible] = useState(false);

    return (
        <>
            <Header setSideBarVisible={setSideBarVisible} />
            <Menu sideBarVisible={sideBarVisible} setSideBarVisible={setSideBarVisible} />
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/Airline" element={<AirlinePage /> } />
                    <Route path="/FlightSchedule" element={<FlightSchedulePage />} />
                    <Route path="*" element={<NotFoundPage />} />
                </Routes>
            </BrowserRouter>
        </>
    );
}

export default App;