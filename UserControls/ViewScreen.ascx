<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewScreen.ascx.vb" Inherits="UserControls_ViewScreen" %>
<style type="text/css">
    /*From styles.css--------------*/
    .table_border_right_bottom1 {
        border-right: 1px solid #8FBAEF;
        border-bottom: 1px solid #8FBAEF;
        /*border-left: 0px;
	border-top: 0px;*/
    }

        .table_border_right_bottom1 td {
            border-left: 1px solid #8FBAEF;
            border-top: 1px solid #8FBAEF;
            /*border-left: 0px;
	border-top: 0px;*/
        }

        .table_border_right_bottom1 th {
            border-left: 1px solid #8FBAEF;
            border-top: 1px solid #8FBAEF;
             border-right: 1px solid #8FBAEF;
            /*border-left: 0px;
	border-top: 0px;*/
        }
</style>
<script type="text/javascript">
    function CheckDelete(parentId) {
        SaveValues(parentId)
        if (window.confirm("Are you sure you want to delete this record????")) return true;
        return false;
    }
    function ValidateSearch(parentId) {
        elm = document.forms[0].elements;
        for (i = 0; i < elm.length; i++) {
            if (elm[i].id.indexOf("txt_Search") != -1) {
                if (elm[i].value == '') {
                    alert('Please enter the ' + elm[i].id.substring(elm[i].id.lastIndexOf('_') + 1, elm[i].id.length - 1));
                    return false;
                }
            }
        }
        SaveValues(parentId)
    }
    function SaveValues(parentId) {
        elm = document.forms[0].elements;

        document.getElementById(parentId + "_Hid_FieldValues").value = "";
        for (i = 0; i < elm.length; i++) {
            if (elm[i].id.indexOf("cbo_Search") != -1) {
                document.getElementById(parentId + "_Hid_FieldValues").value = document.getElementById(parentId + "_Hid_FieldValues").value + elm[i].value + "!"
            }
        }
    }
    function SelectOption(img, id, parentId) {
        var row = img.parentElement.parentElement
        UnselectAll(row)
        document.getElementById(parentId + "_Hid_Id").value = id
        img.src = "../Images/images.JPG"
        row.style.backgroundColor = '#EBFFEE'; //'#E1E1C3' 
        return false
    }
    function UnselectAll(row) {
        var grd = row.parentElement.parentElement
        for (i = 1; i <= (grd.children[0].children.length - 2) ; i++) {
            currRow = grd.children[0].children[i]
            if (currRow.children[0].children[0] != null) {
                currRow.children[0].children[0].src = "../Images/images3.JPG"
                currRow.style.backgroundColor = 'white'
            }
        }
    }
</script>

<table id="tbl_Main" width="100%" align="center" cellspacing="0" cellpadding="0"
    border="0">
    <tr>
        <td id="col_Header" class="SectionHeaderCSS" align="center" runat="server">View Details</td>
    </tr>
    <tr>
        <td class="SeperatorRowCSS"></td>
    </tr>
    <tr>
        <td align="center">
            <table id="Table3" width="95%" cellspacing="0" cellpadding="0" border="0">
                <%-- <tr>
                    <td class="SectionHeaderCSS" align="left" colspan="6">
                        SEARCH SECTION</td>
                </tr>--%>
                <tr>
                    <td align="center">
                        <asp:Panel ID="pnl_Search" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btn_Add" runat="server" CssClass="ButtonCSS" Text="Add" />
                        <asp:Button ID="btn_Search" runat="server" CssClass="ButtonCSS" Text="Search" />
                        <asp:Button ID="btn_ShowAll" runat="server" CssClass="ButtonCSS" Text="Show All" />
                    </td>
                </tr>
                <tr>
                    <td class="SeperatorRowCSS"></td>
                </tr>
                <tr>
                    <td align="center" style="width: 100%">
                        <asp:GridView ID="gv_Details" runat="server" AutoGenerateColumns="true" CssClass="table_border_right_bottom1"
                            AllowPaging="true" AllowSorting="true" PageSize="10">
                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                            <RowStyle HorizontalAlign="left" />
                            <%--<PagerStyle HorizontalAlign="Center" CssClass="GridPagerCSS" />--%>
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtn_Select" runat="server" ImageUrl="~/Images/images3.jpg"
                                            CommandName="Select" Height="13" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="EditRow" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                            CommandName="DeleteRow" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:HiddenField ID="Hid_ColWidths" runat="server" />
                        <asp:HiddenField ID="Hid_ColList" runat="server" />
                        <asp:HiddenField ID="Hid_DefaultSort" runat="server" />
                        <asp:HiddenField ID="Hid_FieldNames" runat="server" />
                        <asp:HiddenField ID="Hid_FieldValues" runat="server" />
                        <asp:HiddenField ID="Hid_ColText" runat="server" />
                        <asp:HiddenField ID="Hid_Id" runat="server" />
                        <asp:HiddenField ID="Hid_SelectedId" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
