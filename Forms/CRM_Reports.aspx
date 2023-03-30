<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="CRM_Reports.aspx.vb" Inherits="Forms_CRM_Reports" Title="CRM Reports" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/calendar.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/jquery-1.8.0.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery-ui-1.8.23.custom.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.core.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.datepicker.js"></script>

    <script language="javascript" type="text/javascript">
      $(document).ready(function () {
           var  rptName = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ReportType").value
             if(rptName != "RetailDebitRpt")
             {
                    jQuery("#ctl00_ContentPlaceHolder1_txt_FromDate").attr("onchange" , "");
                    jQuery("#ctl00_ContentPlaceHolder1_txt_ToDate").attr("onchange" , "");
             }
            $("#fromCalendar").click( function () {
            
                displayDatePicker('ctl00_ContentPlaceHolder1_txt_FromDate',this);
                if( rptName =="RetailDebitRpt")
                {
                     FillConditionalValue1('ctl00_ContentPlaceHolder1_srh_RetailDebit','ctl00_ContentPlaceHolder1_txt_FromDate');
                }
                
            });
            $("#toCalendar").click( function () {
                displayDatePicker('ctl00_ContentPlaceHolder1_txt_ToDate',this);
                if( rptName =="RetailDebitRpt")
                {
                    FillConditionalValue2('ctl00_ContentPlaceHolder1_srh_RetailDebit','ctl00_ContentPlaceHolder1_txt_ToDate');
                }               
               
            });
            
        });
         
      function ShowList(fieldName,valueName,procName,selValues)
        {
            var ret = ShowDialogOpen("SelectUsers.aspx?FieldName="+fieldName+"&ValueName="+valueName+"&ProcName="+procName+"&SelectedValues="+selValues,"250px","498px")
            return ret
        }
        
        function SelectChkBox(chkBoxId,lnkBtnId,lstId)
        {
            //alert(SelectChkBox)
            if(document.getElementById("ctl00_ContentPlaceHolder1_"+chkBoxId) != null)
            {
                //alert(document.getElementById("ctl00_ContentPlaceHolder1_"+chkBoxId).checked)
                if(document.getElementById("ctl00_ContentPlaceHolder1_"+chkBoxId).checked == true)
                {
                    removeAllOptions(document.getElementById("ctl00_ContentPlaceHolder1_"+lstId))
                    document.getElementById("ctl00_ContentPlaceHolder1_"+lnkBtnId).disabled = true
                }
                else
                {
                    document.getElementById("ctl00_ContentPlaceHolder1_"+lnkBtnId).disabled = false
                }
            }
        }
        
        function CheckSelect()
        {
            //alert("CheckSelect")
            SelectChkBox('chk_Customer', 'lnk_AddRemCustomer', 'lst_Customer')        
            SelectChkBox('chk_Finish', 'lnk_AddRemFinish', 'lst_Finish')
            SelectChkBox('chk_Brand', 'lnk_AddRemBrand', 'lst_Brand')         
            SelectChkBox('chk_Preparedby', 'lnk_AddRemPreparedby', 'lst_Preparedby')
            SelectChkBox('chk_OrderNo', 'lnk_AddRemOrderNo', 'lst_OrderNo')
            SelectChkBox('chk_ZipCode', 'lnk_AddRemZipCode', 'lst_ZipCode')
            SelectChkBox('chk_Size', 'lnk_Size', 'lst_Size')
            SelectChkBox('chk_ZipColour', 'lnk_AddRemZipColour', 'lst_ZipColour')            
        }
        
        function removeAllOptions(selectbox)
        {
            var count;
            var optionCnt = selectbox.options.length
            for(count = 0; count < optionCnt; count++)
            {
                selectbox.remove(0);
            }
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="SectionHeaderCSS" align="left" colspan="4">
                MAIN SECTION
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table2" align="right" cellspacing="0" cellpadding="0" border="0" width="80%">
                    <tr align="left" id="row_FromDate" runat="server">
                        <td>
                            From Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS" Width="115px"
                                TabIndex="1"></asp:TextBox><img class="calender" id="fromCalendar" src="../Images/Calender.jpg" />
                        </td>
                    </tr>
                    <tr align="left" id="row_ToDate" runat="server">
                        <td>
                            To Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS" Width="115px" TabIndex="2"></asp:TextBox><img
                                class="calender" id="toCalendar" src="../Images/Calender.jpg" />
                        </td>
                    </tr>
                    <tr align="left" id="row_Staff" runat="server" visible="false">
                        <td>
                            Select Staff:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_Staff" runat="server" ProcName="ID_SEARCH_UserMaster" FormHeight="475"
                                FormWidth="257" SelectedValueName="UM.UserId" ChkLabelName="" ConditionalFieldId=""
                                LabelName="" SelectedFieldName="NameOfUser" SourceType="StoredProcedure" ConditionalFieldName=""
                                Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_CustomerType" runat="server" visible="false">
                        <td>
                            <%--<asp:Label ID="lbl_CustomerType" runat="server" Text="Select Customer Type:" CssClass="LabelCSS"></asp:Label>--%>
                            Select Customer Type:
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_CustomerType" runat="server" ProcName="ID_SEARCH_CustomerType"
                                FormHeight="475" FormWidth="257" SelectedValueName="CustomerTypeId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerTypeName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_customer" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lbl_Customer" runat="server" Text="Select Customer:" CssClass="LabelCSS"></asp:Label>
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_CustomerName" runat="server" ProcName="ID_SEARCH_CustomerMasterNew"
                                FormHeight="470" FormWidth="257" SelectedValueName="CM.CustomerId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left" id="row_customer2" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lbl_CustomerTypeName" runat="server" Text=" Select Customer Type:"
                                CssClass="LabelCSS"></asp:Label>
                        </td>
                        <td style="padding-left: 0px;">
                            <uc:SelectFields ID="srh_CustomerTypeName" class="LabelCSS" runat="server" ProcName="ID_SEARCH_CustomerType"
                                FormHeight="475" FormWidth="257" SelectedValueName="CustomerTypeId" ChkLabelName=""
                                ConditionalFieldId="" LabelName="" SelectedFieldName="CustomerTypeName" SourceType="StoredProcedure"
                                ConditionalFieldName="" Visible="true" ShowLabel="false"></uc:SelectFields>
                        </td>
                    </tr>
                    <tr align="left">
                        <td align="right">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_Print" runat="server" Text="View Report" CssClass="ButtonCSS"
                                TabIndex="19" Width="90px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="Hid_ReportType" runat="server" />
</asp:Content>
