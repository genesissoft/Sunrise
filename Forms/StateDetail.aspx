<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="StateDetail.aspx.vb" Inherits="Forms_StateDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_StateName" runat="server" selectprocname="ID_SEARCH_StateMaster"
                    selectedfieldname="StateName" deleteprocname="ID_DELETE_StateMaster" deletefieldname="StateId"
                    navigateurl="StateMaster.aspx"  PageName ="State" TableName ="StateMaster"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

