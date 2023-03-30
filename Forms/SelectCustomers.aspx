<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectCustomers.aspx.vb" Inherits="Forms_SelectCustomers" EnableViewStateMac="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Fields</title>

    <base target="_self" />
    <link type="text/css" href="../Include/StanChart.css" rel="stylesheet" />
    <script type="text/javascript" src="../Include/DatePicker.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>

    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>
    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function ReturnValues() {
            var lstBox = document.getElementById("lst_Name");
            var texts = "";
            var values = "";
            for (i = 0; i < lstBox.options.length; i++) {
                texts = texts + lstBox.options[i].text + "!";
                values = values + lstBox.options[i].value + "!";
            }
            if (texts == "") {
                alert("Can not Submit without any selection");
                return false;
            }
          
            document.getElementById("Hid_Values").value=values;
            document.getElementById("Hid_Texts").value = texts;
            document.getElementById("Hid_Id").value = document.getElementById("Hid_Texts").value + "#" + document.getElementById("Hid_Values").value
            window.returnValue = document.getElementById("Hid_Id").value;
          
            window.close();
            //var contactIds = ShowSearch(values, texts)

        }
        function ShowSearch(selValues, seltext) {
            var pageUrl = "SelectCustomerContact.aspx?Values=" + selValues;
            var selValues = ""
            var seltexts = seltext;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:300px;dialogHeight:450px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                var contactIds = ret;
                document.getElementById("Hid_Id").value = document.getElementById("Hid_Texts").value + "$" + document.getElementById("Hid_Values").value + "$" + contactIds;
                window.returnValue = document.getElementById("Hid_Id").value;
                window.close();
            }
        }
        function Close() {
            window.close();
            return false;
        }
        function ValidateSearch() {
            if (document.getElementById("txt_Name").value == "") {
                alert("Please Enter the search text");
                return false;
            }
        }

        function ValidateInsert() {


            var chk = document.getElementById("chkList_Select")
            var blnSelected = false
            var rowCount = (document.getElementById("Hid_RowCount").value - 0);

            for (i = 0; i < rowCount; i++) {
                var chkBox = document.getElementById("chkList_Select_" + i);

                if (chkBox.checked == true) {
                    blnSelected = true
                }
            }
            if (blnSelected == false) {
                alert("Please select atleast one option")
                return false;
            }

        }

        function ValidateRemove() {
            if (document.getElementById("lst_Name").value == "") {
                alert('Please Select record which you want to remove');
                return false;
            }
            return true;
        }
    </script>

</head>
<body topmargin="0" leftmargin="0" class="popupbackground">
    <form id="form1" runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" align="center" height="100%"
            border="0">
            <tr>
                <td align="center">
                    <table cellspacing="0" cellpadding="0" width="100%" align="center" height="100%"
                        border="0">
                        <tr>
                            <td bgcolor="#D1E4F8" align="center" colspan="2" class="HeaderCSS">Select Fields
                            </td>
                        </tr>
                        <tr id="row_CustName" runat="server">
                            <td align="center" colspan="2">
                                <asp:TextBox ID="txt_Name" runat="server" CssClass="TextBoxCSS" MaxLength="50" TabIndex="1"></asp:TextBox>
                                <asp:Button ID="btn_Search" TabIndex="2" runat="server" CssClass="ButtonCSS" Text="Search"></asp:Button>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" colspan="2" height="225px">
                                <div id="divSelection" align="left" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 100%; left: 0px; top: 0px;">
                                    <asp:CheckBoxList runat="server" ID="chkList_Select" RepeatLayout="Flow" CssClass="CheckBoxCSS"
                                        Height="100%">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="2">
                                <asp:Button ID="btn_Insert" runat="server" Text="Insert" CssClass="ButtonCSS"></asp:Button>
                                <asp:Button ID="btn_Remove" runat="server" Text="Remove" CssClass="ButtonCSS"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ListBox ID="lst_Name" runat="server" CssClass="TextBoxCSS" Width="231px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btn_Ret" runat="server" Text="Submit" CssClass="ButtonCSS" UseSubmitBehavior="false" />&nbsp;
                                <asp:Button ID="btn_Close" runat="server" CssClass="ButtonCSS" Text="Close" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="Hid_RowCount" runat="server" />
                    <asp:HiddenField ID="Hid_ReturnFields" runat="server" />
                    <asp:HiddenField ID="Hid_ReturnValues" runat="server" />
                    <asp:HiddenField ID="Hid_IssuerId" runat="server" />
                    <asp:HiddenField ID="Hid_CustomerName" runat="server" />
                    <asp:HiddenField ID="Hid_Values" runat="server" />
                    <asp:HiddenField ID="Hid_Texts" runat="server" />
                    <asp:HiddenField ID="Hid_Id" runat="server" />
                    &nbsp;
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
