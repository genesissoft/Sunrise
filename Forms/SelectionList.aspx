<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectionList.aspx.vb" Inherits="Forms_SelectionList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Field Selection</title>
    <link type="text/css" href="../Include/General.css" rel="Stylesheet" />
    <script language="javascript" src="../Include/Common.js" type="text/javascript"></script>
    <style type="text/css">
        html {
            scrollbar-base-color: #AAB08A;
        }

        .hidden {
            display: none;
        }
    </style>
    <script type="text/javascript">
        function SelectList(blnFlag) {
            var rowCount = (document.getElementById("Hid_RowCount").value - 0);
            for (i = 0; i < rowCount; i++) {
                var chkBox = document.getElementById("chkList_Select_" + i);
                chkBox.checked = blnFlag;
            }
        }
        function Validate() {
            if (CheckSelected() == false) {
                AlertMessage('Validation', "Please select at least one field", 175, 450);
                return false
            }
            if (document.getElementById("chkList_Select_0").checked == false) {
                AlertMessage('Validation', "Security Name can not be Unselected", 175, 450)
                return false
            }
            return true
        }
        function CheckSelected() {
            var blnFlag = false;
            var rowCount = (document.getElementById("Hid_RowCount").value - 0);
            for (i = 0; i < rowCount; i++) {
                var chkBox = document.getElementById("chkList_Select_" + i);
                if (chkBox.checked == true) {
                    blnFlag = true;
                    break;
                }
            }
            return blnFlag
        }
        function Submit() {
            var strRetValues = "";
            var selFValues = "";
            var selFinishValues = "";

            var lstBox = document.getElementById("lst_Order");
            var lstCnt = lstBox.options.length;
            for (i = 0; i < lstCnt; i++) {
                //                strRetValues = strRetValues + lstBox.options[i].value + "!"
                selFValues = selFValues + lstBox.options[i].value + ","
                selFinishValues = selFinishValues + lstBox.options[i].innerHTML + ","
            }

            strRetValues = strRetValues + selFValues
            if (document.getElementById("Hid_Form").value == "Format") {
                strRetValues = strRetValues + "!" + selFinishValues
            }
            document.getElementById("Hid_Id").value = strRetValues;
            window.returnValue = document.getElementById("Hid_Id").value;
            window.close();
        }

        //          strReturn = strReturn + lstBox.options[lstBox.options.selectedIndex].text + "!" 
        function Close() {
            window.returnValue = "";
            window.close();
        }
        function MoveItems(strFlag) {
            var lstBox = document.getElementById("lst_Order");
            var lstCnt = lstBox.options.length;
            var selectedIndex = lstBox.options.selectedIndex;
            var tempText
            var tempValue
            if (selectedIndex == -1) return false
            if (strFlag == "UP") {
                if (selectedIndex == 0) return false
                tempText = lstBox.options[selectedIndex - 1].text;
                tempValue = lstBox.options[selectedIndex - 1].value;
                lstBox.options[selectedIndex - 1].text = lstBox.options[selectedIndex].text;
                lstBox.options[selectedIndex - 1].value = lstBox.options[selectedIndex].value;
                lstBox.options[selectedIndex].text = tempText;
                lstBox.options[selectedIndex].value = tempValue;
                lstBox.options.selectedIndex = selectedIndex - 1;
            }
            else {
                if (selectedIndex == lstCnt - 1) return false
                tempText = lstBox.options[selectedIndex + 1].text;
                tempValue = lstBox.options[selectedIndex + 1].value;
                lstBox.options[selectedIndex + 1].text = lstBox.options[selectedIndex].text;
                lstBox.options[selectedIndex + 1].value = lstBox.options[selectedIndex].value;
                lstBox.options[selectedIndex].text = tempText;
                lstBox.options[selectedIndex].value = tempValue;
                lstBox.options.selectedIndex = selectedIndex + 1;
            }
            return false
        }
    </script>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
