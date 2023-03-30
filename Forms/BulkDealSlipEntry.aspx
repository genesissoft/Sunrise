<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="BulkDealSlipEntry.aspx.vb" Inherits="Forms_BulkDealSlipEntry" Title="Bulk Deal Slip Entry" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/BulkEntry.js"></script>

    <script language="javascript" type="text/javascript">
        function OpenModDialog() {

            var OpenUrl = "AddBulk.aspx";
            var ele1 = document.getElementById("ctl00_ContentPlaceHolder1_row_Per");
            var perc = 0;

            document.getElementById("ctl00_ContentPlaceHolder1_Hid_Radio_opt").value = document.getElementById("ctl00_ContentPlaceHolder1_rdo_BrokPerc_0").checked;

            if (document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_txt_Name").value == "") {
                AlertMessage("Validation", "Please Enter Name Of Security.", 175, 450);
                return false;
            }

            if (!(ele1 == null)) {

                var visb = ele1.style.display;
                if (visb == "block") {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_txt_BrokPerc").value == "") {
                        AlertMessage("Validation", "Please Enter Percent Value", 175, 450);
                        return false;
                    }
                    else {
                        var perc = document.getElementById("ctl00_ContentPlaceHolder1_txt_BrokPerc").value;
                    }
                }
            }

            var Hid_NSDLVAL = document.getElementById("ctl00_ContentPlaceHolder1_Hid_NSDLFaceValue").value
            var OpenUrl = "AddBulk.aspx?bAdd='A'&Values=''&percent=" + perc + "&Hid_NSDLFaceValue=" + Hid_NSDLVAL + "&str=''";

            var ret = window.showModalDialog(OpenUrl, 'some argument', 'dialogWidth:600px;dialogHeight:400px;center:1;status:0;resizable:1;');
            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetVal").value = ret;
                document.getElementById('<%= btn_addDet.ClientID%>').click();
            }
        }


        function ValidationForSave() {


            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType").value) == "") {
                AlertMessage("Validation", "Please Select Security Type", 175, 450);
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_IssuerOfSecurity_txt_Name").value) == "") {
                AlertMessage("Validation", "Please Select Issuer Of Security", 175, 450);
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_txt_Name").value) == "") {
                AlertMessage("Validation", "Please Select Name of Security", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_DealDate").value) == "") {
                AlertMessage("Validation", "Please Deal Date ", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_SettmentDate").value) == "") {
                AlertMessage("Validation", "Please Settment Date", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value) == "") {
                AlertMessage("Validation", "Please Enter Rate ", 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Rate").value) == 0) {
                AlertMessage("Validation", "Rate Can not be Zero", 175, 450);
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_PhysicalDMAT_0").checked == true) {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_Bank").value) == "") {
                    AlertMessage("Validation", "Please Enter Our Bank", 175, 450);
                    return false;
                }
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_Demat").value) == "") {
                    AlertMessage("Validation", "Please Enter Our Demat Bank", 175, 450);
                    return false;
                }
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealerName").value) == "") {
                AlertMessage("Validation", "Please Select Seller Dealer Name", 175, 450);
                return false;
            }

            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_Selected")
            if (grd == null) {
                AlertMessage("Validation", "Please Add Details", 175, 450);
                return false;
            }
            else if (grd.rows.length <= 0) {
                AlertMessage("Validation", "Please Add Details", 175, 450);
                return false;
            }

            //cbo_ReportedBy 
            return true;
        }

        function CalcTotal() {
            var totFaceValue = 0
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_Selected")
            for (i = 1; i < grd.rows.length - 1; i++) {
                var col = grd.children[0].children[i].children[2]
                var faceValue = (col.children[0].value - 0)
                var faceMultiple = (col.children[1].value - 0)
                totFaceValue = totFaceValue + (faceValue * faceMultiple)
            }
            document.getElementById("fnt_TotalAmt").innerText = totFaceValue
        }



        function ShowSecurityMaster() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameofSecurity_txt_Name").value) == "") {
                AlertMessage("Validation", "Please Select Name of Security", 175, 450);
                return false;
            }
            var strpagename = "DealSlipEntry.aspx";
            var Id = document.getElementById("ctl00_ContentPlaceHolder1_Hid_SecId").value;
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
            var HideMenu = "HideMenu"
            PageName = PageName + "?Id=" + Id + "&Flag=C" + "&HideMenu=" + HideMenu
            windowprops = "height=" + h + ",width=" + w + ",top=" + wint + ",left=" + winl + ",location=no,"
            + "scrollbars=yes,menubar=yes,toolbar=yes,resizable=yes,status=yes";
            window.open(PageName, "Popup", windowprops);
        }


    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Bulk Deal Slip Entry</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td align="center" valign="top" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table id="Table3" cellspacing="0" cellpadding="0" border="0" width="90%">
                            <tr align="center" valign="top">
                                <td style="width: 48%;">
                                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tr align="left">
                                            <td>Type OF Transaction:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rbl_TypeOFTranction" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Selected="True" Value="P">To Purchase</asp:ListItem>
                                                    <asp:ListItem Value="S">To Sell</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Deal Trans Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_DealTransType" runat="server" CssClass="ComboBoxCSS" Width="188px"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Text="Trading" Value="T" Selected="True"></asp:ListItem>
                                                    <%--<asp:ListItem Text="Broking" Value="B"></asp:ListItem>--%>
                                                    <%--   <asp:ListItem Text="Financial" Value="F"></asp:ListItem>--%>
                                                    <asp:ListItem Text="Distribution" Value="D"></asp:ListItem>
                                                    <asp:ListItem Text="Financial" Value="F"></asp:ListItem>
                                                    <asp:ListItem Text="MB Trading" Value="M"></asp:ListItem>
                                                    <asp:ListItem Text="MB Distribution" Value="O"></asp:ListItem>
                                                </asp:DropDownList><i style="color: red"> *</i>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_FinDealType" style="display: none">
                                            <td>Financial Deal Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_FinancialDealType" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Selected="True" Value="N">Normal</asp:ListItem>
                                                    <asp:ListItem Value="F" Enabled="false"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>SecurityType:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SecurityType" runat="server" Width="188px" CssClass="ComboBoxCSS"
                                                    AutoPostBack="True">
                                                </asp:DropDownList><i style="color: Red; vertical-align: super;"> *</i>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Issuer Of Security:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:Search ID="srh_IssuerOfSecurity" Width="175" runat="server" AutoPostback="true" SelectedFieldId="Id" SelectedFieldName="SecurityIssuer"
                                                    PageName="NameOfIssuer">
                                                </uc:Search>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Name of Security:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:Search ID="Srh_NameofSecurity" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="SecurityName" />
                                                <asp:Label ID="lbl_IPDates" runat="server" Text="" CssClass="LabelCSS"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_BrokingBTB" style="display: none">
                                            <td>Select Broking pur No:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:Search ID="srh_BrokingBTBDealSlipNo" runat="server" AutoPostback="true" ProcName="ID_SEARCH_BrokingBTBDealSlipNo"
                                                    CheckYearCompany="false" CheckCompany="true" SelectedFieldName="DealSlipNo" SourceType="StoredProcedure"
                                                    TableName="SecurityMaster" ConditionExist="true" FormHeight="380" ConditionalFieldName="SM.SecurityId"
                                                    FormWidth="800" Width="180" ConditionalFieldId="Srh_NameofSecurity"></uc:Search>
                                                <asp:Button ID="btn_ShowBrokPurdeal" runat="server" CssClass="ButtonCSS" Visible="false"
                                                    Text="Show" />
                                            </td>
                                        </tr>
                                        <tr align="left" style="display: none">
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_Brokerage1" Text="Brokerage Entry" runat="server" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Brokerage Paid To:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_BrokeragePaidTo" runat="server" CssClass="ComboBoxCSS"
                                                    Width="188px">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Select Brokerage Option:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Table" ID="rdo_BrokPerc" runat="server" CellPadding="0"
                                                    RepeatColumns="2" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS"
                                                    BorderWidth="0">
                                                    <asp:ListItem Value="M" Selected="True">Manual</asp:ListItem>
                                                    <asp:ListItem Value="F">Percentage</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Per" runat="server" style="display: block;">
                                            <td>Percentage:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_BrokPerc" Width="50px" runat="server" CssClass="TextBoxCSS"></asp:TextBox><asp:Label
                                                    ID="lbl_perc" runat="server" Text="%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Select Brokerage Paid Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_brok_paid_type" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" TextAlign="right">
                                                    <asp:ListItem Value="P">Paid</asp:ListItem>
                                                    <asp:ListItem Value="R" Selected="True">Received</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 4%;"></td>
                                <td style="width: 48%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                        <tr align="left">
                                            <td>Deal Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_DealDate" Width="115px" runat="server" CssClass="TextBoxCSS"></asp:TextBox><img
                                                    class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_DealDate',this);"
                                                    id="IMG1" runat="server">
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Sett Date T +:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SettDay" AutoPostBack="true" Width="40px" runat="server"
                                                    CssClass="ComboBoxCSS" TabIndex="1">
                                                    <asp:ListItem Value="0" Selected="True">0</asp:ListItem>
                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                    <asp:ListItem Value="3">3</asp:ListItem>
                                                    <asp:ListItem Value="4">4</asp:ListItem>
                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                    <asp:ListItem Value="6">6</asp:ListItem>
                                                    <asp:ListItem Value="7">7</asp:ListItem>
                                                    <asp:ListItem Value="8">8</asp:ListItem>
                                                    <asp:ListItem Value="9">9</asp:ListItem>
                                                </asp:DropDownList>Sett Date:
                                                <asp:TextBox ID="txt_SettmentDate" Width="75px" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left" runat="server" visible="false">
                                            <td>Select Option:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_SelectOpt" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="B">No of Bonds</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="F">Face Value</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" runat="server" visible="true">
                                            <td>Face Value:
                                            </td>
                                            <td style="padding: 0px;">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr align="left">
                                                        <td>
                                                            <asp:TextBox ID="txt_Amount" Width="75px" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cbo_Amount" runat="server" CssClass="ComboBoxCSS" Width="100px">
                                                                <asp:ListItem Text="THOUSANDS" Value="1000"></asp:ListItem>
                                                                <asp:ListItem Text="LACS" Value="100000" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="CRORES" Value="10000000"></asp:ListItem>
                                                            </asp:DropDownList><i style="color: Red; vertical-align: super;">*</i>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Rate:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Rate" runat="server" Width="180px" onkeypress="javascript:OnlyDecimal();"
                                                    CssClass="TextBoxCSS"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                                <asp:Button ID="btn_CalRate" runat="server" Text="Calc Rate" ToolTip="Calculate Rate"
                                                    CssClass="ButtonCSS hidden" />
                                                <input type="button" id="btn_CalRate1" runat="server" value="Calc Rate" class="ButtonCSS hidden" onclick="return ShowYieldCalculation();" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Mode of Delivery:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList ID="rdo_PhysicalDMAT" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS" TextAlign="right">
                                                    <asp:ListItem Value="D" Selected="True">DMAT</asp:ListItem>
                                                    <asp:ListItem Value="S">SGL</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:RadioButtonList ID="rdo_AccIntDays" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS" TextAlign="right">
                                                    <asp:ListItem Value="3" Selected="True">365</asp:ListItem>
                                                    <asp:ListItem Value="2">366</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_CustSGL" style="display: none">
                                            <td>Customer SGL With:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_CustSGL" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_CounterCustSGLWith" style="display: none">
                                            <td>Counter Cust SGL With:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_CounterCustSGLWith" runat="server" CssClass="ComboBoxCSS"
                                                    Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Mode OF Payment:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cbo_ModeOfPayment" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                    <asp:ListItem Value="H">HIGH VALUE CHEQUE</asp:ListItem>
                                                    <asp:ListItem Value="T">TRANSFER CHEQUE</asp:ListItem>
                                                    <asp:ListItem Value="C">NORMAL CHEQUE</asp:ListItem>
                                                    <asp:ListItem Value="R">RTGS</asp:ListItem>
                                                    <asp:ListItem Value="E">NEFT</asp:ListItem>
                                                    <asp:ListItem Value="S">SGL</asp:ListItem>
                                                    <asp:ListItem Value="N">RTGS-NSCCL-Settlement</asp:ListItem>
                                                    <asp:ListItem Value="B">RTGS-BSE-ICCL-Settlement </asp:ListItem>
                                                    <asp:ListItem Value="L">RTGS-BSE-Settelement</asp:ListItem>
                                                </asp:DropDownList>&nbsp;
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Bank" runat="server">
                                            <td>Our Bank:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_Bank" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_SGL" runat="server" style="display: none">
                                            <td>Our SGL With:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SGLWith" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_Demat" runat="server">
                                            <td>Our Demat:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_Demat" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td visible="false">Seller Dealer Name:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_DealerName" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_Dealar" Visible="false" runat="server" Width="0" CssClass="LabelCSS"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Company Name:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_Company" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList><i style="color: Red; vertical-align: super;"> *</i>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td id="Td1">Reported By:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_ReportedBy" runat="server" CssClass="ComboBoxCSS" Width="188px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_PreviousdealType" runat="server" visible="false">
                                            <td>Previous deal Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_PreviousdealType" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_calcXInt" runat="server" visible="false">
                                            <td>calculate the Ex-Interest:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_calcXInt" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="True">
                                                    <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_refrenceBy" style="display: none;">
                                            <td>Reference By:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:Search ID="Srh_ReferenceBy" runat="server" PageName="NameOFClient" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="CustomerName" />
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_refrenceByDealer" style="display: none;">
                                            <td visible="false">Reference By Dealer:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_ReferenceByDealer" runat="server" CssClass="ComboBoxCSS"
                                                    Width="188px">
                                                </asp:DropDownList><asp:Label ID="Label1" Visible="false" runat="server" Width="0"
                                                    CssClass="LabelCSS"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr align="left" id="row_addmultipleFinancialtextbox" style="display: none">
                                <td class="LabelCSS" colspan="2"></td>
                                <td>
                                    <asp:ListBox ID="lst_addmultipleFinancial" runat="server" Width="148px" Height="45px"
                                        CssClass="fieldcontent" DataTextField="DealSlipNo" DataValueField="DealSlipID"
                                        TabIndex="11"></asp:ListBox>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr align="center">
                                <td class="HeadingCenter" colspan="3">Details Section</td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Button ID="btn_addDet" Width="75px" runat="server" Text="Add Detail"
                                        CssClass="ButtonCSS hidden"></asp:Button>
                                    <input type="button" id="btn_addDet1" runat="server" value="Add Detail" onclick="OpenModDialog();" class="ButtonCSS" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <div id="div1" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative;">
                                        <asp:GridView ID="dg_Selected" runat="server" AutoGenerateColumns="false" CssClass="GridCSS"
                                            TabIndex="26" ShowHeader="true" Width="100%">
                                            <Columns>
                                                <asp:TemplateField Visible="true">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="EditRow" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                            CommandName="DeleteRow" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="SrNo" DataField="SrNo" Visible="true" />
                                                <asp:BoundField HeaderText="CustomerId" DataField="CustomerId" Visible="false" />
                                                <asp:BoundField HeaderText="Customer" DataField="Customer" ItemStyle-Width="300px" />
                                                <asp:BoundField HeaderText="FaceValue" DataField="FaceValue" ItemStyle-Width="150px" />
                                                <asp:BoundField HeaderText="BrokerageAmt" DataField="BrokerageAmt" ItemStyle-Width="150px" />
                                                <asp:BoundField HeaderText="FaceValMul" DataField="FaceVal_WMult" Visible="false" />
                                                <asp:BoundField HeaderText="NoOfBonds" DataField="NoOfBond" Visible="true" />
                                            </Columns>
                                            <RowStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">Total Face Value =
                                    <asp:Label ID="fnt_TotalAmt" Text="" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">Total Count =
                                    <asp:Label ID="fnt_TotalCnt" Text="" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_ShowSecurity" runat="server" Text="Show Security" Width="90px"
                                        ToolTip="Show Security" CssClass="ButtonCSS" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:HiddenField ID="Hid_CompId" runat="server" />
                                    <asp:HiddenField ID="Hid_FirstInterestDate" runat="server" />
                                    <asp:HiddenField ID="Hid_CouponRate" runat="server" />
                                    <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                                    <asp:HiddenField ID="Hid_DealerName" runat="server" />
                                    <asp:HiddenField ID="Hid_NSDLFaceValue" runat="server" />
                                    <asp:HiddenField ID="Hid_NoOfBond" runat="server" />
                                    <asp:HiddenField ID="Hid_QuoteId" runat="server" />
                                    <asp:HiddenField ID="Hid_bond" runat="server" />
                                    <asp:HiddenField ID="Hid_MatDate" runat="server" />
                                    <asp:HiddenField ID="Hid_MatAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_CoupDate" runat="server" />
                                    <asp:HiddenField ID="Hid_CoupRate" runat="server" />
                                    <asp:HiddenField ID="Hid_InterestDate" runat="server" />
                                    <asp:HiddenField ID="Hid_BookClosureDate" runat="server" />
                                    <asp:HiddenField ID="Hid_GovernmentFlag" runat="server" />
                                    <asp:HiddenField ID="Hid_Issue" runat="server" />
                                    <asp:HiddenField ID="Hid_DMATBkDate" runat="server" />
                                    <asp:HiddenField ID="Hid_Frequency" runat="server" />
                                    <asp:HiddenField ID="Hid_Amtshow" runat="server" />
                                    <asp:HiddenField ID="Hid_ShowInterest" runat="server" />
                                    <asp:HiddenField ID="Hid_IntDays" runat="server" />
                                    <asp:HiddenField ID="Hid_SettlementAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_FinlSettlementAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_Quantity" runat="server" />
                                    <asp:HiddenField ID="Hid_Amt" runat="server" />
                                    <asp:HiddenField ID="Hid_AddInterest" runat="server" />
                                    <asp:HiddenField ID="Hid_InterestFromTo" runat="server" />
                                    <asp:HiddenField ID="HiddenField5" runat="server" />
                                    <asp:HiddenField ID="Hid_FinalAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_Round" runat="server" />
                                    <asp:HiddenField ID="Hid_CallDate" runat="server" />
                                    <asp:HiddenField ID="Hid_CallAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_PutDate" runat="server" />
                                    <asp:HiddenField ID="Hid_PutAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_FaceValue" runat="server" />
                                    <asp:HiddenField ID="Hid_Days" runat="server" />
                                    <asp:HiddenField ID="Hid_UserId" runat="server" />
                                    <asp:HiddenField ID="Hid_dealdone" runat="server" />
                                    <asp:HiddenField ID="Hid_Page" runat="server" />
                                    <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                                    <asp:HiddenField ID="Hid_LastIPDate" runat="server" />
                                    <asp:HiddenField ID="Hid_FrequencyOfInterest" runat="server" />
                                    <asp:HiddenField ID="Hid_CostMemoNo" runat="server" />
                                    <asp:HiddenField ID="Hid_CostMemoPageName" runat="server" />
                                    <asp:HiddenField ID="Hid_RetValues" runat="server" />
                                    <asp:HiddenField ID="Hid_PurchaseDealSlipId" runat="server" />
                                    <asp:HiddenField ID="Hid_DealPurcId" runat="server" />
                                    <asp:HiddenField ID="Hid_DealSlipNo" runat="server" />
                                    <asp:HiddenField ID="Hid_SingleRemainFV" runat="server" />
                                    <asp:HiddenField ID="Hid_RemainingFaceValue" runat="server" />
                                    <asp:HiddenField ID="Hid_CostMemoFlag" runat="server" />
                                    <asp:HiddenField ID="Hid_CustomerIdbroking" runat="server" />
                                    <asp:HiddenField ID="Hid_SellBrokDealSlipId" runat="server" />
                                    <asp:HiddenField ID="Hid_PurBrokDealSlipId" runat="server" />
                                    <asp:HiddenField ID="Hid_TotalSettlementAmt" runat="server" />
                                    <asp:HiddenField ID="Hid_BTOBid" runat="server" />
                                    <asp:HiddenField ID="Hid_dgselect_index" runat="server" Value="" />
                                    <asp:HiddenField ID="Hid_dgselect_val" runat="server" Value="" />
                                    <asp:HiddenField ID="Hid_Radio_opt" runat="server" Value="" />
                                    <asp:HiddenField ID="Hid_RetVal" runat="server" Value="" />
                                    <asp:HiddenField ID="Hid_Id" runat="server" />
                                    <asp:HiddenField ID="Hid_RowIndex" runat="server" />
                                    <asp:HiddenField ID="Hid_SecId" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
