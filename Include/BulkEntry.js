// JScript File

function ShowYieldCalculation() {

    var rate = document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_txt_Name").value
    var id = document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_Hid_SelectedId").value
    var PageName = "YieldCalculation.aspx";
    var pageUrl = PageName + "?Id=" + id + "&Rate=" + rate;
    rate = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:700px;dialogHeight:500px;center:1;status:0;resizable:1;');
    if (typeof (rate) != "undefined") {
        var yield = rate.toString().split("!");
        document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value = yield[0];
    }
} 

function ShowDialog(PageName, customerid, Rate, strWidth, strHeight) {
    var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
    var OpenUrl = PageName + "?Id=" + customerid + "&Rate=" + Rate;
    var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
    return ret
}

function UpdateDetails(strval, rowIndex) {

    //    alert(strval);
    var pageUrl = "AddBulk.aspx";

    // alert(;
    document.getElementById("ctl00_ContentPlaceHolder1_Hid_RowIndex").value = rowIndex

    var Hid_NSDLVAL = document.getElementById("ctl00_ContentPlaceHolder1_Hid_NSDLFaceValue").value;
    var perc = 0;

    // alert(Hid_NSDLVAL);
    var ele1 = document.getElementById("ctl00_ContentPlaceHolder1_row_Per");

    // alert(ele1);

    if (!(ele1 == null)) {

        var visb = ele1.style.display;
        if (visb == "") {
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_BrokPerc").value == "") {
                alert("Please Enter Percent Value");
                return false;
            }
            else {
                var perc = document.getElementById("ctl00_ContentPlaceHolder1_txt_BrokPerc").value;
            }
        }
    }

    var pageUrl = "AddBulk.aspx?bAdd='E'&Hid_NSDLFaceValue=" + Hid_NSDLVAL + "&percent=" + perc + "&str=" + strval;
    var DialogOptions = "Center=Yes; scroll=No; dialogWidth=600px; dialogTop=250px; dialogHeight=400px; Help=No; status=No; Resizable=No;titlebar=no;toolbar=no;title=abc;"
    var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:600px;dialogHeight:400px;center:1;status:0;resizable:1;');
    if (typeof (ret) != "undefined") {
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetVal").value = ret;
        document.getElementById("ctl00_ContentPlaceHolder1_btn_addDet").click();
    }
}


function Delete_entry() {
    if (window.confirm("Are you sure you want to Delete record ?")) {
        return true
    }
    else {
        return false
    }
}

function CalcBrokAmt() {

    //alert("CalcBrokAmt");
    var faceValue = document.getElementById("txt_Amount").value;
    var faceMultiple = document.getElementById("cbo_Amount").value;

    var totFaceVal = faceValue * faceMultiple;
    var perc = document.getElementById("hid_percent").value;

    var brok_amt = 0;
    if (!(perc == 0 && (faceValue == 0 || faceValue == ""))) {
        brok_amt = (totFaceVal * (perc / 100));
    }

    var brok_amt_R = round_decimals(brok_amt, 2);
    document.getElementById("txt_Brok").value = brok_amt_R;


}


function CheckDealType() {
    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
        if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
            document.getElementById("Seller").innerHTML = "Name Of Buyer"
        }
        else {
            document.getElementById("Seller").innerHTML = "Name Of Seller"
        }
    }

    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {
        if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {

            //                    document.getElementById("row_BrokingBTB").style.display = "block";
            document.getElementById("row_SelectMethod").style.display = "none";
        }
        else {
            document.getElementById("row_BrokingBTB").style.display = "none";
        }
    }
    else {
        //                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "F" )
        //                {
        //                   
        //                    document.getElementById("row_BrokingBTB").style.display = "none";
        //                    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true)  
        //                    {  
        //                    
        //                        if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_0").checked == true)
        //                        {                          
        //                            document.getElementById("row_addmultipleFinancial").style.display = "none";
        //                        }
        //                    }  
        //                }  
        //                else
        //                {
        //                    document.getElementById("row_BrokingBTB").style.display = "none";
        //                }                 
    }

    //            Showaddlnk()

    if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "F") {
        //                document.getElementById("row_FinDealType").style.display = "block";
        if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_0").checked == true) {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_0").checked == true) {
                //                         document.getElementById("row_BTB").style.display = "none";
            }
            else {
                //                        document.getElementById("row_BTB").style.display = "none";
            }
        }
        else {
            //                    if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_0").checked == true)
            //                    { 
            //                        document.getElementById("row_BTB").style.display = "none";
            //                    }
            //                    else                    
            //                    {
            //                        document.getElementById("row_BTB").style.display = "block";
            //                    }
        }
    }
    else {
        document.getElementById("row_FinDealType").style.display = "none";
    }

    if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true) {

        if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "F") {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_FinancialDealType_0").checked == true) {
                //                                document.getElementById("row_BTB").style.display = "block";
                //                                document.getElementById("row_SelectMethod").style.display = "none";
            }
        }
    }


}



