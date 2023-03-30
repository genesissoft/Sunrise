<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="AdminReport.aspx.vb" Inherits="Forms_AdminReport" Title="Admin Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">
        
 function OpenWindow(strRpt)
{
    var strFromDate = document.getElementById("ctl00_ContentPlaceHolder1_Hid_FromDate").value;
    var strToDate = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ToDate").value;
    var strSelection = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Selection").value;
    if(strSelection == "Dealer")
    {
        var strDealerId = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Dealer").value;
    }
    else
    {
        var strDealerId = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Customer").value;
    }
          
    pageUrl = "ShowFullScreen.aspx?Selection=" + strSelection + "&DealerId=" + strDealerId + "&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&RptName=" + strRpt;
    var x=((screen.width-950) / 2);
    var y=((screen.height-700) / 2);          
    var ret = window.open(pageUrl, target = "_blank", "left="+ x +",top="+ y +",height=650,width=950,toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes,resizable=no, copyhistory=yes");
    if ((typeof (ret) == "undefined") || (ret == "")) 
    {
    return false;
    }
    else 
    {
    return false;
    }  
}


 function Validation()
 {
    document.getElementById("ctl00_ContentPlaceHolder1_Hid_FromDate").value = document.getElementById("ctl00_ContentPlaceHolder1_txt_FromDate").value
    document.getElementById("ctl00_ContentPlaceHolder1_Hid_ToDate").value = document.getElementById("ctl00_ContentPlaceHolder1_txt_ToDate").value
    if(document.getElementById("ctl00_ContentPlaceHolder1_Hid_Selection").value == "Dealer")
    {
        if(document.getElementById("ctl00_ContentPlaceHolder1_cbo_Dealer").value == 0)
        {
            alert('Please select dealer name');
            return false; 
        }
    }
    else
    {
        if(document.getElementById("ctl00_ContentPlaceHolder1_cbo_Customer").value == 0)
        {
            alert('Please select Customer name');
            return false; 
        }
    }
    
    if(document.getElementById("ctl00_ContentPlaceHolder1_txt_ToDate").value == "" && document.getElementById("ctl00_ContentPlaceHolder1_txt_FromDate").value == "")
    {
        alert('Please Enter The Date');
        return false;
    }
    var DateReturn;
    DateReturn= DateValidation(document.getElementById("ctl00_ContentPlaceHolder1_txt_FromDate"));    
    if (DateReturn==false)
    {
         return false;
    }     
    DateReturn= DateValidation(document.getElementById("ctl00_ContentPlaceHolder1_txt_ToDate"));    
    if (DateReturn==false)
    {
         return false;
    }          
} 
    function DateValidation(DateField)
    {
    //debugger ;
    
        var InputValue=trim(DateField.value);
//        var newinput = InputValue.substring(3,6) + InputValue.substring(0,3)+ InputValue.substring(6,10)
//        var d =new Date(newinput);
//        var dt = d.getTime();
//        var millisecond=1;
//	    var second=millisecond*1000;
//	    var minute=second*60;
//	    var hour=minute*60;
//	    var day=hour*24;
//	    var weekday=day*5;
//        var newd = new Date(dt + weekday);
//        var newdate = newd.getDate();
//        var newMonth = newd.getMonth()+1;
//        var newyear = newd.getFullYear();
//        if(newdate<10)
//        {
//            newdate = "0" + newdate;
//        }
//        if(newMonth<10)
//        {
//            newMonth = "0" + newMonth;
//        }
//        var strnewdate = newdate + "/" + newMonth + "/" + newyear;
//        document.getElementById("ctl00_ContentPlaceHolder1_txt_ToDate").value = strnewdate;
        var validformat=/^\d{2}\/\d{2}\/\d{4}$/ //Basic check for format validity
        var returnval=false
        if (!validformat.test(InputValue))
        {
            alert("Invalid Date Format, Enter Correct Date Format (i-e dd/mm/yyyy).")
        }
        else
        { 
            //Detailed check for valid date ranges
            var monthfield=InputValue.split("/")[1]
            var dayfield=InputValue.split("/")[0]
            var yearfield=InputValue.split("/")[2]
            var dayobj = new Date(yearfield, monthfield-1, dayfield)
            if ((dayobj.getMonth()+1!=monthfield)||(dayobj.getDate()!=dayfield)||(dayobj.getFullYear()!=yearfield))
            {
             alert("Invalid date format,enter correct date format (i-e dd/mm/yyyy).")
            }
            else  
            {  
                returnval=true
                //DateField.focus();
            }
        }
        if (returnval==false)    
        {
            //DateField.focus();                                                 
            return false;
        }
    }

    </script>

    <%--<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>--%>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="data_table"
        align="center">
        <tr>
            <td class="formHeader" align="left" style="height: 21px">
                Admin Report
            </td>
        </tr>
        <tr id="row_Dealerwise" runat="server">
            <td align="left">
                <table border="0" cellpadding="0" cellspacing="0" class="data_table">
                    <tr>
                        <td valign="middle" align="left">
                            <asp:Label ID="lbl_Name" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbo_Customer" Visible="false" runat="server" Width="30em" CssClass="combo">
                            </asp:DropDownList>
                            <asp:DropDownList ID="cbo_Dealer" Visible="false" runat="server" Width="15em" CssClass="combo">
                            </asp:DropDownList>
                        </td>
                        <td valign="middle" align="left">
                            From Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_FromDate" Width="115px" runat="server" CssClass="TextBoxCSS"></asp:TextBox><img
                                class="formcontent" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate',this);"
                                id="IMG1" runat="server">
                        </td>
                        <td valign="middle" align="left">
                            &nbsp; To Date:
                        </td>
                        <td align="left">
                        <asp:TextBox ID="txt_ToDate" Width="115px" runat="server" CssClass="TextBoxCSS"></asp:TextBox><img
                                                    class="formcontent" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate',this);"
                                                    id="IMG2" runat="server">
                            
                        </td>
                        <td>
                            <asp:Button ID="btn_Go" runat="server" Text=" Go " CssClass="frmButton" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <%--<tr id="row_Customerwise" runat="server" visible="false">
            <td align="left">
                <table border="0" cellpadding="0" cellspacing="0" class="data_table">
                    <tr>
                        <td valign="middle" align="left">
                            Customer Name:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbo_Customer" runat="server" Width="30em" CssClass="combo">
                            </asp:DropDownList>
                        </td>
                        <td valign="middle" align="left">
                            From Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox1" Enabled="false" runat="server" Style="margin: 2px" onblur="DateValidation(this);"
                                CssClass="text_box jsdate" Width="90px" TabIndex="1"></asp:TextBox>
                        </td>
                        <td valign="middle" align="left">
                            &nbsp; To Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox2" Enabled="false" runat="server" Style="margin: 2px" onblur="DateValidation(this);"
                                CssClass="text_box jsdate1" Width="90px" TabIndex="2"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text=" Go " CssClass="frmButton" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>--%>
        <tr class="SeperatorRowCSS">
            <td>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center">
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="center">
                            <div id="Div1" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px;
                                position: relative; height: 150px; left: 0px; top: 0px;" align="center">
                                <asp:DataGrid ID="dg_DealRpt" AllowPaging="false" runat="server" Width="95%" ShowFooter="True"
                                    AutoGenerateColumns="false" CssClass="table_border_right_bottom">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" CssClass="table_heading" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="top" />
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Position="TopAndBottom" Font-Size="1.3em"
                                        Mode="NumericPages" />
                                    <FooterStyle Font-Bold="true" HorizontalAlign="Left" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="Customer Name" FooterText="Total">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CustomerName" Visible="false" runat="server" Width="320px" align="Left"
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.customername") %>'></asp:Label>
                                                <asp:Label ID="lbl_UserName" Visible="false" runat="server" Width="320px" Text='<%# DataBinder.Eval(Container, "DataItem.NameOfUser") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="CP in Crs.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CPAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.CP") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbl_TotCPAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.TotCP") %>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="CD in Crs.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CDAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.CD") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbl_TotCDAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.TotCD") %>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="SLR in Crs.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_SLRAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.SLR") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbl_TotSLRAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.TotSLR") %>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="NONSLR in Crs.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_NONSLRAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.NON-SLR") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbl_TotNONSLRAmt" runat="server" Width="40px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.TotNONSLR") %>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Total in Crs.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Total" runat="server" Width="80px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Total") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbl_GrandTotal" runat="server" Width="80px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.GrandTotal") %>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <%--<asp:TemplateColumn HeaderText="Match" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Match" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Match") %>'></asp:Label>
                                                        <asp:Label ID="lbl_MMC" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.MMC") %>'></asp:Label>
                                                        <asp:Label ID="lbl_SLRC" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.SLRC") %>'></asp:Label>
                                                        <asp:Label ID="lbl_NONSLRC" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.NONSLRC") %>'></asp:Label>
                                                        <asp:Label ID="lbl_CPPVTC" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.CPPVTC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>--%>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_FullDealRpt" Width ="100px" OnClientClick="return OpenWindow('DealRpt');" Visible="false"
                                runat="server" Text=" Full Screen " CssClass="frmButton" />
                        </td>
                    </tr>
                    <tr class="SeperatorRowCSS">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" width="100%" valign="top">
                            <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td valign="top" width="100%">
                                        <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <div id="Div3" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px;
                                                        position: relative; height: 150px; left: 0px; top: 0px;" align="center">
                                                        <asp:DataGrid ID="dg_MeetingRpt" AllowPaging="false" runat="server" Width="95%" ShowFooter="false"
                                                            AutoGenerateColumns="false" CssClass="table_border_right_bottom">
                                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" CssClass="table_heading" />
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="top" />
                                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Position="TopAndBottom" Font-Size="1.3em"
                                                                Mode="NumericPages" />
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="Meeting Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_MeetingDate" runat="server" Width="30px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.entrydate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Contact Person">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_Contact" runat="server" Width="100px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Contacts") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Customer Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_MeetCustomerName" runat="server" Width="130px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Client") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Meeting Summary">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_MeetingSummary" runat="server" Width="130px" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Remark") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btn_FullMeetingRpt" Width ="100px" OnClientClick="return OpenWindow('MeetingRpt');"
                                                        Visible="false" runat="server" Text=" Full Screen " CssClass="frmButton" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="Hid_Postback" runat="server" />
                            <asp:HiddenField ID="Hid_ManageBy" runat="server" />
                            <asp:HiddenField ID="Hid_FromDate" runat="server" />
                            <asp:HiddenField ID="Hid_ToDate" runat="server" />
                            <asp:HiddenField ID="Hid_UserName" runat="server" />
                            <asp:HiddenField ID="Hid_Selection" runat="server" />
                        </td>
                    </tr>
                </table>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
    </table>
</asp:Content>
