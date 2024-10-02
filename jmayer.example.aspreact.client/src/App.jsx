import { useState } from 'react';
import FlightSchedule from './components/flightSchedule/FlightSchedule.jsx';
import Home from './components/home/Home.jsx';
import Header from './components/layout/Header.jsx';
import Menu from './components/layout/Menu.jsx';
import 'primereact/resources/themes/lara-dark-indigo/theme.css';
import 'primeflex/primeflex.css';
import 'primeicons/primeicons.css';
import './App.css';

//The main layout of the website & the currently selected screen.
function App() {
    const [sideBarVisible, setSideBarVisible] = useState(false);
    const [selectedScreen, setSelectedScreen] = useState('home');

    return (
        <>
            <Header setSideBarVisible={setSideBarVisible} />
            <Menu sideBarVisible={sideBarVisible} setSideBarVisible={setSideBarVisible} setSelectedScreen={setSelectedScreen} />
            {selectedScreen === 'home' && <Home />}
            {selectedScreen === 'flight schedule' && <FlightSchedule /> }
        </>
    );
}

export default App;