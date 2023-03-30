<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendSecurityToDealer.aspx.cs"
    Inherits="Forms_SendSecurityToDealer" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<% Response.AddHeader("Pragma", "no-cache"); %> 
<% Response.AddHeader("Cache-Control", "no-cache"); %> 
<% Response.AddHeader("Cache-Control", "no-store"); %> 


<%-- <%@ PreviousPageType VirtualPath="~/Forms/DailyMarketValueEntry.aspx" %> --%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Send Security To Dealer</title>
    <base target="_self"/>   

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <style type="text/css"> 
       .hidden      {          display:none;      } 
       .txtbgcolorSel
        {
            background-color: #EBFFEE;
        }
        .trbgcolorSel
        {
            background-color: #EBFFEE;
        }    
    </style>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script id="tmplOptions" type="text/x-jquery-tmpl"> 
        <option value=${Value}>${Text}</option>
    </script>

    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            
            //debugger;  
           // window.resizeTo(800,400);
                                 
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
            });
            
            $("#btn_Close").click(function () {
                //debugger;
                Close();
            });            
            
//            $("#btn_Send").click(function () {
//                //debugger;
//                Validate();
//            });
        });       
        
        function Validate()
        {            
            //debugger;
            if ($('#srh_Staff_lst_Select option[value!=""]').size() > 0)
            {
                return true;
            }
            else
            {
                alert('Please Select Dealer !');
                return false;            
            }    
        } 
        
        function Close()
        {            
            window.returnValue = false;
            window.close()
            return false;
        }

        function CheckAll(checkVal) {
             
             //debugger;
             if(checkVal ==true)
             {
                $("input:checkbox","#dg_Selection").attr('checked','checked');
               // $("#dg_Selection tr.GridRowCSS").attr("bgcolor","#EBFFEE");
                $("#dg_Selection tr.GridRowCSS").addclass('trbgcolorSel');
                $("#dg_Selection tr.GridRowCSS").find('input').addClass("txtbgcolorSel"); 
                
             }
             else
             {
                 $("input:checkbox","#dg_Selection").removeAttr('checked') ;
                 //$("#dg_Selection tr.GridRowCSS").attr("bgcolor","#FFFFFF");  
                 $("#dg_Selection tr.GridRowCSS").removeClass('trbgcolorSel');  
                 $("#dg_Selection tr.GridRowCSS").find('input').removeClass('txtbgcolorSel'); 
                               
             }
        }

        function SelectRow(elm) {
            var checkVal = elm.checked;
            // alert('o');  
            if (checkVal == true) {
              // $(elm).closest('tr').attr("bgcolor","#EBFFEE");
               $(elm).closest('tr').addclass('trbgcolorSel');
            }
            else { 
               //debugger; 
               $(elm).closest('tr').removeClass('trbgcolorSel');  
               $(elm).closest('tr').find('input').removeClass('txtbgcolorSel');  
              
//               if($(elm).closest('tr').find('input[type=image]').length==1)
//               {
//                  // alert('44');  
//                  $(elm).closest('tr').attr("bgcolor","#FFFFFF"); 
//                  
//               }
//               else
//               {
//                 // alert('84');  
//                  $(elm).closest('tr').attr("bgcolor","#F5ECCE"); //#F5ECCE //#FFFFFF
//               }                        
            }
        }

        function CreateTrs() {        

            //debugger;
            
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

            //debugger;
            var rowCount = $('#tblRule tr').length;
            var strCond = "";
            var strCondFlds = "";

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
                        strCond = selCol + " LIKE '%" + selVal + "%'";
                        strCondFlds = selCol + "~" + selVal + ";";
                    }
                    else {
                        strCond = strCond + " " + opSel + " " + selCol + " LIKE '%" + selVal + "%'";
                        strCondFlds = strCondFlds + selCol + "~" + selVal + ";";
                    }
                }
            });

            $("#Hid_Cond").val(strCond);
            $("#Hid_CondColVal").val(strCondFlds);
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

                                 
             var str = '[{"Text":"SecurityName","Value":"1"},{"Text":"SecurityIssuer","Value":"1"},'
             + '{"Text":"SecurityTypeName","Value":"2"},{"Text":"MaturityDate","Value":"3"},'
             + '{"Text":"CouponRate","Value":"4"}]';
             
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

            var input = '<input type="text" id="input' + rowCount + '" style="width: 98%;" role="textbox" class="input-elm">';
            var ruleFieldValTd = $("<td class='colval'></td>");

            tr.append(ruleFieldValTd);


            ruleFieldValTd.append(input);
           // debugger;     
            var inputMinus = '<input type="button" style="width: 22px;" value="-" onclick=removeTr(' + rowCount + ',this)>';
            var ruleRemTd = $("<td></td>");

            tr.append(ruleRemTd);
            ruleRemTd.append(inputMinus);
            table.append(tr);
        }

        function removeTr(rowcount, elm) {
            $(elm).closest('tr').remove();
        }
        


    </script>

