<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="ClientProfileMaster.aspx.vb" Inherits="Forms_ClientProfileMaster" Title="Customer Master" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" href="../Include/Style_IPO.css" rel="stylesheet" />

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>
    <style>
        .hidden {
            display: none;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            var strDetails = $("#<%= Hid_DocumentDetails.ClientID %>").val();
            $('#tblDocument tr').each(function (i, row) {
                if (i == 0)
                    return;
                $(this).remove();
            });

            if (strDetails != "") {
                strDetails = eval(strDetails);
                $(strDetails).each(function (i, item) {
                    AddDocumentDetails(item.Id, item.DocumentId, item.FileName);
                });
            }

            //Add new row
            $(document).on('click', '#lnkDocument', function () {
                AddDocumentDetails(0, 0, '');
                return false;
            });

            //Delete existing row
            $(document).on('click', 'a.delete', function () {
                var row = $(this).closest('tr');
                $(row).remove();
                return false;
            });
        });

        function ValidatFile(file) {
            var fileExtension = ['xls', 'xlsx', 'doc', 'docx', 'pdf', 'jpg', 'jpeg', 'png', 'gif'];
            var ext = $(file).val().split('.').pop().toLowerCase();

            if ($.inArray(ext, fileExtension) == -1 && ext != '') {
                AlertMessage('Validation', 'Sorry, only ' + fileExtension.join(', ') + '. file formats are allowed.', 175, 450);
            }
        }

        var counter = 1;
        function AddDocumentDetails(id, documentId, filename) {
            var strData = "<tr align='left'>";
            strData = strData + "<td style='display:none;'>" + id + "</td>";
            strData = strData + "<td><select class='combo' style='width:99%;' id='cboDocumentType" + counter + "'>" + getOption() + "</select></td>";
            strData = strData + "<td><input type='file' name='fileUpload" + counter + "' onchange='javascript:ValidatFile(this)' /></td>";
            if (id > 0)
                strData = strData + "<td style='text-align:center;'><a class='link_bold' href='javascript:window.location.href =\"showdocument.aspx?Id=" + id + "&ReportType=CustomerDocument&Type=CDD\";'>Download</td>";
            else
                strData = strData + "<td>&nbsp;</td>";
            strData = strData + "<td style='text-align:center;'><a href='' class='delete'><img title='Delete' class='imgdelete' src='../Images/delete.gif' /></a></td>";
            strData = strData + "</tr>";

            $("#tblDocument tbody").append(strData);
            if (documentId > 0)
                $("#tblDocument").find('#cboDocumentType' + counter).val(documentId);
            counter++;
        }

        function getOption() {
            var documents = $("#<%= Hid_DocumentMaster.ClientID %>").val();
            var stroptions = "";
            if (documents != "") {
                //documents = JSON.parse(documents);
                documents = eval(documents);
                $(documents).each(function (i, item) {
                    stroptions = stroptions + "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
            }
            return stroptions;
        }

        function ValidateDocument() {
            var Id = "";
            var DocumentId = "";
            var intRow = $("#tblDocument").find('tr').length;
            var fileExtension = ['xls', 'xlsx', 'doc', 'docx', 'pdf', 'jpg', 'jpeg', 'png', 'gif'];
            var pid, docid, docname, file;
            var ret = true;

            try {
                if (intRow > 1) {
                    $("#tblDocument").find('tr').each(function (i, row) {
                        if (i == 0)
                            return;

                        pid = row.children[0].innerHTML.trim();
                        docid = $($(row).find('select').get(0)).val();
                        docname = $($(row).find('select').get(0)).find("option:selected").text();
                        file = $($(row).find('input:file').get(0)).val();
                        var ext = file.split('.').pop().toLowerCase();
                        var filesize = $(row).find('input:file').get(0).files;

                        if (!docid > 0) {
                            AlertMessage('Validation', 'Please select associated document type first.', 175, 450);
                            ret = false;
                            return false;
                        }
                        else if (pid == 0 && file == '') {
                            AlertMessage('Validation', 'Please select associated file first for ' + docname + '.', 175, 450);
                            ret = false;
                            return false;
                        }
                        if ($.inArray(ext, fileExtension) == -1 && ext != '') {
                            AlertMessage('Validation', 'Sorry, only ' + fileExtension.join(', ') + '. file formats are allowed for ' + docname + '.', 175, 450);
                            ret = false;
                            return false;
                        }
                        else {
                            Id = Id + pid + ",";
                            DocumentId = DocumentId + docid + ",";
                        }

                        //work in ie 9 and above
                        if (file != '' && typeof filesize !== 'undefined')
                            if (filesize[0].size / (1024 * 1024) > 2) {
                                AlertMessage('Validation', 'Sorry, Document size must be less than 2MB for ' + docname + '.', 175, 450);
                                ret = false;
                                return false;
                            }
                    });

                    if (!ret)
                        return false;

                    Id = Id.substring(0, Id.length - 1);
                    DocumentId = DocumentId.substring(0, DocumentId.length - 1);
                }
                $('#<%= Hid_CustomerDocumentId.ClientID %>').val(Id);
                $('#<%= Hid_DocumentId.ClientID %>').val(DocumentId);
            }
            catch (err) {
                return false;
            }
            return true;
        }


        function Visiblefalse() {
            document.getElementById("tr_Emp").style.display = "none"
            document.getElementById("tr_Kyc").style.display = "none"
            document.getElementById("row_EmpalmentDt").style.display = "none"
            document.getElementById("row_EmpalmentFrequ").style.display = "none"
        }

        function SelectDocType() {
            var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DecType").value
            //alert(strDealSlipId)  
            if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_DecType").value == "K") {
                document.getElementById("tr_Emp").style.display = "none"
                document.getElementById("tr_Kyc").style.display = "none"
                document.getElementById("row_EmpalmentDt").style.display = "none"
                document.getElementById("row_EmpalmentFrequ").style.display = "none"
            }
            else if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_DecType").value == "E") {
                document.getElementById("tr_Emp").style.display = "none"
                document.getElementById("tr_Kyc").style.display = "none"
                document.getElementById("row_EmpalmentDt").style.display = ""
                document.getElementById("row_EmpalmentFrequ").style.display = ""
            }
            else if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_DecType").value == "B") {
                document.getElementById("tr_Emp").style.display = "none"
                document.getElementById("tr_Kyc").style.display = "none"
                document.getElementById("row_EmpalmentDt").style.display = ""
                document.getElementById("row_EmpalmentFrequ").style.display = ""
            }
            else {
                document.getElementById("tr_Emp").style.display = "none"
                document.getElementById("tr_Kyc").style.display = "none"
                document.getElementById("row_EmpalmentDt").style.display = "none"
                document.getElementById("row_EmpalmentFrequ").style.display = "none"
            }
        }


        function Deletedetails() {
            if (window.confirm("Are you sure u want to delete this detail record")) {
                return true
            }
            else {
                return false
            }
        }

        function ConvertUCase(txtBox) {
            txtBox.value = txtBox.value.toUpperCase()
        }
        function AddAddress() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType").value) == "") {
                AlertMessage('Validation', "Please select the Customer Type", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_CustomerName").value) == "") {
                AlertMessage('Validation', "Please Enter Customer Name", 175, 450);
                return false;
            }
            var pageUrl = "ClientCustomerAddress.aspx";
            var strId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value
            var strProfileType = "CM"
            var strBusniessType = "NO"

            var CustomerTypeId = document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType").value;
            pageUrl = pageUrl + "?CustomerId=" + strId + "&ProfileType=" + strProfileType + "&BusniessType=" + strBusniessType + "&CustomerTypeId=" + CustomerTypeId;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:990px;dialogHeight:600px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret
            }
            else {
            }
        }

        function ValidateDealer() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealerName").value) == "") {
                AlertMessage('Validation', "Please select dealer", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_StartDate").value) == "") {
                AlertMessage('Validation', "Please select start date", 175, 450);
                return false;
            }
            var startdate = Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_StartDate").value);
            var enddate = Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_EndDate").value);
            var Months = 'JanFebMarAprMayJunJulAugSepOctNovDec';
            var startdate1 = document.getElementById("ctl00_ContentPlaceHolder1_txt_StartDate").value.split('/');
            var enddate1 = document.getElementById("ctl00_ContentPlaceHolder1_txt_EndDate").value.split('/');
            var startdate2 = Months.substr((startdate1[1] - 1) * 3, 3) + ' ' + startdate1[0] + ', ' + startdate1[2];
            var enddate2 = Months.substr((enddate1[1] - 1) * 3, 3) + ' ' + enddate1[0] + ', ' + enddate1[2];
            startdate2 = Date.parse(startdate2);
            enddate2 = Date.parse(enddate2);
            if (enddate2 != "") {
                if (startdate2 >= enddate2) {
                    AlertMessage('Validation', "Start date cannot be greater or equal to end date", 175, 450);
                    return false;
                }
            }
        }

        function UpdateContactDetails(ctrl, rowIndex) {
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_RowIndex").value = rowIndex;
            var pageUrl = "ClientContactPerson.aspx";
            var strValues = "";
            var selValues = "";

            var strId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value
            var ctrlItem = document.getElementById("ctl00_ContentPlaceHolder1_dg_Contact_itm" + rowIndex)
            var row = $(ctrl).closest("tr");
            var val = row.find("td").eq(0).find("input").eq(0).val();

            var hidContactDetailId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ContactDetailId").value.split("!")
            var hidPhoneNo2 = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PhoneNo2").value.split("!")
            var hidFaxNo1 = document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaxNo1").value.split("!")
            var hidFaxNo2 = document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaxNo2").value.split("!")
            var Interaction = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Interaction").value.split("!")
            var SectionType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_SectionType").value.split("!")
            strValues = strValues + row.find("td").eq(0).find("input").eq(0).val() + "!"
            strValues = strValues + row.find("td:eq(1)").text().trim() + "!"
            strValues = strValues + row.find("td:eq(2)").text().trim() + "!"
            strValues = strValues + row.find("td:eq(3)").text().trim() + "!"
            strValues = strValues + row.find("td:eq(4)").text().trim() + "!"
            //strValues = strValues + ctrlItem.children(5).children(0).value + "!"                                         
            strValues = strValues + hidPhoneNo2[rowIndex] + "!"
            strValues = strValues + hidFaxNo1[rowIndex] + "!"
            strValues = strValues + hidFaxNo2[rowIndex] + "!"
            strValues = strValues + Interaction[rowIndex] + "!"
            strValues = strValues + SectionType[rowIndex] + "!"
            strValues = strValues + hidContactDetailId[rowIndex] + "!"

            strValues = strValues.replace("&", " ")
            pageUrl = pageUrl + "?CustomerId=" + strId + "&Values=" + strValues;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:600px;dialogHeight:600px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret
                document.getElementById('<%= btn_AddDetails.ClientID%>').click();
            }
            else {
            }
        }
        function ValidateDetails() {


            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_CustomerName").value) == "") {
                AlertMessage('Validation', 'Please Enter Customer Name', 175, 450)
                return false;
            }

            else {
                return AddClientContactPerson()
            }



            return true
        }


        function AddClientContactPerson() {
            var pageUrl = "ClientContactPerson.aspx";
            var strId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value
            pageUrl = pageUrl + "?CustomerId=" + strId;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:600px;dialogHeight:400px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret
                document.getElementById('<%= btn_AddDetails.ClientID%>').click();
            }
            else {
            }
        }

        function ShowDialogOpen(PageName, strWidth, strHeight) {
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
            var OpenUrl = PageName;
            var ret = window.open(OpenUrl, "Yes", DialogOptions);
            return ret
        }

        function ShowHeadCustomer() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_Headselect_0").checked == true) {
                document.getElementById("row_selectCust").style.display = "none"
            }
            else {
                document.getElementById("row_selectCust").style.display = ""
            }
        }
        function ShowCustodian() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_Rdo_Custodian_0").checked == true) {
                document.getElementById("row_Cutodianname").style.display = ""
                document.getElementById("row_custodainheader").style.display = ""
                document.getElementById("row_custodain").style.display = ""
            }
            else {
                document.getElementById("row_Cutodianname").style.display = "none"
                document.getElementById("row_custodainheader").style.display = "none"
                document.getElementById("row_custodain").style.display = "none"
            }
        }


        function ShowDetails(formName, width, height, top) {
            var Id = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value;

            var CustType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType");
            var CustomerType = CustType.options[CustType.options.selectedIndex].text;

            var CustCategory = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Category");

            var CustomerCategory = CustCategory.options[CustCategory.options.selectedIndex].text;
            var CategoryId = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Category").value;
            var CustomerTypeId = document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType").value;



            var ret = ShowDialogDetails(formName + ".aspx", Id, CustomerType, CustomerCategory, CustomerTypeId, CategoryId, width, height, top)
            if (ret == "" || typeof (ret) == "undefined") {
                return false
            }
            else {
                //                 window.location = "ClientProfileMaster.aspx?Id=" + Id;       
                //                return false
            }
        }
        function ShowDialogDetails(PageName, customerid, CustomerType, CustomerCategory, CustomerTypeId, CategoryId, strWidth, strHeight, strTop) {
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=" + strTop + "; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
            var OpenUrl = PageName + "?Id=" + customerid + "&CustomerType=" + CustomerType + "&CustomerCategory=" + CustomerCategory + "&CustomerTypeId=" + CustomerTypeId + "&CategoryId=" + CategoryId;
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
            return ret
        }
        function Validation() {

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType").value) == "") {
                AlertMessage('Validation', "Please select the Customer Type", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_CustomerName").value) == "") {
                AlertMessage('Validation', "Please Enter Customer Name", 175, 450);
                return false;
            }

            //if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_State_Hid_SelectedId").value) == "") {
            //    AlertMessage('Validation', "Please Select State Name", 175, 450);
            //    return false;
            //}

            if (document.getElementById("ctl00_ContentPlaceHolder1_Rdo_Custodian_0").checked == true) {
                if ((document.getElementById("ctl00_ContentPlaceHolder1_srh_Custodian_Hid_SelectedId").value) == "") {
                    AlertMessage('Validation', 'Please select Custodian Name', 175, 450)
                    return false
                }
            }

            var gridView = document.getElementById("<%=dg_Dealer.ClientID %>");
            var rows = gridView.getElementsByTagName("tr")
            if (rows.length == 1) {
                AlertMessage("Validation", "Please enter atleast one dealer details.", 175, 450);
                return false;
            }
            return ValidateDocument();
            return true
        }

        function fnValidatePAN(Obj) {

            if (Obj == null) Obj = window.event.srcElement;
            if (Obj.value != "") {
                ObjVal = Obj.value.toUpperCase();
                document.getElementById("ctl00_ContentPlaceHolder1_txt_PANNo").value = ObjVal


                var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                var code = /([C,P,H,F,A,T,B,L,J,G])/;
                var code_chk = ObjVal.substring(3, 4);

                if (ObjVal.search(panPat) == -1) {
                    AlertMessage('Validation', "Invalid Pan No", 175, 450);
                    Obj.focus();
                    return false;
                }
                if (code.test(code_chk) == false) {
                    AlertMessage('Validation', "Invaild PAN Card No.", 175, 450);
                    return false;
                }

            }
        }

        function ValidateInfo(lnkBtn) {
            var row = lnkBtn.parentElement.parentElement;
            var SGLTransWith = row.children[0].children[0].value;

            if (SGLTransWith == "") {
                AlertMessage('Validation', "Please specify SGL Info", 175, 450);
                return false;
            }
        }
        function ValidateErstWhileInfo(lnkBtn) {
            var row = lnkBtn.parentElement.parentElement;
            var ErstWhileName = row.children[0].children[0].value;

            if (ErstWhileName == "") {
                AlertMessage('Validation', "Please specify Name", 175, 450);
                return false;
            }
        }
        function ValidateSchemeInfo(lnkBtn) {
            var row = lnkBtn.parentElement.parentElement;
            var SGLTransWith = row.children[0].children[0].value;

            if (SGLTransWith == "") {
                AlertMessage('Validation', "Please specify Scheme Name", 175, 450);
                return false;
            }
        }
        function ValidateDPInfo(lnkBtn) {
            var row = lnkBtn.parentElement.parentElement;
            var DpName = row.children[0].children[0].value;
            var DpId = row.children[1].children[0].value;
            var ClientId = row.children[2].children[0].value;
            if (DpName == "" && DpId == "" && ClientId == "") {
                AlertMessage('Validation', "Please specify  DP Info", 175, 450);
                return false;
            }
            if (DpName == "") {
                AlertMessage('Validation', "Please specify  DP Name", 175, 450);
                return false;
            }
        }

        function ValidateBankInfo(lnkBtn) {
            var row = lnkBtn.parentElement.parentElement;
            var BankName = row.children[0].children[0].value;

            if (BankName == "") {
                AlertMessage('Validation', "Please specify  Bank Info", 175, 450);
                return false;
            }
        }
        function ValidateCustodianBankInfo(lnkBtn) {
            var row = lnkBtn.parentElement.parentElement;
            var CustodianBankName = row.children[0].children[0].value;

            if (CustodianBankName == "") {
                AlertMessage('Validation', "Please specify  Bank Info", 175, 450);
                return false;
            }
        }

        function ValidateContactInfo(lnkBtn) {
            var row = lnkBtn.parentElement.parentElement;
            var ContactPerson = row.children[0].children[0].value;
            var PhoneNo = row.children[0].children[0].value;


            if (ContactPerson == "") {
                AlertMessage('Validation', "Please specify  Contact Info", 175, 450);
                return false;
            }
        }

        function ValidateSignaturyInfo(lnkBtn) {
            var row = lnkBtn.parentElement.parentElement;
            var SignaturyName = row.children[0].children[0].value;


            if (SignaturyName == "") {
                AlertMessage('Validation', "Please specify  Signatory  Info", 175, 450);
                return false;
            }
        }
        function showPurchaseOrder() {
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=650px; dialogTop=50px; dialogHeight=550px; Help=No; Status=No; Resizable=Yes;";
            var CustomerId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value

            var OpenUrl = "ShowClientPurImage.aspx?CustomerId=" + CustomerId
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
            if (ret == "" || typeof (ret) == "undefined") {
                return false
            }
        }

        function Add() {
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg1")
            var tempId = 0
            if (grd != null) {
                if (grd.children[0].children.length >= 1) {
                    for (i = 1; i <= (grd.children[0].children.length - 1) ; i++) {
                        var tempId = tempId + 1;
                    }

                }
            }
            var pageUrl = "MainAddressMultiple.aspx";
            var strId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value
            var intClientCustId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ClientCustAddressId").value

            pageUrl = pageUrl + "?CustomerId=" + strId + "&TempId=" + tempId + "&ClientCustId=" + intClientCustId;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:890px;dialogHeight:600px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret
                document.getElementById('<%= btn_Add.ClientID%>').click();
            }
            else {
            }
        }

    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS" align="center" colspan="2">Client Profile Master
            </td>
        </tr>
        <tr>
            <td class="SeperatorRowCSS" colspan="2">&nbsp;
            </td>
        </tr>
        <%--  <tr align="left">
            <td class="SectionHeaderCSS" align="left" colspan="2">
                MAIN SECTION</td>
        </tr>--%>
        <tr>
            <td colspan="2" align="center" valign="top">
                <table cellpadding="0" cellspacing="0" border="0" width="98%">
                    <tr>
                        <td align="center" valign="top" style="width: 50%">
                            <%--  <atlas:UpdatePanel ID="UpdatePanel5" runat="server" Mode="Conditional">--%>
                            <%--  <ContentTemplate>--%>
                            <asp:HiddenField ID="Hid_DecType" runat="server"></asp:HiddenField>
                            <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                                <tr align="left">
                                    <td class="LabelCSS">Customer Code:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_CustomerCode" runat="server" CssClass="TextBoxCSS" TabIndex="3"
                                            ReadOnly="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">BSE Customer Code:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_BSECustomerCode" runat="server" CssClass="TextBoxCSS" Width="80px"
                                            TabIndex="3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">NSE Customer Code:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_NSECustomerCode" runat="server" CssClass="TextBoxCSS" Width="80px"
                                            TabIndex="3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" width="250">Customer Group:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cbo_CustomerGroup" Width="202px" runat="server" CssClass="ComboBoxCSS"
                                            AutoPostBack="false" TabIndex="1">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" width="250">Customer Type:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cbo_CustomerType" Width="202px" runat="server" CssClass="ComboBoxCSS"
                                            AutoPostBack="True" TabIndex="1">
                                        </asp:DropDownList>
                                        <i style="color: red; vertical-align: super;">*</i>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Customer Category:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cbo_Category" Width="202px" runat="server" CssClass="ComboBoxCSS"
                                            TabIndex="1" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Abbreviation :
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cbo_Abbreviation" Width="100px" runat="server" CssClass="ComboBoxCSS">
                                            <asp:ListItem Text="" Value=" "></asp:ListItem>
                                            <asp:ListItem Text="Mr." Value="Mr."></asp:ListItem>
                                            <asp:ListItem Text="Mrs." Value="Mrs."></asp:ListItem>
                                            <asp:ListItem Text="Ms." Value="Ms."></asp:ListItem>
                                            <asp:ListItem Text="M/S" Value="M/S"></asp:ListItem>
                                            <asp:ListItem Text="Dr." Value="Dr."></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Customer Name:
                                    </td>
                                    <td align="left">
                                        <%-- <asp:TextBox ID="txt_CustomerName" runat="server" CssClass="TextBoxCSS" Width="200px"
                                        TabIndex="2" EnableViewState="False" MaxLength="500"></asp:TextBox><i style="color: red">*</i></td>--%>
                                        <asp:TextBox TabIndex="2" ID="txt_CustomerName" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox><i
                                            style="color: red; vertical-align: super;">*</i>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Customer Prefix:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_CustPrefix" runat="server" CssClass="TextBoxCSS" Width="200px"
                                            TabIndex="2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left" visible="false" runat="server">
                                    <td class="LabelCSS">Head:
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rdo_Headselect" runat="server" BorderStyle="None" BorderWidth="1px"
                                            CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem Value="N">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr align="left" id="row_selectCust" runat="server" visible="false">
                                    <td class="LabelCSS">Select Head:
                                    </td>
                                    <td align="left">
                                        <uc:Search ID="srh_HeadCustomer" runat="server" AutoPostback="true" ProcName="ID_SEARCH_HeadCustomer"
                                            ConditionExist="true" SelectedFieldName="CustomerName" SourceType="StoredProcedure"
                                            TableName="CustomerMaster" ConditionalFieldName=" " ConditionalFieldId=" ">
                                        </uc:Search>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Address1:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_Address1" runat="server" CssClass="TextBoxCSS" Width="200px"
                                            TabIndex="3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Address2:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_Address2" runat="server" CssClass="TextBoxCSS" Width="200px"
                                            TabIndex="3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">City:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_City" runat="server" CssClass="TextBoxCSS" TabIndex="6"></asp:TextBox>
                                        <asp:Button ID="btn_Address" runat="server" Text="Add Address" ToolTip="Add Address"
                                            CssClass="ButtonCSS hidden" TabIndex="34" Width="35%" />
                                        <input type="button" id="btn_Address1" runat="server" value="Add Address" class="ButtonCSS" onclick="return AddAddress();" />
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Pin code:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_PinCode" runat="server" CssClass="TextBoxCSS" TabIndex="6"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">State:
                                    </td>
                                    <td align="left">
                                        <uc:Search ID="txt_State" Width="175" runat="server" AutoPostback="false" SelectedFieldId="Id" SelectedFieldName="StateName"
                                            PageName="StateName">
                                        </uc:Search>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Country:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_Country" runat="server" CssClass="TextBoxCSS" TabIndex="6"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" style="height: 26px">Phone No:
                                    </td>
                                    <td align="left" style="height: 26px">
                                        <asp:TextBox ID="txt_PhoneNo" runat="server" CssClass="TextBoxCSS" TabIndex="8" Height="30px"
                                            TextMode="MultiLine" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Fax No:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_FaxNo" runat="server" CssClass="TextBoxCSS" TabIndex="9" Height="30px"
                                            TextMode="MultiLine" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">PAN No:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_PANNo" runat="server" CssClass="TextBoxCSS" TabIndex="12"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Branch:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cbo_Branch" runat="server" Width="140px" CssClass="ComboBoxCSS">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="left" visible="false" runat="server">
                                    <td class="LabelCSS">Accessible To:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cbo_Accessible" runat="server" CssClass="ComboBoxCSS" Width="130px">
                                            <asp:ListItem Value="A">All</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="C">CurrentUser</asp:ListItem>
                                            <asp:ListItem Value="B">Branch</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Web Site:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_EmailId" runat="server" CssClass="TextBoxCSS" Width="200px"
                                            TabIndex="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">Custodian:
                                    </td>
                                    <td align="left" style="padding-left: 0px;">
                                        <asp:RadioButtonList ID="Rdo_Custodian" runat="server" BorderStyle="None" BorderWidth="1px"
                                            CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr align="left" id="row_Cutodianname">
                                    <td class="LabelCSS">Custodian Name:
                                    </td>
                                    <td align="left">
                                        <uc:Search ID="srh_Custodian" runat="server" PageName="CustodianName" AutoPostback="true" ConditionalFieldId="" ConditionalFieldName=""
                                            SelectedFieldId="Id" SelectedFieldName="CustodianName" />
                                        <%-- <asp:TextBox ID="txt_Custodian" runat="server" CssClass="TextBoxCSS"></asp:TextBox>--%>
                                    </td>
                                </tr>
                                <tr align="left" id="row_EmpalmentDt">
                                    <td class="LabelCSS">Empanelment Start date:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_empalmentdate" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                                        <a>
                                            <img class="formcontent" height="14" src="../Images/Calender.jpg" width="14" border="0"
                                                onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_empalmentdate',this);"
                                                align="middle" style="cursor: hand"></a>
                                    </td>
                                </tr>
                                <tr align="left" id="row_EmpalmentFrequ">
                                    <td class="LabelCSS">Empanelment Frequency:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cbo_FrequencyEmpalment" Width="150px" runat="server" CssClass="ComboBoxCSS"
                                            TabIndex="1">
                                            <asp:ListItem Value="Y" Selected="True">Yearly</asp:ListItem>
                                            <asp:ListItem Value="H">HalfYearly</asp:ListItem>
                                            <asp:ListItem Value="Q">Quarterlty</asp:ListItem>
                                            <asp:ListItem Value="M">Monthly</asp:ListItem>
                                            <asp:ListItem Value="N">None</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="left" id="tr_Emp" style="display: none">
                                    <td class="LabelCSS" width="250">Empanelment Documents Submitted:
                                    </td>
                                    <td valign="top">
                                        <uc:SelectFields ID="srh_EmpalementDocuments" runat="server" ProcName="ID_SEARCH_DocumentEmp1"
                                            ConditionalFieldName="CustomerTypeId" FormHeight="470" FormWidth="257" SelectedValueName="DTM.DocumentId"
                                            ChkLabelName="" ConditionalFieldId="cbo_CustomerType" ConditionExist="true" ShowAll="true"
                                            LabelName="" SelectedFieldName="DocumentName" SourceType="StoredProcedure" Visible="true"
                                            ShowLabel="false" Height="40"></uc:SelectFields>
                                    </td>
                                </tr>
                                <tr align="left" id="tr_Kyc" style="display: none">
                                    <td class="LabelCSS">KYC Documents Submitted:
                                    </td>
                                    <td valign="top">
                                        <uc:SelectFields ID="srh_KYCDocuments" runat="server" ProcName="ID_SEARCH_Documentkyc1"
                                            ConditionalFieldName="CustomerTypeId" FormHeight="470" FormWidth="257" SelectedValueName="DTM.DocumentId"
                                            ChkLabelName="" ConditionalFieldId="cbo_CustomerType" ShowAll="true" LabelName=""
                                            SelectedFieldName="DocumentName" SourceType="StoredProcedure" ConditionExist="true"
                                            Visible="true" ShowLabel="false" Height="40"></uc:SelectFields>
                                    </td>
                                    <%--  <td>
                                                <uc:SelectFields ID="srh_KYCDocuments" class="LabelCSS" runat="server" ProcName="ID_SEARCH_Documentkyc1"
                                                    FormHeight="470" FormWidth="257" SelectedValueName="DTM.DocumentTypeId" ChkLabelName=""
                                                    ConditionalFieldId="cbo_CustomerType" LabelName="" SelectedFieldName="DocumentName" SourceType="StoreProcedure"
                                                    ConditionalFieldName="CustomerTypeId" Visible="true" ShowLabel="false" Height="40" >
                                                </uc:SelectFields>
                                            </td>
                                    --%>
                                </tr>
                                <%-- 
                            <tr>
                                <td class="LabelCSS" width="250">
                                    Documents Submitted:
                                </td>
                                <td align="left">
                                    <table align="left" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td>
                                                <uc:SelectFields ID="srh_DocumentTypeName" class="LabelCSS" runat="server" ProcName="ID_SEARCH_DocumentTypeMaster"
                                                    FormHeight="470" FormWidth="257" SelectedValueName="CTM.DocumentTypeId" ChkLabelName=""
                                                    ConditionalFieldId="" LabelName="" SelectedFieldName="DocumentTypeName" SourceType="StoreProcedure"
                                                    ConditionalFieldName="" Visible="true" ShowLabel="false" Height="40">
                                                </uc:SelectFields>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                                <tr align="left" runat="server" visible="false">
                                    <td class="LabelCSS" width="150">Business Type:
                                    </td>
                                    <td align="left" style="padding-left: 0px;">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <uc:SelectFields ID="srh_BusniessType" class="LabelCSS" runat="server" ProcName="ID_SEARCH_BusinessTypeMaster"
                                                        FormHeight="470" FormWidth="257" SelectedValueName="BusinessTypeId" ChkLabelName=""
                                                        ConditionalFieldId="" LabelName="" SelectedFieldName="BusinessType" SourceType="StoredProcedure"
                                                        ConditionalFieldName="" Visible="true" ShowLabel="false" Height="75">
                                                    </uc:SelectFields>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="left" runat="server" visible="false">
                                    <td class="LabelCSS">Hide:
                                    </td>
                                    <td align="left" style="padding-left: 0px;">
                                        <asp:RadioButtonList ID="rdo_HideShow" runat="server" BorderStyle="None" BorderWidth="1px"
                                            CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="true">
                                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">GST No:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_GSTNo" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td class="LabelCSS">Settlement Type:
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rdo_BillSettType" runat="server" BorderStyle="None" BorderWidth="1px"
                                            CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem Selected="True" Value="D">Trade Date</asp:ListItem>
                                            <asp:ListItem Value="S">Settlement Date</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <%--   </ContentTemplate>--%>
                            <%--  </atlas:UpdatePanel>--%>
                        </td>
                        <td valign="top" align="center" style="width: 50%">
                            <table id="Table4" align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                                <tr class="HeadingCenter">
                                    <td>SGL Details
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        <%--<atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">--%>
                                        <%--   <ContentTemplate>--%>
                                        <div id="div_SGL" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 80px">
                                            <asp:DataGrid ID="dg_SGL" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="false"
                                                TabIndex="38" Width="100%">
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GridHeaderCSS" />
                                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="SGLTransWith" HeaderStyle-Width="80%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_SGLTransWith" Width="100%" runat="server" Text='<%# container.dataitem("SGLTransWith") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_SGLTransWith" CssClass="TextBoxCSS" runat="server" align="left"
                                                                onblur="ConvertUCase(this);" Text='<%# container.dataitem("SGLTransWith") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_SGLTransWith" CssClass="TextBoxCSS" runat="server" Width="80%"
                                                                onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="CustSGLId" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CustSGLId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustSGLId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Edit"
                                                                CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                                                CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                                                CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                                OnClientClick="return ValidateInfo(this)">
                                                            </asp:LinkButton>
                                                        </FooterTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                                CommandName="delete" CssClass="InfoLinkCSS" />
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                        <%-- </ContentTemplate>--%>
                                        <%--  </atlas:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr class="HeadingCenter">
                                    <td>DP Details
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        <%-- <atlas:UpdatePanel ID="UpdatePanel2" runat="server" Mode="Conditional">--%>
                                        <%--  <ContentTemplate>--%>
                                        <div id="div1" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 80px">
                                            <asp:DataGrid ID="dg_DP" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="false"
                                                TabIndex="38" Width="100%">
                                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Dp Name" HeaderStyle-Width="25%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_DpName" runat="server" Text='<%# container.dataitem("DpName") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_DpName" Width="90%" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"
                                                                Text='<%# container.dataitem("DpName") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_DpName" Width="90%" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" Width="120px" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Dp Id" HeaderStyle-Width="25%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_DpId" runat="server" Text='<%# container.dataitem("DpId") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_DpId" Width="90%" CssClass="TextBoxCSS" runat="server" Text='<%# container.dataitem("DpId") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_DpId" Width="90%" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" Width="60px" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Client Id" HeaderStyle-Width="25%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ClientId" runat="server" Text='<%# container.dataitem("ClientId") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_ClientId" Width="90%" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"
                                                                Text='<%# container.dataitem("ClientId") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_ClientId" Width="90%" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="CustDPId" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CustDPId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustDPId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Edit"
                                                                CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                                                CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                                                CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                                OnClientClick="return ValidateDPInfo(this)">
                                                            </asp:LinkButton>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderStyle-Width="15%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                                CommandName="delete" CssClass="InfoLinkCSS" />
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                        <%--   </ContentTemplate>--%>
                                        <%--    </atlas:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr class="HeadingCenter">
                                    <td>Bank Details
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td align="left" valign="top" style="height: 50px">
                                        <%-- <atlas:UpdatePanel ID="UpdatePanel3" runat="server" Mode="Conditional">--%>
                                        <%--  <ContentTemplate>--%>
                                        <div id="div4" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 80px">
                                            <asp:DataGrid ID="dg_Bank" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="false"
                                                TabIndex="38" Width="100%">
                                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Bank Name" HeaderStyle-Width="25%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_BankName" runat="server" Text='<%# container.dataitem("BankName") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_BankName" Width="90%" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"
                                                                Text='<%# container.dataitem("BankName") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_BankName" Width="90%" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Account No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_AccountNo" runat="server" Text='<%# container.dataitem("AccountNo") %>'
                                                                CssClass="LabelCSS" Width="90%"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_AccountNo" Width="90%" CssClass="TextBoxCSS" runat="server"
                                                                onblur="ConvertUCase(this);" Text='<%# container.dataitem("AccountNo") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_AccountNo" Width="90%" CssClass="TextBoxCSS" runat="server"
                                                                onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Branch">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Branch" runat="server" Text='<%# container.dataitem("Branch") %>'
                                                                CssClass="LabelCSS" Width="80%"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_Branch" Width="80%" CssClass="TextBoxCSS" onblur="ConvertUCase(this);"
                                                                runat="server" Text='<%# container.dataitem("Branch") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_Branch" Width="80%" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="RTGS Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_RTGSCode" runat="server" Text='<%# container.dataitem("RTGSCode") %>'
                                                                CssClass="LabelCSS" Width="70%"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_RTGSCode" Width="70%" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"
                                                                Text='<%# container.dataitem("RTGSCode") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_RTGSCode" Width="70%" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="CustBankId" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CustBankId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustBankId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Edit"
                                                                CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                                                CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                                                CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                                OnClientClick="return ValidateBankInfo(this)">
                                                            </asp:LinkButton>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                                CommandName="delete" CssClass="InfoLinkCSS" />
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                        <%--</ContentTemplate>--%>
                                        <%--  </atlas:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr class="HeadingCenter" style="display: none">
                                    <td>Signatory Details
                                    </td>
                                </tr>
                                <tr align="left" style="display: none">
                                    <td>
                                        <div id="dv_Signatury" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 80px">
                                            <asp:DataGrid ID="dg_Signatury" runat="server" CssClass="GridCSS" ShowFooter="True"
                                                AutoGenerateColumns="false" TabIndex="38" Width="100%">
                                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Signatory Name" HeaderStyle-Width="35%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_signaturyName" runat="server" Text='<%# container.dataitem("SignaturyName") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_signatury" CssClass="TextBoxCSS" Width="90%" runat="server"
                                                                onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="ContentType" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_FileContent" runat="server" Text='<%# container.dataitem("ContentType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="ContentLength" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ContentLength" runat="server" Text='<%# container.dataitem("ContentLength") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="FileBytes" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_FileBytes" runat="server" Text='<%# container.dataitem("FileBytes") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="FileName" HeaderStyle-Width="50%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="file_AddFile" runat="server" Text='<%# container.dataitem("FileName") %>'
                                                                CommandName="Show"> </asp:LinkButton>
                                                            <asp:Label ID="lbl_AddFile" runat="server" Text="Not Available" Visible="false" CssClass="MessageCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:FileUpload ID="file_AddFile" runat="server" Width="95%" />
                                                        </FooterTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderStyle-Width="15%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                                CommandName="delete" CssClass="InfoLinkCSS" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                                OnClientClick="return ValidateSignaturyInfo(this)">
                                                            </asp:LinkButton>
                                                        </FooterTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                    </td>
                                </tr>
                                <tr class="HeadingCenter" id="row_custodainheader">
                                    <td>Custodian Bank Details
                                    </td>
                                </tr>
                                <tr align="left" id="row_custodain">
                                    <td align="left">
                                        <%--      <atlas:UpdatePanel ID="UpdatePanel4" runat="server" Mode="Conditional">--%>
                                        <%--<ContentTemplate>--%>
                                        <div id="div3" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 80px">
                                            <asp:DataGrid ID="dgCustodianbankdetails" runat="server" CssClass="GridCSS" ShowFooter="True"
                                                AutoGenerateColumns="false" TabIndex="38" Width="100%">
                                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Bank Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CustodianBankName" runat="server" Text='<%# container.dataitem("CustodianBankName") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_CustodianBankName" Width="90%" CssClass="TextBoxCSS" runat="server"
                                                                onblur="ConvertUCase(this);" Text='<%# container.dataitem("CustodianBankName") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_CustodianBankName" Width="90%" CssClass="TextBoxCSS" runat="server"
                                                                onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Account No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CustodianAccountNo" runat="server" Text='<%# container.dataitem("CustodianAccountNo") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_CustodianAccountNo" Width="90%" CssClass="TextBoxCSS" runat="server"
                                                                onblur="ConvertUCase(this);" Text='<%# container.dataitem("CustodianAccountNo") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_CustodianAccountNo" Width="90%" CssClass="TextBoxCSS" runat="server"
                                                                onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Branch">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CustodianBranch" runat="server" Text='<%# container.dataitem("CustodianBranch") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_CustodianBranch" CssClass="TextBoxCSS" Width="75px" onblur="ConvertUCase(this);"
                                                                runat="server" Text='<%# container.dataitem("CustodianBranch") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_CustodianBranch" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="RTGS Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CustodianRTGSCode" runat="server" Text='<%# container.dataitem("CustodianRTGSCode") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_CustodianRTGSCode" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"
                                                                Text='<%# container.dataitem("CustodianRTGSCode") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_CustodianRTGSCode" CssClass="TextBoxCSS" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Edit"
                                                                CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                                                CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                                                CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                                OnClientClick="return ValidateCustodianBankInfo(this)">
                                                            </asp:LinkButton>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                                CommandName="delete" CssClass="InfoLinkCSS" />
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                        <%-- </ContentTemplate>--%>
                                        <%-- </atlas:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr class="HeadingCenter">
                                    <td>Scheme Details
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td align="left">
                                        <%--  <atlas:UpdatePanel ID="UpdatePanelScheme" runat="server" Mode="Conditional">--%>
                                        <%-- <ContentTemplate>--%>
                                        <div id="div_Scheme" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 80px">
                                            <asp:DataGrid ID="dg_Scheme" runat="server" CssClass="GridCSS" ShowFooter="True"
                                                AutoGenerateColumns="false" TabIndex="38" Width="100%">
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GridHeaderCSS" />
                                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Scheme" HeaderStyle-Width="80%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Scheme" runat="server" Text='<%# container.dataitem("SchemeName") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_Scheme" CssClass="TextBoxCSS" runat="server" align="left" onblur="ConvertUCase(this);"
                                                                Text='<%# container.dataitem("SchemeName") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_Scheme" CssClass="TextBoxCSS" runat="server" Width="80%" onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Edit"
                                                                CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                                                CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                                                CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                                OnClientClick="return ValidateSchemeInfo(this)">
                                                            </asp:LinkButton>
                                                        </FooterTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                                CommandName="delete" CssClass="InfoLinkCSS" />
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                        <%--   </ContentTemplate>--%>
                                        <%--   </atlas:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr class="HeadingCenter">
                                    <td>ErstWhile Details
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%--<atlas:UpdatePanel ID="UpdatePanelScheme" runat="server" Mode="Conditional">
                                <ContentTemplate>--%>
                                        <div id="div1" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 80px">
                                            <asp:DataGrid ID="dg_ErstWhile" runat="server" CssClass="GridCSS" ShowFooter="True"
                                                AutoGenerateColumns="false" TabIndex="38" Width="100%">
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GridHeaderCSS" />
                                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="ErstWhile Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CustomerErstWhile" Width="500px" runat="server" Text='<%#Container.DataItem("CustomerErstWhileName") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_CustomerErstWhile" CssClass="TextBoxCSS" runat="server" align="left"
                                                                Width="500px" onblur="ConvertUCase(this);" Text='<%#Container.DataItem("CustomerErstWhileName") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_CustomerErstWhile" CssClass="TextBoxCSS" runat="server" Width="500px"
                                                                onblur="ConvertUCase(this);"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CustomerErstWhileDate" Width="100px" runat="server" Text='<%# container.dataitem("ErstWhileDate") %>'
                                                                CssClass="LabelCSS"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_CustomerErstWhileDate" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDate();"
                                                                onChange="CheckDate(this,false);" Width="100px" runat="server" Text='<%# container.dataitem("ErstWhileDate") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txt_CustomerErstWhileDate" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDate();"
                                                                onChange="CheckDate(this,false);" Width="100px" runat="server"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Center" Width="100px" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="CustomerErstWhileId" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CustomerErstWhileId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerErstWhileId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" Width="20px"
                                                                CommandName="Edit" CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                                                CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                                                CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                                OnClientClick="return ValidateErstWhileInfo(this)">
                                                            </asp:LinkButton>
                                                        </FooterTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                                CommandName="delete" CssClass="InfoLinkCSS" />
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                        <%-- </ContentTemplate>
                            </atlas:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr align="left">
                                                <td class="LabelCSS">Remarks:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txt_Remarks" runat="server"
                                                        CssClass="TextBoxCSS" TabIndex="8" Height="50px"
                                                        TextMode="MultiLine" Width="300px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td class="LabelCSS">TAN No:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txt_TANNo" runat="server" CssClass="TextBoxCSS" TabIndex="12"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table width="55%" align="center">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="LabelCSS">Dealer:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cbo_DealerName" runat="server" CssClass="ComboBoxCSS" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="LabelCSS">Start Date:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_StartDate" runat="server" CssClass="TextBoxCSS jsdate" Width="100px"
                                            TabIndex="17"></asp:TextBox>
                                    </td>
                                    <td class="LabelCSS">End Date:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_EndDate" runat="server" CssClass="TextBoxCSS jsdate" Width="100px"
                                            TabIndex="17"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="Add_Dealer" runat="server" CssClass="ButtonCSS" Text="Save Dealer"
                    TabIndex="32" />
            </td>
        </tr>
        <tr id="row_dealer" runat="server">
            <td colspan="2" align="center">
                <div id="Div5" style="margin-top: 0px; overflow: auto; width: 80%; padding-top: 0px; position: relative; height: 130px; left: 0px; top: 0px;"
                    align="center">
                    <asp:DataGrid ID="dg_Dealer" runat="server" AutoGenerateColumns="False" ShowFooter="false"
                        Width="80%" CssClass="GridCSS">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                        <FooterStyle HorizontalAlign="Center" CssClass="footer" VerticalAlign="Middle"></FooterStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit" />
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                        CommandName="Delete" />
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Dealer">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_NameOFUser" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NameOFUser") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Start Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_StartDate" BackColor="white" Width="80px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.StartDate") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="End Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_EndDate" BackColor="white" Width="80px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.EndDate") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="CustomerId" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CustomerId" Width="75px" runat="server" Text='<%# container.dataitem("CustomerId") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="CustomerDealerId" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CustomerDealerId" Width="75px" runat="server" Text='<%# container.dataitem("CustomerDealerId") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="CustDealerDetailId" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CustDealerDetailId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustDealerDetailId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle PageButtonCount="2" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;
            </td>
        </tr>

        <tr id="row_Broker">
            <td colspan="2" align="center">
                <table width="55%" align="center">

                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="LabelCSS">Broker:
                                    </td>
                                    <td align="left">
                                        <uc:Search ID="srh_Brokername" Width="175" runat="server" AutoPostback="true" SelectedFieldId="Id" SelectedFieldName="BrokerName"
                                            PageName="BrokerName" ConditionalFieldId="">
                                        </uc:Search>
                                    </td>
                                    <td class="LabelCSS">Start Date:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_BrokerStartDate" runat="server" CssClass="TextBoxCSS jsdate" Width="100px"
                                            TabIndex="17"></asp:TextBox>
                                    </td>
                                    <td class="LabelCSS">End Date:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_BrokerEndDate" runat="server" CssClass="TextBoxCSS jsdate" Width="100px"
                                            TabIndex="17"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

            </td>

        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btn_AddBroker" runat="server" CssClass="ButtonCSS" Text="Save Broker"
                    TabIndex="32" />
            </td>
        </tr>
        <tr id="row_Brokergrid" runat="server">
            <td colspan="2" align="center">
                <div id="divb" style="margin-top: 0px; overflow: auto; width: 80%; padding-top: 0px; position: relative; height: 130px; left: 0px; top: 0px;"
                    align="center">
                    <asp:DataGrid ID="dg_Broker" runat="server" AutoGenerateColumns="False" ShowFooter="false"
                        Width="80%" CssClass="GridCSS">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                        <FooterStyle HorizontalAlign="Center" CssClass="footer" VerticalAlign="Middle"></FooterStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit" />
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                        CommandName="Delete" />
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Broker">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Broker" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BrokerName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Start Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_BrokerStartDate" BackColor="white" Width="80px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.StartDate") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="End Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_BrokerEndDate" BackColor="white" Width="80px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.EndDate") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="CustomerId" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CustomerId" Width="75px" runat="server" Text='<%# container.dataitem("CustomerId") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="CustomerbrokerId" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CustomerBrokerId" Width="75px" runat="server" Text='<%#Container.DataItem("CustomerBrokerId") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="CustBrokerDetailId" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CustBrokerDetailId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustBrokerDetailId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle PageButtonCount="2" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;
            </td>
        </tr>
        <tr id="row_contact" runat="server" visible="true">
            <td class="SubHeaderCSS" align="left" colspan="2">Contact Details
                <input type="button" id="btn_AddDetails1" runat="server" class="ButtonCSS" value="Add Details" onclick="return ValidateDetails();" />
                <asp:Button ID="btn_AddDetails" runat="server" Text="Add Details" ToolTip="Add Details"
                    CssClass="ButtonCSS hidden" TabIndex="34" Width="85px" />
            </td>
        </tr>
        <tr id="row_dgcontact" runat="server" visible="true">
            <td colspan="2" align="center">
                <div id="Div2" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 130px; left: 0px; top: 0px;"
                    align="center">
                    <asp:DataGrid ID="dg_Contact" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                        Width="98%" CssClass="GridCSS">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                        <FooterStyle HorizontalAlign="Center" CssClass="footer" VerticalAlign="Middle"></FooterStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="Contact Person">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_ContactPerson" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                        onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("ContactPerson") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Designation">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Designation" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Designation") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="PhoneNo1">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_PhoneNo1" Width="75px" runat="server" Text='<%# container.dataitem("PhoneNo1") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="MobileNo">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_MobileNo" Width="75px" runat="server" Text='<%# container.dataitem("MobileNo") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Email Id">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_EmailId" Width="75px" runat="server" Text='<%# container.dataitem("EmailId") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <%--  <asp:TemplateColumn HeaderText="BusinessTypeNames" Visible="False">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_BusinessTypeNames" BackColor="white" Width="120px" Style="border-left-width: 0;
                                        border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.BusinessTypeNames") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="BusniessTypeIds" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BusniessTypeIds" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BusniessTypeIds") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="NameOfUsers" Visible="False">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_NameOfUsers" BackColor="white" Width="120px" Style="border-left-width: 0;
                                        border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.NameOfUsers") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="UserIds" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_UserIds" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserIds") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>--%>
                            <asp:TemplateColumn HeaderText="PhoneNo2" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_PhoneNo2" Width="75px" runat="server" Text='<%# container.dataitem("PhoneNo2") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="FaxNo1" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FaxNo1" Width="75px" runat="server" Text='<%# container.dataitem("FaxNo1") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="FaxNo2" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FaxNo2" Width="75px" runat="server" Text='<%# container.dataitem("FaxNo2") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Interaction" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Interaction" Width="75px" runat="server" Text='<%# container.dataitem("Interaction") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="SectionType" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SectionType" Width="75px" runat="server" Text='<%# container.dataitem("SectionType") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="ContactDetailId" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ContactDetailId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ContactDetailId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="imgBtn_Edit" CommandName="Edit" runat="server" ToolTip="Edit"
                                        CssClass="TitleText hidden" Text="Edit">                                                                                                          
                                    </asp:LinkButton>
                                    <input type="button" id="imgBtn_Edit1" runat="server" class="TitleText" style="background-image: url('../Images/Edit3.PNG'); background-repeat: no-repeat" />
                                </ItemTemplate>
                                <FooterStyle Wrap="False"></FooterStyle>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="imgBtn_Del" CommandName="Delete" runat="server" ToolTip="Delete"
                                        CssClass="TitleText" Text="Delete">
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle Wrap="False"></FooterStyle>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle PageButtonCount="2" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr style="display: none;">
            <td class="SubHeaderCSS" align="left" colspan="2">Address Details
                <asp:Button ID="btn_Add" runat="server" Text="Add Details" ToolTip="Add Address"
                    CssClass="ButtonCSS hidden" TabIndex="34" />
                <input type="button" id="btn_Add1" runat="server" value="Add Details" class="ButtonCSS" onclick="return Add();" />
            </td>
        </tr>
        <tr style="display: none;">
            <td colspan="2" align="center">
                <table id="TableAddress" align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tr>
                        <td id="td2" align="center" valign="top" runat="server" colspan="8">
                            <div id="divdg" style="margin-top: 0px; overflow: auto; width: 950px; padding-top: 0px; position: relative; height: 168px; left: 0px; top: 0px;"
                                align="center">
                                <asp:DataGrid ID="dg1" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="False"
                                    TabIndex="38" Width="950px">
                                    <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit"
                                                    ToolTip="Edit" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                    CommandName="Delete" ToolTip="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="CustomerBranchName">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CustomerBranchName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerBranchName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Address1">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Address1" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                    onkeypress="scroll();"
                                                    runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("Address1") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Address2">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Address2" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                    onkeypress="scroll();"
                                                    runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("Address2") %>'></asp:TextBox>
                                                <%-- <asp:Label ID="lbl_Address2" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.Address2") %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="City">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_City" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.City") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="PinCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PinCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PinCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="State">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_State" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.State") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Country">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Country" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Country") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Phone">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Phone" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Phone") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="FaxNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_FaxNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FaxNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="EmailId">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_EmailId" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                    onkeypress="scroll();"
                                                    runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("EmailId") %>'></asp:TextBox>
                                                <%--  <asp:Label ID="lbl_EmailId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EmailId") %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="CustomerId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CustomerId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="BusinessType">
                                            <ItemTemplate>
                                                <asp:TextBox ID="lbl_BusinessTypeNames" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                    onkeypress="scroll();"
                                                    runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.BusinessTypeNames") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <%--<asp:TemplateColumn HeaderText="BusniessTypeIds" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_BusniessTypeIds" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BusniessTypeIds") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>--%>
                                        <asp:TemplateColumn HeaderText="ClientCustAddressId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ClientCustAddressId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ClientCustAddressId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_tempId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.tempId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
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
            <td class="ForControls" align="center" valign="middle" colspan="2">
                <asp:Label ID="LabelError" runat="server" CssClass="ForErrorMessages" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="SeperatorRowCSS" colspan="4"></td>
        </tr>
        <tr>
            <td class="HeaderCSS" align="center" style="width: 100%;" colspan="4">Upload Documents
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 95%;" colspan="4">
                <%-- <atlas:UpdatePanel ID="UpdatePanel6" runat="server" Mode="Conditional">
                    <Triggers>
                        <atlas:ControlEventTrigger ControlID="btn_Save" EventName="Click"/>
                    </Triggers>
                    <ContentTemplate>--%>
                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="data_table">
                    <tr>
                        <td width="50%" align="left" valign="top">
                            <table id="Table5" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td>
                                        <div id="Document">
                                            <div style="text-align: right;">
                                                <a id="lnkDocument" style="cursor: pointer;" title="Add new details">Add Details</a>
                                            </div>
                                            <div id="divDocument" runat="server" style="width: 100%;">
                                                <table id="tblDocument" cellpadding="0" cellspacing="0" class="table_border_right_bottom tablerowbg"
                                                    width="100%">
                                                    <tr class="table_heading">
                                                        <td style="width: 0%; display: none;"></td>
                                                        <td style="width: 20%;">Document Type
                                                        </td>
                                                        <td style="width: 50%;">Upload File
                                                        </td>
                                                        <td style="width: 20%;">Download
                                                        </td>
                                                        <td style="width: 3%;">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 0%; display: none;"></td>
                                                        <td style="width: 20%;">
                                                            <select id="cboDocumentType" class="combo">
                                                            </select>
                                                        </td>
                                                        <td style="width: 50%;">
                                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                                        </td>
                                                        <td style="width: 20%;"></td>
                                                        <td>
                                                            <a href="" class="delete">
                                                                <img title="Delete" class="imgdelete" src="../Images/delete.gif" alt="Delete" />
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <%--</ContentTemplate>
                </atlas:UpdatePanel>--%>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                <asp:HiddenField ID="Hid_CustImgId" runat="server" />
                <asp:HiddenField ID="Hid_RetValues" runat="server" />
                <asp:HiddenField ID="Hid_arrContactDetailIds" runat="server" />
                <asp:HiddenField ID="Hid_BusinessTypeId" runat="server" />
                <asp:HiddenField ID="Hid_ClientBusniessDetailId" runat="server" />
                <asp:HiddenField ID="Hid_BusinessTypeNames" runat="server" />
                <asp:HiddenField ID="Hid_ContactDetailId" runat="server" />
                <asp:HiddenField ID="Hid_ContactDetailIds" runat="server" />
                <asp:HiddenField ID="Hid_UserIds" runat="server" />
                <asp:HiddenField ID="Hid_NameOfUsers" runat="server" />
                <asp:HiddenField ID="Hid_PhoneNo2" runat="server" />
                <asp:HiddenField ID="Hid_FaxNo1" runat="server" />
                <asp:HiddenField ID="Hid_FaxNo2" runat="server" />
                <asp:HiddenField ID="Hid_ClientCustAddressId" runat="server" />
                <asp:HiddenField ID="Hid_DocumentDetails" runat="server" />
                <asp:HiddenField ID="Hid_CustomerDocumentId" runat="server" />
                <asp:HiddenField ID="Hid_DocumentId" runat="server" />
                <asp:HiddenField ID="Hid_DocumentMaster" runat="server" />
                <asp:HiddenField ID="Hid_Interaction" runat="server" />
                <asp:HiddenField ID="Hid_SectionType" runat="server" />
                <asp:HiddenField ID="Hid_RowIndex" runat="server" />
                <asp:HiddenField ID="Hid_TempId" runat="server" />
                <asp:HiddenField ID="Hid_DealeretailId" runat="server" />
                <asp:HiddenField ID="Hid_BrokerdetailId" runat="server" />


                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
