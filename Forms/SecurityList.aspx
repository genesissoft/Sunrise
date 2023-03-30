<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SecurityList.aspx.vb" Inherits="Forms_SecurityList" Title="Untitled Page" %>

<%@ Register Src="~/UserControls/ViewScreen.ascx" TagName="ViewScreen" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script language="javascript" type ="text/javascript" >
   
    function ViewAccruedInterest()
    {
         if(document.getElementById("ctl00_ContentPlaceHolder1_vws_SecurityName_Hid_Id").value == "" )
        {
           alert('please Select atleast one row')   
            return false
          
        } 
        else
        {
        var strId = document.getElementById ("ctl00_ContentPlaceHolder1_vws_SecurityName_Hid_Id").value
        var pageName = "AccuredInterest.aspx"
        pageName = pageName + "?Id=" + strId
        var ret  = ShowDialogOpen(pageName,"40","20")
          
        if (ret =="" || typeof(ret)=="undefined")
        {
            return false
        }
          else
            {         
                return true
            }
        }
        
    }

    function ViewCurrentRate()
    {
     if(document.getElementById("ctl00_ContentPlaceHolder1_vws_SecurityName_Hid_Id").value == "" )
        {
           alert('please Select atleast one row')   
            return false
          
        } 
        else
        {
        var strId = document.getElementById ("ctl00_ContentPlaceHolder1_vws_SecurityName_Hid_Id").value
        var pageName = "CurrentRate.aspx"
        pageName = pageName + "?Id=" + strId
        var ret  = ShowDialogOpen(pageName,"35","20")
          
        if (ret =="" || typeof(ret)=="undefined")
        {
            return false
        }
          else
            {             
                return true
            }
        }    
    }
     function ViewYieldCalc()
    {
     if(document.getElementById("ctl00_ContentPlaceHolder1_vws_SecurityName_Hid_Id").value == "" )
        {
           alert('please Select atleast one row')   
            return false
          
        } 
        else
        {
        var strId = document.getElementById ("ctl00_ContentPlaceHolder1_vws_SecurityName_Hid_Id").value
        var pageName = "YieldCalculation.aspx"
        pageName = pageName + "?Id=" + strId
        var ret  = ShowDialogOpen(pageName,"45","30")
          
        if (ret =="" || typeof(ret)=="undefined")
        {
            return false
        }
          else
            {             
                return true
            }
            }
    }

 function ViewSecurity()
    {
         if(document.getElementById("ctl00_ContentPlaceHolder1_vws_SecurityName_Hid_Id").value == "" )
        {
           alert('please Select atleast one row')   
            return false
          
        } 
        else
        {
        var strId = document.getElementById ("ctl00_ContentPlaceHolder1_vws_SecurityName_Hid_Id").value
       
        var pageName = "SecurityMaster.aspx"
        pageName = pageName + "?Id=" + strId
        var ret  = ShowDialogOpen(pageName,"80","100")
          
        if (ret =="" || typeof(ret)=="undefined")
        {
            return false
        }
    }
     return true
    }
   
</script>
    <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div align="center">
                <uc:ViewScreen ID="vws_SecurityName" runat="server" SelectProcName="ID_SEARCH_SecurityMaster"
                    SelectedFieldName="SecurityName" DeleteProcName="ID_DELETE_SecurityMaster" DeleteFieldName="SecurityId" SearchOnly="true" 
                    NavigateUrl="" PageName="Security" CheckCompany="false" TableName="SecurityMaster" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
            <table border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btn_ShowSecurity" runat="server" Text="Show Security" ToolTip="Show Security"
                            CssClass="ButtonCSS" Height="20px" />
                        <asp:Button ID="btn_YieldCalc" Visible="True" runat="server" Text="Yield Calculation"
                            ToolTip="Update" CssClass="ButtonCSS" Height="20px" />
                        <asp:Button ID="btn_AccruedInterest" runat="server" Text="Accrued Interest" ToolTip="Accrued Interest"
                            CssClass="ButtonCSS" Height="20px" />
                        <asp:Button ID="btn_CurrentRate" runat="server" Text="Current Rate" ToolTip="Current Rate"
                            CssClass="ButtonCSS" Height="20px" />
                    </td>
                </tr>
                <asp:HiddenField ID ="Hid_SecurityId" runat ="server" />
            </table>
            
        </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>
