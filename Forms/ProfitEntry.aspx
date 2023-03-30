<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ProfitEntry.aspx.vb" Inherits="Forms_ProfitEntry" Title="Profit Entry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">
   function Validation()
   {
        var grd = document.getElementById("ctl00_ContentPlaceHolder1_dgProfit")
        if (grd != null)
        {
//        alert(grd.rows.length)
            if(grd.rows.length == 1)
            {
                alert("No Records Available for Change Profit")
                return false
            }
       }  
   
   }
   function Redirect(strDealNo)
   {
        var strDealId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value

        alert('Profit Change Successfully of Deal Slip No.  ' + strDealNo)
        if(strDealId != "")
        {
            window.location = "ShowProfitEntry.aspx"
        }    
   }
    </script>

    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Profit Entry</td>
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
                        <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
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
                                                <%-- <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit" />
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                                        CommandName="Delete" />
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>--%>
                                                <asp:TemplateColumn HeaderText="Sell Branch">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SellBranch" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SellBranch") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SellDealNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SellDealSlipNo" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container, "DataItem.SellDealSlipNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SellFV">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SellFV" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SellFV") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Sell Profit(%)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_SellProfit" CssClass="TextBoxCSS" Width="120px" onkeypress="javascript: OnlyDecimal();"
                                                            runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SellProfitPercent") %>'> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Pur Branch">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_PurchaseBranch" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PurchaseBranch") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="PurDealNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_PurDealSlipNo" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PurDealSlipNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="PurFV">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_PurFV" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PurchaseFV") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Pur Profit(%)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_PurProfit" CssClass="TextBoxCSS" Width="120px" onkeypress="javascript: OnlyDecimal();"
                                                            runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PurcProfitPercent") %>'> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Ho Profit(%)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_HOProfit" CssClass="TextBoxCSS" Width="120px" onkeypress="javascript: OnlyDecimal();"
                                                            runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.HOProfitPercent") %>'> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SellDealSlipId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SellDealSlipId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SellDealSlipId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="purDealSlipId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_purDealSlipId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.purDealSlipId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="TotalProfit" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_TotalProfit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TotalProfit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <%--<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="White" BorderStyle="Solid"
                                                            BorderColor="Black" NextPageText="Next" PrevPageText="Previous"></PagerStyle>--%>
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="row_Save" runat="server">
                                <td align="center">
                                    <asp:Button ID="btn_ChangeProfit" runat="server" Text="Change Profit" ToolTip="Save"
                                        CssClass="ButtonCSS" Width="90px" />
                                    <%--<asp:Button ID="btn_Update" runat="server" Text="Update" ToolTip="Update" CssClass="ButtonCSS"
                                        Height="20px" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS"
                                        Height="20px" />--%>
                                </td>
                            </tr>
                            <asp:HiddenField ID="New" runat="server" />
                            <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                            <asp:HiddenField ID="Hid_SellPercent" runat="server" />
                            <asp:HiddenField ID="Hid_DealSlipNo" runat="server" />
                        </table>
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
