<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PasswordConfiguration.aspx.vb"
    Inherits="Forms_PasswordConfiguration" %>

<%@ Register Assembly="Configuration" Namespace="Configuration.UserSecurity" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href="../Include/StanChart.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <cc1:Configuration ID="Configuration1" runat="server" LabelCSSClass="LabelCSS" TextBoxCSSClass="TextBoxCSS"
                ComboBoxCSSClass="ComboBoxCSS" ButtonCSSClass="ButtonCSS" MessageCSSClass="MessageCSS"></cc1:Configuration>
        </div>
    </form>
</body>
</html>
