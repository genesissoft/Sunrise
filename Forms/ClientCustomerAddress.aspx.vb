Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_ClientCustomerAddress
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim dt As DataTable
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")



            If IsPostBack = False Then
                'Session("MainCustAddressTable") = ""
                SetAttributes()
                Hid_CustomerId.Value = Val(Request.QueryString("CustomerId") & "")
                Hid_CustomerTypeId.Value = Val(Request.QueryString("CustomerTypeId") & "")
                Hid_CategoryId.Value = Val(Request.QueryString("CategoryId") & "")


                Hid_BusniessType.Value = Trim(Request.QueryString("BusniessType") & "")
                Hid_ProfileType.Value = Trim(Request.QueryString("ProfileType") & "")
                If Val(Hid_CustomerId.Value) = 0 Then
                    'Session("MainCustAddressTable") = ""
                End If
                FillGrid()
                If Val(Request.QueryString("CustomerId") & "") <> 0 Then
                    Hid_CustomerId.Value = Val(Request.QueryString("CustomerId") & "")
                    'FillFields()
                    btn_Save.Visible = True
                Else
                    btn_Save.Visible = True
                End If
            End If
            btn_Update.Visible = False
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
        btn_Save.Attributes.Add("onclick", "return  submitvalidation();")
        'btn_Add.Attributes.Add("onclick", "return Add();")
    End Sub
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            'SetSaveUpdate()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Close", "Close();", True)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetSaveUpdate()
        Try
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If DeleteDetails(sqlTrans) = False Then Exit Sub
            If SaveDetails(sqlTrans) = False Then Exit Sub

            sqlTrans.Commit()
            ' 'Response.Redirect("ClientProfileDetail.aspx?Id=" & ViewState("Id"), False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function DeleteDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_ClientCustMultipleAddress"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(Hid_CustomerId.Value))
            objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.VarChar, 100, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function SaveDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try

            Dim sqlComm As New SqlCommand

            Dim dgItem As DataGridItem

            Dim strCustomerBranchName As String
            Dim strCity As String
            Dim strPinCode As String
            Dim strState As String
            Dim strCountry As String
            Dim strPhone As String
            Dim strFaxNo As String
            Dim strEmailId As String
            Dim strCustomerId As Integer
            Dim strAddress1 As String
            Dim strAddress2 As String
            Dim strTempId As Integer


            Dim i As Integer

            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_ClientCustMultipleAddress"
            sqlComm.Connection = sqlConn
            For i = 0 To dg1.Items.Count - 1
                dgItem = dg1.Items(i)

                strCustomerBranchName = Trim(CType(dgItem.FindControl("lbl_CustomerBranchName"), Label).Text)
                strCity = Trim(CType(dgItem.FindControl("lbl_City"), Label).Text)
                strPinCode = Trim(CType(dgItem.FindControl("lbl_PinCode"), Label).Text)
                strState = Trim(CType(dgItem.FindControl("lbl_State"), Label).Text)
                strCountry = Trim(CType(dgItem.FindControl("lbl_Country"), Label).Text)
                strPhone = Trim(CType(dgItem.FindControl("lbl_Phone"), Label).Text)
                strFaxNo = Trim(CType(dgItem.FindControl("lbl_FaxNo"), Label).Text)
                strAddress1 = Trim(CType(dgItem.FindControl("txt_Address1"), TextBox).Text)
                strAddress2 = Trim(CType(dgItem.FindControl("txt_Address2"), TextBox).Text)
                strEmailId = Trim(CType(dgItem.FindControl("txt_EmailId"), TextBox).Text)
                strCustomerId = Val(CType(dgItem.FindControl("lbl_CustomerId"), Label).Text)
                strTempId = Val(CType(dgItem.FindControl("lbl_TempId"), Label).Text)

                sqlComm.Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@CustomerBranchName", SqlDbType.VarChar, 100, "I", , , strCustomerBranchName)
                objCommon.SetCommandParameters(sqlComm, "@City", SqlDbType.VarChar, 20, "I", , , strCity)
                objCommon.SetCommandParameters(sqlComm, "@PinCode", SqlDbType.VarChar, 15, "I", , , strPinCode)
                objCommon.SetCommandParameters(sqlComm, "@State", SqlDbType.VarChar, 100, "I", , , strState)
                objCommon.SetCommandParameters(sqlComm, "@Country", SqlDbType.VarChar, 100, "I", , , strCountry)
                objCommon.SetCommandParameters(sqlComm, "@Phone", SqlDbType.VarChar, 50, "I", , , strPhone)
                objCommon.SetCommandParameters(sqlComm, "@FaxNo", SqlDbType.VarChar, 15, "I", , , strFaxNo)
                objCommon.SetCommandParameters(sqlComm, "@Address1", SqlDbType.VarChar, 100, "I", , , strAddress1)
                objCommon.SetCommandParameters(sqlComm, "@Address2", SqlDbType.VarChar, 100, "I", , , strAddress2)
                objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , strEmailId)
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , strCustomerId)
                objCommon.SetCommandParameters(sqlComm, "@TempId", SqlDbType.Int, 4, "I", , , strTempId)
                objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "O")
                objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                sqlComm.ExecuteNonQuery()
                Hid_ClientCustAddressId.Value += Val(sqlComm.Parameters.Item("@ClientCustAddressId").Value & "") & "!"

            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub FillGrid()
        Try
            Dim ds As DataSet
            Dim dt As DataTable
            If TypeOf Session("MainCustAddressTable") Is DataTable Then
                dt = Session("MainCustAddressTable")

            Else
                ds = objCommon.GetDataSet(SqlDataSourceMainAddress)
                dt = ds.Tables(0)
            End If

            Session("MainCustAddressTable") = dt
            ClearOptions()
            dg1.DataSource = dt
            dg1.DataBind()
            HideShowColumns(dg1)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Protected Sub btn_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        AddDetails()
    End Sub
    Private Sub AddDetails()
        Try
            If Hid_RetValues.Value <> "C" Then
                Dim strRetValues() As String
                Dim dt As DataTable = New DataTable()
                Dim dr As DataRow
                Dim strRowIndex As String = Convert.ToString(Hid_RowIndex.Value)
                If strRowIndex <> "" Then
                    Dim intRowIndex As Integer = Convert.ToInt32(strRowIndex)
                    dt = Session("MainCustAddressTable")
                    If Trim(Hid_RetValues.Value & "") <> "" Then
                        strRetValues = Split(Hid_RetValues.Value, "!")
                        dt.Rows(intRowIndex).Item("CustomerBranchName") = strRetValues(0)
                        dt.Rows(intRowIndex).Item("Address1") = strRetValues(1)
                        dt.Rows(intRowIndex).Item("Address2") = strRetValues(2)
                        dt.Rows(intRowIndex).Item("City") = strRetValues(3)
                        dt.Rows(intRowIndex).Item("PinCode") = strRetValues(4)
                        dt.Rows(intRowIndex).Item("State") = strRetValues(5)
                        dt.Rows(intRowIndex).Item("Country") = strRetValues(6)
                        dt.Rows(intRowIndex).Item("Phone") = strRetValues(7)
                        dt.Rows(intRowIndex).Item("FaxNo") = strRetValues(8)
                        dt.Rows(intRowIndex).Item("EmailId") = strRetValues(9)
                        dt.Rows(intRowIndex).Item("CustomerId") = Val(Hid_CustomerId.Value)
                        dt.Rows(intRowIndex).Item("BusniessTypeIds") = (strRetValues(12))
                        dt.Rows(intRowIndex).Item("BusinessTypeNames") = Trim(strRetValues(13))
                        If strRetValues(14) <> "" Then
                            dt.Rows(intRowIndex).Item("ClientCustAddressId") = Val(strRetValues(14))
                        End If
                        dt.Rows(intRowIndex).Item("TempId") = Val(strRetValues(11))
                        Hid_TempId.Value = Val(strRetValues(11))
                    End If
                Else
                    If Trim(Hid_RetValues.Value & "") <> "" Then
                        strRetValues = Split(Hid_RetValues.Value, "!")
                        dt = Session("MainCustAddressTable")
                        dr = dt.NewRow
                        dr.Item("CustomerBranchName") = Trim(strRetValues(0) & "")
                        dr.Item("Address1") = Trim(strRetValues(1))
                        dr.Item("Address2") = Trim(strRetValues(2))
                        dr.Item("City") = Trim(strRetValues(3))
                        dr.Item("PinCode") = Trim(strRetValues(4))
                        dr.Item("State") = Trim(strRetValues(5))
                        dr.Item("Country") = Trim(strRetValues(6))
                        dr.Item("Phone") = Trim(strRetValues(7))
                        dr.Item("FaxNo") = Trim(strRetValues(8))
                        dr.Item("EmailId") = Trim(strRetValues(9))
                        dr.Item("CustomerId") = Val(strRetValues(10))
                        If strRetValues(14) <> "" Then
                            dr.Item("ClientCustAddressId") = Val(strRetValues(14))
                        Else
                            dr.Item("ClientCustAddressId") = 0
                        End If

                        dr.Item("TempId") = Val(strRetValues(11))
                        Hid_TempId.Value = Val(strRetValues(11))
                        If strRetValues(13) <> "" Then
                            dr.Item("BusinessTypeNames") = IIf(Right(strRetValues(13), 1) = ",", Left(strRetValues(13), strRetValues(13).Length - 1), strRetValues(13))
                        Else
                            dr.Item("BusinessTypeNames") = ""
                        End If

                        If strRetValues(12) <> "" Then
                            dr.Item("BusniessTypeIds") = IIf(Right(strRetValues(12), 1) = ",", Left(strRetValues(12), strRetValues(12).Length - 1), strRetValues(12))
                        Else
                            dr.Item("BusniessTypeIds") = ""
                        End If
                        dr.Item("ClientCustAddId") = DBNull.Value
                        dt.Rows.Add(dr)
                    End If
                End If
                Session("MainCustAddressTable") = dt
                FillDetails()
                Hid_RowIndex.Value = ""
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Private Sub FillDetails()
        Try
            Dim dt As DataTable
            If TypeOf Session("MainCustAddressTable") Is DataTable Then
                dt = Session("MainCustAddressTable")
            Else
                dt = objCommon.FillDetailsGrid(dg1, "Id_FILL_ClientCustMultipleAddress", "CustomerId", Val((Hid_CustomerId.Value) & ""))
            End If
            Session("MainCustAddressTable") = dt
            'ClearOptions()
            dg1.DataSource = dt
            dg1.DataBind()
            HideShowColumns(dg1)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub ClearOptions()
        Try
            Hid_BusniessType.Value = ""
            Hid_ProfileType.Value = ""
            Hid_ClientCustAddressId.Value = ""
            Hid_TempId.Value = ""

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg1.ItemCommand

        Try
            Dim strRetValues() As String
            Dim dt As DataTable


            dt = CType(Session("MainCustAddressTable"), DataTable)
            If e.CommandName = "Edit" Then
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt.Rows(e.Item.ItemIndex).Item("CustomerBranchName") = strRetValues(0)
                dt.Rows(e.Item.ItemIndex).Item("Address1") = strRetValues(1)
                dt.Rows(e.Item.ItemIndex).Item("Address2") = strRetValues(2)
                dt.Rows(e.Item.ItemIndex).Item("City") = strRetValues(3)
                dt.Rows(e.Item.ItemIndex).Item("PinCode") = strRetValues(4)
                dt.Rows(e.Item.ItemIndex).Item("State") = strRetValues(5)
                dt.Rows(e.Item.ItemIndex).Item("Country") = strRetValues(6)
                dt.Rows(e.Item.ItemIndex).Item("Phone") = strRetValues(7)
                dt.Rows(e.Item.ItemIndex).Item("FaxNo") = strRetValues(8)
                dt.Rows(e.Item.ItemIndex).Item("EmailId") = strRetValues(9)
                dt.Rows(e.Item.ItemIndex).Item("CustomerId") = Val(Hid_CustomerId.Value)
                dt.Rows(e.Item.ItemIndex).Item("BusniessTypeIds") = (strRetValues(12))
                dt.Rows(e.Item.ItemIndex).Item("BusinessTypeNames") = Trim(strRetValues(13))
                If strRetValues(14) <> "" Then
                    dt.Rows(e.Item.ItemIndex).Item("ClientCustAddressId") = Val(strRetValues(14))
                End If
                'dt.Rows(e.Item.ItemIndex).Item("TempId") = Val(strRetValues(14))
                dt.Rows(e.Item.ItemIndex).Item("TempId") = Val(strRetValues(11))
                Hid_TempId.Value = Val(strRetValues(11))
            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("MainCustAddressTable") = dt
            FillDetails()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try



    End Sub


    Protected Sub dg1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg1.ItemDataBound
        Try
            Dim img As New HtmlImage
            Dim lnkBtnEdit As ImageButton
            Dim lnkbtnDelete As ImageButton
            Dim lnkhtmlbtn As HtmlInputButton
            Dim dr As DataRow
            Dim intId As Integer
            Dim intTempId As Integer
            Hid_TempId.Value = ""

            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then


                lnkbtnDelete = CType(e.Item.FindControl("imgBtn_Delete"), ImageButton)
                lnkbtnDelete.Attributes.Add("onclick", "return Deletedetails()")
                lnkBtnEdit = CType(e.Item.FindControl("imgBtn_Edit"), ImageButton)
                lnkhtmlbtn = CType(e.Item.FindControl("imgBtn_Edit1"), HtmlInputButton)
                intId = Val(CType(e.Item.FindControl("lbl_ClientCustAddressId"), Label).Text)
                intTempId = Val(CType(e.Item.FindControl("lbl_TempId"), Label).Text)
                'lnkBtnEdit.Attributes.Add("onclick", "return UpdateDetails(this," & e.Item.ItemIndex & ")")
                'If Val(Hid_ClientCustAddressId.Value) = 0 Then
                lnkBtnEdit.Attributes.Add("onclick", "return UpdateDetails(this," & intTempId & ")")
                lnkhtmlbtn.Attributes.Add("onclick", "return UpdateDetails(this,'" & e.Item.ItemIndex & "' );")
                'Else
                'lnkBtnEdit.Attributes.Add("onclick", "return UpdateDetails(this," & intId & ")")
                'End If
                'lnkBtnEdit.Attributes.Add("onclick", "return UpdateDetails(this," & e.Item.ItemIndex & "','" & intId & "');")

                'Hid_CustBankMultiDetailId.Value += Val(CType(e.Item.FindControl("lbl_CustBankMultiDetailId"), Label).Text) & "!"
                'Hid_CustomerInsuranceId.Value += Val(CType(e.Item.FindControl("lbl_CustomerInsuranceId"), Label).Text) & "!"
                Hid_CustId.Value += Val(CType(e.Item.FindControl("lbl_CustomerId"), Label).Text) & "!"
                Hid_AddBusinessTypeId.Value += Trim(CType(e.Item.FindControl("lbl_BusniessTypeIds"), Label).Text) & "!"
                Hid_AddBusinessTypeNames.Value += Trim(CType(e.Item.FindControl("lbl_BusinessTypeNames"), TextBox).Text) & "!"
                Hid_ClientCustAddressId.Value = Val(CType(e.Item.FindControl("lbl_ClientCustAddressId"), Label).Text)
                Hid_TempId.Value = Val(CType(e.Item.FindControl("lbl_tempId"), Label).Text)

                dr = TryCast(e.Item.DataItem, DataRowView).Row
                If dr("ClientCustAddId").ToString <> "" Then
                    lnkbtnDelete.Visible = False
                End If

            End If


        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub HideShowColumns(ByVal dg As DataGrid)
        If dg Is Nothing Then
            Return
        End If
        ' Loop through all of the columns in the grid.
        For Each col As DataGridColumn In dg.Columns
            ' Hide the Salary and SS# Columns.
            If col.HeaderText = "TempId" Then
                col.Visible = False
            End If
        Next
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        btn_Save_Click(sender, e)
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
End Class
