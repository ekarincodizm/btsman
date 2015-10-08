<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CourseManage.aspx.cs" Inherits="BTS.Page.CourseManage" %>
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
<%    if ((Request["errorText"] != null) && (Request["errorText"].Length > 0))  { %>

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

    function doChangeFilter() {
        document.getElementById("actPage").value = "list";
       document.getElementById("form1").submit();
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
<caption>คอร์ส  </caption>
  <tr >
    <td>
        <table width="100%" ><tr>
        <td height="24px" width="350px">
            <!-- <a href="javascript:history.back()">Back..</a> -->
            <a href="javascript:setVal('actPage','add');doSubmit('CourseManage.aspx');"><img src="img/sys/add.gif" border=0> เพิ่มคอร์สใหม่..</a>
        </td>
        <td> 
            <b>Filter: </b>
            <select id="filter" name="filter" onchange="javascript:doChangeFilter()">
                <option value="only_open" <%=filterSearch!=null && filterSearch.Equals("only_open")?"selected":"" %>>แสดงเฉพาะคอร์สที่เปิดอยู่</option>
                <option value="only_close" <%=filterSearch!=null && filterSearch.Equals("only_close")?"selected":"" %>>แสดงเฉพาะคอร์สที่ปิดไปแล้ว</option>
                <option value="all" <%=filterSearch!=null && filterSearch.Equals("all")?"selected":"" %>>แสดงทั้งหมด</option>
            </select>
        
        </td>
        <td align=right>
            <input type=text id="qsearch" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch()" value="<%=quickSearch%>"/>  
        </td>
        </tr></table>
    </td>
  </tr>
  <tr>
    <td>
    <table id="Table2" cellspacing="0" summary="..." style="width:800px" border=0>  
      <tr>
        <th scope="col" align=center width="50px" NOWRAP>รหัส</th>
	    <th scope="col" align=center width="400px">คอร์ส</th>
        <th scope="col" align=center width="50px">ราคา</th>
	    <th scope="col" align=center width="200px">วัน-เวลา</th>
	    <th scope="col" align=center width="150px">อาจารย์ผู้สอน</th>
	    <th scope="col" width=70px" align=center abbr="Action">Action</th>	
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

<script type="text/javascript" >
    function jumpToEditRegistration(regisId) {
        location.href = 'RegistrationManage.aspx?actPage=edit&targetID=' + regisId;

    }


</script>


<table border=0 width="800px">
<tr>
<td align=left width="400px"><a href="javascript:history.back()"><< Back</a></td>
<td align=right width="200px"><a href="javascript:setVal('actPage','edit');setVal('targetID','<%=theCourse._courseID %>');doSubmit()"><img src="img/sys/edit.gif" border=0 alt="Edit">แก้ไขข้อมูล</a>&nbsp</td>
<td align=right width="200px"><a href="javascript:setVal('actPage','init_print');setVal('targetID','<%=theCourse._courseID %>');doSubmit()"><img src="img/sys/print.gif" border=0 alt="Edit">พิมพ์รายชื่อนักเรียน</a>&nbsp</td>

<td align=right width="200px"><a href="javascript:setVal('actPage','save_as_excel');setVal('targetID','<%=theCourse._courseID %>');doSubmit()"><img src="img/sys/excel.gif" border=0 alt="Edit">ดาวน์โหลดเป็น Excel</a>&nbsp</td>

</tr>
</table>

<%= outBuf.ToString() %>

<% } else if ((actPage.Equals("add")) || (actPage.Equals("edit"))) { %>


<script type="text/javascript" >
    function refreshStartEndDate() {
        val = document.getElementById("course_type").value;
        if (val == "คอร์ส DVD") {
            document.getElementById("startend_row").style.display = 'none';
        } else {
            document.getElementById("startend_row").style.display = '';
        }
    }

</script>

<!-- Add/Edit -->
<table id="Table1" cellspacing="0" >
<caption>คอร์ส: <%=actPage%> </caption>

  <tr>
    <th scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ชื่อคอร์ส</th>    
    <th scope="row" class="spec" ><input type=text id="course_name" name="course_name" class="txtbox1" style="width:300px" value="<%=theCourse!=null?theCourse._courseName:""%>" /></th>
  </tr>
  <tr>
    <th scope="col" class="formTitle" width="250px" valign="top" NOWRAP>รหัสคอร์ส</th>    
    <th scope="row" class="spec" ><input type=text id="bts_course_id" name="bts_course_id" class="txtbox1" style="width:100px" value="<%=theCourse!=null?theCourse._btsCourseID:""%>" /></th>
  </tr>
  
  <tr>
    <th scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ชื่อย่อ</th>    
    <th scope="row" class="spec" ><input type=text id="short_name" name="short_name" class="txtbox1" maxlength="15" style="width:100px" value="<%=theCourse!=null?theCourse._shortName:""%>" /></th>
  </tr>

  <tr>
    <th scope="col" class="formTitle" width="250px" valign="top" NOWRAP>Type</th>    
    <th scope="row" class="spec" >
        <select id="course_type" name="course_type" class="txtbox1" style="width:160px" onchange="refreshStartEndDate()">
            <option value="คอร์สสด" <%=theCourse._courseType=="คอร์สสด" ? "selected" : "" %> >คอร์สสด</option>
            <option value="คอร์ส DVD" <%=theCourse._courseType=="คอร์ส DVD" ? "selected" : "" %> >คอร์ส DVD</option>
        </select>
    </th>
  </tr>
   
  <tr>
    <th scope="col" class="formTitle" width="250px" valign="top" NOWRAP>รายละเอียด</th>    
    <th scope="row" class="spec"><textarea class="txtarea1" id="course_desc" name="course_desc" style="width:300px;height:70px"><%=theCourse != null ? theCourse._courseDesc : ""%></textarea></th>    
  </tr>

  <tr>
    <th scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ประเภท</th>    
    <th scope="row" class="spec" >
    <select id="category" name="category" class="txtbox1" style="width:160px" >
<% 
    for (int i = 0; i < Config.COURSE_CATE.Length; i++)
    {
        string selected = ((theCourse != null) && (theCourse._category == Config.COURSE_CATE[i])) ? "selected" : "";
%>       
    <option value="<%=Config.COURSE_CATE[i] %>" <%= selected %> ><%=Config.COURSE_CATE[i]%></option>
<% } %>
    </select>
    </th>
  </tr>

  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ห้องเรียน</th>    
    <th scope="row" class="spec" >
    <select id="room_id" name="room_id" class="txtbox1" style="width:160px" >
<% 
    for (int i = 0; i < roomList.Length; i++)
    {
        string selected = ((theCourse != null) && (theCourse._roomID == roomList[i]._roomID)) ? "selected" : "";
%>       
    <option value="<%=roomList[i]._roomID %>" <%= selected %> ><%=roomList[i]._branchName + " - " + roomList[i]._name%></option>
<% } %>
    </select>
    </th>
  </tr>

  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>อาจารย์ผู้สอน</th>    
    <th scope="row" class="spec" >
    <select id="teacher_id" name="teacher_id" class="txtbox1" style="width:160px" >
<% 
    for (int i = 0; i < teacherList.Length; i++)
    {
        string selected = ((theCourse != null) && (theCourse._teacherID == teacherList[i]._teacherID)) ? "selected" : "";
%>       
    <option value="<%=teacherList[i]._teacherID %>" <%= selected %> ><%=teacherList[i]._firstname + " " + teacherList[i]._surname%></option>
<% } %>
    </select>
    </th>
  </tr>
  
    <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>กลุ่มอาจารย์</th>    
    <th scope="row" class="spec" >
    <select id="paid_group_id" name="paid_group_id" class="txtbox1" style="width:160px" >
<% 
    for (int i = 0; i < paidGroupList.Length; i++)
    {
        string selected = ((theCourse != null) && (theCourse._paidGroupID == paidGroupList[i]._paidGroupID)) ? "selected" : "";
%>       
    <option value="<%=paidGroupList[i]._paidGroupID %>" <%= selected %> ><%= PaidGroup.GetPaidGroupID(paidGroupList[i]._paidGroupID)+" "+ paidGroupList[i]._name%></option>
<% } %>
    </select>
    </th>
  </tr>

  
  <tr id="startend_row">
    <link rel="stylesheet" type="text/css" href="js/epoch_v106_en/epoch_styles.css" />
    <script type="text/javascript" src="js/epoch_v106_en/epoch_classes.js"></script>
    <script type="text/javascript">
        /*You can also place this code in a separate file and link to it like epoch_classes.js*/
        var dp_cal, dp_cal2;
        window.onload = function() {
            // bas_cal = new Epoch('epoch_basic', 'flat', document.getElementById('basic_container'));
            dp_cal = new Epoch('epoch_popup', 'popup', document.getElementById('startdate'));
            dp_cal2 = new Epoch('epoch_popup', 'popup', document.getElementById('enddate'));
            //  ms_cal = new Epoch('epoch_multi', 'flat', document.getElementById('multi_container'), true);
            refreshStartEndDate();
        };
    </script>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ช่วงเวลาการสอน</th>    
    <th scope="row" class="spec" >  
        <table border=0>
        <tr>
        <td align=right>เริ่มวันที่ &nbsp</td>
        <td align=left>
            <input id="startdate" name="startdate" type="text" class="txtbox1" width="150px" readonly value="<%=theCourse!=null?theCourse._startdate.ToString("dd/MM/yyyy", ci):DateTime.Today.ToString("dd/MM/yyyy", ci)%>"/>    
        </td>            
        <td align=right>ถึงวันที่ &nbsp</td>
        <td align=left>        
            <input id="enddate" name="enddate" type="text" class="txtbox1" width="150px" readonly value="<%=theCourse!=null?theCourse._enddate.ToString("dd/MM/yyyy", ci):DateTime.Today.ToString("dd/MM/yyyy", ci)%>"/>          
        </td>
        </tr>
        <td align=right>เวลา &nbsp</td>
        <td align=left>        
            <input id="opentime" name="opentime" type="text" class="txtbox1" style="width:100px" 
                    value="<%=theCourse!=null?theCourse._opentime:"08:00-10:00"%>"/>                
        </td>
        <td align=right>วันที่เรียน &nbsp</td>
        <td align=left> 
            <input id="day_of_week" name="day_of_week" type="text" class="txtbox1" style="width:100px" 
                value="<%=theCourse!=null?theCourse._dayOfWeek:"จ"%>"/>   
        </td>
        </tr>
        </table> 
    </th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>ค่าสมัครเรียน</th>    
    <th scope="row" class="spec" ><input type=text id="cost" name="cost" class="txtbox1" style="width:50px" value="<%=theCourse!=null?theCourse._cost.ToString():"1000"%>" /> บาท</th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>จำนวนที่เปิดรับทั้งหมด</th>    
    <th scope="row" class="spec" ><input type=text id="seat_limit" name="seat_limit" class="txtbox1" style="width:50px" value="<%=theCourse!=null?theCourse._seatLimit.ToString():"50"%>" /> ที่นั่ง</th>
  </tr>
 
  <tr>
    <td scope="col" class="formTitle" width="250px" valign="top" NOWRAP>รูปภาพ</th>    
    <th scope="row" class="spec" >
        <img width="75px" height="100px"  src="<%=Config.URL_PIC_COURSE + "/" + (theCourse!=null?theCourse._img:"noimg.jpg") %>" /> <br/><br />        
        <input type=file id="portrait" name="portrait1" class="txtbox1" runat=server style="width:400px" />
        <input type=hidden id="img_old" name="img_old" value="<%=theCourse!=null?theCourse._img:"" %>" />
        </th>
  </tr>
    
  <tr>    
    <td scope="row" align="center" colspan="2" ><p align="center">
    <input type="button" class="btn1" value="Back" onclick="history.back()"/>
    <input type="button" class="btn1" value="Save" onclick="if (checkEmptyText('course_name')) {setVal('actPage','<%=actPage%>_submit');doSubmit();}" />
    <input type="reset" class="btn1" value="Reset" />   
    </p></td>
  </tr>  
</table>


<!-------------------------------------------------------------------------------------------------------------->                
<% } else if (actPage.Equals("init_print")) { %>
<script type="text/javascript" >
    function doPrint() {
        window.open("Print.aspx");
        location.href = "CourseManage.aspx?actPage=view&targetID=<%=targetID%>";
    }
    doPrint();
</script>
<!-------------------------------------------------------------------------------------------------------------->                
<% } else if (actPage.Equals("save_as_excel")) { %>
<script type="text/javascript" >
    function doSaveExcel() {
        //window.open("SaveAsExcel.aspx");
        //location.href = "CourseManage.aspx?actPage=view&targetID=<%=targetID%>";
        location.href="SaveAsExcel.aspx";
    }
    doSaveExcel();
   // location.href = "SaveAsExcel.aspx";
</script>
<% } %>
        

        
    </div>
    </form>

    
<BTS:EndBar id=EndBar1 runat=server ucname="EndBar.ascx" />

</body>
</html>
