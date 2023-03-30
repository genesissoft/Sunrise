<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="DematDeliveryDetail.aspx.vb" Inherits="Forms_DematDeliveryDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_DematDelivery" runat="server" selectprocname="ID_SEARCH_DMATInfo"  CheckYearCompany="true"   
                    selectedfieldname="DealSlipNo" deleteprocname="ID_DELETE_DMATinformation" deletefieldname="DE.DealSlipId" 
                   navigateurl="DMATDelivery.aspx"  PageName ="DematInformation" CheckCompany="false" TableName="DMATInformation"   ConditionExist ="true"  />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

