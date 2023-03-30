Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_AddAccYear
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As SqlConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            If IsPostBack = False Then
                Page.SetFocus(txt_AccYear)
                SetAttributes()
                FillDetails()
            End If
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

    Private Sub CloseConn()
        If sqlConn Is Nothing Then Exit Sub
        If sqlConn.State = ConnectionState.Open Then sqlConn.Close()
    End Sub
    Private Sub SetAttributes()
        btn_AddAccYear.Attributes.Add("onclick", "return Validation()")
        txt_StartDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_StartDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_EndDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_EndDate.Attributes.Add("onblur", "CheckDate(this,false);")
    End Sub
    Protected Sub btn_AddAccYear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddAccYear.Click
        SetSaveUpdate("ID_INSERT_YearMaster")
        FillDetails()

    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("AddAccYear.aspx?Id=" & ViewState("Id"), False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@YearText", SqlDbType.VarChar, 20, "I", , , Trim(txt_AccYear.Text))
            objCommon.SetCommandParameters(sqlComm, "@StartDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_StartDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@EndDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_EndDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
            objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.BigInt, 8, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@YearId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub FillDetails()
        Try
            Dim datStart As Date
            Dim datEnd As Date
            Dim dsFill As DataSet
            Dim intYear As Int16

            dsFill = objCommon.GetDataSet(SqlDataSourceYear)
            If dsFill.Tables(0).Rows.Count = 0 Then
                If Today.Month >= 4 Then
                    datStart = DateSerial(Today.Year, 4, 1)
                    datEnd = DateSerial(Today.Year + 1, 3, 31)
                    txt_AccYear.Text = Today.Year & "-" & (Today.Year + 1)
                Else
                    datStart = DateSerial(Today.Year - 1, 4, 1)
                    datEnd = DateSerial(Today.Year, 3, 31)
                    txt_AccYear.Text = (Today.Year - 1) & "-" & Today.Year
                End If
            Else
                With dsFill.Tables(0).Rows(dsFill.Tables(0).Rows.Count - 1)
                    intYear = Year(.Item("EndDate"))
                    datStart = DateSerial(intYear, 4, 1)
                    datEnd = DateSerial(intYear + 1, 3, 31)
                    txt_AccYear.Text = intYear & "-" & (intYear + 1)
                End With
            End If
            txt_StartDate.Text = Format(datStart, "dd/MM/yyyy")
            txt_EndDate.Text = Format(datEnd, "dd/MM/yyyy")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
