<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DealPurchaseDetailsSell.aspx.cs"
    Inherits="Forms_DealPurchaseDetailsSell" Title="Purchase/Transfer Details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Details</title>
    <meta http-equiv="refresh" content="90" />
    <link type="text/css" href="../Include/Style_New.css" rel="stylesheet" />
    <script type="text/javascript" src="../Include/Script/jquery-1.8.0.min.js" language="javascript"></script>
    <script type="text/javascript" src="../Include/Common.js"></script>
    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>

    <style type="text/css">
        body {
            font-size: 0.7em;
        }

        td {
            line-height: 20px;
        }

        .hide {
            display: none;
        }

        .deleterow {
            cursor: pointer;
        }

        .numeric {
            width: 10em;
        }
    </style>

    <script type="text/javascript">
        var seconds = 90;
        $(document).ready(function () {
            $(document).on('click', 'td.editnumber', function () {
                GetCellValue(this, 'n');
            });

            $(document).on('click', '.deleterow', function () {
                var row = $(this).closest('tr');
                $(row).remove();
                CheckDifference();
            });

            $(".numeric").keypress(function () {
                return OnlyNumericKey(event);
            });

            $("#chkSelectAll").change(function () {
                $(".select").prop('checked', $(this).prop("checked"));
            });
            $("#chkSelectAll,.select").click(function () {
                CheckDifference();
            });

            if ($("#<%= Hid_Marked.ClientID %>").val() != "") {
                $(".select").prop('checked', true);
                CheckDifference();
            }
            tick();
        });

        function tick() {
            seconds--;
            $("#counter").text((seconds < 10 ? "0" : "") + seconds);

            if (seconds > 0) {
                setTimeout(tick, 1000);
            }
        }

        function AddPurchaseDetails() {
            //try {
                var strMarked = "";
                var dealslipip = 0;
                var facevalue = 0;
                var noofbond = 0;

                $("#<%= dgPurchaseDetails.ClientID %> tr").each(function (i, row) {
                    if (i == 0)
                        return;

                    dealslipid = trim($($(row).find('.dealslipid').get(0)).text());
                    facevalue = trim($($(row).find('.adjustedquantity').get(0)).text());
                    noofbond = trim($($(row).find('.adjustedquantity').get(0)).text());
                    facevalue = (facevalue != "") ? parseFloat(facevalue) : 0;
                    noofbond = (noofbond != "") ? parseFloat(noofbond) : 0;

                    strMarked = strMarked + '{';
                    strMarked = strMarked + '"DealSlipId":"' + dealslipid + '",';
                    strMarked = strMarked + '"FaceValue":"' + facevalue + '",';
                    strMarked = strMarked + '"NoofBond":"' + noofbond + '"';
                    strMarked = strMarked + '},';
                });

                if (strMarked != "")
                    strMarked = "[" + encodeURIComponent(strMarked.substring(0, strMarked.length - 1)) + "]";

                var PageUrl = "SearchPurchaseDetails.aspx?id=" + $("#<%= Hid_DealSlipId.ClientID%>").val() + "&securityid=" + $("#<%= Hid_SecurityId.ClientID%>").val() + '&dated=' + $("#<%= Hid_Dated.ClientID%>").val() + "&dealtranstype=" + $("#<%= Hid_DealTransType.ClientID%>").val() + "&marked=" + strMarked;
                var ret = window.showModalDialog(PageUrl, 'some argument', 'dialogWidth:950px;dialogHeight:400px;center:1;status:0;resizable:0;dialogTop=100px;');

                if (ret != "" && typeof (ret) != "undefined") {
                    ret = eval(ret);
                    var strHTML = "";
                    $(ret).each(function (i, item) {
                        strHTML += "<tr align='left'>";
                        strHTML += "<td align='center'><input type='checkbox' class='select' /></td>";
                        strHTML += "<td class='hide dealslipid'>" + item.DealSlipId + "</td>";
                        strHTML += "<td align='center' class='dealslipno'>" + item.DealSlipNo + "</td>";
                        strHTML += "<td align='center'>" + item.DealDate + "</td>";
                        strHTML += "<td align='center'>" + item.SettlementDate + "</td>";
                        strHTML += "<td>" + item.CustomerName + "</td>";

                        strHTML += "<td align='right' class='adjustedquantity editnumber'>" + item.AvblQuantity + "</td>";
                        strHTML += "<td align='right' class='avblquantity hide'>" + item.AvblQuantity + "</td>";
                        strHTML += "<td align='right' class='avblfacevalue'>" + item.AvblFaceValue + "</td>";
                        strHTML += "<td align='right' class='orgquantity'>" + item.OrgQuantity + "</td>";
                        strHTML += "<td align='right'>" + item.OrgFaceValue + "</td>";
                        strHTML += "<td align='right'>" + item.Rate + "</td>";
                        strHTML += "<td>" + item.DealType + "</td>";
                        strHTML += "<td align='center'><img src='../Images/delete.gif' class='deleterow' alt='remove details' /></td>";
                        strHTML += "</tr>";
                    });
                    $("#<%= dgPurchaseDetails.ClientID %> tbody").append(strHTML);
                    $(".select").click(function () {
                        CheckDifference();
                    });
                }
            //}
            //catch (err) {
            //    alert(err);
            //    return false;
            //}
        }

        var flag = 0;
        function GetCellValue(cnt, type) {
            if (flag == 0) {
                strData = cnt.innerText;
                cnt.innerHTML = "<input type='text' style='padding:2px; border:0px; width:97%; background-color:#F7FFE4;' onblur=SetCellValue(this,'n') onkeypress='javascript:return OnlyIntegerKey(event);' maxlength='10' />";

                $(cnt).find("input").get(0).focus();
                $(cnt).find("input").get(0).value = trim(strData);
                flag = 1;
            }
        }

        function SetCellValue(cnt, type) {
            if (flag == 1 && ((type == 'd' && Validate_Date(cnt.value)) || (type == 'n' && Validate_Number(cnt.value)))) {
                $(cnt).closest("td").text(cnt.value);

                CheckDifference();
                flag = 0;
            }
            else {
                
                $(cnt).css("background-color", "#FFBABA");
            }
        }

        function CheckDifference() {
            try {
                var ret = true;
                var sel = false;
                var adjustedquantity = 0;
                var avblquantity = 0;
                var totalquantity = 0;

                $("#<%= dgPurchaseDetails.ClientID %> tr").each(function (i, row) {
                    if (i == 0)
                        return;

                    sel = $(row).find('input:checkbox').get(0).checked;
                    adjustedquantity = trim($($(row).find('.adjustedquantity').get(0)).text());
                    avblquantity = trim($($(row).find('.avblquantity').get(0)).text());

                    if (sel) {
                        adjustedquantity = (adjustedquantity != "") ? parseFloat(adjustedquantity) : 0;
                        avblquantity = (avblquantity != "") ? parseFloat(avblquantity) : 0;

                        if (!(adjustedquantity > 0)) {
                            alert('Adjusted quantity must be greater than zero row no ' + i + '.');
                            return false;
                        }
                        else if (adjustedquantity > avblquantity) {
                            alert('Adjusted quantity can\'t be greater than available quantity row no ' + i + '.');
                            return false;
                        }
                        else {
                            totalquantity += adjustedquantity;
                        }
                    }
                });

                adjustedquantity = parseFloat($("#<%= txtNoofBond.ClientID %>").val());
                $("#<%= txtDifference.ClientID %>").val(totalquantity - adjustedquantity);
            }
            catch (err) {
                alert(err);
            }
        }

        function Submit() {
            try {
                var strData = '';
                var ret = true;
                var sel = false;
                var transferid = 0;
                var warehouseid = 0;
                var dealslipip = 0;
                var dealcode = '';
                var adjustedquantity = 0;
                var avblquantity = 0;
                var totalquantity = 0;

                if ($("#<%= dgPurchaseDetails.ClientID %> tr input:checkbox:checked").length > 0) {
                    $("#<%= dgPurchaseDetails.ClientID %> tr").each(function (i, row) {
                        if (i == 0)
                            return;

                        sel = $(row).find('input:checkbox').get(0).checked;
                        dealslipid = trim($($(row).find('.dealslipid').get(0)).text());
                        dealslipno = trim($($(row).find('.dealslipno').get(0)).text());
                        adjustedquantity = trim($($(row).find('.adjustedquantity').get(0)).text());
                        avblquantity = trim($($(row).find('.avblquantity').get(0)).text());

                        if (sel) {
                            adjustedquantity = (adjustedquantity != "") ? parseFloat(adjustedquantity) : 0;
                            avblquantity = (avblquantity != "") ? parseFloat(avblquantity) : 0;
                            if (!(adjustedquantity > 0)) {
                                alert('Adjusted quantity must be greater than zero row no ' + i + '.');
                                ret = false;
                                return false;
                            }
                            else if (adjustedquantity > avblquantity) {
                                alert('Adjusted quantity can\'t be greater than available quantity row no ' + i + '.');
                                ret = false;
                                return false;
                            }
                            else {
                                strData = strData + '{';
                                strData = strData + '"DealSlipId":"' + dealslipid + '",';
                                strData = strData + '"DealSlipNo":"' + dealslipno + '",';
                                strData = strData + '"FaceValue":"' + adjustedquantity * $("#<%= Hid_CurrSecFaceValue.ClientID %>").val() + '",';
                                strData = strData + '"NoofBond":"' + adjustedquantity + '"';
                                strData = strData + '},';

                                totalquantity += adjustedquantity;
                            }
                    }
                    });
            }
            else {
                alert('please select atleast one row first.')
                ret = false;
            }

            if (!ret)
                return ret

            adjustedquantity = parseFloat($("#<%= txtNoofBond.ClientID %>").val());
            $("#<%= txtDifference.ClientID %>").val(totalquantity - adjustedquantity);

                if (totalquantity != adjustedquantity) {
                    alert('Sum of adjusted value must be equal to ' + adjustedquantity + '.');
                    return false;
                }

                if (strData != "") {
                    strData = "[" + strData.substring(0, strData.length - 1) + "]";
                    $("#<%= Hid_Id.ClientID %>").val(strData);
                    window.returnValue = strData;
                    window.close();
                }
            }
            catch (err) {
                alert(err);
                return false;
            }
        }

        function CloseWindows() {
            window.returnValue = "";
            window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="98%" class="data_table" style="padding-left: auto; margin-right: auto; text-align: center;">
            <tr align="left">
                <td>Purchase Details <a href="javascript:AddPurchaseDetails();" style="font-weight: bold;display :none">Add Purchase</a>
                </td>
                <td align="right">
                    <div>
                        Page will reload in <span id="counter" style="color: red;"></span>&nbsp;Seconds
                    </div>
                </td>
            </tr>
            <tr align="left" valign="top">
                <td colspan="2">
                    <div style="height: 300px; overflow: auto; text-align: center; border: 1px solid #8FBAEF; width: 950px;">
                        <asp:DataGrid ID="dgPurchaseDetails" runat="server" CssClass="table_border_right_bottom tablerowbg"
                            AutoGenerateColumns="false" Width="1400px">
                            <HeaderStyle CssClass="table_heading" />
                            <Columns>
                                <asp:TemplateColumn HeaderStyle-Width="2%">
                                    <HeaderTemplate>
                                        <input id="chkSelectAll" type="checkbox" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input type="checkbox" class="select" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn HeaderText="DealSlipId" DataField="DealSlipId" HeaderStyle-CssClass="hide"
                                    ItemStyle-CssClass="hide dealslipid"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Deal Slip No" DataField="DealSlipNo" HeaderStyle-Width="8%"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="dealslipno"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Deal Date" DataField="DealDate" HeaderStyle-Width="6%"
                                    ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Sett Date" DataField="SettlementDate" HeaderStyle-Width="6%"
                                    ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Customer Name" DataField="CustomerName" HeaderStyle-Width="12%"
                                    ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Avbl Quantity" DataField="AvblQuantity" HeaderStyle-Width="7%"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="adjustedquantity editnumber"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="AvblQuantity" DataField="AvblQuantity" ItemStyle-HorizontalAlign="Right"
                                    ItemStyle-CssClass="avblquantity hide" HeaderStyle-CssClass="hide"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Avbl Face Value" DataField="AvblFaceValue" HeaderStyle-Width="8%"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="avblfacevalue"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Org Quantity" DataField="OrgQuantity" HeaderStyle-Width="7%"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="orgquantity"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Org Face Value" DataField="OrgFaceValue" HeaderStyle-Width="8%"
                                    ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Rate" DataField="Rate" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Deal Type" DataField="DealType" HeaderStyle-Width="8%"
                                    ItemStyle-HorizontalAlign="Left" Visible ="false" ></asp:BoundColumn>
                                <asp:TemplateColumn HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <img src="../Images/delete.gif" class="deleterow" alt="remove details" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">Quantity To Be Adjusted:
                    <asp:TextBox ID="txtFaceValue" runat="server" CssClass="text_box1 numeric hide" ReadOnly="true"></asp:TextBox>
                    <asp:TextBox ID="txtNoofBond" runat="server" CssClass="text_box1 numeric" ReadOnly="true"></asp:TextBox>
                </td>
                <td align="right">Difference:
                    <asp:TextBox ID="txtDifference" runat="server" CssClass="text_box1 numeric" ReadOnly="true"
                        Text="0.00"></asp:TextBox>
                </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    <input id="btn_Ret" name="btn_Ret" type="button" value=" Submit " class="frmButton" style="padding: 5px;"
                        onclick="javascript: Submit();" />
                    <input id="btn_Cancel" name="btn_Cancel" type="button" value=" Cancel " class="frmButton" style="padding: 5px;"
                        onclick="CloseWindows();" />

                    <asp:HiddenField ID="Hid_Dated" runat="server" />
                    <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                    <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                    <asp:HiddenField ID="Hid_DealTransType" runat="server" />
                    <asp:HiddenField ID="Hid_Marked" runat="server" />
                    <asp:HiddenField ID="Hid_CurrSecFaceValue" runat="server" />
                    <asp:HiddenField ID="Hid_MarketType" runat="server" />
                    <asp:HiddenField ID="Hid_Id" runat="server" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
