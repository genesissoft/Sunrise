<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectPurchaseMultipalDeals.aspx.vb" Inherits="Forms_SelectPurchaseMultipalDeals" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
     <table id="Table2" align="Center" width="100%" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">
                Delete Fax Quotes</td>
        </tr>
        <tr>
            <td>
                <table id="Table1" align="Center" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td align="center" valign="middle">
                            <div id="div2" style="margin-top: 0px; overflow: auto; width: 580px; padding-top: 0px;
                                position: relative; height: 350px" align="center">
                                <asp:DataGrid ID="dg_Quote" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                                    TabIndex="38" Width="520px">
                                    <HeaderStyle HorizontalAlign="Left" CssClass="GridHeaderCSS" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_Select" runat="server" Width="22px" />
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="QuoteName">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_QuoteName" runat="server" Width="50px" Text='<%# container.dataitem("QuoteName") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="SavedDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_SavedDate" runat="server" Width="50px" Text='<%# container.dataitem("SavedDate") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="FaxQuoteId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_FaxQuoteId" runat="server" Width="50px" Text='<%# container.dataitem("FaxQuoteId") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" class="SeperatorRowCSS">
                        </td>
                    </tr>
                    <tr><td align="center"> <asp:Label ID="lbl_Deleted" ForeColor="Blue"  runat="server"    CssClass="LabelCSS"></asp:Label>
                    </td></tr>
                    <tr>
                        <td align="center">
                            
                            <asp:Button ID="btn_Ok" runat="server" CssClass="ButtonCSS" Text="Delete" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    </div>
    </form>
</body>
</html>
