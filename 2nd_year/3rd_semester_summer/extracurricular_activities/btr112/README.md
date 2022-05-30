BTR-112
==============================

A Unity-based game that contains the following:

* Augmented reality of models, based on a trigger object
* A click event handler on the 3d model

Languages/Frameworks
------------

* Unity, written in C#
* Vuforia SDK

Prerequisites
------------

* Unity software
* Visual studio (optional)
* Any camera-equipped device, The Android device must have a minimum SDK of 19 (4.4 KitKat)

Getting Started
------------

* Download the project's subdirectory from GitHub
  using [this link](https://minhaskamal.github.io/DownGit/#/home?url=https://github.com/tariqshaban/bachelor-projects/tree/master/2nd_year/3rd_semester_summer/extracurricular_activities/btr112)
  , credit goes to [Minhas Kamal](https://minhaskamal.github.io).
* Import the [Source Code](source_code) as a project in Unity
* Install the [Release APK](release_apk) if you wish to directly inspect the application on an Android device

Notes
------------

* In order to register the event, the camera must be pointed at the image in [this folder](release_apk)
* Some assets may have been ripped from other game files, the application has been made purely for learning purposes and
  is not aimed to be used otherwise
* The application's dependencies are deprecated and require migration, since Vuforia's SDK version build
  was [8.6.7](https://dev.azure.com/vuforia-engine/unity-extension/_artifacts/feed/unity-packages/Npm/com.ptc.vuforia.engine/8.6.7/overview)

--------