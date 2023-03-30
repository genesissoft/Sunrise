<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SGLMaster.aspx.vb" Inherits="Forms_SGLMaster" Title="SGL Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
function Validation()
{

    if((document.getElementById ("ctl00_ContentPlaceHolder1_txt_SGLBankName").value)== "")
    { 
        AlertMessage('Validation', 'Please Enter SGL Bank Name.', 175, 450);
        return false;   
    }
    

}
  function ConvertUCase(txtBox)
        {     
            txtBox.value = txtBox.value.toUpperCase()    
        }  


    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table width="80%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr align="left">
                    <td class="SectionHeaderCSS" colspan="2">
                        SGL Master
                    </td>
                </tr>
                <tr class="line_separator">
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                            <tr align="left">
                                <td>
                                    SGL Bank Name:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_SGLBankName" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox><em><span
                                        style="color: Red; vertical-align: super;">*</span></em>&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    SGL Branch:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_SGLBranch" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Account No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_AccountNo" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Gilt A/c Number:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_GiltAccNo" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="30"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>
                                    RTGS Code:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_RTGSCode" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="30"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Address1:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Address1" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Address2:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Address2" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                            <tr>
                                <td>
                                    <tr align="left">
                                        <td>
                                            City:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_City" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            Pincode:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_Pincode" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            Contact Person:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ContactPerson" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            Fax No:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_FaxNo" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            Phone No:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_PhoneNo" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            Moblie No:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_MoblieNo" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            Email Id:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_EmailId" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox>
                                        </td>
                                    </tr>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="center">
                        <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                        <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                        <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
