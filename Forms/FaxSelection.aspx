<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="FaxSelection.aspx.vb" Inherits="Forms_FaxSelection" Title="Offer Letter Generation" %>

<%@ Register Src="~/UserControls/YieldCalculater.ascx" TagName="YieldCalculater"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript" src="../Include/DatePicker.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <style>
        .vAlign {
            vertical-align: middle;
            margin-top: 0px;
        }

        .no-titlebar .ui-dialog-titlebar {
            display: none;
        }

        .ui-dialog .ui-dialog-buttonpane {
            display: block;
        }

        /*.GridCSS td, th {
            font-weight:bold!important;
        }*/

        form-group {
            display: inline-block;
            margin-left: 10px;
            float: right !important;
        }

        .form-group label {
            display: block;
        }

        .radioCB {
            margin-left: -315px;
            margin-top: 15px;
        }

        .marg-left-CB {
            margin-left: -11px;
        }

        .dgSecurityName {
            display: contents !important;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(function () {
            var radios = $("[id*=rbl_SelectCustomerBroker] input[type=radio]");
            radios.change(function () {
                var CustomerBroker = $(this).val();
                if (CustomerBroker == "C") {
                    $("#<%=dg_Customer.ClientID%>").removeClass("hidden");
                    $("#<%=dg_Broker.ClientID%>").addClass("hidden");
                    $("#<%=btn_AddCustomer1.ClientID%>").removeClass("hidden");
                    $("#<%=btn_AddBroker1.ClientID%>").addClass("hidden");
                    $("#tdBroker").addClass("hidden");
                    $("#tdCustomer").removeClass("hidden");
                }
                else {
                    $("#<%=dg_Customer.ClientID%>").addClass("hidden");
                    $("#<%=dg_Broker.ClientID%>").removeClass("hidden");
                    $("#<%=btn_AddCustomer1.ClientID%>").addClass("hidden");
                    $("#<%=btn_AddBroker1.ClientID%>").removeClass("hidden");
                    $("#tdBroker").removeClass("hidden");
                    $("#tdCustomer").addClass("hidden");
                }
            });
        });

        function DecimalOnly() {
            var KeyId = event.keyCode;
            return ((KeyId >= 48 && KeyId <= 57) || KeyId == 46)
        }

        $(document).ready(function () {

        });

        function FillSettlementDate() {
            if (CheckDate($("#<% = txt_Date.ClientID %>").get(0), false)) {
                var objData = {
                    "dealdate": $("#<% = txt_Date.ClientID %>").val(),
                    "settlementtype": $("#<% = cbo_SettDay.ClientID %>").val()
                }

                $.ajax({
                    url: "IPO_getdata.aspx?pagename=getsettlementdate",
                    type: "POST",
                    data: objData,
                    dataType: "text",
                    async: false,
                    success: function (result) {
                        $("#<% = txt_CalcDate.ClientID %>").val(result);
                    },
                    failure: function (result) {
                        alert(result);
                    }
                });
                }
            }
    </script>

    <script type="text/javascript">
        function CheckDelete(parentId) {
            if (window.confirm("Are you sure you want to delete this record????")) return true;
            return false;
        }

        function hideColumn() {
            var totalRowCount = 0;
            var rowCount = 0;
            var gridView = document.getElementById("<%=dg_Selected.ClientID %>");

            var rows = gridView.getElementsByTagName("tr")
            for (var i = 0; i < rows.length; i++) {
                totalRowCount++;
                if (rows[i].getElementsByTagName("td").length > 0) {
                    rowCount++;
                }
            }
            var message = "Total Row Count: " + totalRowCount;
            message += "\nRow Count: " + rowCount;
            rowCount = parseInt(rowCount);
            // alert(rowCount)
            for (var i = 0; i < gridView.rows.length; i++) {
                gridView.rows[i].cells[11].style.display = "none";
                gridView.rows[i].cells[12].style.display = "none";
                gridView.rows[i].cells[13].style.display = "none";
                gridView.rows[i].cells[14].style.display = "none";
            }
            return false;
        };

        function SecurityInfo_Masters() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Date").value) == "") {
                AlertMessage("Validation", "Please Enter Date", 175, 450);
                return false;
            }
            var ForDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_Date").value;
            var DialogOptions = "Center=Yes;Scrollbar=yes;dialogWidth=875px;dialogTop=200px;dialogHeight=580px;Help=No; Status=No;Resizable=Yes;menubar=Yes";
            var OpenUrl = "SecurityInfo.aspx?fordate=" + ForDate + "&StockUpdate=N";

            var ret = window.showModalDialog(OpenUrl, 'some argument', 'dialogWidth:1000px;dialogTop=150px;dialogHeight:620px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                if (ret == 'E') {
                    SecurityInfo_Masters();
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelVal").value = ret + "|" + "1";
                    document.getElementById('<%= btn_AddSecurity.ClientID%>').click();
                }
            }
        }

        function Clear_YTCSemiRate(lnk) {
            var col1;
            var col2;

            var YTMSemi;
            var YTC;
            var gridView = document.getElementById("<%=dg_Selected.ClientID %>");
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex;
            col1 = gridView.rows[rowIndex].cells[4];
            col2 = gridView.rows[rowIndex].cells[5];
            YTMSemi = gridView.rows[rowIndex].cells[6];
            YTC = gridView.rows[rowIndex].cells[7];
            var inputs = gridView.rows[rowIndex].cells[4].getElementsByTagName("input");
            for (j = 0; j < col1.childNodes.length; j++) {
                if (col1.childNodes[j].type == "text") {
                    col1.childNodes[j].value = parseFloat(0).toFixed(4);
                    col2.childNodes[j].value = parseFloat(0).toFixed(4);
                    YTMSemi.childNodes[j].value = parseFloat(0).toFixed(4);
                    YTC.childNodes[j].value = parseFloat(0).toFixed(4);

                    for (var n = 0; n < inputs.length; ++n) {
                        if (inputs[n].type == "hidden") {
                            var hdnk = inputs[n].value;
                            inputs[n].value = col1.childNodes[j].value;
                        }
                    }
                }
            }
        }

        function Clear_YeildSemi(lnk) {
            var col1;
            var col2;
            var YTCAnn;
            var YTCSemi;
            var gridView = document.getElementById("<%=dg_Selected.ClientID %>");
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex;
            col1 = gridView.rows[rowIndex].cells[4];
            col2 = gridView.rows[rowIndex].cells[5];
            YTCAnn = gridView.rows[rowIndex].cells[7];
            YTCSemi = gridView.rows[rowIndex].cells[8];
            var inputs = gridView.rows[rowIndex].cells[4].getElementsByTagName("input");
            for (j = 0; j < col1.childNodes.length; j++) {
                if (col1.childNodes[j].type == "text") {
                    col1.childNodes[j].value = parseFloat(0).toFixed(4);
                    col2.childNodes[j].value = parseFloat(0).toFixed(4);
                    YTCAnn.childNodes[j].value = parseFloat(0).toFixed(4);
                    YTCSemi.childNodes[j].value = parseFloat(0).toFixed(4);

                    for (var n = 0; n < inputs.length; ++n) {
                        if (inputs[n].type == "hidden") {
                            var hdnk = inputs[n].value;
                            inputs[n].value = col1.childNodes[j].value;
                        }
                    }
                }
            }
        }

        function Clear_Yeild(lnk) {

            var col1;
            var col2;
            var colYTMSemi;
            var colYTCSemi;

            var gridView = document.getElementById("<%=dg_Selected.ClientID %>");
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex;
            col1 = gridView.rows[rowIndex].cells[5];
            col2 = gridView.rows[rowIndex].cells[7];

            colYTMSemi = gridView.rows[rowIndex].cells[6];
            colYTCSemi = gridView.rows[rowIndex].cells[8];

            for (j = 0; j < col1.childNodes.length; j++) {
                if (col1.childNodes[j].type == "text") {
                    col1.childNodes[j].value = parseFloat(0).toFixed(4);
                    col2.childNodes[j].value = parseFloat(0).toFixed(4);
                    colYTMSemi.childNodes[j].value = parseFloat(0).toFixed(4);
                    colYTCSemi.childNodes[j].value = parseFloat(0).toFixed(4);
                }
            }
        }

        function Clear_Rate(lnk) {

            var col1;
            var col2;

            var colYTMSemi;
            var colYTCSemi;

            var gridView = document.getElementById("<%=dg_Selected.ClientID %>");
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex;
            col1 = gridView.rows[rowIndex].cells[4];
            col2 = gridView.rows[rowIndex].cells[7];
            colYTMSemi = gridView.rows[rowIndex].cells[6];
            colYTCSemi = gridView.rows[rowIndex].cells[8];

            var inputs = gridView.rows[rowIndex].cells[4].getElementsByTagName("input");
            for (j = 0; j < col1.childNodes.length; j++) {
                if (col1.childNodes[j].type == "text") {
                    /*alert(col1.childNodes[j].value);*/
                    col1.childNodes[j].value = parseFloat(0).toFixed(4);
                    col2.childNodes[j].value = parseFloat(0).toFixed(4);

                    colYTMSemi.childNodes[j].value = parseFloat(0).toFixed(4);
                    colYTCSemi.childNodes[j].value = parseFloat(0).toFixed(4);

                    for (var n = 0; n < inputs.length; ++n) {
                        if (inputs[n].type == "hidden") {
                            var hdnk = inputs[n].value;
                            inputs[n].value = col1.childNodes[j].value;
                        }
                    }
                }
            }
        }

        function Clear_YTCRate(lnk) {
            var col1;
            var col2;
            var YTMSemi;
            var YTCSemi;

            var gridView = document.getElementById("<%=dg_Selected.ClientID %>");
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex;
            col1 = gridView.rows[rowIndex].cells[4];
            col2 = gridView.rows[rowIndex].cells[5];

            YTMSemi = gridView.rows[rowIndex].cells[6];
            YTCSemi = gridView.rows[rowIndex].cells[8];
            var inputs = gridView.rows[rowIndex].cells[4].getElementsByTagName("input");
            for (j = 0; j < col1.childNodes.length; j++) {
                if (col1.childNodes[j].type == "text") {
                    /*alert(col1.childNodes[j].value);*/
                    col1.childNodes[j].value = parseFloat(0).toFixed(4);
                    col2.childNodes[j].value = parseFloat(0).toFixed(4);

                    YTMSemi.childNodes[j].value = parseFloat(0).toFixed(4);
                    YTCSemi.childNodes[j].value = parseFloat(0).toFixed(4);
                    for (var n = 0; n < inputs.length; ++n) {
                        if (inputs[n].type == "hidden") {
                            var hdnk = inputs[n].value;
                            inputs[n].value = col1.childNodes[j].value;
                        }
                    }
                }
            }
        }

        function showCal(lnk) {
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;

            var inputs = row.cells[4].getElementsByTagName("input");
            var Rate = row.cells[4].getElementsByTagName("input")[0].value;
            var ORate = row.cells[12].getElementsByTagName("span");
            var margin_val = lnk.value - 0
            ORate = ORate[0].innerText;
            row.cells[4].getElementsByTagName("input")[0].value = (parseFloat((ORate)) + parseFloat(margin_val)).toFixed(4);
            row.cells[5].getElementsByTagName("input")[0].value = parseFloat(0).toFixed(4);
            row.cells[6].getElementsByTagName("input")[0].value = parseFloat(0).toFixed(4);
            row.cells[7].getElementsByTagName("input")[0].value = parseFloat(0).toFixed(4);
            row.cells[8].getElementsByTagName("input")[0].value = parseFloat(0).toFixed(4);
            for (var n = 0; n < inputs.length; ++n) {
                if (inputs[n].type == "hidden") {
                    var hdnk = inputs[n].value;
                    inputs[n].value = row.cells[4].getElementsByTagName("input")[0].value;
                }
            }
        }

        function AssignRate(lnk) {
            var row = lnk.parentNode.parentNode;
            var inputs = row.cells[4].getElementsByTagName("input");
            var Rate = row.cells[4].getElementsByTagName("input")[0].value;
            for (var n = 0; n < inputs.length; ++n) {
                if (inputs[n].type == "hidden") {
                    var hdnk = inputs[n].value;
                    inputs[n].value = row.cells[4].getElementsByTagName("input")[0].value;
                }
            }
        }

        function isNumberKey(evt, obj) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            var value = obj.value;
            var dotcontains = value.indexOf(".") != -1;
            if (dotcontains)
                if (charCode == 46) return false;
            if (charCode == 46) return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function ShowTempCust() {

            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=590px; dialogTop=230px; dialogHeight=600px; Help=No; Status=No; Resizable=Yes;"
            var OpenUrl = "SelectTempCustomer.aspx"
            OpenUrl = OpenUrl;
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions)

            if (typeof (ret) == "undefined") {

                return false
            }
            else {

                return true
            }
        }

        function ShowCustomer() {

            var width = "300px"
            var height = "500px"
            var pageUrl = "SelectCustomers.aspx";
            var selValues = ""
            pageUrl = pageUrl + "?SourceType=2&TableName=CustomerMaster&SelectedFieldName=CustomerName&ProcName=ID_SEARCH_CustomerMasterFaxSelection&SelectedValueName=CustomerId";

            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:300px;dialogHeight:500px;center:1;status:0;resizable:0;');

            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret;
                document.getElementById('<%= btn_AddCustomer.ClientID%>').click();
            }
        }

        function ShowBroker() {
            var rowCount = document.getElementById("ctl00_ContentPlaceHolder1_dg_Broker").rows.length;
            if (rowCount > 2) {
                AlertMessage("Validation", "Only one broker can be added at a time", 175, 450);
                return false;
            }
            var width = "300px"
            var height = "500px"
            var pageUrl = "SelectBrokers.aspx";
            var selValues = ""
            pageUrl = pageUrl + "?SourceType=2&TableName=BrokerMaster&SelectedFieldName=BrokerName&ProcName=ID_SEARCH_BrokerMasterFaxSelection&SelectedValueName=BrokerId";

            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:300px;dialogHeight:500px;center:1;status:0;resizable:0;');

            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret;
                document.getElementById('<%= btn_AddBroker.ClientID%>').click();
            }
        }

        function SecurityInformation() {
            var DialogOptions = "Center=Yes; Scrollbar=yes; dialogWidth=875px; dialogTop=200px; dialogHeight=640px; Help=No; Status=No; Resizable=Yes;"
            var FaxQuoteId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaxQuoteId").value
            var OpenUrl = "SecurityInformation.aspx"
            OpenUrl = OpenUrl + "?FaxQuoteId=" + FaxQuoteId;
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions)

            if (ret == "" || typeof (ret) == "undefined") {
                return false
            }
            else {
                return true
            }
        }

        function SecurityInformation_old() {
            var DialogOptions = "Center=Yes; Scrollbar=yes; dialogWidth=300px; dialogTop=200px; dialogHeight=500px; Help=No; Status=No; Resizable=Yes;"
            var FaxQuoteId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaxQuoteId").value
            var OpenUrl = "SelectSecurity.aspx"
            OpenUrl = OpenUrl + "?SourceType=2&TableName=SecurityMaster&SelectedFieldName=SM.SecurityName&ProcName=ID_SEARCH_StockUpdateMaster&SelectedValueName=SM.SecurityId&FaxQuoteId=" + FaxQuoteId;
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions)

            if (ret == "" || typeof (ret) == "undefined") {
                return false
            }
            else {
                return true
            }
        }

        function UpdateDetails(rowIndex) {
            var pageUrl = "SecurityInformation.aspx";
            var strValues = "";
            var selValues = "";
            var FaxQuoteId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaxQuoteId").value

            pageUrl = pageUrl + "?rowIndex=" + rowIndex + "&FaxQuoteId=" + FaxQuoteId;
            var ret = ShowDialogOpen(pageUrl, "875px", "590px")
            if (ret == "" || typeof (ret) == "undefined") {
                return false
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret
                return true
            }
        }

        function ShowSelection(selValues, FormatIndex) {

            //var ret = ShowList("FaxField", "FieldId", "ID_FILL_ClientType", selValues)
            //if (typeof (ret) != "undefined") {
            //    document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelectedFields").value = ret;
            //}
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_FormatIndex").value = FormatIndex;

            var fieldName = "FaxField";
            var valueName = "FieldId";
            var procName = "ID_FILL_ClientType";

            var pageUrl = "SelectionList.aspx?FieldName=" + fieldName + "&ValueName=" + valueName + "&ProcName=" + procName + "&SelectedValues=" + selValues + "&Form=Offer";
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:600px;dialogHeight:600px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                var lastChar = ret.slice(-1);
                if (lastChar == ',') {
                    ret = ret.slice(0, -1);
                }
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelectedFields").value = ret;
                document.getElementById('<%= btn_AddFormat_.ClientID%>').click();
            }
        }


        function ShowList(fieldName, valueName, procName, selValues) {

            var ret = ShowDialogOpen("SelectionList.aspx?FieldName=" + fieldName + "&ValueName=" + valueName + "&ProcName=" + procName + "&SelectedValues=" + selValues + "&Form=Offer", "600px", "550px")
            //debugger;
            if (typeof (ret) != "undefined") {
                var lastChar = ret.slice(-1);
                if (lastChar == ',') {
                    ret = ret.slice(0, -1);
                }
            }
        }
        function ShowDialog(PageName, strSelected, strWidth, strHeight) {

            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=200px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
            var OpenUrl = PageName + "?SelectedField=" + strSelected;
            var ret = window.showModalDialog(OpenUrl, 'some argument', 'dialogWidth:600px;dialogHeight:600px;center:1;status:0;resizable:0;');

        }


        function ValidateSave() {
            if (Validate() == false) return false
            //if ((document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaxQuoteId").value - 0) != 0) return true
            var strName = window.prompt("Enter the Name which will identify this Quote", "");
            if (strName == null || strName == "") {
                return false
            }
            else {
                var flag = false
                var hidAllNames = document.getElementById("ctl00_ContentPlaceHolder1_Hid_AllQuoteNames").value.split("!");
                for (i = 0; i < hidAllNames.length; i++) {
                    if (strName.toLowerCase() == hidAllNames[i].toLowerCase()) {
                        flag = true
                        break
                    }
                }
                if (flag == true) {
                    AlertMessage("Validation", "The Quote Name already exist, Please enter another Name", 175, 450);
                    return false
                }
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_QuoteName").value = strName
            }
        }

        function Validate() {
            var rowCount = document.getElementById("ctl00_ContentPlaceHolder1_dg_Selected").rows.length;
            if (rowCount <= 1) {
                AlertMessage("Validation", "Please select atleast one security for the fax", 175, 450)
                return false
            }
            return true
        }

        function ValidateCustomerBroker() {

            var rb = document.getElementById("<%=rbl_SelectCustomerBroker.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    var selText = label[i].innerHTML
                    var selVal = radio[i].value;
                    if (selVal == "C") {
                        var rowCount = document.getElementById("ctl00_ContentPlaceHolder1_dg_Customer").rows.length;
                        if (rowCount <= 2) {
                            AlertMessage("Validation", "Please select atleast one Customer", 175, 450)
                            return false
                        }
                    }
                    else {
                        var rowCount = document.getElementById("ctl00_ContentPlaceHolder1_dg_Broker").rows.length;
                        if (rowCount <= 2) {
                            AlertMessage("Validation", "Please select atleast one Broker", 175, 450)
                            return false
                        }
                    }
                }
            }
            if (Validate() == false) {
                return false
            }
            return true
        }



        function OpenQuoteWindow() {
            var pageUrl = "ShowQuote.aspx";
            var strValues = "";
            var selValues = "";
            pageUrl = pageUrl;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:300px;dialogHeight:600px;center:1;status:0;resizable:0;');

            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaxQuoteId").value = ret;
                document.getElementById('<%= btn_Open.ClientID%>').click();
            }
        }

        function UpdStock() {
            if ($("#<%=txt_basispoint.ClientID%>").val() != "") {
                <%--               $("#<%=Hid_BasisPoint.ClientID%>").val(parseFloat($("#<%=txt_basispoint.ClientID%>").val()) / 100);--%>
                $("#<%=Hid_BasisPoint.ClientID%>").val(parseFloat($("#<%=txt_basispoint.ClientID%>").val()));
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_date").value) == "") {
                AlertMessage("Validation", "Please Enter Date", 175, 450);
                return false;
            }
            else {
                var totalRowCount = 0;
                var rowCount = 0;
                var gridView = document.getElementById("<%=dg_Selected.ClientID %>");

                var rows = gridView.getElementsByTagName("tr")
                for (var i = 0; i < rows.length; i++) {
                    totalRowCount++;
                    if (rows[i].getElementsByTagName("td").length > 0) {
                        rowCount++;
                    }
                }
                var message = "Total Row Count: " + totalRowCount;
                message += "\nRow Count: " + rowCount;
                rowCount = parseInt(rowCount);

                if (rowCount == 0) {
                    AlertMessage("Validation", "Please Add Security first.", 175, 450);
                    return false;
                }

                var inputs = gridView.getElementsByTagName("input");
                var col1;
                var col2;
                var col3;
                var col4;

                var totalcol1 = 0;
                var count = 0;
                for (i = 1 ; i < gridView.rows.length; i++) {
                    col1 = gridView.rows[i].cells[8];
                    for (j = 0; j < col1.childNodes.length; j++) {
                        if (col1.childNodes[j].type == "text") {
                            if (col1.childNodes[j].value == 0 || col1.childNodes[j].value == "") {
                                count = Number(count) + Number(1)
                            }
                        }
                    }
                }

                if (count > 0) {
                }
                else {
                    document.getElementById("div_onsaveclick").style.display = "block";
                }
            }

        }
        function reset() {
            $("#<%=txt_basispoint.ClientID%>").val("");
            $("#<%=txt_basispoint.ClientID%>").val("0");
        }

    </script>

    <script type="text/javascript">
        $(function () {
            // Create a dialog.
            $("#dialog").dialog({
                dialogClass: "no-titlebar",
                autoOpen: false,
                modal: true,
                width: 875,
                height: 580,
                buttons: [{
                    text: "OK",
                    click: function () {

                        $(this).dialog("close");
                        document.getElementById('<%= btn_AddSecurity.ClientID%>').click();
                    }
                }]
            });

            $("#btn_dialog").on("click", function () {
                // Open the dialog.
                $("#dialog").dialog("open");
            });
        });

    </script>

    <script type="text/javascript">

        var popUpObj;

        function showModalPopUp() {

            $("#dialog-confirm").dialog({
                dialogClass: "no-titlebar",
                resizable: false,

                height: 580,

                width: 875,

                modal: false,

                buttons: {

                    "Delete Text": function () {

                        $("#content").hide();

                        $(this).dialog("close");

                    },

                    Cancel: function () {

                        $(this).dialog("close");

                    }

                }

            });
            LoadModalDiv();
        }

        function LoadModalDiv() {

            var bcgDiv = document.getElementById("dialog-confirm");

            bcgDiv.style.display = "block";

        }

        function HideModalDiv() {

            var bcgDiv = document.getElementById("dialog-confirm");

            bcgDiv.style.display = "none";

        }


        function getCustBrokerOption() {
            var rb = document.getElementById("<%=rbl_SelectCustomerBroker.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    var selText = label[i].innerHTML
                    var selVal = radio[i].value;
                    if (selVal == "B") {
                        $("#<%=dg_Customer.ClientID%>").addClass("hidden");
                        $("#<%=dg_Broker.ClientID%>").removeClass("hidden");
                        $("#<%=btn_AddCustomer1.ClientID%>").addClass("hidden");
                        $("#<%=btn_AddBroker1.ClientID%>").removeClass("hidden");
                        $("#tdBroker").removeClass("hidden");
                        $("#tdCustomer").addClass("hidden");
                    }
                    else if (selVal == "C") {
                        $("#<%=dg_Customer.ClientID%>").removeClass("hidden");
                        $("#<%=dg_Broker.ClientID%>").addClass("hidden");
                        $("#<%=btn_AddCustomer1.ClientID%>").removeClass("hidden");
                        $("#<%=btn_AddBroker1.ClientID%>").addClass("hidden");
                        $("#tdBroker").addClass("hidden");
                        $("#tdCustomer").removeClass("hidden");
                    }
                break;
            }
        }

        return false;

    }


    </script>

    <table id="Table1" width="100%" align="left" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center" colspan="2">Fax Selection
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Label ID="lbl_Msg" runat="server" CssClass="LabelCSS"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table width="50%" cellspacing="0" cellpadding="0" border="0">
                    <tr align="center" valign="top">
                        <td style="padding: 0px;">
                            <table cellpadding="0" cellspacing="0">
                                <tr align="left">

                                    <td>For Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Date" Width="70px" runat="server" CssClass="TextBoxCSS" TabIndex="9"
                                            MaxLength="10" onchange="javascript:FillSettlementDate();" onblur="javascript:FillSettlementDate();"></asp:TextBox><img
                                                class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Date',this);">
                                    </td>
                                    <td>T +:
                                        <asp:DropDownList ID="cbo_SettDay" Width="40px" runat="server" CssClass="ComboBoxCSS"
                                            TabIndex="10" onchange="javascript:FillSettlementDate();">
                                            <asp:ListItem Value="0">0</asp:ListItem>
                                            <asp:ListItem Value="1" Selected="True">1</asp:ListItem>
                                            <asp:ListItem Value="2">2</asp:ListItem>
                                            <asp:ListItem Value="3">3</asp:ListItem>
                                            <asp:ListItem Value="4">4</asp:ListItem>
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_CalcDate" Width="70px" runat="server" CssClass="TextBoxCSS"
                                            TabIndex="11" MaxLength="10"></asp:TextBox>
                                        <%--<asp:Label runat="Server" ID="lblCalcDate" CssClass="TextBoxCSS vAlign" Text=""></asp:Label>--%>
                                    </td>
                                    <td>
                                        <%-- <div class="form-group" style="float: left !important; padding-right: 0px;">--%>

                                        <asp:TextBox ID="txt_basispoint" runat="server" placeholder="Enter basis point (In Rs.)" class="TextBoxCSS" Style="width: 140px;" onkeypress="return DecimalOnly();" MaxLength="6" />
                                        <%--      <label style="color: black!important; font-weight: normal; font-size: 15px; font-family: Calibri; text-transform: capitalize;">(Rs. )</label>
                                        </div>--%>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_UpdateStock" runat="server" CssClass="ButtonCSS" Text="Calc Quote"
                                            Visible="true" Width="80px" />
                                        <asp:Button ID="btn_UpdateGridQuote" runat="server" CssClass="ButtonCSS" Text="Calc Quote"
                                            Visible="true" Width="80px" />
                                        <asp:Button ID="btn_Reset" runat="server" Text="Reset" CssClass="ButtonCSS" OnClientClick="reset();" />


                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr style="margin-top: 50px;">
            <td colspan="2" align="center">
                <table>
                    <tr>
                        <td align="center">
                            <asp:RadioButtonList ID="rbl_SelectCustomerBroker" runat="server" RepeatDirection="Horizontal" AutoPostBack="false" onchange="getCustBrokerOption();" CssClass="radioCB">
                                <asp:ListItem Text="Customer" Value="C" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Broker" Value="B"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Button ID="btn_AddCustomer" runat="server" CssClass="ButtonCSS hidden"
                                Text="Add Customer" />
                            <input type="button" id="btn_AddCustomer1" runat="server" class="ButtonCSS marg-left-CB" onclick="return ShowCustomer();" value="Add Customer" style="width: 100px" />
                            <asp:Button ID="btn_AddBroker" Width="100px" runat="server" CssClass="ButtonCSS hidden"
                                Text="Add Broker" />
                            <input type="button" id="btn_AddBroker1" runat="server" class="ButtonCSS hidden" onclick="return ShowBroker();" value="Add Broker" style="width: 100px" />
                            <asp:Button ID="btn_AddTempCustomer" Width="140px" runat="server" CssClass="ButtonCSS"
                                Text="Add Temp Customer" Visible="false" />
                            <asp:Button ID="btn_AddFormat_" runat="server" CssClass="hidden" Text="Add Format" />
                        </td>
                    </tr>
                </table>
                <div id="div2" style="margin-top: 0px; overflow: auto; width: 442px; padding-top: 0px; position: relative; height:120px"
                    align="left">
                    <asp:GridView ID="dg_Customer" runat="server" AutoGenerateColumns="false" CssClass="GridCSS"
                        Width="420px">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" Width="420px" />
                        <RowStyle HorizontalAlign="Left" CssClass="GridRowCSS" Width="320px" />
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Checked="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                        CommandName="DeleteRow" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CustomerName">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_CustomerName" BackColor="#FFFFFF" Width="150px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerName") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CustomerId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="FieldId" DataField="FieldId" Visible="False" />
                            <asp:BoundField HeaderText="ContactPerson" DataField="ContactPerson" />
                            <asp:BoundField HeaderText="EmailId" DataField="EmailId" Visible="false" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <input type="button" id="btn_AddFormat" runat="server" value="Change Format" class="SearchButtonCSS" style="width: 105px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="dg_Broker" runat="server" AutoGenerateColumns="false" CssClass="GridCSS hidden"
                        Width="420px">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" Width="420px" />
                        <RowStyle HorizontalAlign="Left" CssClass="GridRowCSS" Width="320px" />
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Checked="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                        CommandName="DeleteRow" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BrokerName">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_BrokerName" BackColor="#FFFFFF" Width="150px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.BrokerName") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BrokerCode">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_BrokerCode" BackColor="#FFFFFF" Width="150px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.BrokerCode") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BrokerId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BrokerId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="FieldId" DataField="FieldId" Visible="False" />
                            <asp:BoundField HeaderText="EmailId" DataField="EmailId" Visible="false" />
                            <asp:TemplateField HeaderText="Basis Point">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BasisPoint" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BasisPoint") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <input type="button" id="btn_AddFormat" runat="server" value="Change Format" class="SearchButtonCSS" style="display: none; width: 105px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btn_AddSecurity" runat="server" CssClass="ButtonCSS hidden" Text="Add Security"
                                Width="130px" />
                            <input type="button" id="btn_AddSecurity1" runat="server" class="ButtonCSS" onclick="return SecurityInfo_Masters();" value="Add Security" style="width: 80px" />
                            <input id="btn_dialog" type="button" value="button" style="display: none;" />
                        </td>
                    </tr>
                </table>
                <div id="div1" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 300px"
                    align="left">
                    <asp:GridView ID="dg_Selected" runat="server" AutoGenerateColumns="false" CssClass="GridCSS" Width="100%">
                        <HeaderStyle HorizontalAlign="Left" CssClass="GridHeaderCSS" />
                        <RowStyle HorizontalAlign="left" CssClass="GridRowCSS" Width="1000px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="EditRow"
                                        Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtn_Delete" CommandName="DeleteRow" runat="server" ImageUrl="~/Images/delete.gif"
                                        CommandArgument='<%# DataBinder.Eval(Container,"DataItem.SecurityId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SecurityId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityId") %>'></asp:Label>
                                    <asp:Label ID="lbl_FaxQuoteDetailId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FaxQuoteDetailId") %>'></asp:Label>
                                    <asp:Label ID="lbl_TypeFlag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TypeFlag") %>'></asp:Label>
                                    <asp:Label ID="lbl_SecurityTypeName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityTypeName") %>'></asp:Label>
                                    <asp:Label ID="lbl_PerpetualFlag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NatureOFInstrument") %>'></asp:Label>
                                    <asp:Label ID="lbl_CreditRating" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CreditRating") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SECURITY NAME">
                                <ItemTemplate>
                                    <%--   <asp:TextBox ID="txt_SecurityName" ReadOnly="true" BackColor="#FFFFFF" Width="150px"
                                                            Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                            onkeypress="scroll();" runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityName") %>'></asp:TextBox>--%>
                                    <asp:Label ID="txt_SecurityName" Width="250px" runat="server" Text='<%#Container.DataItem("SecurityName") %>'
                                        CssClass="LabelCSS dgSecurityName"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ISIN NO">
                                <ItemTemplate>
                                    <%--   <asp:TextBox ID="txt_SecurityName" ReadOnly="true" BackColor="#FFFFFF" Width="150px"
                                                            Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                            onkeypress="scroll();" runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityName") %>'></asp:TextBox>--%>
                                    <asp:Label ID="txt_ISINNo" Width="100" runat="server" Text='<%#Container.DataItem("ISINNo") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RATE" SortExpression="SellingRate">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_Rate" Width="50px" runat="server" Text='<%# container.dataitem("SellingRate") %>'
                                        CssClass="TextBoxCSS" onkeypress="OnlyDecimal();Clear_Yeild(this);" onblur="AssignRate(this);"></asp:TextBox>
                                    <asp:HiddenField ID="hdnRate" runat="server" Value='<%#Eval("SellingRate") %>' />
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="true" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YTM(ANN)" SortExpression="Yield">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Yield" Width="50px" runat="server" Text='<%# container.dataitem("Yield") %>'
                                        CssClass="TextBoxCSS" onkeypress="OnlyDecimal();Clear_Rate(this);" Enabled="true"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="true" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YTMSemi" SortExpression="Yield">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_YTMSemi" Width="40px" runat="server" Text='<%# container.dataitem("YTMSemi") %>'
                                        CssClass="TextBoxCSS" onkeypress="OnlyDecimal();Clear_YeildSemi(this);"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="true" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YTC(ANN)" SortExpression="Yield">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_YTC" Width="50px" runat="server" Text='<%# container.dataitem("YTCAnn") %>'
                                        CssClass="TextBoxCSS" onkeypress="OnlyDecimal();Clear_YTCRate(this);" Enabled="true"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="true" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YTCSemi" SortExpression="Yield">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_YTCSemi" Width="40px" runat="server" Text='<%# container.dataitem("YTCSemi") %>'
                                        CssClass="TextBoxCSS" onkeypress="OnlyDecimal();Clear_YTCSemiRate(this);"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="true" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QUANTITY (IN LACS)">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_LotSize" Width="50px" runat="server" Text='<%# container.dataitem("ShowNumber") %>'
                                        CssClass="TextBoxCSS"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--  <asp:TemplateField HeaderText="FaceValue">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_FaceValue" Width="50px" runat="server" Text='<%# container.dataitem("FaceValue") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="RATING REMARK">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_RatingRemark" BackColor="#FFFFFF" Width="80px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.Rating") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--  <asp:TemplateField HeaderText="ORate" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ORate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OriginalSellingRate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                            <%-- <asp:TemplateField HeaderText="TaxFree">
                                                    <ItemTemplate>
                                                        <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_TaxFree" runat="server" CellPadding="0"
                                                            Value='<%#DataBinder.Eval(Container, "DataItem.TaxFree") %>'
                                                            CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                            <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="SemiAnnFlag" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Semi_Ann_Flag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Semi_Ann_Flag") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CallDate" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CallDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CallDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PhysicalDMAT" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_PhysicalDMAT" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PhysicalDMAT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IPCalc" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_IPCalc" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IPCalc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RateActual" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_RateActual" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RateActual") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YTCAnn" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_YTCAnn" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.YTCAnn") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YTPAnn" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_YTPAnn" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.YTPAnn") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YTCSemi" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_YTCSemi" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.YTCSemi") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YTPSemi" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_YTPSemi" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.YTPSemi") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SecurityTypeId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityTypeId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CombineIPMat" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CombineIPMat") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Rate_Actual_Flag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Rate_Actual_Flag") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Equal_Actual_Flag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Equal_Actual_Flag") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_IntDays" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IntDays") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FirstYrAllYr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FirstYrAllYr") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Category" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Category") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SubCategory" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SubCategory") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SecuredUnsec" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SecuredUnsec") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_NameOFPD" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name of PD") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CallFlag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CallFlag") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_RowNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_YieldPriceType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.YieldPriceType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_OrderId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrderId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText ="SECURITY REMARK">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SecurityRemark" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Remark") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr id="rowremark1" visible="false" runat="server">
            <td colspan="2" align="center">
                <div id="div3" style="margin-top: 0px; overflow: auto; width: 442px; padding-top: 0px; position: relative; height: 70px"
                    align="left">
                    <asp:GridView ID="dg_OfferRemark" runat="server" AutoGenerateColumns="false" CssClass="GridCSS"
                        Width="420px">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" Width="420px" />
                        <RowStyle HorizontalAlign="Left" CssClass="GridRowCSS" Width="320px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_ItemChecked" runat="server" Checked="false"></asp:CheckBox>
                                    <asp:Label ID="lbl_RemarkId" Text='<%# DataBinder.Eval(Container,"DataItem.RemarkId") %>'
                                        runat="server" val='<%# DataBinder.Eval(Container, "DataItem.RemarkId") %>' CssClass="hidden" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remark Heading">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_RemarkHeading" BackColor="#FFFFFF" Width="150px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.RemarkHeading") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:CheckBox ID="chk_SendMail" runat="server" AutoPostBack="false" Text="Send Mail" />&nbsp;&nbsp;
                <asp:Button ID="btn_CreateWordFax" runat="server" Text="Create Word" ToolTip="Create Word Fax"
                    Visible="false" CssClass="ButtonCSS" Height="20px" Width="102px" />&nbsp;&nbsp;
                <asp:Button ID="btn_CreateExcelFax" runat="server" Text="Create Excel" ToolTip="Create Excel Fax"
                    CssClass="ButtonCSS" Height="20px" Width="102px" />&nbsp;&nbsp;
                <asp:Button ID="btn_CreatePDFFax" runat="server" Text="Create PDF" ToolTip="Create PDF Fax"
                    CssClass="ButtonCSS" Height="20px" Width="102px" />&nbsp;&nbsp;
                <asp:Button ID="btn_Save" runat="server" Text="Save Quote" ToolTip="Save Quote" CssClass="ButtonCSS"
                    Height="20px" Width="102px" />&nbsp;&nbsp;
            <asp:Button ID="btn_SaveAs" runat="server" Text="Save Quote" ToolTip="Save As" CssClass="ButtonCSS"
                Height="20px" Width="102px" Visible="false" />&nbsp;&nbsp;
                <asp:Button ID="btn_Open" runat="server" Text="Open Quote" ToolTip="Open Quote" CssClass="ButtonCSS hidden"
                    Height="20px" Width="100px" />
                <input type="button" id="btn_Open1" runat="server" value="Open Quote" class="ButtonCSS" style="width: 100px; height: 20px" onclick="return OpenQuoteWindow();" />
                &nbsp;
                <asp:Button ID="TestButton" runat="server" Text="Test Quote" ToolTip="Open Quote"
                    CssClass="ButtonCSS" Height="20px" Width="100px" Visible="false" />
                <asp:HiddenField ID="Hid_RetValues" runat="server" />
                <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                <asp:HiddenField ID="Hid_SelectedFields" runat="server" />
                <asp:HiddenField ID="Hid_FaxQuoteId" runat="server" />
                <asp:HiddenField ID="Hid_AllQuoteNames" runat="server" />
                <asp:HiddenField ID="Hid_QuoteName" runat="server" />
                <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                <asp:HiddenField ID="Hid_SecurityName" runat="server" />
                <asp:HiddenField ID="Hid_FieldId" runat="server" />
                <asp:HiddenField ID="Hid_SecId" runat="server" />
                <asp:HiddenField ID="Hid_BranchId" runat="server" />
                <asp:HiddenField ID="Hid_Wordpath" runat="server" />
                <asp:HiddenField ID="Hid_CustomerEmailId" runat="server" />
                <asp:HiddenField ID="Hid_EmailId" runat="server" />
                <asp:HiddenField ID="Hid_SecTypeId" runat="server" />
                <asp:HiddenField ID="Hid_MatDate" runat="server" />
                <asp:HiddenField ID="Hid_MatAmt" runat="server" />
                <asp:HiddenField ID="Hid_CallDate" runat="server" />
                <asp:HiddenField ID="Hid_CallAmt" runat="server" />
                <asp:HiddenField ID="Hid_CoupDate" runat="server" />
                <asp:HiddenField ID="Hid_CoupRate" runat="server" />
                <asp:HiddenField ID="Hid_PutDate" runat="server" />
                <asp:HiddenField ID="Hid_PutAmt" runat="server" />
                <asp:HiddenField ID="Hid_RateAmtFlag" runat="server" />
                <asp:HiddenField ID="Hid_InterestDate" runat="server" />
                <asp:HiddenField ID="Hid_BookClosureDate" runat="server" />
                <asp:HiddenField ID="Hid_GovernmentFlag" runat="server" />
                <asp:HiddenField ID="Hid_FaceValue" runat="server" />
                <asp:HiddenField ID="Hid_Issue" runat="server" />
                <asp:HiddenField ID="Hid_DMATBkDate" runat="server" />
                <asp:HiddenField ID="Hid_MMYRate" runat="server" />
                <asp:HiddenField ID="Hid_Frequency" runat="server" />
                <asp:HiddenField ID="Hid_TypeFlag" runat="server" />
                <asp:HiddenField ID="Hid_Issuer" runat="server" />
                <asp:HiddenField ID="Hid_Security" runat="server" />
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="Hid_Date" runat="server" />
                <asp:HiddenField ID="Hid_YTMAnn" runat="server" />
                <asp:HiddenField ID="Hid_YTCAnn" runat="server" />
                <asp:HiddenField ID="Hid_YTPAnn" runat="server" />
                <asp:HiddenField ID="Hid_YTMSemi" runat="server" />
                <asp:HiddenField ID="Hid_YTCSemi" runat="server" />
                <asp:HiddenField ID="Hid_YTPSemi" runat="server" />
                <asp:HiddenField ID="Hid_Rate" runat="server" />
                <asp:HiddenField ID="Hid_YTMDate" runat="server" />
                <asp:HiddenField ID="Hid_SecurityMatDate" runat="server" />
                <asp:HiddenField ID="Hid_NextIntDate" runat="server" />
                <asp:HiddenField ID="Hid_SelVal" runat="server" />
                <asp:HiddenField ID="Hid_AnnSemiFlag" runat="server" />
                <asp:HiddenField ID="Hid_ShowNumber" runat="server" />
                <asp:HiddenField ID="Hid_T" runat="server" />
                <asp:HiddenField ID="Hid_FormatIndex" runat="server" />
                <asp:HiddenField ID="Hid_BlankHeaderImage" runat="server" Value="\CompanyLogo.png" />
                <asp:HiddenField ID="Hid_RowNo" runat="server" />
                <asp:HiddenField ID="Hid_BasisPoint" runat="server" />
                <asp:HiddenField ID="Hid_BrokerBasisPoint" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
