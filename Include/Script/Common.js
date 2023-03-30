var dtCh = "/";
var minYear = 1950;
var maxYear = 2150;
var intVal;
var flag = 0;

function OnlyDecimal() {
    var KeyId = event.keyCode;
    if (((KeyId >= 48 && KeyId <= 57) || KeyId == 46) == false) {
        event.keyCode = 0;
    }
}

function OnlyInteger() {
   
    var KeyId = event.keyCode;
    if ((KeyId >= 48 && KeyId <= 57) == false) {
        event.keyCode = 0;
    }
}

function OnlyNumeric() {
    if ((event.keyCode < 48 || event.keyCode > 57)) {
        event.returnValue = false;
    }
}

function getSQLSearchSting(jsonData) {
    return "";
}

function PopupCenter(pageURL, title, w, h) {

    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    var targetWin = window.open(pageURL, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=yes, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
}

//to get current date in dd/MM/yyyy format
function getCurrDate() {

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) { dd = '0' + dd };
    if (mm < 10) { mm = '0' + mm };
    today = mm + '/' + dd + '/' + yyyy;
    //today = dd + '/' + mm + '/' + yyyy; 
    return today;
}

function ConvertUCase(txtBox) {
    txtBox.value = txtBox.value.toUpperCase()
}

//to get date in dd/MM/yyyy format
function getDate(opt) {

    var today = new Date();
    today.setDate(today.getDate() + opt);

    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!  
    var yyyy = today.getFullYear();

    if (dd < 10) { dd = '0' + dd };
    if (mm < 10) { mm = '0' + mm };
    today = mm + '/' + dd + '/' + yyyy;
    //today = dd + '/' + mm + '/' + yyyy; 
    return today;
}

//to get current date in yyyy,MM,dd format
function getCurrDateyyyyMMdd() {

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) { dd = '0' + dd };
    if (mm < 10) { mm = '0' + mm };

    today = yyyy + "," + mm + ',' + dd;
    //today = dd + '/' + mm + '/' + yyyy; 
    return today;

}

function ShowDialogOpen(PageName, strWidth, strHeight) {
    var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=Yes;";
    var OpenUrl = PageName;


    var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
    return ret;
}

function CheckDate(dt, blnFlag) {

    if (dt.value == "" && blnFlag == false) return true

    intVal = dt.value.split(dtCh);
    var len = intVal.length;
    for (i = 0; i < len; i++) {
        if (intVal[i] == "") {
            alert("Invalid Date, Please enter in dd/MM/yyyy");
            dt.focus();
            return false;
        }
    }
    if (len <= 1 || len > 3) {
        alert("Invalid Date, Please enter in dd/MM/yyyy");
        dt.focus();
        return false;
    }
    else if (len == 2) {
        for (i = 0; i < intVal.length; i++) {
            if (intVal[i].length > 2) {
                alert("Invalid Date, Please enter in dd/MM/yyyy");
                dt.focus();
                return false;
            }
        }
        FormatDate();
        var Curr = new Date();
        dt.value = intVal[0] + "/" + intVal[1] + "/" + Curr.getFullYear();
    }
    else if (len == 3) {
        FormatDate();
        if (intVal[2].length == 2 || intVal[2].length == 4) {
            if (intVal[2].length == 2) {
                intVal[2] = "20" + intVal[2];
            }
            dt.value = intVal[0] + "/" + intVal[1] + "/" + intVal[2];
        }
        else {
            alert("Invalid Year, Please enter in dd/MM/yyyy");
            dt.focus();
            return false;
        }
    }
    if (isDate(dt.value) == false) {
        dt.focus();
        return false;
    }
    return true;
}

function FormatDate() {
    for (i = 0; i < intVal.length; i++) {
        if (intVal[i].length == 1) {
            intVal[i] = "0" + intVal[i];
        }
    }
}

function isDate(dtStr) {
    var daysInMonth = DaysArray(12);
    var pos1 = dtStr.indexOf(dtCh);
    var pos2 = dtStr.indexOf(dtCh, pos1 + 1);
    var strDay = dtStr.substring(0, pos1);
    var strMonth = dtStr.substring(pos1 + 1, pos2);
    var strYear = dtStr.substring(pos2 + 1);

    strYr = strYear;
    if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
    if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
    for (var i = 1; i <= 3; i++) {
        if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
    }
    month = parseInt(strMonth);
    day = parseInt(strDay);
    year = parseInt(strYr);

    if (strMonth.length < 1 || month < 1 || month > 12) {
        alert("Invalid Month, Please enter in dd/MM/yyyy");
        return false;
    }

    if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
        alert("Invalid Day, Please enter in dd/MM/yyyy");
        return false;
    }

    if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
        alert("Please enter a valid year between " + minYear + " and " + maxYear);
        return false;
    }

    return true;
}

