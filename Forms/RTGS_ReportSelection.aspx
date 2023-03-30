<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="RTGS_ReportSelection.aspx.vb" Inherits="Forms_RTGS_ReportSelection"
    Title="RTGS ReportSelection" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>

    <script language="javascript" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <script type="text/javascript" language="javascript">

        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_TransCode_txt_Name").value) == "") {
                AlertMessage("Validation", "Please Select DealSlipNo", 175, 450);
                return false;
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_DealSlipType_1").checked == true) {
                if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_DealTransType").value == " ") {

                    AlertMessage("Validation", "Please Select Bank Account Number", 175, 450);
                    return false;

                }
            }



        }
        function Wait() {
            document.getElementById("div_onsaveclick").style.display = "block";
            $('#ctl00_ContentPlaceHolder1_btn_Mail').hide();
        }

    </script>

    <%--  <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>--%>
    <table width="100%" align="center" class="formTable" cellspacing="0" cellpadding="0"
        border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">RTGS Report Selection</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table cellspacing="0" cellpadding="0" border="0" align="center" width="90%">
                    <tr align="center" valign="top">
                        <td>
                            <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                <tr align="left">
                                    <td>Deal Trans Type:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cbo_DealTransType" runat="server" CssClass="ComboBoxCSS" Width="208px"
                                            AutoPostBack="True">
                                            <asp:ListItem Text="Trading" Value="T"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="left" id="tr_DealNo" runat="server">
                                    <td>Deal No.:
                                    </td>
                                    <td style="padding-left: 0px;">
                                        <%-- <uc:Search ID="srh_TransCode" runat="server" AutoPostback="true" ProcName="ID_SEARCH_RetailDebitDeals"
                                            SelectedFieldName="DealSlipNo" SourceType="StoredProcedure" TableName="DealSlipEntry"
                                            CheckYearCompany="true" ConditionExist="true" ConditionalFieldName="DSE.DealTransType"
                                            ConditionalFieldId="cbo_DealTransType" FormWidth="800" Width="200"></uc:Search>--%>
                                        <uc:Search ID="srh_TransCode" runat="server" PageName="RetailDebitDeals" AutoPostback="true"
                                            SelectedFieldId="Id" SelectedFieldName="DealSlipNo" ConditionExist="true" CheckYearCompany="true" ConditionalFieldName="UserId"
                                            ConditionalFieldId="Hid_UserId" ConditionalFieldId1="Hid_UserTypeId" ConditionalFieldName1="UserTypeId" />
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Security:
                                    </td>
                                    <td>
                                        <asp:Literal ID="lit_SecurityName" runat="server">
                                        </asp:Literal>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Customer:
                                    </td>
                                    <td>
                                        <asp:Literal ID="lit_CustName" runat="server">
                                        </asp:Literal>
                                    </td>
                                </tr>
                                <tr align="left" id="row_Ctc" runat="server" visible="false">
                                    <td>Contact person & no:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_CtcDetails" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Kind Attention:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_KindAtten" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Narration:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Narration" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                <tr align="left">
                                    <td>Payment Mode:
                                    </td>
                                    <td>
                                        <asp:Literal ID="txt_PayMode" runat="server">
                                        </asp:Literal>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Deal date:
                                    </td>
                                    <td>
                                        <asp:Literal ID="lit_DealDate" runat="server">
                                        </asp:Literal>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Settlement Date:
                                    </td>
                                    <td>
                                        <asp:Literal ID="lit_SettlementDate" runat="server">
                                        </asp:Literal>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Face Value:
                                    </td>
                                    <td>
                                        <asp:Literal ID="lit_FaceValue" runat="server">
                                        </asp:Literal>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Rate:
                                    </td>
                                    <td>
                                        <asp:Literal ID="lit_Rate" runat="server">
                                        </asp:Literal>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Settlement Amount:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SettlementAmt" runat="server" CssClass="TextBoxCSS" MaxLength="30"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Report Type:
                                    </td>
                                    <td style="padding-left: 0px;">
                                        <asp:RadioButtonList RepeatLayout="Flow" ID="rbl_DealSlipType" runat="server" CellPadding="0"
                                            CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="true">
                                            <asp:ListItem Selected="True" Value="H">HDFC</asp:ListItem>
                                            <asp:ListItem Value="I">ICICI</asp:ListItem>
                                            <asp:ListItem Value="F">Federal</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr align="left" id="row_Acc" runat="server" visible="false">
                                    <td>Account No:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cbo_FedAccountNo" runat="server" CssClass="ComboBoxCSS" Width="208px">
                                        </asp:DropDownList></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="line_separator">
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr align="center" id="Tr_Deal1" runat="server">
                        <td align="right">
                            <asp:Button ID="btn_Print" runat="server" CssClass="ButtonCSS" Text="Print RTGS" />
                            <%--<asp:Button ID="btn_Export" runat="server" CssClass="ButtonCSS" Text="Export Excel" />--%>
                        </td>
                        <td align="left">
                            <%--<asp:Button ID="btn_Mail" runat="server" CssClass="ButtonCSS" Text="Mail" />--%>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td align="center" colspan="6">
                            <div id="div_onsaveclick" style="display: none; vertical-align: middle; text-align: center;"
                                align="center">
                                Sending ........
                            </div>
                        </td>
                    </tr>
                    <tr align="center" runat="server" visible="false">
                        <td align="center" colspan="6">
                            <asp:Label ID="lbl_MailSent" ForeColor="Blue" runat="server" CssClass="LabelCSS"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
        <asp:HiddenField ID="Hid_CustomerId" runat="server" />
        <asp:HiddenField ID="Hid_TransType" runat="server" />
        <asp:HiddenField ID="Hid_DealSlipIds" runat="server" />
        <asp:HiddenField ID="Hid_PhysicalDMAT" runat="server" />
        <asp:HiddenField ID="Hid_DealTransType" runat="server" />
        <asp:HiddenField ID="Hid_AuthorisedPer" runat="server" />
        <asp:HiddenField ID="Hid_DealSlipIdRetValues" runat="server" />
        <asp:HiddenField ID="Hid_CouponRate" runat="server" />
        <asp:HiddenField ID="Hid_DealSlipNo" runat="server" />
        <asp:HiddenField ID="Hid_DealType" runat="server" />
        <asp:HiddenField ID="Hid_Frequency" runat="server" />
        <asp:HiddenField ID="Hid_PayMode" runat="server" />
        <asp:HiddenField ID="Hid_AmtInWords" runat="server" />
        <asp:HiddenField ID="Hid_AccNo" runat="server" />
        <asp:HiddenField ID="Hid_UserId" runat="server" />
        <asp:HiddenField ID="Hid_UserTypeId" runat="server" />

    </table>
    <%--      </ContentTemplate>
    </atlas:UpdatePanel>--%>
</asp:Content>
