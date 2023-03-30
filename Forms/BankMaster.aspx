<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="BankMaster.aspx.vb" Inherits="Forms_BankMaster" Title="Bank Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_BankName").value) == "") {
                AlertMessage('Validation', 'Please Enter Bank Name.', 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Branch").value) == "") {
                AlertMessage('Validation', 'Please Enter Branch Name.', 175, 450);
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_AccountNumber").value) == "") {
                AlertMessage('Validation', 'Please Enter Account Number.', 175, 450);
                return false;
            }
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Bank Master</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table cellspacing="0" cellpadding="0" border="0" align="center" width="45%">
                            <tr align="left">
                                <td>Bank Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_BankName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="30"></asp:TextBox><span style="color: Red; vertical-align: super;"><em>*</em></span></td>
                            </tr>
                            <tr align="left">
                                <td>Branch:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Branch" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="30"></asp:TextBox><em><span
                                        style="color: Red; vertical-align: super;">*</span><span style="color: #ff0000"></span></em></td>
                            </tr>
                            <tr align="left">
                                <td>Location:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Location" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="30"></asp:TextBox><em><span style="color: Red; vertical-align: super;"></span></em></td>
                            </tr>
                            <tr align="left">
                                <td>IFSC Code:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_IFSCcode" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="30"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>RTGS Code:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_RTGSCode" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="30"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Account Number:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_AccountNumber" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="30"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                            </tr>

                            <tr align="left">
                                <td>Relationship Manager:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_ContactPerson" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="30"></asp:TextBox><em><span style="color: Red; vertical-align: super;"></span></em></td>
                            </tr>
                            <tr align="left">
                                <td>Phone:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_PhoneNo" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="30"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Mob No:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_MobNo" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="30"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Fax:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_FaxNo" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="30"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Email:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Email" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="30"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Set As Default:
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDefault" runat="server" Checked="false" />
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Update" Visible="false" runat="server" Text="Update" ToolTip="Update"
                                        CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS" />
                                </td>
                            </tr>
                            <asp:HiddenField ID="Hid_CompId" runat="server" />
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