function TotalNoOfBond() {

    var TotalAppAmount;
    var facevalue = document.getElementById("Hid_NSDLFaceValue").value - 0;

    if (document.getElementById("cbo_Amount").value - 0 != 100000) {
        TotalAppAmount = (document.getElementById("txt_Amount").value - 0) * (document.getElementById("cbo_Amount").value - 0);
    }
    else {
        TotalAppAmount = ((document.getElementById("txt_Amount").value - 0) * 10000 * 10);
    }

    if (facevalue == 0) {
        return TotalAppAmount; // =(document.getElementById("txt_Amount").value-0) * (document.getElementById("cbo_Amount").value-0);
    }

    document.getElementById("txt_NoOfBonds").value = (TotalAppAmount / (document.getElementById("Hid_NSDLFaceValue").value - 0));

}


function CheckPhysicalDMAT(blnFlag) {

    if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PhysicalDMAT_1").checked == true) {

        //               alert('1')
        document.getElementById("ctl00_ContentPlaceHolder1_row_Bank").style.display = "none";
        document.getElementById("ctl00_ContentPlaceHolder1_row_Demat").style.display = "none";

        if (blnFlag == true) {

            document.getElementById("ctl00_ContentPlaceHolder1_row_SGL").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value = "S"

        }
        if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
            document.getElementById("row_CounterCustSGLWith").style.display = "";
        }
        else {
            document.getElementById("row_CounterCustSGLWith").style.display = "none";
        }

    }
    else {
        //alert('2')
        document.getElementById("ctl00_ContentPlaceHolder1_row_Bank").style.display = "";
        document.getElementById("ctl00_ContentPlaceHolder1_row_Demat").style.display = "";
        document.getElementById("row_CustSGL").style.display = "none";
        document.getElementById("ctl00_ContentPlaceHolder1_row_SGL").style.display = "none";
        if (blnFlag == true) {
            document.getElementById("ctl00_ContentPlaceHolder1_cbo_ModeOfPayment").value = "H"
        }

        if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == "B") {
            document.getElementById("row_CounterCustSGLWith").style.display = "none";
        }
        else {
            document.getElementById("row_CounterCustSGLWith").style.display = "none";
        }
    }

}

function ReferenceBy() {

    document.getElementById("tr_refrenceBy").style.display = "none";
    document.getElementById("tr_refrenceByDealer").style.display = "none";
}


function Showaddlnk() {
    //       
    //    if(document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value != "B" )
    //    {
    //     if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTranction_1").checked == true)
    //     {
    ////          if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealType_1").checked == true)
    ////         {   
    //            if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_PurMethod_0").checked == true)
    //            {
    //                document.getElementById("row_lnkadd").style.display  = "none"
    //                document.getElementById("row_BTB").style.display  = "block"   
    //                document.getElementById("row_BrokingBTB").style.display = "none";
    //            }
    //            else
    //            {
    //                document.getElementById("row_lnkadd").style.display  = "block"
    //                document.getElementById("row_BTB").style.display  = "none"
    //                document.getElementById("row_BrokingBTB").style.display = "none"; 
    //            }           
    ////         } 
    //       }              
    //                       
    //     }

}

function CalcRoundofsettAMT() {

    var TotalRoundofsettAMT;
    var Roundoff;
    var SettlementAmt;

    SettlementAmt = (document.getElementById("ctl00_ContentPlaceHolder1_Hid_SettlementAmt").value - 0)
}


function Brokerage() {

    var ele1 = document.getElementById("ctl00_ContentPlaceHolder1_row_Per");

    // alert(document.getElementById("ctl00_ContentPlaceHolder1_row_Per"));

    if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_BrokPerc_0").checked == true) {
        ele1.style.display = "none";
    }
    else {
        ele1.style.display = "";
    }
    document.getElementById("ctl00_ContentPlaceHolder1_Hid_Radio_opt").value = document.getElementById("ctl00_ContentPlaceHolder1_rdo_BrokPerc_0").checked;
}
