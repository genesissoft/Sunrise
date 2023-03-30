<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DealerAllocationDetail.aspx.vb"
    Inherits="Forms_DealerAllocationDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script type="text/javascript" src="../Include/Common.js"></script>

<script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

<script language="javascript" src="../Include/DatePicker.js" type="text/javascript"></script>

<link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self"></base>
    <title>Dealer Allocation Detail</title>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">
     
     $(document).ready(function(){
     
          if( $("#cbo_DealerType option:selected").text() == "Management" || $("#cbo_DealerType option:selected").text() == "Mid-Management")
          {
               $("[id*=chk_BusinessType] input:checked").removeAttr("checked"); 
               $("#row_LstBustype").hide();
          }
         
          $( "#cbo_DealerType").change(function()
          {
                    
              if( $("#cbo_DealerType option:selected").text() == "Management" || $("#cbo_DealerType option:selected").text() == "Mid-Management")
              {
                   $("[id*=chk_BusinessType] input:checked").removeAttr("checked"); 
                   $("#row_LstBustype").hide();
                   
              }
              else
              {
                  $("#row_LstBustype").show();
              }
             
          });
     });
        
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
        
     function Validation()
     {
            
            //debugger;
            if($('#cbo_Dealer :selected').val() == "")
            {
                alert('Please Enter Dealer !');
                return false;
            } 
            var strInteractionTypeId = document.getElementById("cbo_ContactInteraction").value;
            if(strInteractionTypeId =="")
            {
                alert('Please select Interaction type');
                return false;
            }
            
            var strDealerType = document.getElementById("cbo_DealerType").value;
            //alert(strDealerType)
            if(strDealerType =="")
            {
                alert('Please select Dealer type');
                return false;
            }
            
            if($('#txt_FromDate')[0].value == "")
            {
                alert('Please Enter From Date !');
                return false;
            } 
            
            var fromdate=$('#txt_FromDate')[0].value;
            var todate =$('#txt_ToDate')[0].value;
            
            if ((Date.parse(getmdy(fromdate))) > (Date.parse(getmdy(todate)))) {

                alert('From Date can not be less then To Date');
                return false;
            }            
       }
        
       function RetValues(stkDetailId,excessQty)
       {        
            
            //debugger;            
            var strReturn = "";
                   
            var selDBTValues = "";
            var selDBusniessTypeValues = "";
            
            var selCPValues = "";
            var selCPSearchValues = "";
            
            var selectedBDType  = "" ;
            
            var selectedValues = "";
              //debugger;                   
            strReturn = strReturn + $('#cbo_Dealer :selected').text() + "!" ;
            strReturn = strReturn + $('#cbo_Dealer :selected').val() + "!"  ;
            
            strReturn = strReturn + $('#cbo_DealerType :selected').text() + "!" ;
            strReturn = strReturn + $('#cbo_DealerType :selected').val() + "!"  ; 
           
            $("[id*=chk_BusinessType] input:checked").each(function () 
            {
                selectedValues += $(this).next().html() + "," ;
                
            });   
            
            strReturn = strReturn + selectedValues + "!"  
            
            strReturn = strReturn + $('#cbo_ContactInteraction :selected').text() + "!" ;
            strReturn = strReturn + $('#cbo_ContactInteraction :selected').val() + "!"  ;            
            
            strReturn = strReturn + $('#txt_FromDate')[0].value + "!" ;
            strReturn = strReturn + $('#txt_ToDate')[0].value + "!"  ;            
            //alert(strReturn);
            
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
                    Dealer Allocation Details
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="60%" bordercolor="">
                        <tr>
                            <td class="LabelCSS">
                                Dealer:
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cbo_Dealer" TabIndex="8" runat="server" AutoPostBack="false"
                                    CssClass="ComboBoxCSS" Width="185px">
                                </asp:DropDownList><i style="color: red">*</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">
                                Dealer Type:
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cbo_DealerType" TabIndex="8" runat="server" AutoPostBack="false"
                                    CssClass="ComboBoxCSS" Width="185px">
                                    <asp:ListItem Value="">--select Dealer Type--</asp:ListItem>
                                    <asp:ListItem Value="Primary">Primary</asp:ListItem>
                                    <asp:ListItem Value="Secondary">Secondary</asp:ListItem>
                                    <asp:ListItem Value="Management">Management</asp:ListItem>
                                    <asp:ListItem Value="Mid-Management">Mid-Management</asp:ListItem>
                                </asp:DropDownList><i style="color: red">*</i>
                            </td>
                        </tr>
                        <tr id="row_LstBustype" runat="server">
                            <td class="LabelCSS">
                                Dealer Business Types :
                            </td>
                            <td align="left">
                                <asp:CheckBoxList ID="chk_BusinessType" runat="server" RepeatColumns="3" TabIndex = "9">
                                    <asp:ListItem Value="CP">CP</asp:ListItem>
                                     <asp:ListItem Value="NCD">NCD</asp:ListItem>
                                     <asp:ListItem Value="PD">Public Deposit</asp:ListItem>
                                    <asp:ListItem Value="CGB">Capital Gain Bond</asp:ListItem>
                                    <asp:ListItem Value="FD">FD</asp:ListItem>
                                    
                               
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                            <td>
                            </td>
                        </tr>
                        <tr style="margin-top: 10px;">
                            <td class="LabelCSS">
                                Interaction:
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cbo_ContactInteraction" Width="185px" runat="server" CssClass="ComboBoxCSS"
                                    AutoPostBack="false" TabIndex="10">
                                </asp:DropDownList><i style="color: red">*</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">
                                From Date:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_FromDate" Width="158" runat="server" CssClass="TextBoxCSS" TabIndex ="11"></asp:TextBox>
                                <img class="formcontent" height="14" src="../Images/Calender.jpg" onclick="displayDatePicker('txt_FromDate',this);"
                                    width="15" border="0" style="vertical-align: top; cursor: hand;" id="IMG1" runat="server"><i style="color: red">*</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS">
                                To Date:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_ToDate" Width="158" runat="server" CssClass="TextBoxCSS" TabIndex="12"></asp:TextBox>
                                <img class="formcontent" height="14" src="../Images/Calender.jpg" onclick="displayDatePicker('txt_ToDate',this);"
                                    width="15" border="0" style="vertical-align: top; cursor: hand;" id="IMG2" runat="server">
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
                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" ToolTip="Save" Width="10%"/>
                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" ToolTip="Edit" Width="10%" />
                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" OnClientClick="return Close();"
                        UseSubmitBehavior="false" Width="10%"/>
                </td>
            </tr>
            <asp:HiddenField ID="Hid_Ids" runat="server" />
            <asp:HiddenField ID="Hid_CustomerId" runat="server" />
            <asp:HiddenField ID="Hid_ProfileType" runat="server" />
            <asp:HiddenField ID="Hid_BusinessType" runat="server" />
            <asp:HiddenField ID="Hid_DealerAllocationId" runat="server" />
            
        </table>
    </form>
</body>
</html>
