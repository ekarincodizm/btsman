<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SideBar.ascx.cs" Inherits="BTS.Control.SideBar" %>

<!--- Top Page --->

<tr>
<td width=100 valign=top >
    <table border=0>
        <tr><td><a href="RegisterCourse.aspx?actPage=new">สมัครเรียน</a></td></tr>
        <tr><td><a href="RegistrationManage.aspx">พิมพ์บัตร/สลิป</a></td></tr>
        <!-- <tr><td><a href="#">ค้นหาด่วน</a></td></tr> -->
        <tr><td>เบิกจ่ายค่าสอน</td></tr>

<!--        <tr><td>&nbsp&nbsp <a href="PaymentManage.aspx?actPage=preview_multi">จ่ายทั้งหมดตามผู้สอน</a></td></tr>        -->
        <tr><td>&nbsp&nbsp <a href="PaymentManage.aspx">จ่ายบางส่วนตามคอร์ส</a></td></tr>
        <tr><td>จัดการข้อมูล</td></tr>
        <tr><td>&nbsp&nbsp <a href="BranchManage.aspx">สาขา</a></td></tr>
        <tr><td>&nbsp&nbsp <a href="RoomManage.aspx">ห้องเรียน</a></td></tr>        
        <tr><td>&nbsp&nbsp <a href="TeacherManage.aspx">อาจารย์</a></td></tr>
        <tr><td>&nbsp&nbsp <a href="PaidGroupManage.aspx">กลุ่มอาจารย์</a></td></tr>     
        <tr><td>&nbsp&nbsp <a href="StudentManage.aspx">นักเรียน</a></td></tr>                
        <tr><td>&nbsp&nbsp <a href="CourseManage.aspx">คอร์ส</a></td></tr>                
        <tr><td>&nbsp&nbsp <a href="PromotionManage.aspx">โปรโมชั่น</a></td></tr>               
        <tr><td>&nbsp&nbsp <a href="RegistrationManage.aspx">การลงทะเบียน</a></td></tr>                 
        <tr><td>รายงาน</td></tr>        
        <tr><td>&nbsp&nbsp <a href="ReportDailyRegistration.aspx?actPage=report">สรุปยอดการสมัครประจำวัน</a></td></tr>                        
        <!-- <tr><td>&nbsp&nbsp <a href="#">ซับซ้อน</a></td></tr>                                -->
        <tr><td>จัดการระบบ</td></tr>        
        <tr><td>&nbsp&nbsp <a href="Administrator.aspx?actpage=edit&targetID=<%=userID%>">เปลี่ยนข้อมูลส่วนตัว</a></td></tr>                                
        <tr><td NOWRAP >&nbsp&nbsp <a href="Administrator.aspx">จัดการข้อมูลผู้ใช้ระบบ</a></td></tr>                        
<!--         <tr><td>&nbsp&nbsp <a href="#">ปรับแต่งคอนฟิก</a></td></tr>                                -->
<!--        <tr><td>&nbsp&nbsp <a href="#">สำรองข้อมูล</a></td></tr>                                -->
        <!-- <tr><td>&nbsp&nbsp <a href="#">ตรวจล็อก</a></td></tr>                                        -->
    </table>
    

</td>

<td valign="top">

<!--- Center Page --->