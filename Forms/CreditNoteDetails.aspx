<%@ page language="VB" masterpagefile="~/Forms/MasterPage.master"  CodeFile="~/Forms/creditNoteDetails.aspx.vb"   autoeventwireup="false" inherits="Forms_CreditNoteDetails" title="CreditNoteEntry" enableviewstatemac="false" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_CreditNote" runat="server" SelectProcName="MB_SEARCH_CreditNoteEntry"
                    SelectedFieldName="CreditNoteNo" DeleteProcName="MB_DELETE_CreditNoteEntry" TableName="CreditNoteEntry"
                    DeleteFieldName="CreditNoteId" NavigateUrl="CreditNoteEntry.aspx" PageName="CreditNoteEntry" CheckYearCompany ="true" />
            </div>
        </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>

