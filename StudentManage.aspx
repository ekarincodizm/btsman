<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentManage.aspx.cs" Inherits="BTS.Page.StudentManage" %>
<%@ Import Namespace="BTS" %>
<%@ Import Namespace="BTS.Entity" %>
<%@ Import Namespace="BTS.Util" %>

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
<%  if ((Request["errorText"] != null) && (Request["errorText"].Length > 0))   { %>
<table id="ErrTable" cellspacing="0" >
  <tr>
    <th scope="row" class="spec" ><p><font class="font3"><%=Request["errorText"]%></font></p></th>
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

    function clickhistory() {
    if (document.getElementById('divhistory').style.display == 'none')
        eval('document.getElementById(\'divhistory\').style.display=\'table\'');
    else
        eval('document.getElementById(\'divhistory\').style.display=\'none\'');
    }

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
<caption>นักเรียน  </caption>
  <tr >
    <td>
        <table width="100%" ><tr>
        <td height="24px" width="200px">
            <!-- <a href="javascript:history.back()">Back..</a> -->
            <a href="javascript:setVal('actPage','add');doSubmit('StudentManage.aspx');"><img src="img/sys/add.gif" border=0> เพิ่มนักเรียนใหม่..</a>
        </td>
        <td height="24px">
            <a href="javascript:setVal('actPage','save_as_excel');setVal('targetID','none');doSubmit()"><img src="img/sys/excel.gif" border=0 alt="Edit">ดาวน์โหลดทั้งหมดเป็น Excel</a>&nbsp
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
        <th scope="col" align=center width="80px" NOWRAP>รหัสนักเรียน</th>
	    <th scope="col" align=center width="200px">ชื่อ - นามสกุล</th>
	    <th scope="col" align=center width="40px">เพศ</th>
	    <th scope="col" align=center width="30px">อายุ</th>
        <th scope="col" align=center width="120px">โรงเรียน</th>
        <th scope="col" align=center width="50px">ระดับชั้น</th>
        <th scope="col" align=center width="140px">เบอร์ติดต่อ</th>
        <th scope="col" align=center width="140px">E-Mail</th>
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
<td align=right width="350px"><a href="javascript:setVal('actPage','edit');setVal('targetID','<%=theStudent._studentID %>');doSubmit()"><img src="img/sys/edit.gif" border=0 alt="Edit">แก้ไขข้อมูล</a>&nbsp</td>
</tr>
</table>

<%= outBuf.ToString() %>

<% } else if ((actPage.Equals("add")) || (actPage.Equals("edit"))) { %>

<!-- Add/Edit -->
<table id="Table1" cellspacing="0" >
<caption>นักเรียน: <%=actPage%> </caption>

  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ชื่อ</th>    
    <th scope="row" class="spec" ><input type=text id="firstname" name="firstname" class="txtbox1" style="width:150px" value="<%=theStudent!=null?theStudent._firstname:""%>" /></th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>นามสกุล</th>    
    <th scope="row" class="spec" ><input type=text id="surname" name="surname" class="txtbox1" style="width:150px" value="<%=theStudent!=null?theStudent._surname:""%>" /></th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ชื่อเล่น</th>    
    <th scope="row" class="spec" ><input type=text id="nickname" name="nickname" class="txtbox1" style="width:150px" value="<%=theStudent!=null?theStudent._nickname:""%>" /></th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>รหัสบัตรประชาชน</th>    
    <th scope="row" class="spec" ><input type=text id="citizen_id" name="citizen_id" class="txtbox1" style="width:150px" value="<%=theStudent!=null?theStudent._citizenID:""%>" /></th>
  </tr>      
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>เพศ</th>    
    <th scope="row" class="spec" ><font class="font01">
        <input type="radio" name="sex" value="Male" checked <%= ((theStudent!=null)&&(theStudent._sex.Equals("Male")))?"checked":"" %> >ชาย &nbsp
        <input type="radio" name="sex" value="Female" <%= ((theStudent!=null)&&(theStudent._sex.Equals("Female")))?"checked":"" %>>หญิง
        </font>
    </th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>รูปภาพ</th>    
    <th scope="row" class="spec" >
        <img width="75px" height="100px"  src="<%=Config.URL_PIC_STUDENT + "/" + (theStudent!=null?theStudent._img:"noimg.jpg") %>" /> <br/><br />        
        <input type=file id="portrait" name="portrait1" class="txtbox1" runat=server style="width:400px" />
        <input type=hidden id="img_old" name="img_old" value="<%=theStudent!=null?theStudent._img:"" %>" />
        </th>
  </tr>  
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>วันเกิด</th>    
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
      <input id="birthday" name="birthday" type="text" class="txtbox1" width="150px" readonly value="<%=theStudent!=null?theStudent._birthday.ToString("dd/MM/yyyy", ci):DateTime.Today.ToString("dd/MM/yyyy", ci)%>"/>
    
    </th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>โรงเรียน</th>    
    <th scope="row" class="spec" ><input type=text id="school" name="school" class="txtbox1" style="width:300px" value="<%=theStudent!=null?theStudent._school:""%>" /></th>
  </tr>  
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ระดับชั้น</th>    
    <th scope="row" class="spec" >
    <select id="level" name="level" class="txtbox1" style="width:120px" >
<% 
    for (int i = 1; i <= 16; i++)
    {
        string selected = ((theStudent != null) && (theStudent._level == i)) ? "selected" : "";
%>       
    <option value="<%=i%>" <%= selected %> ><%= StringUtil.ConvertEducateLevel(i) %></option>
<% } %>
    <option value="0" <%= ((theStudent != null) && (theStudent._level == 0)) ? "selected" : "" %> ><%= StringUtil.ConvertEducateLevel(0) %></option>
    </select>
    </th>
  </tr>  
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ที่อยู่</th>    
    <th scope="row" class="spec"><textarea class="txtarea1" id="addr" name="addr" style="width:300px;height:70px"><%=theStudent != null ? theStudent._addr : ""%></textarea></th>    
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>โทรศัพท์มือถือ</th>    
    <th scope="row" class="spec" >
        <%
            String tel1 = "";
            String tel2 = "";
            String tel3 = "";

            if ((theStudent != null) && (theStudent._tel != null) && (theStudent._tel.Length>0))
            {
                if (theStudent._tel.Length >= 3) tel1 = theStudent._tel.Substring(0, 3);
                if (theStudent._tel.Length >= 6) tel2 = theStudent._tel.Substring(3, 3);
                else tel2 = theStudent._tel.Substring(3);
                if (theStudent._tel.Length > 6)  tel3 = theStudent._tel.Substring(6);                                   
            }

            String tel21 = "";
            String tel22 = "";
            String tel23 = "";

            if ((theStudent != null) && (theStudent._tel2 != null) && (theStudent._tel2.Length > 0))
            {
                if (theStudent._tel2.Length >= 3) tel21 = theStudent._tel2.Substring(0, 3);
                if (theStudent._tel2.Length >= 6) tel22 = theStudent._tel2.Substring(3, 3);
                else tel22 = theStudent._tel2.Substring(3);
                if (theStudent._tel2.Length > 6) tel23 = theStudent._tel2.Substring(6);
            }       
       
              
        %>
       1) <input type=text id="tel1" name="tel1" class="txtbox1" style="width:20px" maxlength="3" value="<%=tel1%>" /><b> -</b>
       <input type=text id="tel2" name="tel2" class="txtbox1" style="width:20px" maxlength="3" value="<%=tel2%>" /><b> -</b>
       <input type=text id="tel3" name="tel3" class="txtbox1" style="width:30px" maxlength="4" value="<%=tel3%>" /><b>,  2)</b>
       <input type=text id="tel21" name="tel21" class="txtbox1" style="width:20px" maxlength="3" value="<%=tel21%>" /><b> -</b>
       <input type=text id="tel22" name="tel22" class="txtbox1" style="width:20px" maxlength="3" value="<%=tel22%>" /><b> -</b>
       <input type=text id="tel23" name="tel23" class="txtbox1" style="width:30px" maxlength="4" value="<%=tel23%>" />
       
    </th>
  </tr>
    <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>E-Mail</th>    
    <th scope="row" class="spec" ><input type=text id="email" name="email" class="txtbox1" style="width:300px" value="<%=theStudent!=null?theStudent._email:""%>" /></th>
  </tr>
  </tr>
    <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>วันที่สมัครครั้งแรก</th>    
    <th scope="row" class="spec" >
    <input id="Text1" name="create_date" type="text" class="txtbox1" width="150px" disabled value="<%=theStudent!=null?theStudent._create_date.ToString("dd/MM/yyyy", ci):DateTime.Today.ToString("dd/MM/yyyy", ci)%>"/>
    </th>
  </tr>

  </tr>
    <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>แบบสอบถาม</th>    
    <th scope="row" class="spec" >
        ท่านรู้จักโรงเรียนกวดวิชาบัณฑิตย์ศึกษา theBTS ได้อย่างไร? (ตอบได้มากกว่า 1)<br />
        <font class="font01">
        &nbsp&nbsp<input type=checkbox id="quiz1" name="quiz1" <%=((theStudent!=null)&&(theStudent.isQuizNoSelected(1)))?"checked":"" %> />โบรชัวร์ของทางโรงเรียน<br/>
        &nbsp&nbsp<input type=checkbox id="quiz2" name="quiz2" <%=((theStudent!=null)&&(theStudent.isQuizNoSelected(2)))?"checked":"" %> />พี่/น้อง/เพื่อน/ครู บอกต่อกันมา<br/>
        &nbsp&nbsp<input type=checkbox id="quiz3" name="quiz3" <%=((theStudent!=null)&&(theStudent.isQuizNoSelected(3)))?"checked":"" %> />เดินผ่านมาเจอ<br/>
        &nbsp&nbsp<input type=checkbox id="quiz4" name="quiz4" <%=((theStudent!=null)&&(theStudent.isQuizNoSelected(4)))?"checked":"" %> />เว็บไซต์โรงเรียน (www.bts.ac.th)<br/>
        &nbsp&nbsp<input type=checkbox id="quiz5" name="quiz5" <%=((theStudent!=null)&&(theStudent.isQuizNoSelected(5)))?"checked":"" %> />โครงการติวต่าง ๆ (โปรดระบุ)
            <input type=text id="quiztext5" name="quiztext5" class="txtbox1" style="width:200px" value="<%=theStudent!=null?theStudent.getQuizNoText(5):""%>" /></br>        
        &nbsp&nbsp<input type=checkbox id="quiz6" name="quiz6" <%=((theStudent!=null)&&(theStudent.isQuizNoSelected(6)))?"checked":"" %> />อื่น ๆ (โปรดระบุ)
            <input type=text id="quiztext6" name="quiztext6" class="txtbox1" style="width:200px" value="<%=theStudent!=null?theStudent.getQuizNoText(6):""%>" />
        </font>   
    </th>
  </tr>
      
  <tr>    
    <td scope="row" align="center" colspan="2" ><p align="center">
    <input type="button" class="btn1" value="Back" onclick="history.back()"/>
    <input type="button" class="btn1" value="Save" onclick="if (checkEmptyText('firstname')) {setVal('actPage','<%=actPage%>_submit');doSubmit();}" />
    <input type="reset" class="btn1" value="Reset" />   
    </p></td>
  </tr> 
  
<% if (actPage.Equals("edit"))
   { %>  
  <tr>
    <td colspan="2" align="center">
    <div name="divhistory" id="divhistory" onclick="clickhistory()" >
    <table id="Table3" cellspacing="0" summary="..." style="width:550px" border=0>
    <caption> ประวัติการลงทะเบียน: </caption>
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
</table>
<% } %>

<% } %>
        

        
    </div>
    </form>

    
<BTS:EndBar id=EndBar1 runat=server ucname="EndBar.ascx" />

</body>
</html>
