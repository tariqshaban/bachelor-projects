<?php
$error="<head><title>Library</title></head><body style='background-color:gray'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Could Not Reach Host</h1><br><h2><a href='register.html'>Go Back</a></h2></center></body>";
$conn=mysqli_connect("127.0.0.1","root");
if(!$conn)
die($error);
$db=mysqli_select_db($conn,"library");
if(!$db)
die($error);

if($_POST["Operations"]=="Default")
  $Result="<h3 style='color:darkred'><strong>You Have Not Selected Any Operation</strong></h3>";

else if($_POST["Operations"]=="Book_Add")
  if($_POST["user"]!="admin")
  $Result="<h3 style='color:darkred'><strong>Access denied as you do not have sufficient privileges</strong></h3>";
  else
  {$Query=mysqli_query($conn,"INSERT INTO `book`(ID,Title,Publisher,Edition,Price) VALUES ('$_POST[ID]','$_POST[Title]','$_POST[Publisher]','$_POST[Edition]','$_POST[Price]')");
  if($Query!=1) $Result="<h3 style='color:darkred'><strong>Operation Failed; Check Syntax And If Already Exist</strong></h3>";
  else $Result="<h3 style='color:darkred'><strong style='color:gray'>Success</strong></h3>";}

else if($_POST["Operations"]=="Book_Delete")
  if($_POST["user"]!="admin")
  $Result="<h3 style='color:darkred'><strong>Access denied as you do not have sufficient privileges</strong></h3>";
  else
  {
  $Ans=mysqli_num_rows(mysqli_query($conn,"SELECT * FROM book WHERE ID='$_POST[ID]'"));
  $Query=mysqli_query($conn,"DELETE FROM `book` WHERE ID='$_POST[ID]'");
  if($Query!=1) $Result="<h3 style='color:darkred'><strong>Operation Failed, This Is Normally Caused When The Book Is Registered In Either The Loan Or The Sell Database</strong></h3>";
  else $Result="<h3 style='color:darkred'><strong style='color:gray'>Success</strong></h3>";
  if($Ans==0) $Result="<h3 style='color:darkred'><strong>Could Not Find Any Results</strong></h3>";
  }

else if($_POST["Operations"]=="Book_Update")
  if($_POST["user"]!="admin")
  $Result="<h3 style='color:darkred'><strong>Access denied as you do not have sufficient privileges</strong></h3>";
  else
  {$Query=mysqli_query($conn,"UPDATE `book` SET Title='$_POST[Title]',Publisher='$_POST[Publisher]',Edition='$_POST[Edition]',Price='$_POST[Price]' WHERE ID='$_POST[ID]'");
  if($Query!=1) $Result="<h3 style='color:darkred'><strong>Operation Failed</strong></h3>";
  else $Result="<h3 style='color:darkred'><strong style='color:gray'>Success</strong></h3>";}

else if($_POST["Operations"]=="Book_Search")
  {$Query=mysqli_query($conn,"SELECT * FROM book WHERE ID='$_POST[ID]'");
  $Ans=mysqli_fetch_row($Query);
  $Rows=mysqli_num_rows($Query);
  if($Rows==0) $Result="<h3 style='color:darkred'><strong>Could Not Find Any Results</strong></h3>";
  else
  $Result="<h3 style='color:gray'><strong>Found!</strong></h3><br><table width=33% border=1 bordercolor=black style='color:darkblue; text-align:center'><caption><strong>Books Table</strong></caption><thead><tr><td>ID</td><td>Title</td><td>Publisher</td><td>Edition</td><td>Price</td></tr></thead><tbody style='background-color:darkgreen'><tr><td>$Ans[0]</td><td>$Ans[1]</td><td>$Ans[2]</td><td>$Ans[3]</td><td>$Ans[4]</td></tr></table>";
  }

else if($_POST["Operations"]=="Book_Display_All")
  {$Out="<table width=33% border=1 bordercolor=black style='color:darkblue; text-align:center'><caption><strong>Books Table</strong></caption><thead style='background-color:gray'><th>ID</th><th>Title</th><th>Publisher</th><th>Edition</th><th>Price</th></thead><tbody style='background-color:darkgreen'>";
  $Query=mysqli_query($conn,"SELECT * FROM book");
  while($Value=mysqli_fetch_row($Query))
  $Out=$Out."<tr><th>$Value[0]</th><td>$Value[1]</td><td>$Value[2]</td><td>$Value[3]</td><td>$Value[4]</tr>";
  $Result=$Out."</table>";}
  
  
