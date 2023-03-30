<%@ Page Title="Yield Calculator" Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="YieldCalculator.aspx.vb" Inherits="Forms_YieldCalculator" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<style type="text/css">
        .Space
        {
            padding-top:5px; 
            padding-bottom:5px;
        }
    </style>--%>
    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/Calendar.js"></script>


    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <%--<link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />--%>
    <script type="text/javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>
    <script type="text/javascript">

        function TotalFaceValue(type) {
            var CurrSecFaceValue = $("#<%= Hid_CurrSecFaceValue.ClientID%>").val();

              if (CurrSecFaceValue > 0) {

                  var NoofBond = $("#<%= txt_NoOfBonds.ClientID%>").val();
                    var FaceValue = $("#<%= txt_quantum.ClientID%>").val() * $("#<%= cbo_Amount.ClientID%>").val();

                    if (type == 'B')
                        $("#<%= txt_quantum.ClientID%>").val(CurrSecFaceValue * NoofBond / $("#<%= cbo_Amount.ClientID%>").val());
                else if (type == 'F')
                    $("#<%= txt_NoOfBonds.ClientID%>").val(FaceValue / CurrSecFaceValue);
        }
        else
            alert('Please select security name first.');
    }

    function CheckCashFlow() {


        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_YXM_0").checked == true || document.getElementById("ctl00_ContentPlaceHolder1_rdo_YXM_2").checked == true) {

            document.getElementById("ctl00_ContentPlaceHolder1_row_CashFlow").style.display = "none";
        }
        else {

            document.getElementById("ctl00_ContentPlaceHolder1_row_CashFlow").style.display = "none";
        }
    }

    function MakeDisable() {
        var txtAnn, txtSemi;
        var arrRdo = new Array('M', 'C', 'P');
        //alert(arrRdo)
        for (i = 0; i < arrRdo.length; i++) {
            txtAnn = document.getElementById("ctl00_ContentPlaceHolder1_txt_YT" + arrRdo[i] + "Ann");
            txtSemi = document.getElementById("ctl00_ContentPlaceHolder1_txt_YT" + arrRdo[i] + "Semi");
            txtAnn.style.backgroundColor = "#E0DBDB";
            txtSemi.style.backgroundColor = "#E0DBDB";
            txtAnn.disabled = true;
            txtSemi.disabled = true;
        }

        var btn = document.getElementById("ctl00_ContentPlaceHolder1_btn_CalYield")
        btn.value = "Calculate Yield"
        document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").select();
        //document.getElementById("chk_CompRate").disabled = false 
    }
    function MakeEnable(strOption, blnFocus) {
        MakeDisable();
        var SemiAnnFlag;
        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_SemiAnn_1").checked == true) SemiAnnFlag = "Semi"
        else SemiAnnFlag = "Ann"
        var txt;
        txt = document.getElementById("ctl00_ContentPlaceHolder1_txt_YT" + strOption + SemiAnnFlag);
        txt.disabled = false;
        txt.style.backgroundColor = "White";
        if (blnFocus == true) {
            txt.focus();
            txt.select();
        }
        var btn = document.getElementById("ctl00_ContentPlaceHolder1_btn_CalYield")
        if (strOption == "M") btn.value = "Yield to price"
        if (strOption == "C") btn.value = "Call to price"
        if (strOption == "P") btn.value = "Put to price"
    }
    function ChangeSemiAnn(blnFocus) {
        var strFlag = "Y"
        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_MatToRate").checked == true) strFlag = "M"
        else if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_CallToRate").checked == true) strFlag = "C"
        else if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PutToRate").checked == true) strFlag = "P"
        if (strFlag != "Y") {
            MakeEnable(strFlag, blnFocus)
        }
    }

    function ValidateCalculation() {
        var objCheck;
        var SemiAnnFlag;
        var Id = document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value;
        if (Number(Id) == 0) {
            alert("Please select security");
            return false;
        }
        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_Yield").checked == true) {
            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value - 0) == 0) {
                alert("Please enter proper rate");
                return false;
            }
        }
        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_SemiAnn_1").checked == true) SemiAnnFlag = "Semi"
        else SemiAnnFlag = "Ann"
        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_MatToRate").checked == true) {
            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_YTM" + SemiAnnFlag).value - 0) == 0) {
                alert("Please enter proper Maturity yield");
                return false;
            }
        }
        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_CallToRate").checked == true) {
            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_YTC" + SemiAnnFlag).value - 0) == 0) {
                alert("Please enter proper Call Option yield");
                return false;
            }
        }
        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PutToRate").checked == true) {
            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_YTP" + SemiAnnFlag).value - 0) == 0) {
                alert("Please enter proper Put Option yield");
                return false;
            }
        }
        //if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_quantum").value - 0) == 0) {
        //    alert("Please enter proper quantum for accrued");
        //    return false;
        //}
        return true;
    }

    function ValidateCashFlow(CashFlowRpt) {

        var Id = "";

        Id = document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value;
        if (Number(Id) == 0) {
            alert("Please select security");
            return false;
        }
        //if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_YXM_1").checked == false) {
        //    alert("Please select XIRR for cashflow");
        //    return false;
        //}
        //if (document.getElementById("ctl00_ContentPlaceHolder1_chk_CashFlow").checked == false) {
        //    alert("Please check display cash flow checkbox");
        //    return false;
        //}
        if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_YXM_1").checked == true && document.getElementById("ctl00_ContentPlaceHolder1_chk_CashFlow").checked == true) {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_Yield").checked == true) {
                if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value - 0) == 0) {
                    alert("Please enter proper rate");
                    return false;
                }
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_MatToRate").checked == true) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_SemiAnn_0").checked == true) {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_YTMAnn").value - 0) == 0) {
                        alert("Please enter YTM annualy");
                        return false;
                    }
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_SemiAnn_1").checked == true) {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_YTMSemi").value - 0) == 0) {
                        alert("Please enter YTM semi");
                        return false;
                    }
                }
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_CallToRate").checked == true) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_SemiAnn_0").checked == true) {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_YTCAnn").value - 0) == 0) {
                        alert("Please enter YTC annualy");
                        return false;
                    }
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_SemiAnn_1").checked == true) {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_YTCSemi").value - 0) == 0) {
                        alert("Please enter YTC semi");
                        return false;
                    }
                }
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PutToRate").checked == true) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_SemiAnn_0").checked == true) {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_YTPAnn").value - 0) == 0) {
                        alert("Please enter YTP annualy");
                        return false;
                    }
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_SemiAnn_1").checked == true) {
                    if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_YTPSemi").value - 0) == 0) {
                        alert("Please enter YTP semi");
                        return false;
                    }
                }
            }

        }

    }
        


