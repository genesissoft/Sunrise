<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="CRMExpectationDetail.aspx.vb" Inherits="Forms_CRMExpectationDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_ExpectationName" runat="server" selectprocname="ID_SEARCH_ExpectationMaster"
                    selectedfieldname="ExpectationName" deleteprocname="ID_DELETE_ExpectationMaster" deletefieldname="ExpectationId"
                    navigateurl="CRMExpectationMaster.aspx"  PageName ="Expectation" TableName ="ExpectationMaster"/>
            </div>
        </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>

