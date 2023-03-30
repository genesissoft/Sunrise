<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="BrokerDetail.aspx.vb" Inherits="Forms_BrokerDetail" title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">

                 <uc:ViewScreen id="vws_Broker" runat="server" selectprocname="ID_SEARCH_BrokerMaster"
                    selectedfieldname="BrokerName" deleteprocname="ID_DELETE_BrokerMaster" deletefieldname="BM.BrokerId"
                    navigateurl="BrokerMaster.aspx"  PageName ="Broker" CheckCompany="false"  TableName="BrokerMaster" CheckUser ="true" TableAlias ="B"   />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

