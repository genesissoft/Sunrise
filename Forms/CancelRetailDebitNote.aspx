<%@ Page Title="" Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="CancelRetailDebitNote.aspx.vb" Inherits="Forms_CancelRetailDebitNote" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script type="text/javascript">

        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Remark").value) == "") {
                AlertMessage("Validation", "Please Specify Reason of cancellation", 175, 450);
                return false;
            }
            if (window.confirm("Are you sure you want to Cancel this Invoice????")) return true;
            return false;
        }





    </script>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" EnableViewState="true">
    </asp:ScriptManagerProxy>
    <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr align="left">
            <td class="SectionHeaderCSS">Cancel Invoice</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="95%">
                            <tr align="center" valign="top">
                                <td style="width: 49%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" style="width: 100%">

                                        <tr align="left" id="tr_DealNo" runat="server">
                                            <td>Invoice No.:
                                            </td>
                                            <td style="padding-left: 0px;">

                                                <uc:Search ID="Srh_RetailDebitNo" runat="server" PageName="CancelRetailDebitNo" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="RefNo"  ConditionExist="true" FormWidth ="600"
                                                     />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Remark:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Remark" Width="200px" Height="70px" TextMode="MultiLine" runat="server"
                                                    CssClass="TextBoxCSS" TabIndex="21"></asp:TextBox></td>
                                        </tr>
                                        <tr class="line_separator">
                                            <td colspan="3"></td>
                                        </tr>

                                        <tr align="center">
                                            <td colspan="3">
                                                <asp:Label ID="lbl_Deleted" ForeColor="Blue" runat="server" CssClass="LabelCSS"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td colspan="3">
                                                <asp:Button ID="btn_DeleteDeal" runat="server" Text="Cancel" Width="80px" ToolTip="Cancel"
                                                    CssClass="ButtonCSS" TabIndex="29" />
                                            </td>
                                        </tr>

                                        <asp:HiddenField ID="Hid_UserId" runat="server" />
                                        <asp:HiddenField ID="Hid_UserTypeId" runat="server" />
                                          <asp:HiddenField ID="Hid_BrokerId" runat="server" />
                                    </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

