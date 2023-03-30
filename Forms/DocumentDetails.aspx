<%@ page language="VB" masterpagefile="~/Forms/MasterPage.master"  CodeFile="documentdetails.aspx.vb"   autoeventwireup="false" enableviewstatemac="false" inherits="Forms_DocumentDetails" title="DocumentDetails" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_DocumentType" runat="server"  SelectProcName="ID_SEARCH_DocumentMaster" SelectedFieldName="DocumentName"
                   DeleteProcName="MB_DELETE_DocumentMaster" DeleteFieldName="DocumentId"  NavigateUrl="Document.aspx" PageName="Document"  TableName ="DocumentMaster" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

