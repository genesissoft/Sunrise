<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="CRMExpectationMaster.aspx.vb" Inherits="Forms_CRMExpectationMaster" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script type="text/javascript" src="../Include/Common.js"></script>
 
  <script type="text/javascript">
        function Validation()
        {
            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_ExpectationName").value) == "")
            {
                alert("Please Enter Expectation Name");
                return false;
            }
            

        }
    </script>
    
      <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">
                Expectation Master</td>
        </tr>
        <tr class="line_separator">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table width="50%" cellspacing="0" cellpadding="0" border="0" align="center">
                            <tr align="left">
                                <td>
                                    Expectation Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_ExpectationName" runat="server" Width="200px" CssClass="TextBoxCSS"
                                        MaxLength="100"></asp:TextBox><em><span style="color: Red; vertical-align: super;">*</span></em></td>
                            </tr>
                           
                            <tr class="line_separator">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btn_Save" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Update" Visible="false" runat="server" Text="Update" ToolTip="Update"
                                        CssClass="ButtonCSS" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

