import { useState } from 'react';
import Header from './layout/Header.jsx';
import Menu from './layout/Menu.jsx';
import Home from './home/Home.jsx';
import 'primereact/resources/themes/lara-dark-indigo/theme.css';
import 'primeflex/primeflex.css';
import 'primeicons/primeicons.css';
import './App.css';

function App() {
    const [sideBarVisible, setSideBarVisible] = useState(false);
    const [selectedScreen, setSelectedScreen] = useState('home');

    return (
        <>
            <Header setSideBarVisible={setSideBarVisible} />
            <Menu sideBarVisible={sideBarVisible} setSideBarVisible={setSideBarVisible} setSelectedScreen={setSelectedScreen} />
            {selectedScreen === 'home' && <Home />}
        </>
    );
}

export default App;