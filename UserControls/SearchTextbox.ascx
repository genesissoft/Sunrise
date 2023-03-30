<%@ Control Language="VB" AutoEventWireup="false" CodeFile="searchtextbox.ascx.vb" Inherits="UserControl_searchtextbox" %>

<script type="text/javascript">
    $(document).ready(function () {
        $("#<%= txt_Name.ClientID %>").keypress(function (event) {
            return false;
        });

        $("img[id$='" + document.getElementById('<%= Hid_MyId.ClientID %>').value + '_btn_Del' + "']").bind("click", function () {
            document.getElementById("<%= Hid_AllFields.ClientID %>").value = "";
            document.getElementById("<%= Hid_SelectedId.ClientID %>").value = "";
            document.getElementById("<%= txt_Name.ClientID %>").value = "";
        });

        var elmName = document.getElementById('<%= Hid_MyId.ClientID %>').value + "_btn_Search";
        var elm = $("img[id$='" + elmName + "']")[0];

        var evnt = 'click';
        var ret;

        if (elm.attachEvent) {
            elm.attachEvent('onclick', function () {

                var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                var ExtraParams = $("#<%= Hid_ExtraParams.ClientID%>").val().split('#');

                for (var i = 0; i < ExtraParams.length - 1; i++) {
                    FillConditionalValues(parentId, ExtraParams[i], i.toString());
                }

                var queryStr = "search.aspx?";
                queryStr = queryStr + "PageName=" + $("#<%= Hid_PageName.ClientID%>").val();
                queryStr = queryStr + "&ProcName=" + $("#<%= Hid_ProcName.ClientID%>").val();
                queryStr = queryStr + "&SelectedId=" + $("#<%= Hid_SelectedFieldId.ClientID%>").val();
                queryStr = queryStr + "&SelectedName=" + $("#<%= Hid_SelectedFieldName.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName=" + $("#<%= Hid_ConditionalFieldName.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue=" + $("#<%= Hid_ConditionalFieldValue.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName1=" + $("#<%= Hid_ConditionalFieldName1.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue1=" + $("#<%= Hid_ConditionalFieldValue1.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName2=" + $("#<%= Hid_ConditionalFieldName2.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue2=" + $("#<%= Hid_ConditionalFieldValue2.ClientID%>").val();
                queryStr = queryStr + "&CondExist=" + $("#<%= Hid_CondExist.ClientID%>").val();
                queryStr = queryStr + "&CheckYearComp=" + $("#<%= Hid_CompYear.ClientID%>").val();
                queryStr = queryStr + "&CheckComp=" + $("#<%= Hid_CheckComp.ClientID%>").val();
                

                ret = window.showModalDialog(queryStr, 'some argument', 'dialogWidth:' + $("#<%= Hid_FormWidth.ClientID%>").val() + 'px;dialogHeight:' + $("#<%= Hid_FormHeight.ClientID%>").val() + 'px;center:1;status:0;resizable:0;');

                if (typeof (ret) != 'undefined') {
                    var arrRetValues = ret.split('!');
                    document.getElementById("<%= Hid_SelectedId.ClientID %>").value = arrRetValues[0];
                    document.getElementById("<%= txt_Name.ClientID %>").value = arrRetValues[1];
                    document.getElementById("<%= Hid_SelectedFieldText.ClientID %>").value = arrRetValues[1];

                    var txtid = document.getElementById("<%= txt_Name.ClientID %>").id;
                    $("#" + txtid).trigger("onchange");

                    if ($("#<%= Hid_Postback.ClientID%>").val() == "True")
                        document.getElementById('<%= btn_Post.ClientID%>').click();
                }
            });
        }
        else {

            elm.addEventListener('click', function () {

                var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                var ExtraParams = $("#<%= Hid_ExtraParams.ClientID%>").val().split('#');

                for (var i = 0; i < ExtraParams.length - 1; i++) {
                    FillConditionalValues(parentId, ExtraParams[i], i.toString());
                }
                var queryStr = "search.aspx?";
                queryStr = queryStr + "PageName=" + $("#<%= Hid_PageName.ClientID%>").val();
                queryStr = queryStr + "&ProcName=" + $("#<%= Hid_ProcName.ClientID%>").val();
                queryStr = queryStr + "&SelectedId=" + $("#<%= Hid_SelectedFieldId.ClientID%>").val();
                queryStr = queryStr + "&SelectedName=" + $("#<%= Hid_SelectedFieldName.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName=" + $("#<%= Hid_ConditionalFieldName.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue=" + $("#<%= Hid_ConditionalFieldValue.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName1=" + $("#<%= Hid_ConditionalFieldName1.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue1=" + $("#<%= Hid_ConditionalFieldValue1.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName2=" + $("#<%= Hid_ConditionalFieldName2.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue2=" + $("#<%= Hid_ConditionalFieldValue2.ClientID%>").val();
                queryStr = queryStr + "&CondExist=" + $("#<%= Hid_CondExist.ClientID%>").val();
                queryStr = queryStr + "&CheckYearComp=" + $("#<%= Hid_CompYear.ClientID%>").val();
                queryStr = queryStr + "&CheckComp=" + $("#<%= Hid_CheckComp.ClientID%>").val();

                ret = window.showModalDialog(queryStr, 'some argument', 'dialogWidth:' + $("#<%= Hid_FormWidth.ClientID%>").val() + 'px;dialogHeight:' + $("#<%= Hid_FormHeight.ClientID%>").val() + 'px;center:1;status:0;resizable:0;');
                
                if (typeof (ret) != 'undefined') {
                    var arrRetValues = ret.split('!');
                    document.getElementById("<%= Hid_SelectedId.ClientID %>").value = arrRetValues[0];
                    document.getElementById("<%= txt_Name.ClientID %>").value = arrRetValues[1];
                    document.getElementById("<%= Hid_SelectedFieldText.ClientID %>").value = arrRetValues[1];

                    var txtid = document.getElementById("<%= txt_Name.ClientID %>").id;
                    $("#" + txtid).trigger("onchange");

                    if ($("#<%= Hid_Postback.ClientID%>").val() == "True")
                        document.getElementById('<%= btn_Post.ClientID%>').click();

                }
            });
        }
    });

    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        $("#<%= txt_Name.ClientID %>").keypress(function (event) {
            return false;
        });

        $("img[id$='" + document.getElementById('<%= Hid_MyId.ClientID %>').value + '_btn_Del' + "']").bind("click", function () {
            document.getElementById("<%= Hid_AllFields.ClientID %>").value = "";
            document.getElementById("<%= Hid_SelectedId.ClientID %>").value = "";
            document.getElementById("<%= txt_Name.ClientID %>").value = "";
        });

        var elmName = document.getElementById('<%= Hid_MyId.ClientID %>').value + "_btn_Search";
        var elm = $("img[id$='" + elmName + "']")[0];
        var evnt = 'click';
        var ret;

        if (elm.attachEvent) {
            elm.attachEvent('onclick', function () {

                var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                var ExtraParams = $("#<%= Hid_ExtraParams.ClientID%>").val().split('#');

                for (var i = 0; i < ExtraParams.length - 1; i++) {
                    FillConditionalValues(parentId, ExtraParams[i], i.toString());
                }

                var queryStr = "search.aspx?";
                queryStr = queryStr + "PageName=" + $("#<%= Hid_PageName.ClientID%>").val();
                queryStr = queryStr + "&ProcName=" + $("#<%= Hid_ProcName.ClientID%>").val();
                queryStr = queryStr + "&SelectedId=" + $("#<%= Hid_SelectedFieldId.ClientID%>").val();
                queryStr = queryStr + "&SelectedName=" + $("#<%= Hid_SelectedFieldName.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName=" + $("#<%= Hid_ConditionalFieldName.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue=" + $("#<%= Hid_ConditionalFieldValue.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName1=" + $("#<%= Hid_ConditionalFieldName1.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue1=" + $("#<%= Hid_ConditionalFieldValue1.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName2=" + $("#<%= Hid_ConditionalFieldName2.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue2=" + $("#<%= Hid_ConditionalFieldValue2.ClientID%>").val();
                queryStr = queryStr + "&CondExist=" + $("#<%= Hid_CondExist.ClientID%>").val();
                queryStr = queryStr + "&CheckYearComp=" + $("#<%= Hid_CompYear.ClientID%>").val();
                queryStr = queryStr + "&CheckComp=" + $("#<%= Hid_CheckComp.ClientID%>").val();

                ret = window.showModalDialog(queryStr, 'some argument', 'dialogWidth:' + $("#<%= Hid_FormWidth.ClientID%>").val() + 'px;dialogHeight:' + $("#<%= Hid_FormHeight.ClientID%>").val() + 'px;center:1;status:0;resizable:0;');
                
                if (typeof (ret) != 'undefined') {
                    var arrRetValues = ret.split('!');
                    document.getElementById("<%= Hid_SelectedId.ClientID %>").value = arrRetValues[0];
                    document.getElementById("<%= txt_Name.ClientID %>").value = arrRetValues[1];
                    document.getElementById("<%= Hid_SelectedFieldText.ClientID %>").value = arrRetValues[1];

                    var txtid = document.getElementById("<%= txt_Name.ClientID %>").id;
                    $("#" + txtid).trigger("onchange");

                    if ($("#<%= Hid_Postback.ClientID%>").val() == "True")
                        document.getElementById('<%= btn_Post.ClientID%>').click();
                }
            });
        }
        else {

            elm.addEventListener('click', function () {

                var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                var ExtraParams = $("#<%= Hid_ExtraParams.ClientID%>").val().split('#');

                for (var i = 0; i < ExtraParams.length - 1; i++) {
                    FillConditionalValues(parentId, ExtraParams[i], i.toString());
                }
                var queryStr = "search.aspx?";
                queryStr = queryStr + "PageName=" + $("#<%= Hid_PageName.ClientID%>").val();
                queryStr = queryStr + "&ProcName=" + $("#<%= Hid_ProcName.ClientID%>").val();
                queryStr = queryStr + "&SelectedId=" + $("#<%= Hid_SelectedFieldId.ClientID%>").val();
                queryStr = queryStr + "&SelectedName=" + $("#<%= Hid_SelectedFieldName.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName=" + $("#<%= Hid_ConditionalFieldName.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue=" + $("#<%= Hid_ConditionalFieldValue.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName1=" + $("#<%= Hid_ConditionalFieldName1.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue1=" + $("#<%= Hid_ConditionalFieldValue1.ClientID%>").val();
                queryStr = queryStr + "&CondFieldName2=" + $("#<%= Hid_ConditionalFieldName2.ClientID%>").val();
                queryStr = queryStr + "&CondFieldValue2=" + $("#<%= Hid_ConditionalFieldValue2.ClientID%>").val();
                queryStr = queryStr + "&CondExist=" + $("#<%= Hid_CondExist.ClientID%>").val();
                queryStr = queryStr + "&CheckYearComp=" + $("#<%= Hid_CompYear.ClientID%>").val();
                queryStr = queryStr + "&CheckComp=" + $("#<%= Hid_CheckComp.ClientID%>").val();

                ret = window.showModalDialog(queryStr, 'some argument', 'dialogWidth:' + $("#<%= Hid_FormWidth.ClientID%>").val() + 'px;dialogHeight:' + $("#<%= Hid_FormHeight.ClientID%>").val() + 'px;center:1;status:0;resizable:0;');

                if (typeof (ret) != 'undefined') {
                    var arrRetValues = ret.split('!');
                    document.getElementById("<%= Hid_SelectedId.ClientID %>").value = arrRetValues[0];
                    document.getElementById("<%= txt_Name.ClientID %>").value = arrRetValues[1];
                    document.getElementById("<%= Hid_SelectedFieldText.ClientID %>").value = arrRetValues[1];

                    var txtid = document.getElementById("<%= txt_Name.ClientID %>").id;
                    $("#" + txtid).trigger("onchange");

                    if ($("#<%= Hid_Postback.ClientID%>").val() == "True")
                        document.getElementById('<%= btn_Post.ClientID%>').click();
                }
            });
        }
    });

    function FillConditionalValues(parentId, id, cnt) {
        var value = $("#" + id).val();
        if (cnt == 0) {
            cnt = "";
        }
        $("#" + parentId + "_Hid_ConditionalFieldValue" + cnt).val(value);
    }

</script>

<%--<div>
    <asp:TextBox ID="txt_Name" runat="server" CssClass="TextBoxCSS" ReadOnly="true"></asp:TextBox>
    <img src="~/Include/CSS/Images/search1.gif" id="btn_Search" class="searchimage" runat="server"
        alt="Search" title="Search" />
    <img src="../Images/del.gif" id="btn_Del" class="searchimage hidden" runat="server" alt="Search"
        style="height: 7px; width: 7px;" title="Clear" />
</div>--%>
<div>
    <table style="padding: 0px;">
        <tr>
            <td>
                <asp:TextBox ID="txt_Name" runat="server" CssClass="TextBoxCSS" ReadOnly="true"></asp:TextBox></td>
            <td>
                <img src="~/Include/CSS/Images/search1.gif" id="btn_Search" class="searchimage" runat="server"
                    alt="Search" title="Search" /></td>
            <td>
                <img src="../Images/del.gif" id="btn_Del" class="searchimage hidden" runat="server" alt="Search"
                    style="height: 7px; width: 7px;" title="Clear" /></td>
        </tr>
    </table>
</div>
<i id="fnt_Mandatory" style="color: red" runat="server" class="hidden">*</i>
<asp:Button ID="btn_Post" runat="server" CssClass="hidden" />

<asp:HiddenField ID="Hid_PageName" runat="server" />
<asp:HiddenField ID="Hid_ControlId" runat="server" />
<asp:HiddenField ID="Hid_TableName" runat="server" />
<asp:HiddenField ID="Hid_ProcName" runat="server" />
<asp:HiddenField ID="Hid_SelectedFieldId" runat="server" />
<asp:HiddenField ID="Hid_SelectedFieldName" runat="server" />
<asp:HiddenField ID="Hid_SearchFieldName" runat="server" />
<asp:HiddenField ID="Hid_SearchFieldValue" runat="server" />
<asp:HiddenField ID="Hid_SelectedId" runat="server" />
<asp:HiddenField ID="Hid_ColList" runat="server" />
<asp:HiddenField ID="Hid_RetValues" runat="server" />
<asp:HiddenField ID="Hid_SelectedFieldIndex" runat="server" />
<asp:HiddenField ID="Hid_SelectedFieldText" runat="server" />
<asp:HiddenField ID="Hid_ConditionalFieldName" runat="server" />
<asp:HiddenField ID="Hid_ConditionalFieldValue" runat="server" />
<asp:HiddenField ID="Hid_ConditionalFieldName1" runat="server" />
<asp:HiddenField ID="Hid_ConditionalFieldValue1" runat="server" />
<asp:HiddenField ID="Hid_ConditionalFieldName2" runat="server" />
<asp:HiddenField ID="Hid_ConditionalFieldValue2" runat="server" />
<asp:HiddenField ID="Hid_StrPage" runat="server" />
<asp:HiddenField ID="Hid_QueryString" runat="server" />
<asp:HiddenField ID="Hid_TabList" runat="server" />
<asp:HiddenField ID="Hid_WidList" runat="server" />
<asp:HiddenField ID="Hid_VisList" runat="server" />
<asp:HiddenField ID="Hid_DisplayName" runat="server" />
<asp:HiddenField ID="Hid_MyId" runat="server" />
<asp:HiddenField ID="Hid_AllFields" runat="server" />
<asp:HiddenField ID="Hid_ExtraParams" runat="server" />
<asp:HiddenField ID="Hid_Postback" runat="server" />
<asp:HiddenField ID="Hid_FormHeight" runat="server" />
<asp:HiddenField ID="Hid_FormWidth" runat="server" />
<asp:HiddenField ID="Hid_SrcType" runat="server" />
<asp:HiddenField ID="Hid_CondExist" runat="server" />
<asp:HiddenField ID="Hid_CompYear" runat="server" />
<asp:HiddenField ID="Hid_CheckComp" runat="server" />
<asp:HiddenField ID="Hid_ShowAll" runat="server" />

