<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="StockValuation.aspx.vb" Inherits="Forms_StockValuation" Title="Stock Update Detail"
    EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <style type="text/css">
        .hidden
        {
            display: none;
        }
        .trbgcolor
        {
            background-color: #F5ECCE;
        }
        .txtbgcolor
        {
            background-color: #F5ECCE;
        }
        .delbgcolor
        {
            background-color: #D8D8D8;
        }
        .txtbgcolorSel
        {
            background-color: #EBFFEE;
        }
        .trbgcolorSel
        {
            background-color: #EBFFEE;
        }
        .txtbgcolorW
        {
            background-color: #FFFFFF;
        }
        .DataGridFixedHeader
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
        }
    </style>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.core.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.datepicker.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            $("img.ui-datepicker-trigger").css("vertical-align", "middle");
            $("span.hidden:contains(True)[id$='_lbl_InStock']").closest('tr').addClass("trbgcolor"); 
            $("span.hidden:contains(True)[id$='_lbl_InStock']").closest('tr').find('input').addClass("txtbgcolor");  //.css({"background-color": "#EDCA7E"}); //.attr("background-color","#EDCA7E");
            
              //Send button click
            $("#btn_Send").click(function () {
                 SetSeurityIds();
            });             
           SetCheckGrid();          
        });
        
        function SetCheckGrid()
        {             
           if($("#ctl00_ContentPlaceHolder1_Hid_SecIds").val() !="")
           {
              var ids=$("#ctl00_ContentPlaceHolder1_Hid_SecIds").val().split(",");
              
              for (var k = 0; k < ids.length-1; k++) {                    
                   $('span[val="' + ids[k]  + '"]').closest('tr').addClass('trbgcolorSel');    //
                   $('span[val="' + ids[k]  + '"]').prev().attr('checked','checked');                    
               }
            }            
        }
        
        //to show graph
        function SetSeurityIds() {
            /************/
    
            var secids="";
                         
            $("input:checkbox:checked","#ctl00_ContentPlaceHolder1_dg_dme").next('.hidden').each(function(index) 
            {
                secids = secids + $(this).text() + ",";
            });             
            
            if(secids == "")
            {
                alert("Please select at least one option");
                return false;
            }
            else
            {                 
                  
                  var bflag=true;  
                  $("#ctl00_ContentPlaceHolder1_Hid_SecIdsSend").val(secids); 
                  
                  var bflag=true;  
                 
                  $("input:checkbox:checked","#ctl00_ContentPlaceHolder1_dg_dme").closest('tr').find('input[id$="_txt_Rate"]').each(function(index) 
                  {              
                        if($(this).val() == "0" || $(this).val()=="" || $(this).val()=="0.00" || $(this).val()=="0.0000")
                        {               
                            alert("Rate can't be 0 ?");
                            bflag=false;
                            return false;                           
                        }                                
                  });          
                  
                  if (bflag == false )
                  {return false ;}                  
                  
                  $("input:checkbox:checked","#ctl00_ContentPlaceHolder1_dg_dme").closest('tr').find('input[id$="_txt_ShowNumber"]').each(function(index) 
                  {             
//                        if($(this).val() == "0" || $(this).val()=="" || $(this).val()=="0.00")
//                        {                
//                            alert("Show Quantity can't be 0 ?");
//                            bflag=false;
//                            return false;   
//                        }                                
                   });   
            
                   if (bflag == false )
                   {
                        return false ;
                   }
                  
                  $.ajax({
                        async: true,
                        type: 'POST',
                        data: "secids=" + secids,
                        url: "StockUpdateDetailNew.aspx?operation=setSecurityId",
                        success: function (str) {
                           OpenMailDialog();                           
                        }                
                   });        
            }           
        }
                function IsAlphaNumeric(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
           
            return ret;
        }
        
        
        function validatesearch()
        {
            if(document.getElementById("ctl00_ContentPlaceHolder1_txt_Security").value == "")
            {
                alert("Please enter the Search");
                return false;
            }
            else
            {
                 return true;
            }
        }
        
        function Clear_Yeild(lnk)
        {
        
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_CFlag").value = "0";  /*On Clear Rate*/
         var col1; 
         var col2;
         var col3;
         var col4;
         var gridView = document.getElementById("<%=dg_dme.ClientID %>");
           var row = lnk.parentNode.parentNode;
                   var rowIndex = row.rowIndex;
                  
             col1 = gridView.rows[rowIndex].cells[4];
             col2 = gridView.rows[rowIndex].cells[5];
             col3 = gridView.rows[rowIndex].cells[6];
            
             for (j = 0; j < col1.childNodes.length; j++) {
                if (col1.childNodes[j].type == "text") {
                
                 col2.childNodes[j].value = parseFloat (0).toFixed (4);
                 col3.childNodes[j].value = parseFloat (0).toFixed (4);
               
                 
                     //document.getElementById(col1.childNodes[j].id).value = "0.0000";
                     document.getElementById(col2.childNodes[j].id).value = "0.0000";
                     document.getElementById(col3.childNodes[j].id).value = "0.0000";
                   
                     
                }
              }
        }   
        
        function Clear_Rate(lnk)
        {
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_CFlag").value = "1"; /*On Clear YTMAnn*/
         var col1; 
          var col2;
            var col3;
         var col4;
         var gridView = document.getElementById("<%=dg_dme.ClientID %>");
           var row = lnk.parentNode.parentNode;
                   var rowIndex = row.rowIndex;
                col1 = gridView.rows[rowIndex].cells[5];
                col2 = gridView.rows[rowIndex].cells[4];
                col3 = gridView.rows[rowIndex].cells[6];
        
             for (j = 0; j < col1.childNodes.length; j++) {
                if (col1.childNodes[j].type == "text") {
                    /*alert(col1.childNodes[j].value);*/
                 
                    col2.childNodes[j].value = parseFloat (0).toFixed (4);
                    col3.childNodes[j].value = parseFloat (0).toFixed (4);
                                      
                     document.getElementById(col2.childNodes[j].id).value = "0.0000";
                     document.getElementById(col3.childNodes[j].id).value = "0.0000";
                }
              }
        }
        
        function Clear_YTCRate(lnk)
        {
       
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_CFlag").value = "3"; /*On Clear YTCAnn*/
         var col1; 
          var col2;
                    var col3;
         var col4;
         var gridView = document.getElementById("<%=dg_dme.ClientID %>");
           var row = lnk.parentNode.parentNode;
                   var rowIndex = row.rowIndex;
             col1 = gridView.rows[rowIndex].cells[6];
                col2 = gridView.rows[rowIndex].cells[4];
                 col3 = gridView.rows[rowIndex].cells[5];
//                col4 = gridView.rows[rowI ndex].cells[13];
             for (j = 0; j < col1.childNodes.length; j++) {
                if (col1.childNodes[j].type == "text") {
                    /*alert(col1.childNodes[j].value);*/
                  
                     col2.childNodes[j].value = parseFloat (0).toFixed (4);
                   col3.childNodes[j].value = parseFloat (0).toFixed (4);
//                     col4.childNodes[j].value = parseFloat (0).toFixed (4);
                     
                  
                     document.getElementById(col2.childNodes[j].id).value = "0.0000";
                 document.getElementById(col3.childNodes[j].id).value = "0.0000";
//                     document.getElementById(col4.childNodes[j].id).value = "0.0000";
                }
              }
        }   
        
       function HideSendButton()
        {
       document.getElementById("btn_Send").disabled = true;
// document.getElementById('btn_Send').style.display = 'none';
        }
