<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="RatingDetail.aspx.vb" Inherits="Forms_RatingDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_RatingDetails" runat="server"  SelectProcName="ID_SEARCH_RatingMaster" SelectedFieldName="Rating"
                   DeleteProcName="ID_DELETE_RatingMaster" DeleteFieldName="RatingId"  NavigateUrl="RatingMaster.aspx" PageName="Rating" TableName="RatingMaster" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

