<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="DMATDelivery.aspx.vb" Inherits="Forms_DMATDelivery" Title="Demat Delivery" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" language="javascript">
        function validation() {
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_FinancialDeal")
            if (grd != null) {
                if (grd.children[0].children.length <= 2) {
                    AlertMessage('Validation', 'Please Enter atleast one record', 175, 450)
                    return false
                }
                if (ValidateTotalAmt() == false) {
                    return false
                }
                else {
                    return true
                }

            }
        }
        function ValidateTotalAmt() {
            var FaceValue = document.getElementById("ctl00_ContentPlaceHolder1_txt_FaceValue")
            var FaceValMultiple = document.getElementById("ctl00_ContentPlaceHolder1_Cbo_FaceValue")
            var totalAmt = document.getElementById("ctl00_ContentPlaceHolder1_txt_Total")
            var TotalFaceVal = (FaceValue * FaceValMultiple)
            if (totalAmt > TotalFaceVal) {
                AlertMessage('Validation', 'Total Amount Exceeds  face value ', 175, 450)
                return false
            }
            else {
                return true
            }

        }
        function Delete_entry() {
            if (window.confirm("Are you sure you want to Delete record ?")) {
                return true
            }
            else {
                return false
            }
        }

        function Update(strId) {
            document.all("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value = strId;
            return true
        }

        function AddDetails() {
            var HidDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value
            var Hidfacevalue = document.getElementById("ctl00_ContentPlaceHolder1_Hid_facevalue").value
            var HidFaceMultiple = document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaceMultiple").value
            var DematAccTo = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DematAccTo").value
            var HidBalAmt = document.getElementById("ctl00_ContentPlaceHolder1_txt_BalAmt").value
            var HidPayMode = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PayMode").value
            if (HidDealSlipId == "") {
                AlertMessage('Validation', 'Please Select Deal Slip Number', 175, 450);
                return false;
            }

            var pageUrl = "DematDeliveryDetails.aspx";
            pageUrl = pageUrl + "?DealSlipId=" + HidDealSlipId + "&facevalue=" + Hidfacevalue + "&FaceMultiple=" + HidFaceMultiple + "&BalAmt=" + HidBalAmt + "&DematAccTo=" + DematAccTo + "&PayMode=" + HidPayMode;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:780px;dialogHeight:400px;center:1;status:0;resizable:0;');

            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret;
                document.getElementById('<%= btn_AddInfo.ClientID%>').click();

            }
            else {
            }
        }
        function UpdateDetails(rowIndex, strFaceVal, strNSDLFaceVal, strDmatSlipNo, strClientName, strDPName, strDPId, strClientId, strQty, strDelDate, strDematAccTo, strCustSlipNo, strCustDPId, strCustDPName, strCustClientId, strFaceMultiple) {
            var pageUrl = "DematDeliveryDetails.aspx";
            var strReturn = "";
            var Remark = document.getElementById("ctl00_ContentPlaceHolder1_Hid_Remark").value
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_RowIndex").value = rowIndex;
            strReturn = strReturn + strFaceVal + "!"
            strReturn = strReturn + strNSDLFaceVal + "!"
            strReturn = strReturn + strDmatSlipNo + "!"
            strReturn = strReturn + "" + "!"
            strReturn = strReturn + strDPName + "!"
            strReturn = strReturn + strDPId + "!"

            strReturn = strReturn + strClientId + "!"
            strReturn = strReturn + strQty + "!"
            strReturn = strReturn + strDelDate + "!"
            strReturn = strReturn + strDematAccTo + "!"

            strReturn = strReturn + strCustSlipNo + "!"
            strReturn = strReturn + strCustDPId + "!"
            strReturn = strReturn + strCustDPName + "!"

            strReturn = strReturn + strCustClientId + "!"
            strReturn = strReturn + strFaceMultiple + "!"
            strReturn = strReturn + Remark + "!"

            document.getElementById("ctl00_ContentPlaceHolder1_Hid_DematAccTo").value = strDematAccTo

            var DematAccTo = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DematAccTo").value

            var HidDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DealSlipId").value
            var Hidfacevalue = document.getElementById("ctl00_ContentPlaceHolder1_Hid_facevalue").value
            var HidFaceMultiple = document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaceMultiple").value
            var HidPayMode = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PayMode").value
            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_FinancialDeal").children[0]
            var HidBalAmt = document.getElementById("ctl00_ContentPlaceHolder1_txt_BalAmt").value

            HidBalAmt = (HidBalAmt - 0) + (strFaceVal - 0)
            pageUrl = pageUrl + "?Values=" + strReturn + "&Index=" + rowIndex + "&DealSlipId=" + HidDealSlipId + "&facevalue=" + Hidfacevalue + "&FaceMultiple=" + HidFaceMultiple + "&BalAmt=" + HidBalAmt + "&DmatAccto=" + DematAccTo + "&PayMode=" + HidPayMode;
            var ret = window.showModalDialog(pageUrl, 'some argument', 'dialogWidth:780px;dialogHeight:400px;center:1;status:0;resizable:0;');

            if (typeof (ret) != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret;
                document.getElementById('<%= btn_AddInfo.ClientID%>').click();

            }
        }


    </script>

    <asp:UpdatePanel ID="upd" runat="server" Mode="Conditional">
        <ContentTemplate>
            <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr align="left">
                    <td class="SectionHeaderCSS">Demat Delivery</td>
                </tr>
                <tr class="line_separator">
                    <td>&nbsp;
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table cellspacing="0" cellpadding="0" border="0" align="center" width="90%">
                            <tr align="center" valign="top">
                                <td style="width: 48%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                        <tr align="left">
                                            <td>DealSlip No:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <uc:Search ID="srh_TransCode" runat="server" PageName="DMatDelivery_TransCode" AutoPostback="true"
                                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo" ConditionalFieldName="UserId"
                                                    ConditionalFieldId="Hid_UserId" ConditionalFieldId1="Hid_UserTypeId" ConditionalFieldName1="UserTypeId" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Issuer Of Security:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_IssuerOfSecurity" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="100" TabIndex="3" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Name Of Security:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SecurityName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="100" TabIndex="3" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Client Name:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_ClientName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                                    MaxLength="100" TabIndex="3" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 4%;">&nbsp;</td>
                                <td style="width: 48%;">
                                    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
                                        <tr align="left">
                                            <td>Face Value:
                                            </td>
                                            <td style="padding-left: 0px;">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr align="left">
                                                        <td>
                                                            <asp:TextBox ID="txt_FaceValue" runat="server" CssClass="TextBoxCSS" Width="95px"
                                                                TabIndex="9" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Cbo_FaceValue" Width="72px" runat="server" CssClass="ComboBoxCSS"
                                                                TabIndex="1" Enabled="False">
                                                                <asp:ListItem Value="1000">Thousand</asp:ListItem>
                                                                <asp:ListItem Selected="true" Value="100000">Lac</asp:ListItem>
                                                                <asp:ListItem Value="10000000">Crore</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>Settlement Amount:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SettlmntAmt" runat="server" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Deal Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_TransDate" runat="server" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr align="left">
                                            <td>Settlement Date:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SettlmtDate" runat="server" CssClass="TextBoxCSS" MaxLength="50"
                                                    TabIndex="7" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                    </table>
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
                    <td class="HeadingCenter">Add DEMAT Information</td>
                </tr>
                <tr align="center">
                    <td>
                        <asp:Button ID="btn_AddInfo" runat="server" Text="Add Info" ToolTip="Add Info" CssClass="ButtonCSS hidden" />
                        <input type="button" id="btn_AddInfo1" runat="server" class="ButtonCSS" value="Add Info" onclick="return AddDetails();" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div id="div2" style="margin-top: 0px; overflow: auto; width: 90%; padding-top: 0px; position: relative;">
                            <asp:DataGrid ID="dg_FinancialDeal" runat="server" CssClass="GridCSS" ShowFooter="true"
                                AutoGenerateColumns="false" TabIndex="38" Width="100%">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                <Columns>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit" />
                                            <input type="button" id="imgBtn_Edit1" runat="server" class="TitleText hidden" value="Edit" />
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                CommandName="Delete" />
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Deliverydate">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DelDate" Width="60px" runat="server" Text='<%# container.dataitem("Deliverydate") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="DEMAT Slip No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ChequeNumber" runat="server" Width="120px" Text='<%# container.dataitem("DmatSlipNo") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="center" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Qty" runat="server" Width="100px" Text='<%# container.dataitem("Quantity") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="DpName">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DpName" runat="server" Width="100px" Text='<%# container.dataitem("DpName") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="DpId" Visible="True">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DpId" runat="server" Width="100px" Text='<%# container.dataitem("DpId") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="ClientId">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_BankName" runat="server" Width="80px" Text='<%# container.dataitem("ClientId") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="DealSlipId" Visible="false" SortExpression="DealSlipId">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DealSlipId" runat="server" Width="45px" Text='<%# container.dataitem("DealSlipId") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="NSDL Face Value" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_NSDLFaceValue" runat="server" Width="100px" Text='<%# container.dataitem("NSDLFaceValue") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Face Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_FaceValue" runat="server" Width="100px" Text='<%# container.dataitem("FaceValue") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="CustomerName" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CustName" runat="server" Width="100px" Text='<%# container.dataitem("CustomerName") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="CustDPId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CustDPId" runat="server" Width="100px" Text='<%# container.dataitem("CustDPId") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="CustomerSlipNumber" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CustSlipNo" runat="server" Width="100px" Text='<%# container.dataitem("CustomerSlipNumber") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="DMatId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DMatId" runat="server" Width="100px" Text='<%# container.dataitem("DMatId") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="FaceMultiple" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_FaceMultiple" runat="server" Width="100px" Text='<%# container.dataitem("FaceMultiple") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="DematAccTo" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DematAccTo" runat="server" Width="100px" Text='<%# container.dataitem("DematAccTo") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="SettleNo" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_SettleNo" runat="server" Width="100px" Text='<%# container.dataitem("SettleNo") %>'
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
                <tr align="center" id="row_AddInfo" runat="server">
                    <td>
                        <%-- <asp:Button ID="btn_AddInfo" runat="server" Text="Add Info" ToolTip="Add Info" CssClass="ButtonCSS"
                            Height="20px" />--%>
                        <%--    <asp:Button ID="btn_AddNew" runat="server" Text="Clear" ToolTip="Add New" CssClass="ButtonCSS"
                            Height="20px" Visible ="false"  />--%>
                    </td>
                </tr>
                <tr align="center">
                    <td>
                        <table cellpadding="0" cellspacing="5" border="0" align="center">
                            <tr align="left">
                                <td>Total Amount:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Total" runat="server" CssClass="TextBoxCSS" MaxLength="50" TabIndex="7"></asp:TextBox>
                                </td>
                                <td>Balance Amount:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_BalAmt" runat="server" CssClass="TextBoxCSS" MaxLength="50"
                                        TabIndex="7" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="line_separator">
                    <td></td>
                </tr>
                <tr align="center" id="row_Save" runat="server">
                    <td>
                        <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS" />
                        <asp:Button ID="btn_Update" runat="server" Text="Update" ToolTip="Update" CssClass="ButtonCSS" />
                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS" />
                    </td>
                </tr>
                <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                <asp:HiddenField ID="Hid_DpDetailsId" runat="server" />
                <asp:HiddenField ID="Hid_DmatId" runat="server" />
                <asp:HiddenField ID="Hid_Index" runat="server" />
                <asp:HiddenField ID="Hid_DealSlipId" runat="server" />
                <asp:HiddenField ID="Hid_CustDpId" runat="server" />
                <asp:HiddenField ID="Hid_DematInfoId" runat="server" />
                <asp:HiddenField ID="Hid_Id" runat="server" />
                <asp:HiddenField ID="Hid_RetValues" runat="server" />
                <asp:HiddenField ID="Hid_TransType" runat="server" />
                <asp:HiddenField ID="Hid_facevalue" runat="server" />
                <asp:HiddenField ID="Hid_FaceMultiple" runat="server" />
                <asp:HiddenField ID="Hid_BalanceFV" runat="server" />
                <asp:HiddenField ID="Hid_FVS" runat="server" />
                <asp:HiddenField ID="Hid_DematAccTo" runat="server" />
                <asp:HiddenField ID="Hid_PayMode" runat="server" />
                <asp:HiddenField ID="Hid_Remark" runat="server" />
                <asp:HiddenField ID="Hid_RowIndex" runat="server" />
                <asp:HiddenField ID="Hid_UserId" runat="server" />
                <asp:HiddenField ID="Hid_UserTypeId" runat="server" />
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
