<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ShowProfitEntry.aspx.vb" Inherits="Forms_ShowProfitEntry" Title="Show Profit Entry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">
   function Update(intIndex,strSellDealSlipId)
   {
        window.location ="ProfitEntry.aspx?SellDealSlipId=" + strSellDealSlipId ;
   }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Show Profit Entry</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td align="center" valign="top">
                <atlas:UpdatePanel ID="UpdatePanel5" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                            <tr id="row_Grid" runat="server">
                                <td align="center" valign="top">
                                    <div id="divdgbill" style="margin-top: 0px; overflow: auto; width: 98%; padding-top: 0px;
                                        position: relative;" align="center">
                                        <asp:DataGrid ID="dgProfit" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                                            Width="100%" CssClass="GridCSS">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                            <FooterStyle HorizontalAlign="Center" CssClass="footer" VerticalAlign="Middle"></FooterStyle>
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit" />
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SellDealNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SellDealSlipNo" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.SellDealSlipNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Security Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SecurityName" Width="260px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Deal Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealDate" Width="60px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DealDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SettDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SettDate" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SellSettDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="FaceValue">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_FaceValue" Width="100px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SellFaceValue") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="TotalProfit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_TotalProfit" idth="80px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TotalProfit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SellDealSlipId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SellDealSlipId" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SellDealSlipId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="White" BorderStyle="Solid"
                                                BorderColor="Black" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                            <asp:HiddenField ID="Hid_SellDealSlipId" runat="server" />
                            <asp:HiddenField ID="Hid_Index" runat="server" />
                        </table>
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
