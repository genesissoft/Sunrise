<%@ Page Title="" Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="PrintInvoice.aspx.vb" Inherits="Forms_PrintInvoice" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameOFClientBuy_txt_Name").value) == "") {
                alert("Please Select Broker");
                return false;
            }

        }
    </script>
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td colspan="6" class="HeaderCSS" id="Col_Headers" runat="server">DEBIT EXCEL
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table2" width="45%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr id="row_FromDate" runat="server" align="center" style ="display:none ">
                        <td align="right">From Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS" TabIndex="1"></asp:TextBox>
                            <img class="LabelCSS" height="14" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate',this);"
                                width="16" border="0" style="vertical-align: top; cursor: hand;">
                        </td>
                    </tr>
                    <tr id="row_ToDate" runat="server" style ="display :none ">
                        <td align="right">To Date:
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS" TabIndex="2"></asp:TextBox>
                            <img class="LabelCSS" height="14" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate',this);"
                                width="16" border="0" style="vertical-align: top; cursor: hand;">
                        </td>
                    </tr>
                    <tr id="row_Broker" runat="server">
                        <td id="Client" align="right">Broker Name:
                        </td>
                        <td align="left">
                            <uc:Search ID="srh_BrokerName" runat="server" PageName="BrokerName" AutoPostback="true"
                                SelectedFieldId="Id" SelectedFieldName="BrokerName" ConditionExist="true" FormWidth="600" />
                        </td>
                    </tr>
                    <tr id="Tr1" runat="server">
                        <td align="right">
                            <asp:Label ID="Label3" runat="server" Text="Invoice No.: " CssClass="LabelCSS" Width="100px"></asp:Label>
                        </td>
                        <td align="left" valign="top">
                           <uc:Search ID="Srh_RetailDebitNo" runat="server" PageName="RetailDebitDeals" AutoPostback="true"
                                SelectedFieldId="Id" SelectedFieldName="RefNo" ConditionExist="true" FormWidth="600" 
                               CheckYearCompany ="true" ConditionalFieldName="BrokerId" ConditionalFieldId="srh_BrokerName"/>
                        </td>
                    </tr>
                   
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr align="right">
            <td align="center" colspan="2">
                <asp:Button ID="btn_Excel" runat="server" Text="Excel" CssClass="ButtonCSS" TabIndex="19"
                    Width="100px" visible ="false" />
                <asp:Button ID="btn_PDF" runat="server" Text="Download" CssClass="ButtonCSS" TabIndex="19"
                    Width="100px" />
            </td>
        </tr>
    </table>
    <table width="40%" cellpadding="0" cellspacing="0" align="center" border="0" id="TABLE2">
         <tr>
            <td>
                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                    ToolPanelView="GroupTree" BackColor="white" />
            </td>
        </tr>
        <tr>
            <td align="center" width="80%">
                <asp:HiddenField ID="Hid_Flag" runat="server" />
                <asp:HiddenField ID="Hid_ReportType" runat="server" />
                <asp:HiddenField ID="Hid_BrokerName" runat="server" />
                <asp:HiddenField ID="Hid_BrokerId" runat="server" />
                <asp:HiddenField ID="Hid_Fromdate" runat="server" />
                <asp:HiddenField ID="Hid_Todate" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>

