<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SecurityCategoryMaster.aspx.vb" Inherits="Forms_SecurityCategoryMaster"
    Title="Security Category Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation() {
            if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType").value) == "") {
                AlertMessage('Validation', 'Please Select Security Type', 175, 450);
                return false;
            }

            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_SecurityCategory").value) == "") {
                AlertMessage('Validation', 'Please Enter Category Name', 175, 450);
                return false;
            }


        }
        function ConvertUCase(txtBox) {
            txtBox.value = txtBox.value.toUpperCase()
        }


    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="SectionHeaderCSS">Category Master
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table id="Table2" align="center" cellspacing="0" cellpadding="0" border="0" width="45%">
                            <tr align="left">
                                <td>Security Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cbo_SecurityType" Width="208px" runat="server" CssClass="ComboBoxCSS"
                                        AutoPostBack="false" TabIndex="1">
                                    </asp:DropDownList>
                                    <i style="color: red; vertical-align: super;">*</i>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Category Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_SecurityCategory" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox><em><span
                                        style="color: red; vertical-align: super;">*</span></em>
                                </td>
                            </tr>

                            <tr>
                                <td class="SeperatorRowCSS" colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
