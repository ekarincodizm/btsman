<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print.aspx.cs" Inherits="BTS.Page.Print" %>
<STYLE TYPE='text/css'>
    P.pagebreakhere {page-break-before: always}
</STYLE>
<%= outBuf.ToString() %>
<script type="text/javascript" >
    window.print();
</script>