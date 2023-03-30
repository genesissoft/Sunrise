<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="DailyMISReport.aspx.cs" Inherits="DailyMISReport" Title="Daily MIS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <script language="javascript" type="text/javascript">
    
    
      $(document).ready(function () {
            $("#fromCalendar").click( function () {
            
                displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate',this);

                
            });
            $("#toCalendar").click( function () {
                displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate',this);

            });
        });
        
         function Validate() {
            try {
               var card = document.getElementById("ctl00_ContentPlaceHolder1_ddlDealTrasReportType");
            if(card.selectedIndex == 0) {
                alert('Please select deal transaction report type first.');
                return false;
                }
            }
            catch (ex) {
                alert(ex.toString());
                return false;
            }
        }
    </script>

    <table width="100%" align="center" class="formTable" cellspacing="0" cellpadding="0"
        border="0">
        <tr align="left">
            <td class="SectionHeaderCSS" id="Col_Headers" runat="server">
                Report Selection
            </td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table cellpadding="5" cellspacing="0" align="center" border="0">
                  <tr align="left" id="Tr1" runat="server">
                        <td id="Td3" runat="server">
                            Company:
                        </td>
                        <td>
                            <asp:DropDownList ID="cbo_Company" runat="server" CssClass="ComboBoxCSS">
                              
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="left" id="row_FromDate" runat="server">
                        <td>
                            From Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS" Width="115px"
                                TabIndex="1"></asp:TextBox><img class="calender" id="fromCalendar" src="../Images/Calender.jpg" />
                        </td>
                    </tr>
                    <tr align="left" id="row_ToDate" runat="server">
                        <td>
                            To Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS" Width="115px" TabIndex="2"></asp:TextBox><img
                                class="calender" id="toCalendar" src="../Images/Calender.jpg" />
                        </td>
                    </tr>
                    <tr align="left" id="row_DealTranschkAll" runat="server" visible="false">
                        <td id="Td6" runat="server">
                            Select All:
                        </td>
                        <td>
                            <asp:CheckBox ID="chk_DealTranschkAll" runat="server" Checked="true" AutoPostBack="true" />
                        </td>
                    </tr>
                  
                    <tr align="left" id="row_DealTransReportType" runat="server">
                        <td id="Td2" runat="server">
                            Deal Trans Report Type:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDealTrasReportType" runat="server" CssClass="ComboBoxCSS">
                                <asp:ListItem Value="0" Text="Select">
                                </asp:ListItem>
                               
                                <asp:ListItem Value="1" Text="Deal Acknowledgement">
                                </asp:ListItem>
                                 <asp:ListItem Value="2" Text="Daily Transaction Report">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="left" id="row_DealTransTypeAll" runat="server">
                        <td id="Td1" runat="server">
                            Deal Trans Type:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDealTransType" runat="server" CssClass="ComboBoxCSS">
                                <asp:ListItem Value="0" Text="Select">
                                </asp:ListItem>
                                <asp:ListItem Value="1" Text="Trading">
                                </asp:ListItem>
                                <asp:ListItem Value="2" Text="Broking">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btn_Print" runat="server" Text="View Report" CssClass="ButtonCSS"
                                OnClientClick="return Validate();" OnClick="btnPrint_Click" TabIndex="19" Width="90px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
