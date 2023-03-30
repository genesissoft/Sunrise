<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="true"
    CodeFile="CRMEntry.aspx.vb" Inherits="Forms_CRMEntry"
    Title="CRM Entry" EnableEventValidation="false" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" href="../Include/Style_IPO.css" rel="stylesheet" />

    <script type="text/javascript" src="../Include/jquery-1.9.1.js" language="javascript"></script>

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.core.js"></script>

    <script language="javascript" type="text/javascript" src="../Include/ui/jquery.ui.datepicker.js"></script>

    <script type="text/javascript" src="../Include/showModalDialog.js"></script>

    <script type="text/javascript" src="../Include/CRMEntry.js"></script>

    <%--<script type="text/javascript" src="../Include/IssuerInteraction.js"></script>--%>

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

        function callkey() {
            e = window.event
            if (e.keyCode == 37 || e.keyCode == 35 || e.keyCode == 36 || e.keyCode == 39) {
                return true;
            }
            return false;
        }

        function EmailValidate() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_Email").value != "") {
                var x = document.getElementById("ctl00_ContentPlaceHolder1_txt_Email").value;
                var atpos = x.indexOf("@");
                var dotpos = x.lastIndexOf(".");
                if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
                    alert("Not a valid E-mail address");
                    document.getElementById("ctl00_ContentPlaceHolder1_txt_Email").focus();
                    return false;
                }
            }
        }

        function RemoveListBox(RList) {
            var RightDDL = document.getElementById("ctl00_ContentPlaceHolder1_" + RList);
            for (k = RightDDL.options.length - 1; k >= 0; k--) {
                if (RightDDL.options[k].selected == true) {
                    RightDDL.remove(k);
                }
            }
        }

        function FillListBox(LList, RList) {

            var LeftDDL = document.getElementById("ctl00_ContentPlaceHolder1_" + LList);
            var RightDDL = document.getElementById("ctl00_ContentPlaceHolder1_" + RList);

            for (k = 0; k <= LeftDDL.options.length - 1; k++) {
                if (LeftDDL.options[k].selected == true) {
                    var cnt;
                    var selectedText = LeftDDL.options[k].text;
                    var selectedVal = LeftDDL.options[k].value;

                    var bFound = false;

                    if (RightDDL.options.length > 0) {

                        for (i = 0; i <= RightDDL.options.length - 1; i++) {
                            if (selectedText.toUpperCase() == RightDDL.options[i].text.toUpperCase()) {
                                bFound = true;

                            }
                        }
                    }

                    if (bFound == false) {
                        RightDDL.options.add(new Option(selectedText, selectedVal));
                    }
                }
            }

            LeftDDL.selectedIndex = -1;
        }

        function RedirectPage(InteractionId) {
            window.location = "CRMInteractionDetails.aspx?InteractionId=" + InteractionId;
            return false;
        }

        var t1 = null;
        var l1 = "Don't enter comma in these fields";
        var l2 = "File size should be less than 10 MB & of type .txt,.doc,.xls,.pdf";

        function ConfirmFile() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_file").value == "") {
                return false;
            }
            else {
                return confirm('Are you sure you want to delete file?');
            }
        }
        function DeleteFile() {
            var fu = document.getElementById("<%= fileUpld.ClientID %>");
            if (fu != null) {
                try {
                    if (navigator.appName = 'NetScape') {
                        $("#<%= fileUpld.ClientID %>").val("");
                        return false;
                    }
                    else {
                        fu.setAttribute("type", "input");
                        fu.setAttribute("type", "file");
                    }
                }
                catch (ex) {
                    fu.outerHTML = fu.outerHTML;
                }
            }
        }
        function CancelALL() {
            var Pagecnt = document.getElementById("ctl00_ContentPlaceHolder1_Hid_PageCnt").value;
            window.location = "CRMInteractionDetails.aspx?Pagecnt=" + Pagecnt;
            return false;
        }
        function CRMEntry(Val) {
            //            if (Val == "IS") {
            //                document.getElementById("ctl00_ContentPlaceHolder1_row_Issuer").style.display = "";
            //                document.getElementById("ctl00_ContentPlaceHolder1_lbl_Interaction").innerHTML = "ISSUER";

            //            }
            //            else {
            document.getElementById("ctl00_ContentPlaceHolder1_row_Customer").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_lbl_Interaction").innerHTML = "CUSTOMER";

            //}
            //document.getElementById("ctl00_ContentPlaceHolder1_row_Inv").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_Hid_SelectionId").value = Val;
            //document.getElementById("ctl00_ContentPlaceHolder1_Btn_AddnewContact").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_tr_InvContact").style.display = "";
            //document.getElementById("ctl00_ContentPlaceHolder1_btn_AddInvContact").style.display = "";

        }                
    </script>

    <div id="a_Tooltip" style="display: none; background-color: #F7EED9; width: 150px;
        height: 49px; border: solid 1px gray; text-align: center;">
    </div>
    <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_Save" />
        </Triggers>
        <ContentTemplate>--%>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" align="center" class="data_table">
        <tr>
            <td class="SectionHeaderCSS">
                Interaction Entry:
                <asp:Label ID="lbl_Interaction" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tr>
                        <td colspan="3" width="100%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="65%">
                            <table align="center" border="0" cellpadding="2" cellspacing="2" class="tablemargin"
                                width="100%">
                                <tr align="left" id="row_Customer" runat="server">
                                    <td class="LabelCSS" style="width: 30%;">
                                        Customer:
                                    </td>
                                    <td style="width: 70%;">
                                        <asp:TextBox ID="txt_CustName" onkeypress="return false;" onkeydown="return callkey();"
                                            ToolTip="You can't edit in this" runat="server" CssClass="text_box1"></asp:TextBox>
                                        <%--<asp:Button ID="btn_Custname" runat="server" CssClass="SearchButtonCSS1" Text="..." />--%>
                                        <input id="btn_Customer1" type="button" class="frmButton" tabindex="26" value="..."
                                            onclick="return Calldata('Customer');" />
                                        <i style="color: Red; vertical-align: super">*</i>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">
                                        Meeting Date:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_EntryDate" runat="server" CssClass="text_box1 jsdate" TabIndex="0"></asp:TextBox>
                                        <i style="color: Red; vertical-align: super">*</i>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS">
                                        City:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:DropDownList ID="cbo_City" runat="server" CssClass="combo1">
                                        </asp:DropDownList>
                                        <i style="color: Red; vertical-align: super">*</i>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" valign="top">
                                        Purpose:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:DropDownList ID="cbo_Purpose" runat="server" CssClass="combo1">
                                        </asp:DropDownList>
                                        <i style="color: Red; vertical-align: super">*</i>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" valign="top">
                                        In/Out Station:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:DropDownList ID="cbo_Station" runat="server" CssClass="combo1">
                                            <asp:ListItem Selected="True" Text="IN STATION" Value="IN STATION"></asp:ListItem>
                                            <asp:ListItem Text="OUT STATION" Value="OUT STATION"></asp:ListItem>
                                        </asp:DropDownList>
                                        <i style="color: Red; vertical-align: super">*</i>
                                    </td>
                                </tr>
                                <tr align="left" id="tr_InvContact" runat="server">
                                    <td class="LabelCSS" valign="top">
                                        Contact Person:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:DropDownList ID="cbo_InvContact" runat="server" CssClass="combo1">
                                            <asp:ListItem Text="SELECT CONTACT PERSON..." Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <i style="color: Red; vertical-align: super">*</i>
                                    </td>
                                </tr>
                                <tr class="hide">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="Btn_AddnewContact" runat="server" CssClass="frmButton" OnClientClick="return AddNewCnct(this);"
                                            Text="Add New Contact" />
                                    </td>
                                </tr>
                                <tr class="hide">
                                    <td class="LabelCSS" nowrap="nowrap" valign="top">
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="top">
                                        <table align="left" border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td width="30%">
                                                    <asp:ListBox ID="lst_InvContact" runat="server" CssClass="list_box"></asp:ListBox>
                                                </td>
                                                <td align="left" width="70%">
                                                    <asp:Button ID="btn_AddInvContact" runat="server" CssClass="frmButton" OnClientClick="return InsertInvContact1();"
                                                        Text="Insert" Width="65px" />
                                                    <br />
                                                    <br />
                                                    <asp:Button ID="btn_RemoveInvCont" runat="server" Width="65px" CssClass="frmButton"
                                                        OnClientClick="return RemoveItemFromList1();" Style="display: block;" Text="Remove" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS" nowrap="nowrap" valign="top">
                                        Mode Of Contact:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cbo_ModeOfCnct" runat="server" CssClass="combo1">
                                        </asp:DropDownList>
                                        <b style="color: Red; vertical-align: super">*</b>
                                    </td>
                                </tr>
                                <tr align="left" style="display :none">
                                    <td class="LabelCSS" nowrap="nowrap" valign="middle">
                                        Vertical Purpose:
                                    </td>
                                    <td style="padding: 0px;">
                                        <table>
                                            <tr>
                                                <td style="padding-left: 0px;">
                                                    <asp:ListBox ID="Lst_VerticalL" CssClass="list_box" runat="server" onDblClick="javascript: FillListBox('Lst_VerticalL','Lst_VerticalR');">
                                                    </asp:ListBox>
                                                </td>
                                                <td>
                                                    <input type="button" id="btn_AddVertical" class="frmButton" onclick='FillListBox("Lst_VerticalL","Lst_VerticalR");'
                                                        value=">>" />
                                                    <br />
                                                    <input type="button" id="btn_RemoveVertical" class="frmButton" onclick='RemoveListBox("Lst_VerticalR");'
                                                        value="<<" />
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="Lst_VerticalR" CssClass="list_box" runat="server" onDblClick="javascript: RemoveListBox('Lst_VerticalR');">
                                                    </asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" nowrap="nowrap" valign="middle">
                                        Accompanied By:
                                    </td>
                                    <td style="padding: 0px;">
                                        <table>
                                            <tr>
                                                <td style="padding-left: 0px;">
                                                    <asp:ListBox ID="Lst_AccompaniedbyL" CssClass="list_box" runat="server" onDblClick="javascript: FillListBox('Lst_AccompaniedbyL','Lst_AccompaniedbyR');">
                                                    </asp:ListBox>
                                                </td>
                                                <td>
                                                    <input type="button" id="btn_AddAccompaniedby" class="frmButton" onclick='FillListBox("Lst_AccompaniedbyL","Lst_AccompaniedbyR");'
                                                        value=">>" />
                                                    <br />
                                                    <input type="button" id="btn_RemoveAccompaniedby" class="frmButton" onclick='RemoveListBox("Lst_AccompaniedbyR");'
                                                        value="<<" />
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="Lst_AccompaniedbyR" CssClass="list_box" runat="server" onDblClick="javascript: RemoveListBox('Lst_AccompaniedbyR');">
                                                    </asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" nowrap="nowrap" valign="middle">
                                        Customer Expectation:
                                    </td>
                                    <td style="padding: 0px;">
                                        <table>
                                            <tr>
                                                <td style="padding-left: 0px;">
                                                    <asp:ListBox ID="Lst_CustExpec" CssClass="list_box" runat="server" onDblClick="javascript: FillListBox('Lst_CustExpec','Lst_CustExpecR');">
                                                    </asp:ListBox>
                                                </td>
                                                <td>
                                                    <input type="button" id="btn_AddList" class="frmButton" onclick='FillListBox("Lst_CustExpec","Lst_CustExpecR");'
                                                        value=">>" />
                                                    <br />
                                                    <input type="button" id="btn_RemoveList" class="frmButton" onclick='RemoveListBox("Lst_CustExpecR");'
                                                        value="<<" />
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="Lst_CustExpecR" CssClass="list_box" runat="server" onDblClick="javascript: RemoveListBox('Lst_CustExpecR');">
                                                    </asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" nowrap="nowrap" valign="top">
                                        Status:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="Cbo_Status" runat="server" CssClass="combo1">
                                            <asp:ListItem Text="Hot" Value="H"></asp:ListItem>
                                            <asp:ListItem Text="Cold" Value="C"></asp:ListItem>
                                            <asp:ListItem Text="Warm" Value="W"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        Upload File:
                                    </td>
                                    <td>
                                        <%--<asp:FileUpload ID="fileUpld" runat="server" />--%>
                                        <input type="file" onmouseover="return Show(this,l2)" style="width: 50%" class="text_box1"
                                            onmouseout="return Hide(this)" runat="server" name="fileUpld" id="fileUpld" />
                                        <input class="frmButton" name="btn_Remove" onclick="return  DeleteFile()" type="button"
                                            value="Remove" />
                                    </td>
                                </tr>
                                <tr align="left" id="tr_File" runat="server" style="display: none;">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_file" runat="server" CssClass="text_box1" ReadOnly="true" TabIndex="8"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="Btn_Delete" runat="server" OnClientClick="return ConfirmFile();"
                                            CssClass="frmButton" Text="Delete" />
                                    </td>
                                </tr>
                                <tr align="left" valign="middle">
                                    <td class="LabelCSS" style="vertical-align:middle !important;">
                                        Topic Discussed:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_TopicDiscussed" runat="server" CssClass="text_box1" Height="50px"
                                            TabIndex="0" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" style="vertical-align:middle !important;">
                                        Parameters:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_Parameters" runat="server" CssClass="text_box1" Height="50px"
                                            TabIndex="0" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" style="vertical-align:middle !important;">
                                        Opportunity:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_Opportunity" runat="server" CssClass="text_box1" Height="50px"
                                            TabIndex="0" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCSS" style="vertical-align:middle !important;">
                                        Summary Of Discussion:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_Remark" runat="server" CssClass="text_box1" Height="50px" TabIndex="0"
                                            TextMode="MultiLine" Width="300px"></asp:TextBox><i style="color: Red; vertical-align: super">*</i>
                                    </td>
                                </tr>
                                <tr align="left" id="row_AdvComment" runat="server" style="display: none;">
                                    <td class="LabelCSS">
                                        Advisory Comment:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_AdvComment" runat="server" CssClass="text_box1" Height="50px"
                                            TabIndex="0" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left" id="row_AdvStatus" runat="server" style="display: none;">
                                    <td class="LabelCSS">
                                        Advisory Status:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_Advstatus" runat="server" CssClass="text_box1" Height="50px"
                                            TabIndex="0" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="3%">
                        </td>
                        <td align="left" valign="top" width="32%">
                            <div id="div_CnctPerson" style="display: none;">
                                <table>
                                    <tr style="height: 80px">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap">
                                            Contact Person:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ContactPerson" onmouseover="return Show(this,l1)" onmouseout="return Hide(this)"
                                                CssClass="text_box1" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Branch:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ContactBranch" runat="server" CssClass="text_box1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Designation:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ContactDesign" runat="server" CssClass="text_box1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tel No.:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_TelNo" runat="server" CssClass="text_box1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Mobile No.:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_MobileNo" runat="server" CssClass="text_box1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Email:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_Email" runat="server" CssClass="text_box1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="LabelCSS">
                                            Interaction:
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cbo_Interaction" runat="server" CssClass="combo1" TabIndex="0"
                                                Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="Btn_SaveNewCnct" runat="server" CssClass="frmButton" OnClientClick="return SavePermanentContact();"
                                                Text="Save" />
                                            <asp:Button ID="Btn_CancelCnct" runat="server" CssClass="frmButton" OnClientClick="return CancelNewCnct();"
                                                Text="Cancel" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <font color="green">
                                <asp:Label ID="lbl_info" runat="server"></asp:Label>
                            </font>
                        </td>
                    </tr>
                    <tr>
                        <td class="SeperatorRowCSS" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="btn_Save" runat="server" CssClass="frmButton" Text="Save" />
                            &nbsp;
                            <asp:Button ID="btn_Cancel" runat="server" CssClass="frmButton" OnClientClick="return CancelALL();"
                                Text="Cancel" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="SeperatorRowCSS">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="Hid_Id" runat="server" />
    <asp:HiddenField ID="Hid_InteractionId" runat="server" />
    <asp:HiddenField ID="Hid_SelectionId" runat="server" />
    <asp:HiddenField ID="Hid_LstbxTC" runat="server" />
    <asp:HiddenField ID="Hid_LstbxPC" runat="server" />
    <asp:HiddenField ID="Hid_PageCnt" runat="server" />
    <asp:HiddenField ID="Hid_LstCustExpecR" runat="server" />
    <asp:HiddenField ID="Hid_LstAccompaniedbyR" runat="server" />
    <asp:HiddenField ID="Hid_LstVerticalR" runat="server" />
    <asp:HiddenField ID="Hid_CustBusinessType" runat="server" />
    <asp:HiddenField ID="Hid_InvContact" runat="server" />
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
