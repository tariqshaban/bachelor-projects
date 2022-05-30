Soccer Dashboard
==============================

A low-code dashboard that contains the following:

* A soccer system that handles basic CRUD operations
* An authentication module

Languages/Frameworks
------------

* PL/SQL, using Oracle APEX

Prerequisites
------------

* A web browser with an active internet connections

Getting Started
------------

* Download the project's subdirectory from GitHub
  using [this link](https://minhaskamal.github.io/DownGit/#/home?url=https://github.com/tariqshaban/bachelor-projects/tree/master/3rd_year/1st_semester/cis421_oracle/soccer_dashboard)
  , credit goes to [Minhas Kamal](https://minhaskamal.github.io).
* Navigate to [Oracle APEX](https://apex.oracle.com/en) website and log in, create an
  account [here](https://signup.cloud.oracle.com/) if you can not set up an account before
* Setting up the database
    * Select `SQL Scripts`  from the `SQL Workshop` menu bar
    * Press the `Upload` button, and select the provided [DB.txt](sql_script) file
    * Press the `Upload` button
    * Press the `▶` button and press the `Run Now` button, verify that the tables have been created and populated
* Setting up the dashboard
    * Return to the homepage and click the `App Builder` button
    * Press the `Import` button
    * Select the provided [f20031.sql](sql_script) file as the file to upload
    * Compete the wizard
* Setting up the user access control
    * Open the newly created application
    * Click the `Shared Components` button, then `Application Access Control` under the `Security` section
    * Create a new user with a name of your choosing, and assign it the `Read` and `Contributor` privileges
    * Click the user icon in the top right corner and select `Manage Users and Groups`
    * Click `Create User`, and then fill the field, the username MUST have the same value that you have specified
      earlier
* Inspecting the dashboard
    * Open the newly created application
    * Click the `Run Applcation` button
    * Login with the newly created username
    * Inspect the website

Notes
------------

* Some tables allow the modification of their attributes

--------