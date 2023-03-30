<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="AddAccYear.aspx.vb" Inherits="Forms_AddAccYear" Title="Add Accounting Year" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">

        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_AccYear").value) == "") {
                AlertMessage('Validation', 'Please Enter Accounting Date.', 175, 450);
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_StartDate").value) == "") {
                AlertMessage('Validation', 'Please Select Start Date.', 175, 450);
                return false;
            }

            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_EndDate").value) == "") {
                AlertMessage('Validation', 'Please Select End Date.', 175, 450);
                return false;
            }
        }

    </script>

    <asp:ScriptManagerProxy runat="server" EnableViewState="true">
    </asp:ScriptManagerProxy>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
            <ContentTemplate>
                <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr align="left">
                        <td class="SectionHeaderCSS">Add Accounting Year</td>
                    </tr>
                    <tr class="line_separator">
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr align="center" valign="top">
                        <td>
                            <table width="35%" cellspacing="0" cellpadding="0" border="0" align="center">
                                <tr align="left">
                                    <td>Accounting Year:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_AccYear" runat="server" CssClass="TextBoxCSS" MaxLength="20"></asp:TextBox><em><span
                                            style="color: Red; vertical-align: super;">*</span></em>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Start Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_StartDate" runat="server" CssClass="TextBoxCSS jsdate" MaxLength="20"
                                            ReadOnly="True"></asp:TextBox>
                                       </td>
                                </tr>
                                <tr align="left">
                                    <td>End Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_EndDate" runat="server" CssClass="TextBoxCSS" MaxLength="20"
                                            ReadOnly="True"></asp:TextBox>
                                        <img id="Img1" class="calender" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_EndDate',this);"
                                            src="../Images/Calender.jpg" /></td>
                                </tr>
                                <tr class="line_separator">
                                    <td colspan="2"></td>
                                </tr>
                                <tr align="left">
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Button ID="btn_AddAccYear" runat="server" ToolTip="Click to Add a new Accounting Year"
                                            Text="Add Accounting Year" CssClass="ButtonCSS" Width="140px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <asp:SqlDataSource ID="SqlDataSourceYear" runat="server" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"
                        ProviderName="<%$ ConnectionStrings:InstadealConnectionString.ProviderName %>"
                        SelectCommand="ID_FILL_YEAR" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="0" Direction="InputOutput" Name="RET_CODE" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </table>
            </ContentTemplate>
            <%-- <Triggers>
                        <atlas:ControlEventTrigger ControlID="btn_AddAccYear" EventName="Click" />
                    </Triggers>--%>
        </asp:UpdatePanel>
    </div>
</asp:Content>