function daysInFebruary(year) {
    // February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
}

function DaysArray(n) {
    for (var i = 1; i <= n; i++) {
        this[i] = 31
        if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
        if (i == 2) { this[i] = 29 }
    }
    return this
}

//string left function
String.prototype.left = function (n) {
    return this.substring(0, n);
}

//function to get date in mm/dd/yy format from date 
//which is passed as a string in dd/mm/yy format
function getmdy(txt) {
    if ((txt == "") || (txt == null)) return;
    var datePat = /^(\d{1,2})(\/|-)(\d{1,2})\2(\d{2}|\d{4})$/

    var matchArray = txt.match(datePat); // is the format ok?
    month = matchArray[3]; // parse date into variables
    day = matchArray[1];
    year = matchArray[4];
    mmddyy = month + "/" + day + "/" + year;
    return mmddyy;
}

function OnlyDate() {

    var KeyId = event.keyCode;
    if ((KeyId >= 47 && KeyId <= 57) == false) {
        event.keyCode = 0;
    }
}

function Trim(sInString) {
    sInString = sInString.replace(/^\s+/g, "");// strip leading
    return sInString.replace(/\s+$/g, "");// strip trailing
}

function ValidateDate(DateField) {
    var InputValue = trim(DateField.value);
    var validformat = /^\d{2}\/\d{2}\/\d{4}$/   //Basic check for format validity
    var returnval = false;

    if (!validformat.test(InputValue)) {
        //alert("Invalid Date Format, Enter Correct Date Format (i-e dd/mm/yyyy).");
        returnval = false;
    }
    else {
        //Detailed check for valid date ranges
        var monthfield = InputValue.split("/")[1];
        var dayfield = InputValue.split("/")[0];
        var yearfield = InputValue.split("/")[2];
        var dayobj = new Date(yearfield, monthfield - 1, dayfield);

        if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield)) {
            //alert("Invalid date format,enter correct date format (i-e dd/mm/yyyy).");
            returnval = false;
        }
        else {
            returnval = true;
        }
    }

    if (returnval == false) {
        return false;
    }
}

function trim(value) {
    value = value.replace(/^\s+/, '');
    value = value.replace(/\s+$/, '');
    return value;
}

function OnlyNumericKey(event) {
    var keycode = event.which || event.keyCode;
    if (keycode == 8 || keycode == 46 || (keycode >= 48 && keycode <= 57)) {
        return true;
    }
    else {
        return false;
    }
}

function OnlyStringKey(event) {
    var keycode = event.which || event.keyCode;
    if (keycode == 34 || keycode == 39) {
        return false;
    }
}

function OnlyDateKey(event) {
    var keycode = event.which || event.keyCode;
    if (keycode == 8 || (keycode >= 47 && keycode <= 57)) {
        return true;
    }
    else {
        return false;
    }
}

function readonly(event) {
    return false;
}

function Cancel(strPage) {
    window.location = strPage;
    return false;
}

function datePick(element) {
    jQuery(element).datepicker({
        dateFormat: 'dd/mm/yy'
    });
}

function validateNumericField(value, colname) {
    if (value.trim() != '') {
        if (isNaN(value.trim()) == true) {
            return [false, " Please enter only numeric value for " + colname + "."];
        }
        else {
            return [true, ""];
        }
    }
    else {
        return [true, ""];
    }
    return [false, " must be zero a positive integer."];
}

function ValidateNumberField(value) {
    if (value.trim() == "" || value.trim() == 0 || isNaN(value.trim()))
        return false;
    else
        return true;
}

function validateDateField(value, colname) {
    if (value.trim() != '') {
        var InputValue = trim(value);
        var validformat = /^\d{2}\/\d{2}\/\d{4}$/   //Basic check for format validity
        var returnval = false;

        if (!validformat.test(InputValue)) {
            returnval = false;
        }
        else {
            //Detailed check for valid date ranges
            var monthfield = InputValue.split("/")[1];
            var dayfield = InputValue.split("/")[0];
            var yearfield = InputValue.split("/")[2];
            var dayobj = new Date(yearfield, monthfield - 1, dayfield);

            if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield)) {
                returnval = false;
            }
            else {
                returnval = true;
            }
        }

        if (returnval == false) {
            return [false, " Please select proper value for " + colname + " i-e (dd/mm/yyyy)."];
        }
        else {
            return [true, ""];
        }
    }
    else {
        return [true, ""];
    }
}

