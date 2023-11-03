# DriveWise Application

This Car Inventory application allows users to view the car inventories of multiple dealers all in one place.

The 3 entities in this project are the Cars, the Car Models, and the Dealers.

There is a one-to-many relationship between the Cars and the Car Models:
A car can only be of one model but many cars of a model can exist.

There is also a one-to-many relationship between the Cars and the Dealers:
A car can only belong to one dealer but a dealer can have many cars.

Users can see a list of all the cars in the system and also view cars according to a particular model they wish to see. They can also view cars by dealership.

Authorization has also been implemented.
The site has Create, Read, Update, and Delete functionalities for the cars, the car models, and the dealers; but the Create, Read and Delete functionalities are limited to logged-in users who can act as site admins.
The general public can only read information on the site.
