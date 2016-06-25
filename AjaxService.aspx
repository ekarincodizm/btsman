<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AjaxService.aspx.cs" Inherits="BTS.Page.AjaxService" %>

<% if (svc.ToLower().Equals("qcomparechart"))
 { 
%>
        <script type="text/javascript">

            so.write("compare");
        </script>
<% 
} else {
    Response.Write(outBuf.ToString());
}
%>