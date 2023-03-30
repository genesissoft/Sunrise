<%@ Page Language="C#" CodeFile="securityipdates.aspx.cs" Inherits="Forms_securityipdates"
    AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>IP Dates</title>
    <meta http-equiv="X-UA-Compatible" content="IE=9; IE=8; IE=7" />
    <link type="text/css" href="../Include/Style_IPO.css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/Script/jquery-1.8.0.min.js" language="javascript"></script>

    <script type="text/javascript" src="../Include/Common.js" language="javascript"></script>

    <style type="text/css">
        body
        {
            font-size: 0.8em;
        }
        td
        {
            line-height: 20px;
        }
        .hide
        {
            display: none;
        }
        .addrow
        {
            cursor: pointer;
            float: left;
        }
        .deleterow
        {
            cursor: pointer;
            float: right;
        }
        .editdate, .editnumber
        {
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $(document).on('click', 'td.editdate', function () {
                GetCellValue(this,'d');
            });
            $(document).on('click', 'td.editnumber', function () {
                GetCellValue(this,'n');
            });
            
            $(document).on('click', '.addrow', function () {
                var row = $(this).closest('tr');
                var strHTML='<tr>';
                strHTML += '<td class="hide">0</td>';
                strHTML+= '<td class="editdate" align="center"></td>';
                strHTML+= '<td class="editnumber" align="right">0</td>';
                strHTML+= '<td class="editnumber" align="right">0</td>';
                strHTML+= '<td align="right"></td>';
                strHTML+= '<td>';
                strHTML+= '<select name="cboDivisor" id="cboDivisor">';
			    strHTML+= '<option value="365">365</option>';
			    strHTML+= '<option value="366">366</option>';
			    strHTML+= '<option value="360">360</option>';
		        strHTML+= '</select>';
                strHTML+= '</td>';
                strHTML+= '<td><label class="addrow" title="add new row">+</label>&nbsp;<label class="deleterow" title="delete this row">-</label></td>';
                strHTML+= '</tr>';
                $(row).after(strHTML);
            });
            
            $(document).on('click', '.deleterow', function () {
                var row = $(this).closest('tr');
                $(row).remove();
            });
        });
    
    var flag = 0;
    function GetCellValue(cnt,type) {
        if (flag == 0) {
            strData = cnt.innerText;
            if(type=="d")
            cnt.innerHTML = "<input type='text' class='jsdate' style='padding:2px; border:0px; width:97%; background-color:#F7FFE4;' onblur=SetCellValue(this,'d') onkeypress='javascript:return OnlyDateKey(event);' maxlength='10' />";
            else if (type=="n")
            cnt.innerHTML = "<input type='text' style='padding:2px; border:0px; width:97%; background-color:#F7FFE4;' onblur=SetCellValue(this,'n') onkeypress='javascript:return OnlyNumericKey(event);' maxlength='10' />";

            $(cnt).find("input").get(0).focus();
            $(cnt).find("input").get(0).value = trim(strData);
            flag = 1;
        }
    }
     
    function SetCellValue(cnt,type) {
        if (flag == 1 && ((type=='d' && Validate_Date(cnt.value)) || (type=='n' && Validate_Number(cnt.value)))) {
            $(cnt).closest("td").text(cnt.value);
            flag = 0;
        }
        else{
            $(cnt).css("background-color", "#FFBABA");
        }
    }
    
    function UpdateDetails() {
        try {
            var strData = '';
            var id=$("#<%= Hid_Id.ClientID%>").val();
            var ipdate,ipamount,ipamount1;
            var ret=true;
            
            $("#<%= dgInterestDetails.ClientID %> tr").each(function (i, row) {
                if(i==0)
                return;
                
                ipdate=$($(row).find('td').get(1)).text();
                ipamount=$($(row).find('td').get(2)).text();
                ipamount1=$($(row).find('td').get(3)).text();
                
                if (!Validate_Date(ipdate)){
                    alert('Please enter correct value for ip date');
                    ret=false;
                    return false;
                }
                else if (!Validate_Number(ipamount) || !Validate_Number(ipamount1)){
                    alert('Please enter correct value for ip amount');
                    ret=false;
                    return false;
                }
                else{
                    strData = strData + '{';
                    strData = strData + '"Id":"' + $($(row).find('td').get(0)).text() + '",';
                    strData = strData + '"IPDate":"' + ipdate + '",';
                    strData = strData + '"IPAmount":"' + ipamount + '",';
                    strData = strData + '"IPAmount1":"' + ipamount1 + '",';
                    strData = strData + '"Days":"' + $($(row).find('td').get(4)).text() + '",';
                    strData = strData + '"PaymentType":"I",';
                    strData = strData + '"Divisor":"' + $(row).find("select option:selected").val() + '"';
                    strData = strData + '},';
                }
            });
            
            if (!ret)
            return ret
            
            if(ipdate != $("#<%= Hid_MaturityDate.ClientID%>").val()){
                alert('Last ip date and last maturity/call date must be same.');
                return false;
            }
            else if (!Check_Date_Order()){
                alert('IP dates must be in ascending order.');
                return false;
            }
            
            $("#<%= dgMaturityDetails.ClientID %> tr").each(function (i, row) {
                if(i==0)
                return;
                
                strData = strData + '{';
                strData = strData + '"Id":"' + $($(row).find('td').get(0)).text() + '",';
                strData = strData + '"IPDate":"' + $($(row).find('td').get(1)).text() + '",';
                strData = strData + '"IPAmount":"' + $($(row).find('td').get(2)).text() + '",';
                strData = strData + '"IPAmount1":"0",';
                strData = strData + '"Days":"' + $($(row).find('td').get(3)).text() + '",';
                strData = strData + '"PaymentType":"M",';
                strData = strData + '"Divisor":""';
                strData = strData + '},';
            });
            
            $("#<%= dgCallDetails.ClientID %> tr").each(function (i, row) {
                if(i==0)
                return;
                
                strData = strData + '{';
                strData = strData + '"Id":"' + $($(row).find('td').get(0)).text() + '",';
                strData = strData + '"IPDate":"' + $($(row).find('td').get(1)).text() + '",';
                strData = strData + '"IPAmount":"' + $($(row).find('td').get(2)).text() + '",';
                strData = strData + '"IPAmount1":"0",';
                strData = strData + '"Days":"' + $($(row).find('td').get(3)).text() + '",';
                strData = strData + '"PaymentType":"C",';
                strData = strData + '"Divisor":""';
                strData = strData + '},';
            });
            
             $("#<%= dgPutDetails.ClientID %> tr").each(function (i, row) {
                if(i==0)
                return;
                
                strData = strData + '{';
                strData = strData + '"Id":"' + $($(row).find('td').get(0)).text() + '",';
                strData = strData + '"IPDate":"' + $($(row).find('td').get(1)).text() + '",';
                strData = strData + '"IPAmount":"' + $($(row).find('td').get(2)).text() + '",';
                strData = strData + '"IPAmount1":"0",';
                strData = strData + '"Days":"' + $($(row).find('td').get(3)).text() + '",';
                strData = strData + '"PaymentType":"P",';
                strData = strData + '"Divisor":""';
                strData = strData + '},';
            });
            
            if (strData != "") {
                strData = "[" + strData.substring(0, strData.length - 1) + "]";
                $("#<%= Hid_Data.ClientID%>").val(strData);
            }
            else
            ret=false;
            
            return ret;
        }
        catch (err) {
            alert(err);
            return false;
        }
    }
    
    function Check_Date_Order(){
            var pre_ipdate='',ipdate='';
            var arr_pre_ipdate,arr_ipdate;
            var ret=true;
            
            $("#<%= dgInterestDetails.ClientID %> tr").each(function (i, row) {
                if(i==0)
                return;
                
                ipdate=$($(row).find('td').get(1)).text();
                if (pre_ipdate !==''){
                    arr_pre_ipdate= pre_ipdate.split("/");
                    arr_pre_ipdate=new Date(arr_pre_ipdate[2],Number(arr_pre_ipdate[1])-1,arr_pre_ipdate[0]);
                    
                    arr_ipdate= ipdate.split("/");
                    arr_ipdate=new Date(arr_ipdate[2],Number(arr_ipdate[1])-1,arr_ipdate[0]);
                    
                    if (arr_pre_ipdate>arr_ipdate){
                        ret=false;
                        return false;
                    }
                }
                pre_ipdate=ipdate;
            });
            return ret;
    }
    
    function CloseWindow(){
        window.close();
    }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="5" width="98%">
            <tr align="center" valign="top">
                <td style="width: 55%;">
                    <div style="height: 450px; overflow: auto;">
                        <asp:DataGrid ID="dgInterestDetails" runat="server" CssClass="table_border_right_bottom tablerowbg"
                            AutoGenerateColumns="false" Width="100%" OnItemDataBound="dgInterestDetails_ItemDataBound">
                            <HeaderStyle CssClass="table_heading" />
                            <Columns>
                                <asp:BoundColumn HeaderText="Id" DataField="Id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="IP Date" DataField="IPDate" HeaderStyle-Width="25%"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="editdate"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="IP Amount" DataField="IPAmount" HeaderStyle-Width="25%"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="editnumber"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="IP Amount1" DataField="IPAmount1" HeaderStyle-Width="25%"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="editnumber"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Days" DataField="Days" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Divisor" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Literal ID="litDivisor" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Divisor") %>'
                                            Visible="false"></asp:Literal>
                                        <asp:DropDownList ID="cboDivisor" runat="server">
                                            <asp:ListItem Text="365" Value="365"></asp:ListItem>
                                            <asp:ListItem Text="366" Value="366"></asp:ListItem>
                                            <asp:ListItem Text="360" Value="360"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderStyle-Width="10%" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                    <ItemTemplate>
                                        <label class="addrow" title="add new row">+</label>&nbsp;<label class="deleterow" title="delete this row">-</label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
                <td style="width: 45%;">
                    <div>
                        <asp:DataGrid ID="dgMaturityDetails" runat="server" CssClass="table_border_right_bottom tablerowbg"
                            AutoGenerateColumns="false" Width="100%">
                            <HeaderStyle CssClass="table_heading" />
                            <Columns>
                                <asp:BoundColumn HeaderText="Id" DataField="Id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Mat Date" DataField="IPDate" HeaderStyle-Width="40%"
                                    ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Mat Amount" DataField="IPAmount" HeaderStyle-Width="40%"
                                    ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Days" DataField="Days" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                    <div style="padding-top: 20px;">
                        <asp:DataGrid ID="dgCallDetails" runat="server" CssClass="table_border_right_bottom tablerowbg"
                            AutoGenerateColumns="false" Width="100%">
                            <HeaderStyle CssClass="table_heading" />
                            <Columns>
                                <asp:BoundColumn HeaderText="Id" DataField="Id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Call Date" DataField="IPDate" HeaderStyle-Width="40%"
                                    ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Call Amount" DataField="IPAmount" HeaderStyle-Width="40%"
                                    ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Days" DataField="Days" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                    <div style="padding-top: 20px;">
                        <asp:DataGrid ID="dgPutDetails" runat="server" CssClass="table_border_right_bottom tablerowbg"
                            AutoGenerateColumns="false" Width="100%">
                            <HeaderStyle CssClass="table_heading" />
                            <Columns>
                                <asp:BoundColumn HeaderText="Id" DataField="Id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Put Date" DataField="IPDate" HeaderStyle-Width="40%"
                                    ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Put Amount" DataField="IPAmount" HeaderStyle-Width="40%"
                                    ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Days" DataField="Days" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:DropDownList ID="cboUpdateType" runat="server" CssClass="combo1">
                        <asp:ListItem Text="Recalculate cashflow accordinfg to ip dates" Value="R"></asp:ListItem>
                        <asp:ListItem Text="Save details as it is" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnUpdate" runat="server" Text=" Update " CssClass="frmButton" OnClientClick="javascript:return UpdateDetails();"
                        OnClick="btnUpdate_Click" />
                    <input type="button" id="btnCancel" value=" Cancel " class="frmButton" onclick="javascript:window.close();" />
                    <asp:HiddenField ID="Hid_Id" runat="server" />
                    <asp:HiddenField ID="Hid_Data" runat="server" />
                    <asp:HiddenField ID="Hid_FirstInterestDate" runat="server" />
                    <asp:HiddenField ID="Hid_MaturityDate" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
