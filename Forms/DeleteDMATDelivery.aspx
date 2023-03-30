<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="DeleteDMATDelivery.aspx.vb" Inherits="Forms_DeleteDMATDelivery" Title="Demat Delivery" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" language="javascript">
        function validation() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DateType_0").checked == true) {

                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_TransCode_txt_Name").value) == "") {
                    AlertMessage("Validation", "Please select deal Number", 175, 450)
                    return false;
                }
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DateType_1").checked == true) {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_FinanCode_txt_Name").value) == "") {
                    AlertMessage("Validation", "Please select deal Number", 175, 450)
                    return false;
                }
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DateType_0").checked == true) {

                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_DmatRemark").value) == "") {
                    AlertMessage("Validation", "Please Enter Cancel remark", 175, 450)
                    return false;
                }
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DateType_1").checked == true) {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_FinanRemark").value) == "") {
                    AlertMessage("Validation", "Please Enter Cancel remark", 175, 450)
                    return false;
                }
            }

            if (window.confirm("Are you sure you want to Delete this Slip?")) return true;
            return false;

        }
        function ValidateTotalAmt() {
            var FaceValue = document.getElementById("ctl00_ContentPlaceHolder1_txt_FaceValue")
            var FaceValMultiple = document.getElementById("ctl00_ContentPlaceHolder1_Cbo_FaceValue")
            var totalAmt = document.getElementById("ctl00_ContentPlaceHolder1_txt_Total")
            var TotalFaceVal = (FaceValue * FaceValMultiple)
            if (totalAmt > TotalFaceVal) {
                AlertMessage("Validation", 'Total Amount Exceeds  face value ', 175, 450)
                return false
            }
            else {
                return true
            }

        }
        function Delete_entry() {

            if (window.confirm("Are you sure you want to Delete record ?")) {
                return true
            }
            else {
                return false
            }
        }

        function Update(strId) {
            document.all("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value = strId;
            return true

        }


        function SlipType() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DateType_0").checked == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_row_FinanSlip").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_CancelFinan").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_DematSlip").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_CancelDemat").style.display = "";
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_row_FinanSlip").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_DematSlip").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_CancelFinan").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_CancelDemat").style.display = "none";

            }
        }
    </script>
    <asp:UpdatePanel ID="upd" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr align="left">
                    <td class="SectionHeaderCSS">Delete Demat/Financial Slip</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table cellspacing="0" cellpadding="0" border="0" align="center" width="90%">
                            <tr align="center" valign="top">
                                <td style="width: 48%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                        <tr align="left">
                                            <td>Slip Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS" AutoPostBack="True">
                                                    <asp:ListItem Value="D" Selected="True">Demat</asp:ListItem>
                                                    <asp:ListItem Value="F">Financial</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_DematSlip" runat="server">
                                            <td>DealSlip No:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <%-- <uc:Search ID="srh_TransCode" runat="server" AutoPostback="true" ProcName="ID_SEARCH_DeleteDMatDealSlip"
                                                    CheckYearCompany="true" SelectedFieldName="DealSlipNo" SourceType="StoredProcedure"
                                                    TableName="DealSlipEntry" ConditionExist="true" ConditionalFieldName="" ConditionalFieldId=" "
                                                    FormWidth="800" Width="200"></uc:Search>--%>
                                                <uc:Search ID="srh_TransCode" runat="server" PageName="DeleteDMatDealSlip" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo" CheckYearCompany ="true"   />
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_FinanSlip" runat="server">
                                            <td>DealSlip No:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <%-- <uc:Search ID="srh_FinanCode" runat="server" AutoPostback="true" ProcName="ID_SEARCH_DeleteFinanDealSlip"
                                                    CheckYearCompany="true" SelectedFieldName="DealSlipNo" SourceType="StoredProcedure"
                                                    TableName="DealSlipEntry" ConditionExist="true" ConditionalFieldName="" ConditionalFieldId=" "
                                                    FormWidth="800" Width="200"></uc:Search>--%>
                                                <uc:Search ID="srh_FinanCode" runat="server" PageName="DeleteFinancialDealSlip" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo"  CheckYearCompany ="true"  />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Issuer Of Security:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_IssuerOfSecurity" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="100" TabIndex="3" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Name Of Security:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SecurityName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="100" TabIndex="3" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Client Name:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ClientName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="100" TabIndex="3" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left" id="row_CancelDemat" runat="server">
                                            <td>Cancel Remark:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_DmatRemark" Width="200px" Rows="4" TextMode="MultiLine" runat="server"
                                                    CssClass="TextBoxCSS" TabIndex="21"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left" id="row_CancelFinan" runat="server">
                                            <td>Cancel Remark1:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_FinanRemark" Width="200px" Rows="4" TextMode="MultiLine"
                                                    runat="server" CssClass="TextBoxCSS" TabIndex="21"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 4%;">&nbsp;</td>
                                <td style="width: 48%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                        <tr align="left">
                                            <td>Face Value:
                                            </td>
                                            <td style="padding: 0px;">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr align="left">
                                                        <td>
                                                            <asp:TextBox ID="txt_FaceValue" runat="server" CssClass="TextBoxCSS" Width="75px"
                                                                TabIndex="9" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Cbo_FaceValue" Width="75px" runat="server" CssClass="ComboBoxCSS"
                                                                TabIndex="1" Enabled="False">
                                                                <asp:ListItem Value="1000">Thousand</asp:ListItem>
                                                                <asp:ListItem Selected="true" Value="100000">Lac</asp:ListItem>
                                                                <asp:ListItem Value="10000000">Crore</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Settlement Amount:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SettlmntAmt" runat="server" Width="155px" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Deal Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_TransDate" runat="server" Width="155px" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Settlement Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SettlmtDate" runat="server" Width="155px" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" id="row_Save" runat="server">
                    <td>
                        <asp:Button ID="btn_Delete" runat="server" Text="Delete" ToolTip="Delete" CssClass="ButtonCSS" />
                    </td>
                </tr>
                <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                <asp:HiddenField ID="Hid_DpDetailsId" runat="server" />
                <asp:HiddenField ID="Hid_DmatId" runat="server" />
                <asp:HiddenField ID="Hid_Index" runat="server" />
                <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                <asp:HiddenField ID="Hid_CustDpId" runat="server" />
                <asp:HiddenField ID="Hid_DematInfoId" runat="server" />
                <asp:HiddenField ID="Hid_Id" runat="server" />
                <asp:HiddenField ID="Hid_RetValues" runat="server" />
                <asp:HiddenField ID="Hid_TransType" runat="server" />
                <asp:HiddenField ID="Hid_facevalue" runat="server" />
                <asp:HiddenField ID="Hid_FaceMultiple" runat="server" />
                <asp:HiddenField ID="Hid_BalanceFV" runat="server" />
                <asp:HiddenField ID="Hid_FVS" runat="server" />
                <asp:HiddenField ID="Hid_DematAccTo" runat="server" />
                <asp:HiddenField ID="Hid_PayMode" runat="server" />
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
