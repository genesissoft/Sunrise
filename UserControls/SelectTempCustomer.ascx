<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SelectTempCustomer.ascx.vb" Inherits="UserControls_SelectTempCustomer" %>
<script type="text/javascript">
    $(document).ready(function () {
      <%--  var vid_id = document.getElementById("ctl00_ContentPlaceHolder1_srh_ContactPerson_btn_Search");
        vid_id.innerText = document.getElementById("<%= Hid_TableName.ClientID %>").value;--%>
        
        $("img[id$='" + document.getElementById('<%= Hid_MyId.ClientID %>').value + '_btn_Del' + "']").bind("click", function () {
            document.getElementById("<%= Hid_AllFields.ClientID %>").value = "";
            document.getElementById("<%= Hid_SelectedId.ClientID %>").value = "";
        });


        var elmName = document.getElementById('<%= Hid_MyId.ClientID %>').value + "_btn_Search";
        var elm = $("a[id$='" + elmName + "']")[0];

        var evnt = 'click';
        var ret;

        if (elm.attachEvent) {
            elm.attachEvent('onclick', function () {
                debugger;
                var queryStr = "SelectTempCustomer.aspx";
                //queryStr = queryStr + "SourceType=2&TableName=CustomerMaster&SelectedFieldName=CM.CustomerName&ProcName=ID_SEARCH_CustomerMasterFaxSelWithContactPerson_&SelectedValueName=CM.CustomerId";

                ret = window.showModalDialog(queryStr, 'some argument', 'dialogWidth:500px;dialogHeight:550px;center:1;status:0;resizable:0;');
                //alert(ret);

                if (typeof (ret) != 'undefined') {
                    document.getElementById("<%= Hid_RetValues_fax.ClientID%>").value = ret;
                    document.getElementById('<%= btn_Post.ClientID%>').click();
                }
            });
        }
        else {
            elm.addEventListener('click', function () {
                var queryStr = "SelectTempCustomer.aspx";
                //queryStr = queryStr + "SourceType=2&TableName=CustomerMaster&SelectedFieldName=CM.CustomerName&ProcName=ID_SEARCH_CustomerMasterFaxSelWithContactPerson_&SelectedValueName=CM.CustomerId";

                ret = window.showModalDialog(queryStr, 'some argument', 'dialogWidth:500px;dialogHeight:550px;center:1;status:0;resizable:0;');

                if (typeof (ret) != 'undefined') {
                    var arrRetValues = ret.split('|');
                    document.getElementById("<%= Hid_SelectedId.ClientID %>").value = arrRetValues[1];
                    document.getElementById("<%= Hid_RetValues_fax.ClientID%>").value = ret;
                    document.getElementById('<%= btn_Post.ClientID%>').click();
                }
            });
        }
    });



</script>

<div>
    <table border="0">
        <tr style="text-align: center">
            <td>
                <a id="btn_Search" href="javascript:;" runat="server" cssclass="InfoLinkCSS">Add Temp Customer</a>
            </td>
        </tr>
        <tr style="display:none;">
            <td>
                <asp:ListBox ID="lst_Select" runat="server" CssClass="TextBoxCSS" Height="80px"></asp:ListBox>

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
<asp:HiddenField ID="Hid_RetValues_fax" runat="server" />
