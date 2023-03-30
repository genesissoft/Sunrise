<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SuperUserAuthent.aspx.vb"
    Inherits="Forms_SuperUserAuthent" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server" >
<title>  Super User Authentication  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</title>
    <base target="_self" />
    <link href="../Include/DatePicker.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script language="javascript"> 
        function EnterTab(e)
         { 
//        alert(e.srcElement.id)
            if((e.srcElement.id != 'btn_Save')&& (e.srcElement.id != 'btnOk'))
            {
                if(window.event.keyCode == 13) 
                window.event.keyCode = 9;
            }

        }
    
     function fnUnloadHandler(flag)
     {       
         
        // alert("fnUnloadHandler");
        // alert(window.returnValue );
         if( window.returnValue==true)
         {
              window.returnValue = true;
              window.close();
         }
         else if(window.returnValue== undefined || window.returnValue==false)
         {
              window.returnValue = false;
              window.close();
         }
 
     } 
     
     function fnUnloadHandler_1(flag)
     {
        //alert("1");
        document.getElementById("hdn_pwdflag").value = flag;
        if(flag=="T")
        {
             window.returnValue = true;
             window.close();
        }
        else (flag=="N")
        {
             alert("In-Valid Password");                
             return false;
        }       
     }
     
     function CloseFormOk()
     {
         
        var newpwd=document.getElementById("txt_Password").value;
        var oldpwd=document.getElementById("hid_pwd").value;
        //var encrypt_new=Crypt(newpwd);
        
//       alert(newpwd);
//      alert(oldpwd);
        if(oldpwd ==newpwd)
        {
             window.returnValue = true;
             window.close();
        }
        else
        {
             alert("In-Valid Password");                
             return false;
        }
     
     }
  
//      function Crypt(oldval)
//      {

//               var NewVal ;
//               var j ;
//               
//               NewVal = "";
//               for(j=0; j<oldval.length; j++)
//               {
//                    NewVal = NewVal & Chr(170 - Asc(Mid(oldval, j, 1)));
//               }
//               //alert("Crypt");
//               //alert (NewVal);
//               return NewVal;
//        }
//   
//        function Mid(strInput,intStart,intLength)
//        {
//            return strInput.substring(intStart,intLength)  
//        }
        
        

    </script>
    <script>
        function Asc(String)
        {

	        return String.charCodeAt(0);

        }

        function Chr(AsciiNum)
        {

	        return String.fromCharCode(AsciiNum)

        }
    </script>

    <script language="javascript" type="text/javascript">
       
        function CloseFormCancel()
        {
          // alert("CloseFormCancel");
           window.close();
           window.returnValue = false;
        }

    </script>

</head>
<body onbeforeunload="fnUnloadHandler('a')" backcolor="#E1E1C3" >
    <form id="form1" runat="server" backcolor="#E1E1C3" onkeydown ="EnterTab(event)">
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />--%>
        <div align="center">
            <table id="Table2" align="center" cellspacing="0" cellpadding="0" border="0" 
                width="100%">
                <tr>
                    <td class="HeaderCSS" align="center" >
                      Enter Password  
                    </td>
                </tr>
                <tr>
                    <td >
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td bgcolor ="white">
                        <table align="center" cellspacing="0" cellpadding="0" border="0" bgcolor="white">
                            <tr>
                                <td class="LabelCSS">
                                    Password:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Password" runat="server" CssClass="TextBoxCSS" Width="100px"
                                        TextMode="Password"></asp:TextBox><em><span style="color: #ff0000">*</span></em>&nbsp;
                                </td>
                            </tr>
                             <tr>
                    <td class="SeperatorRowCSS">
                        &nbsp;
                    </td>
                </tr>
                            <tr>
                             <td align="center" colspan ="2">
                        <%--<asp:Button ID="btn_Ok" runat="server" CssClass="ButtonCSS" Text="Ok" Width="58px" />--%>
                        <input id="btnOk" onclick="CloseFormOk();"  class="ButtonCSS" type="button" value="Ok" Width="58px"/>
                        <%--<asp:Button ID="btn_Cancel" runat="server" CssClass="ButtonCSS" Text="Cancel" />--%>
                        <%--<input id="btnCancel" onclick="window.returnValue = false;CloseFormCancel();" type="button" value="Cancel" />--%>
                        <asp:Button ID="btnCancel" runat="server" CssClass="ButtonCSS" Text="Cancel" Width="58px" />
                    </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="SeperatorRowCSS">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                   
                </tr>
                <asp:HiddenField ID="hid_pwd" runat="server" />
                <asp:HiddenField ID="hdn_pwdflag" runat="server" Value="" />
                <asp:HiddenField ID="hdn_cnt" runat="server" Value="" />
            </table>
        </div>
    </form>
</body>
</html>
