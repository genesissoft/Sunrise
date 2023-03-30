<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewContactsAppEntry.aspx.vb"
    Inherits="Forms_ViewContactsAppEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script language="javascript" src="../Include/DatePicker.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">

    </script>

    <title>View Contact</title>
</head>
<body>
    <form id="Form1" runat="server">
        <div>
            <table id="OVERLORD_Contact" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="HeaderCSS" align="Center">
                        View Contacts
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table id="Main" cellpadding="3" cellspacing="0" width="100%" border="0" bordercolor="black">
                            <tr class="AlternateRowCSS" >
                                <td class="LabelCSS" width="30%">
                                    &nbsp;Contact Name:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_ContactName" Text="" runat="server" CssClass="LabelCSSS"></asp:Label>
                                </td>
                            </tr>
                            <tr >
                                <td class="LabelCSS">
                                    &nbsp;Designation:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_Designation" Text="" runat="server" CssClass="LabelCSSS"></asp:Label>
                                </td>
                            </tr>
                            <tr class="AlternateRowCSS">
                                <td class="LabelCSS">
                                    &nbsp;Direct No:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_DirectNo" Text="" runat="server" CssClass="LabelCSSS"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    &nbsp;Mobile No:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_MobileNo" Text="" runat="server" CssClass="LabelCSSS"></asp:Label>
                                </td>
                            </tr>
                            <tr class="AlternateRowCSS">
                                <td class="LabelCSS">
                                    &nbsp;Board No:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_BoardNo" Text="" runat="server" CssClass="LabelCSSS"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    &nbsp;Interaction:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_Interaction" Text="" runat="server" CssClass="LabelCSSS"></asp:Label>
                                </td>
                            </tr>
                            <tr class="AlternateRowCSS">
                                <td class="LabelCSS">
                                    &nbsp;Branch:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_Branch" Text="" runat="server" CssClass="LabelCSSS"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    &nbsp;Section:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_Section" Text="" runat="server" CssClass="LabelCSSS"></asp:Label>
                                </td>
                            </tr>
                            <tr class="AlternateRowCSS">
                                <td class="LabelCSS">
                                    &nbsp;Instrument:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_Instrument" Text="" runat="server" CssClass="LabelCSSS"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" class="LabelCSS" style="text-align:center;">
                                    Additional Details:
                                </td>
                            </tr>
                            <tr class="AlternateRowCSS">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lbl_AdditionalContact" Text="" runat="server" CssClass="LabelCSSS"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="HeaderCSS">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
