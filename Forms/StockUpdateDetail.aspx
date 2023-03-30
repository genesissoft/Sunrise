<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="StockUpdateDetail.aspx.vb" Inherits="Forms_StockUpdateDetail" title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <%--<atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">--%>
        <%--<ContentTemplate>--%>
            <div align="center">
                <uc:ViewScreen ID="vws_StockUpdate" runat="server" SelectProcName="ID_SEARCH_StockUpdateMaster"
                    SelectedFieldName="SecurityName" DeleteProcName="ID_DELETE_StockUpdateMaster" TableName="StockUpdateMaster"
                    DeleteFieldName="StockUpdtId" NavigateUrl="StockUpdate.aspx" PageName="StockUpdateMaster" DefaultSort="Type"   >
                </uc:ViewScreen>
            </div>
        <%--</ContentTemplate>--%>
<%--    </atlas:UpdatePanel>--%>
</asp:Content>