function AlertMessage(title, message, hieght, width, type) {
    var strcolor;

    switch (type) {
        case 'S':
            strcolor = "style='color:green;'";
            break;
        case 'D':
        case 'E':
            strcolor = "style='color:red;'";
            break;
        default:
            strcolor = "style='color:black;'";
            break;
    }

    var str = "<div title=" + title + " style='padding:5px;'><p " + strcolor + ">" + message + "</p></div>";

    $(str).dialog({
        resizable: false,
        modal: true,
        height: hieght,
        width: width,
        buttons: {
            "Ok": function () { $(this).dialog("close"); }
        }
    });

    return false;
}

function GetJsonCompatibleString(str) {
    var regex = /\\/g;
    str = str.replace(regex, "\\\\");
    str = str.replace(new RegExp('"', "g"), '\\"');
    str = str.replace(new RegExp("'", "g"), "\\'");

    return str;
}



function myTrim(str) {
    return str.replace(/^\s+|\s+$/gm, '');
}

function ConfirmDialog(title, message, hieght, width, OkFunction, CancelFunction) {
    var str = "<div style='padding:5px;'><p>" + message + "</p></div>";

    $(str).dialog({
        resizable: false,
        modal: true,
        height: hieght,
        width: width,
        title: title,
        buttons: {
            "Yes": function () {
                $(this).dialog("close");
                if (OkFunction != null && OkFunction != undefined && OkFunction != "") {
                    OkFunction();
                }
            },
            "No": function () {
                $(this).dialog("close");
                if (CancelFunction != null && CancelFunction != undefined && CancelFunction != "") {
                    CancelFunction();
                }
            }
        }
    });
    return false;
}

function DeleteRecord(cnt, pagename, id) {
    ConfirmDialog(
    'Confirmation',
    'Are you sure you want to delete this record?',
    175,
    450,
    function () {
        $.ajax({
            url: "getdata.ashx?pagename=" + pagename + "&oper=delete&id=" + id,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (result) {
                AlertMessage('Message', result, 175, 450);
                $(cnt).trigger("reloadGrid");
            },
            failure: function (result) {
                alert(result);
            }
        });
    },
    ''
    );
}

function DeleteRecordNew(cnt, pagename, id) {

    ConfirmDialog(
    'Confirmation',
    'Are you sure you want to delete this record?',
    175,
    450,
    function () {
        $.ajax({
            url: "getdatamanoj.ashx?pagename=" + pagename + "&oper=delete&id=" + id,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (result) {
                AlertMessage('Message', result, 175, 450);
                $(cnt).trigger("reloadGrid");
            },
            failure: function (result) {
                alert(result);
            }
        });
    },
    ''
    );
}

function ValidatePAN(value) {
    var ret = true;
    if (value != "") {
        value = value.toUpperCase();
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = value.substring(3, 4);

        if (value.search(panPat) == -1) {
            ret = false;
        }

        if (code.test(code_chk) == false) {
            ret = false;
        }
    }
    return ret;
}

function GetCellValue(cnt, type) {
    if (flag == 0) {
        strData = cnt.innerText;
        if (type == 'n')
            cnt.innerHTML = "<input type='text' style='padding:2px; border:0px; width:97%; background-color:#F7FFE4;' onblur=SetCellValue(this) onkeypress='javascript:return OnlyNumericKey(event);' maxlength='20' />";
        else
            cnt.innerHTML = "<input type='text' style='padding:2px; border:0px; width:97%; background-color:#F7FFE4;' onblur=SetCellValue(this) maxlength='20' />";
        $(cnt).find("input").get(0).focus();
        $(cnt).find("input").get(0).value = strData.trim();
        flag = 1;
    }
}

function SetCellValue(cnt) {
    if (flag == 1) {
        $(cnt).closest("td").text(cnt.value);
        flag = 0;
    }
}

