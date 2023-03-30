<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShowClientPurImage.aspx.vb"
    Inherits="Forms_ShowClientPurImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self">
    </base>
    <title>Image Preview</title>
    <link href="../Include/General.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Intervention.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Parkstone.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="../Include/SearchList.js" type="text/javascript"></script>

    <script language="javascript" type="text/jscript" src="../Include/calendar.js"></script>

    <script type="text/javascript">
        function Delete_entry()
        {
            if(window.confirm("Are you sure you want to Delete this record?"))
            {
                return true
            }
            else
            {
                return false
            }
        }  
        
         function SendImage(imgId)
        {
            document.getElementById("Hid_ID").value = imgId
            
            window.returnValue = imgId
            window.close()  
            return false
        } 
          
        
         
         
         
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
            <tr>
                <td colspan="4">
                    <div>
                        <table width="100%" cellpadding="0" cellspacing="0" align="center" border="0">
                            <tr>
                                <td id="td1" colspan="4" class="ForHeaders" valign="top" align="center" runat="server">
                                    Preview Image</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="LabelCSS" align="right">
                                    Name of signatory :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_signatory" runat="server" CssClass="TextBoxCSS" Width="90px"></asp:TextBox><font
                                        class="LabelCSS"> </font>
                                </td>
                            </tr>
                            <tr>
                                <td class="LabelCSS" align="right">
                                    Signature:
                                </td>
                                <td align="left">
                                    <asp:FileUpload ID="File_Footer" runat="server" CssClass="TextBoxCSS" Width="230px" />
                                    <asp:Button ID="btn_ShowHeader" runat="server" CssClass="SearchButtonCSS" Text="ADD" /><font
                                        id="row_HeaderMsg" runat="server" class="MessageCSS" color="red">No Image Uploaded.</font>
                                          
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div id="divdg" style="margin-top: 0px; overflow: auto; padding-top: 0px; position: relative"
                                        align="center">
                                        <asp:GridView ID="imgGrid" runat="server" AutoGenerateColumns="False" PageSize="1"
                                            AllowPaging="true" ShowHeader="false">
                                            <RowStyle CssClass="ItemStyle" HorizontalAlign="Left" BorderStyle="Solid" BorderWidth="1px"
                                                BorderColor="Black" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                            <HeaderStyle CssClass="HeaderStyle" BorderColor="#315F92"></HeaderStyle>
                                            <FooterStyle BorderColor="Transparent" ForeColor="Black"></FooterStyle>
                                            <PagerSettings Mode="Numeric" Position="Top" />
                                            <Columns>
                                                <asp:ImageField DataImageUrlField="imgFile" HeaderText="Picture">
                                                </asp:ImageField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btn_Delete" runat="server" Text="Delete" CssClass="ButtonCSS" TabIndex="37" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="Hid_Client" runat="server" />
        <asp:HiddenField ID="Hid_ID" runat="server" />
        <asp:HiddenField ID="Hid_Submit" runat="server" />
        <asp:HiddenField ID="Hid_Show" runat="server" />
        <asp:HiddenField ID="Hid_FooterFileName" runat="server" />
        <asp:HiddenField ID="Hid_ImageUploded" runat="server" />
        <asp:HiddenField ID="Hid_CustImgId" runat="server" />
        
    </form>
</body>
</html>
