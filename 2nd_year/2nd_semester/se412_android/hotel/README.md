Hotel
==============================

An Android native application that contains the following:

* Booking system
* Reservation cost calculation
* Basic theming options

Languages/Frameworks
------------

* Android, written in Java
* XML
* SQLite

Prerequisites
------------

* Android Studio
* An Android device, minimum SDK 19 (4.4 KitKat)

Note: Running Android Studio with an emulator requires capable hardware.

Getting Started
------------

* Download the project's subdirectory from GitHub
  using [this link](https://minhaskamal.github.io/DownGit/#/home?url=https://github.com/tariqshaban/bachelor-projects/tree/master/2nd_year/2nd_semester/se412_android/hotel)
  , credit goes to [Minhas Kamal](https://minhaskamal.github.io).
* Import the [Source Code](source_code) as a project in Android Studio
* Install the [Release APK](release_apk) if you wish to directly inspect the application on an Android device

Notes
------------

* The design of the activities may not be visually appealing
* The application's state is serialized using `Shared Preferences`, both login credentials and the application theme is
  persistent and dependent on it
* For security purposes, the `Google Maps API` key in `google_maps_api.xml` has been redacted

--------