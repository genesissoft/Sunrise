<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="PendingDealSlip.aspx.vb" Inherits="Forms_PendingDealSlip" title="PendingDealSlip" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center"> 
                 <uc:ViewScreen id="vws_DealSlip" runat="server" selectprocname="ID_SEARCH_GenerateDeal"  CheckYearCompany="true"    
                    selectedfieldname="DealSlipNo" deleteprocname="ID_DELETE_DealSlipEntry" deletefieldname="DealSlipID"
                    navigateurl="DealSlipEntry.aspx"  PageName ="PendingDeals" CheckCompany="false"  TableName="DealSlipEntry" TableAlias ="DSE"
                    ConditionExist="true" DefaultSort ="DealDate" CheckUser ="true" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

