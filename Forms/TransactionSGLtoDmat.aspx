<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false" CodeFile="TransactionSGLtoDmat.aspx.vb" Inherits="Forms_TransactionSGLtoDmat" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center" colspan="6">
                Transaction SGL To DMAT
            </td>
        </tr>
        <tr>
            <td colspan="6">
                &nbsp;
            </td>
        </tr>
        
        <tr>
            <td align="center">
                <table id="Table4" align="center" cellspacing="0" cellpadding="0" border="0" width="50%">
                    <tr>
                        <td>
                            <table id="Table5" align="left" cellspacing="0" cellpadding="0" border="0" width="100%">
                              
                                <tr>
                                    <td class="LabelCSS" >
                                        From Date:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_FromDate" runat="server" CssClass="TextBoxCSS" Width="123px"></asp:TextBox>
                                        <a onclick="showCalendar(document.getElementById('txt_FromDate'),'200','300');"
                                            href="javascript:doNothing()">
                                            <img class="formcontent" height="15" src="../Images/Calender.jpg" width="15" border="0" align="middle"></a>
                                    </td>
                                    <td class="LabelCSS" >
                                        To Date:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_ToDate" runat="server" CssClass="TextBoxCSS" Width="123px"></asp:TextBox>
                                        <a onclick="showCalendar(document.getElementById('txt_ToDate'),'200','300');"
                                            href="javascript:doNothing()">
                                            <img class="formcontent" height="15" src="../Images/Calender.jpg" width="15" border="0" align="middle"></a>
                                    </td>
                                    <td>
                                      <asp:Button ID="btn_Show" runat="server" CssClass="SearchButtonCSS" Text="Show" />
                                    
                                    </td>
                                </tr>
                             
                                
                            </table>
                        </td>
                        
                    </tr>
                </table>
            </td>
        </tr>
           
        
        <tr>
            <td style="height: 81px" align="center">
                <div id="div2" style="margin-top: 0px; overflow: auto; width: 530px; padding-top: 0px;
                    position: relative; height: 70px">
                    <asp:DataGrid ID="dg_Dematdetails" runat="server" CssClass="GridCSS" ShowFooter="True"
                        AutoGenerateColumns="false" TabIndex="38" Width="530px">
                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                        <Columns>
                         <asp:TemplateColumn HeaderText="Trans Code" >
                                <ItemTemplate>
                                <asp:Label ID="lbl_TransCode" Width="75px" runat="server" Text='<%# container.dataitem("TransCode") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                  
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" Width="60px" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                            </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="Issuer Of Security" >
                                <ItemTemplate>
                                <asp:TextBox ID="txt_IssuerOfSecurity" Width="75px" runat="server" CssClass="TextBoxCSS" Text='<%# container.dataitem("IssuerOfSecurity") %>'>
                                
                                
                                </asp:TextBox>
                                  
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" Width="60px" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                            </asp:TemplateColumn>
                            
                                <asp:TemplateColumn HeaderText="Security" >
                                <ItemTemplate>
                                <asp:TextBox ID="txt_SecurityName" Width="75px" runat="server"  CssClass="TextBoxCSS" Text='<%# container.dataitem("SecurityName") %>'>
                                
                                
                                </asp:TextBox>
                                  
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" Width="60px" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                            </asp:TemplateColumn>
                              <asp:TemplateColumn HeaderText="Face Value">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Remark" runat="server" Width="100px" Text='<%# container.dataitem("FaceValue") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle"  />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle"  />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Settlement Date" >
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SettlementDate" Width="75px" runat="server" Text='<%# container.dataitem("SettlementDate") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" Width="60px" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                            </asp:TemplateColumn>
                             <asp:TemplateColumn HeaderText="Transaction Date" >
                                <ItemTemplate>
                                    <asp:Label ID="lbl_TransactionDate" Width="75px" runat="server" Text='<%# container.dataitem("TransactionDate") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" Width="60px" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="DMAT Slip No" Visible=false>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ChequeNumber" runat="server" Width="100px" Text='<%# container.dataitem("DmatSlipNo") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle"  />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="ClientId" Visible=false>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Amount" runat="server" Width="45px" Text='<%# container.dataitem("ClientId") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="DpId" Visible=false>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BankName" runat="server" Width="45px" Text='<%# container.dataitem("DpId") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Dp Name" Visible=false>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FDType" runat="server" Width="100px" Text='<%# container.dataitem("DpName") %>'
                                        CssClass="LabelCSS"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" VerticalAlign="Middle"  />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle"  />
                            </asp:TemplateColumn>
                          
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Edit"
                                        CssClass="InfoLinkCSS" Text="Edit"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" Text="Update"
                                        CssClass="InfoLinkCSS"></asp:LinkButton><br />
                                    <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel"
                                        CssClass="InfoLinkCSS" Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="deletebtn" runat="server" CausesValidation="false" Text="Delete"
                                        CommandName="delete" CssClass="InfoLinkCSS" />
                                </ItemTemplate>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        
        <tr>
            <td class="SectionHeaderCSS" align="left" colspan="6">
                 DMAT Information</td>
        </tr>
        <tr>
            <td colspan="6" class="SeperatorRowCSS" style="height: 10px">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="6">
                <table id="Table7" align="center" cellspacing="0" cellpadding="0" border="0" width="532">
                    <tr>
                        <td style="height: 138px" valign="top">
                            <table id="Table6" align="right" cellspacing="0" cellpadding="0" border="0" >
                                <tr>
                                    <td class="LabelCSS" width="25%">
                                        Face Value:
                                    </td>
                                    <td align="left" width="40%">
                                        <asp:TextBox ID="txt_FDType" runat="server" Width="134px" CssClass="TextBoxCSS" MaxLength="50"
                                            TabIndex="7"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS" width="140">
                                        NSDL Face Value:
                                    </td>
                                    <td align="left" width="150">
                                        <asp:TextBox ID="txt_NSDLFaceValue" runat="server" Width="134px" CssClass="TextBoxCSS"
                                            MaxLength="50" TabIndex="7"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">
                                        Slip Number:
                                    </td>
                                    <td align="left" width="150">
                                        <asp:TextBox ID="txt_Amount" runat="server" Width="134px" CssClass="TextBoxCSS" MaxLength="50"
                                            TabIndex="7"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS" style="height: 20px">
                                        Client Name:
                                    </td>
                                    <td align="left" width="150" style="height: 20px">
                                        <asp:TextBox ID="txt_ChqNo" runat="server" Width="134px" CssClass="TextBoxCSS" MaxLength="50"
                                            TabIndex="7"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">
                                        DP Name:
                                    </td>
                                    <td align="left" width="150">
                                        <asp:DropDownList ID="cbo_DPName" runat="server" CssClass="ComboBoxCSS" Height="19px"
                                            Width="136px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">
                                        DP ID:
                                    </td>
                                    <td align="left" width="150">
                                        <asp:TextBox ID="txt_DPId" runat="server" Width="134px" CssClass="TextBoxCSS" MaxLength="50"
                                            TabIndex="7"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">
                                        Client ID:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_ClientId" runat="server" Width="134px" CssClass="TextBoxCSS"
                                            MaxLength="50" TabIndex="7"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" style="height: 138px">
                            <table id="Table2" align="left" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td class="LabelCSS" width="125">
                                        Quantity:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_Qty" runat="server" Width="134px" CssClass="TextBoxCSS" MaxLength="50"
                                            TabIndex="7"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">
                                        Delivery Date:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_DelDate" runat="server" Width="134px" CssClass="TextBoxCSS"
                                            MaxLength="50" TabIndex="7"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">
                                        DMAT Account To:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cbo_DematAccTo" runat="server" CssClass="ComboBoxCSS" Height="19px"
                                            Width="136px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">
                                        Customer Slip No:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_CustSlipNo" runat="server" Width="134px" CssClass="TextBoxCSS"
                                            MaxLength="50" TabIndex="7"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">
                                        Customer DP Name:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_CustDPName" runat="server" Width="134px" CssClass="TextBoxCSS"
                                            MaxLength="50" TabIndex="7"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">
                                        Customer DP Id:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_CustDPId" runat="server" Width="134px" CssClass="TextBoxCSS"
                                            MaxLength="50" TabIndex="7"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="LabelCSS">
                                        Customer Client Id:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_CustClientId" runat="server" Width="134px" CssClass="TextBoxCSS"
                                            MaxLength="50" TabIndex="7"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" class="SeperatorRowCSS">
                        </td>
                    </tr>
                    <tr id="buttonid" runat="server">
                        <td align="center" colspan="2">
                            <asp:Button ID="btn_Savedetails" runat="server" Text="Save" ToolTip="Save" CssClass="ButtonCSS"
                                Height="20px" />
                            <asp:Button ID="btn_canceldetails" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="ButtonCSS"
                                Height="20px" />
                        </td>
                    </tr>
                    
                     <tr id="Tr1" runat="server" visible=false>
                        <td align="center" colspan="2">
                            <asp:Button ID="btn_Submit" runat="server" Text="Submit" ToolTip="Save" CssClass="ButtonCSS"
                                Height="20px" />
                            <asp:Button ID="btn_Nextdetails" runat="server" Text="Next  Details" ToolTip="Cancel" CssClass="ButtonCSS"
                                Height="20px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       
        <tr>
            <td class="SeperatorRowCSS" colspan="6">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="6">
                <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save" />
                <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />
            </td>
        </tr>
    </table>

</asp:Content>

