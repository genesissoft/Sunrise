<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="ExchangeDetail.aspx.vb" Inherits="Forms_ExchangeDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_ExchangeName" runat="server" selectprocname="ID_SEARCH_ExchangeMaster"
                    selectedfieldname="ExchangeName" deleteprocname="ID_DELETE_ExchangeMaster" deletefieldname="ExchangeId"
                    navigateurl="ExchangeMaster.aspx"  PageName ="Exchange" TableName="ExchangeMaster"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

