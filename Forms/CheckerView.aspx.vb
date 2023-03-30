Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Forms_CheckerView
    Inherits System.Web.UI.Page
    Dim sqlConn As SqlConnection
    Dim objCommon As New clsCommonFuns
    Dim hidIds As New HiddenField
    Dim hidTables As New HiddenField
    Dim hidCols As New HiddenField
    Dim blnBind As Boolean

    Dim mailFromChecker As String = (ConfigurationManager.AppSettings("mailFromChecker"))
    Dim mailToChecker As String = (ConfigurationManager.AppSettings("mailToChecker"))
    Dim mailCCChecker As String = (ConfigurationManager.AppSettings("mailCCChecker"))
    Dim mailBCCChecker As String = (ConfigurationManager.AppSettings("mailBCCChecker"))
    Dim mailSubjectChecker As String = (ConfigurationManager.AppSettings("mailSubjectChecker"))

    Dim lblClientName As String = ""
    Dim lblDealSlipNo As String = ""
    Dim lblFaceValue As String = ""
    Dim lblStatus As String = ""
    Dim body As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Session("username") = "") Then
                Response.Redirect("Login.aspx")
                Exit Sub
            End If
            If Page.IsPostBack = False Then
                blnBind = True
                BindData()
                CheckValidation(True)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Sub BindData()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = FillDataTable(sqlConn, "SPCheckerView")
            grdChecker.DataSource = dt
            grdChecker.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub OpenConn()
        If sqlConn Is Nothing Then
            sqlConn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
            sqlConn.Open()
        ElseIf sqlConn.State = ConnectionState.Closed Then
            sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString()
            sqlConn.Open()
        End If
    End Sub
    Public Function FillDataTable(ByVal sqlConn As SqlConnection, ByVal strProc As String) As DataTable
        Try
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dt As New DataTable
            OpenConn()
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.AddWithValue("@UserTypeId", Val(Session("UserTypeId")))
            sqlComm.Parameters.AddWithValue("@UserId", Val(Session("UserId")))
            sqlComm.Parameters.AddWithValue("@CompId", Val(Session("CompId")))
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Protected Sub grdChecker_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdChecker.ItemDataBound
        Try
            Dim hyp As HyperLink
            Dim strId As String
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                hyp = TryCast(e.Item.FindControl("hyplnk"), HyperLink)
                strId = TryCast(e.Item.DataItem, DataRowView).Row.Item("IdVal").ToString
                If strId <> "" Then
                    strId = objCommon.EncryptText(HttpUtility.UrlEncode(strId))
                End If
                hyp.NavigateUrl = hyp.NavigateUrl & "?Id=" & strId & "&Flag=C"
                hyp.Target = "_blank"
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub rblType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim RejectionReasonTextBox As New TextBox
        Dim rbl As RadioButtonList = CType(sender, RadioButtonList)
        Dim parent As Control = rbl.Parent
        While Not (TypeOf parent Is System.Web.UI.WebControls.DataGridItem)
            parent = parent.Parent
        End While

        Dim index As Integer = (CType(parent, DataGridItem)).ItemIndex
        RejectionReasonTextBox = CType(grdChecker.Items(index).FindControl("txtRejectionReason"), TextBox)
        If rbl.SelectedIndex = 1 Then
            RejectionReasonTextBox.Enabled = True
            RejectionReasonTextBox.BackColor = Drawing.Color.White
        Else
            RejectionReasonTextBox.Enabled = False
            RejectionReasonTextBox.Text = ""
            RejectionReasonTextBox.BackColor = Drawing.Color.White
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            '' '' '' **************Check Valiation**********************
            If CheckValidation(False) = False Then Exit Sub
            '' '' '' ***************************************************
            '' '' '' **************Save and Bind Data*******************
            SaveUpdate()
            BindData()
            Clear()
            '' '' '' ***************************************************
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Sub Clear()
        hidIds.Value = ""
        hidTables.Value = ""
        hidCols.Value = ""
    End Sub
    Sub SaveUpdate()
        Try
            hidIds.Value = ""
            hidTables.Value = ""
            hidCols.Value = ""
            SetValue()
            SaveGridValues(grdChecker, hidIds, hidTables, hidCols)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function CheckValidation(ByVal MVDefault As Boolean) As Boolean
        Dim rbtType As RadioButtonList
        Dim txtRejectionReason As TextBox
        For I As Integer = 0 To grdChecker.Items.Count - 1
            rbtType = CType(grdChecker.Items(I).FindControl("rbtType"), RadioButtonList)
            txtRejectionReason = CType(grdChecker.Items(I).FindControl("txtRejectionReason"), TextBox)
            If MVDefault = False Then
                If rbtType.SelectedIndex = 1 Then
                    If Trim(txtRejectionReason.Text) = "" Then
                        txtRejectionReason.Focus()
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Please put the Rejection Reason for all Rejected Entries');", True)
                        Return False
                    End If
                End If
            Else
                If rbtType.SelectedIndex = 2 Then
                    txtRejectionReason.Enabled = False
                    txtRejectionReason.BackColor = Drawing.Color.White
                End If
            End If
        Next
        Return True
    End Function
    Sub SetValue()
        Dim lblIDVal As Label
        Dim lblTableName As Label
        Dim lblPKColName As Label
        For I As Integer = 0 To grdChecker.Items.Count - 1
            lblIDVal = CType(grdChecker.Items(I).FindControl("lblIDVal"), Label)
            hidIds.Value = hidIds.Value & lblIDVal.Text & "!"

            lblTableName = CType(grdChecker.Items(I).FindControl("lblTableName"), Label)
            hidTables.Value = hidTables.Value & lblTableName.Text & "!"

            lblPKColName = CType(grdChecker.Items(I).FindControl("lblPKColName"), Label)
            hidCols.Value = hidCols.Value & lblPKColName.Text & "!"
        Next
    End Sub
    Private Sub SaveGridValues(ByVal dgMC As Object, ByVal hidIds As Object, ByVal hidTables As Object, ByVal hidCols As Object)
        Dim rdoList As Object
        Dim strRejReason As String
        Dim strQry As String
        Dim strIds() As String
        Dim strTables() As String
        Dim strCols() As String

        strIds = Split(hidIds.Value, "!")
        strTables = Split(hidTables.Value, "!")
        strCols = Split(hidCols.Value, "!")

        For I As Int16 = 0 To strTables.Length - 2
            strRejReason = TryCast(dgMC.Items(I).FindControl("txtRejectionReason"), Object).Text
            rdoList = TryCast(dgMC.Items(I).FindControl("rbtType"), Object)
            strQry = BuildValuesInsert(strTables(I), strCols(I), strIds(I), rdoList.SelectedValue, strRejReason)
            ExecuteDynamicQuery(strQry)
            strQry = BuildCheckerUpdate(strTables(I), strCols(I), strIds(I), rdoList.SelectedValue)
            ExecuteDynamicQuery(strQry)
            '' '' '' '' '' ''If strIds(I) <> "" And Trim(rdoList.SelectedValue).ToUpper() = "Accept".ToUpper() Then
            '' '' '' '' '' ''    SendMail(strIds(I))
            '' '' '' '' '' ''End If
        Next
    End Sub
    Sub SendMail(ByVal lblDealSlipID As String)
        Dim fromAddress As String = mailFromChecker
        Dim ToAddress As String = mailToChecker
        Dim CCAddress As String = mailCCChecker
        Dim BCCAddress As String = mailBCCChecker
        Dim Subject As String = ""
        Dim BodyMessage As String = ""
        BodyMessage = MailBodyMessage(lblDealSlipID, Subject)
        If Trim(BodyMessage) <> "" Then
            MailHelper.SendMailMessage(mailFromChecker, mailToChecker, mailCCChecker, mailBCCChecker, Subject, BodyMessage, "", "")
        End If
    End Sub
    Private Function MailBodyMessage(ByVal lblDealSlipID As String, ByRef Subject As String) As String
        Try
            OpenConn()
            Dim dt As DataTable
            Dim SB As New StringBuilder()

            dt = objCommon.FillDataTable(sqlConn, "Id_RPT_DMATBuyingBricsSec", Val(lblDealSlipID), "DealSlipID")
            If dt IsNot Nothing Then
                Subject = "Deal  " & Trim(dt.Rows(0).Item("DealSlipNo") & "")

                SB.Append("<table cellpadding='2' cellspacing='0' align='left' style='width: 100%; border-top: 0px; border-left: 0px; border-right: 0; border-bottom: 0px ;font-family: Verdana, Helvetica, sans-serif;font-size: 0.75em;'>")
                SB.Append("<tr>")
                SB.Append("<td>")
                SB.Append("<table align='left' style='font-family: Verdana, Helvetica, sans-serif;font-size: 0.75em;'><tr><td>")
                SB.Append("Dear Sir/Madam")
                SB.Append("<br/><br/>")
                SB.Append("Pls approve the  deal  " + Trim(dt.Rows(0).Item("DealSlipNo") & "") + " Entered by " + Trim(dt.Rows(0).Item("CounterParty") & ""))
                SB.Append("<br/><br/>")
                If dt.Rows(0).Item("TransType").ToString().ToUpper() = "S" Then
                    SB.Append("Our OutRight Sell")
                Else
                    SB.Append("Our OutRight Buy")
                End If
                SB.Append("</td></tr></table>")
                SB.Append("</td></tr>")

                SB.Append("<tr>")
                SB.Append("<td>")
                SB.Append("<table cellpadding='2' cellspacing='0' align='left' style='width: 500px; border-top: 0px; border-left: 0px; border-right: 1px solid #999999; border-bottom: 1px solid #999999;'>")
                If dt.Rows(0).Item("TransType").ToString().ToUpper() = "S" Then
                    SB.Append("<tr align='left'>")
                    SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'> Counter Party Seller</td>")
                    SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("CompName") & "") + "</td>")
                    SB.Append("</tr>")

                    SB.Append("<tr align='left'>")
                    SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Counter Party Buyer</td>")
                    SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("CounterParty") & "") + "</td>")
                    SB.Append("</tr>")

                End If
                If dt.Rows(0).Item("TransType") = "P" Then
                    SB.Append("<tr align='left'>")
                    SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'> Counter Party Seller</td>")
                    SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("CounterParty") & "") + "</td>")
                    SB.Append("</tr>")

                    SB.Append("<tr align='left'>")
                    SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Counter Party Buyer</td>")
                    SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("CompName") & "") + "</td>")
                    SB.Append("</tr>")
                End If

                SB.Append("<tr align='left'>")
                SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Security Name</td>")
                SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("SecurityName") & "") + "</td>")
                SB.Append("</tr>")
                SB.Append("<tr align='left'>")
                SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Maturity Date</td>")
                SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("Redemption") & "") + "</td>")
                SB.Append("</tr>")
                SB.Append("<tr align='left'>")
                SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>IP Dates</td>")
                SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("IPDates") & "") + "</td>")
                SB.Append("</tr>")
                SB.Append("<tr align='left'>")
                SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Face Value (Rs.Lacs)</td>")
                SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("FaceValue(Rs.Lacs)") & "") + "</td>")
                SB.Append("</tr>")
                SB.Append("<tr align='left'>")
                SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Price</td>")
                SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("Price") & "") + "</td>")
                SB.Append("</tr>")
                SB.Append("<tr align='left'>")
                SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Settlement</td>")
                SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("SettmentDate") & "") + "</td>")
                SB.Append("</tr>")
                SB.Append("<tr align='left'>")
                SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Deal Date</td>")
                SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("DealDate") & "") + "</td>")
                SB.Append("</tr>")
                SB.Append("<tr align='left'>")
                SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Value Date</td>")
                SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("SettmentDate") & "") + "</td>")
                SB.Append("</tr>")
                SB.Append("<tr align='left'>")
                SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Yield</td>")
                SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("Yield") & "") + "</td>")
                SB.Append("</tr>")
                SB.Append("<tr align='left'>")
                SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>TC</td>")
                SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("SettlementAmt") & "") + "</td>")
                SB.Append("</tr>")
                'SB.Append("<tr align='left'>")
                'SB.Append("<td style='width:40%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>Deal Confirmed through</td>")
                'SB.Append("<td style='width:60%; font-family: Verdana, Helvetica, sans-serif; text-align: left; font-size: 0.75em; border-right: 0px;  border-bottom: 0px; border-top: 1px solid #999999; border-left: 1px solid #999999;'>" + Trim(dt.Rows(0).Item("ConfirmedThrough") & "") + "</td>")
                'SB.Append("</tr>")
                SB.Append("</table>")
                SB.Append("</td>")
                SB.Append("</tr>")
                SB.Append("<tr>")
                SB.Append("<td>")
                SB.Append("<br/><table style='font-family: Verdana, Helvetica, sans-serif;font-size: 0.75em;'><tr><td>Regards, <br/>" + Trim(Convert.ToString(Session("NameOfUser")) & "") + "</td></tr></table>")
                SB.Append("</td>")
                SB.Append("</tr>")
                SB.Append("</table>")

            End If
            Return SB.ToString()
        Catch ex As Exception
        End Try
    End Function
    Private Function BuildValuesInsert(ByVal strTable As String, ByVal strPKCol As String, _
                                       ByVal intPKVal As Integer, ByVal chrChkType As Char, _
                                       ByVal strRejReason As String) As String
        Dim SB As New StringBuilder
        SB.AppendLine("INSERT INTO MakerCheckerValues")
        SB.AppendLine("(TableName, PKColName, PKValue, CheckType, RejReason, CheckDateTime)")
        SB.AppendLine("VALUES")
        SB.AppendLine("('" & strTable & "','" & strPKCol & "','" & intPKVal & "','" & chrChkType & "','" & strRejReason & "', GETDATE() )")
        Return SB.ToString
    End Function
    Private Function BuildCheckerUpdate(ByVal strTable As String, ByVal strPKCol As String, _
                                        ByVal intPKVal As Integer, ByVal chrChkType As Char) As String
        Dim SB As New StringBuilder
        SB.AppendLine("ALTER TABLE " & strTable & " DISABLE TRIGGER ALL ")
        SB.AppendLine("UPDATE " & strTable)
        SB.AppendLine("SET Checked = " & IIf(chrChkType = "P", 0, 1) & ", CheckStatus = '" & chrChkType & "' ")
        SB.AppendLine("WHERE " & strPKCol & " = '" & intPKVal & "' ")
        SB.AppendLine("ALTER TABLE " & strTable & " ENABLE TRIGGER ALL ")
        Return SB.ToString
    End Function
    Private Function ExecuteDynamicQuery(ByVal strQry As String) As SqlCommand
        Dim sqlComm As New SqlCommand
        OpenConn()
        sqlComm.CommandText = "sp_executesql"
        sqlComm.CommandType = CommandType.StoredProcedure
        sqlComm.Connection = sqlConn
        SetCommandParameters(sqlComm, "@stmt", SqlDbType.NVarChar, 4000, "I", , , strQry)
        sqlComm.ExecuteNonQuery()
        Return sqlComm
    End Function
    Private Sub SetCommandParameters(ByVal oCommand As SqlCommand, ByVal Name As String, ByVal DataType As SqlDbType, ByVal size As Integer, ByVal Direction As Char, Optional ByVal Scale As Integer = 0, Optional ByVal Precision As Integer = 0, Optional ByVal oValue As Object = vbNull)
        Dim oParam As New SqlParameter
        If size = 0 Then
            oParam = oCommand.Parameters.Add(Name, DataType)
        Else
            oParam = oCommand.Parameters.Add(Name, DataType, size)
        End If
        With oParam
            If Direction = "I" Then .Direction = ParameterDirection.Input
            If Direction = "O" Then .Direction = ParameterDirection.Output
            If Direction = "IO" Then .Direction = ParameterDirection.InputOutput
            If Direction = "R" Then .Direction = ParameterDirection.ReturnValue
            If Not Scale = 0 Then .Scale = Scale
            If Not Precision = 0 Then .Precision = Precision
            If Direction = "I" Or Direction = "IO" Then .Value = oValue
        End With
    End Sub
End Class
