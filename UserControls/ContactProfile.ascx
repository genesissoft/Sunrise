<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ContactProfile.ascx.vb"
    Inherits="UserControls_ContactProfile" %>

<script type="text/javascript" src="../Include/Common.js"></script>

<link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

<script language="javascript" type="text/javascript">

        
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
         function Deletedetails1(ContactDetailId,parentId)
        {
                    if(window.confirm("Are you sure u want to delete this detail record"))
                    {   
                        document.getElementById(parentId + "_Hid_ContactDetailId").value=ContactDetailId;  
                        return true;
                    } 
                    else
                    {
                         return false;
                    }   
        }


        function AddSource(parentId,MB)
        {
            var strId = document.getElementById(parentId + "_Hid_SourceCustomerId").value
           
            var ret = OpenDialog("ClientSourceDetails.aspx",strId,"450px","150px",parentId,MB)
           

             if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            }
            else
            {
                document.getElementById(parentId + "_Hid_RetValues").value = ret
                return true
            }
          
        }
        
         function UpdateSource(rowIndex,parentId,img,MB)
        {
//            alert(MB)   
            var pageUrl = "ClientSourceDetails.aspx";
            var strValues = "";
            var selValues = "";
            
                     
            var strId = document.getElementById(parentId + "_Hid_SourceCustomerId").value 
            var ctrlItem = img.parentElement.parentElement
            var hidBusinessTypeId = document.getElementById(parentId + "_Hid_BusTypeId").value.split("!")     
            var hidSourceId = document.getElementById(parentId + "_Hid_SourceId").value.split("!")    

            strValues = strValues + ctrlItem.children(2).children(0).innerHTML + "!" 
            strValues = strValues + ctrlItem.children(3).children(0).innerHTML + "!"  
//            strValues = strValues + ctrlItem.children(4).children(0).innerHTML + "!"                                         
            strValues = strValues + hidBusinessTypeId[rowIndex] + "!"   
            strValues = strValues + hidSourceId[rowIndex] + "!"
                           
            pageUrl = pageUrl + "?CustomerId=" + strId + "&Values=" + strValues 
            var ret = OpenDialog(pageUrl,strId, "450px", "150px",parentId,MB)        
            if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            } 
            else
            {
                document.getElementById(parentId + "_Hid_RetValues").value = ret
                return true
            }
        }
        function AddContact(parentId,MB)
        {
            var strId = document.getElementById(parentId + "_Hid_CustomerId").value
            var ret = OpenDialog("ClientContactPerson.aspx",strId,"650px","420px",parentId,MB)
           
             if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            }
            else
            {
                document.getElementById(parentId + "_Hid_RetValues").value = ret
                return true
            }
          
        }
        
        
          function UpdateContactDetails(rowIndex,parentId,img,MB)
        {
            
            var pageUrl = "ClientContactPerson.aspx";
            var strValues = "";
            var selValues = "";
                     
            var strId = document.getElementById(parentId + "_Hid_CustomerId").value 
            var ctrlItem = img.parentElement.parentElement
            var hidBusinessTypeIds = document.getElementById(parentId + "_Hid_BusinessTypeId").value.split("!")     
            var hidUserIds = document.getElementById(parentId + "_Hid_UserIds").value.split("!") 
            var hidNameOfUsers = document.getElementById(parentId + "_Hid_NameOfUsers").value.split("!") 
            var hidPhoneNo2 = document.getElementById(parentId + "_Hid_PhoneNo2").value.split("!") 
            var hidFaxNo1 = document.getElementById(parentId + "_Hid_FaxNo1").value.split("!") 
            var hidFaxNo2 = document.getElementById(parentId + "_Hid_FaxNo2").value.split("!") 
            var hidSectionType = document.getElementById(parentId + "_Hid_SectionType").value.split("!") 
            
            var hidResearchDocId = document.getElementById(parentId + "_Hid_ResearchDocId").value.split("!") 
            var hidResearchDocName = document.getElementById(parentId + "_Hid_ResearchDocName").value.split("!")   
           
            var hidinteraction = document.getElementById(parentId + "_Hid_Interaction").value.split("!") 

            var hidBranch = document.getElementById(parentId + "_Hid_Branch").value.split("!") 

            strValues = strValues + ctrlItem.children(2).children(0).value + "!" 
            strValues = strValues + ctrlItem.children(3).children(0).innerHTML + "!"  
            strValues = strValues + ctrlItem.children(4).children(0).innerHTML + "!" 
            strValues = strValues + ctrlItem.children(5).children(0).innerHTML + "!"  
            strValues = strValues + ctrlItem.children(6).children(0).innerHTML + "!"    
            strValues = strValues + ctrlItem.children(7).children(0).value + "!"                                         
            strValues = strValues + hidBusinessTypeIds[rowIndex] + "!"   
            strValues = strValues + hidNameOfUsers[rowIndex] + "!"     
            strValues = strValues + hidUserIds[rowIndex] + "!" 
            strValues = strValues + hidPhoneNo2[rowIndex] + "!" 
            strValues = strValues + hidFaxNo1[rowIndex] + "!" 
            strValues = strValues + hidFaxNo2[rowIndex] + "!" 
            strValues = strValues + hidSectionType[rowIndex] + "!" 
            strValues = strValues + hidResearchDocName[rowIndex] + "!"     
            strValues = strValues + hidResearchDocId[rowIndex] + "!" 
            strValues = strValues + hidinteraction[rowIndex] + "!" 
            strValues = strValues + hidBranch[rowIndex] + "!" 
           
           
            
                           
         
            strValues = strValues.replace("&"," ")                     
            pageUrl = pageUrl + "?CustomerId=" + strId + "&Values=" + strValues 
            var ret = OpenDialog(pageUrl,strId, "650px","410px",parentId,MB)        
            if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            } 
            else
            {
                document.getElementById(parentId + "_Hid_RetValues").value = ret
                return true
            }
        }
        
        
         function OpenDialog(PageName,CustomerId,strWidth, strHeight,parentId,MB)
		{
 
		    var DialogOptions = "Center=Yes; Scrollbar=No; dialogWidth=" + strWidth + "; dialogTop=150px; dialogHeight=" + strHeight + "; Help=No; Status=No; Resizable=No;";
			var OpenUrl = PageName + "?Id=" + CustomerId + "&MercBanking=" + MB 
			var ret = window.showModalDialog(OpenUrl, "Yes", DialogOptions);
			return ret
		}
		
        function AddAddress(parentId,MB)
        {         

           var strId = (document.getElementById(parentId + "_Hid_AddCustomerId").value);
            var ret = OpenDialog("AddressMultiple.aspx",strId,"850px", "300px",parentId,MB)
           
            if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            }
            else
            {
                document.getElementById(parentId + "_Hid_RetValues").value = ret
                return true
            }

        }
        
        
         function UpdateAddressDetails(img,rowIndex,parentId,MB)
        {   
            
            var pageUrl = "AddressMultiple.aspx";
            var pagename = "CustBankDetails.aspx"; 
            var strValues = "";
            var selValues = "";                     
            var id = document.getElementById(parentId + "_Hid_AddCustomerId").value 
            var hidAddBusinessTypeIds = document.getElementById(parentId + "_Hid_AddBusinessTypeId").value.split("!")   
            var ctrlItem = img.parentElement.parentElement

                           
                    
            strValues = strValues + ctrlItem.children(2).children(0).innerHTML + "!"   
            strValues = strValues + ctrlItem.children(3).children(0).value + "!"   
            strValues = strValues + ctrlItem.children(4).children(0).value + "!"   
            strValues = strValues + ctrlItem.children(5).children(0).innerHTML + "!"   
            strValues = strValues + ctrlItem.children(6).children(0).innerHTML + "!"   
            strValues = strValues + ctrlItem.children(7).children(0).innerHTML + "!"   
            strValues = strValues + ctrlItem.children(8).children(0).innerHTML + "!"   
            strValues = strValues + ctrlItem.children(9).children(0).innerHTML + "!"   
            strValues = strValues + ctrlItem.children(10).children(0).innerHTML + "!"     
            strValues = strValues + ctrlItem.children(11).children(0).innerHTML + "!" 
            strValues = strValues + ctrlItem.children(12).children(0).value + "!" 
            strValues = strValues + hidAddBusinessTypeIds[rowIndex] + "!"            
            pageUrl = pageUrl + "?CustomerId=" + id + "&Values=" + strValues 
            var ret = OpenDialog(pageUrl, id,"850px", "300px",parentId,MB)        
            if(ret=="" || typeof(ret)=="undefined")
            {
                return false
            } 
            else
            {
                document.getElementById(parentId + "_Hid_RetValues").value = ret
                return true
            }
        }
        
       
		
		
		   
