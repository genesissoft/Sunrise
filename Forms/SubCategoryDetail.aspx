<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="SubCategoryDetail.aspx.vb" Inherits="Forms_SubCategoryDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_SubCatName" runat="server" selectprocname="ID_SEARCH_SubCategoryMaster"
                    selectedfieldname="SubCategoryName" deleteprocname="ID_DELETE_SubCategoryMaster" deletefieldname="SubCategoryId"
                    navigateurl="SubCategoryMaster.aspx"  PageName ="SubCategory" CheckCompany="false"  TableName="SubCategoryMaster"   />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

