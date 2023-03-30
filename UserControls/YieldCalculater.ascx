<%@ Control Language="VB" AutoEventWireup="false" CodeFile="YieldCalculater.ascx.vb"
    Inherits="UserControls_YieldCalculater" %>

<script type="text/javascript" src="../Include/Common.js"></script>

<script type="text/javascript" src="../Include/DatePicker.js"></script>

<link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
<link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../Include/Script/jquery.js"></script>
<script type="text/javascript" src="../Include/Script/jquery.js"></script>
<script type="text/javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
<script type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
<script type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
<script type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
<script type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
<script type ="text/javascript" src="../Include/Script/showModalDialog.j3s"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $('.jsdate').datepicker({
            showOn: "button",
            buttonImage: "../Images/calendar.gif",
            buttonImageOnly: true,
            dateFormat: 'dd/mm/yy',
            buttonText: 'Select date as (dd/mm/yyyy)'
        });

        $(".jsdate").prop('maxLength', 10);
    });

    function FillSettlementDate() {
        if (CheckDate($("#<% = txt_YTMDate.ClientID %>").get(0), false)) {
            var objData = {
                "dealdate": $("#<% = txt_YTMDate.ClientID %>").val(),
                "settlementtype": $("#<% = cbo_SettDay.ClientID %>").val()
            }

            $.ajax({
                url: "IPO_getdata.aspx?pagename=getsettlementdate",
                type: "POST",
                data: objData,
                dataType: "text",
                async: false,
                success: function (result) {
                    $("#<% = txt_SettDate.ClientID %>").val(result);
                },
                failure: function (result) {
                    alert(result);
                }
            });
            }
        }
