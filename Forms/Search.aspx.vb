
Partial Class search
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")

            If Not Page.IsPostBack Then
                Hid_PageName.Value = Trim(Request.QueryString("PageName"))
                Hid_TableName.Value = Trim(Request.QueryString("TableName"))
                Hid_ProcName.Value = Trim(Request.QueryString("ProcName"))
                Hid_SelectedFieldId.Value = Trim(Request.QueryString("SelectedId"))
                Hid_SelectedFieldName.Value = Trim(Request.QueryString("SelectedName"))
                Hid_CondFieldName1.Value = Trim(Request.QueryString("CondFieldName1"))
                Hid_CondFieldValue1.Value = Trim(Request.QueryString("CondFieldValue1"))
                FillGrid()
                'If Hid_CondFieldValue1.Value <> 1 Then
                '    If Trim(Request.QueryString("CondFieldName") & "") <> "" And Trim(Request.QueryString("CondFieldValue") & "") <> "" Then
                '        If Val(Request.QueryString("CondFieldValue")) > 0 Then
                '            Hid_ManualCond.Value = "( " & Request.QueryString("CondFieldName") & " = " & Request.QueryString("CondFieldValue") & " )"
                '        Else
                '            Hid_ManualCond.Value = "(" & Request.QueryString("CondFieldName") & " = '" & Request.QueryString("CondFieldValue") & "')"
                '        End If
                '    End If
                'End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillGrid()
        Try
            Dim blnCondExist As Boolean
            If Trim(Request.QueryString("CondExist") & "") = "true" Then blnCondExist = True
            Dim blnYearComp As Boolean
            If Trim(Request.QueryString("CheckYearComp") & "") = "true" Then blnYearComp = True
            Dim blnComp As Boolean
            If Trim(Request.QueryString("CheckComp") & "") = "true" Then blnComp = True
            If Hid_CondFieldName1.Value = "RedeemedDeal" Then
                If Hid_CondFieldValue1.Value = "R" Then
                    Hid_PageName.Value = "RedeemedSecurity"
                End If
                Hid_CondFieldName1.Value = ""
                Hid_CondFieldValue1.Value = ""
            End If
            Hid_ManualCond.Value = BuildCondition(Trim(Request.QueryString("CondFieldName") & ""), Trim(Request.QueryString("CondFieldValue") & ""), blnCondExist, blnYearComp, Val(Session("YearId") & ""), Val(Session("CompId") & ""), blnComp, Trim(Hid_CondFieldName1.Value & ""), Trim(Hid_CondFieldValue1.Value & ""), Trim(Hid_CondFieldName2.Value & ""), Trim(Hid_CondFieldValue2.Value & ""), Trim(Hid_PageName.Value & ""))

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Function BuildCondition(
                   ByVal strCondFldName As String, ByVal strCondFldValue As String,
                   ByVal blnCondExist As Boolean, ByVal blnYearComp As Boolean,
                   ByVal intYearId As Integer, ByVal intCompId As Integer, ByVal blnComp As Boolean,
                   ByVal strCondFldName1 As String, ByVal strCondFldValue1 As String, ByVal strCondFldName2 As String,
                   ByVal strCondFldValue2 As String, Optional ByVal strPageName As String = "") As String
        Dim strCond As String = ""
        Dim strOption As String = " WHERE "
        If blnCondExist = True Then strOption = " AND "

        If blnYearComp = True Then
            strCond += " CompId = " & intCompId & " AND YearId = " & intYearId
        End If

        If blnComp = True Then
            strCond += " CompId = " & intCompId
        End If

        If strCond.IndexOf("WHERE") <> -1 Then strOption = " AND "

        If strPageName = "PrintDeals" Or strPageName = "ReportSelectionDealDetail.aspx" Or strPageName = "CancelDealNumber" Or strPageName = "FinancialDelivery_TransCode" _
            Or strPageName = "DMatDelivery_TransCode" Or strPageName = "ConvertToPending" Then
            'If Val(strCondFldValue1) <> 1 Then

            If Trim(Session("RestrictedAccess") & "") = "Y" Then
                If strCondFldName <> "" And strCondFldValue <> "" Then
                    If strCond <> "" Then
                        strCond += strOption & strCondFldName & " = '" & strCondFldValue & "' "
                    Else
                        strCond += strCondFldName & " = '" & strCondFldValue & "' "
                    End If

                End If
            End If

        Else
            If strCondFldName <> "" And strCondFldValue <> "" Then
                If strCond <> "" Then
                    strCond += strOption & strCondFldName & " = '" & strCondFldValue & "' "
                Else
                    strCond += strCondFldName & " = '" & strCondFldValue & "' "
                End If
            End If
        End If

        If strCondFldName1 = "DealTransType" And strCondFldValue1 = "T" Then
            If strCondFldName1 <> "" And strCondFldValue1 <> "" Then
                If strCond <> "" Then
                    strCond += strOption & strCondFldName1 & " in ('T')"
                Else
                    strCond += strCondFldName1 & " in ('T')"
                End If
            End If
        Else
            If strPageName = "CancelDealNumber" Or strPageName = "ConvertToPending" Or strPageName = "RetailDebitDeals" Or strPageName = "DMatDelivery_TransCode" Or strPageName = "FinancialDelivery_TransCode" Then

            ElseIf strCondFldName1 <> "" And strCondFldValue1 <> "" And strPageName <> "PrintDeals" And strPageName <> "ReportSelectionDealDetail.aspx" Then
                If strCond <> "" Then
                    strCond += strOption & strCondFldName1 & " = '" & strCondFldValue1 & "' "
                Else
                    strCond += strCondFldName1 & " = '" & strCondFldValue1 & "' "
                End If
            End If
            If strCondFldName2 <> "" And strCondFldValue2 <> "" And strPageName <> "PrintDeals" And strPageName <> "ReportSelectionDealDetail.aspx" Then
                If strCond <> "" Then
                    strCond += strOption & strCondFldName2 & " = '" & strCondFldValue2 & "' "
                Else
                    strCond += strCondFldName2 & " = '" & strCondFldValue2 & "' "
                End If
            End If


            If Trim(Session("RestrictedAccess")) = "Y" Then
                If strPageName = "NameOFClient" Or strPageName = "CustomerMasterNew" Then
                    strCond += "UserId =  " & Val(Session("UserId") & "")
                End If
                If strPageName = "BrokerName" Or strPageName = "BrokerName" Then
                    strCond += "UserId =  " & Val(Session("UserId") & "")
                End If

            End If
        End If
        Return strCond
    End Function
End Class
