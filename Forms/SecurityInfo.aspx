<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SecurityInfo.aspx.cs" Inherits="Forms_SecurityInfo"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Add Security</title>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />


    <script type="text/javascript" src="../Include/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>
    <script type="text/javascript" src="../Include/Script/showModalDialog.js"></script>

    <style type="text/css">
        .hidden {
            display: none;
        }

        .txtbgcolorSel {
            background-color: #EBFFEE;
        }

        .trbgcolorSel {
            background-color: #EBFFEE;
        }

        .lbl_Msg
        {
            padding:20px;
            margin:10px;
            font-weight:bold;
        }
    </style>


    <script id="tmplOptions" type="text/x-jquery-tmpl">
        <option value="${Value}">${Text}</option>
    </script>

    <script language="javascript">

        $(document).ready(function () {
            $('.pager a').click(function () { GetCondition(); });
            //debugger;                       
            CreateTrs();

            $("#btnAdd").click(function () {
                //debugger;
                appendTr();
            });

            //btn_Search
            $("#btn_Search").click(function () {
                //debugger;
                GetCondition();
            });

            $("#btn_ShowAll").click(function () {
                //debugger;
                $("#Hid_Cond").val("");
                $("#Hid_CondColVal").val("");
                $("#Hid_Operator").val("");
            });


        });

        function Submit() {
            $("input:checkbox:checked", "#dg_Selection").next('.hidden').append(",");
            var secids = $("input:checkbox:checked", "#dg_Selection").next('.hidden').text();
            var lottype = $("input:checkbox:checked", "#dg_Selection").next('.hidden').next('.hidden').next('.hidden').text();
            $("input:checkbox:checked", "#dg_Selection").next('.hidden').next('.hidden').append(",");
            var secname = $("input:checkbox:checked", "#dg_Selection").next('.hidden').next('.hidden').text();

            if (secids == "") {
                alert("Please select at least one option");
                window.returnValue = 'E';
                window.close();
                return false;
            }

            var slds = $("#Hid_AddedSec").val();
            var arr = []; arr = slds.split('|');
            var arrname = []; arrname = secname.split(',');
            var arrsecids = secids.split(',');
            //for (var j = 0; j < arrsecids.length - 1; j++) {
            //    for (var i = 0; i < arr.length - 1; i++) {
            //        if (arrsecids[j] == arr[i]) {
            //            alert("Security " + arrname[j] + " is Already Added.");
            //            window.returnValue = 'E';
            //            window.close();
            //            return false;
            //        }
            //    }
            //}
            $("#<%= Hid_Id.ClientID %>").val(secids + "!!" + lottype);
            window.returnValue = $("#<%= Hid_Id.ClientID %>").val();
            window.close();
        }

        function Close() {
            window.returnValue = "";
            window.close()

        }

        function CheckAll(checkVal) {

            //debugger;
            if (checkVal == true) {
                $("input:checkbox", "#dg_Selection").attr('checked', 'checked');
                // $("#dg_Selection tr.GridRowCSS").attr("bgcolor","#EBFFEE");
                $("#dg_Selection tr.GridRowCSS").addClass('trbgcolorSel');
                $("#dg_Selection tr.GridRowCSS").find('input').addClass('txtbgcolorSel');

            }
            else {
                $("input:checkbox", "#dg_Selection").removeAttr('checked');
                //$("#dg_Selection tr.GridRowCSS").attr("bgcolor","#FFFFFF");     
                $("#dg_Selection tr.GridRowCSS").removeClass('trbgcolorSel');
                $("#dg_Selection tr.GridRowCSS").find('input').removeClass('txtbgcolorSel');

            }
        }

        function SelectRow(elm) {
            var checkVal = elm.checked;
            // alert('o');  
            if (checkVal == true) {
                //$(elm).closest('tr').attr("bgcolor","#EBFFEE");
                $(elm).closest('tr').addClass('trbgcolorSel');
                $(elm).closest('tr').find('input').addClass('txtbgcolorSel');
            }
            else {
                //debugger; 
                //$(elm).closest('tr').attr("bgcolor","#FFFFFF");
                $(elm).closest('tr').removeClass('trbgcolorSel');
                $(elm).closest('tr').find('input').removeClass('txtbgcolorSel');
            }
        }

        function CreateTrs() {



            $("#opSel").val($("#Hid_Operator").val());

            var flds = $("#Hid_CondColVal").val();
            if (flds != "") {
                var arrflds = flds.split(';');

                $.each(arrflds, function (index, value) {
                    // alert(index + ': ' + value); 


                    if (value != "") {
                        var arrColVal = value.split('~');

                        appendTr();

                        // debugger;                     

                        $("#sel" + index + " option", "#tblRule").each(function () {
                            // debugger;
                            //alert($(this).text());                            
                            if ($(this).text() == arrColVal[0]) {
                                $(this).attr('selected', 'selected');
                            }
                        });

                        $("#input" + index, "#tblRule").val(arrColVal[1]);
                    }
                });
            }

        }

        function GetCondition() {
            var rowCount = $('#tblRule tr').length;
            var strCond = "";
            var strCondFlds = "";
            var SecIds = "";
            var trs = $('#tblRule tr');

            $.each(trs, function (i, elm) {
                //debugger;  
                var opSel = $("#opSel option:selected").text();   //opSel 
                var selid = elm.children[0].children[0].id;
                var inputId = elm.children[1].children[0].id;
                var selVal = $("#" + inputId).val();
                var selCol = $("#" + selid + " option:Selected").text()
                if (selCol != "" && selVal != "") {
                    if (strCond == "") {
                        strCond = selCol + " LIKE '%" + selVal.trimLeft().trimRight() + "%'";
                        strCondFlds = selCol + "~" + selVal.trimLeft().trimRight() + ";";
                    }
                    else {
                        strCond = strCond + " " + opSel + " " + selCol + " LIKE '%" + selVal.trimLeft().trimRight() + "%'";
                        strCondFlds = strCondFlds + selCol + "~" + selVal.trimLeft().trimRight() + ";";
                    }
                }
                SecIds += selVal.trimLeft().trimRight() + "\r\n"
            });

            SecIds = "NSDLAcNumber\r\n" + SecIds;
            $("#<%=Hid_CondSecIds.ClientID%>").val(SecIds);


            $("#Hid_Cond").val(strCond);
            $("#Hid_CondColVal").val(strCondFlds);
            $("#Hid_Operator").val($("#opSel option:selected").val());

            return true;

        }

        function SecurityInfo() {

            // debugger;
            var DialogOptions = "Center=Yes; Scrollbar=yes; dialogWidth=875px; dialogTop=200px; dialogHeight=440px; Help=Yes; Status=Yes; Resizable=Yes;"
            var OpenUrl = "SecurityInfo.aspx"
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions)

            if (ret == "" || typeof (ret) == "undefined") {
                return false
            }
            else {
                return true
            }
        }

        function appendTr() {


            var str = '[{"Text":"ISINNumber","Value":"6"},{"Text":"SecurityName","Value":"1"},{"Text":"SecurityIssuer","Value":"2"},'
            + '{"Text":"SecurityTypeName","Value":"3"},{"Text":"MaturityDate","Value":"4"},'
            + '{"Text":"CouponRate","Value":"5"}]';

            //SecurityName,SecurityTypeName,SecurityIssuer,MaturityDate,CouponRate             

            var data = eval('(' + str + ')');

            var table = $("#tblRule");
            var rowCount = $('#tblRule tr').length;

            //table.append(
            //      this.createTableRowForRule()
            //);

            /*
            * Create the rule data for the filter
            */
            // save current entity in a variable so that it could
            // be referenced in anonimous method calls

            var that = this, tr = $("<tr id='tr" + rowCount + "'></tr>"),
            //document.createElement("tr"),

            // first column used for padding
            //tdFirstHolderForRule = document.createElement("td"),
			    i, op, trpar, cm, str = "", selected;
            //tdFirstHolderForRule.setAttribute("class", "first");
            // tr.append("<td class='first'></td>");

            // create field container
            var ruleFieldTd = $("<td class='columns'></td>");
            tr.append(ruleFieldTd);

            // dropdown for: choosing field
            var ruleFieldSelect = $("<select id='sel" + rowCount + "' class='select-elm'></select>"), ina, aoprs = [];

            var strOptions = "";

            $.each(data, function (i, elm) {
                strOptions = strOptions + "<option value=" + elm.Value + ">" + elm.Text + "</option>";
            });

            $(ruleFieldSelect).append(strOptions);

            //$("#tmplOptions").tmpl(data).appendTo(ruleFieldSelect);

            ruleFieldTd.append(ruleFieldSelect);
            tr.append(ruleFieldTd);

            var input = '<input type="text" id="input' + rowCount + '" style="width: 120px;" role="textbox" class="input-elm">';
            var ruleFieldValTd = $("<td class='colval'></td>");

            tr.append(ruleFieldValTd);


            ruleFieldValTd.append(input);
            // debugger;     
            var inputMinus = '<input ID="Rem' + rowCount + '" type="button" style="width: 22px;" value="-" class="ButtonCSS" >';

            var ruleRemTd = $("<td></td>");

            tr.append(ruleRemTd);
            ruleRemTd.append(inputMinus);

            $("#Rem" + rowCount).live("click", function () {
                //alert( "Goodbye!" ); // jQuery 1.3+
                removeTr(rowCount, this);

            });

            table.append(tr);
        }

        function removeTr(rowcount, elm) {
            $(elm).closest('tr').remove();
        }


        function AlertISINMissing(isin)
        {
            AlertMessage("Message", "ISIN's " + isin +  " Not found." , 450, 750);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <%-- <atlas:ScriptManager ID="ScriptManagerProxy1" runat="server" EnableViewState="true">
        </atlas:ScriptManager>--%>
        <div>
            <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td class="HeaderCSS" align="center" colspan="2" style="height: 19px">Security Selection
                    </td>
                </tr>
                <tr>
                    <td class="SubHeaderCSS" width="50%">Selection Details
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <%--<td class='first'></td>--%>
                                <td>Select Fields
                                </td>
                                <td>
                                    <select id="opSel" style="width: 60px;">
                                        
                                        <option value="or">or</option>
                                        <option value="and">and</option>
                                    </select>
                                </td>
                                <td>
                                    <input id="btnAdd" type="button" title="Add rule" value=" + " name="btnAdd" class="ButtonCSS"
                                        style="width: 22px;" />
                                </td>
                            </tr>
                        </table>
                        <table id="tblRule">
                        </table>
                        <%-- <asp:TextBox ID="t1" runat="server"></asp:TextBox></td>--%>
                    </td>
                </tr>
                <tr style="margin-bottom: 10px;">
                    <td align="center" style="padding: 10px">
                        <asp:Button ID="btn_Search" runat="server" CssClass="ButtonCSS" Text="Search" OnClick="btn_Search_Click" />
                        <asp:Button ID="btn_ShowAll" runat="server" CssClass="ButtonCSS" Text="Show All"
                            OnClick="btn_ShowAll_Click" />
                        <asp:HiddenField ID="Hid_ColList" runat="server" />
                        <asp:HiddenField ID="Hid_Cond" runat="server" />
                        <asp:HiddenField ID="Hid_CondColVal" runat="server" />
                        <asp:HiddenField ID="Hid_ForDate" runat="server" />
                        <asp:HiddenField ID="Hid_Operator" runat="server" />
                        <asp:HiddenField ID="Hid_SecIds" runat="server" />
                        <asp:HiddenField ID="Hid_AddedSec" runat="server" />
                        <asp:HiddenField ID="Hid_IsGsec" runat="server" />
                        <asp:HiddenField ID="Hid_IsTypeFlag" runat="server" />
                        <asp:HiddenField ID="Hid_Id" runat="server" />
                        <asp:HiddenField ID="Hid_ErrorCode" runat="server" />
                        <asp:HiddenField ID="Hid_CondSecIds" runat="server" />
                    </td>
                </tr>
                <tr style ="padding-bottom:10px;">
                    <td>
                        <asp:Label ID="lbl_Message" runat ="server" Text ="" CssClass ="LabelCSS lbl_Msg"></asp:Label>
                    </td>
                </tr>
                <tr style ="padding-top:10px;">
                    <td align="center" valign="middle">
                        <div id="div2" style="margin-top: 10px; overflow: auto; width: 900px; padding-top: 0px; position: relative; height: 370px"
                            align="center">
                            <asp:GridView ID="dg_Selection" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                PageSize="15" CssClass="GridCSS" Width="900px" OnPageIndexChanging="dg_Selection_PageIndexChanging"
                                OnRowCommand="dg_Selection_RowCommand">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <RowStyle HorizontalAlign="Center" CssClass="GridRowCSS" Width="900px" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chk_AllItems" Width="10px" runat="server" onclick="CheckAll(this.checked)"
                                                Checked="false"></asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_ItemChecked" runat="server" onclick="SelectRow(this)" Checked="false"></asp:CheckBox>
                                            <asp:Label Text='<%# DataBinder.Eval(Container, "DataItem.SecurityId") %>' runat="server"
                                                CssClass="hidden" />
                                            <asp:Label Text='<%# DataBinder.Eval(Container, "DataItem.SecurityName") %>' runat="server"
                                                CssClass="hidden" />
                                            <asp:Label Text='<%# DataBinder.Eval(Container, "DataItem.LotFlag").ToString() == "" ? " " : DataBinder.Eval(Container, "DataItem.LotFlag").ToString() == "M" ? "Market Lot" : "Odd Lot"%>' runat="server"
                                                CssClass="hidden" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Wrap="True" Width="20px" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SecurityName">
                                        <ItemTemplate>
                                            <%--<asp:TextBox ID="txt_SecurityName" Width="200px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                onkeypress="scroll();" runat="server"
                                                CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityName") %>'></asp:TextBox>--%>
                                            <asp:Label ID="txt_SecurityName" Width="200px" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.SecurityName") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="SecurityType" DataField="SecurityTypeName" />
                                    <asp:BoundField HeaderText="ISIN" DataField="NSDLAcNumber" />


                                    <asp:BoundField HeaderText="Abbreviation" DataField="Abbreviation" Visible="false" />
                                    <asp:TemplateField HeaderText="SecurityIssuer">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_SecurityIssuer" Width="200px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                onkeypress="scroll();" runat="server"
                                                CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityIssuer") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="MaturityDate" DataField="MatDate" />
                                    <asp:BoundField HeaderText="CallDate" DataField="CallDate" />
                                    <asp:BoundField HeaderText="CouponRate" DataField="CouponRate" />
                                    <asp:TemplateField HeaderText="Lot Type" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_LotType" Width="100px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                onkeypress="scroll();" runat="server"
                                                CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.LotFlag").ToString() == "" ? " " : DataBinder.Eval(Container, "DataItem.LotFlag").ToString() == "M" ? "Market Lot" : "Odd Lot"%>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="pager" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <input type="button" id="btn_Submit" class="ButtonCSS" value="Submit" onclick="return Submit();" />
                        <input type="button" style="height: 20px;" class="ButtonCSS hidden" title="Close" id="btn_Close"
                            value="Close" onclick="return Close();" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
