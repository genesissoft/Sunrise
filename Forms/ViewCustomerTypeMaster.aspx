<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ViewCustomerTypeMaster.aspx.vb" Inherits="Forms_ViewCustomerTypeMaster"
    Title="Customer Type Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">
                Customer Type Master</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td class="LabelCSS">
                            Customer Type:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_CustomerType" runat="server" CssClass="TextBoxCSS"   ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center"   colspan="2" >
                            <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                            <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
    </table>
</asp:Content>
