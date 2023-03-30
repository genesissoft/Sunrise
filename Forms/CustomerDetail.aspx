<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="CustomerDetail.aspx.vb" Inherits="Forms_CustomerDetail" title="Customer Master" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_CustomerName" runat="server" selectprocname="ID_SEARCH_CustomerMaster"
                    selectedfieldname="CustomerName" deleteprocname="ID_DELETE_CustomerMaster" deletefieldname="CustomerId"
                    navigateurl="CustomerMaster.aspx"  PageName ="Customer Master" TableName ="CustomerMaster"/>
            </div>
        </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>

