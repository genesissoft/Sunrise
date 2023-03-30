<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ViewReports.aspx.vb" Inherits="Forms_ViewReports" Title="View Reports" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        var shell
        var WinPrint
        function CallPrint(strid) {
            if (CheckSetting() == false) return false
            var prtContent = document.getElementById(strid);
            WinPrint = window.open('', 'Print', 'left=0,top=0,width=800,height=600,menubar=1,toolbar=0,scrollbars=0,status=0,resizable=no,alwaysRaised=yes,scrollbars=yes,screenX=100,left=100,screenY=70,top=70');
            var start = prtContent.innerHTML.indexOf('<DIV class=crtoolbar')
            var end = prtContent.innerHTML.indexOf('</DIV>')
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_PrintHTML").value = prtContent.innerHTML.replace(prtContent.innerHTML.substring(start, end), "")
            WinPrint.document.write(document.getElementById("ctl00_ContentPlaceHolder1_Hid_PrintHTML").value);
            WinPrint.document.close();
            WinPrint.focus();
            SetPrintProperties()
            return false;
        }
        function Mail() {
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=600px; dialogTop=250px; dialogHeight=380px; Help=No; Status=No; Resizable=Yes;"
            var strSearchCustomer = "SearchCustomer"
            var OpenUrl = "MailMessage.aspx"
            //OpenUrl = OpenUrl + "?SearchCustomer=" + strSearchCustomer ;
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions)
        }
        function SetPrintProperties() {
            try {
                shell = new ActiveXObject("WScript.Shell");
                shell.SendKeys("%fu");
                window.setTimeout("javascript:SetPaperSize();", 1200);
                window.setTimeout("javascript:setLandScape();", 2000);
            }
            catch (e) {
                alert('Please Change your Internet Security Settings\nGo to Internet Explorer --> Tools --> Internet Options --> Security --> Internet --> Custom Level \nGo to Initialize and script ActiveX controls not marked as safe --> Select Prompt --> Click Ok')
            }
            return false
        }

        function SetPaperSize() {
            /*if(document.getElementById("ctl00_ContentPlaceHolder1_Hid_Orientation").value=="L")
            {
                shell.SendKeys("{TAB}{TAB}{BACKSPACE}{TAB}{BACKSPACE}%a{TAB}.85{TAB}.5{TAB}.25{TAB}.5{ENTER}");
            }
            else
            {*/
            shell.SendKeys("{TAB}{TAB}{BACKSPACE}{TAB}{BACKSPACE}%o{TAB}.85{TAB}.5{TAB}.25{TAB}.5{ENTER}");
            //}
        }
        function setLandScape() {
            shell.SendKeys("%fp");
        }
        function CheckSetting() {
            try {
                shell = new ActiveXObject("WScript.Shell");
                return true
            }
            catch (e) {
                alert('Please Change your Internet Security Settings\nGo to Internet Explorer --> Tools --> Internet Options --> Security --> Internet --> Custom Level \nGo to Initialize and script ActiveX controls not marked as safe --> Select Enable --> Click Ok')
                return false
            }
        }
    </script>

    <table align="center" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr align="center" valign="top">
            <td style="padding: 0px;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
                    <tr>
                        <td align="left" style="padding: 0px;">
                            <table cellpadding="0" cellspacing="5" border="0">
                                <tr align="left">
                                    <td>Export To:
                                        <%--<asp:Label ID="lbl_Export" CssClass="LabelCSS" runat="server">Export To:</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cbo_Export" runat="server" CssClass="ComboBoxCSS" Width="158px">
                                            <asp:ListItem Text="Portable Document (PDF)" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="MS Word (DOC)" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="MS Excel (XLS)" Value="3" Selected="true"></asp:ListItem>
                                            <asp:ListItem Text="Rich Text (RTF)" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_Export" runat="server" Text="Export" CssClass="ButtonCSS" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <asp:Button ID="btn_Mail" runat="server" Text="Mail" CssClass="ButtonCSS" Visible="false" />
                            <asp:Button ID="btn_Print" runat="server" Text="Print" CssClass="ButtonCSS" Visible="False" />
                            <asp:Button ID="btn_Back" runat="server" Text="Close" CssClass="ButtonCSS" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="left" valign="top">
            <td>
                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                    ToolPanelView="GroupTree" BackColor="white" />
                <asp:HiddenField ID="Hid_FromDate" runat="server" />
                <asp:HiddenField ID="Hid_ToDate" runat="server" />
                <asp:HiddenField ID="Hid_ReportType" runat="server" />
                <asp:HiddenField ID="Hid_Month" runat="server" />
                <asp:HiddenField ID="Hid_PrintHtml" runat="server" />
                <asp:HiddenField ID="Hid_Counter" runat="server" />
                <asp:HiddenField ID="Hid_ImgIds" runat="server" />
                <asp:HiddenField ID="Hid_Intids" runat="server" />
                <asp:HiddenField ID="Hid_dealslipId" runat="server" />
                <asp:HiddenField ID="Hid_Transtype" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
