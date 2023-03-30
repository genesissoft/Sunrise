<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewDealReports.aspx.vb"
    Inherits="Forms_ViewDealReports" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="X-UA-Compatible" content="IE=7" />
<head runat="server">
    <base target="_self"></base>
    <title>View Deals</title>
    <style type="text/css">
        body
        {
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
                    DisplayToolbar="False" EnableDrillDown="False" ToolPanelView="None" DisplayPage="true"/>
            </td>
        </tr>
        <asp:HiddenField ID="Hid_AuthorisedPer" runat="server" />
        <asp:HiddenField ID="Hid_DealslipId" runat="server" />
        <asp:HiddenField ID="Hid_DealSlipNo" runat="server" />
        <asp:HiddenField ID="Hid_dealTransType" runat="server" />
        <asp:HiddenField ID="Hid_TransType" runat="server" />
        <asp:HiddenField ID="Hid_Intids" runat="server" />
        <asp:HiddenField ID="Hid_DealDate" runat="server" />
    </table>
    </form>
</body>
</html>
