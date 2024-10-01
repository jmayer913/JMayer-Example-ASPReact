import { Button } from 'primereact/button';

export default function Header({setSideBarVisible}) {
    return (
        <div className="flex flex-row align-items-center">
            <Button icon="pi pi-bars" size="small" rounded onClick={() => setSideBarVisible(true)} />
            <h1 className="ml-3">ASP.NET Core / React Example</h1>
        </div>
    );
}