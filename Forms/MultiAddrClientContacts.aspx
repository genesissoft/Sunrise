<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MultiAddrClientContacts.aspx.vb"
    Inherits="Forms_MultiAddrClientContacts" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Client Contact Person</title>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">

        function Close() {
            window.returnValue = "";
            window.close();
        }
        function ReturnValues(strReturn) {
            window.returnValue = strReturn;
            window.close();
        }
        function Validation() {
            if ((document.getElementById("txt_ContactPerson").value) == "") {
                alert('Please Enter Contact Person')
                return false;
            }
            RetValues();
        }

        function RetValues(stkDetailId, excessQty) {
            var strReturn = "";
            var selBValues = "";
            var selBusniessTypeValues = "";
            var selDValues = "";
            var selDealerValues = "";
            var selRDValues = "";
            var selRDSearchValues = "";

            var hidSectionType = document.getElementById("Hid_SectionType").value.split("!")

            strReturn = strReturn + document.getElementById("txt_ContactPerson").value + "!"
            strReturn = strReturn + document.getElementById("txt_Designation").value + "!"
            strReturn = strReturn + document.getElementById("txt_PhoneNo1").value + "!"
            strReturn = strReturn + document.getElementById("txt_MobileNo").value + "!"
            strReturn = strReturn + document.getElementById("txt_EmailId").value + "!"

            strReturn = strReturn + document.getElementById("txt_PhoneNo2").value + "!"
            strReturn = strReturn + document.getElementById("txt_FaxNo1").value + "!"
            strReturn = strReturn + document.getElementById("txt_FaxNo2").value + "!"
            //                strReturn = strReturn + hidSectionType + "!" 
            strReturn = strReturn + document.getElementById("rdoList_Section_0").checked + "!"
            document.getElementById("Hid_Id").value = strReturn;
            window.returnValue = document.getElementById("Hid_Id").value;
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
                    <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="65%">
                        <tr>
                            <td class="LabelCSS">Section:
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="rdoList_Section" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                    CssClass="LabelCSS">
                                    <asp:ListItem Text="Front Office" Value="f"></asp:ListItem>
                                    <asp:ListItem Text="Back Office" Selected="True" Value="b"></asp:ListItem>
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
                        <%--  <tr id = "row_LstBustype" runat=server>
                            <td class="LabelCSS">
                                Business Type:
                            </td>
                            <td align="left">
                                <uc:SelectFields ID="srh_BusniessType" class="LabelCSS" runat="server" ProcName="ID_SEARCH_BusinessTypeMasterNew"
                                    FormHeight="470" FormWidth="257" SelectedValueName="BusinessTypeId" ChkLabelName="" Height="40"
                                    ConditionalFieldId="" LabelName="" SelectedFieldName="BusinessType" SourceType="StoreProcedure"
                                    ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">
                                Dealer:
                            </td>
                            <td align="left">
                                <uc:SelectFields ID="srh_Dealer" ShowAll="true"   class="LabelCSS" runat="server" ProcName="ID_SEARCH_UserNames"
                                    FormHeight="470" FormWidth="257" SelectedValueName="UserId" ChkLabelName="" ConditionalFieldId="" Height="40"
                                    LabelName="" SelectedFieldName="NameOfUser" SourceType="StoreProcedure" ConditionalFieldName=""
                                    Visible="true" ShowLabel="false"></uc:SelectFields>
                            </td>
                        </tr>--%>
                        <%--    <tr>
                            <td class="LabelCSS">
                                Research & Informations:
                            </td>
                            <td align="left">
                                <uc:SelectFields ID="srh_SearchRD" class="LabelCSS" runat="server" ProcName="ID_SEARCH_ResearchDocMaster" Height="40"
                                    FormHeight="470" FormWidth="257" SelectedValueName="ResearchDocId" ChkLabelName="" ConditionalFieldId=""
                                    LabelName="" SelectedFieldName="ResearchDocName" SourceType="StoreProcedure" ConditionalFieldName=""
                                    Visible="true" ShowLabel="false"></uc:SelectFields>
                            </td>
                        </tr>--%>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="SeperatorRowCSS" colspan="4"></td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS hidden" Text="Save" ToolTip="Save" />
                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS hidden" Text="Save" ToolTip="Edit" />
                    <input type="button" id="btn_Ret" runat="server" class="ButtonCSS" onclick="return Validation();" value="Save" />
                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" OnClientClick="return Close();"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
            <asp:HiddenField ID="Hid_Ids" runat="server" />
            <asp:HiddenField ID="Hid_CustomerId" runat="server" />
            <asp:HiddenField ID="Hid_SectionType" runat="server" />
            <asp:HiddenField ID="Hid_MultiAddrClCtcs" runat="server" />
            <asp:HiddenField ID="Hid_Id" runat="server" />
        </table>
    </form>
</body>
</html>
