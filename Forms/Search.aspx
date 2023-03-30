<%@ Page Language="VB" AutoEventWireup="false" CodeFile="search.aspx.vb" Inherits="search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Serach</title>
    <link href="../Include/Style_New.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../Include/Common.js"></script>

    <link type="text/css" href="../Include/jqGrid/jquery-ui.css" rel="Stylesheet" />
    <link rel="stylesheet" type="text/css" href="../Include/jqGrid/jquery-ui.css" />
    <link type="text/css" rel="Stylesheet" href="../Include/jqGrid/ui.jqgrid.css" />

    <script type="text/javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="../Include/jqGrid/jquery.jqGrid.js"></script>
    <script type="text/javascript" src="../Include/jqGrid/grid.locale-en.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var pagename = $('#<%=Hid_PageName.ClientID%>').val();
            var condition = $('#<%=Hid_ManualCond.ClientID%>').val();
            $.ajax({
                url: "getdata.ashx?pagename=search_" + pagename + "_model",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var colN = result.colName, colM = result.colModel;

                    $("#jqDetails").jqGrid({
                        url: "getdata.ashx?pagename=search_" + pagename + "_data&condition=" + condition,
                        datatype: "json",
                        colNames: colN,
                        colModel: colM,
                        viewrecords: true,
                        autowidth: true,
                        height: 410,
                        rowNum: 20,
                        loadonce: false,
                        pager: "#jqDetailsPager",
                        loadComplete: function (result) {
                            if (($("#jqDetails").getGridParam("reccount") - 0) > 0) {
                                var row = $("#jqDetails").getRowData();
                                $("#jqDetails").setSelection($("#jqDetails").getDataIDs()[0], true);
                            }
                        },
                        ondblClickRow: function (rowid) {
                            $("#btn_Ret").trigger("click");
                        }
                    });

                    $('#jqDetails').navGrid("#jqDetailsPager", { search: true, add: false, edit: false, del: false, refresh: true },
                    {}, // edit options
                    {}, // add options
                    {}, // delete options
                    { multipleSearch: true, closeOnEscape: true, closeAfterSearch: true }
                    );
                },
                failure: function (result) {
                    alert('Some error has occurred.');
                }
            });
        });

        function submited() {
            var rowId = $("#jqDetails").jqGrid('getGridParam', 'selrow');
            if (rowId != null) {
                var rowData = jQuery("#jqDetails").getRowData(rowId);
                var colId = rowData[$("#<%= Hid_SelectedFieldId.ClientID%>").val()];
                var colName = rowData[$("#<%= Hid_SelectedFieldName.ClientID%>").val()];
                $("#<%= Hid_Id.ClientID %>").val(colId + "!" + colName);
                window.returnValue = colId + "!" + colName;
                window.close();
            }
            else {
                $("#<%= Hid_Id.ClientID %>").val('');
                alert('Please select a row first.');
            }

            return false;
        }

        function CloseWindows() {
            window.close();
        }

    </script>

</head>
<body style="background-image: none; background-color: #FBFBFB;">
    <form id="form1" runat="server">
        <div>
            <table style="width: 100%;" class="data_table">
                <tr>
                    <td class="formHeader">Search Details
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="border-bottom: 2px solid #444444; text-align: center;">
                        <table style="margin-left: auto; margin-right: auto;">
                            <tr>
                                <td>Search Value:
                                </td>
                                <td>
                                    <select id="cboSearchFields" class="combo"></select>
                                </td>
                                <td>
                                    <input type="text" class="text_box" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="text-align: center;">
                    <td>
                        <table id="jqDetails" style="width: 100%;">
                        </table>
                        <div id="jqDetailsPager">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <input type="button" id="btn_Ret" value="Submit" class="frmButton" name="btn_Ret" onclick="submited()" />
                        <input type="button" id="btn_Cancel" value="Cancel" class="frmButton" name="btn_Cancel" onclick="CloseWindows()" />

                        <asp:HiddenField ID="Hid_PageName" runat="server" />
                        <asp:HiddenField ID="Hid_ControlId" runat="server" />
                        <asp:HiddenField ID="Hid_TableName" runat="server" />
                        <asp:HiddenField ID="Hid_ProcName" runat="server" />
                        <asp:HiddenField ID="Hid_SelectedFieldId" runat="server" />
                        <asp:HiddenField ID="Hid_SelectedFieldName" runat="server" />
                        <asp:HiddenField ID="Hid_SearchFieldName" runat="server" />
                        <asp:HiddenField ID="Hid_SearchFieldValue" runat="server" />
                        <asp:HiddenField ID="Hid_ColName" runat="server" />
                        <asp:HiddenField ID="Hid_ColModel" runat="server" />

                        <asp:HiddenField ID="Hid_Id" runat="server" />
                        <asp:HiddenField ID="Hid_JobEntryId" runat="server" />
                        <asp:HiddenField ID="Hid_ColList" runat="server" />
                        <asp:HiddenField ID="Hid_TabList" runat="server" />
                        <asp:HiddenField ID="Hid_ProcessName" runat="server" />
                        <asp:HiddenField ID="Hid_PrimaryKey" runat="server" />
                        <asp:HiddenField ID="Hid_SecondaryKey" runat="server" />
                        <asp:HiddenField ID="Hid_SelField" runat="server" />
                        <asp:HiddenField ID="Hid_ManualCond" runat="server" />
                        <asp:HiddenField ID="Hid_JsonSrh" runat="server" />
                        <asp:HiddenField ID="Hid_WidList" runat="server" />
                        <asp:HiddenField ID="Hid_VisList" runat="server" />
                        <asp:HiddenField ID="Hid_DisplayName" runat="server" />
                        <asp:HiddenField ID="Hid_CondFieldName1" runat ="server" />
                        <asp:HiddenField ID="Hid_CondFieldValue1" runat ="server" />
                        <asp:HiddenField ID="Hid_CondFieldName2" runat ="server" />
                        <asp:HiddenField ID="Hid_CondFieldValue2" runat ="server" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
