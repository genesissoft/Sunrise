<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Forms_Login"
    EnableSessionState="true" %>

<%--<%@ Register Assembly="Login" Namespace="Login.UserSecurity" TagPrefix="cc" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="margin: 0; padding: 0;">
<head id="Head1" runat="server">
    <title>Login Page</title>
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <link href="../include/loginScreen.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="../Include/jquery-1.9.1.js" language="javascript"></script>
    <script type="text/javascript" src="../Include/Common.js"></script>

   <script type="text/javascript" language="javascript">
       $(document).ready(function () {
               $("input:text, input:password").keypress(function () {
                   return ValidateUserInput(this, event);
               });

               $("input:text, input:password").blur(function () {
                   GetValidUserInput(this);
               });
           });
    </script>
</head>
<body class="loginBody" onload="Focus();" style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
        <div id="pageBox" style="margin-top: 10%;">
            <table border="0" align="center" width="450px" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" valign="middle">
                        <table width="100%" border="0" cellspacing="2" cellpadding="2" style="background-color: white ; border-radius: 10px; border: 5px solid #007e9e; border-radius: 10px; -webkit-box-shadow: 6px 5px 5px 0px #83836f; -moz-box-shadow: 6px 5px 5px 0px #83836f; box-shadow: 6px 5px 5px 0px #83836f;">
                            <tr>
                                <td colspan="2" align="left" style="padding-bottom: 1px; border-bottom: 2px solid #007e9e;">
                                       <img src="../Images/CompanyLogo.png" height="80" width="300" /><br />
                                </td>
                            </tr>
                            <tr align="center" valign="top">
                                <td colspan="2" style="padding: 10px;">
                     <table cellspacing="5" cellpadding="5" width="80%" border="0" align="center">
                              
                                <tr align="left">
                                  <td>
                                  Username:
                                  </td>
                                <td>
                                  <asp:TextBox id="UserName" runat="server" CssClass="frmTextBox" MaxLength="50"></asp:TextBox>
                                  <asp:RequiredFieldValidator id="UserNameRequired" runat="server" CssClass="frmLabel" ErrorMessage="User Name is required." ToolTip="User Name is required." ControlToValidate="UserName" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                  </td>
                                </tr>
                              
                                <tr  align="left">
                                  <td>
                                  Password:
                                  </td>
                               
                                  <td>
                                  <asp:TextBox id="Password" runat="server" CssClass="frmTextBox" TextMode="Password" MaxLength="20"></asp:TextBox>
                                  <asp:RequiredFieldValidator id="PasswordRequired" runat="server" CssClass="frmLabel" ErrorMessage="Password is required." ToolTip="Password is required." ControlToValidate="Password" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                  </td>
                                </tr>
                                 <tr align="left">                                 <td>&nbsp;</td>
                                  
                                  <td>
                                  <asp:CheckBox id="RememberMe" runat="server" CssClass="frmLabel" Text="Remember me"></asp:CheckBox>
                                  </td>
                                </tr>
                                <tr align="left">
                                  <td>&nbsp;</td>
                                  <td>
                                  <asp:Button id="btnLogin" runat="server" CssClass="ButtonCSS" Text="Login" 
                                          ValidationGroup="Login1" CommandName="Login"></asp:Button>
                                  </td>
                                </tr>
                                <tr align="center">
                                  <td colspan="2">
                                      <asp:Label ID="lbl_Error" runat="server" ForeColor="Red"></asp:Label>
                                  </td>
                                </tr>
                            </table>
                                    <asp:Label ID="lbl_AttemptCount" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="2" valign="bottom" style="padding: 5px; font-size: 0.8em;">Copyright <b>&copy;</b> 2020 Genesis Software
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
