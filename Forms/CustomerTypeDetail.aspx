<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="CustomerTypeDetail.aspx.vb" Inherits="Forms_CustomerTypeDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_CustomerTypeName" runat="server" selectprocname="ID_SEARCH_CustomerType"
                    selectedfieldname="CustomerTypeName" deleteprocname="ID_DELETE_CustomerTypeMaster" deletefieldname="CustomerTypeId"
                    navigateurl="CustomerTypeMaster.aspx"  PageName ="CustomerType" TableName="CustomerTypeMaster"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

