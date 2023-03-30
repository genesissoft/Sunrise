<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="CustomerMaster.aspx.vb" Inherits="Forms_CustomerMaster" Title="Customer Master" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<%@ Register Src="~/UserControls/SearchListBox.ascx" TagName="SelectFields" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
    
         function Visiblefalse()
             {
                         document.getElementById("tr_Emp").style.display ="none"
                         document.getElementById("tr_Kyc").style.display ="none"
                         document.getElementById("row_EmpalmentDt").style.display ="none"
                         document.getElementById("row_EmpalmentFrequ").style.display ="none"   
             }
        
         function SelectDocType() 
         {
            var strDealSlipId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_DecType").value  
            //alert(strDealSlipId)  
            if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_DecType").value=="K")
            {
                document.getElementById("tr_Emp").style.display ="none"
                document.getElementById("tr_Kyc").style.display ="block"  
                document.getElementById("row_EmpalmentDt").style.display ="none"
                document.getElementById("row_EmpalmentFrequ").style.display ="none"                                        
            }
            else if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_DecType").value=="E")
            {
                document.getElementById("tr_Emp").style.display ="block"
                document.getElementById("tr_Kyc").style.display ="none"   
                document.getElementById("row_EmpalmentDt").style.display ="block"
                document.getElementById("row_EmpalmentFrequ").style.display ="block"                                            
            }
            else if (document.getElementById("ctl00_ContentPlaceHolder1_Hid_DecType").value=="B")
            {
                document.getElementById("tr_Emp").style.display ="block"
                document.getElementById("tr_Kyc").style.display ="block"   
                document.getElementById("row_EmpalmentDt").style.display ="block"
                document.getElementById("row_EmpalmentFrequ").style.display ="block"                                            
            }
            else  
            {
                document.getElementById("tr_Emp").style.display ="none"
                document.getElementById("tr_Kyc").style.display ="none"
                document.getElementById("row_EmpalmentDt").style.display ="none"
                document.getElementById("row_EmpalmentFrequ").style.display ="none"   
            }
        }
        
    
         function Deletedetails()
        {
            if(window.confirm("Are you sure u want to delete this detail record"))
            {
                return true
            } 
            else
            {
                return false
            }
        }
        
         function ConvertUCase(txtBox)
        {     
            txtBox.value = txtBox.value.toUpperCase()    
        }  
        function AddAddress()
        {  
             if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType").value) == "")
            {
                alert("Please select the Customer Type");
                return false;
            }
            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_CustomerName").value) == "")
            {
                alert("Please Enter Customer Name");
                return false;
            }
            var pageUrl = "ClientCustomerAddress.aspx"; 
            var strId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value  
             var strProfileType = "CM" 
              var strBusniessType = "NO"
                
                
//                var CustType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType");
//            var strCustType = CustType.options[CustType.options.selectedIndex].text; 
//            
//            var CustCategory = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Category");            
//            var CustomerCategory = CustCategory.options[CustCategory.options.selectedIndex].text; 
//             var CategoryId = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Category").value;
         var CustomerTypeId = document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType").value;
//            
          
            pageUrl = pageUrl + "?CustomerId=" + strId + "&ProfileType=" + strProfileType + "&BusniessType=" + strBusniessType + "&CustomerTypeId=" + CustomerTypeId;
//            + "&CustomerType=" + strCustType  + "&CustomerCategory=" + CustCategory + "&CategoryId=" + CategoryId + "&CustomerTypeId=" + CustomerTypeId;
            var ret = ShowDialogOpen(pageUrl, "990px", "340px")
            if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            }
            else
            {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret
                return true
            }
        }
        
        function UpdateContactDetails(rowIndex)
        {
            
            var pageUrl = "ClientContactPerson.aspx";
            var strValues = "";
            var selValues = "";
                     
            var strId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value 
            var ctrlItem = document.getElementById("ctl00_ContentPlaceHolder1_dg_Contact_itm" + rowIndex)    
            var hidBusinessTypeIds = document.getElementById("ctl00_ContentPlaceHolder1_Hid_BusinessTypeId").value.split("!")       
             
            var hidUserIds = document.getElementById("ctl00_ContentPlaceHolder1_Hid_UserIds").value.split("!") 
            var hidNameOfUsers = document.getElementById("ctl00_ContentPlaceHolder1_Hid_NameOfUsers").value.split("!")             
            var hidContactDetailId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_ContactDetailId").value.split("!")
           var hidPhoneNo2 = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PhoneNo2").value.split("!") 
//            alert( hidPhoneNo2[rowIndex])
            var hidFaxNo1 = document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaxNo1").value.split("!") 
//             alert(hidFaxNo1)
             var hidFaxNo2 = document.getElementById("ctl00_ContentPlaceHolder1_Hid_FaxNo2").value.split("!") 
//           alert(hidFaxNo2)
            strValues = strValues + ctrlItem.children(0).children(0).value + "!" 
            strValues = strValues + ctrlItem.children(1).children(0).innerHTML + "!"  
            strValues = strValues + ctrlItem.children(2).children(0).innerHTML + "!" 
            strValues = strValues + ctrlItem.children(3).children(0).innerHTML + "!"  
            strValues = strValues + ctrlItem.children(4).children(0).innerHTML + "!"    
            strValues = strValues + ctrlItem.children(5).children(0).value + "!"                                         
            strValues = strValues + hidBusinessTypeIds[rowIndex] + "!"   
            strValues = strValues + hidNameOfUsers[rowIndex] + "!"     
            strValues = strValues + hidUserIds[rowIndex] + "!" 
            strValues = strValues + hidPhoneNo2[rowIndex] + "!" 
            strValues = strValues + hidFaxNo1[rowIndex] + "!" 
            strValues = strValues + hidFaxNo2[rowIndex] + "!" 
//            alert(strValues)
                      
            strValues = strValues + hidContactDetailId[rowIndex] + "!"          
            strValues = strValues.replace("&"," ")                     
            pageUrl = pageUrl + "?CustomerId=" + strId + "&Values=" + strValues;
            var ret = ShowDialogOpen(pageUrl, "610px", "470px")        
            if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            } 
            else
            {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret
                return true
            }
        }
         function ValidateDetails()
        {

           
            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_CustomerName").value) == "")
            {
                alert('Please Enter Customer Name')
                return false;
            }
//             if (TrimString(document.getElementById("ctl00_ContentPlaceHolder1_txt_TransportName").value) == "")
//            {
//                alert('Please Select Transport Name')
//                return false;
//            }
          else
            {
                return AddClientContactPerson()
            }
             
           
            
            return true
        }
       
                
        function AddClientContactPerson()
        {  
            var pageUrl = "ClientContactPerson.aspx"; 
            var strId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value  
            pageUrl = pageUrl + "?CustomerId=" + strId ;
            var ret = ShowDialogOpen(pageUrl, "610px", "470px")
            if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            }
            else
            {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_RetValues").value = ret
                return true
            }
        }
        
        function ShowDialogOpen(PageName, strWidth, strHeight)
		{     
			var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
			var OpenUrl = PageName;  
			var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
			return ret
		}
    
        function ShowHeadCustomer()
        {  
            if(document.getElementById("ctl00_ContentPlaceHolder1_rdo_Headselect_0").checked == true)
            {   
                document.getElementById("row_selectCust").style.display = "none"
            }
            else
            {
                document.getElementById("row_selectCust").style.display = "block"
            }
        }
        function ShowCustodian()
        {  
            if(document.getElementById("ctl00_ContentPlaceHolder1_Rdo_Custodian_0").checked == true)
            {   
                document.getElementById("row_Cutodianname").style.display = "block"
                document.getElementById("row_custodainheader").style.display = "block"
                document.getElementById("row_custodain").style.display = "block"
            }
            else
            {
                document.getElementById("row_Cutodianname").style.display = "none"
                 document.getElementById("row_custodainheader").style.display = "none"
                document.getElementById("row_custodain").style.display = "none"
            }
        }
        
        
        function ShowDetails(formName,width,height,top)
        {  
            //var width = "750px"; var height = "225px"; var top = "225px"; For Bank
            var Id = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value;
            
            var CustType = document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType");
            var CustomerType = CustType.options[CustType.options.selectedIndex].text; 
            
            var CustCategory = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Category");
            
            var CustomerCategory = CustCategory.options[CustCategory.options.selectedIndex].text; 
              var CategoryId = document.getElementById("ctl00_ContentPlaceHolder1_cbo_Category").value;
              var CustomerTypeId = document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType").value;
         
            
//            alert(CustomerCategory)
//            alert(CustomerType)   
             
            var ret = ShowDialogDetails(formName + ".aspx",Id,CustomerType,CustomerCategory,CustomerTypeId,CategoryId,width,height,top)			
            if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            }
            else
            {
//                window.location = "ClientProfileDetail.aspx?Id=" + Id;
                 window.location = "ClientProfileMaster.aspx?Id=" + Id;       
                return false
            }	
        }
        function ShowDialogDetails(PageName,customerid,CustomerType,CustomerCategory,CustomerTypeId,CategoryId,strWidth,strHeight,strTop)
		{   
		    var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=" + strTop + "; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
			var OpenUrl = PageName + "?Id=" + customerid + "&CustomerType=" + CustomerType + "&CustomerCategory=" + CustomerCategory + "&CustomerTypeId=" + CustomerTypeId + "&CategoryId=" + CategoryId;  
			var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);		
