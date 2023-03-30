<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DealPurchaseshow.aspx.vb"
    Inherits="Forms_DealPurchaseshow" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Select Deal Purchase</title>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <link href="../Include/CSS/jquery-ui.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.all.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.base.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.button.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.core.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Css/jquery.ui.theme.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>

    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>
    <style>
        .hidden {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function Validation() {
            document.getElementById("Hid_Id").value = "";
            var grid = document.getElementById("dg_Purdealno").children[0]

            var remainingamt = 0;
            var remainingFV = 0;

            for (i = 1; i <= (grid.children.length - 2) ; i++) {
                remainingamt = remainingamt + (grid.children[i].children[2].children[0].value - 0);
                remainingFV = (grid.children[i].children[2].children[0].value - 0);
            }
            if (remainingamt > (document.getElementById("Hid_RemainFV").value - 0)) {
                alert('Face value can not be more than ' + document.getElementById("Hid_RemainFV").value);
                document.getElementById("Hid_Id").value = "E";
                window.returnValue = document.getElementById("Hid_Id").value;
                window.close();
                return false;
            }

            for (i = 1; i <= (grid.children.length - 2) ; i++) {

                AdjremainingFV = (grid.children[i].children[2].children[0].value - 0);
                //                 alert(AdjremainingFV)  
                remainingFV = (grid.children[i].children[3].children[0].value - 0);
                strdealslipno = (grid.children[i].children[1].children[0].innerHTML);
                //                 alert(strdealslipno)    
                if ((AdjremainingFV > remainingFV) || (AdjremainingFV == 0)) {
                    alert('AdjustedFacevalue for ' + strdealslipno + ' cannot be zero or more then RemainingFaceValue ');
                    document.getElementById("Hid_Id").value = "E";
                    window.returnValue = document.getElementById("Hid_Id").value;
                    window.close();
                    return false;
                }
            }



            var grd = document.getElementById("dg_Purdealno")

            if (grd.children[0].children.length <= 2) {
                alert('Please Enter detail record.');
                document.getElementById("Hid_Id").value = "E";
                window.returnValue = document.getElementById("Hid_Id").value;
                window.close();
                return false;
            }

            if ((grd.children[0].children.length - 2) < 2) {
                alert('Please Enter atleast two detail record or select single purchase method');
                document.getElementById("Hid_Id").value = "E";
                window.returnValue = document.getElementById("Hid_Id").value;
                window.close();
                return false;
            }


            var grid = document.getElementById("dg_Purdealno").children[0]
            var remainingamt = 0;

            for (i = 1; i <= (grid.children.length - 2) ; i++) {
                //             alert(grid.children[i].children[2].children[0].innerHTML-0)
                remainingamt = remainingamt + (grid.children[i].children[2].children[0].value - 0);


            }
            //alert(remainingamt)

            if (remainingamt == "0") {
                alert('Remaining Face Value can not be zero.');
                document.getElementById("Hid_Id").value = "E";
                window.returnValue = document.getElementById("Hid_Id").value;
                window.close();
                return false;
            }

            var FaceValue = document.getElementById("Hid_Facevalue").value - 0
            var facevaluemultiple = document.getElementById("Hid_facevaluemultiple").value - 0
            var Facevalues = ((document.getElementById("Hid_Facevalue").value - 0) * (document.getElementById("Hid_facevaluemultiple").value - 0))
            //alert(Facevalues)
            var amt = Math.round((FaceValue * facevaluemultiple), 4)

            if (remainingamt < amt) {
                alert('Remaining Face value can not be less than Face value');

                document.getElementById("Hid_Id").value = "E";
                window.returnValue = document.getElementById("Hid_Id").value;
                window.close();
                return false;
            }
            var selected_val = Math.round(((document.getElementById("Hid_Facevalue").value - 0) * (document.getElementById("Hid_facevaluemultiple").value - 0)), 4)


            if (remainingamt != selected_val)
                //           if (remainingamt != ((document.getElementById("Hid_Facevalue").value -0 )*(document.getElementById("Hid_facevaluemultiple").value -0)))
            {
                alert('Remaining Face value should be same as Face value');
                document.getElementById("Hid_Id").value = "E";
                window.returnValue = document.getElementById("Hid_Id").value;
                window.close();
                return false;
            }

            RetValues();
        }









        function calculationRemainAmt() {

            var grid = document.getElementById("dg_Purdealno").children[0]
            var remainingamt = 0;
            var remainingFV = 0;

            for (i = 1; i <= (grid.children.length - 2) ; i++) {
                remainingamt = remainingamt + (grid.children[i].children[2].children[0].value - 0);
                remainingFV = (grid.children[i].children[2].children[0].value - 0);
            }
            if (remainingamt > (document.getElementById("Hid_RemainFV").value - 0)) {
                //alert('Face value can not be more than ' + document.getElementById("Hid_RemainFV").value);
                //return false;
            }

            var FaceValue = document.getElementById("Hid_Facevalue").value - 0
            var facevaluemultiple = document.getElementById("Hid_facevaluemultiple").value - 0
            var Facevalues = ((document.getElementById("Hid_Facevalue").value - 0) * (document.getElementById("Hid_facevaluemultiple").value - 0))
            if (remainingamt == "0") {
                document.getElementById("txt_Totremainamt").value = "0"
            }
            else {
                document.getElementById("txt_Totremainamt").value = remainingamt
            }
            if ((document.getElementById("txt_Totremainamt").value - 0) == "0") {
                (document.getElementById("txt_Balamt").value) = "0"
            }
            else {
                (document.getElementById("txt_Balamt").value) = (((document.getElementById("Hid_Facevalue").value - 0) * (document.getElementById("Hid_facevaluemultiple").value - 0)) - (document.getElementById("txt_Totremainamt").value - 0))
                document.getElementById("Hid_BalanceAmt").value = (((document.getElementById("Hid_Facevalue").value - 0) * (document.getElementById("Hid_facevaluemultiple").value - 0)) - (document.getElementById("txt_Totremainamt").value - 0))

            }
            if ((document.getElementById("txt_Totremainamt").value - 0) > ((document.getElementById("Hid_Facevalue").value - 0) * (document.getElementById("Hid_facevaluemultiple").value - 0))) {
                (document.getElementById("txt_Balamt").value) = "0"
            }





        }

        function ValidationRemove() {
            if (((document.getElementById("List_purdealno").value)) == "") {
                alert('Please Select Deal slip No which you want to remove');
                return false;
            }
        }

        function ValidationAdd() {
            if ((document.getElementById("srh_DealSlipNo_txt_Name").value) == "") {
                alert("Please Select Deal Slip No");
                return false;
            }





        }

        function ReturnValues(strReturn) {
            window.returnValue = strReturn;
            window.close();
        }

        function FinalClose() {
            window.returnValue = "";
            window.close();
        }

        function RetValues() {

            var strReturn = "";
            var selpurdealidValues = "";
            var selpurdealnoValues = "";
            var remainingamt = 0;
            var DealSlipIds = "";
            var DealSlipNos = "";
            var settlementdate = "";
            var DealDate = "";
            var grid = document.getElementById("dg_Purdealno").children[0]
            for (i = 1; i <= (grid.children.length - 2) ; i++) {
                DealSlipNos = DealSlipNos + (grid.children[i].children[1].children[0].innerHTML) + ","
                remainingamt = remainingamt + (grid.children[i].children[2].children[0].value - 0) + ","
                //                DealDate = DealDate + + (grid.children[i].children[5].children[0].innerHTML)+ ","

            }
            strReturn = strReturn + document.getElementById("Hid_DealSlipIds").value + "!"
            strReturn = strReturn + DealSlipNos + "!"
            strReturn = strReturn + remainingamt + "!"


            document.getElementById("Hid_Id").value = strReturn;
            window.returnValue = document.getElementById("Hid_Id").value + ":D";

            window.close();
        }


        function ShowPurDeal(intIndex, intDealSlipId) {
            var strpagename = "DealSlipEntry.aspx?DealSlipId=" + intDealSlipId;

            var Id = intDealSlipId
            ShowSecurityForm("DealSlipEntry.aspx", Id, "900px", "680px")
            return false
        }


        function ShowSecurityForm(PageName, Id, Width, Height) {
            var w = Width;
            var h = Height;
            var winl = (screen.width - w) / 2;
            var wint = (screen.height - h) / 2;
            if (winl < 0) winl = 0;
            if (wint < 0) wint = 0;
            var HideMenu = "HideMenu"
            PageName = PageName + "?Id=" + Id + "&Flag=C" + "&HideMenu=" + HideMenu
            //            alert('hi')
            windowprops = "height=" + h + ",width=" + w + ",top=" + wint + ",left=" + winl + ",location=no,"
            + "scrollbars=yes,menubar=yes,toolbar=yes,resizable=yes,status=yes";
            window.open(PageName, "Popup", windowprops);
        }

        function OpenForm() {

        }


    </script>

</head>
<body onunload="OpenForm()">
    <form id="form1" runat="server">
        <div>
            <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td class="HeaderCSS" align="center" colspan="2">Show Purchase Deal
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="middle" colspan="2">
                        <table id="Table2" align="center" cellspacing="0" cellpadding="0" border="1" width="100%">
                            <tr>
                                <td colspan="2">
                                    <table cellspacing="0" cellpadding="0" align="center">
                                        <tr id="row_BTB">
                                            <td class="LabelCSS">Select pur Deal Slip No:
                                            </td>
                                            <td align="left">
                                                <%-- <uc:Search ID="srh_DealSlipNo" runat="server" AutoPostback="false" ProcName="ID_SEARCH_BTBDealSlipNoNew1234"
                                                    CheckYearCompany="false" CheckCompany="true" SelectedFieldName="Dealdate" SourceType="StoredProcedure"
                                                    TableName="SecurityMaster" ConditionExist="true" ConditionalFieldName="SM.SecurityId"
                                                    FormHeight="400" FormWidth="850" ConditionalFieldId="Hid_SecurityId" Width="160"
                                                    ConditionalFieldName1="DSE.DealTransType" ConditionalFieldId1="Hid_DealTransType"></uc:Search>--%>
                                                <%--  <uc:Search ID="srh_DealSlipNo" runat="server" AutoPostback="false" ProcName="ID_SEARCH_BTBDealSlipNoOrdByDealdate"
                                                    CheckYearCompany="false" CheckCompany="true" SelectedFieldName="DealSlipNo" SourceType="StoredProcedure"
                                                    TableName="SecurityMaster" ConditionExist="true" ConditionalFieldName="SM.SecurityId"
                                                    FormHeight="420" FormWidth="800" ConditionalFieldId="Hid_SecurityId" Width="160"
                                                    ConditionalFieldName1="DSE.DealTransType" ConditionalFieldId1="Hid_DealTransType"></uc:Search>--%>

                                                <uc:Search ID="srh_DealSlipNo" runat="server" PageName="BTBDealSlipNoOrderByDealDate" AutoPostback="false"
                                                    SelectedFieldId="Id" SelectedFieldName="DealSlipNo" ConditionExist="true" ConditionalFieldId="Hid_SecurityId"
                                                    ConditionalFieldName="SecurityId" ConditionalFieldName1="DealTransType" ConditionalFieldId1="Hid_DealTransType"
                                                    CheckYearCompany="false" CheckCompany="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%--<tr id="row_BTB">
                                <td class="LabelCSS">
                                    Select pur Deal Slip No:
                                </td>
                                <td align="left">
                                    <uc:Search ID="srh_DealSlipNo" runat="server" AutoPostback="false" ProcName="ID_SEARCH_BTBDealSlipNoNew"
                                        CheckYearCompany="true" SelectedFieldName="DealSlipNo" SourceType="StoredProcedure"
                                        TableName="SecurityMaster" ConditionExist="true" ConditionalFieldName="DealTransType"
                                        FormHeight="380" FormWidth="800" ConditionalFieldId="Hid_DealTransType" Width="160"></uc:Search>
                                </td>
                            </tr>--%>
                            <%-- <tr id="row1" runat="server" visible="false">
                                <td class="LabelCSS">
                                    Pucahse Deal slip No:
                                </td>
                                <td align="left" colspan="3">
                                    <asp:ListBox ID="List_purdealno" runat="server" Width="191px" Height="50px" CssClass="ComboBoxCSS"
                                        TabIndex="14"></asp:ListBox>&nbsp;
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="SectionHeaderCSS" colspan="2" align="left">DEAL SLIP DETAILS
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Button ID="btn_Add" runat="server" CssClass="ButtonCSS" Text="Add Deal Slip"
                                        ToolTip="Add" TabIndex="11" Width="120px" />
                                    <asp:Button ID="btn_Showall" runat="server" CssClass="ButtonCSS" Text="Show All Stock"
                                        ToolTip="Show All Stock" TabIndex="11" Width="120px" />
                                </td>
                            </tr>
                            <tr>
                                <td id="td1" align="center" valign="top" runat="server" colspan="2">
                                    <div id="Div1" style="margin-top: 0px; overflow: auto; width: 800px; padding-top: 0px; position: relative; height: 180px"
                                        align="center">
                                        <asp:DataGrid ID="dg_Purdealno" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                            Width="800px" CssClass="GridCSS">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                            <FooterStyle HorizontalAlign="Center" CssClass="footer" VerticalAlign="Middle"></FooterStyle>
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="deletebtn" runat="server" ImageUrl="~/Images/delete.gif" CommandName="delete"
                                                            ToolTip="Delete" />
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealSlipNo" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_DealSlipNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DealSlipNo") %>'
                                                            ToolTip="Click to See View" Width="100px">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <%--     <ItemTemplate>
                                                        <asp:Label ID="lbl_DealSlipNo" Width="60px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DealSlipNo") %>'></asp:Label>
                                                    </ItemTemplate>--%>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="AdjustedFaceValue">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_RemainingFaceValue" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal(); "
                                                            onkeyup="calculationRemainAmt();" Width="100px" runat="server" Text='<%# container.dataitem("RemainingFaceValue") %>'></asp:TextBox>
                                                        <%-- <asp:Label ID="lbl_RemainingFaceValue" Width="60px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RemainingFaceValue") %>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="FaceValue">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_ActualFaceValue" ReadOnly="true" CssClass="TextBoxCSS" onkeypress="javascript:OnlyDecimal(); "
                                                            onkeyup="calculationRemainAmt();" Width="100px" runat="server" Text='<%# container.dataitem("FV") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="CustomerName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CustomerName" Width="150px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealDate" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DealDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SettlementDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SettlementDate" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SettlementDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_rate" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:Button ID="lbl_Remove" Width="80px" CssClass="ButtonCSS" Text="Remove" runat="server"
                                                            CommandName="RemoveMarking"></asp:Button>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DealSlipID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DealSlipID" Width="90px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DealSlipID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="right" VerticalAlign="Middle" Width="40px" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" align="left">
                                        <tr>
                                            <td class="LabelCSS">Total Remaining Amount:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Totremainamt" runat="server" Width="130px" Height="14px" CssClass="TextBoxCSS"
                                                    MaxLength="30" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table cellspacing="0" cellpadding="0" align="right">
                                        <tr>
                                            <td class="LabelCSS">Balance Amount:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_Balamt" runat="server" Width="130px" Height="14px" CssClass="TextBoxCSS"
                                                    MaxLength="30" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="">
                                <%-- <td class="LabelCSS">
                                </td>--%>
                                <td align="center" colspan="4">
                                    <asp:Label ID="lbl_Msg" runat="server" CssClass="LabelCSS" Text="" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Button ID="btn_Save1" runat="server" CssClass="ButtonCSS hidden" Text="Submit" ToolTip="Save"
                                        TabIndex="11" />
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Submit" ToolTip="Save" UseSubmitBehavior="true"
                                        TabIndex="11" OnClientClick="return Validation();" />
                                    <input type="button" id="btn_Save2" runat="server" class="ButtonCSS hidden" value="Submit 2" onclick="return RetValues();" />
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" UseSubmitBehavior="false"
                                        TabIndex="12" />
                                </td>
                            </tr>
                            <asp:HiddenField ID="Hid_DealTransType" runat="server" />
                            <asp:HiddenField ID="Hid_dealslipId" runat="server" />
                            <asp:HiddenField ID="Hid_PurchaseDealSlipId" runat="server" />
                            <asp:HiddenField ID="Hid_DealSlipNo" runat="server" />
                            <asp:HiddenField ID="Hid_DealSlipIds" runat="server" />
                            <asp:HiddenField ID="Hid_DealSlipNos" runat="server" />
                            <asp:HiddenField ID="Hid_Facevalue" runat="server" />
                            <asp:HiddenField ID="Hid_facevaluemultiple" runat="server" />
                            <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                            <asp:HiddenField ID="Hid_RemainingFaceValue" runat="server" />
                            <asp:HiddenField ID="Hid_RemainFV" runat="server" />
                            <asp:HiddenField ID="Hid_BalanceAmt" runat="server" />
                            <asp:HiddenField ID="Hid_PdealslipId" runat="server" />
                            <asp:HiddenField ID="Hid_SdealSlipId" runat="server" />
                            <asp:HiddenField ID="Hid_Id" runat="server" />
                            <asp:HiddenField ID="Hid_ShowId" runat="server" />
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
