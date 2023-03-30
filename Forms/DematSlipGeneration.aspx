<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="DematSlipGeneration.aspx.vb" Inherits="Forms_DematSlipGeneration" Title="Demat Slip Generation" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function validation() {
            var txtName = ""

            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_dme")
            var blnSelected = false
            var ids = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Intids").value.split("!")
            for (i = 1; i <= (grd.rows.length - 2) ; i++) {
                currRow = grd.children[0].children[i]


                if (currRow.children[0].children[0].checked == true) {
                    txtName = txtName + ids[i - 1] + ",";

                    blnSelected = true
                }
            }
            if (blnSelected == false) {
                AlertMessage("Validation", "Please select atleast one option", 175, 450)
                return false
            }

            document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIds").value = txtName.substring(0, txtName.length - 1)

        }
        function ValidateCustomer() {

            var CustName = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value
            if (CustName == "N") {
                if ((document.getElementById("ctl00_ContentPlaceHolder1_srh_NameOFClient_txt_Name").value) == "") {
                    AlertMessage("Validation", 'Please select Customer Name', 175, 450);
                    return false;
                }

            }


        }

        function CheckAll(checkVal) {
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i]
                if (elm.type == 'checkbox' && elm.disabled == false) {
                    elm.checked = checkVal
                    if (checkVal == true) {

                    }
                    else {

                    }
                }
            }
        }
        function SelectRow(elm) {
            checkVal = elm.checked
            if (checkVal == true) {
                elm.parentElement.parentElement.style.backgroundColor = "white"
            }
            else {
                elm.parentElement.parentElement.style.backgroundColor = "white"
            }
        }


        function Submit() {
            var txtName = ""
            var intid = ""

            var intids = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Intid").value.split("!")
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg1")
            for (i = 1; i <= (grd.children[0].children.length - 2) ; i++) {
                currRow = grd.children[0].children[i]
                if (currRow.style.backgroundColor.toUpperCase() == '#D1E4F8') {
                    txtName = currRow.children[1].children[0].innerHTML
                    intid = intids[i - 1]
                    break
                }
            }
            if (txtName == "") {
                AlertMessage("Validation", "Please select atleast one option", 175, 450)
                return false
            }
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelectedField").value = txtName
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelectedValue").value = intid
            window.returnValue = intid

            return false


        }

        function SelectType() {

            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg1")
            if ((document.getElementById("ctl00_ContentPlaceHolder1_rbl_ReportType_0").checked == true) || (document.getElementById("ctl00_ContentPlaceHolder1_rbl_ReportType_1").checked == true) || (document.getElementById("ctl00_ContentPlaceHolder1_rbl_ReportType_2").checked == true)) {
                document.getElementById("row_Cust").style.display = "";
                document.getElementById("row_DPName").style.display = "";
            }
            else {
                document.getElementById("row_Cust").style.display = "none";
                //                    document.getElementById("row_DPName").style.display = "none";    
                document.getElementById("ctl00_ContentPlaceHolder1_srh_NameOFClient_txt_Name").value = ""
            }

        }

    </script>

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" EnableViewState="true">
    </asp:ScriptManagerProxy>
    <table id="Table4" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS" id="Col_Headers" runat="server">Demat Slip Generation
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                            <tr id="Tr1" runat="server">
                                <td>
                                    <table width="45%" align="center" cellspacing="0" cellpadding="0" border="0">
                                        <tr align="left">
                                            <td>DIS/Letter:
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DISLtr" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="true">
                                                    <asp:ListItem Selected="True" Value="D">DIS</asp:ListItem>
                                                    <asp:ListItem Value="N">NSDL Annexure</asp:ListItem>

                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Report Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rbl_ReportType" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="true">
                                                    <asp:ListItem Selected="True" Value="A">Normal</asp:ListItem>
                                                    <asp:ListItem Value="N">NSCCL</asp:ListItem>
                                                    <asp:ListItem Value="B">ICDM</asp:ListItem>

                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>For Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ForDate" runat="server" CssClass="TextBoxCSS" Width="115px"></asp:TextBox><img
                                                    class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ForDate',this);"
                                                    id="IMG2">
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Cust" runat="server">
                                            <td id="lbl_CustLabel" runat="server">Customer Name:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <%--                                           <uc:Search ID="srh_NameOFClient" FormWidth="700" FormHeight="350" runat="server"
                                                    AutoPostback="true" ProcName="ID_SEARCH_CustName" SelectedFieldName="CustomerName"
                                                    SourceType="StoredProcedure" TableName="CustomerMaster" ConditionalFieldName=" "
                                                    ConditionalFieldId=" "></uc:Search>--%>
                                                <uc:Search ID="srh_NameOFClient" runat="server" PageName="NameOFClient" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="CustomerName" />
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_DPName" runat="server">
                                            <td>DP Name:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_DPName" runat="server" Width="208px" CssClass="ComboBoxCSS"
                                                    AutoPostBack="false">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="line_separator">
                                            <td colspan="2"></td>
                                        </tr>
                                        <tr align="left">
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_Show" runat="server" CssClass="ButtonCSS" Text="Show" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr align="center" valign="top">
                                <td>
                                    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td>
                                                <div id="div1" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 250px">
                                                    <asp:DataGrid ID="dg_dme" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="False"
                                                        TabIndex="38" Width="95%" PageSize="15" AllowSorting="True">
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                        <Columns>
                                                            <asp:TemplateColumn>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chk_AllItems" runat="server" onclick="CheckAll(this.checked)" Checked="false"></asp:CheckBox>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chk_ItemChecked" runat="server" onclick="SelectRow(this)" Checked="false"></asp:CheckBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="DealSlip No" SortExpression="DealSlipNo">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityType" Width="80px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                                        onkeypress="scroll();"
                                                                        Text='<%# container.dataitem("DealSlipNo") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Customer Name" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_CustomerName" Width="300px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                                        onkeypress="scroll();"
                                                                        Text='<%# container.dataitem("CustomerName") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Security Name" SortExpression="SecurityName">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityName" Width="300px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                                        onkeypress="scroll();"
                                                                        Text='<%# container.dataitem("SecurityName") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Face Value" SortExpression="FaceValue">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_FaceValue" Width="100px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                                        onkeypress="scroll();"
                                                                        Text='<%# container.dataitem("FaceValue") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="No of Bonds" SortExpression="NoOfBond">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_NoOfBonds" Width="70px" runat="server" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                                        onkeypress="scroll();"
                                                                        Text='<%# container.dataitem("NoOfBond") %>' CssClass="TextBoxCSS"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="DealSlipID" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_DealSlipID" runat="server" Text='<%# container.dataitem("DealSlipID") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Dmatinfoid" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Dmatinfoid" runat="server" Text='<%# container.dataitem("Dmatinfoid") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="CustomerId" Visible="False" SortExpression="CustomerId">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_CustomerId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="80px" />
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
                                                <asp:Button ID="btn_Print" runat="server" CssClass="ButtonCSS" Text="Print" />
                                            </td>
                                            <asp:HiddenField ID="Hid_ForDate" runat="server" />
                                            <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                                            <asp:HiddenField ID="Hid_Intid" runat="server" />
                                            <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                                            <asp:HiddenField ID="Hid_Intids" runat="server" />
                                            <asp:HiddenField ID="Hid_ReportType" runat="server" />
                                            <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                                            <asp:HiddenField ID="Hid_DealSlipIds" runat="server" />
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
