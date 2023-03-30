<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ViewDealStock.aspx.vb" Inherits="Forms_ViewDealStock" Title="View Deal Stock" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function SelectOption(img, id) {
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_SecurityId").value = id


            var row = img.parentElement.parentElement
            UnselectAll(row)
            img.src = "../Images/images.JPG"
            row.style.backgroundColor = '#D1E4F8'
            row.children[1].children[0].style.backgroundColor = "#D1E4F8"
            row.children[2].children[0].style.backgroundColor = "#D1E4F8"
            row.children[3].children[0].style.backgroundColor = "#D1E4F8"
            ShowStockInfo()


        }
        function UnselectAll(row) {
            var grd = row.parentElement.parentElement
            for (i = 1; i <= (grd.children[0].children.length - 2) ; i++) {
                currRow = grd.children[0].children[i]
                currRow.children[0].children[0].src = "../Images/images3.JPG"

            }
        }


        function ShowStockInfo() {
            var Id = document.getElementById("ctl00_ContentPlaceHolder1_Hid_SecurityId").value;
            //alert(Id)
            ShowDialogCRPopUp('StockInfo.aspx', Id, 400, 200)
            return false;

        }

        function ShowDialogCRPopUp(PageName, Id, Width, Height) {
            var w = Width;
            var h = Height;
            var winl = (screen.width - w) / 2;
            var wint = (screen.height - h) / 2;
            if (winl < 0) winl = 0;
            if (wint < 0) wint = 0;

            PageName = PageName + "?Id=" + Id
            windowprops = "height=" + h + ",width=" + w + ",top=" + wint + ",left=" + winl + ",location=no,"
            + "scrollbars=yes,menubars=yes,toolbars=yes,resizable=no,status=yes";
            window.open(PageName, "Popup", windowprops);
        }
        function Validation() {

            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_Security").value == "") {
                alert('Please Enter Security Name');
                return false;
            }

        }

        function NoEntriesDone() {
            AlertMessage("Validation", "No entries done.", 175, 450);
        }

        function NoRecordsFound() {
            AlertMessage("Validation", "Sorry!!! No Records available to show report.", 175, 450);
        }
    </script>

    <%--<atlas:UpdatePanel ID="UpdatePanel2" runat="server" Mode="Conditional">
                    <ContentTemplate>--%>
    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Deal Summary Report
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table width="40%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr align="left">
                        <td>For Date:
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr align="left">
                                    <td>
                                        <asp:TextBox ID="txt_ForDate" runat="server" CssClass="TextBoxCSS" Width="115px"></asp:TextBox><img
                                            class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ForDate',this);"
                                            id="IMG2">
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_Show" runat="server" CssClass="ButtonCSS" Text="Show" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%--  <tr>
                        <td class="LabelRightCSS">
                            Security Name:
                        </td>
                        <td align="left" style="width: 157px; height: 21px;">
                            <asp:TextBox ID="txt_Security" runat="server" CssClass="TextBoxCSS" Width="123px"></asp:TextBox>&nbsp;
                        </td>
                    </tr>--%>
                    <tr align="left">
                        <td>Security:
                        </td>
                        <td style="padding-left: 0px;">
                            <%-- <uc:Search ID="Srh_security" runat="server" AutoPostback="true" ProcName="ID_SEARCH_SecurityMaster"
                                SelectedFieldName="SecurityName" SourceType="StoredProcedure" TableName="SecurityMaster"
                                ConditionalFieldName=" " ConditionalFieldId=" " Width="200" ConditionExist="false"
                                FormHeight="400" FormWidth="600"></uc:Search>--%>
                            <uc:Search ID="Srh_security" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                SelectedFieldId="Id" SelectedFieldName="SecurityName" />
                        </td>
                    </tr>
                    <tr align="left">
                        <td>Interest:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Interest" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btn_ShowSecurity" runat="server" CssClass="ButtonCSS" Text="Show" />
                            <asp:Button ID="btn_ShowSecurityall" runat="server" CssClass="ButtonCSS" Text="Show All" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center">
            <td class="HeadingCenter">Trading Stock
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Button ID="btn_Print" runat="server" CssClass="ButtonCSS" Text="Export" />
                &nbsp;<asp:Button ID="btn_ViewTrad" runat="server" CssClass="ButtonCSS" Text="Print" Visible="false" />
                <asp:Button ID="btn_Segment" runat="server" CssClass="ButtonCSS" Text="Segment" Visible="false" />
            </td>

        </tr>
        <tr align="center" valign="top">
            <td>
                <div id="div1" style="margin-top: 0px; overflow: auto; width: 1000px; padding-top: 0px; position: relative;">
                    <asp:DataGrid ID="dg_dme" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="False"
                        TabIndex="38" Width="1000px" PageSize="15" AllowSorting="True">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                        <ItemStyle CssClass="GridRowCSS" />
                        <Columns>
                            <asp:TemplateColumn Visible="false">
                                <ItemTemplate>
                                    <asp:ImageButton CommandName="SelectRow" ID="img_Select" Style="cursor: hand" runat="server"
                                        Width="13" Height="13" ImageUrl="~/Images/images3.jpg" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="DealSlipNo">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_DealSlipNo" Width="100px" runat="server" Text='<%# container.dataitem("DealSlipNo") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"/>
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Security Type" SortExpression="SecurityTypeName">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_SecurityType" Width="100px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        Text='<%# container.dataitem("SecurityTypeName") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="ISIN" SortExpression="ISIN">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ISIN" Width="100px" runat="server" Text='<%# container.dataitem("ISIN") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Security Name" SortExpression="SecurityName">
                                <ItemTemplate>
                                    <asp:Label ID="txt_SecurityName" Width="300px" runat="server" Text='<%#Container.DataItem("SecurityName") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="StockFaceValue" SortExpression="StockFaceValue">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_StockFaceValue" Width="150px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        Text='<%# container.dataitem("StockFaceValue") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="NoOfBonds" SortExpression="NoOfBonds">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_NoOfBonds" Width="50px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        Text='<%# container.dataitem("NoOfBonds") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Propotionconsideration" SortExpression="Propotionconsideration">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Propotionconsideration" Width="150px" runat="server" Text='<%# container.dataitem("Propotionconsideration") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="DealDate" SortExpression="DealDate">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_DealDate" Width="80px" runat="server" Text='<%# container.dataitem("DealDate") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="ValueDate" SortExpression="ValueDate">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ValueDate" Width="80px" runat="server" Text='<%# container.dataitem("ValueDate") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="CustomerName" SortExpression="CustomerName">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CustomerName" Width="150px" runat="server" Text='<%# container.dataitem("CustomerName") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Rate" SortExpression="Rate">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Rate" Width="80px" runat="server" Text='<%# container.dataitem("Rate") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>

                            <asp:TemplateColumn HeaderText="Security Id" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SecurityId" Width="150px" runat="server" Text='<%# container.dataitem("SecurityId") %>'
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
                <asp:HiddenField ID="Hid_ForDate" runat="server" />
                <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                <asp:HiddenField ID="Hid_ReportType" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
