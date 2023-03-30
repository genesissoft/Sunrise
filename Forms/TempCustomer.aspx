<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TempCustomer.aspx.vb"
    Inherits="Forms_TempCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Add Temporary Customer</title>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">

        function Validation() {
          
            if (document.getElementById("txt_CustomerName").value == "") {
                alert('Please Enter Customer Name')
                return false;
            }
            if (document.getElementById("txt_ContactPerson").value == "") {
                alert('Please Enter Contact Person')
                return false;
            }
            if (document.getElementById("txt_EMailId").value != "") {
                if (Email(document.getElementById("txt_EMailId").value) == false) {
                    alert('Enter Valid Email');
                    return false;
                }
            }
            document.getElementById("Hid_Id").value = document.getElementById("txt_CustomerName").value + "|" + document.getElementById("txt_ContactPerson").value + "|" + document.getElementById("txt_EMailId").value + "|" + document.getElementById("txt_PhoneNo").value + "|" + document.getElementById("txt_MobileNo").value;
            window.returnValue = document.getElementById("Hid_Id").value;
            window.close();
        }

        function Close(ret) {
          
            window.returnValue = ret;
            window.close();
        }
        function ReturnValues(strReturn) {
          
            window.returnValue = strReturn;
            window.close();
        }
        function FinalClose() {
            window.returnValue = "";
            window.close();
        }

        function ConvertUCase(txtBox) {
            txtBox.value = txtBox.value.toUpperCase()
        }

        function RetValues() {
         
            var strReturn = "";
            var selBValues = "";
            var selBusniessTypeValues = "";
            var selDValues = "";
            var selDealerValues = "";

            strReturn = strReturn + document.getElementById("txt_CustomerName").value + "!"
            strReturn = strReturn + document.getElementById("txt_ContactPerson").value + "!"
            strReturn = strReturn + document.getElementById("txt_PhoneNo").value + "!"
            strReturn = strReturn + document.getElementById("txt_MobileNo").value + "!"
            strReturn = strReturn + document.getElementById("txt_EMailId").value + "!"
            strReturn = strReturn + document.getElementById("Hid_TempCustId").value + "!"

            window.returnValue = strReturn;

            window.close();

        }

        function AddTempCust() {
            
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=590px; dialogTop=230px; dialogHeight=600px; Help=No; Status=No; Resizable=Yes;"
            var OpenUrl = "TempCustomer.aspx"
            OpenUrl = OpenUrl;
            var ret = window.showModalDialog(OpenUrl, 'some argument', 'dialogWidth:500px;dialogHeight:550px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                //return false
            }
            else {

                //return true
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td class="HeaderCSS" align="center" colspan="4">Add Temporary  Customer
                </td>
            </tr>
            <tr>
                <td colspan="4">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" width="80%" valign="top">
                    <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="80%">

                        <tr>
                            <td class="LabelCSS">Customer Name:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_CustomerName" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">Contact Person:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_ContactPerson" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">Phone No:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_PhoneNo" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">Mobile No:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_MobileNo" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">EMail Id:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_EMailId" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                            </td>
                        </tr>


                    </table>
                </td>
            </tr>
            <tr>
                <td class="SeperatorRowCSS" colspan="4"></td>
            </tr>
            <tr>
                <td class="SeperatorRowCSS" colspan="4"></td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" ToolTip="Save" />
                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" ToolTip="Update" />
                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" OnClientClick="return Close();"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
            <asp:HiddenField ID="Hid_UserId" runat="server" />
            <asp:HiddenField ID="Hid_TempCustId" runat="server" />
            <asp:HiddenField ID="Hid_Id" runat="server" />
        </table>
    </form>
</body>
</html>
