<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" CodeFile="~/Forms/creditNoteEntry.aspx.vb"
    AutoEventWireup="false" Inherits="Forms_CreditNoteEntry" Title="Credit Note Entry"
    EnableViewStateMac="false" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation()
        {  
             if(document.getElementById("ctl00_ContentPlaceHolder1_rbl_Through_0").checked == true)
                {
                    if((document.getElementById("ctl00_ContentPlaceHolder1_Srh_ApplNo_txt_Name").value) == "")
                    {
                        alert("Please Select Application No");
                        return false;
                    }  
                }
           else
                {
                 if((document.getElementById("ctl00_ContentPlaceHolder1_Srh_ChannApplNo_txt_Name").value) == "")
                    {
                        alert("Please Select Application No");
                        return false;
                    }             
                }         
            
            
                    
            if((document.getElementById("ctl00_ContentPlaceHolder1_txt_TotAmount").value-0) == 0)
            {
                alert("Total Amount can not be zero");
                return false;
            }
            if((document.getElementById("ctl00_ContentPlaceHolder1_txt_GrossAmt").value-0) == 0)
            {
                alert("Gross Amount Can not be zero");
                return false;
            }
            if((document.getElementById("ctl00_ContentPlaceHolder1_txt_NetAmt").value-0) == 0)
            {
                alert("Net Amount can not be zero");
                return false;
            }              
//            if((document.getElementById("ctl00_ContentPlaceHolder1_txt_ChequeNo").value) == "")
//            {
//                alert("Please enter Cheque No");
//                return false;
//            }
//            if((document.getElementById("ctl00_ContentPlaceHolder1_txt_ChequeDate").value) == "")
//            {
//                alert("Please enter Cheque Date");
//                return false;
//            }
//            if((document.getElementById("ctl00_ContentPlaceHolder1_txt_DrawnOn").value) == "")
//            {
//                alert("Please enter Drawn On Bank Name");
//                return false;
//            }
        }
        
        
         function CheckRateAmt(fntId,txtId)
        {      
       
            var TotalAppAmount = (document.getElementById("ctl00_ContentPlaceHolder1_" + txtId).value-0)
            var inc = ((TotalAppAmount) / 100000).toFixed(2)
            if(inc != 0)
            {
                document.getElementById(fntId).innerHTML =  inc + " LACS";                
                document.getElementById(fntId).style.display = "inline";
            }
            else
            {
                document.getElementById(fntId).style.display = "none";
            }
        }
        
         function ReportView()
        {
           

            var strCreditNoteId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CreditNoteId").value 
            //alert(strCreditNoteId)
        
            pageUrl1 = "ViewNoteReports.aspx?Note=CreditNoteEntry&CreditNoteId="+ strCreditNoteId ;
     
            var ret = window.open(pageUrl1,target="_blank","left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
     
            window.location = "CreditNoteDetails.aspx?Id=" + strCreditNoteId ;
        }
        
        
         function ReportViewForChannel()
        {
           

            var strCreditNoteId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CreditNoteId").value 
            //alert(strCreditNoteId)
        
            pageUrl1 = "ViewNoteReports.aspx?Note=ChanepatCreditNoteEntry&CreditNoteId="+ strCreditNoteId ;
     
            var ret = window.open(pageUrl1,target="_blank","left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
     
            window.location = "CreditNoteDetails.aspx?Id=" + strCreditNoteId ;
        }
        
         function AppType()
            {
                if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_Through_0").checked == true)
                { 
                    document.getElementById("td_channappno").style.display = "None";
                    document.getElementById("td_channappnoSearch").style.display = "None";
                    document.getElementById("td_appno").style.display = "";
                    document.getElementById("td_appnoSearch").style.display = "";
                }
                else
                {  
                   document.getElementById("td_channappno").style.display = "";
                    document.getElementById("td_channappnoSearch").style.display = "";
                    document.getElementById("td_appno").style.display = "None";
                    document.getElementById("td_appnoSearch").style.display = "None";
                }
           }
    </script>

    <div>
        <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
            <ContentTemplate>
                <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr align="left">
                        <td class="SectionHeaderCSS">
                            Credit Note Entry</td>
                    </tr>
                    <tr class="line_separator">
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr align="center" valign="top">
                        <td>
                            <table width="90%" align="center" cellspacing="0" cellpadding="0" border="0">
                                <tr align="left">
                                    <td>
                                        App Through:
                                    </td>
                                    <td style="padding-left: 0px;">
                                        <asp:RadioButtonList ID="rbl_Through" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" CssClass="LabelCSS" TabIndex="0" AutoPostBack="False">
                                            <asp:ListItem Selected="True" Value="I">Investor</asp:ListItem>
                                            <asp:ListItem Value="C">ChannelPartner</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        TDS applicable:
                                    </td>
                                    <td style="padding-left: 0px;">
                                        <asp:RadioButtonList ID="rbl_TDSapplicable" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" CssClass="LabelCSS" TabIndex="0" AutoPostBack="True">
                                            <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem Value="N">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Credit No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_CreditNoteNo" runat="server" CssClass="TextBoxCSS" TabIndex="0"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        Total Amount:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_TotAmount" runat="server" CssClass="TextBoxCSS" TabIndex="0"
                                            ReadOnly="True" Style="text-align: right"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                        <font id="fnt_Perc1" class="TextboxCSS"></font>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Credit Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_CreditDate" runat="server" CssClass="TextBoxCSS" Width="110px"
                                            TabIndex="0"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_CreditDate',this);"
                                                id="IMG2">
                                    </td>
                                    <td>
                                        Gross Amt.:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_GrossAmt" runat="server" CssClass="TextBoxCSS" TabIndex="0"
                                            ReadOnly="True" Style="text-align: right"></asp:TextBox><em><span style="color: Red;
                                                vertical-align: super;">*</span></em>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td id="td_appno">
                                        Application No:
                                    </td>
                                    <td style="padding-left: 0px;" id="td_appnoSearch">
                                        <uc:Search ID="Srh_ApplNo" runat="server" AutoPostback="true" ProcName="MB_SEARCH_CreditApplicationEntry"
                                            SelectedFieldName="ApplicationNo" SourceType="StoredProcedure" TableName="ApplicationEntry"
                                            ConditionalFieldName="" ConditionalFieldId="" ConditionExist="true" FormWidth="800"
                                            FormHeight="350"></uc:Search>
                                    </td>
                                    <td id="td_channappno">
                                        Channel App No:
                                    </td>
                                    <td style="padding-left: 0px;" id="td_channappnoSearch">
                                        <uc:Search ID="Srh_ChannApplNo" runat="server" AutoPostback="true" ProcName="MB_SEARCH_CreditApplicationEntry_New"
                                            SelectedFieldName="ApplicationNo" SourceType="StoredProcedure" TableName="ApplicationEntry"
                                            ConditionalFieldName="" ConditionalFieldId="" ConditionExist="true"></uc:Search>
                                    </td>
                                    <td id="LBL_UTRNO">
                                        TDS:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_TDS" runat="server" CssClass="TextBoxCSS" TabIndex="0" ReadOnly="True"
                                            Style="text-align: right"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
                                        </font>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Issuer Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_IssuerName" runat="server" CssClass="TextBoxCSS" Width="200px"
                                            TabIndex="0" ReadOnly="True"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                    </td>
                                    <td class="LabelCSS" align="left">
                                        Net:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_NetAmt" runat="server" CssClass="TextBoxCSS" TabIndex="0" ReadOnly="True"
                                            Style="text-align: right"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Issue Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_IssueName" runat="server" CssClass="TextBoxCSS" Width="200px"
                                            TabIndex="0" ReadOnly="True"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                    </td>
                                    <td rowspan="4" valign="middle">
                                        Remark:
                                    </td>
                                    <td rowspan="4">
                                        <asp:TextBox ID="txt_Remark" runat="server" CssClass="TextBoxCSS" Width="250px" TabIndex="0"
                                            Height="80px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Investor Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_InvestoreName" runat="server" CssClass="TextBoxCSS" Width="200px"
                                            TabIndex="0" ReadOnly="True"></asp:TextBox><i style="color: Red; vertical-align: super;"></i></td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        ChannelPartnerName:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ChannelPartnerName" runat="server" CssClass="TextBoxCSS" Width="200px"
                                            TabIndex="0" ReadOnly="True"></asp:TextBox><i style="color: Red; vertical-align: super;"></i></td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Cheque No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ChequeNo" runat="server" CssClass="TextBoxCSS" Width="200px"
                                            TabIndex="0" ReadOnly="false"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Cheque Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ChequeDate" runat="server" CssClass="TextBoxCSS" Width="110px"
                                            TabIndex="0" ReadOnly="false"></asp:TextBox><img class="calender" src="../Images/Calender.jpg"
                                                onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ChequeDate',this);"
                                                id="IMG1" />
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Drawn on:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DrawnOn" runat="server" CssClass="TextBoxCSS" Width="200px"
                                            TabIndex="0" ReadOnly="false"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                    </td>
                                </tr>
                                <tr class="line_separator">
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td colspan="4">
                                        <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" TabIndex="0" />
                                        <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" Visible="false"
                                            TabIndex="0" />
                                        <asp:Button ID="btn_Back" runat="server" CssClass="ButtonCSS" Text="Back" TabIndex="0" />
                                        <asp:Button ID="btn_ReCalculate" runat="server" CssClass="ButtonCSS" Text="Re-Calculate"
                                            TabIndex="0" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <asp:HiddenField ID="Hid_IssuerName" runat="server" />
                    <asp:HiddenField ID="Hid_ServiceTaxAmt" runat="server" />
                    <asp:HiddenField ID="Hid_CessAmt" runat="server" />
                    <asp:HiddenField ID="Hid_SecoCessAmt" runat="server" />
                    <asp:HiddenField ID="Hid_totFees" runat="server" />
                    <asp:HiddenField ID="Hid_FeesOn" runat="server" />
                    <asp:HiddenField ID="Hid_IssueSizeQty" runat="server" />
                    <asp:HiddenField ID="Hid_IssueSizeMultiple" runat="server" />
                    <asp:HiddenField ID="Hid_NomanClature" runat="server" />
                    <asp:HiddenField ID="Hid_IssueFee" runat="server" />
                    <asp:HiddenField ID="Hid_CreditNoteNo" runat="server" />
                    <asp:HiddenField ID="Hid_remark" runat="server" />
                    <asp:HiddenField ID="Hid_TotAllotedAmt" runat="server" />
                    <asp:HiddenField ID="Hid_TotalIncentiveAmt" runat="server" />
                    <asp:HiddenField ID="Hid_IssuerId" runat="server" />
                    <asp:HiddenField ID="Hid_IssueId" runat="server" />
                    <asp:HiddenField ID="Hid_TDSRate" runat="server" />
                    <asp:HiddenField ID="Hid_IncentiveType" runat="server" />
                    <asp:HiddenField ID="Hid_AppAmtQty" runat="server" />
                    <asp:HiddenField ID="Hid_AppAmtMultiple" runat="server" />
                    <asp:HiddenField ID="Hid_IncentiveAmt" runat="server" />
                    <asp:HiddenField ID="Hid_IncePer" runat="server" />
                    <asp:HiddenField ID="Hid_CreditNoteId" runat="server" />
                    <asp:HiddenField ID="Hid_ChannelPartInc1" runat="server" />
                    <asp:HiddenField ID="Hid_Instrument" runat="server" />
                </table>
            </ContentTemplate>
        </atlas:UpdatePanel>
    </div>
</asp:Content>
