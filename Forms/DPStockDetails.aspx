<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="DPStockDetails.aspx.cs" Inherits="Forms_DPStockDetails" Title="DP Stock Details" %>

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

    <style type="text/css">
        td
        {
            padding-left: 5px;
        }
    </style>

    <script type="text/javascript">
    $(document).ready(function(){
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
    
    function validation()
    {
        if($("#<%= txtDated.ClientID %>").val() == ""){
            AlertMessage('Validation', 'Please select as on date first.', 175, 450, 'D');
            return false;
        }
    }
     function ReportType()
     {
      if(document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTransfer_0").checked == true )
        {    
          document.getElementById("row_DP").style.display = "";
          document.getElementById("row_SGL").style.display = "none";
        }
      if(document.getElementById("ctl00_ContentPlaceHolder1_rbl_TypeOFTransfer_1").checked == true )
        {    
          document.getElementById("row_DP").style.display = "none";
          document.getElementById("row_SGL").style.display = "";
        }
     
     }
    </script>

    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="HeaderCSS" align="center" >
                DP Stock Details
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="5" class="SeperatorRowCSS">
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            Stock Type:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList RepeatLayout="Flow" ID="rbl_TypeOFTransfer" runat="server" CellPadding="0"
                                CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" RepeatColumns="5"
                                AutoPostBack="false" onchange="ReportType();">
                                <asp:ListItem Selected="True" Value="D">DP</asp:ListItem>
                                <asp:ListItem Value="S">SGL</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            As on date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDated" runat="server" CssClass="text_box1 jsdate" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left" id="row_DP">
                        <td>
                            Select  DP:
                        </td>
                        <td>
                            <asp:DropDownList ID="cboDPId" runat="server" CssClass="combo1" AutoPostBack="true"
                               >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="left" id="row_SGL">
                        <td>
                            Select SGL:
                        </td>
                        <td>
                            <asp:DropDownList ID="cboSGLId" runat="server" CssClass="combo1" AutoPostBack="true"
                               >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            Export:
                        </td>
                        <td align ="left" >
                            <asp:Button ID="btn_Export" runat="server" Text="Export" CssClass="ButtonCSS" OnClick="btn_Export_Click"
                                OnClientClick="return validation();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
