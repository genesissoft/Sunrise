<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="ConfigureAuditTrails.aspx.vb" Inherits="Forms_ConfigureAuditTrails"
    Title="Configure Audit Trials" %>

<%@ Register Assembly="AuditTrailGenerator" Namespace="AuditTrailGenerator.UserSecurity"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div align="center">
        <cc1:AuditTrailGenerator ID="AuditTrailGenerator1" runat="server" GridCSSClass="GridCSS"
            RowCSSClass="GridRowCSS" HeaderCSSClass="GridHeaderCSS" SettingTableName="AuditTrailSettings"
            ButtonCSSClass="ButtonCSS" ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"></cc1:AuditTrailGenerator>
    </div>
</asp:Content>
