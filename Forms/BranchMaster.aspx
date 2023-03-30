<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="BranchMaster.aspx.vb" Inherits="Forms_BranchMaster" Title="Branch Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script type="text/javascript">

        function ConvertUCase(txtBox) {
            txtBox.value = txtBox.value.toUpperCase()
        }


        function Validation() {
            if ((document.getElementById("ctl00_ContentPlaceHolder1_txt_BranchName").value) == "") {
                AlertMessage('Validation', 'Please Enter Branch Name.', 175, 450);
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

    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>--%>
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Branch Master
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="55%">
                    <tr align="left">
                        <td>Branch Name:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_BranchName" runat="server" CssClass="TextBoxCSS" Width="185px"></asp:TextBox>
                            <em><span style="color: Red; vertical-align: super;">*</span></em></td>
                    </tr>
                    <tr align="left">
                        <td>Branch Type:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButtonList ID="Rdo_BranchType" runat="server" CssClass="LabelCSS" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" Width="180px">
                                <asp:ListItem Selected="True" Value="B">Branch</asp:ListItem>
                                <asp:ListItem Value="H">Head Office</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>Address:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Address1" runat="server" CssClass="TextBoxCSS" Width="185px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txt_Address2" runat="server" CssClass="TextBoxCSS" Width="185px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>Phone No:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_BranchPhNo" runat="server" CssClass="TextBoxCSS" Width="185px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>Fax No:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_FaxNo" runat="server" CssClass="TextBoxCSS" Width="185px"></asp:TextBox>
                        </td>
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
                    <tr align="left">
                        <td>Profit:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Profit" runat="server" CssClass="TextBoxCSS" Width="115px"></asp:TextBox><font
                                class="LabelCSS">%</font>
                        </td>
                    </tr>
                    <tr>
                        <td class="ForControls" align="center" valign="middle" colspan="2">
                            <asp:Label ID="LabelError" runat="server" CssClass="ForErrorMessages" Visible="False"></asp:Label></td>
                    </tr>
                    <tr class="line_separator">
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr align="left">
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                            <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                            <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                        </td>
                    </tr>
                </table>
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
    <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
</asp:Content>
