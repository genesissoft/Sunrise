<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="BankDetail.aspx.vb" Inherits="Forms_BankDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_BankName" runat="server" selectprocname="ID_SEARCH_BankMaster"
                    selectedfieldname="BankName" deleteprocname="ID_DELETE_BankMaster" deletefieldname="BankId"
                    navigateurl="BankMaster.aspx"  PageName ="Bank" CheckCompany="true"  TableName="BankMaster"   />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

