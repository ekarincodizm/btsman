<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCourse.aspx.cs" Inherits="BTS.Page.PromotionManage" %>
<%@ Import Namespace="BTS" %>
<%@ Import Namespace="BTS.Constant" %>
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

<!-- Global js -->
<script type="text/javascript" >
    var layerName = 'divdetail';
    var offsetY = -10;
    var offsetX = 20;
    var x;
    var y;

    function getMousePos(e) {
        /*
        x=(document.all)?event.x:e.pageX;
        y=(document.all)?event.y:e.pageY;
        x=mouseX(e);
        y=mouseY(e);
        alert(x+","+y);
        */
        if (IE) { // grab the x-y pos.s if browser is IE
            x = event.clientX + document.documentElement.scrollLeft
            y = event.clientY + document.documentElement.scrollTop
        } else {  // grab the x-y pos.s if browser is NS
            x = e.pageX
            y = e.pageY
        }
        // catch possible negative values in NS4
        if (x < 0) { x = 0 }
        if (y < 0) { y = 0 }

    }

    // Detect if the browser is IE or not.
    // If it is not IE, we assume that the browser is NS.
    var IE = document.all ? true : false
    // If NS -- that is, !IE -- then set up for mouse capture
    if (!IE) document.captureEvents(Event.MOUSEMOVE)
    // Set-up to use getMouseXY function onMouseMove
    document.onmousemove = getMousePos;
    // Main function to retrieve mouse x-y pos.s



    function showDivAt(layerName) {
        //alert(x + "," +y);        
        eval('document.getElementById(\'' + layerName + '\').style.top=' + (y + offsetY));
        eval('document.getElementById(\'' + layerName + '\').style.left=' + (x + offsetX));
        eval('document.getElementById(\'' + layerName + '\').style.visibility=\'visible\'');
        eval('document.getElementById(\'' + layerName + '\').style.display=\'block\'');
        eval('document.getElementById(\'' + layerName + '\').style.position=\'absolute\'');
    }

    function showDiv(layerName) {
        //ajaxPost("AjaxService.aspx", "svc=qchklist&sdid=" + sdid, layerName);

        //eval('document.getElementById(\''+layerName+'\').style.width=\'240px\'');
        //eval('document.getElementById(\''+layerName+'\').style.height=\'120px\'');
        eval('document.getElementById(\'' + layerName + '\').style.visibility=\'visible\'');
        eval('document.getElementById(\'' + layerName + '\').style.display=\'block\'');
        eval('document.getElementById(\'' + layerName + '\').style.position=\'relative\'');
        // eval('document.getElementById(\'' + layerName + '\').style.position=\'absolute\'');
        //eval('document.getElementById(\''+layerName+'\').style.padding=\''+layerPaddingT+'px '+layerPaddingR+'px '+layerPaddingB+'px '+layerPaddingL+'px\'');

        //  document.getElementById(layerName).innerHTML = "<img class=\"img1\" src=\""+divtxt+"\" >";
    }

    function hideDiv(layerName) {
        document.getElementById(layerName).style.display = "none";
        //eval('document.getElementById(\'' + layerName + '\').style.position=\'absolute\'');        
        // document.getElementById(layerName).innerHTML = "<img src=\"img/sys/loading.gif\"/>";
    }

    function showTab(tabHead, tabPane) {
        document.getElementById(tabHead).className = "tabhead1_selected";
        showDiv(tabPane);
    }

    function hideTab(tabHead, tabPane) {
        document.getElementById(tabHead).className = "tabhead1";
        hideDiv(tabPane);
    }


    function advanceTo(toPage) {
        setVal('actPage', toPage);
        doSubmit();
    }

    function listSelectedCourses(enableAction) {
        document.getElementById("c_selected").innerHTML = "<img src=\"img/sys/loading.gif\" />";
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_LIST_SELECTED_COURSE%>&enable_action="+enableAction, "c_selected", true);
    }

    function addCourse(course_id) {
        document.getElementById("c_selected").innerHTML = "<img src=\"img/sys/loading.gif\" />";
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_ADD_SELECTED_COURSE%>&course_id=" + course_id, "c_selected", true);
    }
    function removeCourse(course_id) {
        //document.getElementById("c_selected").innerHTML = "<img src=\"img/sys/loading.gif\" />";    
        hideDiv("divdetail");
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_REM_SELECTED_COURSE%>&course_id=" + course_id, "c_selected", true);
    }

    function queryCourseDetail(course_id) {
        document.getElementById("divdetail").innerHTML = "<img src=\"img/sys/loading.gif\" />";
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_Q_COURSE_DETAIL%>&course_id=" + course_id, "divdetail", true);
    }

    function listSelectedStudents() {
        document.getElementById("s_selected").innerHTML = "<img src=\"img/sys/loading.gif\" />";
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_LIST_SELECTED_STUDENT%>", "s_selected", true);
    }
    function addStudent(student_id) {
        document.getElementById("s_selected").innerHTML = "<img src=\"img/sys/loading.gif\" />";
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_ADD_SELECTED_STUDENT%>&student_id=" + student_id, "s_selected", true);
    }
    function removeStudent(student_id) {
        //document.getElementById("c_selected").innerHTML = "<img src=\"img/sys/loading.gif\" />";    
        hideDiv("divdetail");
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_REM_SELECTED_STUDENT%>&student_id=" + student_id, "s_selected", true);
    }

    function queryStudentDetail(student_id) {
        document.getElementById("divdetail").innerHTML = "<img src=\"img/sys/loading.gif\" />";
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_Q_STUDENT_DETAIL%>&student_id=" + student_id, "divdetail", true);
    }

    function GetRegisDetail_Student() {
        document.getElementById("c_selected").innerHTML = "<img src=\"img/sys/loading.gif\" />";
        document.getElementById("s_selected").innerHTML = "<img src=\"img/sys/loading.gif\" />";
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_LIST_SELECTED_COURSE%>&enable_action=false", "c_selected", false);
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_LIST_SELECTED_STUDENT%>&enable_action=false", "s_selected", false);
    }

    function modifyPCost(pid, def) {
        var dcost = prompt("ลดราคาเหลือ", def);
        if (dcost == null) { dcost = def; }

        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_DISCOUNT_PROMOTION%>&promotion_id=" + pid + "&cost=" + dcost, "c_selected");
    }

    function modifyCCost(cid, def) {
        var dcost = prompt("ลดราคาเหลือ", def);
        if (dcost == null) { dcost = def; }

        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_DISCOUNT_COURSE%>&course_id=" + cid + "&cost=" + dcost, "c_selected");
    }    
    
