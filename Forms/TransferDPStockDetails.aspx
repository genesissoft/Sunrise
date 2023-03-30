<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="TransferDPStockDetails.aspx.vb" Inherits="Forms_TransferDPStockDetails"
    Title="Transfer DP Stock Details" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript" src="../Include/jquery-1.8.0.min.js"></script>

    <script type="text/javascript">
     $(document).ready(function () {
        //$("[id*='imgBtn_Delete']").hide();
    });
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div style="text-align: center; width: 100%;">
                <uc:ViewScreen ID="vws_TransferDPStock" runat="server" SelectProcName="ID_Fill_DPTransferStockDetails"
                    DefaultSort="DPTransferId DESC" NavigateUrl="TransferDPStock.aspx" PageName="Transfer DP Stock" TableName ="DPTransferStock" DeleteFieldName="DPTransferId"
                    CheckYearCompany="true" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