</head>
<body topmargin="0" class="popupbackground" leftmargin="0">
    <form id="form1" runat="server">
        <div>
            <table align="center" height="100%" cellspacing="0" cellpadding="0" width="100%">
                <tr valign="middle">
                    <td valign="middle" align="center">
                        <table cellspacing="0" cellpadding="0" width="95%" align="center" border="1" height="100%">
                            <tr>
                                <td bgcolor="#D1E4F8" align="center" class="HeaderCSS" colspan="2">
                                    <strong>Select Fields</strong>
                                </td>
                                <td bgcolor="#D1E4F8" align="center" class="HeaderCSS">
                                    <strong>Order Fields</strong>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Button ID="btn_SelectAll" runat="server" Width="89px" Text="Select All" CssClass="ButtonCSS" UseSubmitBehavior="False"></asp:Button></td>
                                <td>
                                    <asp:Button ID="btn_UnselectAll" runat="server" Width="84px" Text="Unselect All" CssClass="ButtonCSS" UseSubmitBehavior="False"></asp:Button></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td valign="middle" align="center" colspan="2">
                                    <div id="divSelection" align="left" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 350px">
                                        <asp:CheckBoxList runat="server" CssClass="LabelCSS" BorderWidth="1px" BorderColor="Black"
                                            BackColor="Transparent" BorderStyle="none" ID="chkList_Select" RepeatLayout="Flow">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                                <%--<td vAlign="middle" align="center"   colSpan="2">
							 <div id="div1" align="left" style="MARGIN-TOP: 0px; OVERFLOW: auto; WIDTH: 100%; PADDING-TOP: 0px; POSITION:relative; HEIGHT: 350px">
							<asp:CheckBoxList ID="chkList_Select" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="LabelCSS"
                                        RepeatColumns="5"   TabIndex="13">
                                                   </asp:CheckBoxList>
                                                    </div> </td>--%>



                                <td valign="middle">&nbsp;
							    <table align="center" cellpadding="0" cellspacing="0" border="1">
                                    <tr>
                                        <td rowspan="2">&nbsp;<asp:ListBox ID="lst_Order" runat="server" Width="150px" Height="300px" CssClass="LabelCSS"></asp:ListBox>
                                        </td>
                                        <td>&nbsp;<asp:Button ID="btn_Up" runat="server" Width="50px" Text="Up" CssClass="ButtonCSS" UseSubmitBehavior="False"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;<asp:Button ID="btn_Down" runat="server" Width="50px" Text="Down" CssClass="ButtonCSS" UseSubmitBehavior="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                                    <br />
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Button ID="btn_Ok" runat="server" Width="89px" Text="Ok" CssClass="ButtonCSS"></asp:Button>
                                </td>
                                <td>
                                    <asp:Button ID="btn_Close" runat="server" Width="89px" Text="Cancel" CssClass="ButtonCSS" UseSubmitBehavior="False"></asp:Button>
                                </td>
                                <td>
                                    <asp:Button ID="btn_Sumbit" runat="server" Width="89px" Text="Submit" CssClass="ButtonCSS hidden"></asp:Button>
                                    <input type="button" id="btn_Ret" class="ButtonCSS" runat="server" value="Submit" onclick="Submit();" />
                                </td>
                            </tr>
                        </table>
                        <input class="fieldcontent" id="Hid_ReturnValues" type="hidden" runat="server">
                        <input class="fieldcontent" id="Hid_ReturnFields" type="hidden" name="Hidden1" runat="server">
                    </td>
                </tr>
            </table>
        </div>

        <%-- <asp:SqlDataSource ID="SqlDataSourceFax" runat="server" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"
            SelectCommand="ID_SELECT_FaxFields" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Direction="Output" Name="RET_CODE" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
        <asp:HiddenField ID="Hid_Form" runat="server" />
        <asp:HiddenField ID="Hid_RowCount" runat="server" />
        <asp:HiddenField ID="Hid_FieldId" runat="server" />
        <asp:HiddenField ID="Hid_CustomerId" runat="server" />
        <asp:HiddenField ID="Hid_Id" runat="server" />
        <asp:SqlDataSource ID="SqlDataSourceSelectedFields" runat="server" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"
            SelectCommand="ID_SELECT_FaxFields" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter DefaultValue="True" Name="Selected" Type="Boolean" />
                <asp:Parameter DefaultValue="0" Direction="Output" Name="RET_CODE" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
