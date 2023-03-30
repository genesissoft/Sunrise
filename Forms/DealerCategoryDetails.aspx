<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="DealerCategoryDetails.aspx.vb" Inherits="Forms_DealerCategoryDetails" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_DealerCategory" runat="server" selectprocname="ID_SEARCH_DealerCategoryMaster"
                    selectedfieldname="DealerCategoryName" deleteprocname="ID_DELETE_DealerCategoryMaster" deletefieldname="DealerCategoryId"
                    navigateurl="DealerCategoryMaster.aspx"  PageName ="DealerCategory" TableName ="DealerCategoryMaster"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

