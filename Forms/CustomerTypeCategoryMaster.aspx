<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="CustomerTypeCategoryMaster.aspx.vb" Inherits="Forms_CustomerTypeCategoryMaster"
    Title="CustomerTypeCategory Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation() {

            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_CustTypCategory").value == "") {
                AlertMessage('Validation', 'Please Enter Segment Name', 175, 450);
                return false;
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table id="Table2" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td class="SectionHeaderCSS">Customer Type Category Master</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="45%">
                            <tr align="left">
                                <td>Category Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_CustTypCategory" runat="server" CssClass="TextBoxCSS" Width="200px"
                                        TabIndex="1"></asp:TextBox>
                                    <em><span style="color: Red; vertical-align: super;"></span></em>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2"></td>
                            </tr>
                            <tr align="left">
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" TabIndex="2" />
                                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" Visible="false"
                                        TabIndex="3" />
                                    <asp:Button ID="btn_Back" runat="server" CssClass="ButtonCSS" Text="Back" TabIndex="4" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
