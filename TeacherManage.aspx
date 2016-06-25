<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TeacherManage.aspx.cs" Inherits="BTS.Page.TeacherManage" %>
<%@ Import Namespace="BTS" %>
<%@ Import Namespace="BTS.Entity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" >
    <title><%=Config.WEB_TITLE %></title>
    <link rel="stylesheet" href="style/<%=Config.CSS_STYLE %>" type="text/css" />    
</head>
<body>
<script type="text/javascript" src="js/util.js"></script>




<BTS:TopBar id=TopBar1 runat=server ucname="TopBar.ascx" />
<BTS:SideBar id=SideBar1 runat=server ucname="SideBar.ascx" />

    <form id="form1" enctype="multipart/form-data" runat="server" method="POST" >
    <input type="hidden" id="actPage" name="actPage" value="<%=actPage%>" />
    <input type="hidden" id="targetID" name="targetID" value="<%=targetID%>" />
    
    <div>
<!-- Error box -->    
<% if ((errorText != null) && (errorText.Length>0)) { %>
<table id="ErrTable" cellspacing="0" >
  <tr>
    <th scope="row" class="spec" ><p><font class="font3"><%=errorText%></font></p></th>
  </tr>
</table>
<% } %>
    
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
<caption>อาจารย์  </caption>
  <tr >
    <td>
        <table width="100%" ><tr>
        <td height="24px">
            <!-- <a href="javascript:history.back()">Back..</a> -->
            <a href="javascript:setVal('actPage','add');doSubmit('TeacherManage.aspx');"><img src="img/sys/add.gif" border=0> เพิ่มอาจารย์ใหม่..</a>
        </td>
        <td align=right>
            <input type=text id="qsearch" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch()" />  
        </td>
        </tr></table>
    </td>
  </tr>
  <tr>
    <td>
    <table id="Table2" cellspacing="0" summary="..." style="width:900px" border=0>  
      <tr>
        <th scope="col" align=center width="80px" NOWRAP>รหัสอาจารย์</th>
	    <th scope="col" align=center width="200px">ชื่อ - นามสกุล</th>
	    <th scope="col" align=center width="50px">เพศ</th>
	    <th scope="col" align=center width="220px">ที่อยู่</th>
        <th scope="col" align=center width="180px">ติดต่อ</th>
<!--        <th scope="col" align=center width="120px">ระดับรายได้</th> -->
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

