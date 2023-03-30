Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_SelectFields
    Inherits System.Web.UI.Page
    Dim sqlConn As SqlConnection
    Dim objCommon As New clsCommonFuns
    Dim intGridCols As Int16

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")

            Hid_PageName.Value = Request.QueryString("strPage")
            If Hid_PageName.Value = "debitpaymentreceipt.aspx" Then
                chkList_Select.Attributes.Add("onclick", "CheckOnlyOne(event);")
            End If

            If IsPostBack = False Then
                btn_Close.Attributes.Add("onclick", "return Close();")
                btn_Submit.Attributes.Add("onclick", "return ReturnValues();")
                btn_Search.Attributes.Add("onclick", "return ValidateSearch();")
                btn_Insert.Attributes.Add("onclick", "return ValidateInsert();")
                btn_Remove.Attributes.Add("onclick", "return ValidateRemove();")
                FillList()
                FillSelectedList()
                
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function BuildCondition() As String
        Try
            Dim strCond As String = ""
            Dim strOption As String = " WHERE "
            Dim blnCondExist As Boolean
            Dim strCondFldName As String = Trim(Request.QueryString("CondFieldName") & "")
            Dim strCondFldValue As String = Trim(Request.QueryString("CondFieldValue") & "")
            Dim strCondFldName1 As String = Trim(Request.QueryString("CondFieldName1") & "")
            Dim strCondFldName2 As String = Trim(Request.QueryString("CondFieldName2") & "")
            Dim strCondFldValue1 As String = Trim(Request.QueryString("CondFieldValue1") & "")
            Dim strCondFldValue2 As String = Trim(Request.QueryString("CondFieldValue2") & "")
            Dim strProcName As String = Trim(Request.QueryString("ProcName") & "")


            If Request.QueryString("CondExist") = "true" Then blnCondExist = True
            If blnCondExist = True Then strOption = " AND "
            'If Trim(Request.QueryString("CheckYearComp") & "") = "true" Then
            '    strCond += strOption & " DSE.CompId = " & Val(Session("CompId") & "") & " AND DSE.YearId = " & Val(Session("YearId") & "")

            'End If
            If strCondFldName = "DSE.RetDealType" And strCondFldValue <> "B" Then
                strCond += strOption & strCondFldName & " in ('T','D','F','M','O')"
            ElseIf strCondFldName <> "" And strCondFldValue <> "" Then
                strCond += strOption & strCondFldName & " = '" & strCondFldValue & "' "
            End If
            If strProcName = "ID_SEARCH_Distributor" Then
                If strCondFldValue = 1 Then
                    strCond = ""
                Else
                    strCond = "Where BD.BrokerDealerId =" & strCondFldValue1
                End If
            End If

            'If strCondFldName <> "" And strCondFldValue <> "" Then
            '    strCond += strOption & strCondFldName & " = '" & strCondFldValue & "' "
            'End If
            If Trim(txt_Name.Text) <> "" And Request.QueryString("SelectedFieldName") = "RefNoText" Then
                strCond = strOption & Request.QueryString("SelectedFieldName") & " LIKE '%" & txt_Name.Text & "%'"

            ElseIf Trim(txt_Name.Text) <> "" And Request.QueryString("SelectedFieldName") = "RefNo" Then
                strCond = strOption & Request.QueryString("SelectedFieldName") & " LIKE '%" & txt_Name.Text & "%'"

                'new change
            ElseIf Trim(txt_Name.Text) <> "" And Request.QueryString("SelectedFieldName") = "CustomerName" Then
                strCond = strOption & Request.QueryString("SelectedFieldName") & " LIKE '%" & txt_Name.Text & "%'"
                '


            ElseIf Trim(txt_Name.Text) <> "" Then
                'If blnCondExist = False Then
                '    strOption = " AND "
                '    strCond = strOption & Request.QueryString("SelectedFieldName") & " LIKE '" & txt_Name.Text & "%'"
                'Else
                strCond = strOption & Request.QueryString("SelectedFieldName") & " LIKE '" & txt_Name.Text & "%'"
            End If

            'If strCondFldName <> "" And strCondFldValue <> "" Then
            '    strCond += strOption & strCondFldName & " = '" & strCondFldValue & "' "
            'End If
            If Trim(Request.QueryString("CheckYearComp") & "") = "true" Then
                strCond += strOption & " DSE.CompId = " & Val(Session("CompId") & "") & " AND DSE.YearId = " & Val(Session("YearId") & "")

            End If
            If strCondFldValue1 <> "" And strCondFldValue2 <> "" Then
                If strProcName = "ID_SEARCH_RetailDebitRefNo" Then
                    strCond += strOption & "DSE.FromDate >= " & "Convert(smalldatetime," & "'" & strCondFldValue1 & "'" & ",103)" & " and DSE.FromDate <= " & "Convert(smalldatetime," & "'" & strCondFldValue2 & "'" & ",103)"

                Else
                    strCond += strOption & "DSE.DealDate >= " & "Convert(smalldatetime," & "'" & strCondFldValue1 & "'" & ",103)" & " and DSE.DealDate <= " & "Convert(smalldatetime," & "'" & strCondFldValue2 & "'" & ",103)"
                End If
            End If

            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Search.Click
        FillList()
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

    Private Sub FillList()
        Try
            Dim dt As DataTable
            Dim objSrh As New clsSearch
            Dim strColList As String
            OpenConn()
            If Trim(Request.QueryString("ShowAll") & "") = "true" Or txt_Name.Text <> "" Then
                strColList = Trim(Request.QueryString("SelectedFieldName") & "") & "," & Trim(Request.QueryString("SelectedValueName") & "")
            Else
                strColList = " TOP 10 " & Trim(Request.QueryString("SelectedFieldName") & "") & "," & Trim(Request.QueryString("SelectedValueName") & "")
            End If

            If (Trim(Request.QueryString("ProcName") & "") = "MB_SEARCH_IssueDistributor") Then
                dt = objCommon.FillDataTable(sqlConn, Trim(Request.QueryString("ProcName") & ""))
            Else
                dt = objSrh.FillDataTable(sqlConn, Trim(Request.QueryString("ProcName") & ""), strColList, BuildCondition())
            End If


            Dim strSelFldName As String = Trim(Request.QueryString("SelectedFieldName") & "")
            Dim strSelFldValue As String = Trim(Request.QueryString("SelectedValueName") & "")

            strSelFldName = Right(strSelFldName, strSelFldName.Length - strSelFldName.IndexOf(".") - 1)
            strSelFldValue = Right(strSelFldValue, strSelFldValue.Length - strSelFldValue.IndexOf(".") - 1)
            Dim dv As DataView
            dv = dt.DefaultView
            dv.Sort = Trim(Request.QueryString("SelectedFieldName") & "") & " ASC "
            chkList_Select.DataSource = dv
            chkList_Select.DataTextField = strSelFldName
            chkList_Select.DataValueField = strSelFldValue
            chkList_Select.DataBind()
            Dim lstItem As ListItem = chkList_Select.Items.FindByText("")
            If lstItem IsNot Nothing Then chkList_Select.Items.Remove(lstItem)
            Hid_RowCount.Value = chkList_Select.Items.Count
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Private Sub FillSelectedList()
        Try
            If IsPostBack = False Then
                Dim arrSelTexts() As String
                Dim arrSelValues() As String
                arrSelTexts = Split(Trim(Request.QueryString("SelectedTexts") & "").Replace("^", "&"), "!")
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
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

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
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

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