function GetCurrentFaceValue() {
        var securityId = document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value;
        var valuedate = $("#<%=txt_valuedate.ClientID%>").val();

        if (securityId > 0) {
            $.ajax({
                type: "POST",
                url: "YieldCalculator.aspx/GetCurrentFaceValue",
                data: '{securityId: "' + securityId + '",valuedate: "' + valuedate + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    $("#<%= Hid_CurrSecFaceValue.ClientID %>").val('');
                    if (result.d != "") {
                        var value = JSON.parse(result.d);
                        $("#<%= Hid_CurrSecFaceValue.ClientID %>").val(value.CurrSecFaceValue);
                        $("#<%=txt_quantum.ClientID%>").val(value.CurrSecFaceValue);
                        TotalFaceValue('B');

                    }
                },
                failure: function (result) {
                    alert('Some error has occurred.');
                }
            });
        }
    }
    </script>
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Yield Calculator
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>--%>
                <table align="center" width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tr align="center" valign="top">
                        <td style="width: 50%;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                                <ContentTemplate>
                                    <table cellspacing="0" cellpadding="0" border="1" width="100%">
                                        <tr align="left" style="display: none">
                                            <td>Security Master Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_securitytype" runat="server" Width="208px" AutoPostBack="true" CssClass="ComboBoxCSS">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr align="left">
                                            <td>Security Name:
                                            </td>
                                            <td>
                                                <uc:Search ID="srh_NameofSecurity" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="SecurityName" FormWidth="800" />

                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>ISIN:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_isin" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="9" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left" style="display: none">
                                            <td id="Client">Name OF Client:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:Search ID="Srh_NameOFClient" runat="server" PageName="NameOFClient" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="CustomerName" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Coupon Rate / Maturity:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_couponrate" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="9" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Deal Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Dealdate" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="9"></asp:TextBox><img class="calender"
                                                        src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Dealdate',this);">
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Value Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_valuedate" runat="server" CssClass="TextBoxCSS" Width="100px" onblur="GetCurrentFaceValue();"
                                                    TabIndex="9" AutoPostBack="true"></asp:TextBox><img class="calender"
                                                        src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_valuedate',this);">
                                            </td>
                                        </tr>

                                        <tr align="left">
                                            <td>Quantum:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_quantum" runat="server" CssClass="TextBoxCSS" Width="80px" TabIndex="9" onblur="javascript:TotalFaceValue('F');"></asp:TextBox>
                                                <asp:DropDownList ID="cbo_Amount" runat="server" CssClass="ComboBoxCSS" Width="100px" onchange="javascript:TotalFaceValue('F');">
                                                    <asp:ListItem Text="RUPEES" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="THOUSANDS" Value="1000"></asp:ListItem>
                                                    <asp:ListItem Text="LACS" Value="100000" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="CRORES" Value="10000000"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_NoOfBonds" runat="server">
                                            <td>No. Of Bonds:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NoOfBonds" runat="server" Width="80px" onblur="javascript:TotalFaceValue('B');"
                                                    Text="0" CssClass="TextBoxCSS" MaxLength="20" onkeypress="javascript:return OnlyIntegerKey(event);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Rate / Amount

                                            </td>
                                            <td style="padding-left: 0px;">
                                                <%--<asp:DropDownList ID="cbo_rateamount" runat="server" Width="208px" ToolTip="Rate  for Non DDB Security and Amount for DDB security" CssClass="ComboBoxCSS">
                                                    <asp:ListItem Selected="True" Value="R">Rate</asp:ListItem>
                                                    <asp:ListItem Value="A">Amount</asp:ListItem>
                                                </asp:DropDownList>--%>
                                                <asp:RadioButtonList ID="rdo_RateActual" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" ToolTip="Rate  for Non DDB Security and Amount for DDB security">
                                                    <asp:ListItem Value="R" Selected="True">Rate</asp:ListItem>
                                                    <asp:ListItem Value="A">Amount</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Yield / XIRR /MMY

                                            </td>
                                            <td style="padding-left: 0px;">
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdo_YXM" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                                                CssClass="LabelCSS">
                                                                <asp:ListItem Value="Y">Yield</asp:ListItem>
                                                                <asp:ListItem Value="X" Selected="True">XIRR</asp:ListItem>
                                                                <asp:ListItem Value="M">MMY</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td id="row_CashFlow" runat="server" style="display: none;">
                                                            <asp:CheckBox ID="chk_CashFlow" CssClass="LabelCSS" runat="server" AutoPostBack="false"
                                                                Text="Display Cash Flow" ToolTip="Click to Display cash flow"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                </table>




                                            </td>
                                            <%--<td style="padding-left: 0px;"><asp:DropDownList ID="cbo_yieldxirrmmy" runat="server" Width="208px" CssClass="ComboBoxCSS">
                                                    <asp:ListItem Selected="True" Value="Y">Yield</asp:ListItem>
                                                    <asp:ListItem Value="X">XIRR</asp:ListItem>
                                                    <asp:ListItem Value="M">MMY</asp:ListItem>
                                                </asp:DropDownList></td>--%>
                                        </tr>
                                        <tr align="left">
                                            <td>Calculation Mode:
                                            </td>
                                            <td>
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                    <tr align="left">
                                                        <td align="left">
                                                            <asp:RadioButton ID="rdo_Yield" runat="server" Checked="True" CssClass="LabelCSS"
                                                                GroupName="YM" Text="Price to Yield" ToolTip=" Yield Calculation" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:RadioButton ID="rdo_MatToRate" runat="server" Checked="false" CssClass="LabelCSS"
                                                                GroupName="YM" Text="Yield to price" ToolTip="Click to Calculate YTM to Price" />
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
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

                                            <%--<td>Calculation Mode:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:DropDownList ID="cbo_calculationmode" runat="server" Width="208px" CssClass="ComboBoxCSS">
                                                    <asp:ListItem Selected="True" Value="PY">Price to Yield</asp:ListItem>
                                                    <asp:ListItem Value="YP">Yield to price</asp:ListItem>
                                                    <asp:ListItem Value="CP">Call to price</asp:ListItem>
                                                    <asp:ListItem Value="PP">Put to price</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>--%>
                                        </tr>
                                        <tr align="left">
                                            <td>Annualised / Semi-Annualised

                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList ID="rdo_SemiAnn" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS" ToolTip="Annulised for yearly and semi annulised for half yearly">
                                                    <asp:ListItem Value="A" Selected="True">Annualised</asp:ListItem>
                                                    <asp:ListItem Value="S">Semi-Annualised</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <%--<asp:DropDownList ID="cbo_annualsemi" runat="server" Width="208px" CssClass="ComboBoxCSS">
                                                    <asp:ListItem Selected="True" Value="A">Annualised</asp:ListItem>
                                                    <asp:ListItem Value="S">Semi-Annualised</asp:ListItem>
                                                </asp:DropDownList>--%>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Rate / Price:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Rate" runat="server" CssClass="TextBoxCSS" Width="200px" TabIndex="9"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%--<tr align="left">
                                            <td colspan="2" style="width: 100%;">
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                    <tr align="left">
                                                        <td>YTM(Ann):
                                                        </td>
                                                        <td>&nbsp;
                                                            <asp:TextBox ID="txt_YTMAnn" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td>YTC(Ann):
                                                        </td>
                                                        <td>&nbsp;
                                                            <asp:TextBox ID="txt_YTCAnn" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td>YTP(Ann):
                                                        </td>
                                                        <td>&nbsp;
                                                            <asp:TextBox ID="txt_YTPAnn" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>--%>
                                        <%--<tr align="left">
                                            <td colspan="2" style="width: 100%;">
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                    <tr align="left">
                                                        <td>YTM(Semi):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_YTMSemi" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td>YTC(Semi):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_YTCSemi" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td>YTP(Semi):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_YTPSemi" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>--%>
                                        <tr align="left">
                                            <td colspan="2" style="width: 100%;">
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                    <tr align="left">
                                                        <td>YTM(Ann):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_YTMAnn" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td>YTM(Semi):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_YTMSemi" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td colspan="2" style="width: 100%;">
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                    <tr align="left">
                                                        <td>YTC(Ann):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_YTCAnn" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td>YTC(Semi):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_YTCSemi" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr align="left">
                                            <td colspan="2" style="width: 100%;">
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                    <tr align="left">
                                                        <td>YTP(Ann):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_YTPAnn" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td>YTP(Semi):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_YTPSemi" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td>
                                                <table>
                                                    <tr align="left">
                                                        <td>Last IP Date:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_lastipdate" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Principal Amount:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_PrinciAmt" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Accrued Interest:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_accrued" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>No Of Days:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_nofodays" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>


                                            </td>
                                            <td valign="top">
                                                <table>
                                                    <tr align="left">
                                                        <td>Total Consideration:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_SettlementAmt" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Stamp Duty :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_StampDuty" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Settlement Amount :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_TC" runat="server" CssClass="TextBoxCSS" Width="100px" TabIndex="9"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>

                                        </tr>

                                        <tr align="left" style="display: none;">
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rdo_IPCalc" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS" BorderStyle="None" BorderWidth="1px"
                                                    ToolTip="Equal days for monthly basis and Actual days for daily basis interest">
                                                    <asp:ListItem Value="E" Selected="True">Equal Days</asp:ListItem>
                                                    <asp:ListItem Value="A">Actual Days</asp:ListItem>
                                                </asp:RadioButtonList></td>
                                        </tr>
                                        <tr align="left" style="display: none;">
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rbl_Days" runat="server" BorderStyle="None" BorderWidth="1px"
                                                    CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true">
                                                    <asp:ListItem Value="360">360</asp:ListItem>
                                                    <asp:ListItem Value="365" Selected="True">365</asp:ListItem>
                                                    <asp:ListItem Value="366">366</asp:ListItem>
                                                </asp:RadioButtonList>

                                            </td>
                                        </tr>
                                        <tr align="left" style="display: none;">
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rbl_DaysOptions" runat="server" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="0" CellSpacing="0" CssClass="LabelCSS" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow">
                                                    <asp:ListItem Selected="True" Value="F">First Year</asp:ListItem>
                                                    <asp:ListItem Value="A">All Year</asp:ListItem>
                                                </asp:RadioButtonList>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rdo_PhysicalDMAT" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS" Visible="False">
                                                    <asp:ListItem Value="D" Selected="True">DMAT</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:CheckBox ID="chk_CombineIPMat" runat="server" AutoPostBack="false" CssClass="LabelCSS hidden"
                                                    Text="Combine IP-Mat" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                <asp:Button ID="btn_CalYield" runat="server" CssClass="ButtonCSS" Text="Calculate Yield" Width="150px" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_Reset" runat="server" CssClass="ButtonCSS" Text="Reset Data" Width="150px" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 50%;">
                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                <tr align="left">
                                    <td style="width: 100%;">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" Mode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0" cellpadding="0" border="1" width="100%">
                                                    <tr align="left">
                                                        <td style="width: 35%; padding-top: 5px; padding-bottom: 5px;">Master Type:
                                                        </td>
                                                        <td style="padding-left: 0px; padding-top: 5px; padding-bottom: 5px; width: 65%;">
                                                            <asp:Literal ID="lit_mastertype" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <%--<tr align="left">
                                                        <td >Eligibility Criteria:
                                                        </td>
                                                        <td >
                                                            <asp:Literal ID="lit_eligiblecrie" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>--%>
                                                    <tr align="left">
                                                        <td>ISIN:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_isin" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Coupon Rate (%):
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_couponrate" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Name:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_name" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>

                                                    <tr align="left">
                                                        <td>Allotment Date:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_allotmentdate" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Redemption Date:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_redemptionrate" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Interest Payment Frequency:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_intpayfrequency" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Interest Payment Date:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_intpaydate" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Put / Call Type:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_putcalltype" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>

                                                    <tr align="left">
                                                        <td>Put / Call Date:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_putcalldate" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Shut Period (No of Days):
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_shutperiod" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Step Up / Step Down If Any:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_stepupdown" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Face Value:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_facevalue" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Mature Value:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_maturevalue" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>Issue Rating:
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="lit_issuerating" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>

                                                </table>
                                                   <asp:HiddenField ID="Hid_CurrSecFaceValue" runat="server" />
                            <asp:HiddenField ID="Hid_IsStaggered" runat="server" />
                            <asp:HiddenField ID="HiddenField1" runat="server" />
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
                            <asp:HiddenField ID="Hid_IntDays" runat="server" />
                            <asp:HiddenField ID="Hid_CashFlowRpt" runat="server" />
                            <asp:HiddenField ID="Hid_YTMAnn" runat="server" />
                            <asp:HiddenField ID="Hid_YTCAnn" runat="server" />
                            <asp:HiddenField ID="Hid_YTPAnn" runat="server" />
                            <asp:HiddenField ID="Hid_YTMSemi" runat="server" />
                            <asp:HiddenField ID="Hid_YTCSemi" runat="server" />
                            <asp:HiddenField ID="Hid_YTPSemi" runat="server" />
                            <asp:HiddenField ID="Hid_CashAmount" runat="server" />
                            <asp:HiddenField ID="Hid_CashDate" runat="server" />
                                     <asp:HiddenField ID="Hid_ExcelPDFFont" runat="server" Value="16px" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                            <tr align="left">
                                                <td align="right">
                                                    <asp:Button ID="btn_CashFlowExcel" runat="server" CssClass="ButtonCSS" Text="Download Cashflow in Excel" Width="200px" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_CashFlowPdf" runat="server" CssClass="ButtonCSS" Text="Download Casflow in PDF" Width="200px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td>
                         
                        </td>
                    </tr>
                </table>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
    </table>
</asp:Content>

