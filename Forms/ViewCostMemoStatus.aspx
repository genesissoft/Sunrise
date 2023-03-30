<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ViewCostMemoStatus.aspx.vb" Inherits="Forms_ViewCostMemoStatus" Title="View Cost Memo Status" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Cost Memo Status</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top" runat="server" visible="false">
            <td>
                <table width="35%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr align="left">
                        <td>
                            For Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_ForDate" runat="server" CssClass="TextBoxCSS" Width="115px"></asp:TextBox><img
                                class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ForDate',this);"
                                id="IMG2" />
                        </td>
                        <td>
                            <asp:Button ID="btn_Show" runat="server" CssClass="ButtonCSS" Text="Show" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <div id="div1" style="margin-top: 0px; overflow: auto; width: 75%; padding-top: 0px;
                            position: relative; height: 350px">
                            <asp:DataGrid ID="dg_dme" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="False"
                                TabIndex="38" Width="100%" PageSize="15" AllowSorting="True">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <Columns>
                                    <%--<asp:TemplateColumn HeaderText="Security Name" SortExpression="SecurityName">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_SecurityName" Width="150px" runat="server" Style="border-left-width: 0;
                                                            border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                            Text='<%# container.dataitem("SecurityName") %>' CssClass="TextBoxCSS"></asp:TextBox>

                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>--%>
                                    <asp:TemplateColumn HeaderText="Cost Memo No" SortExpression="CostMemoNo">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_CostMemoNo" Width="80px" runat="server" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                Text='<%# container.dataitem("CostMemoNo") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Security Name" SortExpression="SecurityName">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_SecurityName" Width="300px" runat="server" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                Text='<%# container.dataitem("SecurityName") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Face Value" SortExpression="FaceValue">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_FaceValue" Width="90px" runat="server" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                Text='<%# container.dataitem("FaceValue") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status" SortExpression="Status">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Status" Width="90px" runat="server" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                Text='<%# container.dataitem("Status") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="DealSlipNo" SortExpression="DealSlipNo">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_DealSlipNo" Width="80px" runat="server" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                Text='<%# container.dataitem("DealSlipNo") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="SecurityId" SortExpression="SecurityId" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_SecurityId" Width="100px" runat="server" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                Text='<%# container.dataitem("SecurityId") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                        <%--  <td align="center">
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                                </td>--%>
                        <asp:HiddenField ID="Hid_ForDate" runat="server" />
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
