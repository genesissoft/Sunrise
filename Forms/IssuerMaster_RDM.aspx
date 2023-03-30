<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="IssuerMaster_RDM.aspx.vb" Inherits="Forms_IssuerMaster_RDM" Title="Issuer master RDM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">

        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_IssuerType").value) == "") {
                AlertMessage('Validation', 'Please Select Issuer Type', 175, 450);
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_IssuerName").value) == "") {
                AlertMessage('Validation', 'Please Enter Issuer Name', 175, 450);
                return false;
            }
        }
    </script>

    <table id="Table2" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">Issuer Master</td>
        </tr>
        <tr>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                <ContentTemplate>
                    <div>
                        <table id="Table1" width="45%" align="center" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td>
                                    <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td colspan="6" class="SeperatorRowCSS"></td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Issuer Type:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cbo_IssuerType" Width="182px" runat="server" CssClass="ComboBoxCSS"
                                                    TabIndex="1">
                                                </asp:DropDownList><em><span style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Issuer Name:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_IssuerName" runat="server" CssClass="TextBoxCSS" Width="300px"
                                                    TabIndex="2"></asp:TextBox></td>
                                        </tr>

                                        <tr>
                                            <td class="LabelCSS">Hide:</td>
                                            <td align="left">
                                                <asp:RadioButtonList ID="rdo_HideShow" runat="server" BorderStyle="None" BorderWidth="1px"
                                                    CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="true">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
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
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </tr>
    </table>
</asp:Content>
