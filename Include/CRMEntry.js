
function Show(e, StrHTML) {
    var div = document.getElementById("a_Tooltip");
    if (div.style.display == "none") div.style.display = "block";
    div.style.position = "absolute";
    var offset = $(e).offset();
    div.style.left = offset.left - 130 + "px";
    div.style.top = offset.top - 45 + "px";
    div.innerHTML = StrHTML
}
function Hide() {
    var div = document.getElementById("a_Tooltip");
    div.innerHTML = "";
    if (div.style.display == "block") div.style.display = "none";
}
function ShowDialogOpen(PageName, strWidth, strHeight) {
    var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
    var OpenUrl = PageName;
    var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
    return ret
}
function Calldata(BtnId) {

    var tblName, procName, postback, SelectedFieldName;
    procName = "CRM_FILL_CustomerMaster_CRMENTRY"
    tblName = "CustomerMaster"
    SelectedFieldName = "Name"
    postback = "false"
    Id = "CU"
    var pageUrl = "SearchCRMEntry.aspx";


    var FormWidth = 600 + "px";
    var FormHeight = 380 + "px";

    pageUrl = pageUrl + "?TableName=" + tblName + "&ProcName=" + procName + "&SelectedFieldName=" + SelectedFieldName;

    var DialogOptions = "left=50,top=50,height=400,width=700,toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes,resizable=yes, copyhistory=yes";

    var ret = window.showModalDialog(pageUrl, "", "dialogWidth:600px;dialogHeight:580px;");
    if (ret) {
        var arrRetValues;
        var selectedId;
        var selectedFieldIndex;
        var strTmp;
        var strCustType;
        
        arrRetValues = ret.split("!");
        selectedId = ret.substring(ret.lastIndexOf("!") + 1, ret.length);
        strTmp = new String();
        strTmp = arrRetValues[0];
        strCustType = arrRetValues[1];
        strTmp = strTmp.replace("&amp;", "&")
   
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_Id").value = selectedId;
    
        document.getElementById("ctl00_ContentPlaceHolder1_txt_CustName").value = strTmp

        var cboSuburb = document.getElementById('ctl00_ContentPlaceHolder1_cbo_InvContact');
        cboSuburb.options.length = 0;

        var Lst_InvCnct = document.getElementById('ctl00_ContentPlaceHolder1_lst_InvContact');
        Lst_InvCnct.options.length = 0;
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_LstbxPC").value = "";
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_LstbxTC").value = "";
        if (strCustType == "PF") {
            document.getElementById("ctl00_ContentPlaceHolder1_row_AdvComment").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_row_AdvStatus").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustBusinessType").value = strCustType;
            //document.getElementById("ctl00_ContentPlaceHolder1_txt_AdvComment").value = "";
            //document.getElementById("ctl00_ContentPlaceHolder1_txt_Advstatus").value = "";
        }
        else {
            document.getElementById("ctl00_ContentPlaceHolder1_row_AdvComment").style.display = "none";
            document.getElementById("ctl00_ContentPlaceHolder1_row_AdvStatus").style.display = "none";
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustBusinessType").value = strCustType;
            document.getElementById("ctl00_ContentPlaceHolder1_txt_AdvComment").value = "";
            document.getElementById("ctl00_ContentPlaceHolder1_txt_Advstatus").value = "";
        }
       var cnt=document.getElementById("ctl00_ContentPlaceHolder1_cbo_InvContact");

        $.ajax({
            type: "POST",
            url: "CRMEntry.aspx/BindContacts",
            data: '{"Id":"' + selectedId + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (result) {
                $(cnt).empty();
                $(cnt).append(result.d);
            },
            failure: function (result) {
                alert('Some error has occurred.');
            },
            error: function (result) {
          
                alert('error');
            }
        });
        
        //PageMethods.BindContacts(selectedId, Id, OnRequest, OnRequestError);
        //return false;
    }

}

