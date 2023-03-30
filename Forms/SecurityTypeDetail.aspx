<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="SecurityTypeDetail.aspx.vb" Inherits="Forms_SecurityTypeDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_SecurityTypeName" runat="server" selectprocname="ID_SEARCH_SecurityTypeMaster"
                    selectedfieldname="SecurityTypeName" deleteprocname="ID_DELETE_SecurityTypeMaster" deletefieldname="SecurityTypeId"
                    navigateurl="SecurityTypeMaster.aspx"  PageName ="SecurityType" TableName ="SecurityTypeMaster"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

