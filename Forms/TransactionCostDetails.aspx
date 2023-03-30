<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="TransactionCostDetails.aspx.vb" Inherits="Forms_TransactionCostDetails" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_TransactionCost" runat="server" selectprocname="ID_SEARCH_TransCostMaster"
                    selectedfieldname="FromDate" deleteprocname="ID_DELETE_TransCostMaster" deletefieldname="TransactionCostId"
                    navigateurl="TransactionCostMaster.aspx"  PageName ="TransactionCost" CheckCompany="false"  TableName="TransactionCostMaster"   />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

