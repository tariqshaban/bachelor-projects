<?php
$error="<head><title>Messenger</title></head><body style='background-color:gray'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Could Not Reach Host</h1><br><h2><a href='register.html'>Go Back</a></h2></center></body>";
$conn=mysqli_connect("127.0.0.1","root");
if(!$conn)
die($error);
$db=mysqli_select_db($conn,"messages");
if(!$db)
die($error);
echo("<head><title>Messenger</title></head><body id=Pic><center><h1 style='color:green'>Hi $_POST[sender]! Connection To Server Still OK<br>You Are Now Messaging $_POST[receiver]</h1><div id=Time></div><script src=http://127.0.0.1/Assets/Time.js></script><h4><a href='../index.html'>Logout</a></h4><br><br><br><br>");
$messages=mysqli_query($conn,"SELECT * FROM message WHERE Sender='$_POST[receiver]' AND Receiver='$_POST[sender]'");
while($Read=mysqli_fetch_assoc($messages))
mysqli_query($conn,"UPDATE message SET Is_read=1 WHERE Sender='$_POST[receiver]' AND Receiver='$_POST[sender]'");
$messages=mysqli_query($conn,"SELECT * FROM message WHERE Sender='$_POST[sender]' AND Receiver='$_POST[receiver]' OR Sender='$_POST[receiver]' AND Receiver='$_POST[sender]'");
while($Typer=mysqli_fetch_assoc($messages))
{
if($Typer["Is_read"]==True)
$seen="Seen";
else
$seen="Delivered";
if ($Typer["Sender"]==$_POST["sender"])	
echo("<h4 style='word-wrap:break-word; position:relative; left:25%; text-align:right; background-color:white; border-radius:10px; width:40%; padding:10px; '>$Typer[Content]</h4><h5 style='color:blue; text-align:right; position:relative; right:5%'><span style='color:LimeGreen'>$seen</span> $Typer[Time]</h5>");
else
echo("<h4 style='word-wrap:break-word; position:relative; right:25%; text-align:left; background-color:white; border-radius:10px; width:40%; padding:10px;'>$Typer[Content]</h4><h5 style='color:blue; text-align:right; position:relative; right:82%'>$Typer[Time]</h5>");
}
echo("<div style='position:sticky; bottom:0'><form method='post' action='processor.php'><input type='hidden' name='sender' value='$_POST[sender]'><input type='hidden' name='receiver' value='$_POST[receiver]'><input type='hidden' id='Message_Time' name='Message_Time' value=''><input  type='box' name='string' style='-moz-border-radius:15px; border:solid 1px blue; border-radius:10px; width:75%'></form></div>");
echo("<script src='Message_Time.js'></script>");
echo("<script>window.scrollTo(0,document.body.scrollHeight);</script>");
echo("<script src=http://127.0.0.1/Assets/Pictures.js></script></center></body>");
mysqli_close($conn);
?>