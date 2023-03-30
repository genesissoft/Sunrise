Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class clsSearch
    Dim objCommon As New clsCommonFuns
    Dim gv_Details As GridView
    Dim cbo_Name() As DropDownList
    Dim txt_Name() As TextBox
    Dim SqlConn As New SqlConnection

    Public Function ConvertListToString(ByVal Columns As Object, ByVal chrSep As Char) As String
        Dim SB As New StringBuilder
        Dim strReturn As String
        For I As Int16 = 0 To Columns.Count - 1
            SB.Append(Columns(I) & chrSep)
        Next
        strReturn = SB.ToString()
        If strReturn <> "" Then
            strReturn = Mid(strReturn, 1, strReturn.Length - 1)
        End If
        Return strReturn
    End Function
    'testing
    Public Function FillGrid( _
        ByVal gvw As GridView, ByVal cbo() As DropDownList, ByVal txt() As TextBox, _
        ByVal strCols As String, Optional ByVal intSourceType As Int16 = 0, _
        Optional ByVal strCondFldName As String = "", Optional ByVal strCondFldValue As String = "", _
        Optional ByVal strTableName As String = "", Optional ByVal strProcName As String = "", _
        Optional ByVal strSort As String = "", Optional ByVal intPageIndex As Int16 = 0, _
        Optional ByVal strSelFieldName As String = "", Optional ByVal intSelId As Integer = 0, _
        Optional ByVal blnCondExist As Boolean = False, Optional ByVal blnYearComp As Boolean = False, _
        Optional ByVal intYearId As Integer = 0, Optional ByVal intCompId As Integer = 0, _
        Optional ByVal blnPageIsPostBack As Boolean = False, Optional ByVal blnComp As Boolean = False, _
        Optional ByVal strCondFldName1 As String = "", Optional ByVal strCondFldValue1 As String = "", _
        Optional ByVal strCondFldName2 As String = "", Optional ByVal strCondFldValue2 As String = "", Optional ByVal strPage As String = "", Optional ByVal strParentPage As String = "") As DataTable
        gv_Details = gvw
        cbo_Name = cbo
        txt_Name = txt

        Dim strQry As String
        Dim dt As DataTable
        Dim dv As DataView
        Dim strCond As String

        'If cbo.Length = 1 Then
        Dim intCustIdIndex As Int16
        Dim intPMSCustIdIndex As Int16
        intCustIdIndex = strCols.ToLower.IndexOf("customerid")
        intPMSCustIdIndex = strCols.ToLower.IndexOf(".customerid")
        If intPMSCustIdIndex <> -1 Then
            strCols = strCols.Substring(0, strCols.LastIndexOf(",")) & ",0 as CustomerIndex" & strCols.Substring(strCols.LastIndexOf(","), strCols.Length - strCols.LastIndexOf(","))
            If txt(0).Text <> "" Then
                strSort = "CustomerIndex"
                strCols = strCols.Replace("0 as CustomerIndex", "CHARINDEX('" & txt(0).Text & "',CustomerName) AS CustomerIndex")
            End If
        ElseIf intCustIdIndex <> -1 Then
            strCols = strCols.Substring(0, intCustIdIndex - 1) & ",0 as CustomerIndex,CustomerId"
            If txt(0).Text <> "" Then
                strSort = "CustomerIndex"
                strCols = strCols.Replace("0 as CustomerIndex", "CHARINDEX('" & txt(0).Text & "',CustomerName) AS CustomerIndex")
            End If
        End If
        'End If
        If strCondFldName1 = "RedeemedDeal" Then

            If strCondFldValue1 = "R" Then
                strProcName = "ID_SEARCH_RedeemedSecurity"
            End If
            strCondFldName1 = ""
            strCondFldValue1 = ""

        End If

        strCond = BuildCondition(strCondFldName, strCondFldValue, blnCondExist, blnYearComp, intYearId, intCompId, blnComp, strCondFldName1, strCondFldValue1, strCondFldName2, strCondFldValue2, strPage)

        'ID_SEARCH_BTBDealSlipNoOrdByDealdate

        If intSourceType = 1 Then
            strQry = "SELECT " & strCols & " FROM " & strTableName & strCond
            dt = objCommon.FillDynamicTable(SqlConn, strQry)
        Else

            If strPage = "DealPurchaseshow.aspx" Then
            Else
                If strCondFldName1 = "DSE.DealTransType" And (strCondFldValue1 = "T" Or strCondFldValue1 = "D" Or strCondFldValue1 = "M" Or strCondFldValue1 = "O") Then
                    If strCondFldName1 <> "" And strCondFldValue1 <> "" Then
                        strProcName = "ID_SEARCH_BTBDealSlipNoFortrading"
                    End If
                End If

            End If

        End If

        dt = FillDataTable(SqlConn, strProcName, strCols, strCond, strSelFieldName, intSelId)
        If dt IsNot Nothing Then
            If blnPageIsPostBack = False Then
                FillFieldCombo(dt)
            End If

            SetGrid(gv_Details, dt)
            dv = dt.DefaultView

            'If strSort <> "DealDate" Then
            '    strSort = strSort & " DESC"
            'End If

            'If strSort <> "" And strSort <> "DealDate" And strSort <> "CustomerAddress1" Then dv.Sort = strSort


            gv_Details.PageIndex = intPageIndex

            If ((strPage = "DealPurchaseshow.aspx" And strProcName = "ID_SEARCH_BTBDealSlipNoOrdByDealdate") Or (strPage = "DealSlipEntry.aspx" And strProcName = "ID_SEARCH_BTBDealSlipNoFortrading") Or (strPage = "DealSlipEntry.aspx" And strProcName = "ID_SEARCH_FinancialDealSlipNo") Or (strPage = "Dealsellshow.aspx" And strProcName = "ID_SEARCH_BTBDealSlipNoNewFinancial") Or (strPage = "WDMDealSlipEntry.aspx" And strProcName = "ID_SEARCH_WDMBTBDealSlipNo")) Then
                dt.Columns.Remove("DealDateTime")
            End If
            If strPage = "PrintDeals.aspx" Then
                dt.Columns.Remove("DealDate")
            End If
            gv_Details.DataSource = dv
            gv_Details.DataBind()

            Return dt
        End If
       

    End Function

    Public Function FillDataTable(ByVal sqlConn As SqlConnection, ByVal strProc As String, _
                                   ByVal strColList As String, ByVal strSearchCond As String, _
                                   Optional ByVal strSelFieldName As String = "", _
                                   Optional ByVal intSelId As Integer = 0) As DataTable
        Try
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dt As New DataTable
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            OpenConn()
            sqlComm.Connection = sqlConn
            If intSelId <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@SelectField", SqlDbType.VarChar, 50, "I", , , strSelFieldName)
                objCommon.SetCommandParameters(sqlComm, "@SelectId", SqlDbType.BigInt, 8, "I", , , intSelId)
            End If
            objCommon.SetCommandParameters(sqlComm, "@ColList", SqlDbType.VarChar, 4000, "I", , , strColList)
            objCommon.SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 4000, "I", , , strSearchCond)
            objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
            Dim error_ As String = ex.Message
        Finally
            CloseConn()
        End Try
    End Function

    Private Function BuildCondition( _
                    ByVal strCondFldName As String, ByVal strCondFldValue As String, _
                    ByVal blnCondExist As Boolean, ByVal blnYearComp As Boolean, _
                    ByVal intYearId As Integer, ByVal intCompId As Integer, ByVal blnComp As Boolean, _
                    ByVal strCondFldName1 As String, ByVal strCondFldValue1 As String, ByVal strCondFldName2 As String, ByVal strCondFldValue2 As String, Optional ByVal strPageName As String = "") As String
        Dim strCond As String = ""
        Dim strOption As String = " WHERE "
        If blnCondExist = True Then strOption = " AND "

        If blnYearComp = True Then
            strCond += strOption & " DSE.CompId = " & intCompId & " AND DSE.YearId = " & intYearId
        End If



        If blnComp = True Then
            strCond += strOption & " DSE.CompId = " & intCompId
        End If

        If strCond.IndexOf("WHERE") <> -1 Then strOption = " AND "

        If strPageName = "PrintDeals.aspx" Or strPageName = "ReportSelectionDealDetail.aspx" Then
            If Val(strCondFldValue1) <> 1 Then
                If strCondFldName <> "" And strCondFldValue <> "" Then
                    strCond += strOption & strCondFldName & " = '" & strCondFldValue & "' "
                End If
            End If

        Else
            If strCondFldName <> "" And strCondFldValue <> "" Then
                strCond += strOption & strCondFldName & " = '" & strCondFldValue & "' "
            End If
        End If

       

        If strCondFldName1 = "DSE.DealTransType" And (strCondFldValue1 = "T" Or strCondFldValue1 = "D" Or strCondFldValue1 = "M" Or strCondFldValue1 = "O") Then
            If strCondFldName1 <> "" And strCondFldValue1 <> "" Then
                strCond += strOption & strCondFldName1 & " in ('T','D','M','O')"
                'strProcName = "ID_SEARCH_BTBDealSlipNoFortrading"
            End If


        Else
            If strCondFldName1 <> "" And strCondFldValue1 <> "" And strPageName <> "PrintDeals.aspx" And strPageName <> "ReportSelectionDealDetail.aspx" Then
                strCond += strOption & strCondFldName1 & " = '" & strCondFldValue1 & "' "
            End If
            If strCondFldName2 <> "" And strCondFldValue2 <> "" And strPageName <> "PrintDeals.aspx" And strPageName <> "ReportSelectionDealDetail.aspx" Then
                strCond += strOption & strCondFldName2 & " = '" & strCondFldValue2 & "' "
            End If
        End If


        If txt_Name Is Nothing Then Return strCond
        For I As Int16 = 0 To txt_Name.Length - 1
            Dim cbo As DropDownList = cbo_Name(I)
            Dim txt As TextBox = txt_Name(I)
            If Trim(txt.Text) <> "" And cbo.SelectedValue <> "" Then
                If strCond.IndexOf("WHERE") <> -1 Then strOption = " AND "

                If cbo.SelectedValue.ToLower.IndexOf("t") <> -1 Then
                    strCond += strOption & cbo.SelectedValue & " LIKE '%" & Trim(txt.Text) & "%'"
                ElseIf cbo.SelectedValue.ToLower.IndexOf("customername") <> -1 Then
                    strCond += strOption & cbo.SelectedValue & " LIKE '%" & Trim(txt.Text) & "%'"
                ElseIf cbo.SelectedValue.ToLower.IndexOf("issuername") <> -1 Then
                    strCond += strOption & cbo.SelectedValue & " LIKE '%" & Trim(txt.Text) & "%'"
                ElseIf cbo.SelectedValue.ToLower.IndexOf("no") <> -1 Or cbo.SelectedValue.ToLower.IndexOf("number") <> -1 Then
                    strCond += strOption & cbo.SelectedValue & " LIKE '%" & Trim(txt.Text) & "%'"
                Else
                    strCond += strOption & cbo.SelectedValue & " LIKE '" & Trim(txt.Text) & "%'"
                End If

            End If
        Next
        Return strCond
    End Function

    Private Sub FillFieldCombo(ByVal dt As DataTable)
        If cbo_Name Is Nothing Then Exit Sub
        For I As Int16 = 0 To txt_Name.Length - 1
            Dim cbo As DropDownList = cbo_Name(I)
            cbo.Items.Clear()
            For J As Int16 = 0 To dt.Columns.Count - 2
                cbo.Items.Add(New ListItem(dt.Columns(J).ColumnName, dt.Columns(J).ColumnName))
            Next
        Next
    End Sub

    Public Sub SetGrid(ByVal grdView As GridView, ByVal dt As DataTable)
        If dt.Rows.Count > grdView.PageSize Then
            grdView.ShowFooter = False
        ElseIf dt.Rows.Count = 0 Then
            grdView.ShowFooter = False
        Else
            grdView.ShowFooter = True
        End If
        If dt.Rows.Count = 0 Then
            Dim dr As DataRow = dt.NewRow()
            For I As Int16 = 0 To dt.Columns.Count - 1
                dr.Item(I) = DBNull.Value
            Next
            dt.Rows.Add(dr)
            'Else
            '    If dt.Rows(0).Item(0).ToString = "" Then
            '        dt.Rows.RemoveAt(0)
            '    End If
        End If
    End Sub

    Public Sub SetGrid1(ByVal grdView As GridView, ByVal dt As DataTable)
        If dt.Rows.Count > grdView.PageSize Then
            grdView.ShowFooter = False
        ElseIf dt.Rows.Count = 0 Then
            grdView.ShowFooter = False
        Else
            grdView.ShowFooter = True
        End If
        If dt.Rows.Count = 0 Then
            Dim dr As DataRow = dt.NewRow()
            For I As Int16 = 0 To dt.Columns.Count - 1
                dr.Item(I) = DBNull.Value
            Next
            dt.Rows.Add(dr)
        Else
            If dt.Rows(0).Item(0).ToString = "" Then
                dt.Rows.RemoveAt(0)
            End If
        End If
    End Sub

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

End Class