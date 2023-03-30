<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ClientTypeDetail.aspx.vb" Inherits="Forms_ClientTypeDetail" Title="Client Type Detail" %>

<%@ Register Src="../UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc1:ViewScreen id="vws_ClientTypeName" runat="server" selectprocname="ID_SEARCH_ClientTypeMaster"
                    selectedfieldname="ClientType" deleteprocname="ID_DELETE_ClientTypeMaster"
                    deletefieldname="ClientTypeId" navigateurl="ClientTypeMaster.aspx" PageName="ClientTypeMaster"
                    TableName="ClientTypeMaster">
                </uc1:ViewScreen>
                </div>
        </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>
