<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AccuredInterest.aspx.vb"
    Inherits="Forms_AccuredInterest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Accured Interest</title>
    <link href="../Include/General.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Parkstone.css" rel="stylesheet" type="text/css" />
    <link href="../Include/General.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Intervention.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Style_New.css" type="text/css" rel="stylesheet" />

    <script language="javascript" src="../Include/Common.js" type="text/javascript"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script language="javascript">
        function Validate()
        {
            if((document.getElementById("txt_Rate").value-0)=="")
            {
                alert("Please enter the rate");
                return false
            }     
            if((document.getElementById("txt_FaceValue").value-0)=="")
            {
                alert("Please enter the Face Value");
                return false
            }   
            if(document.getElementById("Hid_Frequency").value!="0" && document.getElementById("Hid_CoupDate").value=="") 
            {
                alert("Coupon Rate not Available, Can not Calculate Interest");
                return false
            }  
            return true
        }
        
         function Close()
        {
            window.returnValue = document.getElementById("txt_AddInterest").value;
            window.close(); 
            return false;
        }
        
//       function Close()
//		{   
//		    var rate, rateActualFlag , YXMFlag , phyDMATFlag, IPClacFlag, accIntDays, daysFlag
//		    rate = document.getElementById("txt_Rate").value
//		    for(i=0; i<2; i++)
//		    {
//		        var rdoRateActual = document.getElementById("rdo_RateActual_"+i)
//		        var rdoYXM = document.getElementById("rdo_YXM_"+i)
//		        var rdophyDMAT = document.getElementById("rdo_PhysicalDMAT_"+i)
//		        var rdoIPCalc  = document.getElementById("rdo_IPCalc_"+i)
//		        var rdoDays = document.getElementById("rbl_Days_"+i)
//		        var rdoDaysOpt = document.getElementById("rbl_DaysOptions_"+i)
//		        
//		        if(rdoRateActual.checked==true) rateActualFlag = rdoRateActual.value.toUpperCase()
//		        if(rdoYXM.checked==true)        YXMFlag = rdoYXM.value.toUpperCase()
//		        if(rdophyDMAT.checked==true)    phyDMATFlag = rdophyDMAT.value.toUpperCase()
//		        if(rdoIPCalc.checked==true)     IPClacFlag = rdoIPCalc.value.toUpperCase()
//		        if(rdoDays.checked==true)       accIntDays = rdoDays.value.toUpperCase()
//		        if(rdoDaysOpt.checked==true)    daysFlag = rdoDaysOpt.value.toUpperCase()
//		    }
//		    if(typeof(YXMFlag)=="undefined") YXMFlag = "X"
//		    
//		    var retValue = rate + "!" + rateActualFlag + "!" + YXMFlag + "!" + phyDMATFlag + "!" + IPClacFlag + "!" + accIntDays + "!" + daysFlag
//		    window.returnValue = retValue; 
//            window.close()
//		}
    </script>