function OnRequest(result) {
    if (result == "" || result == "0") {
    }
    else {
        fillData(result);
    }
}
function fillData(strdata) {
    var Rowdata = strdata.toString().split('|');
    var cboContact;
    var Ids;
    var Hid_Field, Hid_Field2;
    cboContact = document.getElementById('ctl00_ContentPlaceHolder1_cbo_InvContact');
    cboContact.options.length = 0;
    cboContact.options.add(new Option("", ""));
    for (no = 0; no <= (Rowdata.length - 1); no++) {
        var tempData = Rowdata[no].split('~');
        cboContact.options.add(new Option(tempData[1], tempData[0]));
    }
}
function OnRequestError(error) {
    if (error != null) {
        alert(error.get_message());
    }
}

function AddNewCnct(id) {

    // $("#div_CnctPerson").show("slide", { direction: "left" }, 500);
    var strcustId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Id").value;
    if (Number(strcustId) == 0) {
        alert('Please select customer first for new contact');
        return false;
    }
    else {
        $("#div_CnctPerson").toggle("slow");
        document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactPerson").value = "";
        document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactBranch").value = "";
        document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactDesign").value = "";
        document.getElementById("ctl00_ContentPlaceHolder1_txt_TelNo").value = "";
        document.getElementById("ctl00_ContentPlaceHolder1_txt_MobileNo").value = "";
        document.getElementById("ctl00_ContentPlaceHolder1_txt_Email").value = "";
        return false;
    }    
}
function CancelNewCnct() {
    //document.getElementById("ctl00_ContentPlaceHolder1_Btn_AddnewContact").style.display = "";
    //$("#div_CnctPerson").hide("slide", { direction: "left" }, 500);
    $("#div_CnctPerson").toggle("slow");
    return false;
}


function SaveNewCnct() {

    var RightDDL = document.getElementById("ctl00_ContentPlaceHolder1_lst_InvContact");
    var StrNewCnct, StrNewText;
    if (document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactPerson").value == "") {
        alert('Please enter contact name');
        return false;
    }
    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_Interaction").value == "") {
        alert('Please select interaction');
        return false;
    }
    var RetValue = EmailValidate()
    if (RetValue == false) {
        return false;
    }
    StrNewCnct = document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactPerson").value;
    StrNewCnct = StrNewCnct + "!" + document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactBranch").value;
    StrNewCnct = StrNewCnct + "!" + document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactDesign").value;
    StrNewCnct = StrNewCnct + "!" + document.getElementById("ctl00_ContentPlaceHolder1_txt_TelNo").value;
    StrNewCnct = StrNewCnct + "!" + document.getElementById("ctl00_ContentPlaceHolder1_txt_MobileNo").value;
    StrNewCnct = StrNewCnct + "!" + document.getElementById("ctl00_ContentPlaceHolder1_txt_Email").value;
    //StrNewCnct = StrNewCnct + "!" + document.getElementById("ctl00_ContentPlaceHolder1_cbo_Interaction").value;
    var DDL = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Interaction");
    if (DDL.options.length > 0) {
        for (i = 0; i <= DDL.options.length - 1; i++) {
            if (DDL.options[i].selected == true && DDL.options[i].text != "") {
                StrNewText = StrNewCnct + "!" + DDL.options[i].text;
                StrNewCnct = StrNewCnct + "!" + DDL.options[i].value;
            }

        }
    }
    RightDDL.options.add(new Option(StrNewText, 0));
    RightDDL.options[RightDDL.options.length - 1].title = StrNewText;
    // $("#div_CnctPerson").hide("slide", { direction: "left" }, 500);
    $("#div_CnctPerson").toggle("slow");

    return false;
}


