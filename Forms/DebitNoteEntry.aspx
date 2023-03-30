<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" CodeFile="~/forms/debitnoteentry.aspx.vb"
    AutoEventWireup="false" Inherits="Forms_DebitNoteEntry" Title="Debit Note Entry"
    EnableViewStateMac="false" %>

<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagName="Search" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/DatePicker.js"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
    
      
    function FillTax()
        {
        var servicetax;
        var cess;
        var secondarycess;
        var Educationcess; 
        var HEducationcess; 


        servicetax =  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0) * (document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0) / 100;
        Educationcess = document.getElementById("ctl00_ContentPlaceHolder1_txt_CessRate").value-0
        HEducationcess =document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessRate").value-0
        cess =   (servicetax*Educationcess)/100
        secondarycess = (servicetax*HEducationcess)/100
        RCess = Math.round(cess,2)
        ESecCess = Math.round (secondarycess,2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value = servicetax.toFixed(2)
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_ServiceTaxAmt").value = servicetax.toFixed(2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value = cess.toFixed(2)
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_CessAmt").value = cess.toFixed(2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value = secondarycess.toFixed(2)
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_SecoCessAmt").value = secondarycess.toFixed(2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value =  ((servicetax- 0) + (cess - 0)+ (secondarycess- 0) +  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0)).toFixed(2)
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_totFees").value =  ((servicetax- 0) + (cess - 0)+ (secondarycess- 0) +  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0)).toFixed(2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value =  ((servicetax- 0) + (cess - 0)+ (secondarycess- 0) +  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0)).toFixed(2)
        
        }
        
    
    
        function calcTax()
        {
        var servicetax;
        var cess;
        var secondarycess;
        var Educationcess; 
        var HEducationcess; 
        var STFee;
        var ST =(document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0)
        var ECess =((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0) *(document.getElementById("ctl00_ContentPlaceHolder1_txt_CessRate").value-0))/100
        var HECess =((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0) *(document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessRate").value-0))/100
        var totTax = 100 + ST + ECess + HECess
        
        servicetax =  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0) * (document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0) / 100;
        Educationcess = document.getElementById("ctl00_ContentPlaceHolder1_txt_CessRate").value-0
        HEducationcess =document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessRate").value-0
        cess =   RoundNo((servicetax*Educationcess)/100,2)
        secondarycess =  RoundNo((servicetax*HEducationcess)/100 ,2) 
        RCess = cess //Math.round(cess,2)
        ESecCess = secondarycess //Math.round (secondarycess,2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value = servicetax
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_ServiceTaxAmt").value = servicetax
        document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value = cess
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_CessAmt").value = cess
        document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value = secondarycess
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_SecoCessAmt").value = secondarycess
      
//        document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value =  ((servicetax- 0) + (cess - 0)+ (secondarycess- 0) +  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0))
        document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value =  ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0))
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_totFees").value = Math.round ((servicetax- 0) + (cess - 0)+ (secondarycess- 0) +  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0),2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value =  Math.round ((servicetax- 0) + (cess - 0)+ (secondarycess- 0) +  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0),2)
    
       var x  = (((document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value )/totTax))/100
       var fees = document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value -0
       document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value = x
       document.getElementById("ctl00_ContentPlaceHolder1_Hid_Fees").value = document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value
       document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value =((x * 1)/100)
       document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value = ((x * 2)/100)  
     
         if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_RoundOff_0").checked == true)
           {
              document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value = Math.round (fees,2)
              document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value = Math.round (((fees * ST)/100),2)
              document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value =Math.round (((fees * ECess)/100).toFixed(2) ,2)
              document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value = Math.round (((fees * HECess)/100).toFixed(2),2)
              document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value = Math.round (((servicetax- 0) + (cess - 0)+ (secondarycess- 0) +  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0)),2)
              document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value  = Math.round (document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value,2)
             
           }
         else
          {
                      
              document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value = fees.toFixed(2)
            
              document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value = ((fees * ST)/100).toFixed(2)
           
              document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value =((fees * ECess)/100).toFixed(2) 
              document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value = ((fees * HECess)/100).toFixed(2)
              document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value = ((servicetax- 0) + (cess - 0)+ (secondarycess- 0) +  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0)).toFixed(2)
          
              document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value  = document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value
          }
        }
   
         function calcRevTax()
        {

        var servicetax;
        var cess;
        var secondarycess;
        var Educationcess; 
        var HEducationcess; 
        var x;
      
        servicetax =  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0) * (document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0) / 100;
        Educationcess = document.getElementById("ctl00_ContentPlaceHolder1_txt_CessRate").value-0
        HEducationcess =document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessRate").value-0
        cess =   RoundNo((servicetax*Educationcess)/100,2)
        secondarycess =  RoundNo((servicetax*HEducationcess)/100 ,2) 
        RCess =cess// Math.round(cess,2)
        ESecCess = secondarycess//Math.round (secondarycess,2)
        var ST =(document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0)
        var ECess =((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0) *(document.getElementById("ctl00_ContentPlaceHolder1_txt_CessRate").value-0))/100
        var HECess =((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0) *(document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessRate").value-0))/100
        var totTax = 100 + ST + ECess + HECess
        document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value = servicetax
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_ServiceTaxAmt").value = servicetax
        document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value = cess
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_CessAmt").value = cess
        document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value = secondarycess
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_SecoCessAmt").value = secondarycess
 //      var x  = (((document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value * 100)/totTax)*10)/100
         var x  = (((document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value) *100) / totTax*10)/100

       document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value = x
       document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value = x * ST
       document.getElementById("ctl00_ContentPlaceHolder1_Hid_Fees").value = document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value
       document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value =((x * 1)/100)
       document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value = ((x * 2)/100)
     
          if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_RoundOff_0").checked == true)
           {
    var x  = (((document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value) / totTax)*100)
 
            var totfees = document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value
            document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value = Math.round ((x),2) + ".00"  
            document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value = Math.round(((document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value * ST)/100),2) + ".00"    
            document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value =Math.round (((((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value)* Educationcess )/100)),2)+ ".00"  
            document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value = Math.round (((((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value)* HEducationcess )/100)),2) + ".00" 
//            document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value = Math.round ((x- 0) + (((x * (ST * ECess))/100) - 0)+ (((x * (ST * HECess))/100)- 0) +  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0),2)+ ".00" 
              document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value = Math.round (((document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value-0)+(document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value-0) + (document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value-0) + (document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value-0)),2) + ".00" 
            document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value =document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value
           }
         else
          { 
               
    var x  = (((document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value) / totTax) *100)
               var totfees = document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value -0
              document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value = x.toFixed(2)// ((totfees *ST)/100).toFixed(2)
              document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value = ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value * ST)/100).toFixed(2) //  x.toFixed(2)
              document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value =(((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value)* Educationcess )/100).toFixed(2)   
              document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value = (((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value)* HEducationcess )/100).toFixed(2)   
//            document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value = ((x- 0) + (((x * (ST * ECess))/100) - 0)+ (((x * (ST * HECess))/100)- 0) +  (document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value - 0)).toFixed(2)
              document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value = ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value-0)+(document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value-0) + (document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value-0) + (document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value-0)).toFixed(2)
              document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value  = document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value
          }
        }
   
   
   
        
        function RoundNo(num, dec) 
            {
	            return num.toFixed(dec)
            }
        
        function calcnetamtreceived()
        {

        var Debitamt = 0;
        if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value - 0) < (document.getElementById("ctl00_ContentPlaceHolder1_txt_TDSDeducted").value-0))
        {
            alert("TDS Can not be more than Debit amount.");
             document.getElementById("ctl00_ContentPlaceHolder1_txt_Netamtreceived").value = ""    
                            return false;
        
        }
        
        
        Debitamt = document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value - 0 
        document.getElementById("ctl00_ContentPlaceHolder1_txt_Netamtreceived").value = ((document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value - 0) - (document.getElementById("ctl00_ContentPlaceHolder1_txt_TDSDeducted").value - 0))
        document.getElementById("ctl00_ContentPlaceHolder1_Hid_Netamtreceived").value = document.getElementById("ctl00_ContentPlaceHolder1_txt_Netamtreceived").value 
        
        }
        
        function CheckRateAmt(fntId,txtId)
        {   
        
                  
            var TotalAppAmount = (document.getElementById("ctl00_ContentPlaceHolder1_" + txtId).value-0)
                var inc = (TotalAppAmount) / 10000000
              
                if(inc != 0)
                {
                    document.getElementById(fntId).innerHTML =  inc + " CRORE";                
                   document.getElementById(fntId).style.display = "inline";
                }
                else
                {
                    document.getElementById(fntId).style.display = "none";
                }
          
          
        }
              
        function Validation()
        
        {
             
            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitDate").value) == "")
            {
                alert("Please Select the Debit Date");
                return false;
            }
            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_IssuerName_txt_Name").value) == "")
            {
                alert("Please Select the Issuer");
                return false;
            }
            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_srh_IssueDesc_txt_Name").value) == "")
            {
                alert("Please Select the Issue Description");
                return false;           
            }
           
            if((document.getElementById("ctl00_ContentPlaceHolder1_txt_TotAppliamt").value-0) == 0)
            {
                alert("Application Amount can not be zero");
                return false;
            }
            
            
            
            
            if((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0) !=  0)
            
            {
            
            
                    if((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxRate").value-0) == 0)
                    {
                        alert("Please Enter Service Tax Rate");
                        return false;
                    }
                    if((document.getElementById("ctl00_ContentPlaceHolder1_txt_CessRate").value-0) == 0)
                    {
                        alert("Please Enter Cess Rate");
                        return false;
                    }
                     if((document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessRate").value-0) == 0)
                    {
                        alert("Secondary Cess Rate can not be zero");
                        return false;
                    }   
                     if((document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value-0) == 0)
                    {
                        alert("Fees Amount can not be zero");
                        return false;
                    }
                     if((document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value-0) == 0)
                    {
                        alert("Total Fees Amount can not be zero");
                        return false;
                    }    
            }            
                
                             
            return true;
        }
        
        
         function ReportView()
        {
           
           
            var strDebitNoteId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DebitNoteId").value 
            pageUrl1 = "ViewNoteReports.aspx?Note=DebitNoteEntry&DebitNoteId="+ strDebitNoteId ;
            var ret = window.open(pageUrl1,target="_blank","left=80,top=80,height=600,width=780,menubar=yes,resizable=no,scrollbars=yes,toolbar=yes")
            window.location = "DebitNoteDetails.aspx?Id=" + strDebitNoteId ;
        }


    function roundoff()
    {
  
       var fees =document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value
       var STAmt =  document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value
       var CessAmt =document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value
       var ECessAmt =document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value
       var TotFees =document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value
       var DebitNoteAmt =document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value 
   
       if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_RoundOff_0").checked == true)
       {
        document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value = Math.round ((document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value-0),2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value =Math.round ((document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value-0),2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value =Math.round ((document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value-0),2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value =Math.round ((document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value-0),2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value =Math.round ((document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value-0),2)
        document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value  =Math.round ((document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value-0),2)
       
       }
       if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_RoundOff_1").checked == true)
       {
        document.getElementById("ctl00_ContentPlaceHolder1_txt_Fees").value = fees
        document.getElementById("ctl00_ContentPlaceHolder1_txt_ServiceTaxAmt").value =STAmt
        document.getElementById("ctl00_ContentPlaceHolder1_txt_CessAmt").value =CessAmt
        document.getElementById("ctl00_ContentPlaceHolder1_txt_SecoCessAmt").value =ECessAmt
        document.getElementById("ctl00_ContentPlaceHolder1_txt_totFees").value =TotFees
        document.getElementById("ctl00_ContentPlaceHolder1_txt_DebitNoteAmt").value  =DebitNoteAmt
       
       }
        return true;
    }


    </script>

    <div>
        <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
            <ContentTemplate>
                <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr align="left">
                        <td class="SectionHeaderCSS">
                            Debit Note Entry</td>
                    </tr>
                    <tr class="line_separator">
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr align="center" valign="top">
                        <td>
                            <table width="90%" align="center" cellspacing="0" cellpadding="0" border="0">
                                <tr align="left">
                                    <td>
                                        Debit No:
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_DebitNoteNo" runat="server" CssClass="TextBoxCSS" TabIndex="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Debit Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DebitDate" runat="server" CssClass="TextBoxCSS" Width="110px"
                                            TabIndex="0"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_DebitDate',this);"
                                                id="IMG2">
                                    </td>
                                    <td>
                                        Total Allocation Amt.:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_totAlloAmt" runat="server" CssClass="TextBoxCSS" TabIndex="0"
                                            ReadOnly="True"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
                                        <font id="fnt_Perc6" class="TextboxCSS"></font>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Issuer Name:
                                    </td>
                                    <td style="padding-left: 0px;">
                                        <uc:Search ID="srh_IssuerName" runat="server" AutoPostback="false" ProcName="MB_SEARCH_IssuerMasterDebit"
                                            SelectedFieldName="IssuerName" SourceType="StoredProcedure" TableName="IssuerMaster"
                                            FormWidth="700" FormHeight="390" ConditionalFieldName=" " ConditionalFieldId=" "></uc:Search>
                                    </td>
                                    <td id="LBL_UTRNO">
                                        Fees:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Fees" runat="server" CssClass="TextBoxCSS" TabIndex="0" ReadOnly="false"
                                            Style="text-align: right"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Issue Name:
                                    </td>
                                    <td style="padding-left: 0px;">
                                        <uc:Search ID="srh_IssueDesc" runat="server" AutoPostback="true" ProcName="MB_SEARCH_IssueDebitNote"
                                            SelectedFieldName="IssueName" SourceType="StoredProcedure" TableName="IssueMaster"
                                            ConditionalFieldName="IM.IssuerId" ConditionalFieldId="srh_IssuerName"></uc:Search>
                                    </td>
                                    <td>
                                        Service Tax:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="txt_ServiceTaxRate" runat="server" CssClass="TextBoxCSS" Width="35px"
                                            Style="text-align: right" TabIndex="0"></asp:TextBox><font id="fnt_Perc1" class="TextboxCSS">%</font><asp:TextBox
                                                ID="txt_ServiceTaxAmt" runat="server" CssClass="TextBoxCSS" Width="78px" TabIndex="0"
                                                Style="text-align: right" ReadOnly="False"></asp:TextBox><em><span style="color: Red;
                                                    vertical-align: super;">*</span></em>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">
                                        Contact Person:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cbo_ContPerson" runat="server" CssClass="ComboBoxCSS" Width="208px"
                                            TabIndex="0">
                                        </asp:DropDownList>
                                        <%-- <uc:Search ID="Srh_ContPerson" runat="server" AutoPostback="false" ProcName="MB_SEARCH_IssuerContPersonDebit"
                                            SelectedFieldName="ContactPerson" SourceType="StoredProcedure" TableName="IssuerContacts"  
                                            ConditionalFieldName="IM.IssuerId" ConditionalFieldId="srh_IssuerName"></uc:Search>--%>
                                    </td>
                                    <td>
                                        Secondary Cess:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="txt_SecoCessRate" runat="server" CssClass="TextBoxCSS" Width="35px"
                                            TabIndex="0" Style="text-align: right"></asp:TextBox><font id="fnt_Perc3" class="TextboxCSS">%</font><asp:TextBox
                                                ID="txt_SecoCessAmt" runat="server" CssClass="TextBoxCSS" Width="78px" TabIndex="0"
                                                Style="text-align: right"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                    </td>
                                    <%--  <td class="LabelCSS">
                                        Cess:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_CessRate" runat="server" CssClass="TextBoxCSS" Width="40px"
                                            TabIndex="0" Style="text-align: right"></asp:TextBox><font id="fnt_Perc2" class="TextboxCSS">%</font><asp:TextBox
                                                ID="txt_CessAmt" runat="server" CssClass="TextBoxCSS" Width="80px" TabIndex="0"
                                                  Style="text-align: right"></asp:TextBox><i style="color: red">*</i>
                                    </td>--%>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Total Application Amt:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_TotAppliamt" runat="server" CssClass="TextBoxCSS" TabIndex="0"
                                            ReadOnly="True"></asp:TextBox>
                                        <font id="fnt_Perc4" class="TextboxCSS"></font><i style="color: Red; vertical-align: super;">
                                            *</i>
                                    </td>
                                    <td>
                                        Cess:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_CessRate" runat="server" CssClass="TextBoxCSS" Width="35px"
                                            TabIndex="0" Style="text-align: right"></asp:TextBox><font id="fnt_Perc2" class="TextboxCSS">%</font><asp:TextBox
                                                ID="txt_CessAmt" runat="server" CssClass="TextBoxCSS" Width="78px" TabIndex="0"
                                                Style="text-align: right"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                    </td>
                                </tr>
                                <tr align="left" valign="middle">
                                    <td rowspan="2">
                                        Remark:
                                    </td>
                                    <td rowspan="2">
                                        <asp:TextBox ID="txt_Remark" runat="server" CssClass="TextBoxCSS" Width="200px" TabIndex="0"
                                            Height="60px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td>
                                        Total Fees:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_totFees" runat="server" CssClass="TextBoxCSS" Width="134px"
                                            TabIndex="0" ReadOnly="False" Style="text-align: right"></asp:TextBox><i style="color: Red;
                                                vertical-align: super;">*</i>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Round Off :
                                    </td>
                                    <td valign="top" style="padding-left: 0px;">
                                        <asp:RadioButtonList ID="rdo_RoundOff" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                            <asp:ListItem Value="N" Selected="True">NO</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr class="line_separator">
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td class="HeadingCenter" colspan="4">
                                        Payment Details
                                    </td>
                                </tr>
                                <tr class="line_separator">
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Cheque Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Chequedate" runat="server" CssClass="TextBoxCSS" Width="110px"
                                            TabIndex="0"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Chequedate',this);"
                                                id="IMG1">
                                    </td>
                                    <td>
                                        Debit Note Amount:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DebitNoteAmt" runat="server" CssClass="TextBoxCSS" TabIndex="0"
                                            ReadOnly="True"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Cheque No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ChequeNo" runat="server" CssClass="TextBoxCSS" TabIndex="0"></asp:TextBox>
                                    </td>
                                    <td>
                                        TDS Deducted:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_TDSDeducted" runat="server" CssClass="TextBoxCSS" TabIndex="0"></asp:TextBox><em><span
                                            style="color: Red; vertical-align: super;">*</span></em>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Deposited in:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_depositedin" runat="server" CssClass="TextBoxCSS" TabIndex="0"></asp:TextBox>
                                    </td>
                                    <td>
                                        Net Amount Received.:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Netamtreceived" runat="server" CssClass="TextBoxCSS" TabIndex="0"
                                            ReadOnly="True"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Deposit Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DepositDate" runat="server" CssClass="TextBoxCSS" Width="110px"
                                            TabIndex="0"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_DepositDate',this);"
                                                id="IMG3">
                                    </td>
                                    <td>
                                        Delivery Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DeliveryDate" runat="server" CssClass="TextBoxCSS" Width="110px"
                                            TabIndex="0"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_DeliveryDate',0);"
                                                id="IMG4">
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Date Of Receipt Of Fees:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ReceiptdateFees" runat="server" CssClass="TextBoxCSS" Width="110px"
                                            TabIndex="0"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_ReceiptdateFees',this);"
                                                id="IMG5">
                                    </td>
                                    <td>
                                        Dispatch Incentive to Investors:
                                    </td>
                                    <td style="padding-left: 0px;">
                                        <asp:RadioButtonList ID="rdo_DispatchIncentive" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Value="Y">YES</asp:ListItem>
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Form Of Receipt:
                                    </td>
                                    <td style="padding-left: 0px;">
                                        <asp:RadioButtonList ID="rdo_ReceiptForm" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Value="C">Cheque</asp:ListItem>
                                            <asp:ListItem Value="R">RTGS</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        Final date of account Closure :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_AccountClouserDate" runat="server" CssClass="TextBoxCSS" Width="110px"
                                            TabIndex="0"></asp:TextBox><img class="calender" src="../Images/Calender.jpg" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_AccountClouserDate',0);"
                                                id="IMG6">
                                    </td>
                                </tr>
                                <tr class="line_separator">
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="line_separator">
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td colspan="4">
                                        <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" TabIndex="0" />
                                        <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" Visible="false"
                                            TabIndex="0" />
                                        <asp:Button ID="btn_Back" runat="server" CssClass="ButtonCSS" Text="Back" TabIndex="0" />
                                        <asp:Button ID="btn_ReCalculate" runat="server" CssClass="ButtonCSS" Text="Re-Calculate Fees"
                                            TabIndex="0" Visible="false" Width="120px" />
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="Hid_IssuerName" runat="server" />
                            <asp:HiddenField ID="Hid_ServiceTaxAmt" runat="server" />
                            <asp:HiddenField ID="Hid_CessAmt" runat="server" />
                            <asp:HiddenField ID="Hid_SecoCessAmt" runat="server" />
                            <asp:HiddenField ID="Hid_totFees" runat="server" />
                            <asp:HiddenField ID="Hid_FeesOn" runat="server" />
                            <asp:HiddenField ID="Hid_IssueSizeQty" runat="server" />
                            <asp:HiddenField ID="Hid_IssueSizeMultiple" runat="server" />
                            <asp:HiddenField ID="Hid_NomanClature" runat="server" />
                            <asp:HiddenField ID="Hid_IssueFee" runat="server" />
                            <asp:HiddenField ID="Hid_DebitNoteNo" runat="server" />
                            <asp:HiddenField ID="Hid_remark" runat="server" />
                            <asp:HiddenField ID="Hid_DebitNoteId" runat="server" />
                            <asp:HiddenField ID="Hid_TotAllAmt1" runat="server" />
                            <asp:HiddenField ID="Hid_MultiReamrk" runat="server" />
                            <asp:HiddenField ID="Hid_Netamtreceived" runat="server" />
                            <asp:HiddenField ID="Hid_ServTaxInEx" runat="server" />
                            <asp:HiddenField ID="Hid_Fees" runat="server" />
                            <asp:HiddenField ID="Hid_ServFees" runat="server" />
                            <asp:HiddenField ID="Hid_Fee1" runat="server" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </atlas:UpdatePanel>
    </div>
</asp:Content>
