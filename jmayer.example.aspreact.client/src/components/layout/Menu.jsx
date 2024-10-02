import { PanelMenu } from 'primereact/panelmenu';
import { Sidebar } from 'primereact/sidebar';

//The sidebar menu of the website.
//@param {object} props The properties accepted by the component.
//@param {bool} props.sideBarVisible Used to control if the sidebar menu is visibile or not.
//@param {function} props.setSideBarVisible Used by the component to hide the sidebar menu.
//@param {function} props.setSelectedScreen Used by the component to set what screen is displayed in the App component.
export default function Menu({ sideBarVisible, setSideBarVisible, setSelectedScreen }) {
    //Define the labels for the menu.
    const homeLabel = 'Home';
    const flightScheduleLabel = 'Flight Schedule';

    //Define the menus to display.
    const menuItems = [
        {
            label: homeLabel,
            command: () => {
                setSelectedScreen(homeLabel.toLowerCase());
                setSideBarVisible(false);
            },
        },
        {
            label: flightScheduleLabel,
            command: () => {
                setSelectedScreen(flightScheduleLabel.toLowerCase());
                setSideBarVisible(false);
            },
        }
    ];

    return (
        <Sidebar visible={sideBarVisible} onHide={() => setSideBarVisible(false)}>
            <PanelMenu model={menuItems} />
        </Sidebar>
    );
}