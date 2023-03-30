<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="PendingCostMemo.aspx.vb" Inherits="Forms_PendingCostMemo" title="Pending Cost Memo" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center"> 
                 <uc:ViewScreen id="vws_DealSlip" runat="server" selectprocname="ID_SEARCH_PendingCostMemo"    CheckYearCompany="True"
                    selectedfieldname="DealslipNo" deleteprocname="ID_DELETE_DealSlipEntry" deletefieldname="DealSlipID"
                    navigateurl="DealSlipEntry.aspx"  PageName ="PendingCostMemo" CheckCompany="false"  TableName="DealSlipEntry" ConditionExist="true"
                       CheckUser ="true"  TableAlias ="DSE"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

