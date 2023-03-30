Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_AssignChecker
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                RSetTimeCombos()
                SetControls()
                SetAttributes()
                btn_Reassign.Visible = False
                btn_Cancel.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SetControls()
        objCommon.FillControl(cbo_UserName, clsCommonFuns.sqlConn, "Id_FILL_UserChecker", "NameOfUser", "UserId")
    End Sub
    Private Sub SetAttributes()
        txt_CheckerDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_CheckerDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_CheckerDate.Attributes.Add("onblur", "CheckDate(this,false);")
        btn_Assign.Attributes.Add("onclick", "return  Validation();")
        txt_CheckerDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
    End Sub
    Private Sub RSetTimeCombos()
        Try
            Dim i As Integer
            'Hid_txtHr.Value = cbo_hr.SelectedValue
            'Hid_txtMin.Value = cbo_minute.SelectedValue
            cbo_hr.Items.Clear()
            cbo_minute.Items.Clear()
            For i = 1 To 12
                If i < 10 Then
                    'cbo_hr.Items.Add(New ListItem(Trim("0" & i), Val(i)))
                    cbo_hr.Items.Add(New ListItem(Trim("0" & i), Trim("0" & i)))
                Else
                    cbo_hr.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next
            For i = 0 To 59 Step 15
                If i < 10 Then
                    cbo_minute.Items.Add(New ListItem(Trim("0" & i), Val("0" & i)))
                Else
                    cbo_minute.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Assign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Assign.Click

        SetSaveUpdate("ID_UPDATE_UserChecker")
        If (Hid_Cancel.Value = "cancel") Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "cancelmsg", "alert('Temporary Checker has been cancelled.')", True)
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Savemsg", "alert('Temporary Checker has been Assigned.')", True)
        End If



        clearfields()

    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try

            If SaveUpdate(strProc) = False Then Exit Sub
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function SaveUpdate(ByVal strProc As String) As Boolean
        Try
            Dim datCheckerTime As DateTime
            Dim datCheckerDate As Date
            Dim CheckerDateTime As DateTime
            Dim sqlComm As New SqlCommand
            datCheckerDate = objCommon.DateFormat(txt_CheckerDate.Text)
            datCheckerTime = DateAndTime.TimeSerial(cbo_hr.SelectedValue, cbo_minute.SelectedValue, 0)
            If datCheckerTime <> Date.MinValue Then
                CheckerDateTime = datCheckerDate & " " & Format(datCheckerTime, "hh:mm:ss") & " " & cbo_ampm.SelectedValue
            End If
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = clsCommonFuns.sqlConn
            If (Hid_Cancel.value = "cancel") Then
                objCommon.SetCommandParameters(sqlComm, "@CheckerValidDatetime", SqlDbType.DateTime, 8, "I", , , DBNull.Value)
            Else
                objCommon.SetCommandParameters(sqlComm, "@CheckerValidDatetime", SqlDbType.DateTime, 8, "I", , , CheckerDateTime)
            End If
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(cbo_UserName.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub CheckAssign()
        Try
            Dim dt As DataTable
            dt = objCommon.FillDataTable(clsCommonFuns.sqlConn, "Id_FILL_UserMasterNew", cbo_UserName.SelectedValue, "UserId")
            If dt.Rows.Count > 0 Then
                Hid_UserName.Value = Trim(dt.Rows(0).Item("UserName") & "")
                Hid_NameOfUser.Value = Trim(dt.Rows(0).Item("NameOfUser") & "")
                Hid_Checker.Value = Trim(dt.Rows(0).Item("Checker") & "")
                Hid_CheckerValidDatetime.Value = Trim(dt.Rows(0).Item("CheckerValidDatetime") & "")
                lit_prevdatetime.Text = Trim(dt.Rows(0).Item("CheckerValidDatetime") & "")
            End If

            'If Hid_CheckerValidDatetime.Value <> "" Then
            '    Return False
            'Else
            '    Return True
            'End If

            If lit_prevdatetime.Text <> "" Then
                btn_Reassign.Visible = True
                btn_Cancel.Visible = True
                btn_Assign.Visible = False
                row_assigndatetime.Visible = True
            Else
                btn_Reassign.Visible = False
                btn_Cancel.Visible = False
                btn_Assign.Visible = True
                row_assigndatetime.Visible = False
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub cbo_UserName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_UserName.SelectedIndexChanged
        CheckAssign()
        'cbo_UserName.SelectedValue

        FillCheckerValidDatetime()
        Dim dtTimeMinDate As DateTime
        dtTimeMinDate = #12/31/9999#
        Dim dtDate As Date = Left(Hid_CheckerValidDatetime.Value, 10)
        If (dtDate = dtTimeMinDate) Then
            Dim msg As String = "Permanent checker assign for this user"
            Dim strHtml As String
            strHtml = "alert('" + msg + "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)

        End If
    End Sub
    Protected Sub btn_Reassign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Reassign.Click
        btn_Assign_Click(sender, e)
        clearfields()
    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Hid_Cancel.value = "cancel"
        btn_Assign_Click(sender, e)
        clearfields()
    End Sub
    Private Sub clearfields()
        cbo_UserName.SelectedIndex = 0
        RSetTimeCombos()
        lit_prevdatetime.Text = ""
        btn_Assign.Visible = True
        btn_Reassign.Visible = False
        btn_Cancel.Visible = False
        Hid_Cancel.Value = ""
    End Sub

    Private Sub FillCheckerValidDatetime()
        Try
            Dim dt As DataTable
            'Dim intFaceQty As Integer
            'Dim dblFaceMultiple As Double

            dt = objCommon.FillDataTable(clsCommonFuns.sqlConn, "Id_FILL_UserChecker", cbo_UserName.SelectedValue, "UserId")
            If dt.Rows.Count > 0 Then
                Hid_CheckerValidDatetime.Value = Trim(dt.Rows(0).Item("CheckerValidDatetime").ToString)

            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub



End Class
