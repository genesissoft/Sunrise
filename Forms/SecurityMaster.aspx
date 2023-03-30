<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SecurityMaster.aspx.vb" Inherits="Forms_SecurityMaster" Title="Security Master" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" href="../Include/Style_IPO.css" rel="stylesheet" />

    <script language="javascript" src="../Include/Common.js" type="text/javascript"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>

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

            //debugger;
            var id = ('<%= ViewState("Id") %>');
            if (id == "") {
                $('#btnRating').hide();
            }
            else {
                $('#btnRating').show();
            }

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
                strData = strData + "<td style='text-align:center;'><a class='link_bold' href='javascript:window.location.href =\"showdocument.aspx?Id=" + id + "&ReportType=SecurityDocument&Type=SDD\";'>Download</td>";
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
                        //if (file != '' && typeof filesize !== 'undefined')
                        //    if (filesize[0].size / (1024 * 1024) > 2) {
                        //        AlertMessage('Validation', 'Sorry, Document size must be less than 2MB for ' + docname + '.', 175, 450);
                        //        ret = false;
                        //        return false;
                        //    }
                    });

                    if (!ret)
                        return false;

                    Id = Id.substring(0, Id.length - 1);

                    DocumentId = DocumentId.substring(0, DocumentId.length - 1);
                }
                $('#<%= Hid_SecurityDocumentId.ClientID %>').val(Id);
                $('#<%= Hid_DocumentId.ClientID %>').val(DocumentId);
            }
            catch (err) {
                return false;
            }
            return true;
        }


        function EnterTab(e) {

            if ((e.srcElement.id != 'ctl00_ContentPlaceHolder1_btn_Save') && (e.srcElement.id != 'ctl00_ContentPlaceHolder1_btn_Cancel') && (e.srcElement.id != 'ctl00_ContentPlaceHolder1_btn_Update')) {
                if (window.event.keyCode == 13)
                    window.event.keyCode = 9;
            }

            var KeyID = window.event.keyCode;
            switch (KeyID) {
                case 8:

                    return false;
                    break;

            }
        }

        function Validation() {

            if ($("#<%=Hid_SecurityCashFlow.ClientID %>").val() > 0) {
                if (confirm("Cashflow is already calculated for this security, do you want to recalcuate the same?"))
                    $("#<%=Hid_ReCalculateIPDates.ClientID %>").val('Y');
                else
                    $("#<%=Hid_ReCalculateIPDates.ClientID %>").val('N');
            }

            var cboSecuritytype = document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType")
            var strSecuritytype = cboSecuritytype.options[cboSecuritytype.options.selectedIndex].text

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType").value) == "") {
                AlertMessage('Validation', "Please select Security Type", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_IssuerOfSecurity_txt_Name").value) == "") {
                AlertMessage('Validation', "Please Enter Issuer Of Security", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_NameOfSecurity").value) == "") {
                AlertMessage('Validation', "Please Enter Name Of Security", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_FaceValue").value) == "") {
                AlertMessage('Validation', "Please enter Face Value", 175, 450);
                return false;
            }



            //             if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_Exchange_0").checked == false && document.getElementById("ctl00_ContentPlaceHolder1_cbo_Exchange_1").checked == false)
            //                { 
            //                    alert("Please Select Exchange");
            //                    return false;
            //                } 

            if (strSecuritytype != 'COMMERCIAL PAPERS' && strSecuritytype != 'CERTIFICATE OF DEPOSIT') {

                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_NSDLFaceValue").value) == "" || Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_NSDLFaceValue").value) == "0") {
                    AlertMessage('Validation', "Please enter Price Per Bond", 175, 450);
                    return false;
                }

                //                    if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_InterstDate").value) == "")
                //                    {
                //                    alert("Please enter Intrest Date");
                //                    return false;
                //                    } 

                var today = new Date()
                var currDate = new Date(today.getFullYear(), today.getMonth(), today.getDate())
                var InterestDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_InterstDate")
                var BookCloserDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_BookCloserDate")
                var DMATBookCloserDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_DMATBookCloserDate")
                var Issuedate = document.getElementById("ctl00_ContentPlaceHolder1_txt_Issuedate")
                var cboFrequencyOfInterest = document.getElementById("ctl00_ContentPlaceHolder1_cbo_FrequencyOfInterest")
                var strFrequencyOfInterest = cboFrequencyOfInterest.options[cboFrequencyOfInterest.options.selectedIndex].text
                var cboInstrumentName = document.getElementById("ctl00_ContentPlaceHolder1_cbo_InstrumentName")
                var strInstrumentName = cboInstrumentName.options[cboInstrumentName.options.selectedIndex].text

                //            var strInstrumentName = cboInstrumentName.options[cboInstrumentName.options.selectedIndex].text 

                if ((Date.parse(getmdy(InterestDate.value))) >= currDate) {
                    if ((Issuedate.value) == "") {
                        AlertMessage('Validation', "Please enter Issue Date", 175, 450);
                        return false;
                    }
                }



                if ((Date.parse(getmdy(Issuedate.value))) >= (Date.parse(getmdy(InterestDate.value)))) {
                    AlertMessage('Validation', 'Issue date should be less than Interest Date', 175, 450);
                    return false;
                }


                //                    if((Date.parse(getmdy(BookCloserDate.value))) >= (Date.parse(getmdy(InterestDate.value))))
                //                    {    

                //                    alert('Book Closure date should be less than Interest Date');
                //                    return false;
                //                    } 

                //                    if((Date.parse(getmdy(DMATBookCloserDate.value))) >= (Date.parse(getmdy(InterestDate.value))))
                //                    {                
                //                    alert('Dmat Book Closure date should be less than Interest Date');
                //                    return false;
                //                    }  


                if (strFrequencyOfInterest != "None") {
                    if ((((InterestDate.value))) == "") {
                        AlertMessage('Validation', "Please enter Interest Date", 175, 450);
                        return false;
                    }
                }

            }





            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_FrequencyOfInterest").value) == "N") {

                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_InterstDate").value) != "") {
                    AlertMessage('Validation', "InterestDate Should be blank in DDB", 175, 450);
                    return false;
                }

                //                 if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_BookCloserDate").value) != "")
                //                    {
                //                    alert("Book Closure Date Should be blank in DDB");
                //                    return false;
                //                    }

                //                 if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_DMATBookCloserDate").value) != "")
                //                    {
                //                    alert("DMAT Book Closure Date Should be blank in DDB");
                //                    return false;
                //                    }        


            }






            if (Validationmaturity() == false) {
                return false;
            }
             return ValidateDocument();
            return true
        }

        function Validationmaturity() {

            //debugger;
            var today = new Date()
            var currDate = new Date(today.getFullYear(), today.getMonth(), today.getDate())
            var Issuedate = document.getElementById("ctl00_ContentPlaceHolder1_txt_Issuedate")
            var cboInstrumentName = document.getElementById("ctl00_ContentPlaceHolder1_cbo_InstrumentName")
            var strInstrumentName = cboInstrumentName.options[cboInstrumentName.options.selectedIndex].text

            var grdmat = document.getElementById("ctl00_ContentPlaceHolder1_dg_Maturity")
            var grdcoupon = document.getElementById("ctl00_ContentPlaceHolder1_dg_Coupon")
            var grdcall = document.getElementById("ctl00_ContentPlaceHolder1_dg_Call")
            var grdput = document.getElementById("ctl00_ContentPlaceHolder1_dg_Put")

            if (grdmat != null) {
                var lstRowmat = grdmat.children[0].children[grdmat.rows.length - 2];
                if (grdmat.rows.length > 2) {
                    var lastmatdt = (lstRowmat.children[0].children[0].innerHTML);
                }
            }

            if (grdcoupon != null) {
                var lstRowcoupon = grdcoupon.children[0].children[grdcoupon.rows.length - 2];
                if (grdcoupon.rows.length > 2) {
                    var lastcoupondt = (lstRowcoupon.children[0].children[0].innerHTML);
                }

            }

            var lstRowcall = grdcall.children[0].children[grdcall.rows.length - 2];
            if (grdcall.rows.length > 2) {
                var lastcalldt = (lstRowcall.children[0].children[0].innerHTML);
            }
            var lastRowput = grdput.children[0].children[grdput.rows.length - 2];
            if (grdput.rows.length > 2) {
                var lastputdt = (lastRowput.children[0].children[0].innerHTML);
            }

            var cboFrequencyOfInterest = document.getElementById("ctl00_ContentPlaceHolder1_cbo_FrequencyOfInterest")
            var strFrequencyOfInterest = cboFrequencyOfInterest.options[cboFrequencyOfInterest.options.selectedIndex].text

            if (strInstrumentName == "Non-Perpetual") {
                if (grdmat.children[0].children.length <= 2) {
                    AlertMessage('Validation', 'Please Enter atleast one Maturity Option record', 175, 450);
                    return false
                }
                //                if ((grdcoupon != null) &&  (strFrequencyOfInterest != "None"))
                //                {
                //                    if (grdcoupon.children[0].children.length <= 2)
                //                    {
                //                        alert('Please Enter atleast one Coupon Option record');
                //                        return false
                //                    } 
                //                }
                if ((Date.parse(getmdy(Issuedate.value))) >= Date.parse(getmdy(lastmatdt))) {
                    AlertMessage('Validation', 'Maturity Date should be greater then Issue Date', 175, 450);
                    return false;
                }

                if (Date.parse(getmdy(lastcalldt)) > Date.parse(getmdy(lastmatdt))) {
                    AlertMessage('Validation', "Call date should be less than maturity date", 175, 450);
                    return false;
                }
                if (Date.parse(getmdy(lastputdt)) > Date.parse(getmdy(lastmatdt))) {
                    AlertMessage('Validation', "Put date should be less than maturity date", 175, 450);
                    return false;
                }
            }

            if ((Date.parse(getmdy(Issuedate.value))) >= Date.parse(getmdy(lastcalldt))) {
                AlertMessage('Validation', 'Call Date should be grater then Issue Date', 175, 450);
                return false;
            }
            if ((Date.parse(getmdy(Issuedate.value))) >= Date.parse(getmdy(lastputdt))) {
                AlertMessage('Validation', 'Put Date should be grater then Issue Date', 175, 450);
                return false;
            }

            if (strInstrumentName == "Non-Perpetual" && strFrequencyOfInterest != "None") {
                if (grdcoupon.children[0].children.length <= 2) {
                    AlertMessage('Validation', 'Please Enter atleast one Coupon Option record', 175, 450);
                    return false
                }
                if (lastcoupondt == "") {
                    AlertMessage('Validation', "Plese Enter coupon date.", 175, 450);
                    return false;
                }
                if (lastmatdt != lastcoupondt) {
                    AlertMessage('Validation', "Last maturity date and last coupon date should be same", 175, 450);
                    return false;
                }
            }

            //            if (lastmatdt != lastcoupondt)
            //            {
            //                alert("Last maturity date and last coupon date should be same");
            //                return false;
            //            }  

            if (strInstrumentName == "Perpetual") {
                if (lastcoupondt != "") {
                    AlertMessage('Validation', "Last Coupon date should be none.", 175, 450);
                    return false;
                }
                if (grdcall.children[0].children.length <= 2) {
                    AlertMessage('Validation', 'Please Enter atleast one Call Option record', 175, 450);
                    return false
                }
            }

            var grdmats = document.getElementById("ctl00_ContentPlaceHolder1_dg_Maturity").children[0]
            var facevalue = (document.getElementById("ctl00_ContentPlaceHolder1_txt_FaceValue").value - 0) * (document.getElementById("ctl00_ContentPlaceHolder1_cbo_Fv").value - 0)

            var matamt = 0
            if (grdmats != null) {
                for (i = 1; i < grdmats.children.length; i++) {
                    matamt = matamt + (grdmats.children[i].children[1].children[0].innerHTML - 0)
                }
                if ((strFrequencyOfInterest != "None") && (strInstrumentName != "Perpetual")) {

                    if (matamt != facevalue) {
                        if (window.confirm("Maturity Amount does not match with Facevalue," + '\n' + " Are you sure you want to Continue saving this record?")) {
                            return true
                        }
                        else {
                            return false
                        }
                    }
                }
            }
        }

        function Hidegrids() {

            var cboInstrumentName = document.getElementById("ctl00_ContentPlaceHolder1_cbo_InstrumentName")
            var strInstrumentName = cboInstrumentName.options[cboInstrumentName.options.selectedIndex].text
            var cboFreqInterest = document.getElementById("ctl00_ContentPlaceHolder1_cbo_FrequencyOfInterest")
            var strFreqInterest = cboFreqInterest.options[cboFreqInterest.options.selectedIndex].text
            var cboCouponOn = document.getElementById("ctl00_ContentPlaceHolder1_Cbo_CouponOn")
            var strCouponOn = cboCouponOn.options[cboCouponOn.options.selectedIndex].text
            var grdCoupon = document.getElementById("ctl00_ContentPlaceHolder1_dg_Coupon")
            var cboSecuritytype = document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType");
            var strSecuritytype = cboSecuritytype.options[cboSecuritytype.options.selectedIndex].text;

            if (strInstrumentName == "Perpetual") {
                document.getElementById("row_matheader").style.display = "none"
                document.getElementById("row_maturity").style.display = "none"
            }
            else {
                document.getElementById("row_matheader").style.display = ""
                document.getElementById("row_maturity").style.display = ""
            }

            grdCoupon.children[0].children[0].children[1].innerHTML = strCouponOn

            if (strSecuritytype == 'COMMERCIAL PAPERS' || strSecuritytype == 'CERTIFICATE OF DEPOSIT') {

                //document.getElementById("ctl00_ContentPlaceHolder1_txt_NSDLFaceValue").value='0';
                document.getElementById("ctl00_ContentPlaceHolder1_txt_Issuedate").value = '';
                document.getElementById("ctl00_ContentPlaceHolder1_txt_InterstDate").value = '';
                document.getElementById("ctl00_ContentPlaceHolder1_Cbo_CouponOn").value = 'R';
                document.getElementById("ctl00_ContentPlaceHolder1_row_issuedate").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_LastIP").style.display = "none";
                document.getElementById("row_CouponOn").style.display = "none";
                document.getElementById("tr_CallOptionHead").style.display = "none";
                document.getElementById("tr_CallOptionBody").style.display = "none";
                document.getElementById("tr_PutOptionHead").style.display = "none";
                document.getElementById("tr_PutOptionBody").style.display = "none";
                document.getElementById("tr_PutOptionBody").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_FrequencyOfInterest").value = "N";
                document.getElementById("ctl00_ContentPlaceHolder1_cbo_FrequencyOfInterest").disabled = true;
            }

            var cboFreqInterest = document.getElementById("ctl00_ContentPlaceHolder1_cbo_FrequencyOfInterest")
            var strFreqInterest = cboFreqInterest.options[cboFreqInterest.options.selectedIndex].text

            if ((strFreqInterest == "None") || (strSecuritytype == "CERTIFICATE OF DEPOSIT") || (strSecuritytype == "COMMERCIAL PAPERS")) {
                document.getElementById("row_Couponheader").style.display = "none";
                document.getElementById("row_Coupon").style.display = "none";
            }
            else {
                document.getElementById("row_Couponheader").style.display = "";
                document.getElementById("row_Coupon").style.display = "";
            }
        }



        function ValidateInfo(lnkBtn, typeFlag) {
            var row = lnkBtn.parentElement.parentElement;
            var infoDate = row.children[0].children[0].value;
            var infoAmt = row.children[1].children[0].value;
            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_InstrumentName").value == "P" && typeFlag == "I") {
                if (infoAmt == "") {
                    AlertMessage('Validation', "Please specify the Amount", 175, 450);
                    return false;
                }
            }
            else if (infoDate == "" || infoAmt == "") {
                AlertMessage('Validation', "Please specify Date as well as Amount", 175, 450);
                return false;
            }

            var grdmats = row.parentElement

            for (i = 1; i < grdmats.children.length; i++) {
                currRow = grdmats.children[i]
                var currmatdt = currRow.children[0].children[0].innerHTML
                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_InstrumentName").value != "P" && typeFlag != "I") {
                    if (infoDate == currmatdt) {
                        AlertMessage('Validation', 'Date is alreday Present.', 175, 450);
                        return false;
                    }
                }
            }

        }


        function ValidateRating() {

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_RatingOrg").value) == "") {
                AlertMessage('Validation', "Please select Rating Organization", 175, 450);
                return false;
            }
        }
        function ShowIPDates() {
            try {
                //alert(123);
                var id = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Id").value;
                window.open("securityipdates.aspx?id=" + id, "_blank", "toolbar=yes,scrollbars=yes,top=100,left=100,width=700,height=525");
            }
            catch (err) {
                alert(err);
            }
            return false;
        }

        function HideShowInterest() {
            var Input = $('#<%=rdo_InterstHolidays.ClientID %> input[type=radio]:checked').val();
            if (Input == 1) {
                document.getElementById("ctl00_ContentPlaceHolder1_row_Interest").style.display = "";
                var value = "0";
                var radio = $("[id*=ctl00_ContentPlaceHolder1_rdo_InterestSat] input[value=" + value + "]").prop('checked', true);
            } else {
                document.getElementById("ctl00_ContentPlaceHolder1_row_Interest").style.display = "none";
            }
        }

        function HideShowMaturitySat() {
            var Input = $('#<%=rdo_MaturityHolidays.ClientID %> input[type=radio]:checked').val();
            if (Input == 1) {
                document.getElementById("ctl00_ContentPlaceHolder1_row_MaturitySat").style.display = "";
                var value = "0";
                var radio = $("[id*=ctl00_ContentPlaceHolder1_rdo_InterestSatMaturity] input[value=" + value + "]").prop('checked', true);
            } else {
                document.getElementById("ctl00_ContentPlaceHolder1_row_MaturitySat").style.display = "none";
            }
        }

        <%--function ValidateDocumentDetails() {
            var DocumentTypeId = $('#<%=cboDocumentType1.ClientID %>').val();
            if (!(DocumentTypeId > 0)) {
                AlertMessage('Validation', 'Please select File Type First.', 175, 450, 'D');
                return false;
            }
            var fileUpload = $('#<%=FileUpload2.ClientID %>').get(0);
            var files = fileUpload.files;
            var fileExtension = ['xls', 'xlsx', 'doc', 'docx', 'pdf', 'jpg', 'jpeg', 'png', 'gif'];
            var ext = $(fileUpload).val().split('.').pop().toLowerCase();

            if ($(fileUpload).val() != '') {
                if ($.inArray(ext, fileExtension) == -1 && ext != '') {
                    AlertMessage('Validation', 'Sorry, only ' + fileExtension.join(', ') + '. file formats are allowed.', 175, 450);
                    return false;
                }
            } else {
                AlertMessage('Validation', 'Please select associated file first.', 175, 450, 'D');
                return false;
            }


        }--%>

    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"
        onkeydown="EnterTab(Event);">
        <tr>
            <td class="HeaderCSS" align="center" colspan="4">Security Master
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnAttatchDocs" />
                    </Triggers>
                    <ContentTemplate>--%>
                <table align="center" id="Table9" width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td class="SectionHeaderCSS" align="left" colspan="2">MAIN SECTION
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel ID="UpdatePanelIssuer" runat="server">
                                <ContentTemplate>
                                    <table id="Table2" align="right" cellspacing="0" cellpadding="0" border="0" width="90%">
                                        <tr visible="True" runat="server">
                                            <td class="LabelCSS">Name Of Instrument:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cbo_InstrumentName" Width="182px" runat="server" CssClass="ComboBoxCSS"
                                                    TabIndex="1">
                                                    <asp:ListItem Value="NP">Non-Perpetual</asp:ListItem>
                                                    <asp:ListItem Value="P">Perpetual</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Security Type:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cbo_SecurityType" Width="182px" runat="server" CssClass="ComboBoxCSS"
                                                    TabIndex="2" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <em><span style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Security Type Code:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_SecurityTypeCode" runat="server" CssClass="TextBoxCSS" Width="150px"
                                                    TabIndex="3"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td class="LabelCSS">Security Category:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cbo_SecurityCategory" Width="182px" runat="server" CssClass="ComboBoxCSS"
                                                    TabIndex="1" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Security Code:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_SecurityCode" runat="server" CssClass="TextBoxCSS" Width="150px"
                                                    TabIndex="4"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="color: blue" class="LabelCSS">Example:
                                            </td>
                                            <td align="left" style="color: blue; font-size: 10px">9.69% TATA MOTORS 29032019
                                            </td>
                                        </tr>
                                        <tr id="Tr1" runat="Server" visible="false">
                                            <td class="LabelCSS">Issuer of Security:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_IssuerOfSecurrity" runat="server" CssClass="TextBoxCSS" Width="250px"
                                                    TabIndex="9"></asp:TextBox><em><span style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Issuer Of Security:
                                            </td>
                                            <td>
                                                <uc:Search ID="srh_IssuerOfSecurity" Width="175" runat="server" AutoPostback="true" SelectedFieldId="Id" SelectedFieldName="SecurityIssuer"
                                                    PageName="NameOfIssuerRDM">
                                                </uc:Search>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Name Of Security:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_NameOfSecurity" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="6" ToolTip="eg: 7.27% FOOD CORPORATION OF INDIA BONDS 31/03/2015"></asp:TextBox><em><span
                                                        style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="color: blue" class="LabelCSS">Example:
                                            </td>
                                            <td align="left" style="color: blue; font-size: 10px">9.69% TATA MOTORS NCD 29/03/2019
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false">
                                            <td class="LabelCSS">Guaranted By:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Granttedby" runat="server" CssClass="TextBoxCSS" Width="250px"
                                                    TabIndex="7"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Face Value:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_FaceValue" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="8"></asp:TextBox>
                                                <asp:DropDownList ID="cbo_Fv" runat="server" CssClass="ComboBoxCSS" Width="75px"
                                                    TabIndex="9">
                                                    <asp:ListItem Text="RUPEES" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="LACS" Value="100000"></asp:ListItem>
                                                    <asp:ListItem Text="CRORES" Value="10000000"></asp:ListItem>
                                                </asp:DropDownList>
                                                <i style="color: red">*</i>
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false">
                                            <td class="LabelCSS">Issue Price:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_IssuePrice" runat="server" CssClass="TextBoxCSS" Width="150px"
                                                    TabIndex="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="row_Priceperbond" runat="server">
                                            <td class="LabelCSS">Price Per Bond:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_NSDLFaceValue" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="11"></asp:TextBox>
                                                <asp:DropDownList ID="cbo_PriceMultiple" runat="server" CssClass="ComboBoxCSS" Width="75px"
                                                    TabIndex="12">
                                                    <asp:ListItem Text="RUPEES" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="LACS" Value="100000"></asp:ListItem>
                                                    <asp:ListItem Text="CRORES" Value="10000000"></asp:ListItem>
                                                </asp:DropDownList>
                                                <i style="color: red">*</i>
                                            </td>
                                        </tr>
                                        <tr id="row_issuedate" runat="server">
                                            <td class="LabelCSS">Issue Date:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Issuedate" runat="server" CssClass="TextBoxCSS jsdate" Width="150px"
                                                    TabIndex="13"></asp:TextBox>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Frequency Of Interest:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cbo_FrequencyOfInterest" Width="80px" runat="server" CssClass="ComboBoxCSS"
                                                    TabIndex="1" AutoPostBack="true">
                                                    <asp:ListItem Value="Y" Selected="True">Yearly</asp:ListItem>
                                                    <asp:ListItem Value="H">HalfYearly</asp:ListItem>
                                                    <asp:ListItem Value="Q">Quarterlty</asp:ListItem>
                                                    <asp:ListItem Value="M">Monthly</asp:ListItem>
                                                    <asp:ListItem Value="N">None</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RadioButtonList ID="rdo_NormCompound" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                                    CssClass="LabelCSS">
                                                    <asp:ListItem Selected="True" Text="Normal" Value="N"></asp:ListItem>
                                                    <asp:ListItem Text="Compound" Value="C"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr id="row_LastIP" runat="server">
                                            <td class="LabelCSS">First IP Date:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_InterstDate" runat="server" CssClass="TextBoxCSS jsdate" Width="75px"
                                                    TabIndex="15"></asp:TextBox>
                                                <asp:RadioButtonList ID="rdo_MaxActual" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                                    CssClass="LabelCSS" TabIndex="16">
                                                    <asp:ListItem Text="Max" Value="M"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="Actual" Value="A"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Interest Days
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cboInterestDaysType" runat="server" CssClass="combo">
                                                    <asp:ListItem Text="Actual/Actual" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Equal Days" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false">
                                            <td class="LabelCSS">Book Closure Date:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_BookCloserDate" runat="server" CssClass="TextBoxCSS jsdate" Width="150px"
                                                    TabIndex="17"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="true">
                                            <td class="LabelCSS">Shut Period Date:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_DMATBookCloserDate" runat="server" CssClass="TextBoxCSS jsdate" Width="150px"
                                                    TabIndex="18"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">ISIN Number:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_ISINNo" runat="server" CssClass="TextBoxCSS" Width="150px" TabIndex="19"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false">
                                            <td class="LabelCSS">RBI Loan Code:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_RBILoanCode" runat="server" CssClass="TextBoxCSS" Width="150px"
                                                    TabIndex="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="row_CouponOn">
                                            <td class="LabelCSS">Coupon On:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="Cbo_CouponOn" Width="151px" runat="server" CssClass="ComboBoxCSS"
                                                    TabIndex="21">
                                                    <asp:ListItem Selected="True" Value="R">Rate</asp:ListItem>
                                                    <asp:ListItem Value="A">Amount</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Exchange:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="Cbo_Exchange" Width="151px" runat="server" CssClass="ComboBoxCSS"
                                                    TabIndex="1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style ="display :none ">
                                            <td class="LabelCSS">Interest Days:
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList ID="rdo_AccIntDays" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS">
                                                    <asp:ListItem Value="365" Selected="True">365</asp:ListItem>
                                                    <asp:ListItem Value="366">366</asp:ListItem>
                                                    <asp:ListItem Value="360">360</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Remark:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Remark" runat="server" CssClass="TextBoxCSS" Width="250px" TabIndex="23" TextMode ="MultiLine" Height ="50px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </td>
                        <td valign="top">
                            <asp:UpdatePanel ID="UpdatePanelDetailsSection" runat="server">
                                <ContentTemplate>
                                    <table id="Table5" align="left" cellspacing="0" cellpadding="0" border="0">
                                        <tr id="row_matheader">
                                            <td class="SubHeaderCSS">Maturity
                                            </td>
                                        </tr>
                                        <tr id="row_maturity">
                                            <td>
                                                <div id="div5" style="margin-top: 0px; overflow: auto; width: 500px; padding-top: 0px; position: relative; height: 150px">
                                                    <asp:DataGrid ID="dg_Maturity" runat="server" CssClass="GridCSS" ShowFooter="True"
                                                        AutoGenerateColumns="false" TabIndex="38" Width="500px">
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_SecurityInfoDate" Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoDate") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoDate" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDate();"
                                                                        onblur="CheckDate(this,false);" Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoDate") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoDate" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDate();"
                                                                        onblur="CheckDate(this,false);" Width="150px" runat="server"></asp:TextBox>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" Width="150px" />
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_SecurityInfoAmt" runat="server" Width="150px" Text='<%#Container.DataItem("SecurityInfoAmt") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoAmt" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"
                                                                        Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoAmt") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoAmt" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"
                                                                        Width="150px" runat="server"></asp:TextBox>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" Width="150px" />
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                                                        OnClientClick="return ValidateInfo(this,'M')">
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
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
                                            </td>
                                        </tr>
                                        <tr id="row_Couponheader">
                                            <td class="SubHeaderCSS">Coupon
                                            </td>
                                        </tr>
                                        <tr id="row_Coupon">
                                            <td>
                                                <div id="div6" style="margin-top: 0px; overflow: auto; width: 500px; padding-top: 0px; position: relative; height: 70px">
                                                    <asp:DataGrid ID="dg_Coupon" runat="server" CssClass="GridCSS" ShowFooter="True"
                                                        AutoGenerateColumns="false" TabIndex="39" Width="500px">
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_SecurityInfoDate" Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoDate") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoDate" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDate();"
                                                                        onblur="CheckDate(this,false);" Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoDate") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoDate" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDate();"
                                                                        onblur="CheckDate(this,false);" Width="150px" runat="server"></asp:TextBox>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" Width="150px" />
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_SecurityInfoAmt" runat="server" Width="75px" Text='<%#Container.DataItem("SecurityInfoAmt") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoAmt" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"
                                                                        Width="75px" runat="server" Text='<%#Container.DataItem("SecurityInfoAmt") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoAmt" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"
                                                                        Width="75px" runat="server"></asp:TextBox>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" Width="75px" />
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Rate1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_SecurityInfoAmt1" runat="server" Width="75px" Text='<%#Container.DataItem("SecurityInfoAmt1") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoAmt1" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"
                                                                        Width="75px" runat="server" Text='<%#Container.DataItem("SecurityInfoAmt1") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoAmt1" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"
                                                                        Width="75px" runat="server"></asp:TextBox>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" Width="75px" />
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                                                        OnClientClick="return ValidateInfo(this,'I')">
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
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
                                            </td>
                                        </tr>
                                        <tr id="tr_CallOptionHead">
                                            <td class="SubHeaderCSS">Call Option
                                            </td>
                                        </tr>
                                        <tr id="tr_CallOptionBody">
                                            <td>
                                                <div id="div3" style="margin-top: 0px; overflow: auto; width: 500px; padding-top: 0px; position: relative; height: 70px">
                                                    <asp:DataGrid ID="dg_Call" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="false"
                                                        TabIndex="40" Width="500px">
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_SecurityInfoDate" Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoDate") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoDate" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDate();"
                                                                        onblur="CheckDate(this,false);" Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoDate") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoDate" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDate();"
                                                                        onblur="CheckDate(this,false);" Width="150px" runat="server"></asp:TextBox>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" Width="150px" />
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_SecurityInfoAmt" runat="server" Width="150px" Text='<%#Container.DataItem("SecurityInfoAmt") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoAmt" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"
                                                                        Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoAmt") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoAmt" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"
                                                                        Width="150px" runat="server"></asp:TextBox>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" Width="150px" />
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                                                        OnClientClick="return ValidateInfo(this,'C')">
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
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
                                            </td>
                                        </tr>
                                        <tr id="tr_PutOptionHead">
                                            <td class="SubHeaderCSS">Put Option
                                            </td>
                                        </tr>
                                        <tr id="tr_PutOptionBody">
                                            <td>
                                                <div id="div4" style="margin-top: 0px; overflow: auto; width: 500px; padding-top: 0px; position: relative; height: 70px">
                                                    <asp:DataGrid ID="dg_Put" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="false"
                                                        TabIndex="41" Width="500px">
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_SecurityInfoDate" Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoDate") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoDate" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDate();"
                                                                        onblur="CheckDate(this,false);" Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoDate") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoDate" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDate();"
                                                                        onblur="CheckDate(this,false);" Width="150px" runat="server"></asp:TextBox>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" Width="150px" />
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_SecurityInfoAmt" runat="server" Width="150px" Text='<%#Container.DataItem("SecurityInfoAmt") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoAmt" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"
                                                                        Width="150px" runat="server" Text='<%#Container.DataItem("SecurityInfoAmt") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txt_SecurityInfoAmt" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"
                                                                        Width="150px" runat="server"></asp:TextBox>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" Width="150px" />
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                                                        OnClientClick="return ValidateInfo(this,'P')">
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
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
                                            </td>
                                        </tr>
                                        <tr id="row_Mat" runat="server" visible="false">
                                            <td>Maturity Date (If Holiday):
                                                <asp:RadioButtonList ID="rdo_MatDate" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                                    CssClass="LabelCSS">
                                                    <asp:ListItem Text="Previous" Selected="True" Value="P"></asp:ListItem>
                                                    <asp:ListItem Text="Next" Value="N"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr id="Tr2" runat="server">
                                            <td>Master Creation Date :
                                                <asp:label ID="lbl_CreationDate" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                                    CssClass="LabelCSS">
                                                    
                                                </asp:label>
                                            </td>

                                           
                                        </tr>
                                      
                                         <tr id="Tr3" runat="server">
                                             <td>Created By:
                                                <asp:label ID="lbl_CreatedBy" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                                    CssClass="LabelCSS">
                                                    
                                                </asp:label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </td>
                    </tr>
                    <tr id="row_InterestDet" runat="server">
                        <td colspan="2">
                            <table width="100%">
                                <tr>
                                    <td class="SectionHeaderCSS" align="left" colspan="4" width="100%">INTEREST DETAILS
                                    </td>
                                </tr>
                                <tr align="center" height="60px" valign="top">
                                    <td colspan="6" align="left">
                                        <table align="left" id="" width="65%" cellspacing="0" cellpadding="0" border="0"
                                            bordercolor="red">
                                            <tr>
                                                <td valign="top" align="left">
                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                        <td align="left">
                                                            <tr id="row_InterestHoliday" runat="server">
                                                                <td class="LabelCSS" width="180px">Calc Interest on Holidays:
                                                                </td>
                                                                <td align="left">
                                                                    <asp:RadioButtonList ID="rdo_InterstHolidays" runat="server" RepeatDirection="Horizontal"
                                                                        RepeatLayout="Flow" CssClass="LabelCSS" AutoPostBack="false" onchange="HideShowInterest();">
                                                                        <asp:ListItem Value="0" Selected="True">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="1">No</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr id="row_Interest" runat="server" style="display: none;">
                                                                <td class="LabelCSS">Calc Interest on Saturday:
                                                                </td>
                                                                <td align="left">
                                                                    <asp:RadioButtonList ID="rdo_InterestSat" runat="server" RepeatDirection="Horizontal"
                                                                        RepeatLayout="Flow" CssClass="LabelCSS">
                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td class="LabelCSS">Issue Date Inclusive:
                                                                </td>
                                                                <td align="left">
                                                                    <asp:RadioButtonList ID="rdo_IssueDateInclusive" runat="server" RepeatDirection="Horizontal"
                                                                        RepeatLayout="Flow" CssClass="LabelCSS">
                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td class="LabelCSS">Maturity Date Inclusive:
                                                                </td>
                                                                <td align="left">
                                                                    <asp:RadioButtonList ID="rdo_MaturityDateInclusive" runat="server" RepeatDirection="Horizontal"
                                                                        RepeatLayout="Flow" CssClass="LabelCSS">
                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                        </td>
                                                    </table>
                                                </td>
                                                <td valign="top" align="left">
                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                        <td align="left">
                                                            <tr id="row_MaturityInterest" runat="server">
                                                                <td class="LabelCSS" width="180px">Calc Interest on Maturity:
                                                                </td>
                                                                <td align="left">
                                                                    <asp:RadioButtonList ID="rdo_MaturityHolidays" runat="server" RepeatDirection="Horizontal"
                                                                        RepeatLayout="Flow" CssClass="LabelCSS" AutoPostBack="false" onchange="HideShowMaturitySat();">
                                                                        <asp:ListItem Value="0" Selected="True">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="1">No</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr id="row_MaturitySat" runat="server" style="display: none;">
                                                                <td class="LabelCSS">Calc Interest on Saturday:
                                                                </td>
                                                                <td align="left">
                                                                    <asp:RadioButtonList ID="rdo_InterestSatMaturity" runat="server" RepeatDirection="Horizontal"
                                                                        RepeatLayout="Flow" CssClass="LabelCSS">
                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td colspan="2">&nbsp;
                                                                </td>
                                                            </tr>

                                                        </td>
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
                        <td class="SectionHeaderCSS" align="left" colspan="2" width="80%">ADDITIONAL INFORMATION
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                            </table>
                        </td>
                    </tr>
                    <tr id="row_secRating" runat="server" visible="false">
                        <td colspan="2">
                            <table id="Table7" width="60%" align="center" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td class="LabelCSS">Long Rating1:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Rating1" runat="server" CssClass="TextBoxCSS" TabIndex="24"></asp:TextBox>
                                    </td>
                                    <td class="LabelCSS">Short Rating1:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ShortRating1" runat="server" CssClass="TextBoxCSS" TabIndex="25"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">Long Rating2:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Rating2" runat="server" CssClass="TextBoxCSS" TabIndex="26"></asp:TextBox>
                                    </td>
                                    <td class="LabelCSS">Short Rating2:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ShortRating2" runat="server" CssClass="TextBoxCSS" TabIndex="27"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">Long Rating3:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Rating3" runat="server" CssClass="TextBoxCSS" TabIndex="28"></asp:TextBox>
                                    </td>
                                    <td class="LabelCSS" width="100px">Short Rating3:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ShortRating3" runat="server" CssClass="TextBoxCSS" TabIndex="29"></asp:TextBox>
                                    </td>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:UpdatePanel ID="UpdatePannelRating" runat="server">
                                <ContentTemplate>
                                    <table width="80%" align="center">
                                        <tr>
                                            <td>
                                                <tr>
                                                    <td class="LabelCSS">Company:
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cbo_RatingOrg" Width="200px" runat="server" CssClass="ComboBoxCSS"
                                                            TabIndex="1" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="LabelCSS">Rating:
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cbo_Rating" Width="200px" runat="server" CssClass="ComboBoxCSS"
                                                            TabIndex="1">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="LabelCSS">Rating Date:
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txt_RatingDate" runat="server" CssClass="TextBoxCSS jsdate" Width="100px"
                                                            TabIndex="17"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6" align="center">
                                                        <asp:Button ID="Add_Rating" runat="server" CssClass="ButtonCSS" Text="Add Rating"
                                                            TabIndex="32" />
                                                    </td>
                                                </tr>
                                                <tr id="row_Rating" runat="server">
                                                    <td colspan="6" align="center">
                                                        <div id="Div2" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 130px; left: 0px; top: 0px;"
                                                            align="center">
                                                            <asp:DataGrid ID="dg_Rating" runat="server" AutoGenerateColumns="False" ShowFooter="false"
                                                                Width="98%" CssClass="GridCSS">
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                                <FooterStyle HorizontalAlign="Center" CssClass="footer" VerticalAlign="Middle"></FooterStyle>
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="OrganizationName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_OrganizationName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrganizationName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Rating">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lbl_Rating" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                                                onkeypress="scroll();"
                                                                                runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.Rating") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="RatingDate">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lbl_RatingDate" BackColor="white" Width="80px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                                                onkeypress="scroll();"
                                                                                runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.RatingDate") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="SecurityId" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_SecurityId" Width="75px" runat="server" Text='<%#Container.DataItem("SecurityId") %>'
                                                                                CssClass="LabelCSS"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="RatingOrganizationId" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_RatingOrganizationId" Width="75px" runat="server" Text='<%#Container.DataItem("RatingOrganizationId") %>'
                                                                                CssClass="LabelCSS"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="RatingId" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_RatingId" Width="75px" runat="server" Text='<%#Container.DataItem("RatingId") %>'
                                                                                CssClass="LabelCSS"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="SecurityRatingId" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_SecurityRatingId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityRatingId") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="imgBtn_Edit" CommandName="Edit" runat="server" ToolTip="Edit"
                                                                                CssClass="TitleText" Text="Edit">                                                                                                          
                                                                            </asp:LinkButton>
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
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="SeperatorRowCSS" colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="HeaderCSS" align="center" style="width: 100%;" colspan="2">Upload Documents
                        </td>
                    </tr>
                    <tr id="tr_Docs">
                        <td align="center" colspan="3">
                           
                            <table id="col1" cellpadding="0" cellspacing="0" border="0" width="100%">
                                
                                <tr>
                                    <td align="center" style="width: 95%;" colspan="2">
                                        <%-- <atlas:UpdatePanel ID="UpdatePanel6" runat="server" Mode="Conditional">
                    <Triggers>
                        <atlas:ControlEventTrigger ControlID="btn_Save" EventName="Click"/>
                    </Triggers>
                    <ContentTemplate>--%>
                                        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="data_table">
                                            <tr>
                                                <td width="50%" align="left" valign="top">
                                                    <table id="Table3" width="100%" cellspacing="0" cellpadding="0" border="0">
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
                                                                                <td style="width: 20%;">Document Type</td>
                                                                                <td style="width: 50%;">Upload File</td>
                                                                                <td style="width: 20%;">Download</td>
                                                                                <td style="width: 3%;">&nbsp;</td>
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
                                        <%--</ContentTemplate>
                </atlas:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" TabIndex="30" />
                                        <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" TabIndex="31" />
                                        <input type="button" id="btnShowIPDates" value="Show IP Dates" class="ButtonCSS"
                                            onclick="javascript: return ShowIPDates();" />
                                        <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" TabIndex="32" />
                                    </td>
                                </tr>
                                <asp:HiddenField ID="Hid_RowCount" runat="server" />
                                <asp:HiddenField ID="Hid_MatDate" runat="server" />
                                <asp:HiddenField ID="Hid_CallDate" runat="server" />
                                <asp:HiddenField ID="Hid_CouponRate" runat="server" />
                                <asp:HiddenField ID="Hid_TypeFlag" runat="server" />
                                <asp:HiddenField ID="Hid_IPDate" runat="server" />
                                <asp:HiddenField ID="Hid_CustomerTypeId" runat="server" />
                                <asp:HiddenField ID="Hid_FreqOfInterest" runat="server" />
                                <asp:HiddenField ID="Hid_RatingDetailId" runat="server" />
                                <asp:HiddenField ID="Hid_ReCalculateIPDates" runat="server" Value="Y" />
                                <asp:HiddenField ID="Hid_SecurityCashFlow" runat="server" />
                                <asp:HiddenField ID="Hid_DocumentDetails" runat="server" />
                                <asp:HiddenField ID="Hid_SecurityDocumentId" runat="server" />
                                <asp:HiddenField ID="Hid_DocumentId" runat="server" />
                                <asp:HiddenField ID="Hid_DocumentMaster" runat="server" />
                                <asp:HiddenField ID="Hid_Id" runat="server" />
                                <asp:HiddenField ID="Hid_DelSecurityDoc" runat="server" />
                            </table>
                            <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
</asp:Content>
