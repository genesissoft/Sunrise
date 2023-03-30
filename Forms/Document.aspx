<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" CodeFile="~/forms/document.aspx.vb"
    AutoEventWireup="false" EnableViewStateMac="false" Inherits="Forms_Document"
    Title="Document Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript">
        function Validation() {
            if (Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_Document").value) == "") {
                AlertMessage("Validation", "Please enter document name.", 175, 450);
                return false;
            }
            return true;
        }

        function SetFocus() { document.getElementById("ctl00_ContentPlaceHolder1_txt_Document").focus(); return false; }</script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
        <ContentTemplate>
            <div>
                <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                    <tr align="left">
                        <td class="SectionHeaderCSS">Document Master</td>
                    </tr>
                    <tr class="line_separator">
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr align="center" valign="top">
                        <td>
                            <table id="Table3" width="45%" align="center" cellspacing="0" cellpadding="0" border="0">
                                <tr align="left">
                                    <td>Document:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Document" Width="250px" runat="server" CssClass="TextBoxCSS"
                                            TabIndex="1"></asp:TextBox><i style="color: Red; vertical-align: super;">*</i>
                                    </td>
                                </tr>
                                <tr align="left" >
                                    <td runat ="server" visible ="false" > Type:
                                    </td>
                                    <td runat ="server" visible ="false">
                                        <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DocType" runat="server" CellPadding="0"
                                            CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="True">
                                            <asp:ListItem Selected="true" Value="E"> Empalement </asp:ListItem>
                                            <asp:ListItem Value="K"> KYC </asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>Document For:
                                    </td>
                                    <td style="padding-left: 0px;">
                                        <asp:RadioButtonList RepeatLayout="Flow" ID="rdo_DocFor" runat="server" CellPadding="0"
                                            CellSpacing="0" RepeatDirection="Horizontal" CssClass="LabelCSS" AutoPostBack="True">
                                            <asp:ListItem Selected="true" Value="S"> Security </asp:ListItem>
                                            <asp:ListItem Value="C"> Customer </asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr class="line_separator">
                                    <td colspan="2"></td>
                                </tr>
                                <tr align="left">
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" TabIndex="2" />
                                        <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" Visible="false"
                                            TabIndex="3" />
                                        <asp:Button ID="btn_Back" runat="server" CssClass="ButtonCSS" Text="Back" TabIndex="4" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
