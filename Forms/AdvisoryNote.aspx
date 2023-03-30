<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="AdvisoryNote.aspx.vb" Inherits="Forms_AdvisoryNote" Title="Advisory Note Entry" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script language="javascript" type="text/javascript">
     function DateMonthSelection()
        {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_Selection_0").checked == true)
            { 
                document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "None";
                document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "";
                 document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "";
                 
             
            }
            else
            {  
                document.getElementById("ctl00_ContentPlaceHolder1_row_Month").style.display = "";
                document.getElementById("ctl00_ContentPlaceHolder1_row_FromDate").style.display = "none";
                 document.getElementById("ctl00_ContentPlaceHolder1_row_ToDate").style.display = "none";
               
            }
        } 
        
        function Validation()
        {
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_Debitnote")
               if (grd.rows.length < 2 )
               {
                    alert('No Records to Print!')
                    return false
               }
               
                 var txtName = ""
                 var txtName1 = ""
			var txtName2=""
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_Debitnote") 
            var blnSelected = false
            var ids = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIds").value.split("!")
            var BrokerIds =document.getElementById("ctl00_ContentPlaceHolder1_Hid_Intids").value.split("!")
            var transtype = document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value.split("!") 
           
           for(i=1; i<=(grd.rows.length-2); i++)
            {
                
                currRow = grd.children[0].children[i]    
//                                  
                if(currRow.children[0].children[0].checked == true)
                {
                    txtName = txtName + ids[i-1] + ",";
                    txtName1 = txtName1 + BrokerIds[i-1] + ",";
                    txtName2 = txtName2 + transtype[i-1] + ",";
                    blnSelected = true
                }
            }                   
            if(blnSelected == false)
            {
                alert("Please select atleast one option")
                return false
            }
            
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIds").value = txtName.substring(0,txtName.length - 1)
            
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_Intids").value =txtName1.substring(0,txtName1.length - 1)
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value =txtName2.substring(0,txtName2.length - 1)

        
        
        }     
        
        
        
         function CheckAll(checkVal)
        {
            for(i = 0;i < document.forms[0].elements.length; i++)
            {
                elm = document.forms[0].elements[i]
                if(elm.type == 'checkbox' && elm.disabled == false)
                {
                    elm.checked = checkVal
                    if(checkVal == true)
                        {
                        
                        }
                        else
                        {
                           
                        }    
                }
            }
        }
           function SelectRow(elm)
        {
            checkVal = elm.checked 
            if(checkVal == true)
            {
                elm.parentElement.parentElement.style.backgroundColor = "white"
            }
            else
            {
                elm.parentElement.parentElement.style.backgroundColor = "white"
            }    
        }
        
       
         function Submit()
        {
            var txtName = ""
            var intid = ""
          
                    var intids = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Intid").value.split("!")
                    var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_Debitnote") 
                    for(i=1; i<=(grd.children[0].children.length-2); i++)
                    {
                        currRow = grd.children[0].children[i]
                        if(currRow.style.backgroundColor.toUpperCase() == '#D1E4F8')
                        {
                            txtName = currRow.children[1].children[0].innerHTML
                            intid = intids[i-1] 
                            break
                        }
                    }                   
                    if(txtName == "")
                    {
                        alert("Please select atleast one option")
                        return false
                    }
                    document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelectedField").value = txtName 
                    document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelectedValue").value = intid         
                    window.returnValue = intid
                  
                    return false
            
           
        }                
    </script>

    <table id="Table1" width="90%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">
                Advisory Note Entry
            </td>
        </tr>
        <tr>
            <td align="center">
              <%--  <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>--%>
                        <table id="Table3" cellspacing="0" width="45%" cellpadding="0" border="0">
                            <tr>
                                <td colspan="6" class="SeperatorRowCSS">
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lbl_According" runat="server">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;According To:
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass="LabelCSS">
                                        <asp:ListItem Value="D" Selected="True">Deal Date</asp:ListItem>
                                        <asp:ListItem Value="S">Settlement Date</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lbl_print" runat="server">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Print:
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rdo_Selection" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass="LabelCSS" AutoPostBack="true">
                                        <asp:ListItem Value="D" Selected="True">DateWise</asp:ListItem>
                                        <asp:ListItem Value="M">MonthWise</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="row_Month" runat="server">
                                <td align="right">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Select Month-Year:
                                </td>
                                <td align="left">
                                    <asp:DropDownList AutoPostBack="false" ID="cbo_Months" runat="server" CssClass="ComboBoxCSS">
                                        <asp:ListItem Text="January" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList AutoPostBack="false" ID="cbo_Year" runat="server" CssClass="ComboBoxCSS"
                                        Width="94px">
                                        <asp:ListItem Text="2009" Value="2009"></asp:ListItem>
                                        <asp:ListItem Text="2010" Value="2010" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="2011" Value="2011"></asp:ListItem>
                                        <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                                        <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                        <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                                        <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                        <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                        <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                                        <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                        <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                        <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="row_FromDate" runat="server">
                                <td align="right">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;From Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS jsdate" Width="143px"
                                        TabIndex="1"></asp:TextBox>
                                  
                                </td>
                            </tr>
                            <tr id="row_ToDate" runat="server">
                                <td align="right">
                                    To Date:
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS jsdate" Width="143px" TabIndex="2"></asp:TextBox>
                                </td>
                            </tr>
                          
                            <tr id="tr_Broker" runat="server">
                                <td align="right">
                                    <asp:Label ID="Label2" runat="server" Text="Select Advisory: " CssClass="LabelCSS"
                                        Width="100px"></asp:Label>
                                </td>
                                <td align="left" valign="top">
                                    <uc:SelectFields ID="srh_Broker" class="LabelCSS" runat="server" ProcName="ID_SEARCH_BrokerMaster"
                                        FormHeight="475" FormWidth="257" SelectedValueName="AD.AdvisoryId" ChkLabelName=""
                                        ConditionExist="true" ConditionalFieldId="" LabelName="" SelectedFieldName="BrokerName"
                                        SourceType="StoredProcedure" ConditionalFieldName="" Visible="true" ShowLabel="false"
                                        ShowAll="true"></uc:SelectFields>
                                       
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr align="right">
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Show" runat="server" Text="Show" CssClass="ButtonCSS" TabIndex="19" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr align="right">
                                <td style="height: 81px" colspan="2">
                                    <div id="div2" runat="server" style="position: relative; top: 0px; left: 0px; height: 400px;
                                        width: 900px; overflow: auto">
                                        <asp:DataGrid ID="dg_Debitnote" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                                            TabIndex="38" Width="680px" ShowFooter="true" AllowSorting="true">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="right" CssClass="GridRowCSS" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="RefNo" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_RefNo" Width="100px" runat="server" CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chk_AllItems" runat="server" onclick="CheckAll(this.checked)" Checked="false">
                                                        </asp:CheckBox>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk_ItemChecked" runat="server" onclick="SelectRow(this)" Checked="false">
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="WDMDealNumber" SortExpression="WDMDealNumber">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_WDMDealNumber" Width="100px" runat="server" Text='<%# container.dataitem("WDMDealNumber") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealDate" SortExpression="DealDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealDate" runat="server" Width="80px" Text='<%# container.dataitem("DealDate") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SettlementDate" SortExpression="SettlementDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SettlementDate" runat="server" Width="80px" Text='<%# container.dataitem("SettlementDate") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Advisory Name" SortExpression="Brokername">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Brokername" runat="server" Width="305px" Text='<%# container.dataitem("Brokername") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="SecurityName" SortExpression="SecurityName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SecurityName" runat="server" Width="305px" Text='<%# container.dataitem("SecurityName") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Amount" SortExpression="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Amount" runat="server" Width="80px" Text='<%# container.dataitem("Amount") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealEntryId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealEntryId" runat="server" Width="150px" Text='<%# container.dataitem("DealEntryId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="TransType" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_TransType" runat="server" Width="55px" Text='<%# container.dataitem("TransType") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="BrokerId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_BrokerId" runat="server" Width="150px" Text='<%# container.dataitem("BrokerId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="70px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Label ID="lbl_Msg" runat="server" CssClass="LabelCSS"></asp:Label>
                                </td>
                                <asp:HiddenField ID="hid_ReportType" runat="server" />
                                <asp:HiddenField ID="Hid_DebitRefNo" runat="server" />
                                <asp:HiddenField ID="Hid_DealSlipIds" runat="server" />
                                <asp:HiddenField ID="Hid_Intids" runat="server" />
                                <asp:HiddenField ID="Hid_TransType" runat="server" />
                              
                            </tr>
                            <tr align="center" id="row_Export" runat="server" visible="false">
                                <td align="center" colspan="4">
                                    <table cellspacing="0" width="25%" cellpadding="0" border="0">
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="btn_Save" runat="server" Text="Save & Print" CssClass="ButtonCSS"
                                                    TabIndex="19" Width="120px" />
                                            </td>
                                            <td align="left" visible="false">
                                                &nbsp;
                                                <asp:Button ID="btn_Export" runat="server" Text="Export" CssClass="ButtonCSS" TabIndex="19"
                                                    Visible="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                  <%--  </ContentTemplate>
                </atlas:UpdatePanel>--%>
</asp:Content>