function UpdateCheckerStatus(id, checkstatus, remark, tablename, primarykey) {
    var dlg = "<div style='padding:5px;'>";
    dlg = dlg + "<p>Fields marked with <i style='color: red;'>*</i> are required.</p>";
    dlg = dlg + "<table cellpadding='0' cellspacing='0' border='0' class='data_table'>";
    dlg = dlg + "<tr align='left'>";
    dlg = dlg + "<td>Select Status:</td>";
    dlg = dlg + "<td><select id='cboCheckStatus' class='combo'><option value='A'>Accept</option><option value='R'>Reject</option></select><i style='color: red; vertical-align: super;'>*</i></td>";
    dlg = dlg + "</tr>";
    dlg = dlg + "<tr align='left'>";
    dlg = dlg + "<td>Remarks (if any):</td>";
    //dlg = dlg + "<td><input id='txtCheckerRemarks' type='text' class='text_box' /></td>";
    dlg = dlg + "<td><textarea id='txtCheckerRemarks' rows='4' cols='30' class='text_box'></textarea></td>";
    dlg = dlg + "</tr>";
    dlg = dlg + "</table>";
    dlg = dlg + "</div>";

    $(dlg).dialog({
        autoOpen: true,
        height: 300,
        width: 450,
        modal: true,
        buttons: {
            "Update": function () {
                checkstatus = $(this).find("#cboCheckStatus").val();
                remark = $(this).find("#txtCheckerRemarks").val();
                $.ajax({
                    type: "POST",
                    url: "getdata.aspx/UpdateCheckerStatus",
                    data: "{id:'" + id + "',checkstatus:'" + checkstatus + "',remark:'" + remark + "',tablename:'" + tablename + "',primarykey:'" + primarykey + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var value = JSON.parse(result.d)
                        if (value.type == "S")
                            window.location = "checkerview.aspx";
                    },
                    failure: function (result) {
                        alert('Some error has occurred.');
                    }
                });
                $(this).dialog("close");
            },
            "Cancel": function () { $(this).dialog("close"); }
        }
    });
}

function RequiredValiation(form) {
    var value = '';
    var ret = true;
    $(form).find(".required").css({ "border": "1px solid rgba(251,179,18,0.76)", "background-color": "#ffffff" });
    $(form).find(".required").each(function (i, item) {
        if ($(this).val())
            value = $(this).val().trim();
        else
            value = '';

        if ($(this).get(0).tagName.toUpperCase() == "INPUT" && $(this).get(0).type.toUpperCase() == "TEXT" && (value == "" || (value != "" && $(this).hasClass("jsdate") && ValidateDate($(this).get(0)) == false))) {
            $(this).css({ "border": "1px solid #d9534f", "background-color": "#fff5f5" });
            if (ret)
                $(this).focus();
            ret = false;
        }
        else if ($(this).get(0).tagName.toUpperCase() == "SELECT" && $(this).get(0).type.toUpperCase() == "SELECT-ONE" && (value == "" || value == "0")) {
            $(this).css({ "border": "1px solid #d9534f", "background-color": "#fff5f5" });
            if (ret)
                $(this).focus();
            ret = false;
        }
    });
    return ret;
}

function CheckDuplicateValue(id, value, type) {
    var ret = 0;
    $.ajax({
        type: "POST",
        url: "getdata.aspx/CheckDuplicate",
        data: "{id:'" + id + "',value:'" + value + "',type:'AN'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            ret = result.d;
        },
        failure: function (result) {
            alert('Some error has occurred.');
        }
    });
    return ret;
}

function Validate_Date(DateValue) {
    var InputValue = trim(DateValue);
    var validformat = /^\d{2}\/\d{2}\/\d{4}$/;
    var returnval = false;

    if (!validformat.test(InputValue)) {
        returnval = false;
    }
    else {
        var monthfield = InputValue.split("/")[1];
        var dayfield = InputValue.split("/")[0];
        var yearfield = InputValue.split("/")[2];
        var dayobj = new Date(yearfield, monthfield - 1, dayfield);

        if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield)) {
            returnval = false;
        }
        else {
            returnval = true;
        }
    }

    return returnval;
}

function Validate_Number(NumberValue) {
    var InputValue = trim(NumberValue);
    if (InputValue == "" || isNaN(InputValue))
        return false;
    else
        return true;
}

