<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="CompanyMaster.aspx.vb" Inherits="Forms_CompanyMaster" Title="Company Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">
        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_CompName").value) == "") {
                AlertMessage('Validation', 'Please Enter Company Name.', 175, 450);
                return false;
            }
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_PANNo").value) == "") {
                AlertMessage('Validation', 'Please Enter PAN No.', 175, 450);
                return false;
            }
            var strHeaderfiletype = (document.getElementById("ctl00_ContentPlaceHolder1_file_Header").value)
            var lastindexHeaderFileName = (strHeaderfiletype.lastIndexOf('\\'))
            var HeaderFileName = strHeaderfiletype.substr(lastindexHeaderFileName + 1, 20)
            var strFooterfiletype = (document.getElementById("ctl00_ContentPlaceHolder1_File_Footer").value)
            var lastindexHeader = (strHeaderfiletype.lastIndexOf('.'))
            var lastindexFooter = (strFooterfiletype.lastIndexOf('.'))
            var Header = strHeaderfiletype.substr(lastindexHeader, 4)
            var Footer = strFooterfiletype.substr(lastindexFooter, 4)
            if (Header != "") {
                if ((Header == '.tif' || Header == '.jpg' || Header == '.bmp' || Header == '.gif' || Header == '.png' || Header == '.wmf' || Header == '.ico') == false) {
                    AlertMessage('Validation', 'Only gif, bmp or jpg format files supported.', 175, 450);
                    return false;
                }
            }
            if (Footer != "") {
                if ((Footer == '.tif' || Footer == '.jpg' || Footer == '.bmp' || Footer == '.gif' || Footer == '.png' || Footer == '.wmf' || Footer == '.ico') == false) {
                    AlertMessage('Validation', 'Only gif, bmp or jpg format files supported.', 175, 450);
                    return false;
                }
            }
        }
        function ImageChange(id) {
            document.getElementById("ctl00_ContentPlaceHolder1_" + id).value = "TRUE"
            //        alert(document.getElementById("ctl00_ContentPlaceHolder1_" + id).value)
        }

    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Company Master</td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>--%>
                <table width="45%" cellspacing="0" cellpadding="0" border="0" align="center">
                    <tr align="left">
                        <td>Company Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_CompName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                MaxLength="50"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                    </tr>
                    <tr align="left">
                        <td>PAN No:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_PANNo" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="50"></asp:TextBox><em><span
                                style="color: Red; vertical-align: super;">*</span></em></td>
                    </tr>
                    <tr align="left">
                        <td>Confirmation Text:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_confirm" runat="server" Width="200px" CssClass="TextBoxCSS"
                                TextMode="MultiLine" Rows="4"></asp:TextBox><em><span style="color: Red; vertical-align: super;"></td>
                    </tr>
                    <tr align="left">
                        <td>Address 1:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Address" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox>
                    </tr>
                    <tr align="left">
                        <td>Address 2:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Address2" runat="server" Width="200px" CssClass="TextBoxCSS"></asp:TextBox></td>
                    </tr>
                    <tr align="left">
                        <td>City:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_City" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="50"></asp:TextBox></td>
                    </tr>
                    <tr align="left">
                        <td>Pin Code:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_PinCode" runat="server" Width="200px" CssClass="TextBoxCSS"
                                MaxLength="50"></asp:TextBox></td>
                    </tr>
                    <tr align="left">
                        <td>Phone No:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_PhoneNo" runat="server" Width="200px" CssClass="TextBoxCSS"
                                MaxLength="50"></asp:TextBox></td>
                    </tr>
                    <tr align="left">
                        <td>Fax No:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_FaxNo" runat="server" Width="200px" CssClass="TextBoxCSS" MaxLength="50"></asp:TextBox></td>
                    </tr>
                    <tr align="left">
                        <td>Service Tax No.:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_ServiceTaxNo" runat="server" Width="200px" CssClass="TextBoxCSS"
                                MaxLength="50"></asp:TextBox></td>
                    </tr>
                    <tr align="left">
                        <td>GST No:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_GSTNo" runat="server" CssClass="TextBoxCSS" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>Tax Deduction Account No.:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_TDAccountNo" runat="server" Width="200px" CssClass="TextBoxCSS"
                                MaxLength="50"></asp:TextBox></td>
                    </tr>
                    <tr align="left">
                        <td>Header Image:
                        </td>
                        <td>
                            <asp:FileUpload ID="file_Header" runat="server" CssClass="TextBoxCSS" Width="200px" />
                            <asp:Button ID="btn_ShowHeader" runat="server" CssClass="SearchButtonCSS" Text="Show" /><font
                                id="row_HeaderMsg" runat="server" class="MessageCSS" color="red">No Image Uploaded.</font>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>Footer Image:
                        </td>
                        <td>
                            <asp:FileUpload ID="File_Footer" runat="server" CssClass="TextBoxCSS" Width="200px" />
                            <asp:Button ID="btn_showFooter" runat="server" CssClass="SearchButtonCSS" Text="Show" /><font
                                id="row_Footermsg" runat="server" class="MessageCSS" color="red">No Image Uploaded.</font>
                        </td>
                    </tr>
                    <tr class="line_separator">
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr align="left" id="buttonid" runat="server">
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS" />
                            <asp:Button ID="btn_Update" Visible="false" runat="server" Text="Update" ToolTip="Update"
                                CssClass="ButtonCSS" />
                            <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS" />
                        </td>
                    </tr>
                    <asp:HiddenField ID="Hid_Show" runat="server" />
                    <asp:HiddenField ID="Hid_FileName" runat="server" />
                    <asp:HiddenField ID="Hid_uploadHeader" runat="server" />
                    <asp:HiddenField ID="Hid_ImageContentType" runat="server" />
                    <asp:HiddenField ID="Hid_UpdateHeaderFlag" runat="server" />
                    <asp:HiddenField ID="Hid_UpdateFooterFlag" runat="server" />
                    <asp:HiddenField ID="Hid_FooterChange" runat="server" />
                    <asp:HiddenField ID="Hid_HeaderFileName" runat="server" />
                    <asp:HiddenField ID="Hid_FooterFileName" runat="server" />
                    <asp:HiddenField ID="Hid_prevHeaderFileName" runat="server" />
                    <asp:HiddenField ID="Hid_prevFooterFileName" runat="server" />
                </table>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </td>
    </table>

</asp:Content>
