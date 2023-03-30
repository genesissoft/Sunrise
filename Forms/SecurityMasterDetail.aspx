<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SecurityMasterDetail.aspx.vb" Inherits="Forms_SecurityMasterDetail"
    Title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <uc:ViewScreen ID="vws_SecurityMaster" runat="server" SelectProcName="ID_SEARCH_SecurityMaster"
                SelectedFieldName="SecurityName" DeleteProcName="ID_DELETE_SecurityMaster" TableName="SecurityMaster"
                DeleteFieldName="SecurityId" NavigateUrl="securitymaster.aspx" PageName="SecurityMaster">
            </uc:ViewScreen>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