</script>

<table id="Table1" width="98%" align="left" cellspacing="0" cellpadding="0" border="0">
    <tr>
        <td class="SectionHeaderCSS1" colspan="2" align="left">
            Contact Details:</td>
    </tr>
    <tr>
        <td>
            <table id="Table4" align="center" cellspacing="0" cellpadding="0" border="1" width="100%">
                <tr align="center">
                    <td valign="middle" colspan="8" align="center">
                        <asp:Button ID="btn_AddContact" runat="server" CssClass="ButtonCSS" Text="Add Contact" />
                    </td>
                </tr>
                <tr>
                    <td colspan="8" align="center" valign="top">
                        <div id="Div2" style="margin-top: 0px; overflow: auto; width: 850px; padding-top: 0px;
                            position: relative; height: 80px" align="center">
                            <asp:DataGrid ID="dg_Contact" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                Width="850px" CssClass="GridCSS">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                <FooterStyle HorizontalAlign="Center" CssClass="footer" VerticalAlign="Middle"></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit"
                                                ToolTip="Edit" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                CommandName="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>                                   
                                    <asp:TemplateColumn HeaderText="Contact Person">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_ContactPerson" BackColor="white" Width="120px" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeydown="OnlyScroll();"
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
                                    <asp:TemplateColumn HeaderText="BusinessType">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lbl_BusinessTypeNames" BackColor="white" Width="120px" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="OnlyScroll();"
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
                                    <asp:TemplateColumn HeaderText="DealerName">
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
                                    <asp:TemplateColumn HeaderText="Branch">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Branch" Width="75px" runat="server" CssClass="LabelCSS" Text='<%# DataBinder.Eval(Container, "DataItem.Branch") %>'></asp:Label>
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
                                    <asp:TemplateColumn HeaderText="SectionType" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_SectionType" Width="75px" runat="server" Text='<%# container.dataitem("SectionType") %>'
                                                CssClass="LabelCSS"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="ResearchDocName" Visible="False">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lbl_ResearchDocName" BackColor="white" Width="120px" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.ResearchDocName") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="ResearchDocId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ResearchDocId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResearchDocId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Interaction" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Interaction" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Interaction") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />                                    
                                    </asp:TemplateColumn>
                                     <asp:TemplateColumn Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ContactId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ContactId") %>'>  </asp:Label>
                                             <asp:Label ID="lbl_ContatDetailId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ContactDetailId") %>'>  </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle PageButtonCount="2" />
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="SectionHeaderCSS1" colspan="2" align="left">
            Address Details:
        </td>
    </tr>
    <tr>
        <td>
            <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="1" width="100%">
                <tr align="center">
                    <td valign="middle" colspan="8">
                        <asp:Button ID="btn_AddAddress" runat="server" CssClass="ButtonCSS" Text="Add Address" />
                    </td>
                </tr>
                <tr>
                    <td id="td1" align="center" valign="top" runat="server" colspan="8">
                        <div id="div1" style="margin-top: 0px; overflow: auto; width: 850px; padding-top: 0px;
                            position: relative; height: 80px" align="center">
                            <asp:DataGrid ID="dg_Address" runat="server" CssClass="GridCSS" ShowFooter="True"
                                AutoGenerateColumns="False" TabIndex="38" Width="850px">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                <Columns>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit"
                                                ToolTip="Edit" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                CommandName="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="CustomerBranch">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CustomerBranchName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerBranchName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Address1">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Address1" BackColor="white" Width="120px" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="OnlyScroll();"
                                                runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("Address1") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Address2">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Address2" BackColor="white" Width="120px" Style="border-left-width: 0;
                                                border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="OnlyScroll();"
                                                runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("Address2") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="City">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_City" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.City") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="PinCode">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_PinCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PinCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="State">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_State" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.State") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Country">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Country" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Country") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Phone">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Phone" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Phone") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="FaxNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_FaxNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FaxNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="EmailId">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_EmailId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EmailId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="CustomerId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CustomerId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="BusinessType">
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
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="SectionHeaderCSS1" colspan="2" align="left">
            Source Details:
        </td>
    </tr>
    <tr>
        <td>
            <table id="Table2" align="center" cellspacing="0" cellpadding="0" border="1" width="100%">
                <tr align="center">
                    <td valign="middle" colspan="8">
                        <asp:Button ID="btn_AddSourceDetail" runat="server" CssClass="ButtonCSS" Text="Add Source" />
                    </td>
                </tr>
                <tr>
                    <td id="td2" align="center" valign="top" runat="server" colspan="8">
                        <div id="div3" style="margin-top: 0px; overflow: auto; width: 850px; padding-top: 0px;
                            position: relative; height: 80px" align="center">
                            <asp:DataGrid ID="dg_Source" runat="server" CssClass="GridCSS" ShowFooter="True"
                                AutoGenerateColumns="False" TabIndex="38" Width="850px">
                                <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                <Columns>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit"
                                                ToolTip="Edit" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                CommandName="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="SourceName">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_SourceName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SourceName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="BusinessType">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_BusinessType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BusinessType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <%--<asp:TemplateColumn HeaderText="FeesType">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_FeesType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FeesType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>       --%>
                                    <asp:TemplateColumn HeaderText="CustomerId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CustomerId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="SourceId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_SourceId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SourceId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="BusinessTypeId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_BusinessTypeId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BusinessTypeId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
                <asp:HiddenField ID="Hid_BusinessTypeId" runat="server" />
                <asp:HiddenField ID="Hid_BusinessTypeNames" runat="server" />
                <asp:HiddenField ID="Hid_UserIds" runat="server" />
                <asp:HiddenField ID="Hid_NameOfUsers" runat="server" />
                <asp:HiddenField ID="Hid_PhoneNo2" runat="server" />
                <asp:HiddenField ID="Hid_FaxNo1" runat="server" />
                <asp:HiddenField ID="Hid_FaxNo2" runat="server" />
                <asp:HiddenField ID="Hid_RetValues" runat="server" />
                <asp:HiddenField ID="Hid_CustomerId" runat="server" />
                <asp:HiddenField ID="Hid_SectionType" runat="server" />
                <asp:HiddenField ID="Hid_BusTypeId" runat="server" />
                <asp:HiddenField ID="Hid_SourceId" runat="server" />
                <asp:HiddenField ID="Hid_AddBusinessTypeId" runat="server" />
                <asp:HiddenField ID="Hid_AddBusinessTypeNames" runat="server" />
                <asp:HiddenField ID="Hid_AddCustomerId" runat="server" />
                <asp:HiddenField ID="Hid_SourceCustomerId" runat="server" />
                <asp:HiddenField ID="Hid_ResearchDocId" runat="server" />
                <asp:HiddenField ID="Hid_ResearchDocName" runat="server" />
                <asp:HiddenField ID="Hid_Interaction" runat="server" />
                <asp:HiddenField ID="Hid_Branch" runat="server" /> 
                <asp:HiddenField ID="Hid_ContactDetailId" runat="server" />              
            </table>
        </td>
    </tr>
</table>
