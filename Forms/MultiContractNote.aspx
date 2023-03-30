<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="MultiContractNote.aspx.cs" Inherits="Forms_MultiContractNote" Title="Multi Contract Note" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <table id="Table1" width="760px" align="center" class="formTable" cellspacing="0"
        cellpadding="0" border="0">
        <tr>
            <td colspan="6" class="HeaderCSS" id="Col_Headers" runat="server">Report Selection
            </td>
        </tr>
        <tr>
            <td class="frmbgcolor">
                <table width="760px" cellpadding="0" cellspacing="0" align="center" border="0" id="TABLE2">
                    <tr id="row_FromDate" runat="server">
                        <td class="LabelCSS" align="right">Broking Deal No.:
                        </td>
                        <td align="left">
                            <%-- <uc:Search ID="srh_TransCode" runat="server" AutoPostback="true" ProcName="ID_SEARCH_WDMPrintDeal"
                                SelectedFieldName="WDMDealNumber" SourceType="StoredProcedure" TableName="WDMEntry"
                                CheckYearCompany="true" ConditionExist="true" ConditionalFieldName="" ConditionalFieldId=""
                                PageName="PrintWDMDeals" FormHeight="550" FormWidth="650" Width="200" OnButtonClick="srh_TransCodeClick">
                            </uc:Search>--%>
                            <uc:Search ID="srh_TransCode" runat="server" PageName="WDMTransCode" AutoPostback="true"
                                SelectedFieldId="Id" SelectedFieldName="WDMDealNumber" OnButtonClick="srh_TransCodeClick"/>
                        </td>
                    </tr>
                    <tr id="row_buysell" runat="server" visible="true">
                        <td class="LabelCSS" align="right">Contract Note For:
                        </td>
                        <td align="left">
                            <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_ContNotefor" runat="server" CellPadding="0"
                                CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" TabIndex="1"
                                AutoPostBack="True">
                                <asp:ListItem Selected="True" Value="B">Buyer</asp:ListItem>
                                <asp:ListItem Value="S">Seller</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_Print" runat="server" Text="View Report" CssClass="ButtonCSS"
                                TabIndex="19" Width="100px" OnClick="btn_Print_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_ByeLaws" runat="server" Text="View Byelaws" CssClass="ButtonCSS"
                                TabIndex="19" Width="100px" OnClick="btn_Printbackside_Click" />
                        </td>
                    </tr>

                </table>
                <asp:HiddenField ID="Hid_ReportType" runat="server" />
                <div>
                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                        BackColor="white" />
                </div>
</asp:Content>
