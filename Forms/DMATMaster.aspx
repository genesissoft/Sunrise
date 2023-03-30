<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="DMATMaster.aspx.vb" Inherits="Forms_DMATMaster" Title="Demat Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_DPName").value) == "") {
                AlertMessage('Validation', 'Please Enter DP Name.', 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_DPId").value) == "") {
                AlertMessage('Validation', 'Please Enter DPId.', 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_ClientId").value) == "") {
                AlertMessage('Validation', 'Please Enter ClientId.', 175, 450);
                return false;
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr align="left">
                    <td class="SectionHeaderCSS">Demat Master</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;</td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="45%">
                            <tr align="left">
                                <td>DP Name:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_DPName" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox><em><span
                                        style="color: Red; vertical-align: super;">*</span></em>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>DP Id:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_DPId" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox><em><span
                                        style="color: Red; vertical-align: super;">*</span></em></td>
                            </tr>
                            <tr align="left">
                                <td>Client Id:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_ClientId" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox><em><span
                                        style="color: Red; vertical-align: super;">*</span></em>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>CDSL/NSDL:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_CSDLCDSL" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Set As Default:
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDefault" runat="server" Checked="false" />
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr align="left">
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
