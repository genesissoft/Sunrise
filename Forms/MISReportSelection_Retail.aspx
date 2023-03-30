<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="MISReportSelection_Retail.aspx.vb" Inherits="Forms_MISReportSelection_Retail"
    Title="MIS Retail" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery-ui-1.8.23.custom.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.core.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var rptName = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value
            if (rptName == "InwardOutwardSecurityWise") $("#ctl00_ContentPlaceHolder1_btn_Export").removeClass("hidden");
            if (rptName == "IPDateReport") {
                var Input = $('#<%=rdo_IPDate.ClientID %> input[type=radio]:checked').val();
                if (Input == "B") {
                    $(".jsdate").datepicker({
                        showOn: "button",
                        buttonImage: "../Images/calendar.gif",
                        buttonImageOnly: true,
                        changeMonth: true,
                        changeYear: true,
                        dateFormat: 'dd/mm/yy',
                        maxDate: new Date(),
                        buttonText: 'Select date as (dd/mm/yyyy)'
                    });
                }
                if (Input == "P") {
                    $(".jsdate").datepicker({
                        showOn: "button",
                        buttonImage: "../Images/calendar.gif",
                        buttonImageOnly: true,
                        changeMonth: true,
                        changeYear: true,
                        dateFormat: 'dd/mm/yy',
                        minDate: new Date(),
                        buttonText: 'Select date as (dd/mm/yyyy)'
                    });
                }
            }
            else {
                $(".jsdate").datepicker({
                    showOn: "button",
                    buttonImage: "../Images/calendar.gif",
                    buttonImageOnly: true,
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'dd/mm/yy',
                    buttonText: 'Select date as (dd/mm/yyyy)'
                });
            }


            if (rptName != "RetailDebitRpt") {
               
                jQuery("#ctl00_ContentPlaceHolder1_txt_FromDate").attr("onchange", "");
                jQuery("#ctl00_ContentPlaceHolder1_txt_ToDate").attr("onchange", "");
            }



            //$("#fromCalendar").click(function () {

            //    displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate', this);
            //    if (rptName == "RetailDebitRpt") {
            //        alert(ll);
            //        FillConditionalValue1('ctl00_ContentPlaceHolder1_srh_RetailDebit', 'ctl00_ContentPlaceHolder1_txt_FromDate');
            //    }

            //});
            //$("#toCalendar").click(function () {
            //    displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate', this);
            //    if (rptName == "RetailDebitRpt") {
            //        FillConditionalValue2('ctl00_ContentPlaceHolder1_srh_RetailDebit', 'ctl00_ContentPlaceHolder1_txt_ToDate');
            //    }
            //});
        });


        function Validation() {

            obj = document.getElementById("ctl00_ContentPlaceHolder1_srh_Security__Hid_SelectedId");
            if (obj.value == "") {
                AlertMessage('Validation', 'Please select security name.', 175, 450);
                return false;
            }

            //Commented By Khatija On 20 June 2017 -- temporary commented
            //document.getElementById("div_onsaveclick").style.display = "block";
            //Commented By Khatija On 20 June 2017 -- temporary commented
        }

        function HideLoader() {
            alert("Hide");
            document.getElementById("div_onsaveclick").style.display = "none";
        }

        function NoRecordsFound() {
            AlertMessage("Validation", "Sorry!!! No Records available to show report.", 175, 450);
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="SectionHeaderCSS" align="left" colspan="4">MAIN SECTION
            </td>
        </tr>
        <tr align="center">
            <td>
                <table id="Table2" align="center" cellspacing="0" cellpadding="0" border="0" width="80%">
                    <tr align="left" id="row_FromDate" runat="server">
                        <td align="right">From Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS jsdate" Width="115px"
                                TabIndex="1" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left" id="row_ToDate" runat="server">
                        <td align="right">To Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS jsdate" Width="115px"
                                TabIndex="2" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                     <tr align="left" id="row_customer" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lbl_Customer" runat="server" Text="Select Customer:" CssClass="LabelCSS"></asp:Label></td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_CustomerName" runat="server" ProcName="ID_SEARCH_CustomerMasterNew"
                                FormHeight="470" FormWidth="257" SelectedValueName="CM.CustomerId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_RetailDebitNote" runat="server" visible="false">
                        <td>RefNo:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_RetailDebit" CheckYearCompany="true" runat="server" ProcName="ID_SEARCH_RetailDebitRefNo"
                                FormHeight="475" FormWidth="900" SelectedValueName="RefNo" ChkLabelName=""
                                LabelName="" SelectedFieldName="RefNoText" SourceType="StoredProcedure"
                                ConditionalFieldId1="txt_FromDate" ConditionalFieldId2="txt_ToDate"
                                Visible="true" ShowLabel="false" ConditionExist="true" ShowAll="true"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_Dealer" runat="server" visible="false">
                        <td align="right">Dealer:
                        </td>
                        <td align="left">
                            <uc:Search ID="srh_UserBusinessHead" runat="server" AutoPostback="true" ProcName="ID_SEARCH_UserBusinessHeadsRpt"
                                SelectedFieldName="NameOfUser" SourceType="StoredProcedure" TableName="UserMaster"
                                ConditionalFieldName="" ConditionalFieldId="" ConditionExist="true" Width="150"></uc:Search>
                        </td>
                    </tr>
                    <tr id="row_Comp" runat="server" visible="false">
                        <td align="right">Company :
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="rdo_Company" runat="server" Width="100px" CssClass="ComboBoxCSS"
                                AutoPostBack="true">
                                <asp:ListItem Value="C">TCS</asp:ListItem>
                                <asp:ListItem Value="F">TFC</asp:ListItem>
                                <asp:ListItem Value="M">TIAPL</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="row_TFCComp" visible="false" runat="server">
                        <td align="right">Report Type:</td>
                        <td align="left">
                            <asp:RadioButtonList ID="rdo_TFCRpt" runat="server" BorderStyle="None" BorderWidth="1px"
                                AutoPostBack="true" CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Selected="True" Value="B">Broking</asp:ListItem>
                                <asp:ListItem Value="T">Trading</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>


                    <tr id="row_IPDate" visible="false" runat="server">
                        <td align="right">Report Type:</td>
                        <td align="left">
                            <asp:RadioButtonList ID="rdo_IPDate" runat="server" BorderStyle="None" BorderWidth="1px"
                                AutoPostBack="true" CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Selected="True" Value="B">Back Dated</asp:ListItem>
                                <asp:ListItem Value="P">Post Dated</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr align="left" id="trInwardOutwardSecurity" runat="server" style="display: none;">
                        <td align="right">Select Security:		
                        </td>
                        <td align="left">
                            <uc:Search ID="srh_Security_" runat="server" PageName="Security_" AutoPostback="true"
                                SelectedFieldId="Id" SelectedFieldName="SecurityName" FormWidth ="700"/>
                        </td>
                    </tr>

                    <tr align="left">
                        <td align="right">&nbsp;
                        </td>
                        <td align="left">

                            <asp:Button ID="btn_Print" runat="server" Text="View Report" CssClass="ButtonCSS"
                                TabIndex="19" Width="90px" />
                            <asp:Button ID="btn_Export" runat="server" Text="Export Report" CssClass="ButtonCSS hidden"
                                TabIndex="19" Width="90px" OnClientClick="return Validation();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" id="trInwardOutwardGrid" runat="server" style="display: none;">
            <td>
                <div id="DataDivInwardOutward" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px; position: relative; height: 400px; top: 0px; left: 0px; overflow: auto;">
                    <asp:GridView ID="dg_InwardOutwrad" runat="server" AllowPaging="true" PageSize="20" AutoGenerateColumns="false" CssClass="GridCSS " OnPageIndexChanging="OnPageIndexChanging">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                        <RowStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                        <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="Yellow" />
                        <Columns>
                            <asp:BoundField ItemStyle-Width="150px" DataField="Deal Slip No" HeaderText="Deal Slip No" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Deal Slip Type" HeaderText="Deal Slip Type" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Deal Date" HeaderText="Deal Date" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Settlement Date" HeaderText="Settlement Date" />
                            <asp:BoundField ItemStyle-Width="250px" DataField="Counter Party" HeaderText="Counter Party" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Rate" HeaderText="Rate" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="A1" HeaderText="Inward Quantity" DataFormatString="{0:N2}" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="A2" HeaderText="Inward Value" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="A3" HeaderText="Outward Quantity" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="A4" HeaderText="Outward Value" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="A5" HeaderText="Closing Quantity" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="A6" HeaderText="Closing Value" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>

    <div id="div_onsaveclick" style="display: none; vertical-align: middle; text-align: center;"
        align="center">
        <img src="../Images/loading2.gif" alt="" />
    </div>
      <asp:HiddenField ID="Hid_ReportType" runat="server" />
    <asp:HiddenField ID="Hid_SecurityWidth" runat="server" Value="300" />
    <asp:HiddenField ID="Hid_InwardOutwardIndex" runat="server" Value="G" />
    <asp:HiddenField ID="Hid_InwardOutwardLastIndex" runat="server" Value="L" />
    <asp:HiddenField ID="Hid_InwardOutwardDeleteIndex" runat="server" Value="4" />
    <asp:HiddenField ID="Hid_InwardOutwardRowIndex" runat ="server" Value  ="5" />

    <asp:HiddenField ID="Hid_A1_FooterIndex" runat="server" Value="6" />
    <asp:HiddenField ID="Hid_A2_FooterIndex" runat="server" Value="7" />
    <asp:HiddenField ID="Hid_A3_FooterIndex" runat="server" Value="8" />
    <asp:HiddenField ID="Hid_A4_FooterIndex" runat="server" Value="9" />
    <asp:HiddenField ID="Hid_A5_FooterIndex" runat="server" Value="10" />
    <asp:HiddenField ID="Hid_A6_FooterIndex" runat="server" Value="11" />
</asp:Content>
