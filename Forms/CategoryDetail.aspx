<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="CategoryDetail.aspx.vb" Inherits="Forms_CategoryDetail" title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_CategoryDetail" runat="server" SelectProcName="ID_SEARCH_CategoryMaster"
                    SelectedFieldName="CategoryName" DeleteProcName="ID_DELETE_CategoryMaster" DeleteFieldName="CategoryId"
                    NavigateUrl="CategoryMaster.aspx" PageName="CategoryMaster"  TableName="CategoryMaster"  />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

