<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="FormatMaster.aspx.vb" Inherits="Forms_FormatMaster" Title="Format Master" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_FormatName").value) == "") {
                AlertMessage('Validation', "Please Enter Format Name", 175, 450);
                return false;
            }
        }

        function ShowSelectFields() {
            var selValues = ""
            var cboUsers = document.getElementById("ctl00_ContentPlaceHolder1_lst_SelectFields")

            for (i = 0; i < cboUsers.options.length; i++) {
                selValues = selValues + cboUsers.options[i].value + ","
            }
            var fieldName = "FaxField";
            var valueName = "FieldId";
            var procName = "ID_FILL_ClientType";
            var OpenUrl = "SelectionList.aspx?FieldName=" + fieldName + "&ValueName=" + valueName + "&ProcName=" + procName + "&SelectedValues=" + selValues + "&Form=Format";
            var ret = window.showModalDialog(OpenUrl, 'some argument', 'dialogWidth:600px;dialogHeight:450px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                var strRetValues = ret
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_FieldId").value = strRetValues
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaxField").value = strRetValues
                document.getElementById('<%= lnk_SelectFields.ClientID%>').click();
            }
            else {
            }
        }

        function ShowList(fieldName, valueName, procName, selValues) {
            var ret = ShowDialogOpen("SelectionList.aspx?FieldName=" + fieldName + "&ValueName=" + valueName + "&ProcName=" + procName + "&SelectedValues=" + selValues + "&Form=Format", "450px", "498px")
            return ret
        }

        function ShowDialogOpen(PageName, strWidth, strHeight) {
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=Yes;";
            var OpenUrl = PageName;
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
            return ret
        }


    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr align="left">
                    <td class="SectionHeaderCSS">Format Master</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table id="Table3" cellspacing="0" cellpadding="0" border="0" align="center" width="50%">
                            <tr align="left">
                                <td align="left">Format Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_FormatName" runat="server" Width="245px" CssClass="TextBoxCSS"
                                        MaxLength="20"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Select Client:
                                </td>
                                <td>
                                    <uc:SelectFields ID="srh_Client" runat="server" FormHeight="475" FormWidth="257"
                                        ProcName="ID_SEARCH_ClientMaster" SelectedFieldName="CustomerName" ChkLabelName="Customers"
                                        LabelName="" SelectedValueName="CustomerId" SourceType="StoredProcedure" ShowLabel="false">
                                    </uc:SelectFields>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Select Fields:
                                </td>
                                <td style="padding: 0px;">
                                    <table cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td align="center">
                                                <a id="btn_SelectFields" href="javascript:;" runat="server" onclick="return ShowSelectFields();">Add &amp; Remove</a>
                                                <asp:LinkButton ToolTip="Add &amp; Remove" CssClass="InfoLinkCSS hidden" ID="lnk_SelectFields"
                                                    runat="server" Text="Add &amp; Remove" CausesValidation="false" TabIndex="7"></asp:LinkButton>
                                                <input type="button" id="btn_AddFields" runat="server" value="Add &amp; Remove" class="InfoLinkCSS hidden" onclick="return ShowSelectFields();" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:ListBox ID="lst_SelectFields" runat="server" CssClass="TextBoxCSS" Width="250px"
                                                    TabIndex="8" Height="80px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2"></td>
                            </tr>
                            <tr align="left">
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Update" Visible="false" runat="server" Text="Update" ToolTip="Update"
                                        CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%-- <tr>
                    <td class="SectionHeaderCSS" align="left" colspan="6">
                        Select Fields Section  
                        
                        
                        <uc:SelectFields ID="srh_SelectFields" class="LabelCSS" runat="server" ProcName="ID_SEARCH_FaxFields"
                                        FormHeight="470" FormWidth="257" SelectedValueName="FieldId" ChkLabelName=""
                                        ConditionalFieldId="" LabelName="" SelectedFieldName="FaxField" SourceType="StoreProcedure"
                                        ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                </tr>
                <tr>
                    <td colspan="6" class="SeperatorRowCSS">
                    </td>
                </tr>--%>
                <%-- <tr>
                    <td align="center" colspan="2">
                        <table align="center" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td align="left">
                                    <asp:CheckBoxList ID="chkList_Fields" runat="server" RepeatDirection="Horizontal"
                                        RepeatColumns="4" CssClass="TextBoxCSS" TabIndex="13">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>--%>
                <asp:HiddenField runat="server" ID="Hid_FaxField" />
                <asp:HiddenField runat="server" ID="Hid_FieldId" />
                <asp:HiddenField runat="server" ID="Hid_Flag" />
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
