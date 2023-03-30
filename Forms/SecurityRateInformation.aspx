<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SecurityRateInformation.aspx.vb" Inherits="Forms_SecurityRateInformation"
    Title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center" colspan="2">
                Fax Selection
            </td>
        </tr>
        <tr>
            <td class="SubHeaderCSS" width="50%">
                Selection Details</td>
        </tr>
        <tr>
            <td align="center">
                <asp:Panel ID="pnl_Search" runat="server">
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btn_Search" runat="server" CssClass="ButtonCSS" Text="Search" />
                <asp:Button ID="btn_ShowAll" runat="server" CssClass="ButtonCSS" Text="Show All" />
            </td>
        </tr>
        <tr>
            <td align="center" valign="middle">
                <div id="div2" style="margin-top: 0px; overflow: auto; width: 960px; padding-top: 0px;
                    position: relative; height: 180px" align="center">
                    <asp:GridView ID="dg_Selection" runat="server" AutoGenerateColumns="false" CssClass="GridCSS">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                        <RowStyle HorizontalAlign="left" CssClass="GridRowCSS" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="EditRow" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                        CommandName="DeleteRow" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="SecurityTypeName" DataField="SecurityTypeName" />
                            <asp:BoundField HeaderText="SecurityIssuer" DataField="SecurityIssuer" />
                            <asp:BoundField HeaderText="SecurityName" DataField="SecurityName" />
                            <asp:BoundField HeaderText="MaturityDate" DataField="MaturityDate" />
                            <asp:BoundField HeaderText="CouponRate" DataField="CouponRate" />
                            <asp:BoundField HeaderText="Abbreviation" DataField="InvSchemeId" Visible="false" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="6" class="SeperatorRowCSS">
            </td>
        </tr>
    </table>
    <table cellspacing="0" cellpadding="0" width="100%" height="100%" align="center"
        border="0">
        <tr>
            <td bgcolor="#D1E4F8" colspan="4" align="center" height="20px">
                <strong>Security Rate Information</strong>
            </td>
        </tr>
    </table>
    <table width="60%" cellpadding="0" cellspacing="0" border="" align="center">
        <tr>
            <td class="LabelCSS">
                Date :
            </td>
            <td>
                <asp:TextBox ID="txt_Date" runat="server" CssClass="TextBoxCSS" TabIndex="13"></asp:TextBox>
                <img class="formcontent" height="14" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Date',this);"
                    width="15" border="0" style="vertical-align: top; cursor: hand;" id="IMG1">
            </td>
            <td>
                <asp:RadioButtonList ID="rdo_RateActual" runat="server" RepeatDirection="Horizontal"
                    RepeatLayout="Flow" CssClass="LabelCSS">
                    <asp:ListItem Value="R" Selected="True">Rate</asp:ListItem>
                    <asp:ListItem Value="A">Actual</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                Rate : &nbsp;
            </td>
            <td>
                &nbsp;<asp:TextBox ID="txt_Rate" runat="server" Width="100px" Height="16px" CssClass="fieldcontent"
                    MaxLength="20"></asp:TextBox>&nbsp;
                <asp:Button ID="btn_CalRate" runat="server" Text="Calculate Rate" ToolTip="Calculate Rate"
                    CssClass="ButtonCSS" Height="20px" />
            </td>
            <td>
                <asp:RadioButtonList ID="rdo_YXM" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                    CssClass="LabelCSS">
                    <asp:ListItem Value="Y" Selected="True">Yield</asp:ListItem>
                    <asp:ListItem Value="X">XIRR</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                LotSize : &nbsp;
            </td>
            <td>
                &nbsp;<asp:TextBox ID="txt_LotSize" runat="server" Width="340px" Height="32px" CssClass="fieldcontent"
                    TextMode="MultiLine" MaxLength="20"></asp:TextBox>
            </td>
            <td>
                <asp:RadioButtonList ID="rdo_PhysicalDMAT" runat="server" RepeatDirection="Horizontal"
                    RepeatLayout="Flow" CssClass="LabelCSS">
                    <asp:ListItem Value="D" Selected="True">DMAT</asp:ListItem>
                    <asp:ListItem Value="P">Physical</asp:ListItem>
                    <asp:ListItem Value="S">SGL</asp:ListItem>
                </asp:RadioButtonList>
                <div id="divIPCalc">
                    <asp:RadioButtonList ID="rdo_IPCalc" runat="server" CssClass="LabelCSS" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="E" Selected="True">Equal Days</asp:ListItem>
                        <asp:ListItem Value="A">Actual Days</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                Rating\Remark : &nbsp;
            </td>
            <td>
                &nbsp;<asp:TextBox ID="txt_RatingRemark" runat="server" Width="340px" Height="32px"
                    CssClass="fieldcontent" TextMode="MultiLine" MaxLength="20"></asp:TextBox>
            </td>
            <td>
                <asp:RadioButtonList ID="rdo_Days" runat="server" CssClass="LabelCSS" RepeatDirection="Horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Selected="True" Value="365">365</asp:ListItem>
                    <asp:ListItem Value="366">366</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DaysOptions" CssClass="LabelCSS"
                    runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0">
                    <asp:ListItem Selected="True" Value="F">First Year</asp:ListItem>
                    <asp:ListItem Value="A">All Year</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Button ID="btn_Sumbit" runat="server" Text="Submit" ToolTip="Submit" CssClass="ButtonCSS"
                    Height="20px" />&nbsp;
                <asp:Button ID="btn_Close" runat="server" Text="Close" ToolTip="Close" CssClass="ButtonCSS"
                    Height="20px" />
                <asp:HiddenField ID="Hid_ColWidths" runat="server" />
                <asp:HiddenField ID="Hid_ColList" runat="server" />
                <asp:HiddenField ID="Hid_DefaultSort" runat="server" />
                <asp:HiddenField ID="Hid_FieldNames" runat="server" />
                <asp:HiddenField ID="Hid_FieldValues" runat="server" />
                <asp:HiddenField ID="Hid_ColText" runat="server" />
                <asp:HiddenField ID="Hid_SecurityName" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
