function Update()
{
var date=new Date();
var day = (date.getDate()<10?'0':'')+date.getDate();
var month = (date.getMonth()<10?'0':'')+(date.getMonth()+1);
var hours_mark;
var hours = date.getHours();
date.getHours()<12?hours_mark="AM":(hours-=12,hours_mark="PM");
hours<10?hours='0'+hours:'';
var minutes = (date.getMinutes()<10?'0':'')+date.getMinutes();
var seconds = (date.getSeconds()<10?'0':'')+date.getSeconds();
var day_week=["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"];
document.getElementById("Time").innerHTML="<center><strong><div style='color:darkblue'>"+day_week[date.getDay()]+" "+day+"/"+month+"/"+date.getFullYear()+" "+hours+":"+minutes+":"+seconds+" "+hours_mark+"</div></strong></center>";
}
document.getElementById("Time").innerHTML="<strong><div style='color:darkblue'>Retreiving Date Entries, Please Wait...</div><strong>";
setInterval(Update,1000);