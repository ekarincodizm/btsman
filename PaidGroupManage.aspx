<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaidGroupManage.aspx.cs" Inherits="BTS.Page.PaidGroupManage" %>
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
    <input type="hidden" id="target2ID" name="target2ID" value="<%=target2ID%>" />
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
<caption>กลุ่มอาจารย์  </caption>
  <tr >
    <td>
        <table width="100%" ><tr>
        <td height="24px">
            <!-- <a href="javascript:history.back()">Back..</a> -->
            <a href="javascript:setVal('actPage','add');doSubmit('PaidGroupManage.aspx');"><img src="img/sys/add.gif" border=0> เพิ่มกลุ่มอาจารย์ใหม่..</a>
        </td>
        <td align=right>
            <input type=text id="qsearch" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch()" />  
        </td>
        </tr></table>
    </td>
  </tr>
  <tr>
    <td align="center">
    <table id="Table2" cellspacing="0" summary="..." style="width:600px" border=0>  
      <tr>
        <th scope="col" align=center width="100px" NOWRAP>รหัสกลุ่ม</th>
	    <th scope="col" align=center width="200px">ชื่อกลุ่ม</th>
        <th scope="col" align=center width="200px">อัตราการจ่ายเงิน</th>
        <th scope="col" align=center width="300px">สมาชิกกลุ่ม</th>
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
<caption>กลุ่มอาจารย์: <%=actPage%> </caption>

  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>รหัสกลุ่ม</th>    
    <th scope="row" class="spec" ><input type=text id="groupID" name="groupID" class="txtbox1" style="width:150px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup._paidGroupID.ToString():""%>" /></th>
  </tr>
  
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ชื่อกลุ่ม</th>    
    <th scope="row" class="spec" ><input type=text id="name" name="name" class="txtbox1" style="width:150px" value="<%=thePaidGroup!=null?thePaidGroup._name:""%>" /></th>
  </tr>
<% if (actPage.Equals("edit"))
   { %>  
    <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>อาจารย์ในกลุ่ม</th>    
    <th scope="row" class="spec" >    
        <!-- member in group -->
        <table border=0>
        <%
    if (memberList != null)
    {
        for (int i = 0; i < memberList.Length; i++)
        {             
        %>
            <tr>
                <td><%= Teacher.GetTeacherID(memberList[i]._teacherID) + " " + memberList[i]._firstname + " " + memberList[i]._surname%></td>
                <td><a href="javascript:if (confirm('Remove this teacher from group?')) { setVal('actPage','remove_teacher_submit');setVal('targetID','<%=thePaidGroup._paidGroupID%>');setVal('target2ID','<%=memberList[i]._teacherID%>');doSubmit(); }"><img src="img/sys/delete.gif" border=0 alt="Remove teacher"></a></td>
            </tr>
        <%
    }
    }
        %>

        <!-- Add member -->        
        <tr>
            <td>
            <select id="teacher_id" name="teacher_id" class="txtbox1" style="width:160px" >
        <% 
            for (int i = 0; i < teacherList.Length; i++)
            {       
        %>       
            <option value="<%=teacherList[i]._teacherID %>" ><%= Teacher.GetTeacherID(teacherList[i]._teacherID) + " " + teacherList[i]._firstname + " " + teacherList[i]._surname%></option>
        <%  } %>
            </select>        
            </td>
            <td><a href="javascript:setVal('actPage','add_teacher_submit');setVal('targetID','<%=thePaidGroup._paidGroupID%>');doSubmit();"><img src="img/sys/add2.gif" border=0 alt="Add teacher"></a></td>        </tr>
        
        </table>
    
    </th>
  </tr>
<% } %>  
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>อัตราการจ่ายเงิน</th>
    <td>
       <table>
        <tr>
          <th>bound</th>
          <th>percent</th>
        </tr>
        <tr>
         <td> <input type=text id="Text1" name="bound1" class="txtbox1" style="width:100px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(1,1):"0"%>" /> </td>
         <td> <input type=text id="Text2" name="rate1" class="txtbox1" style="width:50px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(1,2):""%>" /> </td>
        </tr>
        <tr>
         <td> <input type=text id="Text3" name="bound2" class="txtbox1" style="width:100px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(2,1):""%>" /> </td>
         <td> <input type=text id="Text4" name="rate2" class="txtbox1" style="width:50px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(2,2):""%>" /> </td>
        </tr>
        <tr>
         <td> <input type=text id="Text5" name="bound3" class="txtbox1" style="width:100px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(3,1):""%>" /> </td>
         <td> <input type=text id="Text6" name="rate3" class="txtbox1" style="width:50px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(3,2):""%>" /> </td>
        </tr>
        <tr>
         <td> <input type=text id="Text7" name="bound4" class="txtbox1" style="width:100px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(4,1):""%>" /> </td>
         <td> <input type=text id="Text8" name="rate4" class="txtbox1" style="width:50px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(4,2):""%>" /> </td>
        </tr>
        <tr>
         <td> <input type=text id="Text9" name="bound5" class="txtbox1" style="width:100px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(5,1):""%>" /> </td>
         <td> <input type=text id="Text10" name="rate5" class="txtbox1" style="width:50px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(5,2):""%>" /> </td>
        </tr>
        <tr>
         <td> <input type=text id="Text11" name="bound6" class="txtbox1" style="width:100px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(6,1):""%>" /> </td>
         <td> <input type=text id="Text12" name="rate6" class="txtbox1" style="width:50px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(6,2):""%>" /> </td>
        </tr>
        <tr>
         <td> <input type=text id="Text13" name="bound7" class="txtbox1" style="width:100px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(7,1):""%>" /> </td>
         <td> <input type=text id="Text14" name="rate7" class="txtbox1" style="width:50px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(7,2):""%>" /> </td>
        </tr>
        <tr>
         <td> <input type=text id="Text15" name="bound8" class="txtbox1" style="width:100px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(8,1):""%>" /> </td>
         <td> <input type=text id="Text16" name="rate8" class="txtbox1" style="width:50px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(8,2):""%>" /> </td>
        </tr>
        <tr>
         <td> <input type=text id="Text17" name="bound9" class="txtbox1" style="width:100px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(9,1):""%>" /> </td>
         <td> <input type=text id="Text18" name="rate9" class="txtbox1" style="width:50px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(9,2):""%>" /> </td>
        </tr>
        <tr>
         <td> <input type=text id="Text19" name="bound10" class="txtbox1" style="width:100px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(10,1):""%>" /> </td>
         <td> <input type=text id="Text20" name="rate10" class="txtbox1" style="width:50px" onkeypress="checkNumber()" value="<%=thePaidGroup!=null?thePaidGroup.GetPaidinfo(10,2):""%>" /> </td>
        </tr>
       </table>
    </td>
  </tr>
  <tr>    
    <td scope="row" align="center" colspan="2" ><p align="center">
    <input type="button" class="btn1" value="Back" onclick="location.href='PaidGroupManage.aspx'"/>
    <input type="button" class="btn1" value="Save" onclick="if (checkEmptyText('groupID')) {setVal('actPage','<%=actPage%>_submit');doSubmit();}" />
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
