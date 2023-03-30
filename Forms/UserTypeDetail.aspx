<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="UserTypeDetail.aspx.vb" Inherits="Forms_UserTypeDetail" title="Untitled Page" %>
 <%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_UserTypeName" runat="server" selectprocname="ID_SEARCH_UserTypeMaster"
                    selectedfieldname="UserTypeName" deleteprocname="ID_DELETE_UserTypeMaster" deletefieldname="UserTypeId"
                    navigateurl="UserTypeMaster.aspx"  PageName ="User Type" TableName="UserTypeMaster"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

