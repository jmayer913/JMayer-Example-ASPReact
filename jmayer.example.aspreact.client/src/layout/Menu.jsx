import { PanelMenu } from 'primereact/panelmenu';
import { Sidebar } from 'primereact/sidebar';

export default function Menu({ sideBarVisible, setSideBarVisible, setSelectedScreen}) {
    const menuItems = [
        {
            label: 'Home',
            command: () => setSelectedScreen('home'),
        }
    ];

    return (
        <Sidebar visible={sideBarVisible} onHide={() => setSideBarVisible(false)}>
            <PanelMenu model={menuItems} />
        </Sidebar>
    );
}