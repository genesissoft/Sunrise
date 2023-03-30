<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="AgingInventory.aspx.cs" Inherits="Forms_AgingInventory" Title="Aging Inventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" href="../Include/Style_IPO.css" rel="stylesheet" />

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

    <script type="text/javascript" src="../Include/Common.js"></script>

    <style type="text/css">
        #ui-datepicker-div
        {
            z-index: 10;
        }
        #gbox_jqSelldownTrackingDetails
        {
            z-index: 0;
        }
        .data_table td
        {
            padding: 3px;
        }
    </style>

    <script type="text/javascript" language="javascript">
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
        
        $("#divTabs").tabs();
        FillAgingInventory();
        
        $('#<%=cboSellDownCompanyName.ClientID %>,#<%=txtSellDownFromDate.ClientID %>,#<%=txtSellDownToDate.ClientID %>').change(function () {
            $("#jqSelldownTrackingDetails").setGridParam({ url: "getdata.ashx?pagename=SelldownTracking&fromdate=" + $("#<%= txtSellDownFromDate.ClientID %>").val() + "&todate=" + $("#<%= txtSellDownToDate.ClientID %>").val() + "&tempcompid=" + $("#<%= cboSellDownCompanyName.ClientID %>").val(), page: 1 });
            $("#jqSelldownTrackingDetails").trigger("reloadGrid");
        });
        
        $("#jqSelldownTrackingDetails").jqGrid({
            url: "getdata.ashx?pagename=SelldownTracking&fromdate=" + $("#<%= txtSellDownFromDate.ClientID %>").val() + "&todate=" + $("#<%= txtSellDownToDate.ClientID %>").val() + "&tempcompid=" + $("#<%= cboSellDownCompanyName.ClientID %>").val(),
            datatype: "json",
            colNames: ['SN','Company Name','Security Name','ISIN','Deal No','Deal Date','Sett Date','FaceValue','Pur Consideration','Curr Consideration','MTM','Holding Since','Holding Days','Stock Type'],
            colModel: [
                { name: 'row', index: 'row', width: 20, sorttype: 'int', align: 'center', sortable: false, search: false },
                { name: 'CompanyName', index: 'CompanyName', width: 200, sorttype: 'text', sortable: false, search: false },
                { name: 'SecurityName', index: 'SecurityName', width: 300, sorttype: 'text', sortable: false, search: false},
                { name: 'ISIN', index: 'ISIN', width: 100, sorttype: 'text', sortable: false, search: false},
                { name: 'DealNo', index: 'DealNo', width: 100, sorttype: 'text', sortable: false, search: false},
                { name: 'DealDate', index: 'DealDate', width: 90, sorttype: 'date', align:'center', sortable: false, search: false},
                { name: 'SettlementDate', index: 'SettlementDate', width: 90, sorttype: 'date', align:'center', sortable: false, search: false},
                { name: 'FaceValue', index: 'FaceValue', width: 100, sorttype: 'float',align:'right', sortable: false, search:false },
                { name: 'PurConsideration', index: 'PurConsideration', width: 100, sorttype: 'float',align:'right', sortable: false, search: false},
                { name: 'CurrentConsideration', index: 'CurrentConsideration', width: 100, sorttype: 'float',align:'right', sortable: false, search: false},
                { name: 'MTM', index: 'MTM', width: 100, sorttype: 'float',align:'right', sortable: false, search: false},
                { name: 'HoldingSince', index: 'HoldingSince', width: 90, sorttype: 'date',align:'center', sortable: false, search: false},
                { name: 'HoldingDays', index: 'HoldingDays', width: 90, sortable: false, search: false},
                { name: 'StockType', index: 'StockType', width: 90,sortable: false, search: false}
            ],
            viewrecords: true,
            shrinkToFit: false,
            width:$("#tdWidth").width()-20,
            height: 400,
            rowNum: 1000,
            loadonce: false,
            pager: "#jqSelldownTrackingDetailsPager"
        });
            
        $('#jqSelldownTrackingDetails').navGrid("#jqSelldownTrackingDetailsPager", { search: false, add: false, edit: false, del: false, refresh: true },
        {}, // edit options
        {}, // add options
        {}, // delete options
        { multipleSearch: true, closeOnEscape: true, closeAfterSearch: true }
        );
    });
    
    function FillAgingInventory(){
        
        if ($("#<%= Hid_GSecExposure.ClientID %>").val() !="")
        {
            var objGSec=eval($("#<%= Hid_GSecExposure.ClientID %>").val());
            if (objGSec.length>0){
                objGSec=objGSec[0];
                $("#tdFV1").text($.isNumeric(objGSec.FV1) ? objGSec.FV1.toFixed(2) : "-");
                $("#tdFV2").text($.isNumeric(objGSec.FV2) ? objGSec.FV2.toFixed(2) : "-");
                $("#tdFV3").text($.isNumeric(objGSec.FV3) ? objGSec.FV3.toFixed(2) : "-");
                $("#tdFV4").text($.isNumeric(objGSec.FV4) ? objGSec.FV4.toFixed(2) : "-");
                $("#tdFV5").text($.isNumeric(objGSec.FV5) ? objGSec.FV5.toFixed(2) : "-");
            
                $("#tdPC1").text($.isNumeric(objGSec.PC1) ? objGSec.PC1.toFixed(2) : "-");
                $("#tdPC2").text($.isNumeric(objGSec.PC2) ? objGSec.PC2.toFixed(2) : "-");
                $("#tdPC3").text($.isNumeric(objGSec.PC3) ? objGSec.PC3.toFixed(2) : "-");
                $("#tdPC4").text($.isNumeric(objGSec.PC4) ? objGSec.PC4.toFixed(2) : "-");
                $("#tdPC5").text($.isNumeric(objGSec.PC5) ? objGSec.PC5.toFixed(2) : "-");
               
                $("#tdQuantity1").text($.isNumeric(objGSec.Quantity1)? objGSec.Quantity1.toFixed(2) : "-");
                $("#tdQuantity2").text($.isNumeric(objGSec.Quantity2) ? objGSec.Quantity2.toFixed(2) : "-");
                $("#tdQuantity3").text($.isNumeric(objGSec.Quantity3) ? objGSec.Quantity3.toFixed(2) : "-");
                $("#tdQuantity4").text($.isNumeric(objGSec.Quantity4) ? objGSec.Quantity4.toFixed(2) : "-");
                $("#tdQuantity5").text($.isNumeric(objGSec.Quantity5) ? objGSec.Quantity5.toFixed(2) : "-");
            
                $("#tdMTM1").text($.isNumeric(objGSec.MTM1) ? objGSec.MTM1.toFixed(2) : "-");
                $("#tdMTM2").text($.isNumeric(objGSec.MTM2) ? objGSec.MTM2.toFixed(2) : "-");
                $("#tdMTM3").text($.isNumeric(objGSec.MTM3)? objGSec.MTM3.toFixed(2) : "-");
                $("#tdMTM4").text($.isNumeric(objGSec.MTM4) ? objGSec.MTM4.toFixed(2) : "-");
                $("#tdMTM5").text($.isNumeric(objGSec.MTM5) ? objGSec.MTM5.toFixed(2) : "-");
              
                $("#tdPV011").text($.isNumeric(objGSec.PV011) ? objGSec.PV011.toFixed(2) : "-");
                $("#tdPV012").text($.isNumeric(objGSec.PV012) ? objGSec.PV012.toFixed(2) : "-");
                $("#tdPV013").text($.isNumeric(objGSec.PV013) ? objGSec.PV013.toFixed(2) : "-");
                $("#tdPV014").text($.isNumeric(objGSec.PV014) ? objGSec.PV014.toFixed(2) : "-");
                $("#tdPV015").text($.isNumeric(objGSec.PV015) ? objGSec.PV015.toFixed(2) : "-");
                
                $("#tdTC1").text($.isNumeric(objGSec.TC1) ? objGSec.TC1.toFixed(2) : "-");
                $("#tdTC2").text($.isNumeric(objGSec.TC2) ? objGSec.TC2.toFixed(2) : "-");
                $("#tdTC3").text($.isNumeric(objGSec.TC3) ? objGSec.TC3.toFixed(2) : "-");
                $("#tdTC4").text($.isNumeric(objGSec.TC4) ? objGSec.TC4.toFixed(2) : "-");
                $("#tdTC5").text($.isNumeric(objGSec.TC5) ? objGSec.TC5.toFixed(2) : "-");
            
                $("#tdCC1").text($.isNumeric(objGSec.CC1) ? objGSec.CC1.toFixed(2) : "-");
                $("#tdCC2").text($.isNumeric(objGSec.CC2) ? objGSec.CC2.toFixed(2) : "-");
                $("#tdCC3").text($.isNumeric(objGSec.CC3) ? objGSec.CC3.toFixed(2) : "-");
                $("#tdCC4").text($.isNumeric(objGSec.CC4) ? objGSec.CC4.toFixed(2) : "-");
                $("#tdCC5").text($.isNumeric(objGSec.CC5)? objGSec.CC5.toFixed(2) : "-");

                $("#tdCTC1").text($.isNumeric(objGSec.CTC1) ? objGSec.CTC1.toFixed(2) : "-");
                $("#tdCTC2").text($.isNumeric(objGSec.CTC2) ? objGSec.CTC2.toFixed(2) : "-");
                $("#tdCTC3").text($.isNumeric(objGSec.CTC3) ? objGSec.CTC3.toFixed(2) : "-");
                $("#tdCTC4").text($.isNumeric(objGSec.CTC4) ? objGSec.CTC4.toFixed(2) : "-");
                $("#tdCTC5").text($.isNumeric(objGSec.CTC5) ? objGSec.CTC5.toFixed(2) : "-");

                $("#tdCR1").text($.isNumeric(objGSec.CR1) ? objGSec.CR1.toFixed(2) : "-");
                $("#tdCR2").text($.isNumeric(objGSec.CR2) ? objGSec.CR2.toFixed(2) : "-");
                $("#tdCR3").text($.isNumeric(objGSec.CR3) ? objGSec.CR3.toFixed(2) : "-");
                $("#tdCR4").text($.isNumeric(objGSec.CR4) ? objGSec.CR4.toFixed(2) : "-");
                $("#tdCR5").text($.isNumeric(objGSec.CR5) ? objGSec.CR5.toFixed(2) : "-");
            }
        }
        
        if ($("#<%= Hid_NonGSecExposure.ClientID %>").val() !="")
        {
            var objNonGSec=eval($("#<%= Hid_NonGSecExposure.ClientID %>").val());
            if (objNonGSec.length>0){
                objNonGSec=objNonGSec[0];
                $("#tdNonGSecFV1").text($.isNumeric(objNonGSec.FV1) ? objNonGSec.FV1.toFixed(2) : "-");
                $("#tdNonGSecFV2").text($.isNumeric(objNonGSec.FV2) ? objNonGSec.FV2.toFixed(2) : "-");
                $("#tdNonGSecFV3").text($.isNumeric(objNonGSec.FV3) ? objNonGSec.FV3.toFixed(2) : "-");
                $("#tdNonGSecFV4").text($.isNumeric(objNonGSec.FV4) ? objNonGSec.FV4.toFixed(2) : "-");
                $("#tdNonGSecFV5").text($.isNumeric(objNonGSec.FV5) ? objNonGSec.FV5.toFixed(2) : "-");
                
                $("#tdNonGSecPC1").text($.isNumeric(objNonGSec.PC1) ? objNonGSec.PC1.toFixed(2) : "-");
                $("#tdNonGSecPC2").text($.isNumeric(objNonGSec.PC2) ? objNonGSec.PC2.toFixed(2) : "-");
                $("#tdNonGSecPC3").text($.isNumeric(objNonGSec.PC3) ? objNonGSec.PC3.toFixed(2) : "-");
                $("#tdNonGSecPC4").text($.isNumeric(objNonGSec.PC4) ? objNonGSec.PC4.toFixed(2) : "-");
                $("#tdNonGSecPC5").text($.isNumeric(objNonGSec.PC5) ? objNonGSec.PC5.toFixed(2) : "-");
                
                $("#tdNonGSecQuantity1").text($.isNumeric(objNonGSec.Quantity1) ? objNonGSec.Quantity1.toFixed(2) : "-");
                $("#tdNonGSecQuantity2").text($.isNumeric(objNonGSec.Quantity2) ? objNonGSec.Quantity2.toFixed(2) : "-");
                $("#tdNonGSecQuantity3").text($.isNumeric(objNonGSec.Quantity3) ? objNonGSec.Quantity3.toFixed(2) : "-");
                $("#tdNonGSecQuantity4").text($.isNumeric(objNonGSec.Quantity4) ? objNonGSec.Quantity4.toFixed(2) : "-");
                $("#tdNonGSecQuantity5").text($.isNumeric(objNonGSec.Quantity5) ? objNonGSec.Quantity5.toFixed(2) : "-");
                
                $("#tdNonGSecMTM1").text($.isNumeric(objNonGSec.MTM1) ? objNonGSec.MTM1.toFixed(2) : "-");
                $("#tdNonGSecMTM2").text($.isNumeric(objNonGSec.MTM2) ? objNonGSec.MTM2.toFixed(2) : "-");
                $("#tdNonGSecMTM3").text($.isNumeric(objNonGSec.MTM3) ? objNonGSec.MTM3.toFixed(2) : "-");
                $("#tdNonGSecMTM4").text($.isNumeric(objNonGSec.MTM4) ? objNonGSec.MTM4.toFixed(2) : "-");
                $("#tdNonGSecMTM5").text($.isNumeric(objNonGSec.MTM5) ? objNonGSec.MTM5.toFixed(2) : "-");
              
                $("#tdNonGSecPV011").text($.isNumeric(objNonGSec.PV011)  ? objNonGSec.PV011.toFixed(2) : "-");
                $("#tdNonGSecPV012").text($.isNumeric(objNonGSec.PV012)  ? objNonGSec.PV012.toFixed(2) : "-");
                $("#tdNonGSecPV013").text($.isNumeric(objNonGSec.PV013)  ? objNonGSec.PV013.toFixed(2) : "-");
                $("#tdNonGSecPV014").text($.isNumeric(objNonGSec.PV014)  ? objNonGSec.PV014.toFixed(2) : "-");
                $("#tdNonGSecPV015").text($.isNumeric(objNonGSec.PV015)  ? objNonGSec.PV015.toFixed(2) : "-");
                
                $("#tdNonGSecTC1").text($.isNumeric(objNonGSec.TC1) ? objNonGSec.TC1.toFixed(2) : "-");
                $("#tdNonGSecTC2").text($.isNumeric(objNonGSec.TC2) ? objNonGSec.TC2.toFixed(2) : "-");
                $("#tdNonGSecTC3").text($.isNumeric(objNonGSec.TC3) ? objNonGSec.TC3.toFixed(2) : "-");
                $("#tdNonGSecTC4").text($.isNumeric(objNonGSec.TC4)? objNonGSec.TC4.toFixed(2) : "-");
                $("#tdNonGSecTC5").text($.isNumeric(objNonGSec.TC5) ? objNonGSec.TC5.toFixed(2) : "-");
                
                $("#tdNonGSecCC1").text($.isNumeric(objNonGSec.CC1) ? objNonGSec.CC1.toFixed(2) : "-");
                $("#tdNonGSecCC2").text($.isNumeric(objNonGSec.CC2)? objNonGSec.CC2.toFixed(2) : "-");
                $("#tdNonGSecCC3").text($.isNumeric(objNonGSec.CC3) ? objNonGSec.CC3.toFixed(2) : "-");
                $("#tdNonGSecCC4").text($.isNumeric(objNonGSec.CC4) ? objNonGSec.CC4.toFixed(2) : "-");
                $("#tdNonGSecCC5").text($.isNumeric(objNonGSec.CC5) ? objNonGSec.CC5.toFixed(2) : "-");

                $("#tdNonGSecCTC1").text($.isNumeric(objNonGSec.CTC1) ? objNonGSec.CTC1.toFixed(2) : "-");
                $("#tdNonGSecCTC2").text($.isNumeric(objNonGSec.CTC2) ? objNonGSec.CTC2.toFixed(2) : "-");
                $("#tdNonGSecCTC3").text($.isNumeric(objNonGSec.CTC3) ? objNonGSec.CTC3.toFixed(2) : "-");
                $("#tdNonGSecCTC4").text($.isNumeric(objNonGSec.CTC4) ? objNonGSec.CTC4.toFixed(2) : "-");
                $("#tdNonGSecCTC5").text($.isNumeric(objNonGSec.CTC5) ? objNonGSec.CTC5.toFixed(2) : "-");

                $("#tdNonGSecCR1").text($.isNumeric(objNonGSec.CR1) ? objNonGSec.CR1.toFixed(2) : "-");
                $("#tdNonGSecCR2").text($.isNumeric(objNonGSec.CR2) ? objNonGSec.CR2.toFixed(2) : "-");
                $("#tdNonGSecCR3").text($.isNumeric(objNonGSec.CR3)? objNonGSec.CR3.toFixed(2) : "-");
                $("#tdNonGSecCR4").text($.isNumeric(objNonGSec.CR4) ? objNonGSec.CR4.toFixed(2) : "-");
                $("#tdNonGSecCR5").text($.isNumeric(objNonGSec.CR5) ? objNonGSec.CR5.toFixed(2) : "-");
            }
        }
    }
    
    function ExportDetails(type){
        if(type=='A')
        window.location.href = 'ShowDocument.aspx?Type=AgingInventory&fromdate=' + $("#<%= txtFromDate.ClientID %>").val() + '&todate=' + $("#<%= txtToDate.ClientID %>").val() + '&compid=' + $("#<%= cboCompanyName.ClientID %>").val() ;
        else if(type=='S')
        window.location.href = 'ShowDocument.aspx?Type=SellDownTracking&fromdate=' + $("#<%= txtSellDownFromDate.ClientID %>").val() + '&todate=' + $("#<%= txtSellDownToDate.ClientID %>").val() + '&compid=' + $("#<%= cboSellDownCompanyName.ClientID %>").val() ;
        else if(type=='D')
        window.location.href = 'ShowDocument.aspx?Type=CompanyStockDetails&fromdate=' + $("#<%= txtStockFromDate.ClientID %>").val() + '&todate=' + $("#<%= txtStockToDate.ClientID %>").val() + '&compid=' + $("#<%= cboStockCompanyName.ClientID %>").val() ;
    }
    </script>

    <table cellspacing="0" cellpadding="0" width="100%" class="data_table">
        <tr>
            <td class="HeaderCSS" align="center">
                Aging Inventory
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr align="center" valign="top">
                        <td id="tdWidth">
                            <div id="divTabs">
                                <ul>
                                    <li><a href="#AgingInventory">Aging Inventory</a></li>
                                    <li><a href="#SellDownTracking">Sell Down Tracking</a></li>
                                <%--    <li><a href="#StockDetails">Stock Details</a></li>--%>
                                </ul>
                                <div id="AgingInventory" style="padding: 0.5em 0.3em;">
                                    <table cellpadding="0" cellspacing="0" style="text-align: left;">
                                        <tr align="left">
                                            <td>
                                                Dated:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="text_box1 jsdate required"></asp:TextBox>
                                            </td>
                                            <td>
                                                Company Name:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cboCompanyName" runat="server" CssClass="combo1" Width="15em" AutoPostBack ="true"  OnSelectedIndexChanged="cboCompanyName_SelectedIndexChanged">
                                                   
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <input type="button" id="btnInventory" value=" Export " class="frmButton" onclick="javascript:ExportDetails('A')" />
                                                <asp:Button ID="btnCalculate" runat="server" Text=" Calculate " CssClass="frmButton"
                                                    OnClick="btnCalculate_Click" Visible="false" />
                                            </td>
                                            <td class="hide">
                                                From Date:
                                            </td>
                                            <td class="hide">
                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="text_box1 jsdate required"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="0" cellspacing="0" style="width: 100%;" class="table_border_right_bottom tablerowbg">
                                        <tr align="left" class="table_heading">
                                            <td style="width: 20%;">
                                                Holding Time
                                            </td>
                                            <td style="width: 16%;">
                                                Less Than 1 Wk
                                            </td>
                                            <td style="width: 16%;">
                                                >1 Wk - < 1 Mon
                                            </td>
                                            <td style="width: 16%;">
                                                >1 Mon - < 3 Mon
                                            </td>
                                            <td style="width: 16%;">
                                                >3 Mon - < 6 Mon
                                            </td>
                                            <td style="width: 16%;">
                                                Greater Than 6 Mon
                                            </td>
                                        </tr>
                                        <tr align="center" style="background-color: #F2F5BF;">
                                            <td colspan="6" class="bold">
                                                G - Sec
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Rates (Face Value)
                                            </td>
                                            <td id="tdFV1">
                                            </td>
                                            <td id="tdFV2">
                                            </td>
                                            <td id="tdFV3">
                                            </td>
                                            <td id="tdFV4">
                                            </td>
                                            <td id="tdFV5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Rates (Pur Consideration)
                                            </td>
                                            <td id="tdPC1">
                                            </td>
                                            <td id="tdPC2">
                                            </td>
                                            <td id="tdPC3">
                                            </td>
                                            <td id="tdPC4">
                                            </td>
                                            <td id="tdPC5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Rates (Quantity)
                                            </td>
                                            <td id="tdQuantity1">
                                            </td>
                                            <td id="tdQuantity2">
                                            </td>
                                            <td id="tdQuantity3">
                                            </td>
                                            <td id="tdQuantity4">
                                            </td>
                                            <td id="tdQuantity5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Rates (MTM)
                                            </td>
                                            <td id="tdMTM1">
                                            </td>
                                            <td id="tdMTM2">
                                            </td>
                                            <td id="tdMTM3">
                                            </td>
                                            <td id="tdMTM4">
                                            </td>
                                            <td id="tdMTM5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                PV01
                                            </td>
                                            <td id="tdPV011" style="color: Red;">
                                            </td>
                                            <td id="tdPV012" style="color: Red;">
                                            </td>
                                            <td id="tdPV013" style="color: Red;">
                                            </td>
                                            <td id="tdPV014" style="color: Red;">
                                            </td>
                                            <td id="tdPV015" style="color: Red;">
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td colspan="6">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Total Consideration
                                            </td>
                                            <td id="tdTC1">
                                            </td>
                                            <td id="tdTC2">
                                            </td>
                                            <td id="tdTC3">
                                            </td>
                                            <td id="tdTC4">
                                            </td>
                                            <td id="tdTC5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Current Consideration
                                            </td>
                                            <td id="tdCC1">
                                            </td>
                                            <td id="tdCC2">
                                            </td>
                                            <td id="tdCC3">
                                            </td>
                                            <td id="tdCC4">
                                            </td>
                                            <td id="tdCC5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Current Total Consideration
                                            </td>
                                            <td id="tdCTC1">
                                            </td>
                                            <td id="tdCTC2">
                                            </td>
                                            <td id="tdCTC3">
                                            </td>
                                            <td id="tdCTC4">
                                            </td>
                                            <td id="tdCTC5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Coupon Received
                                            </td>
                                            <td id="tdCR1">
                                            </td>
                                            <td id="tdCR2">
                                            </td>
                                            <td id="tdCR3">
                                            </td>
                                            <td id="tdCR4">
                                            </td>
                                            <td id="tdCR5">
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td colspan="6">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr align="center" style="background-color: #F2F5BF;">
                                            <td colspan="6" class="bold">
                                                Non G - Sec
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Credit (Face Value)
                                            </td>
                                            <td id="tdNonGSecFV1">
                                            </td>
                                            <td id="tdNonGSecFV2">
                                            </td>
                                            <td id="tdNonGSecFV3">
                                            </td>
                                            <td id="tdNonGSecFV4">
                                            </td>
                                            <td id="tdNonGSecFV5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Credit (Pur Consideration)
                                            </td>
                                            <td id="tdNonGSecPC1">
                                            </td>
                                            <td id="tdNonGSecPC2">
                                            </td>
                                            <td id="tdNonGSecPC3">
                                            </td>
                                            <td id="tdNonGSecPC4">
                                            </td>
                                            <td id="tdNonGSecPC5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Credit (Quantity)
                                            </td>
                                            <td id="tdNonGSecQuantity1">
                                            </td>
                                            <td id="tdNonGSecQuantity2">
                                            </td>
                                            <td id="tdNonGSecQuantity3">
                                            </td>
                                            <td id="tdNonGSecQuantity4">
                                            </td>
                                            <td id="tdNonGSecQuantity5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Credit (MTM)
                                            </td>
                                            <td id="tdNonGSecMTM1">
                                            </td>
                                            <td id="tdNonGSecMTM2">
                                            </td>
                                            <td id="tdNonGSecMTM3">
                                            </td>
                                            <td id="tdNonGSecMTM4">
                                            </td>
                                            <td id="tdNonGSecMTM5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                PV01
                                            </td>
                                            <td id="tdNonGSecPV011" style="color: Red;">
                                            </td>
                                            <td id="tdNonGSecPV012" style="color: Red;">
                                            </td>
                                            <td id="tdNonGSecPV013" style="color: Red;">
                                            </td>
                                            <td id="tdNonGSecPV014" style="color: Red;">
                                            </td>
                                            <td id="tdNonGSecPV015" style="color: Red;">
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td colspan="6">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Total Consideration
                                            </td>
                                            <td id="tdNonGSecTC1">
                                            </td>
                                            <td id="tdNonGSecTC2">
                                            </td>
                                            <td id="tdNonGSecTC3">
                                            </td>
                                            <td id="tdNonGSecTC4">
                                            </td>
                                            <td id="tdNonGSecTC5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Current Consideration
                                            </td>
                                            <td id="tdNonGSecCC1">
                                            </td>
                                            <td id="tdNonGSecCC2">
                                            </td>
                                            <td id="tdNonGSecCC3">
                                            </td>
                                            <td id="tdNonGSecCC4">
                                            </td>
                                            <td id="tdNonGSecCC5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Current Total Consideration
                                            </td>
                                            <td id="tdNonGSecCTC1">
                                            </td>
                                            <td id="tdNonGSecCTC2">
                                            </td>
                                            <td id="tdNonGSecCTC3">
                                            </td>
                                            <td id="tdNonGSecCTC4">
                                            </td>
                                            <td id="tdNonGSecCTC5">
                                            </td>
                                        </tr>
                                        <tr align="right">
                                            <td align="left">
                                                Coupon Received
                                            </td>
                                            <td id="tdNonGSecCR1">
                                            </td>
                                            <td id="tdNonGSecCR2">
                                            </td>
                                            <td id="tdNonGSecCR3">
                                            </td>
                                            <td id="tdNonGSecCR4">
                                            </td>
                                            <td id="tdNonGSecCR5">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="SellDownTracking" style="padding: 0.5em 0.3em;">
                                    <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr align="center" valign="top">
                                            <td>
                                                <table cellpadding="0" cellspacing="0" style="text-align: left;">
                                                    <tr align="left">
                                                        <td>
                                                            Company Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cboSellDownCompanyName" runat="server" CssClass="combo1" Width="15em">
                                                               
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            From Date:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSellDownFromDate" runat="server" CssClass="text_box1 jsdate required"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            To Date:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSellDownToDate" runat="server" CssClass="text_box1 jsdate required"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <input type="button" id="btnSelldown" value=" Export " class="frmButton" onclick="javascript:ExportDetails('S')" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="center" valign="top">
                                            <td>
                                                <table id="jqSelldownTrackingDetails" cellpadding="0" cellspacing="0" width="100%">
                                                </table>
                                                <div id="jqSelldownTrackingDetailsPager">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="StockDetails" style="padding: 0.5em 0.3em;">
                                    <table cellpadding="0" cellspacing="0" style="width: 100%;display :none ;"  >
                                        <tr align="center" valign="top">
                                            <td>
                                                <table cellpadding="0" cellspacing="0" style="text-align: left;">
                                                    <tr align="left">
                                                        <td>
                                                            Company Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cboStockCompanyName" runat="server" CssClass="combo1" Width="15em">
                                                                
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            From Date:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStockFromDate" runat="server" CssClass="text_box1 jsdate required"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            To Date:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStockToDate" runat="server" CssClass="text_box1 jsdate required"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <input type="button" id="btnStockDetails" value=" Export " class="frmButton" onclick="javascript:ExportDetails('D')" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="center" valign="top">
                                            <td>
                                                <table id="jqStockDetails" cellpadding="0" cellspacing="0" width="100%">
                                                </table>
                                                <div id="jqStockDetailsPager">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="Hid_GSecExposure" runat="server" />
                            <asp:HiddenField ID="Hid_NonGSecExposure" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
