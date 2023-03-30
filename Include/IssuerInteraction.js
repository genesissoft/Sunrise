//*******Add item to right side list box*******

function FillListBox(LList, RList) {

    var LeftDDL = document.getElementById("ctl00_ContentPlaceHolder1_" + LList);
    var RightDDL = document.getElementById("ctl00_ContentPlaceHolder1_" + RList);

    for (k = 0; k <= LeftDDL.options.length - 1; k++) {
        if (LeftDDL.options[k].selected == true) {
            var cnt;
            var selectedText = LeftDDL.options[k].text;
            var selectedVal = LeftDDL.options[k].value;

            var bFound = false;

            if (RightDDL.options.length > 0) {

                for (i = 0; i <= RightDDL.options.length - 1; i++) {
                    if (selectedText.toUpperCase() == RightDDL.options[i].text.toUpperCase()) {
                        bFound = true;

                    }
                }
            }

            if (bFound == false) {
                RightDDL.options.add(new Option(selectedText, selectedVal));
            }
        }
    }

    LeftDDL.selectedIndex = -1;
}

//******************

//*******Remove items from right side list box*******

function RemoveListBox(RList) {
    var RightDDL = document.getElementById("ctl00_ContentPlaceHolder1_" + RList);
    for (k = RightDDL.options.length - 1; k >= 0; k--) {
        if (RightDDL.options[k].selected == true) {
            RightDDL.remove(k);
        }
    }
}

//******************

//*******Search for issuer name*******
function GetData(strType) {

    if (strType == 'Issuer') {
        var tblName, procName, postback, SelectedFieldName;
        procName = "Fill_Search_IssuerName"
        tblName = "IssuerMaster"
        SelectedFieldName = "Name"
        postback = "false"
        Id = "IS"
        var pageUrl = "Search.aspx";
    }

    var FormWidth = 600 + "px";
    var FormHeight = 380 + "px";

    pageUrl = pageUrl + "?TableName=" + tblName + "&ProcName=" + procName + "&SelectedFieldName=" + SelectedFieldName;

    var DialogOptions = "left=50,top=50,height=400,width=700,toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes,resizable=yes, copyhistory=yes";

    var ret = window.showModalDialog(pageUrl, "Yes", DialogOptions);

    if (ret == "" || typeof (ret) == "undefined") {
        return false;
    }
    else {
       
        var arrRetValues;
        var selectedId;
        var selectedFieldIndex;
        var strTmp;
        var strCustType;
        if (strType == 'Issuer') {
            arrRetValues = ret.split("!");
            selectedId = ret.substring(ret.lastIndexOf("!") + 1, ret.length);
            selectedFieldIndex = 0

            strTmp = new String();
            strTmp = arrRetValues[selectedFieldIndex];
            strTmp = strTmp.replace("&amp;", "&")
            document.getElementById("ctl00_ContentPlaceHolder1_cboIssuerName").value = selectedId
        }
        //PageMethods.BindContacts(selectedId, Id, OnRequest, OnRequestError);
        
        if (strType == 'Issuer') {
            return true;
        }
        else
        {
            return false;
        }

    }

}
//******************


function callkey() {
    e = window.event
    if (e.keyCode == 37 || e.keyCode == 35 || e.keyCode == 36 || e.keyCode == 39) {
        return true;
    }
    return false;
}
function CancelALL() {

    window.location = "IssuerInteractionDetails.aspx";
    return false;
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