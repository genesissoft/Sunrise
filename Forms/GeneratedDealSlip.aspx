<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="GeneratedDealSlip.aspx.vb" Inherits="Forms_GeneratedDealSlip" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center"> 
                 <uc:ViewScreen id="vws_DealSlip" runat="server" selectprocname="ID_SEARCH_GeneratedDeal" CheckYearCompany="True"    
                    selectedfieldname="DealSlipNo" deleteprocname="ID_DELETE_DealSlipEntry" deletefieldname="DealSlipID"
                    navigateurl="DealSlipEntry.aspx"  PageName ="GeneratedDeals" CheckCompany="false"  TableName="DealSlipEntry"
                    ConditionExist="true" DefaultSort ="DealDate"  CheckUser ="true" TableAlias ="DSE"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

