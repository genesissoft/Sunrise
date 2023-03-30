<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="CancelDealMaster.aspx.vb" Inherits="Forms_CancelDealMaster" Title="Document Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">
                Deal Cancellation Master</td>
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
                            Deal Code:
                        </td>
                        <td>
                            <asp:DropDownList ID="cbo_DealCode" runat="server" CssClass="TextBoxCSS" Width="142px"
                                TabIndex="1">
                            </asp:DropDownList>
                            <asp:Button ID="cbo_SearchDealCode" runat="server" CssClass="ButtonCSS" Text="Save" /></td>
                    </tr>
                    <tr>
                        <td class="LabelCSS">
                            Security Issuer:
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_SecurityIssuer" runat="server" class="LabelCSS" Text="BANK OF INDIA"
                                Width="90px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="LabelCSS">
                            Security Name:
                        </td>
                         <td align="left">
                            <asp:Label ID="lbl_SecurityName" runat="server" class="LabelCSS" Text="10% BOI 2010"
                                Width="90px"></asp:Label></td>
                        
                    </tr>
                    <tr>
                        <td class="LabelCSS">
                            Deal Date:
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_DealDate" runat="server" class="LabelCSS" Text="15/05/2008"
                                Width="90px"></asp:Label></td>
                       
                    </tr>
                     <tr>
                        <td class="LabelCSS">
                            Face Value:
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_FaceValue" runat="server" class="LabelCSS" Text="1,00,00,000.00"
                                Width="90px"></asp:Label></td>
                       
                    </tr>
                     <tr>
                        <td class="LabelCSS">
                            Reason Of Cancellation:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_Reason" runat="server" Height="32px" TextMode="MultiLine" Width="425px"
                               CssClass="TextBoxCSS" MaxLength="20"></asp:TextBox>
                       
                    </tr>
                    <tr>
                        <td colspan="6" class="SeperatorRowCSS">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                            <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
