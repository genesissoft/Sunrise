Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DealPurchaseshow
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As New SqlConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            ' btn_Showall.Visible = False
            If IsPostBack = False Then
                Session("DealPurchaseDetails") = ""
                SetAttributes()
                SetControls()
                FillGrid()
                If Trim(Request.QueryString("DealTransType") & "") <> "" Then
                    Hid_DealTransType.Value = Trim(Request.QueryString("DealTransType") & "")
                End If
                If Val(Request.QueryString("SecurityId") & "") <> 0 Then
                    Hid_SecurityId.Value = Val(Request.QueryString("SecurityId") & "")
                End If
                If Trim(Request.QueryString("Facevalue") & "") <> "" Then
                    Hid_Facevalue.Value = Trim(Request.QueryString("Facevalue") & "")
                End If
                If Trim(Request.QueryString("facevaluemultiple") & "") <> "" Then
                    Hid_facevaluemultiple.Value = Trim(Request.QueryString("facevaluemultiple") & "")
                End If

                Hid_RemainFV.Value = Val(Hid_Facevalue.Value) * (Hid_facevaluemultiple.Value)
                FillListWithoutDealslipid()

                SavePurdeal()



            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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

        'btn_Add.Attributes.Add("onclick", "return  ValidationAdd();")
        'btn_Save.Attributes.Add("onclick", "return  Validation();")
        lbl_Msg.Attributes.Add("style", "color:green")
    End Sub

    Private Sub SetControls()
        Try
            OpenConn()

            'srh_DealSlipNo.Columns.Add("DealSlipNo")
            'srh_DealSlipNo.Columns.Add("FinancialDealType")
            'srh_DealSlipNo.Columns.Add("Cm.CustomerName")
            'srh_DealSlipNo.Columns.Add("SM.SecurityName")
            'srh_DealSlipNo.Columns.Add("CONVERT(varchar,DealDate,103) As DealDate")
            'srh_DealSlipNo.Columns.Add("CONVERT(VARCHAR,SettmentDate,103) As SettlementDate ")
            'srh_DealSlipNo.Columns.Add("Rate")
            'srh_DealSlipNo.Columns.Add("RemainingFaceValue")
            'srh_DealSlipNo.Columns.Add("CONVERT(SMALLDATETIME,DealDate,103) As DealDateTime")
            'srh_DealSlipNo.Columns.Add("DealSlipID")


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "')", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click


        'Dim dt As DataTable
        'Dim dv As DataView
        'dt = TryCast(Session("DealPurchaseDetails"), DataTable)
        'dv = dt.DefaultView
        'dv.Sort = "DealSlipNo ASC"
        'Hid_DealSlipIds.Value = ""
        'Hid_DealSlipNos.Value = ""
        'Hid_RemainingFaceValue.Value = ""
        'dg_Purdealno.DataSource = dv
        'dg_Purdealno.DataBind()
        Delete()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "close", "RetValues();", True)
    End Sub


    Private Sub FillListWithoutDealslipid()
        Try
            OpenConn()
            Dim strRetValues() As String
            Dim dt As New DataTable
            Dim arrRemFaceValues() As String
            Dim arrDealSlipNo() As String

            If Trim(Request.QueryString("strReturn").ToString) <> "!!!" Then
                strRetValues = Split(Request.QueryString("strReturn"), "!")
                Hid_PurchaseDealSlipId.Value = Trim(strRetValues(0))
                Hid_DealSlipNo.Value = Trim(strRetValues(1))
                arrDealSlipNo = strRetValues(1).Split(",")
                arrRemFaceValues = strRetValues(2).Split(",")
                dt = objCommon.FillDataTable(sqlConn, "ID_FILL_DealPurchaseDetailsGridNew", , , , Mid(Hid_PurchaseDealSlipId.Value, 1, Hid_PurchaseDealSlipId.Value.Length - 1), "DealSlipIds")
                For I As Int16 = 0 To dt.Rows.Count - 1
                    For J As Int16 = 0 To arrDealSlipNo.Length - 1
                        If arrDealSlipNo(J) = dt.Rows(I).Item("DealSlipNo") Then
                            dt.Rows(I).Item("RemainingFaceValue") = arrRemFaceValues(J)
                            Exit For
                        End If
                    Next
                Next
                dg_Purdealno.DataSource = dt
                dg_Purdealno.DataBind()

                Session("DealPurchaseDetails") = dt
                If Hid_PurchaseDealSlipId.Value = "" Then
                    btn_Showall.Visible = True
                Else
                    btn_Showall.Visible = False
                End If
                FillAmount()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillGridWithDealslipid()    
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDetailsGrid(dg_Purdealno, "ID_FILL_DealPurchaseDetails", "SellDealSlipId", Val(Hid_dealslipId.Value))
            Session("DealPurchaseDetails") = dt
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub btn_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Try
            OpenConn()
            FillGrid()
            SavePurdeal()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillGrid()
        Try
            OpenConn()
            Dim dt As DataTable
            Dim dtnew As New DataTable
            Dim dr As DataRow
            Dim i As Integer
            Dim RemainAmt As Integer = 0

            Dim dgItem As DataGridItem
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_DealPurchaseDetailsGrid", , , , Val(srh_DealSlipNo.SelectedId), "DealSlipIds")

            If (TypeOf Session("DealPurchaseDetails") Is DataTable) = False Then
                dtnew = dt.Clone
            Else
                dtnew = Session("DealPurchaseDetails")
                For i = 0 To dg_Purdealno.Items.Count - 1
                    RemainAmt = Val(CType(dg_Purdealno.Items(i).FindControl("txt_RemainingFaceValue"), TextBox).Text)
                    dtnew.Rows(i).Item("RemainingFaceValue") = RemainAmt
                Next
                dtnew = Session("DealPurchaseDetails")
            End If
            If (dt.Rows.Count > 0) Then
                If Hid_DealSlipNos.Value.IndexOf(dt.Rows(0).Item("DealSlipNo")) <> -1 Then
                    Dim strHtml As String
                    strHtml = "alert('The Slip No Already Exist');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "msg", strHtml, True)
                    Exit Sub
                End If
            End If
            If dt.Rows.Count > 0 Then
                dr = dtnew.NewRow
                dr.Item("DealSlipNo") = Trim(dt.Rows(0).Item("DealSlipNo"))
                dr.Item("RemainingFaceValue") = Val(dt.Rows(0).Item("RemainingFaceValue"))
                dr.Item("FV") = Val(dt.Rows(0).Item("FV"))
              
                dr.Item("CustomerName") = Trim(dt.Rows(0).Item("CustomerName"))
                dr.Item("DealDate") = dt.Rows(0).Item("DealDate")
                dr.Item("SettlementDate") = dt.Rows(0).Item("SettlementDate")
                dr.Item("rate") = dt.Rows(0).Item("rate")
                dr.Item("DealSlipID") = Val(dt.Rows(0).Item("DealSlipID"))
                dtnew.Rows.Add(dr)
            End If
            Session("DealPurchaseDetails") = dtnew
            Hid_DealSlipIds.Value = ""
            Hid_DealSlipNos.Value = ""
            Hid_RemainingFaceValue.Value = ""
            dg_Purdealno.DataSource = dtnew
            dg_Purdealno.DataBind()
            srh_DealSlipNo.SearchTextBox.Text = ""
            FillAmount()


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillAmount()
        Try
            OpenConn()
            Dim i As Integer
            Dim dgItem As DataGridItem
            Dim dealslipNo As String
            Dim RemainAmt As Integer = 0
            For i = 0 To dg_Purdealno.Items.Count - 1
                dgItem = dg_Purdealno.Items(i)
                RemainAmt = RemainAmt + Val(CType(dg_Purdealno.Items(i).FindControl("txt_RemainingFaceValue"), TextBox).Text)
                dealslipNo = Trim(CType(dg_Purdealno.Items(i).FindControl("lnk_DealSlipNo"), LinkButton).Text)
            Next
            If RemainAmt = 0 Then
                txt_Totremainamt.Text = 0
            Else
                txt_Totremainamt.Text = RemainAmt
            End If

            If Val(txt_Totremainamt.Text) = 0 Then
                txt_Balamt.Text = 0
            Else

                txt_Balamt.Text = Math.Round((Val(Hid_Facevalue.Value) * Val(Hid_facevaluemultiple.Value)), 4) - Val(txt_Totremainamt.Text)
            End If


            If (Val(txt_Totremainamt.Text) > Math.Round((Val(Hid_Facevalue.Value) * Val(Hid_facevaluemultiple.Value)), 4)) Then
                txt_Balamt.Text = 0
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub dg_Purdealno_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Purdealno.ItemCommand
        Try

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Hid_PdealslipId.Value = Val(CType(e.Item.FindControl("lbl_DealSlipId"), Label).Text)
            End If
            Dim dt As DataTable

            dt = CType(Session("DealPurchaseDetails"), DataTable)

            If e.CommandName = "delete" Then
                Dim intabc As Int16 = e.Item.ItemIndex
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            If e.CommandName = "RemoveMarking" Then
                OpenConn()
                Dim sqlTrans As SqlTransaction
                sqlTrans = sqlConn.BeginTransaction
                If DeleteMarking(sqlTrans) = False Then Exit Sub
                sqlTrans.Commit()
                Dim intabc As Int16 = e.Item.ItemIndex
                dt.Rows.RemoveAt(e.Item.ItemIndex)
                Delete()
                CloseConn()
                lbl_Msg.Text = "Marking removed successfully"
            End If
            Session("DealPurchaseDetails") = dt
            Hid_DealSlipIds.Value = ""
            Hid_DealSlipNos.Value = ""
            Hid_RemainingFaceValue.Value = ""
            dg_Purdealno.DataSource = dt
            dg_Purdealno.DataBind()

            'DeletePurchaseId()
            FillAmount()


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Purdealno_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Purdealno.ItemDataBound
        Try
            Dim lnkbutton As LinkButton
            Dim intDealSlipId As Integer
            Dim strDealSlipNo As String
            Dim RemButton As Button
            Dim dr As DataRow
            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                dr = TryCast(e.Item.DataItem, DataRowView).Row
                Hid_DealSlipIds.Value += CType(e.Item.DataItem, DataRowView).Row.Item("DealSlipID") & ","
                Hid_DealSlipNos.Value += CType(e.Item.DataItem, DataRowView).Row.Item("DealSlipNo") & ","
                Hid_RemainingFaceValue.Value += CType(e.Item.FindControl("txt_RemainingFaceValue"), TextBox).Text & ","
                'Hid_RemainingFaceValue.Value += CType(e.Item.DataItem, DataRowView).Row.Item("RemainingFaceValue") & ","
            End If

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                intDealSlipId = Val(CType(e.Item.FindControl("lbl_DealSlipId"), Label).Text)
                strDealSlipNo = Trim(CType(e.Item.FindControl("lnk_DealSlipNo"), LinkButton).Text)
                lnkbutton = CType(e.Item.FindControl("lnk_DealSlipNo"), LinkButton)
                RemButton = CType(e.Item.FindControl("lbl_Remove"), Button)
                Hid_ShowId.Value = HttpUtility.HtmlEncode(objCommon.EncryptText(intDealSlipId))
                lnkbutton.Attributes.Add("onclick", "return ShowPurDeal('" & e.Item.ItemIndex & "','" & intDealSlipId & "');")
            End If
            Delete()
            SavePurdeal()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Private Function SavePurdeal()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            'Hid_ContactDetailId.Value = ""
            dt = Session("DealPurchaseDetails")
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_TempPurdeal"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("DealSlipID").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.BigInt, 8, "I", , , dt.Rows(I).Item("DealSlipID"))
                    objCommon.SetCommandParameters(sqlComm, "@DealPurchaseDetailsId", SqlDbType.BigInt, 8, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                Else
                End If
            Next
            Return True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function
    Private Function Delete()
        Dim sqlComm As New SqlCommand
        Try
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.Text
            sqlComm.CommandText = "delete from TempPurdeal"
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function

    Protected Sub btn_Showall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Showall.Click

        Try
            OpenConn()
            FillPurchaseGrid()
            btn_Showall.Visible = False
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Delete()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "close1", "FinalClose();", True)
    End Sub


    Private Sub FillPurchaseGrid()
        Try
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView
            Dim dt As DataTable
            Dim sqlComm As New SqlCommand
            OpenConn()
            'If TypeOf Session("DealPurchaseDetails") Is DataTable Then
            '    sqldt = Session("DealPurchaseDetails")
            'Else
            Session("DealPurchaseDetails") = ""
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_FILL_DealPurchaseFullDetailsGrid1"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@Securityid", SqlDbType.Int, 4, "I", , , Val(Hid_SecurityId.Value))
            If Hid_DealTransType.Value = "T" Or Hid_DealTransType.Value = "D" Then
                objCommon.SetCommandParameters(sqlComm, "@Dealtranstype", SqlDbType.Char, 1, "I", , , "T")
                objCommon.SetCommandParameters(sqlComm, "@Dealtranstype1", SqlDbType.Char, 1, "I", , , "D")
            Else
                objCommon.SetCommandParameters(sqlComm, "@Dealtranstype", SqlDbType.Char, 1, "I", , , Trim(Hid_DealTransType.Value))
                objCommon.SetCommandParameters(sqlComm, "@Dealtranstype1", SqlDbType.Char, 1, "I", , , DBNull.Value)
            End If
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@ret_code", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)
            'End If
            Session("DealPurchaseDetails") = sqldt
            dg_Purdealno.DataSource = Session("DealPurchaseDetails")
            dg_Purdealno.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub DeletePurchaseId()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_AddPurchaseId"
            sqlComm.Parameters.Clear()

            objCommon.SetCommandParameters(sqlComm, "@PurchaseDealslipId", SqlDbType.Int, 4, "I", , , Val(Hid_PdealslipId.Value))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function DeleteMarking(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_DELETE_MultiRefDealId"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()

            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("DealslipId") & ""))
            objCommon.SetCommandParameters(sqlComm, "@PurchaseDealSlipId", SqlDbType.Int, 4, "I", , , Hid_PdealslipId.Value)
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True

        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
