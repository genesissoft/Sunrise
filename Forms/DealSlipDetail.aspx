<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="DealSlipDetail.aspx.vb" Inherits="Forms_DealSlipDetail" title="Untitled Page" %>
<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">

function ChkDel(intIndex,intDealSlipId)
    {

        if( window.confirm("The Deal Confirmation for this deal has been generated." + '\n' + " Do you want to delete this deal?") )
        {
             if (window.confirm("Are you sure you want to delete this record??") ) return true;
        }
        return false;
    }
   
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                 <uc:ViewScreen id="vws_DealSlip" runat="server" selectprocname="ID_SEARCH_DealSlipEntry"  CheckYearCompany="True"   TableAlias="DSE"  
                    selectedfieldname="Company" deleteprocname="ID_DELETE_DealSlipEntry" deletefieldname="DealSlipID" CheckUser="true"   DefaultSort="DealDate"  
                   navigateurl="DealSlipEntry.aspx"  PageName ="DealSlip" CheckCompany="false" TableName="DealSlipEntry"   ConditionExist="true"   UserIdFieldName="EntryUserId"  />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


