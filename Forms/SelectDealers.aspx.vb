Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_SelectDealers
    Inherits System.Web.UI.Page

    Dim objCommon As New clsCommonFuns
    Dim intGridCols As Int16
    Dim sqlConn As SqlConnection

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
                btn_Submit.Attributes.Add("onclick", "return ReturnValues();")
                btn_Close.Attributes.Add("onclick", "return Close();")
                btn_Search.Attributes.Add("onclick", "return ValidateSearch();")
                btn_Insert.Attributes.Add("onclick", "return ValidateInsert();")
                btn_Remove.Attributes.Add("onclick", "return ValidateRemove();")
                FillList()
                FillSelectedList()

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
            chkList_Select.DataTextField = Trim(Request.QueryString("SelectedFieldName") & "")
            chkList_Select.DataValueField = Trim(Request.QueryString("SelectedValueName") & "")
            chkList_Select.DataBind()
            Dim lstItem As ListItem = chkList_Select.Items.FindByText("")
            If lstItem IsNot Nothing Then chkList_Select.Items.Remove(lstItem)
            Hid_RowCount.Value = chkList_Select.Items.Count
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