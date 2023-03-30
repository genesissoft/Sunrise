<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="ClientTypeMaster.aspx.vb" Inherits="Forms_ClientTypeMaster" title="Client Type Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" src="../Include/Common.js"></script>

<script type="text/javascript" >
function Validation()
{
    if(document.getElementById("ctl00_ContentPlaceHolder1_txt_ClientType").value == "")
    {
        alert('Please Enter Client Type')
        return false;
    }
   
}
function ConvertUCase(txtBox)
{
    txtBox.value = txtBox.value.toUpperCase()   
}
</script>
  
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">
                Client Type Master </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
             <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td class="LabelCSS">
                            Client Type:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_ClientType" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox><span style="color: #ff0000"><em>*</em></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" class="SeperatorRowCSS">
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
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

