<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="IssuerTypeDetail_RDM.aspx.vb" Inherits="Forms_IssuerTypeDetail_RDM" title="Issuer Type RDM" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

  <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_IssuerType" runat="server"  SelectProcName="ID_SEARCH_IssuerTypeMaster_RDM" SelectedFieldName="IssuerTypeName"
                   DeleteProcName="ID_DELETE_IssuerType_RDM" DeleteFieldName="RDMIssuerTypeId"  NavigateUrl="IssuerTypeMaster_RDM.aspx"  PageName="Issuer Type"
                   TableName="IssuerTypeMaster_RDM"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