function calculate_gst(type, sgst, cgst, igst, amount) {
    var sgst_amt = 0, cgst_amt = 0, igst_amt = 0, final_amt = 0;
    try {
        if (type == 'E' && amount > 0) {
            sgst_amt = amount * sgst / 100;
            cgst_amt = amount * cgst / 100;
            igst_amt = amount * igst / 100;
            final_amt = amount + (sgst_amt + cgst_amt);
        }
        else if (type == 'I' && amount > 0) {
            sgst_amt = amount * 100 / 118.0 * sgst / 100;
            cgst_amt = amount * 100 / 118.0 * cgst / 100;
            igst_amt = amount * 100 / 118.0 * igst / 100;
            final_amt = amount;
        }
        return { "sgst": sgst_amt, "cgst": cgst_amt, "igst": igst_amt, "amount": final_amt };
    } catch (err) {
        alert('Error');
        return { "sgst": 0, "cgst": 0, "igst": 0, "amount": 0 };
    }
}

function GetValidUserInput(element) {
    // alert($(element).val());
    var str = $(element).val().trim();
    //str = str.replace(new RegExp("'", "g"), "");
    //str = str.replace(new RegExp("*", "g"), "");
    //str = str.replace(new RegExp("-", "g"), "");
    //str = str.replace(new RegExp("<", "g"), "");
    //str = str.replace(new RegExp(">", "g"), "");
    //str = str.replace(new RegExp("`", "g"), "");
    //str = str.replace(new RegExp("~", "g"), "");

    str = str.replace(/\/\*/gi, "");
    str = str.replace(/\*\//gi, "");

    //str = str.replace(/\\/gi, "");
    //str = str.replace(new RegExp("['*-<>`~]", "g"), "");
    str = str.replace(new RegExp("['*<>`~]", "g"), "");
    //str = str.replace(new RegExp("['`~]", "g"), "");
    str = str.replace(new RegExp("--", "gi"), "");
    str = str.replace(new RegExp("\\\\", "gi"), "");
    str = str.replace(new RegExp("//", "gi"), "");
    //str = str.replace(new RegExp("//*", "gi"), "");
    //str = str.replace(new RegExp("*//", "gi"), "");

    str = str.replace(new RegExp("Delete", "gi"), "");
    str = str.replace(new RegExp("Truncate", "gi"), "");
    str = str.replace(new RegExp("Drop", "gi"), "");
    str = str.replace(new RegExp("Select ", "gi"), "");
    str = str.replace(new RegExp("Update ", "gi"), "");

    //alert(str);
    $(element).val(str.trim());
}

function GetValidInput(strValue) {
    //alert(strValue);
    var str = strValue.trim();

    str = str.replace(/\/\*/gi, "");
    str = str.replace(/\*\//gi, "");

    //str = str.replace(/\\/gi, "");
    //str = str.replace(new RegExp("['*-<>`~]", "g"), "");
    str = str.replace(new RegExp("['*<>`~]", "g"), "");
    //str = str.replace(new RegExp("['`~]", "g"), "");
    str = str.replace(new RegExp("--", "gi"), "");
    str = str.replace(new RegExp("\\\\", "gi"), "");
    str = str.replace(new RegExp("//", "gi"), "");
    //str = str.replace(new RegExp("//*", "gi"), "");
    //str = str.replace(new RegExp("*//", "gi"), "");

    str = str.replace(new RegExp("Delete", "gi"), "");
    str = str.replace(new RegExp("Truncate", "gi"), "");
    str = str.replace(new RegExp("Drop", "gi"), "");
    str = str.replace(new RegExp("Select ", "gi"), "");
    str = str.replace(new RegExp("Update ", "gi"), "");

    //alert(str);
    return str;
}


function blockSpecialChar(e) {
    var KeyId = e.keyCode;
    return ((KeyId > 64 && KeyId < 91) || (KeyId > 96 && KeyId < 123) || KeyId == 8 || KeyId == 32 || (KeyId >= 48 && KeyId <= 57));
}

function OnlyChar(e) {

    var KeyId = e.keyCode;
    return ((KeyId > 96 && KeyId < 123) || (KeyId > 64 && KeyId < 91));
    //if ((KeyId >= 48 && KeyId <= 57) == true) {
    //    event.keyCode = 0;
    //}
    
    //return ((KeyId > 96 && KeyId < 123) || KeyId == 32 || KeyId > 31 && (KeyId < 48 || KeyId > 57) || (KeyId > 64 && KeyId < 91));
}

function validateEmail(emailField) {

    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

    if (reg.test(emailField.value) == false) {
        AlertMessage('Validation', 'Invalid Email Address', 175, 450);
        document.getElementById("ctl00_ContentPlaceHolder1_txtEmail").value = "";
        return false;
    }
    return true;
}