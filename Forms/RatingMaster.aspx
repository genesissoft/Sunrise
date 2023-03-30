<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="RatingMaster.aspx.vb" Inherits="Forms_RatingMaster" Title="Rating Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>
    
     <script type="text/javascript">
             
       
       function Validation()
       {
          if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_RatingOrganization").value) == "")
            {  
                AlertMessage('Validation',"Please Select Rating Organisation.",175,450);
                return false;
             }   
                 
            if(document.getElementById("ctl00_ContentPlaceHolder1_txt_Rating").value == "")
            {
                AlertMessage('Validation', 'Please Enter Rating.',175,450)
                return false;
            }
       }
           
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr align="left">
                    <td class="SectionHeaderCSS">
                        Rating Master</td>
                </tr>
                <tr class="line_separator">
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="40%">
                            <tr align="left">
                                <td>
                                    Rating Organization:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cbo_RatingOrganization" Width="208px" runat="server" CssClass="ComboBoxCSS"
                                        TabIndex="1">
                                    </asp:DropDownList><i style="color: Red; vertical-align: super;">*</i>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Rating:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Rating" Width="200px" runat="server" CssClass="TextBoxCSS" TabIndex="2"></asp:TextBox><i
                                        style="color: Red; vertical-align: super;">*</i>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
