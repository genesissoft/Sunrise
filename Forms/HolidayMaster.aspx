<%@ Page Language="VB" MasterPageFile="~/Forms/MasterPage.master" AutoEventWireup="false"
    CodeFile="HolidayMaster.aspx.vb" Inherits="Forms_HolidayMaster" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"> </script>
 <script type="text/javascript">
        function Add(txtbox)
        {
        
        
        
        
            if(Trim(document.getElementById("ctl00_ContentPlaceHolder1_txt_AprilDate").value) == "")
            {
                alert("Please Enter date");
                return false;
            }
         
            
            
            
        }
    </script>
    <table id="Table1" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="HeaderCSS" align="center">
                Holiday Master</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <atlas:UpdatePanel ID="UpdatePanel1" runat="server" Mode="Conditional">
                    <ContentTemplate>
                        <table id="Table3" cellspacing="0" cellpadding="0" align="center" border="1">
                        <tr>
                        <td colspan="3" align=center>
                        APRIL <asp:Literal ID = "lit_April" runat=server></asp:Literal>
                        </td>
                        <td colspan="3" align="center">
                        MAY <asp:Literal ID = "lit_May" runat=server></asp:Literal>
                        </td>
                        <td colspan="3" align="center">
                        JUNE <asp:Literal ID = "lit_June" runat=server></asp:Literal>
                        </td>
                         <td colspan="3" align="center">
                             JULY <asp:Literal ID = "lit_July" runat=server></asp:Literal>
                        </td>
                        
                        </tr>
                            <tr bordercolorlight="gray">
                                <td class="LabelCSS">
                                   
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_AprilDate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px"></asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG4" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_AprilDate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddAprilDate" runat="server" CssClass="ButtonCSS1" Text="Add"
                                        Width="50px"></asp:Button>
                                </td>
                               <td class="LabelCSS">
                                  
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Maydate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px">
                                    </asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG5" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Maydate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddMayDate" runat="server" CssClass="ButtonCSS1" Text="Add  "
                                        Width="50px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                               
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Junedate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px">
                                    </asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG6" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Junedate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddJunedate" runat="server" CssClass="ButtonCSS1" Text="Add "
                                        Width="50px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_JulyDate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px"></asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG7" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_JulyDate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddJulydate" runat="server" CssClass="ButtonCSS1" Text="Add  "
                                        Width="50px"></asp:Button>
                                </td>
                            </tr>
                            <tr bordercolorlight="gray">
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="list_Aprildate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_Removeaprildate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="lst_Maydate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_RemoveMaydate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="list_Junedate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_RemoveJunedate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="lst_Julydate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_removejulydate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                            </tr>
                            
                               <tr>
                        <td colspan="3" align=center>
                         AUGUST <asp:Literal ID = "lit_Aug" runat=server></asp:Literal>
                        </td>
                        <td colspan="3" align="center">
                        SEPTEMBER <asp:Literal ID = "lit_Sept" runat=server></asp:Literal>
                        </td>
                        <td colspan="3" align="center">
                        OCTOBER <asp:Literal ID = "lit_Oct" runat=server></asp:Literal>
                        </td>
                         <td colspan="3" align="center">
                             NOVEMBER <asp:Literal ID = "lit_Nov" runat=server></asp:Literal>
                        </td>
                        
                        </tr>
                            <tr bordercolorlight="gray">
                                <td class="LabelCSS">
                                 
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Augdate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px"></asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG8" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Augdate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddAugdate" runat="server" CssClass="ButtonCSS1" Text="Add  "
                                        Width="50px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                   
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Septdate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px">
                                    </asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG9" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Septdate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddSeptdate" runat="server" CssClass="ButtonCSS1" Text="Add  "
                                        Width="50px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                  
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Octdate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px">
                                    </asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG10" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Octdate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddOctdate" runat="server" CssClass="ButtonCSS1" Text="Add "
                                        Width="50px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                   
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_NovDate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px"></asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG11" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_NovDate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddNovdate" runat="server" CssClass="ButtonCSS1" Text="Add  "
                                        Width="50px"></asp:Button>
                                </td>
                            </tr>
                            <tr bordercolorlight="gray">
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="lst_augdate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_Removeaugdate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="lst_septdate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_removeseptdate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="lst_Octdate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_Removeoctdate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="lst_Novdate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_removenovdate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                        <td colspan="3" align=center>
                        DECEMBER <asp:Literal ID = "lit_Dec" runat=server></asp:Literal>
                        </td>
                        <td colspan="3" align="center">
                        JANUARY <asp:Literal ID = "lit_Jan" runat=server></asp:Literal>
                        </td>
                        <td colspan="3" align="center">
                        FEBRUARY <asp:Literal ID = "lit_feb" runat=server></asp:Literal>
                        </td>
                         <td colspan="3" align="center">
                             MARCH <asp:Literal ID = "lit_mar" runat=server></asp:Literal>
                        </td>
                        
                        </tr>
                            <tr bordercolorlight="gray">
                                <td class="LabelCSS">
                                
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Decdate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px"></asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG12" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Decdate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddDecdate" runat="server" CssClass="ButtonCSS1" Text="Add  "
                                        Width="50px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                   
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_JanDate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px">
                                    </asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG1" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_JanDate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddJanDate" runat="server" CssClass="ButtonCSS1" Text="Add  "
                                        Width="50px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                    
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Febdate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px">
                                    </asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG2" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_Febdate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddFebdate" runat="server" CssClass="ButtonCSS1" Text="Add "
                                        Width="50px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                  
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_marchdate" TabIndex="13" runat="server" CssClass="TextBoxCSS"
                                        Width="110px"></asp:TextBox>
                                    <img style="vertical-align: top; cursor: hand" id="IMG3" class="formcontent" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txt_marchdate',this);"
                                        height="14" src="../Images/Calender.jpg" width="15" border="0" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_AddMarchdate" runat="server" CssClass="ButtonCSS1" Text="Add  "
                                        Width="50px"></asp:Button>
                                </td>
                            </tr>
                            <tr bordercolorlight="gray">
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="lst_decdate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_removedecdate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="List_JanDate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_RemoveJanDate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="list_Febdate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_RemoveFebDate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                                <td class="LabelCSS">
                                </td>
                                <td>
                                    <asp:ListBox ID="lst_Marchdate" TabIndex="14" runat="server" CssClass="TextBoxCSS"
                                        Width="140px" Height="45px"></asp:ListBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_removeMarchdate" runat="server" CssClass="ButtonCSS1" Text="Remove"
                                        Width="55px"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td class="SeperatorRowCSS" colspan="11">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="12">
                                    <asp:Button ID="btn_Save" runat="server" CssClass="ButtonCSS" Text="Save"></asp:Button>
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Back"></asp:Button>
                                </td>
                            </tr>
                            <asp:HiddenField ID="Hid_CompId" runat="server" />
                            <asp:HiddenField ID="Hid_YearText" runat="server" />
                              <asp:HiddenField ID="Hid_Year" runat="server" />
                                    <asp:HiddenField ID="Hid_HolidayId" runat="server" />
                              
                            
                        </table>
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
