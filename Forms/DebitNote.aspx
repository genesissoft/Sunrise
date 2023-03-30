<%@ Page Title="" Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="DebitNote.aspx.vb" Inherits="Forms_DebitNote" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <link href="../Include/General.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">


        function DateMonthSelection() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_Selection_0").checked == true) {

                document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "block";
                document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "block";


            }
            else {

                document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "block";
                document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "none";


            }
        }

        function Validation() {

            var txtName = ""
            var txtName1 = ""
            var txtName2 = ""
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_Debitnote")
            var blnSelected = false
            var ids = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIds").value.split("!")
            var custIds = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Intids").value.split("!")
            var transtype = document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value.split("!")

            for (i = 1; i <= (grd.rows.length - 2) ; i++) {
                currRow = grd.children[0].children[i]

                if (currRow.children[0].children[0].checked == true) {
                    txtName = txtName + ids[i - 1] + ",";
                    txtName1 = txtName1 + custIds[i - 1] + ",";
                    txtName2 = txtName2 + transtype[i - 1] + ",";
                    blnSelected = true
                }
            }
            if (blnSelected == false) {
                alert("Please select atleast one option")
                return false
            }

            document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIds").value = txtName.substring(0, txtName.length - 1)
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_Intids").value = txtName1.substring(0, txtName1.length - 1)
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value = txtName2.substring(0, txtName2.length - 1)

            document.getElementById("div_onsaveclick").style.display = "block";

            $('#ctl00_ContentPlaceHolder1_btn_Save').hide();
            $('#ctl00_ContentPlaceHolder1_btn_Export').hide();

            return true;

        }


        function CheckAll(checkVal) {
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i]
                if (elm.type == 'checkbox' && elm.disabled == false) {
                    elm.checked = checkVal
                    if (checkVal == true) {

                    }
                    else {

                    }
                }
            }
        }
        function SelectRow(elm) {
            checkVal = elm.checked
            if (checkVal == true) {
                elm.parentElement.parentElement.style.backgroundColor = "white"
            }
            else {
                elm.parentElement.parentElement.style.backgroundColor = "white"
            }
        }


        function Submit() {
            var txtName = ""
            var intid = ""

            var intids = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Intid").value.split("!")
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_Debitnote")
            for (i = 1; i <= (grd.children[0].children.length - 2) ; i++) {
                currRow = grd.children[0].children[i]
                if (currRow.style.backgroundColor.toUpperCase() == '#D1E4F8') {
                    txtName = currRow.children[1].children[0].innerHTML
                    intid = intids[i - 1]
                    break
                }
            }
            if (txtName == "") {
                alert("Please select atleast one option")
                return false
            }
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelectedField").value = txtName
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelectedValue").value = intid
            window.returnValue = intid

            return false


        }
    </script>

    <table id="Table1" width="90%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="SectionHeaderCSS">Retail Debit Note
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table cellspacing="0" cellpadding="0" border="0" align="center" width="45%">
                    <%--<tr>
                        <td align="right" id="lbl_According" runat="server" width="300px">According To:
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="D" Selected="True">Deal Date</asp:ListItem>
                                <asp:ListItem Value="S">Settlement Date</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>--%>
                    <tr align="left">
                        <td>According To:
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="D" Selected="True">Deal Date</asp:ListItem>
                                <asp:ListItem Value="S">Settlement Date</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>

    </table>
</asp:Content>

