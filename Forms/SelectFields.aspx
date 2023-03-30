<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectFields.aspx.vb" Inherits="Forms_SelectFields"
    EnableViewStateMac="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Fields</title>
    <base target="_self" />
    <link type="text/css" href="../Include/Stanchart.css" rel="stylesheet" />
    <link type="text/css" href="../Include/Style_New.css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/Common.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <style type="text/css">
        html {
            scrollbar-base-color: #AAB08A;
        }
        .hidden
        {
            display:none;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            $("[id*=chkAll]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=chkList_Select] input").attr("checked", "checked");
                } else {
                    $("[id*=chkList_Select] input").removeAttr("checked");
                }
            });
            $("[id*=chkList_Select] input").bind("click", function () {
                if ($("[id*=chkList_Select] input:checked").length == $("[id*=chkList_Select] input").length) {
                    $("[id*=chkAll]").attr("checked", "checked");
                } else {
                    $("[id*=chkAll]").removeAttr("checked");
                }
            });
        });
    </script>


    <script type="text/javascript">
        function ReturnValues() {
            //            alert('hi')
            var lstBox = document.getElementById("lst_Name");
            var texts = "";
            var values = "";
            for (i = 0; i < lstBox.options.length; i++) {
                texts = texts + lstBox.options[i].text + "!";
                values = values + lstBox.options[i].value + "!";
            }
            document.getElementById("Hid_Id").value = texts + "|" + values;
            window.returnValue = texts + "|" + values;
            window.close();
        }
        function Close() {
            debugger;
            document.getElementById("Hid_Id").value = "";
            window.returnValue = document.getElementById("Hid_Id").value;
            window.close();
        }
        function ValidateSearch() {
            if (document.getElementById("txt_Name").value == "") {
                AlertMessage('Validation', "Please Enter the search text", 175, 450);
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
                alert("Please Select record which you want to remove");
                return false;
            }
            return true;
        }

        function CheckOnlyOne(e) {
            if (!e) e = window.event;
            var sender = e.target || e.srcElement;
            if (sender.nodeName != 'INPUT') return;
            var checker = sender;
            var chkBox = document.getElementById('<%= chkList_Select.ClientID %>'); // give checkboxlist name
            var chks = chkBox.getElementsByTagName('INPUT');
            for (i = 0; i < chks.length; i++) {
                if (chks[i] != checker)
                    chks[i].checked = false;
            }
        }

    </script>

</head>
<body class="popupbackground">
    <form id="form1" runat="server">
        <table cellspacing="0" cellpadding="0" width="40%" align="center" height="100%"
            border="0" class="data_table">
            <tr align="center" valign="top">
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%" align="center" height="98%" border="0"
                        class="table_border_right_bottom">
                        <tr>
                            <td align="center" colspan="2" class="HeadingCenter">Select Fields
                            </td>
                        </tr>
                        <tr id="row_CustName" runat="server">
                            <td align="center" colspan="2">
                                <asp:TextBox ID="txt_Name" runat="server" CssClass="TextBoxCSS" MaxLength="50" TabIndex="1"></asp:TextBox>
                                <asp:Button ID="btn_Search" TabIndex="2" runat="server" CssClass="ButtonCSS" Text="Search"></asp:Button>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" colspan="2">
                                <asp:CheckBox ID="chkAll" runat="server" CssClass="TextBoxCSS hidden" Text="Select All" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" colspan="2" height="265px">
                                <div id="divSelection" align="left" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 100%; left: 0px; top: 0px;">
                                    <asp:CheckBoxList runat="server" ID="chkList_Select" RepeatLayout="Flow" CssClass="TextBoxCSS"
                                        Height="100%" Width="95%">
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
                        <tr style="height: 5px;">
                            <td style="border-bottom: 0px; border-top: 0px;"></td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btn_Submit" runat="server" Text="Submit" CssClass="ButtonCSS" />&nbsp;
                            <asp:Button ID="btn_Close" runat="server" CssClass="ButtonCSS hidden" Text="Close" />
                               <input type="button" id="btn_Cancel" runat="server" class="ButtonCSS" value="Close" onclick ="return Close();" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="Hid_RowCount" runat="server" />
                    <asp:HiddenField ID="Hid_ReturnFields" runat="server" />
                    <asp:HiddenField ID="Hid_ReturnValues" runat="server" />
                    <asp:HiddenField ID="Hid_IssuerId" runat="server" />
                    <asp:HiddenField ID="Hid_PageName" runat="server" />
                    <asp:HiddenField ID="Hid_Id" runat="server" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
