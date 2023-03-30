<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ServiceTaxMaster.aspx.vb" Inherits="Forms_ServiceTaxMaster" Title="Service Tax Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_FromDate").value) == "") {
                AlertMessage('Validation', "Please select From Date", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_ServTax").value) == "") {
                AlertMessage('Validation', "Please enter service tax", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_ECess").value) == "") {
                AlertMessage('Validation', "Please enter ECess", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Cess").value) == "") {
                AlertMessage('Validation', "Please enter Cess", 175, 450);
                return false;
            }

            var Ecess = document.getElementById("ctl00_ContentPlaceHolder1_txt_ECess").value - 0
            var ST = document.getElementById("ctl00_ContentPlaceHolder1_txt_ServTax").value - 0
            var HECess = document.getElementById("ctl00_ContentPlaceHolder1_txt_Cess").value - 0
            if (ST < 0 || (isNaN(ST) == true)) {
                AlertMessage('Validation', 'Invalid Service Tax Entry!', 175, 450)
                return false;
            }
            if (Ecess < 0 || isNaN(Ecess)) {
                AlertMessage('Validation', 'Invalid Ecess Entry !', 175, 450)
                return false;
            }
            if (HECess < 0 || isNaN(HECess)) {
                AlertMessage('Validation', 'Invalid cess Entry!', 175, 450)
                return false;
            }

            return true

        }


    </script>

    <table id="table1" cellspacing="0" border="0" align="center" width="100%" cellpadding="0">
        <tr>
            <td class="HeaderCSS" align="center" colspan="4">Service Tax Master
            </td>
        </tr>
        <tr>
            <td colspan="4">&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table id="table2" cellpadding="0" cellspacing="0" align="center">
                            <tr>
                                <td class="LabelCSS">From Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS jsdate" Width="100px"
                                        MaxLength="100" TabIndex="1"></asp:TextBox>
                                    <em><span
                                        style="color: #ff0000">*</span></em>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">Service Tax:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_ServTax" runat="server" CssClass="TextBoxCSS" Width="100px"
                                        MaxLength="100" TabIndex="2"></asp:TextBox><em><span style="color: #ff0000">*</span></em>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">E Cess:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_ECess" runat="server" CssClass="TextBoxCSS" Width="100px" MaxLength="100"
                                        TabIndex="2"></asp:TextBox><em><span style="color: #ff0000">*</span></em>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">Cess :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Cess" runat="server" CssClass="TextBoxCSS" Width="100px" MaxLength="100"
                                        TabIndex="2"></asp:TextBox><em><span style="color: #ff0000">*</span></em>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">SBCess :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_SBCess" runat="server" CssClass="TextBoxCSS" Width="100px" MaxLength="100"
                                        TabIndex="2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="SeperatorRowCSS" colspan="4"></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" TabIndex="3" />
                                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" TabIndex="3" />
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" TabIndex="4" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
