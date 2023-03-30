<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="QuoteEntry.aspx.vb" Inherits="Forms_QuoteEntry" Title="QuoteEntry" %>

<%@ Register Src="~/UserControls/YieldCalculater.ascx" TagName="YieldCalculater"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/Calendar.js"></script>

    <script language="javascript" type="text/javascript">
        function ShowCustomer(Type) {
            if (Type == "C") {
                document.getElementById("ctl00_ContentPlaceHolder1_row_Cust").style.display = ""
                document.getElementById("ctl00_ContentPlaceHolder1_row_CustCont").style.display = ""
                document.getElementById("ctl00_ContentPlaceHolder1_row_TempCust").style.display = "none"
                document.getElementById("ctl00_ContentPlaceHolder1_row_TempCustCont").style.display = "none"
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_row_Cust").style.display = "none"
                document.getElementById("ctl00_ContentPlaceHolder1_row_CustCont").style.display = "none"
                document.getElementById("ctl00_ContentPlaceHolder1_row_TempCust").style.display = ""
                document.getElementById("ctl00_ContentPlaceHolder1_row_TempCustCont").style.display = ""
            }
        }

        function Refer() {
            var purSellFlag = 'P';
            var Id = document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value;
            //            alert(Id)
            //            alert(document.getElementById("ctl00_ContentPlaceHolder1_Rdo_QuoteFor_0").checked)
            if (document.getElementById("ctl00_ContentPlaceHolder1_Rdo_QuoteFor_0").checked == true) {
                purSellFlag = 'S'
                //                 alert(purSellFlag)
            }
            else {
                purSellFlag = 'P'
            }

            if (Id != "") {
                ShowDialogCRPopUp('CrossReferanceNew.aspx', Id, 800, 400, purSellFlag)
                return false;
            }
            else {
                ShowDialogCRPopUp('CrossReferanceNew.aspx', 0, 800, 400, purSellFlag)
                return false;
            }
        }
        function ShowDialogCRPopUp(PageName, Id, Width, Height, strFlag) {
            var w = Width;
            var h = Height;
            var winl = (screen.width - w) / 2;
            var wint = (screen.height - h) / 2;
            if (winl < 0) winl = 0;
            if (wint < 0) wint = 0;

            PageName = PageName + "?Id=" + Id + "&purSellFlag=" + strFlag
            windowprops = "height=" + h + ",width=" + w + ",top=" + wint + ",left=" + winl + ",location=no,"
            + "scrollbars=yes,menubars=yes,toolbars=yes,resizable=no,status=yes";
            window.open(PageName, "Popup", windowprops);
        }
        function Validation() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType").value == "") {
                AlertMessage('Validation', 'Please select Security Type.', 175, 450);
                return false
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value == "") {
                AlertMessage('Validation', 'Please select the security for yield calculation.', 175, 450);
                return false
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_yld_Calc_txt_Rate").value == "") {
                AlertMessage('Validation', 'Please Enter Rate.', 175, 450);
                return false
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_yld_Calc_txt_FaceValue").value == "") {
                AlertMessage('Validation', 'Please Enter Face Value.', 175, 450);
                return false
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_CustomerType_0").checked == true) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_srh_NameOFClient_Hid_SelectedId").value == "") {
                    AlertMessage('Validation', 'Please select the Customer.', 175, 450);
                    return false
                }
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_CustomerType_1").checked == true) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomeId").value == "") {
                    AlertMessage('Validation', 'Please select the Customer.', 175, 450);
                    return false
                }
            }

            var cboHr = document.getElementById("ctl00_ContentPlaceHolder1_cbo_hr")
            var currDate = new Date();
            var strValDate = getmdy(document.getElementById("ctl00_ContentPlaceHolder1_txt_ValidTill").value)
            strValDate = strValDate + " " + (document.getElementById("ctl00_ContentPlaceHolder1_cbo_hr").value - 0) + ":"
            strValDate = strValDate + document.getElementById("ctl00_ContentPlaceHolder1_cbo_minute").value + ":00 "
            strValDate = strValDate + document.getElementById("ctl00_ContentPlaceHolder1_cbo_ampm").value

            if (currDate > Date.parse(strValDate)) {
                AlertMessage('Validation', '"Quote valid date and time can not be less then Current Date and time.', 175, 450);
                return false
            }
            return true
        }

        function ShowSecurityMaster() {
            var strpagename = "QuoteEntry.aspx";
            //var Id = document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value;
            var Id = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Id").value;
            ShowSecurityForm("SecurityMaster.aspx", Id, "900px", "680px")
            return false
        }
        function ShowSecurityForm(PageName, Id, Width, Height) {
            var w = Width;
            var h = Height;
            var winl = (screen.width - w) / 2;
            var wint = (screen.height - h) / 2;
            if (winl < 0) winl = 0;
            if (wint < 0) wint = 0;

            PageName = PageName + "?Id=" + Id + "&Flag=C"
            windowprops = "height=" + h + ",width=" + w + ",top=" + wint + ",left=" + winl + ",location=no,"
            + "scrollbars=yes,menubar=yes,toolbar=yes,resizable=yes,status=yes";
            window.open(PageName, "Popup", windowprops);
        }
        function CheckSecurity() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value == "") return false
            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType").value == "") return false
            return true
        }

        function ShowTempCust_Old() {
            var strQuote = "Quote"
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=590px; dialogTop=230px; dialogHeight=600px; Help=No; Status=No; Resizable=Yes;"
            var OpenUrl = "SelectTempCustomer.aspx"
            OpenUrl = OpenUrl + "?Quote=" + strQuote;
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions)
            if (typeof (ret) == "undefined") {
                return false
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomeId").value = ret
                return true
            }
        }

        function ShowTempCust() {
            var strQuote = "Quote"
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=590px; dialogTop=230px; dialogHeight=600px; Help=No; Status=No; Resizable=Yes;"
            var OpenUrl = "SelectTempCustomer.aspx"
            OpenUrl = OpenUrl + "?Quote=" + strQuote;
            var ret = window.showModalDialog(OpenUrl, 'some argument', 'dialogWidth:600px;dialogHeight:400px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                var arrRetValues3 = ret.split("|");
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomeId").value = arrRetValues3[0];
                document.getElementById("ctl00_ContentPlaceHolder1_txt_TempCustomer").value = arrRetValues3[1];
                document.getElementById("ctl00_ContentPlaceHolder1_txt_TempContact").value = arrRetValues3[2];
            }
            else {
            }
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Quote Entry
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table align="center" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr align="center" valign="top">
                                <td style="width: 40%;">
                                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tr align="left" runat="server" visible="false">
                                            <td>Routing:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList ID="Rdo_Routing" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="R">Routing</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="N">Non Routing</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Quote For:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList ID="Rdo_TransactionType" runat="server" RepeatLayout="Flow"
                                                    RepeatDirection="Horizontal" ToolTip="To display this quote to all branches or head office">
                                                    <asp:ListItem Value="A">All Branches</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="H">Head Office</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Trans Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList ID="Rdo_QuoteFor" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="P">To Purchase</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="S">To Sell</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Date" runat="server" CssClass="TextBoxCSS" TabIndex="9"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Valid Till:
                                            </td>
                                            <td style="padding: 0px;">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txt_ValidTill" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                                TabIndex="9" ToolTip="Enter quote validation date and time"></asp:TextBox><img class="calender"
                                                                    src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ValidTill',this);">
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cbo_hr" runat="server" Width="40px" CssClass="ComboBoxCSS">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cbo_minute" runat="server" Width="40px" CssClass="ComboBoxCSS">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cbo_ampm" runat="server" Width="40px" CssClass="ComboBoxCSS">
                                                                <asp:ListItem Value="AM">AM</asp:ListItem>
                                                                <asp:ListItem Value="PM">PM</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Ip Dates:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_IpDates" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Security Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SecurityType" runat="server" Width="208px" CssClass="ComboBoxCSS"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Issuer Name:
                                            </td>
                                            <td style="padding: 0px;">
                                                <uc:Search ID="srh_IssuerOfSecurity" Width="175" runat="server" AutoPostback="true" SelectedFieldId="Id" SelectedFieldName="SecurityIssuer"
                                                    PageName="NameOfIssuer">
                                                </uc:Search>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td nowrap="nowrap">Name Of Security:
                                            </td>
                                            <td style="padding: 0px;">
                                                <table style="padding: 0px;">
                                                    <tr>
                                                        <td style="padding: 0px;">
                                                            <uc:Search ID="srh_NameofSecurity" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                                                SelectedFieldId="Id" SelectedFieldName="SecurityName" FormWidth ="700" />
                                                        </td>
                                                        <td style="padding: 0px;">
                                                            <asp:Button ID="btn_Refresh" runat="server" Text="Refresh" CssClass="ButtonCSS" Width="50px" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="left" runat="server" visible="false">
                                            <td>Tax Free(Step Up/Step Down):
                                            </td>
                                            <td>
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_TaxFree" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="true">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" style="display: none">
                                            <td>Customer Type:
                                            </td>
                                            <td style="padding: 0px;">
                                                <asp:RadioButtonList ID="rdo_CustomerType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="C">Customer</asp:ListItem>
                                                    <asp:ListItem Value="T">Temp Customer</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Cust" runat="server">
                                            <td>Client Name:
                                            </td>
                                            <td style="padding: 0px;">
                                                <uc:Search ID="srh_NameOFClient" runat="server" PageName="NameOFClient" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="CustomerName" />
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_CustCont" runat="server">
                                            <td>Contact Person:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_ContactPerson" runat="server" Width="208px" CssClass="ComboBoxCSS">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_TempCust" runat="server">
                                            <td>Customer Name:
                                            </td>
                                            <td nowrap="nowrap">
                                                <asp:TextBox ID="txt_TempCustomer" runat="server" CssClass="TextBoxCSS" Width="150px"
                                                    TabIndex="9"></asp:TextBox>
                                                <asp:Button ID="btn_AddTempCustomer" runat="server" CssClass="ButtonCSS"
                                                    Width="60px" Text="TmpCust" />
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_TempCustCont" runat="server">
                                            <td>Contact Person:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_TempContact" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="9"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Dealer:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_DealerName" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="9" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Remark:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Remark" runat="server" CssClass="TextBoxCSS" Width="200px" TabIndex="9"
                                                    TextMode="MultiLine" Rows="4"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 60%; padding: 0px;">
                                    <uc:YieldCalculater ID="yld_Calc" runat="server" ShowCloseButton="false"></uc:YieldCalculater>
                                    <br />
                                    <asp:Button ID="btnShowAccInerest" runat="server" Text="Show Accured Interest" ToolTip="Show Accured Interest"
                                        CssClass="ButtonCSS" Width="145px" />
                                    <br />
                                    <table border="0" style="border-color: Black;" width="100%" id="tblAccInterest" runat="server"
                                        visible="false">
                                        <tr align="left">
                                            <td style="width: 15%;">Amount :
                                            </td>
                                            <td style="width: 30%;">
                                                <asp:Label ID="txt_Amount" runat="server" Width="100px" CssClass=" LabelCSS" MaxLength="20"></asp:Label>
                                            </td>
                                            <td style="width: 25%;">&nbsp;
                                            </td>
                                            <td style="width: 30%;">&nbsp;
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                <font id="row_Interest" runat="server" class="LabelCSS">Interest : </font>
                                            </td>
                                            <td>
                                                <asp:Label ID="txt_AddInterest" runat="server" Width="100px" CssClass=" LabelCSS"
                                                    MaxLength="20"></asp:Label>
                                            </td>
                                            <td align="left">From Date - To Date :
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lbl_AddInterest" runat="server" Height="16px" CssClass=" LabelCSS"
                                                    BorderStyle="Solid" BorderWidth="0px" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Sett. Amount :
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="txt_SettAmt" runat="server" Width="100px" CssClass="LabelCSS" MaxLength="20"></asp:Label>
                                            </td>
                                            <td align="left">Days :
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lbl_SettAmt" runat="server" CssClass="LabelCSS" BorderStyle="Solid"
                                                    BorderWidth="0px" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr align="center">
                                <td class="HeadingCenter" colspan="2">Detail Section
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table align="center" cellspacing="0" cellpadding="0" class="table_border_right_bottom"
                                        width="75%">
                                        <tr class="table_heading" align="center">
                                            <td>Maturity
                                            </td>
                                            <td>Coupon
                                            </td>
                                            <td>Call
                                            </td>
                                            <td>Put
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Literal ID="lit_Maturity" runat="server"></asp:Literal>
                                            </td>
                                            <td align="left">
                                                <asp:Literal ID="lit_Coupon" runat="server"></asp:Literal>
                                            </td>
                                            <td align="left">
                                                <asp:Literal ID="lit_Call" runat="server"></asp:Literal>
                                            </td>
                                            <td align="left">
                                                <asp:Literal ID="lit_Put" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                                    <asp:Button ID="btn_ShowSecurity" runat="server" CssClass="ButtonCSS" Text="Show Security"
                                        Width="110px" />
                                    <asp:Button ID="btn_GenerateDealSlip" runat="server" CssClass="ButtonCSS" Text="Generate Deal Slip"
                                        Width="130px" Visible="false" />
                                    <asp:Button ID="btn_Refer" runat="server" CssClass="ButtonCSS" Text="Refer" Visible="false" />
                                </td>
                            </tr>
                            <asp:HiddenField ID="Hid_ValidDate" runat="server" />
                            <asp:HiddenField ID="Hid_TotalValue" runat="server" />
                            <asp:HiddenField ID="Hid_FaceValue" runat="server" />
                            <asp:HiddenField ID="Hid_TypeFlag" runat="server" />
                            <asp:HiddenField ID="Hid_PageName" runat="server" />
                            <asp:HiddenField ID="Hid_NatureOfInstrument" runat="server" />
                            <asp:HiddenField ID="Hid_QuoteId" runat="server" />
                            <asp:HiddenField ID="Hid_CustomeId" runat="server" />
                            <asp:HiddenField ID="HDAcc_Id" runat="server" />
                            <asp:HiddenField ID="HDAcc_Date" runat="server" />
                            <asp:HiddenField ID="HDAcc_Rate" runat="server" />
                            <asp:HiddenField ID="HDAcc_FaceValue" runat="server" />
                            <asp:HiddenField ID="HDAcc_Multiple" runat="server" />
                            <asp:HiddenField ID="HDAcc_StepUp" runat="server" />
                            <asp:HiddenField ID="HDAcc_ComboFaceValue" runat="server" />
                            <asp:HiddenField ID="HDAcc_RateActual" runat="server" />
                            <asp:HiddenField ID="Hid_IntDays" runat="server" />
                            <asp:HiddenField ID="Hid_Id" runat="server" />
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
