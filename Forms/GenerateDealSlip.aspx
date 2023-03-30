<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="GenerateDealSlip.aspx.vb" Inherits="Forms_GenerateDealSlip" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center"> 
                 <uc:ViewScreen id="vws_DealSlip" runat="server" selectprocname="ID_SEARCH_GenerateDeal"    
                    selectedfieldname="No" deleteprocname="ID_DELETE_DealSlipEntry" deletefieldname="DealSlipID"
                    navigateurl="DealSlipEntry.aspx"  PageName ="DealSlip" CheckCompany="false"  TableName="DealSlipEntry" ConditionExist="true"   />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

