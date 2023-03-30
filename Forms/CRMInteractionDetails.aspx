<%@ Page Language="C#" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="CRMInteractionDetails.aspx.cs" Inherits="Forms_CRMInteractionDetails"
    Title="CRM Interaction Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" href="../Include/Style_IPO.css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/Script/jquery-1.8.0.min.js" language="javascript"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.core.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.datepicker.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
             $(".jsdate").datepicker({
                showOn: "button",
                buttonImage: "../Images/calendar.gif",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                buttonText: 'select date as (dd/mm/yyyy)'
            });
            $(".jsdate").prop('maxLength', 10);
            $(".numeric").keypress(function () {
                return OnlyNumericKey(event);
            });
        });
    </script>

    <script type="text/javascript" language="javascript">
        function Search() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_Search").value == "") {
                alert('Please enter search text');
                return false;
            }
        }

        function OpenWindow(FileName) {
            pageUrl = "ShowAttachment.aspx?attachment=" + FileName;
            var ret = window.open(pageUrl, target = "_blank", "left=50,top=50,height=400,width=800,toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes,resizable=yes, copyhistory=yes");
            if ((typeof (ret) == "undefined") || (ret == "")) {
                return false;
            }
            else {
                return false;
            }

        }
        function Delete(InteractionId, Access) {
            if (Access == "N") {
                alert("you don't have permission to delete this record");
                return false;

            }
            
            if (window.confirm("Are you sure want to delete this record?")) {
                document.getElementById("ctl00_ContentPlaceHolder1_Hid_Id").value = InteractionId;
                return true
            }
            else {
                return false
            }
        }
        
        function Search() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_Search").value == "") {
                alert('Please enter search text');
                return false;
            }
        }
        
        function Update(InteractionId, Access) {
            if (Access == "N") {
                alert("you don't have permission to edit in this record");
                return false;
            }
            else {
                var Pagecnt = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PageCnt").value;
                pageUrl = "CRMEntry.aspx?" + "Id=" + InteractionId + "&Edit=1" + "&Pagecnt=" + Pagecnt;
                window.location = pageUrl;
                return false;
            }
        }
    </script>

    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>--%>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="data_table"
        align="center">
        <tr>
            <td class="SectionHeaderCSS" align="left">
                Client Interaction Monthly Details
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <table border="0" cellpadding="0" cellspacing="0" class="data_table">
                    <tr align="left">
                        <td valign="middle" align="left">
                            From Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_Fromdate" runat="server" TabIndex="1" Style="margin: 2px" CssClass="text_box jsdate" ></asp:TextBox>
                               
                        </td>
                        <td valign="middle" align="left">
                            &nbsp; To Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_ToDate" runat="server" TabIndex="1" Style="margin: 2px" CssClass="text_box jsdate" ></asp:TextBox>
                              
                        </td>
                        <td>
                            <asp:Button ID="btn_Go" runat="server" Text=" Go " CssClass="frmButton" OnClick="btn_Go_Click" />
                        </td>
                    </tr>
                    <tr align="left">
                        <td colspan="5">
                            Select Field:
                            <asp:DropDownList ID="cbo_ClientType" runat="server" CssClass="combo">
                            </asp:DropDownList>
                            <asp:TextBox ID="txt_Search" runat="server" CssClass="text_box"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="5">
                            <font color="green">
                                <asp:Label ID="lbl_msg" runat="server"></asp:Label></font>
                        </td>
                    </tr>
                    <tr class="SeperatorRowCSS">
                        <td colspan="5">
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                        </td>
                        <td colspan="4">
                            <asp:Button ID="btn_Add" runat="server" CssClass="frmButton" Text="Add" OnClick="btn_Add_Click" />&nbsp;
                            <asp:Button ID="btn_Search" runat="server" Text="Search" OnClientClick="return Search();"
                                CssClass="frmButton" OnClick="btn_Search_Click" />&nbsp;
                            <asp:Button ID="btn_ShowAll" runat="server" CssClass="frmButton" Text="Show All"
                                OnClick="btn_ShowAll_Click" />&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="SeperatorRowCSS">
            <td>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center">
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="center">
                            <div id="Div1" align="center" style="width: 100%">
                                <asp:DataGrid ID="dg_CRMDetails" AllowPaging="true" runat="server" Width="90%" ShowFooter="True"
                                    AutoGenerateColumns="false" PageSize="100" OnItemDataBound="dg_CRMDetails_ItemDataBound"
                                    OnItemCommand="dg_CRMDetails_ItemCommand" CssClass="table_border_right_bottom"
                                    OnPageIndexChanged="dg_CRMDetails_PageIndexChanged">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" CssClass="table_heading" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Position="TopAndBottom" Font-Size="1.3em"
                                        Mode="NumericPages" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="img_Edit" runat="server" align="center" CommandName="Edit" ImageUrl="~/Images/edit3.PNG"
                                                    ToolTip="Edit" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="img_Delete" runat="server" CommandName="Delete" align="center"
                                                    ImageUrl="~/Images/delete.gif" ToolTip="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="SN" HeaderText="SN" HeaderStyle-Width="3%"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="MeetingDate" HeaderText="Meeting Date" HeaderStyle-Width="10%">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CustomerName" HeaderText="Client Name" HeaderStyle-Width="20%">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ContactPerson" HeaderText="Contact Person" HeaderStyle-Width="14%">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Purpose" HeaderText="Purpose" HeaderStyle-Width="14%">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UserName" HeaderText="User Name" HeaderStyle-Width="13%">
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Attachment" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hyperlink_Viewprofile" NavigateUrl='<%# "ShowAttachment.aspx?attachment=" + DataBinder.Eval(Container, "DataItem.FileName") + "&UserName=" + DataBinder.Eval(Container, "DataItem.LoginName") %>'
                                                    Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FileName") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="LastUpdate" HeaderText="Last Update" HeaderStyle-Width="10%">
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ManageById" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.ManageById") %>'></asp:Label>
                                                <asp:Label ID="lbl_ClientId" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.ClientId") %>'></asp:Label>
                                                <asp:Label ID="lbl_InteractionId" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.InteractionId") %>'></asp:Label>
                                                <asp:Label ID="lbl_ClientDiff" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.ClientDiff") %>'></asp:Label>
                                                <asp:Label ID="lbl_Temp" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.AddTemporary") %>'></asp:Label>
                                                <asp:Label ID="lbl_Access" runat="server" align="Left" Text='<%# DataBinder.Eval(Container, "DataItem.UserName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="Hid_SelectionId" runat="server" />
                            <asp:HiddenField ID="Hid_Id" runat="server" />
                            <asp:HiddenField ID="Hid_PageCnt" runat="server" />
                            <asp:HiddenField ID="Hid_ManageBy" runat="server" />
                        </td>
                    </tr>
                </table>
                <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
    </table>
</asp:Content>
