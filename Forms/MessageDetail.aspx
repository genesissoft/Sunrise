<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="MessageDetail.aspx.vb" Inherits="Forms_MessageDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_Message" runat="server" selectprocname="ID_SEARCH_MessageBoard"
                    selectedfieldname="Message" deleteprocname="ID_DELETE_MessageBoard" deletefieldname="MessageId"
                    navigateurl="MessageBoard.aspx"   PageName ="Message" CheckCompany="false" TableName ="MessageBoard" CheckUser="true"  TableAlias="MB"    />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

