<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportDailyRegistration.aspx.cs" Inherits="BTS.Page.TeacherManage" %>
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
<% if ((errorText != null) && (errorText.Length>0)) { %>
<table id="ErrTable" cellspacing="0" >
  <tr>
    <th scope="row" class="spec" ><p><font class="font3"><%=errorText%></font></p></th>
  </tr>
</table>
<% } %>
    
<% if ((actPage == null) || (actPage.Trim().Length==0) || (actPage.Equals("report")))  {  %>
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
    //document.onmousemove = mouseMove;
    // Main function to retrieve mouse x-y pos.s

</script>
<!-- <a href="javascript:history.back()">Back..</a> -->
<link rel="stylesheet" type="text/css" href="js/epoch_v106_en/epoch_styles.css" />
<script type="text/javascript" src="js/epoch_v106_en/epoch_classes.js"></script>
<script type="text/javascript">
   /*You can also place this code in a separate file and link to it like epoch_classes.js*/
   var sdate_cal, edate_cal;
   window.onload = function() {
       sdate_cal = new Epoch('epoch_popup', 'popup', document.getElementById('start_date'));
       edate_cal = new Epoch('epoch_popup', 'popup', document.getElementById('end_date'));
   };
</script>

<table id="mytable" cellspacing="0" summary="..." style="width:800px" border=0>
<caption>รายงานสรุปยอดการลงทะเบียนประจำวันแยกตามสาขา  </caption>
  <tr >
    <td>
        <table width="100%" border=0 ><tr>
          <td height="24px width="80px" align=right>จากวันที่</td>
          <td align=left width="120px">          
           <input id="start_date" name="start_date" type="text" class="txtbox1" width="120px" readonly value="<%=startDate!=null?startDate.ToString("dd/MM/yyyy", ci):DateTime.Today.ToString("dd/MM/yyyy", ci)%>"/>   
          </td >
          <td  width="100px" align=right>ถึงวันที่ </td>
           <td align=left width="120px">
           <input id="end_date" name="end_date" type="text" class="txtbox1" width="120px" readonly value="<%=endDate!=null?endDate.ToString("dd/MM/yyyy", ci):DateTime.Today.ToString("dd/MM/yyyy", ci)%>"/>   
           </td>
           <td width="70px" align=right>การจ่ายเงิน</td>
           <td align=left width="100px">
         <select id="paid_method" name="paid_method" class="txtbox1" style="width:80px" >
            <option value="-1" <%= paidMethod.Equals("-1")?"selected":"" %> >ทั้งหมด</option>
        <% 
            for (int i = 0; i < Registration.PAID_METHOD.Length; i++)
            {
                string selected = ((paidMethod != null) && (paidMethod.Equals(i.ToString()))) ? "selected" : "";
        %>       
            <option value="<%=i.ToString() %>" <%= selected %> ><%=Registration.PAID_METHOD[i] %></option>
        <% } %>            
         </select>
         </td>
        <td align=center valign=top rowspan=2>
            <!-- <a href="#bottom">Bottom</a> -->
            <input type="button" class="btn1" value="แสดงรายงาน" onclick="doSubmit()"/>
        </td>
         
         </tr>
         <tr>
         <td align=right>สาขาที่ทำรายการ</td>
         <td align=left>
        <select id="branch_regis_id" name="branch_regis_id" class="txtbox1" style="width:130px" >
            <option value="0" <%= branchRegisedID.Equals("0")?"selected":"" %> >ทุกสาขา</option>
        <% 
            for (int i = 0; i < branchList.Length; i++)
            {
                string selected = ((branchRegisedID != null) && (branchRegisedID.Equals(branchList[i]._branchID.ToString()))) ? "selected" : "";
        %>       
            <option value="<%=branchList[i]._branchID %>" <%= selected %> ><%=branchList[i]._branchName %></option>
        <% } %>
            </select>
          </td>
      
        <td align=right>สาขาของคอร์ส</td>
         <td align=left>
        <select id="branch_id" name="branch_id" class="txtbox1" style="width:130px" >
            <option value="0" <%= branchID.Equals("0")?"selected":"" %> >ทุกสาขา</option>
        <% 
            for (int i = 0; i < branchList.Length; i++)
            {
                string selected = ((branchID != null) && (branchID.Equals(branchList[i]._branchID.ToString()))) ? "selected" : "";
        %>       
            <option value="<%=branchList[i]._branchID %>" <%= selected %> ><%=branchList[i]._branchName %></option>
        <% } %>
            </select>
          </td>
          <td align=right>ผู้รับสมัคร</td>
          <td align=left>
            <select id="username" name="username" class="txtbox1" style="width:130px" >
            <option value="all" <%= userName.Equals("all")?"selected":"" %> >ทุกคน</option>
            <% 
                for (int i = 0; i < userList.Length; i++)
                {
                    string selected = ((userName != null) && (userName.Equals(userList[i]._username.ToString()))) ? "selected" : "";
            %>       
                <option value="<%=userList[i]._username %>" <%= selected %> ><%=userList[i]._firstname + " " + userList[i]._surname%></option>
            <% } %>
           </select>
          </td>     
        </tr></table>
    </td>
  </tr>
<% if ((reg == null) || (reg.Length == 0)){ %>      
  <tr>
    <td><p align=center>ไม่มีข้อมูลในช่วงเวลาดังกล่าว</p></td>
  </tr>  
<% } else { %>        
  <tr>
    <td align=right>
        <script type="text/javascript" >
            function doPrint() {
                window.open("Print.aspx");
            }
        </script>
        <a href="javascript:doPrint()">พิมพ์รายงาน<img src="img/sys/print.gif" border=0 ></a>&nbsp&nbsp&nbsp&nbsp
        <a href="javascript:setVal('actPage','save_as_excel');setVal('targetID','none');doSubmit()">ดาวน์โหลดทั้งหมดเป็น Excel<img src="img/sys/excel.gif" border=0 alt="Export Excel"></a>&nbsp
    </td>
  </tr>
    <tr>
    <td align=center>ค้นพบ <%=reg.Length %> รายการ</td>
  </tr>
  <tr>
    <td>
    <table id="Table2" cellspacing="0" summary="..." style="width:900px" border=0>  
      <tr>
        <th scope="col" align=center width="30px" NOWRAP>ลำดับ</th>
        <th scope="col" align=center width="80px">วัน-เวลา</th>
        <th scope="col" align=center width="100px">รหัสใบเสร็จ</th>
	    <th scope="col" align=center width="150px">นักเรียน</th>
	    <th scope="col" align=center width="100px">โรงเรียน</th>	    
	    <th scope="col" align=center width="100px">ระดับชั้น</th>	    
	    <th scope="col" align=center width="200px">คอร์ส</th>
	    <th scope="col" align=center width="50px">ราคาจ่าย</th>
        <th scope="col" align=center width="70px">วิธีชำระเงิน</th>
        <th scope="col" align=center width="70px">สถานะ</th>
        <th scope="col" align=center width="150px">ผู้รับสมัคร</th>
      </tr>
<%
   Response.Write(this.outBuf.ToString());
%>
    </table>
    </td>
  </tr>
  <tr>
    <td>
       <table width="300px" align=right border=1 cellpadding=0 cellspacing=0 bordercolor="#C1DAD7" bgcolor="#FFFFFF">
       <tr bgcolor="#CAE8EA">
            <td colspan=3 align=center><b>สรุปยอดรับสมัคร</b></td>
       </tr>
       <tr bgcolor="#CAE8EA">
            <td width="100px" align=center><b>วิธีชำระเงิน</b></td>
            <td width="100px" align=center><b>รายการ</b></td>
            <td width="100px" align=center><b>ยอดรวม(บาท)</b></td>
       </tr>
<% for (int i=0;i<numPaidMethod.Length;i++) { %>       
       <tr bgcolor="#FFFFFF">
            <td align=center><%=Registration.GetPaidMethodText(i.ToString())%></td>
            <td align=center><%= numPaidMethod[i] %></td>
            <td align=right><%= StringUtil.Int2StrComma(sumCostByPaidMethod[i]) %>&nbsp&nbsp</td>
       </tr>
<% } %>       
       <tr bgcolor="#FFFFFF">
            <td align=center><b>รวมทั้งสิ้น</b></td>
            <td align=center><b><%= numSuccess %></b></td>
            <td align=right><b><%= StringUtil.Int2StrComma(sumAllCost) %></b>&nbsp&nbsp</td>
       </tr>
       </table>    
    </td>
   </tr>

  <tr>
    <td>
       <table width="300px" align=right border=1 cellpadding=0 cellspacing=0 bordercolor="#C1DAD7" bgcolor="#FFFFFF">
       <tr bgcolor="#CAE8EA">
            <td colspan=3 align=center><b>สรุปยอดยกเลิก</b></td>
       </tr>
       <tr bgcolor="#CAE8EA">
            <td width="100px" align=center><b>วิธีชำระเงิน</b></td>
            <td width="100px" align=center><b>รายการ</b></td>
            <td width="100px" align=center><b>ยอดรวม(บาท)</b></td>
       </tr>
<% for (int i=0;i<numPaidMethodCancel.Length;i++) { %>       
       <tr bgcolor="#FFFFFF">
            <td align=center><%=Registration.GetPaidMethodText(i.ToString())%></td>
            <td align=center><%= numPaidMethodCancel[i] %></td>
            <td align=right><%= StringUtil.Int2StrComma(sumCostByPaidMethodCancel[i]) %>&nbsp&nbsp</td>
       </tr>
<% } %>       
       <tr bgcolor="#FFFFFF">
            <td align=center><b>รวมทั้งสิ้น</b></td>
            <td align=center><b><%= numCancel %></b></td>
            <td align=right><b><%= StringUtil.Int2StrComma(sumCancelCost) %></b>&nbsp&nbsp</td>
       </tr>
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
<anchor name="bottom"/>
        </td>
        </tr></table>
    </td>
  </tr>   
</table>               
<% } %>      

<% } else if ((actPage.Equals("add")) || (actPage.Equals("edit"))) { %>



<% } %>
        

        
    </div>
    </form>

    
<BTS:EndBar id=EndBar1 runat=server ucname="EndBar.ascx" />

</body>
</html>
