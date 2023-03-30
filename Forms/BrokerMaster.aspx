<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="BrokerMaster.aspx.vb" Inherits="Forms_BrokerMaster" Title="Broker Master" %>

<%@ Register Src="~/UserControls/SearchTextbox.ascx" TagPrefix="uc" TagName="Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link type="text/css" href="../Include/Style_IPO.css" rel="stylesheet" />

    <script language="javascript" src="../Include/Common.js" type="text/javascript"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript" src="../Include/Script/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Include/Script/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.datepicker.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="../Include/Script/jquery-ui.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            BindDate();

        });


        $(document).ready(function () {
            var strDetails = $("#<%= Hid_DocumentDetails.ClientID %>").val();
            $('#tblDocument tr').each(function (i, row) {
                if (i == 0)
                    return;
                $(this).remove();
            });

            if (strDetails != "") {
                strDetails = eval(strDetails);
                $(strDetails).each(function (i, item) {
                    AddDocumentDetails(item.Id, item.DocumentId, item.FileName);
                });
            }

            //Add new row
            $(document).on('click', '#lnkDocument', function () {
                AddDocumentDetails(0, 0, '');
                return false;
            });

            //Delete existing row
            $(document).on('click', 'a.delete', function () {
                var row = $(this).closest('tr');
                $(row).remove();
                return false;
            });

            //debugger;
            var id = ('<%= ViewState("Id") %>');
            if (id == "") {
                $('#btnRating').hide();
            }
            else {
                $('#btnRating').show();
            }

        });

        function ValidatFile(file) {
            var fileExtension = ['xls', 'xlsx', 'doc', 'docx', 'pdf', 'jpg', 'jpeg', 'png', 'gif'];
            var ext = $(file).val().split('.').pop().toLowerCase();

            if ($.inArray(ext, fileExtension) == -1 && ext != '') {
                alert('Sorry, only ' + fileExtension.join(', ') + '. file formats are allowed.');
            }
        }

        var counter = 1;
        function AddDocumentDetails(id, documentId, filename) {
            var strData = "<tr align='left'>";
            strData = strData + "<td style='display:none;'>" + id + "</td>";
            strData = strData + "<td><select class='combo' style='width:99%;' id='cboDocumentType" + counter + "'>" + getOption() + "</select></td>";
            strData = strData + "<td><input type='file' name='fileUpload" + counter + "' onchange='javascript:ValidatFile(this)' /></td>";
            if (id > 0)
                strData = strData + "<td style='text-align:center;'><a class='link_bold' href='javascript:window.location.href =\"showdocument.aspx?Id=" + id + "&ReportType=BrokerDocument&Type=BDD\";'>Download</td>";
            else
                strData = strData + "<td>&nbsp;</td>";
            strData = strData + "<td style='text-align:center;'><a href='' class='delete'><img title='Delete' class='imgdelete' src='../Images/delete.gif' /></a></td>";
            strData = strData + "</tr>";

            $("#tblDocument tbody").append(strData);
            if (documentId > 0)
                $("#tblDocument").find('#cboDocumentType' + counter).val(documentId);
            counter++;
        }

        function getOption() {
            var documents = $("#<%= Hid_DocumentMaster.ClientID %>").val();
            var stroptions = "";
            if (documents != "") {
                //documents = JSON.parse(documents);
                documents = eval(documents);
                $(documents).each(function (i, item) {
                    stroptions = stroptions + "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
            }
            return stroptions;
        }

        function ValidateDocument() {
            var Id = "";
            var DocumentId = "";
            var intRow = $("#tblDocument").find('tr').length;
            var fileExtension = ['xls', 'xlsx', 'doc', 'docx', 'pdf', 'jpg', 'jpeg', 'png', 'gif'];
            var pid, docid, docname, file;
            var ret = true;

            try {
                if (intRow > 1) {
                    $("#tblDocument").find('tr').each(function (i, row) {
                        if (i == 0)
                            return;

                        pid = row.children[0].innerHTML.trim();
                        docid = $($(row).find('select').get(0)).val();
                        docname = $($(row).find('select').get(0)).find("option:selected").text();
                        file = $($(row).find('input:file').get(0)).val();
                        var ext = file.split('.').pop().toLowerCase();
                        var filesize = $(row).find('input:file').get(0).files;

                        if (!docid > 0) {
                            alert('Please select associated document type first.');
                            ret = false;
                            return false;
                        }
                        else if (pid == 0 && file == '') {
                            alert('Please select associated file first for ' + docname + '.');
                            ret = false;
                            return false;
                        }
                        if ($.inArray(ext, fileExtension) == -1 && ext != '') {
                            alert('Sorry, only ' + fileExtension.join(', ') + '. file formats are allowed for ' + docname + '.');
                            ret = false;
                            return false;
                        }
                        else {
                            Id = Id + pid + ",";
                            DocumentId = DocumentId + docid + ",";
                        }

                        //work in ie 9 and above
                        if (file != '' && typeof filesize !== 'undefined')
                            if (filesize[0].size / (1024 * 1024) > 200) {
                                alert('Sorry, Document size must be less than 2MB for ' + docname + '.');
                                ret = false;
                                return false;
                            }
                    });

                    if (!ret)
                        return false;

                    Id = Id.substring(0, Id.length - 1);

                    DocumentId = DocumentId.substring(0, DocumentId.length - 1);
                }
                $('#<%= Hid_BrokerDocumentId.ClientID %>').val(Id);
                $('#<%= Hid_DocumentId.ClientID %>').val(DocumentId);
            }
            catch (err) {
                return false;
            }
            return true;
        }


        function BindDate() {
            $('.jsdate').datepicker({
                showOn: "button",
                buttonImage: "../Images/calendar.gif",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',

                buttonText: 'Select date as (dd/mm/yyyy)'

            });
        }


        function Validation() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_txt_Broker").value == "") {
                AlertMessage('Validation', 'Please Enter Broker name', 175, 450)
                return false;
            }
            return ValidateDocument();
            return true;
        }

        function ConvertUCase(txtBox) {
            txtBox.value = txtBox.value.toUpperCase()
        }
    </script>

    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">Broker Master
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>--%>
                <table id="Table3" align="center" cellspacing="0" cellpadding="0" border="0" width="80%">
                    <tr>
                        <td>
                            <table id="t2" align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                                <tr>
                                    <td class="LabelCSS">Broker Name:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_Broker" runat="server" Width="170px" Height="14px" CssClass="TextBoxCSS"></asp:TextBox><span
                                        style="color: #ff0000"><em>*</em></span>
                                    </td>
                                    <td class="LabelCSS">PinCode:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_BrokerPinCode" runat="server" Width="170px" Height="14px"
                                        CssClass="TextBoxCSS"></asp:TextBox><em><span style="color: #ff0000"></span></em>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">NSE Broker Code:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_NSEBrokerCode" runat="server" Width="170px" Height="14px"
                                        CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                    <td class="LabelCSS" colspan="">Phone:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_BrokerPhone" runat="server" Width="170px" Height="14px"
                                        CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">BSE Broker Code:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_BSEBrokerCode" runat="server" Width="170px" Height="14px"
                                        CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                    <td class="LabelCSS">Fax:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_BrokerFax" runat="server" Width="170px" Height="14px"
                                        CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">Address1:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_BrokerAddress1" runat="server" Width="170px" Height="14px"
                                        CssClass="TextBoxCSS"></asp:TextBox><em><span style="color: #ff0000"></span></em>
                                    </td>
                                    <td class="LabelCSS">PANNumber:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_PANNumber" runat="server" Width="170px" Height="14px"
                                        CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">Address2:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_BrokerAddress2" runat="server" Width="170px" Height="14px"
                                        CssClass="TextBoxCSS"></asp:TextBox><em><span style="color: #ff0000"></span></em>
                                    </td>
                                    <td class="LabelCSS">STRegNo:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_STRegNo" runat="server" Width="170px" Height="14px" CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">City:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_BrokerCity" runat="server" Width="170px" Height="14px"
                                        CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                    <td class="LabelCSS">Email:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_Email" runat="server" Width="170px" Height="14px" CssClass="TextBoxCSS"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="LabelCSS">GST No:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_GSTNo" runat="server" CssClass="TextBoxCSS" Width="170px"
                                        Height="14px"></asp:TextBox>
                                    </td>
                                    <td align="right" class="LabelCSS">Aadhar No:
                                    </td>
                                    <td align="left">&nbsp;<asp:TextBox ID="txt_AadharNo" runat="server" CssClass="TextBoxCSS" Width="170px"
                                        Height="14px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr id="row_Bank">
                        <td class="SubHeaderCSS">Bank Details
                        </td>
                    </tr>

                    <tr>
                        <td align ="center" >
                            <table width ="80%">
                                <tr>
                                    <td align ="center" >
                                        <table id="td_bank" align="center" cellspacing="0" cellpadding="0" border="0" width="100%">
                                            <tr>
                                                <td align="right" class="LabelCSS">Bank Name:
                                                </td>
                                                <td align="left">&nbsp;<asp:TextBox ID="txt_BankName" runat="server" CssClass="TextBoxCSS" Width="170px"
                                                    Height="14px"></asp:TextBox>
                                                </td>
                                                <td align="right" class="LabelCSS">Beneficiary Name:
                                                </td>
                                                <td align="left">&nbsp;<asp:TextBox ID="txt_BeneficiaryName" runat="server" CssClass="TextBoxCSS" Width="170px"
                                                    Height="14px"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                 <td align="right" class="LabelCSS">Account No:
                                                </td>
                                                <td align="left">&nbsp;<asp:TextBox ID="txt_AccountNo" runat="server" CssClass="TextBoxCSS" Width="170px"
                                                    Height="14px"></asp:TextBox>
                                                </td>
                                                <td align="right" class="LabelCSS">IFSC Code:
                                                </td>
                                                <td align="left">&nbsp;<asp:TextBox ID="txt_IFSCCode" runat="server" CssClass="TextBoxCSS" Width="170px"
                                                    Height="14px"></asp:TextBox>
                                                </td>

                                            </tr>
                                              <tr>
                                                 <td align="right" class="LabelCSS">Branch:
                                                </td>
                                                <td align="left">&nbsp;<asp:TextBox ID="txt_Branch" runat="server" CssClass="TextBoxCSS" Width="170px"
                                                    Height="14px"></asp:TextBox>
                                                </td>
                                               

                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                            </table>

                        </td>

                    </tr>
                    <tr id="row_Dealer">
                        <td class="SubHeaderCSS">Dealer Details
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <table width="100%" align="center">
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td class="LabelCSS">Dealer:
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cbo_DealerName" runat="server" CssClass="ComboBoxCSS" Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="LabelCSS">Start Date:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txt_StartDate" runat="server" CssClass="TextBoxCSS jsdate" Width="100px"
                                                        TabIndex="17"></asp:TextBox>
                                                </td>
                                                <td class="LabelCSS">End Date:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txt_EndDate" runat="server" CssClass="TextBoxCSS jsdate" Width="100px"
                                                        TabIndex="17"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="6"></td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <asp:Button ID="Add_Dealer" runat="server" CssClass="ButtonCSS" Text="Save Dealer"
                                TabIndex="32" />
                        </td>
                    </tr>
                    <tr id="row_dealer" runat="server">
                        <td colspan="2" align="center">
                            <div id="Div5" style="margin-top: 0px; overflow: auto; width: 80%; padding-top: 0px; position: relative; height: 130px; left: 0px; top: 0px;"
                                align="center">
                                <asp:DataGrid ID="dg_Dealer" runat="server" AutoGenerateColumns="False" ShowFooter="false"
                                    Width="80%" CssClass="GridCSS">
                                    <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                    <FooterStyle HorizontalAlign="Center" CssClass="footer" VerticalAlign="Middle"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtn_Edit" runat="server" ImageUrl="~/Images/edit3.PNG" CommandName="Edit" />
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtn_Delete" runat="server" ImageUrl="~/Images/delete.gif"
                                                    CommandName="Delete" />
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Dealer">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_NameOFUser" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NameOFUser") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Start Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="lbl_StartDate" BackColor="white" Width="80px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                    onkeypress="scroll();"
                                                    runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.StartDate") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="End Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="lbl_EndDate" BackColor="white" Width="80px" Style="border-left-width: 0; border-bottom-width: 0; border-right-width: 0; border-top-width: 0;"
                                                    onkeypress="scroll();"
                                                    runat="server" CssClass="TextBoxCSS" Text='<%# DataBinder.Eval(Container, "DataItem.EndDate") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="BrokerId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_BrokerId" Width="75px" runat="server" Text='<%#Container.DataItem("BrokerId") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="BrokerDealerId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_BrokerDealerId" Width="75px" runat="server" Text='<%#Container.DataItem("BrokerDealerId") %>'
                                                    CssClass="LabelCSS"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="BrokerDealerDetailId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_BrokerDealerDetailId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BrokerDealerDetailId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle PageButtonCount="2" />
                                </asp:DataGrid>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="SeperatorRowCSS" colspan="6"></td>
                    </tr>
                    <tr>
                        <td class="HeaderCSS" align="center" style="width: 100%;" colspan="6">Upload Documents
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="6">
                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td width="100%" align="center" valign="top">
                                        <table id="Table4" width="100%" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <div id="Document">
                                                        <div style="text-align: left;" class="SectionHeaderCSS" height="150px">
                                                            <a id="lnkDocument" style="cursor: pointer; font-weight: normal; color: Blue; border-bottom-color: Blue;"
                                                                title="Add new documents">CLICK HERE TO ADD DOCUMENTS</a>
                                                        </div>
                                                        &nbsp;&nbsp;&nbsp;
                                                                    <div id="divDocument" runat="server" style="width: 100%;">
                                                                        <table id="tblDocument" cellpadding="0" cellspacing="0" class="table_border_right_bottom tablerowbg"
                                                                            width="100%">
                                                                            <tr class="table_heading">
                                                                                <td style="width: 0%; display: none;"></td>
                                                                                <td width="200px">Document Type
                                                                                </td>
                                                                                <td>Upload File
                                                                                </td>
                                                                                <td>Download
                                                                                </td>
                                                                                <td style="width: 3%;">&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 0%; display: none;"></td>
                                                                                <td style="width: 20%;">
                                                                                    <select id="cboDocumentType" class="combo">
                                                                                    </select>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                                                                </td>
                                                                                <td></td>
                                                                                <td>
                                                                                    <a href="" class="delete">
                                                                                        <img title="Delete" class="imgdelete" src="../Images/delete.gif" alt="Delete" />
                                                                                    </a>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                            <asp:Button ID="btn_Update" runat="server" CssClass="ButtonCSS" Text="Update" />
                            <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="Hid_DealeretailId" runat="server" />
                <asp:HiddenField ID="Hid_DocumentDetails" runat="server" />
                <asp:HiddenField ID="Hid_BrokerDocumentId" runat="server" />
                <asp:HiddenField ID="Hid_DocumentId" runat="server" />
                <asp:HiddenField ID="Hid_DocumentMaster" runat="server" />
                <%--  </ContentTemplate>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
    </table>
</asp:Content>
