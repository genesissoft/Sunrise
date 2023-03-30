<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShowQuote.aspx.vb"
    Inherits="ShowQuote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Untitled Page</title>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <script language="javascript">
        function SelectOption(img, id, name) {
            document.getElementById("Hid_FaxQuoteId").value = id
            var row = img.parentElement.parentElement
            UnselectAll(row)
            img.src = "../Images/images.JPG"
            row.style.backgroundColor = '#D1E4F8'
        }
        function UnselectAll(row) {
            var grd = row.parentElement.parentElement
            for (i = 1; i <= (grd.children[0].children.length - 2) ; i++) {
                currRow = grd.children[0].children[i]
                currRow.children[0].children[0].src = "../Images/images3.JPG"
                currRow.style.backgroundColor = 'white'
            }
        }

        function Close(strVal) {
            window.returnValue = strVal
            window.close()
            return true
        }


        function Submit() {
            var strId = document.getElementById("Hid_FaxQuoteId").value
            window.returnValue = strId
            window.close()
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="Table1" align="center" cellspacing="0" width="300px" cellpadding="0" border="0">
                <tr>
                    <td class="HeaderCSS" align="center">Show Quote
                    </td>
                </tr>

                <tr>
                    <td align="center" valign="top">
                        <div id="div2" style="margin-top: 0px; overflow: auto; width: 300px; padding-top: 0px; position: relative; height: 450px"
                            align="center">
                            <asp:GridView ID="dg_Quote" runat="server" AutoGenerateColumns="false"
                                CssClass="GridCSS">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <RowStyle HorizontalAlign="Center" CssClass="GridRowCSS" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <img src="../Images/images3.JPG" id="img_Select" style="cursor: hand" runat="server"
                                                height="13" onmouseover="" width="13" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="FaxQuoteId" DataField="FaxQuoteId" Visible="false" />
                                    <asp:BoundField HeaderText="QuoteName" DataField="QuoteName" />
                                    <asp:BoundField HeaderText="SavedDate" DataField="SavedDate" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btn_Sumbit" runat="server" Text="Submit" ToolTip="Submit" CssClass="ButtonCSS hidden" UseSubmitBehavior="false" OnClientClick="Submit();"
                            Height="20px" />
                        <input type="button" id="btn_Save" class="ButtonCSS" value="Submit" onclick="Submit();" />
                        <asp:Button ID="btn_Close" runat="server" Text="Close" ToolTip="Close" CssClass="ButtonCSS"
                            Height="20px" /></td>
                </tr>

                <tr>
                    <td colspan="6" class="SeperatorRowCSS">
                        <asp:HiddenField ID="Hid_FaxQuoteId" runat="server" />
                    </td>
                </tr>
                
            </table>
        </div>
    </form>
</body>
</html>
