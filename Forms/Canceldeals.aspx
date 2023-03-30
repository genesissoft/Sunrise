<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="Canceldeals.aspx.vb" Inherits="Forms_Canceldeals" Title="Cancel Deal" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <%-- <script language="javascript">
  
        function OpenReport(dealFlag)
        {   
            var strDealType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealType").value
            var strFreqInterest = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Frequency").value            
            var strTransType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value            
            var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value
            //alert(strDealSlipId)
            //alert(strFreqInterest)
            pageUrl = "ViewDealReports.aspx?DealFlag=" + dealFlag + "&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType
                    + "&DealType=" + strDealType + "&FreqInterest=" + strFreqInterest;
            var ret = window.open(pageUrl,target="_blank","left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")            
            return false    
        }
        
        
        function GenerateContractNote(strId)
        {  

            strId = document.all("ctl00_ContentPlaceHolder1_srh_TransCode_Hid_SelectedId").value ;             
            var pageUrl = "ContractNote.aspx";     
            pageUrl = pageUrl + "?DealSlipId=" + strId ; 
            var ret = ShowDialogOpen(pageUrl, "380px", "190px")
            if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            }
            else
            {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIdRetValues").value = ret
                return true   
            }              
            return true
        }
  
    </script>--%>

    <script type="text/javascript">

        function Validation() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DateType_0").checked == true) {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Remark").value) == "") {
                    AlertMessage("Validation", "Please Specify Reason of cancellation", 175, 450);
                    return false;
                }
                if (window.confirm("Are you sure you want to Cancel this Deal????")) return true;
                return false;
            }
        }


        function DateType() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DateType_0").checked == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_tr_MergeDealNo").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_tr_DealNo").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_tr_ConvertPending").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_CancelType").style.display = "";

            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_tr_MergeDealNo").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_tr_DealNo").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_tr_ConvertPending").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_CancelType").style.display = "none"

            }
        }


    </script>

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" EnableViewState="true">
    </asp:ScriptManagerProxy>
    <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr align="left">
            <td class="SectionHeaderCSS">Cancel Deal</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="95%">
                            <tr align="center" valign="top">
                                <td style="width: 49%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" style="width: 100%">
                                        <tr align="left">
                                            <td id="lbl_According" runat="server">Deal Type:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass="LabelCSS" AutoPostBack="True">
                                                    <asp:ListItem Value="D" Selected="True">Simple Deal</asp:ListItem>
                                                    <asp:ListItem Value="P">Convert to Pending</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_DealNo" runat="server">
                                            <td>Deal No.:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <%--<uc:Search ID="Srh_DealNumber" runat="server" FormWidth="550" FormHeight="400" AutoPostback="true"
                                                    ProcName="ID_SEARCH_CancelDealnew" SelectedFieldName="DealSlipNo" SourceType="StoredProcedure"
                                                    TableName="DealSlipEntry" ConditionExist="true" ConditionalFieldName="" ConditionalFieldId=""
                                                    CheckYearCompany="true"></uc:Search>--%>
                                                <uc:Search ID="Srh_DealNumber" runat="server" PageName="CancelDealNumber" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo" ConditionalFieldName="UserId" ConditionExist="true"
                                                    ConditionalFieldId="Hid_UserId" ConditionalFieldId1="Hid_UserTypeId" ConditionalFieldName1="UserTypeId" CheckYearCompany="true" />
                                            </td>
                                        </tr>
                                           <tr align="left" id="tr_ConvertPending" runat="server">
                                            <td>
                                                Deal No.:
                                            </td>
                                            <td style="padding-left: 0px;">

                                                 <uc:Search ID="Srh_ConvertPending" runat="server" PageName="ConvertToPending" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo" ConditionalFieldName="UserId" ConditionExist="true"
                                                    ConditionalFieldId="Hid_UserId" ConditionalFieldId1="Hid_UserTypeId" ConditionalFieldName1="UserTypeId" CheckYearCompany ="true" />


                                              <%--  <uc:Search ID="Srh_ConvertPending" runat="server" FormWidth="550" FormHeight="400" AutoPostback="true"
                                                    ProcName="ID_SEARCH_ConvertToPending" SelectedFieldName="DealSlipNo" SourceType="StoredProcedure"
                                                    TableName="DealSlipEntry" ConditionExist="true" ConditionalFieldName="" ConditionalFieldId=""
                                                    CheckYearCompany="true"></uc:Search>--%>
                                            </td>
                                        </tr>
                                        <tr align="left" id="tr_MergeDealNo" runat="server">
                                            <td>MergeDealNo.:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <%-- <uc:Search ID="Srh_MergeTrnsCode" runat="server" FormWidth="550" FormHeight="400"
                                                    AutoPostback="true" ProcName="ID_SEARCH_mergedealentry" SelectedFieldName="MergedealNo"
                                                    SourceType="StoredProcedure" TableName="MergeDealEntry" ConditionExist="true"
                                                    ConditionalFieldName="" ConditionalFieldId="" CheckYearCompany="true"></uc:Search>--%>

                                                <uc:Search ID="Srh_MergeTrnsCode" runat="server" PageName="MergeDealNo" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Issuer:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_Issuer" runat="server"> 
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Name Of Security:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_SecurityName" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Customer Name:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_CustName" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Contact Person:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_Contact" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Remark:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Remark" Width="200px" Height="70px" TextMode="MultiLine" runat="server"
                                                    CssClass="TextBoxCSS" TabIndex="21"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 2%;">&nbsp;</td>
                                <td style="width: 49%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" style="width: 100%">
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
                                            <td>Mode Of Delivery:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_ModeofDelivery" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Payment Mode:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_PaymentMode" runat="server">
                                                </asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3"></td>
                            </tr>
                            <%--<tr>
                                <td align="center" colspan="6">
                                    <asp:Button ID="btn_DealConfirmation" runat="server" CssClass="ButtonCSS" Text="Deal Confirmation" />
                                    <asp:Button ID="btn_SGLLetter" runat="server" CssClass="ButtonCSS" Text="SGL Letter" />
                                    <asp:Button ID="btn_PrintDealSlip" runat="server" CssClass="ButtonCSS" Text="Deal Slip" />
                                    <asp:Button ID="btn_GenerateContractNote" runat="server" CssClass="ButtonCSS" Text="Generate Contract Note" />
                                </td>
                            </tr>--%>
                            <tr align="center">
                                <td colspan="3">
                                    <asp:Label ID="lbl_Deleted" ForeColor="Blue" runat="server" CssClass="LabelCSS"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="3">
                                    <asp:Button ID="btn_DeleteDeal" runat="server" Text="Cancel Deal" Width="80px" ToolTip="DeleteDeal"
                                        CssClass="ButtonCSS" TabIndex="29" />
                                </td>
                            </tr>
                            <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                            <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                            <asp:HiddenField ID="Hid_TransType" runat="server" />
                            <asp:HiddenField ID="Hid_DealSlipIds" runat="server" />
                            <asp:HiddenField ID="Hid_DealType" runat="server" />
                            <asp:HiddenField ID="Hid_Frequency" runat="server" />
                            <asp:HiddenField ID="Hid_DealSlipIdRetValues" runat="server" />
                            <asp:HiddenField ID="Hid_DealTransType" runat="server" />
                            <asp:HiddenField ID="Hid_FinancialDealType" runat="server" />
                            <asp:HiddenField ID="Hid_UserId" runat="server" />
                            <asp:HiddenField ID="Hid_UserTypeId" runat="server" />

                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
