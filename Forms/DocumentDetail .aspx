<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="DocumentDetail .aspx.vb" Inherits="Forms_DocumentDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_DocumentTypeName" runat="server" selectprocname="ID_SEARCH_Document1" 
                    selectedfieldname="DocumentTypeName" deleteprocname="ID_DELETE_DocumentTypeMaster" deletefieldname="DocumentTypeId"
                    navigateurl="DocumentMaster.aspx"  PageName ="DocumentMaster" TableName="DocumentTypeMaster"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

