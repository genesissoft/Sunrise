<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="GroupDetail.aspx.vb" Inherits="Forms_GroupDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_GroupName" runat="server" selectprocname="ID_SEARCH_GroupMaster"
                    selectedfieldname="GroupName" deleteprocname="ID_DELETE_GroupMaster" deletefieldname="GroupId"
                    navigateurl="GroupMaster.aspx"  PageName ="GroupMaster" TableName="GroupMaster"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

