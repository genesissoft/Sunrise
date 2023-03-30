<%@ Page Title="" Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="UploadAcknowledgedDeals.aspx.vb" Inherits="Forms_UploadAcknowledgedDeals" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link type="text/css" href="../Include/jqGrid/jquery-ui.css" rel="Stylesheet" />
    <link rel="stylesheet" type="text/css" href="../Include/jqGrid/jquery-ui.css" />
    <link type="text/css" rel="Stylesheet" href="../Include/jqGrid/ui.jqgrid.css" />
    <script type="text/javascript" src="../Include/Common.js"></script>
    <script type="text/javascript" src="../Include/jquery.js"></script>
    <script type="text/javascript" src="../Include/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="../Include/jqGrid/jquery.jqGrid.js"></script>
    <script type="text/javascript" src="../Include/jqGrid/grid.locale-en.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery.ui.datepicker.js"></script>


    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            $('#<%=FilePicker.ClientID %>').change(function () {
                var fileExtension = ['pdf'];
                var ext = $(this).val().split('.').pop().toLowerCase();


                if ($.inArray(ext, fileExtension) == -1 && ext != '') {
                    AlertMessage('Validation', 'Sorry, only .pdf file format is allowed.', 175, 450, 'D');
                }
            });

            $("#jqAcknowledgedDeals").jqGrid({
                url: "getdata.ashx?pagename=AcknowledgedDeals",
                datatype: "json",
                colNames: ['SN', '', '', 'DealSlipNo', 'TradeDate', 'CustomerName', 'PAN', 'SecurityName', 'ISIN', 'Download', ''],
                colModel: [
                     { name: 'row', index: 'row', width: 15, sorttype: 'int', align: 'center', sortable: false, search: false },
                        { name: 'Id', index: 'Id', width: 0, search: false, hidden: true, key: true },
                        { name: 'DealSlipId', index: 'DealSlipId', width: 0, search: false, key: true, hidden: true },
                        { name: 'DealSlipNo', index: 'DealSlipNo', width: 80, sorttype: 'text', sortable: true, search: true, searchrules: { required: true } },
                        { name: 'TradeDate', index: 'TradeDate', width: 80, sorttype: 'text', sortable: true, search: true, searchrules: { required: true } },
                        { name: 'CustomerName', index: 'CustomerName', width: 250, sorttype: 'text', sortable: true, search: true, searchrules: { required: true } },
                         { name: 'PAN', index: 'PAN', width: 90, sorttype: 'text', sortable: true, search: true, searchrules: { required: true } },
                        { name: 'SecurityName', index: 'SecurityName', width: 250, sorttype: 'text', sortable: true, search: true, searchrules: { required: true } },
                        { name: 'ISIN', index: 'ISIN', width: 90, sorttype: 'text', sortable: true, search: true, searchrules: { required: true } },
                        { name: 'Download', index: 'Download', width: 50, sorttype: 'text', sortable: true, search: true, searchrules: { required: true }, formatter: Download },
                        { name: '', index: '', width: 10, align: 'center', sortable: false, search: false, formatter: DeleteImage }

                ],
                viewrecords: true,
                autowidth: true,
                height: 200,
                rowNum: 50,
                width: 1000,

                loadonce: false,
                pager: "#jqAcknowledgedDealsPager"
            });
            $('#jqAcknowledgedDeals').navGrid("#jqAcknowledgedDealsPager", { search: true, add: false, edit: false, del: false, refresh: true },
           {}, // edit options
           {}, // add options
           {}, // delete options
           { multipleSearch: true, closeOnEscape: true, closeAfterSearch: true }
           );



        });

        function Download(cellvalue, options, rowdata) {

            return '<a href="showdocument.aspx?id=' + rowdata[2] + '&Type=DealAck">Download</a>';
        }
        function DeleteImage(cellvalue, options, rowdata) {
            return '<a href="javascript:DeleteRecord(\'#jqAcknowledgedDeals\',\'AcknowledgedDeals\',\'' + rowdata[1] + '\');"><img title="Delete" class="imgdelete" src="../Images/delete.gif"></a>';
        }
        function NoRecordsFound() {
            AlertMessage("Validation", "Acknowledgement already uploaded for this deal.", 175, 450);
        }

        function ValidateUploadFile() {
            var fileUpload = $('#<%=FilePicker.ClientID %>').get(0);
            var files = fileUpload.files;
            var fileExtension = ['pdf'];
            var ext = $(fileUpload).val().split('.').pop().toLowerCase();
            if ($(fileUpload).val() != '') {
                if ($.inArray(ext, fileExtension) == -1 && ext != '') {
                    AlertMessage('Validation', 'Sorry, only .pdf file format is allowed.', 175, 450, 'D');
                    return false;
                }
            }
            return true;
        }
    </script>

    <table align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr align="left">
            <td class="SectionHeaderCSS">Upload Acknowledged Deals
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>

        <tr id="row_UploadSignedAck" runat="server" visible="true" align="left">
            <td align="left">
                <table align="center" width="40%" border="0">
                    <tr align="left" id="tr_DealNo" runat="server">
                        <td align="right" class="LabelCSS">Deal No.:
                        </td>
                        <td align="left">
                            <uc:Search ID="srh_TransCode" runat="server" PageName="PrintDeals" AutoPostback="true"
                                SelectedFieldId="Id" SelectedFieldName="DealSlipNo" CheckYearCompany="true" ConditionExist="true" ConditionalFieldName="UserId"
                                ConditionalFieldId="Hid_UserId" ConditionalFieldId1="Hid_UserTypeId" ConditionalFieldName1="UserTypeId" FormWidth="700" />
                        </td>
                    </tr>
                    <tr id="row_UploadDoc" runat="server" visible="false">
                        <td class="LabelCSS" align="right">Upload Acknowledged Deal Confirmation:
                        </td>
                        <td class="ForControls" align="left" valign="middle">
                            <input id="FilePicker" type="file" name="File1" class="LabelCSS" runat="server" style="width: 208px; !important"
                                onchange="UploadTempImage();" tabindex="1" />
                        </td>
                    </tr>

                    <tr>
                        <td align="center" valign="middle" colspan="6">
                            <asp:Label ID="LabelError" runat="server" CssClass="LabelCSS"></asp:Label>
                        </td>
                    </tr>

                    <tr align="left">
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" OnClientClick="return ValidateUploadFile();" />

                        </td>
                    </tr>

                </table>
            </td>
        </tr>


        <tr align="center">
            <td colspan="3">
                <table id="jqAcknowledgedDeals" cellpadding="0" cellspacing="0">
                </table>
                <div id="jqAcknowledgedDealsPager">
                </div>
            </td>
        </tr>
        <asp:HiddenField ID="Hid_Show" runat="server" />
        <asp:HiddenField ID="Hid_uploadImagePath" runat="server" />
        <asp:HiddenField ID="Hid_ImageContentType" runat="server" />
        <asp:HiddenField ID="Hid_DocumentDetails" runat="server" />
        <asp:HiddenField ID="Hid_DealAckDocument" runat="server" />
        <asp:HiddenField ID="Hid_DocumentId" runat="server" />
        <asp:HiddenField ID="Hid_DocumentMaster" runat="server" />
        <asp:HiddenField ID="Hid_UserId" runat="server" />
        <asp:HiddenField ID="Hid_UserTypeId" runat="server" />

    </table>
</asp:Content>

