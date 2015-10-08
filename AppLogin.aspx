<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppLogin.aspx.cs" Inherits="BTS.Page.AppLogin" %>
<%@ Import Namespace="BTS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1">
    <title><%=Config.WEB_TITLE %></title>
    <link rel="stylesheet" href="style/<%=Config.CSS_STYLE %>" type="text/css" />  
</head>
<body>
<%
    if (outBufTop!=null)
   Response.Write(this.outBufTop.ToString());
%>
    <form id="form1" runat="server">
    <div>
    <table id="Table1" cellspacing="0"  align=center valign=>
   <tr>
   <td>
    <img src="img/sys/login_logo.jpg" />
   
   </td>
   
   </tr>
  <tr>  
    <!-- <th scope="row" class="spec" > -->
    <td>
    <table border=0 align=center>
    <tr>
        <td>Username</td>
         <td colspan=2><input type=text id="username" name="username" class="txtbox1" style="width:100px" value="" />
        </td>
    </tr>
    <tr>
        <td>Password</td>
         <td>
         <input type=password id="Text1" name="pwd" class="txtbox1" style="width:100px" value="" />
        </td>
        <td> <input type="submit" class="btn1" value="Login" /></td>
        </tr>
    <tr ><td colspan=3 class="font1" style="height:0px">
    <%
    if (outBuf!=null)
   Response.Write(this.outBuf.ToString());
%>    </td></tr>
    </table>
    </th>
  </tr>
    </table>  
    
    </div>
    </form>
</body>
</html>
