<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SecurityTypeMaster.aspx.vb" Inherits="Forms_SecurityTypeMaster" Title="Security Type Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation()
        {
            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_SecurityTypeName").value) == "")
            {
                AlertMessage('Validation',"Please Enter Security Type Name",175,450);
                return false;
            }
            

        }
    </script>

    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Security Type Master</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table width="50%" cellspacing="0" cellpadding="0" border="0" align="center">
                            <tr align="left">
                                <td>
                                    Security Type Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_SecurityTypeName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="100"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Security Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cbo_SecurityType" runat="server" CssClass="ComboBoxCSS" Height="19px"
                                        Width="208px">
                                        <asp:ListItem Value="G">GOVERNMENT(360)</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="N">NON GOVERNMENT(365)</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Abbreviation:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Abbreviation" runat="server" CssClass="TextBoxCSS" MaxLength="3"></asp:TextBox><em><span
                                        style="color: Red; vertical-align: super;"></span></em></td>
                            </tr>
                              <tr align="left">
                                <td>
                                    Order Id:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_OrderId" runat="server" CssClass="TextBoxCSS" MaxLength="6" onkeypress="javascript:return OnlyNumeric();"></asp:TextBox><em><span
                                        style="color: Red; vertical-align: super;"></span></em></td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    &nbsp;
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
            </td>
        </tr>
    </table>
</asp:Content>
