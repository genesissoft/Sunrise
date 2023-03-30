<%@ page language="VB" masterpagefile="~/Forms/MasterPage.master"  CodeFile="~/Forms/debitNoteDetails.aspx.vb"  autoeventwireup="false" inherits="Forms_DebitNoteDetails" title="Untitled Page" enableviewstatemac="false" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_DebitNote" runat="server" SelectProcName="MB_SEARCH_DebitNoteEntry" 
                    SelectedFieldName="DebitNoteNo" DeleteProcName="MB_DELETE_DebitNoteEntry" TableName="DebitNoteEntry"
                    DeleteFieldName="DebitNoteId" NavigateUrl="DebitNoteEntry.aspx" PageName="DebitNoteEntry" ConditionExist="true" CheckYearCompany ="true"   />
            </div>
        </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>

