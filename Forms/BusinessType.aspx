<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="BusinessType.aspx.vb" Inherits="Forms_BusinessType" Title="Business Type Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
function Validation()
{
    
    if((document.getElementById ("ctl00_ContentPlaceHolder1_txt_BusinessType").value)== "")
    { 
        alert('Please Enter Business Type');
        return false;   
    }
    

}
  function ConvertUCase(txtBox)
        {     
            txtBox.value = txtBox.value.toUpperCase()    
        }  


    </script>

    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
            Business Type Master
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="45%">
                            <tr align="left">
                                <td>
                                    Business Type:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_BusinessType" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox><em><span
                                        style="color: Red; vertical-align: super;">*</span></em>&nbsp;
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    &nbsp;</td>
                                <td>
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
