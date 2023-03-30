Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports Newtonsoft.Json
Imports System.Collections.Generic
Partial Class Forms_UploadAcknowledgedDeals
    Inherits System.Web.UI.Page
    Dim sqlComm As New SqlCommand
    Dim objCommon As New clsCommonFuns
    Dim param As New SqlParameter
    Dim sqlConn As SqlConnection
    Private Sub Forms_UploadAcknowledgedDeals_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
            Else
                UploadTempImage()
            End If


            Hid_UserId.Value = Val(Session("UserId") & "")
            Hid_UserTypeId.Value = Val(Session("UserTypeId") & "")

        Catch ex As Exception

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

    Protected Sub Doc2SQLServer()
        Try
            OpenConn()

            With sqlComm

                .Parameters.Clear()
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_UPLOADDealAck"
                .Connection = sqlConn
                param = New Data.SqlClient.SqlParameter("@Dealslipid", Data.SqlDbType.Int)
                param.Value = Val(srh_TransCode.SelectedId)
                .Parameters.Add(param)
                .ExecuteNonQuery()
                LabelError.Visible = True
                LabelError.Text = "File uploaded successfully."

                row_UploadDoc.Visible = False
                srh_TransCode.SelectedId = 0
                srh_TransCode.SearchTextBox.Text = ""
            End With

        Catch ex As Exception
            Response.Write(ex.Message)

        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub btn_Save_Click(sender As Object, e As EventArgs) Handles btn_Save.Click
        Try
            Doc2SQLServer()

        Catch ex As Exception

        End Try
    End Sub
    Private Sub UploadTempImage()
        Dim intLength As Integer
        Dim arrContent As Byte()
        If FilePicker.PostedFile Is Nothing Then
            LabelError.Text = "No file specified."
            LabelError.Visible = False
            Exit Sub
        Else
            Dim fileName As String = FilePicker.PostedFile.FileName
            Hid_uploadImagePath.Value = fileName
            If fileName Is Nothing Or fileName = "" Then Exit Sub
            Dim strFile As String = Right(fileName, fileName.Length - fileName.LastIndexOf("\") - 1)
            strFile = Left(strFile, strFile.LastIndexOf("."))
            Dim ext As String = fileName.Substring(fileName.LastIndexOf("."))
            ext = ext.ToLower
            Dim imgType = FilePicker.PostedFile.ContentType

            Hid_ImageContentType.Value = FilePicker.PostedFile.ContentType

            If ext = ".pdf" Then
                'ElseIf ext = ".tif" Then
                'ElseIf ext = ".bmp" Then
                'ElseIf ext = ".gif" Then
                'ElseIf ext = "jpg" Then
                'ElseIf ext = "bmp" Then
                'ElseIf ext = "gif" Then
                'ElseIf ext = "tif" Then
                'ElseIf ext = ".pdf" Then

            Else
                LabelError.Text = "Only gif, bmp, or jpg format files supported."
                Exit Sub
            End If
            intLength = Convert.ToInt32(FilePicker.PostedFile.InputStream.Length)
            ReDim arrContent(intLength)
            FilePicker.PostedFile.InputStream.Read(arrContent, 0, intLength)
            If TempDoc2SQLServer(srh_TransCode.SearchTextBox.Text, arrContent, intLength, imgType) = True Then
                LabelError.Visible = True
                LabelError.Text = "Document uploaded successfully."
            Else
                LabelError.Text = "An error occured while uploading Image... Please try again."

            End If
        End If
    End Sub
    Protected Function TempDoc2SQLServer(ByVal title As String, ByVal Content As Byte(), ByVal Length As Integer, ByVal strType As String) As Boolean
        Try
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_TEMPUPLOADIMAGE"
                param = New Data.SqlClient.SqlParameter("@content", Data.SqlDbType.Image)
                param.Value = Content
                .Parameters.Add(param)
                param = New Data.SqlClient.SqlParameter("@length", Data.SqlDbType.BigInt)
                param.Value = Length
                .Parameters.Add(param)
                param = New Data.SqlClient.SqlParameter("@title", Data.SqlDbType.VarChar)
                param.Value = title
                .Parameters.Add(param)
                param = New Data.SqlClient.SqlParameter("@type", Data.SqlDbType.VarChar)
                param.Value = strType
                .Parameters.Add(param)
                param = New Data.SqlClient.SqlParameter("@tempimgdate", Data.SqlDbType.SmallDateTime)
                param.Value = Today.Date
                .Parameters.Add(param)
                param = New Data.SqlClient.SqlParameter("@DealSlipId", Data.SqlDbType.Int)
                param.Value = Val(srh_TransCode.SelectedId)
                .Parameters.Add(param)
                .ExecuteNonQuery()
            End With
            Return True
        Catch ex As Exception
            Response.Write(ex.Message)

            Return False
        Finally
            CloseConn()
        End Try
    End Function

    Private Sub srh_TransCode_ButtonClick(sender As Object, e As EventArgs) Handles srh_TransCode.ButtonClick
        Try
            Dim dt As DataTable
            Dim dtdealAck As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_PrintDealSlip", srh_TransCode.SelectedId, "DealSlipID")
            dtdealAck = objCommon.FillDataTable(sqlConn, "ID_CHECK_AckDealExist", srh_TransCode.SelectedId, "DealSlipID")
            'If dt.Rows.Count > 0 Then
            '    Hid_DocumentDetails.Value = Trim(dt.Rows(0).Item("DocumentDetails") & "")
            'End If
            If dtdealAck.Rows.Count > 0 Then
                row_UploadDoc.Visible = False
                Dim strHtml As String
                strHtml = "alert('Sorry!!! No Records available to show report');"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                Exit Sub
            Else
                row_UploadDoc.Visible = True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            CloseConn()
        End Try
    End Sub


End Class
