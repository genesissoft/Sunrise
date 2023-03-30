Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.ExceptionManagement

Partial Class Forms_SelectionList
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim I As Int32
    Dim errorPage As String = Convert.ToString(ConfigurationManager.AppSettings("errorPageUrl"))
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Val(Session("UserId") & "") = 0 Then
            '    Response.Redirect("Login.aspx", False)
            '    Exit Sub
            'End If
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            If Trim(Session("UserTypeSection") & "") <> "F" Then
                objCommon.SetPageTitle(Page, Trim(Session("CompName") & ""), Trim(Session("YearText") & ""))
            End If

            If IsPostBack = False Then

                If Trim(Request.QueryString("rowIndex") & "") <> "" Then
                    Hid_CustomerId.Value = Trim(Request.QueryString("rowIndex") & "")
                End If

                btn_SelectAll.Attributes.Add("onclick", "SelectList(true)")
                btn_UnselectAll.Attributes.Add("onclick", "SelectList(false)")
                btn_Close.Attributes.Add("onclick", "Close()")
                btn_Ok.Attributes.Add("onclick", "return Validate()")
                'btn_Sumbit.Attributes.Add("onclick", "return Sumbit()")
                btn_Up.Attributes.Add("onclick", "return MoveItems('UP')")
                btn_Down.Attributes.Add("onclick", "return MoveItems('DOWN')")
                Hid_Form.Value = Trim(Request.QueryString("Form") & "")
                FillList()
                SelectFields()
                EnblDiblBtns(True)
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

    Private Sub SelectFields()
        Try
            Dim strSelected() As String
            Dim J As Int32
            strSelected = Split(Trim(Request.QueryString("SelectedValues") & ""), ",")
            For I = 0 To chkList_Select.Items.Count - 1
                For J = 0 To strSelected.Length - 1
                    If chkList_Select.Items(I).Value = Val(strSelected(J)) Then
                        chkList_Select.Items(I).Selected = True
                        Exit For
                    End If
                Next
            Next
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Sub

    Protected Sub btn_Ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Ok.Click
        Try
            EnblDiblBtns(False)
            FillOrderList()
            lst_Order.SelectedIndex = 0
            lst_Order.Focus()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Sub

    Private Sub EnblDiblBtns(ByVal blnFlag As Boolean)
        Try
            '  btn_Ok.Enabled = blnFlag
            btn_Sumbit.Enabled = Not blnFlag
            btn_Up.Enabled = Not blnFlag
            btn_Down.Enabled = Not blnFlag
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Sub

    Private Sub FillList()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillControl(chkList_Select, sqlConn, "ID_SELECT_FaxFields", "FaxField", "FieldId")
            chkList_Select.Items.RemoveAt(0)
            Hid_RowCount.Value = chkList_Select.Items.Count
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        Finally
            CloseConn()
        End Try

    End Sub

    Private Sub FillOrderList()
        Try
            lst_Order.Items.Clear()
            For I = 0 To chkList_Select.Items.Count - 1
                With chkList_Select.Items(I)
                    If .Selected = True Then
                        lst_Order.Items.Add(New ListItem(Trim(.Text), Val(.Value)))
                    End If
                End With
            Next
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class