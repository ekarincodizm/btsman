<%@ Register TagPrefix="BTS" TagName="VerifyAA" Src="control/VerifyAA.ascx" %>
<%@ Register TagPrefix="BTS" TagName="TopBar" Src="control/TopBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="SideBar" Src="control/SideBar.ascx" %>
<%@ Register TagPrefix="BTS" TagName="EndBar" Src="control/EndBar.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="BTS.Page.Home" %>
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

</form>
    
<BTS:EndBar id=EndBar1 runat=server ucname="EndBar.ascx" />

<%
    if (Session[SessionVar.BRANCH_SELECTED]==null)
    {
%>
<script language="javascript">
    showPopup();
   // setTimeout("showPopup()",1000);
</script>        
<%
    }
%>

</body>
</html>
