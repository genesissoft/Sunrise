<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RatingDetails.aspx.vb" Inherits="Forms_RatingDetails" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self">
    </base>
    <title>Rating Details</title>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script language="javascript">
     function Close()
        {
            window.returnValue = "";
            window.close();
        }
        function ReturnValues(strReturn)
        {
            window.returnValue = strReturn;
            window.close();
        }
        function FinalClose()
        {
            window.returnValue = "";
            window.close();
        }        
        
//        function Validation()
//        {
//            if((document.getElementById("Cbo_INVESTMENTTYPE").value) == "")
//            {
//                alert('Please Select Investment Type')
//                 return false;
//            } 

//        }
        
        function ConvertUCase(txtBox)
        {     
            txtBox.value = txtBox.value.toUpperCase()    
        }  
        
         function RetValues()
        {
            var strReturn = "";
            var selBValues = "";
            var selBusniessTypeValues = "";
             var selDValues = "";
            var selDealerValues = "";
             var cboInstrumentType = document.getElementById("cbo_InstrumentType") 
                     
           
                strReturn = strReturn + cboInstrumentType.options[cboInstrumentType.options.selectedIndex].text + "!" 
                strReturn = strReturn + document.getElementById("txt_Rating1").value + "!"   
                strReturn = strReturn + document.getElementById("txt_Rating2").value + "!"   
                strReturn = strReturn + document.getElementById("txt_Rating3").value + "!"   
                strReturn = strReturn + document.getElementById("txt_Rating4").value + "!"   
                 strReturn = strReturn + document.getElementById("txt_Agency1").value + "!"   
                strReturn = strReturn + document.getElementById("txt_Agency2").value + "!"   
                strReturn = strReturn + document.getElementById("txt_Agency3").value + "!"   
                strReturn = strReturn + document.getElementById("txt_Agency4").value + "!"   
                strReturn = strReturn + document.getElementById("Hid_CustomerId").value + "!"
                              
                window.returnValue = strReturn;
              
                window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td class="HeaderCSS" align="center" colspan="4">
                    Rating Details
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" width="80%" valign="top">
                    <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="80%">
                        <tr>
                            <td class="LabelCSS">
                                Instrument Type:
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cbo_InstrumentType" runat="server" CssClass="ComboBoxCSS" Width="160px"
                                    TabIndex="7">
                                    <asp:ListItem Text="Perpetual" Value="Perpetual"></asp:ListItem>
                                    <asp:ListItem Text="Upper Tier II" Value="Upper Tier II"></asp:ListItem>
                                    <asp:ListItem Text="Lower Tier II" Value="Lower Tier II"></asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <%-- <tr>
                            <td class="LabelCSS">
                                As on 31st March of previous year ending:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_31stMarch" runat="server" CssClass="TextBoxCSS" Width="158px"  ></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="LabelCSS">
                                Rating 1:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_Rating1" runat="server" CssClass="TextBoxCSS" Width="158px"></asp:TextBox>
                            </td>
                            <td class="LabelCSS">
                                Agency 1:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_Agency1" runat="server" CssClass="TextBoxCSS" Width="158px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">
                                Rating 2:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_Rating2" runat="server" CssClass="TextBoxCSS" Width="158px"></asp:TextBox>
                            </td>
                            <td class="LabelCSS">
                                Agency 2:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_Agency2" runat="server" CssClass="TextBoxCSS" Width="158px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">
                                Rating 3:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_Rating3" runat="server" CssClass="TextBoxCSS" Width="158px"></asp:TextBox>
                            </td>
                            <td class="LabelCSS">
                                Agency 3:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_Agency3" runat="server" CssClass="TextBoxCSS" Width="158px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">
                                Rating 4:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_Rating4" runat="server" CssClass="TextBoxCSS" Width="158px"></asp:TextBox>
                            </td>
                            <td class="LabelCSS">
                                Agency 4:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_Agency4" runat="server" CssClass="TextBoxCSS" Width="158px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="SeperatorRowCSS" colspan="4">
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" ToolTip="Save" />
                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Save" ToolTip="Edit" />
                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" OnClientClick="return Close();"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
            <asp:HiddenField ID="Hid_Ids" runat="server" />
            <asp:HiddenField ID="Hid_CustomerId" runat="server" />
        </table>
    </form>
</body>
</html>
