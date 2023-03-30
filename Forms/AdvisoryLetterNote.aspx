<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="AdvisoryLetterNote.aspx.vb" Inherits="Forms_AdvisoryLetterNote" Title="Advisory Letter Entry" %>

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
                document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "block";
                 document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "block";
                 
             
            }
            else
            {  
                document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "block";
                document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "none";
                 document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "none";
               
            }
        }             
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">
                Debit Note Entry</td>
        </tr>
        <tr>
            <td align="center">
                <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table id="Table3" cellspacing="0" width="45%" cellpadding="0" border="0">
                            <tr>
                                <td colspan="6" class="SeperatorRowCSS">
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lbl_According" runat="server">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;According To:
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass="LabelCSS">
                                        <asp:ListItem Value="D" Selected="True">Deal Date</asp:ListItem>
                                        <asp:ListItem Value="S">Settlement Date</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lbl_print" runat="server">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Print:
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rdo_Selection" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass="LabelCSS">
                                        <asp:ListItem Value="D" Selected="True">DateWise</asp:ListItem>
                                        <asp:ListItem Value="M">MonthWise</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="row_Month" runat="server">
                                <td align="right">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Select Month-Year:
                                </td>
                                <td align="left">
                                    <asp:DropDownList AutoPostBack="false" ID="cbo_Months" runat="server" CssClass="ComboBoxCSS">
                                        <asp:ListItem Text="January" Value="1" Selected="True"></asp:ListItem>
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
                                        Width="94px">
                                        <asp:ListItem Text="2009" Value="2009" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="2010" Value="2010"></asp:ListItem>
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
                            <tr id="row_FromDate" runat="server">
                                <td align="right">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;From Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS" Width="143px"
                                        TabIndex="1"></asp:TextBox>
                                    <img class="LabelCSS" height="14" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate',this);"
                                        width="16" border="0" style="vertical-align: top; cursor: hand;"></td>
                            </tr>
                            <tr id="row_ToDate" runat="server">
                                <td align="right">
                                    To Date:</td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS" Width="143px" TabIndex="2"></asp:TextBox>
                                    <img class="LabelCSS" height="14" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate',this);"
                                        width="16" border="0" style="vertical-align: top; cursor: hand;"></td>
                            </tr>
                            <%-- <tr id="tr2" runat="server">
                                <td align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Select Customer: " CssClass="LabelCSS"
                                        Width="100px"></asp:Label></td>
                                <td align="left" valign="top">
                                    <uc:SelectFields ID="srh_Customer" class="LabelCSS" runat="server" ProcName="ID_SEARCH_CustomerMaster"
                                        FormHeight="475" FormWidth="257" SelectedValueName="CM.CustomerId" ChkLabelName=""
                                        ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerName" SourceType="StoreProcedure"
                                        ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                                </td>
                            </tr>--%>
                            <tr id="tr_Broker" runat="server">
                                <td align="right">
                                    <asp:Label ID="Label2" runat="server" Text="Select Advisory: " CssClass="LabelCSS"
                                        Width="100px"></asp:Label></td>
                                <td align="left" valign="top">
                                    <uc:SelectFields ID="srh_Broker" class="LabelCSS" runat="server" ProcName="ID_SEARCH_AdvisoryBrokerMasterNew"
                                        FormHeight="475" FormWidth="257" SelectedValueName="Brokerid" ChkLabelName="" ConditionExist="true" 
                                        ConditionalFieldId="" LabelName="" SelectedFieldName="AdvisoryName" SourceType="StoredProcedure"
                                        ConditionalFieldName="" Visible="true" ShowLabel="false" ShowAll="true"></uc:SelectFields>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr align="right">
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Show" runat="server" Text="Show" CssClass="ButtonCSS" TabIndex="19" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr align="right">
                                <td style="height: 81px" colspan="2">
                                    <div id="div2" style="margin-top: 0px; overflow: auto; width: 680px; padding-top: 0px;
                                        position: relative; height: 150px">
                                        <asp:DataGrid ID="dg_Debitnote" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                                            TabIndex="38" Width="680px">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="right" CssClass="GridRowCSS" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="RefNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_RefNo" Width="100px" runat="server" CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" Width="60px" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="WDMDealNumber">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_WDMDealNumber" Width="100px" runat="server" Text='<%# container.dataitem("WDMDealNumber") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" Width="60px" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Brokername">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Brokername" runat="server" Width="305px" Text='<%# container.dataitem("Brokername") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SecurityName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SecurityName" runat="server" Width="305px" Text='<%# container.dataitem("SecurityName") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealDate" runat="server" Width="150px" Text='<%# container.dataitem("DealDate") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="70px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealEntryId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealEntryId" runat="server" Width="150px" Text='<%# container.dataitem("DealEntryId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="70px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="TransType" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_TransType" runat="server" Width="150px" Text='<%# container.dataitem("TransType") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="70px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                            </tr>
                            <tr align="right">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lbl_Msg" runat="server" CssClass="LabelCSS"></asp:Label>
                                </td>
                            </tr>
                            <tr align="right">
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Save" runat="server" Text="Save & Print" CssClass="ButtonCSS"
                                        TabIndex="19" />
                                </td>
                                <asp:HiddenField ID="Hid_ReportType" runat="server" />
                                <asp:HiddenField ID="Hid_DebitRefNo" runat="server" />
                            </tr>
                        </table>
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
