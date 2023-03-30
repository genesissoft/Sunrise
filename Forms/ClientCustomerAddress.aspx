<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ClientCustomerAddress.aspx.vb"
    Inherits="Forms_ClientCustomerAddress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Customer Address</title>

    <script type="text/javascript" src="../Include/Common.js"></script>
    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>
    <script type="text/javascript">
        function Close() {
            //      window.returnValue = document.getElementById("Hid_CustomerId").value; 
            //             window.returnValue = "";                   
            window.close();
            return false;
        }

        function FinalClose() {
            debugger;
            document.getElementById("Hid_Id").value = "";
            window.returnValue = document.getElementById("Hid_Id").value;
            window.close();
        }


        function Deletedetails() {
            if (window.confirm("Are you sure u want to delete this detail record")) {
                return true
            }
            else {
                return false
            }
        }
        function FillIndex(index) {
            document.getElementById("Hid_Index").value = index
        }

        function Validation() {
            if ((document.getElementById("txt_CustBranchname").value) == "") {
                alert('Please Enter Branch name')
                return false;
            }
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

        function Add() {
            var grd = document.getElementById("dg1")
            var tempId = 0
            if (grd != null) {
                if (grd.children[0].children.length >= 1) {
                    for (i = 1; i <= (grd.children[0].children.length - 1) ; i++) {
                        var tempId = tempId + 1;
                    }

                }

            }
            var pageUrl = "MainAddressMultiple.aspx";
            var strId = document.getElementById("Hid_CustomerId").value
            var intClientCustId = document.getElementById("Hid_ClientCustAddressId").value

            pageUrl = pageUrl + "?CustomerId=" + strId + "&TempId=" + tempId + "&ClientCustId=" + intClientCustId;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:890px;dialogHeight:500px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                document.getElementById("Hid_RetValues").value = ret
                document.getElementById('<%= btn_Add.ClientID%>').click();
            }
            else {
            }
        }

        function UpdateDetails(img, rowIndex) {
            document.getElementById("Hid_RowIndex").value = rowIndex;

            var pageUrl = "MainAddressMultiple.aspx";
            var pagename = "CustBankDetails.aspx";
            var strValues = "";
            var selValues = "";
            var hidDetId = "";
            var hidCustomerId = document.getElementById("Hid_CustomerId").value
            hidDetId = document.getElementById("Hid_TempId").value
            var custAddrId = document.getElementById("Hid_ClientCustAddressId").value

            var intClientCustId = document.getElementById("Hid_ClientCustAddressId").value
            var ctrlItem = document.getElementById("ctl00_ContentPlaceHolder1_dg1" + rowIndex)
            var row = $(img).closest("tr");
            var val = row.find("td").eq(0).find("input").eq(0).val();

            var hidCustId = document.getElementById("Hid_CustId").value.split("!")
            var hidAddBusinessTypeIds = document.getElementById("Hid_AddBusinessTypeId").value.split("!")


            strValues = strValues + row.find("td:eq(2)").text().trim() + "!"

            strValues = strValues + row.find("td").eq(3).find("input").eq(0).val() + "!"

            strValues = strValues + row.find("td").eq(4).find("input").eq(0).val() + "!"

            strValues = strValues + row.find("td:eq(5)").text().trim() + "!"

            strValues = strValues + row.find("td:eq(6)").text().trim() + "!"

            strValues = strValues + row.find("td:eq(7)").text().trim() + "!"
            strValues = strValues + row.find("td:eq(8)").text().trim() + "!"
            strValues = strValues + row.find("td:eq(9)").text().trim() + "!"
            strValues = strValues + row.find("td:eq(10)").text().trim() + "!"
            strValues = strValues + row.find("td").eq(11).find("input").eq(0).val() + "!"
            strValues = strValues + row.find("td").eq(12).find("input").eq(0).val() + "!"
            strValues = strValues + row.find("td:eq(13)").text().trim() + "!"
            strValues = strValues + hidAddBusinessTypeIds[rowIndex] + "!"

            pageUrl = pageUrl + "?CustomerId=" + hidCustomerId + "&Values=" + strValues + "&custMAddrId=" + custAddrId + "&TempId=" + rowIndex + "&ClientCustId=" + intClientCustId;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:890px;dialogHeight:500px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                document.getElementById("Hid_RetValues").value = ret
                document.getElementById('<%= btn_Add.ClientID%>').click();
            }
            else {
                document.getElementById("Hid_RowIndex").value = "";
            }
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
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btn_Add" runat="server" Text="Add Details" ToolTip="Add Details"
                            CssClass="ButtonCSS hidden" TabIndex="34" />
                        <input type="button" id="btn_Add1" runat="server" value="Add Details" class="ButtonCSS" onclick="return Add();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <table id="Table4" align="center" cellspacing="0" cellpadding="0" border="1" width="100%">
                            <tr>
                                <td id="td2" align="center" valign="top" runat="server" colspan="8">
                                    <div id="divdg" style="margin-top: 0px; overflow: auto; width: 950px; padding-top: 0px; position: relative; height: 168px; left: 0px; top: 0px;"
                                        align="center">
                                        <asp:DataGrid ID="dg1" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="False"
                                            TabIndex="38" Width="950px">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit"
                                                            ToolTip="Edit" CssClass="hidden" />

                                                        <input type="button" id="imgBtn_Edit1" runat="server" class="TitleText" style="background-image: url('../Images/edit3.PNG'); background-repeat:no-repeat" />

                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                            CommandName="Delete" ToolTip="Delete" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="CustomerBranchName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CustomerBranchName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerBranchName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Address1">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_Address1" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                            onkeypress="scroll();"
                                                            runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("Address1") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Address2">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_Address2" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                            onkeypress="scroll();"
                                                            runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("Address2") %>'></asp:TextBox>
                                                        <%-- <asp:Label ID="lbl_Address2" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.Address2") %>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="City">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_City" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.City") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="PinCode">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_PinCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PinCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="State">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_State" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.State") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Country">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Country" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Country") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Phone">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Phone" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Phone") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="FaxNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_FaxNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FaxNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="EmailId">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_EmailId" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                            onkeypress="scroll();"
                                                            runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("EmailId") %>'></asp:TextBox>
                                                        <%--  <asp:Label ID="lbl_EmailId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EmailId") %>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="CustomerId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CustomerId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="BusinessType">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lbl_BusinessTypeNames" BackColor="white" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                            onkeypress="scroll();"
                                                            runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.BusinessTypeNames") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="BusniessTypeIds" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_BusniessTypeIds" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BusniessTypeIds") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="ClientCustAddressId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ClientCustAddressId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ClientCustAddressId") %>'></asp:Label>
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
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Submit" ToolTip="Save" />
                        <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Submit" ToolTip="Edit" />
                       <%-- <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" UseSubmitBehavior="false"
                            OnClientClick="return FinalClose();" />--%>
                        <input type ="button" id="btn_Cancel" runat ="server" class ="ButtonCSS" value ="Cancel" onclick="return FinalClose();" />
                    </td>
                </tr>
                <asp:HiddenField ID="Hid_BusniessType" runat="server" />
                <asp:HiddenField ID="Hid_ProfileType" runat="server" />
                <asp:HiddenField ID="Hid_CustBankDetailId" runat="server" />
                <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                <asp:HiddenField ID="Hid_Index" runat="server" />
                <asp:HiddenField ID="Hid_CustBankMultiDetailId" runat="server" />
                <asp:HiddenField ID="Hid_RetValues" runat="server" />
                <asp:HiddenField ID="Hid_CustId" runat="server" />
                <asp:HiddenField ID="Hid_ClientCustAddressId" runat="server" />
                <asp:HiddenField ID="Hid_AddBusinessTypeId" runat="server" />
                <asp:HiddenField ID="Hid_AddBusinessTypeNames" runat="server" />
                <asp:HiddenField ID="Hid_CustomerTypeId" runat="server" />
                <asp:HiddenField ID="Hid_CategoryId" runat="server" />
                <asp:HiddenField ID="Hid_TempId" runat="server" />
                <asp:HiddenField ID="Hid_RowIndex" runat="server" />
                <asp:HiddenField ID="Hid_Id" runat="server" />
                <asp:SqlDataSource ID="SqlDataSourceMainAddress" runat="server" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"
                    ProviderName="<%$ ConnectionStrings:InstadealConnectionString.ProviderName %>"
                    SelectCommand="Id_FILL_ClientCustMultipleAddress" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Hid_CustomerId" DefaultValue="0" Name="CustomerId"
                            PropertyName="Value" Type="Int64" />
                        <asp:ControlParameter ControlID="Hid_BusniessType" DefaultValue="" Name="BusniessType"
                            PropertyName="Value" Type="String" />
                        <asp:ControlParameter ControlID="Hid_ProfileType" DefaultValue="" Name="ProfileType"
                            PropertyName="Value" Type="String" />
                        <asp:Parameter Direction="Output" Name="RET_CODE" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </table>
        </div>
    </form>
</body>
</html>
