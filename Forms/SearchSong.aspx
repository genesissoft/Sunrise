<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SearchSong.aspx.vb" Inherits="Forms_SearchSong" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <table id="Table2" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td align="left">
                    <img src="../Images/script test2.jpg" style="width: 100%; height: 43px;" />
                </td>
            </tr>
            <tr>
                <td class="SeperatorRowCSS">
                </td>
            </tr>
            <tr>
                <td class="HeaderCSS" align="center" colspan="2">
                    Search Song</td>
            </tr>
            <tr>
                <td align="center">
                    <table id="Table1" width="60%" align="center" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td class="SeperatorRowCSS">
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS" width="200" style="height: 21px">
                                Language:
                            </td>
                            <td align="left" style="height: 21px">
                                <asp:DropDownList ID="cbo_Language" runat="server" Width="150px" CssClass="ComboBoxCSS">
                                    <asp:ListItem Value="E">English</asp:ListItem>
                                    <asp:ListItem Value="H">Hindi</asp:ListItem>
                                    <asp:ListItem Value="M">Marathi</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS" width="200">
                                Select:
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="rdoList_Select" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" CssClass="ComboBoxCSS" AutoPostBack="True" Width="360px">
                                    <asp:ListItem Selected="True" Value="TL">Title</asp:ListItem>
                                    <asp:ListItem Value="AL">Album</asp:ListItem>
                                    <asp:ListItem Value="AT">Artist</asp:ListItem>
                                    <asp:ListItem Value="GE">Genre</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelCSS" width="200">
                                Title:
                            </td>
                            <td align="left" style="height: 27px">
                                <asp:TextBox ID="txt_Title" runat="server" CssClass="fieldcontent1" Width="124px"
                                    TabIndex="25"></asp:TextBox>
                                <asp:Button ID="btn_Titles" runat="server" CssClass="ButtonCSS" Text="Search" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="height: 27px" colspan="2">
                                Album:<asp:TextBox ID="txt_Album" runat="server" CssClass="fieldcontent1" Width="95px"
                                    TabIndex="25"></asp:TextBox>
                                <asp:Button ID="btn_SearchAlbum" runat="server" CssClass="SearchButtonCSS" Text="..." />
                                Artist:<asp:TextBox ID="txt_Artist" runat="server" CssClass="fieldcontent1" Width="95px"
                                    TabIndex="25"></asp:TextBox>
                                <asp:Button ID="btn_SearchArtist" runat="server" CssClass="SearchButtonCSS" Text="..." />
                                Genre:<asp:TextBox ID="txt_Genre" runat="server" CssClass="fieldcontent1" Width="95px"
                                    TabIndex="25"></asp:TextBox>
                                <asp:Button ID="btn_SearchGenre" runat="server" CssClass="SearchButtonCSS" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td class="SeperatorRowCSS">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <div id="div2" style="margin-top: 0px; overflow: auto; width: 600px; padding-top: 0px;
                                    position: relative; height: 200px">
                                    <asp:DataGrid ID="dgProduct" runat="server" CssClass="GridCSS" ShowFooter="True"
                                        AutoGenerateColumns="false" TabIndex="38" Width="600px">
                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridHeaderCSS" />
                                        <ItemStyle HorizontalAlign="Left" CssClass="GridRowCSS" />
                                        <Columns>
                                            <%--<asp:TemplateColumn>
                                            <ItemTemplate>
                                                <img src="../Images/images3.JPG" id="chkSelect" style="cursor: hand" runat="server"
                                                    height="13" onmouseover="" width="13" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="imgBtn_Edit" CommandName="Edit" runat="server" ToolTip="Edit"
                                                    CssClass="TitleText" Text="Edit">                                                                                                          
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <FooterStyle Wrap="False"></FooterStyle>
                                        </asp:TemplateColumn>--%>
                                            <asp:TemplateColumn HeaderText="Title">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Title" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>'
                                                        Width="100px"></asp:Label>
                                                    <%--<asp:LinkButton ID="lnk_OrderNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrderNumber") %>'
                                                    CommandName="OpenViewWindow" ToolTip="Click to See View" Width="50px"></asp:LinkButton>--%>
                                                </ItemTemplate>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Artist">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Artist" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Artist") %>'
                                                        Width="100px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Album">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Album" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Album") %>'
                                                        Width="100px"></asp:Label>
                                                    <%--<asp:TextBox ID="txt_ProductDescription" BackColor="#FFFFFF" Width="90px" Style="border-left-width: 0;
                                                    border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                    runat="server" CssClass="formcontentGridText" Text='<%# DataBinder.Eval(Container, "DataItem.ProductDescription") %>'></asp:TextBox>--%>
                                                </ItemTemplate>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Genre">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Genre" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Genre") %>'
                                                        Width="100px"></asp:Label>
                                                    <%--<asp:TextBox ID="txt_Finish" BackColor="#FFFFFF" Width="90px" Style="border-left-width: 0;
                                                    border-bottom-width: 0; border-right-width: 0; border-top-width: 0;" onkeypress="scroll();"
                                                    runat="server" CssClass="formcontentGridText" Text='<%# DataBinder.Eval(Container, "DataItem.FinishDescription") %>'></asp:TextBox>--%>
                                                </ItemTemplate>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle PageButtonCount="2" />
                                    </asp:DataGrid>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
