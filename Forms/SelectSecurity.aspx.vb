Imports System.Data
Imports System.Data.SqlClient

Partial Class Forms_SelectSecurity
    Inherits System.Web.UI.Page

    Dim objCommon As New clsCommonFuns
    Dim intGridCols As Int16
    Dim sqlConn As SqlConnection
    Dim objUtil As New Util
    Dim PgName As String = "$SelectSecurity$"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            'If Val(Session("UserId") & "") = 0 Then
            '    Response.Redirect("Login.aspx", False)
            '    Exit Sub
            'End If
            'objCommon.OpenConn()
            If IsPostBack = False Then
                btn_Sumbit.Attributes.Add("onclick", "return ReturnValues();")
                btn_Close.Attributes.Add("onclick", "return Close();")
                btn_Search.Attributes.Add("onclick", "return ValidateSearch();")
                btn_Insert.Attributes.Add("onclick", "return ValidateInsert();")
                btn_Remove.Attributes.Add("onclick", "return ValidateRemove();")
                FillList()
                FillSelectedList()
                GetDataTableValues()
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub GetDataTableValues()
        Try
            Dim dt As DataTable
            Dim dr As DataRow
            Dim dtNew As DataTable
            dt = Session("TempSecurityTable")
            dtNew = dt.Copy()

            If dtNew.Rows.Count > 0 Then
                dtNew.DefaultView.Sort = "OrderId Asc,CreditRating Asc"
                dtNew = dtNew.DefaultView.ToTable()
                dr = dtNew.Rows(Hid_RowIndex.Value)
                Hid_SecurityId.Value = (dr("SecurityId") & "")
                Hid_SecurityName.Value = (dr("SecurityName") & "")
                'txt_Date.Text = (dr.Item("Date") & "")
                'txt_LotSize.Text = (dr.Item("LotSize") & "")
                'txt_Rate.Text = (dr.Item("Rate") & "")
                'txt_RatingRemark.Text = (dr.Item("RatingRemark") & "")
                'rdo_Days.SelectedValue = (dr.Item("Days") & "")
                'rdo_DaysOptions.SelectedValue = (dr.Item("DaysOptions") & "")
                'rdo_IPCalc.SelectedValue = (dr.Item("IPCalc") & "")
                'rdo_PhysicalDMAT.SelectedValue = (dr.Item("PhysicalDMAT") & "")
                'rdo_RateActual.SelectedValue = (dr.Item("RateActual") & "")
                'rdo_YXM.SelectedValue = (dr.Item("YXM") & "")
                Hid_SecurityTypeId.Value = (dr("SecurityTypeId") & "")
                Hid_OrderId.Value = (dr("OrderId") & "")
                Hid_CreditRating.Value = Val(dr("CreditRating") & "")
                Hid_Semi_Ann_Flag.Value = (dr("Semi_Ann_Flag") & "")
                Hid_Semi_Ann_Flag.Value = (dr("Semi_Ann_Flag") & "")
                Hid_CombineIPMat.Value = (dr("CombineIPMat") & "")
                Hid_Rate_Actual_Flag.Value = (dr("Rate_Actual_Flag") & "")
                Hid_Equal_Actual_Flag.Value = (dr("Equal_Actual_Flag") & "")
                Hid_IntDays.Value = (dr("IntDays") & "")
                Hid_FirstYrAllYr.Value = (dr("FirstYrAllYr") & "")
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetDataTableValues", "Error in GetDataTableValues", "", ex)
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

    Private Function BuildCondition() As String
        Try
            Dim strCond As String = ""
            If Trim(txt_Name.Text) <> "" Then
                strCond = " WHERE " & Request.QueryString("SelectedFieldName") & " LIKE '" & txt_Name.Text & "%'"
            End If
            Return strCond
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Search.Click
        FillList()
    End Sub

    Private Sub FillList()
        Try
            Dim dt As DataTable
            Dim objSrh As New clsSearch
            Dim strColList As String
            OpenConn()
            strColList = " TOP 10 " & Trim(Request.QueryString("SelectedFieldName") & "") & "," & Trim(Request.QueryString("SelectedValueName") & "")
            dt = objSrh.FillDataTable(sqlConn, Trim(Request.QueryString("ProcName") & ""), strColList, BuildCondition())

            chkList_Select.DataSource = dt
            chkList_Select.DataTextField = Trim(Request.QueryString("SelectedFieldName") & "").Replace("SM.", "")
            chkList_Select.DataValueField = Trim(Request.QueryString("SelectedValueName") & "").Replace("SM.", "")
            chkList_Select.DataBind()
            Dim lstItem As ListItem = chkList_Select.Items.FindByText("")
            If lstItem IsNot Nothing Then chkList_Select.Items.Remove(lstItem)
            'Hid_RowCount.Value = chkList_Select.Items.Count
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillSelectedList()
        Try
            If IsPostBack = False Then
                Dim arrSelTexts() As String
                Dim arrSelValues() As String
                arrSelTexts = Split(Trim(Request.QueryString("SelectedTexts") & ""), "!")
                arrSelValues = Split(Trim(Request.QueryString("SelectedValues") & ""), "!")
                For I As Int16 = 0 To arrSelTexts.Length - 1
                    If Trim(arrSelTexts(I)) <> "" Then
                        lst_Name.Items.Add(New ListItem(arrSelTexts(I), arrSelValues(I)))
                    End If
                Next
            Else
                For I As Int16 = 0 To chkList_Select.Items.Count - 1
                    If chkList_Select.Items(I).Selected = True Then
                        Dim lstItem As ListItem = lst_Name.Items.FindByValue(chkList_Select.Items(I).Value)
                        If lstItem Is Nothing Then
                            lst_Name.Items.Add(New ListItem(chkList_Select.Items(I).Text, chkList_Select.Items(I).Value))
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Public Sub btn_Sumbit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Sumbit.Click
        SubmitData(False)
    End Sub

    Private Sub SubmitData(ByVal blnUpdate As Boolean)
        Dim SecurityId As String = ""
        SecurityId = Hid_SelectedSecurityIds.Value
        Dim strArrId() As String
        strArrId = SecurityId.Split("!")

    End Sub

    Private Sub SubmitData_OldLogic(ByVal blnUpdate As Boolean)
        Try
            Dim dt1 As DataTable
            Dim dt2 As DataTable
            Dim dt3 As DataTable
            dt1 = Session("TempSecurityTable")
            'If blnUpdate = True Then dt1.Rows.RemoveAt(Hid_RowIndex.Value)
            dt2 = Session("SelectedTempSecurityTable")

            dt3 = mergeDTs(dt1, dt2)
            Session("TempSecurityTable") = dt3
            Page.ClientScript.RegisterStartupScript(Me.GetType, "select", "Close();", True)

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SubmitData", "Error in SubmitData", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Public Shared Function mergeDTs(ByVal dt1 As DataTable, ByVal dt2 As DataTable) As DataTable
        Dim dtResult As DataTable = dt1.Clone()
        For Each dr As DataRow In dt1.Rows
            dtResult.Rows.Add(dr.ItemArray)
        Next
        For Each dr As DataRow In dt2.Rows
            dtResult.Rows.Add(dr.ItemArray)
        Next
        Return dtResult
    End Function

    Protected Sub btn_Insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Insert.Click
        FillSelectedList()
    End Sub

    Protected Sub btn_Remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Remove.Click
        RemoveFillRecord()
    End Sub
    Private Sub RemoveFillRecord()
        Try
            Try
                'Dim strYKKOrderNo As String
                'strYKKOrderNo = txt_YKKOrderNo.Text
                lst_Name.Items.RemoveAt(lst_Name.SelectedIndex)
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

End Class
