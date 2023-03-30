<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BackSideNormal.aspx.vb" Inherits="Forms_BackSideNormal" Title="BackSideNormal"  %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
                 BackColor="white" Height="50px" ReportSourceID="CrystalReportSource1"
                Width="350px" DisplayToolbar="False" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" />
            <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                <Report FileName="C:\Inetpub\wwwroot\eInstaDeal\Reports\BacksideContractNoteNormal.rpt">
                </Report>
            </CR:CrystalReportSource>
            &nbsp;
        </div>
    </form>
</body>
</html>
