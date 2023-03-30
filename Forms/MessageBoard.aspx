<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="MessageBoard.aspx.vb" Inherits="Forms_MessageBoard" Title="Message Board" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectOptions" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>
    <script type="text/javascript" src="../Include/Common.js"></script>
    <script language="javascript" type="text/javascript">
        function ConvertUCase(txtBox) {
            txtBox.value = txtBox.value.toUpperCase()
        }

        function validate() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_Message").value == "") {
                AlertMessage('Validation', 'Please Enter The Message', 175, 450);
                return false;
            }
            var RegisteredDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_RegisteredDate").value;
            var EndDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_EndDate").value;
            if ((Date.parse(getmdy(RegisteredDate))) > (Date.parse(getmdy(EndDate)))) {

                AlertMessage('Validation', 'End Date can not be less then Registered Date', 175, 450);
                return false;
            }
            var currDate = new Date();
            var hrs = 0, mins;
            if (document.getElementById("ctl00_ContentPlaceHolder1_cbo_ampm").value == "PM") hrs = 12
            hrs = hrs + (document.getElementById("ctl00_ContentPlaceHolder1_cbo_hr").value - 0);
            mins = (document.getElementById("ctl00_ContentPlaceHolder1_cbo_minute").value - 0);
            if ((Date.parse(getmdy(RegisteredDate))) == (Date.parse(getmdy(EndDate)))) {
                if (hrs < currDate.getHours()) {
                    AlertMessage('Validation', "The End time to post a message is not proper", 175, 450)
                    return false
                }
                else if (hrs == currDate.getHours()) {
                    if (mins < currDate.getMinutes()) {
                        AlertMessage('Validation', "The End time to post a message is not proper", 175, 450)
                        return false
                    }
                }
            }
            //           if(currDate>Date.parse(getmdy(EndDate)))
            //           {
            //               alert('End Date can not be less then Today date');
            //               return false;
            //           }         
            if (currDate == Date.parse(getmdy(EndDate))) {
                if (hrs < currDate.getHours()) {
                    AlertMessage('Validation', "The End time to post a message is not proper", 175, 450)
                    return false
                }
                else if (hrs == currDate.getHours()) {
                    if (mins < currDate.getMinutes()) {
                        AlertMessage('Validation', "The End time to post a message is not proper", 175, 450)
                        return false
                    }
                }
            }

            if (Validation() == false) {
                return false;
            }
            return true
        }
        function Delete_entry(strId) {
            if (window.confirm("Are you sure you want to Delete ?")) {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_MessageId").value = strId;
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_Submit").value = "Delete";
                var theForm = document.forms['aspnetForm'];
                theForm.submit();
            }
        }
        function Update(strid) {
            //alert(strid); 
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_MessageId").value = strid;
            //alert(document.getElementById("ctl00_ContentPlaceHolder1_Hid_MessageId").value);
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_Submit").value = "Update";
            var theForm = document.forms['aspnetForm'];
            theForm.submit();
        }

        function CheckList(strName) {
            var lst = document.getElementById("ctl00_ContentPlaceHolder1_srh_" + strName + "_lst_Select")
            if (lst != null) {
                var lstCnt = lst.options.length
                if ((document.getElementById("ctl00_ContentPlaceHolder1_srh_" + strName + "_chk_Select").checked) == false && lstCnt == 0) {
                    if (strName == "Users") {
                        strName = "User"
                    }
                    else {
                        strName = "Branch"
                    }
                    AlertMessage('Validation', 'Please select atleast one ' + strName, 175, 450);
                    return false;
                }
            }
            return true
        }
        function Validation() {
            if (CheckList("Users") == false) return false
            if (CheckList("Branches") == false) return false
            return true
        }

        function msg() {
            AlertMessage("Validation", "This message already exist.", 175, 450);
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr align="left">
                    <td class="SectionHeaderCSS">Message Board</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table cellspacing="0" cellpadding="0" border="0" align="center" width="60%">
                            <tr align="left">
                                <td>Message:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Message" runat="server" Width="435px" CssClass="TextBoxCSS" MaxLength="200"
                                        TextMode="MultiLine" Rows="4"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>Select:
                                </td>
                                <td align="left">
                                    <table cellspacing="0" cellpadding="0" border="0">
                                        <tr align="left">
                                            <td style="padding-left: 0px;">
                                                <uc:SelectOptions ID="srh_Users" runat="server" FormHeight="475" FormWidth="257"
                                                    ProcName="ID_SEARCH_UserMaster" SelectedFieldName="NameOfUser" ChkLabelName="Users"
                                                    LabelName="" SelectedValueName="UM.UserId" SourceType="StoredProcedure" ShowLabel="false"
                                                    width="220">
                                                </uc:SelectOptions>
                                            </td>
                                            <td>
                                                <uc:SelectOptions ID="srh_Branches" runat="server" FormHeight="475" FormWidth="257"
                                                    ProcName="ID_SEARCH_BranchMaster" SelectedFieldName="BranchName" ChkLabelName="Branches"
                                                    LabelName="" SelectedValueName="BranchId" SourceType="StoredProcedure" ShowLabel="false"
                                                    width="220">
                                                </uc:SelectOptions>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>User Type Section:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList ID="rbl_UserTypeSection" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" CssClass="LabelCSS" Height="4px" TabIndex="2">
                                        <asp:ListItem Selected="True" Value="B">Back-Office</asp:ListItem>
                                        <asp:ListItem Value="F">Front-Office</asp:ListItem>
                                        <asp:ListItem Value="O">Both</asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                            <tr align="left">
                                <td>Registered Date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_RegisteredDate" runat="server" Width="115px" CssClass="TextBoxCSS"
                                        MaxLength="50" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>End Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_EndDate" runat="server" CssClass="TextBoxCSS jsdate" TabIndex="13"
                                        Width="115px"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>End Time:
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr align="left">
                                            <td>
                                                <asp:DropDownList ID="cbo_hr" runat="server" Width="50px" CssClass="ComboBoxCSS">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_minute" runat="server" Width="50px" CssClass="ComboBoxCSS">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_ampm" runat="server" Width="50px" CssClass="ComboBoxCSS">
                                                    <asp:ListItem Value="AM">AM</asp:ListItem>
                                                    <asp:ListItem Value="PM">PM</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Update" Visible="false" runat="server" Text="Update" ToolTip="Update"
                                        CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Reset" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
