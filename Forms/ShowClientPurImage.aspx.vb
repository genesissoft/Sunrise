Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_ShowClientPurImage
    Inherits System.Web.UI.Page
    Dim sqlconn As SqlConnection
    Dim objComm As New clsCommonFuns
    Dim sqlComm As New SqlCommand
    Dim CurrentPageIndex As Int16 = 0
    Dim PageSize As Int16 = 4
    Dim pages As Int16 = 0
    Dim ResultRowsFound As Int16 = 0
    Dim NavigationSize As Int16 = 5
    Dim orderNo As Int32
    Dim dtFill As DataTable
    Dim CreditNoteno As String
    Dim _showBottomPager As Boolean = True
    Dim DebitNoteNo As String
    Dim ZipCreditNoteno As String
    Dim ZipDebitNoteNo As String
    Dim trmenu As DataRow
    Dim trmenumisamt As DataRow
    Dim param As New SqlParameter
    Dim CustomerId As Int32
    Dim objCommon As New clsCommonFuns
    Dim bytHeader() As Byte
    Dim bytFooter() As Byte
    Dim strHeaderType As String
    Dim strFooterType As String
    Dim strHeaderFileName As String
    Dim strFooterFileName As String
    Dim lngHeaderLen As Long
    Dim lngFooterLen As Long
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Val(Session("UserId") & "") = 0 Then
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        btn_Delete.Attributes.Add("onclick", "return Delete_entry();")
        CustomerId = Val(Request.QueryString("CustomerId").ToString() & "")
        row_HeaderMsg.Visible = True
        If IsPostBack = False Then
            If (CustomerId = 0) Then
                FillCustomerImages()
            Else
                SaveCustImages()
                FillCustomerImages()
                DeleteCustomerMainImage()

                row_HeaderMsg.Visible = False
            End If
        End If

    End Sub
     
   
    Protected Sub imgGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles imgGrid.PageIndexChanging
        Try
            

            FillCustomerImages(e.NewPageIndex)
            row_HeaderMsg.Visible = False


        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Try
            If Request.QueryString("CustomerId".ToString() & "") <> "" Then
                DeleteCustomerImage()
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub DeleteCustomerImage()
        Try
            Dim sqlComm As New SqlCommand

            With sqlComm
                .Connection = clsCommonFuns.sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_DELETE_CustImageUpload"
                .Parameters.Clear()
                objComm.SetCommandParameters(sqlComm, "@CustImageId", SqlDbType.Int, 4, "I", , , Val(Hid_ID.Value))
                objComm.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
            End With


            FillCustomerImages()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillCustomerImages(Optional ByVal intPageIndex As Int16 = 0)
        Try
            Dim sqlDa As New SqlDataAdapter
            Dim dtFill As New DataTable
            Dim dvFill As New DataView
            sqlComm.Connection = clsCommonFuns.sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.Clear()
            sqlComm.CommandText = "FA_FILL_ShowCustImage"
            objComm.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.VarChar, 50, "I", , , CustomerId)
            'objComm.SetCommandParameters(sqlComm, "@CompanyId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            'objComm.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objComm.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()

            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtFill)
            If (dtFill.Rows.Count > 0) Then
                txt_signatory.Text = Trim(dtFill.Rows(0)("signatory"))
            End If
            dtFill.Columns.Add("imgFile")
            For Each tempRow As DataRow In dtFill.Rows
                tempRow.Item("imgFile") = ("TempImgGrab.aspx?Id=" & tempRow.Item("CustImageId"))
            Next
            If dtFill.Rows.Count > 0 Then
                Hid_ID.Value = dtFill.Rows(intPageIndex).Item("CustImageId")
                If (dtFill.Rows.Count > 0) Then
                    txt_signatory.Text = Trim(dtFill.Rows(0)("signatory"))
                End If
            Else
                btn_Delete.Visible = False
            End If
            dvFill = dtFill.DefaultView
            imgGrid.PageIndex = intPageIndex
            imgGrid.DataSource = dvFill
            imgGrid.DataBind()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
        End Try
    End Sub
    'Private Sub FillMainCustomerImages(Optional ByVal intPageIndex As Int16 = 0)
    '    Try
    '        Dim sqlDa As New SqlDataAdapter
    '        Dim dtFill As New DataTable
    '        Dim dvFill As New DataView
    '        sqlComm.Connection = clsCommonFuns.sqlConn
    '        sqlComm.CommandType = CommandType.StoredProcedure
    '        sqlComm.Parameters.Clear()
    '        sqlComm.CommandText = "ID_FILL_ShowCustImage"
    '        objComm.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.VarChar, 50, "I", , , CustomerId)
    '        'objComm.SetCommandParameters(sqlComm, "@CompanyId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
    '        'objComm.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
    '        objComm.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
    '        sqlComm.ExecuteNonQuery()

    '        sqlDa.SelectCommand = sqlComm
    '        sqlDa.Fill(dtFill)
    '        dtFill.Columns.Add("imgFile")
    '        For Each tempRow As DataRow In dtFill.Rows
    '            tempRow.Item("imgFile") = ("TempImgGrab.aspx?Id=" & tempRow.Item("CustImageId"))
    '        Next
    '        If dtFill.Rows.Count > 0 Then
    '            Hid_ID.Value = dtFill.Rows(intPageIndex).Item("CustImageId")
    '        Else
    '            btn_Delete.Visible = False
    '        End If
    '        dvFill = dtFill.DefaultView
    '        imgGrid.PageIndex = intPageIndex
    '        imgGrid.DataSource = dvFill
    '        imgGrid.DataBind()

    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    Finally
    '    End Try
    'End Sub



    'USE FOR INSERT DATA FROM MAIN TABLE TO TEMP TABLE
    Private Function SaveCustImages()
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = clsCommonFuns.sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_TempCustImageUpload"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.VarChar, 50, "I", , , CustomerId)
            'objCommon.SetCommandParameters(sqlComm, "@CompanyId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            'objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@CustImageId", SqlDbType.BigInt, 8, "O")
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()

            Hid_CustImgId.Value = Val(sqlComm.Parameters.Item("@CustImageId").Value & "")
            Return True
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

        End Try
    End Function

    Private Function UploadImage() As Boolean
        Try
            Dim strFooter() As String

            bytFooter = File_Footer.FileBytes

            If bytFooter.Length <> 0 Then
                strFooterType = File_Footer.PostedFile.ContentType
                strFooter = Split(File_Footer.PostedFile.FileName, "\")
                strFooterFileName = strFooter(UBound(strFooter))
                Hid_FooterFileName.Value = strFooterFileName
                lngFooterLen = File_Footer.PostedFile.ContentLength
                strFooter = Split(File_Footer.PostedFile.FileName, ".")
                Hid_ImageUploded.Value = 1

            End If
            Return True
            If (Hid_ImageUploded.Value = 1) Then
                row_HeaderMsg.Visible = False
            Else
                row_HeaderMsg.Visible = True

            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function SaveCustomerImage(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_tempCustImage"
            sqlComm.Connection = clsCommonFuns.sqlConn
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , CustomerId)
            objCommon.SetCommandParameters(sqlComm, "@signatory", SqlDbType.VarChar, 50, "I", , , txt_signatory.Text)
            objCommon.SetCommandParameters(sqlComm, "@FooterType", SqlDbType.VarChar, 50, "I", , , strFooterType)
            objCommon.SetCommandParameters(sqlComm, "@FooterLength", SqlDbType.Int, 4, "I", , , lngFooterLen)
            objCommon.SetCommandParameters(sqlComm, "@FooterFileName", SqlDbType.VarChar, 100, "I", , , strFooterFileName)
            objCommon.SetCommandParameters(sqlComm, "@FooterData", SqlDbType.Image, 0, "I", , , bytFooter)
            objCommon.SetCommandParameters(sqlComm, "@CustImageId", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_ShowHeader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ShowHeader.Click
        If (txt_signatory.Text = "") Then
            Dim msg As String = "Please Conform signatory Name For This Signature"
            Dim strHtml As String
            strHtml = "alert('" + msg + "');"
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
            Exit Sub
        End If

        Dim sqlTrans As SqlTransaction
        UploadImage()
        sqlTrans = clsCommonFuns.sqlConn.BeginTransaction
        If SaveCustomerImage(sqlTrans) = False Then Exit Sub
        sqlTrans.Commit()
        FillCustomerImages()
    End Sub


    Private Function DeleteCustomerMainImage()
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = clsCommonFuns.sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_MainCustImageUpload"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , CustomerId)
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
   
End Class



