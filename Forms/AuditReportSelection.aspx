<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="AuditReportSelection.aspx.vb" Inherits="Forms_AuditReportSelection"
    Title="AuditReportSelection" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>

    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            BindDate();
            var rptName = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value
        });


        function HidRow() {

            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "S") {
                document.getElementById("Row_dealslipno").style.display = "none";
                document.getElementById("row_customer").style.display = "none";
                document.getElementById("row_security").style.display = "";
                document.getElementById("tr_WDMdeal").style.display = "none";
                document.getElementById("tr_Trading").style.display = "none";


            }
            else if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "C") {
                document.getElementById("Row_dealslipno").style.display = "none";
                document.getElementById("row_customer").style.display = "";
                document.getElementById("row_security").style.display = "none";
                document.getElementById("tr_WDMdeal").style.display = "none";
                document.getElementById("tr_Trading").style.display = "none";
            }
            else if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "D") {
                document.getElementById("Row_dealslipno").style.display = "";
                document.getElementById("row_customer").style.display = "none";
                document.getElementById("row_security").style.display = "none";
                document.getElementById("tr_WDMdeal").style.display = "none";
                document.getElementById("tr_Trading").style.display = "none";

            }
            else if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "S") {
                document.getElementById("Row_dealslipno").style.display = "none";
                document.getElementById("row_customer").style.display = "none";
                document.getElementById("row_security").style.display = "";
                document.getElementById("tr_WDMdeal").style.display = "none";
                document.getElementById("tr_Trading").style.display = "none";
            }

            else if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "W") {
                document.getElementById("Row_dealslipno").style.display = "none";
                document.getElementById("row_customer").style.display = "none";
                document.getElementById("row_security").style.display = "none";
                document.getElementById("tr_WDMdeal").style.display = "";
                document.getElementById("tr_Trading").style.display = "none";

            }
            else if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "T") {
                document.getElementById("Row_dealslipno").style.display = "none";
                document.getElementById("row_customer").style.display = "none";
                document.getElementById("row_security").style.display = "none";
                document.getElementById("tr_WDMdeal").style.display = "none";
                document.getElementById("tr_Trading").style.display = "";
            }
            else if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "") {
                document.getElementById("Row_dealslipno").style.display = "none";
                document.getElementById("row_customer").style.display = "none";
                document.getElementById("row_security").style.display = "none";
                document.getElementById("tr_WDMdeal").style.display = "none";
                document.getElementById("tr_Trading").style.display = "none";
            }


        }

        function Validation() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "") {
                AlertMessage('Validation', 'Please Select Report type', 175, 450)
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "C") {
                if ((document.getElementById("ctl00_ContentPlaceHolder1_Srh_Customer_txt_Name").value) == "") {
                    AlertMessage('Validation', 'Please Select Customer name', 175, 450)
                    return false;
                }
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "S") {
                if ((document.getElementById("ctl00_ContentPlaceHolder1_Srh_security_txt_Name").value) == "") {
                    AlertMessage('Validation', 'Please Select Security name', 175, 450)
                    return false;
                }
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_RetailWDM").value == "D") {
                if ((document.getElementById("ctl00_ContentPlaceHolder1_Srh_DealSlipNo_txt_Name").value) == "") {
                    AlertMessage('Validation', 'Please Select Deal No', 175, 450)
                    return false;
                }
            }
        }

        function BindDate() {
            $('.jsdate').datepicker({
                showOn: "button",
                buttonImage: "../Images/calendar.gif",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                buttonText: 'Select date as (dd/mm/yyyy)'
            });
        }

        function CallOnNoRecords() {
            AlertMessage("Validation", "Sorry!!! No Records available to show report.", 175, 450);
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table width="100%" align="center" class="formTable" cellspacing="0" cellpadding="0"
                border="0">
                <tr align="left">
                    <td class="SectionHeaderCSS" id="Col_Headers" runat="server">Audit Report Selection
                    </td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="40%">
                            <tr align="left" id="row_FromDate" runat="server">
                                <td align="right">From Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS jsdate" Width="115px"
                                        TabIndex="1"></asp:TextBox>
                                    <%--<img class="calender" id="fromCalendar" src="../Images/Calender.jpg" />--%>
                                </td>
                            </tr>
                            <tr align="left" id="row_ToDate" runat="server">
                                <td align="right">To Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS jsdate" Width="115px" TabIndex="2"></asp:TextBox>
                                    <%--<img class="calender" id="toCalendar" src="../Images/Calender.jpg" />--%>
                                </td>
                            </tr>
                            <tr align="left" id="row_RetailWDM" runat="server">
                                <td id="Td1" runat="server" align="right">Report:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cbo_RetailWDM" runat="server" CssClass="ComboBoxCSS" Width="208px"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="C">Customer Master</asp:ListItem>
                                        <asp:ListItem Value="S">Security Master</asp:ListItem>
                                        <asp:ListItem Value="D">DealSlipentry</asp:ListItem>
                                        <%--<asp:ListItem Value="W">WDMEntry</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr align="left" id="Row_dealslipno">
                                <td align="right">DealSlip No:
                                </td>
                                <td style="padding-left: 0px;">
                                    <uc:Search ID="Srh_DealSlipNo" runat="server" PageName="DealSlipNoAudit" AutoPostback="true"
                                        SelectedFieldId="Id" SelectedFieldName="DealSlipNo" />
                                </td>
                            </tr>
                            <tr align="left" id="row_customer">
                                <td align="right">Customer:
                                </td>
                                <td style="padding-left: 0px;">
                                    <uc:Search ID="Srh_Customer" runat="server" PageName="CustomerNameAudit" AutoPostback="true"
                                        SelectedFieldId="Id" SelectedFieldName="CustomerName" />
                                </td>
                            </tr>
                            <tr align="left" id="row_security">
                                <td align="right">Security:
                                </td>
                                <td style="padding-left: 0px;">
                                    <uc:Search ID="Srh_security" runat="server" PageName="SecurityNameAudit" AutoPostback="true"
                                        SelectedFieldId="Id" SelectedFieldName="SecurityName" />
                                </td>
                            </tr>
                            <tr align="left" id="tr_WDMdeal">
                                <td align="right">Broking Deal No.:
                                </td>
                                <td style="padding-left: 0px;">
                                    <uc:Search ID="srh_TransCode" runat="server" PageName="WDM_TransCodeAudit" AutoPostback="true"
                                        SelectedFieldId="Id" SelectedFieldName="WDMDealNumber" />
                                </td>
                            </tr>
                            <tr align="left" id="tr_Trading">
                                <td align="right">Trading Deal No.:
                                </td>
                                <td style="padding-left: 0px;">
                                    <%--  <uc:Search ID="srh_WDMTransCode" runat="server" AutoPostback="true" ProcName="ID_SEARCH_WDMPrintDeals"
                                        SelectedFieldName="DealSlipNo" SourceType="StoredProcedure" TableName="DealSlipEntry"
                                        CheckYearCompany="true" ConditionExist="true" ConditionalFieldName="" ConditionalFieldId=""
                                        FormHeight="500" FormWidth="500" Width="200"></uc:Search>--%>
                                    <uc:Search ID="srh_WDMTransCode" runat="server" PageName="WDM_WDMTransCode" AutoPostback="true"
                                        SelectedFieldId="Id" SelectedFieldName="DealSlipNo" />
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2"></td>
                            </tr>
                            <tr align="left">
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:HiddenField ID="Hid_bPopUp" runat="server" />
                                    <asp:HiddenField ID="Hid_ReportType" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table>
        <tr>
            <td>
                <asp:Button ID="btn_Print" runat="server" Text="View Report" CssClass="ButtonCSS"
                    TabIndex="19" Width="90px" />
            </td>
        </tr>
    </table>
</asp:Content>
