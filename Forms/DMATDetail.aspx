<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="DMATDetail.aspx.vb" Inherits="Forms_DMATDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_DPName" runat="server" selectprocname="ID_SEARCH_DMATMaster"  DefaultSort="DMatId DESC"  
                    selectedfieldname="DPName" deleteprocname="ID_DELETE_DMATMasters" deletefieldname="DMatId"
                    navigateurl="DMATMaster.aspx"  PageName ="Demat" CheckCompany="true" TableName="DMATMaster"   />
                                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
