<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectYear.aspx.vb" Inherits="Forms_SelectYear" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <link href="../include/loginScreen.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style_New.css" type="text/css" rel="stylesheet" />
    <title>Year & Company Selection</title>
</head>
    <script type="text/javascript">
        function Validation() {
            if ((document.getElementById("ctl00_ContentPlaceHolder1_cbo_Company").value) == "") {
                AlertMessage('Validation', 'Please Select Company', 175, 450);
                return false;
            }

          


        }
        

    </script>
<body style="margin-left: auto; margin-right: auto; margin-top: 0; background-color: #def0ff;">
    <form id="form1" runat="server">
        <div id="pageBox" style="background-color:#def0ff;">
            <table cellpadding="0" cellspacing="0" width="975px" style="margin-left: auto; margin-right: auto;
                vertical-align: top; border: 1px solid #969696; background-color: #f3f9ff;">
<%--                <tr style="height: 18px;">
                    <td>
                        <img src="../Images/striptop.PNG" style="width: 100%; height: 18px;" alt="img" />
                    </td>
                </tr>--%>
                <tr>
                    <td align="center">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left" style="width: 50%; padding-left: 15px; padding-top: 15px;">
                               
                                </td>
                                <td align="right" style="width: 50%; padding-right: 5px;" nowrap="nowrap">
                                    <%--  &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1"
                            runat="server" Text="Welcome," CssClass="masterpageformcontent"></asp:Label><asp:Label
                                ID="lbl_user" runat="server" CssClass="masterpageformcontent"></asp:Label>
                        <asp:Label ID="CompanyYear" runat="server" CssClass="masterpageformcontent"></asp:Label>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center" valign="middle" style="width: 100%;">
                        <table border="0" cellpadding="0" width="100%" cellspacing="0" class="data_table">
                            <tr>
                                <td align="center">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 350px;" class="table_border_right_bottom">
                                        <tr class="table_heading LineHight">
                                            <td align="center" colspan="2">
                                                Company &amp; Year Selection</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding: 0px;">
                                                <img src="../Images/gr.jpg" alt="img" width="100%" height="300" />
                                            </td>
                                        </tr>
                                        <tr align="left" class="LineHight" id="row_SelectCompany" runat="server">
                                            <td style="width: 40%;">
                                                Select Company:
                                            </td>
                                            <td style="width: 60%;">
                                                <asp:DropDownList ID="cbo_Company" runat="server" Width="190px" CssClass="ComboBoxCSS"
                                                    AutoPostBack="false">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" class="LineHight">
                                            <td>
                                                Select Year:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SelectYear" runat="server" Width="190px" CssClass="ComboBoxCSS"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left" class="LineHight">
                                            <td>
                                                Start Date:
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_StartDate" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left" class="LineHight">
                                            <td>
                                                End Date:
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_EndDate" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="LineHight">
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btn_Ok" runat="server" CommandName="Login" CssClass="ButtonCSS" Text="Ok"  />&nbsp;
                                                <asp:Button ID="btn_Cancel" runat="server" CommandName="Login" CssClass="ButtonCSS"
                                                    Text="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="Hid_CompId" runat="server" />
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
                        <asp:HiddenField ID="Hid_Amount" runat="server" />
                        <asp:HiddenField ID="Hid_FaceValueMultiple" runat="server" />
                        <asp:HiddenField ID="Hid_Rate" runat="server" />
                        <asp:HiddenField ID="Hid_AllotDate" runat="server" />
                        <asp:HiddenField ID="Hid_FrequencyOfInterest" runat="server" />
                        <asp:HiddenField ID="Hid_DealSlipNo" runat="server" />
                        <asp:HiddenField ID="Hid_PrimaryIssueEntryId" runat="server" />
                        <asp:HiddenField ID="Hid_TotalAllotmentAmt" runat="server" />
                        <asp:HiddenField ID="Hid_AllotmentDate" runat="server" />
                        <asp:HiddenField ID="Hid_IssueId" runat="server" />
                        <asp:HiddenField ID="Hid_FeeAmount" runat="server" />
                    </td>
                </tr>
                <tr style="height: 10px;">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td class="GrayText">
                        Copyright <b>&copy;</b> 2020 Genesis Software
                    </td>
                </tr>
                <tr style="height: 10px; background-color: #def0ff;">
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <%--  <asp:SqlDataSource ID="SqlDataSourceYearMaster" runat="server" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"
            InsertCommand="ID_INSERT_ExchangeMaster" InsertCommandType="StoredProcedure"
            SelectCommand="ID_SELECT_YearMaster" SelectCommandType="StoredProcedure" UpdateCommand="ID_UPDATE_ExchangeMaster"
            UpdateCommandType="StoredProcedure">
            <UpdateParameters>
                <asp:ControlParameter ControlID="Hid_ID" Name="ExchangeId" PropertyName="Value" Type="Int32" />
                <asp:ControlParameter ControlID="txt_ExchangeName" Name="ExchangeName" PropertyName="Text"
                    Size="50" Type="String" />
                <asp:Parameter DefaultValue="0" Direction="Output" Name="Intflag" Type="Int32" />
                <asp:Parameter Direction="Output" Name="strmessage" Size="100" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Direction="Output" Name="RET_CODE" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:ControlParameter ControlID="txt_ExchangeName" Name="ExchangeName" PropertyName="Text"
                    Size="50" Type="String" />
                <asp:Parameter Direction="Output" Name="strmessage" Size="100" Type="String" />
                <asp:Parameter DefaultValue="0" Direction="Output" Name="intFlag" Type="Int32" />
                <asp:Parameter Name="ExchangeId" Type="Int32" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceYearId" runat="server" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"
            InsertCommand="ID_INSERT_ExchangeMaster" InsertCommandType="StoredProcedure"
            SelectCommand="ID_SELECT_YearMaster" SelectCommandType="StoredProcedure" UpdateCommand="ID_UPDATE_ExchangeMaster"
            UpdateCommandType="StoredProcedure">
            <UpdateParameters>
                <asp:ControlParameter ControlID="Hid_ID" Name="ExchangeId" PropertyName="Value" Type="Int32" />
                <asp:ControlParameter ControlID="txt_ExchangeName" Name="ExchangeName" PropertyName="Text"
                    Size="50" Type="String" />
                <asp:Parameter DefaultValue="0" Direction="Output" Name="Intflag" Type="Int32" />
                <asp:Parameter Direction="Output" Name="strmessage" Size="100" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Direction="Output" Name="RET_CODE" Type="Int32" />
                <asp:ControlParameter ControlID="cbo_SelectYear" Name="YearId" PropertyName="SelectedValue"
                    Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:ControlParameter ControlID="txt_ExchangeName" Name="ExchangeName" PropertyName="Text"
                    Size="50" Type="String" />
                <asp:Parameter Direction="Output" Name="strmessage" Size="100" Type="String" />
                <asp:Parameter DefaultValue="0" Direction="Output" Name="intFlag" Type="Int32" />
                <asp:Parameter Name="ExchangeId" Type="Int32" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceCompanyMaster" runat="server" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"
            DeleteCommand="ID_DELETE_CompanyMaster" DeleteCommandType="StoredProcedure" InsertCommand="ID_INSERT_ExchangeMaster"
            InsertCommandType="StoredProcedure" SelectCommand="ID_SELECT_CompanyMaster" SelectCommandType="StoredProcedure"
            UpdateCommand="ID_UPDATE_ExchangeMaster" UpdateCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Direction="Output" Name="RET_CODE" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:ControlParameter ControlID="txt_ExchangeName" Name="ExchangeName" PropertyName="Text"
                    Size="50" Type="String" />
                <asp:Parameter DefaultValue="0" Direction="Output" Name="intflag" Type="Int32" />
                <asp:Parameter Direction="Output" Name="strmessage" Size="100" Type="String" />
                <asp:Parameter DefaultValue="0" Direction="Output" Name="CompId" Type="Int32" />
            </InsertParameters>
            <DeleteParameters>
                <asp:ControlParameter ControlID="Hid_CompId" Name="CompId" PropertyName="Value" Type="Int32" />
                <asp:Parameter Direction="InputOutput" Name="IntFlag" Type="Int32" DefaultValue="0" />
                <asp:Parameter Direction="InputOutput" Name="StrMessage" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:ControlParameter ControlID="txt_ExchangeName" Name="ExchangeName" Size="50"
                    Type="String" />
                <asp:Parameter Direction="InputOutput" Name="IntFlag" Type="Int32" />
                <asp:Parameter Direction="InputOutput" Name="strmessage" Size="100" Type="String" />
                <asp:ControlParameter ControlID="Hid_CompId" Name="ExchangeName" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>--%>
    </form>
</body>
</html>
