<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="ClientProfileDetail.aspx.vb" Inherits="Forms_ClientProfileDetail" title="Client Profile Master" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_CustomerName" runat="server" selectprocname="ID_SEARCH_CustomerMasterNew12"
                    selectedfieldname="CustomerName" deleteprocname="ID_DELETE_CustomerMaster" deletefieldname="CustomerId"
                    navigateurl="ClientProfileMaster.aspx"  PageName ="Client Profile" TableName ="CustomerMaster" CheckUser ="true" TableAlias ="CU"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

