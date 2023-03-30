<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true" CodeFile="DB_Backup.aspx.cs" Inherits="Forms_DB_Backup" Title="Database Backup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <link type="text/css" href="../Include/Style_IPO.css" rel="stylesheet" />
 <div>  
        <table cellpadding="10" cellspacing="10" width ="45%">
          
            <tr>  
                <td style="height: 35px;  font-weight: bold; font-size: 16pt;  
                    font-family: Times New Roman; color: Black" align="center">  
                    Backup eInstadeal DataBase  
                </td>  
            </tr>  
            <tr>  
                <td align="center">  
                    <asp:Label ID="lblError" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>  
                </td>  
            </tr>  
            <tr>  
                <td align="center">  
                    <asp:Button ID="btnBackup" runat="server" Text="Backup DataBase" OnClick="btnBackup_Click" CssClass="ButtonCSS" Width ="100px"/>  
                </td>  
   
            </tr>   
        </table>  
    </div>  
</asp:Content>

