<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="PrintDeals.aspx.vb" Inherits="Forms_PrintDeals" Title="Print Deal" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">



        function OpenReport(dealFlag, WordPrintFlag) {
            var strTransType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value
            var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value
            var strDelMode = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PhysicalDMAT").value
            var strDealTransType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealTransType").value
            var strRateAmt = document.getElementById("ctl00_ContentPlaceHolder1_Hid_RateAmt").value
            var strmodeofdelivery = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PhysicalDMAT").value;

            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PrntLetterHead_0").checked == true) {
                var strPrintLH = "Y"
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PrntLetterHead_1").checked == true) {
                var strPrintLH = "N"
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PrintSignStamp_0").checked == true) {
                var strPrintSignStamp = "Y"
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PrintSignStamp_1").checked == true) {
                var strPrintSignStamp = "N"
            }

            var strPrintDP = "N"
            if (strmodeofdelivery == "D") {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DPDetails_0").checked == true) {
                    strPrintDP = "Y"
                }
            }

            if (strRateAmt > 1) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PrintRateAmt_0").checked == true) {
                    var strPrintRA = "R"
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PrintRateAmt_1").checked == true) {
                    var strPrintRA = "A"
                }
            }
            else {
                var strPrintRA = ""
            }

            if ((dealFlag == "FinancialAuthority") || (dealFlag == "PhysicalAuthority")) {

                var strName = window.prompt("Please Enter the Name of the Authorised person", "");
                if (strName == null || strName == "") {
                    //                         alert('Please Enter the Name of the Authorised person')
                    return false
                }
                else {
                    var pageUrl = "ViewDealReports.aspx?DealFlag=" + dealFlag + "&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType + "&ModeofDelivery=" + strDelMode + "&DealType=" + strDealTransType + "&AuthorisedPerson=" + strName + "&WordPrintFlag="
                        + WordPrintFlag + "&strPrintLH=" + strPrintLH + "&PrintDealNo" + "Y" + "&strPrintDP=" + strPrintDP + "&strPrintSignStamp=" + strPrintSignStamp;
                    var ret = window.open(pageUrl, target = "_blank", "left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
                    return false
                }
            }
            else {
                var pageUrl = "ViewDealReports.aspx?DealFlag=" + dealFlag + "&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType + "&ModeofDelivery=" + strDelMode + "&DealType=" + strDealTransType + "&WordPrintFlag=" + WordPrintFlag + "&PrintRateAmt=" + strPrintRA + "&strPrintLH=" + strPrintLH + "&PrintDealNo" + "Y" + "&strPrintDP=" + strPrintDP + "&strPrintSignStamp=" + strPrintSignStamp + "&PrintYield=" + "Y";
                var ret = window.open(pageUrl, target = "_blank", "left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
                return false
            }
        }



        function OpenMergeReport(dealFlag, WordPrintFlag) {


            var strTransType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value
            var strDelMode = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PhysicalDMAT").value
            var strDealTransType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealTransType").value
            var strPrintDet = ""

            if ((dealFlag == "PhysicalAuthorityMerge") || (dealFlag == "FinancialAuthorityMerge")) {

                var strName = window.prompt("Please Enter the Name of the Authorised person", "");
                if (strName == null || strName == "") {
                    //                         alert('Please Enter the Name of the Authorised person')
                    return false
                }
                else {
                    var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipNo").value

                    var pageUrl = "ViewDealReports.aspx?DealFlag=" + dealFlag + "&MergeDealNo=" + strDealSlipId + "&TransType=" + strTransType + "&ModeofDelivery=" + strDelMode + "&DealType=" + strDealTransType + "&AuthorisedPerson=" + strName + "&WordPrintFlag=" + WordPrintFlag;
                    var ret = window.open(pageUrl, target = "_blank", "left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
                    return false
                }
            }
            else {
                var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipNo").value
                var pageUrl = "ViewDealReports.aspx?DealFlag=" + dealFlag + "&MergeDealNo=" + strDealSlipId + "&TransType=" + strTransType + "&ModeofDelivery=" + strDelMode + "&DealType=" + strDealTransType + "&WordPrintFlag=" + WordPrintFlag + "&strPrintDet=" + strPrintDet;
                var ret = window.open(pageUrl, target = "_blank", "left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
                return false
            }
        }



        function GenerateContractNote(strId) {

            strId = document.all("ctl00_ContentPlaceHolder1_srh_TransCode_Hid_SelectedId").value;
            strCouponrate = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CouponRate").value;
            strDealslipNo = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipNo").value;
            var SettlmntDate = document.getElementById("ctl00_ContentPlaceHolder1_Hid_SettDate").value;
            var pageUrl = "ContractNote.aspx";
            pageUrl = pageUrl + "?DealSlipId=" + strId + "&Couponrate=" + strCouponrate + "&DealslipNo=" + strDealslipNo + "&SettlmntDate=" + SettlmntDate;
            var ret = ShowDialogOpen(pageUrl, "600px", "450px")
            if (ret == "" || typeof (ret) == "undefined") {
                return false
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIdRetValues").value = ret
                return true
            }
            return true
        }

        function DateType() {
            //                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DateType_0").checked == true)
            //                { 
            document.getElementById("ctl00_ContentPlaceHolder1_Tr_MergeDeal").style.display = "None";
            document.getElementById("ctl00_ContentPlaceHolder1_Tr_Deal1").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_tr_MergeDealNo").style.display = "none";
            document.getElementById("ctl00_ContentPlaceHolder1_tr_DealNo").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_Tr_Deal").style.display = "";
            //                }
            //                else
            //                {  
            //                   document.getElementById("ctl00_ContentPlaceHolder1_Tr_MergeDeal").style.display = "";
            //                   document.getElementById("ctl00_ContentPlaceHolder1_tr_MergeDealNo").style.display = "";
            //                   document.getElementById("ctl00_ContentPlaceHolder1_tr_DealNo").style.display = "None";
            //                   document.getElementById("ctl00_ContentPlaceHolder1_Tr_Deal").style.display = "None";
            //                   document.getElementById("ctl00_ContentPlaceHolder1_Tr_Deal1").style.display = "None";
            //                   document.getElementById("ctl00_ContentPlaceHolder1_tr_MergerNDS").style.display = "";
            //                     
            //                }
        }

        function DealConfReportView(WordPrintFlag) {

            var strDealType = 'MDDC'
            var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipNo").value
            var strDelMode = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PhysicalDMAT").value
            var strRateAmt = document.getElementById("ctl00_ContentPlaceHolder1_Hid_RateAmt").value
            if (strRateAmt > 1) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PrintRateAmt_0").checked == true) {
                    var strPrintRA = "R"
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PrintRateAmt_1").checked == true) {
                    var strPrintRA = "A"
                }
            }
            else {
                var strPrintRA = ""
            }
            var left = 80;
            var pageUrl1 = "ViewDealReports.aspx?DealFlag=DealConf&MergeDealNo=" + strDealSlipId + "&DealTypeFlag=" + strDealType + "&ModeOfDelivery=" + strDelMode + "&WordPrintFlag=" + WordPrintFlag + "&PrintRateAmt=" + strPrintRA;
            var ret = window.open(pageUrl1, target = "_blank", "left=" + left + ",top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
            left = left + 50;
        }
        function MergDealConfReportView(WordPrintFlag) {
            var strDealType = 'NDSMDDC'
            var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipNo").value
            var strDelMode = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PhysicalDMAT").value
            var strRateAmt = document.getElementById("ctl00_ContentPlaceHolder1_Hid_RateAmt").value
            var left = 80;
            var pageUrl1 = "ViewDealReports.aspx?DealFlag=DealConf&MergeDealNo=" + strDealSlipId + "&DealTypeFlag=" + strDealType + "&ModeOfDelivery=" + strDelMode + "&WordPrintFlag=" + WordPrintFlag;
            var ret = window.open(pageUrl1, target = "_blank", "left=" + left + ",top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
            left = left + 50;
        }



        function HDFCSglLatterView() {
            var strDealType = 'MHDFC'
            var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipNo").value
            var left = 80;
            var pageUrl1 = "ViewDealReports.aspx?DealFlag=DealConf&MergeDealNo=" + strDealSlipId + "&DealTypeFlag=" + strDealType;

            var ret = window.open(pageUrl1, target = "_blank", "left=" + left + ",top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
            left = left + 50;
        }

        function FedralSglLatterView() {
            var strDealType = 'MFED'
            var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipNo").value
            var left = 80;
            var pageUrl1 = "ViewDealReports.aspx?DealFlag=DealConf&MergeDealNo=" + strDealSlipId + "&DealTypeFlag=" + strDealType;
            var ret = window.open(pageUrl1, target = "_blank", "left=" + left + ",top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
            left = left + 50;
        }


        function MergeDealReportView() {
            var strDealType = new Array('MS');
            var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipNo").value
            var left = 80;
            for (i = 0; i < strDealType.length; i++) {
                var pageUrl1 = "ViewDealReports.aspx?DealFlag=DealConf&MergeDealNo=" + strDealSlipId + "&DealTypeFlag=" + strDealType[i];
                var ret = window.open(pageUrl1, target = "_blank", "left=" + left + ",top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
                left = left + 50;
            }
        }
    </script>

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" EnableViewState="true">
    </asp:ScriptManagerProxy>
    <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr align="left">
            <td class="SectionHeaderCSS">Print Deals
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
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="90%">
                            <tr align="center" valign="top">
                                <td>
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                        <tr align="left" visible="false" runat="server">
                                            <td id="lbl_According" runat="server">Deal Type:
                                            </td>
                                            <td id="Td1" runat="server" style="padding-left: 0px;">
                                                <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS" AutoPostBack="True" Visible="false">
                                                    <asp:ListItem Value="D" Selected="True">Simple Deal</asp:ListItem>
                                                    <%--<asp:ListItem Value="S">Merge Deal</asp:ListItem>--%>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_DealNo" runat="server">
                                            <td>Deal No.:
                                            </td>
                                            <td>
                                                <%-- <uc:Search ID="srh_TransCode" runat="server" AutoPostback="true" ProcName="ID_SEARCH_PrintDeals"
                                                    SelectedFieldName="DealSlipNo" SourceType="StoredProcedure" TableName="DealSlipEntry"
                                                    CheckYearCompany="true" ConditionExist="true" ConditionalFieldName="DSE.UserId"
                                                    ConditionalFieldId="Hid_UserId" ConditionalFieldId1 ="Hid_UserTypeId" ConditionalFieldName1 ="UserTypeId" 
                                                    FormWidth="800" PageName="PrintDeals"></uc:Search>--%>
                                                <uc:Search ID="srh_TransCode" runat="server" PageName="PrintDeals" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo" CheckYearCompany="true" ConditionExist="true" ConditionalFieldName="UserId"
                                                    ConditionalFieldId="Hid_UserId" ConditionalFieldId1="Hid_UserTypeId" ConditionalFieldName1="UserTypeId"  FormWidth ="700"/>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_MergeDealNo" runat="server">
                                            <td>MergeDealNo.:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <%--     <uc:Search ID="Srh_MergeTrnsCode" runat="server" FormWidth="550" FormHeight="400"
                                                    AutoPostback="true" ProcName="ID_SEARCH_mergedealentry" SelectedFieldName="MergedealNo"
                                                    SourceType="StoredProcedure" TableName="MergeDealEntry" ConditionExist="true"
                                                    ConditionalFieldName="" ConditionalFieldId="" CheckYearCompany="true"></uc:Search>--%>
                                                <uc:Search ID="Srh_MergeTrnsCode" runat="server" PageName="MergeDealNo" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="MergedealNo"></uc:Search>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Issuer:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_Issuer" runat="server"> 
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Security Name:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_SecurityName" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Customer Name:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_CustName" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Contact Person:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_Contact" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                        <tr align="left">
                                            <td>Deal date:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_DealDate" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Settlement Date:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_SettlementDate" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Face Value:
                                            </td>
                                            <td class="LabelCSS">
                                                <asp:Literal ID="lit_FaceValue" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Rate:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_Rate" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Mode Of Delivery:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_ModeofDelivery" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Payment Mode:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_PaymentMode" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_BrokRept" runat="server">
                                            <td>Print Letter Head:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_PrntLetterHead" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_DPDetails" runat="server">
                                            <td>Print DP Details:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DPDetails" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        <tr align="left" id="Tr1" runat="server">
                                            <td>Print Sign Stamp:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_PrintSignStamp" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_PrintRateAmt" visible="false" runat="server">
                                            <td class="LabelCSS">Print Maturity Rate/Amount:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_PrintRateAmt" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="R" Selected="True">Rate</asp:ListItem>
                                                    <asp:ListItem Value="A">Amount</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr align="center" id="Tr_Deal" runat="server">
                                            <td align="center">
                                                <asp:Button ID="btn_DealConfirmation" runat="server" CssClass="ButtonCSS" Text="Deal Confirmation"
                                                    Width="115px" />
                                                <asp:Button ID="btn_DealConfirmationWord" runat="server" CssClass="ButtonCSS" Text="Deal Conf Word Format"
                                                    Width="140px" />
                                                <asp:Button ID="btn_DealConfirmationPDF" runat="server" CssClass="ButtonCSS" Text="Deal Conf PDF Format"
                                                    Width="140px" />

                                                <asp:Button ID="btn_SGLLetter" runat="server" CssClass="ButtonCSS" Text="SGL Letter"
                                                    Visible="false" Width="75px" />
                                                <asp:Button ID="btn_PrintDealSlip" runat="server" CssClass="ButtonCSS" Text="Deal Slip"
                                                    Width="75px" />
                                                <asp:Button ID="btn_DealSlipPDF" runat="server" CssClass="ButtonCSS" Text="Deal Slip PDF"
                                                    Width="75px" />
                                                <asp:Button ID="btn_NDSOM" runat="server" CssClass="ButtonCSS" Text="NDS OM Format"
                                                    Width="130px" />
                                                <asp:Button ID="btn_ExpNDSOM" runat="server" Width="130px" CssClass="ButtonCSS" Text="NDS OM Format(Word)" />
                                                <asp:Button ID="btn_TCSAnnex" runat="server" Width="130px" CssClass="ButtonCSS" Text="TCS Annexure" />
                                                <asp:Button ID="btn_CSGL" runat="server" Width="130px" CssClass="ButtonCSS" Text="CSGL" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:Button ID="btn_SGLFedFormat" runat="server" CssClass="ButtonCSS" Visible="false"
                                                    Text="SGL Federal Format" Width="130px" />
                                                <asp:Button ID="btn_SGLHDFCFormat" runat="server" Visible="False" CssClass="ButtonCSS"
                                                    Text="SGL HDFC Format" Width="130px" />
                                                <asp:Button ID="btn_SGLFedFormatWord" runat="server" Visible="False" CssClass="ButtonCSS"
                                                    Text="SGL Federal(Word)" Width="130px" />
                                                <asp:Button ID="btn_SGLFedFormatPDF" runat="server" CssClass="ButtonCSS" Text="Deal Conf PDF Format"
                                                    Visible="false" Width="140px" />
                                            </td>
                                        </tr>
                                        <tr id="Tr_Deal1" runat="server">
                                            <td align="center">
                                                <asp:Button ID="btn_SGLHDFCFormatWord" runat="server" CssClass="ButtonCSS" Visible="false"
                                                    Text="SGL HDFC(Word)" Width="115px" />
                                                <asp:Button ID="btn_PhyAuthority" Visible="False" runat="server" CssClass="ButtonCSS"
                                                    Text="Physical Authority Letter" Width="150px" />
                                                <asp:Button ID="btn_FinAuth" Visible="False" runat="server" CssClass="ButtonCSS"
                                                    Text="Financial Authority Letter" Width="150px" />
                                                <asp:Button ID="btn_GenerateContractNote" Visible="False" runat="server" CssClass="ButtonCSS"
                                                    Text="Generate Contract Note" Width="150px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_MergeDeal" runat="server">
                                <td align="center" colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btn_MergeDealConfirmation" runat="server" CssClass="ButtonCSS" Text="Deal Confirmation"
                                                    Width="115px" />
                                                <asp:Button ID="btn_MergeDealConfirmationW" runat="server" CssClass="ButtonCSS" Text="Deal Confirmation(Word)"
                                                    Width="150px" />
                                                <asp:Button ID="btn_MergePrintDealSlip" runat="server" CssClass="ButtonCSS" Text="Deal Slip"
                                                    Width="75px" />
                                                <asp:Button ID="btn_MergeSGLLetter" runat="server" CssClass="ButtonCSS" Text="SGL Letter"
                                                    Width="75px" />
                                                <asp:Button ID="btn_MergeGenerateContractNote" runat="server" CssClass="ButtonCSS"
                                                    Text="Generate Contract Note" Width="150px" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btn_SGLFedFormatM" runat="server" CssClass="ButtonCSS" Text="SGL Federal Format"
                                                    Width="130px" Visible="false" />
                                                <asp:Button ID="btn_SGLHDFCFormatM" runat="server" CssClass="ButtonCSS" Text="SGL HDFC Format"
                                                    Width="130px" Visible="false" />
                                                <asp:Button ID="btn_PhyAuthorityM" runat="server" CssClass="ButtonCSS" Text="Physical Authority Letter"
                                                    Width="150px" Visible="False" />
                                                <asp:Button ID="btn_FinAuthM" runat="server" CssClass="ButtonCSS" Text="Financial Authority Letter"
                                                    Width="150px" Visible="False" />
                                            </td>
                                        </tr>
                                        <tr id="tr_MergerNDS" runat="server">
                                            <%--  <td>
                                                <asp:Button ID="btn_MergerNDSOM" runat="server" CssClass="ButtonCSS" Text="Merge NDS OM Format" />
                                            </td>--%>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                            <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                            <asp:HiddenField ID="Hid_TransType" runat="server" />
                            <asp:HiddenField ID="Hid_DealSlipIds" runat="server" />
                            <asp:HiddenField ID="Hid_PhysicalDMAT" runat="server" />
                            <asp:HiddenField ID="Hid_DealTransType" runat="server" />
                            <asp:HiddenField ID="Hid_AuthorisedPer" runat="server" />
                            <asp:HiddenField ID="Hid_DealSlipIdRetValues" runat="server" />
                            <asp:HiddenField ID="Hid_CouponRate" runat="server" />
                            <asp:HiddenField ID="Hid_DealSlipNo" runat="server" />
                            <asp:HiddenField ID="Hid_DealType" runat="server" />
                            <asp:HiddenField ID="Hid_Frequency" runat="server" />
                            <asp:HiddenField ID="Hid_RateAmt" runat="server" />
                            <asp:HiddenField ID="Hid_SettDate" runat="server" />
                            <asp:HiddenField ID="Hid_UserId" runat="server" />
                            <asp:HiddenField ID="Hid_UserTypeId" runat="server" />
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
