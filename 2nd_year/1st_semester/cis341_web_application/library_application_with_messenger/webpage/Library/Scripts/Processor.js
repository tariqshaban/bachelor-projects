function listener()
{
if (document.getElementById("Operations").value=="Default")
document.getElementById("field").innerHTML="<strong>Please Select An Operation<strong>";
else if (document.getElementById("Operations").value=="Book_Add")
document.getElementById("field").innerHTML="ID<br><input type='box' name='ID'><br><br>Title<br><input type='box' name='Title'><br><br>Publisher<br><input type='box' name='Publisher'><br><br>Edition<br><input type='box' name='Edition'><br><br>Price<br><input type='box' name='Price'><h4>*Requires Elevated Permissions</h4>";
else if (document.getElementById("Operations").value=="Book_Delete")
document.getElementById("field").innerHTML="ID<br><input type='box' name='ID'><br><h4 style='color:darkred'>Warning: You Cant Delete A Book If There Is A Borrower For It</h4><h4>*Requires Elevated Permissions</h4>";
else if (document.getElementById("Operations").value=="Book_Update")
document.getElementById("field").innerHTML="ID(current)<br><input type='box' name='ID'><br><br>Title<br><input type='box' name='Title'><br><br>Publisher<br><input type='box' name='Publisher'><br><br>Edition<br><input type='box' name='Edition'><br><br>Price<br><input type='box' name='Price'><h4>*Requires Elevated Permissions</h4>";
else if (document.getElementById("Operations").value=="Book_Search")
document.getElementById("field").innerHTML="ID<br><input type='box' name='ID'>";
else if (document.getElementById("Operations").value=="Book_Display_All")
document.getElementById("field").innerHTML="";

else if (document.getElementById("Operations").value=="Borrow_Add")
document.getElementById("field").innerHTML="Num<br><input type='box' name='Num'><br><br>Name<br><input type='box' name='Name'><br><br>Phone<br><input type='box' name='Phone'><br><br>BDate<br><input type='box' name='BDate'><h4>*Requires Elevated Permissions</h4>";
else if (document.getElementById("Operations").value=="Borrow_Delete")
document.getElementById("field").innerHTML="Num<br><input type='box' name='Num'><br><h4 style='color:darkred'>Warning: You Cant Delete A Borrower If He Made A Transaction</h4><h4>*Requires Elevated Permissions</h4>";

else if (document.getElementById("Operations").value=="Sell_Add")
document.getElementById("field").innerHTML="ID<br><input type='box' name='ID'><br><br>Num<br><input type='box' name='Num'><br><br>SDate<br><input type='box' name='Sdate'><h4>*Requires Elevated Permissions</h4>";
else if (document.getElementById("Operations").value=="Sell_Display_All")
document.getElementById("field").innerHTML="";

else if (document.getElementById("Operations").value=="Loan_Add")
document.getElementById("field").innerHTML="ID<br><input type='box' name='ID'><br><br>Num<br><input type='box' name='Num'><br><br>Out Date<br><input type='box' name='Out_Date'><br><br>Due_Date<br><input type='box' name='Due_Date'><h4>*Requires Elevated Permissions</h4>";
else if (document.getElementById("Operations").value=="Loan_Display_All")
document.getElementById("field").innerHTML="";
}