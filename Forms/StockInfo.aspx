<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StockInfo.aspx.vb" Inherits="Forms_StockInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>StockInfo</title>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="Table1" align="left" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td align="center" valign="middle">
                        <div id="div2" style="margin-top: 0px; overflow: auto; width: 350px; padding-top: 0px;
                            position: relative; height: 100px" align="center">
                            <asp:GridView ID="dg_StockDtl" runat="server" AutoGenerateColumns="false" CssClass="GridCSS">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <RowStyle HorizontalAlign="Center" CssClass="GridRowCSS"   />
                                <Columns>
                                    <asp:BoundField HeaderText="NameOfUser" DataField="NameOfUser" />
                                    <asp:BoundField HeaderText="BlockedFaceValue" DataField="BlockedFaceValue" />
                                    <asp:BoundField HeaderText="Type" DataField="Type" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                 <asp:HiddenField ID="Hid_ID" runat="server" />
                <tr>
                    <td colspan="6" class="SeperatorRowCSS">
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