</head>
<body>
    <form id="form1" runat="server">
        <%-- <atlas:ScriptManager ID="ScriptManagerProxy1" runat="server" EnableViewState="true">
        </atlas:ScriptManager>--%>
        <div>
            <table id="Table1" width="90%" align="center" cellspacing="0" style="border: 0px;"
                cellpadding="0">
                <tr>
                    <td class="HeaderCSS" align="center" style="height: 19px;">
                        Selected Security
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="middle" width="100%">
                        <div id="div2" style="margin-top: 0px; overflow: auto; padding-top: 0px; position: relative;
                            margin-top: 20px; height:340px;" align="center">
                            <asp:GridView ID="dg_Selection" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                PageSize="15" CssClass="GridCSS" Width="100%" OnPageIndexChanging="dg_Selection_PageIndexChanging"
                                OnRowCommand="dg_Selection_RowCommand">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <RowStyle HorizontalAlign="Center" CssClass="GridRowCSS" Width="875px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Security Name">
                                        <HeaderStyle HorizontalAlign="center" Wrap="True" Width="330px" />
                                        <ItemTemplate>
                                         <asp:Label ID="lbl_StockUpdtId" Text='<%# DataBinder.Eval(Container, "DataItem.StockUpdtId") %>'
                                                runat="server" CssClass="hidden" />
                                            <asp:Label ID="Label1" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityName") %>'
                                                runat="server" CssClass="hidden" />
                                            <asp:TextBox ID="txt_SecurityName" Text='<%# DataBinder.Eval(Container, "DataItem.SecurityName") %>'
                                                runat="server" CssClass="TextBoxCSS" Width="95%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="ISIN Number" DataField="ISIN"  />
                                    <asp:BoundField HeaderText="SellingRate" DataField="SellingRate" />
                                   <%-- <asp:BoundField HeaderText="NoOfBonds" DataField="NoOfBonds" />--%>
                                    <asp:BoundField HeaderText="Show Quantity" DataField="ShowNumber" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="margin-top:10px;">
                            <tr id="tr_Emp">
                                <td class="LabelCSS" width="250">
                                    Dealers :</td>
                                <td style="width: 207px">
                                    <uc:SelectFields ID="srh_Staff" class="LabelCSS" runat="server" ProcName="ID_SEARCH_UserMaster"
                                        FormHeight="475" FormWidth="400" SelectedValueName="UM.UserId" ChkLabelName=""
                                        ConditionalFieldId="" LabelName="" SelectedFieldName="NameOfUser" SourceType="StoredProcedure"
                                        ConditionalFieldName="" Visible="true" ShowLabel="false" ShowAll="true"></uc:SelectFields>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btn_Send" runat="server" Text="Send" ToolTip="Send Mail" CssClass="ButtonCSS"
                            Height="20px" OnClick="btn_Send_Click" />
                       <%-- <input type="button" style="height: 20px;" class="ButtonCSS" title="Submit" id="btn_Send"
                            value="Send Mail" name="btn_Send" language="javascript" onclick="return btn_Send_onclick()">--%>
                        <%-- <asp:Button ID="btn_Close" runat="server" Text="Close" ToolTip="Close" CssClass="ButtonCSS"
                            Height="20px" />--%>
                            
                        <input type="button" style="height: 20px;" class="ButtonCSS" title="Close" id="btn_Close"
                            value="Close">
                        <asp:HiddenField ID="Hid_ColList" runat="server" />
                        <asp:HiddenField ID="Hid_Cond" runat="server" />
                        <asp:HiddenField ID="Hid_CondColVal" runat="server" />
                        <asp:HiddenField ID="Hid_ForDate" runat="server" />
                        <asp:HiddenField ID="Hid_SecIds" runat="server" />
                        <%--<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />--%>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
