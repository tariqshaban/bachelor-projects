<?php
$error="<head><title>Messenger</title></head><body style='background-color:gray'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Could Not Reach Host</h1><br><h2><a href='register.html'>Go Back</a></h2></center></body>";
$conn=mysqli_connect("127.0.0.1","root");
if(!$conn)
die($error);
$db=mysqli_select_db($conn,"messages");
if(!$db)
die($error);
echo("<head><title>Messenger</title></head><body id=Pic><center><h1 style='color:green'>Hi ".$_POST["user"]."! Connection To Server OK</h1><div id=Time></div><script src=http://127.0.0.1/Assets/Time.js></script>");
$No_Message=mysqli_num_rows(mysqli_query($conn,"SELECT Sender FROM message WHERE Receiver='$_POST[user]'"));
$No_Unread=mysqli_num_rows(mysqli_query($conn,"SELECT Sender FROM message WHERE is_read=0 AND Receiver='$_POST[user]'"));
$No_Contacts=mysqli_num_rows(mysqli_query($conn,"SELECT user FROM users WHERE user!='$_POST[user]'"));
echo ("<h3>You Have A Total Of <span style='color:blue'>$No_Message Messages<span><br></h3>");
echo ("<h3>You Have A Total Of <span style='color:blue'>$No_Unread Unread Messages<span><br></h3>");
echo ("<h3>You Have A Total Of <span style='color:blue'>$No_Contacts Contacts<span><br></h3>");
$temp=mysqli_query($conn,"SELECT user FROM users WHERE user!='$_POST[user]'");
while($Contacts=mysqli_fetch_assoc($temp))
echo("<form method='post' action='Message_Service.php'><input type='Submit' value='$Contacts[user]'><input type='hidden' value='$_POST[user]' name='sender'><input type='hidden' value='$Contacts[user]' name='receiver'></form>");
echo("<script src=http://127.0.0.1/Assets/Pictures.js></script><h4><a href='../index.html'>Logout</a></h4></center></body>");
mysqli_close($conn);
?>