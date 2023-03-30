<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
 CodeFile="ScriptEvaluationReport.aspx.cs" Inherits="Forms_ScriptEvaluationReport" Title="Script Evaluation Report Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <script language="javascript" type="text/javascript">
      $(document).ready(function () {
            $("#fromCalendar").click( function () {
                displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate',this);
            });
        });
      $(document).ready(function(){
            $("#ToCalendar").click( function() {
                displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate',this);
            });
      })       
    </script>

    <table width="100%" align="center" class="formTable" cellspacing="0" cellpadding="0"
        border="0">
        <tr align="left">
            <td class="SectionHeaderCSS" id="Col_Headers" runat="server">
                Script Evaluation  Report
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
                    <tr align="left" id="row_FromDate" runat="server">
                        <td>
                            Form Date:
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
                            <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS" Width="115px"
                                TabIndex="1"></asp:TextBox><img class="calender" id="ToCalendar" src="../Images/Calender.jpg" />
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btn_Print" runat="server" Text="View Report" OnClick="btnPrint_Click" CssClass="ButtonCSS"
                                TabIndex="19" Width="90px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

