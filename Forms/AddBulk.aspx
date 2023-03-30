<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddBulk.aspx.vb" Inherits="Forms_AddBulk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Customer</title>
    <base target="_self" />
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/BulkEntry.js"></script>

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
    <script type="text/javascript">
        function CloseFormOk() {
            var ret = Validation();
            if (ret == false) {
                return false;
            } else {
                if (document.getElementById("Srh_NameOFClient_Hid_SelectedId").value == "") {
                    var valu = document.getElementById("Srh_NameOFClient_hid_CustId").value;
                }
                else {
                    var valu = document.getElementById("Srh_NameOFClient_Hid_SelectedId").value;
                }

                valu = valu + "!" + document.getElementById("Srh_NameOFClient_txt_Name").value;
                valu = valu + "!" + document.getElementById("txt_Amount").value;
                valu = valu + "!" + document.getElementById("cbo_Amount").value;
                valu = valu + "!" + document.getElementById("txt_Brok").value;
                valu = valu + "!" + document.getElementById("txt_NoOfBonds").value;
                valu = valu + "!" + document.getElementById("Hid_bFlag").value;
                valu = valu + "!" + document.getElementById("Hid_Index").value;
                document.getElementById("Hid_Id").value = valu;
                window.returnValue = document.getElementById("Hid_Id").value;
                window.close();
            }
        }


        function Validation() {

            if (document.getElementById("Srh_NameOFClient_txt_Name").value == "") {
                AlertMessage("Validation", "Please Enter Customer", 175, 450);
                return false;
            }
            if (document.getElementById("txt_Amount").value == "") {
                AlertMessage("Validation", "Please Enter Face Value", 175, 450);
                return false;
            }
            if (document.getElementById("txt_Amount").value == 0) {
                AlertMessage("Validation", "Face Value Can't be Zero.", 175, 450);
                return false;
            }
            if (document.getElementById("txt_Brok").value == "") {
                AlertMessage("Validation", "Please Enter Brokerage Amount", 175, 450);
                return false;
            }

            var qty = document.getElementById("txt_NoOfBonds").value;
            if ((qty - 0) == 0) {
                AlertMessage("Validation", "The No. Of Bond can not be Zero.", 175, 450);
                return false;
            }
            else if (qty.indexOf(".") >= 0) {
                AlertMessage("Validation", "The No. Of Bond can not be in Decimals.It is " + qty, 175, 450);
                return false;

            }
            return true;
        }


        function ShowCustomerMaster() {
            if (document.getElementById("Srh_NameOFClient_txt_Name").value == "") {
                AlertMessage("Validation", "Please Select Customer", 175, 450);
                return false;
            }
            var strpagename = "AddBulk.aspx";
            var Id = document.getElementById("Hid_CId").value;
            ShowSecurityForm("ClientProfileMaster.aspx", Id, "900px", "680px")
            return false
        }

        function ShowSecurityForm(PageName, Id, Width, Height) {
            var w = Width;
            var h = Height;
            var winl = (screen.width - w) / 2;
            var wint = (screen.height - h) / 2;
            if (winl < 0) winl = 0;
            if (wint < 0) wint = 0;
            var HideMenu = "HideMenu"
            PageName = PageName + "?Id=" + Id + "&Flag=C" + "&HideMenu=" + HideMenu
            windowprops = "height=" + h + ",width=" + w + ",top=" + wint + ",left=" + winl + ",location=no,"
            + "scrollbars=yes,menubar=yes,toolbar=yes,resizable=yes,status=yes";
            window.open(PageName, "Popup", windowprops);
        }

    </script>

    <script language="javascript" type="text/javascript">

        function CloseFormCancel() {
            window.close();
            window.returnValue = "";
            return false;
        }

    </script>

</head>
<body style="margin-left: 0px; margin-top: 5px;" class="popupbackground">
    <form id="form1" runat="server">
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />--%>
        <div>
            <table id="Table2" align="center" cellspacing="0" cellpadding="0" border="0" width="100%"
                class="data_table">
                <tr align="center">
                    <td class="SectionHeaderCSS popupbackground" style="text-align: center;">Add Customer</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="95%">
                            <tr align="left" id="row_BrokCustId">
                                <td id="Seller">Customer Name:
                                </td>
                                <td style="padding-left: 0px;">
                                    <uc:Search ID="Srh_NameOFClient" runat="server" PageName="NameOFClient" AutoPostback="true"
                                        SelectedFieldId="Id" SelectedFieldName="CustomerName" />
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Face Value:
                                </td>
                                <td style="padding: 0px;">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr align="left">
                                            <td>
                                                <asp:TextBox ID="txt_Amount" Width="100px" runat="server" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal();"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_Amount" runat="server" CssClass="ComboBoxCSS" Width="108px">
                                                    <asp:ListItem Text="THOUSANDS" Value="1000"></asp:ListItem>
                                                    <asp:ListItem Text="LACS" Value="100000" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="CRORES" Value="10000000"></asp:ListItem>
                                                </asp:DropDownList><i style="color: Red; vertical-align: super;"> *</i>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr align="left" id="row_NoOfBonds" runat="server" style="display: none">
                                <td>No. Of Bonds:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NoOfBonds" runat="server" Width="100px" Text="0" CssClass="TextBoxCSS"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left" id="row_Brok" runat="server">
                                <td>Brokerage Amount:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Brok" runat="server" Width="100px" Text="0" CssClass="TextBoxCSS"
                                        MaxLength="20" onkeypress="javascript:OnlyDecimal();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>&nbsp;</td>
                                <td>
                                    <input id="btn_Ret" onclick="CloseFormOk();" class="ButtonCSS" type="button" value="Save" />
                                    <input id="btnCancel" onclick="CloseFormCancel();" type="button" value="Cancel" class="ButtonCSS" />
                                    <asp:Button ID="btn_ShowCustomer" runat="server" Text="Show Customer" ToolTip="Show Security"
                                        CssClass="ButtonCSS" Width="100px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="line_separator">
                    <td>
                        <asp:HiddenField ID="hid_pwd" runat="server" />
                        <asp:HiddenField ID="hdn_pwdflag" runat="server" Value="" />
                        <asp:HiddenField ID="hdn_cnt" runat="server" Value="" />
                        <asp:HiddenField ID="hid_SelectedField" runat="server" />
                        <asp:HiddenField ID="hid_SelectedValue" runat="server" />
                        <asp:HiddenField ID="Hid_NSDLFaceValue" runat="server" />
                        <asp:HiddenField ID="hid_percent" runat="server" />
                        <asp:HiddenField ID="hid_CustId" runat="server" />
                        <asp:HiddenField ID="Hid_bond" runat="server" />
                        <asp:HiddenField ID="Hid_bFlag" runat="server" />
                        <asp:HiddenField ID="Hid_Index" runat="server" />
                        <asp:HiddenField ID="Hid_Id" runat="server" />
                        <asp:HiddenField ID="Hid_CId" runat="server" />
                        <%--<asp:HiddenField ID="hid_percent" runat="server" />--%>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
