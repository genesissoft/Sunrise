<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="RetailCreditNote.aspx.vb" Inherits="Forms_RetailDebitNote" Title="Retail Debit Note" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script language="javascript" type="text/javascript">

        function showvalidation()
        {
             if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitDate").value) == "")
            {  alert("Please select Debit Date");
               return false;
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
            var ServTax =""
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_Debitnote") 
            var blnSelected = false
           
            var ids = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIds").value.split("!")
          
            
            for(i=1; i<=(grd.rows.length-2); i++)
            {
                currRow = grd.children[0].children[i]   
                  
                if(currRow.children[0].children[0].checked == true)
                {
                    txtName = txtName + ids[i-1] + ",";                    
                    blnSelected = true
              
                    if(currRow.children[1].children[0].checked == true)
                    {
                        ServTax = ServTax + "Y"  + ",";
                    }
                    else
                    
                    {
                        ServTax = ServTax + "N"  + ",";
                    }
                    
                }
                 
              
            
                
                
            }                   
            if(blnSelected == false)
            {
                alert("Please select atleast one option")
                return false
            }
            
            
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIds").value = txtName.substring(0,txtName.length - 1)
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_STInEx").value =ServTax.substring(0,ServTax.length - 1)
            alert(document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipIds").value)
            alert(document.getElementById("ctl00_ContentPlaceHolder1_Hid_STInEx").value)
            
        
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
        function CheckAll1(checkVal)
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
           function SelectRow1(elm)
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
            
    </script>

    <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Retail Credit Note</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <%--  <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>--%>
                <table cellspacing="0" width="50%" cellpadding="0" border="0">
                    <tr align="left">
                        <td id="lbl_According" runat="server">
                            According To:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rdo_DateType" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS">
                                <asp:ListItem Value="D" Selected="True">Deal Date</asp:ListItem>
                                <asp:ListItem Value="S">Settlement Date</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <%--   <tr>
                        <td align="right" id="Td1" runat="server">
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Report Type:
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rdo_DebitCredit" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS">
                                <asp:ListItem Value="D" Selected="True">Debit Note</asp:ListItem>
                                <asp:ListItem Value="C">Credit Note</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>--%>
                    <%-- <tr runat ="server" visible ="false" >
                        <td align="right" id="lbl_print" runat="server">
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Print:
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rdo_Selection" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS">
                                <asp:ListItem Value="D" Selected="True">DateWise</asp:ListItem>
                                <asp:ListItem Value="M">MonthWise</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>--%>
                    <tr align="left" id="row_Month" runat="server">
                        <td>
                            Select Month-Year:
                        </td>
                        <td>
                            <asp:DropDownList AutoPostBack="false" ID="cbo_Months" runat="server" CssClass="ComboBoxCSS"
                                Width="95px">
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
                                Width="65px">
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
                    <tr align="left">
                        <td>
                            Deal Trans Type:
                        </td>
                        <td>
                            <asp:DropDownList ID="cbo_DealTransType" runat="server" CssClass="ComboBoxCSS" Width="95px"
                                AutoPostBack="True">
                                <asp:ListItem Text="All" Value="A"> </asp:ListItem>
                                <asp:ListItem Text="Trading" Value="T"></asp:ListItem>
                                <asp:ListItem Text="Distribution" Value="D"></asp:ListItem>
                            </asp:DropDownList><i style="color: Red; vertical-align: super;">*</i>
                        </td>
                    </tr>
                    <tr align="left" id="row_FromDate" runat="server" visible="false">
                        <td>
                            From Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS" Width="115px"
                                TabIndex="1"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate',this);" /></td>
                    </tr>
                    <tr align="left" id="row_ToDate" runat="server" visible="false">
                        <td align="right">
                            To Date:</td>
                        <td>
                            <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS" Width="115px" TabIndex="2"></asp:TextBox><img
                                class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate',this);" /></td>
                    </tr>
                    <%-- <tr id="tr2" runat="server">
                                <td align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Select Customer: " CssClass="LabelCSS"
                                        Width="100px"></asp:Label></td>
                                <td align="left" valign="top">
                                    <uc:SelectFields ID="srh_Customer" class="LabelCSS" runat="server" ProcName="ID_SEARCH_CustomerMaster"
                                        FormHeight="475" FormWidth="257" SelectedValueName="CM.CustomerId" ChkLabelName=""
                                        ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerName" SourceType="StoreProcedure"
                                        ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                                </td>
                            </tr>--%>
                    <tr align="left" id="tr_Broker" runat="server" visible="false">
                        <td>
                            <%-- <asp:Label ID="Label1" runat="server" Text="Select Broker: " CssClass="LabelCSS"
                                Width="100px"></asp:Label>--%>
                            Select Broker:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_Broker" runat="server" ProcName="ID_SEARCH_CreditBroker"
                                FormHeight="475" FormWidth="257" SelectedValueName="Brokerid" ChkLabelName=""
                                ConditionExist="false" ConditionalFieldId="" LabelName="" SelectedFieldName="BrokerName"
                                SourceType="StoredProcedure" ConditionalFieldName="" Visible="true" ShowLabel="false"
                                ShowAll="true"></uc:SelectFields>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>--%>
                    <%--  <tr>
                        <td align="right">
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Service Tax:
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rdo_incServtax" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="LabelCSS">
                                <asp:ListItem Value="I" Selected="True">Inclusive</asp:ListItem>
                                <asp:ListItem Value="E">Exclusive</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>--%>
                    <tr align="left">
                        <td>
                            Debit Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_DebitDate" runat="server" CssClass="TextBoxCSS" Width="90px"
                                TabIndex="0"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_DebitDate',this);"
                                    id="IMG2" />
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
                            <asp:Button ID="btn_Show" runat="server" Text="Show" CssClass="ButtonCSS" TabIndex="19" />
                        </td>
                    </tr>
                    <tr class="line_separator">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center" valign="top">
            <td colspan="2">
                <div id="div2" style="margin-top: 0px; overflow: auto; width: 75%; padding-top: 0px;
                    position: relative; height: 150px">
                    <asp:DataGrid ID="dg_Debitnote" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                        TabIndex="38" Width="95%" ShowFooter="true">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                        <ItemStyle HorizontalAlign="right" CssClass="GridRowCSS" />
                        <Columns>
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
                            <asp:TemplateColumn HeaderText="Service tax">
                                <%-- <HeaderTemplate>
                                                <asp:CheckBox ID="chk_AllItems1" runat="server" onclick="CheckAll1(this.checked)" Checked="false">
                                                </asp:CheckBox>
                                            </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_ItemChecked1" runat="server" onclick="SelectRow1(this)" Checked="false">
                                    </asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" />
                            </asp:TemplateColumn>
                            <%--  <asp:TemplateColumn>
                                            <ItemTemplate>
                                              <asp:RadioButton id="rdo" runat="server" AutoPostBack="True" Text ="Y"></asp:RadioButton>

                                            </ItemTemplate>
                                        </asp:TemplateColumn>--%>
                            <%--  <asp:TemplateColumn HeaderText="RefNo" Visible >
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_RefNo" Width="100px" runat="server" CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>--%>
                            <asp:TemplateColumn HeaderText="DealslipNo">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_DealNumber" Width="100px" runat="server" Text='<%# container.dataitem("DealslipNo") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Broker Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Brokername" runat="server" Width="400px" Text='<%# container.dataitem("BrokerName") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="BrokerId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BrokerId" runat="server" Width="305px" Text='<%# container.dataitem("BrokerId") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <%--   <asp:TemplateColumn HeaderText="DealDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_DealDate" runat="server" Width="80px" Text='<%# container.dataitem("DealDate") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>--%>
                            <asp:TemplateColumn HeaderText="DealSlipId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_DealSlipId" runat="server" Width="150px" Text='<%# container.dataitem("DealSlipId") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="BrokerageAmt" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BrokAmt" runat="server" Width="55px" Text='<%# container.dataitem("BrokerageAmt") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Label ID="lbl_Msg" runat="server" CssClass="LabelCSS"></asp:Label>
            </td>
            <asp:HiddenField ID="hid_ReportType" runat="server" />
            <asp:HiddenField ID="Hid_DebitRefNo" runat="server" />
            <asp:HiddenField ID="Hid_ServTax" runat="server" />
            <asp:HiddenField ID="Hid_Cess" runat="server" />
            <asp:HiddenField ID="Hid_ECess" runat="server" />
            <asp:HiddenField ID="Hid_ReptForm" runat="server" />
            <asp:HiddenField ID="Hid_DealSlipIds" runat="server" />
            <asp:HiddenField ID="Hid_STInEx" runat="server" />
        </tr>
        <tr align="center" id="row_Export" runat="server" visible="false">
            <td align="center" colspan="6">
                <table cellspacing="0" width="100%" cellpadding="0" border="0">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Save" runat="server" Text="Save & Print" CssClass="ButtonCSS"
                                TabIndex="19" />
                        </td>
                        <td align="left" runat="server">
                            &nbsp;
                            <asp:Button ID="btn_Export" runat="server" Text="Export" CssClass="ButtonCSS" TabIndex="19" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%--       </ContentTemplate>
                </atlas:UpdatePanel>--%>
</asp:Content>