//        
       function ShowSendButton()
        {
        document.getElementById("btn_Send").disabled = false;
//document.getElementById('btn_Send').style.display = 'block';
        }
        
        function Success()
        {
            //alert("1");
        }
        
        function ContinueDelete()
        {
            var answer = confirm("Do you want to delete ?")
            if (answer) {
            return true;
            }
            return false;
       }
        
        function checkVal(elm)
        {          
            
             //debugger;
             var bflag = $(elm).closest('tr').find('input[id$="_Hid_Added"]').val().toLowerCase();             
             if( Number($(elm).val()) > Number($(elm).closest('td').prev().text()) && bflag != "true")
             {             
                 alert("Show Quantity can't be greater than No Of Bonds ?");
                 $(elm).val('0');                
                 return false;
             }   
             else
             {
               // $("#btn_Send").attr("disabled", "disabled");    
             }      
        }      
        
        function SaveIds()
        {
            
            //debugger;
            var secids="";
             
            $("input:checkbox:checked","#ctl00_ContentPlaceHolder1_dg_dme").next('.hidden').each(function(index) 
            {
                secids = secids + $(this).text() + ",";
            });             
            
            if(secids == "")
            {
                alert("Please select at least one optionsss");
                return false;
            }
            else
            {
                  
                  var bflag=true;  
                 
                  $("input:checkbox:checked","#ctl00_ContentPlaceHolder1_dg_dme").closest('tr').find('input[id$="_txt_SellingRate"]').each(function(index) 
                  {              
                        if($(this).val() == "0" || $(this).val()=="" || $(this).val()=="0.00" || $(this).val()=="0.0000")
                        {                
                            alert("Selling Rate can't be null ?");
                            bflag=false;
                            return false;
                        }                                
                  });          
                  
                  if (bflag == false )
                  {return false ;}
                 
                  $("input:checkbox:checked","#ctl00_ContentPlaceHolder1_dg_dme").closest('tr').find('input[id$="_txt_ShowNumber"]').each(function(index) 
                  {              
                        //debugger;                        
                        if($(this).val() == "0" || $(this).val()=="" || $(this).val()=="0.00")
                        {                
                            //debugger;
                            alert("Show Quantity can't be null ?");
                            bflag=false;
                            return false;   
                           /// break;                 
                        }                                
                   });   
            
                   if (bflag == false )
                   {
                        return false ;
                   }
                   
                  $("#ctl00_ContentPlaceHolder1_Hid_SecIdsSend").val(secids);                                    
            } 
            
            //alert('test');            
        }
        
        function postData() 
        {
               
           // debugger;   
            var secids;
             
            $("input:checkbox:checked","#ctl00_ContentPlaceHolder1_dg_dme").next('.hidden').each(function(index) 
            {
                secids = $(this).text() + ",";
            });             
            
            if(secids == "")
            {
                alert("Please select at least one option");
                return false;
            }        
           
            var saveData1 = $.post('SendMailToDealer.aspx', {secids: secids}, 
            function (result) {  
            
                var w = "780";
                var h = "500";
                var winl = (screen.width-w)/2;
                var wint = (screen.height-h)/2;
                if (winl < 0) winl = 0;
                if (wint < 0) wint = 0;
               
                windowprops = "height="+h+",width="+w+",top="+ wint +",left="+ winl +",location=no,"
                + "scrollbars=yes,menubars=yes,toolbars=yes,resizable=no,status=no";
               
                WinId = window.open('','newwin', windowprops);	
                WinId.document.open();             
                WinId.document.write(result);
                    
            }); 
            
            saveData1.error(function() { alert("Something went wrong"); });
                        
        }

        function OpenMailDialog() {

            var DialogOptions = "Center=Yes;Scrollbar=yes;dialogWidth=855px;dialogTop=200px;"
                                +"dialogHeight=540px;Help=No; Status=No;Resizable=Yes;menubar=Yes";
            var OpenUrl = "SendSecurityToDealer.aspx?fordate=" + $("#ctl00_ContentPlaceHolder1_txt_ForDate").val();

            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions)

            if (ret == "" || typeof (ret) == undefined || ret == false || ret == null) {
                return false;
            }
            else {
                return true;
            }
        }

        function CheckDelete(elm) {
            //debugger;
            var answer = confirm("Do you want to delete ?")

            if (answer) {
                $('#' + elm).next().text("D");
                $('#' + elm).next().next().val("D");
                $('#' + elm).closest('tr').attr("bgcolor", "#D8D8D8"); //666666
                $('#' + elm).closest('tr').find('input').addClass("delbgcolor");
                $('#' + elm).hide();
            }
            return false;
        }


        function CheckAll(checkVal) {

            //debugger;
            if (checkVal == true) {
                $("input:checkbox", "#ctl00_ContentPlaceHolder1_dg_dme").attr('checked', 'checked');
                $("#ctl00_ContentPlaceHolder1_dg_dme tr.GridRowCSS").addClass("trbgcolorSel"); 
                $("#ctl00_ContentPlaceHolder1_dg_dme tr.GridRowCSS").find('input').addClass("txtbgcolorSel"); 
            }
            else {
                $("input:checkbox", "#ctl00_ContentPlaceHolder1_dg_dme").removeAttr('checked');
                $("#ctl00_ContentPlaceHolder1_dg_dme tr.GridRowCSS").removeClass('trbgcolorSel'); 
                $("#ctl00_ContentPlaceHolder1_dg_dme tr.GridRowCSS").find('input').removeClass('txtbgcolorSel');               
            }
        }

        function SelectRow(elm) {
         
            //debugger;
            var checkVal = elm.checked;
            
            if (checkVal == true) {
               $(elm).closest('tr').addClass("trbgcolorSel"); 
               $(elm).closest('tr').find('input').addClass("txtbgcolorSel"); 
            }
            else { 
              
               $(elm).closest('tr').removeClass('trbgcolorSel');
               if($(elm).closest('tr').find('input[type=image]').length==1)
               {
                  $(elm).closest('tr').find('input').removeClass('txtbgcolorSel');
               }
               else
               {
                  $(elm).closest('tr').find('input').removeClass('txtbgcolorSel');
               }                        
            }
        }  
             
        function SecurityInfo() {

            var DialogOptions = "Center=Yes;Scrollbar=yes;dialogWidth=875px;dialogTop=200px;dialogHeight=580px;Help=No; Status=No;Resizable=Yes;menubar=Yes";
            var OpenUrl = "SecurityInfo.aspx?fordate=" + $("#ctl00_ContentPlaceHolder1_txt_ForDate").val();

            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions)

            if (ret == "" || typeof (ret) == undefined || ret == false || ret == null) {
                return false;
            }
            else {
                //alert(ret);
                $("#ctl00_ContentPlaceHolder1_Hid_SelVal").val(ret);
                return true;
            }
        }

        function ShowSearch(parentId, width, height, srcType, tblName, procName, selFldName, selFldValueName, cndFldName, cndExist, showAll, compYear, cndFldName1, cndFldName2) {

            //debugger;
            parentId = "ctl00_ContentPlaceHolder1";
            var pageUrl = "SelectFields.aspx";

            width = width + "px";
            height = height + "px";

            var selValues = ""
            var values = "";
            var texts = "";
            var strPage = document.getElementById(parentId + "_Hid_StrPage").value;

            var cndFldValue = document.getElementById(parentId + "_Hid_ConditionalFieldValue").value;
            var cndFldValue1 = document.getElementById(parentId + "_Hid_ConditionalFieldValue1").value;
            var cndFldValue2 = document.getElementById(parentId + "_Hid_ConditionalFieldValue2").value;

            texts = document.getElementById(parentId + "_Hid_SelText").value; //Hid_SelText
            values = document.getElementById(parentId + "_Hid_SelVal").value; //Hid_SelVal

            texts = texts.replace("&", "^");
            var temp = texts;
            var intIndexOfMatch = temp.indexOf("&");

            // Loop over the string value replacing out each matching
            // substring.
            while (intIndexOfMatch != -1) {
                // Relace out the current instance.
                temp = temp.replace("&", "^")
                // Get the index of any next matching substring.
                intIndexOfMatch = temp.indexOf("&");
            }

            texts = temp;

            //      alert(texts)
            var cndFldValue = document.getElementById(parentId + "_Hid_ConditionalFieldValue").value;
            pageUrl = pageUrl + "?SourceType=" + srcType + "&TableName=" + tblName
                          + "&SelectedFieldName=" + selFldName + "&ProcName=" + procName
                          + "&SelectedValueName=" + selFldValueName + "&SelectedValues=" + values
                          + "&SelectedTexts=" + texts + "&CondExist=" + cndExist
                          + "&CondFieldName=" + cndFldName + "&CondFieldValue=" + cndFldValue
                          + "&ShowAll=" + showAll + "&CheckYearComp=" + compYear
                          + "&CondFieldName1=" + cndFldName1 + "&CondFieldValue1=" + cndFldValue1
                          + "&CondFieldName2=" + cndFldName2 + "&CondFieldValue2=" + cndFldValue2
                          + "&strPage=" + strPage;


            var ret = ShowDialogOpen(pageUrl, width, height);
            if (typeof (ret) != "undefined") {
                document.getElementById(parentId + "_Hid_RetValues").value = ret;
                return true;
            }
            return false;

        }
    
    </script>

    <script type="text/javascript" language="javascript">
    
     function ValidateDate() {
     var today = new Date()
            var currDate = new Date(today.getFullYear(), today.getMonth(), today.getDate())
            //alert(currDate)
            var ForDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_ForDate")
            if ((ForDate.value) == "") {
                alert("Please enter For Date");
                return false;
            }
     }
