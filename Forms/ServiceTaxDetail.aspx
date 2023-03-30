<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="ServiceTaxDetail.aspx.vb" Inherits="Forms_ServiceTaxDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_SerciceTax" runat="server" selectprocname="ID_SEARCH_ServiceTaxMaster"
                    selectedfieldname="FromDate" deleteprocname="ID_DELETE_TaxMaster" deletefieldname="TaxId"
                    navigateurl="ServiceTaxMaster.aspx"  PageName ="Tax" CheckCompany="false"  TableName="TaxMaster"   />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

