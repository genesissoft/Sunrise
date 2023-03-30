<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddressMultiple.aspx.vb" Inherits="Forms_AddressMultiple" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Address</title>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script language="javascript">
        function Close() {
            //      window.returnValue = document.getElementById("Hid_CustomerId").value; 
            //          window.returnValue = "";                   
            window.close();
            //            return false;
        }

        //         function Close()
        //        {
        //            window.returnValue = "";
        //            window.close();
        //        }
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

        function Validation() {
            if ((document.getElementById("txt_CustBranchname").value) == "") {
                alert('Please Enter Branch name')
                return false;
            }
            if ((document.getElementById("txt_Address1").value) == "") {
                alert('Please Enter Address')
                return false;
            }
            return true
        }


        function submitvalidation() {
            var grd = document.getElementById("dg1")
            if (grd != null) {
                if (grd.children[0].children.length <= 2) {
                    alert('Please Enter atleast one detail record');
                    return false
                }
            }
        }



        function RetValues() {
            var strReturn = "";
            var selBValues = "";
            var selBusniessTypeValues = "";
            var cboBusniessType = document.getElementById("srh_BusniessType_lst_Select")
            var pagename = document.getElementById("Hid_PageName").value

            strReturn = strReturn + document.getElementById("txt_CustBranchname").value + "!"
            strReturn = strReturn + document.getElementById("txt_Address1").value + "!"
            strReturn = strReturn + document.getElementById("txt_Address2").value + "!"
            strReturn = strReturn + document.getElementById("txt_City").value + "!"
            strReturn = strReturn + document.getElementById("txt_Pincode").value + "!"
            strReturn = strReturn + document.getElementById("txt_State").value + "!"
            strReturn = strReturn + document.getElementById("txt_Country").value + "!"
            strReturn = strReturn + document.getElementById("txt_PhoneNo").value + "!"
            strReturn = strReturn + document.getElementById("txt_FaxNo").value + "!"
            strReturn = strReturn + document.getElementById("txt_Email").value + "!"
            strReturn = strReturn + document.getElementById("Hid_CustomerId").value + "!"

            if (cboBusniessType != null) {
                for (i = 0; i < cboBusniessType.options.length; i++) {
                    selBValues = selBValues + cboBusniessType.options[i].value + ","
                    selBusniessTypeValues = selBusniessTypeValues + cboBusniessType.options[i].innerHTML + ","
                }
            }

            if (pagename == "MercBanking") {
                selBusniessTypeValues = "MerchantBanking,"

            }


            strReturn = strReturn + selBValues + "!"
            strReturn = strReturn + selBusniessTypeValues + "!"
            strReturn = strReturn + document.getElementById("Hid_ClientCustAddressId").value + "!"

            //            alert(strReturn)
            window.returnValue = strReturn;
            //            alert(window.returnValue)
            window.close();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="Table1" width="95%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td class="HeaderCSS" align="center" colspan="2">Customer Address
                    </td>
                </tr>



                <tr>
                    <td align="center" valign="middle" colspan="2">
                        <table id="Table2" align="center" cellspacing="0" cellpadding="0" border="1" width="100%">
                            <tr align="center">
                                <td valign="middle">
                                    <table id="Table6" align="center" cellspacing="0" cellpadding="0" border="0" width="100%"
                                        bordercolor="Green">

                                        <tr>
                                            <td class="LabelCSS" width="20%">Customer Branch Name:
                                            </td>
                                            <td align="left" valign="top" colspan="3">
                                                <asp:TextBox ID="txt_CustBranchname" runat="server" CssClass="TextBoxCSS" Width="250px" TabIndex="1"></asp:TextBox>
                                                <em><span style="color: #ff0000">*</span></em>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" width="15%">Address1:
                                            </td>
                                            <td align="left" valign="top" width="35%">
                                                <asp:TextBox ID="txt_Address1" runat="server" CssClass="TextBoxCSS" Width="250px" TabIndex="2"></asp:TextBox>
                                            </td>
                                            <td class="LabelCSS" width="13%">State:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_State" runat="server" CssClass="TextBoxCSS" Width="150px" TabIndex="7"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Address2:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Address2" runat="server" CssClass="TextBoxCSS" Width="250px" TabIndex="3"></asp:TextBox>
                                            </td>
                                            <td class="LabelCSS">Country:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Country" runat="server" CssClass="TextBoxCSS" Width="150px" TabIndex="8"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">City:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_City" runat="server" CssClass="TextBoxCSS" Width="150px" TabIndex="4"></asp:TextBox>
                                            </td>
                                            <td class="LabelCSS">Phone No.:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_PhoneNo" runat="server" CssClass="TextBoxCSS" Width="150px" TabIndex="9"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Pincode:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Pincode" runat="server" CssClass="TextBoxCSS" Width="150px" TabIndex="5"></asp:TextBox>
                                            </td>
                                            <td class="LabelCSS">Fax No.:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_FaxNo" runat="server" CssClass="TextBoxCSS" Width="150px" TabIndex="10"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Email:
                                            </td>
                                            <td align="left" colspan="4">
                                                <asp:TextBox ID="txt_Email" runat="server" CssClass="TextBoxCSS" Width="250px" TabIndex="6"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr id="row_LstBustype" runat="server">
                                            <td class="LabelCSS">Business Type:
                                            </td>
                                            <td align="left">
                                                <uc:SelectFields ID="srh_BusniessType" class="LabelCSS" runat="server" ProcName="ID_SEARCH_BusinessTypeMasterNew"
                                                    FormHeight="470" FormWidth="257" SelectedValueName="BusinessTypeId" ChkLabelName="" Height="40"
                                                    ConditionalFieldId="" LabelName="" SelectedFieldName="BusinessType" SourceType="StoredProcedure"
                                                    ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Submit" ToolTip="Save" TabIndex="11" />
                        <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Submit" ToolTip="Edit" TabIndex="13" />
                        <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" UseSubmitBehavior="false"
                            OnClientClick="return FinalClose();" TabIndex="12" />
                    </td>
                </tr>
                <asp:HiddenField ID="Hid_CustBankDetailId" runat="server" />
                <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                <asp:HiddenField ID="Hid_PageName" runat="server" />
                <asp:HiddenField ID="Hid_Index" runat="server" />
                <asp:HiddenField ID="Hid_CustBankMultiDetailId" runat="server" />
                <asp:HiddenField ID="Hid_ClientCustAddressId" runat="server" />
            </table>
        </div>
    </form>
</body>
</html>