</script>
<script type="text/javascript">
    function CheckCashFlow(parentId) {

        if (document.getElementById(parentId + "_rdo_YXM_0").checked == true || document.getElementById(parentId + "_rdo_YXM_2").checked == true) {
            document.getElementById(parentId + "_row_CashFlow").style.display = "none";
        }
        else {
            document.getElementById(parentId + "_row_CashFlow").style.display = "";
        }
    }

    function OpenCurrentRate() {
        parentId = '<%=Session("uc_parentid")%>';
        if (document.getElementById(parentId + "_Hid_Security").value == "") {
            alert("Please select the security to Calculate Current Rate");
            return false
        }
        var strId = document.getElementById(parentId + "_Hid_SecurityId").value
        var strPageFlag = document.getElementById(parentId + "_Hid_PageFlag").value
        ShowCurrRate("CurrentRate.aspx", strId, "615px", "390px", parentId)
        //return false;
    }


    function ShowCurrRate(PageName, SecurityId, strWidth, strHeight, parentId) {
        
        var rate = document.getElementById(parentId + "_txt_Rate").value
        var purDate = document.getElementById(parentId + "_txt_YTMDate").value
        if (document.getElementById(parentId + "_txt_FaceValue") != null) {
            var FaceValue = document.getElementById(parentId + "_txt_FaceValue").value
            var cboFaceValue = document.getElementById(parentId + "_Cbo_FaceValue").value
        }
        else {
            var FaceValue = 0
            var cboFaceValue = 100000
        }
        var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
        var OpenUrl = PageName + "?Id=" + SecurityId + "&Rate=" + rate + "&PurDate=" + purDate + "&FaceValue=" + FaceValue + "&Multiple=" + cboFaceValue + "&ParentId=" + parentId;
        //var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
        var ret = window.showModalDialog(OpenUrl, 'some argument', 'dialogWidth:700px;dialogHeight:350px;dialogTop:50px;center:1;status:0;resizable:0;');
        if (typeof (ret) != "undefined") {
            var ArrRetVal = ret.split(":");
            var ParentId = ArrRetVal[1];
            document.getElementById(ParentId + "_txt_Rate").value = ArrRetVal[0] - 0; //(ret - 0);;
        }
        //return ret
    }

    function ShowAccuredInterest() {
        parentId = '<%=Session("uc_parentid")%>';
        if (document.getElementById(parentId + "_Hid_Security").value == "") {
            alert("Please select the security to calculate accured interest");
            return false;
        }
        if ((document.getElementById(parentId + "_txt_Rate").value - 0) == 0) {
            alert("Please enter proper rate");
            return false;
        }
        var rate = document.getElementById(parentId + "_txt_Rate").value - 0;
        var id = (document.getElementById(parentId + "_Hid_SecurityId").value);
        var YTMDate = (document.getElementById(parentId + "_txt_YTMDate").value);
        ShowDialog("AccuredInterest.aspx", id, rate, "700px", "200px", parentId)
        return false;
    }

    function ShowDialog(PageName, customerid, Rate, strWidth, strHeight, parentId) {
        var YTMDate = document.getElementById(parentId + "_txt_YTMDate").value
        var StepUp = document.getElementById(parentId + "_Hid_StepUp").value;
        if (document.getElementById(parentId + "_txt_FaceValue") != null) {
            var FaceValue = document.getElementById(parentId + "_txt_FaceValue").value
            var cboFaceValue = document.getElementById(parentId + "_Cbo_FaceValue").value
        }
        else {
            var FaceValue = 0
            var cboFaceValue = 100000
        }
        var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
        var OpenUrl = PageName + "?Id=" + customerid + "&Rate=" + Rate + "&Date=" + YTMDate + "&FaceValue=" + FaceValue + "&Multiple=" + cboFaceValue + "&StepUp=" + StepUp;
        var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
    }
    function MakeDisable(parentId) {
        var txtAnn, txtSemi;
        var arrRdo = new Array('M', 'C', 'P');
        //alert(arrRdo)
        for (i = 0; i < arrRdo.length; i++) {
            txtAnn = document.getElementById(parentId + "_txt_YT" + arrRdo[i] + "Ann");
            txtSemi = document.getElementById(parentId + "_txt_YT" + arrRdo[i] + "Semi");
            txtAnn.style.backgroundColor = "#E0DBDB";
            txtSemi.style.backgroundColor = "#E0DBDB";
            txtAnn.disabled = true;
            txtSemi.disabled = true;
        }
        var btn = document.getElementById(parentId + "_btn_CalYield")
        btn.value = "Calculate Yield"
        document.getElementById(parentId + "_txt_Rate").select();
        //document.getElementById("chk_CompRate").disabled = false 
    }
    function MakeEnable(parentId, strOption, blnFocus) {
        MakeDisable(parentId)
        var SemiAnnFlag;
        if (document.getElementById(parentId + "_rdo_SemiAnn_1").checked == true) SemiAnnFlag = "Semi"
        else SemiAnnFlag = "Ann"
        var txt;
        txt = document.getElementById(parentId + "_txt_YT" + strOption + SemiAnnFlag);
        txt.disabled = false;
        txt.style.backgroundColor = "White";
        if (blnFocus == true) {
            txt.focus();
            txt.select();
        }
        var btn = document.getElementById(parentId + "_btn_CalYield")
        if (strOption == "M") btn.value = "Yield to price"
        if (strOption == "C") btn.value = "Call to price"
        if (strOption == "P") btn.value = "Put to price"
    }
    function ChangeSemiAnn(parentId, blnFocus) {
        var strFlag = "Y"
        if (document.getElementById(parentId + "_rdo_MatToRate").checked == true) strFlag = "M"
        else if (document.getElementById(parentId + "_rdo_CallToRate").checked == true) strFlag = "C"
        else if (document.getElementById(parentId + "_rdo_PutToRate").checked == true) strFlag = "P"
        if (strFlag != "Y") {
            MakeEnable(parentId, strFlag, blnFocus)
        }
    }
    function ValidateCalculation(parentId) {
        var objCheck;
        var SemiAnnFlag;
        if (document.getElementById(parentId + "_rdo_Yield").checked == true) {
            if ((document.getElementById(parentId + "_txt_Rate").value - 0) == 0) {
                alert("Please enter proper rate");
                return false
            }
        }
        if (document.getElementById(parentId + "_rdo_SemiAnn_1").checked == true) SemiAnnFlag = "Semi"
        else SemiAnnFlag = "Ann"
        if (document.getElementById(parentId + "_rdo_MatToRate").checked == true) {
            if ((document.getElementById(parentId + "_txt_YTM" + SemiAnnFlag).value - 0) == 0) {
                alert("Please enter proper Maturity yield");
                return false
            }
        }
        if (document.getElementById(parentId + "_rdo_CallToRate").checked == true) {
            if ((document.getElementById(parentId + "_txt_YTC" + SemiAnnFlag).value - 0) == 0) {
                alert("Please enter proper Call Option yield");
                return false
            }
        }
        if (document.getElementById(parentId + "_rdo_PutToRate").checked == true) {
            if ((document.getElementById(parentId + "_txt_YTP" + SemiAnnFlag).value - 0) == 0) {
                alert("Please enter proper Put Option yield");
                return false
            }
        }
        return true
    }
    function ShowCashFlow(PageName, strAmt, strDate, Width, Height) {
        var w = Width;
        var h = Height;
        var winl = (screen.width - w) / 2;
        var wint = (screen.height - h) / 2;
        if (winl < 0) winl = 0;
        if (wint < 0) wint = 0;
        PageName = PageName + "?Amount=" + strAmt + "&Date=" + strDate;
        windowprops = "height=" + h + ",width=" + w + ",top=150,left=800,location=no,"
        + "scrollbars=yes,menubars=no,toolbars=no,resizable=yes,status=no";
        window.showModalDialog(PageName, target = '_blank', windowprops);
    }

    function DealSlip() {
        var purSellFlag = 'P'
        var securityid = document.getElementById("Hid_Securityid").value
        var custId = document.getElementById("cbo_clientname").value
        //var rate = document.getElementById("Hid_Rate").value 
        var faceValue = document.getElementById("txt_FaceValue").value;
        var faceMultiple = document.getElementById("cbo_FaceValue").value;
        var rate = document.getElementById("txt_Rate").value
        var ytmDate = document.getElementById("txt_YTMDate").value
        var phyDMATSGLFlag = "D"
        if (document.getElementById("rbl_DMATphy_1").checked == true) phyDMATSGLFlag = "P"
        if (document.getElementById("rbl_DMATphy_2").checked == true) phyDMATSGLFlag = "S"

        if (faceMultiple == 'T') faceMultiple = "1000"
        if (faceMultiple == 'L') faceMultiple = "100000"
        if (faceMultiple == 'C') faceMultiple = "10000000"

        if (document.getElementById("RBL_typeofTranction_1").checked == true) purSellFlag = 'S'
        ShowDealSlip("DealSlip.aspx", securityid, rate, custId, ytmDate, faceValue, faceMultiple, purSellFlag, phyDMATSGLFlag, "450px", "450px")
    }
    function ShowDealSlip(PageName, secId, rate, custId, ytmDate, faceValue, faceMultiple, purSellFlag, phyDMATSGLFlag, strWidth, strHeight) {
        var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
        var OpenUrl = PageName + "?Id=" + secId + "&Rate=" + rate + "&CustomerId=" + custId + "&YTMDate=" + ytmDate + "&FaceValue=" + faceValue + "&FaceMultiple=" + faceMultiple + "&PurSellFlag=" + purSellFlag + "&PhyDMATSGLFlag=" + phyDMATSGLFlag;
        var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
    }

    function Close() {

        parentId = '<%=Session("uc_parentid")%>';
        var returnval = "";
        var AnnSemi = "A"
        var combinedipmat = 0;
        var EqualActFlag = "F";
        var intdays = "F";
        var RateActual = "R";
        var FirstYrAllYr = "F";
        var Instrument = "";
        Instrument = document.getElementById(parentId + "_Hid_NatureOfInstrument").value
        GovFlag = document.getElementById(parentId + "_Hid_GovernmentFlag").value
        
        if (document.getElementById(parentId + "_rdo_RateActual_1").checked == true) {
            RateActual = "R";
        }
        else {
            RateActual = "A";
        }

        combinedipmat = 0;
        if (document.getElementById(parentId + "_rdo_SemiAnn_1").checked == true) {
            AnnSemi = "A";
        }
        else {
            AnnSemi = "S";
        }


        if (document.getElementById(parentId + "_rdo_IPCalc_1").checked == true) {
            EqualActFlag = "E"
        }
        else {
            EqualActFlag = "A"
        }



        if (document.getElementById(parentId + "_rbl_DaysOptions_1").checked == true) {
            FirstYrAllYr = "F"
        }
        else {
            FirstYrAllYr = "A"
        }

        if (document.getElementById(parentId + "_rbl_Days_1").checked == true) {
            intdays = "365"
        }
        else {
            intdays = "366"
        }
        returnval = returnval + document.getElementById(parentId + "_txt_Rate").value + "!";
        if (Instrument == "P") {
            returnval = returnval + document.getElementById(parentId + "_txt_YTCAnn").value + "!";
        }
        else {
            if (Instrument != "P") {
                var ytc = document.getElementById(parentId + "_txt_YTCAnn").value
                if (ytc != 0) {
                    returnval = returnval + document.getElementById(parentId + "_txt_YTCAnn").value + "!";
                }
                else {
                    if (GovFlag == 'N') {

                        returnval = returnval + document.getElementById(parentId + "_txt_YTMAnn").value + "!";
                    }
                    else {
                        returnval = returnval + document.getElementById(parentId + "_txt_YTMSemi").value + "!";
                    }
                }
            }
        }
        //returnval = returnval + document.getElementById(parentId + "_txt_Rate").value + "!";
        //returnval = returnval + document.getElementById(parentId + "_txt_YTMAnn").value + "!";
        returnval = returnval + AnnSemi + "!";
        returnval = returnval + combinedipmat + "!";
        returnval = returnval + RateActual + "!";
        returnval = returnval + EqualActFlag + "!";
        returnval = returnval + intdays + "!";
        returnval = returnval + FirstYrAllYr + "!";

        pageFlag = '<%=Session("PageFlag")%>';
        document.getElementById("Hid_Id").value = returnval;
        window.returnValue = returnval
        window.close();
       
        if (typeof (returnval) != "undefined") {
            var yield = returnval.split('!');
            if (pageFlag == "2") {
                parent.document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value = yield[0];
                parent.document.getElementById("Hid_Semi_Ann_Flag").value = yield[2];
                parent.document.getElementById("Hid_CombineIPMat").value = yield[3];
                parent.document.getElementById("Hid_Rate_Actual_Flag").value = yield[4];
                parent.document.getElementById("Hid_Equal_Actual_Flag").value = yield[5];
                parent.document.getElementById("Hid_IntDays").value = yield[6];
                parent.document.getElementById("Hid_FirstYrAllYr").value = yield[7];
                '<%=Session("PageFlag")%>'.value = ""
            }
            else {
                parent.document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value = yield[0];
                parent.document.getElementById("ctl00_ContentPlaceHolder1_Hid_Yield").value = yield[1];
                parent.document.getElementById("ctl00_ContentPlaceHolder1_txt_Yield").value = yield[1];
            }
        }
    }

    function FillSettlementDate() {
        if (CheckDate($("#<% = txt_YTMDate.ClientID %>").get(0), false)) {
            var objData = {
                "dealdate": $("#<% = txt_YTMDate.ClientID %>").val(),
                "settlementtype": $("#<% = cbo_SettDay.ClientID %>").val()
            }

            $.ajax({
                url: "IPO_getdata.aspx?pagename=getsettlementdate",
                type: "POST",
                data: objData,
                dataType: "text",
                async: false,
                success: function (result) {
                    $("#<% = txt_SettDate.ClientID %>").val(result);
                },
                failure: function (result) {
                    alert(result);
                }
            });
            }
        }

