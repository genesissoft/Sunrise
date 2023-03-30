<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DematDeliveryDetails.aspx.vb"
    Inherits="Forms_DematDeliveryDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Demat Delivery detail</title>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Style_New.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">


        function Validation() {
            if (document.getElementById("txt_DelDate").value == "") {
                alert("Please Enter delivery date");
                return false
            }
            if (document.getElementById("cbo_DPName").value == "") {
                alert("Please Select DP Name ");
                return false
            }
            RetValues();
        }


        function TotalNoOfBond() {
            var TotalAppAmount;
            if ((document.getElementById("txt_NSDLFaceValue").value - 0) == 0) return
            TotalAppAmount = (document.getElementById("txt_FaceVal").value - 0) * (document.getElementById("cbo_FaceVal").value - 0);
            document.getElementById("txt_Qty").value = TotalAppAmount / (document.getElementById("txt_NSDLFaceValue").value - 0);
            document.getElementById("Hid_bond").value = (document.getElementById("txt_Qty").value)
        }



        function RetValues() {
            var strReturn = "";
            var strFaceVal = "";
            var strNSDLFaceVal = "";
            var strDmatSlipNo = "";
            var strClientName = "";
            var strDPName = "";
            var strDPId = "";
            var strClientId = "";
            var strQty = "";
            var strDelDate = "";
            var strDematAccTo = "";
            var strCustSlipNo = "";
            var strCustDPName = "";
            var strCustDPId = "";
            var strCustClientId = "";
            var strDealSlipId = "";
            var cboFaceMultiple = "";
            var strSettleNo = "";

            var cboDPName = document.getElementById("cbo_DPName")
            var cboCustDPName = document.getElementById("cbo_CustDPName")
            var cboFaceMultiple = document.getElementById("cbo_FaceVal")
            var strPayMode = document.getElementById("Hid_PayMode").value
            if (strPayMode == "B") {
                strSettleNo = document.getElementById("txt_SettleNo").value
            }
            else {
                strSettleNo = ""
            }


            strFaceVal = document.getElementById("txt_FaceVal").value * document.getElementById("cbo_FaceVal").value
            strNSDLFaceVal = (document.getElementById("txt_NSDLFaceValue").value - 0)
            strDmatSlipNo = document.getElementById("txt_DmatSlipNo").value
            strClientName = document.getElementById("txt_ChqNo").value
            strDPName = document.getElementById("cbo_DPName").value
            strDPId = document.getElementById("txt_DPId").value
            strClientId = document.getElementById("txt_ClientId").value
            strQty = document.getElementById("Hid_bond").value
            strDelDate = document.getElementById("txt_DelDate").value
            strDematAccTo = document.getElementById("cbo_DematAccTo").value
            strCustSlipNo = document.getElementById("txt_CustSlipNo").value
            strCustDPName = document.getElementById("cbo_CustDPName").value
            strCustDPId = document.getElementById("txt_CustDPId").value
            strCustClientId = document.getElementById("txt_CustClientId").value
            strDealSlipId = document.getElementById("Hid_DealSlipId").value
            strFaceMultiple = document.getElementById("cbo_FaceVal").value
            document.getElementById("Hid_DematAccTo").value = strDematAccTo
            strRemark = document.getElementById("txt_Remark").value

            strReturn = strReturn + strFaceVal + "!"
            strReturn = strReturn + strNSDLFaceVal + "!"
            strReturn = strReturn + strDmatSlipNo + "!"
            strReturn = strReturn + strClientName + "!"
            strReturn = strReturn + strDPName + "!"
            strReturn = strReturn + strDPId + "!"
            strReturn = strReturn + strClientId + "!"
            strReturn = strReturn + strQty + "!"
            strReturn = strReturn + strDelDate + "!"
            strReturn = strReturn + strDematAccTo + "!"
            strReturn = strReturn + strCustSlipNo + "!"
            strReturn = strReturn + strCustDPName + "!"
            strReturn = strReturn + strCustDPId + "!"
            strReturn = strReturn + strCustClientId + "!"
            strReturn = strReturn + strDealSlipId + "!"
            strReturn = strReturn + cboDPName.options[cboDPName.options.selectedIndex].text + "!"
            strReturn = strReturn + cboCustDPName.options[cboCustDPName.options.selectedIndex].text + "!"
            strReturn = strReturn + strFaceMultiple + "!"
            strReturn = strReturn + strSettleNo + "!"
            strReturn = strReturn + strRemark + "!"

            document.getElementById("Hid_Id").value = strReturn;
            window.returnValue = document.getElementById("Hid_Id").value;
            window.close();
        }
    </script>

