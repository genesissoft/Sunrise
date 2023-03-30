<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ConfigurePassword.aspx.vb" Inherits="Forms_ConfigurePassword" Title="Untitled Page" %>

<%@ Register Assembly="Configuration" Namespace="Configuration.UserSecurity" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">
                Configure Password</td>
        </tr>
        <tr>
            <td colspan="6" class="SeperatorRowCSS">
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table2" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td align="center">
                            <div>
                                <cc1:Configuration ID="Configuration1" runat="server" LabelCSSClass="LabelCSS" TextBoxCSSClass="TextBoxCSS"
                                    ComboBoxCSSClass="ComboBoxCSS" ButtonCSSClass="ButtonCSS" MessageCSSClass="MessageCSS"></cc1:Configuration>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