</script>

<table cellpadding="0" cellspacing="0" border="0" width="100%" class="table_border_right_bottom">
    <tr align="left">
        <td>YTM Date:
        </td>
        <td>
            <asp:TextBox ID="txt_YTMDate" runat="server" Width="70px" CssClass="TextBoxCSS jsdate" MaxLength="20"></asp:TextBox>
            T +:
            <asp:DropDownList ID="cbo_SettDay" Width="40px" runat="server" CssClass="ComboBoxCSS"
                AutoPostBack="true" onchange="javascript:FillSettlementDate();"
                TabIndex="1">
                <%--onchange="javascript:FillSettlementDate();">--%>
                <asp:ListItem Value="0">0</asp:ListItem>
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
                <asp:ListItem Value="4">4</asp:ListItem>
                <asp:ListItem Value="5">5</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="txt_SettDate" runat="server" Width="70px" CssClass="TextBoxCSS"
                MaxLength="10"></asp:TextBox>
        </td>
        <td>Rate:
        </td>
        <td>
            <asp:TextBox ID="txt_Rate" runat="server" Width="60px" CssClass="TextBoxCSS" MaxLength="20"></asp:TextBox><em><span
                style="color: Red; vertical-align: super;">*</span></em>
        </td>
        <td>
            <asp:Literal ID="lit_FaceValue" runat="server" Text="Face Value:"></asp:Literal>
        </td>
        <td>
            <asp:TextBox ID="txt_FaceValue" runat="server" Width="60px" CssClass="TextBoxCSS"
                MaxLength="20"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
        </td>
        <td>
            <asp:DropDownList ID="Cbo_FaceValue" Width="75px" runat="server" CssClass="ComboBoxCSS"
                TabIndex="1">
                <asp:ListItem Value="1">Rupees</asp:ListItem>
                <asp:ListItem Value="1000">Thousand</asp:ListItem>
                <asp:ListItem Selected="true" Value="100000">Lac</asp:ListItem>
                <asp:ListItem Value="10000000">Crore</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr align="left">
        <td colspan="2" nowrap="nowrap">
            <asp:RadioButtonList ID="rdo_RateActual" runat="server" RepeatDirection="Horizontal"
                RepeatLayout="Flow" ToolTip="Rate  for Non DDB Security and Amount for DDB security">
                <asp:ListItem Value="R" Selected="True">Rate</asp:ListItem>
                <asp:ListItem Value="A">Amount</asp:ListItem>
            </asp:RadioButtonList>
            <%-- <asp:CheckBox ID="chk_CompRate" runat="server" AutoPostBack="false" CssClass="LabelCSS"
                Text="Comprehensive Rate" />--%>
        </td>
        <td colspan="2">
            <asp:RadioButtonList ID="rdo_YXM" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                CssClass="LabelCSS">
                <asp:ListItem Value="Y" >Yield</asp:ListItem>
                <asp:ListItem Value="X" Selected="True">XIRR</asp:ListItem>
                <asp:ListItem Value="M">MMY</asp:ListItem>
            </asp:RadioButtonList>
            <%--<asp:CheckBox ID="chk_CashFlow" CssClass="LabelCSS" runat="server" AutoPostBack="false"
                Text="Display Cash Flow"></asp:CheckBox>--%>
        </td>
        <td colspan="3" class="table_border_none">
            <table id="row_CashFlow" runat="server" cellpadding="0" cellspacing="0" border="0">
                <tr align="left">
                    <td>
                        <asp:CheckBox ID="chk_CashFlow" CssClass="LabelCSS" runat="server" AutoPostBack="false"
                            Text="Display Cash Flow" ToolTip="Click to Display cash flow"></asp:CheckBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr align="left" style ="display:none;">
        <td colspan="3">
            <asp:RadioButtonList ID="rdo_IPCalc" runat="server" RepeatDirection="Horizontal"
                RepeatLayout="Flow" CssClass="LabelCSS" BorderStyle="None" BorderWidth="1px"
                ToolTip="Equal days for monthly basis and Actual days for daily basis interest">
                <asp:ListItem Value="E" Selected="True">Equal Days</asp:ListItem>
                <asp:ListItem Value="A">Actual Days</asp:ListItem>
            </asp:RadioButtonList></td>
        <td colspan="2">
            <asp:RadioButtonList ID="rbl_Days" runat="server" BorderStyle="None" BorderWidth="1px"
                CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true">
                <asp:ListItem Value="360">360</asp:ListItem>
                <asp:ListItem Value="365" Selected="True">365</asp:ListItem>
                <asp:ListItem Value="366">366</asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td colspan="2">
            <asp:RadioButtonList ID="rbl_DaysOptions" runat="server" BorderStyle="None" BorderWidth="1px"
                CellPadding="0" CellSpacing="0" CssClass="LabelCSS" RepeatDirection="Horizontal"
                RepeatLayout="Flow">
                <asp:ListItem Selected="True" Value="F">First Year</asp:ListItem>
                <asp:ListItem Value="A">All Year</asp:ListItem>
            </asp:RadioButtonList></td>
    </tr>
    <tr align="left">
        <td colspan="3">
            <asp:RadioButtonList ID="rdo_PhysicalDMAT" runat="server" RepeatDirection="Horizontal"
                RepeatLayout="Flow" CssClass="LabelCSS" Visible="False">
                <asp:ListItem Value="D" Selected="True">DMAT</asp:ListItem>
            </asp:RadioButtonList>
            <asp:CheckBox ID="chk_CombineIPMat" runat="server" AutoPostBack="false" CssClass="LabelCSS hidden"
                Text="Combine IP-Mat" />
        </td>
        <td colspan="4" rowspan="2" class="table_border_none">
            <table align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left">
                        <asp:RadioButton ID="rdo_Yield" runat="server" Checked="True" CssClass="LabelCSS"
                            GroupName="YM" Text="Yield" ToolTip=" Yield Calculation" />
                    </td>
                    <td align="left">
                        <asp:RadioButton ID="rdo_MatToRate" runat="server" Checked="false" CssClass="LabelCSS"
                            GroupName="YM" Text="YTM to price" ToolTip="Click to Calculate YTM to Price" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:RadioButton ID="rdo_CallToRate" runat="server" Checked="false" CssClass="LabelCSS"
                            GroupName="YM" Text="Call to price" ToolTip="Click to Calculate YTC" />
                    </td>
                    <td align="left">
                        <asp:RadioButton ID="rdo_PutToRate" runat="server" Checked="false" CssClass="LabelCSS"
                            GroupName="YM" Text="Put to price" ToolTip="Click to Calculate YTP" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr align="left">
        <td colspan="3">
            <asp:RadioButtonList ID="rdo_SemiAnn" runat="server" RepeatDirection="Horizontal"
                RepeatLayout="Flow" CssClass="LabelCSS" ToolTip="Annulised for yearly and semi annulised for half yearly">
                <asp:ListItem Value="A" Selected="True">Annualised</asp:ListItem>
                <asp:ListItem Value="S">Semi-Annualised</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr align="left">
        <td>YTM(Semi):
        </td>
        <td colspan="2">
            <asp:TextBox ID="txt_YTMSemi" runat="server" CssClass="TextBoxCSS" Width="100px"
                TabIndex="9"></asp:TextBox>
        </td>
        <td>YTM(Ann):
        </td>
        <td colspan="3">
            <asp:TextBox ID="txt_YTMAnn" runat="server" CssClass="TextBoxCSS" Width="100px"
                TabIndex="9"></asp:TextBox>
        </td>
    </tr>
    <tr align="left">
        <td>YTC(Semi):
        </td>
        <td colspan="2">
            <asp:TextBox ID="txt_YTCSemi" runat="server" CssClass="TextBoxCSS" Width="100px"
                TabIndex="9"></asp:TextBox>
        </td>
        <td>YTC(Ann):
        </td>
        <td colspan="3">
            <asp:TextBox ID="txt_YTCAnn" runat="server" CssClass="TextBoxCSS" Width="100px"
                TabIndex="9"></asp:TextBox>
        </td>
    </tr>
    <tr align="left">
        <td>YTP(Semi):
        </td>
        <td colspan="2">
            <asp:TextBox ID="txt_YTPSemi" runat="server" CssClass="TextBoxCSS" Width="100px"
                TabIndex="9"></asp:TextBox>
        </td>
        <td>YTP(Ann):
        </td>
        <td colspan="3">
            <asp:TextBox ID="txt_YTPAnn" runat="server" CssClass="TextBoxCSS" Width="100px"
                TabIndex="9"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="7" nowrap="nowrap">
            <%--<asp:ScriptManager ID="scr1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="up1" runat="server">--%>
            <%-- <ContentTemplate>--%>
            <asp:Button ID="btn_CalCurrRate" runat="server" Text="Calculate Current Rate" ToolTip="Calculate Current Rate"
                CssClass="ButtonCSS" Width="145px" Visible="false" />&nbsp;
         <input type="button" id="btn_Cal_Curr_Rate" value="Calculate Current Rate" class="ButtonCSS" style="width: 145px;" onclick="OpenCurrentRate();" />
            <asp:Button ID="btn_CalInterest" runat="server" Text="Show Accured Interest" ToolTip="Show Accured Interest"
                CssClass="ButtonCSS" Width="145px" Visible="false" />&nbsp;
              <%--  </ContentTemplate>
            </asp:UpdatePanel>--%>
            <asp:Button ID="btn_CalYield" runat="server" Text="Calculate Yield" ToolTip="Calculate Yield"
                Width="130px" CssClass="ButtonCSS" />&nbsp;
            <asp:Button ID="btn_Ret" runat="server" Text="Close" ToolTip="Close" CssClass="ButtonCSS" ClientIDMode="Static" />
            <%--<input type="button" id="btn_Ret" value="Close" class="ButtonCSS" style="width: 145px;" onclick="Close();" />--%>
        </td>
    </tr>
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
    <asp:HiddenField ID="Hid_SecurityId" runat="server" />
    <asp:HiddenField ID="Hid_Date" runat="server" />
    <asp:HiddenField ID="Hid_Id" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="Hid_PageFlag" runat="server" />
    <asp:HiddenField ID="Hid_StepUp" runat="server" />
    <asp:HiddenField ID="Hid_NatureOfInstrument" runat="server" />
</table>
