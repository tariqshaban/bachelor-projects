<?php
$Validate="/^([A-z]+[0-9]{0,3}){1,20}$/";
if(!preg_match($Validate,$_POST["user"]))
die("<head><title>Messenger</title></head><body style='id=Pic'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Invalid Username Or Password</h1><div id=Time></div><script src=http://127.0.0.1/Assets/Time.js></script><br><script src=http://127.0.0.1/Assets/Pictures.js></script><br><h2><a href='register.html'>Go Back</a></h2></center></body>");
if(!preg_match($Validate,$_POST["pass"]))
die("<head><title>Messenger</title></head><body style='background-color:gray'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Invalid Username Or Password</h1><div id=Time></div><script src=http://127.0.0.1/Assets/Time.js></script><br><script src=http://127.0.0.1/Assets/Pictures.js></script><br><h2><a href='register.html'>Go Back</a></h2></center></body>");
$error="<head><title>Messenger</title></head><body style='id=Pic'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Could Not Reach Host</h1><br><h2><a href='register.html'>Go Back</a></h2></center></body>";
$conn=mysqli_connect("127.0.0.1","root");
if(!$conn)
die($error);
$db=mysqli_select_db($conn,"messages");
if(!$db)
die($error);
$Validate_2=mysqli_query($conn,"SELECT user FROM users WHERE user='$_POST[user]'");
if(mysqli_num_rows($Validate_2)>0)
die("<head><title>Messenger</title></head><body style='id=Pic'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Account Already Exists</h1><div id=Time></div><script src=http://127.0.0.1/Assets/Time.js></script><br><script src=http://127.0.0.1/Assets/Pictures.js></script><br><h2><a href='register.html'>Go Back</a></h2></center></body>");
echo("<script src='Message_Time.js'></script>");
mysqli_query($conn,"INSERT INTO message (sender,receiver,content,is_read,time) VALUES ('admin','$_POST[user]','Welcome to the experimental messaging application!!','false','$_POST[Message_Time]') ");
mysqli_query($conn,"INSERT INTO users (user,pass) VALUES ('$_POST[user]','$_POST[pass]') ");
echo("<head><title>Messenger</title></head><body style='id=Pic'><br><br><br><br><br><br><br><br><br><center><h1 style='color:blue'>User Registered!</h1><div id=Time></div><script src=http://127.0.0.1/Assets/Time.js></script><br><script src=http://127.0.0.1/Assets/Pictures.js></script><h2><a href='../index.html'>Go Back</a></h2></center></body>");
mysqli_close($conn);
?>