//        function ValidateDate() {

//            //debugger;
//            var today = new Date()
//            var currDate = new Date(today.getFullYear(), today.getMonth(), today.getDate())
//            //alert(currDate)
//            var ForDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_ForDate")
//            if ((ForDate.value) == "") {
//                alert("Please enter For Date");
//                return false;
//            }

////            if (Date.parse(getmdy(ForDate.value)) < currDate) {
////                alert("You can not save Record For Previous Date");
////                return false;
////            }
//            
//            var secid="";

//            var grd = document.getElementById("ctl00_ContentPlaceHolder1_dg_dme")
//            var count = 0;
//            for (i = 1; i < grd.rows.length - 1; i++) {
//                var MktRate = grd.children[0].children[i].children[5].children[0].value;
//                var SellRate = grd.children[0].children[i].children[6].children[0].value;
//              
//                              
//              
////                var Qty = grd.rows[i].cells[12];
////                  for (j = 0; j < Qty.childNodes.length; j++) {
////                if (Qty.childNodes[j].type == "text") {
////                    /*alert(col1.childNodes[j].value);*/
////                               if (Qty.childNodes[j].value == 0)
////                  {
////                    count = parseInt (count) + parseInt (1)
////                  }
////                }
////              }
//             
////             if(count >0)
////             {
////             alert ("Quantity can not be zero, please enter proper quantity.");
////             return false;
////             }
//              
//                if (Number(MktRate) == 0 || Number(SellRate) == 0) {
//                    var strMsg = "Some Value of Market Rate and Selling Rate are Zero,Do You Want to Continue."
////                    if (window.confirm(strMsg) == false) {
////                        return false;
//                    }
//                    else {                  
//                            //debugger;
//                            $("input:checkbox:checked","#ctl00_ContentPlaceHolder1_dg_dme").next('.hidden').each(function(index) 
//                            {
//                                secid = secid + $(this).text() + ",";
//                            });                        
//                        
//                            $("#ctl00_ContentPlaceHolder1_Hid_SecIds").val(secid); 
//                            return true;
//                    }
//                }
//            }            
//        }
        
    </script>

    <%--    <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>--%>
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"  >
        <tr>
            <td class="HeaderCSS" align="center" colspan="2" style="height: 20px">
                Stock Update Details
            </td>
        </tr>
        <tr align ="center" >
            <%--<atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                <ContentTemplate>--%>
            <td style="height: 39px; padding-left:40px;" align ="center" >
                <table id="Table3" width="100%" align="center" cellspacing="0" cellpadding="0" border="0" >
                    <tr align="center" >
                        <td align="right">
                            For Date:
                        </td>
                        <td align="left" style="width: 125px" valign="middle">
                            <asp:TextBox ID="txt_ForDate" Width="70px" runat="server" CssClass="TextBoxCSS" TabIndex="9"></asp:TextBox>
                            <img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ForDate',this);">
                        </td>
                        <td align="left" colspan="4">
                            <asp:Button ID="btnView" runat="server" Text="View" CssClass="ButtonCSS" />
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4" align="center">
                            <asp:Label ID="lblMsg" ForeColor="Blue" runat="server" Visible="false" Text="" CssClass="LabelCSS"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                            <asp:Button ID="btn_Export" runat="server" CssClass="ButtonCSS" Text="Export" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <%--</ContentTemplate>
            </atlas:UpdatePanel>--%>
        </tr>
        <tr>
            <td align="center">
                <%-- <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>--%>
                <table id="Table2" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"
                    style="margin-top: 5px;">
                    <tr align ="center" >
                        <td align ="center" >
                            <%--<atlas:UpdatePanel ID="UpdatePanel2" runat="server" Mode="Conditional">
                                        <ContentTemplate>--%>
                            <div id="div1" style="margin-top: 0px; overflow: auto; width: 1000px; padding-top: 0px;
                                position: relative; height: 500px">
                                <%--OnPageIndexChanging="dg_dme_PageIndexChanging"--%>
                                <asp:GridView ID="dg_dme" runat="server" PageSize="15" AllowPaging="false" CssClass="GridCSS"
                                    OnPageIndexChanging="dg_dme_PageIndexChanging" OnRowDataBound="dg_dme_OnRowDataBound"
                                    OnRowCommand="dg_dme_RowCommand" ShowFooter="false" AutoGenerateColumns="False"
                                    TabIndex="38" Width="1000px" AllowSorting="true">
                                    <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS DataGridFixedHeader" />
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chk_AllItems" Width="10px" runat="server" onclick="CheckAll(this.checked)"
                                                    Checked="false"></asp:CheckBox>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_ItemChecked" runat="server" onclick="SelectRow(this)" Checked="false">
                                                </asp:CheckBox>
                                                <asp:Label ID="lbl_Security" Text='<%# DataBinder.Eval(Container,"DataItem.SecurityId") %>'
                                                    runat="server" val='<%# DataBinder.Eval(Container, "DataItem.SecurityId") %>'
                                                    CssClass="hidden" />
                                                <asp:Label ID="lbl_StockUpdtId" Text='<%# DataBinder.Eval(Container,"DataItem.SecurityEvalId") %>'
                                                    runat="server" val='<%# DataBinder.Eval(Container, "DataItem.SecurityEvalId") %>'
                                                    CssClass="hidden" />
                                                <asp:Label ID="lbl_TypeFlag" Text='<%# DataBinder.Eval(Container,"DataItem.TypeFlag") %>'
                                                    runat="server" val='<%# DataBinder.Eval(Container, "DataItem.TypeFlag") %>' CssClass="hidden" />
                                                <asp:Label ID="lbl_PerpetualFlag" Text='<%# DataBinder.Eval(Container,"DataItem.NatureOFInstrument") %>'
                                                    runat="server" val='<%# DataBinder.Eval(Container, "DataItem.NatureOFInstrument") %>'
                                                    CssClass="hidden"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Wrap="True" Width="20px" />
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="EditRow" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                    OnClientClick="return ContinueDelete();" CommandName="DeleteRow" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.SecurityId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Security Name" SortExpression="SecurityName">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_SecurityName" Text='<%# DataBinder.Eval(Container,"DataItem.SecurityName") %>'
                                                    runat="server" Width="350px" />
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_SecurityType" Text='<%# DataBinder.Eval(Container,"DataItem.SecurityTypeName") %>'
                                                    runat="server" Width="70px" />
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ISIN No" SortExpression="ISIN">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ISINNo" Text='<%# DataBinder.Eval(Container,"DataItem.ISIN") %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CallDate" SortExpression="CallDate" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CallDate" Text='<%# DataBinder.Eval(Container,"DataItem.CallDate") %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FaceValue" SortExpression="StockFaceValue">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_QTM" Text='<%# DataBinder.Eval(Container,"DataItem.StockFaceValue") %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" SortExpression="SellingRate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_SellingRate" Width="70px" runat="server" Text='<%# container.dataitem("SellingRate") %>'
                                                    CssClass="TextBoxCSS" onkeypress="OnlyDecimal();Clear_Yeild(this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="YTMAnn" SortExpression="Yield">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Yield" Width="50px" runat="server" Text='<%# container.dataitem("YTMAnn") %>'
                                                    CssClass="TextBoxCSS" onkeypress="OnlyDecimal();Clear_Rate(this);"></asp:TextBox>
                                                <div style="text-align: left">
                                                    <asp:Label ID="lbl_Yield" runat="server" Text='<%# container.dataitem("YTMAnn") %>'
                                                        Visible="false"></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="YTCAnn" SortExpression="YTCAnn">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_YTC" Width="50px" runat="server" Text='<%# container.dataitem("YTCAnn") %>'
                                                    CssClass="TextBoxCSS" onkeypress="OnlyDecimal();Clear_YTCRate(this);"></asp:TextBox>
                                                <div style="text-align: left">
                                                    <asp:Label ID="lbl_YTCAnn" runat="server" Text='<%# container.dataitem("YTCAnn") %>'
                                                        Visible="false"></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Semi_Ann_Flag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Semi_Ann_Flag") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CombineIPMat" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CombineIPMat") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Rate_Actual_Flag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Rate_Actual_Flag") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Equal_Actual_Flag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Equal_Actual_Flag") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_IntDays" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IntDays") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_FirstYrAllYr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FirstYrAllYr") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%-- </ContentTemplate>
                                    </atlas:UpdatePanel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="SeperatorRowCSS">
                        </td>
                    </tr>
                    <tr>
                        <asp:HiddenField ID="Hid_ForDate" runat="server" />
                        <asp:HiddenField ID="Hid_ColList" runat="server" />
                        <asp:HiddenField ID="Hid_RetValues" runat="server" />
                        <asp:HiddenField ID="Hid_SelectedId" runat="server" />
                        <asp:HiddenField ID="Hid_SelectedFieldIndex" runat="server" />
                        <asp:HiddenField ID="Hid_SelectedFieldText" runat="server" />
                        <asp:HiddenField ID="Hid_ConditionalFieldValue" runat="server" />
                        <asp:HiddenField ID="Hid_ConditionalFieldValue1" runat="server" />
                        <asp:HiddenField ID="Hid_ConditionalFieldValue2" runat="server" />
                        <asp:HiddenField ID="Hid_StrPage" runat="server" />
                        <asp:HiddenField ID="Hid_SelText" runat="server" />
                        <asp:HiddenField ID="Hid_SelVal" runat="server" />
                        <asp:HiddenField ID="Hid_SecIds" runat="server" />
                        <asp:HiddenField ID="Hid_MatDate" runat="server" />
                        <asp:HiddenField ID="Hid_MatAmt" runat="server" />
                        <asp:HiddenField ID="Hid_CallDate" runat="server" />
                        <asp:HiddenField ID="Hid_CallAmt" runat="server" />
                        <asp:HiddenField ID="Hid_CoupDate" runat="server" />
                        <asp:HiddenField ID="Hid_CoupRate" runat="server" />
                        <asp:HiddenField ID="Hid_PutDate" runat="server" />
                        <asp:HiddenField ID="Hid_PutAmt" runat="server" />
                        <asp:HiddenField ID="Hid_RateAmtFlag" runat="server" />
                        <asp:HiddenField ID="Hid_InterestDate" runat="server" />
                        <asp:HiddenField ID="Hid_BookClosureDate" runat="server" />
                        <asp:HiddenField ID="Hid_GovernmentFlag" runat="server" />
                        <asp:HiddenField ID="Hid_FaceValue" runat="server" />
                        <asp:HiddenField ID="Hid_Issue" runat="server" />
                        <asp:HiddenField ID="Hid_DMATBkDate" runat="server" />
                        <asp:HiddenField ID="Hid_MMYRate" runat="server" />
                        <asp:HiddenField ID="Hid_Frequency" runat="server" />
                        <asp:HiddenField ID="Hid_TypeFlag" runat="server" />
                        <asp:HiddenField ID="Hid_Issuer" runat="server" />
                        <asp:HiddenField ID="Hid_Security" runat="server" />
                        <asp:HiddenField ID="Hid_SecurityId" runat="server" />
                        <asp:HiddenField ID="Hid_Date" runat="server" />
                        <asp:HiddenField ID="Hid_SecIdsSend" runat="server" />
                        <asp:HiddenField ID="Hid_YTMAnn" runat="server" />
                        <asp:HiddenField ID="Hid_YTCAnn" runat="server" />
                        <asp:HiddenField ID="Hid_YTPAnn" runat="server" />
                        <asp:HiddenField ID="Hid_YTMSemi" runat="server" />
                        <asp:HiddenField ID="Hid_YTCSemi" runat="server" />
                        <asp:HiddenField ID="Hid_YTPSemi" runat="server" />
                        <asp:HiddenField ID="Hid_Rate" runat="server" />
                        <asp:HiddenField ID="Hid_YTMDate" runat="server" />
                        <asp:HiddenField ID="Hid_SecurityMatDate" runat="server" />
                        <asp:HiddenField ID="Hid_NextIntDate" runat="server" />
                        <asp:HiddenField ID="Hid_CFlag" runat="server" />
                    </tr>
                </table>
                <%-- </ContentTemplate>
                </atlas:UpdatePanel>--%>
            </td>
        </tr>
    </table>
    <%--   </ContentTemplate>
    </atlas:UpdatePanel>--%>
</asp:Content>
