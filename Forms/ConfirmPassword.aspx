<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfirmPassword.aspx.cs"
    Inherits="Forms_ConfirmPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Confirm Password</title>
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Style_New.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" language="javascript">
     function OnCancel() {

            window.returnValue = '';
            window.close();

        }
        
         function OnSave() {

            if (window.opener) {
                window.opener.returnValue = 'Yes';
            }
            else {
                window.returnValue = 'Yes';
            }
            window.close();
        }
    </script>

</head>
<body style="margin-left: 0px; margin-top: 5px;">
    <form id="form1" runat="server">
        <div>
            <table cellpadding="0" cellspacing="0" border="0" width="98%" class="data_table"
                align="center">
                <tr align="center">
                    <td class="SectionHeaderCSS" style="text-align: center;">
                        Confirm Password
                    </td>
                </tr>
                <tr class="line_separator">
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="95%">
                            <tr align="left">
                                <td>
                                    Enter Password:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Password" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Re Enter Password:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_RePassword" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
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
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" CssClass="ButtonCSS" OnClick="btn_Save_Click" />
                                    <input type="button" value="Cancel" class="ButtonCSS" onclick="javascript:return OnCancel();" />
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                    <asp:HiddenField ID="Hid_UserId" runat="server" />
                                    <asp:HiddenField ID="Hid_IssueId" runat="server" />
                                    &nbsp;
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
