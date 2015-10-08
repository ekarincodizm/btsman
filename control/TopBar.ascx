<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopBar.ascx.cs" Inherits="BTS.Control.TopBar" %>

<%@ Import Namespace="BTS" %>
<%@ Import Namespace="BTS.Constant" %>
<%@ Import Namespace="BTS.Entity" %>


<script language="javascript">
    function changeBranch() {
        var val = document.getElementById("login_branch_id").value;
        location.href = "AppLogin.aspx?action=change_branch&return_page=Home.aspx&branch_id=" + val;
    }
</script>
<!--  breanch select popup -->
<div id="divg" class="ModalBackground graydiv">  
</div>  
<div id="divSignin" style="border: '1px soild green'; display: none; z-index: 100002;  
    width: 550px; position:absolute;" >  
<div style="text-align:right;height:70px;width:300px;background-color:White;border:solid 1px lightyellow" style.display="none">  
<table align=center width="100%">
<tr>
<td align=center>เลือกสาขาที่จะปฏิบัติงาน: 
<select name="login_branch_id" id="login_branch_id">
<% 
    Branch[] branches = (Branch[])Session["BRANCHES"];
    if (branches!=null)
    foreach (Branch b in branches) 
    {   
%>
        <option value="<%=b._branchID%>"><%=b._branchName %></option>
<%  } %>
</select>
</td>
</tr>
<tr>
<td align=center>
<input type="button" class="btn1" value="ตกลง" onclick="closePopup();changeBranch();" />  
</td>
</tr>
</table>
</div>  
</div>  

    
<table border=0 "width=150px" height=100%>
<tr height=100>
<td bgcolor="#99ccff" width="1000px" height="100px" style="background: url('img/sys/banner1000.jpg') no-repeat;" onclick="location.href='Home.aspx'" align=right valign=top> v0.760 </td></tr>
<tr height=30 >
    <td>
        <table border=0 >
        <tr bgcolor=white>
            <td width="100px" align="center"><a href="Home.aspx">Home</a></td>
            <td>
                <table border=0 ><tr>
                
                <td width="500px" align="center"><!-- <img src="<%= Config.URL_PIC_SYS + "/mail.jpg" %>" width="16px" height="12px" />  คุณมี 2 ข้อความที่ยังไม่ได้เปิดอ่าน --> &nbsp</td>                        
    <!--            <td width="100px" align="center"><a href=#>About</a></td> -->
                <td width="400px" align="right" NOWRAP>
    <%
        string userHTML = "";
        string branchHTML = "";
        
        if (Session[SessionVar.USER]!=null)
        {
            AppUser user = (AppUser)Session[SessionVar.USER];
            userHTML = user._firstname + "&nbsp" + user._surname + "(<a href=\"LogOut.aspx\">Logout</a>)";

            //if (user.IsAdmin())
            {
                branchHTML = "สาขา: <a href=\"#\" onclick=\"showPopup();\" >" + user._branchName + "</a>";
            }
            /*
            else
            {
                branchHTML = "สาขา: " + user._branchName;
            }*/
        } else {
            userHTML = "Guest";
        }

        
    %>            
                &nbsp&nbsp&nbsp<%= userHTML %>&nbsp <%= branchHTML %>
                
                
                </td>               
                </tr></table>
           </td>
        </tr>       
    </td>
</tr>




