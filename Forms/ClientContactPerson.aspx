<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ClientContactPerson.aspx.vb"
    Inherits="Forms_ClientContactPerson" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Client Contact Person</title>

    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>

    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>
    <script type="text/javascript" src="../Include/Common.js"></script>
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">

        function Close() {
            window.returnValue = "";
            window.close();
        }
        function ReturnValues(strReturn) {
            document.getElementById("Hid_Id").value = strReturn;
            window.returnValue = strReturn;
            window.close();
        }
        function Validation() {
            if ((document.getElementById("txt_ContactPerson").value) == "") {
                //AlertMessage('Validation', 'Please Enter Contact Person', 175, 450)
                alert("Please enter contact person.");
                return false;
            }
            RetValues();
        }

        function RetValues() {
            var strReturn = "";
            var selBValues = "";
            var selBusniessTypeValues = "";
            var selDValues = "";
            var selDealerValues = "";
            var selRDValues = "";
            var selRDSearchValues = "";
            strReturn = strReturn + document.getElementById("txt_ContactPerson").value + "!"
            strReturn = strReturn + document.getElementById("txt_Designation").value + "!"
            strReturn = strReturn + document.getElementById("txt_PhoneNo1").value + "!"
            strReturn = strReturn + document.getElementById("txt_MobileNo").value + "!"
            strReturn = strReturn + document.getElementById("txt_EmailId").value + "!"
            strReturn = strReturn + document.getElementById("txt_PhoneNo2").value + "!"
            strReturn = strReturn + document.getElementById("txt_FaxNo1").value + "!"
            strReturn = strReturn + document.getElementById("txt_FaxNo2").value + "!"
            strReturn = strReturn + document.getElementById("cbo_ContactInteraction").value + "!"
            if (document.getElementById("rdoList_Section_0").checked) {
                strReturn = strReturn + "F" + "!"
            }
            else {
                strReturn = strReturn + "B" + "!"
            }
            strReturn = strReturn + document.getElementById("txt_Branch").value + "!"
            document.getElementById("Hid_Id").value = strReturn;
            window.returnValue = strReturn;
            window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td class="HeaderCSS" align="center" colspan="4">Contact Person
                </td>
            </tr>
            <tr>
                <td colspan="4">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="60%">
                        <tr>
                            <td class="LabelCSS">Section:
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="rdoList_Section" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                    CssClass="LabelCSS">
                                    <asp:ListItem Text="Front Office" Value="F"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="Back Office" Value="B"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">Contact Person:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_ContactPerson" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox><em><span
                                    style="color: #ff0000">*</span></em>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">Designation:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_Designation" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">Phone No1:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_PhoneNo1" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">Phone No2:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_PhoneNo2" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">MobileNo:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_MobileNo" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">Fax No1:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_FaxNo1" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">Fax No2:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_FaxNo2" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">EmailId:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_EmailId" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="LabelCSS">Branch:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_Branch" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="row_LstBustype" runat="server" visible="false">
                            <td class="LabelCSS">Business Type:
                            </td>
                            <td align="left">
                                <uc:SelectFields ID="srh_BusniessType" class="LabelCSS" runat="server" ProcName="ID_SEARCH_BusinessTypeMasterNew"
                                    FormHeight="470" FormWidth="257" SelectedValueName="BusinessTypeId" ChkLabelName=""
                                    Height="40" ConditionalFieldId="" LabelName="" SelectedFieldName="BusinessType"
                                    SourceType="StoredProcedure" ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                            </td>
                        </tr>
                        <tr runat="server" visible="false">
                            <td class="LabelCSS">Dealer:
                            </td>
                            <td align="left">
                                <uc:SelectFields ID="srh_Dealer" ShowAll="true" class="LabelCSS" runat="server" ProcName="ID_SEARCH_UserNames"
                                    FormHeight="470" FormWidth="257" SelectedValueName="UserId" ChkLabelName="" ConditionalFieldId=""
                                    Height="40" LabelName="" SelectedFieldName="NameOfUser" SourceType="StoredProcedure"
                                    ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                            </td>
                        </tr>
                        <tr runat="server" visible="false">
                            <td class="LabelCSS">Research & Informations:
                            </td>
                            <td align="left">
                                <uc:SelectFields ID="srh_SearchRD" class="LabelCSS" runat="server" ProcName="ID_SEARCH_ResearchDocMaster"
                                    Height="40" FormHeight="470" FormWidth="257" SelectedValueName="ResearchDocId"
                                    ChkLabelName="" ConditionalFieldId="" LabelName="" SelectedFieldName="ResearchDocName"
                                    SourceType="StoredProcedure" ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">Interaction:
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cbo_ContactInteraction" Width="100px" runat="server" CssClass="ComboBoxCSS"
                                    AutoPostBack="false" TabIndex="8">
                                    <%--<asp:ListItem Value="W" Selected="True">WEEKELY</asp:ListItem>
                                    <asp:ListItem Value="F">FORTNIGHTLY</asp:ListItem>
                                    <asp:ListItem Value="M">MONTHLY</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="SeperatorRowCSS" colspan="4"></td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" ToolTip="Save" Visible="false"/>
                    <input type="button" id="btn_Ret" runat="server" class="ButtonCSS" value="Save" onclick="return Validation();" />
                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Save" ToolTip="Edit" />
                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" OnClientClick="return Close();"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
            <asp:HiddenField ID="Hid_Ids" runat="server" />
            <asp:HiddenField ID="Hid_CustomerId" runat="server" />
            <asp:HiddenField ID="Hid_Id" runat="server" />
        </table>
    </form>
</body>
</html>
