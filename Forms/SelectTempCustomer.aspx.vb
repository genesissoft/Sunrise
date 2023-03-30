Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_SelectTempCustomer
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If (Session("username") = "") Then
                Response.Redirect("Login.aspx")
                Exit Sub
            End If
            'objCommon.OpenConn()
            If Trim(Request.QueryString("Submit_Quote") & "") <> "" Then
                btn_Ret_Click(sender, e)
            Else
                Response.Buffer = True
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
                Response.Expires = -1500
                Response.CacheControl = "no-cache"
                Response.AddHeader("Pragma", "no-cache")
                Response.AddHeader("Cache-Control", "no-cache")
                Response.AddHeader("Cache-Control", "no-store")
                If IsPostBack = False Then
                    If Trim(Request.QueryString("Quote") & "") <> "" Then
                        btn_Ret.Attributes.Add("onclick", "return Select();")
                    Else
                        ' btn_Submit.Attributes.Add("onclick", "return Select();")
                        btn_Ret.Attributes.Add("onclick", "return Validation();")
                    End If
                    btn_Addnew.Attributes.Add("onclick", "return AddTempCust();")
                    ' btn_Submit.Attributes.Add("onclick", "return Validation();")
                    txt_Search.Attributes.Add("onblur", "ConvertUCase(this)")
                    FillGrid("CustomerId", 0)

                End If

                Dim HiddenData As String
                HiddenData = Hid_Id.Value

                Dim HdnData() As String = HiddenData.Split("|")
                Dim FirstValue = HdnData(0)

                If HiddenData <> "" Then
                    If Val(Session("Cust_Id") & "") <> 0 Then
                        SaveUpdate("ID_UPDATE_TempCustomer", Trim(HdnData(1)), Trim(HdnData(2)), Trim(HdnData(3)), Trim(HdnData(4)), Trim(HdnData(5)))
                        FillGrid("CustomerId", 0)
                        Hid_Id.Value = ""
                        Session("Cust_Id") = ""
                    Else

                        If FirstValue = "AddTempCust" Then
                            SaveUpdate("ID_INSERT_TempCustomer", Trim(HdnData(1)), Trim(HdnData(2)), Trim(HdnData(3)), Trim(HdnData(4)), Trim(HdnData(5)))
                            FillGrid("CustomerId", 0)
                            Hid_Id.Value = ""
                            Session("Cust_Id") = ""
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SaveUpdate(ByVal strProc As String, ByVal CustomerName As String, ByVal ContactPerson As String, ByVal EMailId As String, ByVal PhoneNo As String, ByVal MobileNo As String)
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            If Val(ViewState("Id") & "") = 0 And Val(Session("Cust_Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "O")
            ElseIf Val(Session("Cust_Id") & "") <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , Session("Cust_Id"))
            Else
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@CustomerName", SqlDbType.VarChar, 100, "I", , , Trim(CustomerName))
            objCommon.SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 100, "I", , , Trim(ContactPerson))
            objCommon.SetCommandParameters(sqlComm, "@PhoneNo", SqlDbType.VarChar, 50, "I", , , Trim(PhoneNo))
            objCommon.SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 50, "I", , , Trim(MobileNo))
            objCommon.SetCommandParameters(sqlComm, "@Email", SqlDbType.VarChar, 50, "I", , , Trim(EMailId))
            objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Protected Sub btn_Ret_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Ret.Click
        Try
            If Trim(Request.QueryString("Quote") & "") <> "" Then
                For i As Int16 = 0 To dg_TempCust.Items.Count - 1
                    Dim chk As CheckBox
                    chk = CType(dg_TempCust.Items(i).FindControl("chk_Select"), CheckBox)
                    If chk.Checked = True Then
                        Hid_TempCustId.Value = Val(CType(dg_TempCust.Items(i).FindControl("lbl_CustomerId"), Label).Text)
                    End If
                Next
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "return", "Close('" & Val(Hid_TempCustId.Value) & "');", True)
            Else
                Dim dt As DataTable
                Dim dr As DataRow
                dt = TryCast(Session("TempCustTable"), DataTable)
                dt.Rows.Clear()
                For i As Int16 = 0 To dg_TempCust.Items.Count - 1
                    Dim chk As CheckBox
                    dr = dt.NewRow
                    chk = CType(dg_TempCust.Items(i).FindControl("chk_Select"), CheckBox)
                    If chk.Checked = True Then
                        dr.Item("CustomerName") = Trim(CType(dg_TempCust.Items(i).FindControl("lbl_CustomerName"), Label).Text)
                        dr.Item("CustomerId") = Val(CType(dg_TempCust.Items(i).FindControl("lbl_CustomerId"), Label).Text)
                        dr.Item("ContactPerson") = Trim(CType(dg_TempCust.Items(i).FindControl("lbl_ContactPerson"), Label).Text)
                        dr.Item("FieldId") = "1,30,21,64,10,3,4,5,11,26"
                        dr.Item("CustomerCity") = ""
                        dr.Item("EmailId") = Trim(CType(dg_TempCust.Items(i).FindControl("lbl_EmailId"), Label).Text)
                        dt.Rows.Add(dr)
                        Session("TempCustTable") = dt
                    End If
                Next
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "close", "Close('Submit');", True)
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

    Private Function FillGrid(ByVal strSort As String, Optional ByVal intPageIndex As Int16 = 0)
        Try
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dtFill As New DataTable
            Dim dvFill As DataView
            OpenConn()
            sqlComm.Connection = sqlConn
            With sqlComm
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_TempCustomer"
                .Parameters.Clear()
                If cbo_Selection.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@" & cbo_Selection.SelectedItem.Text, SqlDbType.VarChar, 1000, "I", , , Trim(txt_Search.Text.Replace("'", "")))
                End If
                objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
                objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
                sqlDa.SelectCommand = sqlComm
                sqlDa.Fill(dtFill)
                Session("TempCustTable") = dtFill
                dvFill = dtFill.DefaultView
                dvFill.Sort = strSort
                dg_TempCust.CurrentPageIndex = intPageIndex
                dg_TempCust.DataSource = dvFill
                dg_TempCust.DataBind()
            End With
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Function

    Protected Sub dg_TempCust_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_TempCust.ItemCommand
        Try
            If e.CommandName = "Delete" Then
                Delete()
                FillGrid("CustomerId", 0)
            ElseIf e.CommandName = "Edit" Then
                FillGrid("CustomerId", 0)
                Dim id As Integer
                id = Val(e.CommandArgument)

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "close", "Update('" & id & "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_TempCust_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_TempCust.ItemDataBound
        Try
            Dim intTempCustId As Integer
            Dim imgbtn As ImageButton
            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                intTempCustId = Val(CType(e.Item.FindControl("lbl_CustomerId"), Label).Text)
                imgbtn = CType(e.Item.FindControl("imgBtn_Delete"), ImageButton)
                imgbtn.Attributes.Add("onclick", "return Delete('" & intTempCustId & "')")
                imgbtn = CType(e.Item.FindControl("imgBtn_Edit"), ImageButton)
                'imgbtn.Attributes.Add("onclick", "return Update('" & intTempCustId & "')")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_TempCust_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dg_TempCust.PageIndexChanged
        Try

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Addnew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Addnew.Click
        FillGrid("CustomerId", 0)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "close", "AddTempCust();", True)
    End Sub
    Private Function Delete()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn

            With sqlComm
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_DELETE_TempCustomer"
                .Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , Val(Hid_TempCustId.Value))
                objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
                objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Function




    Protected Sub btn_ShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ShowAll.Click
        cbo_Selection.SelectedIndex = 0
        txt_Search.Text = ""
        FillGrid("CustomerId", 0)
    End Sub

    Protected Sub btn_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Search.Click
        FillGrid("CustomerId", 0)
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