function SavePermanentContact() {
    var strcustId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Id").value;
    var strId = "CU";
    var strCont = "";
    var strBranch = "";
    var strdesig = "";
    var strtel = "";
    var strmob = "";
    var stremail = "";
    var strExpDate = "";
    var strinteractionId = "";
    
    if (document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactPerson").value == "") {
        alert('Please enter contact name');
        return false;
    }
    
    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_Interaction").value == "") {
        alert('Please select interaction');
        return false;
    }
    
    var RetValue = EmailValidate()
    if (RetValue == false) {
        return false;
    }
    strCont = document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactPerson").value;
    
    strBranch = document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactBranch").value;
    
    strdesig = document.getElementById("ctl00_ContentPlaceHolder1_txt_ContactDesign").value;
    
    strtel = document.getElementById("ctl00_ContentPlaceHolder1_txt_TelNo").value;
    
    strmob = document.getElementById("ctl00_ContentPlaceHolder1_txt_MobileNo").value;
    
    stremail = document.getElementById("ctl00_ContentPlaceHolder1_txt_Email").value;
    
    strExpDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_EntryDate").value;
    
    strinteractionId = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Interaction").value;
    $("#div_CnctPerson").toggle("slow");
    PageMethods.Save_BindContacts(strcustId, strId, strCont, strBranch, strdesig, strtel, strmob, stremail, strinteractionId, strExpDate, OnContRequest, OnContRequestError);

    return false;
}



function OnContRequest(result) {
    if (result == "" || result == "0") {
    }
    else {
        fillData(result);
    }
}
function fillData(strdata) {
    var Rowdata = strdata.toString().split('|');
    var cboContact;
    var Ids;
    var Hid_Field, Hid_Field2;
    cboContact = document.getElementById('ctl00_ContentPlaceHolder1_cbo_InvContact');
    cboContact.options.length = 0;
    cboContact.options.add(new Option("", ""));
    for (no = 0; no <= (Rowdata.length - 1); no++) {
        var tempData = Rowdata[no].split('~');
        cboContact.options.add(new Option(tempData[1], tempData[0]));
    }
}
function OnContRequestError(error) {
    if (error != null) {
        alert(error.get_message());
    }
}


function FillList(LeftDDL, RightDDL) {
    for (k = 0; k <= LeftDDL.options.length - 1; k++) {
        if (LeftDDL.options[k].selected == true && LeftDDL.options[k].text != "") {
            var cnt;
            var selectedText = LeftDDL.options[k].text;
            var selectedVal = LeftDDL.options[k].value;
            var bFound = false;
            if (RightDDL.options.length > 0) {
                for (i = 0; i <= RightDDL.options.length - 1; i++) {
                    if (selectedText.toUpperCase() == RightDDL.options[i].text.toUpperCase()) {
                        bFound = true;
                        LeftDDL.selectedIndex = -1;
                        return false;
                    }
                }
            }
            if (bFound == false) {
                RightDDL.options.add(new Option(selectedText, selectedVal));
                LeftDDL.remove(k);
                return false;
            }
        }
    }
}



function RemoveList(ListBx, ComboBx) {
    for (k = 0; k <= ListBx.options.length - 1; k++) {
        if (ListBx.options[k].selected == true && ListBx.options[k].text != "") {
            var selectedText = ListBx.options[k].text;
            var selectedVal = ListBx.options[k].value;
            var bFound = false;
            var count = selectedText.match(/,/g);
            if (count != null) {
                var cnt;
                selectedText = selectedText.substring(0, selectedText.lastIndexOf(",") + 2);
                ListBx.remove(k);
                return false;

            }
            if (ComboBx.options.length > 0) {
                for (i = 0; i <= ComboBx.options.length - 1; i++) {
                    if (selectedText.toUpperCase() == ComboBx.options[i].text.toUpperCase()) {
                        //                                    if(selectedVal == 0)
                        //                                    {                             
                        bFound = true;
                        ListBx.remove(k);
                        ListBx.selectedIndex = -1;
                        return false;
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        alert('Can not delete from here, it is a permanent contact person');
                        //                                        return false;
                        //                                    }
                    }
                }
                if (bFound == false) {
                    ComboBx.options.add(new Option(selectedText, selectedVal));
                    //                                 if(selectedVal == 0)
                    //                                 {
                    ListBx.remove(k);
                    ListBx.selectedIndex = -1;
                    return false;
                    //                                 }    
                    //                                 else
                    //                                 {
                    //                                    alert('Can not delete from here, it is a permanent contact person');
                    //                                    return false;
                    //                                 }
                }
            }
        }
    }
}
function InsertInvContact1() {
    var LeftDDL = document.getElementById("ctl00_ContentPlaceHolder1_cbo_InvContact");
    var RightDDL = document.getElementById("ctl00_ContentPlaceHolder1_lst_InvContact");
    var ret = FillList(LeftDDL, RightDDL);
    return false;

}

