<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SGLMasterDetail.aspx.vb" Inherits="Forms_SGLMasterDetail" Title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_SGLBankName" runat="server" SelectProcName="ID_SEARCH_SGLMASTER"
                    SelectedFieldName="BankName" DeleteProcName="ID_DELETE_SGLMaster" DeleteFieldName="SGLId"
                    NavigateUrl="SGLMaster.aspx" PageName="SGLMaster" CheckCompany="true" TableName="SGLMaster"  />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
