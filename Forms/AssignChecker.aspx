<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="AssignChecker.aspx.vb" Inherits="Forms_AssignChecker" Title="Assign Checker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
    
    function Validation()
    {
        if(document.getElementById("ctl00_ContentPlaceHolder1_cbo_UserName").value == "")
        {
            alert('Please Select User Name')
            return false;
        }
        if(document.getElementById("ctl00_ContentPlaceHolder1_txt_CheckerDate").value == "")
        {
            alert('Please Enter Checker Date')
            return false;
        }
//         if(CheckAssign() == false)
//            {
//                return false
//            } 
//            return true
    }
    
    
     function CheckAssign()
        {
            var Checker = (document.getElementById("ctl00_ContentPlaceHolder1_Hid_Checker").value)
            alert(Checker)
            var validateTime = (document.getElementById("ctl00_ContentPlaceHolder1_Hid_CheckerValidDatetime").value)
             alert(validateTime)
            if (Checker == 'T')
            {
                    if(window.confirm("This user name is already assign with " + validateTime + " this date and time "))
                    {
                        return true
                    } 
                    else
                    {
                        return false
                    }
            
            }
           
        }
    </script>

    <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td class="HeaderCSS" align="center">
                        Assign Checker</td>
                </tr>
                <tr>
                    <td align="center">
                        <table id="Table3" width="50%" cellspacing="0" cellpadding="0" border="0" align="center">
                            <tr>
                                <td class="SeperatorRowCSS">
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS" width="200">
                                    User Name:
                                </td>
                                <td align="left" width="275">
                                    <asp:DropDownList ID="cbo_UserName" runat="server" CssClass="ComboBoxCSS" Height="19px"
                                        Width="151px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Till Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_CheckerDate" runat="server" Width="133px" CssClass="TextBoxCSS"
                                        MaxLength="50" TabIndex="7"></asp:TextBox><img id="IMG2" border="0" class="formcontent"
                                            height="14" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_CheckerDate',this);"
                                            src="../Images/Calender.jpg" style="vertical-align: top; cursor: hand" width="15" /></td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Till Time:
                                </td>
                                 <td align="left">
                                <asp:DropDownList ID="cbo_hr" runat="server" Width="52px" CssClass="ComboBoxCSS"
                                    TabIndex="11">
                                </asp:DropDownList><asp:DropDownList ID="cbo_minute" runat="server" Width="52px"
                                    CssClass="ComboBoxCSS" TabIndex="12">
                                </asp:DropDownList><asp:DropDownList ID="cbo_ampm" runat="server" Width="48px" CssClass="ComboBoxCSS"
                                    TabIndex="13">
                                    <asp:ListItem Value="AM">AM</asp:ListItem>
                                    <asp:ListItem Value="PM">PM</asp:ListItem>
                                </asp:DropDownList>
                               
                                </td>
                            </tr>
                            
                            <tr id = "row_assigndatetime" runat=server>
                            <td class="LabelCSS">
                            Assign Date and Time:
                            
                            </td>
                            <td align="left">
                            <asp:Literal  ID = "lit_prevdatetime" runat=server></asp:Literal>
                            
                            </td>
                            
                            </tr>
                            <tr>
                                <td class="SeperatorRowCSS" colspan ="2">
                                </td>
                            </tr>
                            <tr>
                            <td colspan ="2">
                            <asp:Button ID="btn_Assign" runat="server" CssClass="ButtonCSS" Text="Assign" />
                              <asp:Button ID="btn_Reassign" Visible="false" runat="server" Text="Reassign" ToolTip="Reassign"
                            CssClass="ButtonCSS" Height="20px" />
                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel Assign" ToolTip="Cancel" CssClass="ButtonCSS"
                            Height="20px" />
                            </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="Hid_txtHr" runat="server" />
                            <asp:HiddenField ID="Hid_txtMin" runat="server" />
                            <asp:HiddenField ID="Hid_UserName" runat="server" />
                            <asp:HiddenField ID="Hid_NameOfUser" runat="server" />
                             <asp:HiddenField ID="Hid_Checker" runat="server" />
                             <asp:HiddenField ID="Hid_CheckerValidDatetime" runat="server" />
                             
                             <asp:HiddenField ID="Hid_Cancel" runat="server" />
                             
                            
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>
