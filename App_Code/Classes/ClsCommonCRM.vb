Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Web
Imports System.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Security.Cryptography
Imports CrystalDecisions.Shared

Public Class ClsCommonCRM
    Dim SqlConn As New SqlConnection
    ' Public objExcel As Excel.Application = Singleton.GetInstance

    'original 
    Public Function FillDataTable_Password(ByVal sqlConn As SqlConnection, ByVal strProc As String, _
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
                SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 8000, "I", , , strSearchCond)
            End If
            SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")

            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Return Nothing
        Finally
            CloseConn()
        End Try
    End Function

    Public Function FillDataTableRuntime_EQUITYCONTACTS(ByVal _IntId As Integer, ByVal stradd As Char) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            Dim ds As New DataTable
            Dim _Strout As String
            sqlComm.CommandText = "CRM_SEARCH_EQUITYCONTACTS"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 8).Value = _IntId
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 8).Value = DBNull.Value
            End If
            If Val(HttpContext.Current.Session("UserId")) <> 0 Then
                sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 8).Value = Val(HttpContext.Current.Session("UserId"))
            Else
                sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 8).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("strAdd", SqlDbType.Char, 1).Value = stradd
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(ds)
            _Strout = sqlComm.Parameters("@RET_CODE").Value.ToString()
            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function


    Public Function FillDataTable(ByVal strProc As String, Optional ByVal _StrSearch As String = "") As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try

            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session("UserId"))
            Else
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            End If
            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 200).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 200).Value = DBNull.Value
            End If

            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Function FillDataTableIssureMaster(ByVal strProc As String, Optional ByVal _StrSearch As String = "") As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session("UserId"))
            Else
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = DBNull.Value 'Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            End If

            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 200).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 200).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function


    Public Function FillDataSecurityIssuer(ByVal strProc As String, Optional ByVal _StrSearch As String = "", Optional ByVal Id As Integer = 0) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            'If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
            If Val(Id) <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = Val(Id)
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = DBNull.Value 'Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            End If

            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function


    Public Function FillDataSecurityName(ByVal strProc As String, Optional ByVal _StrSearch As String = "", Optional ByVal SecIssuer As String = "") As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            'If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
            If SecIssuer <> "" Then
                sqlComm.Parameters.Add("@SecurityIssuer", SqlDbType.VarChar, 200).Value = Trim(SecIssuer)
            Else
                sqlComm.Parameters.Add("@SecurityIssuer", SqlDbType.VarChar, 200).Value = DBNull.Value 'Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            End If

            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Function FillDataSecurityName_FromId(ByVal strProc As String, Optional ByVal _StrSearch As String = "", Optional ByVal SecIssuer As Integer = 0) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            'If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
            If SecIssuer <> 0 Then
                sqlComm.Parameters.Add("@IssuerId", SqlDbType.Int, 4).Value = Val(SecIssuer)
            Else
                sqlComm.Parameters.Add("@IssuerId", SqlDbType.Int, 4).Value = DBNull.Value 'Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            End If

            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function


    Public Function FillCustomer(ByVal strProc As String, Optional ByVal _StrSearch As String = "") As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            'If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
            'If SecIssuer <> "" Then
            '    sqlComm.Parameters.Add("@SecurityIssuer", SqlDbType.VarChar, 200).Value = Trim(SecIssuer)
            'Else
            '    sqlComm.Parameters.Add("@SecurityIssuer", SqlDbType.VarChar, 200).Value = DBNull.Value 'Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            'End If

            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Function FillDataSecurityIssuer_TSS(ByVal strProc As String, Optional ByVal _StrSearch As String = "", Optional ByVal Id As Integer = 0) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("TSSConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            'If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
            If Val(Id) <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = Val(Id)
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = DBNull.Value 'Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            End If

            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function


    Public Function FillDataSecurityName_TSS(ByVal strProc As String, Optional ByVal _StrSearch As String = "", Optional ByVal SecIssuer As String = "") As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("TSSConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            'If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
            If SecIssuer <> "" Then
                sqlComm.Parameters.Add("@SecurityIssuer", SqlDbType.VarChar, 200).Value = Trim(SecIssuer)
            Else
                sqlComm.Parameters.Add("@SecurityIssuer", SqlDbType.VarChar, 200).Value = DBNull.Value 'Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            End If

            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Function FillCustomer_TSS(ByVal strProc As String, Optional ByVal _StrSearch As String = "") As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("TSSConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            'If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
            'If SecIssuer <> "" Then
            '    sqlComm.Parameters.Add("@SecurityIssuer", SqlDbType.VarChar, 200).Value = Trim(SecIssuer)
            'Else
            '    sqlComm.Parameters.Add("@SecurityIssuer", SqlDbType.VarChar, 200).Value = DBNull.Value 'Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            'End If

            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function


    Public Function FillDataTableRuntime_Webmethod(ByVal _IntId As Integer) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("TSSConnectionString").ToString())
        Try
            Dim ds As New DataTable
            Dim _Strout As String
            sqlComm.CommandText = "TRUST_CRM_SEARCH_Contactperson"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@CustomerId", SqlDbType.BigInt, 8).Value = _IntId
            Else
                sqlComm.Parameters.Add("@CustomerId", SqlDbType.BigInt, 8).Value = DBNull.Value
            End If
            'If _IntId <> "" Then
            '    sqlComm.Parameters.Add("@CustomerName", SqlDbType.VarChar, 500).Value = _IntId
            'Else
            '    sqlComm.Parameters.Add("@CustomerName", SqlDbType.VarChar, 500).Value = DBNull.Value
            'End If
            'sqlComm.Parameters.Add("@Para", SqlDbType.VarChar, 10).Value = Para
            'If Convert.ToInt32(HttpContext.Current.Session("UserId")) <> 0 Then
            '    sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session("UserId"))
            'Else
            '    sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = DBNull.Value
            'End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(ds)
            _Strout = sqlComm.Parameters("@RET_CODE").Value.ToString()
            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Function FillCustomerAddress_TSS(ByVal strProc As String, Optional ByVal _StrSearch As String = "", Optional ByVal Id As Integer = 0) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("TSSConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            'If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
            If Val(Id) <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = Val(Id)
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = DBNull.Value 'Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            End If

            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 1000).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Function FillDataTable_WeeklyCustomer(ByVal strProc As String, Optional ByVal _StrSearch As String = "", Optional ByVal _StrDate As String = "") As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim obj As New ClsCommonCRM
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try

            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator" Then
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session("UserId"))
            Else
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session("UserId")) ' DBNull.Value
            End If
            If _StrSearch <> "" Then
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 200).Value = _StrSearch
            Else
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 200).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@WeekDate", SqlDbType.VarChar, 20).Value = DateFormatMMDDYY(_StrDate)
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
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
            Return Nothing
        Finally
            CloseConn()
        End Try
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

    Public Function FillDataTableId(ByVal strProc As String, Optional ByVal _IntId As Integer = 0) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = _IntId
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = DBNull.Value
            End If
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function


    Public Function FillEquityDealer_DataTableId(ByVal strProc As String, Optional ByVal _IntId As Integer = 0) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 8).Value = _IntId
            Else
                sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 8).Value = DBNull.Value
            End If
            SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    ' with long id type
    Public Function FillDataTableId(ByVal strProc As String, ByVal _IntId As Long) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.BigInt, 4).Value = _IntId
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.BigInt, 4).Value = DBNull.Value
            End If
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Function FillDataTableUserId(ByVal strProc As String, ByVal Para As String, Optional ByVal _IntId As Int64 = 0) As DataSet
        Try
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim ds As New DataSet
            Dim _Strout As String
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            OpenConn()
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.BigInt, 8).Value = _IntId
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.BigInt, 8).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@Para", SqlDbType.VarChar, 10).Value = Para
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(ds)
            _Strout = sqlComm.Parameters("@RET_CODE").Value.ToString()

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            CloseConn()
        End Try
    End Function



    Public Function SavePermanentContact(ByVal intCustId As String, ByVal strCntPerson As String, ByVal strBranch As String, _
                                             ByVal strDesig As String, ByVal strTel As String, ByVal strMb As String, _
                                             ByVal stremail As String, ByVal intInteractionId As Integer, ByVal strExpDate As String)
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            Dim ds As New DataTable

            sqlComm.CommandText = "CRM_INSERT_PERMANENTCONTACTS_CRMENTRY"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            SqlConn.Open()
            sqlComm.Parameters.Clear()
            sqlComm.Parameters.Add("@CustomerId", SqlDbType.BigInt, 8).Value = intCustId
            sqlComm.Parameters.Add("@ContactPerson", SqlDbType.VarChar, 100).Value = strCntPerson
            sqlComm.Parameters.Add("@Branch", SqlDbType.VarChar, 100).Value = strBranch
            sqlComm.Parameters.Add("@Designation", SqlDbType.VarChar, 100).Value = strDesig
            sqlComm.Parameters.Add("@TelNo", SqlDbType.VarChar, 50).Value = strTel
            sqlComm.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = strMb
            sqlComm.Parameters.Add("@Email", SqlDbType.VarChar, 200).Value = stremail
            sqlComm.Parameters.Add("@InteractionTypeId", SqlDbType.Int, 4).Value = intInteractionId
            'sqlComm.Parameters.Add("@ExpDate", SqlDbType.SmallDateTime, 4).Value = strExpDate
            If Convert.ToInt32(HttpContext.Current.Session("UserId")) <> 0 Then
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session("UserId"))
            Else
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = DBNull.Value
            End If
            sqlComm.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
            SqlConn.Close()
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function


    Public Function FillDataTableRuntime(ByVal Para As String, Optional ByVal _IntId As Int64 = 0) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            Dim ds As New DataTable
            Dim _Strout As String
            sqlComm.CommandText = "CRM_SEARCH_CONTACTS"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = _IntId
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@Para", SqlDbType.VarChar, 10).Value = Para
            If Convert.ToInt32(HttpContext.Current.Session("UserId")) <> 0 Then
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session("UserId"))
            Else
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(ds)
            _Strout = sqlComm.Parameters("@RET_CODE").Value.ToString()
            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Function FillDataTablePrimaryDealerRuntime(ByVal Para As String, Optional ByVal _IntId As Int64 = 0) As DataSet
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            Dim ds As New DataSet
            Dim _Strout As String
            sqlComm.CommandText = "CRM_SEARCH_CONTACTS"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = _IntId
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@Para", SqlDbType.VarChar, 10).Value = Para
            If Convert.ToInt32(HttpContext.Current.Session("UserId")) <> 0 Then
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session("UserId"))
            Else
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(ds)
            _Strout = sqlComm.Parameters("@RET_CODE").Value.ToString()

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Function FillDataTable_BusinessType(ByVal Para As String, Optional ByVal _IntId As Int64 = 0) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try
            Dim ds As New DataTable
            Dim _Strout As String
            sqlComm.CommandText = "CRM_SEARCH_CUSTOMER_BUSINESSTYPE"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = _IntId
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@Para", SqlDbType.VarChar, 10).Value = Para
            If Convert.ToInt32(HttpContext.Current.Session("UserId")) <> 0 Then
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session("UserId"))
            Else
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(ds)
            _Strout = sqlComm.Parameters("@RET_CODE").Value.ToString()

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Sub OpenConn()
        If SqlConn Is Nothing Then
            SqlConn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
            SqlConn.Open()
        ElseIf SqlConn.State = ConnectionState.Closed Then
            SqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString()
            SqlConn.Open()
        End If
    End Sub
    Public Sub CloseConn()
        If SqlConn Is Nothing Then Exit Sub
        If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
    End Sub

    Public Function DateFormat(ByVal strDate As String) As Date
        If strDate = "" Then
            Return Date.MinValue
        Else
            Return DateSerial(Right(strDate, 4), Mid(strDate, 4, 2), Left(strDate, 2))
        End If
    End Function
    Public Function DateFormatMMDDYY(ByVal strDate As String) As String
        If strDate = "" Then
            Return ""
        Else
            Return Mid(strDate, 4, 2) + "/" + Left(strDate, 2) + "/" + Right(strDate, 4)
        End If
    End Function

    Public Function LoadGrid(ByVal strProc As String, Optional ByVal _IntId As Integer = 0) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        Try

            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = _IntId
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = DBNull.Value
            End If
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

    Public Function LoadCombo(ByVal strProc As String, Optional ByVal _IntId As Integer = 0) As DataTable
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())

        Try
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = SqlConn
            If _IntId <> 0 Then
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = _IntId
            Else
                sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = DBNull.Value
            End If

            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            SqlConn = Nothing
            sqlComm.Dispose()
        End Try
    End Function

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
            Return Nothing
        Finally
        End Try
    End Function

    'Public Function FillInteractionDetails_Week(ByVal strProc As String, ByVal _IntId As Integer, ByVal _search As String, ByVal FromDt As String, ByVal ToDt As String) As DataTable
    '    Dim sqlComm As New SqlCommand
    '    Dim sqlDa As New SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim SqlConn As SqlConnection
    '    SqlConn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
    '    Dim _Strout As String = ""
    '    Try
    '        sqlComm.CommandText = strProc
    '        sqlComm.CommandType = CommandType.StoredProcedure
    '        sqlComm.Connection = SqlConn
    '        If (Convert.ToString(HttpContext.Current.Session("UserType")).ToLower() <> "administrator") Then
    '            sqlComm.Parameters.Add("@UserType", SqlDbType.Char, 1).Value = "U"
    '        Else
    '            sqlComm.Parameters.Add("@UserType", SqlDbType.Char, 1).Value = "A"
    '        End If
    '        sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 8).Value = Val(HttpContext.Current.Session("UserId"))
    '        sqlComm.Parameters.Add("@FromDt", SqlDbType.SmallDateTime, 4).Value = DateFormat(FromDt)
    '        sqlComm.Parameters.Add("@ToDt", SqlDbType.SmallDateTime, 4).Value = DateFormat(ToDt)
    '        If (_search = "") Then
    '            sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = DBNull.Value
    '        Else
    '            sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = _search
    '        End If
    '        sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
    '        sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
    '        sqlDa.SelectCommand = sqlComm
    '        sqlDa.Fill(dt)
    '        _Strout = Convert.ToString(sqlComm.Parameters("@RET_CODE").Value)
    '        Return dt
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        SqlConn.Close()
    '        SqlConn.Dispose()
    '        sqlComm.Dispose()
    '        sqlDa.Dispose()
    '    End Try
    'End Function

    'Public Sub ExportToExcels()
    '    Dim strFileName As String


    '    'The full path where the excel file will be stored
    '    'Dim strFileName As String = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\")
    '    strFileName = (HttpContext.Current.Server.MapPath("") & "\Report.xls")
    '    'strFileName = strFileName & "\MyExcelFile" & System.DateTime.Now.Ticks.ToString() ".xls"


    '    Dim objBooks As Excel.Workbooks, objBook As Excel.Workbook
    '    Dim objSheets As Excel.Sheets
    '    ', objSheet As Excel.Worksheet oSheet
    '    Dim objRange As Excel.Range

    '    Try
    '        'Creating a new object of the Excel application object
    '        objExcel = New Excel.Application
    '        'Hiding the Excel application
    '        objExcel.Visible = False
    '        'Hiding all the alert messages occurring during the process
    '        objExcel.DisplayAlerts = False

    '        'Adding a collection of Workbooks to the Excel object
    '        objBook = CType(objExcel.Workbooks.Add(), Excel.Workbook)


    '        If File.Exists(Server.MapPath("") & "\Report.xls") Then
    '            File.Delete(Server.MapPath("") & "\Report.xls")
    '        End If


    '        'Saving the Workbook as a normal workbook format.
    '        objBook.SaveAs(strFileName, Excel.XlFileFormat.xlWorkbookNormal)

    '        'Getting the collection of workbooks in an object
    '        objBooks = objExcel.Workbooks

    '        'Get the reference to the first sheet in the workbook collection in a variable
    '        oSheet = CType(objBooks(1).Sheets.Item(1), Excel.Worksheet)
    '        oSheet.Name = "Retail"
    '        objRange = oSheet.Cells
    '        SaveReports("QuarterlyDealerRpt_Retail", "Id_RPT_AllQuarterlyDealerRept", "Retail")
    '        'WriteourUploadData()
    '        'WriteArrangerSummary()
    '        objExcel.Workbooks.Open(Server.MapPath("") & "\" & "QuarterlyDealerRpt_Retail" & ".xls")
    '        objExcel.ActiveSheet.Cells.Select()

    '        objExcel.Selection.Copy()
    '        objExcel.Windows("Report.xls").Activate()
    '        objExcel.ActiveSheet.Paste()
    '        oSheet.SaveAs(strFileName)

    '        oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
    '        oSheet.Name = "WDM"
    '        objRange = oSheet.Cells
    '        SaveReports("QuarterlyDealerRpt_WDM", "Id_RPT_AllQuarterlyDealerRept", "WDM")

    '        objExcel.Workbooks.Open(Server.MapPath("") & "\" & "QuarterlyDealerRpt_WDM" & ".xls")
    '        objExcel.ActiveSheet.Cells.Select()

    '        objExcel.Selection.Copy()
    '        objExcel.Windows("Report.xls").Activate()
    '        objExcel.ActiveSheet.Paste()
    '        'WriteArrangerSummary()
    '        oSheet.SaveAs(strFileName)


    '        oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
    '        'oSheet = CType(objBooks(1).Sheets.Item(1), Excel.Worksheet)
    '        'Optionally name the worksheet
    '        'oSheet.Name = "Fee Details"
    '        'You can even set the font attributes of a range of cells in the sheet. Here we have set the fonts to bold.
    '        'oSheet.Range("A1", "Z1").Font.Bold = True

    '        'Get the cells collection of the sheet in a variable, to write the data.
    '        objRange = oSheet.Cells

    '        'Calling the function to write the dataset data in the cells of the first sheet.
    '        'WriteFeeDetailsData("MB_FILL_FEEDETAILS")


    '        'Setting the width of the specified range of cells so as to absolutely fit the written data.
    '        'objSheet.Range("A1", "Z1").EntireColumn.AutoFit()
    '        'Saving the worksheet.
    '        oSheet.SaveAs(strFileName)

    '        objBook = objBooks.Item(1)
    '        objSheets = objBook.Worksheets
    '        'oSheet = CType(objBooks(1).Sheets.Item(2), Excel.Worksheet)

    '        oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
    '        'oSheet.Name = "Our Investor details"
    '        'Setting the color of the specified range of cells to Red (ColorIndex 3 denoted Red color)
    '        'objSheet.Range("A1", "Z1").Font.ColorIndex = 3

    '        objRange = oSheet.Cells
    '        'WriteourInvestmentDetailsData("MB_FILL_OURINVESTORDETAILS")

    '        'WriteourInvestmentDetailsData()

    '        'objSheet.Range("A1", "Z1").EntireColumn.AutoFit()

    '        oSheet.SaveAs(strFileName)

    '        'oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
    '        'oSheet.Name = "Upload Arranger"
    '        ''Setting the color of the specified range of cells to Red (ColorIndex 3 denoted Red color)
    '        ''objSheet.Range("A1", "Z1").Font.ColorIndex = 3

    '        'objRange = oSheet.Cells
    '        ''WriteourInvestmentDetailsData("MB_FILL_OURINVESTORDETAILS")
    '        'SaveReports("SubReport", "MB_FILL_CompreArrangerApplication", "UploadArr")
    '        ''WriteourUploadData()

    '        ''objSheet.Range("A1", "Z1").EntireColumn.AutoFit()

    '        'oSheet.SaveAs(strFileName)


    '        '===========================
    '        objBook = objBooks.Item(1)
    '        objSheets = objBook.Worksheets
    '        oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
    '        oSheet.Name = "MBD"
    '        objRange = oSheet.Cells
    '        'SaveReports("QuarterlyDealerRpt_MBD", "Id_RPT_AllQuarterlyDealerRept", "MBD")
    '        SaveReports("QuarterlyDealerRpt_MBD", "Id_RPT_AllQuarterlyDealerRept", "MBD")
    '        'WriteIssueDetails()

    '        'ChDir("C:\Documents and Settings\Admin\Desktop")
    '        objExcel.Workbooks.Open(Server.MapPath("") & "\" & "QuarterlyDealerRpt_MBD" & ".xls")
    '        objExcel.ActiveSheet.Cells.Select()

    '        objExcel.Selection.Copy()
    '        objExcel.Windows("Report.xls").Activate()
    '        objExcel.ActiveSheet.Paste()
    '        'Dim tempEF As New ExcelFile
    '        'tempEF.LoadXls(Server.MapPath("") & "\" & "IssueDetailsRpt" & ".xls")
    '        'Dim tempWS As ExcelWorksheet = tempEF.Worksheets(0)
    '        'objBook.Worksheets.Copy(, tempWS)

    '        oSheet.SaveAs(strFileName)
    '        'oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
    '        'oSheet.Name = "Upload Arranger"
    '        ''Setting the color of the specified range of cells to Red (ColorIndex 3 denoted Red color)
    '        ''objSheet.Range("A1", "Z1").Font.ColorIndex = 3

    '        'objRange = oSheet.Cells
    '        ''WriteourInvestmentDetailsData("MB_FILL_OURINVESTORDETAILS")
    '        'SaveReports("SubReport", "MB_FILL_CompreArrangerApplication", "UploadArr")

    '        'WriteourUploadData()

    '        ''objSheet.Range("A1", "Z1").EntireColumn.AutoFit()

    '        'oSheet.SaveAs(strFileName)

    '        '=================================================================
    '        For Each xlSheet As Excel.Worksheet In objExcel.ActiveWorkbook.Sheets
    '            If xlSheet.Name.IndexOf("Sheet") <> -1 Then
    '                xlSheet.Delete()
    '            End If
    '        Next


    '        objExcel.ActiveWorkbook.Save()
    '        objExcel.ActiveWorkbook.Close()
    '        'objSheets.Delete()
    '        'objBook.Close()
    '        'objExcel.Workbooks.Close()
    '        objExcel.Quit()
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(objBook)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(objSheets)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(objExcel)
    '        objBook = Nothing
    '        objSheets = Nothing
    '        objExcel = Nothing
    '        GC.Collect()


    '        With HttpContext.Current.Response
    '            .Clear()
    '            .Charset = ""
    '            .ClearHeaders()
    '            .ContentType = "application/vnd.ms-excel"
    '            .AddHeader("content-disposition", "attachment;filename=" + "DealerwiseDetails.xls")
    '            .WriteFile(strFileName)
    '            .Flush()
    '            .End()

    '        End With
    '        Session("dtfill") = ""

    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    Finally
    '        'Close the Excel application

    '        If objExcel IsNot Nothing Then
    '            If objExcel.Workbooks.Count > 0 Then objExcel.ActiveWorkbook.Close()
    '            objExcel.Quit()
    '            objExcel = Nothing
    '            'objExcel.Collect()
    '        End If
    '        'objExcel.Quit()

    '        'Release all the COM objects so as to free the memory
    '        'ReleaseComObject(objRange)
    '        'ReleaseComObject(objSheet)
    '        'ReleaseComObject(objSheets)
    '        'ReleaseComObject(objBook)
    '        'ReleaseComObject(objBooks)
    '        'ReleaseComObject(objExcel)

    '        'Set the all the objects for the Garbage collector to collect them.
    '        objExcel = Nothing
    '        objBooks = Nothing
    '        objBook = Nothing
    '        objSheets = Nothing
    '        oSheet = Nothing
    '        objRange = Nothing

    '        'Specifically call the garbage collector.
    '        System.GC.Collect()
    '    End Try
    'End Sub

    'Public Sub SaveExcels(ByRef xlWorkSheet As Excel.Worksheet, ByVal path As String)
    '    Try
    '        xlWorkSheet.SaveAs(path)
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Public Function DeleteFromMaster(ByVal intUserId As Integer, ByVal intMasterTableId As Integer, ByVal strMasterTableColumnName As String, ByVal strMasterTableName As String) As Integer

        Dim sqlComm As New SqlCommand
        Dim intRetuen As Integer

        Try
            sqlComm.Connection = SqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "CRM_DeleteFromMaster"
            sqlComm.Parameters.Clear()
            sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = intUserId
            sqlComm.Parameters.Add("@MasterTableId", SqlDbType.Int, 4).Value = intMasterTableId
            sqlComm.Parameters.Add("@MasterTableColumnName", SqlDbType.VarChar, 100).Value = strMasterTableColumnName
            sqlComm.Parameters.Add("@MasterTableName", SqlDbType.VarChar, 100).Value = strMasterTableName
            sqlComm.Parameters.Add("@StrMessage", SqlDbType.VarChar, 100)
            sqlComm.Parameters("@StrMessage").Direction = ParameterDirection.Output
            'str = sqlComm.Parameters("@RET_CODE").Value
            OpenConn()
            intRetuen = sqlComm.ExecuteNonQuery()
            Return intRetuen
        Catch ex As Exception
            Throw ex
        Finally
            'CloseConn()
        End Try
    End Function
End Class