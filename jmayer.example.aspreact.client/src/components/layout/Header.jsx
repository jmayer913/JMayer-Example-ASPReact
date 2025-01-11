import { Button } from 'primereact/button';

//The function returns  the top header of the website.
//@param {object} props The properties accepted by the component.
//@param {function} props.openMenu Used by the component to show the sidebar menu.
export default function Header({openMenu}) {
    return (
        <div className="flex flex-row align-items-center">
            <Button icon="pi pi-bars" size="small" rounded onClick={openMenu} />
            <h1 className="ml-3">ASP.NET Core / React Example</h1>
        </div>
    );
}