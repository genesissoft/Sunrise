<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="CustomerTypeMaster.aspx.vb" Inherits="Forms_CustomerTypeMaster" Title="Customer Type" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">

        function SelectDocType() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DocType_0").checked == true) {
                document.getElementById("tr_Emp").style.display = "none"
                document.getElementById("tr_Kyc").style.display = "none"

            }
            else if (document.getElementById("ctl00_ContentPlaceHolder1_rdo_DocType_1").checked == true) {
                document.getElementById("tr_Emp").style.display = "none"
                document.getElementById("tr_Kyc").style.display = "none"

            }
            else {
                document.getElementById("tr_Emp").style.display = "none"
                document.getElementById("tr_Kyc").style.display = "none"

            }
        }

        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_CustomerType").value) == "") {
                AlertMessage('Validation', 'Please Enter Customer Type', 175, 450);
                return false;
            }
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="SectionHeaderCSS">Customer Type Master</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="50%">
                            <tr align="left" runat="server">
                                <td>Category:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cbo_Category" runat="server" CssClass="ComboBoxCSS" Width="180px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Customer Type:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_CustomerType" runat="server" Width="180px" CssClass="TextBoxCSS"></asp:TextBox><span
                                        style="color: #ff0000"><em style="vertical-align: super;">*</em></span>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Customer Abbreviation:</td>
                                <td align="left">
                                    <asp:TextBox ID="txt_CustomerAbbr" runat="server" CssClass="TextBoxCSS" TabIndex="3"
                                        ReadOnly="false"></asp:TextBox></td>
                            </tr>
                            <tr align="left" style ="display :none ">
                                <td>Document Type:
                                </td>
                                <td align="left" style="padding-left: 0px;">
                                    <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DocType" runat="server" CellPadding="0"
                                        CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="false">
                                        <asp:ListItem Selected="true" Value="B">Both</asp:ListItem>
                                        <asp:ListItem Value="E"> Empanelment </asp:ListItem>
                                        <asp:ListItem Value="K"> KYC </asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr align="left" id="tr_Emp">
                                <td>Empanelment Documents Submitted:</td>
                                <td style="padding-left: 0px;">
                                    <uc:SelectFields ID="srh_EmpalementDocuments" runat="server" ProcName="ID_SEARCH_DocumentEmpTypeMaster"
                                        FormHeight="470" FormWidth="257" SelectedValueName="DocumentId" ChkLabelName=""
                                        ConditionExist="true" ShowAll="true" LabelName="" SelectedFieldName="DocumentName"
                                        SourceType="StoredProcedure" Visible="true" ShowLabel="false"></uc:SelectFields>
                                </td>
                            </tr>
                            <tr align="left" id="tr_Kyc">
                                <td>KYC Documents Submitted:</td>
                                <td style="padding-left: 0px;">
                                    <uc:SelectFields ID="srh_KYCDocuments" runat="server" ProcName="ID_SEARCH_DocumentKycTypeMaster123"
                                        FormHeight="470" FormWidth="257" SelectedValueName="DocumentId" ChkLabelName=""
                                        ConditionExist="true" ShowAll="true" LabelName="" SelectedFieldName="DocumentName"
                                        SourceType="StoredProcedure" Visible="true" ShowLabel="false"></uc:SelectFields>
                                </td>
                            </tr>
                            <tr align="left">
                                <td class="SeperatorRowCSS" colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                                    <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Back" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
