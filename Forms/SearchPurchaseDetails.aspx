<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchPurchaseDetails.aspx.cs"
    Inherits="Forms_SearchPurchaseDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase/Transfer Details</title>
    <link type="text/css" href="../Include/Style_New.css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/Script/jquery-1.8.0.min.js" language="javascript"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

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
        $(document).ready(function () {
            $(document).on('click', '.deleterow', function () {
                var row = $(this).closest('tr');
                $(row).remove();
            });

            $("#chkSelectAll").change(function () {
                $(".select").prop('checked', $(this).prop("checked"));
            });

            $("#txtSearch").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#<%= dgPurchaseDetails.ClientID %> tr").not(".table_heading").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });

        function Submit() {
            try {
                var strData = '';
                if ($("#<%= dgPurchaseDetails.ClientID %> tr:visible input:checkbox:checked").length > 0) {
                $("#<%= dgPurchaseDetails.ClientID %> tr:visible").each(function (i, row) {
                    if (i == 0)
                        return;

                    sel = $(row).find('input:checkbox').get(0).checked;
                    if (sel) {
                        strData = strData + '{';
                        strData = strData + '"DealSlipId":"' + trim($($(row).find('.dealslipid').get(0)).text()) + '",';
                        strData = strData + '"DealSlipNo":"' + trim($($(row).find('.dealslipno').get(0)).text()) + '",';
                        strData = strData + '"DealDate":"' + trim($($(row).find('.dealdate').get(0)).text()) + '",';
                        strData = strData + '"SettlementDate":"' + trim($($(row).find('.settlementdate').get(0)).text()) + '",';
                        strData = strData + '"CustomerName":"' + trim($($(row).find('.customername').get(0)).text()) + '",';
                        strData = strData + '"AvblQuantity":"' + trim($($(row).find('.avblquantity').get(0)).text()) + '",';
                        strData = strData + '"AvblFaceValue":"' + trim($($(row).find('.avblfacevalue').get(0)).text()) + '",';
                        strData = strData + '"OrgQuantity":"' + trim($($(row).find('.orgquantity').get(0)).text()) + '",';
                        strData = strData + '"OrgFaceValue":"' + trim($($(row).find('.orgfacevalue').get(0)).text()) + '",';
                        strData = strData + '"Rate":"' + trim($($(row).find('.rate').get(0)).text()) + '",';
                        strData = strData + '"DealType":"' + trim($($(row).find('.dealtype').get(0)).text()) + '",';
                        strData = strData + '},';
                    }
                });

                strData = "[" + strData.substring(0, strData.length - 1) + "]";
                $("#<%= Hid_Id.ClientID %>").val(strData);
                window.returnValue = strData;
                window.close();
            }
            else {
                alert('please select atleast one row first.')
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
        <table cellpadding="0" cellspacing="0" width="99%" class="data_table" style="padding-left: auto; margin-right: auto; text-align: center;">
            <tr align="left" class="">
                <td>Search Purchase/Transfer Details:
                    <input type="text" id="txtSearch" class="text_box1" placeholder="type something for search..." />
                </td>
            </tr>
            <tr align="left" valign="top">
                <td>
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
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="dealdate"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Sett Date" DataField="SettlementDate" HeaderStyle-Width="6%"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="settlementdate"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Customer Name" DataField="CustomerName" HeaderStyle-Width="12%"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="customername"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Avbl Quantity" DataField="AvblQuantity" HeaderStyle-Width="7%"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="avblquantity"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Avbl Face Value" DataField="AvblFaceValue" HeaderStyle-Width="8%"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="avblfacevalue"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Org Quantity" DataField="OrgQuantity" HeaderStyle-Width="7%"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="orgquantity"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Org Face Value" DataField="OrgFaceValue" HeaderStyle-Width="8%"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="orgfacevalue"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Rate" DataField="Rate" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Right"
                                    ItemStyle-CssClass="rate"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Deal Type" DataField="DealType" HeaderStyle-Width="8%"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="dealtype"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr align="center">
                <td style="padding-top: 10px;">
                    <input id="btn_Ret" name="btn_Ret" type="button" value=" Add " class="frmButton" style="padding: 5px;"
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
