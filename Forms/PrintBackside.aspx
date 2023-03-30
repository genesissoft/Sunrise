<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="PrintBackside.aspx.vb" Inherits="Forms_PrintBackside" title="Untitled Page" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                   BackColor="white"  />
    <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="Reports\BacksideContractNote.rpt">
        </Report>
    </CR:CrystalReportSource>
   
</asp:Content>