function RemoveItemFromList1() {
    var Listbx = document.getElementById("ctl00_ContentPlaceHolder1_lst_InvContact");
    var Combobx = document.getElementById("ctl00_ContentPlaceHolder1_cbo_InvContact");
    var ret = RemoveList(Listbx, Combobx);
    return false;

}



function Selection() {
//    document.getElementById("ctl00_ContentPlaceHolder1_txt_IssName").value = "";
    document.getElementById("ctl00_ContentPlaceHolder1_txt_CustName").value = "";
    document.getElementById("ctl00_ContentPlaceHolder1_Hid_Id").value = "";
    document.getElementById("ctl00_ContentPlaceHolder1_Hid_LstbxPC").value = "";
    document.getElementById("ctl00_ContentPlaceHolder1_Hid_LstbxTC").value = "";

    var lstInvContact = document.getElementById("ctl00_ContentPlaceHolder1_lst_InvContact");
    lstInvContact.options.length = 0;
    var cboContact = document.getElementById("ctl00_ContentPlaceHolder1_cbo_InvContact");
    cboContact.options.length = 0;
    var SelectionId;
    document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelectionId").value = "CU";
    SelectionId = "CU";
    
        //document.getElementById("ctl00_ContentPlaceHolder1_row_Customer").style.display = "";
        //document.getElementById("ctl00_ContentPlaceHolder1_row_Issuer").style.display = "none";
        //document.getElementById("ctl00_ContentPlaceHolder1_tr_InvContact").style.display = "";
        //document.getElementById("ctl00_ContentPlaceHolder1_btn_AddInvContact").style.display = "";
        //document.getElementById("ctl00_ContentPlaceHolder1_Btn_AddnewContact").style.display = "";
        document.getElementById("ctl00_ContentPlaceHolder1_lbl_Interaction").innerHTML = "CUSTOMER";
    
    return false;

}

function Validation() {
    if (document.getElementById("ctl00_ContentPlaceHolder1_txt_CustName").value == "") {
        alert('Please enter customer name');
        return false;
    }
    
    var obj = document.getElementById("ctl00_ContentPlaceHolder1_txt_EntryDate");
    if (myTrim(obj.value) == "" || ValidateDate(obj) == false) {
        alert('Please select correct meeting date (i-e dd/mm/yyyy).');
        return false;
    }
    
    if (document.getElementById("ctl00_ContentPlaceHolder1_txt_EntryDate").value != "") {
        var Seldate = document.getElementById("ctl00_ContentPlaceHolder1_txt_EntryDate").value;
        var d = new Date();
        var date1 = d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();

        var startDate = getDateObject(Seldate, "/");
        var endDate = getDateObject(date1, "/");
        if (startDate > endDate) {
            alert('Date cannot be greater than current date');
            return false;
        }
    }

    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_City").value == "0") {
        alert('Please select city name');
        return false;
    }
    
    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_Purpose").value == "0") {
        alert('Please select purpose');
        return false;
    }
    
    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_InvContact").value == "0") {
        alert('Please select contact person');
        return false;
    }
    else{
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_InvContact").value =  document.getElementById("ctl00_ContentPlaceHolder1_cbo_InvContact").value;
    }
    
    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfCnct").value == "0") {
        alert('Please Select mode of contact');
        return false;
    }

    if (document.getElementById("ctl00_ContentPlaceHolder1_txt_Remark").value == "") {
        alert('Please enter summary of discussion');
        return false;
    }

    var LeftVertical = document.getElementById("ctl00_ContentPlaceHolder1_Lst_VerticalR");
    var str = "";
    for (k = 0; k <= LeftVertical.options.length - 1; k++) {
        var cnt;
        LeftVertical.options[k].selected = false;
        var selectedText = LeftVertical.options[k].text;
        var selectedVal = LeftVertical.options[k].value;
        str = str + selectedVal + ",";
    }
    str = str.substring(0, str.length - 1);
