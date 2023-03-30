<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectCustomerContact.aspx.vb" Inherits="Forms_SelectCustomerContact" EnableViewStateMac="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Fields</title>
    <base target="_self" />
    <link type="text/css" href="../Include/StanChart.css" rel="stylesheet" />
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Close() {
            var values = "";
            var checked_checkboxes = $("[id*=chkList_Select] input:checked");
            var message = "";
            checked_checkboxes.each(function () {
                values += $(this).val();
                values = 53 + "!"
                var text = $(this).closest("td").find("label").html();
            });

            values = values + "$" + document.getElementById("Hid_ReturnValues").value
            document.getElementById("Hid_Id").value = values;
            window.returnValue = document.getElementById("Hid_Id").value;
            window.close();
        }
        function ValidateSearch() {
            if (document.getElementById("txt_Name").value == "") {
                alert("Please Enter the search text");
                return false;
            }
        }

    </script>

</head>
<body topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" align="center" height="100%"
            border="0">
            <tr>
                <td align="center">
                    <table cellspacing="0" cellpadding="0" width="100%" align="center" height="100%"
                        border="0">
                        <tr>
                            <td bgcolor="#D1E4F8" align="left" colspan="2" class="HeaderCSS">Select Fields
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" colspan="2" height="225px">
                                <div id="divSelection" align="left" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 100%; left: 0px; top: 0px;">
                                    <asp:CheckBoxList runat="server" ID="chkList_Select" RepeatLayout="Table" CssClass="CheckBoxCSS"
                                        Height="100%">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btn_Submit" runat="server" Text="Submit" CssClass="ButtonCSS hidden" />&nbsp;
                                <input type="button" id="btn_Ret" runat="server" class="ButtonCSS" value="Submit" onclick="return Close();" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="Hid_RowCount" runat="server" />
                    <asp:HiddenField ID="Hid_ReturnFields" runat="server" />
                    <asp:HiddenField ID="Hid_ReturnValues" runat="server" />
                    <asp:HiddenField ID="Hid_IssuerId" runat="server" />
                    <asp:HiddenField ID="Hid_CustomerName" runat="server" />
                    <asp:HiddenField ID="Hid_Id" runat="server" />
                    &nbsp;
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
