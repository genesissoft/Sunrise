<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="RetailDebitNote.aspx.vb" Inherits="Forms_RetailDebitNote" Title="Retail Debit Note" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <script type="text/javascript" src="../Include/DatePicker.js"></script>
    <style type="text/css">
        .FixedHeader {
            position: relative;
            font-weight: bold;
            color: #004B80 !important;
            background-color: #CCE3FF;
            text-decoration: none !important;
            border-bottom: 1px solid;
            border-bottom-color: #8FBAEF !important;
        }

        table th {
            border-width: 1px;
            border-color: Gray;
            background-color: Gray;
            position: relative;
            top: expression(this.parentNode.parentNode.parentNode.scrollTop-1);
        }
    </style>

    <script language="javascript" type="text/javascript">
        function setonScroll(divObj, DgID) {
            var datagrid = document.getElementById(DgID);
            var HeaderCells = datagrid.getElementsByTagName('th');
            var HeaderRow;
            if (HeaderCells == null || HeaderCells.length == 0) {
                var AllRows = datagrid.getElementsByTagName('tr');
                HeaderRow = AllRows[0];
            }
            else {
                HeaderRow = HeaderCells[0].parentNode;
            }

            var DivsTopPosition = parseInt(divObj.scrollTop);

            if (DivsTopPosition > 0) {
                HeaderRow.style.position = 'absolute';
                HeaderRow.style.top = (parseInt(DivsTopPosition)).toString() + 'px';
                HeaderRow.style.width = datagrid.style.width;
                HeaderRow.style.zIndex = '1000';
            }
            else {
                divObj.scrollTop = 0;
                HeaderRow.style.position = 'relative';
                HeaderRow.style.top = '0';
                HeaderRow.style.bottom = '0';
                HeaderRow.style.zIndex = '0';
            }
        }
        function DateMonthSelection() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_Selection_0").checked == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "";


            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "none";

            }
        }
        function showvalidation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitDate").value) == "") {
                AlertMessage("Validation", "Please select Debit Date!", 175, 450);
                return false;
            }
        }
        function Validation() {

            var txtName = ""
            var txtName1 = ""
            var txtName2 = ""
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_Debitnote")
            var blnSelected = false

            var ids = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIds").value.split("!")
            var custIds = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Intids").value.split("!")
            var transtype = document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value.split("!")

            for (i = 1; i <= (grd.rows.length - 2) ; i++) {

                currRow = grd.children[0].children[i]


                if (currRow.children[0].children[0].checked == true) {
                    txtName = txtName + ids[i - 1] + ",";
                    txtName1 = txtName1 + custIds[i - 1] + ",";
                    txtName2 = txtName2 + transtype[i - 1] + ",";
                    blnSelected = true
                }
            }
            if (blnSelected == false) {
                alert("Please select atleast one option")
                return false
            }

            document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIds").value = txtName.substring(0, txtName.length - 1)
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_Intids").value = txtName1.substring(0, txtName1.length - 1)
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value = txtName2.substring(0, txtName2.length - 1)
            //            alert(document.getElementById("ctl00_ContentPlaceHolder1_div_onsaveclick"))
            document.getElementById("div_onsaveclick").style.display = "block";
            $('#ctl00_ContentPlaceHolder1_btn_Save').hide();
            $('#ctl00_ContentPlaceHolder1_btn_Export').hide();
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_Debitnote")
            if (grd.rows.length < 2) {
                AlertMessage("Validation", "No Records to Print!", 175, 450)
                return false
            }
        }

        function CheckAll(checkVal) {
            $('#<%=dg_Debitnote.ClientID %>').find("input:checkbox").each(function () {
                this.checked = checkVal;
            });
        } function SelectRow(elm) {
            checkVal = elm.checked
            if (checkVal == true) {
                elm.parentElement.parentElement.style.backgroundColor = "white"
            }
            else {
                elm.parentElement.parentElement.style.backgroundColor = "white"
            }
        }

        function NoRecordsFound() {
            AlertMessage("Validation", "No Records Found.", 175, 450);
        }

    </script>
    <asp:UpdatePanel ID="Upd" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="Table1" width="90%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td class="HeaderCSS" align="center">Retail Debit Note
                    </td>
                </tr>
                <tr>
                    <td align="center">

                        <table id="Table3" cellspacing="0" width="45%" cellpadding="0" border="0">
                            <tr>
                                <td colspan="6" class="SeperatorRowCSS"></td>
                            </tr>
                            <tr style="display: none">
                                <td align="right" id="lbl_According" runat="server">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;According To:
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass="LabelCSS" Style="display: none">
                                        <asp:ListItem Value="D" Selected="True">Deal Date</asp:ListItem>
                                        <asp:ListItem Value="S">Settlement Date</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>



                            <tr id="row_FromDate" runat="server">
                                <td align="right">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;From Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS jsdate" Width="143px"
                                        TabIndex="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="row_ToDate" runat="server">
                                <td align="right">To Date:</td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS jsdate" Width="143px" TabIndex="2"></asp:TextBox>
                                </td>
                            </tr>


                            <tr align="left" id="tr_Customer" runat="server" >
                                <td align="right">
                                    <asp:Label ID="lbl_test" runat="server" Text="Select Broker:" CssClass="LabelCSS"></asp:Label>

                                </td>
                                <td style="padding-left: 0px;">
                                    <uc:SelectFields ID="srh_Customer" runat="server" ProcName="ID_SEARCH_Distributor"
                                        FormHeight="475" FormWidth="257" SelectedValueName="BM.BrokerID" ChkLabelName=""
                                        ConditionalFieldId="Hid_UserTypeId" LabelName="" SelectedFieldName="BrokerName" SourceType="StoredProcedure"
                                        ConditionalFieldName="UserTypeId" ConditionalFieldId1 ="Hid_UserId" ConditionalFieldName1 ="UserId" Visible="true"
                                         ShowLabel="false" ConditionExist="false" ></uc:SelectFields>
                                </td>
                            </tr>

                            <tr>
                                <td align="right">Debit Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_DebitDate" runat="server" CssClass="TextBoxCSS jsdate" Width="100px"
                                        TabIndex="0"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr align="right">
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Show" runat="server" Text="Show" CssClass="ButtonCSS" TabIndex="19" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr align="center">
                                <td style="height: 81px" colspan="2">
                                       <div id="DivGrid" runat="server" style="position: relative; top: 0px; left: 0px; height: 400px; width: 1200px; overflow: auto">
                                    <%--<div id="div2" style="margin-top: 0px; overflow: auto; width: 1200px; padding-top: 0px; position: relative; height: 150px">--%>
                                        <%-- <asp:DataGrid ID="dg_Debitnote" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                                            TabIndex="38" Width="1000px" HeaderStyle-CssClass="FixedHeader">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />--%>
                                        <asp:DataGrid ID="dg_Debitnote" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                                            TabIndex="38" Width="1200px" ShowFooter="true" AllowSorting="true" HeaderStyle-CssClass="FixedHeader">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="FixedHeader" Width="100%" BackColor="#CCE3FF" Font-Bold="true" Height="30px" BorderColor="#004B80" />
                                            <ItemStyle HorizontalAlign="right" CssClass="GridRowCSS" />
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chk_AllItems" Width="10px" runat="server" onclick="CheckAll(this.checked)"
                                                            Checked="false"></asp:CheckBox>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk_ItemChecked" runat="server" onclick="SelectRow(this)" Checked="false"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" Width="20px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealSlipNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Dealslipno" Width="100px" runat="server" CssClass="LabelCSS" Text='<%#Container.DataItem("Dealslipno") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="DealDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealDate" Width="80px" runat="server" CssClass="LabelCSS" Text='<%#Container.DataItem("DealDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="Broker Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Brokername" runat="server" Width="150px" Text='<%#Container.DataItem("Brokername") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="Customer Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CustomerName" runat="server" Width="200px" Text='<%#Container.DataItem("CustomerName") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="Security Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SecurityName" runat="server" Width="300px" Text='<%#Container.DataItem("SecurityName") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>



                                                <asp:TemplateColumn HeaderText="BrokerageAmt" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_BrokAmt" runat="server" Width="35px" Text='<%#Container.DataItem("BrokerageAmt") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="BrokerID" FooterStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_BrokerId" runat="server" Width="30px" Text='<%#Container.DataItem("BrokerId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="DealSlipId" FooterStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealSlipId" runat="server" Width="30px" Text='<%#Container.DataItem("DealSlipId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="TransType" FooterStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_TransType" runat="server" Width="30px" Text='<%#Container.DataItem("TransType") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>


                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="center" colspan="4">
                        <asp:Label ID="lbl_Msg" runat="server" CssClass="LabelCSS"></asp:Label>
                    </td>
                    <asp:HiddenField ID="hid_ReportType" runat="server" />
                    <asp:HiddenField ID="Hid_DebitRefNo" runat="server" />
                    <asp:HiddenField ID="Hid_ServTax" runat="server" />
                    <asp:HiddenField ID="Hid_Cess" runat="server" />
                    <asp:HiddenField ID="Hid_ECess" runat="server" />
                    <asp:HiddenField ID="Hid_ReptForm" runat="server" />
                    <asp:HiddenField ID="Hid_Intids" runat="server" />
                    <asp:HiddenField ID="Hid_DealSlipIds" runat="server" />
                    <asp:HiddenField ID="Hid_TransType" runat="server" />
                    <asp:HiddenField ID="Hid_UserTypeId" runat ="server" />
                    <asp:HiddenField ID="Hid_UserId" runat ="server" />
                </tr>
                
                <tr align="center" id="row_Export" runat="server" visible="false">
                    <td align="center" colspan="6">
                        <table cellspacing="0" width="100%" cellpadding="0" border="0">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_Save" runat="server" Text="Save & Print" CssClass="ButtonCSS"
                                        TabIndex="19" />
                                </td>
                                <%--    <td align="left" runat ="server" visible ="false" >
                            &nbsp;
                            <asp:Button ID="btn_Export" runat="server" Text="Export" CssClass="ButtonCSS" TabIndex="19" />
                        </td>--%>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
