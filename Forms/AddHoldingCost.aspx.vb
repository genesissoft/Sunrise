Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_AddHoldingCost
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As New SqlConnection

    Dim dtHoldingRate As New DataTable
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
                SetAttributes()
                dtHoldingRate = Session("HoldingCost")
                If dtHoldingRate.Rows.Count > 0 Then
                    dg_dme.DataSource = dtHoldingRate
                    dg_dme.DataBind()
                End If
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
        btn_SaveInfo.Attributes.Add("onclick", "return close();")
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_SaveInfo.Click
        Try
            OpenConn()
            SetSaveUpdate("ID_INSERT_HoldingCostDebtPerformance")

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "close", "RetValues();", True)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            SaveUpdate()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub


    Private Function SaveUpdate()
        Try
            'CHANGE 
            Dim dtData As New DataTable()
            dtData.Columns.Add("FromDate", GetType(Date))
            dtData.Columns.Add("Holdingrate", GetType(Decimal))
            For K As Int32 = 0 To dg_dme.Rows.Count - 1
                Dim FromDate As Date = objCommon.DateFormat(CType(dg_dme.Rows(K).FindControl("lbl_ValueDate"), Label).Text)
                Dim Holdingrate As Decimal = Convert.ToDecimal(CType(dg_dme.Rows(K).FindControl("txt_HoldingRate"), TextBox).Text)
                Dim datarow As DataRow = dtData.NewRow()
                datarow("FromDate") = FromDate
                datarow("Holdingrate") = Holdingrate
                dtData.Rows.Add(datarow)
            Next
            If dtData.Rows.Count > 0 Then
                Dim sqlComm As New SqlCommand
                OpenConn()
                sqlComm.CommandType = CommandType.StoredProcedure
                sqlComm.Connection = sqlConn
                sqlComm.CommandText = "ID_INSERT_HoldingCostDebtPerformance"
                sqlComm.Parameters.Clear()
                sqlComm.Parameters.Add("@InsertHoldingCost", SqlDbType.Structured).Value = dtData
                sqlComm.ExecuteNonQuery()

                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "close();", True)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
End Class
