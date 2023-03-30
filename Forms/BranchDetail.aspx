<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="BranchDetail.aspx.vb" Inherits="Forms_BranchDetail" Title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_BranchName" runat="server" SelectProcName="ID_SEARCH_BranchMaster"
                    SelectedFieldName="BranchName" DeleteProcName="ID_DELETE_BranchMaster" DeleteFieldName="Branchid"
                    NavigateUrl="BranchMaster.aspx" PageName="Branch" TableName="BranchMaster" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
