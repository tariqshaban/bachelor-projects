<?php
$error="<head><title>Library</title></head><body style='background-color:gray'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Could Not Reach Host</h1><br><h2><a href='login.html'>Go Back</a></h2><script src=http://127.0.0.1/Assets/Pictures.js></script></center></body>";
$conn=mysqli_connect("127.0.0.1","root");
if(!$conn)
die($error);
$db=mysqli_select_db($conn,"library");
if(!$db)
die($error);
$Validate=mysqli_query($conn,"SELECT * FROM users WHERE User='$_POST[user]' AND pass='$_POST[pass]'");
if(mysqli_num_rows($Validate)==1)
echo("<head><title>Library</title></head><body id=Pic><br><br><br><br><br><br><br><br><br><center><h1 style='color:blue'>Logged In!</h1><div id=Time></div><script src=http://127.0.0.1/Assets/Pictures.js></script><script src=http://127.0.0.1/Assets/Time.js></script><br><form method='POST' action='Main_UI.php'><input type='hidden' value='$_POST[user]' name='user'><input type='hidden' value='$_POST[pass]' name='pass'><input type='submit' value='Jump To Messenger'></form></center></body>");
else
die("<head><title>Library</title></head><body id=Pic><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Invalid Username Or Password</h1><div id=Time></div><script src=http://127.0.0.1/Assets/Pictures.js></script><script src=http://127.0.0.1/Assets/Time.js></script><br><h2><a href='login.html'>Go Back</a><script src=http://127.0.0.1/Assets/Pictures.js></script></h2></center></body>");
mysqli_close($conn);
?>