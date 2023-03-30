<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShowSelectedSecurity.aspx.vb"
    Inherits="ShowSelectedSecurity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Selected Security</title>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="Table1"  align="left" cellspacing="0" cellpadding="0" border="0">
               <%-- <tr>
                    <td class="HeaderCSS" align="center"  >
                        Fax Selection
                    </td>
                </tr>
                <tr>
                    <td class="SubHeaderCSS"   >
                        Selected Security Details</td>
                </tr>--%>
                  
                <tr>
                    <td align="center" valign="middle">
                        <div id="div2" style="margin-top: 0px; overflow: auto; width: 800px; padding-top: 0px;
                            position: relative; height: 380px" align="left">
                            <asp:GridView ID="dg_SelectedSecurity" runat="server" AutoGenerateColumns="false"
                                CssClass="GridCSS">
                                <HeaderStyle HorizontalAlign="Left" CssClass="GridHeaderCSS" />
                                <RowStyle HorizontalAlign="left" CssClass="GridRowCSS" width = "1000px" />
                                <Columns>
                                     
                                  <%-- <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                CommandName="DeleteRow" />
                                        </ItemTemplate>
                                    </asp:TemplateField> --%>
                                    <asp:BoundField HeaderText="SecurityId" DataField="SecurityId" Visible="false" />
                                       <asp:TemplateField HeaderText="SecurityName">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txt_SecurityName" BackColor="#FFFFFF" Width="185px" Style="border-left-width: 0;
                                                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                                                runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityName") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                     
                                                 </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="SecurityName" DataField="SecurityName" />--%>
                                    <asp:BoundField HeaderText="Date" DataField="Date" />
                                    <asp:BoundField HeaderText="Rate" DataField="Rate" />
                                    <asp:BoundField HeaderText="LotSize" DataField="LotSize" />
                                    <asp:BoundField HeaderText="RatingRemark" DataField="RatingRemark" />
                                    <asp:BoundField HeaderText="Days" DataField="Days" />
                                    <asp:BoundField HeaderText="DaysOptions" DataField="DaysOptions" />
                                    <asp:BoundField HeaderText="PhysicalDMAT" DataField="PhysicalDMAT" />
                                    <asp:BoundField HeaderText="IPCalc" DataField="IPCalc" />
                                     <asp:BoundField HeaderText="RateActual" DataField="RateActual" />
                                    <asp:BoundField HeaderText="YXM" DataField="YXM" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
               <%-- <tr>
                    <td align="center">
                        <asp:Button ID="btn_Back" runat="server" Text="Back" ToolTip="Back" CssClass="ButtonCSS"
                            Height="20px" />
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="6" class="SeperatorRowCSS">
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
