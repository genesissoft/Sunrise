<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="FinancialDeliveryInformation.aspx.vb" Inherits="Forms_FinancialDeliveryInformation"
    Title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_DematDelivery" runat="server" SelectProcName="Id_SEARCH_FinancialDelivery"
                    SelectedFieldName="DealSlipNo" DeleteProcName="" DeleteFieldName="DSE.DealSlipId" CheckYearCompany="true"   
                    NavigateUrl="FinancialDelivery.aspx" PageName="Financial Information" CheckCompany="false"
                    TableName="FinancialInformation" ConditionExist ="true"/>
            </div>
        </ContentTemplate> 
    </asp:UpdatePanel>
</asp:Content>