//    if (str == "") {
//        alert('Please Select Vertical Purpose');
//        return false;
//    }
//    else {
//        document.getElementById("ctl00_ContentPlaceHolder1_Hid_LstVerticalR").value = str;
//    }

    var LeftDDL = document.getElementById("ctl00_ContentPlaceHolder1_Lst_CustExpecR");
    var str = "";
    for (k = 0; k <= LeftDDL.options.length - 1; k++) {
        var cnt;
        LeftDDL.options[k].selected = false;
        var selectedText = LeftDDL.options[k].text;
        var selectedVal = LeftDDL.options[k].value;
        str = str + selectedVal + ",";
    }
    str = str.substring(0, str.length - 1);
    document.getElementById("ctl00_ContentPlaceHolder1_Hid_LstCustExpecR").value = str;

    var LeftDDL1 = document.getElementById("ctl00_ContentPlaceHolder1_Lst_AccompaniedbyR");
    var str1 = "";
    for (k = 0; k <= LeftDDL1.options.length - 1; k++) {
        var cnt;
        LeftDDL1.options[k].selected = false;
        var selectedText = LeftDDL1.options[k].text;
        var selectedVal = LeftDDL1.options[k].value;
        str1 = str1 + selectedVal + ",";
    }
    str1 = str1.substring(0, str1.length - 1);
    document.getElementById("ctl00_ContentPlaceHolder1_Hid_LstAccompaniedbyR").value = str1;
    //SaveContacts()
}

function SaveContacts() {

    var lstInvContact = document.getElementById("ctl00_ContentPlaceHolder1_lst_InvContact");
    var Hid_FieldTC, Hid_FieldPC;



    Hid_FieldPC = document.getElementById('ctl00_ContentPlaceHolder1_Hid_LstbxPC');
    Hid_FieldTC = document.getElementById('ctl00_ContentPlaceHolder1_Hid_LstbxTC');


    if (lstInvContact.options.length > 0) {
        for (i = 0; i <= lstInvContact.options.length - 1; i++) {
            //var selectedText= lstInvContact.options[i].text;                                            
            var selectedText = lstInvContact.options[i].title;
            var selectedVal = lstInvContact.options[i].value;

            var bFound = false;
            var count = selectedText.match(/!/g);


            selectedText.substring(0, selectedText.lastIndexOf("!") + 2);

            if (count != null) {
                //Hid_FieldTC.value = Hid_FieldTC.value + "0" + "!" + selectedText.substring(0, selectedText.lastIndexOf("!") + 2) + "~";
                Hid_FieldTC.value = Hid_FieldTC.value + "0" + "!" + selectedText + "~";
            }
            else {
                Hid_FieldPC.value = Hid_FieldPC.value + lstInvContact.options[i].value + "!" + lstInvContact.options[i].text + "~";
            }
            document.getElementById('ctl00_ContentPlaceHolder1_Hid_LstbxPC').value = Hid_FieldPC.value;
            document.getElementById('ctl00_ContentPlaceHolder1_Hid_LstbxTC').value = Hid_FieldTC.value;
        }
    }
}

function getDateObject(dateString, dateSeperator) {
    //This function return a date object after accepting 
    //a date string ans dateseparator as arguments
    var curValue = dateString;
    var sepChar = dateSeperator;
    var curPos = 0;
    var cDate, cMonth, cYear;

    //extract day portion
    curPos = dateString.indexOf(sepChar);
    cDate = dateString.substring(0, curPos);

    //extract month portion	
    endPos = dateString.indexOf(sepChar, curPos + 1);
    cMonth = dateString.substring(curPos + 1, endPos);

    //extract year portion	
    curPos = endPos;
    endPos = curPos + 5;
    cYear = curValue.substring(curPos + 1, endPos);

    //Create Date Object
    dtObject = new Date(cYear, cMonth, cDate);
    return dtObject;
}