<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentManage.aspx.cs" Inherits="BTS.Page.PaymentManage" %>
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

        document.getElementById(layerName).innerHTML = "<img class=\"img1\" src=\"" + divtxt + "\" >";
    }

    function hideDiv() {
        document.getElementById(layerName).style.display = "none";
        document.getElementById(layerName).innerHTML = "<img src=\"img/sys/loading.gif\"/>";
    }

    function doSearch() {
        if (event.keyCode == 13)
            document.getElementById("actPage").value = "list";
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
    
<% if ((actPage == null) || (actPage.Trim().Length==0) || (actPage.Equals("list")))  {  %>
<!-- List -->
<div name="divchklist" id="divdetail" style="border-color:Black;background-color:White; filter:alpha(opacity=75); -moz-opacity: 0.75; opacity: 0.75;display:none;position:relative">
<img src="img/sys/loading.gif" />
</div>


<table id="mytable" cellspacing="0" summary="..." style="width:800px" border=0>
<caption>เบิกจ่ายค่าสอน  </caption>
  <tr >
    <td>
        <table width="100%" ><tr>
        <td height="24px">
            <font color=red>*รายการที่แสดงขึ้นอยู่กับสาขาที่ผู้ใช้งานระบบสังกัด</font>
            <select name="filter_payment" id="filter_payment" class="chb1" onchange="doSubmit()">
                <option value=0 <%=filterPayment.Equals("0")?"selected":""%> >แสดงคอร์สที่สามารถเบิกจ่ายได้</option>
                <option value=1 <%=filterPayment.Equals("1")?"selected":""%> >แสดงทั้งหมด</option>
            </select>
        </td>
        <td align=right>
            &nbsp
        </td>
        </tr></table>
    </td>
  </tr>
  <tr>
    <td>
    <table id="Table2" cellspacing="0" summary="..." style="width:800px" border=0>  
      <tr>
        <th scope="col" align=center width="350px">คอร์ส</th>
	    <th scope="col" align=center width="80px">รายได้</th>
	    <th scope="col" align=center width="80px">ส่วนแบ่ง</th>	    
	    <th scope="col" align=center width="80px">จ่ายแล้ว</th>
	    <th scope="col" align=center width="80px">คงเหลือ</th>
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
        <table width="100%" >
        <tr><td colspan=2 align=center> <input type=button value="เบิกจ่ายค่าสอน" class="btn1" onclick="setVal('actPage','preview_multi');doSubmit()"  /></td></tr>
        <tr>
        <td height="20px">
            &nbsp
        </td>
        <td align=right>
<% 
    Response.Write(this.outBuf2.ToString());
%>                       
        </td>
        </tr>
        </table>
    </td>
  </tr>   
</table>               
<!-------------------------------------------------------------------------------------------------------------->                        
<% } else if (actPage.Equals("list_by_teacher")) { %>

<!-------------------------------------------------------------------------------------------------------------->                        
<% } else if (actPage.Equals("view")) { %>

<script type="text/javascript" >

    function confirmPaid() {
        var cost = document.getElementById('paid_cost').value;
            if ((cost.length == 0) || (!isInt(cost))) {alert("โปรดระบุจำนวนเงินที่ต้องการเบิก");return; }
            var maxpaid = <%= (thePayment._sumMaxPayable - thePayment._sumPaidCost) %>;
            if ((parseInt(cost) <= 0) || (parseInt(cost)>maxpaid)) { alert("จำนวนเงินเบิกได้ไม่เกิน "+maxpaid+" บาท"); return; }
            
            if (confirm('ต้องการเบิกจ่ายเงินจำนวน ' + cost + ' บาท?  กด OK เพื่อยืนยัน \r\n โปรดเก็บใบเสร็จพร้อมลายเซ้นผู้รับเงินไว้เป็นหลักฐานทุกครั้ง')) {
                setVal('actPage', 'paid_submit'); 
                setVal('targetID', '<%= thePayment._courseID %>');
                doSubmit(); 
            }
    }
    
    function doInitPrint(payid) {        
        location.href = "PaymentManage.aspx?actPage=init_print&targetID="+payid;
    }
                    
</script>                

       <table width="800px" cellpadding=0 cellspacing=0 >
        <tr>
            <td height="24px" width="50px" align=center class="tabhead1_selected" NOWRAP valign="middle" ><b><%= thePayment._btsCourseID %></b></td>            
            <td width="700px" align=right>&nbsp</td>
        </tr>        
        <tr>
            <td align=left colspan=2 class="tabpane1">
                    <%= outBuf.ToString() %> <br />
            </td>
        </tr>
        <tr height="100px">
            <td align=center colspan=2 class="tabpane1_bottom">
                <table border=0>
                    <tr><td width="100px" align=right>เบิกจ่ายเงิน&nbsp&nbsp</td><td align=left><input type=text name="paid_cost" id="paid_cost" class="txtbox1" /> </td></tr>  
                    <tr><td width="100px" align=right>ผู้รับเงิน&nbsp&nbsp</td><td align=left>
                        <select name="receiver_teacher_id" id="receiver_teacher_id" class="txtbox1" > 
<% for (int i=0;i<listTeacher.Length;i++) { %>                        
                            <option value=<%= listTeacher[i]._teacherID %> ><%= listTeacher[i]._firstname + " " + listTeacher[i]._surname %></option>
<% } %>
                        </select>
                    </td></tr>                      
                    <tr><td width="100px" align=right>&nbsp&nbsp</td><td align=left>
                        <input type=button value="เบิกเงิน" class="btn1" onclick="confirmPaid()" /> 
                        <input type=button value="ย้อนกลับ" class="btn1" onclick="history.back()" />                         
                    </td></tr>                      
                </table>
            </td>
        </tr>      
        </table>   
<!-------------------------------------------------------------------------------------------------------------->                        
<% } else if (actPage.Equals("preview_multi")) { %>

<table id="Table5" cellspacing="0" summary="..." style="width:800px" border=0>
<caption>เบิกจ่ายค่าสอนตามผู้สอน  </caption>
  <tr >
    <td>
        <table width="100%" ><tr>
        <td height="24px">
<!--            <font color=red>*รายการที่แสดงขึ้นอยู่กับสาขาที่ผู้ใช้งานระบบสังกัด</font> -->
            <select name="teacher_id" id="teacher_id" class="chb1" onchange="doSubmit()">
<% for (int i = 0; i < listTeacher.Length; i++)  { %>            
                <option value=<%=listTeacher[i]._teacherID %>  <%=((theTeacher!=null)&&(theTeacher._teacherID==listTeacher[i]._teacherID))?"selected":"" %> >
                    <%= Teacher.GetTeacherID(listTeacher[i]._teacherID) + " " + listTeacher[i]._firstname + " " + listTeacher[i]._surname%>
                </option>
<% } %>            
            </select>
        </td>
        <td align=right>
            &nbsp
        </td>
        </tr></table>
    </td>
  </tr>
  <tr>
    <td>
       <%= outBuf.ToString() %>  
    </td>
  </tr>                     
  <tr>
    <td>
  
          <table border=0 width="800px" align=left>
          <tr height="100px">
           <td align=center colspan=2 class="tabpane1_bottom">
                <table border=0>
                    <tr><td width="100px" align=right>&nbsp&nbsp</td><td align=left>
                        <input type=button value="เบิกเงิน" class="btn1" onclick="confirmPaid()" /> 
                        <input type=button value="ย้อนกลับ" class="btn1" onclick="history.back()" />                         
                    </td></tr>                      
                </table>
            </td>
        </tr>      
        </table>
    </td>
  </tr>                     
 </table>       
        
<!-------------------------------------------------------------------------------------------------------------->                
<% } else if (actPage.Equals("init_print")) { %>
<script type="text/javascript" >
    function doPrint() {
        window.open("Print.aspx");
        location.href = "PaymentManage.aspx?actPage=list";
    }
</script>


<p align=center> ข้อมูลการเบิกจ่ายถูกบันทึกเรียบร้อย โปรดพิมพ์ใบเสร็จพร้อมลายเซ็นต์ผู้รับเงินเก็บไว้เป้นหลักฐาน  </p>
<p align=center> <input type=button value="พิมพ์ใบเสร็จ" class="btn1" onclick="doPrint()" /> </p>
<p align=center> <input type=button value="กลับไปหน้าเบิกจ่าย" class="btn1" onclick="location.href='PaymentManage.aspx?actPage=list'" /> </p>
<%= outBuf.ToString() %> 

<% } else if ((actPage.Equals("add")) || (actPage.Equals("edit"))) { %>

<!-- Add/Edit -->
<table id="Table1" cellspacing="0" >
<caption>คอร์ส: <%=actPage%> </caption>


<% } %>
        

        
    </div>
    </form>

    
<BTS:EndBar id=EndBar1 runat=server ucname="EndBar.ascx" />

</body>
</html>
