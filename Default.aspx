﻿<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">

 />
    <asp:Calendar ID="Calendar1" runat="server" BackColor="White" 
        BorderColor="Black" DayNameFormat="Shortest" Font-Names="Times New Roman" 
        Font-Size="10pt" ForeColor="Black" Height="220px" NextPrevFormat="FullMonth" 
        TitleFormat="Month" Width="400px">
        <SelectedDayStyle BackColor="#CC3333" ForeColor="White" />
        <SelectorStyle BackColor="#CCCCCC" Font-Bold="True" Font-Names="Verdana" 
            Font-Size="8pt" ForeColor="#333333" Width="1%" />
        <TodayDayStyle BackColor="#CCCC99" />
        <OtherMonthDayStyle ForeColor="#999999" />
        <DayStyle Width="14%" />
        <NextPrevStyle Font-Size="8pt" ForeColor="White" />
        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" 
            ForeColor="#333333" Height="10pt" />
        <TitleStyle BackColor="Black" Font-Bold="True" Font-Size="13pt" 
            ForeColor="White" Height="14pt" />
    </asp:Calendar>
    </form>
</body>
</html>
