<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="FormatDetail.aspx.vb" Inherits="Forms_FormatDetail" Title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_FormatName" runat="server" SelectProcName="ID_SEARCH_FormatMaster"
                    SelectedFieldName="FormatName" DeleteProcName="ID_DELETE_FormatMaster" DeleteFieldName="FormatId"
                    NavigateUrl="FormatMaster.aspx" PageName="Format" TableName="FormatMaster" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
