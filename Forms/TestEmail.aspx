<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="TestEmail.aspx.cs" Inherits="Forms_TestEmail" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td align="left">
                User Name : 
            </td>
            <td>
                <asp:TextBox ID="txtUserName" Width="200px"  runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left">
                Pass word : 
            </td>
            <td>
                <asp:TextBox ID="txtPassword" Width="200px" TextMode="Password"  runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left">
                To : 
            </td>
            <td>
                <asp:TextBox ID="txtto" Width="200px" runat="server"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td align="left">
                CC : 
            </td>
            <td>
                <asp:TextBox ID="txtCC" Width="200px" runat="server"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td align="left">
                BCC : 
            </td>
            <td>
                <asp:TextBox ID="txtBCC" Width="200px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left">
                SMTP Server : 
            </td>
            <td>
                <asp:TextBox ID="txtSmtpServer" Width="200px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left">
                Port : 
            </td>
            <td>
                <asp:TextBox ID="txtPort" Width="200px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left">
                SSL : 
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSSL" Width="200px" runat="server">
                    <asp:ListItem Selected="True" Text="True" Value="True">True</asp:ListItem>
                    <asp:ListItem Text="False" Value="False">False</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
             &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btnSend" Text="Send Email " runat="server" 
                    onclick="btnSend_Click" />
            </td>
        </tr>
         <tr>
            <td align="center" colspan="2">
             &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Label ID="lblmsg" runat="server" ForeColor="Red"   Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