//			var ret = ShowDialogOpen(pageUrl, "580px", "350px")
            return ret	
		}
        function Validation()
        {
            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType").value) == "")
            {
                alert("Please select the Customer Type");
                return false;
            }
            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_CustomerName").value) == "")
            {
                alert("Please Enter Customer Name");
                return false;
            }
//             if(document.getElementById("ctl00_ContentPlaceHolder1_txt_EmailId").value!="")
//            {    
//                if(Email(document.getElementById("ctl00_ContentPlaceHolder1_txt_EmailId").value)==false)
//                {
//                    alert('Enter Valid Email')
//                    return false
//                }
//            }
            if(document.getElementById("ctl00_ContentPlaceHolder1_Rdo_Custodian_0").checked == true)
            {              
                if((document.getElementById("ctl00_ContentPlaceHolder1_srh_Custodian_Hid_SelectedId").value)== "")
                {
                    alert('Please select Custodian Name')
                    return false
                }
            } 
            return true
        }
        function ValidateInfo(lnkBtn)
        {
            var row = lnkBtn.parentElement.parentElement;
            var SGLTransWith = row.children[0].children[0].value;
             
            if(SGLTransWith == "")
            {
                alert("Please specify SGL Info");
                return false;
            }
        }
         function ValidateDPInfo(lnkBtn)
        {
            var row = lnkBtn.parentElement.parentElement;
            var DpName = row.children[0].children[0].value;
//            alert(DpName)
            var DpId = row.children[1].children[0].value;
//            alert(DpId)
            var ClientId = row.children[2].children[0].value;
//             alert(ClientId)
            if(DpName == "" && DpId == "" && ClientId == "")
            {
                alert("Please specify  DP Info");
                return false;
            }
        }
        
        function ValidateBankInfo(lnkBtn)
        {
            var row = lnkBtn.parentElement.parentElement;
            var BankName = row.children[0].children[0].value;
               
            if(BankName == "")
            {
                alert("Please specify  Bank Info");
                return false;
            }
        }
        function ValidateCustodianBankInfo(lnkBtn)
        {
            var row = lnkBtn.parentElement.parentElement;
            var CustodianBankName = row.children[0].children[0].value;
               
            if(CustodianBankName == "")
            {
                alert("Please specify  Bank Info");
                return false;
            }
        }
        
         function ValidateContactInfo(lnkBtn)
        {
            var row = lnkBtn.parentElement.parentElement;
            var ContactPerson = row.children[0].children[0].value;
            var PhoneNo = row.children[0].children[0].value;
           
             
            if(ContactPerson == "" )
            {
                alert("Please specify  Contact Info");
                return false;
            }
        }
        
        function ValidateSignaturyInfo(lnkBtn)
        {
            var row = lnkBtn.parentElement.parentElement;
            var SignaturyName = row.children[0].children[0].value;
            
             
            if(SignaturyName == "" )
            {
                alert("Please specify  Signatory  Info");
                return false;
            }
        }
         function  showPurchaseOrder()
        {
            var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=650px; dialogTop=50px; dialogHeight=550px; Help=No; Status=No; Resizable=Yes;";
            var CustomerId = document.getElementById("ctl00_ContentPlaceHolder1_Hid_CustomerId").value
           
            var OpenUrl = "ShowClientPurImage.aspx?CustomerId="+CustomerId
            var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
            if(ret == "" || typeof(ret)== "undefined")
            {
                return false
             } 
        }
       
        
        
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center" colspan="6">
                Client Profile Master</td>
        </tr>
        <tr>
            <td class="SectionHeaderCSS" align="left" colspan="6">
                MAIN SECTION</td>
        </tr>
        <tr>
            <td colspan="6" class="SeperatorRowCSS">
            </td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <atlas:UpdatePanel ID="UpdatePanel5" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="Hid_DecType" runat="server"></asp:HiddenField>
                        <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="80%">
                            <tr>
                                <td class="LabelCSS" width="250">
                                    Customer Group:</td>
                                <td align="left">
                                    <asp:DropDownList ID="cbo_CustomerGroup" Width="202px" runat="server" CssClass="ComboBoxCSS"
                                        AutoPostBack="false" TabIndex="1">
                                    </asp:DropDownList><i style="color: red">*</i>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS" width="250">
                                    Customer Type:</td>
                                <td align="left">
                                    <asp:DropDownList ID="cbo_CustomerType" Width="202px" runat="server" CssClass="ComboBoxCSS"
                                        AutoPostBack="True" TabIndex="1">
                                    </asp:DropDownList><i style="color: red">*</i>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Customer Category:</td>
                                <td align="left">
                                    <asp:DropDownList ID="cbo_Category" Width="202px" runat="server" CssClass="ComboBoxCSS"
                                        TabIndex="1" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Customer Name:</td>
                                <td align="left">
                                    <asp:TextBox ID="txt_CustomerName" runat="server" CssClass="TextBoxCSS" Width="200px"
                                        TabIndex="2"></asp:TextBox><i style="color: red">*</i></td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Customer Prefix:</td>
                                <td align="left">
                                    <asp:TextBox ID="txt_CustPrefix" runat="server" CssClass="TextBoxCSS" Width="200px"
                                        TabIndex="2"></asp:TextBox><i style="color: red">*</i></td>
                            </tr>
                            <tr id="Tr1" visible="false" runat="server">
                                <td class="LabelCSS">
                                    Head:</td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rdo_Headselect" runat="server" BorderStyle="None" BorderWidth="1px"
                                        CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Value="N">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="row_selectCust" runat="server" visible="false">
                                <td class="LabelCSS">
                                    Select Head:
                                </td>
                                <td align="left">
                                    <uc:Search ID="srh_HeadCustomer" runat="server" AutoPostback="true" ProcName="ID_SEARCH_HeadCustomer"
                                        ConditionExist="true" SelectedFieldName="CustomerName" SourceType="StoredProcedure"
                                        TableName="CustomerMaster" ConditionalFieldName=" " ConditionalFieldId=" ">
                                    </uc:Search>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Address1:</td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Address1" runat="server" CssClass="TextBoxCSS" Width="200px"
                                        TabIndex="3"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Address2:</td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Address2" runat="server" CssClass="TextBoxCSS" Width="200px"
                                        TabIndex="3"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    City:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_City" runat="server" CssClass="TextBoxCSS" TabIndex="6"></asp:TextBox>
                                    <asp:Button ID="btn_Address" runat="server" Text="Add Address" ToolTip="Add Address"
                                        CssClass="ButtonCSS" TabIndex="34" Width="35%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Pin code:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_PinCode" runat="server" CssClass="TextBoxCSS" TabIndex="6"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    State:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_State" runat="server" CssClass="TextBoxCSS" TabIndex="6"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Country:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Country" runat="server" CssClass="TextBoxCSS" TabIndex="6"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="LabelCSS" style="height: 26px">
                                    Phone No:
                                </td>
                                <td align="left" style="height: 26px">
                                    <asp:TextBox ID="txt_PhoneNo" runat="server" CssClass="TextBoxCSS" TabIndex="8" Height="30px"
                                        TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Fax No:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_FaxNo" runat="server" CssClass="TextBoxCSS" TabIndex="9" Height="30px"
                                        TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    PAN No:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_PANNo" runat="server" CssClass="TextBoxCSS" TabIndex="12"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="Tr2" visible="false" runat="server">
                                <td class="LabelCSS">
                                    Accessible To:
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cbo_Accessible" runat="server" CssClass="ComboBoxCSS" Width="130px">
                                        <asp:ListItem Value="A">All</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="C">CurrentUser</asp:ListItem>
                                        <asp:ListItem Value="B">Branch</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Web Site:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_EmailId" runat="server" CssClass="TextBoxCSS" Width="200px"
                                        TabIndex="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS">
                                    Custodian:</td>
                                <td align="left">
                                    <asp:RadioButtonList ID="Rdo_Custodian" runat="server" BorderStyle="None" BorderWidth="1px"
                                        CssClass="LabelCSS" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="row_Cutodianname">
                                <td class="LabelCSS">
                                    Custodian Name:
                                </td>
                                <td align="left">
                                    <uc:Search ID="srh_Custodian" runat="server" AutoPostback="false" ProcName="ID_SEARCH_CustodianMaster"
                                        ConditionExist="true" SelectedFieldName="CustodianName" SourceType="StoredProcedure"
                                        TableName="CustodianMaster" ConditionalFieldName=" " ConditionalFieldId=" ">
                                    </uc:Search>
                                    <%-- <asp:TextBox ID="txt_Custodian" runat="server" CssClass="TextBoxCSS"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr id="row_EmpalmentDt">
                                <td class="LabelCSS">
                                    Empalment Start date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_empalmentdate" runat="server" CssClass="TextBoxCSS"></asp:TextBox>
                                    <a>
                                        <img class="formcontent" height="14" src="../Images/Calender.jpg" width="14" border="0"
                                            onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_empalmentdate',this);"
                                            align="middle" style="cursor: hand"></a>
                                </td>
                            </tr>
                            <tr id="row_EmpalmentFrequ">
                                <td class="LabelCSS">
                                    Empalment Frequency:
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cbo_FrequencyEmpalment" Width="150px" runat="server" CssClass="ComboBoxCSS"
                                        TabIndex="1">
                                        <asp:ListItem Value="Y" Selected="True">Yearly</asp:ListItem>
                                        <asp:ListItem Value="H">HalfYearly</asp:ListItem>
                                        <asp:ListItem Value="Q">Quarterlty</asp:ListItem>
                                        <asp:ListItem Value="M">Monthly</asp:ListItem>
                                        <asp:ListItem Value="N">None</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="tr_Emp">
                                <td class="LabelCSS" width="250">
                                    Empalement Documents Submitted:</td>
                                <td style="width: 207px">
                                    <uc:SelectFields ID="srh_EmpalementDocuments" class="LabelCSS" runat="server" ProcName="ID_SEARCH_DocumentEmp"
                                        FormHeight="470" FormWidth="257" SelectedValueName="DTM.DocumentTypeId" ChkLabelName=""
                                        ConditionalFieldId="cbo_CustomerType" LabelName="" SelectedFieldName="DocumentTypeName"
                                        SourceType="StoredProcedure" ConditionalFieldName="CustomerTypeId" Visible="true"
                                        ShowLabel="false" Height="40"></uc:SelectFields>
                                </td>
                            </tr>
                            <tr id="tr_Kyc">
                                <td class="LabelCSS">
                                    KYC Documents Submitted:</td>
                                <td style="width: 207px">
                                    <uc:SelectFields ID="srh_KYCDocuments" class="LabelCSS" runat="server" ProcName="ID_SEARCH_Documentkyc"
                                        FormHeight="470" FormWidth="257" SelectedValueName="DTM.DocumentTypeId" ChkLabelName=""
                                        ConditionalFieldId="cbo_CustomerType" LabelName="" SelectedFieldName="DocumentTypeName"
                                        SourceType="StoredProcedure" ConditionalFieldName="CustomerTypeId" Visible="true"
                                        ShowLabel="false" Height="40"></uc:SelectFields>
                                </td>
                            </tr>
                            <%-- 
                            <tr>
                                <td class="LabelCSS" width="250">
                                    Documents Submitted:
                                </td>
                                <td align="left">
                                    <table align="left" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td>
                                                <uc:SelectFields ID="srh_DocumentTypeName" class="LabelCSS" runat="server" ProcName="ID_SEARCH_DocumentTypeMaster"
                                                    FormHeight="470" FormWidth="257" SelectedValueName="CTM.DocumentTypeId" ChkLabelName=""
                                                    ConditionalFieldId="" LabelName="" SelectedFieldName="DocumentTypeName" SourceType="StoreProcedure"
                                                    ConditionalFieldName="" Visible="true" ShowLabel="false" Height="40">
                                                </uc:SelectFields>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="LabelCSS" width="150">
                                    Business Type:
                                </td>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <uc:SelectFields ID="srh_BusniessType" class="LabelCSS" runat="server" ProcName="ID_SEARCH_BusinessTypeMaster"
                                                    FormHeight="470" FormWidth="257" SelectedValueName="BusinessTypeId" ChkLabelName=""
                                                    ConditionalFieldId="" LabelName="" SelectedFieldName="BusinessType" SourceType="StoredProcedure"
                                                    ConditionalFieldName="" Visible="true" ShowLabel="false" Height="40">
                                                </uc:SelectFields>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
            <td valign="top" align="center">
                <table id="Table4" align="center" cellspacing="0" cellpadding="0" border="0" width="80%">
                    <tr>
                        <td class="SubHeaderCSS" align="left" colspan="3">
                            SGL Details
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                                <ContentTemplate>
                                    <div id="div_SGL" style="margin-top: 0px; overflow: auto; width: 500px; padding-top: 0px;
                                        position: relative; height: 80px">
                                        <asp:DataGrid ID="dg_SGL" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="false"
                                            TabIndex="38" Width="500px">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="SGLTransWith">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SGLTransWith" Width="275px" runat="server" Text='<%# container.dataitem("SGLTransWith") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_SGLTransWith" CssClass="TextBoxCSS" runat="server" align="left"
                                                            onblur="ConvertUCase(this);" Text='<%# container.dataitem("SGLTransWith") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_SGLTransWith" CssClass="TextBoxCSS" runat="server" Width="450px"
                                                            onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" Width="20px"
                                                            CommandName="Edit" CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                                            CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                                            CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                            OnClientClick="return ValidateInfo(this)">
                                                        </asp:LinkButton>
                                                    </FooterTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                            CommandName="delete" CssClass="InfoLinkCSS" />
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </ContentTemplate>
                            </atlas:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHeaderCSS" align="left" colspan="3">
                            &nbsp;DP Details
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <atlas:UpdatePanel ID="UpdatePanel2" runat="server" Mode="Conditional">
                                <ContentTemplate>
                                    <div id="div1" style="margin-top: 0px; overflow: auto; width: 500px; padding-top: 0px;
                                        position: relative; height: 80px">
                                        <asp:DataGrid ID="dg_DP" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="false"
                                            TabIndex="38" Width="500px">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Dp Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DpName" runat="server" Width="120px" Text='<%# container.dataitem("DpName") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_DpName" CssClass="TextBoxCSS" Width="75px" runat="server" onblur="ConvertUCase(this);"
                                                            Text='<%# container.dataitem("DpName") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_DpName" CssClass="TextBoxCSS" Width="120px" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="120px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Dp Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DpId" Width="75px" runat="server" Text='<%# container.dataitem("DpId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_DpId" CssClass="TextBoxCSS" Width="75px" runat="server" Text='<%# container.dataitem("DpId") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_DpId" CssClass="TextBoxCSS" Width="120px" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="120px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" Width="60px" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Client Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ClientId" Width="75px" runat="server" Text='<%# container.dataitem("ClientId") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_ClientId" CssClass="TextBoxCSS" Width="75px" runat="server"
                                                            onblur="ConvertUCase(this);" Text='<%# container.dataitem("ClientId") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_ClientId" CssClass="TextBoxCSS" Width="120px" runat="server"
                                                            onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="120px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Edit"
                                                            CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                                            CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                                            CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                            OnClientClick="return ValidateDPInfo(this)">
                                                        </asp:LinkButton>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                            CommandName="delete" CssClass="InfoLinkCSS" />
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </ContentTemplate>
                            </atlas:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHeaderCSS" align="left" colspan="3">
                            &nbsp;Bank Details
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" style="height: 50px">
                            <atlas:UpdatePanel ID="UpdatePanel3" runat="server" Mode="Conditional">
                                <ContentTemplate>
                                    <div id="div4" style="margin-top: 0px; overflow: auto; width: 500px; padding-top: 0px;
                                        position: relative; height: 80px">
                                        <asp:DataGrid ID="dg_Bank" runat="server" CssClass="GridCSS" ShowFooter="True" AutoGenerateColumns="false"
                                            TabIndex="38" Width="500px">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Bank Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_BankName" Width="75px" runat="server" Text='<%# container.dataitem("BankName") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_BankName" CssClass="TextBoxCSS" Width="75px" runat="server"
                                                            onblur="ConvertUCase(this);" Text='<%# container.dataitem("BankName") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_BankName" CssClass="TextBoxCSS" Width="120px" runat="server"
                                                            onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="120px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Account No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_AccountNo" Width="75px" runat="server" Text='<%# container.dataitem("AccountNo") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_AccountNo" CssClass="TextBoxCSS" Width="75px" runat="server"
                                                            onblur="ConvertUCase(this);" Text='<%# container.dataitem("AccountNo") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_AccountNo" CssClass="TextBoxCSS" Width="70px" runat="server"
                                                            onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="70px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Branch">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Branch" Width="75px" runat="server" Text='<%# container.dataitem("Branch") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Branch" CssClass="TextBoxCSS" Width="75px" onblur="ConvertUCase(this);"
                                                            runat="server" Text='<%# container.dataitem("Branch") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_Branch" CssClass="TextBoxCSS" Width="120px" runat="server" onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="120px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="RTGS Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_RTGSCode" Width="75px" runat="server" Text='<%# container.dataitem("RTGSCode") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_RTGSCode" CssClass="TextBoxCSS" Width="75px" runat="server"
                                                            onblur="ConvertUCase(this);" Text='<%# container.dataitem("RTGSCode") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_RTGSCode" CssClass="TextBoxCSS" Width="70px" runat="server"
                                                            onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="70px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" Width="50px" CausesValidation="false"
                                                            CommandName="Edit" CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                                            CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                                            CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                            OnClientClick="return ValidateBankInfo(this)">
                                                        </asp:LinkButton>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                            CommandName="delete" CssClass="InfoLinkCSS" />
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </ContentTemplate>
                            </atlas:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHeaderCSS" align="left" colspan="3">
                            Signatory Details
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div id="dv_Signatury" style="margin-top: 0px; overflow: auto; width: 500px; padding-top: 0px;
                                position: relative; height: 80px">
                                <asp:DataGrid ID="dg_Signatury" runat="server" CssClass="GridCSS" ShowFooter="True"
                                    AutoGenerateColumns="false" TabIndex="38" Width="500px">
                                    <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="Signatory Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_signaturyName" Width="150px" runat="server" Text='<%# container.dataitem("SignaturyName") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt_signatury" CssClass="TextBoxCSS" Width="150px" runat="server"
                                                    onblur="ConvertUCase(this);"></asp:TextBox>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" Width="150px" />
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="ContentType" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_FileContent" Width="75px" runat="server" Text='<%# container.dataitem("ContentType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="ContentLength" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ContentLength" Width="75px" runat="server" Text='<%# container.dataitem("ContentLength") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="FileBytes" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_FileBytes" Width="75px" runat="server" Text='<%# container.dataitem("FileBytes") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="FileName">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="file_AddFile" runat="server" Text='<%# container.dataitem("FileName") %>'
                                                    CommandName="Show"> </asp:LinkButton>
                                                <asp:Label ID="lbl_AddFile" Width="75px" runat="server" Text="Not Available" Visible="false"
                                                    CssClass="MessageCSS"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:FileUpload ID="file_AddFile" runat="server" Width=" " />
                                            </FooterTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                    CommandName="delete" CssClass="InfoLinkCSS" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                    OnClientClick="return ValidateSignaturyInfo(this)">
                                                </asp:LinkButton>
                                            </FooterTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </td>
                    </tr>
                    <%--====================================--%>
                    <tr id="row_custodainheader">
                        <td class="SubHeaderCSS" align="left" colspan="3">
                            &nbsp;Custodian Bank Details
                        </td>
                    </tr>
                    <tr id="row_custodain">
                        <td align="left">
                            <atlas:UpdatePanel ID="UpdatePanel4" runat="server" Mode="Conditional">
                                <ContentTemplate>
                                    <div id="div3" style="margin-top: 0px; overflow: auto; width: 500px; padding-top: 0px;
                                        position: relative; height: 80px">
                                        <asp:DataGrid ID="dgCustodianbankdetails" runat="server" CssClass="GridCSS" ShowFooter="True"
                                            AutoGenerateColumns="false" TabIndex="38" Width="500px">
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                            <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Bank Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CustodianBankName" Width="75px" runat="server" Text='<%# container.dataitem("CustodianBankName") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_CustodianBankName" CssClass="TextBoxCSS" Width="75px" runat="server"
                                                            onblur="ConvertUCase(this);" Text='<%# container.dataitem("CustodianBankName") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_CustodianBankName" CssClass="TextBoxCSS" Width="120px" runat="server"
                                                            onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="120px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Account No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CustodianAccountNo" Width="75px" runat="server" Text='<%# container.dataitem("CustodianAccountNo") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_CustodianAccountNo" CssClass="TextBoxCSS" Width="75px" runat="server"
                                                            onblur="ConvertUCase(this);" Text='<%# container.dataitem("CustodianAccountNo") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_CustodianAccountNo" CssClass="TextBoxCSS" Width="70px" runat="server"
                                                            onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="70px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Branch">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CustodianBranch" Width="75px" runat="server" Text='<%# container.dataitem("CustodianBranch") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_CustodianBranch" CssClass="TextBoxCSS" Width="75px" onblur="ConvertUCase(this);"
                                                            runat="server" Text='<%# container.dataitem("CustodianBranch") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_CustodianBranch" CssClass="TextBoxCSS" Width="120px" runat="server"
                                                            onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="120px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="RTGS Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CustodianRTGSCode" Width="75px" runat="server" Text='<%# container.dataitem("CustodianRTGSCode") %>'
                                                            CssClass="LabelCSS"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_CustodianRTGSCode" CssClass="TextBoxCSS" Width="75px" runat="server"
                                                            onblur="ConvertUCase(this);" Text='<%# container.dataitem("CustodianRTGSCode") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_CustodianRTGSCode" CssClass="TextBoxCSS" Width="70px" runat="server"
                                                            onblur="ConvertUCase(this);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="70px" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" Width="50px" CausesValidation="false"
                                                            CommandName="Edit" CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                                            CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                                            CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="addbtn" runat="server" Text="Add" CssClass="InfoLinkCSS" CommandName="add"
                                                            OnClientClick="return ValidateCustodianBankInfo(this)">
                                                        </asp:LinkButton>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                                            CommandName="delete" CssClass="InfoLinkCSS" />
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </ContentTemplate>
                            </atlas:UpdatePanel>
                        </td>
                    </tr>
                    <%--  ============================================--%>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table id="Table2" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr id="row_MerchantbankProfile" runat="server">
                        <td align="center" rowspan="7" valign="top">
                            <asp:Button ID="Btn_PMSProfile" runat="server" Text="PMS Client Profile" ToolTip="PMS Client Profile"
                                CssClass="ButtonCSS" TabIndex="34" />
                        </td>
                        <td align="center" rowspan="7" valign="top">
                            <asp:Button ID="btn_MerchantbankProfile" runat="server" Text="Merchant Banking Profile"
                                ToolTip="Merchant Banking Profile" CssClass="ButtonCSS" TabIndex="34" />
                        </td>
                    </tr>
                    <tr id="row_Insurance" runat="server">
                        <td align="center">
                            <asp:Button ID="btn_SaveEditInsurance" runat="server" CssClass="ButtonCSS" Text="WDM Client Proflie" />&nbsp;
                        </td>
                    </tr>
                    <tr id="row_PFDetails" runat="server">
                        <td align="center">
                            <asp:Button ID="btn_SaveEditPFDetails" runat="server" CssClass="ButtonCSS" Text="WDM Client Profile" />&nbsp;
                        </td>
                    </tr>
                    <tr id="row_MFDetails" runat="server">
                        <td align="center" style="height: 23px">
                            <asp:Button ID="btn_SaveEditMFDetails" runat="server" CssClass="ButtonCSS" Text="WDM Client Profile" />&nbsp;
                        </td>
                    </tr>
                    <tr id="row_BankDetails" runat="server">
                        <td align="center">
                            <asp:Button ID="btn_SaveEditBankDetails" runat="server" CssClass="ButtonCSS" Text="WDM Client Profile" />&nbsp;
                        </td>
                    </tr>
                    <tr id="row_CoOpBank" runat="server">
                        <td align="center">
                            <asp:Button ID="btn_CoOpBank" runat="server" CssClass="ButtonCSS" Text="WDM Client Profile" />&nbsp;
                        </td>
                    </tr>
                    <tr id="row_OtherDetails" runat="server">
                        <td align="center">
                            <asp:Button ID="btn_SaveEditOtherDetails" runat="server" CssClass="ButtonCSS" Text="WDM Client Profile" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="row_contact" runat="server" visible="false">
            <td class="SubHeaderCSS" align="left" colspan="3">
                Contact Details
                <asp:Button ID="btn_AddDetails" runat="server" Text="Add Details" ToolTip="Add Details"
                    CssClass="ButtonCSS" TabIndex="34" />
            </td>
        </tr>
        <tr id="row_dgcontact" runat="server" visible="false">
            <td colspan="4" align="center">
                &nbsp;
                <div id="Div2" style="margin-top: 0px; overflow: auto; width: 700px; padding-top: 0px;
                    position: relative; height: 130px; left: 0px; top: 0px;" align="center">
                    <asp:DataGrid ID="dg_Contact" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                        Width="700px" CssClass="GridCSS">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                        <FooterStyle HorizontalAlign="Center" CssClass="footer" VerticalAlign="Middle"></FooterStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="Contact Person">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_ContactPerson" BackColor="white" Width="120px" Style="border-left-width: 0;
                                        border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("ContactPerson") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Designation">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Designation" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Designation") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="PhoneNo1">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_PhoneNo1" Width="75px" runat="server" Text='<%# container.dataitem("PhoneNo1") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="MobileNo">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_MobileNo" Width="75px" runat="server" Text='<%# container.dataitem("MobileNo") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Email Id">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_EmailId" Width="75px" runat="server" Text='<%# container.dataitem("EmailId") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="BusinessTypeNames">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_BusinessTypeNames" BackColor="white" Width="120px" Style="border-left-width: 0;
                                        border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.BusinessTypeNames") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="BusniessTypeIds" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BusniessTypeIds" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BusniessTypeIds") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="NameOfUsers">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_NameOfUsers" BackColor="white" Width="120px" Style="border-left-width: 0;
                                        border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                        runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.NameOfUsers") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="UserIds" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_UserIds" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserIds") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="PhoneNo2" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_PhoneNo2" Width="75px" runat="server" Text='<%# container.dataitem("PhoneNo2") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="FaxNo1" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FaxNo1" Width="75px" runat="server" Text='<%# container.dataitem("FaxNo1") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="FaxNo2" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FaxNo2" Width="75px" runat="server" Text='<%# container.dataitem("FaxNo2") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="ContactDetailId" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ContactDetailId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ContactDetailId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="imgBtn_Edit" CommandName="Edit" runat="server" ToolTip="Edit"
                                        CssClass="TitleText" Text="Edit">                                                                                                          
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle Wrap="False"></FooterStyle>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="imgBtn_Del" CommandName="Delete" runat="server" ToolTip="Delete"
                                        CssClass="TitleText" Text="Delete">
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle Wrap="False"></FooterStyle>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle PageButtonCount="2" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td class="ForControls" align="center" valign="middle" colspan="4">
                <asp:Label ID="LabelError" runat="server" Width="488px" CssClass="ForErrorMessages"
                    Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td class="SeperatorRowCSS" colspan="4">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                <asp:HiddenField ID="Hid_CustImgId" runat="server" />
                <asp:HiddenField ID="Hid_RetValues" runat="server" />
                <asp:HiddenField ID="Hid_arrContactDetailIds" runat="server" />
                <asp:HiddenField ID="Hid_BusinessTypeId" runat="server" />
                <asp:HiddenField ID="Hid_ClientBusniessDetailId" runat="server" />
                <asp:HiddenField ID="Hid_BusinessTypeNames" runat="server" />
                <asp:HiddenField ID="Hid_ContactDetailId" runat="server" />
                <asp:HiddenField ID="Hid_ContactDetailIds" runat="server" />
                <asp:HiddenField ID="Hid_UserIds" runat="server" />
                <asp:HiddenField ID="Hid_NameOfUsers" runat="server" />
                <asp:HiddenField ID="Hid_PhoneNo2" runat="server" />
                <asp:HiddenField ID="Hid_FaxNo1" runat="server" />
                <asp:HiddenField ID="Hid_FaxNo2" runat="server" />
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
