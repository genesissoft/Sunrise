<%@ Page Language="VB" AutoEventWireup="false" CodeFile="YieldCalculation.aspx.vb"
    Inherits="Forms_YieldCalculation" %>

<%@ Register Src="~/UserControls/YieldCalculater.ascx" TagName="YieldCalc" TagPrefix="uc" %>
<%--<%@ Register Src="~/UserControls/YieldCalculater.ascx" TagName="YieldCalc" TagPrefix="uc" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self"></base>
    <title>Yield Calculation</title>
    <link href="../Include/StanChart.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style_New.css" type="text/css" rel="stylesheet" />
</head>
<body style="margin-left: 0px; margin-top: 5px;" class="popupbackground">
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0" align="center" width="98%" class="data_table">
            <tr align="center">
                <td class="SectionHeaderCSS popupbackground" style="text-align: center;">
                    Yield Calculation
                </td>
            </tr>
            <tr class="line_separator">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    <table cellpadding="0" cellspacing="0" align="center" width="65%">
                        <tr align="left">
                            <td>
                                Issuer Of Security:
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Issuer" runat="server" Width="300px" CssClass="TextBoxCSS" MaxLength="20"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr align="left">
                            <td>
                                Name Of Security:
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Security" runat="server" Width="300px" CssClass="TextBoxCSS"
                                    MaxLength="20" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="line_separator">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    <uc:YieldCalc ID="UC_YieldCalc" runat="server"></uc:YieldCalc>
                    <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                    <asp:HiddenField ID="Hid_StepUp" runat="server" />
                </td>
            </tr>
            <tr align="center" valign="bottom">
                <td>
                    <asp:Button ID="btnShowAccInerest" runat="server" Text="Show Accured Interest" ToolTip="Show Accured Interest"
                        CssClass="ButtonCSS" Width="145px" />
                </td>
            </tr>
        </table>
        <table border="0" align="center" style="border-color: Black;" width="90%" id="tblAccInterest" visible="false"  
            runat="server">
            <tr align="left">
                <td style="width: 15%;" class="LabelCSS">
                    Amount :
                </td>
                <td style="width: 30%;">
                    <asp:Label ID="txt_Amount" runat="server" Width="100px" CssClass=" LabelCSS" MaxLength="20"></asp:Label>
                </td>
                <td style="width: 25%;">
                    &nbsp;
                </td>
                <td style="width: 30%;">
                    &nbsp;
                </td>
            </tr>
            <tr align="left" class="LabelCSS">
                <td>
                    <font id="row_Interest" runat="server" class="LabelCSS">Interest : </font>
                </td>
                <td>
                    <asp:Label ID="txt_AddInterest" runat="server" Width="100px" CssClass=" LabelCSS"
                        MaxLength="20"></asp:Label>
                </td>
                <td align="left" class="LabelCSS">
                    From Date - To Date :
                </td>
                <td align="left">
                    <asp:Label ID="lbl_AddInterest" runat="server" Height="16px" CssClass=" LabelCSS"
                        BorderStyle="Solid" BorderWidth="0px" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="LabelCSS">
                    Sett. Amount :
                </td>
                <td align="left">
                    <asp:Label ID="txt_SettAmt" runat="server" Width="100px" CssClass="LabelCSS" MaxLength="20"></asp:Label>
                </td>
                <td align="left" class="LabelCSS">
                    Days :
                </td>
                <td align="left">
                    <asp:Label ID="lbl_SettAmt" runat="server" CssClass="LabelCSS" BorderStyle="Solid"
                        BorderWidth="0px" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="HDAcc_Id" runat="server" />
        <asp:HiddenField ID="HDAcc_Date" runat="server" />
        <asp:HiddenField ID="HDAcc_Rate" runat="server" />
        <asp:HiddenField ID="HDAcc_FaceValue" runat="server" />
        <asp:HiddenField ID="HDAcc_Multiple" runat="server" />
        <asp:HiddenField ID="HDAcc_StepUp" runat="server" />
        <asp:HiddenField ID="HDAcc_ComboFaceValue" runat="server" />
        <asp:HiddenField ID="HDAcc_RateActual" runat="server" />
        <asp:HiddenField ID="Hid_IntDays" runat="server" />
    </div>
    </form>
</body>
</html>
