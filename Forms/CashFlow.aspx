<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CashFlow.aspx.vb" Inherits="Forms_CashFlow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <base target="_self">
    </base>
    <title>Cash Flow</title>
    <link href="../Include/General.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Parkstone.css" rel="stylesheet" type="text/css" />
    <link href="../Include/General.css" type="text/css" rel="stylesheet" />
    <link href="../Include/Intervention.css" type="text/css" rel="stylesheet" />
    <link href="../Include/StanChart.css" type="text/css" rel="stylesheet" />

    <script language="javascript" src="../Include/Common.js"></script>

    <script language="javascript" src="../Include/calendar.js" type="text/javascript"></script>
  <script language="javascript">
        function closeWin()
        {
            timer = setTimeout('checkForOpener()', 0);
        }

        function checkForOpener()
        {
            try
            { 
                var myVar=window.opener.document;
                timer = setTimeout('checkForOpener()', 0);
            }
            catch(e)
            {
                window.close();
            }
        }
        function validateEmailAddress() 
        {
            if(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(document.frm.emailAddr.value))
            {
                return true
            }
            alert("Error: Invalid E-mail Address! \n\nPlease enter a valid Email Address.")
            return false
        }
    </script>  
</head>
<body onload="closeWin()" >
    <form id="frmCashFlow" runat="server">
    <div>
        <table align=center cellpadding=0 cellspacing=0 height="100%" width="100%"> 
            <tr>
                <td align=center id="TD1" >
                    <table align=center cellpadding=0 cellspacing=0 height="80%" width="80%" class="table_border_right_bottom">
                        <tr>
                            <td  class="HeaderCSS" align="center" colspan="4" >
                              <strong>Cash Flow</strong> 
                            </td>
                        </tr>
                        <tr>
                            <td >&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align=center   >
                                <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="False" CellPadding="4" CssClass="GridCSS"
                                      >
                                         <RowStyle CssClass="GridRowCSS" />
                                        <HeaderStyle CssClass="GridHeaderCSS" />
                                        <PagerStyle CssClass="table_border_none" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" ReadOnly="True" DataField="Date"  >
                                            <ItemStyle BorderStyle=Solid BorderColor=black BorderWidth="1px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Amount" ReadOnly="True" DataField="Amount"  >
                                            <ItemStyle BorderStyle=Solid BorderColor=black BorderWidth="1px" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                    </Columns>
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <EditRowStyle BackColor="#999999" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle BorderStyle=Solid BorderColor=black BorderWidth="1px"  BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
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
