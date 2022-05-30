<?php
$error="<head><title>Messenger</title></head><body style='background-color:gray'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Could Not Reach Host</h1><br><h2><a href='register.html'>Go Back</a></h2></center></body>";
$conn=mysqli_connect("127.0.0.1","root");
if(!$conn)
die($error);
$db=mysqli_select_db($conn,"messages");
if(!$db)
die($error);
echo("<head><title>Messenger</title></head><body style='background-color:gray'><center><h1 style='color:green'>Hi $_POST[sender]! Youre Request Is Being Processed</h1><h4><a href='login.html'>Logout</a></h4><br><br><br><br>");
mysqli_query($conn,"INSERT INTO message (sender,receiver,content,is_read,time) VALUES ('$_POST[sender]','$_POST[receiver]','$_POST[string]','false','$_POST[Message_Time]') ");
echo("<form id='ready' method='post' action='Message_Service.php'><input type='hidden' value='$_POST[sender]' name='sender'><input type='hidden' value='$_POST[receiver]' name='receiver'><br></form>");
echo("<script>document.getElementById('ready').submit();</script>");
echo("</center></body>");
mysqli_close($conn);
?>