<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CurrentRate.aspx.vb" Inherits="Forms_CurrentRate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Current Rate</title>
    <link href="../Include/General.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Parkstone.css" rel="stylesheet" type="text/css" />
    <link href="../Include/General.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Intervention.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Style_New.css" type="text/css" rel="stylesheet" />

    <script language="javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/DatePicker.js" type="text/javascript"></script>

    <script language="javascript">
        function Validate() {
            if ((document.getElementById("txt_Rate").value - 0) == 0) {
                alert("Please enter a proper Rate");
                return false
            }
            if ((document.getElementById("txt_FaceValue").value - 0) == 0) {
                alert("Please enter a proper Face Value");
                return false
            }
            if ((document.getElementById("txt_HoldingCost").value - 0) == 0) {
                alert("Please enter a proper Holding Cost");
                return false
            }
            if ((document.getElementById("txt_HoldingCost").value - 0) > 100) {
                alert("The Holding Cost Percent can not be greater then 100");
                return false
            }
            return true
        }
        function Close() {
            if ((document.getElementById("txt_Currentrate").value - 0) == 0) {
                window.returnValue = document.getElementById("txt_Rate").value ;
            }
            else {
                window.returnValue = document.getElementById("txt_Currentrate").value;
            }
            document.getElementById("Hid_Id").value = window.returnValue + ":" + '<%=Session("ParentId")%>';
            window.returnValue = window.returnValue + ":" + '<%=Session("ParentId")%>';
            window.close();
        }
    </script>

</head>
<body topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
        <div>
            <table cellpadding="0" cellspacing="0" align="center" height="100%" width="100%"
                class="data_table">
                <tr align="center">
                    <td align="center">
                        <table align="center" cellpadding="0" cellspacing="0" width="95%" border="0">
                            <tr align="center">
                                <td align="center" class="SectionHeaderCSS" style="text-align: center;" colspan="2">Current Rate Calculation
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Issuer:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Issuer" runat="server" Width="250px" CssClass="TextBoxCSS" MaxLength="20"
                                        ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Security:</td>
                                <td>
                                    <asp:TextBox ID="txt_Security" runat="server" Width="250px" CssClass="TextBoxCSS"
                                        MaxLength="20" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Date of Purchase:</td>
                                <td>
                                    <asp:TextBox ID="txt_PurDate" runat="server" Width="100px" CssClass="TextBoxCSS"
                                        MaxLength="20"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('txt_PurDate',this);" />
                                    <%--<a href="javascript:doNothing()" onclick="showCalendar(document.getElementById('txt_PurDate'),'200','300');">
                                            <img border="0" class="LabelCSS" height="17" src="../Images/Calender.jpg" width="15" /></a>--%>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Rate:</td>
                                <td>
                                    <asp:TextBox ID="txt_Rate" Style="text-align: right" runat="server" Width="100px"
                                        CssClass="TextBoxCSS" MaxLength="20"></asp:TextBox>
                                    <asp:RadioButtonList ID="rdo_PhysicalDMAT" runat="server" CssClass="LabelCSS" RepeatDirection="Horizontal"
                                        Visible="false" RepeatLayout="Flow">
                                        <asp:ListItem Selected="True" Value="D">DMAT\SGL</asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                            <tr align="left">
                                <td>Face Value:</td>
                                <td style="padding: 0px;">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr align="left">
                                            <td>
                                                <asp:TextBox ID="txt_FaceValue" Style="text-align: right" runat="server" Width="100px"
                                                    CssClass="TextBoxCSS" MaxLength="20"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_FaceValue" runat="server" Width="95px" CssClass="ComboBoxCSS"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Value="1000">Thousands</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="100000">Lacs</asp:ListItem>
                                                    <asp:ListItem Value="10000000">Crores</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Holding Cost:</td>
                                <td>
                                    <asp:TextBox ID="txt_HoldingCost" Style="text-align: right" runat="server" Width="100px"
                                        CssClass="TextBoxCSS" MaxLength="20"></asp:TextBox>&nbsp;<font>%</font>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Date of Calculation:</td>
                                <td>
                                    <asp:TextBox ID="txt_CalcDate" runat="server" Width="100px" CssClass="TextBoxCSS"
                                        MaxLength="20"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('txt_CalcDate',this);" />
                                    <%-- <a href="javascript:doNothing()" onclick="showCalendar(document.getElementById('txt_CalcDate'),'200','300');">
                                            <img border="0" class="LabelCSS" height="17" src="../Images/Calender.jpg" width="15" /></a>--%>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Current Rate:</td>
                                <td>
                                    <asp:TextBox ID="txt_Currentrate" Style="text-align: right" runat="server" Width="100px"
                                        CssClass="TextBoxCSS" MaxLength="20" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btn_CalcRate" runat="server" Text="Calculate Current Rate" ToolTip="Calculate Current Rate"
                                        CssClass="ButtonCSS" Width="150px" />
                                    <asp:Button ID="btn_Ret" runat="server" Text="Close" ToolTip="Close" CssClass="ButtonCSS"
                                        OnClientClick="return Close();" UseSubmitBehavior="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="Hid_MatDate" runat="server" />
            <asp:HiddenField ID="Hid_MatAmt" runat="server" />
            <asp:HiddenField ID="Hid_CallDate" runat="server" />
            <asp:HiddenField ID="Hid_CallAmt" runat="server" />
            <asp:HiddenField ID="Hid_PutDate" runat="server" />
            <asp:HiddenField ID="Hid_PutAmt" runat="server" />
            <asp:HiddenField ID="Hid_CoupDate" runat="server" />
            <asp:HiddenField ID="Hid_CoupRate" runat="server" />
            <asp:HiddenField ID="Hid_SecurityId" runat="server" />
            <asp:HiddenField ID="Hid_Frequency" runat="server" />
            <asp:HiddenField ID="Hid_GovernmentFlag" runat="server" />
            <asp:HiddenField ID="Hid_Issue" runat="server" />
            <asp:HiddenField ID="Hid_InterestDate" runat="server" />
            <asp:HiddenField ID="Hid_BookClosureDate" runat="server" />
            <asp:HiddenField ID="Hid_DMATBkDate" runat="server" />
            <asp:HiddenField ID="Hid_FaceValue" runat="server" />
            <asp:HiddenField ID="Hid_RateAmtFlag" runat="server" />
            <asp:HiddenField ID="Hid_Id" runat="server" />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
        </div>
    </form>
</body>
</html>
