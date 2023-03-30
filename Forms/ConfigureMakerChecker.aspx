<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ConfigureMakerChecker.aspx.vb" Inherits="Forms_ConfigureMakerChecker"
    Title="Maker Checker" %>

<%@ Register Assembly="MakerChecker" Namespace="MakerChecker.UserSecurity" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">
                Maker Checker</td>
        </tr>
        <tr>
            <td colspan="6" class="SeperatorRowCSS">
            </td>
        </tr>
        <tr>
            <td align="center" >
                <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td align ="center" >
                            <div>
                                <cc1:MakerChecker ID="MakerChecker1" runat="server" GridCSSClass="GridCSS" RowCSSClass="GridRowCSS"
                                    HeaderCSSClass="GridHeaderCSS" ButtonCSSClass="ButtonCSS" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"></cc1:MakerChecker>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