else if($_POST["Operations"]=="Borrow_Add")
  if($_POST["user"]!="admin")
  $Result="<h3 style='color:darkred'><strong>Access denied as you do not have sufficient privileges</strong></h3>";
  else
  {$Query=mysqli_query($conn,"INSERT INTO `borrower`(Num,Name,Phone,BDate) VALUES ('$_POST[Num]','$_POST[Name]','$_POST[Phone]','$_POST[BDate]')");
  if($Query!=1) $Result="<h3 style='color:darkred'><strong>Operation Failed; Check Syntax And If Already Exist</strong></h3>";
  else $Result="<h3 style='color:darkred'><strong style='color:gray'>Success</strong></h3>";}
  
else if($_POST["Operations"]=="Borrow_Delete")
  if($_POST["user"]!="admin")
  $Result="<h3 style='color:darkred'><strong>Access denied as you do not have sufficient privileges</strong></h3>";
  else
  {
  $Ans=mysqli_num_rows(mysqli_query($conn,"SELECT * FROM borrower WHERE Num='$_POST[Num]'"));
  $Query=mysqli_query($conn,"DELETE FROM `borrower` WHERE Num='$_POST[Num]'");
  if($Query!=1) $Result="<h3 style='color:darkred'><strong>Operation Failed, This Is Normally Caused When The Borrower Is Registered In Either The Loan Or The Sell Database</strong></h3>";
  else $Result="<h3 style='color:darkred'><strong style='color:gray'>Success</strong></h3>";
  if($Ans==0) $Result="<h3 style='color:darkred'><strong>Could Not Find Any Results</strong></h3>";
  }
  
  
else if($_POST["Operations"]=="Sell_Add")
  if($_POST["user"]!="admin")
  $Result="<h3 style='color:darkred'><strong>Access denied as you do not have sufficient privileges</strong></h3>";
  else
  {
  $Query=mysqli_query($conn,"INSERT INTO `sell`(ID,Num,Sdate) VALUES ('$_POST[ID]','$_POST[Num]','$_POST[Sdate]')");
  if($Query!=1) $Result="<h3 style='color:darkred'><strong>Operation Failed; Check Syntax, Check That The Book And The Borrower Are Registered As Well </strong></h3>";
  else $Result="<h3 style='color:darkred'><strong style='color:gray'>Success</strong></h3>";
  }

else if($_POST["Operations"]=="Sell_Display_All")
  {$Out="<table width=33% border=1 bordercolor=black style='color:darkblue; text-align:center'><caption><strong>Sell Table</strong></caption><thead style='background-color:gray'><th>ID</th><th>Num</th><th>Sdate</th></thead><tbody style='background-color:darkgreen'>";
  $Query=mysqli_query($conn,"SELECT * FROM sell");
  while($Value=mysqli_fetch_row($Query))
  $Out=$Out."<tr><th>$Value[0]</th><td>$Value[1]</td><td>$Value[2]</td></tr>";
  $Result=$Out."</table>";}
  
  
else if($_POST["Operations"]=="Loan_Add")
  if($_POST["user"]!="admin")
  $Result="<h3 style='color:darkred'><strong>Access denied as you do not have sufficient privileges</strong></h3>";
  else
  {
  $Query=mysqli_query($conn,"INSERT INTO `loan`(ID,Num,Out_Date,Due_Date) VALUES ('$_POST[ID]','$_POST[Num]','$_POST[Out_Date]','$_POST[Due_Date]')");
  if($Query!=1) $Result="<h3 style='color:darkred'><strong>Operation Failed; Check Syntax, Check That The Book And The Borrower Are Registered As Well </strong></h3>";
  else $Result="<h3 style='color:darkred'><strong style='color:gray'>Success</strong></h3>";
  }

else if($_POST["Operations"]=="Loan_Display_All")
  {$Out="<table width=33% border=1 bordercolor=black style='color:darkblue; text-align:center'><caption><strong>Loans Table</strong></caption><thead style='background-color:gray'><th>ID</th><th>Num</th><th>Out_Date</th><th>Due_Date</th></thead><tbody style='background-color:darkgreen'>";
  $Query=mysqli_query($conn,"SELECT * FROM loan");
  while($Value=mysqli_fetch_row($Query))
  $Out=$Out."<tr><th>$Value[0]</th><td>$Value[1]</td><td>$Value[2]</td><td>$Value[3]</td></tr>";
  $Result=$Out."<tbody></table>";}
  

echo("<head><title>Library</title></head><body id=Pic><center><h1 style='color:gray'>Hi ".$_POST["user"]."! Connection To Server OK</h1><div id=Time></div><script src=http://127.0.0.1/Assets/Time.js></script>");
echo("<br><br><br>$Result<br><br><br><script src=http://127.0.0.1/Assets/Pictures.js></script><h4></h4><form method='POST' action='Main_UI.php'><input type='hidden' name='user' value='$_POST[user]'><input type='submit' value='Go Back'></form><script src='Processor.js'></script><strong><a href='../index.html'>Logout</a></strong></center></body>");
mysqli_close($conn);
?>