<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="MakerView.aspx.vb" Inherits="Forms_MakerView" Title="View Rejected" %>

<%@ Register Assembly="MakerView" Namespace="MakerView.UserSecurity" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                View Rejections</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td align="center">
                            <div>
                                <cc1:MakerView ID="MakerView1" runat="server" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"
                                    GridCSSClass="GridCSS" RowCSSClass="GridRowCSS" HeaderCSSClass="GridHeaderCSS"
                                    ButtonCSSClass="ButtonCSS"></cc1:MakerView>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