</head>
<body style="margin-left: 0px; margin-top: 5px;" class="popupbackground">
    <form id="form1" runat="server">
        <div>
            <table width="98%" align="center" cellpadding="0" cellspacing="0" class="data_table">
                <tr align="center">
                    <td class="SectionHeaderCSS HeaderCSS popupbackground">Details Section</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top" id="row_Details" runat="server">
                    <td>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="98%">
                            <tr align="center" valign="top">
                                <td style="width: 49%;">
                                    <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tr align="left">
                                            <td>Face Value:
                                            </td>
                                            <td style="padding: 0px;">
                                                <table cellspacing="0" cellpadding="0" border="0">
                                                    <tr align="left">
                                                        <td>
                                                            <asp:TextBox ID="txt_FaceVal" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                                TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cbo_FaceVal" Width="95px" runat="server" CssClass="ComboBoxCSS"
                                                                TabIndex="1">
                                                                <asp:ListItem Value="1000">Thousand</asp:ListItem>
                                                                <asp:ListItem Selected="true" Value="100000">Lac</asp:ListItem>
                                                                <asp:ListItem Value="10000000">Crore</asp:ListItem>
                                                            </asp:DropDownList><em><span style="color: Red; vertical-align: super;"></span></em>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>NSDL Face Value:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NSDLFaceValue" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7" Enabled="False"></asp:TextBox><em><span style="color: Red; vertical-align: super;"></span></em></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Slip Number:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_DmatSlipNo" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Client Name:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_ChqNo" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7" Enabled="False"></asp:TextBox><em><span style="color: Red; vertical-align: super;"></span></em></td>
                                        </tr>
                                        <tr align="left">
                                            <td>DP Name:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_DPName" runat="server" CssClass="ComboBoxCSS" Width="208px"
                                                    AutoPostBack="True" Enabled="False" Font-Bold="True">
                                                </asp:DropDownList><em><span style="color: Red; vertical-align: super;">*</span></em>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>DP ID:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_DPId" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7" ReadOnly="True" Enabled="False" Font-Bold="True"></asp:TextBox><em><span
                                                        style="color: Red; vertical-align: super;"></span></em></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Client ID:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_ClientId" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7" ReadOnly="True" Enabled="False" Font-Bold="True"></asp:TextBox><em><span
                                                        style="color: Red; vertical-align: super;"></span></em></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Remark:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Remark" runat="server" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7" TextMode="MultiLine" Width="200" Rows="4"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 2%;">&nbsp;</td>
                                <td style="width: 49%;">
                                    <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tr align="left">
                                            <td>Quantity:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Qty" runat="server" CssClass="TextBoxCSS" MaxLength="50" TabIndex="7"
                                                    ReadOnly="True"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                                        </tr>
                                        <tr align="left">
                                            <td class="LabelCSS">Delivery Date:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_DelDate" runat="server" CssClass="TextBoxCSS" TabIndex="13"></asp:TextBox><img
                                                    id="IMG2" border="0" class="calender" onclick="displayDatePicker('txt_DelDate',0);"
                                                    src="../Images/Calender.jpg" /><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                                        </tr>
                                        <tr align="left">
                                            <td>DMAT Account To:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_DematAccTo" runat="server" CssClass="ComboBoxCSS" AutoPostBack="True">
                                                    <asp:ListItem Value="C">Current User</asp:ListItem>
                                                    <asp:ListItem Value="O">Others</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Customer Slip No:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_CustSlipNo" runat="server" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Customer DP Name:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_CustDPName" runat="server" CssClass="ComboBoxCSS" AutoPostBack="True"
                                                    Enabled="False" Font-Bold="True">
                                                </asp:DropDownList><em><span style="color: Red; vertical-align: super;">*</span></em>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Customer DP Id:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_CustDPId" runat="server" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7" ReadOnly="True" Enabled="False" Font-Bold="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Customer Client Id:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_CustClientId" runat="server" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7" ReadOnly="True" Enabled="False" Font-Bold="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left" id="row_SettleNo" runat="server" visible="false">
                                            <td>Settle No:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SettleNo" runat="server" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr align="center" id="row_Save" runat="server">
                                <td colspan="3">
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS hidden" />
                                    <asp:Button ID="btn_Update" runat="server" Text="Update" ToolTip="Update" CssClass="ButtonCSS hidden" />
                                    <input type="button" id="btn_Ret" runat="server" value="Save Info" class="ButtonCSS" onclick="return Validation();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                        <asp:HiddenField ID="Hid_DematInfoId" runat="server" />
                        <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                        <asp:HiddenField ID="Hid_DmatId" runat="server" />
                        <asp:HiddenField ID="Hid_CustDpId" runat="server" />
                        <asp:HiddenField ID="Hid_FaceValue" runat="server" />
                        <asp:HiddenField ID="Hid_bond" runat="server" />
                        <asp:HiddenField ID="Hid_FVS" runat="server" />
                        <asp:HiddenField ID="Hid_currFV" runat="server" />
                        <asp:HiddenField ID="Hid_BalAmt" runat="server" />
                        <asp:HiddenField ID="Hid_DematAccTo" runat="server" />
                        <asp:HiddenField ID="Hid_PayMode" runat="server" />
                        <asp:HiddenField ID="Hid_Id" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
