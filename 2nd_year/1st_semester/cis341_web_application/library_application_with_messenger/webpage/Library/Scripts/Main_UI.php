<?php
$error="<head><title>Library</title></head><body style='background-color:gray'><br><br><br><br><br><br><br><br><br><center><h1 style='color:red'>Could Not Reach Host</h1><br><h2><a href='register.html'>Go Back</a></h2></center></body>";
$conn=mysqli_connect("127.0.0.1","root");
if(!$conn)
die($error);
$db=mysqli_select_db($conn,"library");
if(!$db)
die($error);
echo("<head><title>Library</title></head><body id=Pic><center><h1 style='color:green'>Hi ".$_POST["user"]."! Connection To Server OK</h1><div id=Time></div><script src=http://127.0.0.1/Assets/Time.js></script>");
echo("<h3 style='color:blue'>What Operations Do You Wish To Enroll?</h3><br>");
echo("<form method='POST' action='Processor.php'><input type='hidden' name='user' value='$_POST[user]'>
<select style='-moz-border-radius:15px; border-radius:15px; border:solid 1px blue' id='Operations'  name='Operations' onchange='listener()'>
<option value='Default'>---</option>
<optgroup label='Book Operations'><option value='Book_Add'>Add Book</option><option value='Book_Delete'>Delete Book</option><option value='Book_Update'>Update Book</option><option value='Book_Search'>Search Book</option><option value='Book_Display_All'>Display All Books</option>
<optgroup label='Borrow Operations'><option value='Borrow_Add'>Add Borrower</option><option value='Borrow_Delete'>Delete Borrower</option>
<optgroup label='Sell Operations'><option value='Sell_Add'>Add Selled Books</option><option value='Sell_Display_All'>Display All Selled Books</option>
<optgroup label='Loan Operations'><option value='Loan_Add'>Add Loan</option><option value='Loan_Display_All'>Display All Loans</option>
</select>
<br><br><div id='field'><strong>Please Select An Operation</strong></div><br><input type='submit' value='Submit'>
</form><br><br>");
echo("<script src=http://127.0.0.1/Assets/Pictures.js></script><h4><a href='../index.html'>Logout</a></h4></center><script src=Processor.js></script></body>");
mysqli_close($conn);
?>