</head>
<body style="margin-left: 0px; margin-top: 0px;">
    <form id="form1" runat="server">
        <div>
            <table cellpadding="0" cellspacing="0" align="center" width="100%" class="data_table">
                <tr align="center" valign="top">
                    <td>
                        <table align="center" cellpadding="0" cellspacing="0" width="95%" border="0">
                            <tr align="center">
                                <td colspan="2" class="SectionHeaderCSS" style="text-align: center;">
                                    Accured Interest Calculation
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Date" runat="server" Width="100px" CssClass="TextBoxCSS" MaxLength="20"></asp:TextBox>
                                    <asp:RadioButtonList ID="rdo_Days" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                        CssClass="LabelCSS">
                                        <asp:ListItem Value="360">360</asp:ListItem>
                                        <asp:ListItem Value="365" Selected="True">365</asp:ListItem>
                                        <asp:ListItem Value="366">366</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Rate:</td>
                                <td>
                                    <asp:TextBox ID="txt_Rate" runat="server" Width="100px" CssClass="TextBoxCSS" MaxLength="20"></asp:TextBox>
                                    <asp:RadioButtonList ID="rdo_RateActual" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass=" LabelCSS">
                                        <asp:ListItem Value="R" Selected="True">Rate</asp:ListItem>
                                        <asp:ListItem Value="A">Actual</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Face Value:</td>
                                <td style="padding: 0px;">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr align="left">
                                            <td>
                                                <asp:TextBox ID="txt_FaceValue" runat="server" Width="100px" CssClass="TextBoxCSS"
                                                    MaxLength="20" Height="17px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cbo_FaceValue" runat="server" Width="100px" CssClass="ComboBoxCSS"
                                                    AutoPostBack="false">
                                                    <asp:ListItem Value="1000">Thousands</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="100000">Lacs</asp:ListItem>
                                                    <asp:ListItem Value="10000000">Crores</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdo_PhysicalDMAT" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" CssClass=" LabelCSS" Visible="false">
                                                    <asp:ListItem Value="D" Selected="True">DMAT\SGL</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Interest Dates:</td>
                                <td>
                                    <asp:TextBox ID="txt_IPDates" runat="server" Width="100px" CssClass="TextBoxCSS"
                                        MaxLength="20"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Amount:</td>
                                <td>
                                    <asp:TextBox ID="txt_Amount" runat="server" Width="100px" CssClass="TextBoxCSS" MaxLength="20"></asp:TextBox></td>
                            </tr>
                            <tr align="left">
                                <td>
                                    <font id="row_Interest" runat="server" class="LabelCSS">Interest</font>:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_AddInterest" runat="server" Width="100px" CssClass="TextBoxCSS"
                                        MaxLength="20"></asp:TextBox>
                                    <asp:Label ID="lbl_AddInterest" runat="server" Height="16px" CssClass=" LabelCSS"
                                        BorderStyle="Solid" BorderWidth="1px" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Sett. Amount:</td>
                                <td>
                                    <asp:TextBox ID="txt_SettAmt" runat="server" Width="100px" CssClass="TextBoxCSS"
                                        MaxLength="20"></asp:TextBox>
                                    <asp:Label ID="lbl_SettAmt" runat="server" CssClass="LabelCSS" BorderStyle="Solid"
                                        BorderWidth="1px" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btn_CalInterest" runat="server" Text="Calculate Accured Interest"
                                        ToolTip="Show Accured Interest" CssClass="ButtonCSS" Width="160px" />
                                    <asp:Button ID="btn_Close" runat="server" Text="Close" ToolTip="Close" CssClass="ButtonCSS"
                                        OnClientClick="return Close();" UseSubmitBehavior="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="Hid_MatDate" runat="server" />
            <asp:HiddenField ID="Hid_MatAmt" runat="server" />
            <asp:HiddenField ID="Hid_CallDate" runat="server" />
            <asp:HiddenField ID="Hid_CallAmt" runat="server" />
            <asp:HiddenField ID="Hid_PutDate" runat="server" />
            <asp:HiddenField ID="Hid_PutAmt" runat="server" />
            <asp:HiddenField ID="Hid_CoupDate" runat="server" />
            <asp:HiddenField ID="Hid_CoupRate" runat="server" />
            <asp:HiddenField ID="Hid_SecurityId" runat="server" />
            <asp:HiddenField ID="Hid_Frequency" runat="server" />
            <asp:HiddenField ID="Hid_GovernmentFlag" runat="server" />
            <asp:HiddenField ID="Hid_Issue" runat="server" />
            <asp:HiddenField ID="Hid_InterestDate" runat="server" />
            <asp:HiddenField ID="Hid_BookClosureDate" runat="server" />
            <asp:HiddenField ID="Hid_DMATBkDate" runat="server" />
            <asp:HiddenField ID="Hid_FaceValue" runat="server" />
            <asp:HiddenField ID="Hid_NSDLFaceValue" runat="server" />
            <asp:HiddenField ID="Hid_IntDays" runat="server" />
        </div>
    </form>
</body>
</html>
