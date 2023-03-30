Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.IO
Imports System.Security.Cryptography
Public Class clsCommonFuns
    Public Const DecimalPlaces As Int16 = 4
    Public Shared sqlConn As New SqlConnection
    Public Shared strRecordSelection As String
    Public Shared strSelectionClause As String
    Public Function GetRaiseEventString(ByVal DataSource As SqlDataSource) As String
        Dim ds As DataSet
        Dim strReturn As String
        ds = GetDataSet(DataSource)
        strReturn = ds.GetXml
        Return strReturn
    End Function
    Public Function DateFormatMMDDYY(ByVal strDate As String) As String
        If strDate = "" Then
            Return ""
        Else
            Return Mid(strDate, 4, 2) + "/" + Left(strDate, 2) + "/" + Right(strDate, 4)
        End If
    End Function
    Public Function GetDataSet(ByVal DataSource As SqlDataSource) As DataSet
        Dim dv As DataView
        Dim dt As New DataTable
        dv = CType(DataSource.Select(DataSourceSelectArguments.Empty), DataView)
        dt = dv.Table
        Return dt.DataSet
    End Function
    Public Function DateFormat(ByVal strDate As String) As Date
        If strDate = "" Then
            Return Date.MinValue
        Else
            Return DateSerial(Right(strDate, 4), Mid(strDate, 4, 2), Left(strDate, 2))
        End If
    End Function
    Public Function DecimalFormat4(ByVal objDecimal As Object) As Decimal

        Dim rounded As Decimal = Decimal.Round(objDecimal, 4)
        Return rounded

    End Function
    Public Function DecimalFormat2(ByVal objDecimal As Object) As Decimal

        Dim rounded As Decimal = Decimal.Round(objDecimal, 2)
        Return rounded

    End Function
    Public Function CheckDuplicateInvSch( _
         ByVal sqlConn As SqlConnection, ByVal table As String, _
         ByVal TextFieldName As String, ByVal TextFieldValue As String, _
         Optional ByVal IdFieldName As String = "", Optional ByVal IdFieldValue As Integer = 0, _
         Optional ByVal OptIdFieldName As String = "", Optional ByVal OptIdFieldValue As String = "") As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim a As Integer

            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_CHECK_InvSchDuplicates"
                .Parameters.Clear()
                If IdFieldValue <> 0 Then
                    SetCommandParameters(sqlComm, "@IdFieldName", SqlDbType.VarChar, 50, "I", , , IdFieldName)
                    SetCommandParameters(sqlComm, "@IdFieldValue", SqlDbType.BigInt, 8, "I", , , IdFieldValue)
                End If
                If OptIdFieldValue <> "" Then
                    SetCommandParameters(sqlComm, "@OptIdFieldName", SqlDbType.VarChar, 500, "I", , , OptIdFieldName)
                    SetCommandParameters(sqlComm, "@OptIdFieldValue", SqlDbType.VarChar, 500, "I", , , OptIdFieldValue)
                End If
                SetCommandParameters(sqlComm, "@TextFieldName", SqlDbType.VarChar, 50, "I", , , TextFieldName)
                SetCommandParameters(sqlComm, "@TextFieldValue", SqlDbType.VarChar, 500, "I", , , Trim(TextFieldValue))
                SetCommandParameters(sqlComm, "@TableName", SqlDbType.VarChar, 255, "I", , , table)
                SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                SetCommandParameters(sqlComm, "@Valid", SqlDbType.Bit, 1, "O")
                a = (sqlComm.Parameters("@Valid").Value)
                .ExecuteNonQuery()
            End With
            Return CBool(sqlComm.Parameters("@Valid").Value)
        Catch ex As Exception
            'objUtil.WritErrorLog(PgName, "CheckDuplicateInvSch", "Error in CheckDuplicateInvSch", "", ex)
            ''errorinfo.send_error(ex)
        Finally

        End Try
    End Function

    Public Function DecimalFormat(ByVal objDecimal As Object) As Decimal

        Dim rounded As Decimal = Decimal.Round(objDecimal, DecimalPlaces)
        Return rounded

    End Function

    Public Function SelectOptions(ByVal obj As Object, ByVal strValue As String) As Boolean
        Dim I As Int16
        Dim blnRet As Boolean = False
        For I = 0 To obj.Items.Count - 1
            obj.Items(I).Selected = False
            If UCase(Trim(obj.Items(I).Value)) = UCase(Trim(strValue)) Then
                blnRet = True
                obj.Items(I).Selected = True
            End If
        Next
        Return blnRet
    End Function

    Public Sub ShowMessage(ByVal page As System.Web.UI.Page, ByVal message As String)
        message = message.Replace("'", "''")
        page.RegisterStartupScript("Alert", String.Format("<script DEFER : TRUE LANGUAGE = JavaScript> alert('{0}');</script>", message))
    End Sub
    Public Sub SetCommandParameters(ByVal oCommand As SqlCommand, ByVal Name As String, ByVal DataType As SqlDbType, ByVal size As Integer, ByVal Direction As Char, Optional ByVal Scale As Integer = 0, Optional ByVal Precision As Integer = 0, Optional ByVal oValue As Object = vbNull)
        Dim oParam As New SqlParameter
        If size = 0 Then
            oParam = oCommand.Parameters.Add(Name, DataType)
        Else
            oParam = oCommand.Parameters.Add(Name, DataType, size)
        End If
        With oParam
            If Direction = "I" Then .Direction = ParameterDirection.Input
            If Direction = "O" Then .Direction = ParameterDirection.Output
            If Direction = "IO" Then .Direction = ParameterDirection.InputOutput
            If Direction = "R" Then .Direction = ParameterDirection.ReturnValue
            If Not Scale = 0 Then .Scale = Scale
            If Not Precision = 0 Then .Precision = Precision
            If Direction = "I" Or Direction = "IO" Then .Value = oValue
        End With
    End Sub

    Public Function FormatCurr(ByVal dblValue As Double) As String
        Dim strReturn As String
        Dim strValue As String
        Dim intCutLen As Int16 = 6
        strValue = Format(dblValue, "################0.00")
        If Len(strValue) <= 6 Then
            Return strValue
        Else
            strReturn = strValue
            While Len(strReturn) > intCutLen
                '@cur = LEFT(@cur, @Len-@CutLen) + ',' + RIGHT(@cur, @CutLen)
                strReturn = Left(strReturn, Len(strReturn) - intCutLen) & "," & Right(strReturn, intCutLen)
                intCutLen = intCutLen + 3
            End While
        End If
        Return strReturn
    End Function

    Public Function GetReport(ByVal ReportName As String, ByVal crDc As ReportDocument) As ReportDocument
        'Dim crDc As ReportDocument = New ReportDocument
        'Dim strReportPath As String = ConfigurationManager.AppSettings("ReportsPath").ToString
        'crDc.Load(strReportPath + "\" + ReportName)
        Dim crtableLogoninfos As TableLogOnInfos = New TableLogOnInfos
        Dim crtableLogoninfo As TableLogOnInfo = New TableLogOnInfo
        Dim crConnectionInfo As ConnectionInfo = New ConnectionInfo
        Dim CrTables As Tables
        crConnectionInfo.ServerName = ConfigurationManager.AppSettings("DBServerName").ToString
        crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings("DatabaseName").ToString
        crConnectionInfo.UserID = ConfigurationManager.AppSettings("DBUserID").ToString
        crConnectionInfo.Password = ConfigurationManager.AppSettings("DBPassword").ToString
        CrTables = crDc.Database.Tables
        For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next
        Return crDc
    End Function

    Public Sub SetPageTitle(ByVal page As Page, ByVal strCompName As String, ByVal strFinYear As String)
        page.Title = strCompName & "(" & strFinYear & ")-" & page.Title
    End Sub

    Public Function FillControl(ByVal ctrl As Object, ByVal sqlConn As SqlConnection, ByVal strProc As String, Optional ByVal strText As String = "", Optional ByVal strValue As String = "", Optional ByVal intOptParamId1 As Integer = 0, Optional ByVal strOptParamName1 As String = "", Optional ByVal strOptParamId2 As String = "", Optional ByVal strOptParamName2 As String = "", Optional ByVal strOptParamId3 As String = "", Optional ByVal strOptParamName3 As String = "") As DataTable
        Dim dt As DataTable
        dt = FillDataTable1(sqlConn, strProc, intOptParamId1, strOptParamName1, , strOptParamId2, strOptParamName2, strOptParamId3, strOptParamName3)
        ctrl.DataSource = dt

        If strText <> "" Then
            ctrl.DataTextField = strText
            ctrl.DataValueField = strValue
        End If

        ctrl.DataBind()
        If strText <> "" Then ctrl.Items.Insert(0, "")
        Return dt
    End Function
    Public Function FillDistinctDataCombo(ByVal ctrl As Object, ByVal sqlConn As SqlConnection, ByVal strProc As String, Optional ByVal strText As String = "", Optional ByVal strValue As String = "", Optional ByVal intOptParamId1 As Integer = 0, Optional ByVal strOptParamName1 As String = "", Optional ByVal strOptParamId2 As String = "", Optional ByVal strOptParamName2 As String = "", Optional ByVal strOptParamId3 As String = "", Optional ByVal strOptParamName3 As String = "") As DataTable
        Dim dt As DataTable
        Dim dv As DataView
        Dim distinctDt As DataTable

        dt = FillDataTable1(sqlConn, strProc, intOptParamId1, strOptParamName1, , strOptParamId2, strOptParamName2, strOptParamId3, strOptParamName3)

        dv = New DataView(dt)
        distinctDt = dv.ToTable(True, strText, strValue)

        For I As Int16 = 0 To distinctDt.Rows.Count - 1
            ctrl.Items.Add(New ListItem(distinctDt.Rows(I)(strText), distinctDt.Rows(I)(strValue)))
        Next
        ctrl.DataBind()

        If strText <> "" Then ctrl.Items.Insert(0, "")
        Return dt
    End Function



    Public Function FillGrid(ByVal gv As GridView, ByVal sqlConn As SqlConnection, ByVal strProc As String, Optional ByVal strSearchCond As String = "", Optional ByVal intPageIndex As Int16 = 0, Optional ByVal intOptParamId As Integer = 0, Optional ByVal strOptParamName As String = "") As DataTable
        Dim dt As DataTable
        dt = FillDataTable(sqlConn, strProc, intOptParamId, strOptParamName)
        gv.PageIndex = intPageIndex
        gv.DataSource = dt
        gv.DataBind()
        Return dt
    End Function
    Public Function FillDataTable1(ByVal sqlConn As SqlConnection, ByVal strProc As String, _
            Optional ByVal intOptParamId1 As Integer = 0, Optional ByVal strOptParamName1 As String = "", _
            Optional ByVal strSearchCond As String = "", _
            Optional ByVal stOptParamId2 As String = "", Optional ByVal strOptParamName2 As String = "", _
            Optional ByVal stOptParamId3 As String = "", Optional ByVal strOptParamName3 As String = "") As DataTable
        Try
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dt As New DataTable
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            'If sqlTrans IsNot Nothing Then sqlComm.Transaction = sqlTrans '13072010 sanjeev
            If intOptParamId1 <> 0 Then
                SetCommandParameters(sqlComm, strOptParamName1, SqlDbType.BigInt, 8, "I", , , intOptParamId1)
            End If
            If Trim(stOptParamId2) <> "" Then
                SetCommandParameters(sqlComm, strOptParamName2, SqlDbType.VarChar, 50, "I", , , stOptParamId2)
            End If
            If Trim(stOptParamId3) <> "" Then
                SetCommandParameters(sqlComm, strOptParamName3, SqlDbType.VarChar, 50, "I", , , stOptParamId3)
            End If
            If strSearchCond <> "" Then
                SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 8000, "I", , , strSearchCond)
            End If
            SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception

        Finally

        End Try
    End Function


    Public Function FillDataTable(ByVal sqlConn As SqlConnection, ByVal strProc As String, _
            Optional ByVal intOptParamId1 As Integer = 0, Optional ByVal strOptParamName1 As String = "", _
            Optional ByVal strSearchCond As String = "", _
            Optional ByVal stOptParamId2 As String = "", Optional ByVal strOptParamName2 As String = "", _
            Optional ByVal stOptParamId3 As String = "", Optional ByVal strOptParamName3 As String = "", _
            Optional ByVal sqlTrans As SqlTransaction = Nothing) As DataTable
        Try
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dt As New DataTable
            OpenConn()
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            'sqlComm.CommandTimeout = 10
            If sqlTrans IsNot Nothing Then sqlComm.Transaction = sqlTrans '13072010 sanjeev
            If intOptParamId1 <> 0 Then
                SetCommandParameters(sqlComm, strOptParamName1, SqlDbType.BigInt, 8, "I", , , intOptParamId1)
            End If
            If Trim(stOptParamId2) <> "" Then
                SetCommandParameters(sqlComm, strOptParamName2, SqlDbType.VarChar, 8000, "I", , , stOptParamId2)
            End If
            If Trim(stOptParamId3) <> "" Then
                SetCommandParameters(sqlComm, strOptParamName3, SqlDbType.VarChar, 8000, "I", , , stOptParamId3)
            End If
            If strSearchCond <> "" Then
                SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 8000, "I", , , Trim(strSearchCond))
            End If
            SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")

            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Dim str As String = ex.ToString()
        Finally
            CloseConn()
        End Try
    End Function
    Public Function FillDataSet(ByVal sqlConn As SqlConnection, ByVal strProc As String, _
                Optional ByVal intOptParamId1 As Integer = 0, Optional ByVal strOptParamName1 As String = "", _
                Optional ByVal strSearchCond As String = "", _
                Optional ByVal stOptParamId2 As Integer = 0, Optional ByVal strOptParamName2 As String = "", _
                Optional ByVal stOptParamId3 As Date = Nothing, Optional ByVal strOptParamName3 As String = "", _
                Optional ByVal sqlTrans As SqlTransaction = Nothing) As DataSet
        Try
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim ds As New DataSet

            OpenConn()
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            If sqlTrans IsNot Nothing Then sqlComm.Transaction = sqlTrans '13072010 sanjeev
            If intOptParamId1 <> 0 Then
                SetCommandParameters(sqlComm, strOptParamName1, SqlDbType.BigInt, 8, "I", , , intOptParamId1)
            End If
            If Trim(stOptParamId2) <> "" Then
                SetCommandParameters(sqlComm, strOptParamName2, SqlDbType.BigInt, 8, "I", , , stOptParamId2)
            End If
            If Trim(stOptParamId3) <> "" Then
                SetCommandParameters(sqlComm, strOptParamName3, SqlDbType.SmallDateTime, 4, "I", , , stOptParamId3)
            End If
            If strSearchCond <> "" Then
                SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 8000, "I", , , strSearchCond)
            End If
            SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")

            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(ds)
            Return ds

        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Function

    Public Function FillDynamicTable(ByVal sqlConn As SqlConnection, ByVal strQuery As String) As DataTable
        Try
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dt As New DataTable
            OpenConn()
            sqlComm.CommandText = "sp_executesql"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            SetCommandParameters(sqlComm, "@stmt", SqlDbType.NVarChar, 4000, "I", , , strQuery)
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Function

    Public Function FillDynamicGrid(ByVal gv As GridView, ByVal sqlConn As SqlConnection, ByVal strQuery As String, Optional ByVal intPageIndex As Int16 = 0) As DataTable
        Dim dt As DataTable

        dt = FillDynamicTable(sqlConn, strQuery)
        gv.PageIndex = intPageIndex
        gv.DataSource = dt
        gv.DataBind()
        Return dt

    End Function

    Private Sub OpenConn()
        If sqlConn Is Nothing Then
            sqlConn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
            sqlConn.Open()
        ElseIf sqlConn.State = ConnectionState.Closed Then
            sqlConn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
            sqlConn.Open()
            'sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString()
            'sqlConn.Open()
        End If
    End Sub
    Private Sub CloseConn()
        If sqlConn Is Nothing Then Exit Sub
        If sqlConn.State = ConnectionState.Open Then sqlConn.Close()
    End Sub

    Public Function ConvertArrayToString(ByVal arr As Array) As String
        Dim SB As New StringBuilder
        Dim strReturn As String
        For I As Int16 = 0 To arr.Length - 1
            SB.Append(arr(I) & ",")
        Next
        strReturn = SB.ToString()
        If strReturn <> "" Then
            strReturn = Mid(strReturn, 1, strReturn.Length - 1)
        End If
        Return strReturn
    End Function
    Public Function FillDetailsGrid(ByVal grdView As Object, ByVal strProc As String, ByVal strParamName As String, ByVal intParamId As Integer, Optional ByVal strParamName1 As String = "", Optional ByVal strParamId1 As String = "", Optional ByVal strParamName2 As String = "", Optional ByVal strParamId2 As String = "") As DataTable
        Try
            OpenConn()
            Dim dtGrid As DataTable
            Dim objSearch As New clsSearch
            dtGrid = FillControl(grdView, sqlConn, strProc, , , intParamId, strParamName, strParamId1, strParamName1, strParamId2, strParamName2)
            If TypeOf grdView Is GridView Then
                objSearch.SetGrid(grdView, dtGrid)
            End If
            grdView.DataSource = dtGrid
            grdView.DataBind()
            Return dtGrid
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Function

    Public Sub SetGridRows(ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imgBtnEdit As ImageButton
            Dim imgBtnDelete As ImageButton
            imgBtnEdit = CType(e.Row.FindControl("imgBtn_Edit"), ImageButton)
            imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)

            If e.Row.Cells(2).Text = "&nbsp;" Or e.Row.Cells(2).Text = "" Then
                imgBtnEdit.Visible = False
                imgBtnDelete.Visible = False
            Else
                imgBtnDelete.Attributes.Add("onclick", "return CheckDelete();")
            End If
        End If
    End Sub

    Public Shared Function Encrypt(ByVal plainText As String, _
                                   ByVal passPhrase As String, _
                                   ByVal saltValue As String, _
                                   ByVal hashAlgorithm As String, _
                                   ByVal passwordIterations As Integer, _
                                   ByVal initVector As String, _
                                   ByVal keySize As Integer) _
                           As Byte()

        ' Convert strings into byte arrays.
        ' Let us assume that strings only contain ASCII codes.
        ' If strings include Unicode characters, use Unicode, UTF7, or UTF8 
        ' encoding.
        Dim initVectorBytes As Byte()
        initVectorBytes = Encoding.ASCII.GetBytes(initVector)

        Dim saltValueBytes As Byte()
        saltValueBytes = Encoding.ASCII.GetBytes(saltValue)

        ' Convert our plaintext into a byte array.
        ' Let us assume that plaintext contains UTF8-encoded characters.
        Dim plainTextBytes As Byte()
        plainTextBytes = Encoding.UTF8.GetBytes(plainText)

        ' First, we must create a password, from which the key will be derived.
        ' This password will be generated from the specified passphrase and 
        ' salt value. The password will be created using the specified hash 
        ' algorithm. Password creation can be done in several iterations.
        Dim password As PasswordDeriveBytes
        password = New PasswordDeriveBytes(passPhrase, _
                                           saltValueBytes, _
                                           hashAlgorithm, _
                                           passwordIterations)

        ' Use the password to generate pseudo-random bytes for the encryption
        ' key. Specify the size of the key in bytes (instead of bits).
        Dim keyBytes As Byte()
        keyBytes = password.GetBytes(keySize / 8)

        ' Create uninitialized Rijndael encryption object.
        Dim symmetricKey As RijndaelManaged
        symmetricKey = New RijndaelManaged()

        ' It is reasonable to set encryption mode to Cipher Block Chaining
        ' (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC

        ' Generate encryptor from the existing key bytes and initialization 
        ' vector. Key size will be defined based on the number of the key 
        ' bytes.
        Dim encryptor As ICryptoTransform
        encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)

        ' Define memory stream which will be used to hold encrypted data.
        Dim memoryStream As MemoryStream
        memoryStream = New MemoryStream()

        ' Define cryptographic stream (always use Write mode for encryption).
        Dim cryptoStream As CryptoStream
        cryptoStream = New CryptoStream(memoryStream, _
                                        encryptor, _
                                        CryptoStreamMode.Write)
        ' Start encrypting.
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)

        ' Finish encrypting.
        cryptoStream.FlushFinalBlock()

        ' Convert our encrypted data from a memory stream into a byte array.
        Dim cipherTextBytes As Byte()
        cipherTextBytes = memoryStream.ToArray()

        ' Close both streams.
        memoryStream.Close()
        cryptoStream.Close()

        ' Convert encrypted data into a base64-encoded string.
        Dim cipherText As String
        cipherText = Convert.ToBase64String(cipherTextBytes)

        ' Return encrypted string.
        Encrypt = cipherTextBytes
    End Function

    Public Function GetEncryptedPassword(ByVal strPassword As String, ByVal strEncryptType As String) As Byte()
        Dim plainText As String
        Dim passPhrase As String
        Dim saltValue As String
        Dim hashAlgorithm As String
        Dim passwordIterations As Integer
        Dim initVector As String
        Dim keySize As Integer

        plainText = "dev9999*"          ' original plaintext
        passPhrase = "Pas5pr@se"        ' can be any string
        saltValue = "s@1tValue"         ' can be any string
        hashAlgorithm = "SHA1"          ' can be "MD5"
        passwordIterations = 2          ' can be any number
        initVector = "@1B2c3D4e5F6g7H8" ' must be 16 bytes
        keySize = 128                   ' can be 128 or 192 or 256

        Dim bytPassword() As Byte
        bytPassword = Encrypt(plainText, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize)
        Return bytPassword
    End Function

    Public Function CheckDuplicate( _
           ByVal sqlConn As SqlConnection, ByVal table As String, _
           ByVal TextFieldName As String, ByVal TextFieldValue As String, _
           Optional ByVal IdFieldName As String = "", Optional ByVal IdFieldValue As Integer = 0, _
           Optional ByVal OptIdFieldName As String = "", Optional ByVal OptIdFieldValue As Integer = 0) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim a As Integer

            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_CHECK_Duplicates"
                .Parameters.Clear()
                If IdFieldValue <> 0 Then
                    SetCommandParameters(sqlComm, "@IdFieldName", SqlDbType.VarChar, 50, "I", , , IdFieldName)
                    SetCommandParameters(sqlComm, "@IdFieldValue", SqlDbType.BigInt, 8, "I", , , IdFieldValue)
                End If
                If OptIdFieldValue <> 0 Then
                    SetCommandParameters(sqlComm, "@OptIdFieldName", SqlDbType.VarChar, 50, "I", , , OptIdFieldName)
                    SetCommandParameters(sqlComm, "@OptIdFieldValue", SqlDbType.BigInt, 8, "I", , , OptIdFieldValue)
                End If
                SetCommandParameters(sqlComm, "@TextFieldName", SqlDbType.VarChar, 50, "I", , , TextFieldName)
                SetCommandParameters(sqlComm, "@TextFieldValue", SqlDbType.VarChar, 500, "I", , , Trim(TextFieldValue))
                SetCommandParameters(sqlComm, "@TableName", SqlDbType.VarChar, 255, "I", , , table)
                SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                SetCommandParameters(sqlComm, "@Valid", SqlDbType.Bit, 1, "O")
                a = (sqlComm.Parameters("@Valid").Value)
                .ExecuteNonQuery()
            End With
            Return CBool(sqlComm.Parameters("@Valid").Value)
        Catch ex As Exception
        Finally

        End Try
    End Function
    Public Sub FillSelectedListBox(ByVal lstBox As ListBox, ByVal Hid_Name As HiddenField, ByVal Hid_Id As HiddenField)
        Dim strNames() As String
        Dim strId() As String
        Dim I As Integer
        strNames = Split(Hid_Name.Value, "!")
        strId = Split(Hid_Id.Value, "!")
        lstBox.Items.Clear()
        For I = 0 To strNames.Length - 1
            If strNames(I) <> "" Then
                lstBox.Items.Add(New ListItem(strNames(I), strId(I)))
            End If
        Next
    End Sub
    Public Sub FillList(ByVal chkList As CheckBoxList, ByVal strproc As String, ByVal txtname As TextBox, Optional ByVal intid As Int16 = 0, Optional ByVal intOptid As Int16 = 0, Optional ByVal intPageIndex As Int16 = 0, Optional ByVal chrFlag As Char = "", Optional ByVal intnextOpt As Int16 = 0, Optional ByVal FieldName As String = "", Optional ByVal ValueName As String = "")
        Try
            Dim dtfill As New DataTable
            Dim dvfill As New DataView
            Dim sqlda As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strproc
                .Parameters.Clear()
                If intid <> 0 Then
                    SetCommandParameters(sqlComm, "@intid", SqlDbType.BigInt, 8, "I", , , intid)
                End If
                If intOptid <> 0 Then
                    SetCommandParameters(sqlComm, "@intOptid", SqlDbType.BigInt, 8, "I", , , intOptid)
                End If
                If intnextOpt <> 0 Then
                    SetCommandParameters(sqlComm, "@intnextOpt", SqlDbType.BigInt, 8, "I", , , intnextOpt)
                End If
                If Trim(txtname.Text <> "") Then
                    SetCommandParameters(sqlComm, "@txtname", SqlDbType.VarChar, 100, "I", , , Trim(txtname.Text))
                End If
                If Trim(chrFlag & "") <> "" Then
                    SetCommandParameters(sqlComm, "@chrFlag", SqlDbType.VarChar, 100, "I", , , Trim(chrFlag))
                End If
                SetCommandParameters(sqlComm, "RET_CODE", SqlDbType.Int, 4, "O")
                sqlComm.ExecuteNonQuery()
                sqlda.SelectCommand = sqlComm
                sqlda.Fill(dtfill)
                dvfill = dtfill.DefaultView
                chkList.DataSource = dvfill
                chkList.DataTextField = FieldName
                chkList.DataValueField = ValueName
                chkList.DataBind()
            End With
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Sub
    Public Function ISNameexists(ByVal strproc As String, ByVal sqlConn1 As SqlConnection, ByVal txtname As TextBox, Optional ByVal intid As Int16 = 0, Optional ByVal intOptid As Int16 = 0, Optional ByVal intOptid1 As Int16 = 0) As Boolean
        Try
            Dim dt As New DataTable
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strproc
                .Parameters.Clear()
                If intid <> 0 Then
                    SetCommandParameters(sqlComm, "@intid", SqlDbType.BigInt, 8, "I", , , intid)
                End If
                If intOptid <> 0 Then
                    SetCommandParameters(sqlComm, "@intOptid", SqlDbType.BigInt, 8, "I", , , intOptid)
                End If
                If intOptid1 <> 0 Then
                    SetCommandParameters(sqlComm, "@intOptid1", SqlDbType.BigInt, 8, "I", , , intOptid1)
                End If
                SetCommandParameters(sqlComm, "@txtname", SqlDbType.VarChar, 50, "I", , , Trim(txtname.Text))
                SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                SetCommandParameters(sqlComm, "@Valid", SqlDbType.Bit, 1, "O")
                .ExecuteNonQuery()
            End With
            If CBool(sqlComm.Parameters("@Valid").Value) = False Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Function
    Public Function SaveContact(ByVal sqlTrans As SqlTransaction, ByVal dg As DataGrid, ByVal strproc As String, ByVal dt As DataTable, ByVal ProfileType As Int16, ByVal BusinessType As Int16) As Boolean
        Dim sqlComm As New SqlCommand
        Try
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = strproc
                For I As Int16 = 0 To dt.Rows.Count - 1
                    If dt.Rows(I).Item("ContactPerson").ToString <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("ContactPerson"))
                        SetCommandParameters(sqlComm, "@Designation", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Designation"))
                        SetCommandParameters(sqlComm, "@PhoneNo1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("PhoneNo1"))
                        SetCommandParameters(sqlComm, "@PhoneNo2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("PhoneNo2"))
                        SetCommandParameters(sqlComm, "@FaxNo1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FaxNo1"))
                        SetCommandParameters(sqlComm, "@FaxNo2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FaxNo2"))
                        SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("MobileNo"))
                        SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("EmailId"))
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustomerId"))
                        SetCommandParameters(sqlComm, "@SectionType", SqlDbType.Char, 1, "I", , , dt.Rows(I).Item("SectionType"))
                        SetCommandParameters(sqlComm, "@ProfileType", SqlDbType.Char, 2, "I", , , ProfileTypes(ProfileType))
                        SetCommandParameters(sqlComm, "@BusniessType", SqlDbType.Char, 2, "I", , , BusinessTypes(BusinessType))
                        SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.SmallInt, 4, "O")
                        SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                        sqlComm.ExecuteNonQuery()


                        'If SaveClientBusniessDetails(sqlTrans, Trim(dt.Rows(I).Item("BusniessTypeIds") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False
                        'If SaveClientUserDetails(sqlTrans, Trim(dt.Rows(I).Item("UserIds") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False
                        'If SaveClientResearchDetails(sqlTrans, Trim(dt.Rows(I).Item("ResearchDocId") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False



                        If SaveClientBusniessDetails(sqlTrans, Trim(dt.Rows(I).Item("BusniessTypeIds") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False
                        If SaveClientUserDetails(sqlTrans, Trim(dt.Rows(I).Item("UserIds") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False
                        If SaveClientResearchDetails(sqlTrans, Trim(dt.Rows(I).Item("ResearchDocId") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False

                    End If
                Next
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            CloseConn()
        End Try
    End Function
    'New Function for save contact due to connection issues-23-jul-10
    Public Function SaveContactNew(ByVal sqlconn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal dg As DataGrid, ByVal strproc As String, ByVal dt As DataTable, ByVal ProfileType As Int16, ByVal BusinessType As Int16) As Boolean
        Dim sqlComm As New SqlCommand
        Try
            'OpenConn()
            With sqlComm
                .Connection = sqlconn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = strproc
                For I As Int16 = 0 To dt.Rows.Count - 1
                    If dt.Rows(I).Item("ContactPerson").ToString <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("ContactPerson"))
                        SetCommandParameters(sqlComm, "@Designation", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Designation"))
                        SetCommandParameters(sqlComm, "@PhoneNo1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("PhoneNo1"))
                        SetCommandParameters(sqlComm, "@PhoneNo2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("PhoneNo2"))
                        SetCommandParameters(sqlComm, "@FaxNo1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FaxNo1"))
                        SetCommandParameters(sqlComm, "@FaxNo2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FaxNo2"))
                        SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("MobileNo"))
                        SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("EmailId"))
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustomerId"))
                        SetCommandParameters(sqlComm, "@SectionType", SqlDbType.Char, 1, "I", , , dt.Rows(I).Item("SectionType"))
                        SetCommandParameters(sqlComm, "@Interaction", SqlDbType.Char, 1, "I", , , dt.Rows(I).Item("Interaction"))
                        SetCommandParameters(sqlComm, "@Branch", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Branch"))
                        SetCommandParameters(sqlComm, "@ProfileType", SqlDbType.Char, 2, "I", , , ProfileTypes(ProfileType))
                        SetCommandParameters(sqlComm, "@BusniessType", SqlDbType.Char, 2, "I", , , BusinessTypes(BusinessType))
                        SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.SmallInt, 4, "O")
                        SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                        sqlComm.ExecuteNonQuery()
                        If SaveClientBusniessDetailsNew(sqlconn, sqlTrans, Trim(dt.Rows(I).Item("BusniessTypeIds") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False
                        If SaveClientUserDetailsNew(sqlconn, sqlTrans, Trim(dt.Rows(I).Item("UserIds") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False
                        If SaveClientResearchDetailsNew(sqlconn, sqlTrans, Trim(dt.Rows(I).Item("ResearchDocId") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False

                    End If
                Next
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            'CloseConn()
        End Try
    End Function

    'Amit
    Public Function SaveContact(ByVal sqlconn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal dg As DataGrid, ByVal strproc As String, ByVal dt As DataTable, ByVal ProfileType As Int16, ByVal BusinessType As Int16) As Boolean
        Dim sqlComm As New SqlCommand
        Dim _int As String = 0
        Try
            'OpenConn()
            With sqlComm
                .Connection = sqlconn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = strproc
                For I As Int16 = 0 To dt.Rows.Count - 1
                    If dt.Rows(I).Item("ContactPerson").ToString <> "" Then
                        sqlComm.Parameters.Clear()
                        _int = Convert.ToString(dt.Rows(I).Item("ContactDetailId"))
                        If _int = "" Then
                            sqlComm.CommandText = strproc
                            SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.Int, 4, "O")
                        Else
                            'also delete ClientUserDetails 
                            sqlComm.CommandText = "ID_UPDATE_ClientContactsDetail"
                            SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("ContactDetailId"))
                        End If
                        SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("ContactPerson"))
                        SetCommandParameters(sqlComm, "@Designation", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Designation"))
                        SetCommandParameters(sqlComm, "@PhoneNo1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("PhoneNo1"))
                        SetCommandParameters(sqlComm, "@PhoneNo2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("PhoneNo2"))
                        SetCommandParameters(sqlComm, "@FaxNo1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FaxNo1"))
                        SetCommandParameters(sqlComm, "@FaxNo2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FaxNo2"))
                        SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("MobileNo"))
                        SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("EmailId"))
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustomerId"))
                        SetCommandParameters(sqlComm, "@SectionType", SqlDbType.Char, 1, "I", , , dt.Rows(I).Item("SectionType"))
                        SetCommandParameters(sqlComm, "@Interaction", SqlDbType.Char, 1, "I", , , dt.Rows(I).Item("Interaction"))
                        SetCommandParameters(sqlComm, "@Branch", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Branch"))
                        SetCommandParameters(sqlComm, "@ProfileType", SqlDbType.Char, 2, "I", , , ProfileTypes(ProfileType))
                        SetCommandParameters(sqlComm, "@BusniessType", SqlDbType.Char, 2, "I", , , BusinessTypes(BusinessType))
                        'SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.SmallInt, 4, "O")
                        SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                        sqlComm.ExecuteNonQuery()
                        If SaveClientBusniessDetailsNew(sqlconn, sqlTrans, Trim(dt.Rows(I).Item("BusniessTypeIds") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False
                        If SaveClientUserDetailsNew(sqlconn, sqlTrans, Trim(dt.Rows(I).Item("UserIds") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False
                        If SaveClientResearchDetailsNew(sqlconn, sqlTrans, Trim(dt.Rows(I).Item("ResearchDocId") & ""), sqlComm.Parameters("@ContactDetailId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False

                    End If
                Next
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            'CloseConn()
        End Try
    End Function
    Public Function SaveClientBusniessDetails(ByVal sqlTrans As SqlTransaction, ByVal strBussTypes As String, _
                                              ByVal intId As Int32, ByVal intCustId As Integer) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim arrBussType() As String
            arrBussType = Split(strBussTypes, ",")
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_ClientBusniessDetails"
                For I As Int16 = 0 To arrBussType.Length - 1
                    If Trim(arrBussType(I)) <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustId)
                        SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.BigInt, 8, "I", , , intId)
                        SetCommandParameters(sqlComm, "@BusinessTypeId", SqlDbType.BigInt, 8, "I", , , Val(arrBussType(I)))
                        SetCommandParameters(sqlComm, "@ClientBusniessDetailId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                    End If
                Next
            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            CloseConn()
        End Try
    End Function
    'New Function for save BusinessDetails due to connection issues-10-aug-10
    Public Function SaveClientBusniessDetailsNew(ByVal sqlconn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal strBussTypes As String, _
                                              ByVal intId As Int32, ByVal intCustId As Integer) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim arrBussType() As String
            arrBussType = Split(strBussTypes, ",")

            With sqlComm
                .Connection = sqlconn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_ClientBusniessDetails"
                For I As Int16 = 0 To arrBussType.Length - 1
                    If Trim(arrBussType(I)) <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustId)
                        SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.BigInt, 8, "I", , , intId)
                        SetCommandParameters(sqlComm, "@BusinessTypeId", SqlDbType.BigInt, 8, "I", , , Val(arrBussType(I)))
                        SetCommandParameters(sqlComm, "@ClientBusniessDetailId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                    End If
                Next
            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally

        End Try
    End Function

    Public Function SaveClientResearchDetails(ByVal sqlTrans As SqlTransaction, ByVal strResearchDocName As String, _
                                              ByVal intId As Int32, ByVal intCustId As Integer) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim arrResearchDocName() As String
            arrResearchDocName = Split(strResearchDocName, ",")
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_ClientResearchDetails"
                For I As Int16 = 0 To arrResearchDocName.Length - 1
                    If Trim(arrResearchDocName(I)) <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustId)
                        SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.BigInt, 8, "I", , , intId)
                        SetCommandParameters(sqlComm, "@ResearchDocId", SqlDbType.BigInt, 8, "I", , , Val(arrResearchDocName(I)))
                        SetCommandParameters(sqlComm, "@ClientResearchDetailId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                    End If
                Next
            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            CloseConn()
        End Try
    End Function
    Public Function SaveClientResearchDetailsNew(ByVal sqlconn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal strResearchDocName As String, _
                                            ByVal intId As Int32, ByVal intCustId As Integer) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim arrResearchDocName() As String
            arrResearchDocName = Split(strResearchDocName, ",")

            With sqlComm
                .Connection = sqlconn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_ClientResearchDetails"
                For I As Int16 = 0 To arrResearchDocName.Length - 1
                    If Trim(arrResearchDocName(I)) <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustId)
                        SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.BigInt, 8, "I", , , intId)
                        SetCommandParameters(sqlComm, "@ResearchDocId", SqlDbType.BigInt, 8, "I", , , Val(arrResearchDocName(I)))
                        SetCommandParameters(sqlComm, "@ClientResearchDetailId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                    End If
                Next
            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally

        End Try
    End Function

    Public Function SaveClientUserDetails(ByVal sqlTrans As SqlTransaction, ByVal strNameOfUser As String, _
                                              ByVal intId As Int32, ByVal intCustId As Integer) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim arrUserIds() As String
            arrUserIds = Split(strNameOfUser, ",")
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_ClientUserDetails"
                For I As Int16 = 0 To arrUserIds.Length - 1
                    If Trim(arrUserIds(I)) <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustId)
                        SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.BigInt, 8, "I", , , intId)
                        SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(arrUserIds(I)))
                        SetCommandParameters(sqlComm, "@ClientUserDetailId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                    End If
                Next
            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            CloseConn()
        End Try
    End Function
    Public Function SaveClientUserDetailsNew1(ByVal sqlconn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal strNameOfUser As String, _
                                             ByVal intId As Int32, ByVal intCustId As Integer) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim arrUserIds() As String
            Dim Dt As New DataTable
            Dim int1 As Integer
            arrUserIds = Split(strNameOfUser, ",")
            'OpenConn()
            With sqlComm
                .Connection = sqlconn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure

                Dt = CType(HttpContext.Current.Session("contactTable"), DataTable)


                For I1 As Int16 = 0 To Dt.Rows.Count - 1

                    sqlComm.Parameters.Clear()
                    int1 = Convert.ToInt32(Dt.Rows(I1).Item("ClientUserDetailId"))
                    If int1 = 0 Then
                        .CommandText = "ID_INSERT_ClientUserDetails"
                        SetCommandParameters(sqlComm, "@ClientUserDetailId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_ClientContactsDetail"
                        SetCommandParameters(sqlComm, "@ClientUserDetailId", SqlDbType.Int, 4, "I", , , int1)
                    End If

                    For I As Int16 = 0 To arrUserIds.Length - 1
                        If Trim(arrUserIds(I)) <> "" Then
                            sqlComm.Parameters.Clear()
                            SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustId)
                            SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.BigInt, 8, "I", , , intId)
                            SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(arrUserIds(I)))
                            SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                            SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                            sqlComm.ExecuteNonQuery()
                        End If
                    Next

                Next

            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            'CloseConn()
        End Try
    End Function
    Public Function SaveClientUserDetailsNew(ByVal sqlconn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal strNameOfUser As String, _
                                               ByVal intId As Int32, ByVal intCustId As Integer) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim arrUserIds() As String
            arrUserIds = Split(strNameOfUser, ",")
            'OpenConn()
            With sqlComm
                .Connection = sqlconn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_ClientUserDetails1"
                '.CommandText = "ID_INSERT_ClientUserDetails"
                For I As Int16 = 0 To arrUserIds.Length - 1
                    If Trim(arrUserIds(I)) <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustId)
                        SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.BigInt, 8, "I", , , intId)
                        SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(arrUserIds(I)))
                        SetCommandParameters(sqlComm, "@ClientUserDetailId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                    End If
                Next
            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            'CloseConn()
        End Try
    End Function


    Public Function SaveClientAddress(ByVal sqlTrans As SqlTransaction, ByVal dg As DataGrid, ByVal strproc As String, ByVal dt As DataTable, ByVal ProfileType As Int16, ByVal BusinessType As Int16) As Boolean
        Dim sqlComm As New SqlCommand
        Try
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = strproc
                For I As Int16 = 0 To dt.Rows.Count - 1
                    If dt.Rows(I).Item("CustomerBranchName").ToString <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@CustomerBranchName", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("CustomerBranchName"))
                        SetCommandParameters(sqlComm, "@City", SqlDbType.VarChar, 20, "I", , , dt.Rows(I).Item("City"))
                        SetCommandParameters(sqlComm, "@PinCode", SqlDbType.VarChar, 15, "I", , , dt.Rows(I).Item("PinCode"))
                        SetCommandParameters(sqlComm, "@State", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("State"))
                        SetCommandParameters(sqlComm, "@Country", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Country"))
                        SetCommandParameters(sqlComm, "@Phone", SqlDbType.VarChar, 50, "I", , , dt.Rows(I).Item("Phone"))
                        SetCommandParameters(sqlComm, "@FaxNo", SqlDbType.VarChar, 15, "I", , , dt.Rows(I).Item("FaxNo"))
                        SetCommandParameters(sqlComm, "@Address1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Address1"))
                        SetCommandParameters(sqlComm, "@Address2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Address2"))
                        SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("EmailId"))
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , dt.Rows(I).Item("CustomerId"))
                        SetCommandParameters(sqlComm, "@ProfileType", SqlDbType.Char, 2, "I", , , ProfileTypes(ProfileType))
                        SetCommandParameters(sqlComm, "@BusniessType", SqlDbType.Char, 2, "I", , , BusinessTypes(BusinessType))
                        SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                        If SaveClientAddBusniessDetails(sqlTrans, Trim(dt.Rows(I).Item("BusniessTypeIds") & ""), sqlComm.Parameters("@ClientCustAddressId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False
                    End If
                Next
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            CloseConn()
        End Try
    End Function
    'New Function for Client Addr due to connection issues-23-jul-10
    Public Function SaveClientAddressNew(ByVal sqlconn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal dg As DataGrid, ByVal strproc As String, ByVal dt As DataTable, ByVal ProfileType As Int16, ByVal BusinessType As Int16) As Boolean
        Dim sqlComm As New SqlCommand
        Try
            'OpenConn()
            With sqlComm
                .Connection = sqlconn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = strproc
                For I As Int16 = 0 To dt.Rows.Count - 1
                    If dt.Rows(I).Item("CustomerBranchName").ToString <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@CustomerBranchName", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("CustomerBranchName"))
                        SetCommandParameters(sqlComm, "@City", SqlDbType.VarChar, 20, "I", , , dt.Rows(I).Item("City"))
                        SetCommandParameters(sqlComm, "@PinCode", SqlDbType.VarChar, 15, "I", , , dt.Rows(I).Item("PinCode"))
                        SetCommandParameters(sqlComm, "@State", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("State"))
                        SetCommandParameters(sqlComm, "@Country", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Country"))
                        SetCommandParameters(sqlComm, "@Phone", SqlDbType.VarChar, 50, "I", , , dt.Rows(I).Item("Phone"))
                        SetCommandParameters(sqlComm, "@FaxNo", SqlDbType.VarChar, 15, "I", , , dt.Rows(I).Item("FaxNo"))
                        SetCommandParameters(sqlComm, "@Address1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Address1"))
                        SetCommandParameters(sqlComm, "@Address2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Address2"))
                        SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("EmailId"))
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , dt.Rows(I).Item("CustomerId"))
                        SetCommandParameters(sqlComm, "@ProfileType", SqlDbType.Char, 2, "I", , , ProfileTypes(ProfileType))
                        SetCommandParameters(sqlComm, "@BusniessType", SqlDbType.Char, 2, "I", , , BusinessTypes(BusinessType))
                        SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                        If SaveClientAddBusniessDetailsNew(sqlconn, sqlTrans, Trim(dt.Rows(I).Item("BusniessTypeIds") & ""), sqlComm.Parameters("@ClientCustAddressId").Value, dt.Rows(I).Item("CustomerId")) = False Then Return False
                    End If
                Next
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            'CloseConn()
        End Try
    End Function
    Public Function SaveClientAddBusniessDetails(ByVal sqlTrans As SqlTransaction, ByVal strAddBussTypes As String, _
                                              ByVal intId As Int32, ByVal intCustId As Integer) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim arrBussType() As String
            arrBussType = Split(strAddBussTypes, ",")
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_ClientAddBusinessDetails"
                For I As Int16 = 0 To arrBussType.Length - 1
                    If Trim(arrBussType(I)) <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustId)
                        SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.BigInt, 8, "I", , , intId)
                        SetCommandParameters(sqlComm, "@BusinessTypeId", SqlDbType.BigInt, 8, "I", , , Val(arrBussType(I)))
                        SetCommandParameters(sqlComm, "@ClientAddBusniessDetailId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                    End If
                Next
            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            CloseConn()
        End Try
    End Function
    'New Function for Client Business Details due to connection issues-10-aug-10
    Public Function SaveClientAddBusniessDetailsNew(ByVal sqlconn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal strAddBussTypes As String, _
                                              ByVal intId As Int32, ByVal intCustId As Integer) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim arrBussType() As String
            arrBussType = Split(strAddBussTypes, ",")

            With sqlComm
                .Connection = sqlconn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_ClientAddBusinessDetails"
                For I As Int16 = 0 To arrBussType.Length - 1
                    If Trim(arrBussType(I)) <> "" Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustId)
                        SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.BigInt, 8, "I", , , intId)
                        SetCommandParameters(sqlComm, "@BusinessTypeId", SqlDbType.BigInt, 8, "I", , , Val(arrBussType(I)))
                        SetCommandParameters(sqlComm, "@ClientAddBusniessDetailId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                    End If
                Next
            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally

        End Try
    End Function
    Public Function ProfileTypes(ByVal ProfileType As Int16) As String
        Dim strProfileType As String
        If (ProfileType = 1) Then
            strProfileType = "OT"
        ElseIf (ProfileType = 2) Then
            strProfileType = "MF"
        ElseIf (ProfileType = 3) Then
            strProfileType = "IN"
        ElseIf (ProfileType = 4) Then
            strProfileType = "BA"
        ElseIf (ProfileType = 5) Then
            strProfileType = "PF"
        ElseIf (ProfileType = 6) Then
            strProfileType = "CO"
        ElseIf (ProfileType = 7) Then
            strProfileType = "CM"
        End If
        Return strProfileType
    End Function

    Public Function BusinessTypes(ByVal BusinessType As Int16) As String
        Dim strBusinessType As String
        If (BusinessType = 1) Then
            strBusinessType = "NO"
        ElseIf (BusinessType = 2) Then
            strBusinessType = "MB"
        ElseIf (BusinessType = 3) Then
            strBusinessType = "PM"

        End If
        Return strBusinessType
    End Function


    Public Function SaveClientsource(ByVal sqlTrans As SqlTransaction, ByVal dg As DataGrid, ByVal strproc As String, ByVal dt As DataTable, ByVal ProfileType As Int16, ByVal BusinessType As Int16) As Boolean
        Dim sqlComm As New SqlCommand
        Try
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = strproc
                For I As Int16 = 0 To dt.Rows.Count - 1
                    If Val(dt.Rows(I).Item("CustomerId")) <> 0 Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@SourceId", SqlDbType.BigInt, 8, "I", , , Val(dt.Rows(I).Item("SourceId")))
                        SetCommandParameters(sqlComm, "@BusinessTypeId", SqlDbType.BigInt, 8, "I", , , dt.Rows(I).Item("BusinessTypeId"))
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , dt.Rows(I).Item("CustomerId"))
                        SetCommandParameters(sqlComm, "@ProfileType", SqlDbType.Char, 2, "I", , , ProfileTypes(ProfileType))
                        SetCommandParameters(sqlComm, "@BusniessType", SqlDbType.Char, 2, "I", , , BusinessTypes(BusinessType))
                        SetCommandParameters(sqlComm, "@ClientSourceDetailId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                    End If
                Next
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            CloseConn()
        End Try
    End Function
    'New Function for client source due to connection issues-23-jul-10
    Public Function SaveClientsourceNew(ByVal sqlconn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal dg As DataGrid, ByVal strproc As String, ByVal dt As DataTable, ByVal ProfileType As Int16, ByVal BusinessType As Int16) As Boolean
        Dim sqlComm As New SqlCommand
        Try

            With sqlComm
                .Connection = sqlconn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = strproc
                For I As Int16 = 0 To dt.Rows.Count - 1
                    If Val(dt.Rows(I).Item("CustomerId")) <> 0 Then
                        sqlComm.Parameters.Clear()
                        SetCommandParameters(sqlComm, "@SourceId", SqlDbType.BigInt, 8, "I", , , Val(dt.Rows(I).Item("SourceId")))
                        SetCommandParameters(sqlComm, "@BusinessTypeId", SqlDbType.BigInt, 8, "I", , , dt.Rows(I).Item("BusinessTypeId"))
                        SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , dt.Rows(I).Item("CustomerId"))
                        SetCommandParameters(sqlComm, "@ProfileType", SqlDbType.Char, 2, "I", , , ProfileTypes(ProfileType))
                        SetCommandParameters(sqlComm, "@BusniessType", SqlDbType.Char, 2, "I", , , BusinessTypes(BusinessType))
                        SetCommandParameters(sqlComm, "@ClientSourceDetailId", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                        SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                        sqlComm.ExecuteNonQuery()
                    End If
                Next
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally

        End Try
    End Function

    Public Function GetReport(ByVal crDc As ReportDocument) As ReportDocument
        Dim crtableLogoninfos As TableLogOnInfos = New TableLogOnInfos
        Dim crtableLogoninfo As TableLogOnInfo = New TableLogOnInfo
        Dim crConnectionInfo As ConnectionInfo = New ConnectionInfo
        Dim CrTables As Tables
        crConnectionInfo.ServerName = ConfigurationManager.AppSettings("DBServerName").ToString
        crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings("DatabaseName").ToString
        crConnectionInfo.UserID = ConfigurationManager.AppSettings("DBUserID").ToString
        crConnectionInfo.Password = ConfigurationManager.AppSettings("DBPassword").ToString
        CrTables = crDc.Database.Tables
        For Each CrTable As Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next
        Return crDc
    End Function
    Public Function UpdateLiquidAmount(ByVal sqlTrans As SqlTransaction, ByVal intCustomerId As Integer) As Boolean
        Dim sqlComm As New SqlCommand
        Try
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_CheckLiquidAmounts"
                sqlComm.Parameters.Clear()
                'SetCommandParameters(sqlComm, "@LiquidAmount", SqlDbType.Decimal, 9, "I", , , decLiquidAmount)
                SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustomerId)
                SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                sqlComm.ExecuteNonQuery()
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            CloseConn()
        End Try
    End Function

    Public Function UpdateLiquidAmount1(ByVal sqlconn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal intCustomerId As Integer) As Boolean
        Dim sqlComm As New SqlCommand
        Try

            With sqlComm
                .Connection = sqlconn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_CheckLiquidAmounts"
                sqlComm.Parameters.Clear()
                'SetCommandParameters(sqlComm, "@LiquidAmount", SqlDbType.Decimal, 9, "I", , , decLiquidAmount)
                SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , intCustomerId)
                SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                sqlComm.ExecuteNonQuery()
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally

        End Try
    End Function
    Public Sub RemoveRecord(ByVal lstbox As ListBox, ByVal txtbox As TextBox)

        Try
            'Dim strdate As Date
            'strdate = txtbox.Text
            lstbox.Items.RemoveAt(lstbox.SelectedIndex)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub FillListHoliday(ByVal lstBox As ListBox, ByVal txtbox As TextBox)
        Try
            Dim strdates(10) As String
            If lstBox.Items.Count <> 0 Then
                Dim i As Integer = lstBox.Items.Count
                Dim j As Int16
                For j = 0 To i - 1
                    strdates(j) = lstBox.Items(j).Text
                    If lstBox.Text = lstBox.Items(j).Text Then
                        '  Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert(' Date  is already exist');", True)
                        txtbox.Text = ""
                        Exit Sub
                    End If
                Next
            End If

            Dim strdate As Date
            strdate = txtbox.Text
            lstBox.Items.Add(strdate)
            txtbox.Text = ""
        Catch ex As Exception
            'Response.Write(ex.Message)
        End Try
    End Sub

    Public Function Crypt(ByVal oldval As String) As String
        Dim NewVal As String
        Dim j As Integer

        NewVal = ""
        For j = 1 To Len(oldval)
            NewVal = NewVal & Chr(170 - Asc(Mid(oldval, j, 1)))
        Next

        Return NewVal
    End Function

    Public Function DeCrypt(ByVal oldval As String) As String
        Dim NewVal As String
        Dim j As Integer

        NewVal = ""
        For j = 1 To Len(oldval)
            NewVal = NewVal & Chr(170 - Asc(Mid(oldval, j, 1)))
        Next

        Return NewVal
    End Function
    Enum enumObjectType
        StrType = 0
        IntType = 1
        DblType = 2
    End Enum

    Public Function CheckDBNull(ByVal obj As Object, _
    Optional ByVal ObjectType As enumObjectType = enumObjectType.StrType) As Object
        Dim objReturn As Object
        objReturn = obj
        If ObjectType = enumObjectType.StrType And IsDBNull(obj) Then
            objReturn = 0
        ElseIf ObjectType = enumObjectType.IntType And IsDBNull(obj) Then
            objReturn = 0
        ElseIf ObjectType = enumObjectType.DblType And IsDBNull(obj) Then
            objReturn = 0.0
        End If
        Return objReturn
    End Function


    'Amit
    Public Function DeleteClientContact(ByVal ContactDetailId As Integer) As Boolean
        Try

            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            'sqlComm.CommandText = "ID_DELETE_ClientContact"
            sqlComm.CommandText = "ID_DELETE_ClientContact1"
            OpenConn()
            sqlComm.Connection = sqlConn
            If ContactDetailId <> 0 Then
                SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.SmallInt, 2, "I", , , ContactDetailId)
                SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
                SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.VarChar, 100, "O")
                If (sqlComm.ExecuteNonQuery() <> 0) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return True
            End If

            'SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.SmallInt, 2, "I", , , ContactDetailId)
            'SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
            'SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.VarChar, 100, "O")
            'If (sqlComm.ExecuteNonQuery() <> 0) Then
            '    Return True
            'Else
            '    Return False
            'End If
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Function
    Public Sub SetGridRows1(ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imgBtnEdit As HtmlAnchor
            Dim imgBtnDelete As ImageButton
            imgBtnEdit = CType(e.Row.FindControl("imgBtn_Edit"), HtmlAnchor)
            imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)

            If e.Row.Cells(2).Text = "&nbsp;" Or e.Row.Cells(2).Text = "" Then
                imgBtnEdit.Visible = False
                imgBtnDelete.Visible = False
            Else
                imgBtnDelete.Attributes.Add("onclick", "return CheckDelete();")
            End If
        End If
    End Sub
    Public Function DecimalFormat10(ByVal objDecimal As Object) As Decimal
        Dim rounded As Decimal = Decimal.Round(objDecimal, 10)
        Return rounded
    End Function

    Public Function EncryptText(clearText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
         &H65, &H64, &H76, &H65, &H64, &H65,
         &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function
    Public Function DecryptText(cipherText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        cipherText = cipherText.Replace(" ", "+")
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function
    Public Function GetValidInput(strValue As String) As String
        If Trim(strValue & "") <> "" Then
            strValue = strValue.Replace("/*", "")
            strValue = strValue.Replace("*/", "")
            strValue = strValue.Replace("//", "")
            strValue = strValue.Replace("\\", "")

            strValue = Regex.Replace(strValue, "['*<>`~]", "")
            strValue = Regex.Replace(strValue, "--", "", RegexOptions.IgnoreCase)
            strValue = Regex.Replace(strValue, "Delete", "", RegexOptions.IgnoreCase)
            strValue = Regex.Replace(strValue, "Truncate", "", RegexOptions.IgnoreCase)
            strValue = Regex.Replace(strValue, "Drop", "", RegexOptions.IgnoreCase)
            strValue = Regex.Replace(strValue, "Select ", "", RegexOptions.IgnoreCase)
            strValue = Regex.Replace(strValue, "Update ", "", RegexOptions.IgnoreCase)
        End If

        Return Trim(strValue)
    End Function
    Public Function InsertError(ByVal strMessage As String, ByVal strProc As String) As Integer
        Try
            OpenConn()
            Dim objCommon As New clsCommonFuns
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@ErrorMessage", SqlDbType.VarChar, 500, "I", , , Trim(strMessage))
            sqlComm.ExecuteNonQuery()

        Catch ex As Exception
        Finally
            CloseConn()
        End Try
        Return 0
    End Function
End Class
