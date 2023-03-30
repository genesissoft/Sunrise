<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="BusinessTypeDetail.aspx.vb" Inherits="Forms_BusinessTypeDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_BusinessTypeDetail" runat="server" SelectProcName="ID_SEARCH_BusinessTypeMaster"
                    SelectedFieldName="BusinessType" DeleteProcName="ID_DELETE_BusinessTypeMaster" DeleteFieldName="BusinessTypeId"
                    NavigateUrl="BusinessType.aspx" PageName="BusinessTypeMaster"  TableName="BusinessTypeMaster"  />
            </div>
        </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>

