<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="SubCategoryMaster.aspx.vb" Inherits="Forms_SubCategoryMaster" Title="Sub Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">
    
        function CheckType()
        {
            if(document.getElementById("ctl00_ContentPlaceHolder1_rbl_UserTypeSection_1").checked == true)
            {
                document.getElementById("row_Type").style.display ="none"
                document.getElementById("row_Category").style.display =""
            }
            else
            {
                document.getElementById("row_Type").style.display =""
                document.getElementById("row_Category").style.display ="none"
           }
        }
    
        function Validation()
        {
            if(document.getElementById("ctl00_ContentPlaceHolder1_txt_Subcategory").value =="")
            {
                AlertMessage('Validation','Please enter Sub Category Name',175,450);
                return false;
            }
            if(document.getElementById("ctl00_ContentPlaceHolder1_rbl_UserTypeSection_0").checked == true)
            {
                if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_CustomerType").value) =="")
                {
                    AlertMessage('Validation', 'please select Customer Type',175,450);
                    return false;
                }
            }
            else
            {
                if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_cbo_Category").value) =="")
                {
                    AlertMessage('Validation', 'please select Category',175,450);
                    return false;
                }
            }      
            
            return true;        
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="SectionHeaderCSS" align="center">
                Sub Category</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="50%">
                    <tr align="left">
                        <td>
                            Sub Category Name:</td>
                        <td>
                            <asp:TextBox ID="txt_Subcategory" runat="server" CssClass="TextBoxCSS" MaxLength="30"
                                TabIndex="1" Width="222px"></asp:TextBox><i style="color: red; vertical-align: super;">*</i>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            Type Section:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="rbl_UserTypeSection" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                RepeatLayout="Table" CssClass="LabelCSS" Height="4px" TabIndex="2">
                                <asp:ListItem Selected="True" Value="T">Type</asp:ListItem>
                                <asp:ListItem Value="C">Category</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                    <tr align="left" id="row_Type">
                        <td>
                            Customer Type:
                        </td>
                        <td>
                            <asp:DropDownList ID="cbo_CustomerType" runat="server" CssClass="ComboBoxCSS" Height="19px"
                                Width="197px">
                            </asp:DropDownList><i style="color: red; vertical-align: super;">*</i>
                        </td>
                    </tr>
                    <tr align="left" id="row_Category">
                        <td>
                            Customer Category:
                        </td>
                        <td>
                            <asp:DropDownList ID="cbo_Category" runat="server" CssClass="ComboBoxCSS" Height="19px"
                                Width="197px">
                            </asp:DropDownList><i style="color: red; vertical-align: super;">*</i>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="SeperatorRowCSS">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
            </td>
        </tr>
    </table>
</asp:Content>
