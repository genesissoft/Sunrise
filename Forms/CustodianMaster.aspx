<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="CustodianMaster.aspx.vb" Inherits="Forms_CustodianMaster" Title="Custodian Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_CustodianName").value) == "") {
                AlertMessage('Validation', "Please Enter Custodian Name", 175, 450);
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_email").value != "") {
                if (Email(document.getElementById("ctl00_ContentPlaceHolder1_txt_email").value) == false) {
                    AlertMessage('Validation', 'Enter Valid Email', 175, 450)
                    return false
                }
            }
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="SectionHeaderCSS">Custodian Master</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table id="Table2" width="45%" cellspacing="0" cellpadding="0" border="0" align="center">
                            <tr align="left">
                                <td>Custodian Name:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_CustodianName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="50" TabIndex="1"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                            </tr>
                            <tr align="left">
                                <td>Address1:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_address1" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="100" TabIndex="2"></asp:TextBox><em><span style="color: Red; vertical-align: super;"></span></em></td>
                            </tr>
                            <tr align="left">
                                <td>Address2:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_address2" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="100" TabIndex="3"></asp:TextBox><em><span style="color: Red; vertical-align: super;"></span></em></td>
                            </tr>
                            <tr align="left">
                                <td>City:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_city" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="20"
                                        TabIndex="4"></asp:TextBox><em><span style="color: Red; vertical-align: super;"></span></em></td>
                            </tr>
                            <tr align="left">
                                <td>Pin Code:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_pincode" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="7" TabIndex="5"></asp:TextBox><em><span style="color: Red; vertical-align: super;"></span></em></td>
                            </tr>
                            <tr align="left">
                                <td>Phone:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_phone1" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="50"
                                        TabIndex="6"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Fax:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Fax1" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="50"
                                        TabIndex="7"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Email:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_email" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="100"
                                        TabIndex="8"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Web Site:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Website" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="50" TabIndex="9"></asp:TextBox><em><span style="color: Red; vertical-align: super;"></span></em></td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr id="buttonid" runat="server">
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS"
                                        TabIndex="10" />
                                    <asp:Button ID="btn_Update" Visible="false" runat="server" Text="Update" ToolTip="Update"
                                        CssClass="ButtonCSS" TabIndex="10" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS"
                                        TabIndex="11" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
