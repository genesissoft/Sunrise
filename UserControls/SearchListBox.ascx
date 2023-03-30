<%@ Control Language="VB" AutoEventWireup="false" CodeFile="searchlistbox.ascx.vb" Inherits="UserControl_searchlistbox" %>
<script type="text/javascript" src="../Include/Common.js"></script>

<script type="text/javascript">
    $(function () {
        var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
        $("[id*=" + parentId + "_chk_Select" + "]").bind("click", function () {
            if ($(this).is(":checked")) {
                $(this).closest("table").find("select").empty();
            } else {
            }
        });
    });

    $(document).ready(function () {
        var parentId_ = $("#<%= Hid_ControlId.ClientID%>").val();
        var txt_ = "";
        var val_ = "";
        var lstBox_ = document.getElementById(parentId_ + "_lst_Select");
        for (i = 0; i < lstBox_.options.length; i++) {
            txt_ = txt_ + lstBox_.options[i].text + "!";
            val_ = val_ + lstBox_.options[i].value + "!";
        }

        document.getElementById("<%= Hid_SelectedId.ClientID %>").value = val_;
        document.getElementById("<%= Hid_SelectedFieldText.ClientID %>").value = txt_;

        $("img[id$='" + document.getElementById('<%= Hid_MyId.ClientID %>').value + '_btn_Del' + "']").bind("click", function () {
            document.getElementById("<%= Hid_AllFields.ClientID %>").value = "";
            document.getElementById("<%= Hid_SelectedId.ClientID %>").value = "";
            $("#<%= lst_Select.ClientID%>").empty();
        });

        var elmName = document.getElementById('<%= Hid_MyId.ClientID %>').value + "_btn_Search";
        var elm = $("a[id$='" + elmName + "']")[0];

        var evnt = 'click';
        var ret;

        if (elm.attachEvent) {
            elm.attachEvent('onclick', function () {
                var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                var ExtraParams = $("#<%= Hid_ExtraParams.ClientID%>").val().split('#');
                if (document.getElementById(parentId + "_chk_Select") != null) {
                    if (document.getElementById(parentId + "_chk_Select").checked == true) return false
                }
                for (var i = 0; i < ExtraParams.length - 1; i++) {
                    FillConditionalValues(parentId, ExtraParams[i], i.toString());
                }
                //alert($("#<%= Hid_ConditionalFieldValue.ClientID%>").val());
                var lstBox = document.getElementById(parentId + "_lst_Select");
                var texts = "";
                var values = "";
                var strPage = document.getElementById(parentId + "_Hid_StrPage").value;
                var cndFldValue = document.getElementById(parentId + "_Hid_ConditionalFieldValue").value;
                var cndFldValue1 = document.getElementById(parentId + "_Hid_ConditionalFieldValue1").value;
                var cndFldValue2 = document.getElementById(parentId + "_Hid_ConditionalFieldValue2").value;
                for (i = 0; i < lstBox.options.length; i++) {
                    texts = texts + lstBox.options[i].text + "!";
                    values = values + lstBox.options[i].value + "!";
                }
                texts = texts.replace("&", "^");
                var temp = texts;
                var intIndexOfMatch = temp.indexOf("&");
                while (intIndexOfMatch != -1) {
                    // Relace out the current instance.
                    temp = temp.replace("&", "^")
                    // Get the index of any next matching substring.
                    intIndexOfMatch = temp.indexOf("&");
                }

                texts = temp;
                $("#<%= Hid_SelectedId.ClientID%>").val(values);
                $("#<%= Hid_SelectedFieldText.ClientID%>").val(texts);
                var queryStr = "SelectFields.aspx?";

                queryStr = queryStr + "?SourceType=" + $("#<%= Hid_SrcType.ClientID%>").val() + "&TableName=" + $("#<%= Hid_TableName.ClientID%>").val()
                                + "&SelectedFieldName=" + $("#<%= Hid_SelectedFieldName.ClientID%>").val() + "&ProcName=" + $("#<%= Hid_ProcName.ClientID%>").val()
                + "&SelectedValueName=" + $("#<%= Hid_SearchFieldValue.ClientID%>").val() + "&SelectedValues=" + values
                + "&SelectedTexts=" + texts + "&CondExist=" + $("#<%= Hid_CondExist.ClientID%>").val()
                + "&CondFieldName=" + $("#<%= Hid_ConditionalFieldName.ClientID%>").val() + "&CondFieldValue=" + cndFldValue
                + "&ShowAll=" + $("#<%= Hid_ShowAll.ClientID%>").val() + "&CheckYearComp=" + $("#<%= Hid_CompYear.ClientID%>").val()
                + "&CondFieldName1=" + $("#<%= Hid_ConditionalFieldName1.ClientID%>").val() + "&CondFieldValue1=" + cndFldValue1
                + "&CondFieldName2=" + $("#<%= Hid_ConditionalFieldName2.ClientID%>").val() + "&CondFieldValue2=" + cndFldValue2
                + "&strPage=" + $("#<%= Hid_PageName.ClientID%>").val()

                ret = window.showModalDialog(queryStr, 'some argument', 'dialogWidth:300px;dialogHeight:480px;center:1;status:0;resizable:0;');
                if (typeof (ret) != 'undefined') {
                    var arrRetValues = ret.split('|');
                    document.getElementById("<%= Hid_SelectedId.ClientID %>").value = arrRetValues[1];
                    document.getElementById("<%= Hid_SelectedFieldText.ClientID %>").value = arrRetValues[0];

                    var listBox = document.getElementById("<%= lst_Select.ClientID%>");
                    FillList(arrRetValues[1], arrRetValues[0], listBox);
                    //$("#" + listBox).trigger("onchange");
                    var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                    $(document.getElementById(parentId + "_listBox")).trigger("onchange");
                    if ($("#<%= Hid_Postback.ClientID%>").val() == "True")
                        document.getElementById('<%= btn_Post.ClientID%>').click();
                }
            });
        }
        else {
            elm.addEventListener('click', function () {
                var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                var ExtraParams = $("#<%= Hid_ExtraParams.ClientID%>").val().split('#');
                if (document.getElementById(parentId + "_chk_Select") != null) {
                    if (document.getElementById(parentId + "_chk_Select").checked == true) return false
                }
                for (var i = 0; i < ExtraParams.length - 1; i++) {
                    FillConditionalValues(parentId, ExtraParams[i], i.toString());
                }

                var lstBox = document.getElementById(parentId + "_lst_Select");
                var texts = "";
                var values = "";
                var strPage = document.getElementById(parentId + "_Hid_StrPage").value;
                var cndFldValue = document.getElementById(parentId + "_Hid_ConditionalFieldValue").value;
                var cndFldValue1 = document.getElementById(parentId + "_Hid_ConditionalFieldValue1").value;
                var cndFldValue2 = document.getElementById(parentId + "_Hid_ConditionalFieldValue2").value;
                for (i = 0; i < lstBox.options.length; i++) {
                    texts = texts + lstBox.options[i].text + "!";
                    values = values + lstBox.options[i].value + "!";
                }
                texts = texts.replace("&", "^");
                var temp = texts;
                var intIndexOfMatch = temp.indexOf("&");
                while (intIndexOfMatch != -1) {
                    // Relace out the current instance.
                    temp = temp.replace("&", "^")
                    // Get the index of any next matching substring.
                    intIndexOfMatch = temp.indexOf("&");
                }

                texts = temp;
                $("#<%= Hid_SelectedId.ClientID%>").val(values);
                $("#<%= Hid_SelectedFieldText.ClientID%>").val(texts);
                var queryStr = "SelectFields.aspx?";

                queryStr = queryStr + "?SourceType=" + $("#<%= Hid_SrcType.ClientID%>").val() + "&TableName=" + $("#<%= Hid_TableName.ClientID%>").val()
                                + "&SelectedFieldName=" + $("#<%= Hid_SelectedFieldName.ClientID%>").val() + "&ProcName=" + $("#<%= Hid_ProcName.ClientID%>").val()
                + "&SelectedValueName=" + $("#<%= Hid_SearchFieldValue.ClientID%>").val() + "&SelectedValues=" + values
                + "&SelectedTexts=" + texts + "&CondExist=" + $("#<%= Hid_CondExist.ClientID%>").val()
                + "&CondFieldName=" + $("#<%= Hid_ConditionalFieldName.ClientID%>").val() + "&CondFieldValue=" + cndFldValue
                + "&ShowAll=" + $("#<%= Hid_ShowAll.ClientID%>").val() + "&CheckYearComp=" + $("#<%= Hid_CompYear.ClientID%>").val()
                + "&CondFieldName1=" + $("#<%= Hid_ConditionalFieldName1.ClientID%>").val() + "&CondFieldValue1=" + cndFldValue1
                + "&CondFieldName2=" + $("#<%= Hid_ConditionalFieldName2.ClientID%>").val() + "&CondFieldValue2=" + cndFldValue2
                + "&strPage=" + $("#<%= Hid_PageName.ClientID%>").val()

                ret = window.showModalDialog(queryStr, 'some argument', 'dialogWidth:300px;dialogHeight:480px;center:1;status:0;resizable:0;');
                if (typeof (ret) != 'undefined') {
                    var arrRetValues = ret.split('|');
                    document.getElementById("<%= Hid_SelectedId.ClientID %>").value = arrRetValues[1];
                    document.getElementById("<%= Hid_SelectedFieldText.ClientID %>").value = arrRetValues[0];
                    var listBox = document.getElementById("<%=lst_Select.ClientID%>");
                    FillList(arrRetValues[1], arrRetValues[0], listBox);
                    //$("#" + listBox).trigger("onchange");
                    var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                    $(document.getElementById(parentId + "_listBox")).trigger("onchange");

                    if ($("#<%= Hid_Postback.ClientID%>").val() == "True")
                        document.getElementById('<%= btn_Post.ClientID%>').click();
                }
            });
        }
    });


    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        $("img[id$='" + document.getElementById('<%= Hid_MyId.ClientID %>').value + '_btn_Del' + "']").bind("click", function () {
            document.getElementById("<%= Hid_AllFields.ClientID %>").value = "";
            document.getElementById("<%= Hid_SelectedId.ClientID %>").value = "";
            $("#<%= lst_Select.ClientID%>").empty();
        });

        var elmName = document.getElementById('<%= Hid_MyId.ClientID %>').value + "_btn_Search";
        var elm = $("a[id$='" + elmName + "']")[0];

        var evnt = 'click';
        var ret;

        if (elm.attachEvent) {
            elm.attachEvent('onclick', function () {
                var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                var ExtraParams = $("#<%= Hid_ExtraParams.ClientID%>").val().split('#');
                if (document.getElementById(parentId + "_chk_Select") != null) {
                    if (document.getElementById(parentId + "_chk_Select").checked == true) return false
                }
                for (var i = 0; i < ExtraParams.length - 1; i++) {
                    FillConditionalValues(parentId, ExtraParams[i], i.toString());
                }

                //alert($("#<%= Hid_ConditionalFieldValue.ClientID%>").val());
                var lstBox = document.getElementById(parentId + "_lst_Select");
                var texts = "";
                var values = "";
                var strPage = document.getElementById(parentId + "_Hid_StrPage").value;
                var cndFldValue = document.getElementById(parentId + "_Hid_ConditionalFieldValue").value;
                var cndFldValue1 = document.getElementById(parentId + "_Hid_ConditionalFieldValue1").value;
                var cndFldValue2 = document.getElementById(parentId + "_Hid_ConditionalFieldValue2").value;
                for (i = 0; i < lstBox.options.length; i++) {
                    texts = texts + lstBox.options[i].text + "!";
                    values = values + lstBox.options[i].value + "!";
                }
                texts = texts.replace("&", "^");
                var temp = texts;
                var intIndexOfMatch = temp.indexOf("&");
                while (intIndexOfMatch != -1) {
                    // Relace out the current instance.
                    temp = temp.replace("&", "^")
                    // Get the index of any next matching substring.
                    intIndexOfMatch = temp.indexOf("&");
                }

                texts = temp;
                $("#<%= Hid_SelectedId.ClientID%>").val(values);
                $("#<%= Hid_SelectedFieldText.ClientID%>").val(texts);
                var queryStr = "SelectFields.aspx?";

                queryStr = queryStr + "?SourceType=" + $("#<%= Hid_SrcType.ClientID%>").val() + "&TableName=" + $("#<%= Hid_TableName.ClientID%>").val()
                                + "&SelectedFieldName=" + $("#<%= Hid_SelectedFieldName.ClientID%>").val() + "&ProcName=" + $("#<%= Hid_ProcName.ClientID%>").val()
                + "&SelectedValueName=" + $("#<%= Hid_SearchFieldValue.ClientID%>").val() + "&SelectedValues=" + values
                + "&SelectedTexts=" + texts + "&CondExist=" + $("#<%= Hid_CondExist.ClientID%>").val()
                + "&CondFieldName=" + $("#<%= Hid_ConditionalFieldName.ClientID%>").val() + "&CondFieldValue=" + cndFldValue
                + "&ShowAll=" + $("#<%= Hid_ShowAll.ClientID%>").val() + "&CheckYearComp=" + $("#<%= Hid_CompYear.ClientID%>").val()
                + "&CondFieldName1=" + $("#<%= Hid_ConditionalFieldName1.ClientID%>").val() + "&CondFieldValue1=" + cndFldValue1
                + "&CondFieldName2=" + $("#<%= Hid_ConditionalFieldName2.ClientID%>").val() + "&CondFieldValue2=" + cndFldValue2
                + "&strPage=" + $("#<%= Hid_PageName.ClientID%>").val()

                ret = window.showModalDialog(queryStr, 'some argument', 'dialogWidth:300px;dialogHeight:550px;center:1;status:0;resizable:0;');
                if (typeof (ret) != 'undefined') {
                    var arrRetValues = ret.split('|');
                    document.getElementById("<%= Hid_SelectedId.ClientID %>").value = arrRetValues[1];
                    document.getElementById("<%= Hid_SelectedFieldText.ClientID %>").value = arrRetValues[0];

                    var listBox = document.getElementById("<%= lst_Select.ClientID%>");
                    FillList(arrRetValues[1], arrRetValues[0], listBox);
                    //$("#" + listBox).trigger("onchange");
                    var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                    $(document.getElementById(parentId + "_listBox")).trigger("onchange");
                    if ($("#<%= Hid_Postback.ClientID%>").val() == "True")
                        document.getElementById('<%= btn_Post.ClientID%>').click();
                }
            });
        }
        else {
            elm.addEventListener('click', function () {
                var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                var ExtraParams = $("#<%= Hid_ExtraParams.ClientID%>").val().split('#');
                if (document.getElementById(parentId + "_chk_Select") != null) {
                    if (document.getElementById(parentId + "_chk_Select").checked == true) return false
                }
                for (var i = 0; i < ExtraParams.length - 1; i++) {
                    FillConditionalValues(parentId, ExtraParams[i], i.toString());
                }
                var lstBox = document.getElementById(parentId + "_lst_Select");
                var texts = "";
                var values = "";
                var strPage = document.getElementById(parentId + "_Hid_StrPage").value;
                var cndFldValue = document.getElementById(parentId + "_Hid_ConditionalFieldValue").value;
                var cndFldValue1 = document.getElementById(parentId + "_Hid_ConditionalFieldValue1").value;
                var cndFldValue2 = document.getElementById(parentId + "_Hid_ConditionalFieldValue2").value;
                for (i = 0; i < lstBox.options.length; i++) {
                    texts = texts + lstBox.options[i].text + "!";
                    values = values + lstBox.options[i].value + "!";
                }
                texts = texts.replace("&", "^");
                var temp = texts;
                var intIndexOfMatch = temp.indexOf("&");
                while (intIndexOfMatch != -1) {
                    // Relace out the current instance.
                    temp = temp.replace("&", "^")
                    // Get the index of any next matching substring.
                    intIndexOfMatch = temp.indexOf("&");
                }

                texts = temp;
                $("#<%= Hid_SelectedId.ClientID%>").val(values);
                $("#<%= Hid_SelectedFieldText.ClientID%>").val(texts);
                var queryStr = "SelectFields.aspx?";

                queryStr = queryStr + "?SourceType=" + $("#<%= Hid_SrcType.ClientID%>").val() + "&TableName=" + $("#<%= Hid_TableName.ClientID%>").val()
                                + "&SelectedFieldName=" + $("#<%= Hid_SelectedFieldName.ClientID%>").val() + "&ProcName=" + $("#<%= Hid_ProcName.ClientID%>").val()
                + "&SelectedValueName=" + $("#<%= Hid_SearchFieldValue.ClientID%>").val() + "&SelectedValues=" + values
                + "&SelectedTexts=" + texts + "&CondExist=" + $("#<%= Hid_CondExist.ClientID%>").val()
                + "&CondFieldName=" + $("#<%= Hid_ConditionalFieldName.ClientID%>").val() + "&CondFieldValue=" + cndFldValue
                + "&ShowAll=" + $("#<%= Hid_ShowAll.ClientID%>").val() + "&CheckYearComp=" + $("#<%= Hid_CompYear.ClientID%>").val()
                + "&CondFieldName1=" + $("#<%= Hid_ConditionalFieldName1.ClientID%>").val() + "&CondFieldValue1=" + cndFldValue1
                + "&CondFieldName2=" + $("#<%= Hid_ConditionalFieldName2.ClientID%>").val() + "&CondFieldValue2=" + cndFldValue2
                + "&strPage=" + $("#<%= Hid_PageName.ClientID%>").val()

                ret = window.showModalDialog(queryStr, 'some argument', 'dialogWidth:300px;dialogHeight:550px;center:1;status:0;resizable:0;');
                if (typeof (ret) != 'undefined') {
                    var arrRetValues = ret.split('|');
                    document.getElementById("<%= Hid_SelectedId.ClientID %>").value = arrRetValues[1];
                    document.getElementById("<%= Hid_SelectedFieldText.ClientID %>").value = arrRetValues[0];

                    var listBox = document.getElementById("<%=lst_Select.ClientID%>");
                    FillList(arrRetValues[1], arrRetValues[0], listBox);
                    //$("#" + listBox).trigger("onchange");
                    var parentId = $("#<%= Hid_ControlId.ClientID%>").val();
                    $(document.getElementById(parentId + "_listBox")).trigger("onchange");
                    if ($("#<%= Hid_Postback.ClientID%>").val() == "True")
                        document.getElementById('<%= btn_Post.ClientID%>').click();
                }
            });
        }
    });


    function FillList(strId, strName, listBox) {
        var j = 0

        var arrId = strId.split('!');
        var arrName = strName.split('!');

        $(listBox).empty();
        if (arrId.length > 0) {
            for (j = 0; j < arrId.length; j++) {
                listBox.options.add(new Option(arrName[j], arrId[j]));
            }
        }
    }

    function FillConditionalValues(parentId, id, cnt) {
        var value = $("#" + id).val();
        if (cnt == 0) {
            cnt = "";
        }
        $("#" + parentId + "_Hid_ConditionalFieldValue" + cnt).val(value);
    }

</script>

<div>
    <table border="0">
        <tr style="text-align: center">
            <td>
                <a id="btn_Search" href="javascript:;" runat="server">Add & Remove</a>
                <asp:CheckBox ID="chk_Select" runat="server" Checked="true" TabIndex="14" CssClass="LabelCSS"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="lst_Select" runat="server" CssClass="TextBoxCSS uc_list" Width="18em"></asp:ListBox>
            </td>
        </tr>
    </table>
</div>
<asp:Button ID="btn_Post" runat="server" Style="display: none;" />
<i id="fnt_Mandatory" style="color: red" runat="server"></i>

<asp:HiddenField ID="Hid_SrcType" runat="server" />
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
<asp:HiddenField ID="Hid_CondExist" runat="server" />
<asp:HiddenField ID="Hid_CompYear" runat="server" />
<asp:HiddenField ID="Hid_ShowAll" runat="server" />

