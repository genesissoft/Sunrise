<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="QuoteDetails.aspx.vb" Inherits="Forms_QuoteDetails" Title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_QuoteEntry" runat="server" SelectProcName="ID_SEARCH_QuoteEntry"
                    SelectedFieldName="QuoteDate" DeleteProcName="ID_DELETE_QuoteList" TableName="QuoteList"
                    ConditionExist="true" DeleteFieldName="QuoteId" NavigateUrl="QuoteEntry.aspx" PageName="QuoteEntry"  >
                </uc:ViewScreen>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
