<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="UserTypeMaster.aspx.vb" Inherits="Forms_UserTypeMaster" Title="User Type Master" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function CheckBranch() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_Checker_1").checked == true) {
                document.getElementById("row_Branch").style.display = "none";
            }
            else {
                document.getElementById("row_Branch").style.display = "none";
            }
        }


        function CommonValidation() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_UserType").value == "") {
                AlertMessage('Validation', 'Please enter User Type Name.', 175, 450);
                return false;
            }
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr align="left">
                    <td class="SectionHeaderCSS">User Type Master</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="50%">
                            <tr align="left">
                                <td>User Type:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_UserType" runat="server" CssClass="TextBoxCSS" MaxLength="30"
                                        TabIndex="1" Width="200px"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                </td>
                            </tr>

                            <tr align="left">
                                <td>User Type Section:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList ID="rbl_UserTypeSection" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" CssClass="LabelCSS" Height="4px" TabIndex="2">
                                        <asp:ListItem Selected="True" Value="B">Back-Office</asp:ListItem>
                                        <asp:ListItem Value="F">Front-Office</asp:ListItem>
                                        <asp:ListItem Value="O">Both</asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                            <tr align="left">
                                <td>Checker:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList ID="rdo_Checker" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" CssClass="LabelCSS" Height="4px" TabIndex="2">
                                        <asp:ListItem Selected="True" Value="T">Yes</asp:ListItem>
                                        <asp:ListItem Value="F">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>

                              <tr align="left">
                                <td>Restricted Access:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList ID="rdo_RestrictedAccess" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" CssClass="LabelCSS" Height="4px" TabIndex="2">
                                        <asp:ListItem  Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>

                            <tr align="left" id="row_Branch"  style ="display:none;">
                                <td>For Branch:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList ID="rdo_Branch" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" CssClass="LabelCSS" Height="4px" TabIndex="2">
                                        <asp:ListItem Selected="True" Value="A">All</asp:ListItem>
                                        <asp:ListItem Value="C">Current</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr align="left" runat="server" visible="false">
                                <td>Business Type:</td>
                                <td style="padding-left: 0px;">
                                    <uc:SelectFields ID="srh_BusinessType" class="LabelCSS" runat="server" ProcName="ID_SEARCH_BusinessTypeMaster"
                                        FormHeight="470" FormWidth="257" SelectedValueName="BusinessTypeId" ChkLabelName=""
                                        ConditionalFieldId="" LabelName="" SelectedFieldName="BusinessType" SourceType="StoredProcedure"
                                        ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:HyperLink ID="lnk_MerchantBanking" runat="server" Font-Bold="True" ForeColor="Blue">Merchant Banking</asp:HyperLink>&nbsp;&nbsp;
                                    <asp:HyperLink ID="lnk_PMS" runat="server" Font-Bold="True" ForeColor="Blue">PMS</asp:HyperLink>&nbsp;&nbsp;
                                    <asp:HyperLink ID="lnk_WDM" runat="server" Font-Bold="True" ForeColor="Blue">WDM/RETAIL</asp:HyperLink>
                                </td>
                            </tr>
                            <%-- <tr>
                        <td align="right" class="LabelCSS">
                            User Type Section:
                        </td>
                        <td>
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                RepeatLayout="Table" CssClass="LabelCSS" Height="4px" TabIndex="2">
                                <asp:ListItem Selected="True" Value="B">Back-Office</asp:ListItem>
                                <asp:ListItem Value="F">Front-Office</asp:ListItem>
                                <asp:ListItem Value="O">Both</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>--%>
                        </table>
                    </td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center">
                    <td class="HeadingCenter">Access Levels Section</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr valign="top" align="center">
                    <td id="td2" runat="server">
                        <div id="div_Access" style="margin-top: 0px; overflow: auto; padding-top: 0px; position: relative; width:500px; height: 250px;">
                            <asp:DataGrid ID="gv_Access" runat="server" AutoGenerateColumns="false" CssClass="GridCSS"
                                AllowPaging="false" AllowSorting="false" PageSize="10" Width="500px">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <ItemStyle HorizontalAlign="left" CssClass="GridRowCSS table_border_none" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Form Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Name" CssClass="LabelCSS" runat="server" Width="200px" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="URL" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_URL" CssClass="LabelCSS" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.Url") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Menu ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_MenuID" CssClass="LabelCSS" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.MenuId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Access"  >
                                        <ItemTemplate>
                                            <asp:RadioButtonList ID="rdo_Incetive1" runat="server" RepeatColumns="3" RepeatDirection="Vertical"
                                                RepeatLayout="Table" CssClass="LabelCSS" TabIndex="18" AutoPostBack="False" Text='<%# DataBinder.Eval(Container, "DataItem.UserAccess") %>'>
                                                <asp:ListItem Value="e">Enable</asp:ListItem>
                                                <asp:ListItem Value="d">Disable</asp:ListItem>
                                                <asp:ListItem Value="r">Restricted</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center">
                    <td>
                        <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                        <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                        <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                    </td>
                </tr>
                <asp:HiddenField ID="Hid_BusinessTypeId" runat="server" />
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
