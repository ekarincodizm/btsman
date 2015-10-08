<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistrationManage.aspx.cs" Inherits="BTS.Page.RegistrationManage" %>
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
    //document.onmousemove = mouseMove;
    // Main function to retrieve mouse x-y pos.s

</script>

<table id="mytable" cellspacing="0" summary="..." style="width:800px" border=0>
<caption>ข้อมูลการลงทะเบียน  </caption>
  <tr >
    <td>
        <table width="100%" ><tr>
        <td height="24px">
            <a href="javascript:history.back()">Back..</a>             
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
        <th scope="col" align=center width="100px" NOWRAP>รหัส</th>
	    <th scope="col" align=center width="70px">วันที่สมัคร</th>
	    <th scope="col" align=center width="70px">วันที่รับชำระเงิน</th>
	    <th scope="col" align=center width="210px">คอร์ส</th>
	    <th scope="col" align=center width="200px">นักเรียน</th>
        <th scope="col" align=center width="90px">ค่าเรียน(จ่ายจริง)</th>
        <th scope="col" align=center width="50px">สถานะ</th>        
        <th scope="col" align=center width="100px">หมายเหตุ</th>
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
<% } else if (actPage.Equals("edit")) { %>

<script type="text/javascript" >

     function confirmRefund() {
        var cost = document.getElementById('refund_cost').value;
            if ((cost.length != 0) && (!isInt(cost))) {alert("โปรดระบุจำนวนเงินที่ต้องการคืน");return; }
            
            if (parseInt(cost) == 0) {
                setVal('actPage', 'refund'); 
                setVal('targetID', '<%= theReg._regisID %>');
                doSubmit(); 
            } else {
            
                var maxrefund = <%= (theReg._discountedCost) %>;
                if ((parseInt(cost) < 0) || (parseInt(cost)>maxrefund)) { alert("จำนวนคืนเงินได้ไม่เกิน "+maxrefund+" บาท"); return; }

                if (confirm('ต้องการคืนเงินจำนวน ' + cost + ' บาท? \r\n โปรดตรวจสอบก่อนว่าเงินจำนวนนี่้ยังไม่ถูกเบิกจ่าย มิฉะนั้นบัญชีจะติดลบ \r\n กด OK เพื่อยืนยัน')) {
                    setVal('actPage', 'refund'); 
                    setVal('targetID', '<%= theReg._regisID %>');
                    doSubmit(); 
                }
           }
    } 
    
     function confirmEditInfo() {

            if (confirm('ต้องการแก้ไขข้อมูล \r\n กด OK เพื่อยืนยัน')) {
                setVal('actPage', 'edit_submit'); 
                setVal('targetID', '<%= theReg._regisID %>');
                doSubmit(); 
            }
    }    
    
    function doInitPrintCard(payid) {        
        location.href = "RegistrationManage.aspx?actPage=init_print_card&targetID="+payid;
    }
                    
</script>                

       <table width="800px" cellpadding=0 cellspacing=0 border=0>
        <tr>
            <td height="24px" width="50px" align=center class="tabhead1_selected" NOWRAP valign="middle" ><b><%= Registration.GetRegistrationID(theReg._regisID) %></b></td>            
            <td width="700px" align=right>&nbsp</td>
        </tr>        
        <tr >
            <td  height="30px" width="300px" align=right valign=bottom colspan=2 class="tabpane1">
                  พิมพ์การ์ด
                  <a href="RegistrationManage.aspx?actPage=init_print_card&targetID=<%=theReg._regisID %>"><img src="img/sys/print_32.gif" border=0></a>
                  &nbsp
                  พิมพ์ใบเสร็จ
                  <a href="RegistrationManage.aspx?actPage=init_print_receipt&targetID=<%=theReg._regisID %>"><img src="img/sys/print_32.gif" border=0></a>                  
                  &nbsp
                  พิมพ์ทุกอย่าง
                  <a href="RegistrationManage.aspx?actPage=init_print_all&targetID=<%=theReg._regisID %>"><img src="img/sys/print_32.gif" border=0></a>                  
                  
            </td>
        </tr>
      
        <tr>
            <td align=left colspan=2 class="tabpane1">
                    <%= outBuf.ToString() %> <br />
            </td>
        </tr>
        <tr>
            <td width=100% align="center" ><td><font color=red><%=Request["msgText"] %></font> </td>
        </tr>
        <tr height="100px">        
            <td align=center colspan=2 class="tabpane1_bottom">
                <table border=0>
                    <tr><td width="100px" align=right>จ่ายเงินผ่านทาง&nbsp&nbsp</td><td align=left>
                        <select name="paid_method" id="paid_method" class="txtbox1">
                            <option value=0 <%=((theReg!=null)&&(theReg._paidMethod==0))?"selected":"" %>><%= Registration.GetPaidMethodText("0")%></option>
                            <option value=1 <%=((theReg!=null)&&(theReg._paidMethod==1))?"selected":"" %>><%= Registration.GetPaidMethodText("1")%></option>
                            <option value=2 <%=((theReg!=null)&&(theReg._paidMethod==2))?"selected":"" %>><%= Registration.GetPaidMethodText("2")%></option>
                            <option value=3 <%=((theReg!=null)&&(theReg._paidMethod==3))?"selected":"" %>><%= Registration.GetPaidMethodText("3")%></option>
                            <option value=4 <%=((theReg!=null)&&(theReg._paidMethod==4))?"selected":"" %>><%= Registration.GetPaidMethodText("4")%></option>
                        </select>                        
                    </td></tr>                                      
                    <tr><td width="100px" align=right valign=top>หมายเหตุ&nbsp&nbsp</td><td align=left>
                        <textarea name="note" id="note" class="txtbox1" style="width:200px;height:70px"><%=((theReg!=null)&&(theReg._note!=null))?theReg._note:"" %></textarea>                        
                    </td></tr>                      
                    <tr><td width="100px" align=right>เปลี่ยนสถานะ&nbsp&nbsp</td><td align=left>
                        <select name="status" id="status" class="txtbox1" > 
                            <option value="0" <%=((theReg!=null)&&(theReg._status==0))?"selected":"" %> >ปกติ</option>
                            <option value="1" <%=((theReg!=null)&&(theReg._status==1))?"selected":"" %> >ยกเลิก</option>
                        </select>
                    </td></tr>                                          
                
                    <tr><td width="100px" align=right>คืนเงิน&nbsp&nbsp</td><td align=left>
                        <input type=text id="refund_cost" name="refund_cost" class="txtbox1" style="width:150px" value="0" />                    
                    </td></tr>                      
                    
                    <tr><td width="100px" align=right>&nbsp&nbsp</td><td align=left>
                        <input type=button value="แก้ไขข้อมูล" class="btn1" onclick="confirmRefund()" /> 
                        <input type=button value="ย้อนกลับ" class="btn1" onclick="location.href='RegistrationManage.aspx?actPage=list'" />                         
                    </td></tr>                      
                </table>
            </td>
        </tr>      
              
        </table>   
<% } else if ((actPage.Equals("init_print_card")) || (actPage.Equals("init_print_receipt")) || (actPage.Equals("init_print_all"))) { %>
<script type="text/javascript" >
    function doPrint() {
        window.open("Print.aspx");
        location.href = "RegistrationManage.aspx?actPage=edit&targetID=<%=targetID%>";
    }
    doPrint();
</script>


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
