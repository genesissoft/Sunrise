<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="DeleteFaxQuotes.aspx.vb" Inherits="From_DeleteFaxQuotes" Title="Delete Fax Quotes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table id="Table2" align="Center" width="100%" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Delete Fax Quotes</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table id="Table1" align="Center" cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tr>
                        <td align="center" valign="middle">
                            <div id="div2" style="margin-top: 0px; overflow: auto; width: 75%; padding-top: 0px;
                                position: relative;" align="center">
                                <asp:DataGrid ID="dg_Quote" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                                    TabIndex="38" Width="100%">
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
                                            <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="SavedDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_SavedDate" runat="server" Width="50px" Text='<%# container.dataitem("SavedDate") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="FaxQuoteId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_FaxQuoteId" runat="server" Width="50px" Text='<%# container.dataitem("FaxQuoteId") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </td>
                    </tr>
                    <tr class="line_separator">
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Deleted" ForeColor="Blue" runat="server" CssClass="LabelCSS"></asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Button ID="btn_Ok" runat="server" CssClass="ButtonCSS" Text="Delete" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
