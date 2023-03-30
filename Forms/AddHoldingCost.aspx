<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddHoldingCost.aspx.vb"
    Inherits="Forms_AddHoldingCost" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Holding Cost</title>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Style_New.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <script type="text/javascript">
     function Validation()
        {
            var count = 0;
            var grid = document.getElementById("<%=dg_dme.ClientID%>");
            var inputs = grid.getElementsByTagName("input");
            for(var i=0;i<inputs.length;i++)
            {
                if(inputs[i].type =="text")
                {
                   if (inputs[i].value == 0)
                   {
                        count = 0;
                   }
                   else
                   {
                    count = 1;
                   }
                }
            }
            if(count == 0)
            {
                alert ("failed");
                return false;
            }
            return true;
        }
        
        function RetValues()
        {
            window.returnValue = "";
            window.close();
        }
    </script>

</head>
<body style="margin-left: 0px; margin-top: 5px;" class="popupbackground">
    <form id="form1" runat="server">
    <div>
        <table id="Table1" width="98%" align="center" cellspacing="0" cellpadding="0" border="0"
            class="data_table">
            <tr align="left">
                <td class="SectionHeaderCSS popupbackground" style="text-align: center;">
                    Add Holding Cost
                </td>
            </tr>
            <tr class="line_separator">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    <table align="center" cellspacing="0" cellpadding="0" border="0" width="98%">
                        <tr align="center" valign="top">
                            <td style="width: 2%;">
                                &nbsp;
                            </td>
                            <td style="width: 49%;">
                                <table id="tbl_AddHoldingCost" align="center" cellspacing="0" cellpadding="0" border="0"
                                    width="100%">
                                    <tr align="center" valign="top">
                                        <td align="center">
                                            <div id="div1" style="margin-top: 0px; overflow: auto; width: 98%; padding-top: 0px;
                                                position: relative;">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="3">
                                <div id="div2" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px;
                                    position: relative;">
                                    <asp:GridView ID="dg_dme" runat="server" AutoGenerateColumns="false" CssClass="GridCSS"
                                        Width="420px">
                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" Width="420px" />
                                        <RowStyle HorizontalAlign="Left" CssClass="GridRowCSS" Width="320px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Value Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_ValueDate" CssClass="LabelCSS" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FromDate") %>'
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Holding Cost">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_HoldingRate" CssClass="TextBoxCSS" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Holdingrate") %>'
                                                        Width="200px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="line_separator">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr align="center" id="buttonid" runat="server">
                <td>
                    <asp:Button ID="btn_SaveInfo" runat="server" Text="Save Info" ToolTip="Save Info"
                        CssClass="ButtonCSS" />
                    <asp:Button ID="btn_Clear" runat="server" Text="Clear" ToolTip="Clear" CssClass="ButtonCSS" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
    <asp:HiddenField ID="Hid_FDId" runat="server" />
    <asp:HiddenField ID="Hid_AdjDealSlipId" runat="server" />
    <asp:HiddenField ID="Hid_BalAmt" runat="server" />
    <asp:HiddenField ID="Hid_CustomerId" runat="server" />
    <asp:HiddenField ID="Hid_TransType" runat="server" />
    </form>
</body>
</html>
