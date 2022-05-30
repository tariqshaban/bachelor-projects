<?php
$error="<head><title>Library</title></head><body style='background-color:gray'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Could Not Reach Host</h1><br><h2><a href='login.html'>Go Back</a></h2></center></body>";
$conn=mysqli_connect("127.0.0.1","root");
if(!$conn)
die($error);
$db=mysqli_select_db($conn,"Student");
if(!$db)
die($error);
mysqli_query($conn,"update student set grade=$_POST[Grade] where id=$_POST[ID]");
?>