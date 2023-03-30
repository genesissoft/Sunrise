<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="DealSlipEntry.aspx.vb" Inherits="Forms_DealSlipEntry" Title="Deal Slip Entry" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

       <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>
    <style type="text/css">
        .progress {
            position: fixed;
            z-index: 9999;
            height: 100%;
            width: 100%;
            top: 0;
            left: 0;
            background-color: #d1ddff;
            filter: alpha(opacity=60);
            opacity: 0.5;
            -moz-opacity: 0.5;
        }

            .progress img {
                z-index: 1000;
                margin: 300px auto;
                padding: 10px;
                border-radius: 10px;
                filter: alpha(opacity=100);
                opacity: 1;
                -moz-opacity: 1;
            }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            BindDate();
            getRdoRedeemedSec();
        });

        function getRdoRedeemedSec() {
            var rb = document.getElementById("<%=rdo_RedeemedSec.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    document.getElementById("<%=Hid_RedeemedFlag.ClientID%>").value = radio[i].value;
                    break;
                }
            }
        }
        function BindDate() {
            $('.jsdate').datepicker({
                showOn: "button",
                buttonImage: "../Images/calendar.gif",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: $("#<%= Hid_MinDate.ClientID%>").val(),
                maxDate: $("#<%= Hid_MaxDate.ClientID%>").val(),
                buttonText: 'Select date as (dd/mm/yyyy)'

            });
        }

        function FillSettlementDate() {
            if (CheckDate($("#<% = txt_DealDate.ClientID %>").get(0), false)) {
                var objData = {
                    "dealdate": $("#<% = txt_DealDate.ClientID %>").val(),
                    "settlementtype": $("#<% = cbo_SettDay.ClientID %>").val()
                }

                $.ajax({
                    url: "IPO_getdata.aspx?pagename=getsettlementdate",
                    type: "POST",
                    data: objData,
                    dataType: "text",
                    async: false,
                    success: function (result) {
                        $("#<% = txt_SettmentDate.ClientID %>").val(result);
                    },
                    failure: function (result) {

                    }
                });
                }
            }

            //    $(document).ready(function () {
            //                $('.jsdate').datepicker({
            //                   showOn: "button",
            //                buttonImage: "../Images/calendar.gif",
            //                buttonImageOnly: true,
            //                changeMonth: true,
            //                changeYear: true,
            //                dateFormat: 'dd/mm/yy',
            //                maxDate: new Date(),
            //                buttonText: 'Select date as (dd/mm/yyyy)'
            //                });

            //            });


        function UploadTempImage() {
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_Show").value = "True";
            var theForm = document.forms['aspnetForm'];
            theForm.submit();
        }

            function CallServerSide() {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_T").value = 1
                __doPostBack("ctl00_ContentPlaceHolder1_cbo_SettDay", "ddlchange");
            }
            function addInterest() {
                //debugger;
                var interestAmt = document.getElementById("<%=txt_InterestAmt.ClientID%>").value;
                var interestAmtWithoutcomma = interestAmt.replace(/,/g, '');

                var Amount = document.getElementById("<%=lbl_Amount.ClientID%>").innerHTML;
                var StampDuty = document.getElementById("<%=lbl_StampDuty.ClientID%>").innerHTML;
                var TCS = document.getElementById("<%=lbl_TCSAmount.ClientID%>").innerHTML;
                var TDS = document.getElementById("<%=lbl_TDSAmount.ClientID%>").innerHTML;
                var AmountWithoutcomma = Amount.replace(/,/g, '');

                document.getElementById("<%=lbl_SettlementAmt.ClientID%>").innerHTML = parseFloat(interestAmtWithoutcomma) + parseFloat(AmountWithoutcomma);

                document.getElementById("<%=lbl_TotalSettlementAmt.ClientID%>").value = parseFloat(interestAmtWithoutcomma) + parseFloat(AmountWithoutcomma);

                CalcRoundofsettAMT();
            }

            function CalcRoundofsettAMT() {


                var TotalRoundofsettAMT = 0;
                var Roundoff;
                var SettlementAmt;
                var SettlBrockerage = 0;
                var SettlOtherChrgs = 0;

                Roundoff = (document.getElementById("ctl00_ContentPlaceHolder1_txt_Roundoff").value - 0);
                var Amount = document.getElementById("<%=lbl_SettlementAmt.ClientID%>").innerHTML;
                var StampDuty = document.getElementById("<%=lbl_StampDuty.ClientID%>").innerHTML;
                var TCS = document.getElementById("<%=lbl_TCSAmount.ClientID%>").innerHTML;
                var TDS = document.getElementById("<%=lbl_TDSAmount.ClientID%>").innerHTML;

                var AmountWithoutcomma = Amount.replace(/,/g, '');
                //alert(AmountWithoutcomma);
                SettlementAmt = (parseFloat(AmountWithoutcomma) - 0);
                // alert(SettlementAmt);
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_roundoff_0").checked == true) {
                    TotalRoundofsettAMT = parseFloat(SettlementAmt) + parseFloat(Roundoff) + parseFloat(StampDuty) + parseFloat(TCS) - parseFloat(TDS);
                }
                else {
                    TotalRoundofsettAMT = parseFloat(SettlementAmt) - parseFloat(Roundoff) + parseFloat(StampDuty) + parseFloat(TCS) - parseFloat(TDS);
                    // alert(TotalRoundofsettAMT);
                }

                if (isNaN(parseFloat(TotalRoundofsettAMT))) {
                    document.getElementById("ctl00_ContentPlaceHolder1_Hid_TotalSettlementAmt").value = '';
                    document.getElementById("ctl00_ContentPlaceHolder1_lbl_TotalSettlementAmt").value = '';
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_Hid_TotalSettlementAmt").value = parseFloat(TotalRoundofsettAMT);//+ parseFloat(StampDuty) + parseFloat(TCS);
                    document.getElementById("ctl00_ContentPlaceHolder1_lbl_TotalSettlementAmt").value = parseFloat((document.getElementById("ctl00_ContentPlaceHolder1_Hid_TotalSettlementAmt").value - 0).toFixed(2)) //+ parseFloat(StampDuty) + parseFloat(TCS);
                }



            }

            function ShowYieldCalculation() {

                var rate = document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_txt_Name").value
                var id = document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_Hid_SelectedId").value
                //****************Mehul***********rate , FaceValue and FacevalueMultiple Pass to Showdialog box Popup**** 
                var Sellrate = document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value
                var FaceValue = document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value
                var FaceValueMultiple = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value
                var DealDate = $("#<%= txt_DealDate.ClientID%>").val();
                var SettDay = $("#<%= cbo_SettDay.ClientID%>").val();

                var StepUp = ""
                var SettDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_SettmentDate").value;
                var page = "DealSlipEntry.aspx";
                var PageName = "YieldCalculation.aspx";
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_TaxFree_0").checked == true) {
                    StepUp = "Y";
                }
                else {
                    StepUp = "N";
                }

                var OpenUrl = PageName + "?Id=" + id + "&Rate=" + rate + "&StepUp=" + StepUp + "&SettDate=" + SettDate + "&page=" + page + "&Sellrate=" + Sellrate + "&FaceValue=" + FaceValue + "&FaceValueMultiple=" + FaceValueMultiple + "&DealDate=" + DealDate + "&SettDay=" + SettDay;
                var ret = window.showModalDialog(OpenUrl, 'some argument', 'dialogWidth:780px;dialogHeight:450px;center:1;status:0;resizable:0;');
                if (typeof (ret) != "undefined") {

                    var yield = ret.toString().split('!');
                    document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value = yield[0];
                    //alert(yield[0]);
                    document.getElementById("ctl00_ContentPlaceHolder1_Hid_Yield").value = yield[1];
                    document.getElementById("ctl00_ContentPlaceHolder1_txt_Yield").value = yield[1];
                }
            }

            function ShowDialog(PageName, customerid, Rate, Sellrate, FaceValue, FaceValueMultiple, DealDate, SettDay, strWidth, strHeight) {
                var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
                var StepUp = ""
                var SettDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_SettmentDate").value;
                var page = "DealSlipEntry.aspx";

                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_TaxFree_0").checked == true) {
                    StepUp = "Y";
                }
                else {
                    StepUp = "N";
                }
                var OpenUrl = PageName + "?Id=" + customerid + "&Rate=" + Rate + "&StepUp=" + StepUp + "&SettDate=" + SettDate + "&page=" + page + "&Sellrate=" + Sellrate + "&FaceValue=" + FaceValue + "&FaceValueMultiple=" + FaceValueMultiple + "&DealDate=" + DealDate + "&SettDay=" + SettDay;
                var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
                return ret
            }

            function AddMarking() {
                var dated = $("#<%= txt_SettmentDate.ClientID%>").val();
                var dealtranstype = $("#<%= cbo_DealTransType.ClientID%>").val();
                var securityid = document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_Hid_SelectedId").value;

                var facevalue = $("#<%= txt_Amount.ClientID%>").val() * $("#<%= cbo_Amount.ClientID%>").val();
                var noofbond = $("#<%= txt_NoOfBonds.ClientID%>").val();
                var markettype = $("#<%= rdo_MarketType.ClientID%> input:checked").val();

                if (!(securityid > 0)) {
                    alert('Please select security name first.');
                    return false;
                }

                //else if (!(facevalue > 0)) {
                //    alert('Please enter correct amount for face value.');
                //    return false;
                //}
                //else {
                var strMarked = "";
                var str = $("#<%= Hid_Marked.ClientID%>").val();

                if (str != "") {
                    str = eval(str);
                    $(str).each(function (i, item) {
                        strMarked = strMarked + '{';
                        strMarked = strMarked + '"DealSlipId":"' + item.DealSlipId + '",';
                        strMarked = strMarked + '"FaceValue":"' + item.FaceValue + '",';
                        strMarked = strMarked + '"NoofBond":"' + item.NoofBond + '"';
                        strMarked = strMarked + '},';
                    });
                    strMarked = "[" + encodeURIComponent(strMarked.substring(0, strMarked.length - 1)) + "]";
                }

                var PageUrl = "DealPurchaseDetailsSell.aspx?id=" + $("#<%= Hid_DealSlipId.ClientID%>").val() + "&securityid=" + securityid + "&facevalue=" + facevalue.toFixed(2) + "&dated=" + dated + "&marked=" + strMarked + "&dealtranstype=" + dealtranstype + "&noofbond=" + noofbond + "&markettype=" + markettype;
             var ret = window.showModalDialog(PageUrl, 'some argument', 'dialogWidth:975px;dialogHeight:425px;center:1;status:0;resizable:0;');
             if (ret != "" && typeof (ret) != "undefined") {
                 $("#<%= Hid_Marked.ClientID%>").val(ret);
                   document.getElementById('<%= lnk_add.ClientID%>').click();
               }
                //}
           }

           function AddMultiplepucahse() {

               if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_txt_Name").value) == "") {
                   AlertMessage("Validation", "Please Select Name of Security", 175, 450, 'D');
                   return false;
               }
               if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0) == 0) {
                   AlertMessage("Validation", "Please enter Face value it can not be zero or blank", 175, 450, 'D');
                   return false;
               }
               if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_1").checked == true) {
                   var Facevalue = document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0
                   var facevaluemultiple = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value
                   var SecurityId = document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_Hid_SelectedId").value
                   var strReturn = "";
                   var pageUrl = "DealPurchaseshow.aspx";
                   var selpurdealidValues = "";
                   var selpurdealnoValues = "";
                   var dealslipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value
                   var lstpurdealno = document.getElementById("ctl00_ContentPlaceHolder1_lst_addmultiple")
                   var DealTransType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value


                   for (i = 0; i < lstpurdealno.options.length; i++) {

                       selpurdealidValues = selpurdealidValues + lstpurdealno.options[i].value + ","
                       selpurdealnoValues = selpurdealnoValues + lstpurdealno.options[i].innerHTML + ","
                   }


                   strReturn = strReturn + selpurdealidValues + "!"
                   strReturn = strReturn + selpurdealnoValues + "!"
                   strReturn = strReturn + document.getElementById("ctl00_ContentPlaceHolder1_Hid_RemainingFaceValue").value + "!"
                   pageUrl = pageUrl + "?DealTransType=" + DealTransType + "&dealslipId=" + dealslipId + "&strReturn=" + strReturn + "&Facevalue=" + Facevalue + "&facevaluemultiple=" + facevaluemultiple + "&SecurityId=" + SecurityId;
                   var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:850px;dialogHeight:600px;center:1;status:0;resizable:0;');

                   if (typeof (ret) != "undefined") {
                       document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret
                       document.getElementById('<%= lnk_add.ClientID%>').click();
                    }
                }
            }



            function AddMultipleSell() {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_txt_Name").value) == "") {
                    AlertMessage("Validation", "Please Select Name of Security", 175, 450, 'D');
                    return false;
                }
                if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0) == 0) {
                    AlertMessage("Validation", "Please enter Face value it can not be zero or blank", 175, 450, 'D');
                    return false;
                }
                if ((document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true) && (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "F")) {
                    var Facevalue = document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0
                    var facevaluemultiple = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value
                    var SecurityId = document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_Hid_SelectedId").value
                    var strReturn = "";
                    var pageUrl = "Dealsellshow.aspx";
                    var selpurdealidValues = "";
                    var selpurdealnoValues = "";
                    var dealslipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value
                    var lstpurdealno = document.getElementById("ctl00_ContentPlaceHolder1_lst_addmultipleFinancial")
                    var DealTransType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value
                    for (i = 0; i < lstpurdealno.options.length; i++) {
                        selpurdealidValues = selpurdealidValues + lstpurdealno.options[i].value + ","
                        selpurdealnoValues = selpurdealnoValues + lstpurdealno.options[i].innerHTML + ","

                    }

                    strReturn = strReturn + selpurdealidValues + "!"
                    strReturn = strReturn + selpurdealnoValues + "!"
                    strReturn = strReturn + document.getElementById("ctl00_ContentPlaceHolder1_Hid_RemainingFaceValue").value + "!"

                    pageUrl = pageUrl + "?DealTransType=" + DealTransType + "&dealslipId=" + dealslipId + "&strReturn=" + strReturn + "&Facevalue=" + Facevalue + "&facevaluemultiple=" + facevaluemultiple + "&SecurityId=" + SecurityId;

                    var ret = ShowDialogOpen(pageUrl, "850px", "600px")
                    if (ret == "" || typeof (ret) == "undefined") {
                        return false
                    }
                    else {
                        document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret
                        return true
                    }
                }
            }


            function OpenReport(dealFlag) {


                var strTransType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value
                var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value
                var strDelMode = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PhysicalDMAT").value
                var strDealTransType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealTransType").value
                if ((dealFlag == "FinancialAuthority") || (dealFlag == "PhysicalAuthority")) {

                    var strName = window.prompt("Please Enter the Name of the Authorised person", "");
                    if (strName == "") {
                        AlertMessage("Validation", 'Please Enter the Name of the Authorised person', 175, 450, 'D')
                        return false
                    }
                    else {

                        var pageUrl = "ViewDealReports.aspx?DealFlag=" + dealFlag + "&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType + "&ModeofDelivery=" + strDelMode + "&DealType=" + strDealTransType + "&AuthorisedPerson=" + strName;
                        var ret = window.open(pageUrl, target = "_blank", "left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
                        return false
                    }
                }
                else {
                    var pageUrl = "ViewDealReports.aspx?DealFlag=" + dealFlag + "&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType + "&ModeofDelivery=" + strDelMode + "&DealType=" + strDealTransType;
                    var ret = window.open(pageUrl, target = "_blank", "left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
                    return false
                }
            }




            function ShowSecurityMaster() {
                var strpagename = "DealSlipEntry.aspx";
                //var Id = document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_Hid_SelectedId").value;
                var Id = document.getElementById("ctl00_ContentPlaceHolder1_Hid_SecId").value;
                ShowSecurityForm("SecurityMaster.aspx", Id, "900px", "680px")
                return false
            }



            function ShowPurDeal() {
                var strpagename = "DealSlipEntry.aspx";
                var Id = document.getElementById("ctl00_ContentPlaceHolder1_Hid_BTOBid").value
                ShowSecurityForm("DealSlipEntry.aspx", Id, "900px", "680px")
                return false
            }

            function ShowBrokPurDeal() {
                var strpagename = "DealSlipEntry.aspx";
                var Id = document.getElementById("ctl00_ContentPlaceHolder1_Hid_BTOBid").value
                ShowSecurityForm("DealSlipEntry.aspx", Id, "900px", "680px")
                return false
            }




            function ShowCustomerMaster() {
                var strpagename = "DealSlipEntry.aspx";
                //var Id = document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameOFClient_Hid_SelectedId").value;
                var Id = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustId").value;
                ShowSecurityForm("ClientProfileMaster.aspx", Id, "900px", "680px")
                return false
            }



            function ShowSecurityForm(PageName, Id, Width, Height) {

                var w = Width;
                var h = Height;
                var winl = (screen.width - w) / 2;
                var wint = (screen.height - h) / 2;
                if (winl < 0) winl = 0;
                if (wint < 0) wint = 0;
                var HideMenu = "HideMenu"
                PageName = PageName + "?Id=" + Id + "&Flag=C" + "&HideMenu=" + HideMenu
                windowprops = "height=" + h + ",width=" + w + ",top=" + wint + ",left=" + winl + ",location=no,"
                + "scrollbars=yes,menubar=yes,toolbar=yes,resizable=yes,status=yes";
                window.open(PageName, "Popup", windowprops);
            }


            function ValidateDeal() {
                Validation()
                var strDealType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PhysicalDMAT_0").checked == true) {
                    if (strDealType != "B") {

                    }
                }
                else {

                }

                return true
            }
            function DealReportView() {
                var strFreqInterest = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Frequency").value
                var strDealType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value
                var strRateAmt = document.getElementById("ctl00_ContentPlaceHolder1_Hid_RateAmt").value
                var TCS = document.getElementById("ctl00_ContentPlaceHolder1_Hid_TCSApplicable").value

                if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value != "N") && (document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value != "B") && (document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value != "L")) {

                    if (TCS == "Y") {
                        var strTCS = "TCSAnnex"
                    }
                    else {

                        var strTCS = "DealTicket"
                    }

                }
                else {

                    var strTCS = "DealTicket"
                }


                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked) {
                    var strTransType = "P"
                }
                else {
                    var strTransType = "S"
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PhysicalDMAT_0").checked) {
                    var strDelMode = "D"
                }
                else {
                    var strDelMode = "S"
                }
                if (strDealType == "B") {

                    if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PrntBrokDetails_0").checked == true) {
                        var strPrintDet = "Y"
                    }
                    if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PrntBrokDetails_1").checked == true) {
                        var strPrintDet = "N"
                    }
                }
                else {
                    var strPrintDet = ""
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
                var strPrintLH = "N";
                var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value
                var pageUrl1 = "ViewDealReports.aspx?DealFlag=DealConf&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType
                               + "&DealType=" + strDealType + "&FreqInterest=" + strFreqInterest + "&ModeofDelivery=" + strDelMode + "&strPrintDet=" + strPrintDet + "&PrintRateAmt=" + strPrintRA + "&strPrintLH=" + "N" + "&PrintDealNo" + "Y" + "&PrintYield=" + "Y";
                var pageUrl2 = "ViewDealReports.aspx?DealFlag=" + strTCS + "&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType
                                + "&DealType=" + strDealType + "&FreqInterest=" + strFreqInterest + "&PrintRateAmt=" + strPrintRA + "&strPrintLH=" + "N" + "&PrintDealNo" + "Y" + "&PrintYield=" + "Y";
                var pageUrl3 = "ViewDealReports.aspx?DealFlag=SGLLetter&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType
                                 + "&DealType=" + strDealType + "&FreqInterest=" + strFreqInterest + "&ModeofDelivery=" + strDelMode + "&PrintRateAmt=" + strPrintRA + "&strPrintLH=" + "N" + "&PrintDealNo" + "Y" + "&PrintYield=" + "Y";
                var ret = window.open(pageUrl1, target = "_blank", "left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
                //if (strTCS == "TCSAnnex") {

                //    var ret = window.open(pageUrl2, target = "_blank", "left=130,top=130,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
                //}
                window.location = "GeneratedDealSlip.aspx";
            }



            function DealTicketReportView() {


                var strFreqInterest = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Frequency").value

                var strDealType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value
                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked) {
                    var strTransType = "P"
                }
                else {
                    var strTransType = "S"
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PhysicalDMAT_0").checked) {
                    var strDelMode = "D"
                }
                else {
                    var strDelMode = "S"
                }
                var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value

                //var pageUrl2 = "ViewDealReports.aspx?DealFlag=DealTicket&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType
                //              + "&DealType=" + strDealType + "&FreqInterest=" + strFreqInterest;

                //var ret = window.open(pageUrl2, target = "_blank", "left=130,top=130,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")

                window.location = "DealSlipDetail.aspx";
            }



            function DealTicketReportViewBrok() {
                var strFreqInterest = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Frequency").value

                var strDealType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value
                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked) {
                    var strTransType = "P"
                }
                else {
                    var strTransType = "S"
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PhysicalDMAT_0").checked) {
                    var strDelMode = "D"
                }
                else {
                    var strDelMode = "S"
                }
                var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PurBrokDealSlipId").value
                var strSellDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_SellBrokDealSlipId").value
                var pageUrl1 = "ViewDealReports.aspx?DealFlag=DealTicket&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType
                        + "&DealType=" + strDealType + "&FreqInterest=" + strFreqInterest;
                var ret = window.open(pageUrl1, target = "_blank", "left=130,top=130,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")

                var pageUrl2 = "ViewDealReports.aspx?DealFlag=DealTicket&DealSlipId=" + strSellDealSlipId + "&TransType=" + strTransType
                        + "&DealType=" + strDealType + "&FreqInterest=" + strFreqInterest;
                var ret = window.open(pageUrl2, target = "_blank", "left=130,top=130,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")

                window.location = "DealSlipDetail.aspx";
            }


            function Validation() {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType").value) == "") {
                    AlertMessage("Validation", "Please Select Security Type", 175, 450, 'D');
                    return false;
                }

                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_IssuerOfSecurity_txt_Name").value) == "") {
                    AlertMessage("Validation", "Please Select Issuer Of Security", 175, 450, 'D');
                    return false;
                }

                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_txt_Name").value) == "") {
                    AlertMessage("Validation", "Please Select Name of Security", 175, 450, 'D');
                    return false;
                }

                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameOFClient_txt_Name").value) == "") {
                    AlertMessage("Validation", "Please Select  Name OF Client", 175, 450, 'D');
                    return false;
                }
                var cboBrokNameOfSeller = document.getElementById("ctl00_ContentPlaceHolder1_srh_BrokNameOfSeller")
                if (cboBrokNameOfSeller != null) {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameOFClient_txt_Name").value) = (document.getElementById("ctl00_ContentPlaceHolder1_srh_BrokNameOfSeller_txt_Name").value)) {
                        AlertMessage("Validation", "Buyer and seller name cannot be same.", 175, 450, 'D');
                        return false;
                    }
                }

                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                        if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_BrokingBTBDealSlipNo_txt_Name").value) == "") {
                            AlertMessage("Validation", "Please Select  BTB DealSlip No", 175, 450, 'D');
                            return false;
                        }
                    }
                }

                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_DealDate").value) == "") {
                    AlertMessage("Validation", "Please Deal Date ", 175, 450, 'D');
                    return false;
                }
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value) == "") {
                    AlertMessage("Validation", "Please Enter Face Value ", 175, 450, 'D');
                    return false;
                }
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value) == 0) {
                    AlertMessage("Validation", "Face Value Can not be Zero", 175, 450, 'D');
                    return false;
                }
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value) == "") {
                    AlertMessage("Validation", "Please Enter Rate ", 175, 450, 'D');
                    return false;
                }
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value) == 0) {
                    AlertMessage("Validation", "Rate Can not be Zero", 175, 450, 'D');
                    return false;
                }
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_SettmentDate").value) == "") {
                    AlertMessage("Validation", "Please Settment Date", 175, 450, 'D');
                    return false;
                }


                var qty = document.getElementById("ctl00_ContentPlaceHolder1_txt_NoOfBonds").value;
                if ((qty - 0) == 0 || qty.indexOf(".") != -1) {
                    AlertMessage("Validation", "The Quantity can not be Zero or Decimals", 175, 450, 'D');
                    return false;
                }


                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_Company").value) == "") {
                    AlertMessage("Validation", "Please Select Company Name", 175, 450, 'D');
                    return false;
                }

                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealerName").value) == "") {
                    AlertMessage("Validation", "Please Select Dealer Name", 175, 450, 'D');
                    return false;
                }
                //suvi          
                //                 if(document.getElementById("ctl00_ContentPlaceHolder1_chk_Brokerage1").checked==true) 
                //                 {
                //                            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_ConsBroker").value) == "")
                //                            {  alert("Please Select Introducing Broker");
                //                                return false;
                //                            }
                //                             if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_NilComm_1").checked == true)
                //                             {
                //                                if((Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Conchargesreceived").value) == "")|| (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Conchargesreceived").value) == "0.00"))
                //                                {  alert("Please enter YTM / YTC to IB");
                //                                    return false;
                //                                }
                //                             }
                //                             if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_NilComm_1").checked == true)
                //                             {
                //                                 if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_CommChecked_0").checked == true)
                //                                 {
                //                                     if((Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Conchargespaid").value) == "")|| (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Conchargespaid").value) == "0.00")|| (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Conchargespaid").value) == "0.0000"))
                //                                     {
                //                                      {  alert("Please enter Commission on trade");
                //                                        return false;
                //                                    }  
                //                                     }
                //                                 
                //                                 }
                //                             }
                //                           
                //                            
                //                 }
                var PurMethod = document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod")
                var lstpurno1 = document.getElementById("ctl00_ContentPlaceHolder1_lst_addmultiple")

                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {

                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {

                        if (PurMethod != null) {
                            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true) {

                                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {
                                    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "F") {
                                        if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B") {
                                            if ((document.getElementById("ctl00_ContentPlaceHolder1_srh_BTBDealSlipNo_Hid_SelectedId").value - 0) == 0) {

                                                AlertMessage("Validation", "Please Select pur Deal Slip No", 175, 450, 'D');

                                                return false;
                                            }
                                        }
                                    }
                                }
                            }

                            else {

                                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {
                                    if (lstpurno1.options.length == 0) {
                                        AlertMessage("Validation", "Please Enter Purchase deal slip no", 175, 450, 'D');
                                        return false;
                                    }
                                    //if (lstpurno1.options.length < 2) {
                                    //    AlertMessage("Validation", "Please Enter two Purchase deal slip no", 175, 450, 'D');
                                    //    return false;
                                    //}
                                }
                            }
                        }

                    }
                }

                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {

                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                        if (PurMethod != null) {
                            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true) {

                                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "F") {
                                    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B") {
                                        if (((document.getElementById("ctl00_ContentPlaceHolder1_srh_BTBDealSlipNo_Hid_SelectedId").value - 0) == 0) || ((document.getElementById("ctl00_ContentPlaceHolder1_srh_BTBDealSlipNo_txt_Name").value - 0) == "")) {

                                            AlertMessage("Validation", "Please Select pur Deal Slip No", 175, 450, 'D');

                                            return false;
                                        }
                                    }
                                }
                            }

                        }


                    }
                }




                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {

                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                        if (PurMethod != null) {
                            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true) {

                            }
                        }
                    }
                }

                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_RefDealSlip_0").checked == true) {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_srh_RefDealSlipNo_Hid_SelectedId").value - 0) == 0) {
                        AlertMessage("Validation", "Please Ref Deal Slip No", 175, 450, 'D');
                        return false;
                    }

                    if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_CancelRemark").value) == "") {
                        AlertMessage("Validation", "Please Enter remark", 175, 450, 'D');
                        return false;
                    }

                }


                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                    var PurMethod = document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod")
                    var SingleRemainFV = (document.getElementById("ctl00_ContentPlaceHolder1_Hid_SingleRemainFV").value - 0)

                }


                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                    var PurMethod = document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod")
                    var lstpurno = document.getElementById("ctl00_ContentPlaceHolder1_lst_addmultiple")

                    if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "T") || (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "D") || (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "O") || (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "M")) {

                        if (PurMethod != null) {
                            if ((document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_1").checked == true)) {
                                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {

                                    if (lstpurno.options.length == 0) {
                                        AlertMessage("Validation", "Please Enter Purchase deal slip no", 175, 450, 'D');
                                        return false;
                                    }
                                    //if (lstpurno.options.length < 2) {
                                    //    AlertMessage("Validation", "Please Enter two Purchase deal slip no", 175, 450, 'Ds');
                                    //    return false;
                                    //}
                                }
                            }
                        }
                    }
                }



                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                    var PurMethod = document.getElementById("ctl00_ContentPlaceHolder1_row_SellMethod")
                    var lstpurno = document.getElementById("ctl00_ContentPlaceHolder1_lst_addmultipleFinancial")
                }



                var PurMethod = document.getElementById("ctl00_ContentPlaceHolder1_row_SellMethod")
                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {

                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                        if (PurMethod != null) {
                            if (document.getElementById("ctl00_ContentPlaceHolder1_row_SellMethod_0").checked == true) {
                                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "F") {
                                    if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_0").checked == true) {
                                        if (((document.getElementById("ctl00_ContentPlaceHolder1_srh_FinancialDealSlipNo_Hid_SelectedId").value - 0) == 0) || ((document.getElementById("ctl00_ContentPlaceHolder1_srh_FinancialDealSlipNo_txt_Name").value - 0) == "")) {
                                            AlertMessage("Validation", "Please Select Sell Deal Slip No", 175, 450, 'D');
                                            return false;
                                        }
                                    }
                                }
                            }
                        }

                    }

                }

                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                    var PurMethod = document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod")
                    var SingleRemainFV = 0
                    SingleRemainFV = (document.getElementById("ctl00_ContentPlaceHolder1_Hid_SingleRemainFV").value - 0)


                    //Financial costmemo
                    if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_1").checked == true) {
                        if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "F")) {
                            if (PurMethod != null) {
                                if ((document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true)) {
                                    var cbo_val = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value;

                                    var amt;

                                    if (cbo_val == 100000) {

                                        amt = ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0) * 1000 * 10);
                                    }
                                    else {

                                        amt = ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0) * (cbo_val - 0));
                                        var amt1 = Math.round(amt, 4)
                                    }
                                    if ((document.getElementById("ctl00_ContentPlaceHolder1_Hid_SingleRemainFV").value - 0) < amt1) {


                                        AlertMessage("Validation", 'Face value can not be more than ' + SingleRemainFV + ' Remaining Face Value', 175, 450, 'D')
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    else {

                        if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "F")) {

                            if (PurMethod != null) {
                                if ((document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true)) {
                                    var cbo_val = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value;

                                    var amt;

                                    if (cbo_val == 100000) {

                                        amt = ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0) * 1000 * 10);
                                    }
                                    else {

                                        amt = ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0) * (cbo_val - 0));
                                        var amt1 = Math.round(amt, 4)


                                    }
                                    if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_SingleRemainFV").value < amt1) {

                                        AlertMessage("Validation", 'Face value can not be more than ' + SingleRemainFV + ' Remaining Face Value', 175, 450, 'D')
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    var SellMethod = document.getElementById("ctl00_ContentPlaceHolder1_row_SellMethod")
                    var SingleRemainFV = (document.getElementById("ctl00_ContentPlaceHolder1_Hid_SingleRemainFV").value - 0)

                    //Financial costmemo
                    if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_0").checked == true) {
                        if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "F")) {
                            if (SellMethod != null) {
                                if ((document.getElementById("ctl00_ContentPlaceHolder1_row_SellMethod_0").checked == true)) {
                                    var cbo_val = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value;
                                    var amt;

                                    if (cbo_val == 100000) {
                                        amt = ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0) * 1000 * 10);
                                    }
                                    else {
                                        amt = ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0) * (cbo_val - 0));
                                        var amt1 = Math.round(amt, 4)
                                    }
                                    if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_SingleRemainFV").value < amt1) {

                                        AlertMessage("Validation", 'Face value can not be more than ' + SingleRemainFV + ' Remaining Face Value', 175, 450, 'D')
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                if ((querySt("Page") == "PendingDealSlip.aspx") || (querySt("Page") == "GeneratedDealSlip.aspx")) {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {



                        if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                            if (PurMethod != null) {
                                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true) {

                                    if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_1").checked == true) {


                                        if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "F") {
                                            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B") {
                                                if ((document.getElementById("ctl00_ContentPlaceHolder1_srh_BTBDealSlipNo_Hid_SelectedId").value - 0) == 0) {
                                                    AlertMessage("Validation", "Please Select pur Deal Slip No", 175, 450, 'D');

                                                    return false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }




                if ((document.getElementById("ctl00_ContentPlaceHolder1_rbl_Reference_0").checked == true)) {
                    if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_ReferenceBy_txt_Name").value) == "") {
                        AlertMessage("Validation", "Please Enter Reference By Name", 175, 450, 'D');
                        return false;
                    }
                }



                return true;
            }


            function CheckDealType() {

                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                    document.getElementById("row_SelectMethod").style.display = "none";
                }
                else {
                    document.getElementById("row_SelectMethod").style.display = "";
                }


                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {

                        document.getElementById("Client").innerHTML = "Name Of Seller"
                        document.getElementById("Seller").innerHTML = "Name Of Buyer"

                    }
                    else {
                        document.getElementById("Client").innerHTML = "Name Of Buyer"
                        document.getElementById("Seller").innerHTML = "Name Of Seller"

                    }
                }



                else {

                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {

                        document.getElementById("Client").innerHTML = "Name Of Buyer"
                    }
                    else if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                        document.getElementById("Client").innerHTML = "Name Of Seller"

                    }
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {

                    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                        document.getElementById("row_BrokingBTB").style.display = "";
                        document.getElementById("row_BTB").style.display = "none";
                        document.getElementById("row_PurMethod").style.display = "none";
                        document.getElementById("row_SelectMethod").style.display = "none";
                    }
                    else if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {
                        document.getElementById("row_BTB").style.display = "";
                        document.getElementById("row_PurMethod").style.display = "";
                        document.getElementById("row_SelectMethod").style.display = "";
                    }
                    else {
                        document.getElementById("row_BrokingBTB").style.display = "none";
                        document.getElementById("row_BTB").style.display = "none";
                        document.getElementById("row_PurMethod").style.display = "none";
                        document.getElementById("row_addmultiple").style.display = "none";
                        document.getElementById("row_SelectMethod").style.display = "";

                    }



                }
                else {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "F") {
                        document.getElementById("row_BTB").style.display = "none";
                        document.getElementById("row_BrokingBTB").style.display = "none";
                        if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {

                            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_0").checked == true) {

                                document.getElementById("row_BTB").style.display = "none";
                            }
                            else {

                                document.getElementById("row_BTB").style.display = "";

                            }
                        }
                    }
                    else {
                        document.getElementById("row_BrokingBTB").style.display = "none";
                        document.getElementById("row_BTB").style.display = "none";

                    }
                }


                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {

                    document.getElementById("row_BrokCustId").style.display = "";
                    document.getElementById("row_BrokCustTypeId").style.display = "";
                    //                 document.getElementById("row_BrokCustomerScheme").style.display = "block";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_ConsBRok").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_PaidConsBRok").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_RecConsBRok").style.display = "none";
                    //                document.getElementById("ctl00_ContentPlaceHolder1_row_commchkd").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Bpto").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Bp").style.display = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_BrFrom").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Br").style.display = "";
                    document.getElementById("row_BackOffice1").style.display = "";
                    document.getElementById("row_BackOfficeheader").style.display = "";

                    document.getElementById("row_BackOfficeheader").style.display = "";



                    document.getElementById("tr_interestsection").style.display = "";
                    document.getElementById("tr_interestsectionTable").style.display = "";

                    document.getElementById("tr_SellerDealerName").style.display = "";
                    document.getElementById("tr_CounterCustomerBank").style.display = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_tr_SellerBrokeragePaid").style.display = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_tr_SellerBrokerageReceived").style.display = "";
                    document.getElementById("tr_Countcustperson").style.display = "";
                    document.getElementById("tr_CountcustpersonTextbox").style.display = "";
                    document.getElementById("tr_SelectCountAddress").style.display = "";

                }
                else {

                    document.getElementById("row_BrokCustId").style.display = "none";
                    document.getElementById("row_BrokCustTypeId").style.display = "none";
                    //                document.getElementById("row_BrokCustomerScheme").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_ConsBRok").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_PaidConsBRok").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_RecConsBRok").style.display = "none";
                    //                document.getElementById("ctl00_ContentPlaceHolder1_row_commchkd").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Bpto").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Bp").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_BrFrom").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Br").style.display = "none";

                    document.getElementById("tr_SellerDealerName").style.display = "none";
                    document.getElementById("tr_CounterCustomerBank").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_tr_SellerBrokeragePaid").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_tr_SellerBrokerageReceived").style.display = "none";
                    document.getElementById("tr_Countcustperson").style.display = "none";
                    document.getElementById("tr_CountcustpersonTextbox").style.display = "none";
                    document.getElementById("tr_SelectCountAddress").style.display = "none";
                    if ((querySt("PageName") == "QuoteEntry.aspx") || (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B")) {

                        document.getElementById("row_BackOffice1").style.display = "none";
                        document.getElementById("row_BackOfficeheader").style.display = "none";
                        document.getElementById("tr_interestsection").style.display = "none";
                        document.getElementById("tr_interestsectionTable").style.display = "none";

                    }

                }
                Showaddlnk()


                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "P") {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                        document.getElementById("row_Remove").style.display = "none";
                    }

                }

                if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B")) {

                    //                    document.getElementById("row_BackOffice1").style.display = "none";  
                    //                    document.getElementById("row_BackOfficeheader").style.display = "none";
                    //                    document.getElementById("tr_interestsection").style.display = "none";
                    //                    document.getElementById("tr_interestsectionTable").style.display = "none";     

                }

                //                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true ) 
                //                {
                //                     alert("1");
                //                     if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "T"))
                //                     {
                //                        alert("2");
                //                        document.getElementById('<%= rbl_DealSlipType.ClientID %>').disabled = false;
                //                     }
                //                }
                Brokerage()
                if (querySt("Page") == "DealSlipDetail.aspx") {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B") {
                        if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                            document.getElementById("row_lnkadd").style.display = "none"
                            document.getElementById("row_addmultiple").style.display = "none"
                            document.getElementById("row_BTB").style.display = "none"
                            document.getElementById("row_Remove").style.display = "none";
                            document.getElementById("row_PurMethod").style.display = "none";
                            document.getElementById("row_SelectMethod").style.display = "none";

                        }
                    }
                }
            }


            function querySt(ji) {
                hu = window.location.search.substring(1);
                gy = hu.split("&");
                for (i = 0; i < gy.length; i++) {
                    ft = gy[i].split("=");
                    if (ft[0] == ji) {
                        return ft[1];
                    }
                }
            }

            function Showaddlnk() {
                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B") {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                        if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {
                            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true) {

                                document.getElementById("row_lnkadd").style.display = "none"
                                document.getElementById("row_addmultiple").style.display = "none"
                                document.getElementById("row_BTB").style.display = ""
                                document.getElementById("row_BrokingBTB").style.display = "none";

                            }
                            else {
                                document.getElementById("row_lnkadd").style.display = ""
                                document.getElementById("row_addmultiple").style.display = ""
                                document.getElementById("row_BTB").style.display = "none"
                                document.getElementById("row_BrokingBTB").style.display = "none";
                            }
                        }
                    }

                }
                if (querySt("Page") == "DealSlipDetail.aspx") {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B") {
                        if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                            document.getElementById("row_lnkadd").style.display = "none"
                            document.getElementById("row_addmultiple").style.display = "none"
                            document.getElementById("row_BTB").style.display = "none"
                            document.getElementById("row_Remove").style.display = "none";
                            document.getElementById("row_PurMethod").style.display = "none"

                        }
                    }
                }


                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                    document.getElementById("row_addmultiple").style.display = "none"
                    document.getElementById("row_PurMethod").style.display = "none"
                    document.getElementById("row_Remove").style.display = "none";
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B") {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_PendngFlag").value == "Generated") {
                        if ((document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) && (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_0").checked == true)) {
                            document.getElementById("row_Remove").style.display = "";
                        }
                        else {
                            document.getElementById("row_Remove").style.display = "none";
                        }
                    }
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_PendngFlag").value == "Generated") {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) && (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_1").checked == true)) {
                        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true) {
                            document.getElementById("row_Remove").style.display = "";
                        }
                        else {
                            document.getElementById("row_Remove").style.display = "none";
                        }

                    }
                }

                if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_PendngFlag").value == "Generated") {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) && (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_0").checked == true) && (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "F")) {
                        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true) {
                            document.getElementById("row_Remove").style.display = "";
                        }
                        else {
                            document.getElementById("row_Remove").style.display = "none";
                        }

                    }
                }


                if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "D") || (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "T")) {

                    if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_PendngFlag").value == "Generated") {

                        if ((document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) && (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_0").checked == true)) {

                            document.getElementById("row_Remove").style.display = "none";
                        }
                    }
                }


                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                    document.getElementById("row_Remove").style.display = "none";
                }

                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "P") {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                        document.getElementById("row_Remove").style.display = "none";
                    }

                }
                if ((document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) && (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_0").checked == true)) {
                    document.getElementById("row_Remove").style.display = "none";

                }
                document.getElementById("row_FinDealType").style.display = "none";

            }




            function showAddLinkRemove() {
                var SingleRemainFV = (document.getElementById("ctl00_ContentPlaceHolder1_Hid_SingleRemainFV").value - 0)

                ClearText()
                if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "F") || (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B")) {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                        document.getElementById("row_Remove").style.display = "none"
                    }
                    else {
                        document.getElementById("row_Remove").style.display = ""
                    }
                }

                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {

                    if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_1").checked == true) {
                        document.getElementById("row_Remove").style.display = "none"
                    }
                    else {
                        document.getElementById("row_Remove").style.display = ""
                    }

                }

                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                    document.getElementById("row_Remove").style.display = "none"
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                    document.getElementById("row_Remove").style.display = "none"
                }


                if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B")) {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true) && document.getElementById("ctl00_ContentPlaceHolder1_srh_BTBDealSlipNo_txt_Name").value != "") {
                        document.getElementById("row_Remove").style.display = ""
                    }
                    else {
                        document.getElementById("row_Remove").style.display = "none"

                    }

                }


                if ((Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_BTBDealSlipNo_txt_Name").value) == "") && (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_1").checked == true)) {

                    document.getElementById("row_Remove").style.display = "none"
                }



                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                    document.getElementById("row_Remove").style.display = "none"
                }
                if ((document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) && (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "F")) {
                    document.getElementById("row_Remove").style.display = "none"
                }


                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "P") {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                        document.getElementById("row_Remove").style.display = "none";
                    }

                }

                if ((document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) && (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_0").checked == true)) {
                    document.getElementById("row_Remove").style.display = "none";
                }
            }

            function securityType() {

                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                    document.getElementById("row_BrokCustId").style.display = "";
                    document.getElementById("row_BrokCustTypeId").style.display = "";

                    document.getElementById("ctl00_ContentPlaceHolder1_row_ConsBRok").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_PaidConsBRok").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_RecConsBRok").style.display = "none";
                    //                document.getElementById("ctl00_ContentPlaceHolder1_row_commchkd").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Bpto").style.display = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Bp").style.display = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_BrFrom").style.display = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Br").style.display = "";
                }
                else {
                    document.getElementById("row_BrokCustId").style.display = "none";
                    document.getElementById("row_BrokCustTypeId").style.display = "none";

                    document.getElementById("ctl00_ContentPlaceHolder1_row_ConsBRok").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_PaidConsBRok").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_RecConsBRok").style.display = "none";
                    //            document.getElementById("ctl00_ContentPlaceHolder1_row_commchkd").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Bpto").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Bp").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_BrFrom").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Br").style.display = "none";

                }

                document.getElementById("row_FinDealType").style.display = "none";

            }

            function RefDealSlipNoe() {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_RefDealSlip_0").checked == true) {
                    document.getElementById("row_RefDealNo").style.display = "";
                    document.getElementById("col_Remark").innerHTML = "Cancel Remark"
                }
                else {
                    document.getElementById("row_RefDealNo").style.display = "none";
                    document.getElementById("col_Remark").innerHTML = "Remark"
                }
            }


            function ReferenceBy() {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_Reference_0").checked == true) {
                    document.getElementById("tr_refrenceBy").style.display = "";
                    document.getElementById("tr_refCustType").style.display = "";
                    document.getElementById("tr_refrenceByDealer").style.display = "";

                }
                else {
                    document.getElementById("tr_refrenceBy").style.display = "none";
                    document.getElementById("tr_refCustType").style.display = "none";
                    document.getElementById("tr_refrenceByDealer").style.display = "none";
                }
            }


            function TotalFaceValue() {
                //var TotalAppAmount;
                //var noofbond;
                //var Nsdlfacevalue;
                //noofbond = document.getElementById("ctl00_ContentPlaceHolder1_txt_NoOfBonds").value
                //Nsdlfacevalue = (document.getElementById("ctl00_ContentPlaceHolder1_Hid_NSDLFaceValue").value - 0)
                //TotalAppAmount = (noofbond * Nsdlfacevalue)
                //document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value = (TotalAppAmount / (document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value))
                //document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value

            }
            function TotalFaceValue(type) {
                var CurrSecFaceValue = $("#<%= Hid_CurrSecFaceValue.ClientID%>").val();
                if (CurrSecFaceValue > 0) {
                    var NoofBond = $("#<%= txt_NoOfBonds.ClientID%>").val();
                    var FaceValue = $("#<%= txt_Amount.ClientID%>").val() * $("#<%= cbo_Amount.ClientID%>").val();

                    if (type == 'B')
                        $("#<%= txt_Amount.ClientID%>").val(CurrSecFaceValue * NoofBond / $("#<%= cbo_Amount.ClientID%>").val());
                 else if (type == 'F')
                     $("#<%= txt_NoOfBonds.ClientID%>").val(FaceValue / CurrSecFaceValue);
            }
            else
                alert('Please select security name first.');
        }
        function TotalNoOfBond() {
            var TotalAppAmount;
            if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value - 0) != 100000) {

                TotalAppAmount = (document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0) * (document.getElementById("ctl00_ContentPlaceHolder1_cbo_Amount").value - 0);
            }
            else {

                TotalAppAmount = ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value - 0) * 10000 * 10);
            }

            if ((document.getElementById("ctl00_ContentPlaceHolder1_Hid_NSDLFaceValue").value - 0) == 0) {

                if ((document.getElementById("ctl00_ContentPlaceHolder1_Hid_NSDLFaceValue").value - 0) == 0) {
                    return TotalAppAmount;
                }
                else {
                    return TotalAppAmount;
                }
            }

            var NoOfBonds = (Math.round(TotalAppAmount, 0) / (document.getElementById("ctl00_ContentPlaceHolder1_Hid_NSDLFaceValue").value - 0));
            document.getElementById("ctl00_ContentPlaceHolder1_txt_NoOfBonds").value = (Math.round(TotalAppAmount, 0) / (document.getElementById("ctl00_ContentPlaceHolder1_Hid_NSDLFaceValue").value - 0));
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_bond").value = document.getElementById("ctl00_ContentPlaceHolder1_txt_NoOfBonds").value;

        }



        function Brokerage() {

            if (document.getElementById("ctl00_ContentPlaceHolder1_chk_Brokerage1").checked == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_row_Bpto").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_Bp").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_BrFrom").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_Br").style.display = "";

                document.getElementById("ctl00_ContentPlaceHolder1_row_ConsBRok").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_PaidConsBRok").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_RecConsBRok").style.display = "none";
                //                    document.getElementById("ctl00_ContentPlaceHolder1_row_commchkd").style.display = "";

            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_row_Bpto").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_Bp").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_BrFrom").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_Br").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_ConsBRok").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_PaidConsBRok").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_RecConsBRok").style.display = "none";
                //                    document.getElementById("ctl00_ContentPlaceHolder1_row_commchkd").style.display =  "none";
            }



            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {

                if (document.getElementById("ctl00_ContentPlaceHolder1_chk_Brokerage1").checked == true) {
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Bpto").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Bp").style.display = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_BrFrom").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_Br").style.display = "";

                    document.getElementById("ctl00_ContentPlaceHolder1_row_ConsBRok").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_PaidConsBRok").style.display = "none";
                    document.getElementById("ctl00_ContentPlaceHolder1_row_RecConsBRok").style.display = "none";
                    //                     document.getElementById("ctl00_ContentPlaceHolder1_row_commchkd").style.display = "none";

                }
            }
            document.getElementById("row_FinDealType").style.display = "none";

        }

        function CheckPhysicalDMAT(blnFlag) {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PhysicalDMAT_1").checked == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_row_SGL").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_Bank").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_Demat").style.display = "none";
                document.getElementById("row_CustSGL").style.display = "";
                document.getElementById("tr_SettleNo").style.display = "none";
                document.getElementById("row_CustDemate").style.display = "none";

                if (blnFlag == true) {

                    document.getElementById("ctl00_ContentPlaceHolder1_row_SGL").style.display = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value = "S"

                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                    document.getElementById("row_CounterCustSGLWith").style.display = "";

                    document.getElementById("row_CounterCustDemate").style.display = "none";
                }
                else {

                    document.getElementById("row_CounterCustSGLWith").style.display = "none";

                    document.getElementById("row_CounterCustDemate").style.display = "none";
                }
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_row_SGL").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_Bank").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_Demat").style.display = "";
                document.getElementById("row_CustSGL").style.display = "none";

                document.getElementById("row_CustDemate").style.display = "";
                if (blnFlag == true) {
                    document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value = "H"
                }

                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
                    document.getElementById("tr_SettleNo").style.display = "";
                }
                else {
                    document.getElementById("tr_SettleNo").style.display = "";
                }

                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                    document.getElementById("row_CounterCustSGLWith").style.display = "none";

                    document.getElementById("row_CounterCustDemate").style.display = "";
                }
                else {

                    document.getElementById("row_CounterCustSGLWith").style.display = "none";

                    document.getElementById("row_CounterCustDemate").style.display = "none";
                }
            }

        }


        function CheckVisibleFalse() {
            if ((document.getElementById("ctl00_ContentPlaceHolder1_Hid_Page").value == 0) && (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B")) {



            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_row_BackOffice").style.display = "";
                document.getElementById("row_BackOfficeheader").style.display = "";

            }

            if (querySt("Page") == "PendingCostMemo.aspx") {

                document.getElementById("ctl00_ContentPlaceHolder1_row_BackOffice").style.display = "";
                document.getElementById("row_BackOfficeheader").style.display = "";
            }
        }







        function ValidationForSaveUpdate() {

            var Dealdate = document.getElementById("ctl00_ContentPlaceHolder1_txt_DealDate")
            var Settdate = document.getElementById("ctl00_ContentPlaceHolder1_txt_SettmentDate")

            if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                    if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_BrokingBTBDealSlipNo_txt_Name").value) == "") {
                        AlertMessage("Validation", "Please Select  BTB DealSlip No", 175, 450, 'D');
                        return false;
                    }
                }
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType").value) == "") {
                AlertMessage("Validation", "Please Select Security Type", 175, 450, 'D');
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_IssuerOfSecurity_txt_Name").value) == "") {
                AlertMessage("Validation", "Please Select Issuer Of Security", 175, 450, 'D');
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_txt_Name").value) == "") {
                AlertMessage("Validation", "Please Select Name of Security", 175, 450, 'D');
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameOFClient_txt_Name").value) == "") {
                AlertMessage("Validation", "Please Select  Name OF Client", 175, 450, 'D');
                return false;
            }


            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_BrokNameOfSeller_txt_Name").value) == "") {
                    AlertMessage("Validation", "Please Select  Name OF Client", 175, 450, 'D');
                    return false;
                }
            }



            var cboBrokNameOfSeller = document.getElementById("ctl00_ContentPlaceHolder1_srh_BrokNameOfSeller")
            if (cboBrokNameOfSeller != null) {
                if ((document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameOFClient_txt_Name").value) = (document.getElementById("ctl00_ContentPlaceHolder1_srh_BrokNameOfSeller_txt_Name").value)) {
                    AlertMessage("Validation", "Buyer and seller name cannot be same.", 175, 450, 'D');
                    return false;
                }
            }



            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_DealDate").value) == "") {
                AlertMessage("Validation", "Please Enter Deal Date ", 175, 450, 'D');
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value) == "") {
                AlertMessage("Validation", "Please Enter Face Value ", 175, 450, 'D');
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value) == 0) {
                AlertMessage("Validation", "Face Value Can not be Zero", 175, 450, 'D');
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value) == "") {
                AlertMessage("Validation", "Please Enter Rate ", 175, 450, 'D');
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value) == 0) {
                AlertMessage("Validation", "Rate Can not be Zero", 175, 450, 'D');
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_SettmentDate").value) == "") {
                AlertMessage("Validation", "Please Settment Date", 175, 450, 'D');
                return false;
            }


            if ((Date.parse(getmdy(Settdate.value))) < (Date.parse(getmdy(Dealdate.value)))) {
                AlertMessage("Validation", 'Settlement Date  should  not  be less than Deal Date', 175, 450, 'D');
                return false;
            }

            var qty = document.getElementById("ctl00_ContentPlaceHolder1_txt_NoOfBonds").value;

            if ((qty - 0) == 0 || qty.indexOf(".") != -1) {
                AlertMessage("Validation", "The Quantity can not be Zero or Decimals", 175, 450, 'D');
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_Company").value) == "") {
                AlertMessage("Validation", "Please Select Company Name", 175, 450, 'D');
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealerName").value) == "") {
                AlertMessage("Validation", "Please Select Dealer Name", 175, 450, 'D');
                return false;
            }
            //if (document.getElementById("ctl00_ContentPlaceHolder1_chk_Brokerage1").checked == true) {
            //    if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_ConsBroker").value) == "") {
            //        AlertMessage("Validation", "Please Select Introducing Broker", 175, 450, 'D');
            //        return false;
            //    }
            //    if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_NilComm_1").checked == true) {
            //        if ((Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Conchargesreceived").value) == "") || (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Conchargesreceived").value) == "0.00")) {
            //            AlertMessage("Validation", "Please enter YTM / YTC to IB", 175, 450, 'D');
            //            return false;
            //        }
            //    }
            //}
            var PurMethod = document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod")
            var lstpurno1 = document.getElementById("ctl00_ContentPlaceHolder1_lst_addmultiple")

            if ((querySt("Page") == "PendingDealSlip.aspx") || (querySt("Page") == "GeneratedDealSlip.aspx")) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {



                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                        if (PurMethod != null) {
                            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true) {

                                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_1").checked == true) {


                                    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "F") {
                                        if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B") {

                                            if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {
                                                if ((document.getElementById("ctl00_ContentPlaceHolder1_srh_BTBDealSlipNo_Hid_SelectedId").value - 0) == 0) {
                                                    alert("Please Select pur Deal Slip No");

                                                    return false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true) {

                                    if (lstpurno1.options.length == 0) {
                                        AlertMessage("Validation", "Please Enter Purchase deal slip no", 175, 450, 'D');
                                        return false;
                                    }
                                    //if (lstpurno1.options.length < 2) {
                                    //    AlertMessage("Validation", "Please Enter two Purchase deal slip no", 175, 450, 'D');
                                    //    return false;
                                    //}
                                }
                            }

                        }


                    }
                }
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
                if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value != "N") && (document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value != "B") && (document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value != "L")) {
                    //if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_Demat").value == "") {
                    //    AlertMessage("Validation", 'Please select Our Demat', 175, 450, 'D');
                    //    return false;
                    //}


                    //if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustDemate").value == "") {
                    //    AlertMessage("Validation", 'Please select Customer Demat', 175, 450, 'D');
                    //    return false;
                    //}
                    //if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerBank").value == "") {
                    //    AlertMessage("Validation", 'Please select Customer Bank', 175, 450, 'D');
                    //    return false;
                    //}
                }
            }

            return true;
        }


        function CostMemoReadonly() {

            document.getElementById("row_SelectMethod").style.display = "none";
            document.getElementById("row_PurMethod").style.display = "none";
            document.getElementById("row_addmultiple").style.display = "none";
            document.getElementById("row_BTB").style.display = "none";

        }






        function ClearText() {

            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_1").checked == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_srh_BTBDealSlipNo_txt_Name").value = ""
                document.getElementById("row_Remove").style.display = "none"
            }

        }


        function PaymentMode() {

            var dematval = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Demat").value
            if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value == "N") || (document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value == "B") || (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") || (document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value == "L")) {
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_Demat").value = 0
            }

            else {
                var cbodematname = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Demat")
                var cbodemat = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Demat").value

                if (cbodemat != "") {
                    document.getElementById("ctl00_ContentPlaceHolder1_Hid_Demat").value = cbodemat

                }

                if (cbodemat != "") {
                    document.getElementById("ctl00_ContentPlaceHolder1_cbo_Demat").value = cbodemat
                }
                else {
                    if (cbodemat != null) {
                        document.getElementById("ctl00_ContentPlaceHolder1_cbo_Demat").value = dematval
                    }
                }


            }

        }



    </script>

    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0px">
        <tr align="left">
            <td class="SectionHeaderCSS">Deal Slip Entry
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td align="center" valign="top" colspan="2">
               
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table id="Table3" cellspacing="0" cellpadding="0" border="1" width="90%">
                            <tr align="center" valign="top">
                                <td valign="top" style="width: 49%;">
                                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tr align="left">
                                            <td>Type OF Transaction:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rbl_TypeOFTranction" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Selected="True" Value="P">To Purchase</asp:ListItem>
                                                    <asp:ListItem Value="S">To Sell</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" runat="server" visible="true">
                                            <td>Deal Slip Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rbl_DealSlipType" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="C">Cost Memo</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="D">Deal</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" style="display: none">
                                            <td>Reference Deal Slip:
                                            </td>
                                            <td id="rbl_RefDealSlips" style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rbl_RefDealSlip" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="M">Modified</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="N">New</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_RefDealNo">
                                            <td>Reference Deal Slip No:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <%--<uc:Search ID="srh_RefDealSlipNo" runat="server" AutoPostback="false" ProcName="ID_SEARCH_RefDealSlipNo"
                                                    SelectedFieldName="DealSlipNo" SourceType="StoredProcedure" TableName="SecurityMaster"
                                                    ConditionalFieldName="TransType" FormHeight="400" FormWidth="800" ConditionalFieldId="rbl_TypeOFTranction"
                                                    ConditionExist="true" Width="180"></uc:Search>--%>
                                                <uc:Search ID="srh_RefDealSlipNo" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="SecurityName" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Deal Trans Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_DealTransType" runat="server" CssClass="ComboBoxCSS" Width="188px"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Text="Trading" Value="T"></asp:ListItem>

                                                </asp:DropDownList>
                                                <i style="color: red">*</i>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_DealType" runat="server" visible="false">
                                            <td>Deal Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_DealType" runat="server" CssClass="ComboBoxCSS" Width="188px"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Text="Normal" Value="N"></asp:ListItem>

                                                </asp:DropDownList>
                                                <i style="color: red">*</i>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_FinDealType">
                                            <td>Financial Deal Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_FinancialDealType" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Selected="True" Value="N">Normal</asp:ListItem>
                                                    <asp:ListItem Value="F">From Purchase</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Deal Slip No:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_DealSlipNo" runat="server" Width="180px" CssClass="TextBoxCSS"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>SecurityType:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SecurityType" runat="server" Width="188px" CssClass="ComboBoxCSS"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td class="LabelCSS">Issuer Of Security:
                                            </td>
                                            <td>
                                                <uc:Search ID="srh_IssuerOfSecurity" Width="175" runat="server" AutoPostback="true" SelectedFieldId="Id" SelectedFieldName="SecurityIssuer"
                                                    PageName="NameOfIssuer">
                                                </uc:Search>
                                            </td>
                                        </tr>
                                        <tr align="left" id="Tr8">
                                            <td>Deal Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_RedeemedSec" runat="server" CellPadding="0" onChange="getRdoRedeemedSec();"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Selected="True" Value="N">Normal</asp:ListItem>
                                                    <asp:ListItem Value="R">Redemption</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Market Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_MarketType" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" onclick="ShowHideMarking()">
                                                    <asp:ListItem Text="Primary" Value="P"></asp:ListItem>
                                                    <asp:ListItem Text="Secondary" Value="S" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Name of Security:
                                            </td>
                                            <td>
                                                <uc:Search ID="Srh_NameofSecurity" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="SecurityName" ConditionalFieldName="SecurityIssuer"
                                                    ConditionalFieldId="srh_IssuerOfSecurity" ConditionExist="true"
                                                    ConditionalFieldId1="Hid_RedeemedFlag" ConditionalFieldName1="RedeemedDeal" FormWidth="800" />
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="Tr9" runat="server">
                                            <td class="LabelCSS" width="200px">ISIN :
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="txt_ISIN" runat="server" Width="165px" Height="14px" CssClass="LabelCSS"
                                                    MaxLength="30"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trRedemption" runat="server" align="left" valign="top">
                                            <td id="lbl_redem" runat="server" visible="false">Redemption (if any):
                                            </td>
                                            <td>
                                                <asp:DataGrid ID="dgRedemptionDetails" runat="server" CssClass="GridCSS"
                                                    AutoGenerateColumns="false" Width="55%">
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                    <Columns>
                                                        <asp:BoundColumn HeaderText="Dated" DataField="Dated" HeaderStyle-Width="50%" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-CssClass="dealslipno"></asp:BoundColumn>
                                                        <asp:BoundColumn HeaderText="Amount" DataField="Amount" HeaderStyle-Width="50%" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Tax Free(Step Up/Step Down):
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_TaxFree" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Retail</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">Corporate</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_BrokingBTB">
                                            <td>Select Broking pur No:
                                            </td>
                                            <td style="padding-left: 0px;">
                                             

                                                <uc:Search ID="srh_BrokingBTBDealSlipNo" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="SecurityName" />
                                                <asp:Button ID="btn_ShowBrokPurdeal" runat="server" CssClass="ButtonCSS" Visible="false"
                                                    Text="Show" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Customer Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_CustomerType" Width="188px" runat="server" CssClass="ComboBoxCSS"
                                                    AutoPostBack="True" TabIndex="1">
                                                </asp:DropDownList>
                                                <i style="color: Red; vertical-align: super;">*</i>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td id="Client">Name OF Client:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:Search ID="Srh_NameOFClient" runat="server" PageName="NameOFClient" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="CustomerName" FormWidth="800" />
                                            </td>
                                        </tr>
                                        <tr id="Tr10" runat="server">
                                            <td class="LabelCSS" width="200px">PAN:
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lbl_PAN" runat="server" Width="165px" Height="14px" CssClass="LabelCSS"
                                                    MaxLength="30"></asp:Label>
                                            </td>
                                        </tr>
                                       
                                        <tr align="left" id="row_BrokCustTypeId">
                                            <td>Customer Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_BrokCustomerType" Width="188px" runat="server" CssClass="ComboBoxCSS"
                                                    AutoPostBack="True" TabIndex="1">
                                                </asp:DropDownList>
                                                <i style="color: Red; vertical-align: super;">*</i>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_BrokCustId">
                                            <td id="Seller">Name OF Seller:
                                            </td>
                                            <td style="padding-left: 0px;">
                                       

                                                <uc:Search ID="srh_BrokNameOfSeller" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="SecurityName" />
                                            </td>
                                        </tr>
                                    
                                        <tr align="left" id="tr_Emp">
                                            <td>Contact Person:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                
                                                <uc:SelectFields ID="srh_ContactPerson" runat="server" FormHeight="475" FormWidth="257"
                                                    ProcName="ID_SEARCH_Contactperson_TC" SelectedFieldName="ContactPerson" ChkLabelName=""
                                                    LabelName="" SelectedValueName="ContactDetailId" SourceType="StoredProcedure" ShowLabel="false"
                                                    ConditionalFieldId="Srh_NameOFClient" ConditionalFieldName="CustomerId">
                                                </uc:SelectFields>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Contact Person:
                                            </td>
                                           
                                            <td>
                                                <asp:TextBox ID="txt_ContactPerson" Rows="3" runat="server" CssClass="TextBoxCSS"
                                                    TextMode="MultiLine" Width="180px"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_Countcustperson">
                                            <td>Count Contact Person:
                                            </td>
                                            <td style="padding-left: 0px;">
                                           

                                                <uc:SelectFields ID="srh_CountContactPerson" runat="server" FormHeight="475" FormWidth="257"
                                                    ProcName="ID_SEARCH_Contactperson_TC" SelectedFieldName="ContactPerson" ChkLabelName=""
                                                    LabelName="" SelectedValueName="ContactDetailId" SourceType="StoredProcedure" ShowLabel="false"
                                                    ConditionalFieldId="srh_BrokNameOfSeller" ConditionalFieldName="CustomerId">
                                                </uc:SelectFields>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_CountcustpersonTextbox">
                                            <td>Count Contact Person:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_CountContactPerson" Rows="3" runat="server" CssClass="TextBoxCSS"
                                                    TextMode="MultiLine" Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left" id="Tr3">
                                            <td id="Td2">Select Address:
                                            </td>
                                            <td style="padding-left: 0px;">
                                              
                                                <uc:Search ID="srh_SelectAddress" runat="server" PageName="CustomerAddress" AutoPostback="true" ConditionalFieldId="Srh_NameOFClient" ConditionalFieldName="CustomerId"
                                                    SelectedFieldId="Id" SelectedFieldName="Address" />
                                            </td>

                                        </tr>
                                        <tr style="padding-left: 0px;" align="left" id="tr_SelectCountAddress">
                                            <td class="LabelCSS" id="Td5">Select Count Cust Address:
                                            </td>
                                            <td style="padding-left: 0px;">
                                              

                                                <uc:Search ID="srh_CountSelectAddress" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="SecurityName" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                       
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_Brokerage1" Text="Brokerage Entry" runat="server" />
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_ConsBRok" runat="server" style="display: none">
                                            <td>Consultant Broker:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_ConsBroker" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_PaidConsBRok" runat="server">
                                            <td>Consultancy Paid :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Conchargespaid" Width="180px" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_RecConsBRok" runat="server">
                                            <td>Consultancy received :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Conchargesreceived" Width="180px" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_PrintMaturity" runat="server" style="display: none">
                                            <td>Commission NIL:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_NilComm" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_commchkd" style="display: none">
                                            <td>Commission Checked:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_CommChecked" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Bpto" runat="server">
                                            <td>Brokerage Paid To:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_BrokeragePaidTo" runat="server" CssClass="ComboBoxCSS"
                                                    Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Brokerage on:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_BrokerageRateAmt" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="true"  >
                                                    <asp:ListItem Text="Amount" Value="A" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Rate" Value="R" ></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:TextBox ID="txt_BrokerageRate" Width="50px" runat="server" CssClass="TextBoxCSS" AutoPostBack ="true" >0</asp:TextBox>

                                            </td>

                                        </tr>
                                        <tr align="left" id="tr_SellerBrokeragePaid" runat="server">
                                            <td>Buyer Brokerage Paid:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SellBrokeragePaid" Width="180px" runat="server" CssClass="TextBoxCSS">0</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_SellerBrokerageReceived" runat="server">
                                            <td>Buyer Brokerage Received:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SellBrokeragereceived" Width="180px" runat="server" CssClass="TextBoxCSS">0</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Bp" runat="server">
                                            <td>Seller Brokerage Paid:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_BrokeragePaid" Width="180px" runat="server" CssClass="TextBoxCSS">0</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_BrFrom" runat="server">
                                            <td>Brokerage Rec From:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_BrokeragereceivedFrom" runat="server" CssClass="ComboBoxCSS"
                                                    Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Br" runat="server">
                                            <td>Seller Brokerage Received:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Brokeragereceived" Width="180px" runat="server" CssClass="TextBoxCSS">0</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left" runat="server" visible="false">
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_SettCharges" Text=" Charges In Settlement:" runat="server" />
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_SettBrok" runat="server" visible="false">
                                            <td>Dealer Amount:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_DealerAmt" Width="180px" runat="server" CssClass="TextBoxCSS">0</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_SettOthrChrgs" runat="server" visible="false">
                                            <td>Other Charges:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SettOtherChrgs" Width="180px" runat="server" CssClass="TextBoxCSS">0</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="row_HoldingCost" align="left" runat="server" visible="false">
                                            <td style="height: 20px">Holding Cost:&nbsp;
                                            </td>
                                            <td style="height: 20px">
                                                <asp:Label ID="txt_HoldingCost" runat="server" CssClass="LabelCSS" Width="180px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="row_IntraCost" align="left" runat="server" visible="false">
                                            <td style="height: 20px">Intraday Cost:
                                            </td>
                                            <td style="height: 20px">
                                                <asp:Label ID="txt_IntraDayCost" runat="server" CssClass="LabelCSS" Width="180px"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 2%;">&nbsp;
                                </td>
                                <td valign="top" style="width: 49%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                        <tr align="left">
                                            <td>Deal Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_DealDate" Width="115px" runat="server" CssClass="TextBoxCSS jsdate"
                                                    onchange="javascript:FillSettlementDate();" onblur="javascript:FillSettlementDate();" AutoPostBack="true"></asp:TextBox>

                                                <%-- <img class="formcontent" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_DealDate',this);"
                                                    id="IMG1" runat="server" />--%>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Sett Date T +:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SettDay" AutoPostBack="true" Width="40px" runat="server"
                                                    CssClass="ComboBoxCSS" TabIndex="1" onchange="javascript:FillSettlementDate();">
                                                    <asp:ListItem Value="0" Selected="True">0</asp:ListItem>
                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                    <asp:ListItem Value="3">3</asp:ListItem>
                                                    <asp:ListItem Value="4">4</asp:ListItem>
                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                    <asp:ListItem Value="6">6</asp:ListItem>
                                                    <asp:ListItem Value="7">7</asp:ListItem>
                                                    <asp:ListItem Value="8">8</asp:ListItem>
                                                    <asp:ListItem Value="9">9</asp:ListItem>
                                                </asp:DropDownList>
                                                Sett Date:
                                                <%--<asp:Label ID="lbl_SettDate" runat="server" CssClass="LabelCSS"></asp:Label>--%>
                                                <asp:TextBox ID="txt_SettmentDate" Width="75px" runat="server" CssClass="TextBoxCSS"
                                                    Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td class="LabelCSS">
                                                Settlement Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SettmentDate" Width="158" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                                                <img class="formcontent" height="14" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_SettmentDate',this);"
                                                    width="15" border="0" style="vertical-align: top; cursor: hand;" id="IMG2" runat="server">
                                            </td>
                                        </tr> --%>
                                        <tr align="left">
                                            <td>Select Option:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_SelectOpt" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="B">No of Bonds</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="F">Face Value</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Face Value:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Amount" Width="75px" runat="server" CssClass="TextBoxCSS" onblur="javascript:TotalFaceValue('F');"></asp:TextBox>
                                                <asp:DropDownList ID="cbo_Amount" runat="server" CssClass="ComboBoxCSS" Width="100px" onchange="javascript:TotalFaceValue('F');">
                                                    <asp:ListItem Text="RUPEES" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="THOUSANDS" Value="1000"></asp:ListItem>
                                                    <asp:ListItem Text="LACS" Value="100000" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="CRORES" Value="10000000"></asp:ListItem>
                                                </asp:DropDownList>
                                                <i style="color: Red; vertical-align: super;">*</i>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_NoOfBonds" runat="server">
                                            <td>No. Of Bonds:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NoOfBonds" runat="server" Width="180px" onblur="javascript:TotalFaceValue('B');"
                                                    Text="0" CssClass="TextBoxCSS" MaxLength="20" onkeypress="javascript:return OnlyIntegerKey(event);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Rate:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Rate" runat="server" Width="100px" onkeypress="javascript: OnlyDecimal();"
                                                    CssClass="TextBoxCSS"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                                <asp:Button ID="btn_CalRate" runat="server" Text="Calc Rate" ToolTip="Calculate Rate"
                                                    CssClass="ButtonCSS hidden" />
                                                <input type="button" id="btn_CalRate1" runat="server" value="Calc Rate" class="ButtonCSS hidden" onclick="ShowYieldCalculation();" />
                                                <input type="button" id="Button1" runat="server" value="Calc Rate" class="ButtonCSS" onclick="return ShowYieldCalculation();" />

                                            </td>
                                        </tr>
                                        <tr id="Tr11" align="left" runat="server">
                                            <td>Yield:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Yield" runat="server" Width="100px" onkeypress="javascript: OnlyDecimal();"
                                                    CssClass="TextBoxCSS"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                            </td>
                                        </tr>

                                        <tr align="left" id="row_CutOffRate" runat="server" visible="true">
                                            <td>Cut Off Rate:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_CutOffRate" runat="server" Width="100px" onkeypress="javascript: OnlyDecimal();"
                                                    CssClass="TextBoxCSS"></asp:TextBox>
                                            </td>
                                        </tr>
                                         <tr id="Tr14" align="left" runat="server">
                                            <td>Cut off Yield:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_CutOffYield" runat="server" Width="100px" onkeypress="javascript: OnlyDecimal();"
                                                    CssClass="TextBoxCSS"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                            </td>
                                        </tr>
                                        <tr id="tr_StaggMat" visible="false" runat="server">
                                            <td class="LabelCSS">Calculate In:
                                            </td>
                                            <td>
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_StaggMat" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="true">
                                                    <asp:ListItem Value="P">ParValue</asp:ListItem>
                                                    <asp:ListItem Value="F" Selected="True">FaceValue</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Mode of Delivery:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList ID="rdo_PhysicalDMAT" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS">
                                                    <asp:ListItem Value="D" Selected="True">DMAT</asp:ListItem>
                                                    <asp:ListItem Value="S">SGL</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:RadioButtonList ID="rdo_AccIntDays" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS" Visible="false">
                                                    <asp:ListItem Value="3" Selected="True">365</asp:ListItem>
                                                    <asp:ListItem Value="2">366</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_CustSGL">
                                            <td>Customer SGL With:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_CustSGL" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_CounterCustSGLWith">
                                            <td>Counter Cust SGL With:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_CounterCustSGLWith" runat="server" CssClass="ComboBoxCSS"
                                                    Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Mode OF Payment:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_ModeOfPayment" runat="server" CssClass="ComboBoxCSS" Width="188px"
                                                    AutoPostBack="false">
                                                    <%--<asp:ListItem Value="H">HIGH VALUE CHEQUE</asp:ListItem>--%>
                                                    <%--<asp:ListItem Value="T">TRANSFER CHEQUE</asp:ListItem>
                                                    <asp:ListItem Value="C">NORMAL CHEQUE</asp:ListItem>--%>
                                                    <asp:ListItem Value="R">RTGS</asp:ListItem>
                                                    <asp:ListItem Value="E">NEFT</asp:ListItem>
                                                    <asp:ListItem Value="S">SGL</asp:ListItem>
                                                    <asp:ListItem Value="N">RTGS-NSCCL-Settlement</asp:ListItem>
                                                    <%--<asp:ListItem Value="B">RTGS-BSE-ICCL-Settlement </asp:ListItem>
                                                    <asp:ListItem Value="L">RTGS-BSE-Settelement</asp:ListItem>--%>
                                                    <asp:ListItem Value="B">RTGS-ICCL-Settlement </asp:ListItem>
                                                    <%--<asp:ListItem Value="L">RTGS-ICCL-NSDL-Settelement</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Bank" runat="server">
                                            <td>Our Bank:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_Bank" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_SGL" runat="server">
                                            <td>Our SGL With:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SGLWith" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Demat" runat="server">
                                            <td>Our Demat:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_Demat" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_CustDemate">
                                            <td>Customer Demat:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_CustDemate" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_CounterCustDemate">
                                            <td>Counter Cust Demat:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="Cbo_CounterCustDemat" runat="server" CssClass="ComboBoxCSS"
                                                    Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="Tr2">
                                            <td>Customer Bank:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_CustomerBank" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_CounterCustomerBank">
                                            <td>Counter Customer Bank:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SellerCustomerBank" runat="server" CssClass="ComboBoxCSS"
                                                    Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_SettleNo">
                                            <td>SettleNo:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SettleNo" runat="server" CssClass="TextBoxCSS" Width="180px"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Sett Terms & Cond:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SettTurms" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                    <asp:ListItem Value="D">DVP</asp:ListItem>
                                                    <asp:ListItem Value="S">SAME DAY STOCK</asp:ListItem>
                                                    <asp:ListItem Value="A">ONE DAY ADVANCE</asp:ListItem>
                                                    <asp:ListItem Value="N">NON DVP</asp:ListItem>
                                                    <asp:ListItem Value="F">FUNDS AGAINST DMAT SLIP</asp:ListItem>
                                                    <asp:ListItem Value="R">FTK AGAINST CLEAR FUNDS</asp:ListItem>
                                                    <asp:ListItem Value="C">NSCCL</asp:ListItem>
                                                    <asp:ListItem Value="I">ICDM</asp:ListItem>
                                                    <%--  <asp:ListItem Value="D">DVP</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_SellerDealerName">
                                            <td visible="false">Buyer Dealer Name:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SellerDealerName" runat="server" CssClass="ComboBoxCSS"
                                                    Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                            <%-- <asp:Label ID="Label2" Visible="false" runat="server" Width="0" CssClass="LabelCSS"></asp:Label><td>
                                            </td>--%>
                                        </tr>
                                        <tr align="left">
                                            <td visible="false">Seller Dealer Name:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_DealerName" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_Dealar" Visible="false" runat="server" Width="0" CssClass="LabelCSS"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Company Name:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_Company" runat="server" CssClass="ComboBoxCSS" Width="188px"
                                                    Enabled="false">
                                                </asp:DropDownList>
                                                <i style="color: Red; vertical-align: super;">*</i>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td id="Td1">Reported By:
                                            </td>
                                            <%--<td>
                                                <asp:TextBox ID="txt_ReportedBy" Height="35px" runat="server" CssClass="TextBoxCSS"
                                                    TextMode="MultiLine" Width="183px" TabIndex="20"></asp:TextBox>&nbsp;
                                            </td>--%>
                                            <td>
                                                <asp:DropDownList ID="cbo_ReportedBy" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td id="col_Remark">Remark:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_CancelRemark" Rows="3" runat="server" CssClass="TextBoxCSS"
                                                    TextMode="MultiLine" Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Comment:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Comment" Rows="3" runat="server" CssClass="TextBoxCSS" TextMode="MultiLine"
                                                    Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr align="left">
                                            <td style="padding-left: 0px;">Reference:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rbl_Reference" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        <tr align="left" id="tr_PreviousdealType" runat="server" visible="false">
                                            <td>Previous deal Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_PreviousdealType" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_calcXInt" runat="server" visible="false">
                                            <td>calculate the Ex-Interest:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_calcXInt" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="True">
                                                    <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr7" runat="server" style="display: none">
                                            <td>Deal Acknowledged:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DealAck" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="True">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        <tr align="left" id="tr12" runat="server">
                                            <td>TCS Applicable
                                            </td>
                                            <td>
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_TCSApplicable" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="True">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr13" runat="server">
                                            <td>TDS Applicable
                                            </td>
                                            <td>
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_TDSApplicable" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="True">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_refCustType">
                                            <td>Customer Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_refCustType" Width="188px" runat="server" CssClass="ComboBoxCSS"
                                                    AutoPostBack="True" TabIndex="1">
                                                </asp:DropDownList>
                                                <i style="color: Red; vertical-align: super;">*</i>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_refrenceBy">
                                            <td>Reference By:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:Search ID="Srh_ReferenceBy" runat="server" PageName="NameOFClient" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="CustomerName" />
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_refrenceByDealer">
                                            <td visible="false">Reference By Dealer:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_ReferenceByDealer" runat="server" CssClass="ComboBoxCSS"
                                                    Width="188px">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label1" Visible="false" runat="server" Width="0" CssClass="LabelCSS"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr id="row_BackOfficeheader" align="center">
                                <td class="HeadingCenter" colspan="3">Back Office Section
                                </td>
                            </tr>
                            <tr align="center" valign="top" id="row_BackOffice1">
                                <td id="row_BackOffice" runat="server">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center">
                                        <tr align="left" id="Tr1">
                                            <td>Our Exchange:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_Exchange" runat="server" CssClass="ComboBoxCSS" Width="188px"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td id="PreInterest">Coupon Received:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_PreviousInterest" runat="server" CssClass="TextBoxCSS" Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td id="Td3">Coupon Paid:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_CouponPaid" runat="server" CssClass="TextBoxCSS" Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Time Of Deal:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_hr" runat="server" Width="45px" CssClass="ComboBoxCSS">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cbo_minute" runat="server" Width="45px" CssClass="ComboBoxCSS">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cbo_DealSecond" runat="server" Width="45px" CssClass="ComboBoxCSS" Visible="false">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <table cellspacing="0" cellpadding="0" border="0" align="center">
                                        <tr align="left" id="row_SelectMethod">
                                            <td>Select Method:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList Enabled="false" RepeatLayout="Flow" ID="rbl_DealType" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="F" Enabled="false">FIFO</asp:ListItem>
                                                    <asp:ListItem Value="M" Selected="True">Manual</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_PurMethod">
                                            <td>Select Purchase Mtd:
                                            </td>
                                            <td align="left" style="padding: 0px;">
                                                <table cellspacing="0" cellpadding="0" border="0">
                                                    <tr align="left">
                                                        <td style="padding-left: 0px;">
                                                            <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_PurMethod" runat="server" CellPadding="0"
                                                                CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                                <asp:ListItem Selected="True" Value="S">Single</asp:ListItem>
                                                                <asp:ListItem Value="M">Multiple</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td id="row_lnkadd">
                                                            <asp:LinkButton ID="lnk_add" runat="server" Text="Add Pur" CssClass="InfoLinkCSS hidden"
                                                                CommandName="add">
                                                            </asp:LinkButton>
                                                            <input type="button" id="btn_AddPur" class="ButtonCSS" value="..." onclick="javascript: return AddMarking();" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_addmultiple">
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:ListBox ID="lst_addmultiple" runat="server" Width="148px" Height="45px" CssClass="fieldcontent"
                                                    DataTextField="DealSlipNo" DataValueField="DealSlipID" TabIndex="11"></asp:ListBox>
                                                <br />

                                            </td>
                                        </tr>
                                        <tr align="left" id="row_RemoveMarking" runat="server" visible="false">
                                            <td>Remove Marking
                                            </td>
                                            <td>
                                                <asp:Button ID="btnRemoveMarking" runat="server" Text="Remove" CssClass="ButtonCSS"
                                                    Visible="false" />

                                            </td>

                                        </tr>
                                        <tr align="left" id="row_BTB">
                                            <td>Select pur DealNo:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <%--   <uc:Search ID="srh_BTBDealSlipNo" runat="server" AutoPostback="true" ProcName="ID_SEARCH_BTBDealSlipNo"
                                                    CheckYearCompany="false" CheckCompany="True" SelectedFieldName="DealSlipNo" SourceType="StoredProcedure"
                                                    TableName="SecurityMaster" ConditionExist="true" ConditionalFieldName="SM.SecurityId"
                                                    FormHeight="380" FormWidth="800" ConditionalFieldId="Srh_NameofSecurity" Width="100"
                                                    ConditionalFieldName1="DSE.DealTransType" ConditionalFieldId1="cbo_DealTransType"></uc:Search>--%>

                                                <%-- <uc:Search ID="srh_BTBDealSlipNo" runat="server" PageName="BTBDealSlipNo" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo" ConditionExist="true" CheckCompany="True" ConditionalFieldId="Srh_NameofSecurity"
                                                    ConditionalFieldName="SecurityId" ConditionalFieldName1="DealTransType" ConditionalFieldId1="cbo_DealTransType"  />
                                                <asp:Button ID="btn_ShowPur" runat="server" CssClass="ButtonCSS" Visible="false"
                                                    Text="Show" />--%>
                                                <table>
                                                    <tr>
                                                        <td style="padding: 0px;">
                                                            <uc:Search ID="srh_BTBDealSlipNo" runat="server" PageName="BTBDealSlipNo" AutoPostback="true"
                                                                SelectedFieldId="Id" SelectedFieldName="DealSlipNo" ConditionExist="true" CheckCompany="True" ConditionalFieldId="Srh_NameofSecurity"
                                                                ConditionalFieldName="SecurityId" ConditionalFieldName1="DealTransType" ConditionalFieldId1="cbo_DealTransType" />
                                                        </td>
                                                        <td style="padding: 0px;">
                                                            <asp:Button ID="btn_ShowPur" runat="server" CssClass="ButtonCSS" Visible="false"
                                                                Text="Show" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Remove">
                                            <td>Remove:
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="Btn_Remove" runat="server" CssClass="ButtonCSS" Text="Remove" />
                                            </td>
                                        </tr>
                                        <tr align="center" id="">
                                            <td colspan="2">
                                                <asp:Label ID="lbl_Msg" runat="server" CssClass="LabelCSS" Text="" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr align="center" id="tr_interestsection">
                                <td class="HeadingCenter" colspan="3">Interest Section
                                </td>
                            </tr>
                            <tr id="tr_interestsectionTable" align="center" valign="top">
                                <td colspan="3">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="95%">
                                        <tr align="left" id="Tr4">
                                            <td>Amount:
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Amount" runat="server" Text=" "></asp:Label>
                                            </td>
                                            <td>Interest Days:
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_InterestDays" runat="server" Text=" "></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left" id="Tr5">
                                            <td>Interest Amt:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_InterestAmt" runat="server" Text=" " onkeypress="javascript: OnlyDecimalAndMinus();"
                                                    onchange="addInterest();" CssClass="TextBoxCSS"></asp:TextBox>
                                                <%--<asp:Label ID="lbl_InterestAmt" runat="server" Text=" "></asp:Label>--%>
                                            </td>
                                            <td>InterestFromToDates:
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_InterestFromToDates" runat="server" Text=" "></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left" id="Tr6">
                                            <td>SettlementAmt:
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_SettlementAmt" runat="server" Text=" "></asp:Label>
                                            </td>
                                            <td>IP Dates:
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_IPDates" runat="server" Text=" "></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Stamp Duty:
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_StampDuty" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>TCS:
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_TCSAmount" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>TDS:
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_TDSAmount" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Total SettlementAmt:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="lbl_TotalSettlementAmt" runat="server" onkeypress="javascript: OnlyDecimal();"
                                                    CssClass="TextBoxCSS"></asp:TextBox>
                                            </td>
                                            <td>Round off:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_roundoff" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" onclick="CalcRoundofsettAMT();">
                                                    <asp:ListItem Value="Y" Selected="True">+</asp:ListItem>
                                                    <asp:ListItem Value="N">-</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:TextBox ID="txt_Roundoff" runat="server" Width="106px" onkeypress="javascript: OnlyDecimal();"
                                                    CssClass="TextBoxCSS"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Update" Visible="false" runat="server" Text="Update" ToolTip="Update"
                                        CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_ConvertToDeal" runat="server" Text="ConvertToDeal" ToolTip="ConvertToDeal"
                                        CssClass="ButtonCSS" Width="100px" />
                                    <asp:Button ID="btn_savegenerateDeal" runat="server" Text="Save & Generate Deal Confirmation"
                                        ToolTip="Save & Generate Deal" CssClass="ButtonCSS" Width="200px" />
                                    <asp:Button ID="btn_ShowSecurity" runat="server" Text="Show Security" ToolTip="Show Security"
                                        CssClass="ButtonCSS" Width="110px" />
                                    <asp:Button ID="btn_ShowCustomer" runat="server" Text="Show Customer" ToolTip="Show Security"
                                        CssClass="ButtonCSS" Width="110px" />
                                    <asp:Button ID="btn_SGLFedFormat" runat="server" CssClass="ButtonCSS" Visible="false"
                                        Text="SGL Federal Format" Width="130px" />
                                    <asp:Button ID="btn_SGLHDFCFormat" runat="server" CssClass="ButtonCSS" Visible="false"
                                        Text="SGL HDFC Format" Width="130px" />
                                    <asp:Button ID="btn_dealconf" runat="server" CssClass="ButtonCSS" Visible="false"
                                        Text="PrintDealConf" Width="110px" />
                                    <%--<asp:Button ID="btn_ShowSecurity" runat="server" CssClass="ButtonCSS" Text="Show Security" />--%>
                                    <asp:HiddenField ID="Hid_CompId" runat="server" />
                                    <asp:HiddenField ID="Hid_FirstInterestDate" runat="server" />
                                    <asp:HiddenField ID="Hid_CouponRate" runat="server" />
                                    <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                                    <asp:HiddenField ID="Hid_DealerName" runat="server" />
                                    <asp:HiddenField ID="Hid_NSDLFaceValue" runat="server" />
                                    <asp:HiddenField ID="Hid_NoOfBond" runat="server" />
                                    <asp:HiddenField ID="Hid_QuoteId" runat="server" />
                                    <asp:HiddenField ID="Hid_bond" runat="server" />
                                    <asp:HiddenField ID="Hid_MatDate" runat="server" />
                                    <asp:HiddenField ID="Hid_MatAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_CoupDate" runat="server" />
                                    <asp:HiddenField ID="Hid_CoupRate" runat="server" />
                                    <asp:HiddenField ID="Hid_InterestDate" runat="server" />
                                    <asp:HiddenField ID="Hid_BookClosureDate" runat="server" />
                                    <asp:HiddenField ID="Hid_GovernmentFlag" runat="server" />
                                    <asp:HiddenField ID="Hid_Issue" runat="server" />
                                    <asp:HiddenField ID="Hid_DMATBkDate" runat="server" />
                                    <asp:HiddenField ID="Hid_Frequency" runat="server" />
                                    <asp:HiddenField ID="Hid_Amtshow" runat="server" />
                                    <asp:HiddenField ID="Hid_ShowInterest" runat="server" />
                                    <asp:HiddenField ID="Hid_IntDays" runat="server" />
                                    <asp:HiddenField ID="Hid_SettlementAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_FinlSettlementAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_Quantity" runat="server" />
                                    <asp:HiddenField ID="Hid_Amt" runat="server" />
                                    <asp:HiddenField ID="Hid_AddInterest" runat="server" />
                                    <asp:HiddenField ID="Hid_InterestFromTo" runat="server" />
                                    <asp:HiddenField ID="HiddenField5" runat="server" />
                                    <asp:HiddenField ID="Hid_FinalAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_Round" runat="server" />
                                    <asp:HiddenField ID="Hid_CallDate" runat="server" />
                                    <asp:HiddenField ID="Hid_CallAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_PutDate" runat="server" />
                                    <asp:HiddenField ID="Hid_PutAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_FaceValue" runat="server" />
                                    <asp:HiddenField ID="Hid_Days" runat="server" />
                                    <asp:HiddenField ID="Hid_UserId" runat="server" />
                                    <asp:HiddenField ID="Hid_dealdone" runat="server" />
                                    <asp:HiddenField ID="Hid_Page" runat="server" />
                                    <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                                    <asp:HiddenField ID="Hid_LastIPDate" runat="server" />
                                    <asp:HiddenField ID="Hid_FrequencyOfInterest" runat="server" />
                                    <asp:HiddenField ID="Hid_CostMemoNo" runat="server" />
                                    <asp:HiddenField ID="Hid_CostMemoPageName" runat="server" />
                                    <asp:HiddenField ID="Hid_RetValues" runat="server" />
                                    <asp:HiddenField ID="Hid_PurchaseDealSlipId" runat="server" />
                                    <asp:HiddenField ID="Hid_DealPurcId" runat="server" />
                                    <asp:HiddenField ID="Hid_DealSlipNo" runat="server" />
                                    <asp:HiddenField ID="Hid_SingleRemainFV" runat="server" />
                                    <asp:HiddenField ID="Hid_RemainingFaceValue" runat="server" />
                                    <asp:HiddenField ID="Hid_CostMemoFlag" runat="server" />
                                    <asp:HiddenField ID="Hid_CustomerIdbroking" runat="server" />
                                    <asp:HiddenField ID="Hid_SellBrokDealSlipId" runat="server" />
                                    <asp:HiddenField ID="Hid_PurBrokDealSlipId" runat="server" />
                                    <asp:HiddenField ID="Hid_TotalSettlementAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_BTOBid" runat="server" />
                                    <asp:HiddenField ID="Hid_UserTypeId" runat="server" />
                                    <asp:HiddenField ID="Hid_IntRecvable" runat="server" />
                                    <asp:HiddenField ID="Hid_PurIds" runat="server" />
                                    <asp:HiddenField ID="Hid_RefDealId" runat="server" />
                                    <asp:HiddenField ID="Hid_PendngFlag" runat="server" />
                                    <asp:HiddenField ID="Hid_MultiAddrId" runat="server" />
                                    <asp:HiddenField ID="Hid_CustomerName" runat="server" />
                                    <asp:HiddenField ID="Hid_CountMultiAddrId" runat="server" />
                                    <asp:HiddenField ID="Hid_CountCustomerName" runat="server" />
                                    <asp:HiddenField ID="Hid_SecurityFV" runat="server" />
                                    <asp:HiddenField ID="Hid_AccIntDays" runat="server" />
                                    <asp:HiddenField ID="Hid_Demat" runat="server" />
                                    <asp:HiddenField ID="Hid_SecFaceValue" runat="server" />
                                    <asp:HiddenField ID="Hid_RateAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_NorCompoundInt" runat="server" />
                                    <asp:HiddenField ID="Hid_IntOnHoliday" runat="server" />
                                    <asp:HiddenField ID="Hid_IntOnSat" runat="server" />
                                    <asp:HiddenField ID="Hid_MatIntOnHoliday" runat="server" />
                                    <asp:HiddenField ID="Hid_MatIntOnSat" runat="server" />
                                    <asp:HiddenField ID="Hid_Yield" runat="server" />
                                    <asp:HiddenField ID="Hid_YTC" runat="server" />
                                    <asp:HiddenField ID="Hid_YTP" runat="server" />
                                    <asp:HiddenField ID="Hid_CoupRate1" runat="server" />
                                    <asp:HiddenField ID="Hid_MaturityAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_IssuePrice" runat="server" />
                                    <asp:HiddenField ID="Hid_T" runat="server" />
                                    <asp:HiddenField ID="Hid_MinDate" runat="server" />
                                    <asp:HiddenField ID="Hid_MaxDate" runat="server" />
                                    <asp:HiddenField ID="Hid_RedeemedFlag" runat="server" />
                                    <asp:HiddenField ID="Hid_SecId" runat="server" />
                                    <asp:HiddenField ID="Hid_CustId" runat="server" />
                                    <asp:HiddenField ID="Hid_TCSApplicable" runat="server" />
                                    <asp:HiddenField ID="Hid_StampDutyAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_TCSAmount" runat="server" />
                                    <asp:HiddenField ID="Hid_CurrSecFaceValue" Value="0" runat="server" />
                                    <asp:HiddenField ID="Hid_NSDLORGFacevalue" runat="server" />
                                    <asp:HiddenField ID="Hid_Marked" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="row_UploadSignedAck" runat="server" visible="false" align="left">
            <td align="left" colspan="6">
                <table align="center" style="padding-left: 100px;">
                    <tr id="row_UploadDoc" runat="server" visible="true">
                        <td class="LabelCSS">Upload Acknowledged Deal Confirmation:
                        </td>
                        <td class="ForControls" align="left" valign="middle">
                            <input id="FilePicker" type="file" name="File1" class="LabelCSS" runat="server" style="width: 208px; !important"
                                onchange="UploadTempImage();" tabindex="1" />
                        </td>
                    </tr>
                    <tr id="row_Doc" runat="server" visible="true">
                        <td class="LabelCSS">View Document:
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_view" runat="server" CssClass="ButtonCSS" Text="view" TabIndex="0" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="middle" colspan="6">
                            <asp:Label ID="LabelError" runat="server" CssClass="LabelCSS"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:HiddenField ID="Hid_Show" runat="server" />
        <asp:HiddenField ID="Hid_uploadImagePath" runat="server" />
        <asp:HiddenField ID="Hid_ImageContentType" runat="server" />
    </table>
</asp:Content>
