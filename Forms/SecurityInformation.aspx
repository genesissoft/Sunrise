<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SecurityInformation.aspx.vb"
    Inherits="Forms_SecurityInformation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Security Information</title>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript">
     
        function OpenViewWindow()
        {   
            var pageUrl = "ShowSelectedSecurity.aspx";
            //var ret = window.open(pageUrl,target="_top", "width=680,height=330");
            window.open(pageUrl,target="_blank","width=800,height=330")
            return false;
        }
        
        function SelectOption(img,id,name)
        {
            document.getElementById("Hid_SecurityId").value = id
            document.getElementById("Hid_SecurityName").value = name
            document.getElementById("txt_SecurityName").value = name
            
            if(img != null)
            {
                var row = img.parentElement.parentElement
                UnselectAll(row)
                img.src = "../Images/images.JPG"
                row.style.backgroundColor = '#D1E4F8' 
                //row.children[1].children[0].style.backgroundColor = "#D1E4F8"
                row.children[2].children[0].style.backgroundColor = "#D1E4F8"
                row.children[3].children[0].style.backgroundColor = "#D1E4F8"
            }
            ShowStockInfo(id)
            
            return false
        }
        
        function UnselectAll(row)
        {
            var grd = row.parentElement.parentElement
            for(i=1; i<=(grd.children[0].children.length-2); i++)
            {
                currRow = grd.children[0].children[i]
                currRow.children[0].children[0].src = "../Images/images3.JPG"/*
                currRow.style.backgroundColor = 'white'
                 currRow.children[1].children[0].style.backgroundColor = "white"
                 currRow.children[2].children[0].style.backgroundColor = "white"
                 currRow.children[3].children[0].style.backgroundColor = "white"*/
            }
        }
        
        function ShowStockInfo(Id)
        {           
            //var Id = document.getElementById("Hid_SecurityId").value;
            // alert(Id)
            ShowDialogCRPopUp('StockInfo.aspx',Id,400,200)
            return false;
                   
        }
          
         function ShowDialogCRPopUp(PageName, Id, Width, Height)
	    {
                var w = Width;
                var h = Height;
                var winl = (screen.width-w)/2;
                var wint = (screen.height-h)/2;
                if (winl < 0) winl = 0;
                if (wint < 0) wint = 0;

                PageName = PageName + "?Id=" + Id  
                windowprops = "height="+h+",width="+w+",top="+ wint +",left="+ winl +",location=no,"
                + "scrollbars=yes,menubars=yes,toolbars=yes,resizable=no,status=yes";
                window.open(PageName, "Popup", windowprops);	   
	    }
	    
        function ShowYieldCalculation()
        {          
            var rate = document.getElementById("Hid_SecurityName").value          
            var id = document.getElementById("Hid_SecurityId").value    
           
            var rate =  Number((document.getElementById("txt_Rate").value-0)) 
            rate = ShowDialog("YieldCalculation.aspx",id,rate,"740px","450px")
          
            if (typeof(rate) != "undefined")
            {
                var yield = rate.split('!');
                
                document.getElementById("txt_Rate").value = yield[0];
                document.getElementById("Hid_Semi_Ann_Flag").value = yield[2];
                document.getElementById("Hid_CombineIPMat").value = yield[3];
                document.getElementById("Hid_Rate_Actual_Flag").value = yield[4];
                document.getElementById("Hid_Equal_Actual_Flag").value = yield[5];
                document.getElementById("Hid_IntDays").value = yield[6];
                document.getElementById("Hid_FirstYrAllYr").value = yield[7];
            }
            return false;
        }
        
        function ShowDialog(PageName,customerid,Rate,strWidth, strHeight)
		{
//		    var YTMDate = document.getElementById(parentId + "_txt_YtmDate").value
		    var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
		    var OpenUrl = PageName + "?Id=" + customerid + "&Rate=" + Rate ;
			var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
			return ret
		}   
        
        function Submit()
        {
            var agentName = ""
            var agentCode = ""
                     
            var agentCodes = document.getElementById("Hid_AgentId").value.split("!")
            var grd = document.getElementById("dg1") 
            for(i=1; i<=(grd.children[0].children.length-2); i++)
            {
                currRow = grd.children[0].children[i]
                if(currRow.style.backgroundColor.toUpperCase() == '#D1E4F8')
                {
                    agentName = currRow.children[1].children[0].innerHTML
                    agentCode = agentCodes[i-1] 
                    break
                }
            }
            if(agentName == "")
            {
                alert("Please select atleast one option")
                return false
            }
            document.getElementById("Hid_SelectedField").value = agentName 
            document.getElementById("Hid_SelectedValue").value = agentCode 
         
            window.returnValue = agentCode
           
            window.close()  
             return true
        }
        
        function Close(strVal)
        {
             //debugger;
             var  dblYTMAnn =  "<%=dblYTMAnn%>";
             var  dblYTCAnn =  "<%=dblYTCAnn%>";
          
            if (strVal == "Submit" && dblYTMAnn == 0 && dblYTCAnn == 0)
            {
           
               alert ("Please calculate rate !")
               return false ;            
            }
            
            window.returnValue = strVal
            window.close()
            return true
        }
           
        function SaveValues()
        {
            elm = document.forms[0].elements;
         
            document.getElementById("Hid_FieldValues").value = "";
            for(i=0; i<elm.length; i++)
            {
                if(elm[i].id.indexOf("cbo_Search") != -1)
                {
                    document.getElementById("Hid_FieldValues").value = document.getElementById("Hid_FieldValues").value + elm[i].value + "!"
                  
                }
            }
        }  
        
        function RateValidation()
        {
            
            var rate = (document.getElementById("txt_Rate").value-0)
            if ( rate == 0)
            {
                alert('Rate cannot be zero or blank')
                return false;
                
            }
             
            return true;
        }


    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td class="HeaderCSS" align="center" colspan="2">
                    Fax Selection
                </td>
            </tr>
            <tr>
                <td class="SubHeaderCSS" width="50%">
                    Selection Details
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnl_Search" runat="server">
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btn_Search" runat="server" CssClass="ButtonCSS" Text="Search" />
                    <asp:Button ID="btn_ShowAll" runat="server" CssClass="ButtonCSS" Text="Show All" />
                </td>
            </tr>
            <tr>
                <td align="center" valign="middle">
                    <div id="div2" style="margin-top: 0px; overflow: auto; width: 850px; padding-top: 0px;
                        position: relative; height: 200px" align="center">
                        <asp:GridView ID="dg_Selection" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                            PageSize="10" CssClass="GridCSS" Width="850px">
                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                            <RowStyle HorizontalAlign="Center" CssClass="GridRowCSS" Width="875px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandName="SelectRow" ID="img_Select" Style="cursor: hand" runat="server"
                                            Width="13" Height="13" ImageUrl="~/Images/images3.jpg" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="SecurityId" DataField="SecurityId" Visible="false" />
                                <asp:TemplateField HeaderText="SecurityName">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_SecurityName" BackColor="#FFFFFF" Width="300px" Style="border-left-width: 0;
                                            border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                            runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityName") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="SecurityTypeName" DataField="SecurityTypeName" />
                                <asp:TemplateField HeaderText="SecurityIssuer">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_SecurityIssuer" BackColor="#FFFFFF" Width="180px" Style="border-left-width: 0;
                                            border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                            runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityIssuer") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ColorId" Visible="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_ColorId" BackColor="#FFFFFF" Width="173px" Style="border-left-width: 0;
                                            border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                            runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.ColorId") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="MaturityDate" DataField="MaturityDate" />
                                <asp:BoundField HeaderText="CouponRate" DataField="CouponRate" />
                                <asp:BoundField HeaderText="FaceValue" DataField="FaceValue" />
                                <asp:BoundField HeaderText="ISINNumber" DataField="ISINNumber" />
                                <%-- <asp:BoundField HeaderText="StockFaceValue" DataField="StockFaceValue" /> --%>
                                <asp:BoundField HeaderText="Abbreviation" DataField="Abbreviation" Visible="false" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="6" class="SeperatorRowCSS" style="height: 10px" align="center">
                    <asp:Label ID="lbl_Save" runat="server" Text="Security Added Successfuly Add New Security Or Clik On Submit"
                        ForeColor="#00C000"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="SubHeaderCSS" width="50%" colspan="4">
                    Security Rate Information
                </td>
            </tr>
        </table>
        <table id="Table2" width="90%" align="center" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td class="LabelCSS">
                    Security Name : &nbsp;
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txt_SecurityName" Font-Bold="true" runat="server" Width="340px"
                        CssClass="TextBoxCSS" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="LabelCSS">
                    Date:
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txt_Date" runat="server" CssClass="TextBoxCSS" TabIndex="13"></asp:TextBox>
                    <img class="formcontent" height="14" src="../Images/Calender.jpg" onclick="displayDatePicker('txt_Date',this);"
                        width="15" border="0" style="vertical-align: top; cursor: hand;" id="IMG2">
                    <%--   <asp:LinkButton ID="btn_Show" runat="server" Text="Show Selected Securities"></asp:LinkButton>--%>
                </td>
                <td>
                    <asp:RadioButtonList ID="rdo_RateActual" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" CssClass="LabelCSS">
                        <asp:ListItem Value="R" Selected="True">Rate</asp:ListItem>
                        <asp:ListItem Value="A">Actual</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="LabelCSS">
                    Rate : &nbsp;
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txt_Rate" runat="server" Width="150px" Height="16px" CssClass="TextBoxCSS"
                        MaxLength="20"></asp:TextBox>&nbsp;
                    <asp:Button ID="btn_CalRate" runat="server" Text="Calculate Rate" ToolTip="Calculate Rate"
                        CssClass="ButtonCSS" Height="20px" Width="130px" />
                </td>
                <td>
                    <asp:RadioButtonList ID="rdo_YXM" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        CssClass="LabelCSS">
                        <asp:ListItem Value="Y">Yield</asp:ListItem>
                        <asp:ListItem Value="X" Selected="True">XIRR</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="LabelCSS">
                    LotSize : &nbsp;
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txt_LotSize" runat="server" Width="340px" Height="32px" CssClass="fieldcontent"
                        TextMode="MultiLine" MaxLength="20"></asp:TextBox>
                </td>
                <td>
                    <asp:RadioButtonList ID="rdo_PhysicalDMAT" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" CssClass="LabelCSS">
                        <asp:ListItem Value="D" Selected="True">DMAT</asp:ListItem>
                        <asp:ListItem Value="P">Physical</asp:ListItem>
                        <asp:ListItem Value="S">SGL</asp:ListItem>
                    </asp:RadioButtonList>
                    <div id="divIPCalc">
                        <asp:RadioButtonList ID="rdo_IPCalc" runat="server" CssClass="LabelCSS" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="E" Selected="True">Equal Days</asp:ListItem>
                            <asp:ListItem Value="A">Actual Days</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="LabelCSS">
                    Rating\Remark : &nbsp;
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txt_RatingRemark" runat="server" Width="340px" Height="32px"
                        CssClass="fieldcontent" TextMode="MultiLine" MaxLength="20"></asp:TextBox>
                </td>
                <td>
                    <asp:RadioButtonList ID="rdo_Days" runat="server" CssClass="LabelCSS" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                        <asp:ListItem Selected="True" Value="365">365</asp:ListItem>
                        <asp:ListItem Value="366">366</asp:ListItem>
                    </asp:RadioButtonList>
                    <br />
                    <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DaysOptions" CssClass="LabelCSS"
                        runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0">
                        <asp:ListItem Selected="True" Value="F">First Year</asp:ListItem>
                        <asp:ListItem Value="A">All Year</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
        </table>
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Button ID="btn_Add" runat="server" Text="Add" ToolTip="Add" CssClass="ButtonCSS"
                        Height="20px" />&nbsp;
                    <asp:Button ID="btn_update" runat="server" Text="Update" ToolTip="Update" CssClass="ButtonCSS"
                        Height="20px" />&nbsp;
                    <asp:Button ID="btn_Show" runat="server" Text="Show Selected" ToolTip="Show Selected"
                        CssClass="ButtonCSS" Height="20px" Width="150px" />
                    <asp:HiddenField ID="Hid_ColWidths" runat="server" />
                    <asp:HiddenField ID="Hid_ColList" runat="server" />
                    <asp:HiddenField ID="Hid_DefaultSort" runat="server" />
                    <asp:HiddenField ID="Hid_FieldNames" runat="server" />
                    <asp:HiddenField ID="Hid_FieldValues" runat="server" />
                    <asp:HiddenField ID="Hid_ColText" runat="server" />
                    <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                    <asp:HiddenField ID="Hid_SecurityName" runat="server" />
                    <asp:HiddenField ID="Hid_RowIndex" runat="server" />
                    <asp:HiddenField ID="Hid_MatDate" runat="server" />
                    <asp:HiddenField ID="Hid_MatAmt" runat="server" />
                    <asp:HiddenField ID="Hid_CallDate" runat="server" />
                    <asp:HiddenField ID="Hid_CallAmt" runat="server" />
                    <asp:HiddenField ID="Hid_CoupDate" runat="server" />
                    <asp:HiddenField ID="Hid_CoupRate" runat="server" />
                    <asp:HiddenField ID="Hid_PutDate" runat="server" />
                    <asp:HiddenField ID="Hid_PutAmt" runat="server" />
                    <asp:HiddenField ID="Hid_RateAmtFlag" runat="server" />
                    <asp:HiddenField ID="Hid_InterestDate" runat="server" />
                    <asp:HiddenField ID="Hid_BookClosureDate" runat="server" />
                    <asp:HiddenField ID="Hid_GovernmentFlag" runat="server" />
                    <asp:HiddenField ID="Hid_FaceValue" runat="server" />
                    <asp:HiddenField ID="Hid_Issue" runat="server" />
                    <asp:HiddenField ID="Hid_DMATBkDate" runat="server" />
                    <asp:HiddenField ID="Hid_MMYRate" runat="server" />
                    <asp:HiddenField ID="Hid_Frequency" runat="server" />
                    <asp:HiddenField ID="Hid_TypeFlagSec" runat="server" />
                    <asp:HiddenField ID="Hid_Issuer" runat="server" />
                    <asp:HiddenField ID="Hid_Security" runat="server" />
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="Hid_Date" runat="server" />
                    <asp:HiddenField ID="Hid_SecurityTypeId" runat="server" />
                    <asp:HiddenField ID="Hid_OrderId" runat="server" />
                    <asp:HiddenField ID="Hid_CreditRating" runat="server" />
                    <asp:HiddenField ID="Hid_Semi_Ann_Flag" runat="server" />
                    <asp:HiddenField ID="Hid_CombineIPMat" runat="server" />
                    <asp:HiddenField ID="Hid_Rate_Actual_Flag" runat="server" />
                    <asp:HiddenField ID="Hid_Equal_Actual_Flag" runat="server" />
                    <asp:HiddenField ID="Hid_IntDays" runat="server" />
                    <asp:HiddenField ID="Hid_FirstYrAllYr" runat="server" />
                    <asp:HiddenField ID="Hid_FaxQuoteId" runat="server" />
                    
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btn_Sumbit" runat="server" Text="Submit" ToolTip="Submit" CssClass="ButtonCSS"
                        Height="20px" Visible="false" />
                    <asp:Button ID="btn_Close" runat="server" Text="Close" ToolTip="Close" CssClass="ButtonCSS"
                        Height="20px" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
