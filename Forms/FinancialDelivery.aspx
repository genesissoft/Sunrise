<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="FinancialDelivery.aspx.vb" Inherits="Forms_FinancialDelivery" Title="Financial Delivery" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script language="javascript" type="text/jscript">

        function AddDetails() {
            var SettleMentAmt = (document.getElementById("ctl00_ContentPlaceHolder1_txt_SettlemntAmt").value - 0)
            var TotAmt = (document.getElementById("ctl00_ContentPlaceHolder1_txt_Total").value - 0)
            var BalAmt = ((SettleMentAmt - TotAmt) - 0)
            var strCustomerId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value
            var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value
            var strTransType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value

            if (strCustomerId == "") {
                AlertMessage('Validation', 'Please Select Deal Slip Number.', 175, 450)
                return false
            }
            var pageUrl = "FinancialDeliveryInfo.aspx";
            pageUrl = pageUrl + "?CustomerId=" + strCustomerId + "&BalAmt=" + BalAmt + "&DealSlipId=" + strDealSlipId + "&TransType=" + strTransType;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:780px;dialogHeight:300px;center:1;status:0;resizable:0;');

            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret;
                document.getElementById('<%= btn_AddInfo.ClientID%>').click();
            }
        }

        function UpdateDetails(rowIndex, strFDType, decAmount, strRemark, strPaymentDate, strBankName, strChequeNo, strChequeDate, intCustomerId, strDealSlipId, strDealSlipIds, TransType) {
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_RowIndex").value = rowIndex;
            var SettleMentAmt = (document.getElementById("ctl00_ContentPlaceHolder1_txt_SettlemntAmt").value - 0)
            var TotAmt = (document.getElementById("ctl00_ContentPlaceHolder1_txt_Total").value - 0)
            var BalAmt = ((SettleMentAmt - TotAmt) - 0)
            var TransType = document.getElementById("ctl00_ContentPlaceHolder1_Hid_TransType").value

            var pageUrl = "FinancialDeliveryInfo.aspx";
            var strValues = "";
            strValues = strValues + strFDType + "!"
            strValues = strValues + decAmount + "!"
            strValues = strValues + strRemark + "!"
            strValues = strValues + strPaymentDate + "!"
            strValues = strValues + strBankName + "!"
            strValues = strValues + strChequeNo + "!"
            strValues = strValues + strChequeDate + "!"
            strValues = strValues + intCustomerId + "!"
            strValues = strValues + strDealSlipId + "!"
            strValues = strValues + strDealSlipIds + "!"
            strValues = strValues + BalAmt + "!"
            strValues = strValues + TransType + "!"

            pageUrl = pageUrl + "?Values=" + strValues;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:780px;dialogHeight:300px;center:1;status:0;resizable:0;');
            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret;
                document.getElementById('<%= btn_AddInfo.ClientID%>').click();
            }
        }

        function ValidateGrid() {
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_FinancialDeal")
            if (grd != null) {
                if (grd.children[0].children.length <= 1) {
                    alert('Please Enter atleast one record')
                    return false
                }

                else {
                    return true
                }

            }
        }

        //        function ValidateTotalAmt()
        //        {
        //           var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_FinancialDeal")
        //           var txtSettlmntAmt = document.getElementById("ctl00_ContentPlaceHolder1_txt_SettlemntAmt").value
        //           var flag = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Flag").value
        //           var index = (document.getElementById("ctl00_ContentPlaceHolder1_Hid_Index").value-0) + 1
        //           
        //           var Amt = (document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value-0)
        //           for (i=1; i<= (grd.rows.length-1);i++)
        //           {
        //                if(flag == "E" && index == i)
        //                {
        //                    
        //                }
        //                else
        //                {
        //                    Amt = Amt + (grd.children[0].children[i].children[2].children[0].innerHTML-0)
        //                }
        //           }
        //            if (Amt > txtSettlmntAmt)
        //            {
        //                alert('Amount cannot be greater than settlement Amount')
        //                return false
        //            }
        //            return true
        //        }



        //        function Validation()
        //        {
        //           
        //            var cboFDType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_FDType")
        //            var strFD = cboFDType.options[cboFDType.options.selectedIndex].text
        //            var txtAmount = document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value
        //            var txtSettlmntAmt = document.getElementById("ctl00_ContentPlaceHolder1_txt_SettlemntAmt").value
        //            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_FDType").value) == "")
        //            {  
        //                alert("Please Select FD Type");
        //                return false;
        //            }
        //            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Amount").value) == "")
        //            {  
        //                alert("Please Enter Amount");
        //                return false;
        //            }

        //            if(strFD.substr(0,1) == "N")
        //            {
        //                if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_PaymntDate").value) == "")
        //                {  
        //                    alert("Please Enter Payment Date");
        //                    return false;
        //                }

        //                if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_BankName").value) == "")
        //                {  
        //                    alert("Please Enter Bank Name");
        //                    return false;
        //                }
        //                if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_ChqNo").value) == "")
        //                {  
        //                    alert("Please Enter Cheque Number");
        //                    return false;
        //                }
        //                if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_ChequeDate").value) == "")
        //                {  
        //                    alert("Please Enter Cheque Date");
        //                    return false;
        //                }
        //                     
        //               
        //            }
        //            if(ValidateTotalAmt() == false)
        //            {
        //                return false
        //            }
        //            return true
        //        }   

        //        function SelectRow()
        //        {
        //            var cboFDType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_FDType")
        //            var strFD = cboFDType.options[cboFDType.options.selectedIndex].text
        //           
        //            if(strFD.substr(0,1) == "N")
        //            {            
        //                document.getElementById ("td_AdjAgnstTrans").style.display ="none"
        //                document.getElementById ("td_PymtDet").style.display ="block"
        //            }
        //            else if(strFD.substr(0,1) == "A")
        //            {
        //                document.getElementById ("td_AdjAgnstTrans").style.display ="block"
        //                document.getElementById ("td_PymtDet").style.display ="none"
        //            }
        //        }
        function Delete_entry() {
            if (window.confirm("Are you sure you want to Delete record ?")) {
                return true
            }
            else {
                return false
            }
        }
        //	    function ConvertUCase(txtBox)
        //        {     
        //            txtBox.value = txtBox.value.toUpperCase()    
        //        }
    </script>

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" EnableViewState="true">
    </asp:ScriptManagerProxy>
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Financial Delivery</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table cellspacing="0" cellpadding="0" border="0" align="center" width="90%">
                            <tr align="center" valign="top">
                                <td style="width: 48%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                        <tr align="left">
                                            <td>Deal Slip No.:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:Search ID="srh_TransCode" runat="server" PageName="FinancialDelivery_TransCode" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo" CheckYearCompany="true" ConditionalFieldName="UserId"
                                                    ConditionalFieldId="Hid_UserId" ConditionalFieldId1="Hid_UserTypeId" ConditionalFieldName1="UserTypeId"></uc:Search>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Issuer Of Security:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_IssuerOfSecurity" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="100" TabIndex="3" Enabled="false" Font-Bold="true"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Name Of Security:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SecurityName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="100" TabIndex="3" Enabled="false" Font-Bold="true"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Client Name:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ClientName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="100" TabIndex="3" Enabled="false" Font-Bold="true"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 4%;">&nbsp;</td>
                                <td style="width: 48%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                        <tr align="left">
                                            <td>Face Value:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_FaceValue" runat="server" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7" Enabled="false" Font-Bold="true"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Settlement Amount:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SettlemntAmt" runat="server" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7" Enabled="false" Font-Bold="true"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Deal Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_DealDate" runat="server" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7" Enabled="false" Font-Bold="true"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Settlement Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SettlemntDate" runat="server" CssClass="TextBoxCSS"
                                                    MaxLength="50" TabIndex="7" Enabled="false" Font-Bold="true"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr align="center">
                                <td class="HeadingCenter" colspan="3">Add Financial Delivery Information</td>
                            </tr>
                            <tr align="center" id="tr_AddInfo" runat="server">
                                <td colspan="3">
                                    <asp:Button ID="btn_AddInfo" runat="server" CssClass="ButtonCSS hidden" Text="Add Info" />
                                    <input type="button" id="btn_AddInfo1" runat="server" class="ButtonCSS" value="Add Info" onclick="return AddDetails();" />
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="3">
                                    <div id="div2" style="margin-top: 0px; overflow: auto; width: 80%; padding-top: 0px; position: relative;">
                                        <asp:DataGrid ID="dg_FinancialDeal" runat="server" CssClass="GridCSS" AutoGenerateColumns="false"
                                            TabIndex="38" Width="80%">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                            CommandName="Delete" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="PaymentDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_PaymentDate" Width="75px" runat="server" Text='<%# container.dataitem("PaymentDate") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" Width="60px" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="ChequeNumber">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ChequeNumber" runat="server" Width="100px" Text='<%# container.dataitem("ChequeNumber") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="70px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Amount" runat="server" Width="100px" Text='<%# container.dataitem("Amount") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="BankName">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_BankName" BackColor="#FFFFFF" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                            onkeypress="scroll();"
                                                            runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.BankName") %>'></asp:TextBox>
                                                        <%--<asp:Label ID="lbl_BankName" runat="server" Width="45px" Text='<%# container.dataitem("BankName") %>'
                                                            CssClass="LabelCSS"></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="FDType">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_FDType" runat="server" Width="45px" Text='<%# container.dataitem("FDType") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="ChequeDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ChequeDate" runat="server" Width="70px" Text='<%# container.dataitem("ChequeDate") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Remark">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lbl_Remark" runat="server" Width="45px" Text='<%# container.dataitem("Remark") %>'
                                                            CssClass="LabelCSS"></asp:Label>--%>
                                                        <asp:TextBox ID="txt_Remark" BackColor="#FFFFFF" Width="120px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                            onkeypress="scroll();"
                                                            runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.Remark") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="FDId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_FDId" runat="server" Width="45px" Text='<%# container.dataitem("FDId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealSlipId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealSlipIds" runat="server" Width="45px" Text='<%# container.dataitem("DealSlipId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <%--  <asp:TemplateColumn HeaderText="BankId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_BankId" runat="server" Width="45px" Text='<%# container.dataitem("BankId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                                </asp:TemplateColumn>--%>
                                                <asp:TemplateColumn HeaderText="TransType" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_TransType" runat="server" Width="45px" Text='<%# container.dataitem("TransType") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr class="line_separator">
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr align="center" valign="top">
                                <td colspan="3">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center">
                                        <tr align="center">
                                            <td>Total Amount:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Total" runat="server" CssClass="TextBoxCSS" MaxLength="50" TabIndex="7"></asp:TextBox>
                                            </td>
                                            <td>Balance Amount:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_BalAmt" runat="server" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="line_separator">
                                            <td colspan="4">&nbsp;
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td colspan="4">
                                                <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                                                <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                                                <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                                    <asp:HiddenField ID="Hid_FDId" runat="server" />
                                    <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                                    <asp:HiddenField ID="Hid_Flag" runat="server" />
                                    <asp:HiddenField ID="Hid_Index" runat="server" />
                                    <asp:HiddenField ID="Hid_DealSlipIds" runat="server" />
                                    <asp:HiddenField ID="Hid_FDType" runat="server" />
                                    <asp:HiddenField ID="Hid_AdjDealSlipId" runat="server" />
                                    <asp:HiddenField ID="Hid_RetValues" runat="server" />
                                    <asp:HiddenField ID="Hid_TransType" runat="server" />
                                    <asp:HiddenField ID="Hid_RowIndex" runat="server" />
                                    <asp:HiddenField ID="Hid_UserId" runat="server" />
                                    <asp:HiddenField ID="Hid_UserTypeId" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
