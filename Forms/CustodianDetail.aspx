<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="CustodianDetail.aspx.vb" Inherits="Forms_CustodianDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_CustodianName" runat="server" selectprocname="ID_SEARCH_CustodianMaster"
                    selectedfieldname="CustodianName" deleteprocname="ID_DELETE_CustodianMaster" deletefieldname="CustodianId"
                    navigateurl="CustodianMaster.aspx"  PageName ="Custodian" TableName="CustodianMaster"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

