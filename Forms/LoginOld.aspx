<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LoginOld.aspx.vb" Inherits="Forms_Login"
    EnableSessionState="true" %>
<%@ Register Assembly="Login" Namespace="Login.UserSecurity" TagPrefix="cc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="margin: 0; padding: 0;">
<head id="Head1" runat="server">
    <title>Login Page</title>
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
    
    
    
    
function Focus() {  
	document.getElementById("Login1_UserName").focus();
}
function panelBG() {
	var arVersion = navigator.appVersion.split("MSIE")
	var version = parseFloat(arVersion[1]);
	//&& (document.body.filters)
	if (version >= 5.5 )
	{  
	    imgStr = "background-image: url();";
	} else {
		imgStr = "background-image: url(../images/none.png);";
		
	}
	document.write("<style type=\"text/css\">");
	document.write("<!--");
	document.write(".loginBoxTD {");
	document.write(imgStr);
	document.write("}");
	document.write("-->");
	document.write("</style>");
}
panelBG();
    </script>

    <!--[if lt IE 7]>
<script language="JavaScript">
function correctPNG() // correctly handle PNG transparency in Win IE 5.5 & 6.
{
   var arVersion = navigator.appVersion.split("MSIE")
   var version = parseFloat(arVersion[1])
   if ((version >= 5.5) && (document.body.filters)) 
   {
	  for(var i=0; i<document.images.length; i++)
	  {
		 var img = document.images[i]
		 var imgName = img.src.toUpperCase()
		 if (imgName.substring(imgName.length-3, imgName.length) == "PNG")
		 {
			var imgID = (img.id) ? "id='" + img.id + "' " : ""
			var imgClass = (img.className) ? "class='" + img.className + "' " : ""
			var imgTitle = (img.title) ? "title='" + img.title + "' " : "title='" + img.alt + "' "
			var imgStyle = "display:inline-block;" + img.style.cssText 
			if (img.align == "left") imgStyle = "float:left;" + imgStyle
			if (img.align == "right") imgStyle = "float:right;" + imgStyle
			if (img.parentElement.href) imgStyle = "cursor:hand;" + imgStyle
			var strNewHTML = "<span " + imgID + imgClass + imgTitle
			+ " style=\"" + "width:" + img.width + "px; height:" + img.height + "px;" + imgStyle + ";"
			+ "filter:progid:DXImageTransform.Microsoft.AlphaImageLoader"
			+ "(src=\'" + img.src + "\', sizingMethod='scale');\"></span>" 
			img.outerHTML = strNewHTML
			i = i-1
		 }
	  }
   }    
}
window.attachEvent("onload", correctPNG);
</script>
<![endif]-->
    <link href="../include/loginScreen.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!
        -- #formBox
        {
            position: absolute;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 1;
        }
        -- ></style>

    <script type="text/javascript">
<!--
function MM_reloadPage(init) {  //reloads the window if Nav4 resized
  if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
    document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
  else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
}
MM_reloadPage(true);
//-->
    </script>

    <meta http-equiv="Content-Type" />
</head>
<body class="loginBody" onload="Focus();" style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <div id="pageBox" style="margin-top: 10%;">
        <table border="0" align="center" width="500px" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" valign="middle">
                    <table width="100%" border="0" cellspacing="2" cellpadding="2" style="background-color: white;
                        border-radius: 10px; border: 5px solid #007e9e; border-radius: 10px; -webkit-box-shadow: 6px 5px 5px 0px #83836f;
                        -moz-box-shadow: 6px 5px 5px 0px #83836f; box-shadow: 6px 5px 5px 0px #83836f;">
                        <tr>
                            <td align="left" style="padding-bottom:0px; margin-bottom"0px;">
                                <img src="../Images/CompanyLogo.png" height="80" width="300" /><br />
                            </td>
                            <td align="right" style="font-weight: bold; font-size: 18px; color:#444444; padding-bottom:0px; margin-bottom:0px; padding-right:5px;">
                                Staff Login
                            </td>
                        </tr>
                        <tr style="border-bottom: 1px solid #007e9e; padding:0px; margin:0px;">
                        <td colspan="2" style="border-bottom: 1px solid #007e9e; padding:0px; margin:0px;">
                        &nbsp;
                        </td>
                        </tr>
                        <tr align="center" valign="top">
                            <td colspan="2" style="padding:10px;">
                                <asp:Label ID="lbl_Error" Font-Size="9" runat="server" ForeColor="white" Visible="false"></asp:Label>
                                <cc:Login ID="Login1" runat="server" BorderPadding="0" BorderStyle="None" BorderWidth="0px"
                                    Font-Names="Verdana" Font-Size="1.0em" ForeColor="#393c1f" Width="100%" DestinationPageUrl="SelectYear.aspx"
                                    ChangeDateDatabaseField="PasswordChangeDate" UserMasterTableName="UserMaster"
                                    PasswordDatabaseField="Password" LoginNameDatabaseField="UserName" InactiveDatabaseField="Status"
                                    InactiveDatabaseValue="Status" DisableDatabaseField="D" 
                                    ConnectionString="<%$ ConnectionStrings:InstadealConnectionString %>"
                                    ChangePasswordUrl="~/Forms/ChangePassword.aspx?Flag=C">
                  <LayoutTemplate>
                     <table cellspacing="5" cellpadding="5" width="80%" border="0" align="center">
                              
                                <tr align="left">
                                  <td>
                                  Username:
                                  </td>
                                <td>
                                  <asp:TextBox id="UserName" runat="server" CssClass="frmTextBox"></asp:TextBox>
                                  <asp:RequiredFieldValidator id="UserNameRequired" runat="server" CssClass="frmLabel" ErrorMessage="User Name is required." ToolTip="User Name is required." ControlToValidate="UserName" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                  </td>
                                </tr>
                              
                                <tr  align="left">
                                  <td>
                                  Password:
                                  </td>
                               
                                  <td>
                                  <asp:TextBox id="Password" runat="server" CssClass="frmTextBox" TextMode="Password"></asp:TextBox>
                                  <asp:RequiredFieldValidator id="PasswordRequired" runat="server" CssClass="frmLabel" ErrorMessage="Password is required." ToolTip="Password is required." ControlToValidate="Password" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                  </td>
                                </tr>
                                 <tr align="left">
                                  <td>&nbsp;</td>
                                  
                                  <td>
                                  <asp:CheckBox id="RememberMe" runat="server" CssClass="frmLabel" Text="Remember me"></asp:CheckBox>
                                  </td>
                                </tr>
                                <tr align="left">
                                  <td>&nbsp;</td>
                                  <td>
                                  <asp:Button id="Login1" runat="server" CssClass="ButtonCSS" Text="Login" 
                                          ValidationGroup="Login1" CommandName="Login"></asp:Button>
                                  </td>
                                </tr>
                               
                                <tr align="center">
                                  
                                  <td colspan="2">
                                  <asp:Literal id="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                  </td>
                                </tr>
                            </table>
                           
                  </LayoutTemplate>
                                </cc:Login>
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="2" valign="bottom" style="padding: 5px;">
                                Copyright <b>&copy;</b> 2015 Genesis Software
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
