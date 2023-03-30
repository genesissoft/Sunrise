<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="DocumentMaster.aspx.vb" Inherits="Forms_DocumentMaster" Title="Document Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="SectionHeaderCSS">
                Document Master</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;</td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table align="center" cellspacing="0" width="35%" cellpadding="0" border="0">
                            <tr align="left">
                                <td>
                                    Document:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Document" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox><span
                                        style="color: Red; vertical-align: super;"><em>*</em></span>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Document Type:
                                </td>
                                <td align="left" style="padding-left: 0px;">
                                    <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DocType" runat="server" CellPadding="0"
                                        CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="True">
                                        <asp:ListItem Selected="true" Value="E"> Empalement </asp:ListItem>
                                        <asp:ListItem Value="K"> KYC </asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr align="left">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
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
