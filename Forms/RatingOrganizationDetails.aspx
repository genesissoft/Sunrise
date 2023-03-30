<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="RatingOrganizationDetails.aspx.vb" Inherits="Forms_RatingOrganizationDetails" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_RatingOrg" runat="server"  SelectProcName="ID_SEARCH_RatingOrganization" SelectedFieldName="OrganizationName"
                   DeleteProcName="ID_DELETE_RatingOrganization" DeleteFieldName="RatingOrganizationId"  NavigateUrl="RatingOrganization.aspx" PageName="RatingOrganization" TableName ="RatingOrganization"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

