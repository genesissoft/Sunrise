<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="MultiDealCon.aspx.cs" Inherits="Forms_MultiDealCon" Title="Multi Deal Confirmation" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/Common.js"></script>

    <%--  <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>--%>
    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="Table1" width="760px" align="center" class="formTable" cellspacing="0"
                cellpadding="0" border="0">
                <tr>
                    <td class="HeaderCSS" id="Col_Headers" runat="server">Report Selection
                    </td>
                </tr>
                <tr>
                    <td class="frmbgcolor">
                        <table width="760px" cellpadding="0" cellspacing="0" align="center" border="0" id="TABLE2">
                            <tr id="row_FromDate" runat="server">
                                <td align="right">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;From Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS jsdate" Width="143px"
                                        TabIndex="1" AutoPostBack="true"></asp:TextBox>
                                    <%-- <img class="LabelCSS" id="fromCalendar" height="14" src="../Images/Calender.jpg"
                                        onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate',this);" width="16"
                                        border="0" style="vertical-align: top; cursor: hand;">--%>
                                </td>
                            </tr>
                            <tr id="row_ToDate" runat="server">
                                <td align="right">To Date:
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS jsdate" Width="143px" TabIndex="2" AutoPostBack="true"></asp:TextBox>
                                    <%-- <img class="LabelCSS" id="toCalendar" height="14" src="../Images/Calender.jpg" width="16"
                                        onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate',this);" border="0"
                                        style="vertical-align: top; cursor: hand;">--%>
                                </td>
                            </tr>


                            <tr align="left" id="tr2" runat="server">
                                <td align="right">
                                    <%-- <asp:Label ID="Label1" runat="server" Text="Select Customer:" CssClass="LabelCSS"></asp:Label>--%>
                            Select Customer:
                                </td>
                                <td align="left">
                                    <uc:SelectFields ID="srh_Customer" runat="server" ProcName="ID_SEARCH_CustomerMasterNew"
                                        FormHeight="475" FormWidth="257" SelectedValueName="CM.CustomerId" ChkLabelName=""
                                        ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerName" SourceType="StoredProcedure" ConditionExist="false"
                                        ConditionalFieldName="" Visible="true" ShowLabel="false">
                                    </uc:SelectFields>
                                </td>
                            </tr>

                            <tr>
                                <td align="right" valign="top">
                                    <table >
                                        <tr id="row_buysell" runat="server" visible="true">
                                            <td align="right">Print Letter Head:
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_LetterHead" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" TabIndex="1"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        <tr id="Tr1" runat="server" visible="true">
                                            <td align="right">Print Deal Ref No:
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DealNoPrint" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" TabIndex="1"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="Tr3" runat="server">
                                            <td align="right">Print Sign Stamp:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_PrintSignStamp" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                         <tr align="left" id="row_PrintYield" runat="server">
                                            <td>Print Yield:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_PrintYield" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" >No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>


                                    </table>

                                </td>

                                <td align="left" valign="top">
                                    <table>
                                        <tr id="Tr4" runat="server" visible="true">
                                            <td align="left">Deal Type:
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_TransType" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" TabIndex="1"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Value="B" Selected="True">Both</asp:ListItem>
                                                    <asp:ListItem Value="P">Purchase Only</asp:ListItem>
                                                    <asp:ListItem Value="S">Sell Only</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>


                                        <tr align="left" id="Tr6" runat="server">
                                            <td align="left">Deal Con Header:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DealconHeader" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        <tr align="left" id="Tr7" runat="server">
                                            <td align="right">Print Signature note:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_PrintsignNote" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_DPDetails" runat="server">
                                            <td>Print DP Details:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DPDetails" runat="server"
                                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        
                                    </table>

                                </td>

                            </tr>

                        </table>
                    </td>
                </tr>

                <tr align="right">
                    <td align="center"></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="Hid_ReportType" runat="server" />
    <table align="center" style="padding-right: 185px;">
        <tr>
            <td>
                <asp:Button ID="btn_Print" runat="server" Text="View Report" CssClass="ButtonCSS"
                    TabIndex="19" Width="100px" OnClick="btn_Print_Click" />
            </td>
        </tr>
    </table>

    <div>
        <CR:CrystalReportViewer ID="DealConfirmation" runat="server" AutoDataBind="true"
            BackColor="white" />
    </div>
</asp:Content>
