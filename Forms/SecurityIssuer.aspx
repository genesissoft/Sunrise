<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SecurityIssuer.aspx.vb" Inherits="Forms_SecurityIssuer" Title="Security Issuer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
    
        function Validation()
        {
      
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_IssuerName").value) == "")
            {
                AlertMessage('Validation','Please Enter Issuer Name',175,450);
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
                                            <td class="LabelCSS">Issuer Name:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_IssuerName" runat="server" CssClass="TextBoxCSS" Width="150px"
                                                    TabIndex="2"></asp:TextBox></td>
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
