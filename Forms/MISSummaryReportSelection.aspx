<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="MISSummaryReportSelection.aspx.vb" Inherits="Forms_MISSummaryReportSelection"
    Title="MIS Summary Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery-ui-1.8.23.custom.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.core.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.datepicker.js"></script>

    <script type="text/javascript">
        $(document).ready(function() {


            $(".jsdate").datepicker({
                showOn: "button",
                buttonImage: "../Images/calendar.gif",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                buttonText: 'Select date as (dd/mm/yyyy)'
            });
            var rptName = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value

            if (rptName != "RetailDebitRpt") {
                jQuery("#ctl00_ContentPlaceHolder1_txt_FromDate").attr("onchange", "");
                jQuery("#ctl00_ContentPlaceHolder1_txt_ToDate").attr("onchange", "");
            }



            $("#fromCalendar").click(function() {

                displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate', this);
                if (rptName == "RetailDebitRpt") {
                    FillConditionalValue1('ctl00_ContentPlaceHolder1_srh_RetailDebit', 'ctl00_ContentPlaceHolder1_txt_FromDate');
                }

            });
            $("#toCalendar").click(function() {
                displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate', this);
                if (rptName == "RetailDebitRpt") {
                    FillConditionalValue2('ctl00_ContentPlaceHolder1_srh_RetailDebit', 'ctl00_ContentPlaceHolder1_txt_ToDate');
                }

            });

        });


        function Validation() {
            if ((Trim(document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value) != "TMIS") && (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value) != "ReturnSheet") && (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value) != "MonthlySummary")&&(Trim(document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value) != "TMISSumm")) {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_Company").value) == "") {
                    alert("Please Select Company Name");
                    return false;
                }
            }
        }
    </script>
 <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="SectionHeaderCSS" align="left" colspan="4">
                MAIN SECTION
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table2" align="right" cellspacing="0" cellpadding="0" border="0" width="90%">
                    <tr id="row_MISDetail" runat="server" visible="false">
                        <td align="right">
                            Company Name:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbo_Company" runat="server" CssClass="ComboBoxCSS" Width="188px"
                                Enabled="true" AutoPostBack="false">
                            </asp:DropDownList>
                            <i style="color: Red; vertical-align: super;">*</i>
                        </td>
                    </tr>
                    <tr align="left" id="row_MISDetail1" runat="server" visible="false">
                        <td align="right">
                            Security Type:
                        </td>
                        <td aligh="Left">
                            <asp:RadioButtonList RepeatLayout="Flow" ID="rbl_SecurityType" runat="server" CellPadding="0"
                                CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                <asp:ListItem Selected="True" Value="N">Bonds</asp:ListItem>
                                <asp:ListItem Value="G">G-Sec</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                  
                    <tr align="left" id="row_FromDate" runat="server" visible="false">
                        <td align="right">
                            From Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS jsdate" Width="115px"
                                TabIndex="1"></asp:TextBox>
                             <%--   <img class="calender" id="fromCalendar" src="../Images/Calender.jpg" />--%>
                        </td>
                    </tr>
                    <tr align="left" id="row_ToDate" runat="server" visible="false">
                        <td align="right">
                            To Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS jsdate" Width="115px" TabIndex="2"></asp:TextBox>
                            <%--<img class="calender" id="toCalendar" src="../Images/Calender.jpg" />--%>
                        </td>
                    </tr>
                       <tr align="left" id="row_SummType" runat="server" visible="false">
                        <td align="right">
                            Report Type:
                        </td>
                        <td aligh="Left">
                            <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_SummType" runat="server" CellPadding="0"
                                CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                <asp:ListItem Selected="True" Value="C">Category</asp:ListItem>
                                <asp:ListItem Value="R">Rating</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr align="left">
                        <td align="right">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_Print" runat="server" Text="View Report" CssClass="ButtonCSS"
                                TabIndex="19" Width="90px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="Hid_ReportType" runat="server" />
</asp:Content>
