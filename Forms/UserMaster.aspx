<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="UserMaster.aspx.vb" Inherits="Forms_UserMaster" Title="User Master" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">
        function Validation(flag) {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_usertype").value) == "") {
                AlertMessage('Validation', 'Please Select User Type Name.', 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_nameofuser").value) == "") {
                AlertMessage('Validation', 'Please Enter Name Of User.', 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_Branch").value) == "") {
                AlertMessage('Validation', 'Please Select Branch.', 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_loginname").value) == "") {
                AlertMessage('Validation', 'Please Enter Loginname.', 175, 450);
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_Password").value != document.getElementById("ctl00_ContentPlaceHolder1_txt_ConfirmPassword").value) {
                AlertMessage('Validation', 'Your Password and Re-entered Password Does Not Matched.', 175, 450);
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_EmailId").value != "") {
                if (Email(document.getElementById("ctl00_ContentPlaceHolder1_txt_EmailId").value) == false) {
                    AlertMessage('Validation', 'Please enter valid Email.', 175, 450);
                    return false;
                }
            }
            var strChg = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ChangePwd").value
            if (flag == 'S' || strChg == "none") {
                if (ValidatePassword() == false) return false
            }
        }


        function ChangePassword(strFlag1, strFlag2) {
            document.getElementById("row_Password").style.display = strFlag1;
            document.getElementById("row_ConfirmPassword").style.display = strFlag1;
            document.getElementById("ctl00_ContentPlaceHolder1_Btn_ChangePassword").style.display = strFlag2;
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_ChangePwd").value = strFlag2;
            return false
        }

        function ValidatePassword() {
            var strLogin = document.getElementById('ctl00_ContentPlaceHolder1_txt_loginname').value;
            var strPwd = document.getElementById('ctl00_ContentPlaceHolder1_txt_Password').value;
            var intMinChars = document.getElementById('ctl00_ContentPlaceHolder1_Hid_MinChars').value;
            var strFormat = document.getElementById('ctl00_ContentPlaceHolder1_Hid_Format').value;
            var strContainsLogin = document.getElementById('ctl00_ContentPlaceHolder1_Hid_ContainsLogin').value;
            // *************************************************************************************************
            // For Minimum Password Characters
            // *************************************************************************************************
            if (strPwd.length < intMinChars) {
                ShowMsg(intMinChars, strFormat);
                return false
            }
            // *************************************************************************************************
            // For Checking whether the Password Contains the login name
            // *************************************************************************************************
            if (strContainsLogin == "False") {
                if (strPwd.indexOf(strLogin) != -1 && strLogin != '') {
                    ShowMsg(intMinChars, strFormat);
                    return false
                }
            }
            // *************************************************************************************************
            // For Checking Whether the Format of the Password
            // *************************************************************************************************
            var blnNums = false;
            var blnAlphas = false;
            var blnSplChr = false;
            for (i = 0; i < strPwd.length; i++) {
                var chrAsc = (strPwd.charCodeAt(i) - 0)
                if (chrAsc >= 48 && chrAsc <= 57) blnNums = true;
                if ((chrAsc >= 65 && chrAsc <= 90) || (chrAsc >= 97 && chrAsc <= 122)) blnAlphas = true;
                if ((chrAsc >= 33 && chrAsc <= 47) || (chrAsc >= 58 && chrAsc <= 64) || (chrAsc >= 91 && chrAsc <= 96) || (chrAsc >= 123 && chrAsc <= 126)) blnSplChr = true;
            }
            if (strFormat == "ALP") {
                if (blnAlphas == false || blnNums == true || blnSplChr == true) {
                    ShowMsg(intMinChars, strFormat);
                    return false
                }
            }
            if (strFormat == "ANU") {
                if (blnAlphas == false || blnNums == false || blnSplChr == true) {
                    ShowMsg(intMinChars, strFormat);
                    return false
                }
            }
            if (strFormat == "ANS") {
                if (blnAlphas == false || blnNums == false || blnSplChr == false) {
                    ShowMsg(intMinChars, strFormat);
                    return false
                }
            }
            // *************************************************************************************************
        }
        function ShowMsg(intMinChars, strFormat) {
            var strFormatText = ''
            if (strFormat == "ALP") strFormatText = "Only Alphbets"
            if (strFormat == "ANU") strFormatText = "Alphabets & Numerics"
            if (strFormat == "ANS") strFormatText = "Alphabets, Numerics & Atleast One Special Character"
            AlertMessage('Validation', 'The Password entered is not in a correct format.\nThe Password needs to be minimum ' + intMinChars + ' characters in length.\nThe Password can not contain the login name.\nThe Password needs to be ' + strFormatText, 190, 550);
        }


        function ShowList(fieldName, valueName, procName, selValues) {
            var ret = ShowDialogOpen("SelectUsers.aspx?FieldName=" + fieldName + "&ValueName=" + valueName + "&ProcName=" + procName + "&SelectedValues=" + selValues, "250px", "498px")
            return ret
        }

    </script>

    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">User Master</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table width="90%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr align="center" valign="top">
                        <td style="width: 49%;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                                <ContentTemplate>
                                    <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tr align="left">
                                            <td>User Type :</td>
                                            <td>
                                                <asp:DropDownList ID="cbo_usertype" runat="server" CssClass="ComboBoxCSS" Width="208px"
                                                    DataTextField="UserTypeName" TabIndex="1">
                                                </asp:DropDownList><i style="color: Red; vertical-align: super;">*</td>
                                        </tr>
                                        <tr align="left">
                                            <td>Name Of User:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_nameofuser" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="2"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Employee Code:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_EmployeeCode" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="Tr1" align="left" runat="server">
                                            <td>Unique Code:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_UniqueCode" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Branch:</td>
                                            <td align="left">
                                                <asp:DropDownList ID="cbo_Branch" runat="server" Width="208px" DataTextField="Branch"
                                                    TabIndex="3" CssClass="ComboBoxCSS">
                                                </asp:DropDownList><i style="color: Red; vertical-align: super;">*</td>
                                        </tr>
                                        <tr align="left">
                                            <td>Managed By:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cbo_ManagedBy" runat="server" Width="208px" DataTextField="Branch"
                                                    TabIndex="3" CssClass="ComboBoxCSS">
                                                </asp:DropDownList>
                                                <i style="color: Red; vertical-align: super;">*
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Status:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_Status" runat="server" CssClass="ComboBoxCSS" Width="208px"
                                                    DataTextField="UserTypeName" TabIndex="4">
                                                    <asp:ListItem Text="Active" Value="A" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Inactive" Value="I"></asp:ListItem>
                                                    <asp:ListItem Text="Disable" Value="D"></asp:ListItem>
                                                </asp:DropDownList><i style="color: Red; vertical-align: super;">*</td>
                                        </tr>
                                        <tr align="left">
                                            <td>Login Name:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_loginname" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="9"></asp:TextBox><i style="color: Red; vertical-align: super;">*</td>
                                        </tr>
                                        <tr align="left" runat="server" id="row_btn">
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Button ID="Btn_ChangePassword" runat="server" Text="ChangePwd" CssClass="ButtonCSS"
                                                    TabIndex="9" Width="90px" /></td>
                                        </tr>
                                        <tr align="left" id="row_Password">
                                            <td>Password:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Password" TextMode="Password" runat="server" CssClass="TextBoxCSS"
                                                    Width="200px" TabIndex="10"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                            </td>
                                        </tr>
                                        <tr align="left" id="row_ConfirmPassword">
                                            <td>Confirm Password:</td>
                                            <td>
                                                <asp:TextBox ID="txt_ConfirmPassword" TextMode="Password" runat="server" CssClass="TextBoxCSS"
                                                    Width="200px" TabIndex="11"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 2%;">&nbsp;</td>
                        <td style="width: 49%;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" Mode="Conditional">
                                <ContentTemplate>
                                    <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tr align="left" runat="server">
                                            <td>Joining Date:
                                            </td>
                                            <td>
                                                <%-- <asp:TextBox ID="txt_JoiningDate" runat="server" CssClass="TextBoxCSS" Width="115px"
                                                    TabIndex="1"></asp:TextBox><img class="calender" id="fromCalendar" src="../Images/Calender.jpg" />--%>
                                                <asp:TextBox ID="txt_JoiningDate" runat="server" CssClass="TextBoxCSS jsdate" Width="115px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Phone No:</td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_PhoneNo" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="5"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Mobile No:</td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_MobileNo" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="6"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Fax No:</td>
                                            <td>
                                                <asp:TextBox ID="txt_FaxNo" runat="server" CssClass="TextBoxCSS" Width="200px" TabIndex="7"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Email Id:</td>
                                            <td>
                                                <asp:TextBox ID="txt_EmailId" runat="server" TabIndex="8" CssClass="TextBoxCSS" Width="200px"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left" valign="middle" runat="server" visible="false">
                                            <td>DealerCategoryName:
                                                <%--  <asp:Label ID="lbl_DealerName" runat="server" Text="DealerCategoryName: " CssClass="LabelCSS"
                                                    Width="100px"></asp:Label>--%>
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:SelectFields ID="srh_CategoryName" runat="server" ProcName="ID_SEARCH_DealerCategoryName"
                                                    FormHeight="470" FormWidth="257" width="210" Height="70" SelectedValueName="DealerCategoryId"
                                                    ChkLabelName="" ShowAll="true" LabelName="" SelectedFieldName="DealerCategoryName"
                                                    SourceType="StoredProcedure" Visible="true" ShowLabel="false"></uc:SelectFields>
                                            </td>
                                        </tr>
                                        <tr align="left" valign="middle" runat="server" visible="true">
                                            <td>Company:
                                               
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:SelectFields ID="srh_Company" runat="server" ProcName="ID_SEARCH_DealerCompany"
                                                    FormHeight="470" FormWidth="257" width="210" Height="70" SelectedValueName="CompId"
                                                    ChkLabelName="" ShowAll="true" LabelName="" SelectedFieldName="CompName"
                                                    SourceType="StoredProcedure" Visible="true" ShowLabel="false" ConditionExist="true"></uc:SelectFields>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr class="line_separator">
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="3">
                            <asp:HiddenField ID="Hid_ChangePwd" runat="server" />
                            <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                            <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                            <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                            <asp:HiddenField ID="Hid_MinChars" runat="server" />
                            <asp:HiddenField ID="Hid_ContainsLogin" runat="server" />
                            <asp:HiddenField ID="Hid_Format" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
