<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="StateMaster.aspx.vb" Inherits="Forms_StateMaster" title="State Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation()
        {
            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_StateName").value) == "")
            {
                AlertMessage('Validation', "Please Enter State Name",175,450);
                return false;
            }
            


            if((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServTax").value) == "")
            {
                AlertMessage('Validation', "Please Enter Stamp Duty",175,450);
                return false;
            }
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">
               State Master</td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table id="Table3" width="50%" cellspacing="0" cellpadding="0" border="0" align="center">
                            <tr>
                                <td colspan="6" class="SeperatorRowCSS">
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    State Name:
                                </td>
                                <td align="left">
                                    &nbsp;<asp:TextBox ID="txt_StateName" runat="server" Width="250px" CssClass="TextBoxCSS"
                                        MaxLength="100"></asp:TextBox><em><span style="color: #ff0000">*</span></em></td>
                            </tr>
                          
                            <tr>
                                <td class="LabelCSS">
                                    Stamp Duty:
                                </td>
                                <td align="left">
                                    &nbsp;<asp:TextBox ID="txt_ServTax" runat="server" Width="100px" CssClass="TextBoxCSS"
                                       ></asp:TextBox><em><span style="color: #ff0000">*</span></em></td>
                            </tr>
                          
                            <tr>
                                <td colspan="6" class="SeperatorRowCSS">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS"
                                        Height="20px" />
                                    <asp:Button ID="btn_Update" Visible="false" runat="server" Text="Update" ToolTip="Update"
                                        CssClass="ButtonCSS" Height="20px" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS"
                                        Height="20px" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

