<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="CheckerView.aspx.vb" Inherits="Forms_CheckerView" Title="Checker Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate >
            <table width="100%" >
                <tr align="left">
                    <td class="SectionHeaderCSS">
                       Checker View
                    </td>
                </tr>
                <tr class="line_separator">
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr align="center" >
                    <td align="center" >
                        <asp:DataGrid ID="grdChecker" runat="server" CssClass="GridCSS" AllowPaging="false"
                            AutoGenerateColumns="false" Width="90%">
                            <AlternatingItemStyle CssClass="GridRowCSS" />
                            <ItemStyle CssClass="GridRowCSS" />
                            <HeaderStyle CssClass="GridHeaderCSS" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="Check">
                                    <ItemTemplate>
                                        &nbsp;<asp:HyperLink ID="hyplnk" NavigateUrl='<%# DataBinder.EVal(Container,"DataItem.FormFileName") %>'
                                            runat="server" Text='<%# DataBinder.EVal(Container,"DataItem.CheckDesc") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="CheckType" HeaderText="CheckType" />
                                <asp:BoundColumn DataField="FormName" HeaderText="FormName" />
                                <asp:TemplateColumn Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFormFileName" runat="server" Text='<%# DataBinder.EVal(Container,"DataItem.FormFileName") %>'></asp:Label>
                                        <asp:Label ID="lblIDVal" runat="server" Text='<%# DataBinder.EVal(Container,"DataItem.IDVal") %>'></asp:Label>
                                        <asp:Label ID="lblPKColName" runat="server" Text='<%# DataBinder.EVal(Container,"DataItem.PKColName") %>'></asp:Label>
                                        <asp:Label ID="lblTableName" runat="server" Text='<%# DataBinder.EVal(Container,"DataItem.TableName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Authorize">
                                    <ItemTemplate>
                                        <asp:RadioButtonList ID="rbtType" runat="server" CssClass="LabelCSS" RepeatLayout="Flow"
                                            CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" AutoPostBack="true"
                                            Width="180" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                            <asp:ListItem Text="Accept" Value="Accept">Accept</asp:ListItem>
                                            <asp:ListItem Text="Reject" Value="Reject">Reject</asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Pending" Value="Pending">Pending</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Rejection Reason" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRejectionReason" CssClass="TextBoxCSS" MaxLength="255" Width="275"
                                            runat="server" Text=""></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </t>
                <tr>
                    <td>
                        &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="ButtonCSS" Text="Submit" />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:Label ID="lbl1" runat="server" Text=""></asp:Label>   
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
