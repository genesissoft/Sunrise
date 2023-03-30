<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SuperChangePassword.aspx.vb" Inherits="Forms_SuperChangePassword" Title="ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">
     function Validation()
        {     
//            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_OldPassword").value) == "")
//            {
//                alert('Please Enter Old Password');
//                return false;
//            }
            if (document.getElementById("ctl00$ContentPlaceHolder1$cbo_PasswordType").selectedIndex == 0)
            {
                alert('Please Select Password Change For dropdown');
                return false;
            }
             if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_NewPassword").value) == "")
            {
                alert('Please Enter New Password');
                return false;
            }
             if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_ConfirmPassword").value) == "")
            {
                alert('Please Enter Confirm Password');
                return false;
            }
            
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_NewPassword").value != document.getElementById("ctl00_ContentPlaceHolder1_txt_ConfirmPassword").value)
            {
                alert('Your Password and Reentered Password Does Not Matched');
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_OldPassword").value == document.getElementById("ctl00_ContentPlaceHolder1_txt_NewPassword").value)
            {
                alert('Your New Password can not be same as the Old password');
                return false;
            }
            if(ValidatePassword()==false) return false
        }   
        
        
        function ValidatePassword()
        {
            var strLogin = document.getElementById('ctl00_ContentPlaceHolder1_Hid_loginname').value;
            var strPwd = document.getElementById('ctl00_ContentPlaceHolder1_txt_ConfirmPassword').value;
            var intMinChars = document.getElementById('ctl00_ContentPlaceHolder1_Hid_MinChars').value;
            var strFormat = document.getElementById('ctl00_ContentPlaceHolder1_Hid_Format').value;
            var strContainsLogin = document.getElementById('ctl00_ContentPlaceHolder1_Hid_ContainsLogin').value;
            // *************************************************************************************************
            // For Minimum Password Characters
            // *************************************************************************************************
            if( strPwd.length < intMinChars )
            {
                ShowMsg(intMinChars,strFormat);
                return false
            }
            // *************************************************************************************************
            // For Checking whether the Password Contains the login name
            // *************************************************************************************************
            if( strContainsLogin == "False")
            {
                if(strPwd.toLowerCase().indexOf(strLogin.toLowerCase()) != -1 && strLogin != '')
                {
                    ShowMsg(intMinChars,strFormat);
                    return false
                }
            }
            // *************************************************************************************************
            // For Checking Whether the Format of the Password
            // *************************************************************************************************
            var blnNums = false;
            var blnAlphas = false;
            var blnSplChr = false;         
            for(i=0; i<strPwd.length; i++)
            {
                var chrAsc = (strPwd.charCodeAt(i)-0)
                if(  chrAsc >= 48 && chrAsc <= 57 ) blnNums = true;
                if( (chrAsc >= 65 && chrAsc <= 90) || (chrAsc >= 97 && chrAsc <= 122) ) blnAlphas = true;
                if( (chrAsc >= 33 && chrAsc <= 47) || (chrAsc >= 58 && chrAsc <= 64) || (chrAsc >= 91 && chrAsc <= 96) || (chrAsc >= 123 && chrAsc <= 126) ) blnSplChr = true;
            }
            if(strFormat == "ALP")
            {
                if(blnAlphas == false || blnNums == true || blnSplChr == true)
                {
                    ShowMsg(intMinChars,strFormat);
                    return false
                }
            }
            if(strFormat == "ANU")
            {
                if(blnAlphas == false || blnNums == false || blnSplChr == true)
                {
                    ShowMsg(intMinChars,strFormat);
                    return false
                }
            }
            if(strFormat == "ANS")
            {
                if(blnAlphas == false || blnNums == false || blnSplChr == false)
                {
                    ShowMsg(intMinChars,strFormat);
                    return false
                }
            }
            // *************************************************************************************************
        }
        function ShowMsg(intMinChars,strFormat)
        {
            var strFormatText = '' 
            if(strFormat == "ALP") strFormatText = "Only Alphbets"
            if(strFormat == "ANU") strFormatText = "Alphabets & Numerics"
            if(strFormat == "ANS") strFormatText = "Alphabets, Numerics & Atleast One Special Character"
            alert('The Password entered is not in a correct format.\nThe Password needs to be minimum ' + intMinChars + ' characters in length.\nThe Password can not contain the login name.\nThe Password needs to be ' + strFormatText)
        } 
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Change Super User Password</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table align="center" cellspacing="0" cellpadding="0" border="0" width="40%">
                            <tr align="left">
                                <td>
                                    Password Change For:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cbo_PasswordType" runat="server" CssClass="ComboBoxCSS" Width="208px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Old Password:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_OldPassword" runat="server" CssClass="TextBoxCSS" Width="200px"
                                        TextMode="Password" MaxLength="20"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    New Password:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NewPassword" runat="server" CssClass="TextBoxCSS" Width="200px"
                                        TextMode="Password" MaxLength="20"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Confirm Password:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_ConfirmPassword" runat="server" CssClass="TextBoxCSS" Width="200px"
                                        TextMode="Password" MaxLength="20"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="Hid_ChangePwd" runat="server" />
                        <asp:HiddenField ID="Hid_loginname" runat="server" />
                        <asp:HiddenField ID="Hid_MinChars" runat="server" />
                        <asp:HiddenField ID="Hid_Format" runat="server" />
                        <asp:HiddenField ID="Hid_ContainsLogin" runat="server" />
                        <asp:HiddenField ID="Hid_LastPasswordCnt" runat="server" />
                        <asp:HiddenField ID="Hid_password" runat="server" />
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
