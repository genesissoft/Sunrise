<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ViewStockDetails.aspx.vb" Inherits="Forms_ViewStockDetails" Title="View Stock Details" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript">

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

        function NoRecordsFound() {
            AlertMessage("Validation", "Sorry!!! No Records available to show report.", 175, 450);
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">View Stock Details
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table width="40%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr align="left" runat="server">
                        <td>For Date:
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr align="left">
                                    <td>
                                        <asp:TextBox ID="txt_ForDate" runat="server" CssClass="TextBoxCSS" Width="90px"></asp:TextBox><img
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

                    <tr align="left">
                        <td>Security:
                        </td>
                        <td style="padding-left: 0px;">
                            <%-- <uc:Search ID="Srh_security" runat="server" AutoPostback="true" ProcName="ID_SEARCH_SecurityMaster"
                                SelectedFieldName="SecurityName" SourceType="StoredProcedure" TableName="SecurityMaster"
                                ConditionalFieldName=" " ConditionalFieldId=" " Width="200" ConditionExist="false"
                                FormHeight="400" FormWidth="600"></uc:Search>--%>
                            <uc:Search ID="Srh_security" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                SelectedFieldId="Id" SelectedFieldName="SecurityName"  FormWidth="800"  />
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
            <td class="HeadingCenter">Stock
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Button ID="btn_Print" runat="server" CssClass="ButtonCSS" Text="Print" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <div id="div1" style="margin-top: 0px; overflow: auto; width: 95%; padding-top: 0px; position: relative;">
                            <asp:DataGrid ID="dg_dme" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="False"
                                TabIndex="38" Width="100%" PageSize="15" AllowSorting="True">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                <Columns>
                                    <asp:TemplateColumn Visible="false">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandName="SelectRow" ID="img_Select" Style="cursor: hand" runat="server"
                                                Width="13" Height="13" ImageUrl="~/Images/images3.jpg" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Security Type" SortExpression="SecurityTypeName">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_SecurityType" Width="150px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                onkeypress="scroll();"
                                                Text='<%# container.dataitem("SecurityTypeName") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="150px"  />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Security Name" SortExpression="SecurityName">
                                        <%-- <ItemTemplate>
                                            <asp:label ID="txt_SecurityName" Width="200px" runat="server" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" 
                                                Text='<%# container.dataitem("SecurityName") %>' CssClass="LabelCSS"></asp:label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                                        <ItemTemplate>
                                            <asp:Label ID="txt_SecurityName" Width="300px" runat="server" Text='<%#Container.DataItem("SecurityName") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" Width ="300px" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="ISIN" SortExpression="ISIN">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_ISIN" Width="100px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                onkeypress="scroll();"
                                                Text='<%# container.dataitem("ISIN") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width ="100px" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Stock Face Value" SortExpression="StockFaceValue">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_StockFaceValue" Width="150px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                onkeypress="scroll();"
                                                Text='<%# container.dataitem("StockFaceValue") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="150px"/>
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Quantity" SortExpression="StockQuantity">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_StockQuantity" Width="50px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                onkeypress="scroll();"
                                                Text='<%# container.dataitem("StockQuantity") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="50px" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="Stock Consideration" SortExpression="StockSettlementAmt">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_StockSettlementAmt" Width="100px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                onkeypress="scroll();"
                                                Text='<%# container.dataitem("StockSettlementAmt") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"/>
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
                        <%--  <td align="center">
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                                </td>--%>
                        <asp:HiddenField ID="Hid_ForDate" runat="server" />
                        <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <table width="80%" align="left" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td>Total Stock Face Value:
                        </td>
                        <td>
                            <asp:Label ID="TStockFaceValue" runat="server" CssClass="labelcss"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>Total Physical Stock:
                        </td>
                        <td>
                            <asp:Label ID="TPhysicalStock" runat="server" CssClass="labelcss"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>Total Stock Consideration:
                        </td>
                        <td>
                            <asp:Label ID="TStockConsideration" runat="server" CssClass="labelcss"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HiddenField2" runat="server" />
    </table>
</asp:Content>