<% } else if (actPage.Equals("view")) { %>

<table border=0 width="700px">
<tr>
<td align=left width="350px"><a href="javascript:history.back()"><< Back</a></td>
<td align=right width="350px"><a href="javascript:setVal('actPage','edit');setVal('targetID','<%=theTeacher._teacherID %>');doSubmit()"><img src="img/sys/edit.gif" border=0 alt="Edit">แก้ไขข้อมูล</a>&nbsp</td>
</tr>
</table>

<%= outBuf.ToString() %>

<% } else if ((actPage.Equals("add")) || (actPage.Equals("edit"))) { %>

<!-- Add/Edit -->
<table id="Table1" cellspacing="0" >
<caption>อาจารย์: <%=actPage%> </caption>

  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>ชื่อ</th>    
    <th scope="row" class="spec" ><input type=text id="firstname" name="firstname" class="txtbox1" style="width:150px" value="<%=theTeacher!=null?theTeacher._firstname:""%>" /></th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>นามสกุล</th>    
    <th scope="row" class="spec" ><input type=text id="surname" name="surname" class="txtbox1" style="width:150px" value="<%=theTeacher!=null?theTeacher._surname:""%>" /></th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>เพศ</th>    
    <th scope="row" class="spec" ><font class="font01">
        <input type="radio" name="sex" value="Male" checked <%= ((theTeacher!=null)&&(theTeacher._sex.Equals("Male")))?"checked":"" %> >ชาย &nbsp
        <input type="radio" name="sex" value="Female" <%= ((theTeacher!=null)&&(theTeacher._sex.Equals("Female")))?"checked":"" %>>หญิง
        </font>
    </th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>วันเกิด</th>    
    <th scope="row" class="spec" >  
    
    <link rel="stylesheet" type="text/css" href="js/epoch_v106_en/epoch_styles.css" />
    <script type="text/javascript" src="js/epoch_v106_en/epoch_classes.js"></script>
    <script type="text/javascript">
        /*You can also place this code in a separate file and link to it like epoch_classes.js*/
        var bas_cal, dp_cal, ms_cal;
        window.onload = function() {
           // bas_cal = new Epoch('epoch_basic', 'flat', document.getElementById('basic_container'));
            dp_cal = new Epoch('epoch_popup', 'popup', document.getElementById('birthday'), false, 543);
          //  ms_cal = new Epoch('epoch_multi', 'flat', document.getElementById('multi_container'), true);
        };
    </script>
      <input id="birthday" name="birthday" type="text" class="txtbox1" width="150px" readonly value="<%=theTeacher!=null?theTeacher._birthday.ToString("dd/MM/yyyy", ci):DateTime.Today.ToString("dd/MM/yyyy", ci)%>"/>
    
    </th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>รหัสบัตรประชาชน</th>    
    <th scope="row" class="spec" ><input type=text id="citizen_id" name="citizen_id" class="txtbox1" style="width:150px" value="<%=theTeacher!=null?theTeacher._citizenID:""%>" /></th>
  </tr>  
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>รูปภาพ</th>    
    <th scope="row" class="spec" >
        <img width="75px" height="100px"  src="<%=Config.URL_PIC_TEACHER + "/" + (theTeacher!=null?theTeacher._img:"noimg.jpg") %>" /> <br/><br />        
        <input type=file id="portrait" name="portrait1" class="txtbox1" runat=server style="width:400px" />
        <input type=hidden id="img_old" name="img_old" value="<%=theTeacher!=null?theTeacher._img:"" %>" />
        </th>
  </tr>
  
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>ที่อยู่</th>    
    <th scope="row" class="spec"><textarea class="txtarea1" id="addr" name="addr" style="width:300px;height:70px"><%=theTeacher != null ? theTeacher._addr : ""%></textarea></th>    
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>โทรศัพท์</th>    
    <th scope="row" class="spec" ><input type=text id="tel" name="tel" class="txtbox1" style="width:300px" value="<%=theTeacher!=null?theTeacher._tel:""%>" /></th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>อีเมล</th>    
    <th scope="row" class="spec" ><input type=text id="email" name="email" class="txtbox1" style="width:300px" value="<%=theTeacher!=null?theTeacher._email:""%>" /></th>
  </tr>
  <tr>    
    <td scope="row" align="center" colspan="2" ><p align="center">
    <input type="button" class="btn1" value="Back" onclick="history.back()"/>
    <input type="button" class="btn1" value="Save" onclick="if (checkEmptyText('firstname')) {setVal('actPage','<%=actPage%>_submit');doSubmit();}" />
    <input type="reset" class="btn1" value="Reset" />   
    </p></td>
  </tr>  
  
<% if (!actPage.Equals("add")) { %>
   <tr>
    <td colspan="2" align="center">
    <div name="divhistory" id="divhistory" onclick="clickhistory()" >
    <table id="Table3" cellspacing="0" summary="..." style="width:550px" border=0>
    <caption> ประวัติการสอน: </caption>
      <tr>
        <th scope="col" align=center width="100px" NOWRAP>วันที่เริ่มคอร์ส</th>
	    <th scope="col" align=center width="80px">รหัสคอร์ส</th>
	    <th scope="col" align=center width="320px">ชื่อคอร์ส</th>
        <th scope="col" align=center width="50px">ราคา</th>
      </tr>
<%
   Response.Write(this.outBuf3.ToString());
%>
    </table>
    
    </div>
    </td>
  </tr> 
<% } %>
</table>


<% } %>
        

        
    </div>
    </form>

    
<BTS:EndBar id=EndBar1 runat=server ucname="EndBar.ascx" />

</body>
</html>
