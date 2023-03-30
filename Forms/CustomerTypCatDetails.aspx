<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="CustomerTypCatDetails.aspx.vb" Inherits="Forms_CustomerTypCatDetails" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <asp:UpdatePanel ID="UpdatePanel2" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_CustomerTypeCategory" runat="server"  SelectProcName="ID_SEARCH_CustTypeCategoryMaster" SelectedFieldName="CustomerTypeCategory"
                   DeleteProcName="ID_DELETE_CustTypeCategoryMaster" DeleteFieldName="CustomerTypeCatId"  NavigateUrl="CustomerTypeCategoryMaster.aspx" PageName="CustomerTypeCategory" TableName="CustomerTypeCategoryMaster" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

