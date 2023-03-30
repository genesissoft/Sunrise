<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="TransactionCostMaster.aspx.vb" Inherits="Forms_TransactionCostMaster"
    Title="Transaction Cost Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PhysicalDMAT_0").checked == true) {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_Bank").value) == "") {
                    AlertMessage('Validation', 'Please Select Bank', 175, 450);
                    return false;
                }
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PhysicalDMAT_1").checked == true) {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_SGLWith").value) == "") {
                    AlertMessage('Validation', 'Please Select SGL Bank', 175, 450);
                    return false;
                }
            }

            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_FromDate").value) == "") {
                AlertMessage('Validation', 'Please Enter Start Date', 175, 450);
                return false;
            }
            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_HCAmount").value) == "") {
                AlertMessage('Validation', 'Please Enter Holding Cost Rate', 175, 450);
                return false;
            }
            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_IDAmount").value) == "") {
                AlertMessage('Validation', 'Please Enter Intraday Rate', 175, 450);
                return false;
            }
        }

        function CheckPhysicalDMAT() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PhysicalDMAT_1").checked == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_row_SGL").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_Bank").style.display = "none";
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_row_SGL").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_Bank").style.display = "";
            }
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Transaction Cost Master
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table cellspacing="0" cellpadding="0" border="0" align="center" width="45%">
                            <tr align="left">
                                <td>Security Type:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList ID="rdo_SecurityType" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass="LabelCSS" TabIndex="2" AutoPostBack="false">
                                        <asp:ListItem Value="S" Selected="True">SLR</asp:ListItem>
                                        <asp:ListItem Value="N">Non-SLR</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Mode of Delivery:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList ID="rdo_PhysicalDMAT" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass="LabelCSS" AutoPostBack="true">
                                        <asp:ListItem Value="D" Selected="True">DMAT</asp:ListItem>
                                        <asp:ListItem Value="S">SGL</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr align="left" id="row_SGL" runat="server">
                                <td>Our SGL With:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cbo_SGLWith" runat="server" CssClass="ComboBoxCSS" Width="200px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr align="left" id="row_Bank" runat="server">
                                <td>Our Bank:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cbo_Bank" runat="server" CssClass="ComboBoxCSS" Width="200px">
                                    </asp:DropDownList>
                                    <em><span style="color: Red; vertical-align: super;">*</span></em>
                                </td>
                            </tr>

                            <tr align="left">
                                <td>From Date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS jsdate" TabIndex="1"></asp:TextBox>
                                    <em>*</em>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Holding Cost %:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_HCAmount" runat="server" CssClass="TextBoxCSS" MaxLength="30"
                                        TabIndex="4"></asp:TextBox><span style="color: Red; vertical-align: super;"><em>*</em></span>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Intra Day %:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_IDAmount" runat="server" CssClass="TextBoxCSS" MaxLength="30"
                                        TabIndex="4"></asp:TextBox><span style="color: Red; vertical-align: super;"><em>*</em></span>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2"></td>
                            </tr>
                            <tr align="left">
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Update" Visible="false" runat="server" Text="Update" ToolTip="Update"
                                        CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:HiddenField ID="Hid_CompId" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
