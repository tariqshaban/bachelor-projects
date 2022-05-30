JUST Bus
==============================

A multi-platform application that contains the following:

* An end-user application, that does not require a login, fast and efficient
* A dedicated driver application to track his displacements, fast and efficient
* A role-based access control API endpoints to handle a request, with rate-limiting implementation
* A database that stores the following:
    * Drivers credentials and last known location
    * Stop coordinates with an optional image/panorama view
    * Logs of the API calls with the IP address

Languages/Frameworks
------------

* Flutter framework
* .NET Web API
* SQL Server, with SQL Server Management Studio (SSMS)

Prerequisites
------------

* Android Studio, with Flutter SDK installed
* An Android device, minimum SDK 21 (5.0 Lollipop)
* Visual Studio, with .NET Framework installed
* SQL Server instance

Note: Running Android Studio with an emulator requires capable hardware.

Getting Started
------------

* Download the project's subdirectory from GitHub
  using [this link](https://minhaskamal.github.io/DownGit/#/home?url=https://github.com/tariqshaban/bachelor-projects/tree/master/4th_year/1st_semester/cis492_graduation_project_II/just_bus)
  , credit goes to [Minhas Kamal](https://minhaskamal.github.io).
* Use `1234` as the number and `12345` as the password for the driver's portal

Notes
------------

* The application's state is serialized using `Shared Preferences`, both login credentials and the application theme is
  persistent and dependent on it, as well as the favourite route
* The application contains pinging functionality, for debugging purposes
* For security purposes, the `Google Maps API` key in `google_maps_api.xml` has been redacted
* You may need to acquire an SSL certificate, in order to leverage HTTPS security benefits
* Argon2 algorithm is used for credentials encryption

--------