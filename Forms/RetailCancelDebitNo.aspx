<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="RetailCancelDebitNo.aspx.vb" Inherits="Forms_RetailCancelDebitNo" Title="Retail Cancel Debit No" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script type="text/javascript">
        function Validation()
        {
        
              if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_NameOFClient_txt_Name").value) == "")
                {  alert("Please select Client");
                    return false;
                } 
                
                if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_DebitRefNo_txt_Name").value) == "")
                {  alert("Please select Debit No");
                    return false;
                } 
             
             if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Remark").value) == "")
                {  alert("Please Specify Reason of cancellation");
                    return false;
                } 
                if (window.confirm("Are you sure you want to Cancel this Debit Ref. No.??")) return true;
                return false;
            }
    </script>

    <atlas:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" EnableViewState="true">
    </atlas:ScriptManagerProxy>
    <table id="Table1" align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Cancel Debit No</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table cellspacing="0" cellpadding="0" border="0" align="center" width="40%">
                            <tr align="left">
                                <td id="Client">
                                    Customer Name:
                                </td>
                                <td style="padding-left: 0px;">
                                    <uc:Search ID="Srh_NameOFClient" runat="server" AutoPostback="true" ProcName="ID_SEARCH_CustomerMaster"
                                        SelectedFieldName="CustomerName" SourceType="StoredProcedure" TableName="CustomerMaster"
                                        ConditionExist="true" ConditionalFieldName=" " ConditionalFieldId=" " Width="200"
                                        FormHeight="340" FormWidth="800"></uc:Search>
                                </td>
                            </tr>
                            <tr align="left" id="Tr1" runat="server">
                                <td>
                                    <%--<asp:Label ID="Label3" runat="server" Text="DebitRefNo: " CssClass="LabelCSS" Width="100px"></asp:Label>--%>
                                    DebitRefNo:
                                </td>
                                <td style="padding-left: 0px;">
                                    <uc:Search ID="srh_DebitRefNo" runat="server" Width="200" AutoPostback="true" ProcName="ID_SEARCH_RetDebitRefNo"
                                        SelectedFieldName="DebitRefNo" SourceType="StoredProcedure" TableName="DealslipEntry"
                                        ConditionExist="true" ConditionalFieldName="DSE.CustomerId" ConditionalFieldId="Srh_NameOFClient"
                                        FormHeight="400" FormWidth="600" CheckYearCompany="true"></uc:Search>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Remark:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Remark" Width="200px" Height="70px" TextMode="MultiLine" runat="server"
                                        CssClass="TextBoxCSS" TabIndex="21"></asp:TextBox></td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="2">
                                    <asp:Label ID="lbl_Deleted" ForeColor="Green" runat="server" CssClass="LabelCSS"></asp:Label>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btn_DeleteDeal" runat="server" Text="Cancel DebitNo" ToolTip="DeleteDeal"
                                        CssClass="ButtonCSS" Width="100px" TabIndex="29" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
