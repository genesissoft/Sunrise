<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="TransferDPStock.aspx.cs" Inherits="Forms_TransferDPStock" Title="Transfer DP Stock" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" href="../Include/Style_IPO.css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript" src="../Include/Jquery_Vertical/jquery.js"></script>

    <script type="text/javascript" src="../Include/Jquery_Vertical/jquery-1.8.0.min.js"></script>

    <script type="text/javascript" src="../Include/Jquery_Vertical/ui/jquery.ui.core.js"></script>

    <script type="text/javascript" src="../Include/Jquery_Vertical/ui/jquery.ui.datepicker.js"></script>

    <script type="text/javascript" src="../Include/Jquery_Vertical/jqGrid/jquery-ui.js"></script>

    <link type="text/css" href="../Include/Jquery_Vertical/jqGrid/jquery-ui.css" rel="Stylesheet" />
    <link type="text/css" rel="Stylesheet" href="../Include/Jquery_Vertical/jqGrid/ui.jqgrid.css" />

    <script type="text/javascript" src="../Include/Jquery_Vertical/jqGrid/jquery.jqGrid.js"
        language="javascript"></script>

    <script type="text/javascript" src="../Include/Jquery_Vertical/jqGrid/grid.locale-en.js"
        language="javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".jsdate").datepicker({
                showOn: "button",
                buttonImage: "../Images/calendar.gif",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                buttonText: 'select date as (dd/mm/yyyy)'
            });
            $(".jsdate").prop('maxLength', 10);
        });

        function validation() {
            var obj = $("#<%=txtDated.ClientID %>");
            if (obj.val() == "") {
                alert("Please select the As on date first.");
                return false;
            }
            var obj = $("#ctl00_ContentPlaceHolder1_Srh_NameofSecurity_Hid_SelectedId");
            if (obj.val() == "" || obj.val() == "0") {
                AlertMessage("Validation", "Please select security name first.", 175, 450);
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTransfer_0").checked == true) {
                var obj = $("#<%=cboFromDPId.ClientID %>");
            if (obj.val() == "" || obj.val() == "0") {
                AlertMessage("Validation", "Please select the From DP first.", 175, 450);
                return false;
            }

            var obj = $("#<%=cboToDPId.ClientID %>");
             if (obj.val() == "" || obj.val() == "0") {
                 AlertMessage("Validation", "Please select the To DP first.", 175, 450);
                 return false;
             }
         }

         if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTransfer_1").checked == true) {
             var obj = $("#<%=cboFromDPId.ClientID %>");
            if (obj.val() == "" || obj.val() == "0") {
                AlertMessage("Validation", "Please select the From DP first.", 175, 450);
                return false;
            }

            var obj = $("#<%=cboToSGLId.ClientID %>");
             if (obj.val() == "" || obj.val() == "0") {
                 AlertMessage("Validation", "Please select the To SGL first.", 175, 450);
                 return false;
             }

         }
         if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTransfer_2").checked == true) {
             var obj = $("#<%=cboFromSGLId.ClientID %>");
            if (obj.val() == "" || obj.val() == "0") {
                AlertMessage("Validation", "Please select the From SGL first.", 175, 450);
                return false;
            }

            var obj = $("#<%=cboToDPId.ClientID %>");
             if (obj.val() == "" || obj.val() == "0") {
                 AlertMessage("Validation", "Please select the To DP first.", 175, 450);
                 return false;
             }

         }

         if ($("#<%=cboFromDPId.ClientID %>").val() == $("#<%=cboToDPId.ClientID %>").val()) {
             AlertMessage("Validation", "From DP & To DP should not be same.", 175, 450);
                return false;
            }
            var obj = $("#<%=txtTransferQty.ClientID %>");
            if (obj.val() == "" || obj.val() == 0) {
                AlertMessage("Validation", "Please enter quantity to tansfer first.", 175, 450);
                obj.focus();
                return false;
            }
            var stkqty = parseInt($("#<%=txtStkQty.ClientID %>").val());
        var transferqty = parseInt($("#<%=txtTransferQty.ClientID %>").val());
            //alert("stkQty: " + stkqty + "  transQty: " + transferqty);
            if (transferqty > stkqty) {
                AlertMessage("Validation", "Transfer quantity should not be greater than available stock quantity.", 175, 450);
                return false;
            }
        }
        function ReportType() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTransfer_0").checked == true) {

                document.getElementById("row_FromDP").style.display = "";
                document.getElementById("row_ToDP").style.display = "";
                document.getElementById("row_FromSGL").style.display = "none";
                document.getElementById("row_ToSGL").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_cboToSGLId").value = 0;
                document.getElementById("ctl00_ContentPlaceHolder1_cboFromSGLId").value = 0;
                document.getElementById("ctl00_ContentPlaceHolder1_txtTransferQty").value = 0;

            }


            if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTransfer_1").checked == true) {
                document.getElementById("row_FromDP").style.display = "";
                document.getElementById("row_ToSGL").style.display = "";
                document.getElementById("row_FromSGL").style.display = "none";
                document.getElementById("row_ToDP").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_cboToDPId").value = 0;
                document.getElementById("ctl00_ContentPlaceHolder1_cboFromSGLId").value = 0;
                document.getElementById("ctl00_ContentPlaceHolder1_txtTransferQty").value = 0;
            }


            if (document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTransfer_2").checked == true) {
                document.getElementById("row_FromSGL").style.display = "";
                document.getElementById("row_ToDP").style.display = "";
                document.getElementById("row_FromDP").style.display = "none";
                document.getElementById("row_ToSGL").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_cboFromDPId").value = 0;
                document.getElementById("ctl00_ContentPlaceHolder1_cboToSGLId").value = 0;
                document.getElementById("ctl00_ContentPlaceHolder1_txtTransferQty").value = 0;

            }




        }
    </script>

    <asp:UpdatePanel ID="Panel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="data_table">
                <tr>
                    <td class="HeaderCSS" align="center">Transfer DP Stock
                    </td>
                </tr>
                <tr align="center" valign="top">
                    <td>
                        <table cellpadding="0" cellspacing="0" width="70%">
                            <tr align="left">
                                <td>Type OF Transfer:
                                </td>
                                <td style="padding-left: 0px;">
                                    <asp:RadioButtonList RepeatLayout="Flow" ID="rbl_TypeOFTransfer" runat="server"
                                        CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" RepeatColumns="5"
                                        AutoPostBack="true">
                                        <asp:ListItem Selected="True" Value="DPToDP">DP To DP</asp:ListItem>
                                        <asp:ListItem Value="DPToSGL">DP To SGL</asp:ListItem>
                                        <asp:ListItem Value="SGLToDP">SGL TO DP</asp:ListItem>

                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>As on date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDated" runat="server" CssClass="text_box1 jsdate" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Security Name:
                                </td>
                                <td style="padding: 0px">
                                    <%-- <uc:Search ID="Srh_NameofSecurity" runat="server" AutoPostback="false" ProcName="ID_SEARCH_SecurityName"
                                        SelectedFieldName="SecurityName" SourceType="StoredProcedure" TableName="SecurityMaster"
                                        ConditionalFieldName="SecurityIssuer" FormWidth="800" FormHeight="320" ConditionalFieldId="srh_IssuerOfSecurity"
                                        ConditionExist="true" Width="180" ConditionalFieldId1="rdo_RedeemedSec" ConditionalFieldName1="RedeemedDeal"></uc:Search>--%>
                                    <uc:Search ID="Srh_NameofSecurity" runat="server" PageName="NameOfSecurity" AutoPostback="true"
                                        SelectedFieldId="Id" SelectedFieldName="SecurityName" />
                                </td>
                            </tr>
                            <tr align="left" id="row_FromDP">
                                <td>Select From DP:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cboFromDPId" runat="server" CssClass="combo1" AutoPostBack="true"
                                        OnSelectedIndexChanged="cboFromDPId_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr align="left" id="row_FromSGL">
                                <td>Select From SGL:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cboFromSGLId" runat="server" CssClass="combo1" AutoPostBack="true" OnSelectedIndexChanged="cboFromSGLId_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr align="left" id="row_ToDP">
                                <td>Select To DP:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cboToDPId" runat="server" CssClass="combo1" AutoPostBack="true"
                                        OnSelectedIndexChanged="cboFromDPId_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr align="left" id="row_ToSGL">
                                <td>Select To SGL:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cboToSGLId" runat="server" CssClass="combo1">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Available Stock Qty:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStkQty" runat="server" CssClass="text_box1" Width="10em" onkeypress="OnlyInteger();"
                                        Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Enter Quantity to Transfer:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransferQty" runat="server" CssClass="text_box1" Width="10em"
                                        onkeypress="OnlyInteger();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <td></td>
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" CssClass="ButtonCSS" OnClientClick="return validation();"
                                        OnClick="btnSave_Click" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CssClass="ButtonCSS" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="Hid_Id" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
