<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MainAddressMultiple.aspx.vb"
    Inherits="Forms_MainAddressMultiple" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Address</title>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <link href="../Include/CSS/jquery-ui.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.all.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.base.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.button.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.core.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.theme.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>

    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>

    <script type="text/javascript">
        function abc() {
            alert('abc');
        }
        function Close() {
            //      window.returnValue = document.getElementById("Hid_CustomerId").value; 
            //          window.returnValue = "";                   
            window.close();
            //            return false;
        }

        function ReturnValues(strReturn) {
            window.returnValue = strReturn;
            window.close();
        }

        function FinalClose() {
            window.returnValue = "C";
            window.close();
        }


        function ConvertUCase(txtBox) {
            txtBox.value = txtBox.value.toUpperCase()
        }

        function Validation() {
            if ((document.getElementById("txt_CustBranchname").value) == "") {
                AlertMessage('Validation', 'Please Enter Branch name', 175, 450)
                document.getElementById("Hid_Id").value = "E";
                window.returnValue = document.getElementById("Hid_Id").value;
                window.close();
                return false;
            }
            if ((document.getElementById("txt_Address1").value) == "") {
                AlertMessage('Validation', 'Please Enter Address', 175, 450)
                document.getElementById("Hid_Id").value = "E";
                window.returnValue = document.getElementById("Hid_Id").value;
                window.close();
                return false;
            }
            
            RetValues();
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
            strReturn = strReturn + document.getElementById("Hid_TempId").value + "!"

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
            strReturn = strReturn + document.getElementById("Hid_ClientCustMulId").value + "!"
            strReturn = strReturn + document.getElementById("Hid_TempId").value + "!"
            document.getElementById("Hid_Id").value = strReturn;
            window.returnValue = document.getElementById("Hid_Id").value;
            window.close();
        }
        function OpenDialog(PageName, CustomerId, strWidth, strHeight, parentId) {
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
            var OpenUrl = PageName + "?Id=" + CustomerId
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
            return ret
        }
        function AddContact() {
            var strId = document.getElementById("Hid_CustomerId").value;
            var pageUrl = "MultiAddrClientContacts.aspx?Id=" + strId;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:500px;dialogHeight:400px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                document.getElementById("Hid_RetValues").value = ret
                document.getElementById('<%= btn_AddContact.ClientID%>').click();
            }
            else {
            }
        }
        function UpdateContactDetails(ctrl, rowIndex) {
            document.getElementById("Hid_RowIndex").value = rowIndex;
            var pageUrl = "MultiAddrClientContacts.aspx";
            var strValues = "";
            var selValues = "";
            var strId = document.getElementById("Hid_CustomerId").value
            var hidPhoneNo2 = document.getElementById("Hid_PhoneNo2").value.split("!")
            var hidFaxNo1 = document.getElementById("Hid_FaxNo1").value.split("!")
            var hidFaxNo2 = document.getElementById("Hid_FaxNo2").value.split("!")
            var hidSectionType = document.getElementById("Hid_SectionType").value.split("!")
            var row = $(ctrl).closest("tr");
            strValues = strValues + row.find("td").eq(2).find("input").eq(0).val() + "!"
            strValues = strValues + row.find("td:eq(3)").text().trim() + "!"
            strValues = strValues + row.find("td:eq(4)").text().trim() + "!"
            strValues = strValues + row.find("td:eq(5)").text().trim() + "!"
            strValues = strValues + row.find("td:eq(6)").text().trim() + "!"
            strValues = strValues + hidPhoneNo2[rowIndex] + "!"
            strValues = strValues + hidFaxNo1[rowIndex] + "!"
            strValues = strValues + hidFaxNo2[rowIndex] + "!"
            strValues = strValues + hidSectionType[rowIndex] + "!"
            strValues = strValues.replace("&", " ")
            pageUrl = pageUrl + "?CustomerId=" + strId + "&Values=" + strValues
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:500px;dialogHeight:400px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                document.getElementById("Hid_RetValues").value = ret
                document.getElementById('<%= btn_AddContact.ClientID%>').click();
            }
            else {
            }
        }
        function Deletedetails() {
            if (window.confirm("Are you sure u want to delete this detail record")) {
                return true
            }
            else {
                return false
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
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
                                                <asp:TextBox ID="txt_CustBranchname" runat="server" CssClass="TextBoxCSS" Width="250px"
                                                    TabIndex="1"></asp:TextBox>
                                                <em><span style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" width="15%">Address1:
                                            </td>
                                            <td align="left" valign="top" width="35%">
                                                <asp:TextBox ID="txt_Address1" runat="server" CssClass="TextBoxCSS" Width="250px"
                                                    TabIndex="2"></asp:TextBox>
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
                                                <asp:TextBox ID="txt_Address2" runat="server" CssClass="TextBoxCSS" Width="250px"
                                                    TabIndex="3"></asp:TextBox>
                                            </td>
                                            <td class="LabelCSS">Country:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Country" runat="server" CssClass="TextBoxCSS" Width="150px"
                                                    TabIndex="8"></asp:TextBox>
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
                                                <asp:TextBox ID="txt_PhoneNo" runat="server" CssClass="TextBoxCSS" Width="150px"
                                                    TabIndex="9"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS">Pincode:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Pincode" runat="server" CssClass="TextBoxCSS" Width="150px"
                                                    TabIndex="5"></asp:TextBox>
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
                                                    FormHeight="470" FormWidth="257" SelectedValueName="BusinessTypeId" ChkLabelName=""
                                                    Height="40" ConditionalFieldId="" LabelName="" SelectedFieldName="BusinessType"
                                                    SourceType="StoredProcedure" ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="Table4" align="center" cellspacing="0" cellpadding="0" border="1" width="100%"
                                        runat="server">
                                        <tr align="center">
                                            <td valign="middle" colspan="8" align="center">
                                                <asp:Button ID="btn_AddContact" runat="server" CssClass="ButtonCSS hidden" Text="Add Contact" />
                                                <input type="button" id="btn_AddC" runat="server" class="ButtonCSS" onclick="return AddContact();" value="Add Contact" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="8" align="center" valign="top">
                                                <div id="Div2" style="margin-top: 0px; overflow: auto; width: 850px; padding-top: 0px; position: relative; height: 110px"
                                                    align="center">
                                                    <asp:DataGrid ID="dg_Contact" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                                        Width="850px" CssClass="GridCSS">
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                                        <FooterStyle HorizontalAlign="Center" CssClass="footer" VerticalAlign="Middle"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit"
                                                                        ToolTip="Edit" CssClass="hidden" />
                                                                    <input type="button" id="imgBtn_Edit1" runat="server" class="TitleText" value="Edit" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                                        CommandName="Delete" ToolTip="Delete" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Contact Person">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_ContactPerson" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                                        onkeydown="OnlyScroll();"
                                                                        runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("ContactPerson") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Designation" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Designation") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="PhoneNo1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_PhoneNo1" Width="75px" runat="server" Text='<%# container.dataitem("PhoneNo1") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="MobileNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_MobileNo" Width="75px" runat="server" Text='<%# container.dataitem("MobileNo") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Email Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_EmailId" Width="75px" runat="server" Text='<%# container.dataitem("EmailId") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="PhoneNo2" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_PhoneNo2" Width="75px" runat="server" Text='<%# container.dataitem("PhoneNo2") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="FaxNo1" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_FaxNo1" Width="75px" runat="server" Text='<%# container.dataitem("FaxNo1") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="FaxNo2" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_FaxNo2" Width="75px" runat="server" Text='<%# container.dataitem("FaxNo2") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="SectionType" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_SectionType" Width="75px" runat="server" Text='<%# container.dataitem("SectionType") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="CustomerId" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_CustomerId" Width="75px" runat="server" Text='<%# container.dataitem("CustomerId") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="ClientCustAddressId" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_ClientCustAddressId" Width="75px" runat="server" Text='<%# container.dataitem("ClientCustAddressId") %>'
                                                                        CssClass="LabelCSS"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_tempId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.tempId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <PagerStyle PageButtonCount="2" />
                                                    </asp:DataGrid>
                                                </div>
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
                        <input type="button" id="btn_Save1" runat="server" class="ButtonCSS" value="Submit" onclick="return Validation();" style="display: none;" />

                        <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Submit" ToolTip="Save" UseSubmitBehavior="true"
                            TabIndex="11" />
                        <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS hidden" Text="Submit" ToolTip="Edit"
                            TabIndex="13" />
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
                <asp:HiddenField ID="Hid_RetValues" runat="server" />
                <asp:HiddenField ID="Hid_BusinessTypeId" runat="server" />
                <asp:HiddenField ID="Hid_BusinessTypeNames" runat="server" />
                <asp:HiddenField ID="Hid_UserIds" runat="server" />
                <asp:HiddenField ID="Hid_NameOfUsers" runat="server" />
                <asp:HiddenField ID="Hid_PhoneNo2" runat="server" />
                <asp:HiddenField ID="Hid_FaxNo1" runat="server" />
                <asp:HiddenField ID="Hid_FaxNo2" runat="server" />
                <asp:HiddenField ID="Hid_SectionType" runat="server" />
                <asp:HiddenField ID="Hid_MACustAddrCtcid" runat="server" />
                <asp:HiddenField ID="Hid_TempId" runat="server" />
                <asp:HiddenField ID="Hid_ClientCustMulId" runat="server" />
                <asp:HiddenField ID="Hid_Id" runat="server" />
                <asp:HiddenField ID="Hid_RowIndex" runat="server" />
            </table>
        </div>
    </form>


</body>
</html>
