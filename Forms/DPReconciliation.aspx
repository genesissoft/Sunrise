<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="DPReconciliation.aspx.cs" Inherits="Forms_DPReconciliation" Title="DP Reconciliation" %>

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
    #ui-datepicker-div
    {
        z-index:10;
    }
    
    #gbox_jqMarketRateDetails
    {
        z-index:0;
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
    
        function ValidateUpload() {
            if($("#<%= cboDPId.ClientID %>").val() == "0" || $("#<%= cboDPId.ClientID %>").val() == ""){
                AlertMessage('Validation', 'Please Select DPId first.', 175, 450, 'D');
                return false;
            }
            
            var fileUpload = $('#<%=fUpload.ClientID %>').get(0);
            var files = fileUpload.files;
            var fileExtension = ['xls', 'xlsx'];
            var ext = $(fileUpload).val().split('.').pop().toLowerCase();
            if ($(fileUpload).val() != '') {
                if ($.inArray(ext, fileExtension) == -1 && ext != '') {
                    AlertMessage('Validation', 'Sorry, only .xls or .xlsx (Excel) file formats are allowed.', 175, 450, 'D');
                    return false;
                }
            }
            else {
                AlertMessage('Validation', 'Please select associated file first.', 175, 450, 'D');
            }
            return true;
        }

        function Clear() {
            var fileUpload = document.getElementById("<%=fUpload.ClientID %>");
            var id = fileUpload.id;
            var name = fileUpload.name;
            var newFileUpload = document.createElement("INPUT");

            newFileUpload.type = "FILE";
            fileUpload.parentNode.insertBefore(newFileUpload, fileUpload.nextSibling);
            fileUpload.parentNode.removeChild(fileUpload);
            newFileUpload.id = id;
            newFileUpload.name = name;
            $(newFileUpload).addClass("upload");
            return false;
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

    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="data_table">
        <tr>
            <td class="HeaderCSS" align="center" style="padding-top:15px;">
               DP Reconciliation
                <div class="progress hide">
                    <img src="../Images/processing.gif" alt="Please wait while processing" title="Please wait while processing" />
                </div>
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
                            Select File:
                        </td>
                        <td>
                            <div class="border">
                                <asp:FileUpload ID="fUpload" runat="server" CssClass="upload" />
                                 <asp:Button ID="btn_Upload" runat="server" Text="Upload" CssClass="ButtonCSS" OnClientClick="javascript: return ValidateUpload();"
                                OnClick="btnUpload_Click" />
                            </div>
                        </td>
                        
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center">
            <td>
                <div id="divconfirm" title="Confirmation" style="display: none;">
                    <table cellpadding="0" cellspacing="0" width="100%" class="data_table">
                        <tr align="left">
                            <td style="font-weight: bold; color: Red;">
                                Disclaimer:<br />
                            </td>
                        </tr>
                        <tr align="left">
                            <td>
                                By clicking on "Submit" you agrees that all the information provided is accurate
                                and in case of any discrepancy you will be held liable.
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
