<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ReportSelectionDealDetail.aspx.vb" Inherits="Forms_ReportSelectionDealDetail"
    Title="Deal Report Selection" %>

<%--<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function validation()
        {
            var DealSlipNo = document.getElementById("ctl00_ContentPlaceHolder1_Srh_DealSlipNo")
            var Customer = document.getElementById("ctl00_ContentPlaceHolder1_Srh_Customer")
            var security = document.getElementById("ctl00_ContentPlaceHolder1_Srh_security")
            if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_select_1").checked == true)
            {
                if(DealSlipNo!=  null)
                {
                    if((document.getElementById("ctl00_ContentPlaceHolder1_Srh_DealSlipNo_txt_Name").value) == "")
                    {  
                        AlertMessage("Validation", "Please Select DealSlipNo", 175, 450);
               
                        return false;  
                    }   
                }
    
            }
            if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_select_2").checked == true)
            {
                if(Customer!=  null)
                {
                    if((document.getElementById("ctl00_ContentPlaceHolder1_Srh_Customer_txt_Name").value) == "")
                    {  
                        AlertMessage("Validation", "Please Select CustomerName", 175, 450);
               
                        return false;  
                    }   
                }
    
            }
            if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_select_3").checked == true)
            {
                if(security!=  null)
                {
                    if((document.getElementById("ctl00_ContentPlaceHolder1_Srh_security_txt_Name").value) == "")
                    {  
                        AlertMessage("Validation", "Please Select Securityname", 175, 450);
               
                        return false;  
                    }   
                }
    
            }
    
        }
        function Update(intIndex,strDealSlipId,strTransType,strDealTransType,strDealslipNo)
        {
            var a = navigator.onLine; 
            var Rpt= "DealDetailReport"
            var strFromdate = document.getElementById("ctl00_ContentPlaceHolder1_txt_Date").value;
            var pageUrl1 = "ViewDealReports.aspx?DealSlipId=" + strDealSlipId  + "&TransType=" + strTransType + "&DealTransType=" + strDealTransType +"&Rpt=" + Rpt +  "&Fromdate=" + strFromdate + "&DealslipNo=" + strDealslipNo;      
            if (a == true)
            {
                window.open(pageUrl1,target="_blank","left=80,top=80,height=600,width=980,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")             
            }
        }
   
        function HidRow()
        {
    
            if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_select_0").checked == true)
            {   
                document.getElementById("Row_dealslipno").style.display = "none";
                document.getElementById("row_customer").style.display = "none";
                document.getElementById("row_security").style.display = "none";
                document.getElementById("row_dealdate").style.display = "";
                
            }
            else if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_select_1").checked == true)
            {
                document.getElementById("Row_dealslipno").style.display = "";
                document.getElementById("row_customer").style.display = "none";
                document.getElementById("row_security").style.display = "none";
                document.getElementById("row_dealdate").style.display = "none";
            }
            else if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_select_2").checked == true)
            {
                document.getElementById("Row_dealslipno").style.display = "none";
                document.getElementById("row_customer").style.display = "";
                document.getElementById("row_security").style.display = "none";
                document.getElementById("row_dealdate").style.display = "none";
            }
            else if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_select_3").checked == true)
            {
                document.getElementById("Row_dealslipno").style.display = "none";
                document.getElementById("row_customer").style.display = "none";
                document.getElementById("row_security").style.display = "";
                document.getElementById("row_dealdate").style.display = "none";
            }
        
   
        }
   
        function OpenReport(intIndex,strDealSlipId,strTransType,strDealTransType,strDealslipNo)
        {   
            var a=navigator.onLine; 
                  
            var Rpt= "DealDetail"
            var strFromdate = document.getElementById("ctl00_ContentPlaceHolder1_txt_Date").value;
            var pageUrl = "ViewDealReports.aspx?DealSlipId=" + strDealSlipId  + "&TransType=" + strTransType + "&DealTransType=" + strDealTransType +"&Rpt=" + Rpt +  "&Fromdate=" + strFromdate + "&DealslipNo=" + strDealslipNo;
            if (a  == true)
            {
                window.open(pageUrl,target="_blank","left=80,top=80,height=0,width=0,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")                

            }
         
         
        }
        
        
        
    </script>

    <script type="text/javascript">
        function SetPrintProperties() 
        {
                
            try 
            {
                shell = new ActiveXObject("WScript.Shell");
                shell.SendKeys("%fu");
                window.setTimeout("javascript:SetPaperSize();", 1200);

            }
            catch(e)
            {
                AlertMessage("Validation", ('Please Change your Internet Security Settings\nGo to Internet Explorer --> Tools --> Internet Options --> Security --> Internet --> Custom Level \nGo to Initialize and script ActiveX controls not marked as safe --> Select Prompt --> Click Ok', 175, 450)
                }
            return false
        }

        function SetPaperSize() 
        {
           
            shell.sendKeys("AA%a{TAB}{TAB}{TAB}{DEL}{TAB}{DEL}");
            shell.sendKeys("AA%a{TAB}%o{TAB}.2{TAB}0{TAB}0{TAB}0{ENTER}");            //}
        }
        function setLandScape() 
        {

            shell.sendKeys("%fp"); 
            window.print();
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
                AlertMessage("Validation", 'Please Change your Internet Security Settings\nGo to Internet Explorer --> Tools --> Internet Options --> Security --> Internet --> Custom Level \nGo to Initialize and script ActiveX controls not marked as safe --> Select Enable --> Click Ok', 175, 450)
                return false
            }            
        }
    </script>

    <div>
        <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
            <tr align="left">
                <td class="SectionHeaderCSS">Report Selection
                </td>
            </tr>
            <tr class="line_separator">
                <td>&nbsp;
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    <table align="center" cellspacing="0" cellpadding="0" border="0" width="45%">
                        <tr align="left" id="row_select">
                            <td>Select
                            </td>
                            <td style="padding-left: 0px;" nowrap="nowrap">
                                <asp:RadioButtonList ID="rdo_select" runat="server" RepeatDirection="Horizontal"
                                    CssClass="LabelCSS" CellPadding="0" CellSpacing="0">
                                    <asp:ListItem Value="DealDate" Selected="true">DealDate</asp:ListItem>
                                    <asp:ListItem Value="DealSlipNo">DealSlipNo</asp:ListItem>
                                    <asp:ListItem Value="CustomerName">Customer</asp:ListItem>
                                    <asp:ListItem Value="SecurityName">Security</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr align="left" id="row_dealdate">
                            <td>Deal Date:
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Date" runat="server" CssClass="TextBoxCSS" Width="115px" TabIndex="9"></asp:TextBox><img
                                    class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Date',this);"
                                    id="IMG5" />
                            </td>
                        </tr>
                        <tr align="left" id="Row_dealslipno">
                            <td class="LabelCSS" id="Td1">DealSlip No:
                            </td>
                            <td style="padding-left: 0px;">
                                <uc:Search ID="Srh_DealSlipNo" runat="server" PageName="PrintDeals" AutoPostback="true"
                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo" ConditionExist="true" ConditionalFieldName="UserId"
                                    ConditionalFieldId="Hid_UserId" ConditionalFieldId1="Hid_UserTypeId"
                                    ConditionalFieldName1="UserTypeId" CheckYearCompany="true" ></uc:Search>
                            </td>
                        </tr>
                        <tr align="left" id="row_customer">
                            <td id="Client">Customer:
                            </td>
                            <td style="padding-left: 0px;">
                                <uc:Search ID="Srh_Customer" runat="server" AutoPostback="true" PageName="CustomerMasterNew"
                                    SelectedFieldName="CustomerName" SelectedFieldId="Id"></uc:Search>
                            </td>
                        </tr>
                        <tr align="left" id="row_security">
                            <td>Security:
                            </td>
                            <td style="padding-left: 0px;">
                                <uc:Search ID="Srh_security" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                    SelectedFieldId="Id" SelectedFieldName="SecurityName" />
                            </td>
                        </tr>
                        <tr align="left">
                            <td>Select
                            </td>
                            <td style="padding-left: 0px;">
                                <asp:RadioButtonList ID="rdo_PrintOption" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" CssClass="LabelCSS" CellPadding="0" CellSpacing="0">
                                    <asp:ListItem Value="P" Selected="true">Print</asp:ListItem>
                                    <asp:ListItem Value="E">Export</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr class="line_separator">
                            <td colspan="2"></td>
                        </tr>
                        <tr align="left">
                            <td>&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btn_Show" runat="server" CssClass="ButtonCSS" Text="Show" TabIndex="45" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="line_separator">
                <td>&nbsp;
                </td>
            </tr>
            <tr align="center">
                <td class="HeadingCenter">Details Setion
                </td>
            </tr>
            <tr class="line_separator">
                <td>&nbsp;
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    <div id="div2" style="margin-top: 0px; overflow: auto; width: 95%; padding-top: 0px; position: relative; height: 250px">
                        <asp:DataGrid ID="dg1" runat="server" CssClass="GridCSS" ShowFooter="True" Width="95%"
                            AutoGenerateColumns="false" TabIndex="38">
                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                            <%--<ItemStyle  CssClass="GridRowCSS" />--%>
                            <Columns>
                                <asp:TemplateColumn HeaderText="DealSlipNo">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnk_DealSlipNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DealSlipNo") %>'
                                            ToolTip="Click to See View" Width="100px">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="center" Width="100px" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Security">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_SecurityName" BackColor="#FFFFFF" Width="100px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                            onkeypress="scroll();"
                                            runat="server" CssClass="LabelCSS" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityName") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="SecurityIssuer">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_SecurityIssuer" BackColor="#FFFFFF" Width="80px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                            onkeypress="scroll();"
                                            runat="server" CssClass="LabelCSS" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityIssuer") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="DealDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_DealDate" runat="server" Width="80px" Text='<%# container.dataitem("DealDate") %>'
                                            CssClass="LabelCSS"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="center" Width="100px" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="SettlementDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_SettlementDate" runat="server" Width="80px" Text='<%# container.dataitem("SettmentDate") %>'
                                            CssClass="LabelCSS"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="center" Width="100px" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Rate" runat="server" Width="60px" Text='<%# container.dataitem("Rate") %>'
                                            CssClass="LabelCSS"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="FaceValue">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_FaceValue" runat="server" Width="60px" Text='<%# container.dataitem("FaceValue") %>'
                                            CssClass="LabelCSS"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="TransType" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_TransType" Width="80px" runat="server" Text='<%# container.dataitem("TransType") %>'
                                            CssClass="LabelCSS"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Right" Width="80px" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="left" VerticalAlign="Middle" Width="80px" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="RemainingFV">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_RemainingFaceValue" runat="server" Width="100px" Text='<%# container.dataitem("RemainingFaceValue") %>'
                                            CssClass="LabelCSS"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="right" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Profit">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ProfitPursell" runat="server" Width="100px" Text='<%# container.dataitem("ProfitPursell") %>'
                                            CssClass="LabelCSS"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="right" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="DealSlipID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_DealSlipID" Width="80px" runat="server" Text='<%# container.dataitem("DealSlipID") %>'
                                            CssClass="LabelCSS"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Right" Width="80px" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="left" VerticalAlign="Middle" Width="80px" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="DealTransType" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_DealTransType" Width="80px" runat="server" Text='<%# container.dataitem("DealTransType") %>'
                                            CssClass="LabelCSS"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Right" Width="80px" VerticalAlign="Middle" />
                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        HorizontalAlign="left" VerticalAlign="Middle" Width="80px" />
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="Hid_DealslipId" runat="server" />
        <asp:HiddenField ID="Hid_DealSlipNo" runat="server" />
        <asp:HiddenField ID="Hid_dealTransType" runat="server" />
        <asp:HiddenField ID="Hid_TransType" runat="server" />
        <asp:HiddenField ID="Hid_UserId" runat="server" />
        <asp:HiddenField ID="Hid_UserTypeId" runat="server" />
    </div>
</asp:Content>
