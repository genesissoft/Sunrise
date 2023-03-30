<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="StockUpdate.aspx.vb" Inherits="Forms_StockUpdate" Title="StockUpdate" %>

<%@ Register Src="~/UserControls/YieldCalculater.ascx" TagName="YieldCalculater"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/Calendar.js"></script>

    <script language="javascript" type="text/javascript">
    
                
                
       function Refer()
        {
            var purSellFlag = 'P';
            var Id = document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value;

            if(document.getElementById("ctl00_ContentPlaceHolder1_Rdo_QuoteFor_0").checked == true)
            {
                 purSellFlag = 'S'

            }
            else
            {
                purSellFlag = 'P'
            }
           
            if(Id != "")
            {
                ShowDialogCRPopUp('CrossReferanceNew.aspx',Id,800,400,purSellFlag)
                return false;
            }
            else
            {
                ShowDialogCRPopUp('CrossReferanceNew.aspx',0,800,400,purSellFlag)
                return false;
            }
        }
        function ShowDialogCRPopUp(PageName, Id, Width, Height, strFlag)
	    {
            var w = Width;
            var h = Height;
            var winl = (screen.width-w)/2;
            var wint = (screen.height-h)/2;
            if (winl < 0) winl = 0;
            if (wint < 0) wint = 0;

            PageName = PageName + "?Id=" + Id + "&purSellFlag=" + strFlag
            windowprops = "height="+h+",width="+w+",top="+ wint +",left="+ winl +",location=no,"
            + "scrollbars=yes,menubars=yes,toolbars=yes,resizable=no,status=yes";
            window.open(PageName, "Popup", windowprops);	   
	    }
         function Validation()
        {
             if(document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType").value=="") 
            {
                alert("Please select Security Type");
                return false
            }

            if(document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value=="") 
            {
                alert("Please select the security for yield calculation");
                return false
            }

            if(document.getElementById("ctl00_ContentPlaceHolder1_yld_Calc_txt_Rate").value=="") 
            {
                alert("Please Enter Rate");
                return false
            }
            if(document.getElementById("ctl00_ContentPlaceHolder1_yld_Calc_txt_FaceValue").value=="") 
            {
                alert("Please Enter Face Value");
                return false
            }
            
          
            
           
            return true
        }

        function ShowSecurityMaster()
        {  
            var strpagename = "QuoteEntry.aspx";
            var Id = document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value;
            ShowSecurityForm("SecurityMaster.aspx",Id,"900px","680px")
            return false
        }
        function ShowSecurityForm(PageName,Id, Width, Height)
		{
            var w = Width;
            var h = Height;
            var winl = (screen.width-w)/2;
            var wint = (screen.height-h)/2;
            if (winl < 0) winl = 0;
            if (wint < 0) wint = 0;

            PageName = PageName + "?Id=" + Id + "&Flag=C"
            windowprops = "height="+h+",width="+w+",top="+ wint +",left="+ winl +",location=no,"
            + "scrollbars=yes,menubar=yes,toolbar=yes,resizable=yes,status=yes";
            window.open(PageName, "Popup", windowprops);	   
		}
        function CheckSecurity()
        {

            if(document.getElementById("ctl00_ContentPlaceHolder1_srh_NameofSecurity_Hid_SelectedId").value=="") return false
            if(document.getElementById("ctl00_ContentPlaceHolder1_cbo_SecurityType").value=="") return false
            return true
        } 
        
         function ShowTempCust()
        {
            var strQuote = "Quote"
            var DialogOptions ="Center=Yes; Scrollbar=No; dialogWidth=590px; dialogTop=230px; dialogHeight=600px; Help=No; Status=No; Resizable=Yes;"
            var OpenUrl ="SelectTempCustomer.aspx" 
            OpenUrl=OpenUrl +"?Quote=" + strQuote;
            var ret = window.showModalDialog(OpenUrl,"Yes",DialogOptions)
    
            if (typeof(ret) =="undefined")
            {
                return false
            }
            else
            {
                 document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomeId").value = ret
               
                 return true
            }
        }	
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
               Stock Update</td>
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
                        <table align="center" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr align="center" valign="top">
                                <td style="width: 40%;">
                                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                     
                                        <tr align="left">
                                            <td>
                                                Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Date" runat="server" CssClass="TextBoxCSS" TabIndex="9"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                Valid Till:
                                            </td>
                                            <td style="padding: 0px;">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txt_ValidTill" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                                TabIndex="9" ToolTip="Enter quote validation date and time"></asp:TextBox><img class="calender"
                                                                    src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ValidTill',this);">
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cbo_hr" runat="server" Width="40px" CssClass="ComboBoxCSS">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cbo_minute" runat="server" Width="40px" CssClass="ComboBoxCSS">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cbo_ampm" runat="server" Width="40px" CssClass="ComboBoxCSS">
                                                                <asp:ListItem Value="AM">AM</asp:ListItem>
                                                                <asp:ListItem Value="PM">PM</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                Ip Dates:
                                            </td>
                                            <td>
                                                <asp:Literal ID="lit_IpDates" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                Security Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_SecurityType" runat="server" Width="208px" CssClass="ComboBoxCSS"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                Issuer Name:
                                            </td>
                                            <td style="padding: 0px;">
                                                <uc:Search ID="srh_IssuerOfSecurity" Width="175" runat="server" AutoPostback="true"
                                                    ProcName="ID_SEARCH_SecurityIssuer" SelectedFieldName="SecurityIssuer" SourceType="StoredProcedure"
                                                    TableName="SecurityMaster" ConditionalFieldName="SM.SecurityTypeId" ConditionalFieldId="cbo_SecurityType"></uc:Search>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td nowrap="nowrap">
                                                Name Of Security:
                                            </td>
                                            <td style="padding: 0px;">
                                                <uc:Search ID="srh_NameofSecurity" Width="175" runat="server" AutoPostback="true"
                                                    ProcName="ID_SEARCH_SecurityName" SelectedFieldName="SecurityName" SourceType="StoredProcedure"
                                                    TableName="SecurityMaster" ConditionalFieldName="SecurityIssuer" FormWidth="800"
                                                    ConditionExist="true" FormHeight="350" ConditionalFieldId="srh_IssuerOfSecurity"></uc:Search>
                                            </td>
                                        </tr>
                                    
                                        <tr align="left">
                                            <td>
                                                Dealer:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_DealerName" runat="server" CssClass="TextBoxCSS" Width="200px"
                                                    TabIndex="9" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                Remark:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Remark" runat="server" CssClass="TextBoxCSS" Width="200px" TabIndex="9"
                                                    TextMode="MultiLine" Rows="4"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 60%; padding: 0px;">
                                    <uc:YieldCalculater ID="yld_Calc" runat="server" ShowCloseButton="false"></uc:YieldCalculater>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr align="center">
                                <td class="HeadingCenter" colspan="2">
                                    Detail Section
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                          
                            <tr class="line_separator">
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                                    <asp:Button ID="btn_ShowSecurity" runat="server" CssClass="ButtonCSS" Text="Show Security"
                                        Width="110px" />
                                    <asp:Button ID="btn_GenerateDealSlip" runat="server" CssClass="ButtonCSS" Text="Generate Deal Slip"
                                        Width="130px" visible ="false" />
                                    <asp:Button ID="btn_Refer" runat="server" CssClass="ButtonCSS" Text="Refer" visible ="false" />
                                </td>
                            </tr>
                            <asp:HiddenField ID="Hid_ValidDate" runat="server" />
                            <asp:HiddenField ID="Hid_TotalValue" runat="server" />
                            <asp:HiddenField ID="Hid_FaceValue" runat="server" />
                            <asp:HiddenField ID="Hid_TypeFlag" runat="server" />
                            <asp:HiddenField ID="Hid_PageName" runat="server" />
                            <asp:HiddenField ID="Hid_NatureOfInstrument" runat="server" />
                            <asp:HiddenField ID="Hid_QuoteId" runat="server" />
                            <asp:HiddenField ID="Hid_CustomeId" runat="server" />
                             <asp:HiddenField ID="Hid_Rate" runat="server" />
                             <asp:HiddenField ID="Hid_UserId" runat="server" />
                        </table>
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
