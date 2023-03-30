<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ReportSelection.aspx.vb" Inherits="Forms_ReportSelection" Title="Report Selection" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="objRptMaster" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <script language="javascript" type="text/javascript">


        $(document).ready(function () {

            var rptName = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value

            if ((rptName != "DebitNote") && (rptName != "AdvisoryReport") && (rptName != "AdvisoryLatterReport") && (rptName != "RetailDebitRpt")) {
                jQuery("#ctl00_ContentPlaceHolder1_txt_FromDate").attr("onchange", "");
                jQuery("#ctl00_ContentPlaceHolder1_txt_ToDate").attr("onchange", "");
            }
        });

        function OpenModDialog(OpenUrl) {
            var DialogOptions = "Center=Yes; scroll=No; dialogWidth=340px; dialogTop=250px; dialogHeight=108px; Help=No; status=No; Resizable=No;titlebar=no;toolbar=no;title=abc;"
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);


            if (ret == false) {
                window.location.href("PendingDealSlip.aspx");
            }
            else if (ret == "" || typeof (ret) == "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_bPopUp").value = ret;
                return false;
            }
            else if (ret == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_bPopUp").value = ret;
                return true;
            }

        }

        function DateMonthSelection() {
            var selection = document.getElementById("ctl00_ContentPlaceHolder1_rdo_Selection")

            if (selection != null) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_Selection_0").checked == true) {
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "None";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "";
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "none";
                }

            }

        }

        function ShowList(fieldName, valueName, procName, selValues) {
            var ret = ShowDialogOpen("SelectUsers.aspx?FieldName=" + fieldName + "&ValueName=" + valueName + "&ProcName=" + procName + "&SelectedValues=" + selValues, "250px", "498px")
            return ret
        }

        function SelectChkBox(chkBoxId, lnkBtnId, lstId) {

            if (document.getElementById("ctl00_ContentPlaceHolder1_" + chkBoxId) != null) {

                if (document.getElementById("ctl00_ContentPlaceHolder1_" + chkBoxId).checked == true) {
                    removeAllOptions(document.getElementById("ctl00_ContentPlaceHolder1_" + lstId))
                    document.getElementById("ctl00_ContentPlaceHolder1_" + lnkBtnId).disabled = true
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_" + lnkBtnId).disabled = false
                }
            }
        }

        function CheckSelect() {

            SelectChkBox('chk_Customer', 'lnk_AddRemCustomer', 'lst_Customer')
            SelectChkBox('chk_Finish', 'lnk_AddRemFinish', 'lst_Finish')
            SelectChkBox('chk_Brand', 'lnk_AddRemBrand', 'lst_Brand')
            SelectChkBox('chk_Preparedby', 'lnk_AddRemPreparedby', 'lst_Preparedby')
            SelectChkBox('chk_OrderNo', 'lnk_AddRemOrderNo', 'lst_OrderNo')
            SelectChkBox('chk_ZipCode', 'lnk_AddRemZipCode', 'lst_ZipCode')
            SelectChkBox('chk_Size', 'lnk_Size', 'lst_Size')
            SelectChkBox('chk_ZipColour', 'lnk_AddRemZipColour', 'lst_ZipColour')
        }

        function removeAllOptions(selectbox) {
            var count;
            var optionCnt = selectbox.options.length
            for (count = 0; count < optionCnt; count++) {
                selectbox.remove(0);
            }
        }

        function NoRecordsFound() {
            AlertMessage("Validation", "No Records Found.", 175, 450);
        }


    </script>

    <script type="text/javascript" language="javascript">

        function CheckList(strName) {
            var lst = document.getElementById("ctl00_ContentPlaceHolder1_lst_" + strName)
            if (lst != null) {
                var lstCnt = lst.options.length
                if ((document.getElementById("ctl00_ContentPlaceHolder1_chk_" + strName).checked) == false && lstCnt == 0) {
                    alert('Please select atleast one ' + strName);
                    return false;
                }
            }
            return true
        }
        function CheckListNew(strName) {

            var lst = document.getElementById("ctl00_ContentPlaceHolder1_srh_" + strName + "_lst_Select")
            if (lst != null) {
                var lstCnt = lst.options.length
                if ((document.getElementById("ctl00_ContentPlaceHolder1_srh_" + strName + "_chk_Select").checked) == false && lstCnt == 0) {
                    alert('Please select atleast one ' + strName);
                    return false;
                }
            }
            return true
            //            
        }
        function Validationnew() {

            if ((document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value == "MISStockExcelRpt") || (document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value == "MISDetailTrading") || (document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value == "MISGeneralSettlementdt") || (document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value == "MISGeneralDealdt")) {
                if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Intcost").value - 0) == 0) {
                    alert("Please enter interest cost");
                    return false;
                }
            }


        }

        function ValidationS() {
            if (CheckList("ClientWiseMonthly") == false) return false

        }

        function Validation() {

            if (CheckListNew("exchange") == false) return false
            if (CheckListNew("Staff") == false) return false
            if (CheckListNew("Security") == false) return false
            if (CheckListNew("CustomerType") == false) return false
            if (CheckListNew("CustomerName") == false) return false
            if (CheckListNew("CustomerTypeName") == false) return false
            if (CheckListNew("Customer") == false) return false
            if (CheckListNew("WDMSecurity") == false) return false
            if (CheckListNew("CustomerName") == false) return false
        }
        function CheckDealTypes() {
            if ((document.getElementById("ctl00_ContentPlaceHolder1_chk_DealTranschkAll").checked) == true) {

                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_0").checked = true
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_1").checked = true
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_2").checked = true
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_3").checked = true
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_4").checked = true
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_5").checked = true
                //                   document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_6").checked =true
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_0").checked = false
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_1").checked = false
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_2").checked = false
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_3").checked = false
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_4").checked = false
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_5").checked = false
                //                   document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType_6").checked =false
            }

        }


    </script>

    <table width="100%" align="center" class="formTable" cellspacing="0" cellpadding="0"
        border="0">
        <tr align="left">
            <td class="SectionHeaderCSS" id="Col_Headers" runat="server">Report Selection</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table cellpadding="5" cellspacing="0" align="center" border="0">
                    <tr align="left" id="Tr_NameOFClient" runat="server">
                        <td>Name OF Client:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:Search ID="Srh_NameOFClient" runat="server" PageName="NameOFClient" AutoPostback="true"
                                SelectedFieldId="Id" SelectedFieldName="CustomerName" />
                        </td>
                    </tr>
                    <tr align="left">
                        <td id="lbl_According" runat="server">According To:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS">
                                <asp:ListItem Value="D" Selected="True">Deal Date</asp:ListItem>
                                <asp:ListItem Value="S">Settlement Date</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr align="left">
                        <td id="lbl_print" runat="server">Print:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rdo_Selection" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS">
                                <asp:ListItem Value="D" Selected="True">DateWise</asp:ListItem>
                                <asp:ListItem Value="M">MonthWise</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr align="left" id="tr_Mis" runat="server" visible="false">
                        <td>Select Report:
                        </td>
                        <td>
                            <asp:DropDownList ID="rdo_MisReport" runat="server" CssClass="ComboBoxCSS" Width="208px">
                                <asp:ListItem Value="C" Selected="True">Daily Client</asp:ListItem>
                                <asp:ListItem Value="D">Daily Dealer Wise</asp:ListItem>
                                <asp:ListItem Value="A">Advisory Details</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr align="left" id="row_FromDate" runat="server">
                        <td>From Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS jsdate" Width="115px"
                                TabIndex="1"  AutoPostBack="true"></asp:TextBox></td>
                    </tr>
                    <tr align="left" id="row_ToDate" runat="server">
                        <td>To Date:</td>
                        <td>
                            <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS jsdate" Width="115px" TabIndex="2"  AutoPostBack="true"></asp:TextBox></td>
                    </tr>
                    <tr align="left" id="row_DealTranschkAll" runat="server" visible="false">
                        <td id="Td6" runat="server">Select All:</td>
                        <td>
                            <asp:CheckBox ID="chk_DealTranschkAll" runat="server" Checked="true" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr align="left" id="row_DealTransType" runat="server" visible="false">
                        <td id="Dealtrns" runat="server">Deal Trans Type:</td>
                        <td>
                            <asp:CheckBoxList ID="cbo_DealTransType" runat="server" CssClass="ComboBoxCSS" Width="300px"
                                RepeatDirection="Horizontal" RepeatColumns="3" TabIndex="14">
                                <asp:ListItem Text="Trading" Value="T" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Broking" Value="B" Selected="True"></asp:ListItem>
                                <%--<asp:ListItem Text="Financial" Value="F" Selected="True"></asp:ListItem>--%>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr id="row_WDMTransRepts" runat="server" visible="false">
                        <td id="Td4" align="right" runat="server">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Select Report:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbo_WDMTransRepts" runat="server" CssClass="LabelCSS" Width="250px" AutoPostBack="true">
                                <asp:ListItem Value="R">Dealer wise Volume(Broking)</asp:ListItem>
                                <asp:ListItem Value="V">Dealer wise Volume</asp:ListItem>
                                <asp:ListItem Value="K">Dealer wise Brokerage(Broking)</asp:ListItem>
                                <asp:ListItem Value="O">Dealer wise Brokerage</asp:ListItem>
                                <asp:ListItem Value="E">Exchange Wise</asp:ListItem>
                                <asp:ListItem Value="C">Consultancy </asp:ListItem>
                                <asp:ListItem Value="B">Zero Brokerage </asp:ListItem>

                                <asp:ListItem Value="W">DealerWise Segment</asp:ListItem>

                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="left" id="row_MISDealTransType" runat="server" visible="false">
                        <td id="MISDealtrns" runat="server">Deal Trans Type:</td>
                        <td>
                            <asp:DropDownList ID="cbo_MISDealTransType" runat="server" CssClass="ComboBoxCSS"
                                Width="108px" TabIndex="14">
                                <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Trading" Value="T"></asp:ListItem>
                                <asp:ListItem Text="Broking" Value="B"></asp:ListItem>

                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="left" id="row_WDMDealType" runat="server" visible="false">
                        <td id="Td1" runat="server">Deal Type:</td>
                        <td>
                            <asp:DropDownList ID="cbo_DealType" runat="server" CssClass="ComboBoxCSS" Width="108px"
                                TabIndex="14">
                                <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="B">Broking</asp:ListItem>
                                <%--<asp:ListItem Value="R">Routing</asp:ListItem>--%>
                                <asp:ListItem Value="D">Direct</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="left" id="row_DealType" runat="server" visible="false">
                        <td id="Td5" runat="server">Deal Type:</td>
                        <td valign="top">
                            <asp:DropDownList ID="cbo_ConsoDealType" runat="server" Width="108px" CssClass="ComboBoxCSS"
                                AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Value="A">All</asp:ListItem>
                                <asp:ListItem Value="B">Broking</asp:ListItem>
                                <asp:ListItem Value="D">Direct</asp:ListItem>


                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="left" id="row_RetailDebitType" runat="server" visible="false">

                        <td id="Td2" runat="server">Deal Type:</td>
                        <td>
                            <asp:DropDownList ID="cbo_retailDealType" runat="server" CssClass="ComboBoxCSS" Width="108px"
                                TabIndex="14">
                                <asp:ListItem Value="B" Selected="True">Broking</asp:ListItem>
                                <asp:ListItem Value="O">Others</asp:ListItem>
                            </asp:DropDownList>
                        </td>

                    </tr>
                    <tr align="left" id="row_RetailCreditType" runat="server" visible="false">

                        <td id="Td3" runat="server">Deal Type:</td>
                        <td>
                            <asp:DropDownList ID="cbo_RetailCreditType" runat="server" CssClass="ComboBoxCSS"
                                Width="108px" TabIndex="14">
                                <asp:ListItem Value="B" Selected="True">Broking</asp:ListItem>
                                <asp:ListItem Value="O">Others</asp:ListItem>
                            </asp:DropDownList>
                        </td>

                    </tr>
                    <tr align="left" id="TR_exchange" runat="server" visible="false">
                        <td>Select Exchange:</td>
                        <td>
                            <asp:DropDownList ID="cbo_Exchange" runat="server" CssClass="ComboBoxCSS" Width="108px"
                                TabIndex="14">
                                <asp:ListItem Text="BOTH" Selected="True" Value="0"></asp:ListItem>
                                <asp:ListItem Text="NSE" Value="2"></asp:ListItem>
                                <asp:ListItem Text="BSE" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="left" id="row_Month" runat="server">
                        <td>Select Month-Year:
                        </td>
                        <td>
                            <asp:DropDownList AutoPostBack="false" ID="cbo_Months" runat="server" CssClass="ComboBoxCSS"
                                Width="90px">
                                <asp:ListItem Text="January" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                <asp:ListItem Text="December" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList AutoPostBack="false" ID="cbo_Year" runat="server" CssClass="ComboBoxCSS"
                                Width="65px">
                                <asp:ListItem Text="2009" Value="2009" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="2010" Value="2010"></asp:ListItem>
                                <asp:ListItem Text="2011" Value="2011"></asp:ListItem>
                                <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                                <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                                <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                                <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr align="left" id="tr2" runat="server" visible="false">
                        <td>
                            <%-- <asp:Label ID="Label1" runat="server" Text="Select Customer:" CssClass="LabelCSS"></asp:Label>--%>
                            Select Customer:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_Customer" runat="server" ProcName="ID_SEARCH_CustomerMasterNew"
                                FormHeight="475" FormWidth="257" SelectedValueName="CM.CustomerId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="tr_Broker" runat="server" visible="false">
                        <td>
                            <%-- <asp:Label ID="Label2" runat="server" Text="Select Broker: " CssClass="LabelCSS"
                                Width="100px"></asp:Label>--%>
                            Select Broker:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_Broker" runat="server" ProcName="ID_SEARCH_BrokerMasterNew"
                                FormHeight="475" FormWidth="257" SelectedValueName="Brokerid" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="BrokerName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_Security" runat="server" visible="false">
                        <td>Select Security:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_Security" runat="server" ProcName="ID_SEARCH_SecurityMaster"
                                FormHeight="475" FormWidth="257" SelectedValueName="SecurityId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="SecurityName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_WDMSecurity" runat="server" visible="false">
                        <td>Select Security:
                        </td>
                        <td style="padding-left: 0px;">

                            <uc:Search ID="srh_WDMSecurity" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                SelectedFieldId="Id" SelectedFieldName="SecurityName" />
                        </td>
                    </tr>
                    <tr align="left" id="row_Category" runat="server" visible="false">
                        <td>Select Category:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_category" runat="server" ProcName="ID_SEARCH_CategoryMaster"
                                FormHeight="475" FormWidth="257" SelectedValueName="CM.CategoryId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="CategoryName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_Exchangewise" runat="server" visible="false">
                        <td>Select Exchange:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_exchange" runat="server" ProcName="ID_SEARCH_ExchangeMaster"
                                FormHeight="475" FormWidth="257" SelectedValueName="EM.ExchangeId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="ExchangeName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_Staff" runat="server" visible="false">
                        <td>Select Staff:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_Staff" runat="server" ProcName="ID_SEARCH_UserMaster" FormHeight="475"
                                FormWidth="257" SelectedValueName="UM.UserId" ChkLabelName="" ConditionalFieldId=""
                                LabelName="" SelectedFieldName="NameOfUser" SourceType="StoredProcedure" ConditionalFieldName=""
                                Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_InflowOutflow" runat="server" visible="false">
                        <td>Report Type:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rbl_Transaction" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                RepeatLayout="Table" CssClass="LabelCSS" TabIndex="2">
                                <asp:ListItem Value="P" Selected="True">Inflow</asp:ListItem>
                                <asp:ListItem Value="S">Outflow</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                    <tr align="left" id="row_InflowOutflowPayment" runat="server" visible="false">
                        <td>Report Type:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rbl_TransactionPayment" runat="server" RepeatColumns="3"
                                RepeatDirection="Horizontal" RepeatLayout="Table" CssClass="LabelCSS" TabIndex="2">
                                <asp:ListItem Value="S" Selected="True">Inflow</asp:ListItem>
                                <asp:ListItem Value="P">Outflow</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                    <tr align="left" id="row_CustomerType" runat="server" visible="false">
                        <td>
                            <%--<asp:Label ID="lbl_CustomerType" runat="server" Text="Select Customer Type:" CssClass="LabelCSS"></asp:Label>--%>
                            Select Customer Type:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_CustomerType" runat="server" ProcName="ID_SEARCH_CustomerType"
                                FormHeight="475" FormWidth="257" SelectedValueName="CustomerTypeId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerTypeName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>

                    <tr align="left" id="row_SecurityType" runat="server" visible="false">
                        <td>
                            <%--<asp:Label ID="lbl_SecurityType" runat="server" Text="Select Security Type: " CssClass="LabelCSS"></asp:Label>--%>
                            Select Security Type:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_SecurityType" runat="server" ProcName="ID_SEARCH_SecurityTypeMaster"
                                FormHeight="475" FormWidth="257" SelectedValueName="SecurityTypeId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="SecurityTypeName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false" ShowAll="true"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_Intcost" runat="server" visible="false">
                        <td>Int cost:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Intcost" runat="server" CssClass="TextBoxCSS" MaxLength="30"></asp:TextBox></td>
                    </tr>
                    <tr align="left" id="row_WithWithoutbrok" runat="server" visible="false">
                        <td>
                            <%--  <asp:Label ID="lbl_brokrept" runat="server" Text="Report Type:" CssClass="LabelCSS"></asp:Label>--%>
                            Report Type:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rdo_WithWithoutbrok" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                RepeatLayout="Table" CssClass="LabelCSS" TabIndex="2">
                                <asp:ListItem Value="Y" Selected="True">With Brokerage</asp:ListItem>
                                <asp:ListItem Value="N">Without Brokerage</asp:ListItem>
                                <asp:ListItem Value="A">All</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                    <tr align="left" id="row_customer" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lbl_Customer" runat="server" Text="Select Customer:" CssClass="LabelCSS"></asp:Label></td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_CustomerName" runat="server" ProcName="ID_SEARCH_CustomerMasterNew"
                                FormHeight="470" FormWidth="257" SelectedValueName="CM.CustomerId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_customer2" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lbl_CustomerTypeName" runat="server" Text=" Select Customer Type:"
                                CssClass="LabelCSS"></asp:Label></td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_CustomerTypeName" class="LabelCSS" runat="server" ProcName="ID_SEARCH_CustomerType"
                                FormHeight="475" FormWidth="257" SelectedValueName="CustomerTypeId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerTypeName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_DealSlipNo" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lbl_DealSlipNo" runat="server" Text="DealSlipNo:" CssClass="LabelCSS"></asp:Label></td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_DealSlipNo" CheckYearCompany="true" class="LabelCSS" runat="server"
                                ProcName="ID_SEARCH_DealSlipEntry" FormHeight="475" FormWidth="257" SelectedValueName="DealSlipID"
                                ChkLabelName="" ConditionalFieldId="" LabelName="" SelectedFieldName="DealSlipNo"
                                SourceType="StoredProcedure" ConditionalFieldName="" Visible="true" ShowLabel="false"
                                ConditionExist="true"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="tr_DebitRefNo" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="DebitRefNo: " CssClass="LabelCSS" Width="100px"></asp:Label></td>
                        <td style="padding-left: 0px;">
                            <%--<uc:SelectFields ID="srh_DebitRefNo" CheckYearCompany="true" cssclass="LabelCSS"
                                runat="server" ProcName="ID_SEARCH_DebitRefNo" FormHeight="475" FormWidth="757"
                                SelectedValueName="RefNo" ChkLabelName="" ConditionalFieldId="rdo_InvDebit" LabelName=""
                                SelectedFieldName="RefNoText" SourceType="StoreProcedure" ConditionalFieldName="wdm.DebitInvFlag"
                                Visible="true" ShowLabel="false" ConditionExist="true" ShowAll="true" ConditionalFieldId1="txt_FromDate"
                                ConditionalFieldId2="txt_ToDate"></uc:SelectFields>--%>
                            <uc:SelectFields ID="srh_DebitRefNo" CheckYearCompany="false" runat="server" ProcName="ID_SEARCH_DebitRefNoBR"
                                FormHeight="475" FormWidth="900" SelectedValueName="RefNo" ChkLabelName="" ConditionalFieldId="rdo_InvDebit"
                                LabelName="" SelectedFieldName="RefNoText" SourceType="StoredProcedure" ConditionalFieldName="wdm.DebitInvFlag"
                                Visible="true" ShowLabel="false" ConditionExist="true" ShowAll="true" ConditionalFieldId1="txt_FromDate"
                                ConditionalFieldId2="txt_ToDate"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="tr_DebitRefNoNormal" runat="server" visible="false">
                        <td>DebitRefNo:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_DebitRefNoNormal" CheckYearCompany="true" runat="server"
                                ProcName="ID_SEARCH_DebitRefNoNormal" FormHeight="475" FormWidth="757" SelectedValueName="RefNo"
                                ChkLabelName="" ConditionalFieldId="" LabelName="" SelectedFieldName="RefNoText"
                                SourceType="StoredProcedure" ConditionalFieldName="" Visible="true" ShowLabel="false"
                                ConditionExist="true" ShowAll="true"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="tr_AdvisoryRefNo" runat="server" visible="false">
                        <td>AdvisoryRefNo:
                           
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="Srh_AdvisoryRefNo" CheckYearCompany="true" runat="server" ProcName="ID_SEARCH_AdvisoryRefNo"
                                FormHeight="475" FormWidth="757" SelectedValueName="RefNo" ChkLabelName="" ConditionalFieldId=""
                                LabelName="" SelectedFieldName="RefNoText" SourceType="StoredProcedure" ConditionalFieldName=""
                                Visible="true" ShowLabel="false" ConditionExist="true" ShowAll="true"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="tr_AdvLatter" runat="server" visible="false">
                        <td>AdvisoryLetterRefNo:
                           
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="SelectFields1" runat="server" ProcName="ID_SEARCH_AdvisoryLetterRefNo"
                                FormHeight="475" FormWidth="757" SelectedValueName="RefNo" ChkLabelName="" ConditionalFieldId=""
                                LabelName="" SelectedFieldName="RefNoText" SourceType="StoredProcedure" ConditionalFieldName=""
                                Visible="true" ShowLabel="false" ConditionExist="false" ShowAll="true"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr id="row_RetailDebitNote" runat="server" visible="false">
                        <td align="right">RefNo:
                        </td>
                        <td align="left">
                            <uc:SelectFields ID="srh_RetailDebit" CheckYearCompany="true" runat="server" ProcName="ID_SEARCH_RetailDebitRefNo"
                                FormHeight="475" FormWidth="900" SelectedValueName="RefNo" ChkLabelName=""
                                LabelName="" SelectedFieldName="RefNoText" SourceType="StoredProcedure"
                                ConditionalFieldId1="txt_FromDate" ConditionalFieldId2="txt_ToDate"
                                Visible="true" ShowLabel="false" ConditionExist="true" ShowAll="true"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_RetailCreditRef" runat="server" visible="false">
                        <td>Credit Ref No:
                           
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_RetailCreditRef" CheckYearCompany="true" runat="server"
                                ProcName="ID_SEARCH_RetailCreditRefNo" FormHeight="475" FormWidth="757" SelectedValueName="RefNo"
                                ChkLabelName="" LabelName="" SelectedFieldName="RefNoText" SourceType="StoredProcedure"
                                Visible="true" ShowLabel="false" ConditionExist="true" ShowAll="true"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_InvDebit" runat="server" visible="false">
                        <td>Debit /Invoice:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rdo_InvDebit" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS" AutoPostBack="true">
                                <asp:ListItem Value="D" Selected="True">Debit Note</asp:ListItem>
                                <asp:ListItem Value="I">Invoice</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                    <tr align="left" id="row_STAccCode" runat="server" visible="false">
                        <td>Print ST A/c Code:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="chk_STAccCode" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS">
                                <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                                <asp:ListItem Value="N">No</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                    <tr align="left" id="row_WDMInvoiceAddr" runat="server" visible="false">
                        <td>Print Address:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rdo_WDMInvAddr" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS">
                                <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                                <asp:ListItem Value="N">No</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                    <tr align="left" id="row_PrintAsDebInv" runat="server" visible="false">
                        <td>Print As:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rdo_PrintAsDebInv" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS" AutoPostBack="true">
                                <asp:ListItem Value="D" Selected="True">Debit Note</asp:ListItem>
                                <asp:ListItem Value="I">Invoice</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                    <tr align="left" id="row_ActiveInactive" runat="server" visible="false">
                        <td>Customer Status:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rdo_ActivInActiv" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS">
                                <asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
                                <asp:ListItem Value="I">InActive</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr align="left" id="row_ActiveInactive2" runat="server" visible="false">
                        <td>Select Range:
                        </td>
                        <td>
                            <asp:DropDownList ID="cbo_ActivInactiv" runat="server" CssClass="ComboBoxCSS" Width="95px"
                                TabIndex="14">
                                <asp:ListItem Text="3 Months" Selected="True" Value="3"></asp:ListItem>
                                <asp:ListItem Text="6 Months" Value="6"></asp:ListItem>
                                <asp:ListItem Text="9 Months" Value="9"></asp:ListItem>
                                <asp:ListItem Text="1 Year" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="line_separator">
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr align="left">
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btn_Print" runat="server" Text="View Report" CssClass="ButtonCSS"
                                TabIndex="19" Width="90px" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="Hid_Name" runat="server" />
                <asp:HiddenField ID="Hid_Id" runat="server" />
                <asp:HiddenField ID="Hid_ProdTypeName" runat="server" />
                <asp:HiddenField ID="Hid_ProdTypeId" runat="server" />
                <asp:HiddenField ID="Hid_Flag" runat="server" />
                <asp:HiddenField ID="Hid_ReportType" runat="server" />
                <asp:HiddenField ID="Hid_CustomerName" runat="server" />
                <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                <asp:HiddenField ID="Hid_SystemUserName" runat="server" />
                <asp:HiddenField ID="Hid_SystemUserId" runat="server" />
                <asp:HiddenField ID="Hid_Fromdate" runat="server" />
                <asp:HiddenField ID="Hid_Todate" runat="server" />
                <asp:HiddenField ID="Hid_bPopUp" runat="server" Value="hi" />
                <asp:HiddenField ID="Hid_PageName" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
