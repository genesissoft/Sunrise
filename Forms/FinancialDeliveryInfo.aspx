<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FinancialDeliveryInfo.aspx.vb"
    Inherits="Forms_FinancialDeliveryInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Financial Delivery Info</title>

    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Style_New.css" type="text/css" rel="stylesheet" />

    <link href="../Include/Css/jquery-ui.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.all.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.base.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.button.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.core.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.theme.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/DatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>

    <script language="javascript" type="text/javascript">

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

        function SelectRow() {
            var cboFDType = document.getElementById("cbo_FDType")
            var strTransType = document.getElementById("Hid_TransType").value


            var strFD = cboFDType.options[cboFDType.options.selectedIndex].text
            if (strTransType == "P") {
                document.getElementById("row_cboBank").style.display = ""
                document.getElementById("row_Bank").style.display = "none"
            }
            else {
                document.getElementById("row_cboBank").style.display = "none"
                document.getElementById("row_Bank").style.display = ""
            }




            if (strFD.substr(0, 1) == "N") {

                document.getElementById("tbl_Adjust").style.display = "none"
                document.getElementById("tbl_Normal").style.display = ""
            }

            else if (strFD.substr(0, 1) == "A") {
                document.getElementById("tbl_Adjust").style.display = ""
                document.getElementById("tbl_Normal").style.display = "none"
                document.getElementById("txt_PaymntDate").value = ""
                document.getElementById("txt_BankName").value = ""
                document.getElementById("txt_ChqNo").value = ""
                document.getElementById("txt_ChequeDate").value = ""
            }
        }

        function RetValues() {
            var strReturn = "";
            var strFdType = "";
            var strAmount = "";
            var strRemark = "";
            var strPaymentDate = "";
            var strBnakName = "";
            var strChequeNo = "";
            var strChequeDate = "";
            var strAdjDealSlipIds = "";
            var strBank = document.getElementById("cbo_Bank")

            strFdType = document.getElementById("cbo_FDType").value
            strAmount = (document.getElementById("txt_Amount").value - 0)
            strRemark = document.getElementById("txt_Remark").value
            strPaymentDate = document.getElementById("txt_PaymntDate").value
            strBnakName = document.getElementById("txt_BankName").value
            strChequeNo = document.getElementById("txt_ChqNo").value
            strChequeDate = document.getElementById("txt_ChequeDate").value
            strAdjDealSlipIds = document.getElementById("Hid_AdjDealSlipId").value
            strTransType = document.getElementById("Hid_TransType").value

            strReturn = strReturn + strFdType + "!"
            strReturn = strReturn + strAmount + "!"
            strReturn = strReturn + strRemark + "!"
            strReturn = strReturn + strPaymentDate + "!"
            strReturn = strReturn + strBnakName + "!"
            strReturn = strReturn + strChequeNo + "!"
            strReturn = strReturn + strChequeDate + "!"
            strReturn = strReturn + strAdjDealSlipIds + "!"
            strReturn = strReturn + strTransType + "!"
            strReturn = strReturn + strBank.options[strBank.options.selectedIndex].text + "!"
            document.getElementById("Hid_Id").value = strReturn;
            window.returnValue = document.getElementById("Hid_Id").value;
            window.close();
        }

        function Validation() {
            var cboFDType = document.getElementById("cbo_FDType")
            var strFD = cboFDType.options[cboFDType.options.selectedIndex].text
            if (Trim(document.getElementById("cbo_FDType").value) == "") {
                AlertMessage("Validation", "Please Select FD Type", 175, 450);
                return false;
            }
            if ((document.getElementById("txt_Amount").value - 0) == 0) {
                AlertMessage("Validation", "Amount can not blank or zero", 175, 450);
                return false;
            }
            if (strFD.substr(0, 1) == "N") {
                if (Trim(document.getElementById("txt_PaymntDate").value) == "") {
                    AlertMessage("Validation", "Please Enter Payment Date", 175, 450);
                    return false;
                }
                if (document.getElementById("Hid_TransType").value == "S") {
                    if (Trim(document.getElementById("txt_BankName").value) == "") {
                        AlertMessage("Validation", "Please Enter Bank Name", 175, 450);
                        return false;
                    }
                }
                if (document.getElementById("Hid_TransType").value == "P") {
                    if (Trim(document.getElementById("cbo_Bank").value) == "") {
                        AlertMessage("Validation", "Please Select Bank Name", 175, 450);
                        return false;
                    }
                }
                if (Trim(document.getElementById("txt_ChqNo").value) == "") {
                    AlertMessage("Validation", "Please Enter Cheque Number", 175, 450);
                    return false;
                }
                if (Trim(document.getElementById("txt_ChequeDate").value) == "") {
                    AlertMessage("Validation", "Please Enter Cheque Date", 175, 450);
                    return false;
                }
            }
            var grd = document.getElementById("dg_AdjstTrans")
            var DealSlipId = false;
            if (Trim(document.getElementById("cbo_FDType").value) == "A") {
                if (grd != null) {
                    if (grd.rows.length == 1) {
                        AlertMessage("Validation", "No Deals Available for Adjustment", 175, 450)
                        return false;
                    }
                    for (i = 1; i <= (grd.rows.length - 1) ; i++) {
                        currRow = grd.children[0].children[i]
                        var chkBox = currRow.children[0].children[0].children[0]
                        if (chkBox.checked == true) {
                            DealSlipId = true
                            break
                        }
                    }
                    if (DealSlipId == false) {
                        AlertMessage("Validation", "Please select atleast one option", 175, 450)
                        return false;
                    }
                }
            }
            var strBalAmt = (document.getElementById("Hid_BalAmt").value - 0)
            var strExceedAmount = "";
            strExceedAmount = ((strBalAmt - 0) + 10.00)
            var strAmt = (document.getElementById("txt_Amount").value - 0)
            
            if (strBalAmt >= strAmt) {
                return true;
            }
            else if (strExceedAmount < strAmt) {
                AlertMessage("Validation", 'Amount should be less than from Balance Amount' + ',' + 'Your Balance Amt is = ' + strBalAmt, 175, 450)
                return false;
            }
            else (strExceedAmount >= strAmt)
            {
                if (window.confirm('Your Amount Exceed from Balance Amount' + ',' + 'Your Balance Amt is = ' + strBalAmt) == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function ValidateScript() {
            var ret = Validation();
            if (ret == true) {
                var grd = document.getElementById("dg_AdjstTrans")
                if (Trim(document.getElementById("cbo_FDType").value) == "A") {
                    if (grd != null) {
                        for (i = 1; i <= (grd.rows.length - 1) ; i++) {
                            currRow = grd.children[0].children[i]
                            var chkBox = currRow.children[0].children[0].children[0]
                            if (chkBox.checked == true) {
                                var val = currRow.children[4].children[0];
                                document.getElementById("Hid_AdjDealSlipId").value += val.innerText + ",";
                            }
                        }
                    }
                }
                RetValues();
            }
        }
    </script>
</head>
<body style="margin-left: 0px; margin-top: 5px;" class="popupbackground">
    <form id="form1" runat="server">
        <div>
            <table id="Table1" width="98%" align="center" cellspacing="0" cellpadding="0" border="0"
                class="data_table">
                <tr align="left">
                    <td class="SectionHeaderCSS popupbackground" style="text-align: center;">Financial Delivery Info</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="98%">
                            <tr align="center" valign="top">
                                <td style="width: 49%;">
                                    <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tr align="left">
                                            <td>FD Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_FDType" runat="server" CssClass="ComboBoxCSS"
                                                    Width="158px">
                                                    <asp:ListItem Value="N">Normal</asp:ListItem>
                                                    <asp:ListItem Value="A">Adjustment Against Trans</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Amount:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Amount" runat="server" Width="150px" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Remark:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Remark" runat="server" Width="150px" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7" TextMode="MultiLine" Rows="4"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 2%;">&nbsp;</td>
                                <td style="width: 49%;">
                                    <table id="tbl_Normal" align="center" cellspacing="0" cellpadding="0" border="0"
                                        width="100%">
                                        <tr align="left">
                                            <td>Payment Date:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_PaymntDate" runat="server" Width="132px" CssClass="TextBoxCSS jsdate"
                                                    MaxLength="50" TabIndex="7"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                                        </tr>
                                        <tr align="left" id="row_Bank">
                                            <td>Bank Name:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_BankName" runat="server" Width="150px" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                                        </tr>
                                        <tr align="left" id="row_cboBank">
                                            <td>Our Bank:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_Bank" runat="server" CssClass="ComboBoxCSS" Width="158px"
                                                    TabIndex="24">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>ChequeNo./RTGSDetail:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_ChqNo" runat="server" Width="150px" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7"></asp:TextBox>
                                                <%-- <em><span style="color: #ff0000">*</span></em>--%>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Cheque Date:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_ChequeDate" runat="server" Width="132px" CssClass="TextBoxCSS jsdate"
                                                    MaxLength="50" TabIndex="7"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                                        </tr>
                                    </table>
                                    <table id="tbl_Adjust" align="center" cellspacing="0" cellpadding="0" border="0" width="100%" style="height: 20%">
                                        <tr align="center" valign="top">
                                            <td align="center">
                                                <div id="div1" style="margin-top: 0px; overflow: auto; width: 98%; padding-top: 0px; position: relative; height: 20%">
                                                    <asp:DataGrid ID="dg_AdjstTrans" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                                                        TabIndex="38" Width="100%" Height="20%">
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                        <Columns>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chk_Select" runat="server" Width="20px" />
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="DealSlipNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_DealSlipNo" Width="75px" runat="server" Text='<%# container.dataitem("DealSlipNo") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="TransType">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_TransType" runat="server" Width="45px" Text='<%# container.dataitem("TransType") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Amount" runat="server" Width="55px" Text='<%# container.dataitem("Amount") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="DealSlipId" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_DealSlipId" runat="server" Width="45px" Text='<%#Container.DataItem("DealSlipId") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </div>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString%>"
                                                    SelectCommand="ID_FILL_BankMaster" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:Parameter Direction="Output" Name="RET_CODE" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" id="buttonid" runat="server">
                    <td>
                        <asp:Button ID="btn_SaveInfo" runat="server" Text="Save Info" ToolTip="Save Info"
                            CssClass="ButtonCSS hidden" />
                        <input type="button" id="btn_Ret" runat="server" value="Save Info" class="ButtonCSS" onclick="return ValidateScript();" />
                        <asp:Button ID="btn_Clear" runat="server" Text="Clear" ToolTip="Clear" CssClass="ButtonCSS" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
        <asp:HiddenField ID="Hid_FDId" runat="server" />
        <asp:HiddenField ID="Hid_AdjDealSlipId" runat="server" />
        <asp:HiddenField ID="Hid_BalAmt" runat="server" />
        <asp:HiddenField ID="Hid_CustomerId" runat="server" />
        <asp:HiddenField ID="Hid_TransType" runat="server" />
        <asp:HiddenField ID="Hid_Id" runat="server" />
    </form>
</body>
</html>
