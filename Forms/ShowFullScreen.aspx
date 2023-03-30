<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShowFullScreen.aspx.vb"
    Inherits="Forms_ShowFullScreen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_blank" />
    <title>Show All Details</title>
    <link href="../Include/Style_New.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0" width="100%" border="0" align="center" class="data_table">
            <tr>
                <td class="formHeader">
                    Show All Details:
                </td>
            </tr>
            <tr runat="server" id="row_DealRpt" visible="false">
                <td align="center" width="100%">
                    <asp:DataGrid ID="dg_DealRpt" AllowPaging="false" runat="server" Width="95%" ShowFooter="True"
                        AutoGenerateColumns="false" CssClass="table_border_right_bottom">
                        <HeaderStyle HorizontalAlign="Center" Wrap="false" CssClass="table_heading" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="top" BackColor="#FFEFD5" />
                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Position="TopAndBottom" Font-Size="1.3em"
                            Mode="NumericPages" />
                        <FooterStyle Font-Bold="true" HorizontalAlign="Left" />
                        <Columns>
                            
                            <asp:TemplateColumn HeaderText="Customer Name" FooterText="Total">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CustomerName" Visible ="false" runat="server" Width="190px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.customername") %>'></asp:Label>
                                    <asp:Label ID="lbl_UserName" Visible ="false"  runat="server" Width="320px" Text='<%# DataBinder.Eval(Container, "DataItem.NameOfUser") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="CP in Crs.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CPAmt" runat="server" Width="35px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.CP") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_TotCPAmt" runat="server" Width="35px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.TotalCP") %>'></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="CD in Crs.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CDAmt" runat="server" Width="35px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.CD") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_TotCDAmt" runat="server" Width="35px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.TotCD") %>'></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="SLR in Crs.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SLRAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.SLR") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_TotSLRAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.TotSLR") %>'></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="NONSLR in Crs.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_NONSLRAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.NON-SLR") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_TotNONSLRAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.TotNONSLR") %>'></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn HeaderText="Total in Crs.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Total" runat="server" Width="70px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Total") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_GrandTotal" runat="server" Width="70px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.GrandTotal") %>'></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <%--<asp:TemplateColumn HeaderText="Match" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Match" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Match") %>'></asp:Label>
                                    <asp:Label ID="lbl_MMC" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.MMC") %>'></asp:Label>
                                    <asp:Label ID="lbl_SLRC" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.SLRC") %>'></asp:Label>
                                    <asp:Label ID="lbl_NONSLRC" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.NONSLRC") %>'></asp:Label>
                                    <asp:Label ID="lbl_CPPVTC" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.CPPVTC") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>--%>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
           
            <tr runat="server" id="row_MeetingRpt" visible="false">
                <td align="center" width="100%">
                    <asp:DataGrid ID="dg_MeetingRpt" AllowPaging="false" runat="server" Width="95%" ShowFooter="false"
                        AutoGenerateColumns="false" CssClass="table_border_right_bottom">
                        <HeaderStyle HorizontalAlign="Center" Wrap="false" CssClass="table_heading" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="top" BackColor="#FFEFD5" />
                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Position="TopAndBottom" Font-Size="1.3em"
                            Mode="NumericPages" />
                        <Columns>
                            
                            <asp:TemplateColumn HeaderText="Meeting Date">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_MeetingDate" runat="server" Width="30px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.entrydate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Contact Person">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Contact" runat="server" Width="100px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Contacts") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Customer Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_MeetCustomerName" runat="server" Width="150px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Client") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Meeting Summary">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_MeetingSummary" runat="server" Width="450px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Remark") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
