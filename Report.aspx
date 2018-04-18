<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Report" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function Print()
        {
            var restoreContent = document.body.innerHTML;
            var printContent = document.getElementById('ReportViewer1_ctl09').innerHTML
            document.body.innerHTML = printContent
            window.print();
            document.body.innerHTML = restoreContent;
        }
    </script>
    <style type="text/css" media="print">
@page {
    size: auto;   /* auto is the initial value */
    margin: 0;  /* this affects the margin in the printer settings */
}
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="float:right ; margin-right:10px; margin-bottom:5px;">
        <asp:Button ID="btn" runat="server"  Text="Print Report" OnClientClick="Print()" />
         </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <rsweb:ReportViewer ID="ReportViewer1" runat="server">
        </rsweb:ReportViewer>
    </form>
</body>
</html>
