<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="DealerCategoryMaster.aspx.vb" Inherits="Forms_DealerCategoryMaster"
    Title="Dealer Category Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">
        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_CategoryName").value) == "") {
                AlertMessage('Validation', 'Please enter dealer category name.', 175, 450);
                return false;
            }
        }
    </script>
    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Dealer Category Master</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table width="40%" cellspacing="0" cellpadding="0" border="0" align="center">
                            <tr align="left">
                                <td>Category Name:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_CategoryName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="100"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2"></td>
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
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
