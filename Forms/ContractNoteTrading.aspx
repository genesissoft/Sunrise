<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ContractNoteTrading.aspx.vb"
    Inherits="Forms_ContractNoteTrading" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self">
    </base>
    <title>Contract Note</title>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">
    
    
    
    
    
    function OpenReport(DealFlag)
        {   
                     
            var strDealEntryId = document.getElementById("Hid_DealEntryId").value
            var strWDMDealNumber = document.getElementById("Hid_WDMDealNumber").value
            var hidExchange = document.getElementById("Hid_Exchange").value
            //alert(hidExchange)
           
            pageUrl = "ViewDealReports.aspx?DealEntryId=" + strDealEntryId + "&ExchangeName=" + hidExchange    + "&WDMDealNumber=" + strWDMDealNumber + "&DealFlag=" + DealFlag;
            //alert(pageUrl)
            var ret = window.open(pageUrl,target="_blank","left=80,top=80,height=400,width=1000,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")            
            ret.focus();
            SetPrintProperties(); 
            //window.close(); 
            return false    
        }
        
        function CalcDutybyJS()
        {   
            var SubTotal    
            var StampDuty = (document.getElementById("txt_StampDuty").value-0)
            var ServiceTax = (document.getElementById("txt_ServiceTax").value-0)
            var Educationcess = (document.getElementById("txt_Educationcess").value-0)
            var HEducationcess = (document.getElementById("txt_HEducationcess").value-0)
            var SettlementAmount = (document.getElementById("Hid_SettlementAmount").value-0)
            
            document.getElementById("Hid_StampDutyAmt").value = RoundNo((SettlementAmount*StampDuty)/100, 2)
            document.getElementById("lbl_StampDuty").value = document.getElementById("Hid_StampDutyAmt").value
            if(document.getElementById("rdo_ContNotefor_0").checked == true)
            {
                var BuyBrokerage = (document.getElementById("Hid_BuyBroker").value-0)
                SubTotal = (BuyBrokerage - StampDuty)
            }
            else
            {
                var SellBrokerage = (document.getElementById("Hid_SellBroker").value-0)
                SubTotal = (SellBrokerage - StampDuty)
            }   
            document.getElementById("Hid_ServiceTaxAMT").value =  RoundNo((SubTotal*ServiceTax)/100 ,2)
            document.getElementById("Hid_EducationcessAMT").value =   RoundNo(((document.getElementById("Hid_ServiceTaxAMT").value-0)*Educationcess)/100,2)
            document.getElementById("Hid_HEducationcessAMT").value =  RoundNo(((document.getElementById("Hid_ServiceTaxAMT").value-0)*HEducationcess)/100 ,2)
            
            document.getElementById("lbl_ServiceTax").value = document.getElementById("Hid_ServiceTaxAMT").value
            document.getElementById("lbl_Educationcess").value = document.getElementById("Hid_EducationcessAMT").value
            document.getElementById("lbl_HEducationcess").value = document.getElementById("Hid_HEducationcessAMT").value
        }
        
        function RoundNo(num, dec) 
        {
	        return num.toFixed(dec)
        }
        
        
     function Validation()
        {

             if((document.getElementById("txt_OrderNo").value) == "")
            {
                alert('Please enter Order No');
                return false;            
            }           
            if((document.getElementById("txt_TradeNo").value) == "")
            {
                alert('Please enter Trade No');
                return false;            
            }
//               if((document.getElementById("txt_Couponrate").value) == "")
//            {
//                alert('Please enter Coupon Rate');
//                return false;            
//            }
            if(document.getElementById("rdo_TradeType_0").checked == true)
            {
            var txtRepoPeriod = document.getElementById("txt_RepoPeriod")
            var txtRepoRate = document.getElementById("txt_RepoRate")
            if (txtRepoPeriod != null)
            {
                if((document.getElementById("txt_RepoPeriod").value) == "")
                {
                    alert('Please enter Repo Period');
                    return false;            
                }    
                if((document.getElementById("txt_RepoRate").value) == "")
                {
                    alert('Please enter Repo Rate');
                    return false;            
                }
              }          
            }
        
//            FinalClose()
                 
            
        }
        
         function FinalClose()
        {
            window.returnValue = "";
            window.close();
        }    
