<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="Calendar.aspx.vb" Inherits="Forms_Calendar" Title="Calender" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
  function Validation()
    {   
        //alert(document.getElementById("ctl00_ContentPlaceHolder1_lst_Datelist").options.length)
         if(document.getElementById("ctl00_ContentPlaceHolder1_lst_Datelist").options.length == 0)   
        {
             AlertMessage('Validation', 'Please Select Date which you want to Add',175,450);
             return false;        
        }                             
       
    }
  function ValidationRemove()
    {
        if (((document.getElementById ("ctl00_ContentPlaceHolder1_lst_Datelist").value)) == "")
        {
            AlertMessage('Validation', 'Please Select Date which you want to remove',175,450);
            return false;
        }    
    }
     function ValidationAdd()
       {
       var calendar = document.getElementById("ctl00_ContentPlaceHolder1_Calendar1")  
       var rowCount = calendar.children[0].children.length-2;
       var colCount = 5
       var colourname = ""
       var blnExist = false
  
        for(i=2; i<=rowCount; i++)
        {
            for(j=1;j<=colCount; j++)
            {            
                currRow = calendar.children[0].children[i].children[j]
                colourname = currRow.style.backgroundColor.toUpperCase()
                if(colourname == '#009999')
                {
                    blnExist = true
                    break
                }            
            } 
        }
        if(blnExist == false)     
        {
//             alert('Please Select Date which you want to Add');
//             return false;        
        }                             
       }
    </script>

    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Calendar Master</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table cellspacing="0" width="50%" cellpadding="0" border="0" align="center">
                            <tr>
                                <td align="left">
                                    <table id="Table2" cellspacing="0" cellpadding="0" width="100%" class="table_border_right_bottom">
                                        <tr align="center">
                                            <td>
                                                <asp:DropDownList ID="Cbo_Month" Width="100px" runat="server" CssClass="ComboBoxCSS"
                                                    AutoPostBack="true" TabIndex="1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="Cbo_Year" Width="100px" runat="server" CssClass="ComboBoxCSS"
                                                    AutoPostBack="true" TabIndex="1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <b>List Of Holidays</b></td>
                                        </tr>
                                        <tr align="center" valign="middle">
                                            <td colspan="2" style="padding: 0px;">
                                                <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0" class="table_border_none">
                                                    <tr align="center" valign="top">
                                                        <td style="width: 80%;">
                                                            <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#3366CC"
                                                                BorderWidth="1px" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
                                                                ForeColor="#003399" Height="200px" Width="220px" CellPadding="1">
                                                                <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                                                <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                                                                <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                                                                <OtherMonthDayStyle ForeColor="#999999" />
                                                                <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                                                                <DayHeaderStyle BackColor="#99CCCC" Height="1px" ForeColor="#336666" />
                                                                <TitleStyle BackColor="#003399" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF"
                                                                    BorderColor="#3366CC" BorderWidth="1px" Height="15px" />
                                                                <WeekendDayStyle BackColor="#CCCCFF" />
                                                            </asp:Calendar>
                                                        </td>
                                                        <td align="center" valign="middle" style="width: 20%; border-left: 1px solid #8FBAEF;
                                                            padding-top: 0px; padding-bottom: 0px;">
                                                            <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btn_AddDate" runat="server" CssClass="ButtonCSS" Text="Add"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btn_Removedate" runat="server" CssClass="ButtonCSS" Text="Remove"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:ListBox ID="lst_Datelist" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                                    Width="120px" Height="200px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS" />
                                    <%-- <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS"
                                        Height="20px" />--%>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3">
                                    <asp:Literal ID="lit_msg" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <asp:HiddenField ID="Hid_CompId" runat="server" />
                            <asp:HiddenField ID="Hid_Date" runat="server" />
                            <asp:HiddenField ID="Hid_HolidayId" runat="server" />
                            <asp:HiddenField ID="Hid_Month" runat="server" />
                            <asp:HiddenField ID="Hid_AddDate" runat="server" />
                            <asp:HiddenField ID="Hid_SessionYear" runat="server" />
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
