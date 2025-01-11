//The function returns the home page; it displays a description about the example website.
export default function HomePage() {
    return (
        <div class="px-2 py-2">
            <h1>Hello!</h1>

            <p>
                This example project is a simplified flight management software for a BHS (baggage handling system).
                In a BHS, bags are introduced into the system at the ticket counters and a scanner before the sortation
                system will read the tag on the bag. Sortation will use the scanned tag to find a flight in the flight
                schedule to determine what sort destination the bag needs to be sent to. The sort destination is written
                to the lower level and the lower level will track/divert the bag to the sort destination.
            </p>
                
            <p>
                A flight schedule, airlines, gates and sort destinations are pregenerated for the example on startup. The
                example has two pages, airlines and flight schedule. The airlines page allows the user to add/edit/delete
                airlines. The flight schedule page allows the user to add/edit/delete flights in the schedule.
            </p>
        </div>
    );
}