</script>    

<!-- Error box -->    
<%  if ((Request["errorText"] != null) && (Request["errorText"].Length > 0))   { %>
<table id="ErrTable" cellspacing="0" >
  <tr>
    <th scope="row" class="spec" ><p><font class="font3"><%=Request["errorText"]%></font></p></th>
  </tr>
</table>
<% } %>


<div name="divdetail" id="divdetail" style="border-color:Black;background-color:White; filter:alpha(opacity=75); -moz-opacity: 0.75; opacity: 0.75;display:none;position:relative"></div>    
<% if ((actPage == null) || (actPage.Trim().Length==0) || (actPage.Equals("new")) || (actPage.Equals("select_course")))  {  %>
<!-- Select Course -->
<script type="text/javascript" >
    function doSearch1() {
        //if (event.keyCode == 13) {
            //document.getElementById("actPage").value = "list";
            search = document.getElementById("qsearch1").value;
            //if (event.keyCode != 13) { search = search +  String.fromCharCode(event.keyCode); }
            if (search.length >3 )
            {                 
                document.getElementById("c_result").innerHTML = "<img src=\"<%=Config.URL_PIC_SYS + "/loading.gif" %>\" />";
                ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_Q_COURSES%>&search=" + search, "c_result");
            }
        //}
    }
</script>

<table id="mytable" cellspacing="0" summary="..." style="width:800px" valign="top" border=0>
<caption>
  <table width="800px">
    <tr valign="middle">
        <td width="200px">&nbsp</td>
        <td width="400px" align=center> ลงทะเบียนเรียน :: เลือกคอร์ส </td>
        <td width="200px" align=right><a href="javascript:advanceTo('select_student')"><h3><b>Next : เลือกนักเรียน>></b></h3></a></td>
    </tr>
  </table>
</caption>  
  <tr>
    <td>
   
        <table width="200px" cellpadding=0 cellspacing=0 >
        <tr>
            <td height="24px" width="50px" align=center class="tabhead1_selected" NOWRAP valign="middle" >คอร์สที่เลือก 
                <a href="javascript:listSelectedCourses(true)"><img src="<%=Config.URL_PIC_SYS + "/refresh.gif" %>" border=0 width="18px" height="18px" /></a></td>            
            <td width="700px" align=right>&nbsp</td>
        </tr>
        <tr>
            <td align=left colspan=3>
                <div id="c_selected" name="c_selected" class="tabpane1_bottom" style="width:800px;" >
                    <img src="<%=Config.URL_PIC_SYS + "/loading.gif" %>" />
                </div>
                <script type="text/javascript" >
                    // preload selected courses
                    listSelectedCourses(true);
                </script>                
            </td>
        </tr>
        </table>    
    </td>
  </tr>
  </tr><td>&nbsp</td></tr>
  <tr >
    <td>
        <table width="200px" cellpadding=0 cellspacing=0>
        <tr>
            <td height="24px" width="50px" align=center class="tabhead1_selected" NOWRAP id="tabsearch1" name="tabsearch1"
                onclick="showTab('tabsearch1','tabpane1');hideTab('tabsearch2','tabpane2');" >ค้นหา</td>
            <td height="24px" width="100px" align=center class="tabhead1" NOWRAP id="tabsearch2" name="tabsearch2"
                onclick="showTab('tabsearch2','tabpane2');hideTab('tabsearch1','tabpane1');">ค้นหาแบบละเอียด</td>
            <td width="400px">&nbsp&nbsp&nbsp</td>
        </tr>
        <tr>
            <td align=left colspan=3>
                <div id=tabpane2 name=tabpane2 class="tabpane1" style="width:800px;display:none">
                    <table border=0>
                    <tr>
                        <td>
                            <input type=text id="Text2" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch2()" />  
                        </td>
                        <td>
                            <input type=text id="Text3" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch2()" />  
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type=text id="Text4" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch2()" />  
                        </td>
                        <td>
                            <input type=text id="Text5" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch2()" />  
                        </td>
                    </tr>
                    </table>        
                </div>
                <div id=tabpane1 name=tabpane1 class="tabpane1" style="width:800px;">
                    <table border=0>
                    <tr>
                        <td>              
                            <input type=text id="qsearch1" name="qsearch1" class="txtbox2" style="width:150px" onkeyup="" />  
                            <input type="button" class="btn1" value="ค้นหา" onclick="if (checkEmptyText('qsearch1')) {setVal('actPage','_submit');doSearch1();}" />
                        </td>
                    </tr>
                    </table>
                </div>
                <div id="c_result" name=c_result class="tabpane1_bottom" style="width:800px;" >                    
                </div>

            </td>

        </tr>
        </table>
    </td>
  </tr>
<!--  TODO: Promotion suggestion  -->
  
</table>               

<% } else if (actPage.Equals("select_student")) { %>


<!-- Select Course -->
<script type="text/javascript" >
    function doSearch1() {
        //if (event.keyCode == 13) {
            //document.getElementById("actPage").value = "list";
            search = document.getElementById("qsearch1").value;
            //if (event.keyCode != 13) { search = search +  String.fromCharCode(event.keyCode); }
            if (search.length > 0)
            {                 
                document.getElementById("s_result").innerHTML = "<img src=\"<%=Config.URL_PIC_SYS + "/loading.gif" %>\" />";
                ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_Q_STUDENTS%>&search=" + search, "s_result");
            }
        //}
    }

        
    
</script>

<table id="Table2" cellspacing="0" summary="..." style="width:800px" valign="top" border=0>
<caption>
  <table width="800px">
    <tr valign="middle">
        <td width="200px" align=left><a href="javascript:advanceTo('select_course')"><h3><b><< Back : เลือกคอร์ส</b></h3></a></td>
        <td width="400px" align=center> ลงทะเบียนเรียน :: เลือกนักเรียน</td>
        <td width="200px" align=right><a href="javascript:advanceTo('confirm_registration')"><h3><b>Next : ยืนยันข้อมูล>></b></h3></a></td>
    </tr>
  </table>
</caption>  
  <tr>
    <td>
   
        <table width="200px" cellpadding=0 cellspacing=0 >
        <tr>
            <td height="24px" width="150px" align=center class="tabhead1_selected" NOWRAP valign="middle" >คอร์สที่เลือก 
                <a href="javascript:listSelectedCourses()"><img src="<%=Config.URL_PIC_SYS + "/refresh.gif" %>" border=0 width="18px" height="18px" /></a></td>            
            <td width="700px" align=right>&nbsp</td>
        </tr>
        <tr>
            <td align=left colspan=3>
                <div id="c_selected" name="c_selected" class="tabpane1_bottom" style="width:800px;" >
                    <img src="<%=Config.URL_PIC_SYS + "/loading.gif" %>" />
                </div>
             
            </td>
        </tr>
        </table>    
    </td>
  </tr>
  </tr><td>&nbsp</td></tr>
  <tr>
    <td>
   
        <table width="200px" cellpadding=0 cellspacing=0 >
        <tr>
            <td height="24px" width="150px" align=center class="tabhead1_selected" NOWRAP valign="middle" >นักเรียนที่เลือก 
                <a href="javascript:listSelectedStudents()"><img src="<%=Config.URL_PIC_SYS + "/refresh.gif" %>" border=0 width="18px" height="18px" /></a></td>            
            <td width="700px" align=right>&nbsp</td>
        </tr>
        <tr>
            <td align=left colspan=3>
                <div id="s_selected" name="s_selected" class="tabpane1_bottom" style="width:800px;" >
                    <img src="<%=Config.URL_PIC_SYS + "/loading.gif" %>" />
                </div>
                <script type="text/javascript" >
                    // preload selected courses
                    GetRegisDetail_Student();
                </script>                
            </td>
        </tr>
        </table>    
    </td>
  </tr>
  </tr><td>&nbsp</td></tr>  
  <tr >
    <td>
        <table width="700px" cellpadding=0 cellspacing=0>
        <tr>
            <td height="24px" width="50px" align=center class="tabhead1_selected" NOWRAP id="tabsearch1" name="tabsearch1"
                onclick="showTab('tabsearch1','tabpane1');hideTab('tabsearch2','tabpane2');hideTab('tabnewstudent','tabpane3');" >ค้นหา</td>
            <td height="24px" width="100px" align=center class="tabhead1" NOWRAP id="tabsearch2" name="tabsearch2"
                onclick="showTab('tabsearch2','tabpane2');hideTab('tabsearch1','tabpane1');hideTab('tabnewstudent','tabpane3');">ค้นหาแบบละเอียด</td>
            <td height="24px" width="100px" align=center class="tabhead1" NOWRAP id="tabnewstudent" name="tabnewstudent"
                onclick="showTab('tabnewstudent','tabpane3');hideTab('tabsearch2','tabpane2');hideTab('tabsearch1','tabpane1');dp_cal.updatePosition();">เพื่มนักเรียนใหม่</td>                
            <td width="300px">&nbsp&nbsp&nbsp</td>
        </tr>
        <tr>
            <td align=left colspan=4>
               <div id=tabpane1 name=tabpane1 class="tabpane1" style="width:800px;">
                    <table border=0>
                    <tr>
                        <td>              
                            <input type=text id="qsearch1" name="qsearch1" class="txtbox2" style="width:150px" onkeyup="" />  
                            <input type="button" class="btn1" value="ค้นหา" onclick="if (checkEmptyText('qsearch1')) {setVal('actPage','_submit');doSearch1();}" />
                        </td>
                    </tr>
                    </table>
                </div>            
                <div id=tabpane2 name=tabpane2 class="tabpane1" style="width:800px;display:none">
                    <table border=0>?
                    <tr>
                        <td>
                            <input type=text id="Text1" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch2()" />  
                        </td>
                        <td>
                            <input type=text id="Text6" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch2()" />  
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type=text id="Text7" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch2()" />  
                        </td>
                        <td>
                            <input type=text id="Text8" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch2()" />  
                        </td>
                    </tr>
                    </table>        
                </div>
                 <div id=tabpane3 name=tabpane3 class="tabpane1" style="width:800px;display:none">
                <!-- Add new student  -->
                <!-- Add new student  -->
                
                    <table id="Table3" cellspacing="0" >
                      <tr width="20px"><td colspan=2 >&nbsp</td></tr>
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>ชื่อ</b>&nbsp&nbsp</td>    
                        <th scope="row" class="spec_top" ><input type=text id="firstname" name="firstname" class="txtbox1" style="width:150px" value="" /></th>
                      </tr>
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>นามสกุล</b>&nbsp&nbsp</td>    
                        <th scope="row" class="spec" ><input type=text id="surname" name="surname" class="txtbox1" style="width:150px" value="" /></th>
                      </tr>
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>ชื่อเล่น</b>&nbsp&nbsp</td>    
                        <th scope="row" class="spec" ><input type=text id="nickname" name="nickname" class="txtbox1" style="width:150px" value="" /></th>
                      </tr>
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>รหัสบัตรประชาชน</b>&nbsp&nbsp</td>    
                        <th scope="row" class="spec" ><input type=text id="citizen_id" name="citizen_id" class="txtbox1" style="width:150px" value="" /></th>
                      </tr>    
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>เพศ</b>&nbsp&nbsp</td>    
                        <th scope="row" class="spec" ><font class="font01">
                            <input type="radio" name="sex" value="Male" checked  >ชาย &nbsp
                            <input type="radio" name="sex" value="Female" >หญิง
                            </font>
                        </th>
                      </tr>
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>รูปภาพ</b>&nbsp&nbsp</td>    
                        <th scope="row" class="spec" >
                            <img width="75px" height="100px"  src="<%=Config.URL_PIC_STUDENT + "/noimg.jpg"%>" /> <br/><br />        
                            <input type=file id="portrait" name="portrait1" class="txtbox1" runat=server style="width:400px" />
                            </th>
                      </tr>  
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>วันเกิด</b>&nbsp&nbsp</td>    
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
                          <input id="birthday" name="birthday" type="text" class="txtbox1" width="150px" readonly value="<%=DateTime.Today.ToString("dd/MM/yyyy", ci)%>" 
                          onclick="dp_cal.updatePosition()"/>

                        </th>
                      </tr>
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>โรงเรียน</b>&nbsp&nbsp</td>    
                        <th scope="row" class="spec" ><input type=text id="school" name="school" class="txtbox1" style="width:300px" value="" /></th>
                      </tr>
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>ระดับชั้น</b></th>    
                        <th scope="row" class="spec" >
                        <select id="level" name="level" class="txtbox1" style="width:120px" >
                    <% 
                        for (int i = 1; i <= 16; i++)
                        {
                    %>       
                        <option value="<%=i%>"><%= StringUtil.ConvertEducateLevel(i) %></option>
                    <% } %>
                        <option value="0"><%= StringUtil.ConvertEducateLevel(0) %></option>
                        </select>
                        </th>
                      </tr>
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>ที่อยู่</b>&nbsp&nbsp</td>    
                        <th scope="row" class="spec"><textarea class="txtarea1" id="addr" name="addr" style="width:300px;height:70px"></textarea></th>    
                      </tr>
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>โทรศัพท์มือถือ</b>&nbsp&nbsp</td>    
                        <th scope="row" class="spec" >
                            1) <input type=text id="tel1" name="tel1" class="txtbox1" style="width:20px" maxlength="3" value="" /><b> -</b>
                            <input type=text id="tel2" name="tel2" class="txtbox1" style="width:20px" maxlength="3"  value="" /><b> -</b>
                            <input type=text id="tel3" name="tel3" class="txtbox1" style="width:30px" maxlength="4" value="" /><b>, 2)</b>
                            <input type=text id="tel21" name="tel21" class="txtbox1" style="width:20px" maxlength="3" value="" /><b> -</b>
                            <input type=text id="tel22" name="tel22" class="txtbox1" style="width:20px" maxlength="3"  value="" /><b> -</b>
                            <input type=text id="tel23" name="tel23" class="txtbox1" style="width:30px" maxlength="4" value="" />                           
                            </th>
                      </tr>
                      <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>E-Mail</b></th>    
                        <th scope="row" class="spec" ><input type=text id="email" name="email" class="txtbox1" style="width:300px" value="" /></th>
                      </tr>

                     <tr>
                        <td scope="col" width="100px" valign="top" align=right NOWRAP><b>แบบสอบถาม</b></th>      
                        <th scope="row" class="spec" >
                            ท่านรู้จักโรงเรียนกวดวิชาบัณฑิตย์ศึกษา theBTS ได้อย่างไร? (ตอบได้มากกว่า 1)<br />
                            <font class="font01">
                            &nbsp&nbsp<input type=checkbox id="quiz1" name="quiz1" />โบรชัวร์ของทางโรงเรียน<br/>
                            &nbsp&nbsp<input type=checkbox id="quiz2" name="quiz2" />พี่/น้อง/เพื่อน/ครู บอกต่อกันมา<br/>
                            &nbsp&nbsp<input type=checkbox id="quiz3" name="quiz3" />เดินผ่านมาเจอ<br/>
                            &nbsp&nbsp<input type=checkbox id="quiz4" name="quiz4" />เว็บไซต์โรงเรียน (www.bts.ac.th)<br/>
                            &nbsp&nbsp<input type=checkbox id="quiz5" name="quiz5" />โครงการติวต่าง ๆ (โปรดระบุ)
                                <input type=text id="quiztext5" name="quiztext5" class="txtbox1" style="width:200px" value="" /></br>        
                            &nbsp&nbsp<input type=checkbox id="quiz6" name="quiz6" />อื่น ๆ (โปรดระบุ)
                                <input type=text id="quiztext6" name="quiztext6" class="txtbox1" style="width:200px" value="" />
                            </font>   
                        </th>
                      </tr>
                      
                      <tr>    
                        <td scope="row" align="center" colspan="2" ><p align="center">
                        <input type="button" class="btn1" value="Save" onclick="if (checkEmptyText('firstname')) {setVal('actPage','add_new_student_submit');doSubmit();}" />
                        <input type="reset" class="btn1" value="Reset" />   
                        </p></td>
                      </tr>  
                    </table>
                                    
                                    
                
                
                </div>
                <div id="s_result" name=s_result class="tabpane1_bottom" style="width:800px;" >                    
                </div>

            </td>

        </tr>
        </table>
    </td>
  </tr>

  
