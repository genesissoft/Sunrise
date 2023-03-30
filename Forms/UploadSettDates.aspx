<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="UploadSettDates.aspx.cs" Inherits="Forms_UploadSettDates" Title="Upload settlement Dates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/jquery/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../Include/jquery/jquery-ui.css"></script>
    <script type="text/javascript" src="../Include/jquery/jquery-ui.js"></script>
    <%--   <script type="text/javascript" src="../Include/jquery/JqueryDialogue/jquery-1.7.2.js"></script>

    <script src="../Include/jquery/JqueryDialogue/jquery-ui-1.8.9.js" type="text/javascript"></script>

    <link href="../Include/jquery/JqueryDialogue/jquery-ui-1.8.9.css" rel="stylesheet"
        type="text/css" />--%>

    <script type="text/javascript">
       
        function ValidateUpload() {
            var fileUpload = $("#<%=fuDetails.ClientID %>").val();
            if (!fileUpload) {
                AlertMessage("Validation", 'The file is required.', 175, 450);
                // event.preventDefault();
                return false;
            }
            var extension = fileUpload.substring(fileUpload.lastIndexOf('.') + 1);

            if (extension != 'csv') {
                AlertMessage("Validation", 'CSV files only.', 175, 450);
                //event.preventDefault();
                return false;
            }
            return true;
        }



        function ShowPopup() {
            //     $(function () {

            $("#dialog").dialog({
                modal: true,
                // autoOpen: false,
                title: "Confirmation",
                width: 350,
                height: 160,
                buttons: [
                {
                    id: "Yes",
                    text: "Yes",
                    click: function () {
                        $("[id*=btnUpdate]").attr("rel", "Update");
                        $("[id*=btnUpdate]").click();
                    }
                },
                {
                    id: "No",
                    text: "No",
                    click: function () {
                        $(this).dialog('close');
                    }
                }
                ]
            });
            //    });
        }


    </script>

    <table id="Table1" width="95%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr align="left">
            <td class="SectionHeaderCSS">Upload Settlement Dates
            </td>
        </tr>
        <tr class="line_separator">
            <td>&nbsp;
            </td>
        </tr>
        <tr align="center" valign="top">
            <td colspan="3">
                <table cellpadding="0" cellspacing="0">
                    <tr align="left">
                        <td>Type : </td>
                        <td>
                            <asp:RadioButtonList ID="rbtType" AutoPostBack="true" runat="server" RepeatDirection="Horizontal" Width="200px">
                                <asp:ListItem Selected="True" Text="NORMAL" Value="ICCL">ICCL</asp:ListItem>
                                <asp:ListItem Text="NSCCL" Value="NSCCL">NSCCL</asp:ListItem>
                                <%--<asp:ListItem Text="ICCL" Value="ICCL">ICCL</asp:ListItem>--%>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                </table>

                <table cellpadding="0" cellspacing="0">
                    <tr align="left">
                        <td>
                            <div style="border: 1px solid rgba(251,179,18,0.76);">
                                <asp:FileUpload ID="fuDetails" runat="server" CssClass="upload" />
                            </div>
                        </td>
                        <td>
                            <asp:Button ID="btnUpload" Text="Upload" CssClass="frmButton" OnClick="btnUpload_Click"
                                runat="server" OnClientClick="return ValidateUpload();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnUpdate" runat="server" Text="Update" Style="display: none" OnClick="UpdateRecord"
        UseSubmitBehavior="false" />
    <div id="dialog" align="center" style="display: none;">
        Settlement Dates are already uploaded. Do you want to continue to update?
    </div>
</asp:Content>
