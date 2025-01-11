import { PanelMenu } from 'primereact/panelmenu';
import { Sidebar } from 'primereact/sidebar';

//The function returns the sidebar menu of the website.
//@param {object} props The properties accepted by the component.
//@param {bool} props.visible Used to control if the sidebar menu is visibile or not.
//@param {function} props.hide Used by the component to hide the sidebar menu.
export default function Menu({ visible, hide }) {
    //Define the menus to display.
    const menuItems = [
        {
            label: 'Home',
            url: '/',
        },
        {
            label: 'Airlines',
            url: '/Airline',
        },
        {
            label: 'Flight Schedule',
            url: '/FlightSchedule',
        }
    ];

    return (
        <Sidebar visible={visible} onHide={hide}>
            <PanelMenu model={menuItems} />
        </Sidebar>
    );
}