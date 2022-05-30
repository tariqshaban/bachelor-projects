<?php
echo '<!DOCTYPE html>
<html lang="en" >
<head>
  <meta charset="UTF-8">
  <title>Marks</title>
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.1.2/css/bootstrap.min.css">
<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.2.0/css/all.css"><link rel="stylesheet" href="./Table.css">
</head>
<body style="background-image: url(Back.jpg); background-size: cover">';

$error='<center><strong><h1 style="margin-top:250px; color:red; border-radius: 25px; width: 600px; text-align:center; background-color:rgba(0, 0, 0, 0.5);"> Could Not Reach Host</h1></strong></center>';
$conn=mysqli_connect("127.0.0.1","root");
if(!$conn)
die($error);
$db=mysqli_select_db($conn,"Student");
if(!$db)
die($error);
$pass=md5($_POST['password']);
$Validate=mysqli_query($conn,"SELECT std_id FROM Password WHERE std_id='$_POST[id]' AND password='$pass'");

if(mysqli_num_rows($Validate)==1){

$loop =  mysqli_query($conn,"select id,first_name, last_name,grade from Student where id='$_POST[id]'");

$Value=mysqli_fetch_row($loop);
if(mysqli_fetch_row(mysqli_query($conn,"select write_ac from uac where id='$_POST[id]'"))[0]==false){
    $Students[]=array("ID" => "$Value[0]","name" => "$Value[1]  $Value[2]","grade" => "$Value[3]");
    echo '
<table class="table table-bordered table-striped" id="gradeTable" style="margin-top:150px; width:800px">
  <thead class="thead-dark">
    <tr>
	  <th scope="col" class="name-col sortable" data-field="name">ID <span class="sort-arrow"></span></th>
      <th scope="col" class="name-col sortable" data-field="name">Name <span class="sort-arrow"></span></th>
      <th scope="col" class="grade-col sortable" data-field="grade">Grade <span class="sort-arrow"></span></th>
    </tr>
  </thead>
  <tbody>
  </tbody>
</table>
<form id="studentForm"></form>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>';
}
else{
$loop1 =  mysqli_query($conn,"select id,first_name, last_name,grade from student where id !='$_POST[id]'");
$count=0;
$Students=array();
while($Value = mysqli_fetch_row($loop1))
{
	$Students[]=array("ID" => "$Value[0]","name" => "$Value[1]  $Value[2]","grade" => "$Value[3]");
}
echo '
<table class="table table-bordered table-striped" id="gradeTable" style="margin-top:150px; width:800px">
  <thead class="thead-dark">
    <tr>
	  <th scope="col" class="name-col sortable" data-field="name">ID <span class="sort-arrow"></span></th>
      <th scope="col" class="name-col sortable" data-field="name">Name <span class="sort-arrow"></span></th>
      <th scope="col" class="grade-col sortable" data-field="grade">Grade <span class="sort-arrow"></span></th>
    </tr>
  </thead>
  <tbody>
  </tbody>
  <tfoot>
    <tr>
      <td style="background-color:white"><input type="number" form="studentForm" id="name-field" name="name" class="form-control" placeholder="Student ID" required /></td>
      <td style="background-color:white" class="grade-col"><input type="number" form="studentForm" name="grade" class="form-control" placeholder="Grade" id="grade-field" required />
        <button type="submit" form="studentForm" class="btn btn-success">+</button></td>
      <td style="background-color:white"><span id="average">0</span></td>
    </tr>
  </tfoot>
</table>
<form id="studentForm"></form>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>';

}}
else{
    die('
    <center><strong><h1 style="margin-top:250px; color:red; border-radius: 25px; width: 600px; text-align:center; background-color:rgba(0, 0, 0, 0.5);"> Invalid Username Or Password</h1></strong></center>');
}
echo '</body></html>';
?>

<script>
let table;
let tbody;
let selectAllCheckbox;
let averageContainer;
let passingGrade = 50;
let students = <?php echo json_encode($Students) ?>;

const validators = [
  function(data) {
    const grade = parseFloat(data.grade);
    if(grade < 0 || grade > 100) {
      return {
        field: 'grade',
        message: 'Grade needs to be between 0 and 100'
      }
    }
    
    return true;
  }
]

function addRow(student) {
  tbody.append(studentRowTemplate(student));
}

function studentRowTemplate(student) {
  const gradeClass = (parseFloat(student.grade) >= passingGrade) ? 'grade-pass' : 'grade-fail';
  return `<tr>
      <td class="name-col" style="background-color:white"><span  data-field="name">${student.ID}</span></td>
      <td class="name-col" style="background-color:white"><span  data-field="name">${student.name}</span></td>
      <td class="grade-col ${gradeClass}"><span  data-field="grade">${student.grade}</span></td>
    </tr>`;
}

function calculateAverage() {
  let total = 0;
  for(let i = 0; i < students.length; i++) {
    total += parseFloat(students[i].grade);
  }
  if(total === 0) return 0;
  return (total / students.length).toFixed(2);
}

function addStudent(student) {
  students.push(student);
  addRow(student);
  averageContainer.text(calculateAverage());
}

function renderStudentTable() {
  tbody.html('');
  for(let i = 0; i < students.length; i++) {
    addRow(students[i]);
  }
  
  renderAverage();
}

function renderAverage() {
  averageContainer.text(calculateAverage());
}

function checkRowSelectedStatus() {
  const inputs = tbody.find('.row-checkbox');
  let checkState = true;
  
  if(students.length > 0) {
    for(let i = 0; i < inputs.length; i++) {
      if(!inputs[i].checked) {
        checkState = false;
        break;
      }
    }
  } else {
    checkState = false;
  }
  
  selectAllCheckbox[0].checked = checkState;
}

function getStudentRow(element) {
  const row = $(element).closest('tr');
  const studentIndex = row.index();
  
  return {
    row: row,
    studentIndex: studentIndex
  }
}

$(function() {
	
  const idField = $('#id-field');
  const nameField = $('#name-field');
  const gradeField = $('#grade-field');
  table = $('#gradeTable');
  tbody = table.find('tbody');
  averageContainer = $('#average');
  selectAllCheckbox = $('#select-all');
    
  $('th.sortable').on('click', function() {
    const row = $(this).parent();
    const field = $(this).attr('data-field');
    let dir = ($(this).attr('data-dir'));
    if(dir === undefined) {
      dir = 1;
    } else {
      dir = parseInt(dir) * -1;
    }
    
    students.sort(function(a, b) {
      let aValue, bValue;
      switch(field) {
        case 'name':
          aValue = a.name.toUpperCase();
          bValue = b.name.toUpperCase();
          if(aValue > bValue) return 1 * dir;
          if(aValue < bValue) return -1 * dir;
          return 0;
          break;
        case 'grade':
          aValue = parseFloat(a.grade);
          bValue = parseFloat(b.grade);
          return dir * (aValue - bValue);
      }
    });
    
    renderStudentTable();
    row.find('.sort-arrow').html('');
    const icon = dir === 1 ? '<i class="fas fa-long-arrow-alt-down"></i>' :  '<i class="fas fa-long-arrow-alt-up"></i>';
    $(this).find('.sort-arrow').html(icon);
    $(this).attr('data-dir', dir);
  })
  
  tbody.on('change', '.row-checkbox', function() {
    checkRowSelectedStatus();
  });
  
  tbody.on('click', '.editable', function() {
    const {row, studentIndex} = getStudentRow(this);
    const student = students[studentIndex];
    const field = $(this).attr('data-field');
    
    const inputType = field === 'name' ? 'text' : 'number';
    const newInput = $(`<input type="${inputType}" id="editable-input" class="form-control" value="${student[field]}" />`);
    
    const generateSpan = function(data) {
      return `<span class="editable" data-field="${field}">${data}</span>`
    }

    const td = $(this).parent();
        
    td.html(newInput);
    newInput.trigger('focus');
    newInput
      .on('blur', function() {
        newInput.remove();
        student[field] = newInput.val().trim();
        students[studentIndex] = student;
        if(field === 'grade') {
          td.removeClass('grade-pass').removeClass('grade-fail');
          const gradeClass = student.grade >= passingGrade ? 'grade-pass' : 'grade-fail';
          td.addClass(gradeClass);
          renderAverage();
        }
        td.html(generateSpan(student[field]));
      })
      .on('keydown', function(event) {
        switch(event.keyCode) {
          case 13: // enter
            newInput.trigger('blur');
            break;
          case 27: // esc
            newInput.remove();
            td.html(generateSpan(student[field]));
            break;
        }
      })
  });
  
  tbody.on('click', '.delete-row-btn', function() {
    const row = $(this).closest('tr');
    const studentIndex = row.index();

    students.splice(studentIndex, 1);
    row.remove();
    renderAverage();
    checkRowSelectedStatus();
  });
  
  table.on('click', 'tbody tr', function(e) {
    if(!$(e.target).hasClass('row-checkbox')
       && !$(e.target).hasClass('editable')
       && !$(e.target).hasClass('form-control')
      ) {
      const checkbox = $(this).find('.row-checkbox');
      checkbox.prop('checked', !checkbox.prop('checked'));
      checkbox.trigger('change');
    }
  });
  
  students.forEach(function(student) {
    addRow(student);
    renderAverage();
  });
  
  $('#studentForm').on('submit', function(event) {//////////////////////////////////////////////////////
    event.preventDefault();
    nameField.removeClass('is-invalid');
    gradeField.removeClass('is-invalid');
	
	var x=document.getElementById("name-field").value;
	const row = $("#gradeTable td:contains("+x+")").parent();
	if(row.length==1 && !(document.getElementById("grade-field").value<0 || document.getElementById("grade-field").value>100))
	{
	var name= $("#gradeTable td:contains("+x+")").parent().find("td:eq(1)").text();
    const studentIndex = row.index();
    students.splice(studentIndex, 1);
    row.remove();
	
	renderAverage();
    
    const formData = new FormData(event.target);
    const data = {
	  ID: formData.get('name'),
      name: name,
      grade: parseFloat(formData.get('grade'))
    }
    
    const errors = [];
    for(let i = 0; i < validators.length; i++) {
      const result = validators[i](data);
      if(result !== true) {
        console.error(result.message);
        if(result.field === 'name') {
          nameField.addClass('is-invalid');
        } else {
          gradeField.addClass('is-invalid');
        }
        errors.push(result);
      }
    }
    
    if(errors.length > 0) {
      return;
    }
    addStudent(data);
	
    $.ajax({
	type: 'post',
    url: '/Update.php',
    data: {ID: document.getElementById("name-field").value, Grade: document.getElementById("grade-field").value},
   });
	
	event.target.reset();
	document.getElementById("name-field").placeholder="Student ID";
    }
	else
	{
		event.target.reset();
		document.getElementById("name-field").placeholder="Invalid!";
	}});
  
  selectAllCheckbox.on('click', function(e) {
    tbody.find('.row-checkbox').prop('checked', this.checked);
  });
  
  $('#delete-all-checked-btn').on('click', function() {
    let selectedIndices = [];

    tbody.find('.row-checkbox:checked').each(function() {
      selectedIndices.push($(this).closest('tr').index());
    });
    
    for(let i = selectedIndices.length - 1; i >= 0; i--) {
      students.splice(selectedIndices[i], 1);
    }
    
    renderStudentTable();
    checkRowSelectedStatus();
  });
});
</script>