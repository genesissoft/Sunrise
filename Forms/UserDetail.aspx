<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="UserDetail.aspx.vb" Inherits="Forms_UserDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc"   %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_UserMaster" runat="server" SelectProcName="ID_SEARCH_UserMaster"
                    SelectedFieldName="UserName" DeleteProcName="ID_DELETE_UserMaster" DeleteFieldName="UserId"
                    NavigateUrl="UserMaster.aspx" PageName="User" TableName="UserMaster" ></uc:ViewScreen>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
