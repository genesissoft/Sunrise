Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.ExceptionManagement

Partial Class Forms_YieldCalculation
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'objCommon.OpenConn()

        Try
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            If IsPostBack = False Then
                Hid_SecurityId.Value = Val(Request.QueryString("Id") & "")
                UC_YieldCalc.SecurityId = Val(Request.QueryString("Id") & "")
                If Request.QueryString("Sellrate") IsNot Nothing Then
                    UC_YieldCalc.TextBoxRate.Text = Val(Request.QueryString("Sellrate") & "")
                End If
                If Request.QueryString("FaceValue") IsNot Nothing Then
                    UC_YieldCalc.TextBoxFaceValue.Text = Val(Request.QueryString("FaceValue") & "")
                End If
                If Request.QueryString("FaceValueMultiple") IsNot Nothing Then
                    UC_YieldCalc.cboFaceValue.SelectedValue = Val(Request.QueryString("FaceValueMultiple") & "")
                End If
                If Request.QueryString("SettDate") IsNot Nothing Then
                    UC_YieldCalc.TextBoxYTMDate.Text = Trim(Request.QueryString("SettDate") & "")
                End If
                UC_YieldCalc.FindControl("btn_CalInterest").Visible = False
                'UC_YieldCalc.FindControl("rdo_IPCalc").Visible = False

                FillOptions()
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

    Private Sub FillOptions()
        Try
            Dim dtSecurity As DataTable
            Dim I As Int32
            OpenConn()
            Hid_SecurityId.Value = Val(Request.QueryString("Id") & "")
            Hid_StepUp.Value = Trim(Request.QueryString("StepUp") & "")
            dtSecurity = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecurityId.Value, "SecurityId")
            For I = 0 To dtSecurity.Rows.Count - 1
                With dtSecurity.Rows(I)
                    txt_Issuer.Text = Trim(.Item("SecurityIssuer") & "")
                    txt_Security.Text = Trim(.Item("SecurityName") & "")

                End With
            Next
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
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

    Protected Sub btnshowAccInerest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAccInerest.Click
        Try

            HDAcc_Rate.Value = UC_YieldCalc.TextBoxRate.Text
            HDAcc_FaceValue.Value = UC_YieldCalc.TextBoxFaceValue.Text
            HDAcc_Date.Value = UC_YieldCalc.TextBoxSettDate.Text
            HDAcc_Multiple.Value = UC_YieldCalc.cboFaceValue.SelectedValue
            If Val(HDAcc_Rate.Value) = 0 Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Please Enter Rate...');", True)
                Exit Sub
            End If
            If Val(HDAcc_FaceValue.Value) = 0 Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Please Enter Facevalue...');", True)
                Exit Sub
            End If
            If Val(Hid_SecurityId.Value) = 0 Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Please Enter Name Of Security...');", True)
                Exit Sub
            End If
            tblAccInterest.Visible = True
            FillAccruedDetails()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillAccruedDetails()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dt As New DataTable
            Dim sqldv As New DataView
            Dim RateActual As String = "R"

            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_Fill_QuoteEntry_AccruedDetails"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.Clear()

            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 4, "I", , , Val(Hid_SecurityId.Value))
            objCommon.SetCommandParameters(sqlComm, "@SettlementDate", SqlDbType.Date, 4, "I", , , objCommon.DateFormat(HDAcc_Date.Value))
            objCommon.SetCommandParameters(sqlComm, "@TotalFaceValue", SqlDbType.Decimal, 20, "I", , , Val(HDAcc_FaceValue.Value) * Val(HDAcc_Multiple.Value))
            objCommon.SetCommandParameters(sqlComm, "@StepUp", SqlDbType.Char, 1, "I", , , HDAcc_StepUp.Value)
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(dt)

            If dt.Rows.Count > 0 Then
                '    Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
                If RateActual = "R" Then
                    txt_Amount.Text = RoundToTwo(Val(HDAcc_FaceValue.Value) * Val(HDAcc_Multiple.Value) * RoundToFour(Val(HDAcc_Rate.Value)) / 100)
                    txt_AddInterest.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                Else
                    txt_Amount.Text = RoundToTwo((txt_Amount.Text + txt_AddInterest.Text) / Val(HDAcc_FaceValue.Value) * Val(HDAcc_FaceValue.Value) * Val(HDAcc_Multiple.Value))
                    txt_AddInterest.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                End If

                txt_SettAmt.Text = Val(txt_Amount.Text) + Val(txt_AddInterest.Text)
                lbl_AddInterest.Visible = True
                lbl_SettAmt.Visible = True
                If Val(txt_AddInterest.Text) = 0 Then
                    lbl_AddInterest.Text = ""
                    lbl_SettAmt.Text = ""
                End If
                If Val(dt.Rows(0).Item("AccruedDays") & "") > 0 Then
                    row_Interest.TagName = "Add Interest:"
                    lbl_AddInterest.Text = Trim(dt.Rows(0).Item("AccruedDates") & "")
                    lbl_SettAmt.Text = "(" & Val(dt.Rows(0).Item("AccruedDays") & "") & " Days)"
                    txt_AddInterest.Attributes.Add("style", "color:black")
                    lbl_SettAmt.Attributes.Add("style", "color:black")
                Else
                    row_Interest.TagName = "Less Interest:"
                    Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
                    lbl_AddInterest.Text = Trim(dt.Rows(0).Item("AccruedDates") & "")
                    lbl_SettAmt.Text = "(" & Hid_IntDays.Value & " Days)"
                    txt_AddInterest.Text = "-" & txt_AddInterest.Text
                    txt_AddInterest.Attributes.Add("style", "color:red")
                    lbl_SettAmt.Attributes.Add("style", "color:red")
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function RoundToTwo(ByVal dec As Decimal) As Decimal
        Try
            Dim rounded As Decimal = Decimal.Round(dec, 2)
            rounded = Format(rounded, "###################0.00")
            Return rounded
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function RoundToFour(ByVal dec As Decimal) As Decimal
        Try
            Dim rounded As Decimal = Decimal.Round(dec, 4)
            rounded = Format(rounded, "###################0.0000")
            Return rounded
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
End Class