# ASP.NET Core / React Example Project

This example project is a simplified flight management software for a BHS (baggage handling system). In a BHS, bags are introduced into the system at the ticket counters and a scanner before the sortation system will read the tag on the bag. Sortation will use the scanned tag to 
find a flight in the flight schedule to determine what sort destination the bag needs to be sent to. The sort destination is written to the lower level and the lower level will track/divert the bag to the sort destination.

On startup, the example project pregenerates a few airlines, gates and sort destinations and then, a flight schedule is pregenerated; a flight every 10 minutes between 4AM and 10PM. The example has two pages, airlines and flight schedule.

## Airlines Page

The airlines page allows the user to add/edit/delete airlines. 

<img width="1919" height="590" alt="image" src="https://github.com/user-attachments/assets/10863f13-529e-4c2a-aa07-c05cb587803f" />

### Add / Edit

On the airlines page, the user can create a new airline or edit an existing airline.

* Name - The friendly name for the airline; required and must be unique.
* Description - A description about the airline; optional.
* IATA - The code assigned by the International Air Transport Association to the airline; required and must be two letters or a letter and a number.
* ICAO - The code assigned by the International Civil Aviation Organization to the airline; required, must be 3 letters and must be unique.
* Number Code - The number code assigned by the International Air Transport Association to the airline; required, must be 3 digits and must be unique unless 000 (unassigned).
* Sort Destination - The default sort destination for the airline; required.

<img width="401" height="553" alt="image" src="https://github.com/user-attachments/assets/f25e4ee8-f610-425f-a88f-f7422c7dae31" />

<img width="398" height="549" alt="image" src="https://github.com/user-attachments/assets/ebe27af6-34f3-4b20-ac5c-040d5bbb7bb2" />

### Delete

On the airlines page, the user can delete an airline. The user will be required to confirm the deletion or cancel. On confirmation, the airline and its associated flights will be deleted.

<img width="413" alt="image" src="https://github.com/user-attachments/assets/de48d6ec-4046-44fa-850c-298005312601" />

## Flight Schedule
The flight schedule page allows the user to add/edit/delete flights in the schedule.

<img width="1906" alt="image" src="https://github.com/user-attachments/assets/91b40650-d124-451c-8515-8837d9e1e107" />

### Add / Edit

On the flight schedule page, the user can create a new flight or edit an existing flight.

* Gate - The gate the flight will be docked at for departure; required.
* Airline - The airline which owns the plane; required.
* Flight Number - The number assigned to the flight; required and must be 4 digits or 4 digits and a letter.
* Destination - The next destination for the flight; required and must be 3 letters.
* Depart Time - The schedule time the plane will depart from the gate; required.
* Sort Destination - The sort destination bags will be sorted too for the flight; required.

If the flight's airline, flight number and destination match another flight, the add or edit will be rejected by the server.

<img width="404" alt="image" src="https://github.com/user-attachments/assets/cd3dbf00-6797-4f05-b683-7fe5b8e49294" />

<img width="401" alt="image" src="https://github.com/user-attachments/assets/9953df72-ecef-45fb-9821-95964a7e45ba" />

### Delete

On the flight schedule page, the user can delete a flight. The user will be required to confirm the deletion or cancel. On confirmation, the flight will be deleted.

<img width="399" alt="image" src="https://github.com/user-attachments/assets/cd04326b-4f16-4b9a-90e8-1f2a7a86b020" />

## Edit Conflict

When two users are editing an airline or flight at the same time, whoever submits first will win; the other user will be told to try again.

<img width="400" height="188" alt="image" src="https://github.com/user-attachments/assets/f5c7e697-7ba7-4177-bd40-4be5d3a03a95" />

