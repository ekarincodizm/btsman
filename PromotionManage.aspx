<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromotionManage.aspx.cs" Inherits="BTS.Page.PromotionManage" %>
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
<caption>โปรโมชัน  </caption>
  <tr >
    <td>
        <table width="100%" ><tr>
        <td height="24px">
            <!-- <a href="javascript:history.back()">Back..</a> -->
            <a href="javascript:setVal('actPage','add');doSubmit('PromotionManage.aspx');"><img src="img/sys/add.gif" border=0> เพิ่มโปรโมชันใหม่..</a>
        </td>
        <td align=right>
            <input type=text id="qsearch" name="qsearch" class="txtbox2" style="width:150px" onkeypress="doSearch()" />  
        </td>
        </tr></table>
    </td>
  </tr>
  <tr>
    <td>
    <table id="Table2" cellspacing="0" summary="..." style="width:800px" border=0>  
      <tr>
        <th scope="col" align=center width="50px" NOWRAP>รหัส</th>
        <th scope="col" align=center width="70px" NOWRAP>Active</th>
        <th scope="col" align=center width="300px" NOWRAP>โปรโมชั่น</th>
	    <th scope="col" align=center width="300px">คอร์ส</th>
	    <th scope="col" align=center width="100px">ราคาเต็ม</th>
	    <th scope="col" align=center width="100px">ลดเหลือ</th>
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
<!-- div show course info  -->
<div name="divdetail" id="divdetail" style="border-color:Black;background-color:White; filter:alpha(opacity=75); -moz-opacity: 0.75; opacity: 0.75;display:none;position:relative">xxxxxxxxxxxxxxxxxxxx</div>    


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

    function queryCourseDetail(course_id) {
        showDivAt('divdetail');
        document.getElementById(layerName).innerHTML = "<img src=\"img/sys/loading.gif\" />";
        ajaxPost("AjaxService.aspx", "svc=<%=AjaxSvc.WIZ_Q_COURSE_DETAIL%>&course_id=" + course_id, layerName, true);        
    }

</script>

<script type="text/javascript" >
    function sumFullCost() {
        var numfc = document.getElementById('fullcost_num');
        var fullcost = 0;
        var node = "course";   //"sonode" + foldId + "_" + objId;
        var inputList = document.documentElement.getElementsByTagName("input");

        for (var i = 0; i < inputList.length; i++) {
            var elem = inputList[i];
            if (elem.type == "checkbox") {
                if (elem.name.indexOf("course") == 0) {
                    if (elem.checked) {
                        fullcost += parseInt(document.getElementById("cost_" + elem.name).value);                            
                    }
                }
            }
        }

        document.getElementById("fullcost").value = fullcost;
    }

    function doChangeFilterCourse() {
        document.getElementById("actPage").value = "add";
        doSubmit('PromotionManage.aspx');
    }

</script>


<table id="Table1" cellspacing="0" width="700px" >
<caption>โปรโมชัน: <%=actPage%> </caption>

  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>ชื่อโปรโมชัน</th>    
    <th scope="row" class="spec" ><input type=text id="promotion_name" name="promotion_name" class="txtbox1" style="width:150px" value="<%=thePromotion!=null?thePromotion._promotionName:""%>" /></th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>รายละเอียด</th>    
    <th scope="row" class="spec"><textarea class="txtarea1" id="promotion_desc" name="promotion_desc" style="width:300px;height:70px"><%=thePromotion != null ? thePromotion._promotionDesc : ""%></textarea></th>    
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>Active</th>    
    <th scope="row" class="spec<%=((thePromotion!=null)&&(thePromotion._isActive))?"_2":"" %>"  onMouseover="this.className='spec_2'" onMouseout=" if (!document.getElementById('is_active').checked) {this.className='spec'; }" 
        onclick="document.getElementById('is_active').checked=!document.getElementById('is_active').checked; if (document.getElementById('is_active').checked) {this.className='spec_2';}"; return;"  >
        <input type=checkbox id="is_active" name="is_active" <%=((thePromotion!=null)&&(thePromotion._isActive))?"checked":"" %>  onclick="document.getElementById('is_active').checked=!document.getElementById('is_active').checked;"/>
        &nbsp<font class="font02">*Active โปรโมชั่นจะถูกใช้ในการคำนวนรายได้</font> </th>

  </tr>
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>คอร์ส</th>  
    <th scope="row" class="spec">
        <%= outBuf.ToString() %>
    </th>
  </tr>    
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>ราคาเต็ม</th>    
    <th scope="row" class="spec" ><input type=text id="fullcost" name="fullcost" class="txtbox1" readonly style="width:150px" value="<%=fullCost.ToString()%>" /></th>
  </tr>
  <tr>
    <td scope="col" class="formTitle" width="100px" valign="top" NOWRAP>ราคาหักส่วนลด</th>    
    <th scope="row" class="spec" ><input type=text id="cost" name="cost" class="txtbox1" style="width:150px" value="<%=thePromotion!=null?thePromotion._cost.ToString():"0"%>" /></th>
  </tr>
  

    
  <tr>    
    <td scope="row" align="center" colspan="2" ><p align="center">
    <input type="button" class="btn1" value="Back" onclick="history.back()"/>
    <input type="button" class="btn1" value="Save" onclick="if (checkEmptyText('promotion_name')) {setVal('actPage','<%=actPage%>_submit');doSubmit();}" />
    <input type="reset" class="btn1" value="Reset" />   
    </p></td>
  </tr>  
</table>


<% } %>
        

        
    </div>
    </form>

    
<BTS:EndBar id=EndBar1 runat=server ucname="EndBar.ascx" />

</body>
</html>
