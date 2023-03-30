<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SearchCRMEntry.aspx.vb" Inherits="Forms_SearchCRMEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Search</title>
    
    <link type="text/css" href="../Include/Style_New.css"  rel="stylesheet" />  
    <link type="text/css" href="../Include/StanChart.css"  rel="stylesheet" />    
    <script type="text/javascript" src="../Include/Common.js"></script>
    
    
    <script type="text/javascript">
        function SelectRow(img,id)
        {
            
          
            document.getElementById("Hid_SelectedId").value = id;
            UnselectRows();
            img.src = "../Images/images.jpg";
            var row = img.parentElement.parentElement;
            row.style.backgroundColor = "#F7EED9";
            img.parentNode.parentNode.style.color='black';  
            return false;
        }
        
        function UnselectRows()
        {
            
            var grd = document.getElementById("dg_Search");
         
            for(i=1; i<=grd.rows.length-3; i++)
            {
               
                var row = grd.children[0].children[i];
                var img = row.children[0].children[0];
                img.src = "../Images/images3.jpg";
                row.style.backgroundColor = "white";
            }
        }
        
        function Submit()
        {
            //debugger;
            
            var grd = document.getElementById("dg_Search");
            var retString = "";
            var colCount = document.getElementById("Hid_ColCount").value;

                
            for(i=1; i<=grd.rows.length-2; i++)
            {
                var row = grd.children[0].children[i];                  
                var bgColor = row.style.color.toUpperCase();                  
                if(bgColor == 'BLACK')
                {   
                    for(j=0; j<colCount; j++)
                    {   
                        //retString = retString + row.children[1].innerText + "!";
                        retString = retString + row.children[1].children[0].innerHTML + "!";			             
                        retString = retString + row.children[2].children[0].innerHTML + "!";			             
                    }
                    break;
                }
            }  
            
            if(retString == "")
            {
                alert("Please select atleast one option");
                return false;
            }

            retString = retString + document.getElementById("Hid_SelectedId").value;
            
            document.getElementById("Hid_modalret").value = retString;   
            window.returnValue = retString;
            window.close();         
            return false;
        }
        
        function Close()
        {
            window.close();
            return false;
        }
        
        function Validation()
        {
            if(document.getElementById("txt_Name").value == "") 
            {
                alert("Please enter the search criteria text");
                return false;
            }
            return true;
        }
    </script>

    <script language="javascript" type="text/javascript">
           function SetFocus()
            {
                document.getElementById("ctl00_ContentPlaceHolder1_txt_Name").focus();
                return false;
            } 
    </script>

</head>
<body>
    <form id="form1" runat="server">
  <%--  <div id="page_effect" style="display: none;" align="center">--%>
        <table id="Table1" width="100%" align="center"  class="data_table" cellspacing="0"
            cellpadding="0" border="0">
            <tr>
                <td class="formHeader" align="center">
                    Search List
                </td>
            </tr>
            <tr>
                <td height="3px">
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="center">
                                &nbsp;&nbsp;&nbsp;
                                <asp:TextBox ID="txt_Name" runat="server" CssClass="text_box" Width="30%"></asp:TextBox>
                                <asp:Button ID="btn_Search" runat="server" CssClass="frmButton" Text="Search" TabIndex="3" />
                                <asp:Button ID="btn_ShowAll" runat="server" CssClass="frmButton" Text="Show All"
                                    TabIndex="4" />
                            </td>
                        </tr>
                        <tr>
                            <td height="3px">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" height="100px" width="70%">
                                <%--<div id="div2" style="margin-top: 0px; overflow: auto; width: 100%; padding-top: 0px;
                                    position: relative; height: 100%">--%>
                                <asp:DataGrid ID="dg_Search" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                    PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center" PageSize="20"
                                    ShowFooter="True" AllowSorting="True"  CssClass="table_border_right_bottom" Width="90%" Height="100%">
                                    <ItemStyle HorizontalAlign="left" />
                                    <HeaderStyle CssClass="table_heading"  />
                                    <PagerStyle Font-Size="Larger" />
                                    <Columns>
                                        <asp:TemplateColumn ItemStyle-Width="05%">
                                            <ItemTemplate>
                                                <asp:Image ID="img_Select" ondblclick="Submit();" ImageUrl="~/Images/images3.jpg"
                                                    runat="server" CssClass="SelectImageCSS" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Name" runat="server" ItemStyle-HorizontalAlign="Left" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Ctype" runat="server" Style="display: none;" Text='<%# DataBinder.Eval(Container, "DataItem.Ctype") %>'></asp:Label>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Id" runat="server" Style="display: none;" Text='<%# DataBinder.Eval(Container, "DataItem.Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                                <asp:HiddenField ID="Hid_SelectedId" runat="server" />
                                <asp:HiddenField ID="Hid_ColCount" runat="server" />
                                <asp:HiddenField ID="Hid_DefaultSort" runat="server" />
                                <asp:HiddenField ID="Hid_PageName" runat="server" />
                                <asp:HiddenField ID="Hid_modalret" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td height="3px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_Submit" runat="server" CssClass="frmButton modalsubmit" Text="Submit" TabIndex="5" />&nbsp;
                                <asp:Button ID="btn_Close" runat="server" CssClass="frmButton modalclose" Text="Close" TabIndex="6" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    <%--</div>--%>
    </form>
</body>
</html>
