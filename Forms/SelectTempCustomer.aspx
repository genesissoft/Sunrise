<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectTempCustomer.aspx.vb"
    Inherits="Forms_SelectTempCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>

    <base target="_self"></base>
    <title>Select Temporary Customer</title>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>

    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>
    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">
        function ShowAddTempCustomer() {
            $("#dialogTempCustomer").dialog({
                resizable: false,
                modal: true,
                height: 400,
                width: 650,
                buttons: {
                    Save: function () { $(this).dialog("close"); },
                    Cancel: function () { $(this).dialog("close"); }
                }
            });
        }

        function Close(ret) {

            window.returnValue = ret;
            window.close();
        }

        function AddTempCust() {
           
            var strAddCustomer = "AddCustomer"
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=500px; dialogTop=230px; dialogHeight=300px; Help=No; Status=No; Resizable=Yes;"
            var OpenUrl = "TempCustomer.aspx"

            OpenUrl = OpenUrl + "?AddCustomer=" + strAddCustomer;
            var ret = window.showModalDialog(OpenUrl, 'some argument', 'dialogWidth:500px;dialogHeight:550px;center:1;status:0;resizable:0;');

            if (typeof (ret) != "undefined") {
                document.getElementById("Hid_Id").value = "AddTempCust|" + ret;
                //alert(document.getElementById("Hid_Id"));
                document.getElementById('<%= btn_Post.ClientID%>').click();
            }
            else {
                //alert("false ....");
            }
        }
        //function AddTempCust() {
        //    debugger;
        //    var strAddCustomer = "AddCustomer"
        //    var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=500px; dialogTop=230px; dialogHeight=300px; Help=No; Status=No; Resizable=Yes;"
        //    var OpenUrl = "TempCustomer.aspx"
        //    OpenUrl = OpenUrl + "?AddCustomer=" + strAddCustomer;
        //    var ret = window.open(OpenUrl, 'some argument', 'dialogWidth:400px;dialogHeight:300px;center:1;status:0;resizable:0;');
        //    alert("5");
        //    if (typeof (ret) != "undefined") {

        //        //return false
        //        alert("6");
        //    }
        //    else {

        //        //return true
        //    }
        //}

        function Delete(strId) {
            if (window.confirm("Are you sure you want to Delete this Bank Name?")) {
                document.getElementById("Hid_TempCustId").value = strId;
                return true
            }
            else {
                return false
            }
        }

        function Update(strId) {
          
            var strCustomerId = document.getElementById("Hid_TempCustId").value = strId;
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=500px; dialogTop=230px; dialogHeight=300px; Help=No; Status=No; Resizable=Yes;"
            var OpenUrl = "TempCustomer.aspx"
            OpenUrl = OpenUrl + "?CustomerId=" + strCustomerId;
            var ret = window.showModalDialog(OpenUrl, 'some argument', 'dialogWidth:500px;dialogHeight:550px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                document.getElementById("Hid_Id").value = "UpdateTempCustomer|" + ret;
                //alert(document.getElementById("Hid_Id"));
                document.getElementById('<%= btn_Post.ClientID%>').click();
            }
            else {
                
            }

        }
        function ConvertUCase(txtBox) {
            txtBox.value = txtBox.value.toUpperCase()
        }

        function Validation() {
          
            var grd = document.getElementById("dg_TempCust")
            var Selected = false;

            if (grd != null) {
                if (grd.rows.length == 1) {
                    alert("Customer Not Available")
                    return false;
                }
                for (i = 1; i <= (grd.rows.length - 1) ; i++) {
                    currRow = grd.children[0].children[i]
                    var chkBox = currRow.children[0].children[0].children[0]
                    if (chkBox.checked == true) {
                        Selected = true
                        break
                    }
                }

                if (Selected == false) {
                    alert("Please select atleast one option")
                    return false;
                }

                Select_FaxSelection();
            }
        }

        function Select_FaxSelection() {
           
            var grd = document.getElementById("dg_TempCust")
            var cnt = 0;
            var labelCell;
            var cell;

          
            var cust_id;
            var cust_name;
            var contact_person;
            var FieldId;
            var CustomerCity;
            var EmailId;
            var strRet = "";
            $('#dg_TempCust tr').each(function (i, row) {
                if (i == 0)
                    return;
                var sel1 = $(row).find('input:checkbox').get(0).checked;

                if (sel1) {
                    cust_id = $($(row).find('td')[4]).text().trim();
                    cust_name = $($(row).find('td')[2]).text().trim();
                    contact_person = $($(row).find('td')[3]).text().trim();
                    FieldId = "1,30,21,64,10,3,4,5,11,26";
                    CustomerCity = "";
                    EmailId = $($(row).find('td')[5]).text().trim();
                    strRet += cust_id + "!" + cust_name + "!" + contact_person + "!" + FieldId + "!" + CustomerCity + "!" + EmailId + "|";
                }
            });
            document.getElementById("Hid_Id").value = strRet;
            window.returnValue = strRet;
            window.close();
        }

        function Select() {
            var grd = document.getElementById("dg_TempCust")
            var cnt = 0;
            var labelCell;
            var cell;
            if (grd != null) {
                if (grd.rows.length == 1) {
                    alert("Customer Not Available")
                    return false;
                }
            }
            for (i = 1; i <= (grd.rows.length - 1) ; i++) {
                currRow = grd.children[0].children[i]
                var chkBox = currRow.children[0].children[0].children[0]
                if (chkBox.checked == true) {
                    //Selected = true
                    cnt = cnt + 1
                    var txtval = currRow.children[1].innerHTML;
                    //break;
                }
            }
            if (cnt == 0) {
                alert("Please select any one option")
                return false;
            }
            if (cnt > 1) {
                alert("Please select only one option")
                return false;
            }


            var cust_id;
            var cust_name;
            var contact_person;
            $('#dg_TempCust tr').each(function (i, row) {
                if (i == 0)
                    return;
                var sel1 = $(row).find('input:checkbox').get(0).checked;

                if (sel1) {
                    cust_id = $($(row).find('td')[4]).text().trim();
                    alert(cust_id);
                    cust_name = $($(row).find('td')[2]).text().trim();
                    contact_person = $($(row).find('td')[3]).text().trim();
                }
            });
            document.getElementById("Hid_Id").value = cust_name + "|" + contact_person;
            window.returnValue = cust_id + "|" + cust_name + "|" + contact_person;
            window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td class="HeaderCSS" align="center" colspan="2">Select Temporary Customer
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:DropDownList ID="cbo_Selection" runat="server" CssClass="ComboBoxCSS" Height="19px"
                            Width="130px">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="CustomerName">CustomerName</asp:ListItem>
                            <asp:ListItem Value="ContactPerson">ContactPerson</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txt_Search" runat="server" Width="120px" CssClass="TextBoxCSS" MaxLength="50"
                            TabIndex="7"></asp:TextBox>
                        <asp:Button ID="btn_Search" runat="server" CssClass="ButtonCSS" Text="Search" Width="70px" />
                        <asp:Button ID="btn_ShowAll" runat="server" CssClass="ButtonCSS" Text="Show All"
                            Width="90px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6" class="SeperatorRowCSS" align="center"></td>
                </tr>
                <tr>
                    <td align="center" valign="middle" style="height: 266px">
                        <div id="div2" style="margin-top: 0px; overflow: auto; width: 580px; padding-top: 0px; position: relative; height: 250px"
                            align="center">
                            <asp:DataGrid ID="dg_TempCust" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                                TabIndex="38" Width="520px">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                <Columns>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_Select" runat="server" Width="22px" />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hid_CustID" runat="server" Value='<%# container.dataitem("CustomerId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CustomerName" Width="250px" runat="server" Text='<%# container.dataitem("CustomerName") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Contact Person">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ContactPerson" runat="server" Width="220px" Text='<%# container.dataitem("ContactPerson") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="CustomerId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CustomerId" runat="server" Width="50px" Text='<%# container.dataitem("CustomerId") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="FieldId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_FieldId" runat="server" Width="50px" Text='<%# container.dataitem("FieldId") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="EmailId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_EmailId" runat="server" Width="50px" Text='<%# container.dataitem("EmailId") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit" CommandArgument='<%# Container.DataItem("CustomerId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                CommandName="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btn_Ret" runat="server" CssClass="ButtonCSS" Text="Submit" OnClick="btn_Ret_Click" />

                        <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Close" OnClientClick="return Close();" />
                        <asp:Button ID="btn_Addnew" runat="server" CssClass="ButtonCSS" Text="Add New" Visible="false" />
                        <asp:Button ID="btn_Post" runat="server" Style="display: none;" />
                        <input type="button" value="Add New" class="ButtonCSS" onclick="AddTempCust();" />
                    </td>
                    <asp:HiddenField ID="Hid_UserId" runat="server" />
                    <asp:HiddenField ID="Hid_TempCustId" runat="server" />
                    <asp:HiddenField ID="Hid_RetValues" runat="server" />
                    <asp:HiddenField ID="Hid_Id" runat="server" />
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
