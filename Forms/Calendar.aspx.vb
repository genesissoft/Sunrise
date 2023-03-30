Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Partial Class Forms_Calendar
    Inherits System.Web.UI.Page
    Dim list As New List(Of DateTime)()
    Dim a As Int16
    Dim sqlConn As New SqlConnection
    Dim objCommon As New clsCommonFuns


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Populate_MonthddList()
            Populate_YearddList()
            SetAttributes()

            FillList()
            Session("SelectedDates") = ""
            Calendar1.NextMonthText = ""
            Calendar1.PrevMonthText = ""
        End If
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

    Private Sub CloseConn()
        If sqlConn Is Nothing Then Exit Sub
        If sqlConn.State = ConnectionState.Open Then sqlConn.Close()
    End Sub
    Private Sub SetAttributes()
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_AddDate.Attributes.Add("onclick", "return ValidationAdd();")
        btn_Removedate.Attributes.Add("onclick", "return ValidationRemove();")
    End Sub

    Protected Sub Cbo_Month_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Month.SelectedIndexChanged
        Try
            Calendar1.TodaysDate = Convert.ToDateTime(Cbo_Month.SelectedItem.Value + " 1, " + Cbo_Year.SelectedItem.Value)
            For J As Integer = 0 To lst_Datelist.Items.Count - 1
                Dim selDate As DateTime
                selDate = objCommon.DateFormat(lst_Datelist.Items(J).Value)
                Calendar1.SelectedDates.Add(selDate)
            Next
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Cbo_Year_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Year.SelectedIndexChanged
        Try
            Calendar1.TodaysDate = Convert.ToDateTime(Cbo_Month.SelectedItem.Value + " 1, " + Cbo_Year.SelectedItem.Value)
            For J As Integer = 0 To lst_Datelist.Items.Count - 1
                Dim selDate As DateTime
                selDate = objCommon.DateFormat(lst_Datelist.Items(J).Value)
                Calendar1.SelectedDates.Add(selDate)
            Next
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub Populate_MonthddList()

        Cbo_Month.Items.Add("January")
        Cbo_Month.Items.Add("February")
        Cbo_Month.Items.Add("March")
        Cbo_Month.Items.Add("April")
        Cbo_Month.Items.Add("May")
        Cbo_Month.Items.Add("June")
        Cbo_Month.Items.Add("July")
        Cbo_Month.Items.Add("August")
        Cbo_Month.Items.Add("September")
        Cbo_Month.Items.Add("October")
        Cbo_Month.Items.Add("November")
        Cbo_Month.Items.Add("December")

        Cbo_Month.Items.FindByValue(MonthName(DateTime.Now.Month)).Selected = True

    End Sub


    Private Sub Populate_YearddList()

        Dim ArrYear As Array
        ArrYear = Split(Session("YearText"), "-")
        For i As Integer = 0 To ArrYear.Length - 1
            Cbo_Year.Items.Add(ArrYear(i))
        Next
    End Sub

    Protected Sub Calendar1_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles Calendar1.DayRender
        Try
            Hid_Date.Value = e.Day.IsSelected
            If e.Day.IsSelected = True Then
                list.Add(e.Day.[Date])
            End If
            If (e.Day.IsWeekend) Then
                e.Day.IsSelectable = False
                e.Cell.Text = e.Day.DayNumberText
            End If
            If (e.Day.IsOtherMonth) Then
                e.Day.IsSelectable = False
                e.Cell.Text = e.Day.DayNumberText
            End If
            Session("SelectedDates") = list
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        Try
            If Session("SelectedDates") IsNot Nothing Then
                Dim newList As List(Of DateTime) = DirectCast(Session("SelectedDates"), List(Of DateTime))
                For Each dt As DateTime In newList
                    Calendar1.SelectedDates.Add(dt)
                Next
                list.Clear()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_AddDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddDate.Click
        Try
            If Validtion() = True Then
                Exit Sub
            End If
            If Session("SelectedDates") IsNot Nothing Then
                Dim newList As List(Of DateTime) = DirectCast(Session("SelectedDates"), List(Of DateTime))
                For Each dt As DateTime In newList
                    'Response.Write(dt.ToShortDateString() & "<BR/>")
                    If lst_Datelist.Items.FindByText(dt.ToString("dd/MM/yyyy")) Is Nothing Then
                        lst_Datelist.Items.Add(dt.ToString("dd/MM/yyyy"))
                    End If
                Next
            End If
            Calendar1.SelectedDates.Clear()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Removedate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Removedate.Click
        Try
            Calendar1.SelectedDates.Remove(objCommon.DateFormat(lst_Datelist.SelectedValue))
            lst_Datelist.Items.RemoveAt(lst_Datelist.SelectedIndex)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            SetSaveUpdate("ID_INSERT_HolidayMaster")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            'sqlTrans = sqlConn.BeginTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If Delete(sqlTrans) = False Then Exit Sub
            If SaveHoliday(sqlTrans) = False Then Exit Sub
            If SaveSatSun(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            lit_msg.Text = "Holidays assigned successfuly"
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function SaveHoliday(ByRef sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim strdate As String
            Dim I As Int16
            Dim LstItem As ListItem
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandText = "ID_INSERT_HolidayMaster"
            For I = 0 To lst_Datelist.Items.Count - 1
                LstItem = lst_Datelist.Items(I)
                strdate = Trim(LstItem.Value)
                GetMonth()
                sqlComm.Parameters.Clear()
                With sqlComm
                    objCommon.SetCommandParameters(sqlComm, "@HolidayDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(strdate))
                    objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                    objCommon.SetCommandParameters(sqlComm, "@Month", SqlDbType.SmallInt, 4, "I", , , Hid_Month.Value)
                    objCommon.SetCommandParameters(sqlComm, "@Year", SqlDbType.SmallInt, 4, "I", , , Cbo_Year.SelectedValue)
                    objCommon.SetCommandParameters(sqlComm, "@SatSunflag", SqlDbType.Bit, 4, "I", , , 0)
                    objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@HolidayId", SqlDbType.Int, 4, "O")
                End With
                sqlComm.ExecuteNonQuery()
                Hid_HolidayId.Value = Val(sqlComm.Parameters.Item("@HolidayId").Value & "")
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Return False
        End Try
    End Function
    Private Function SaveSatSun(ByRef sqlTrans As SqlTransaction) As Boolean
        Try
            Dim StartDate As Date
            Dim EndDate As Date
            Dim startyr As Integer
            Dim endyr As Integer
            startyr = Left(Session("YearText"), 4)
            endyr = Right(Session("YearText"), 4)
            StartDate = DateSerial(startyr, 4, 1)
            EndDate = DateSerial(endyr, 3, 31)
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandText = "ID_INSERT_SatSunHolidayMaster"

            sqlComm.Parameters.Clear()
            With sqlComm
                objCommon.SetCommandParameters(sqlComm, "@StartDate", SqlDbType.SmallDateTime, 4, "I", , , (StartDate))
                objCommon.SetCommandParameters(sqlComm, "@EndDate", SqlDbType.SmallDateTime, 4, "I", , , (EndDate))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                objCommon.SetCommandParameters(sqlComm, "@SatSunflag", SqlDbType.Bit, 4, "I", , , 1)
                objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                objCommon.SetCommandParameters(sqlComm, "@HolidayId", SqlDbType.Int, 4, "O")
            End With
            sqlComm.ExecuteNonQuery()
            Hid_HolidayId.Value = Val(sqlComm.Parameters.Item("@HolidayId").Value & "")

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Return False
        End Try
    End Function
    Private Function Delete(ByRef osqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .Transaction = osqlTrans
                .CommandText = "ID_DELETE_Holidaymaster"
                .Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
            End With
            Return True
        Catch ex As Exception
            osqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Return False
        End Try
    End Function
    Private Sub FillList()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dtFill As New DataTable
            Dim dvFill As DataView

            GetMonth()
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_HolidayMaster"
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
                sqlDa.SelectCommand = sqlComm
                sqlDa.Fill(dtFill)
                dvFill = dtFill.DefaultView
            End With
            'sqldr = sqlComm.ExecuteReader()
            lst_Datelist.DataTextField = "HolidayDate"
            lst_Datelist.DataValueField = "HolidayDate"
            lst_Datelist.DataSource = dvFill
            lst_Datelist.DataBind()
            'sqldr.Close()
            For J As Integer = 0 To lst_Datelist.Items.Count - 1
                Dim selDate As DateTime
                selDate = objCommon.DateFormat(lst_Datelist.Items(J).Value)
                Calendar1.SelectedDates.Add(selDate)
            Next
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()

        End Try
    End Sub
    Private Sub GetMonth()
        Try
            If Cbo_Month.SelectedValue = "January" Then
                Hid_Month.Value = "1"
            ElseIf Cbo_Month.SelectedValue = "February" Then
                Hid_Month.Value = "2"
            ElseIf Cbo_Month.SelectedValue = "March" Then
                Hid_Month.Value = "3"
            ElseIf Cbo_Month.SelectedValue = "April" Then
                Hid_Month.Value = "4"
            ElseIf Cbo_Month.SelectedValue = "May" Then
                Hid_Month.Value = "5"
            ElseIf Cbo_Month.SelectedValue = "June" Then
                Hid_Month.Value = "6"
            ElseIf Cbo_Month.SelectedValue = "July" Then
                Hid_Month.Value = "7"
            ElseIf Cbo_Month.SelectedValue = "August" Then
                Hid_Month.Value = "8"
            ElseIf Cbo_Month.SelectedValue = "September" Then
                Hid_Month.Value = "9"
            ElseIf Cbo_Month.SelectedValue = "October" Then
                Hid_Month.Value = "10"
            ElseIf Cbo_Month.SelectedValue = "November" Then
                Hid_Month.Value = "11"
            ElseIf Cbo_Month.SelectedValue = "December" Then
                Hid_Month.Value = "12"
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Calendar1_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles Calendar1.VisibleMonthChanged

    End Sub
    Private Function Validtion()
        Try
            'If Cbo_Year.SelectedIndex = 0 Then
            '    If Cbo_Month.SelectedIndex <= 2 Then
            '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Please select Month greater than April month');", True)
            '        Calendar1.SelectedDates.Clear()
            '        Return True
            '    End If
            'End If
            'If Cbo_Year.SelectedIndex = 1 Then
            '    If Cbo_Month.SelectedIndex >= 3 Then
            '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Please select Month less than April month');", True)
            '        Calendar1.SelectedDates.Clear()
            '        Return True
            '    End If
            'End If
            Return False
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class