</table>               

<% } else if (actPage.Equals("confirm_registration")) { %>


<!-- Select Course -->
<script type="text/javascript" >
    function doSearch1() {
        //if (event.keyCode == 13) {
            //document.getElementById("actPage").value = "list";
            search = document.getElementById("qsearch1").value;
            //if (event.keyCode != 13) { search = search +  String.fromCharCode(event.keyCode); }
            if (search.length > 0)
            {                 
                document.getElementById("s_result").innerHTML = "<img src=\"<%=Config.URL_PIC_SYS + "/loading.gif" %>\" />";
                ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_Q_STUDENTS%>&search=" + search, "s_result");
            }
        //}
    }
 
    
</script>

<table id="Table1" cellspacing="0" summary="..." style="width:800px" valign="top" border=0>
<caption>
  <table width="800px">
    <tr valign="middle">
        <td width="200px" align=left><a href="javascript:advanceTo('select_student')"><h3><b><< Back : เลือกนักเรียน</b></h3></a></td>
        <td width="400px" align=center> ลงทะเบียนเรียน :: ยืนยันการลงทะเบียน</td>
        <td width="200px" align=right>&nbsp</td>
    </tr>
  </table>
</caption>  
  <tr>
    <td>
   
        <table width="200px" cellpadding=0 cellspacing=0 >
        <tr>
            <td height="24px" width="150px" align=center class="tabhead1_selected" NOWRAP valign="middle" >คอร์สที่เลือก 
                <a href="javascript:listSelectedCourses()"><img src="<%=Config.URL_PIC_SYS + "/refresh.gif" %>" border=0 width="18px" height="18px" /></a></td>            
            <td width="700px" align=right>&nbsp</td>
        </tr>
        <tr>
            <td align=left colspan=3>
                <div id="c_selected" name="c_selected" class="tabpane1_bottom" style="width:800px;" >
                    <img src="<%=Config.URL_PIC_SYS + "/loading.gif" %>" />
                </div>
         
            </td>
        </tr>
        </table>    
    </td>
  </tr>
  </tr><td>&nbsp</td></tr>
  <tr>
    <td>
   
        <table width="200px" cellpadding=0 cellspacing=0 >
        <tr>
            <td height="24px" width="150px" align=center class="tabhead1_selected" NOWRAP valign="middle" >นักเรียนที่เลือก 
                <a href="javascript:listSelectedStudents()"><img src="<%=Config.URL_PIC_SYS + "/refresh.gif" %>" border=0 width="18px" height="18px" /></a></td>            
            <td width="700px" align=right>&nbsp</td>
        </tr>
        <tr>
            <td align=left colspan=3>
                <div id="s_selected" name="s_selected" class="tabpane1_bottom" style="width:800px;" >
                    <img src="<%=Config.URL_PIC_SYS + "/loading.gif" %>" />
                </div>
                <script type="text/javascript" >
                    // preload selected courses
                    GetRegisDetail_Student();
                </script>                
        
            </td>
        </tr>
        </table>    
    </td>
  </tr>
  </tr><td>&nbsp</td></tr>  
  <tr >
    <td>
                    <table border=0>
                    <tr><td>ชำระเงินผ่านทาง</td>
                    <td colspan="2">
                        <select name="paid_method" id="paid_method" class="txtbox1">
                            <option value=0><%= Registration.GetPaidMethodText("0")%></option>
                            <option value=1><%= Registration.GetPaidMethodText("1")%></option>
                            <option value=2><%= Registration.GetPaidMethodText("2")%></option>
                            <option value=3><%= Registration.GetPaidMethodText("3")%></option>
                            <option value=4><%= Registration.GetPaidMethodText("4")%></option>
                        </select>
                    </td></tr>  
                    <tr>
                        <td align=left><b>คอร์ส</b></td>
                        <td align=center><b>หมายเลขที่นั่ง</b></td>
                        <td align=center><b>หมายเหตุ</b></td>
                    </tr>                  
                        <%= outBuf.ToString() %>                    

                </table>   
        <table width="100%">
            <tr align=center width="100%"> <td> โปรดตรวจสอบข้อมูลให้ถูกต้องก่อนยืนยันการลงทะเบียน </td></tr>
            <tr align=center > 
            <td> 
                <input type="button" class="btn1" value="ยืนยัน" onclick="setVal('actPage','submit_registration');doSubmit();"/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                <input type="button" class="btn1" value="ยกเลิก" onclick="setVal('actPage','cancel_registration');doSubmit();"/>
            </td></tr>
        </table>
    </td>
  </tr>

  
</table>               

<% } else if (actPage.Equals("registration_complete"))   { %>

<script type="text/javascript" >
    function doPrint() {
        window.open("Print.aspx");
        //location.href = "RegistrationManage.aspx?actPage=edit&targetID=<%=targetID%>";
    }
</script>

<p align=center>บันทึกข้อมูลการลงทะเบียนเรียบร้อย</p>
<p align=center><a href="javascript:doPrint()">พิมพ์บัตรและใบเสร็จ<img src="img/sys/print_32.gif" border=0 ></a></p>
<table border=0>
<tr>
<td>
<%= outBuf.ToString() %>
</td>
</tr>
</table>


<% } %>
        

        
    </div>
    </form>

    
<BTS:EndBar id=EndBar1 runat=server ucname="EndBar.ascx" />

</body>
</html>