//        function CheckRateAmount()   
//        {
//            if (document.getElementById("rdo_RateAmount_0").checked == true)
//            {  
//                document.getElementById("fnt_Perc").innerHTML = "%";
//            }
//            else
//            {               
//                document.getElementById("fnt_Perc").innerHTML = "";
//            }
//        }
        
         function CheckRepo()   
        {
            if (document.getElementById("rdo_TradeType_0").checked == true)
            {  
                
                document.getElementById("row_RepoRate").style.display = "block"
                document.getElementById("row_RepoPeriod").style.display = "block"
            }
            else
            {               
                document.getElementById("row_RepoRate").style.display = "none"
                document.getElementById("row_RepoPeriod").style.display = "none"
                document.getElementById("txt_RepoPeriod").value = ""
                document.getElementById("txt_RepoRate").value = ""
            }
        }
        
        
       function ShowBacksideReportr()
        {  
            var strpagename = "BackSide.aspx";
            window.open(strpagename);
            SetPrintProperties() 
        }
        
        
        
 
    </script>

    <script type="text/javascript">
        function SetPrintProperties() 
        {
            try 
            {
                //alert()
                shell = new ActiveXObject("WScript.Shell");
                shell.SendKeys("%fu");
                //window.setTimeout("javascript:DoNothing();", 1200);
                window.setTimeout("javascript:SetPaperSize();", 1200);
                //window.setTimeout("javascript:setLandScape();", 2000);
            }
            catch(e)
            {
                alert('Please Change your Internet Security Settings\nGo to Internet Explorer --> Tools --> Internet Options --> Security --> Internet --> Custom Level \nGo to Initialize and script ActiveX controls not marked as safe --> Select Prompt --> Click Ok')
            }
            return false
        }

        function SetPaperSize() 
        {
            // A4 ==> Landscape ==> L-R-T-P Margins ==> Enter
            shell.SendKeys("AA%a%l0.48%r0.75%t0.89%b0.166{ENTER}");
        }
        function CheckSetting()
        {
            try 
            {
                shell = new ActiveXObject("WScript.Shell");
                return true
            }
            catch(e)
            {
                alert('Please Change your Internet Security Settings\nGo to Internet Explorer --> Tools --> Internet Options --> Security --> Internet --> Custom Level \nGo to Initialize and script ActiveX controls not marked as safe --> Select Enable --> Click Ok')
                return false
            }            
        }        
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="1">
                <tr>
                    <td class="HeaderCSS" align="center" colspan="4">
                        Contract Note
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top" colspan="2">
                        <table id="Table2" align="center" cellspacing="0" cellpadding="0" border="0" width="80%">
                            <tr align="center">
                                <td valign="middle" align="center" width="20%">
                                    <table id="Table6" align="center" cellspacing="0" cellpadding="0" border="0" width="100%"
                                        bordercolor="Green">
                                        <tr>
                                            <td class="LabelCSS" width="200px">
                                                Exchange :
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lbl_Exchange" runat="server" Width="170px" CssClass="LabelCSS" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        
                                         <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                Security Type Code:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_SecurityTypeCode"  ReadOnly="true"  runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox>
                                                <em><span style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        
                                        
                                         <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                Security Code :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_SecurityCode" ReadOnly="true" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox>
                                                <em><span style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        
                                        
                                   
                                        <tr id="row_buysell" runat="server" visible="false">
                                            <td class="LabelCSS" align="center" width="130px">
                                                Contract Note For:
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_ContNotefor" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" TabIndex="1">
                                                    <asp:ListItem Selected="True" Value="B">Buyer</asp:ListItem>
                                                    <asp:ListItem Value="S">Seller</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                Order No:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_OrderNo" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox>
                                                <em><span style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                Trade No:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_TradeNo" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox>
                                                <em><span style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                Order Attributes:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_OrderAttributes" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="row_issue" runat="server">
                                            <td class="LabelCSS" align="center" width="130px">
                                                Issue:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_Couponrate" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox><font class="LabelCSS" id="Font1">%</font> <em><span style="color: #ff0000">
                                                        *</span></em>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                Trade Type:
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_TradeType" runat="server" CellPadding="0"
                                                    CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" TabIndex="1">
                                                    <asp:ListItem Value="R">Repo</asp:ListItem>
                                                    <asp:ListItem Value="N" Selected="True">Non Repo</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr id="row_RepoPeriod">
                                            <td class="LabelCSS" align="center" width="130px">
                                                Repo Period:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_RepoPeriod" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox>
                                                <em><span style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        <tr id="row_RepoRate">
                                            <td class="LabelCSS" align="center" width="130px">
                                                Repo Rate:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_RepoRate" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox><font class="LabelCSS" id="fnt_Reporate">%</font>
                                                <em><span style="color: #ff0000">*</span></em>
                                            </td>
                                        </tr>
                                        <tr id="tradetime">
                                            <td class="LabelCSS" align="center" width="130px">
                                                Trade Date:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_TradeDate" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td class="LabelCSS">
                                                Trade Time:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cbo_hr" runat="server" Width="52px" CssClass="ComboBoxCSS"
                                                    TabIndex="11">
                                                </asp:DropDownList><asp:DropDownList ID="cbo_minute" runat="server" Width="52px"
                                                    CssClass="ComboBoxCSS" TabIndex="12">
                                                </asp:DropDownList><%--<asp:DropDownList ID="cbo_ampm" runat="server" Width="48px" CssClass="ComboBoxCSS"
                                                    TabIndex="13">
                                                    <asp:ListItem Value="AM">AM</asp:ListItem>
                                                    <asp:ListItem Value="PM" Selected="True">PM</asp:ListItem>
                                                </asp:DropDownList> 
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td class="LabelCSS" align="center" width="150px">
                                                Constituent Ref No:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_ConstRefNo" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                Participant Code:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_ParticipantCode" runat="server" CssClass="TextBoxCSS" Width="100px"
                                                    TabIndex="1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                Stamp Duty:
                                            </td>
                                            <td align="left" valign="top" width="200px">
                                                <asp:TextBox ID="txt_StampDuty" runat="server" Style="text-align: right" onchange="CalcDutybyJS();"
                                                    CssClass="TextBoxCSS" Width="60px" TabIndex="1" Text="0.0005"></asp:TextBox>%
                                                <asp:TextBox ID="lbl_StampDuty" Style="text-align: right" CssClass="TextBoxCSS" runat="server"
                                                    Width="75px" Enabled="false" /></td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                Service Tax:
                                            </td>
                                            <td align="left" valign="top" width="200px">
                                                <asp:TextBox ID="txt_ServiceTax" runat="server" Style="text-align: right" onchange="CalcDutybyJS();"
                                                    CssClass="TextBoxCSS" Width="60px" TabIndex="1" Text="10"></asp:TextBox>%
                                                <asp:TextBox ID="lbl_ServiceTax" Style="text-align: right" CssClass="TextBoxCSS"
                                                    runat="server" Width="75px" Enabled="false" /></td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                Educationcess:
                                            </td>
                                            <td align="left" valign="top" width="200px">
                                                <asp:TextBox ID="txt_Educationcess" runat="server" Style="text-align: right" onchange="CalcDutybyJS();"
                                                    CssClass="TextBoxCSS" Width="60px" TabIndex="1" Text="2"></asp:TextBox>%
                                                <asp:TextBox ID="lbl_Educationcess" Style="text-align: right" CssClass="TextBoxCSS"
                                                    runat="server" Width="75px" Enabled="false" /></td>
                                        </tr>
                                        <tr>
                                            <td class="LabelCSS" align="center" width="130px">
                                                H.Educationcess:
                                            </td>
                                            <td align="left" valign="top" width="200px">
                                                <asp:TextBox ID="txt_HEducationcess" runat="server" Style="text-align: right" onchange="CalcDutybyJS();"
                                                    CssClass="TextBoxCSS" Width="60px" TabIndex="1" Text="1"></asp:TextBox>%
                                                <asp:TextBox ID="lbl_HEducationcess" Style="text-align: right" CssClass="TextBoxCSS"
                                                    runat="server" Width="75px" Enabled="false" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:Button ID="btn_SaveandPrint" runat="server" CssClass="ButtonCSS" Text="Save and Print"
                            ToolTip="Print" TabIndex="11" />
                        <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Close" UseSubmitBehavior="false"
                            OnClientClick="return FinalClose();" TabIndex="12" />
                        <asp:Button ID="btn_ShowBackside" runat="server" CssClass="ButtonCSS" Text="ShowBackside"
                            UseSubmitBehavior="false" TabIndex="12" />
                    </td>
                </tr>
                <asp:HiddenField ID="Hid_CustBankDetailId" runat="server" />
                <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                <asp:HiddenField ID="Hid_Index" runat="server" />
                <asp:HiddenField ID="Hid_DealEntryId" runat="server" />
                <asp:HiddenField ID="Hid_TradeTime" runat="server" />
                <asp:HiddenField ID="Hid_WDMDealNumber" runat="server" />
                <asp:HiddenField ID="Hid_Exchange" runat="server" />
                <asp:HiddenField ID="Hid_BuyBroker" runat="server" />
                <asp:HiddenField ID="Hid_SellBroker" runat="server" />
                <asp:HiddenField ID="Hid_SettlementAmount" runat="server" />
                <asp:HiddenField ID="Hid_StampDutyAMT" runat="server" />
                <asp:HiddenField ID="Hid_ServiceTaxAMT" runat="server" />
                <asp:HiddenField ID="Hid_EducationcessAMT" runat="server" />
                <asp:HiddenField ID="Hid_HEducationcessAMT" runat="server" />
            </table>
        </div>
    </form>
</body>
</html>
