<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="SecurityCategoryDetail.aspx.vb" Inherits="Forms_SecurityCategoryDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_SecCategoryDetail" runat="server" SelectProcName="ID_SEARCH_SecurityCategoryMaster"
                    SelectedFieldName="SecurityCategory" DeleteProcName="ID_DELETE_SecurityCategoryMaster" DeleteFieldName="SecurityCatId"
                    NavigateUrl="SecurityCategoryMaster.aspx" PageName="SecurityCategoryMaster"  TableName="SecurityCategoryMaster"  />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

