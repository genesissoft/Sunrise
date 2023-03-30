<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="GroupMaster.aspx.vb" Inherits="Forms_GroupMaster" Title="Group Master" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectOptions" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function validation() {

            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_GroupName").value == "") {
                AlertMessage('Validation','Please enter Group  Name',175,450);
                return false;
            }
            Validate()
        }

        function CheckList(strName) {
            var lst = document.getElementById("ctl00_ContentPlaceHolder1_srh_" + strName + "_lst_Select")
            if (lst != null) {
                var lstCnt = lst.options.length
                if ((document.getElementById("ctl00_ContentPlaceHolder1_srh_" + strName + "_chk_Select").checked) == false && lstCnt == 0) {
                    if (strName == "Customers") {
                        strName = "Customer"
                    }

                    AlertMessage('Validation', 'Please select atleast one ' + strName,175,450);
                    return false;
                }
            }
            return true
        }
        function Validate() {
            if (CheckList("Customers") == false) return false
            return true
        }


    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="SectionHeaderCSS">Group Master</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="50%">
                            <tr align="left">
                                <td>Group Code:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_GroupCode" runat="server" CssClass="TextBoxCSS" TabIndex="3"
                                        ReadOnly="true"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Customer Type:</td>
                                <td>
                                    <asp:DropDownList ID="cbo_CustomerType" Width="208px" runat="server" CssClass="ComboBoxCSS"
                                        AutoPostBack="True" TabIndex="1">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Group Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_GroupName" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox><i style="color: red">*</i>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Hide:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList ID="rdo_HideShow" runat="server" BorderStyle="None" BorderWidth="1px"
                                        CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="true">
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="SeperatorRowCSS">&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
