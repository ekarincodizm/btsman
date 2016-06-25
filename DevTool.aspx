<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DevTool.aspx.cs" Inherits="BTS.Page.DevTool" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    
<script language=javascript >
    //Prevent accidently run
    
<%
 if (!isSubmit) {
%>
    var pwd = prompt("Password?");
    if (pwd != "netta") { history.back();  }
    
<% } %>    
    
</script>    

<form name=form1 id=form1 method=Post runat=server>

Action:
<select name=devaction id=devaction>
<option value="none" selected>-- NONE --</option>
<option value="add_mock_teacher">Add Mockup Teacher</option>
<option value="add_mock_student">Add Mockup Student</option>
</select><br />

Param1: <input type="text" name="param1" id="param1" /> <br />
Param2: <input type="text" name="param2" id="param2" /> <br />
Param3: <input type="text" name="param3" id="param3" /> <br />

<input type=submit />
</form>






</body>
</html>
