import { PanelMenu } from 'primereact/panelmenu';
import { Sidebar } from 'primereact/sidebar';

//The sidebar menu of the website.
//@param {object} props The properties accepted by the component.
//@param {bool} props.sideBarVisible Used to control if the sidebar menu is visibile or not.
//@param {function} props.setSideBarVisible Used by the component to hide the sidebar menu.
export default function Menu({ sideBarVisible, setSideBarVisible }) {
    //Define the menus to display.
    const menuItems = [
        {
            label: 'Home',
            url: '/',
        },
        {
            label: 'Flight Schedule',
            url: '/FlightSchedule',
        }
    ];

    return (
        <Sidebar visible={sideBarVisible} onHide={() => setSideBarVisible(false)}>
            <PanelMenu model={menuItems} />
        </Sidebar>
    );
}