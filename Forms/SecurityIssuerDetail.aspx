<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="SecurityIssuerDetail.aspx.vb" Inherits="Forms_SecurityIssuerDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_Issuer" runat="server"  SelectProcName="ID_SEARCH_IssuerMaster_RDM" SelectedFieldName="IssuerName"
                   DeleteProcName="ID_DELETE_IssuerMaster_RDM" DeleteFieldName="RDMIssuerId"  NavigateUrl="SecurityIssuer.aspx"  PageName="Issuer"
                   TableName="IssuerMaster_RDM"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

