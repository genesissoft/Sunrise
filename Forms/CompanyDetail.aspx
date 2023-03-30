<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="CompanyDetail.aspx.vb" Inherits="Forms_CompanyDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_CompName" runat="server" selectprocname="ID_SEARCH_Company"
                    selectedfieldname="CompName" deleteprocname="ID_DELETE_CompanyMaster" deletefieldname="CompId"
                    navigateurl="CompanyMaster.aspx"  PageName ="Company" TableName="CompanyMaster"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

