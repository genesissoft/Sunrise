<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="DeleteDeal.aspx.vb" Inherits="Forms_DeleteDeal" Title="DeleteDeal" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
     
     function Validation()
        {
             
               if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_Srh_DealNumber_txt_Name").value) == "")
                {  
                    alert("Please Select Deal No");
                    return false;
                } 
                
                
              if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_IssuerOfSecurity_txt_Name").value) == "")
                {  
                    alert("Please Select Issuer Of Security");
                    return false;
                } 
                       
             if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Remark").Text) == "")
                {  
                    alert("Please Specify Reason of Deletion");
                    return false;
                 }   
           
            if  window.confirm("Are you sure you want to delete this record????") ) return true;
            {return false;}
        
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <%--<tr>
            <td class="LabelCSS">
                Deal Number:
            </td>
            <td>
                <uc:Search ID="Srh_DealNumber" runat="server" AutoPostback="true" ProcName="ID_SEARCH_DealNumber"
                    SelectedFieldName="Dealslipno" SourceType="StoredProcedure" TableName="Dealslipentry"
                    ConditionalFieldName=" " ConditionalFieldId=" " Width="160" ConditionExist="false"
                    FormHeight="400" FormWidth="800"></uc:Search>
            </td>
        </tr>--%>
        <tr>
            <td class="LabelCSS">
                Deal No.:
            </td>
            <td align="left">
                <uc:Search ID="Srh_DealNumber" runat="server" AutoPostback="true" ProcName="ID_SEARCH_FinancialInfo"
                    SelectedFieldName="DealSlipNo" SourceType="StoredProcedure" TableName="DealSlipEntry"
                    ConditionExist="true" ConditionalFieldName="" ConditionalFieldId="" FormHeight="400" FormWidth="800"></uc:Search>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                Issuer:
            </td>
            <td align="left" class="LabelCSS">
                <asp:Literal ID="lit_Issuer" runat="server"> 
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS" width="100">
                Name Of Security:
            </td>
            <td align="left" class="LabelCSS">
                <asp:Literal ID="lit_SecurityName" runat="server">
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                Customer Name:
            </td>
            <td align="left" class="LabelCSS">
                <asp:Literal ID="lit_CustName" runat="server">
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                Trans Type:
            </td>
            <td class="LabelCSS">
                <asp:Literal ID="lit_TransType" runat="server">
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                Deal date:
            </td>
            <td class="LabelCSS">
                <asp:Literal ID="lit_DealDate" runat="server">
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                Settlement Date:
            </td>
            <td class="LabelCSS">
                <asp:Literal ID="lit_SettlementDate" runat="server">
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                Face Value:
            </td>
            <td class="LabelCSS">
                <asp:Literal ID="lit_FaceValue" runat="server">
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                Rate:
            </td>
            <td class="LabelCSS">
                <asp:Literal ID="lit_Rate" runat="server">
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="LabelCSS">
                Remark:
            </td>
            <td>
                <asp:TextBox ID="txt_Remark" Width="250px" Height="70px" TextMode="MultiLine" runat="server"
                    CssClass="TextBoxCSS" TabIndex="21"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lbl_Deleted" ForeColor="Blue" runat="server" CssClass="LabelCSS"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Button ID="btn_DeleteDeal" runat="server" Text="Save" ToolTip="DeleteDeal" CssClass="ButtonCSS"
                    Height="20px" TabIndex="29" />
    </table>
</asp:Content>
