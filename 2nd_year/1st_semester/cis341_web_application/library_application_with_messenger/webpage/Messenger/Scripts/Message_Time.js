var date=new Date();
var day = (date.getDate()<10?'0':'')+date.getDate();
var month = (date.getMonth()<10?'0':'')+date.getMonth();
var hours_mark;
var hours = date.getHours();
date.getHours()<12?hours_mark="AM":(hours-=12,hours_mark="PM");
hours<10?hours='0'+hours:'';
var minutes = (date.getMinutes()<10?'0':'')+date.getMinutes();
var seconds = (date.getSeconds()<10?'0':'')+date.getSeconds();
var day_week=["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"];
document.getElementById("Message_Time").value=day_week[date.getDay()]+" "+day+"/"+month+"/"+date.getFullYear()+" "+hours+":"+minutes+":"+seconds+" "+hours_mark;