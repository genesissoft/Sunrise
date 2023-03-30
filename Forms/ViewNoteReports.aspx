<%@ page language="VB" autoeventwireup="false" inherits="Forms_ViewNoteReports" CodeFile ="~/Forms/viewNoteReports.aspx.vb"  enableviewstatemac="false" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self">
    </base>
    <title>View Notes</title>
    <style  type="text/css"  >
    body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
    </style>
</head>
<body topmargin="0">
    <form id="form1" runat="server">
        <table id="Table1" align="center" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td>
                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                       DisplayToolbar="False" EnableDrillDown="False" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
