Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports System.Collections.Generic
Imports System.IO
Partial Class Forms_BrokerMaster
    Inherits System.Web.UI.Page
    Dim sqlConn As New SqlConnection
    Dim objCommon As New clsCommonFuns


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
            Dim strUrlName As String = Request.QueryString("Page") & ""

            If IsPostBack = False Then
                SetAttributes()
                FillCombo()
                FillBlankDealerGrid()
                Page.SetFocus(txt_Broker)
                If Request.QueryString("Id") <> "" Then
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)

                    FillFields()
                    FillDealerDetails()
                    txt_Broker.Focus()
                    btn_Save.Visible = False
                    btn_Update.Visible = True

                End If
            Else
                'txt_AdvEntryDate.Text = Format(Now, "dd/MM/yyyy")
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
        Try
            OpenConn()
            txt_Broker.Attributes.Add("onblur", "ConvertUCase(this);")
            txt_NSEBrokerCode.Attributes.Add("onblur", "ConvertUCase(this);")
            txt_BSEBrokerCode.Attributes.Add("onblur", "ConvertUCase(this);")
            txt_BrokerAddress1.Attributes.Add("onblur", "ConvertUCase(this);")
            txt_BrokerAddress2.Attributes.Add("onblur", "ConvertUCase(this);")
            txt_BrokerCity.Attributes.Add("onblur", "ConvertUCase(this);")
            txt_PANNumber.Attributes.Add("onblur", "ConvertUCase(this);")
            btn_Save.Attributes.Add("onclick", "return Validation();")
            btn_Update.Attributes.Add("onclick", "return Validation();")
            btn_Update.Visible = False
            objCommon.FillControl(cbo_DealerName, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")

        Catch ex As Exception
        Finally
            CloseConn()

        End Try


    End Sub

    Private Sub FillCombo()
        Try
            Dim ds As New DataSet()
            ds = objComm.FillAllCombo("ID_Fill_CustomerDocumentsList")
            Hid_DocumentMaster.Value = objComm.Trim(ds.Tables(0).Rows(0)("Document").ToString())
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub FillBlankDealerGrid()
        Try
            Dim dtInfoGrid As New DataTable
            dtInfoGrid.Columns.Add("NameOFUser", GetType(String))
            dtInfoGrid.Columns.Add("StartDate", GetType(String))
            dtInfoGrid.Columns.Add("EndDate", GetType(String))
            dtInfoGrid.Columns.Add("BrokerId", GetType(String))
            dtInfoGrid.Columns.Add("BrokerDealerId", GetType(String))
            dtInfoGrid.Columns.Add("BrokerDealerDetailId", GetType(String))

            Session("BrokerDealerTable") = dtInfoGrid
            dg_Dealer.DataSource = dtInfoGrid
            dg_Dealer.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub FillFields()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable1(sqlConn, "Id_FILL_BrokerMaster", ViewState("Id"), "BrokerId")
            If dt.Rows.Count > 0 Then
                txt_Broker.Text = Trim(dt.Rows(0).Item("BrokerName") & "")
                txt_NSEBrokerCode.Text = Trim(dt.Rows(0).Item("NSEBrokerCode") & "")
                txt_BSEBrokerCode.Text = Trim(dt.Rows(0).Item("BSEBrokerCode") & "")
                txt_BrokerAddress1.Text = Trim(dt.Rows(0).Item("BrokerAddress1") & "")
                txt_BrokerAddress2.Text = Trim(dt.Rows(0).Item("BrokerAddress2") & "")
                txt_BrokerCity.Text = Trim(dt.Rows(0).Item("BrokerCity") & "")
                txt_BrokerPinCode.Text = Trim(dt.Rows(0).Item("BrokerPinCode") & "")
                txt_BrokerPhone.Text = Trim(dt.Rows(0).Item("BrokerPhone") & "")
                txt_PANNumber.Text = Trim(dt.Rows(0).Item("PANNumber") & "")
                txt_STRegNo.Text = Trim(dt.Rows(0).Item("STRegNo") & "")
                txt_Email.Text = Trim(dt.Rows(0).Item("EmailId") & "")
                txt_BrokerFax.Text = Trim(dt.Rows(0).Item("BrokerFax") & "")
                txt_GSTNo.Text = Trim(dt.Rows(0).Item("BrokerGST") & "")
                Hid_DocumentDetails.Value = Trim(dt.Rows(0).Item("DocumentDetails") & "")
                txt_AadharNo.Text = Trim(dt.Rows(0).Item("AadharNo") & "")
                txt_AccountNo.Text = Trim(dt.Rows(0).Item("AccountNo") & "")
                txt_BankName.Text = Trim(dt.Rows(0).Item("BankName") & "")
                txt_BeneficiaryName.Text = Trim(dt.Rows(0).Item("Beneficiary") & "")
                txt_Branch.Text = Trim(dt.Rows(0).Item("Branch") & "")
                txt_IFSCCode.Text = Trim(dt.Rows(0).Item("IFSCCode") & "")
            End If
            dt = objCommon.FillDetailsGrid(dg_Dealer, "ID_FILL_BrokerDealerDetails", "BrokerId", Val((ViewState("Id")) & ""))
            Session("BrokerDealerTable") = dt
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub InsertUpdateDealer(ByVal blnEdit As Boolean)
        Try
            Dim dr As DataRow
            Dim dt As DataTable
            Dim intIndex As Int16
            intIndex = Val(ViewState("RowIndex") & "")
            dt = Session("BrokerDealerTable")
            Dim view As New DataView(dt)
            view.Sort = "BrokerDealerId"

            If blnEdit = False Then
                For I As Int16 = 0 To dt.Rows.Count - 1
                    If (Val(dt.Rows(I).Item("BrokerDealerId"))) = Val(cbo_DealerName.SelectedValue) Then
                        'record exists
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Dealer Already Exists');", True)
                        Exit Sub
                    End If
                Next
                dr = dt.NewRow
            Else
                dr = dt.Rows(intIndex)
            End If
            dr.Item("NameOfUser") = Trim(cbo_DealerName.SelectedItem.Text)

            If txt_StartDate.Text <> "" Then
                dr.Item("StartDate") = Format(objCommon.DateFormat(txt_StartDate.Text), "dd/MM/yyyy")
            End If

            If txt_EndDate.Text <> "" Then
                dr.Item("EndDate") = Format(objCommon.DateFormat(txt_EndDate.Text), "dd/MM/yyyy")
            End If

            dr.Item("BrokerDealerId") = Val(cbo_DealerName.SelectedValue)
            dr.Item("BrokerDealerDetailId") = 0
            dr.Item("BrokerId") = Val(ViewState("Id"))
            'dt.Rows.Add(dr)

            If blnEdit = False Then dt.Rows.Add(dr)

            DeleteBlankRow(dt, "NameOfUser")
            cbo_DealerName.SelectedIndex = 0
            cbo_DealerName.SelectedItem.Text = ""
            txt_StartDate.Text = ""
            txt_EndDate.Text = ""
            dg_Dealer.DataSource = dt
            dg_Dealer.DataBind()
            Session("BrokerDealerTable") = dt
            ViewState("DealerEditFlag") = False




        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub FillDealerDetails()
        Try
            Dim dt As DataTable
            If TypeOf Session("BrokerDealerTable") Is DataTable Then
                dt = Session("BrokerDealerTable")
            Else
                dt = objCommon.FillDetailsGrid(dg_Dealer, "ID_FILL_BrokerDealerDetails", "BrokerId", Val((ViewState("Id")) & ""))
            End If
            Session("BrokerDealerTable") = dt

            dg_Dealer.DataSource = dt
            dg_Dealer.DataBind()
        Catch ex As Exception

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub



    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            If CBool(objCommon.CheckDuplicate(sqlConn, "BrokerMaster", "BrokerName", txt_Broker.Text)) = False Then
                Dim msg As String = "This Broker Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_BrokerMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    
    End Sub

    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            'sqlTrans = clsCommonFuns.sqlConn.BeginTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            If SaveDealers(sqlTrans) = False Then Exit Sub
            If SaveKycDocuments(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("BrokerDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            'CHANGE 
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn

            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@BrokerName", SqlDbType.VarChar, 200, "I", , , Trim(txt_Broker.Text))
            objCommon.SetCommandParameters(sqlComm, "@NSEBrokerCode", SqlDbType.VarChar, 200, "I", , , Trim(txt_NSEBrokerCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@BSEBrokerCode", SqlDbType.VarChar, 200, "I", , , Trim(txt_BSEBrokerCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@BrokerAddress1", SqlDbType.VarChar, 100, "I", , , Trim(txt_BrokerAddress1.Text))
            objCommon.SetCommandParameters(sqlComm, "@BrokerAddress2", SqlDbType.VarChar, 100, "I", , , Trim(txt_BrokerAddress2.Text))
            objCommon.SetCommandParameters(sqlComm, "@BrokerCity", SqlDbType.VarChar, 20, "I", , , Trim(txt_BrokerCity.Text))
            objCommon.SetCommandParameters(sqlComm, "@BrokerPinCode", SqlDbType.VarChar, 15, "I", , , Trim(txt_BrokerPinCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@BrokerPhone", SqlDbType.VarChar, 15, "I", , , Trim(txt_BrokerPhone.Text))
            objCommon.SetCommandParameters(sqlComm, "@BrokerFax", SqlDbType.VarChar, 15, "I", , , Trim(txt_BrokerFax.Text))
            objCommon.SetCommandParameters(sqlComm, "@PANNumber", SqlDbType.VarChar, 15, "I", , , Trim(txt_PANNumber.Text))
            objCommon.SetCommandParameters(sqlComm, "@STRegNo", SqlDbType.VarChar, 20, "I", , , Trim(txt_STRegNo.Text))
            'objCommon.SetCommandParameters(sqlComm, "@ExchangeType ", SqlDbType.Char, 1, "I", , , rbl_Exchange.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , Trim(txt_Email.Text))
            objCommon.SetCommandParameters(sqlComm, "@BrokerGST", SqlDbType.VarChar, 100, "I", , , Trim(txt_GSTNo.Text))

            objCommon.SetCommandParameters(sqlComm, "@AadharNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_AadharNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@BankName", SqlDbType.VarChar, 200, "I", , , Trim(txt_BankName.Text))
            objCommon.SetCommandParameters(sqlComm, "@IFSCCode", SqlDbType.VarChar, 50, "I", , , Trim(txt_IFSCCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@Branch", SqlDbType.VarChar, 100, "I", , , Trim(txt_Branch.Text))
            objCommon.SetCommandParameters(sqlComm, "@Beneficiary", SqlDbType.VarChar, 500, "I", , , Trim(txt_BeneficiaryName.Text))
            objCommon.SetCommandParameters(sqlComm, "@AccountNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_AccountNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@BrokerId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function SaveDealers(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable


            dt = Session("BrokerDealerTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = ""
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("NameOfUser").ToString <> "" Then
                    sqlComm.Parameters.Clear()

                    If dg_Dealer.Items(I).FindControl("imgBtn_Delete").Visible = True Then
                        sqlComm.CommandText = "ID_INSERT_BrokerDealerDetails"
                        objCommon.SetCommandParameters(sqlComm, "@BrokerDealerDetailId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_BrokerDealerDetails"
                        objCommon.SetCommandParameters(sqlComm, "@BrokerDealerDetailId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("BrokerDealerDetailId"))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.BigInt, 8, "I", , , Val(ViewState("Id")))
                    If dt.Rows(I).Item("StartDate") & "" <> "" Then
                        objCommon.SetCommandParameters(sqlComm, "@StartDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(dt.Rows(I).Item("StartDate")))
                    End If
                    If dt.Rows(I).Item("EndDate") & "" <> "" Then
                        objCommon.SetCommandParameters(sqlComm, "@EndDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(dt.Rows(I).Item("EndDate")))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@BrokerDealerId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("BrokerDealerId"))
                    objCommon.SetCommandParameters(sqlComm, "@EnteredBy", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                End If


                sqlComm.ExecuteNonQuery()
                Hid_DealeretailId.Value += Val(sqlComm.Parameters.Item("@BrokerDealerDetailId").Value & "") & "!"
                Hid_DealeretailId.Value += "!"

            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Sub DeleteBlankRow(ByRef dt As DataTable, ByVal strColName As String)
        Try
            For I As Int16 = 0 To dt.Rows.Count - 1
                If String.IsNullOrEmpty(dt.Rows(I).Item(strColName).ToString()) Then
                    dt.Rows.RemoveAt(I)
                    Exit Sub
                End If
            Next
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "BrokerMaster", "BrokerName", Trim(txt_Broker.Text), "BrokerId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Broker Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_BrokrMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("BrokerDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("BrokerDetail.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            'Session("AdvisoryDetails") = ""
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub dg_Dealer_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles dg_Dealer.ItemCommand
        Try
            Dim imgBtn As ImageButton
            Dim dgItem As DataGridItem
            Dim dr As DataRow
            Dim dt As DataTable

            dt = CType(Session("BrokerDealerTable"), DataTable)
            If e.CommandName = "Edit" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                dgItem = e.Item
                e.Item.BackColor = Drawing.Color.FromName("#CCE3FF")
                ViewState("RowIndex") = dgItem.DataSetIndex
                ViewState("DealerEditFlag") = True
                dr = dt.Rows(dgItem.DataSetIndex)
                cbo_DealerName.SelectedValue = dr.Item("BrokerDealerId")
                If dr.Item("StartDate").ToString <> "" Then
                    txt_StartDate.Text = Trim(dr.Item("StartDate") & "")
                End If

                If dr.Item("StartDate").ToString <> "" Then
                    txt_EndDate.Text = Trim(dr.Item("EndDate") & "")
                End If
            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("BrokerDealerTable") = dt
            FillDealerDetails()
        Catch ex As Exception

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub Add_Dealer_Click(sender As Object, e As EventArgs) Handles Add_Dealer.Click
        Try
            InsertUpdateDealer(ViewState("DealerEditFlag"))

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function SaveKycDocuments(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim lstParam As List(Of SqlParameter) = New List(Of SqlParameter)
            Dim blnReturn As Boolean = False
            Dim dtDocumnet As DataTable = New DataTable
            Dim filename As String = ""
            Dim ext As String = ""
            Dim contenttype As String = ""
            If Val(ViewState("Id") & "") = 0 Then
                lstParam.Add(New SqlParameter("@BrokerId", 0))
            Else
                lstParam.Add(New SqlParameter("@BrokerId", ViewState("Id")))
            End If
            dtDocumnet.Columns.Add("Id", GetType(Int32))
            dtDocumnet.Columns.Add("DocumentId", GetType(Int32))
            dtDocumnet.Columns.Add("FileName", GetType(String))
            dtDocumnet.Columns.Add("FileData", GetType(System.Byte()))
            dtDocumnet.Columns.Add("FileType", GetType(String))
            dtDocumnet.Columns.Add("FileExtension", GetType(String))
            Dim Id() As String = Hid_BrokerDocumentId.Value.Split(Microsoft.VisualBasic.ChrW(44))
            Dim DocumentId() As String = Hid_DocumentId.Value.Split(Microsoft.VisualBasic.ChrW(44))
            If (Hid_BrokerDocumentId.Value <> "") Then
                Dim i As Integer = 0
                Do While (i < Id.Length)
                    Dim PostedFile As HttpPostedFile = Request.Files(i)
                    Dim datarow As DataRow = dtDocumnet.NewRow
                    datarow("Id") = objComm.Val(Id(i))
                    datarow("DocumentId") = objComm.Val(DocumentId(i))
                    If (PostedFile.ContentLength > 0) Then
                        filename = PostedFile.FileName
                        filename = Path.GetFileName(filename)
                        ext = Path.GetExtension(filename)
                        contenttype = PostedFile.ContentType
                        Dim fs As Stream = PostedFile.InputStream
                        Dim br As BinaryReader = New BinaryReader(fs)
                        Dim bytes() As Byte = br.ReadBytes(CType(fs.Length, Int32))
                        datarow("FileName") = filename
                        datarow("FileData") = bytes
                        datarow("FileType") = contenttype
                        datarow("FileExtension") = ext
                    End If

                    dtDocumnet.Rows.Add(datarow)
                    i = (i + 1)
                Loop
            End If

            lstParam.Add(New SqlParameter("@tblDocumentDetails", dtDocumnet))
            If objComm.InsertUpdateDetails(lstParam, "ID_InsertUpdate_BrokerDocumentsDetails") > 0 Then
                blnReturn = True
            End If

            Return blnReturn
        Catch ex As Exception

            sqlTrans.Rollback()
            objCommon.InsertError(ex.Message.ToString(), "InsertUpdate_Error")

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try


    End Function
End Class
