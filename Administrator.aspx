<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Administrator.aspx.cs" Inherits="BTS.Page.Administrator" %>
<%@ Import Namespace="BTS" %>
<%@ Import Namespace="BTS.Entity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" >
    <title><%=Config.WEB_TITLE %></title>
    <link rel="stylesheet" href="style/<%=Config.CSS_STYLE %>" type="text/css" />    
</head>
<body>

    <form id="form1" enctype="multipart/form-data" runat="server" method="POST" >
<script type="text/javascript" src="js/util.js"></script>




<BTS:TopBar id=TopBar1 runat=server ucname="TopBar.ascx" />
<BTS:SideBar id=SideBar1 runat=server ucname="SideBar.ascx" />

    <input type="hidden" id="actPage" name="actPage" value="<%=actPage%>" />
    <input type="hidden" id="targetID" name="targetID" value="<%=targetID%>" />
    
    <div>
    
<% if ((actPage == null) || (actPage.Trim().Length==0) || (actPage.Equals("list")))  {  %>
<!-- List -->
<div name="divchklist" id="divdetail" style="border-color:Black;background-color:White; filter:alpha(opacity=75); -moz-opacity: 0.75; opacity: 0.75;display:none;position:relative">
<img src="img/sys/loading.gif" />
</div>
<script type="text/javascript" >
    var layerName = 'divdetail';
    var offsetY = -10;
    var offsetX = 20;

    function showDiv(divtxt) {

        //ajaxPost("AjaxService.aspx", "svc=qchklist&sdid=" + sdid, layerName);
        
        //eval('document.getElementById(\''+layerName+'\').style.width=\'240px\'');
        //eval('document.getElementById(\''+layerName+'\').style.height=\'120px\'');
        eval('document.getElementById(\'' + layerName + '\').style.visibility=\'visible\'');
        eval('document.getElementById(\'' + layerName + '\').style.display=\'block\'');
        eval('document.getElementById(\'' + layerName + '\').style.position=\'absolute\'');
        //eval('document.getElementById(\''+layerName+'\').style.padding=\''+layerPaddingT+'px '+layerPaddingR+'px '+layerPaddingB+'px '+layerPaddingL+'px\'');

        document.getElementById(layerName).innerHTML = "<img class=\"img1\" src=\""+divtxt+"\" >";
    }

    function hideDiv() {
        document.getElementById(layerName).style.display = "none";
        document.getElementById(layerName).innerHTML = "<img src=\"img/sys/loading.gif\"/>";
    }

    function doSearch() {
        if (event.keyCode == 13)
            document.getElementById("actPage").value="list";
    }
    // Detect if the browser is IE or not.
    // If it is not IE, we assume that the browser is NS.
    var IE = document.all ? true : false
    // If NS -- that is, !IE -- then set up for mouse capture
    if (!IE) document.captureEvents(Event.MOUSEMOVE)
    // Set-up to use getMouseXY function onMouseMove
    document.onmousemove = mouseMove;
    // Main function to retrieve mouse x-y pos.s

</script>

<table id="mytable" cellspacing="0" summary="..." style="width:800px" border=0>
<caption>ผู้ใช้งานระบบ</caption>
  <tr >
    <td>
        <table width="100%" ><tr>
        <td height="24px">
            <!-- <a href="javascript:history.back()">Back..</a> -->
            <a href="javascript:setVal('actPage','add');doSubmit('Administrator.aspx');"><img src="img/sys/add.gif" border=0> เพิ่มผู้ใช้ใหม่..</a>
        </td>
        <td align=right>
            <input type=text id="qsearch" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch()" />  
        </td>
        </tr></table>
    </td>
  </tr>
  <tr>
    <td align="center">
    <table id="Table2" cellspacing="0" summary="..." style="width:750px" border=0>  
      <tr>
        <th scope="col" align=center width="30px" NOWRAP>รหัส</th>
        <th scope="col" align=center width="135px" NOWRAP>ชื่อเข้าใช้</th>
	    <th scope="col" align=center width="300px">ชื่อ - นามสกุล</th>
        <th scope="col" align=center width="135px">ระดับการเข้าใช้</th>
        <th scope="col" align=center width="150px">สาขาประจำ</th>
	    <th scope="col" width="30px" align=center abbr="Action">Action</th>	
      </tr>
<%
   Response.Write(this.outBuf.ToString());
%>
    </table>
    </td>
   </tr>
  <!-- Table Footer -->
  <tr >
    <td>
        <table width="100%" ><tr>
        <td height="20px">
            &nbsp
        </td>
        <td align=right>
<% 
    Response.Write(this.outBuf2.ToString());
%>                       
        </td>
        </tr></table>
    </td>
  </tr>   
</table>               


<% } else if ((actPage.Equals("add")) || (actPage.Equals("edit"))) { %>

<!-- Add/Edit -->
<table id="Table1" cellspacing="0" >
<caption>ผู้ใช้งานระบบ: <%=actPage%> </caption>

<% if (actPage.Equals("edit")) { %>  
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>รหัสผู้ใช้</th>    
    <th scope="row" class="spec" ><%=theUser!=null?theUser._userId.ToString():"-"%></th>
  </tr>  

<% } %>  


  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ชื่อเข้าใช้</th>    
    <th scope="row" class="spec" ><input type=text id="username" <%=(!actPage.Equals("add"))?"disabled":""%> name="username" class="txtbox1" style="width:150px" value="<%=theUser!=null?theUser._username:""%>" /></th>
  </tr>

  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>รหัสผ่าน</th>    
    <th scope="row" class="spec" ><input type=password id="passwd" name="passwd" class="txtbox1" style="width:150px" value="<%=theUser!=null?theUser._passwd:""%>" /></th>
  </tr>
  
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ชื่อ</th>    
    <th scope="row" class="spec" ><input type=text id="firstname" name="firstname" class="txtbox1" style="width:150px" value="<%=theUser!=null?theUser._firstname:""%>" /></th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>นามสกุล</th>    
    <th scope="row" class="spec" ><input type=text id="surname" name="surname" class="txtbox1" style="width:150px" value="<%=theUser!=null?theUser._surname:""%>" /></th>
  </tr>
  
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ระดับผู้ใช้</th>    
    <th scope="row" class="spec" >
    
        <select id="role_id" name="role_id" class="txtbox1" style="width:160px" >
        <% 
        for (int i = 0; i < roleList.Length; i++)
        {
            string selected = ((theUser != null) && (theUser._roleId == roleList[i]._roleId)) ? "selected" : "";
        %>       
        <option value="<%=roleList[i]._roleId %>" <%= selected %> ><%=roleList[i]._name %></option>
        <% } %>
        </select>
    </th>
  </tr>
  
    <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>สาขาประจำ</th>    
    <th scope="row" class="spec" >
    
        <select id="Select1" name="branch_id" class="txtbox1" style="width:160px" >
        <% 
        for (int i = 0; i < branchList.Length; i++)
        {
            string selected = ((theUser != null) && (theUser._branchID == branchList[i]._branchID)) ? "selected" : "";
        %>       
        <option value="<%=branchList[i]._branchID %>" <%= selected %> ><%=branchList[i]._branchName%></option>
        <% } %>
        </select>
    </th>
  </tr>

  <tr>    
    <td scope="row" align="center" colspan="2" ><p align="center">
    <input type="button" class="btn1" value="Back" onclick="history.back()"/>
    <input type="button" class="btn1" value="Save" onclick="if (checkEmptyText('username') && checkEmptyText('passwd') ) {setVal('actPage','<%=actPage%>_submit');doSubmit();}" />
    <input type="reset" class="btn1" value="Reset" />   
    </p></td>
  </tr>  
</table>


<% } %>
        

        
    </div>

    
<BTS:EndBar id=EndBar1 runat=server ucname="EndBar.ascx" />

    </form>

    
</body>
</html>
