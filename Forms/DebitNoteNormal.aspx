<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="DebitNoteNormal.aspx.vb" Inherits="Forms_DebitNoteNormal" Title="Debit Note Entry" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script language="javascript" type="text/javascript">
     function DateMonthSelection()
        {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_Selection_0").checked == true)
            { 
                document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "None";
                document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "";
                 document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "";
                 
             
            }
            else
            {  
                document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "none";
                 document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "none";
               
            }
        }             
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Debit Note Entry</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table cellspacing="0" width="45%" cellpadding="0" border="0">
                            <tr align="left">
                                <td id="lbl_According" runat="server">
                                    According To:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass="LabelCSS">
                                        <asp:ListItem Value="D" Selected="True">Deal Date</asp:ListItem>
                                        <asp:ListItem Value="S">Settlement Date</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td style="padding-left: 0px;" id="lbl_print" runat="server">
                                    Print:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList ID="rdo_Selection" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass="LabelCSS">
                                        <asp:ListItem Value="D" Selected="True">DateWise</asp:ListItem>
                                        <asp:ListItem Value="M">MonthWise</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr align="left" id="row_Month" runat="server">
                                <td>
                                    Select Month-Year:
                                </td>
                                <td align="left">
                                    <asp:DropDownList AutoPostBack="false" ID="cbo_Months" runat="server" CssClass="ComboBoxCSS"
                                        Width="95px">
                                        <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList AutoPostBack="false" ID="cbo_Year" runat="server" CssClass="ComboBoxCSS"
                                        Width="75px">
                                        <asp:ListItem Text="2009" Value="2009"></asp:ListItem>
                                        <asp:ListItem Text="2010" Value="2010" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="2011" Value="2011"></asp:ListItem>
                                        <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                                        <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                        <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                                        <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                        <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                        <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                                        <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                        <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                        <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr align="left" id="row_FromDate" runat="server">
                                <td>
                                    From Date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS" Width="115px"
                                        TabIndex="1"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate',this);" /></td>
                            </tr>
                            <tr align="left" id="row_ToDate" runat="server">
                                <td>
                                    To Date:</td>
                                <td>
                                    <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS" Width="115px" TabIndex="2"></asp:TextBox><img
                                        class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate',this);" /></td>
                            </tr>
                            <tr align="left" id="tr2" runat="server">
                                <td>
                                    <%--  <asp:Label ID="Label1" runat="server" Text="Select Customer: " CssClass="LabelCSS"
                                        Width="100px"></asp:Label>--%>
                                    Select Customer:
                                </td>
                                <td style="padding-left: 0px;">
                                    <uc:SelectFields ID="srh_Customer" class="LabelCSS" runat="server" ProcName="ID_SEARCH_CustomerMasterNew"
                                        FormHeight="475" FormWidth="257" SelectedValueName="CM.CustomerId" ChkLabelName=""
                                        ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerName" SourceType="StoredProcedure"
                                        ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btn_Show" runat="server" Text="Show" CssClass="ButtonCSS" TabIndex="19" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table cellspacing="0" width="90%" cellpadding="0" border="0">
                            <tr align="center" valign="top">
                                <td>
                                    <div id="div2" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px;
                                        position: relative;">
                                        <asp:DataGrid ID="dg_Debitnote" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                                            TabIndex="38" Width="100%">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="right" CssClass="GridRowCSS" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="RefNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_RefNo" Width="100px" runat="server" CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" Width="60px" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealNumber">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealNumber" Width="100px" runat="server" Text='<%# container.dataitem("DealNumber") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" Width="60px" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Customername">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Customername" runat="server" Width="305px" Text='<%# container.dataitem("Customername") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SecurityName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SecurityName" runat="server" Width="305px" Text='<%# container.dataitem("SecurityName") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealDate" runat="server" Width="150px" Text='<%# container.dataitem("DealDate") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="70px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealEntryId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealEntryId" runat="server" Width="150px" Text='<%# container.dataitem("DealEntryId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="70px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle"  />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="WdmDeal" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_WdmDeal" runat="server" Width="150px" Text='<%# container.dataitem("WdmDeal") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="70px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="BrokerId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_BrokerId" runat="server" Width="150px" Text='<%# container.dataitem("BrokerId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="70px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle"  />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="TransType" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_TransType" runat="server" Width="150px" Text='<%# container.dataitem("TransType") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="70px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Label ID="lbl_Msg" runat="server" CssClass="LabelCSS"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" Text="Save & Print" CssClass="ButtonCSS"
                                        TabIndex="19" Width="85px" />
                                    <asp:HiddenField ID="Hid_ReportType" runat="server" />
                                    <asp:HiddenField ID="Hid_DebitRefNo" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
