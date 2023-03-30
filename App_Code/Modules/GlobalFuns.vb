Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System
Imports log4net
'Imports Microsoft.Office.Interop
Imports YieldCalc
Imports System.Configuration
Imports System.Collections.Generic
'Imports Microsoft.Web.UI

Public Module GlobalFuns
    'Public objExcel As Excel.Application
    'Public objExcel As Excel.Application = Singleton.GetInstance
    Public objCommon As New clsCommonFuns
    Public objComm As New ClsCommon
    Dim sqlConn As SqlConnection
    Dim PgName As String = "$YieldCalculater$"
    Public dblYield As Double
    Public dblYTMSemi As Double
    Public dblYTMAnn As Double
    Public dblYTCSemi As Double
    Public dblYTCAnn As Double
    Public dblYTPSemi As Double
    Public dblYTPAnn As Double
    Public dblPTM As Double
    Public dblPTC As Double
    Public dblPTP As Double
    Public strCashAmount As String = ""
    Public strCashDate As String = ""
    Public decMarketRate As Decimal
    Public XirrDate() As Date
    Public XirrAmt() As Double
    Public CntXirr As Int32 = 0
    Dim MonthDays() As Int32 = {0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
    Public blnRptRange As Boolean
    Public blnRptHeading As Boolean
    Public strRecordSelection As String
    Public blnLandscape As Boolean
    Public chrMaxActFlag As Char
    Dim FstIntDate As Date
    Dim objUtil As New Util
    Dim newdate As Date

    Public Sub SetExcelObject()
        'objExcel = New Excel.Application
        'objExcel.RegisterXLL(objExcel.LibraryPath & "\Analysis\ANALYS32.XLL")
    End Sub

    '-original
    'Public Sub CalculateYield( _
    '            ByVal YTMDate As Date, ByVal FaceValue As Double, ByVal Rate As Double, _
    '            ByVal NonGovernmentFlag As Boolean, ByVal RateActualFlag As Boolean, _
    '            ByVal MaturityDate As Date, ByVal MaturityAmt As Double, _
    '            ByVal CouponDate As Date, ByVal CouponRate As Double, _
    '            ByVal CallDate As Date, ByVal CallAmt As Double, _
    '            ByVal PutDate As Date, ByVal PutAmt As Double, ByVal intFrequency As Int16, _
    '            ByVal strOption As String, ByVal dblResult As Double, ByVal strSemiAnnFlag As String)
    '    Dim decMarketValue As Double
    '    Dim intBasis As Int16

    '    decMarketValue = IIf(RateActualFlag = True, Rate * FaceValue / 100, Rate)
    '    decMarketValue = decMarketValue * 100 / FaceValue
    '    decMarketValue = objCommon.DecimalFormat(decMarketValue)
    '    MaturityAmt = MaturityAmt * 100 / FaceValue
    '    MaturityAmt = objCommon.DecimalFormat(MaturityAmt)
    '    CallAmt = CallAmt * 100 / FaceValue
    '    CallAmt = objCommon.DecimalFormat(CallAmt)
    '    PutAmt = PutAmt * 100 / FaceValue
    '    PutAmt = objCommon.DecimalFormat(PutAmt)
    '    'intBasis = IIf(NonGovernmentFlag = True, 3, 1)
    '    intBasis = IIf(NonGovernmentFlag = True, 3, 4)

    '    If strOption = "Y" Then
    '        With objExcel
    '            '.Workbooks.Open(objExcel.LibraryPath & "\analysis\atpvbaen.xla")
    '            '.Workbooks("atpvbaen.xla").RunAutoMacros(1)
    '            If MaturityDate <> Date.MinValue And MaturityDate > YTMDate Then
    '                dblYield = CType(objExcel.Run("YIELD", YTMDate, MaturityDate, CouponRate / 100, decMarketValue, MaturityAmt, intFrequency, intBasis), Double)
    '                dblYTMAnn = CType(objExcel.Run("EFFECT", dblYield, intFrequency), Double)
    '                dblYTMSemi = CType(objExcel.Run("NOMINAL", dblYTMAnn, 2), Double)
    '                dblYTMAnn = dblYTMAnn * 100
    '                dblYTMSemi = dblYTMSemi * 100
    '            Else
    '                dblYTMAnn = 0
    '                dblYTMSemi = 0
    '            End If
    '            If CallDate <> Date.MinValue And CallDate > YTMDate Then
    '                dblYield = CType(objExcel.Run("YIELD", YTMDate, CallDate, CouponRate / 100, decMarketValue, CallAmt, intFrequency, intBasis), Double)
    '                dblYTCAnn = CType(objExcel.Run("EFFECT", dblYield, intFrequency), Double)
    '                dblYTCSemi = CType(objExcel.Run("NOMINAL", dblYTCAnn, 2), Double)
    '                dblYTCAnn = dblYTCAnn * 100
    '                dblYTCSemi = dblYTCSemi * 100
    '            Else
    '                dblYTCAnn = 0
    '                dblYTCSemi = 0
    '            End If
    '            If PutDate <> Date.MinValue And PutDate > YTMDate Then
    '                dblYield = CType(objExcel.Run("YIELD", YTMDate, PutDate, CouponRate / 100, decMarketValue, PutAmt, intFrequency, intBasis), Double)
    '                dblYTPAnn = CType(objExcel.Run("EFFECT", dblYield, intFrequency), Double)
    '                dblYTPSemi = CType(objExcel.Run("NOMINAL", dblYTPAnn, 2), Double)
    '                dblYTPAnn = dblYTPAnn * 100
    '                dblYTPSemi = dblYTPSemi * 100
    '            Else
    '                dblYTPAnn = 0
    '                dblYTPSemi = 0
    '            End If
    '        End With
    '    ElseIf strOption = "M" Then
    '        dblResult = GetResult(strSemiAnnFlag, intFrequency, dblResult)
    '        GoalSeek(YTMDate, MaturityDate, MaturityAmt, CouponRate, intFrequency, intBasis, dblResult, RateActualFlag, FaceValue)
    '    ElseIf strOption = "C" Then
    '        dblResult = GetResult(strSemiAnnFlag, intFrequency, dblResult)
    '        GoalSeek(YTMDate, CallDate, CallAmt, CouponRate, intFrequency, intBasis, dblResult, RateActualFlag, FaceValue)
    '    ElseIf strOption = "P" Then
    '        dblResult = GetResult(strSemiAnnFlag, intFrequency, dblResult)
    '        GoalSeek(YTMDate, PutDate, PutAmt, CouponRate, intFrequency, intBasis, dblResult, RateActualFlag, FaceValue)
    '    End If
    'End Sub

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

    Public Sub CalculateYield( _
           ByVal YTMDate As Date, ByVal FaceValue As Double, ByVal Rate As Double, _
           ByVal NonGovernmentFlag As Boolean, ByVal RateActualFlag As Boolean, _
           ByVal MaturityDate As Date, ByVal MaturityAmt As Double, _
           ByVal CouponDate As Date, ByVal CouponRate As Double, _
           ByVal CallDate As Date, ByVal CallAmt As Double, _
           ByVal PutDate As Date, ByVal PutAmt As Double, ByVal intFrequency As Int16, _
           ByVal strOption As String, ByVal dblResult As Double, ByVal strSemiAnnFlag As String)

        Try
            Dim decMarketValue As Double
            Dim intBasis As Int16

            decMarketValue = IIf(RateActualFlag = True, Rate * FaceValue / 100, Rate)
            decMarketValue = decMarketValue * 100 / FaceValue
            decMarketValue = objCommon.DecimalFormat(decMarketValue)
            MaturityAmt = MaturityAmt * 100 / FaceValue
            MaturityAmt = objCommon.DecimalFormat(MaturityAmt)
            CallAmt = CallAmt * 100 / FaceValue
            CallAmt = objCommon.DecimalFormat(CallAmt)
            PutAmt = PutAmt * 100 / FaceValue
            PutAmt = objCommon.DecimalFormat(PutAmt)
            intBasis = IIf(NonGovernmentFlag = True, 3, 4)
            dblYTMAnn = 0
            dblYTMSemi = 0
            dblYTCAnn = 0
            dblYTCSemi = 0
            dblYTPAnn = 0
            dblYTPSemi = 0

            If strOption = "Y" Then
                If MaturityDate <> Date.MinValue And MaturityDate > YTMDate Then
                    dblYield = FinancialFunc.Yield(YTMDate, MaturityDate, (CouponRate / 100), decMarketValue, MaturityAmt, intFrequency, intBasis)
                    dblYTMAnn = FinancialFunc.effect(dblYield, intFrequency)
                    dblYTMSemi = FinancialFunc.nominal(dblYTMAnn, 2)
                    dblYTMAnn = dblYTMAnn * 100
                    dblYTMSemi = dblYTMSemi * 100
                End If

                If CallDate <> Date.MinValue And CallDate > YTMDate Then
                    dblYield = FinancialFunc.Yield(YTMDate, CallDate, (CouponRate / 100), decMarketValue, CallAmt, intFrequency, intBasis)
                    dblYTCAnn = FinancialFunc.effect(dblYield, intFrequency)
                    dblYTCSemi = FinancialFunc.nominal(dblYTCAnn, 2)
                    dblYTCAnn = dblYTCAnn * 100
                    dblYTCSemi = dblYTCSemi * 100
                End If

                If PutDate <> Date.MinValue And PutDate > YTMDate Then
                    dblYield = FinancialFunc.Yield(YTMDate, PutDate, (CouponRate / 100), decMarketValue, PutAmt, intFrequency, intBasis)
                    dblYTPAnn = FinancialFunc.effect(dblYield, intFrequency)
                    dblYTPSemi = FinancialFunc.nominal(dblYTPAnn, 2)
                    dblYTPAnn = dblYTPAnn * 100
                    dblYTPSemi = dblYTPSemi * 100
                End If
            ElseIf strOption = "M" Then
                dblResult = GetResult(strSemiAnnFlag, intFrequency, dblResult)
                decMarketRate = FinancialFunc.Price(YTMDate, MaturityDate, (CouponRate / 100), (dblResult / 100), MaturityAmt, intFrequency, intBasis)
                If RateActualFlag = False Then
                    decMarketRate = (decMarketRate * FaceValue) / 100
                End If
            ElseIf strOption = "C" Then
                dblResult = GetResult(strSemiAnnFlag, intFrequency, dblResult)
                decMarketRate = FinancialFunc.Price(YTMDate, CallDate, (CouponRate / 100), (dblResult / 100), CallAmt, intFrequency, intBasis)
                If RateActualFlag = False Then
                    decMarketRate = (decMarketRate * FaceValue) / 100
                End If
            ElseIf strOption = "P" Then
                dblResult = GetResult(strSemiAnnFlag, intFrequency, dblResult)
                decMarketRate = FinancialFunc.Price(YTMDate, PutDate, (CouponRate / 100), (dblResult / 100), PutAmt, intFrequency, intBasis)
                If RateActualFlag = False Then
                    decMarketRate = (decMarketRate * FaceValue) / 100
                End If
            End If

            'decMarketValue = IIf(RateActualFlag = True, Rate * FaceValue / 100, Rate)
            'decMarketValue = decMarketValue * 100 / FaceValue
            'decMarketValue = objCommon.DecimalFormat(decMarketValue)
            'MaturityAmt = MaturityAmt * 100 / FaceValue
            'MaturityAmt = objCommon.DecimalFormat(MaturityAmt)
            'CallAmt = CallAmt * 100 / FaceValue
            'CallAmt = objCommon.DecimalFormat(CallAmt)
            'PutAmt = PutAmt * 100 / FaceValue
            'PutAmt = objCommon.DecimalFormat(PutAmt)
            ''intBasis = IIf(NonGovernmentFlag = True, 3, 1)
            'intBasis = IIf(NonGovernmentFlag = True, 3, 4)

            'If strOption = "Y" Then
            '    With objExcel
            '        '.Workbooks.Open(objExcel.LibraryPath & "\analysis\atpvbaen.xla")
            '        '.Workbooks("atpvbaen.xla").RunAutoMacros(1)
            '        If MaturityDate <> Date.MinValue And MaturityDate > YTMDate Then
            '            If objExcel.Version = 15.0 Or objExcel.Version = 12.0 Then

            '                dblYield = CType(objExcel.Evaluate("Yield(" & "DATE(" & YTMDate.Year _
            '                & "," & YTMDate.Month & "," & YTMDate.Day & ")" & "," & "DATE(" & _
            '                MaturityDate.Year & "," & MaturityDate.Month & "," & MaturityDate.Day & ")" _
            '                & "," & (CouponRate / 100) & "," & decMarketValue & "," & MaturityAmt _
            '                & "," & intFrequency & "," & intBasis & ")"), Double)

            '                dblYTMAnn = CType(objExcel.WorksheetFunction.Effect(dblYield, intFrequency), Double)
            '                dblYTMSemi = CType(objExcel.WorksheetFunction.Nominal(dblYTMAnn, 2), Double)

            '            Else
            '                dblYield = CType(objExcel.Run("YIELD", YTMDate, MaturityDate, CouponRate / 100, decMarketValue, MaturityAmt, intFrequency, intBasis), Double)
            '                dblYTMAnn = CType(objExcel.Run("EFFECT", dblYield, intFrequency), Double)
            '                dblYTMSemi = CType(objExcel.Run("NOMINAL", dblYTMAnn, 2), Double)
            '            End If

            '            dblYTMAnn = dblYTMAnn * 100
            '            dblYTMSemi = dblYTMSemi * 100
            '        Else
            '            dblYTMAnn = 0
            '            dblYTMSemi = 0
            '        End If

            '        If CallDate <> Date.MinValue And CallDate > YTMDate Then

            '            'dblYield = CType(objExcel.Run("YIELD", YTMDate, CallDate, CouponRate / 100, decMarketValue, CallAmt, intFrequency, intBasis), Double)
            '            'dblYTCAnn = CType(objExcel.Run("EFFECT", dblYield, intFrequency), Double)
            '            'dblYTCSemi = CType(objExcel.Run("NOMINAL", dblYTCAnn, 2), Double)
            '            'If objExcel.Version = 12.0 Then

            '            '    dblYield = CType(objExcel.Evaluate("Yield(" & "DATE(" & YTMDate.Year _
            '            '    & "," & YTMDate.Month & "," & YTMDate.Day & ")" & "," & "DATE(" & _
            '            '    MaturityDate.Year & "," & MaturityDate.Month & "," & MaturityDate.Day & ")" _
            '            '    & "," & (CouponRate / 100) & "," & decMarketValue & "," & MaturityAmt _
            '            '    & "," & intFrequency & "," & intBasis & ")"), Double)

            '            '    dblYTCAnn = CType(objExcel.WorksheetFunction.EFFECT(dblYield, intFrequency), Double)
            '            '    dblYTCSemi = CType(objExcel.WorksheetFunction.NOMINAL(dblYTMAnn, 2), Double)
            '            'Else
            '            If objExcel.Version = 15.0 Or objExcel.Version = 12.0 Then

            '                dblYield = CType(objExcel.Evaluate("Yield(" & "DATE(" & YTMDate.Year _
            '                & "," & YTMDate.Month & "," & YTMDate.Day & ")" & "," & "DATE(" & _
            '                CallDate.Year & "," & CallDate.Month & "," & CallDate.Day & ")" _
            '                & "," & (CouponRate / 100) & "," & decMarketValue & "," & CallAmt _
            '                & "," & intFrequency & "," & intBasis & ")"), Double)

            '                dblYTCAnn = CType(objExcel.WorksheetFunction.Effect(dblYield, intFrequency), Double)
            '                dblYTCSemi = CType(objExcel.WorksheetFunction.Nominal(dblYTCAnn, 2), Double)
            '            Else
            '                dblYield = CType(objExcel.Run("YIELD", YTMDate, CallDate, CouponRate / 100, decMarketValue, CallAmt, intFrequency, intBasis), Double)
            '                dblYTCAnn = CType(objExcel.Run("EFFECT", dblYield, intFrequency), Double)
            '                dblYTCSemi = CType(objExcel.Run("NOMINAL", dblYTCAnn, 2), Double)
            '            End If
            '            'End If

            '            dblYTCAnn = dblYTCAnn * 100
            '            dblYTCSemi = dblYTCSemi * 100
            '        Else
            '            dblYTCAnn = 0
            '            dblYTCSemi = 0
            '        End If

            '        If PutDate <> Date.MinValue And PutDate > YTMDate Then

            '            'dblYield = CType(objExcel.Run("YIELD", YTMDate, PutDate, CouponRate / 100, decMarketValue, PutAmt, intFrequency, intBasis), Double)
            '            'dblYTPAnn = CType(objExcel.Run("EFFECT", dblYield, intFrequency), Double)
            '            'dblYTPSemi = CType(objExcel.Run("NOMINAL", dblYTPAnn, 2), Double)

            '            'If objExcel.Version = 12.0 Then
            '            '    dblYield = CType(objExcel.Evaluate("Yield(" & "DATE(" & YTMDate.Year _
            '            '    & "," & YTMDate.Month & "," & YTMDate.Day & ")" & "," & "DATE(" & _
            '            '    MaturityDate.Year & "," & MaturityDate.Month & "," & MaturityDate.Day & ")" _
            '            '    & "," & (CouponRate / 100) & "," & decMarketValue & "," & MaturityAmt _
            '            '    & "," & intFrequency & "," & intBasis & ")"), Double)

            '            '    dblYTPAnn = CType(objExcel.WorksheetFunction.EFFECT(dblYield, intFrequency), Double)
            '            '    dblYTPSemi = CType(objExcel.WorksheetFunction.NOMINAL(dblYTMAnn, 2), Double)
            '            'Else

            '            If objExcel.Version = 15.0 Or objExcel.Version = 12.0 Then

            '                dblYield = CType(objExcel.Evaluate("Yield(" & "DATE(" & YTMDate.Year _
            '                & "," & YTMDate.Month & "," & YTMDate.Day & ")" & "," & "DATE(" & _
            '                PutDate.Year & "," & PutDate.Month & "," & PutDate.Day & ")" _
            '                & "," & (CouponRate / 100) & "," & decMarketValue & "," & PutAmt _
            '                & "," & intFrequency & "," & intBasis & ")"), Double)

            '                dblYTPAnn = CType(objExcel.WorksheetFunction.Effect(dblYield, intFrequency), Double)
            '                dblYTPSemi = CType(objExcel.WorksheetFunction.Nominal(dblYTPAnn, 2), Double)

            '            Else
            '                dblYield = CType(objExcel.Run("YIELD", YTMDate, PutDate, CouponRate / 100, decMarketValue, PutAmt, intFrequency, intBasis), Double)
            '                dblYTPAnn = CType(objExcel.Run("EFFECT", dblYield, intFrequency), Double)
            '                dblYTPSemi = CType(objExcel.Run("NOMINAL", dblYTPAnn, 2), Double)
            '            End If
            '            'End If

            '            dblYTPAnn = dblYTPAnn * 100
            '            dblYTPSemi = dblYTPSemi * 100
            '        Else
            '            dblYTPAnn = 0
            '            dblYTPSemi = 0
            '        End If
            '    End With
            'ElseIf strOption = "M" Then
            '    dblResult = GetResult(strSemiAnnFlag, intFrequency, dblResult)
            '    GoalSeek(YTMDate, MaturityDate, MaturityAmt, CouponRate, intFrequency, intBasis, dblResult, RateActualFlag, FaceValue)
            'ElseIf strOption = "C" Then
            '    dblResult = GetResult(strSemiAnnFlag, intFrequency, dblResult)
            '    GoalSeek(YTMDate, CallDate, CallAmt, CouponRate, intFrequency, intBasis, dblResult, RateActualFlag, FaceValue)
            'ElseIf strOption = "P" Then
            '    dblResult = GetResult(strSemiAnnFlag, intFrequency, dblResult)
            '    GoalSeek(YTMDate, PutDate, PutAmt, CouponRate, intFrequency, intBasis, dblResult, RateActualFlag, FaceValue)
            'End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateYield", "Error in CalculateYield", "", ex)
            Throw ex
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Function GetResult(ByVal strSemiAnnFlag As String, ByVal intFrequency As Int16, ByVal dblResult As Double) As Double
        Try
            If strSemiAnnFlag = "A" Then
                If intFrequency <> 1 Then
                    dblResult = FinancialFunc.nominal(dblResult / 100, 2) * 100
                    'If objExcel.Version = 15.0 Or objExcel.Version = 12.0 Then
                    '    dblResult = CType(objExcel.WorksheetFunction.Nominal(dblResult / 100, 2), Double) * 100
                    'Else
                    '    dblResult = CType(objExcel.Run("NOMINAL", dblResult / 100, 2), Double) * 100
                    'End If
                End If
            Else
                If intFrequency <> 2 Then
                    dblResult = FinancialFunc.effect(dblResult / 100, 2)
                    dblResult = FinancialFunc.nominal(dblResult, intFrequency) * 100
                    'If objExcel.Version = 15.0 Or objExcel.Version = 12.0 Then
                    '    dblResult = CType(objExcel.WorksheetFunction.Effect(dblResult / 100, intFrequency), Double)
                    '    dblResult = CType(objExcel.WorksheetFunction.Nominal(dblResult, intFrequency), Double) * 100
                    'Else
                    '    dblResult = CType(objExcel.Run("EFFECT", dblResult / 100, 2), Double)
                    '    dblResult = CType(objExcel.Run("NOMINAL", dblResult, intFrequency), Double) * 100
                    'End If
                End If
            End If
            Return dblResult
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetResult", "Error in GetResult", "", ex)
            Throw ex
        End Try

    End Function

    Private Sub GoalSeek( _
                    ByVal YTMDate As Date, ByVal GivenDate As Date, ByVal GivenAmt As Double, _
                    ByVal CouponRate As Double, ByVal intFrequency As Int16, _
                    ByVal intBasis As Int16, ByVal dblResult As Double, ByVal RateActualFlag As Boolean, _
                    ByVal FaceValue As Double)
        Try
            'Dim oBook As Object
            'Dim oSheet As Excel.Worksheet

            'oBook = objExcel.Workbooks.Add
            'objExcel.MaxChange = 0.000000001
            'oSheet = oBook.ActiveSheet
            'oSheet.Range("A1").Value = YTMDate
            'oSheet.Range("A2").Value = GivenDate
            'oSheet.Range("A3").Value = CouponRate / 100
            'oSheet.Range("A4").Value = GivenAmt
            'oSheet.Range("A5").Value = GivenAmt
            'oSheet.Range("A6").Value = intFrequency
            'oSheet.Range("A7").Value = intBasis
            'oSheet.Range("B1").Formula = "=YIELD(A1,A2,A3,A4,A5,A6,A7)*100"
            'oSheet.Range("B1").GoalSeek(dblResult, oSheet.Range("A4"))
            'decMarketRate = objCommon.DecimalFormat(oSheet.Range("A4").Value)
            'If RateActualFlag = False Then
            '    decMarketRate = (decMarketRate * FaceValue) / 100
            'End If
            'oSheet = Nothing
            'oBook = Nothing
        Catch ex As Exception
            Throw ex
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Public Sub GetXIRRResult(ByVal intFrequency As Int32, ByRef dblYTMAnn As Double, ByRef dblYTMSemi As Double)

        Try
            dblYTMAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
            dblYTMSemi = FinancialFunc.nominal(dblYTMAnn, 2)
            dblYTMAnn = dblYTMAnn * 100
            dblYTMSemi = dblYTMSemi * 100

            'Dim I As Integer
            'Dim J As Integer

            'For I = 0 To UBound(XirrDate)
            '    If Not InStr(XirrDate(I), "/") > 0 Then
            '        Exit For
            '    End If
            'Next

            'Dim strDate(I - 1) As String
            'Dim dDate(I - 1) As Date
            'Dim dblAmount(I - 1) As Double
            ''poonam
            'Dim strDays(I - 1) As Double

            'For J = 0 To I - 1
            '    dDate(J) = XirrDate(J)
            '    dblAmount(J) = XirrAmt(J)
            '    strDate(J) = XirrDate(J)

            '    strDays(J) = (XirrDate(J) - DateTime.MinValue).TotalDays
            'Next

            'Try
            '    If objExcel.Version = 15.0 Then
            '        dblYTMAnn = CType(objExcel.WorksheetFunction.Xirr(dblAmount, strDate, 0.1), Double)
            '        dblYTMSemi = CType(objExcel.WorksheetFunction.Nominal(dblYTMAnn, 2), Double)
            '    ElseIf objExcel.Version = 12.0 Then
            '        dblYTMAnn = CType(objExcel.WorksheetFunction.Xirr(dblAmount, strDays, 0.1), Double)
            '        dblYTMSemi = CType(objExcel.WorksheetFunction.Nominal(dblYTMAnn, 2), Double)
            '    Else
            '        dblYTMAnn = CType(objExcel.Run("XIRR", XirrAmt, XirrDate, 0.1), Double)
            '        dblYTMSemi = CType(objExcel.Run("NOMINAL", dblYTMAnn, 2), Double)
            '    End If

            '    dblYTMAnn = dblYTMAnn * 100
            '    dblYTMSemi = dblYTMSemi * 100
            'Catch ex As Exception
            '    Throw ex
            'End Try
        Catch ex As Exception

        End Try

    End Sub

    Public Function GetDDBResult() As Double
        Try
            dblYTMAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
            dblYTMAnn = dblYTMAnn * 100

            'Dim I As Integer
            'Dim J As Integer

            'For I = 0 To UBound(XirrDate)
            '    If Not InStr(XirrDate(I), "/") > 0 Then
            '        Exit For
            '    End If
            'Next

            'Dim strDate(I - 1) As String
            'Dim dDate(I - 1) As Date
            'Dim dblAmount(I - 1) As Double

            'For J = 0 To I - 1
            '    dDate(J) = XirrDate(J)
            '    dblAmount(J) = XirrAmt(J)
            '    strDate(J) = XirrDate(J)
            'Next

            'If objExcel.Version = 15.0 Or objExcel.Version = 12.0 Then
            '    dblYTMAnn = CType(objExcel.WorksheetFunction.Xirr(dblAmount, strDate, 0.1), Double) * 100
            'Else
            '    dblYTMAnn = CType(objExcel.Run("XIRR", XirrAmt, XirrDate, 0.1), Double) * 100
            'End If

            Return dblYTMAnn
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetDDBResult", "Error in GetDDBResult", "", ex)
            Throw ex
        End Try

    End Function

    Public Sub CalculateMMY( _
            ByVal YTMDate As Date, ByVal ParVal As Decimal, ByVal Rate As Decimal, _
            ByVal NonGovernmentFlag As Boolean, ByVal RateActualFlag As Boolean, _
            ByRef MaturityDate() As Date, ByRef MaturityAmt() As Double, _
            ByRef CouponDate() As Date, ByRef CouponRate() As Double, ByVal BKDiff As Integer, _
            ByVal NxtIntDate As Date, ByVal IssueDate As Date, ByVal intFrequency As Int32, ByVal dblMMYRate As Double, _
            ByVal DaysOption As Int16, ByVal chrDaysFlag As Char, Optional ByVal BrokenInterest As Boolean = False, Optional ByVal InterestOnHoliday As Boolean = False, Optional ByVal InterestOnSat As Boolean = False, Optional ByVal MaturityOnHoliday As Boolean = False, Optional ByVal MaturityOnSat As Boolean = False)
        Try
            Dim dblMMYAmt As Double = 0
            Dim dblTemp As Double
            Dim dblYTMAmt As Double
            Dim I As Integer
            Dim intDays As Integer
            If intFrequency = 0 Then
                For I = 0 To UBound(MaturityDate)
                    If MaturityDate(I) <> MaturityDate(UBound(MaturityDate)) Then
                        intDays = DateDiff(DateInterval.Day, MaturityDate(I), MaturityDate(UBound(MaturityDate)))
                        dblMMYAmt = dblMMYAmt + (MaturityAmt(I) * dblMMYRate * intDays / 365)
                        dblMMYAmt = dblMMYAmt + MaturityAmt(I)
                    Else
                        dblMMYAmt = MaturityAmt(I)
                    End If
                Next
                dblYTMAmt = IIf(RateActualFlag = True, Rate * ParVal / 100, Rate)
                intDays = DateDiff(DateInterval.Day, YTMDate, MaturityDate(UBound(MaturityDate)))
                dblYTMAnn = ((dblMMYAmt - dblYTMAmt) * 365) / ((intDays * (dblYTMAmt))) * 100
            Else
                CntXirr = 0
                'change this
                CalculateXIRR(YTMDate, ParVal, Rate, NonGovernmentFlag, RateActualFlag, MaturityDate, _
                              MaturityAmt, CouponDate, CouponRate, BKDiff, NxtIntDate, IssueDate, intFrequency, False, "A", False, DaysOption, chrDaysFlag, BrokenInterest, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                dblMMYAmt = XirrAmt(0) + XirrAmt(1)
                If CntXirr > 2 Then
                    For I = 2 To CntXirr - 2
                        dblTemp = XirrAmt(I) * dblMMYRate * DateDiff(DateInterval.Day, XirrDate(I), XirrDate(CntXirr)) / 365
                        dblMMYAmt = dblMMYAmt + dblTemp
                    Next
                End If
                For I = 2 To CntXirr
                    dblMMYAmt = dblMMYAmt + XirrAmt(I)
                Next
                dblYTMAnn = (dblMMYAmt * 365) / (DateDiff(DateInterval.Day, XirrDate(0), XirrDate(CntXirr)) * -(XirrAmt(0) + XirrAmt(1))) * 100
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateMMY", "Error in CalculateMMY", "", ex)
            Throw ex
        End Try

    End Sub

    Public Function CalculateMMYMarketRate( _
                ByVal YTMDate As Date, ByVal ParVal As Decimal, ByVal Rate As Decimal, _
                ByVal NonGovernmentFlag As Boolean, ByVal RateActualFlag As Boolean, _
                ByVal GivenDate() As Date, ByVal GivenAmt() As Double, _
                ByRef CouponDate() As Date, ByRef CouponRate() As Double, ByVal BKDiff As Integer, _
                ByVal NxtIntDate As Date, ByVal IssueDate As Date, ByVal intFrequency As Int32, _
                ByVal dblMMYRate As Double, ByVal DaysOption As Int16, ByVal chrDaysFlag As Char, Optional ByVal BrokenInterest As Boolean = False, Optional ByVal InterestOnHoliday As Boolean = False, Optional ByVal InterestOnSat As Boolean = False, Optional ByVal MaturityOnHoliday As Boolean = False, Optional ByVal MaturityOnSat As Boolean = False) As Double
        Try
            Dim I As Integer
            Dim MarketVal As Double = 0
            Dim AccIntDays As Int32
            Dim dblMMYAmt As Double = 0
            Dim intDays As Integer

            AccIntDays = IIf(NonGovernmentFlag = True, 365, 360)
            If intFrequency = 0 Then
                For I = 0 To UBound(GivenDate)
                    If GivenDate(I) <> GivenDate(UBound(GivenDate)) Then
                        intDays = DateDiff(DateInterval.Day, GivenDate(I), GivenDate(UBound(GivenDate)))
                        dblMMYAmt = dblMMYAmt + GivenAmt(I) * dblMMYRate * intDays / AccIntDays
                        dblMMYAmt = dblMMYAmt + GivenAmt(I)
                    Else
                        dblMMYAmt = GivenAmt(I)
                    End If
                Next
                intDays = DateDiff(DateInterval.Day, YTMDate, GivenDate(UBound(GivenDate)))
                MarketVal = dblMMYAmt / ((dblYTMAnn / 100 * (intDays) / AccIntDays) + 1)
            Else
                CntXirr = 0
                'change this
                CalculateXIRR(YTMDate, ParVal, MarketVal, NonGovernmentFlag, RateActualFlag, GivenDate, _
                              GivenAmt, CouponDate, CouponRate, BKDiff, NxtIntDate, IssueDate, intFrequency, False, "A", False, DaysOption, chrDaysFlag, BrokenInterest, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                dblMMYAmt = XirrAmt(0) + XirrAmt(1)
                If CntXirr > 2 Then
                    For I = 2 To CntXirr - 2
                        dblMMYAmt = dblMMYAmt + (XirrAmt(I) * dblMMYRate * (DateDiff(DateInterval.Day, XirrDate(I), XirrDate(CntXirr)) / AccIntDays))
                    Next
                End If
                For I = 2 To CntXirr
                    dblMMYAmt = dblMMYAmt + XirrAmt(I)
                Next
                intDays = DateDiff(DateInterval.Day, XirrDate(0), XirrDate(CntXirr))
                'MarketVal = ((365 * dblMMYAmt) - (dblYTMAnn / 200 * -(XirrAmt(0) + XirrAmt(1)) * (intDays))) / ((dblYTMAnn / 200 * (intDays)) + AccIntDays)
                MarketVal = ((365 * dblMMYAmt) - (dblYTMAnn / (intFrequency * 100) * -(XirrAmt(0) + XirrAmt(1)) * (intDays))) / ((dblYTMAnn / (intFrequency * 100) * (intDays)) + AccIntDays)
            End If
            MarketVal = IIf(RateActualFlag = True, MarketVal * 100 / ParVal, MarketVal)
            Return MarketVal
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateMMYMarketRate", "Error in CalculateMMYMarketRate", "", ex)
            Throw ex
        End Try

    End Function

    'Public Function CalculateIntReceivable( _
    '        ByVal YTMDate As Date, ByVal ParVal As Decimal, ByVal Rate As Decimal, _
    '        ByVal NonGovernmentFlag As Boolean, ByVal RateActualFlag As Boolean, _
    '        ByRef MaturityDate() As Date, ByRef MaturityAmt() As Double, _
    '        ByRef CouponDate() As Date, ByRef CouponRate() As Double, ByVal BKDiff As Integer, _
    '        ByVal NxtIntDate As Date, ByVal IssueDate As Date, ByVal intFrequency As Int32, _
    '        ByVal PurcSettDate As Date, ByVal SellSettDate As Date, _
    '        ByVal CompRateFlag As Boolean, ByVal IPCalcFlag As String, ByVal IPMatFlag As Boolean, _
    '        Optional ByVal DaysOption As Int16 = 365, Optional ByVal chrDaysFlag As Char = "F", _
    '        Optional ByVal SecurityFaceValue As Decimal = 0, Optional ByVal chrIntFlag As Char = "C")

    '    Dim AccIntDays As Int16
    '    Dim CalDate As Date
    '    Dim blnIntChk As Boolean = False
    '    Dim Divisor As Int32 = intFrequency
    '    Dim dblInterestAmt As Double = 0
    '    Dim K As Integer
    '    Dim M As Integer
    '    Dim C As Integer
    '    Dim blnCoupExist As Boolean = False
    '    Dim blnMatExist As Boolean = False
    '    Dim blnOutOfLoop As Boolean = False
    '    Dim MatAmtA As Double
    '    Dim MatAmtP As Double
    '    Dim MatDateP As Date
    '    Dim CouponDateP As Date
    '    Dim CouponRateP As Double
    '    Dim CouponRateA As Double
    '    Dim PrevIntDate As Date

    '    AccIntDays = IIf(NonGovernmentFlag = True, 365, 360)
    '    CalDate = YTMDate
    '    '***************************************************************************************************
    '    ' Code for Evaluating Previous and Next Interest Date
    '    ' If the Settlement Date is between the issue Date and the first int Date

    '    If IssueDate <> Date.MinValue And NxtIntDate > CalDate Then
    '        PrevIntDate = NxtIntDate
    '        blnIntChk = True
    '    Else
    '        GetIntDate(CalDate, Divisor, NxtIntDate, PrevIntDate)
    '        PrevIntDate = Prev_Date(NxtIntDate, Divisor)
    '    End If
    '    '***************************************************************************************************

    '    If NxtIntDate > SellSettDate Then
    '        'dblInterestAmt = ParVal * CouponRate(C) * DaysGap(SellSettDate, CalDate, AccIntDays) / AccIntDays
    '        'dblInterestAmt = dblInterestAmt / 100
    '    Else
    '        C = GetCouponDate(CouponDate, CalDate, UBound(CouponDate))
    '        M = GetMatDate(MaturityDate, MaturityAmt, CalDate, UBound(MaturityDate), ParVal)
    '        K = C
    '        MatAmtA = ParVal

    '        While (NxtIntDate <= SellSettDate)
    '            blnCoupExist = False
    '            blnMatExist = False
    '            If NxtIntDate = SellSettDate Then blnOutOfLoop = True

    '            If NxtIntDate > CouponDate(C) Then
    '                CouponRateP = CouponRate(C)
    '                CouponDateP = CouponDate(C)
    '                C = C + 1
    '                CouponRateA = CouponRate(C)
    '                blnCoupExist = True
    '            End If

    '            If NxtIntDate > MaturityDate(M) Then
    '                MatDateP = MaturityDate(M)
    '                MatAmtP = MatAmtA
    '                ParVal = ParVal - (ParVal * MaturityAmt(M) / SecurityFaceValue)
    '                M = M + 1
    '                MatAmtA = ParVal
    '                blnMatExist = True
    '            End If

    '            '*************************************************************************************
    '            If blnCoupExist = False And blnMatExist = False Then
    '                If Prev_Date(NxtIntDate, Divisor) < CalDate Then
    '                    dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DateDiff("D", CalDate, NxtIntDate) / (AccIntDays))
    '                Else
    '                    If Divisor < 1 Then
    '                        dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) / Divisor)
    '                    Else
    '                        If IPCalcFlag = "E" Then
    '                            dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) / Divisor)
    '                        Else
    '                            dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DateDiff("D", CalDate, NxtIntDate) / (AccIntDays))
    '                        End If
    '                    End If
    '                End If
    '            ElseIf blnCoupExist = True And blnMatExist = False Then
    '                dblInterestAmt = dblInterestAmt + (ParVal * CouponRateP * DaysGap(CouponDateP, CalDate, AccIntDays) / AccIntDays)
    '                dblInterestAmt = dblInterestAmt + (ParVal * CouponRateA * DaysGap(NxtIntDate, CouponDateP, AccIntDays) / AccIntDays)
    '            ElseIf blnCoupExist = False And blnMatExist = True Then
    '                dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRate(C) * DaysGap(MatDateP, CalDate, AccIntDays) / AccIntDays)
    '                dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRate(C) * DaysGap(NxtIntDate, MatDateP, AccIntDays) / AccIntDays)
    '            Else
    '                If MatDateP > CouponDateP Then
    '                    dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateP * DaysGap(CouponDateP, CalDate, AccIntDays) / AccIntDays)
    '                    dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateA * DaysGap(MatDateP, CouponDateP, AccIntDays) / AccIntDays)
    '                    dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(NxtIntDate, MatDateP, AccIntDays) / AccIntDays)
    '                Else
    '                    dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateP * DaysGap(MatDateP, CalDate, AccIntDays) / AccIntDays)
    '                    dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateP * DaysGap(CouponDateP, MatDateP, AccIntDays) / AccIntDays)
    '                    dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(NxtIntDate, CouponDateP, AccIntDays) / AccIntDays)
    '                End If
    '            End If
    '            dblInterestAmt = dblInterestAmt / 100

    '            If NxtIntDate = MaturityDate(M) And MaturityDate(M) <> SellSettDate Then
    '                MatDateP = MaturityDate(M)
    '                MatAmtP = MatAmtA
    '                ParVal = ParVal - (ParVal * MaturityAmt(M) / SecurityFaceValue)
    '                M = M + 1
    '                MatAmtA = ParVal
    '                dblInterestAmt = dblInterestAmt + (MatAmtP - ParVal)
    '            End If
    '            If NxtIntDate = CouponDate(C) And CouponDate(C) <> SellSettDate Then
    '                CouponRateP = CouponRate(C)
    '                CouponDateP = CouponDate(C)
    '                C = C + 1
    '                CouponRateA = CouponRate(C)
    '            End If
    '            GetNextPrevDate(SellSettDate, Divisor, NxtIntDate, PrevIntDate)
    '        End While

    '        If PrevIntDate < SellSettDate And PrevIntDate >= PurcSettDate Then
    '            blnCoupExist = False
    '            blnMatExist = False
    '            If SellSettDate > CouponDate(C) Then
    '                CouponRateP = CouponRate(C)
    '                CouponDateP = CouponDate(C)
    '                C = C + 1
    '                CouponRateA = CouponRate(C)
    '                blnCoupExist = True
    '            End If
    '            If SellSettDate > MaturityDate(M) Then
    '                MatDateP = MaturityDate(M)
    '                MatAmtP = MatAmtA
    '                ParVal = ParVal - MaturityAmt(M)
    '                M = M + 1
    '                MatAmtA = ParVal
    '                blnMatExist = True
    '            End If
    '            If blnCoupExist = False And blnMatExist = False Then
    '                dblInterestAmt = dblInterestAmt + ((ParVal * CouponRate(C) * DaysGap(SellSettDate, PrevIntDate, AccIntDays) / AccIntDays) / 100)
    '            ElseIf blnCoupExist = True And blnMatExist = False Then
    '                dblInterestAmt = dblInterestAmt + ((ParVal * CouponRateP * DaysGap(CouponDateP, PrevIntDate, AccIntDays) / AccIntDays) / 100)
    '                dblInterestAmt = dblInterestAmt + ((ParVal * CouponRateA * DaysGap(SellSettDate, CouponDateP, AccIntDays) / AccIntDays) / 100)
    '            ElseIf blnCoupExist = False And blnMatExist = True Then
    '                dblInterestAmt = dblInterestAmt + ((MatAmtP * CouponRate(C) * DaysGap(MatDateP, PrevIntDate, AccIntDays) / AccIntDays) / 100)
    '                dblInterestAmt = dblInterestAmt + ((MatAmtA * CouponRate(C) * DaysGap(SellSettDate, MatDateP, AccIntDays) / AccIntDays) / 100)
    '            Else
    '                If MatDateP > CouponDateP Then
    '                    dblInterestAmt = dblInterestAmt + ((MatAmtP * CouponRateP * DaysGap(CouponDateP, PrevIntDate, AccIntDays) / AccIntDays) / 100)
    '                    dblInterestAmt = dblInterestAmt + ((MatAmtP * CouponRateA * DaysGap(MatDateP, CouponDateP, AccIntDays) / AccIntDays) / 100)
    '                    dblInterestAmt = dblInterestAmt + ((MatAmtA * CouponRateA * DaysGap(SellSettDate, MatDateP, AccIntDays) / AccIntDays) / 100)
    '                Else
    '                    dblInterestAmt = dblInterestAmt + ((MatAmtP * CouponRateP * DaysGap(MatDateP, PrevIntDate, AccIntDays) / AccIntDays) / 100)
    '                    dblInterestAmt = dblInterestAmt + ((MatAmtA * CouponRateP * DaysGap(CouponDateP, MatDateP, AccIntDays) / AccIntDays) / 100)
    '                    dblInterestAmt = dblInterestAmt + ((MatAmtA * CouponRateA * DaysGap(SellSettDate, CouponDateP, AccIntDays) / AccIntDays) / 100)
    '                End If
    '            End If
    '        End If
    '    End If
    '    Return dblInterestAmt
    'End Function
    Public Function CalculateIntReceivable( _
             ByVal YTMDate As Date, ByVal ParVal As Decimal, ByVal Rate As Decimal, _
             ByVal NonGovernmentFlag As Boolean, ByVal RateActualFlag As Boolean, _
             ByRef MaturityDate() As Date, ByRef MaturityAmt() As Double, _
             ByRef CouponDate() As Date, ByRef CouponRate() As Double, ByVal BKDiff As Integer, _
             ByVal NxtIntDate As Date, ByVal IssueDate As Date, ByVal intFrequency As Int32, _
             ByVal PurcSettDate As Date, ByVal SellSettDate As Date, _
             ByVal CompRateFlag As Boolean, ByVal IPCalcFlag As String, ByVal IPMatFlag As Boolean, _
             Optional ByVal AccIntDays As Int16 = 0, Optional ByVal chrDaysFlag As Char = "F", _
             Optional ByVal SecurityFaceValue As Decimal = 0, Optional ByVal chrPurIntFlag As Char = "C", _
             Optional ByVal chrSellIntFlag As Char = "A", Optional ByVal chrCalcFlag As Char = "N", Optional ByVal InterestOnHoliday As Boolean = False, Optional ByVal InterestOnSat As Boolean = False, Optional ByVal MaturityOnHoliday As Boolean = False, Optional ByVal MaturityOnSat As Boolean = False)


        'Dim AccIntDays As Int16
        Try
            Dim CalDate As Date
            Dim blnIntChk As Boolean = False
            Dim Divisor As Int32 = intFrequency
            Dim dblInterestAmt As Double = 0
            Dim K As Integer
            Dim M As Integer
            Dim C As Integer
            Dim blnCoupExist As Boolean = False
            Dim blnMatExist As Boolean = False
            Dim blnOutOfLoop As Boolean = False
            Dim MatAmtA As Double
            Dim MatAmtP As Double
            Dim MatDateP As Date
            Dim CouponDateP As Date
            Dim CouponRateP As Double
            Dim CouponRateA As Double
            Dim PrevIntDate As Date
            Dim FirstIntDate As Date
            Dim ParVal_Org As Decimal = ParVal
            Dim Ratio As Double
            Dim AddDays As Int32
            Dim SecurityFaceValue_new As Decimal = SecurityFaceValue

            AddDays = 0

            FirstIntDate = NxtIntDate
            'devendratesting
            FstIntDate = NxtIntDate
            IPCalcFlag = IIf(NonGovernmentFlag = True, "A", "E")
            CalDate = YTMDate

            'nextintdate/caldate holiday calc

            '***************************************************************************************************
            ' Code for Evaluating Previous and Next Interest Date
            ' If the Settlement Date is between the issue Date and the first int Date

            If IssueDate <> Date.MinValue And NxtIntDate > CalDate Then
                PrevIntDate = IssueDate
                blnIntChk = True
            Else
                GetIntDate(CalDate, Divisor, NxtIntDate, PrevIntDate)
                PrevIntDate = Prev_Date(NxtIntDate, Divisor)
            End If
            '***************************************************************************************************
            If chrPurIntFlag = "E" Then
                CalDate = NxtIntDate
                GetIntDate(CalDate, Divisor, NxtIntDate, PrevIntDate)
                PrevIntDate = Prev_Date(NxtIntDate, Divisor)
            End If



            'Tmp_fromdate = SellSettDate
            'Tmp_todate = NxtIntDate

            If NxtIntDate > SellSettDate Then
                If chrSellIntFlag = "A" Then
                    dblInterestAmt = 0
                    Return Format(dblInterestAmt, "###################0.00")
                ElseIf chrPurIntFlag = "E" And chrSellIntFlag = "E" And PurcSettDate > PrevIntDate Then
                    dblInterestAmt = 0
                    Return Format(dblInterestAmt, "###################0.00")
                    'ElseIf chrSellIntFlag = "E" Then
                    '    Fromdate = CalDate
                    '    Todate = NxtIntDate
                End If
            End If

            CalDate = PrevIntDate
            'original start test
            C = GetCouponDate(CouponDate, CalDate, UBound(CouponDate))
            M = GetMatDate(MaturityDate, MaturityAmt, CalDate, UBound(MaturityDate), SecurityFaceValue_new, YTMDate)
            K = C
            MatAmtA = SecurityFaceValue_new
            Ratio = (ParVal * MatAmtA / SecurityFaceValue) / ParVal * 100
            ParVal = (ParVal * MatAmtA / SecurityFaceValue)
            SecurityFaceValue = SecurityFaceValue_new
            MatAmtA = ParVal
            'original end test

            While (NxtIntDate <= SellSettDate)
                blnCoupExist = False
                blnMatExist = False
                If NxtIntDate = SellSettDate Then blnOutOfLoop = True

                If NxtIntDate > CouponDate(C) And (CouponDate.Length) - 1 > (C) Then
                    CouponRateP = CouponRate(C)
                    CouponDateP = CouponDate(C)
                    C = C + 1
                    CouponRateA = CouponRate(C)
                    blnCoupExist = True
                End If

                If UBound(MaturityDate) > -1 Then
                    If NxtIntDate > MaturityDate(M) And (MaturityDate.Length) - 1 > (M) Then
                        MatDateP = MaturityDate(M)
                        MatAmtP = MatAmtA
                        ParVal = ParVal - (ParVal * MaturityAmt(M) / SecurityFaceValue)
                        SecurityFaceValue = SecurityFaceValue - MaturityAmt(M)
                        M = M + 1
                        MatAmtA = ParVal
                        blnMatExist = True
                    End If
                End If
                '*************************************************************************************
                Dim intDeno As Int16
                If chrCalcFlag = "A" Then
                    intDeno = GetIPIssueDiff(intFrequency, FirstIntDate, IssueDate, AccIntDays, CouponRate(C), Ratio, ParVal, dblInterestAmt, Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AddDays)
                Else
                    'intDeno = Denominator
                    intDeno = AccIntDays
                End If
                'NxtIntDate = Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat)
                'CalDate = Interest_Date(CalDate, InterestOnHoliday, InterestOnSat)
                CalDate = PrevIntDate
                If blnCoupExist = False And blnMatExist = False Then
                    If Prev_Date(NxtIntDate, Divisor) < FirstIntDate And CalDate = IssueDate Then
                        If NonGovernmentFlag = True Then
                            dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / intDeno)
                            'dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DaysGap(NxtIntDate, CalDate, AccIntDays) * DaysGap(NxtIntDate, PrevIntDate, AccIntDays) / AccIntDays)
                        Else
                            dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / intDeno)
                        End If
                    Else
                        If Prev_Date(NxtIntDate, Divisor) < CalDate Then
                            'dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DateDiff("D", CalDate, NxtIntDate) / (AccIntDays))
                            dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / intDeno)
                        Else
                            If Divisor < 1 Then
                                dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) / Divisor)
                            Else
                                If IPCalcFlag = "E" Then
                                    dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) / Divisor)
                                Else
                                    dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / intDeno)
                                End If
                            End If
                        End If
                    End If
                ElseIf blnCoupExist = True And blnMatExist = False Then
                    dblInterestAmt = dblInterestAmt + (ParVal * CouponRateP * DaysGap(CouponDateP, Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / intDeno)
                    dblInterestAmt = dblInterestAmt + (ParVal * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), CouponDateP, AccIntDays) / intDeno)
                ElseIf blnCoupExist = False And blnMatExist = True Then
                    dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRate(C) * DaysGap(MatDateP, Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / intDeno)
                    dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), MatDateP, AccIntDays) / intDeno)
                Else
                    If MatDateP > CouponDateP Then
                        dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateP * DaysGap(CouponDateP, Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / intDeno)
                        dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateA * DaysGap(MatDateP, CouponDateP, AccIntDays) / intDeno)
                        dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), MatDateP, AccIntDays) / intDeno)
                    Else
                        dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateP * DaysGap(MatDateP, Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / intDeno)
                        dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateP * DaysGap(CouponDateP, MatDateP, AccIntDays) / intDeno)
                        dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), CouponDateP, AccIntDays) / intDeno)
                    End If
                End If

                If UBound(MaturityDate) > -1 Then
                    If NxtIntDate = MaturityDate(M) And MaturityDate(M) <> SellSettDate Then
                        MatDateP = MaturityDate(M)
                        MatAmtP = MatAmtA
                        ParVal = ParVal - (ParVal * MaturityAmt(M) / SecurityFaceValue)
                        SecurityFaceValue = SecurityFaceValue - MaturityAmt(M)
                        M = M + 1
                        MatAmtA = ParVal
                        dblInterestAmt = dblInterestAmt + (MatAmtP - ParVal)
                    End If
                End If
                If NxtIntDate = CouponDate(C) And CouponDate(C) <> SellSettDate Then
                    CouponRateP = CouponRate(C)
                    CouponDateP = CouponDate(C)
                    C = C + 1
                    CouponRateA = CouponRate(C)
                End If
                GetNextPrevDate(SellSettDate, Divisor, NxtIntDate, PrevIntDate)
            End While
            If NxtIntDate > SellSettDate And chrSellIntFlag = "E" Then
                CalDate = PrevIntDate
                C = GetCouponDate(CouponDate, CalDate, UBound(CouponDate))
                M = GetMatDate(MaturityDate, MaturityAmt, CalDate, UBound(MaturityDate), ParVal, YTMDate)
                K = C
                MatAmtA = ParVal


                blnCoupExist = False
                blnMatExist = False
                If NxtIntDate = SellSettDate Then blnOutOfLoop = True

                If NxtIntDate > CouponDate(C) And (CouponDate.Length) - 1 < (C) Then
                    CouponRateP = CouponRate(C)
                    CouponDateP = CouponDate(C)
                    C = C + 1
                    CouponRateA = CouponRate(C)
                    blnCoupExist = True
                End If

                If UBound(MaturityDate) > -1 Then
                    If NxtIntDate > MaturityDate(M) And (MaturityDate.Length) - 1 < (M) Then
                        MatDateP = MaturityDate(M)
                        MatAmtP = MatAmtA
                        ParVal = ParVal - (ParVal * MaturityAmt(M) / SecurityFaceValue)
                        SecurityFaceValue = SecurityFaceValue - MaturityAmt(M)
                        M = M + 1
                        MatAmtA = ParVal
                        blnMatExist = True
                    End If
                End If
                '*************************************************************************************
                If blnCoupExist = False And blnMatExist = False Then
                    If Prev_Date(NxtIntDate, Divisor) < CalDate Then
                        'dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DateDiff("D", CalDate, NxtIntDate) / (AccIntDays))
                        dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / (AccIntDays))
                    Else
                        If Divisor < 1 Then
                            dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) / Divisor)
                        Else
                            If IPCalcFlag = "E" Then
                                dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) / Divisor)
                            Else
                                dblInterestAmt = dblInterestAmt + (ParVal * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / (AccIntDays))
                            End If
                        End If
                    End If
                ElseIf blnCoupExist = True And blnMatExist = False Then
                    dblInterestAmt = dblInterestAmt + (ParVal * CouponRateP * DaysGap(CouponDateP, Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / AccIntDays)
                    dblInterestAmt = dblInterestAmt + (ParVal * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), CouponDateP, AccIntDays) / AccIntDays)
                ElseIf blnCoupExist = False And blnMatExist = True Then
                    dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRate(C) * DaysGap(MatDateP, Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / AccIntDays)
                    dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), MatDateP, AccIntDays) / AccIntDays)
                Else
                    If MatDateP > CouponDateP Then
                        dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateP * DaysGap(CouponDateP, Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / AccIntDays)
                        dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateA * DaysGap(MatDateP, CouponDateP, AccIntDays) / AccIntDays)
                        dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), MatDateP, AccIntDays) / AccIntDays)
                    Else
                        dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateP * DaysGap(MatDateP, Interest_Date(CalDate, InterestOnHoliday, InterestOnSat), AccIntDays) / AccIntDays)
                        dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateP * DaysGap(CouponDateP, MatDateP, AccIntDays) / AccIntDays)
                        dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), CouponDateP, AccIntDays) / AccIntDays)
                    End If
                End If
                dblInterestAmt = dblInterestAmt / 100

                If UBound(MaturityDate) > -1 Then
                    If NxtIntDate = MaturityDate(M) And MaturityDate(M) <> SellSettDate Then
                        MatDateP = MaturityDate(M)
                        MatAmtP = MatAmtA
                        ParVal = ParVal - (ParVal * MaturityAmt(M) / SecurityFaceValue)
                        SecurityFaceValue = SecurityFaceValue - MaturityAmt(M)
                        M = M + 1
                        MatAmtA = ParVal
                        dblInterestAmt = dblInterestAmt + (MatAmtP - ParVal)

                    End If
                Else
                    'dblInterestAmt = dblInterestAmt / 100
                End If
            Else
                dblInterestAmt = dblInterestAmt / 100
            End If


            Return Format(dblInterestAmt, "###################0.00")
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateIntReceivable", "Error in CalculateIntReceivable", "", ex)
            Throw ex
        End Try

    End Function

    Public Sub CalculateXIRR( _
            ByVal YTMDate As Date, ByVal ParVal As Decimal, ByVal Rate As Decimal, _
            ByVal NonGovernmentFlag As Boolean, ByVal RateActualFlag As Boolean, _
            ByRef MaturityDate() As Date, ByRef MaturityAmt() As Double, _
            ByRef CouponDate() As Date, ByRef CouponRate() As Double, ByVal BKDiff As Integer, _
            ByVal NxtIntDate As Date, ByVal IssueDate As Date, ByVal intFrequency As Int32, _
            ByVal CompRateFlag As Boolean, ByVal IPCalcFlag As String, ByVal IPMatFlag As Boolean, _
            Optional ByVal DaysOption As Int16 = 365, Optional ByVal chrDaysFlag As Char = "F", _
            Optional ByVal BrokenInterest As Boolean = False, Optional ByVal InterestOnHoliday As Boolean _
            = False, Optional ByVal InterestOnSat As Boolean = False, Optional ByVal MaturityOnHoliday _
            As Boolean = False, Optional ByVal MaturityOnSat As Boolean = False)
        Try
            Dim AccIntDays As Int16
            Dim CalDate As Date
            Dim LstMatDate As Date
            Dim LstMatAmt As Decimal
            Dim PrevIntDate As Date
            Dim MarketVal As Decimal
            Dim blnIntChk As Boolean = False
            Dim Divisor As Int32 = intFrequency
            Dim dblInterestAmt As Double = 0
            Dim K As Integer
            Dim M As Integer
            Dim C As Integer
            Dim blnCoupExist As Boolean = False
            Dim blnMatExist As Boolean = False
            Dim blnOutOfLoop As Boolean = False
            Dim MatAmtA As Double
            Dim MatAmtP As Double
            Dim MatDateP As Date
            Dim CouponDateP As Date
            Dim CouponRateP As Double
            Dim CouponRateA As Double
            Dim IPMatAmt As Double

            FstIntDate = NxtIntDate
            '***************************************************************************************************
            ' Code for Evaluating Accured Interest Days & Initializing Last Maturity Date and Amount and CalDate

            MarketVal = IIf(RateActualFlag = True, Rate * ParVal / 100, Rate)
            AccIntDays = IIf(NonGovernmentFlag = True, 365, 360)
            CalDate = YTMDate
            LstMatDate = MaturityDate(UBound(MaturityDate))
            LstMatAmt = MaturityAmt(UBound(MaturityAmt))
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for Evaluating Previous and Next Interest Date
            ' If the Settlement Date is between the issue Date and the first int Date

            If IssueDate <> Date.MinValue And NxtIntDate > CalDate Then
                PrevIntDate = NxtIntDate
                blnIntChk = True
            Else
                GetIntDate(CalDate, Divisor, NxtIntDate, PrevIntDate)
                PrevIntDate = Prev_Date(NxtIntDate, Divisor)
            End If
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for Intializing Array Size

            Dim intSize As Int64
            intSize = (DateDiff("YYYY", NxtIntDate, LstMatDate) * Divisor) + 30
            ReDim XirrAmt(intSize)
            ReDim XirrDate(intSize)
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for Determining either Ex-interest or Cum-Interest
            ' Ex-Interest is to pay to the buyer and Cum-Interest is to take from the buyer

            If DateDiff("D", YTMDate, NxtIntDate) < BKDiff Then
                'Ex-interest
                CalDate = YTMDate
            Else
                'Cum-Interest
                If blnIntChk Then
                    CalDate = IssueDate
                Else
                    CalDate = Interest_Date(PrevIntDate, InterestOnHoliday, InterestOnSat)
                End If
                NxtIntDate = YTMDate
            End If
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for Intializing Array Starting Element

            C = GetCouponDate(CouponDate, CalDate, UBound(CouponDate))
            M = GetMatDate(MaturityDate, MaturityAmt, CalDate, UBound(MaturityDate), ParVal, YTMDate)
            K = C
            MatAmtA = ParVal
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for determining the Current & Previous Date & Rate of Coupon & Maturity

            If NxtIntDate > CouponDate(C) And (CouponDate.Length) - 1 < (C) Then
                CouponRateP = CouponRate(C)
                CouponDateP = CouponDate(C)
                C = C + 1
                CouponRateA = CouponRate(C)
                blnCoupExist = True
            End If

            If NxtIntDate > Maturity_Date(MaturityDate(M), MaturityOnHoliday, MaturityOnSat) And (MaturityDate.Length) - 1 < (M) Then
                MatDateP = Maturity_Date(MaturityDate(M), MaturityOnHoliday, MaturityOnSat)
                XirrAmt(CntXirr) = MaturityAmt(M)
                XirrDate(CntXirr) = Maturity_Date(MaturityDate(M), MaturityOnHoliday, MaturityOnSat)
                CntXirr = CntXirr + 1
                MatAmtP = MatAmtA
                ParVal = ParVal - MaturityAmt(M)
                M = M + 1
                MatAmtA = ParVal
                blnMatExist = True
            End If
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for conditionally calculating interest amount according to the existance coupon & maturity

            'new code for 365 & 366 days implementation
            If Date.IsLeapYear(Year(NxtIntDate)) = True And chrDaysFlag <> "F" And DaysOption = 366 And NonGovernmentFlag = True Then
                AccIntDays = 366
            End If
            'If DaysOption = 366 And NonGovernmentFlag = True Then
            '    AccIntDays = 366
            'End If
            If blnCoupExist = False And blnMatExist = False Then
                dblInterestAmt = ParVal * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), CalDate, AccIntDays) / AccIntDays
            ElseIf blnCoupExist = True And blnMatExist = False Then
                dblInterestAmt = ParVal * CouponRateP * DaysGap(CouponDateP, CalDate, AccIntDays) / AccIntDays
                dblInterestAmt = dblInterestAmt + (ParVal * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), CouponDateP, AccIntDays) / AccIntDays)
            ElseIf blnCoupExist = False And blnMatExist = True Then
                dblInterestAmt = MatAmtP * CouponRate(C) * DaysGap(Maturity_Date(MatDateP, MaturityOnHoliday, MaturityOnSat), CalDate, AccIntDays) / AccIntDays
                dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), Maturity_Date(MatDateP, MaturityOnHoliday, MaturityOnSat), AccIntDays) / AccIntDays)
            Else
                If MatDateP > CouponDateP Then
                    dblInterestAmt = MatAmtP * CouponRateP * DaysGap(CouponDateP, CalDate, AccIntDays) / AccIntDays
                    dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateA * DaysGap(Maturity_Date(MatDateP, MaturityOnHoliday, MaturityOnSat), CouponDateP, AccIntDays) / AccIntDays)
                    dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), Maturity_Date(MatDateP, MaturityOnHoliday, MaturityOnSat), AccIntDays) / AccIntDays)
                Else
                    dblInterestAmt = MatAmtP * CouponRateP * DaysGap(Maturity_Date(MatDateP, MaturityOnHoliday, MaturityOnSat), CalDate, AccIntDays) / AccIntDays
                    dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateP * DaysGap(CouponDateP, Maturity_Date(MatDateP, MaturityOnHoliday, MaturityOnSat), AccIntDays) / AccIntDays)
                    dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), CouponDateP, AccIntDays) / AccIntDays)
                End If
            End If
            If NonGovernmentFlag = True Then
                AccIntDays = 365
            End If
            '***************************************************************************************************

            XirrAmt(CntXirr) = -MarketVal
            XirrDate(CntXirr) = YTMDate
            CntXirr = CntXirr + 1

            '***************************************************************************************************
            ' Code for calculating Final amount by adding(Cum-interest)  or subtracting(Ex-interest) interest amount 
            ' and determining the next interest date and previous interest date
            ' and applying the first element of XirrDate & XirrAmt arrays.

            dblInterestAmt = dblInterestAmt / 100
            If CalDate = YTMDate Then
                CalDate = NxtIntDate
                GetIntDate(CalDate, Divisor, NxtIntDate, PrevIntDate)
                If CompRateFlag = False Then
                    XirrAmt(CntXirr) = dblInterestAmt
                    XirrDate(CntXirr) = Interest_Date(YTMDate, InterestOnHoliday, InterestOnSat)
                    CntXirr = CntXirr + 1
                End If
            Else
                GetIntDate(NxtIntDate, Divisor, CalDate, PrevIntDate)
                NxtIntDate = CalDate
                CalDate = PrevIntDate
                C = K
                If CompRateFlag = False Then
                    XirrAmt(CntXirr) = -dblInterestAmt
                    XirrDate(CntXirr) = Interest_Date(YTMDate, InterestOnHoliday, InterestOnSat)
                    CntXirr = CntXirr + 1
                End If
            End If
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code loop for conditionally applying XirrAmt and XirrDate arrays with maturity amount & date

            While NxtIntDate <= LstMatDate
                If NxtIntDate = LstMatDate Then
                    NxtIntDate = Maturity_Date(LstMatDate, MaturityOnHoliday, MaturityOnSat)
                End If
                blnCoupExist = False
                blnMatExist = False
                dblInterestAmt = 0
                If NxtIntDate = Maturity_Date(LstMatDate, MaturityOnHoliday, MaturityOnSat) Then blnOutOfLoop = True

                If NxtIntDate > CouponDate(C) And (CouponDate.Length) - 1 < (C) Then
                    CouponRateP = CouponRate(C)
                    CouponDateP = CouponDate(C)
                    C = C + 1
                    CouponRateA = CouponRate(C)
                    blnCoupExist = True
                End If

                If NxtIntDate > Maturity_Date(MaturityDate(M), MaturityOnHoliday, MaturityOnSat) And (MaturityDate.Length) - 1 > (M) Then
                    MatDateP = Maturity_Date(MaturityDate(M), MaturityOnHoliday, MaturityOnSat)
                    MatAmtP = MatAmtA
                    XirrAmt(CntXirr) = MaturityAmt(M)
                    XirrDate(CntXirr) = Maturity_Date(MaturityDate(M), MaturityOnHoliday, MaturityOnSat)
                    CntXirr = CntXirr + 1
                    ParVal = ParVal - MaturityAmt(M)
                    M = M + 1
                    MatAmtA = ParVal
                    blnMatExist = True
                End If

                CalDate = Interest_Date(CalDate, InterestOnHoliday, InterestOnSat)
                '*************************************************************************************
                'new code for 365 & 366 days implementation
                '*************************************************************************************
                If Date.IsLeapYear(Year(NxtIntDate)) = True And DaysOption = 366 And NonGovernmentFlag = True Then
                    'And chrDaysFlag <> "F"
                    AccIntDays = 366
                End If
                '*************************************************************************************
                If blnCoupExist = False And blnMatExist = False Then
                    If Prev_Date(NxtIntDate, Divisor) < CalDate Then
                        dblInterestAmt = ParVal * CouponRate(C) * DateDiff("D", CalDate, Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat)) / (AccIntDays)
                    Else
                        If Divisor < 1 Then
                            dblInterestAmt = ParVal * CouponRate(C) / Divisor
                        Else
                            If IPCalcFlag = "E" Then
                                If Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat) = NxtIntDate Then
                                    dblInterestAmt = ParVal * CouponRate(C) / Divisor
                                Else
                                    dblInterestAmt = (ParVal * CouponRate(C) / Divisor) + (ParVal * CouponRate(C) * DateDiff("D", NxtIntDate, Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat)) / (AccIntDays))
                                    'dblInterestAmt = ParVal * CouponRate(C) * DateDiff("D", CalDate, Interest_Date(NxtIntDate)) / (AccIntDays)
                                End If
                            Else
                                dblInterestAmt = ParVal * CouponRate(C) * DateDiff("D", Interest_Date(CalDate), Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat)) / (AccIntDays)
                            End If
                        End If
                    End If
                ElseIf blnCoupExist = True And blnMatExist = False Then
                    dblInterestAmt = ParVal * CouponRateP * DaysGap(CouponDateP, CalDate, AccIntDays) / AccIntDays
                    dblInterestAmt = dblInterestAmt + (ParVal * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), CouponDateP, AccIntDays) / AccIntDays)
                ElseIf blnCoupExist = False And blnMatExist = True Then
                    dblInterestAmt = MatAmtP * CouponRate(C) * DaysGap(Maturity_Date(MatDateP, MaturityOnHoliday, MaturityOnSat), CalDate, AccIntDays) / AccIntDays
                    dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRate(C) * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), Maturity_Date(MatDateP, MaturityOnHoliday, MaturityOnSat), AccIntDays) / AccIntDays)
                Else
                    If MatDateP > CouponDateP Then
                        dblInterestAmt = MatAmtP * CouponRateP * DaysGap(CouponDateP, CalDate, AccIntDays) / AccIntDays
                        dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateA * DaysGap(MatDateP, CouponDateP, AccIntDays) / AccIntDays)
                        dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), MatDateP, AccIntDays) / AccIntDays)
                    Else
                        dblInterestAmt = MatAmtP * CouponRateP * DaysGap(MatDateP, CalDate, AccIntDays) / AccIntDays
                        dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateP * DaysGap(CouponDateP, MatDateP, AccIntDays) / AccIntDays)
                        dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat), CouponDateP, AccIntDays) / AccIntDays)
                    End If
                End If
                dblInterestAmt = dblInterestAmt / 100
                XirrAmt(CntXirr) = dblInterestAmt
                XirrDate(CntXirr) = Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat)
                CntXirr = CntXirr + 1

                CalDate = NxtIntDate
                '*************************************************************************************
                'new code 
                '*************************************************************************************
                If NonGovernmentFlag = True Then
                    AccIntDays = 365
                End If
                '*************************************************************************************
                If NxtIntDate = MaturityDate(M) And MaturityDate(M) <> LstMatDate Then
                    MatDateP = MaturityDate(M)
                    MatAmtP = MatAmtA
                    XirrAmt(CntXirr) = MaturityAmt(M)
                    XirrDate(CntXirr) = Maturity_Date(MaturityDate(M), MaturityOnHoliday, MaturityOnSat)
                    CntXirr = CntXirr + 1
                    ParVal = ParVal - MaturityAmt(M)
                    M = M + 1
                    MatAmtA = ParVal
                End If
                If NxtIntDate = CouponDate(C) And CouponDate(C) <> LstMatDate Then
                    CouponRateP = CouponRate(C)
                    CouponDateP = CouponDate(C)
                    C = C + 1
                    CouponRateA = CouponRate(C)
                End If
                GetIntDate(CalDate, Divisor, NxtIntDate, PrevIntDate)
            End While
            Dim IPCnt As Int32

            'if next interest date is not equal to last maturity date then 
            If blnOutOfLoop = False Then
                blnCoupExist = False
                blnMatExist = False
                If IPMatFlag = True Then
                    IPCnt = CntXirr - 1
                    IPMatAmt = XirrAmt(IPCnt)
                End If
                If LstMatDate > CouponDate(C) Then
                    CouponRateP = CouponRate(C)
                    CouponDateP = CouponDate(C)
                    C = C + 1
                    CouponRateA = CouponRate(C)
                    blnCoupExist = True
                End If
                If LstMatDate > MaturityDate(M) Then
                    MatDateP = MaturityDate(M)
                    MatAmtP = MatAmtA
                    If IPMatFlag = True Then
                        IPMatAmt = MaturityAmt(M)
                    Else
                        XirrAmt(CntXirr) = MaturityAmt(M)
                        XirrDate(CntXirr) = Maturity_Date(MaturityDate(M), MaturityOnHoliday, MaturityOnSat)
                        CntXirr = CntXirr + 1
                    End If
                    ParVal = ParVal - MaturityAmt(M)
                    M = M + 1
                    MatAmtA = ParVal
                    blnMatExist = True
                End If
                '*************************************************************************************
                'new code for 365 & 366 days implementation
                '*************************************************************************************
                If Date.IsLeapYear(Year(LstMatDate)) = True And chrDaysFlag <> "F" And DaysOption = 366 And NonGovernmentFlag = True Then
                    AccIntDays = 366
                End If


                CalDate = Interest_Date(CalDate, InterestOnHoliday, InterestOnSat)

                If IssueDate <> Date.MinValue And BrokenInterest Then
                    dblInterestAmt = (ParVal * CouponRate(C) / Divisor) - (ParVal * CouponRate(C) * DaysGap(FstIntDate, IssueDate, AccIntDays) / AccIntDays)
                Else
                    '*************************************************************************************
                    If blnCoupExist = False And blnMatExist = False Then
                        dblInterestAmt = ParVal * CouponRate(C) * DaysGap(LstMatDate, (CalDate), AccIntDays) / AccIntDays
                    ElseIf blnCoupExist = True And blnMatExist = False Then
                        dblInterestAmt = ParVal * CouponRateP * DaysGap(CouponDateP, (CalDate), AccIntDays) / AccIntDays
                        dblInterestAmt = dblInterestAmt + (ParVal * CouponRateA * DaysGap(LstMatDate, CouponDateP, AccIntDays) / AccIntDays)
                    ElseIf blnCoupExist = False And blnMatExist = True Then
                        dblInterestAmt = MatAmtP * CouponRate(C) * DaysGap(MatDateP, (CalDate), AccIntDays) / AccIntDays
                        dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRate(C) * DaysGap(LstMatDate, MatDateP, AccIntDays) / (AccIntDays))
                    Else
                        If MatDateP > CouponDateP Then
                            dblInterestAmt = MatAmtP * CouponRateP * DaysGap(CouponDateP, CalDate, AccIntDays) / (AccIntDays)
                            dblInterestAmt = dblInterestAmt + (MatAmtP * CouponRateA * DaysGap(MatDateP, CouponDateP, AccIntDays) / (AccIntDays))
                            dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(LstMatDate, MatDateP, AccIntDays) / (AccIntDays))
                        Else
                            dblInterestAmt = MatAmtP * CouponRateP * DaysGap(MatDateP, CalDate, AccIntDays) / (AccIntDays)
                            dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateP * DaysGap(CouponDateP, MatDateP, AccIntDays) / (AccIntDays))
                            dblInterestAmt = dblInterestAmt + (MatAmtA * CouponRateA * DaysGap(LstMatDate, CouponDateP, AccIntDays) / (AccIntDays))
                        End If
                    End If
                End If
                If IPMatFlag = True Then
                    IPMatAmt = IPMatAmt + (dblInterestAmt / 100)
                Else
                    XirrAmt(CntXirr) = dblInterestAmt / 100
                    XirrDate(CntXirr) = Interest_Date(LstMatDate, InterestOnHoliday, InterestOnSat)
                    CntXirr = CntXirr + 1
                End If
            End If
            '***************************************************************************************************
            If IPMatFlag = True Then
                XirrAmt(IPCnt) = IPMatAmt + LstMatAmt
                XirrDate(IPCnt) = Maturity_Date(LstMatDate, MaturityOnHoliday, MaturityOnSat)
            Else
                XirrAmt(CntXirr) = LstMatAmt
                XirrDate(CntXirr) = Maturity_Date(LstMatDate, MaturityOnHoliday, MaturityOnSat)
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateXIRR", "Error in CalculateXIRR", "", ex)
            Throw ex
        End Try

    End Sub

    Public Sub CalculateAccuredInterest( _
                ByVal YTMDate As Date, ByVal ParVal As Decimal, ByVal Rate As Decimal, _
                ByVal NonGovernmentFlag As Boolean, ByVal RateActualFlag As Boolean, _
                ByRef MaturityDate() As Date, ByRef MaturityAmt() As Double, _
                ByRef CouponDate() As Date, ByRef CouponRate() As Double, ByVal BKDiff As Integer, _
                ByVal NxtIntDate As Date, ByVal IssueDate As Date, ByVal intFrequency As Int32, _
                ByVal Denominator As Int32, ByRef FinalAmount As Double, ByRef IntDate As Date, ByRef AddLess As String, _
                ByRef AddLessNoofDays As Integer, ByRef Ratio As Double, ByRef dblInterestAmt As Double, ByVal tmp_ExInt As Char, Optional ByVal BrokenInt As Boolean = False, Optional ByVal InterestOnHoliday As Boolean = False, Optional ByVal InterestOnSat As Boolean = False, Optional ByVal MaturityOnHoliday As Boolean = False, Optional ByVal MaturityOnSat As Boolean = False)
        Try
            Dim AccIntDays As Int32
            Dim CalDate As Date
            Dim LstMatDate As Date
            Dim LstMatAmt As Decimal
            Dim PrevIntDate As Date
            Dim MarketVal As Decimal
            Dim blnIntChk As Boolean = False
            Dim Divisor As Int32 = intFrequency
            Dim K As Integer
            Dim M As Integer
            Dim C As Integer
            Dim blnCoupExist As Boolean = False
            Dim blnMatExist As Boolean = False
            Dim blnOutOfLoop As Boolean = False
            Dim MatAmtA As Double
            Dim MatAmtP As Double
            Dim MatDateP As Date
            Dim CouponDateP As Date
            Dim CouponRateP As Double
            Dim CouponRateA As Double
            'Dim dblFinalAmt As Double
            Dim ParVal_Org As Decimal = ParVal
            Dim brokenintr As Boolean


            FstIntDate = NxtIntDate
            brokenintr = False
            'RateActualFlag = False
            '***************************************************************************************************
            ' Code for Evaluating Accured Interest Days & Initializing Last Maturity Date and Amount and CalDate

            MarketVal = IIf(RateActualFlag = True, Rate * ParVal / 100, Rate)
            AccIntDays = IIf(NonGovernmentFlag = True, Denominator, 360)
            CalDate = YTMDate
            If UBound(MaturityDate) > -1 Then
                LstMatDate = MaturityDate(UBound(MaturityDate))
                LstMatAmt = MaturityAmt(UBound(MaturityAmt))
            End If
            dblInterestAmt = 0
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for Evaluating Previous and Next Interest Date
            ' If the Settlement Date is between the issue Date and the first int Date

            If IssueDate <> Date.MinValue And NxtIntDate > CalDate Then
                PrevIntDate = NxtIntDate
                blnIntChk = True
            Else
                GetIntDate(CalDate, Divisor, NxtIntDate, PrevIntDate)
                PrevIntDate = Prev_Date(NxtIntDate, Divisor)
            End If

            PrevIntDate = Interest_Date(PrevIntDate, InterestOnHoliday, InterestOnSat)
            NxtIntDate = Interest_Date(NxtIntDate, InterestOnHoliday, InterestOnSat)
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for Determining either Ex-interest or Cum-Interest
            ' Ex-Interest is to pay to the buyer and Cum-Interest is to take from the buyer

            If tmp_ExInt = "Y" Then
                If DateDiff("D", YTMDate, NxtIntDate) < BKDiff Then
                    'Ex-interest
                    CalDate = YTMDate
                Else
                    'Cum-Interest
                    If blnIntChk Then
                        CalDate = IssueDate
                    Else
                        CalDate = PrevIntDate
                    End If
                    NxtIntDate = YTMDate
                End If
            Else
                If blnIntChk Then
                    CalDate = IssueDate
                Else
                    CalDate = PrevIntDate
                End If
                NxtIntDate = YTMDate
            End If
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for Intializing Array Starting Element

            C = GetCouponDate(CouponDate, CalDate, UBound(CouponDate))
            If UBound(MaturityDate) > -1 Then
                M = GetMatDate(MaturityDate, MaturityAmt, CalDate, UBound(MaturityDate), ParVal, YTMDate)

            Else
                M = 0
            End If
            K = C
            MatAmtA = ParVal
            If (ParVal_Org > 0) Then
                Ratio = (ParVal / ParVal_Org) * 100
            End If
            MarketVal = IIf(RateActualFlag = True, Rate * ParVal / 100, Rate)

            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for determining the Current & Previous Date & Rate of Coupon & Maturity
            If NxtIntDate > CouponDate(C) And (CouponDate.Length) - 1 < (C) Then
                CouponRateP = CouponRate(C)
                CouponDateP = CouponDate(C)
                C = C + 1
                CouponRateA = CouponRate(C)
                blnCoupExist = True



            End If
            If M <> 0 Then
                If NxtIntDate >= MaturityDate(M) Then
                    If (MaturityDate.Length) - 1 < (M) Then
                        MatDateP = Maturity_Date(MaturityDate(M), MaturityOnHoliday, MaturityOnSat)
                        MatAmtP = MatAmtA
                        ParVal = ParVal - MaturityAmt(M)
                        M = M + 1
                        MatAmtA = ParVal
                        blnMatExist = True
                    Else
                        brokenintr = True
                    End If
                End If
            End If
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for conditionally calculating interest amount according to the existance coupon & maturity

            If IssueDate <> Date.MinValue And brokenintr And BrokenInt Then
                dblInterestAmt = (ParVal * CouponRate(C) / Divisor) - (ParVal * CouponRate(C) * DaysGap(FstIntDate, IssueDate, Denominator) / Denominator)


            ElseIf blnCoupExist = False And blnMatExist = False Then

                dblInterestAmt = CouponRate(C) * (Ratio / 100) * DaysGap(NxtIntDate, CalDate, Denominator) / Denominator
                AddLessNoofDays = DaysGap(NxtIntDate, CalDate, Denominator)

            ElseIf blnCoupExist = True And blnMatExist = False Then
                dblInterestAmt = CouponRateP * (Ratio / 100) * DaysGap(CouponDateP, CalDate, Denominator) / Denominator
                dblInterestAmt = dblInterestAmt + (CouponRateA * (Ratio / 100) * DaysGap(NxtIntDate, CouponDateP, Denominator) / Denominator)
            ElseIf blnCoupExist = False And blnMatExist = True Then
                dblInterestAmt = CouponRate(C) * ((MatAmtP / ParVal_Org)) * DaysGap(MatDateP, CalDate, Denominator) / Denominator
                dblInterestAmt = dblInterestAmt + (CouponRate(C) * (Ratio / 100) * DaysGap(NxtIntDate, MatDateP, Denominator) / Denominator)
            Else
                If MatDateP > CouponDateP Then
                    dblInterestAmt = CouponRateP * ((MatAmtP / ParVal_Org)) * DaysGap(CouponDateP, CalDate, Denominator) / Denominator
                    dblInterestAmt = dblInterestAmt + (CouponRateA * ((MatAmtP / ParVal_Org)) * DaysGap(MatDateP, CouponDateP, Denominator) / Denominator)
                    dblInterestAmt = dblInterestAmt + (CouponRateA * (Ratio / 100) * DaysGap(NxtIntDate, MatDateP, Denominator) / Denominator)
                Else
                    dblInterestAmt = CouponRateP * ((MatAmtP / ParVal_Org)) * DaysGap(MatDateP, CalDate, Denominator) / Denominator
                    dblInterestAmt = dblInterestAmt + (CouponRateP * (Ratio / 100) * DaysGap(CouponDateP, MatDateP, Denominator) / Denominator)
                    dblInterestAmt = dblInterestAmt + (CouponRateA * (Ratio / 100) * DaysGap(NxtIntDate, CouponDateP, Denominator) / Denominator)
                End If
            End If
            '***************************************************************************************************

            '***************************************************************************************************
            ' Code for calculating Final amount by adding(Cum-interest)  or subtracting(Ex-interest) interest amount 
            ' and determining the next interest date and previous interest date
            dblInterestAmt = dblInterestAmt / 100
            If CalDate = YTMDate Then
                AddLess = "L"
                AddLessNoofDays = System.Math.Abs(DaysGap(YTMDate, NxtIntDate, Denominator))
                FinalAmount = MarketVal
                If blnIntChk Then
                    If IssueDate <> Date.MinValue And PrevIntDate > IssueDate And YTMDate > PrevIntDate Then
                        IntDate = IssueDate
                    Else
                        IntDate = Interest_Date(PrevIntDate, InterestOnHoliday, InterestOnSat)
                    End If

                Else
                    'IntDate = DateAdd("M", 12 / Divisor, PrevIntDate)
                    IntDate = Interest_Date(DateAdd("M", 12 / Divisor, PrevIntDate), InterestOnHoliday, InterestOnSat)

                End If
            Else
                AddLess = "A"
                AddLessNoofDays = DaysGap(YTMDate, CalDate, Denominator)
                FinalAmount = MarketVal
                If blnIntChk Then
                    IntDate = IssueDate
                Else
                    IntDate = CalDate
                End If
                C = K
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateAccuredInterest", "Error in CalculateAccuredInterest", "", ex)
            Throw ex
        End Try

        '***************************************************************************************************
    End Sub

    '***************************************************************************************************
    ' Functions for getting the next & previous interest dates
    '***************************************************************************************************
    Private Sub GetIntDate(ByRef CalDate As Date, ByRef Divisor As Int32, ByRef NxtIntDate As Date, ByRef PrevIntDate As Date)
        Try
            Dim tmp_date As Date
            If NxtIntDate < PrevIntDate And PrevIntDate <> Date.MinValue Then
                ' This will swap the previous and next interest date
                tmp_date = NxtIntDate
                NxtIntDate = PrevIntDate
                PrevIntDate = tmp_date
            Else
                ' Loop that will determine the Next & Previous Interest Date
                While NxtIntDate <= CalDate
                    PrevIntDate = NxtIntDate
                    NxtIntDate = Next_Date(NxtIntDate, Divisor)
                End While
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetIntDate", "Error in GetIntDate", "", ex)
            Throw ex
        End Try

    End Sub

    Private Sub GetNextPrevDate(ByRef SellSettDate As Date, ByRef Divisor As Int32, ByRef NxtIntDate As Date, ByRef PrevIntDate As Date)
        Try
            Dim tmp_date As Date
            If NxtIntDate < PrevIntDate And PrevIntDate <> Date.MinValue Then
                ' This will swap the previous and next interest date
                tmp_date = NxtIntDate
                NxtIntDate = PrevIntDate
                PrevIntDate = tmp_date
            Else
                ' Loop that will determine the Next & Previous Interest Date
                'While NxtIntDate <= SellSettDate
                '    PrevIntDate = NxtIntDate
                '    NxtIntDate = Next_Date(NxtIntDate, Divisor)
                'End While
                If NxtIntDate <= SellSettDate Then
                    PrevIntDate = NxtIntDate
                    NxtIntDate = Next_Date(NxtIntDate, Divisor)
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetNextPrevDate", "Error in GetNextPrevDate", "", ex)
            Throw ex
        End Try

    End Sub

    Public Function Next_Date(ByVal NxtIntDate As Date, ByVal Divisor As Integer) As Date
        Try
            Dim NxtDate As Date
            Dim strDate As String
            Dim strTemp As String
            Dim strTemp2 As String
            If Day(NxtIntDate) >= MonthDays(Month(NxtIntDate)) Then
                NxtDate = DateAdd("M", 12 / Divisor, NxtIntDate)
                strTemp = IIf(Len(CStr(Month(NxtDate))) = 1, "0", "")
                If chrMaxActFlag = "A" Then
                    If FstIntDate = Date.MinValue Then
                        strTemp2 = IIf(Len(CStr(Day(NxtIntDate))) = 1, "0", "")
                        strDate = strTemp2 & Day(NxtIntDate) & "/" & strTemp & Month(NxtDate) & "/" & Year(NxtDate)
                    Else
                        strTemp2 = IIf(Len(CStr(Day(FstIntDate))) = 1, "0", "")
                        strDate = strTemp2 & Day(FstIntDate) & "/" & strTemp & Month(NxtDate) & "/" & Year(NxtDate)
                    End If
                Else
                    strDate = MonthDays(Month(NxtDate)) & "/" & strTemp & Month(NxtDate) & "/" & Year(NxtDate)
                End If
                NxtDate = objCommon.DateFormat(strDate)
            Else
                NxtDate = DateAdd("M", 12 / Divisor, NxtIntDate)
            End If


            newdate = NxtDate
            'While checkdate(newdate) = True
            '    newdate = DateAdd(DateInterval.Day, 1, newdate)
            'End While


            Return newdate
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Next_Date", "Error in Next_Date", "", ex)
            Throw ex
        End Try

    End Function
    Private Function checkdate(ByVal newdate As Date) As Boolean
        Try
            Dim Sqlcomm As New SqlCommand
            OpenConn()
            With Sqlcomm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_HolidaysNew"
                .Parameters.Clear()
                'objCommon.SetCommandParameters(Sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                objCommon.SetCommandParameters(Sqlcomm, "@Month", SqlDbType.Int, 4, "I", , , Val(newdate.Month))
                objCommon.SetCommandParameters(Sqlcomm, "@HolidayDate", SqlDbType.DateTime, 4, "I", , , newdate)
                objCommon.SetCommandParameters(Sqlcomm, "@RET_CODE", SqlDbType.Int, 4, "O")
            End With
            If Sqlcomm.ExecuteScalar > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "checkdate", "Error in checkdate", "", ex)

        Finally
            CloseConn()
        End Try
    End Function

    Public Function Prev_Date(ByVal NxtIntDate As Date, ByVal Divisor As Integer) As Date
        Try
            Dim PrevDate As Date
            Dim strDate As String
            Dim strTemp As String
            Dim strTemp2 As String
            If Day(NxtIntDate) >= MonthDays(Month(NxtIntDate)) Then
                PrevDate = DateAdd("M", -12 / Divisor, NxtIntDate)
                strTemp = IIf(Len(CStr(Month(PrevDate))) = 1, "0", "")
                'strDate = MonthDays(Month(PrevDate)) & "/" & strTemp & Month(PrevDate) & "/" & Year(PrevDate)
                If chrMaxActFlag = "A" Then
                    If FstIntDate = Date.MinValue Then
                        strTemp2 = IIf(Len(CStr(Day(NxtIntDate))) = 1, "0", "")
                        strDate = strTemp2 & Day(NxtIntDate) & "/" & strTemp & Month(PrevDate) & "/" & Year(PrevDate)
                    Else
                        strTemp2 = IIf(Len(CStr(Day(FstIntDate))) = 1, "0", "")
                        strDate = strTemp2 & Day(FstIntDate) & "/" & strTemp & Month(PrevDate) & "/" & Year(PrevDate)
                    End If
                Else
                    strDate = MonthDays(Month(PrevDate)) & "/" & strTemp & Month(PrevDate) & "/" & Year(PrevDate)
                End If
                PrevDate = objCommon.DateFormat(strDate)
            Else
                PrevDate = DateAdd("M", -12 / Divisor, NxtIntDate)
            End If
            Return PrevDate
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Prev_Date", "Error in Prev_Date", "", ex)
            Throw ex
        End Try

    End Function
    '***************************************************************************************************

    '***************************************************************************************************
    ' Functions index of the array from where is to calculate Coupon & Maturity
    '***************************************************************************************************
    Private Function GetCouponDate(ByVal CouponDate() As Date, ByVal CalDate As Date, ByVal CntCoup As Integer) As Integer
        Try
            Dim pc As Integer
            For pc = 0 To CntCoup
                If CouponDate(pc) >= CalDate Then Exit For
            Next
            'If pc <> 0 Then
            '    pc = pc - 1
            'End If
            Return pc
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetCouponDate", "Error in GetCouponDate", "", ex)
            Throw ex
        End Try

    End Function

    Private Function GetMatDate(ByVal MaturityDate() As Date, ByVal MaturityAmt() As Double, ByVal CalDate As Date, ByVal CntMat As Integer, ByRef ParVal As Double, ByVal YTMDate As Date) As Integer
        Try
            Dim pm As Integer
            For pm = 0 To CntMat

                ''changed here
                If YTMDate = CalDate Then
                    '15Feb16
                    If MaturityDate(pm) > CalDate Then
                        Exit For
                    Else
                        ParVal = ParVal - MaturityAmt(pm)
                    End If
                Else
                    If MaturityDate(pm) > CalDate Then
                        Exit For
                    Else
                        ParVal = ParVal - MaturityAmt(pm)
                    End If
                End If
            Next
            Return pm
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetMatDate", "Error in GetMatDate", "", ex)
            Throw ex
        End Try

    End Function
    '***************************************************************************************************

    '***************************************************************************************************
    ' Function will return the number of days that are present between Date1 & Date2 
    ' according to the accured interest days
    '***************************************************************************************************
    Private Function DaysGap(ByVal Date1 As Date, ByVal Date2 As Date, ByVal AccIntDays As Integer) As Long
        Try
            Dim intDate1 As Integer
            Dim intDate2 As Integer
            Dim ddtot As Long
            Dim mmtot As Long
            Dim yytot As Long
            If AccIntDays = 365 Or AccIntDays = 366 Then
                Return DateDiff(DateInterval.Day, Date2, Date1)
            Else
                yytot = (Year(Date1) - Year(Date2)) * 360
                mmtot = (Month(Date1) - Month(Date2)) * 30
                intDate2 = IIf(Day(Date2) > 30, 30, Day(Date2))
                intDate1 = IIf(Day(Date1) = 31, 30, Day(Date1))
                ddtot = intDate1 - intDate2
                Return yytot + mmtot + ddtot
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "DaysGap", "Error in DaysGap", "", ex)
            Throw ex
        End Try

    End Function

    Public Function CalculateXIRRMarketRate( _
                ByVal YTMDate As Date, ByVal ParVal As Decimal, ByVal Rate As Decimal, _
                ByVal NonGovernmentFlag As Boolean, ByVal RateActualFlag As Boolean, _
                ByVal GivenDate() As Date, ByVal GivenAmt() As Double, _
                ByRef CouponDate() As Date, ByRef CouponRate() As Double, ByVal BKDiff As Integer, _
                ByVal NxtIntDate As Date, ByVal IssueDate As Date, ByVal intFrequency As Int32, _
                ByVal strSemiAnn As String, ByVal strOption As String, ByVal IPCalcFlag As String, _
                ByVal IPMatFlag As Boolean, ByVal DaysOption As Int16, ByVal chrDaysFlag As Char, Optional ByVal BrokenInt As Boolean = False, Optional ByVal InterestOnHoliday As Boolean = False, Optional ByVal InterestOnSat As Boolean = False, Optional ByVal MaturityOnHoliday As Boolean = False, Optional ByVal MaturityOnSat As Boolean = False) As Double
        Try
            Dim dbltmpYTM As Decimal
            Dim J As Int32
            Dim MarketVal As Double = 0
            Dim AccIntDays As Int32

            AccIntDays = IIf(NonGovernmentFlag = True, 365, 360)
            If intFrequency = 0 Then
                dbltmpYTM = CalcTmpYTMDDB(strSemiAnn, strOption)
                MarketVal = 0
                For J = 0 To UBound(GivenAmt)
                    If GivenDate(J) > YTMDate Then Exit For
                Next
                For J = J To UBound(GivenAmt)
                    MarketVal = MarketVal + (GivenAmt(J) * (1 / (((1 + (dbltmpYTM / 200)) ^ ((DateDiff(DateInterval.Day, YTMDate, GivenDate(J))) / (AccIntDays / 2))))))
                Next
            Else
                MarketVal = 0
                CntXirr = 0
                CalculateXIRR(YTMDate, ParVal, MarketVal, NonGovernmentFlag, RateActualFlag, GivenDate, _
                              GivenAmt, CouponDate, CouponRate, BKDiff, NxtIntDate, IssueDate, intFrequency, False, IPCalcFlag, IPMatFlag, DaysOption, chrDaysFlag, BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                dbltmpYTM = CalcTmpYTM(strSemiAnn, strOption)
                MarketVal = XirrAmt(0)
                MarketVal = FinancialFunc.XNPV(dbltmpYTM, XirrAmt, XirrDate)
                'For J = 1 To CntXirr
                '    MarketVal = MarketVal + (XirrAmt(J) * (1 / (((1 + (dbltmpYTM / 200)) ^ ((DateDiff(DateInterval.Day, YTMDate, XirrDate(J))) / (365 / 2))))))
                'Next
            End If
            MarketVal = IIf(RateActualFlag = True, MarketVal * 100 / ParVal, MarketVal)

            Return MarketVal
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '***************************************************************************************************
    Private Function CalcTmpYTM(ByVal strSemiAnn As String, ByVal strOption As String) As Double
        Try
            Dim dbltmpYTM As Double
            Dim dblAnn As Double
            Dim dblSemi As Double
            Select Case strOption
                Case "M"
                    dblAnn = dblYTMAnn
                    dblSemi = dblYTMSemi
                Case "C"
                    dblAnn = dblYTCAnn
                    dblSemi = dblYTCSemi
                Case "P"
                    dblAnn = dblYTPAnn
                    dblSemi = dblYTPSemi
            End Select
            If strSemiAnn = "A" Then
                dbltmpYTM = FinancialFunc.nominal(dblAnn / 100, 2) * 100
                'With objExcel
                '    If objExcel.Version = 15.0 Or objExcel.Version = 12.0 Then
                '        dbltmpYTM = CType(objExcel.WorksheetFunction.Nominal(dblAnn / 100, 2), Double) * 100
                '    Else
                '        dbltmpYTM = CType(objExcel.Run("NOMINAL", dblAnn / 100, 2), Double) * 100
                '    End If
                'End With
            Else
                dbltmpYTM = dblSemi
            End If
            Return dbltmpYTM
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalcTmpYTM", "Error in CalcTmpYTM", "", ex)
            Throw ex
        End Try

    End Function

    '***************************************************************************************************
    Private Function CalcTmpYTMDDB(ByVal strSemiAnn As String, ByVal strOption As String) As Double
        Try
            Dim dbltmpYTM As Double
            Dim dblAnn As Double
            Dim dblSemi As Double
            Select Case strOption
                Case "M"
                    dblAnn = dblYTMAnn
                    dblSemi = dblYTMSemi
                Case "C"
                    dblAnn = dblYTCAnn
                    dblSemi = dblYTCSemi
                Case "P"
                    dblAnn = dblYTPAnn
                    dblSemi = dblYTPSemi
            End Select
            If strSemiAnn = "A" Then
                dbltmpYTM = FinancialFunc.nominal(dblAnn / 100, 2) * 100
                'With objExcel
                '    If objExcel.Version = 15.0 Or objExcel.Version = 12.0 Then
                '        dbltmpYTM = CType(objExcel.WorksheetFunction.Nominal(dblAnn / 100, 2), Double) * 100
                '    Else
                '        dbltmpYTM = CType(objExcel.Run("NOMINAL", dblAnn / 100, 2), Double) * 100
                '    End If
                'End With
            Else
                'dblYTMSemi = CDbl(objSemi.Value)
                dbltmpYTM = dblSemi / 100
            End If
            Return dbltmpYTM
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalcTmpYTMDDB", "Error in CalcTmpYTMDDB", "", ex)
            Throw ex
        End Try

    End Function

    Public Function CalculateBookClosureDiff(ByVal BkClosureDate As Date, ByVal BkClosuRetype As String, ByVal NxtIntDate As Date, ByVal NonGovernmentFlag As Boolean) As Integer
        Try
            Dim intBKDiff As Integer
            If BkClosureDate = Date.MinValue Then
                If BkClosuRetype = "P" Then
                    intBKDiff = 31
                Else
                    intBKDiff = 3
                End If
                If NonGovernmentFlag = True And BkClosureDate = Date.MinValue Then
                    intBKDiff = 31
                Else
                    intBKDiff = 3
                End If
            Else
                If NxtIntDate <> Date.MinValue Then
                    intBKDiff = DateDiff(DateInterval.Day, BkClosureDate, NxtIntDate)
                Else
                    intBKDiff = 0
                End If
            End If
            Return intBKDiff
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateBookClosureDiff", "Error in CalculateBookClosureDiff", "", ex)
            Throw ex
        End Try

    End Function

    Public Function CheckLastIPMaturity(ByVal FirstIntDate As Date, ByVal MaturityDate As Date, ByVal intFrequency As Int16) As Boolean
        Try
            While FirstIntDate < MaturityDate
                FirstIntDate = Next_Date(FirstIntDate, intFrequency)
            End While
            If FirstIntDate = MaturityDate Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CheckLastIPMaturity", "Error in CheckLastIPMaturity", "", ex)
            Throw ex
        End Try

    End Function
    Private Function GetIPIssueDiff(ByVal intFrequency As Int16, ByVal FirstIntDate As Date, _
                                    ByVal IssueDate As Date, ByVal Denominator As Int32, _
                                    ByVal dblCouponRate As Double, ByVal dblRatio As Double, _
                                    ByVal ParVal As Decimal, ByRef dblInterestAmt As Double, _
                                    ByRef Calcdate As Date, ByRef AddDays As Int32) As Int32
        Try
            Dim InterestDate As Date
            Dim NextIPDate As Date
            Dim PrevIPDate As Date
            Dim intDeno As Int32
            AddDays = 0
            FstIntDate = FirstIntDate
            InterestDate = FirstIntDate
            NextIPDate = FirstIntDate
            PrevIPDate = FirstIntDate
            If IssueDate <> Date.MinValue And Calcdate < FirstIntDate Then
                For I As Int16 = 1 To intFrequency
                    If Month(IssueDate) = Month(InterestDate) And Day(IssueDate) = Day(InterestDate) Then
                        'While IssueDate < PrevIPDate
                        '    NextIPDate = PrevIPDate
                        '    PrevIPDate = Prev_Date(NextIPDate, intFrequency)
                        'End While
                        intDeno = intFrequency * DateDiff(DateInterval.Day, IssueDate, FirstIntDate)
                        Return intDeno
                    End If
                    InterestDate = Next_Date(InterestDate, intFrequency)
                Next
                While IssueDate < PrevIPDate
                    NextIPDate = PrevIPDate
                    PrevIPDate = Prev_Date(NextIPDate, intFrequency)
                End While
                'intDeno = intFrequency * (DateDiff(DateInterval.Day, IssueDate, NextIPDate) / DateDiff(DateInterval.Day, PrevIPDate, NextIPDate))
                intDeno = intFrequency * DateDiff(DateInterval.Day, PrevIPDate, NextIPDate)
                ' CouponRate(C) * (Ratio / 100) * DaysGap(NxtIntDate, CalDate, Denominator) / intDeno
                dblInterestAmt = (dblCouponRate * (dblRatio / 100) * DateDiff(DateInterval.Day, IssueDate, NextIPDate) / intDeno)
                AddDays = DateDiff(DateInterval.Day, IssueDate, NextIPDate)
                Calcdate = NextIPDate
                PrevIPDate = NextIPDate
                NextIPDate = Next_Date(NextIPDate, intFrequency)
                intDeno = intFrequency * DateDiff(DateInterval.Day, PrevIPDate, NextIPDate)
                Return intDeno
            Else
                Dim tmpCalcDate As Date
                tmpCalcDate = Calcdate
                While NextIPDate <= tmpCalcDate
                    tmpCalcDate = NextIPDate
                    NextIPDate = Next_Date(NextIPDate, intFrequency)
                End While
                PrevIPDate = Prev_Date(NextIPDate, intFrequency)
                intDeno = intFrequency * DateDiff(DateInterval.Day, PrevIPDate, NextIPDate)
                Return intDeno
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetIPIssueDiff", "Error in GetIPIssueDiff", "", ex)
            Throw ex
        End Try


    End Function

    Public Function CheckForDuplicate(ByVal Dt As DataTable, ByVal strNameDesig As String, ByVal strfld1 As String, ByVal strfld2 As String) As Boolean
        Try
            If Dt.Rows.Count > 0 Then
                For i As Int16 = 0 To Dt.Rows.Count - 1
                    If (Dt.Rows(i).Item(strfld1).ToString() + Dt.Rows(i).Item(strfld2).ToString()) = strNameDesig Then
                        Return True
                        Exit Function
                    End If
                Next
            End If
            Return False
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CheckForDuplicate", "Error in CheckForDuplicate", "", ex)
            Throw ex
        End Try
    End Function

    Public Function Interest_Date(ByVal Intdate As Date, Optional ByVal InterestOnHoliday As Boolean = False, Optional ByVal InterestOnSat As Boolean = False) As Date
        If InterestOnHoliday Then
            If InterestOnSat Then
                While WeekdayName(Weekday(Intdate)) = "Sunday" Or CommonHolidays(Intdate) = True Or (checkdate(Intdate) = True And WeekdayName(Weekday(Intdate)) <> "Saturday")
                    Intdate = DateAdd(DateInterval.Day, 1, Intdate)
                End While
            Else
                While WeekdayName(Weekday(Intdate)) = "Saturday" Or WeekdayName(Weekday(Intdate)) = "Sunday" Or CommonHolidays(Intdate) = True Or (checkdate(Intdate) = True And WeekdayName(Weekday(Intdate)) <> "Saturday" And WeekdayName(Weekday(Intdate)) <> "Sunday")
                    Intdate = DateAdd(DateInterval.Day, 1, Intdate)
                End While
            End If
        End If
        Return Intdate
    End Function
    Public Function Maturity_Date(ByVal Matdate As Date, Optional ByVal MaturityOnHoliday As Boolean = False, Optional ByVal MaturityOnSat As Boolean = False) As Date
        If MaturityOnHoliday Then
            If MaturityOnSat Then
                While WeekdayName(Weekday(Matdate)) = "Sunday" Or CommonHolidays(Matdate) = True Or (checkdate(Matdate) = True And WeekdayName(Weekday(Matdate)) <> "Saturday")
                    Matdate = DateAdd(DateInterval.Day, -1, Matdate)
                End While
            Else
                While WeekdayName(Weekday(Matdate)) = "Saturday" Or WeekdayName(Weekday(Matdate)) = "Sunday" Or CommonHolidays(Matdate) = True And (checkdate(Matdate) = True And WeekdayName(Weekday(Matdate)) <> "Saturday" And WeekdayName(Weekday(Matdate)) <> "Sunday")
                    Matdate = DateAdd(DateInterval.Day, -1, Matdate)
                End While
            End If
        End If
        Return Matdate
    End Function

    Private Function CommonHolidays(ByVal Intdate As Date) As Boolean
        Try
            Dim Sqlcomm As New SqlCommand
            OpenConn()
            With Sqlcomm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_CommonHolidays"
                .Parameters.Clear()
                objCommon.SetCommandParameters(Sqlcomm, "@MonthNo", SqlDbType.Int, 4, "I", , , Val(Intdate.Month))
                objCommon.SetCommandParameters(Sqlcomm, "@DayNo", SqlDbType.Int, 4, "I", , , Intdate.Day)
                objCommon.SetCommandParameters(Sqlcomm, "@RET_CODE", SqlDbType.Int, 4, "O")
            End With
            If Sqlcomm.ExecuteScalar > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "checkdate", "Error in checkdate", "", ex)
        Finally
            CloseConn()
        End Try
    End Function

    Public Function SecurityNext_IntDate(ByVal NxtIntDate As Date, ByVal Divisor As Integer, ByVal MaxActual As Char, ByVal FstIntDate As Date) As Date
        Try
            Dim NxtDate As Date
            Dim strDate As String
            Dim strTemp As String
            Dim strTemp2 As String
            If Day(NxtIntDate) >= MonthDays(Month(NxtIntDate)) Then
                NxtDate = DateAdd("M", 12 / Divisor, NxtIntDate)
                strTemp = IIf(Len(CStr(Month(NxtDate))) = 1, "0", "")
                If MaxActual = "A" Then
                    If FstIntDate = Date.MinValue Then
                        strTemp2 = IIf(Len(CStr(Day(NxtIntDate))) = 1, "0", "")
                        strDate = strTemp2 & Day(NxtIntDate) & "/" & strTemp & Month(NxtDate) & "/" & Year(NxtDate)
                    Else
                        strTemp2 = IIf(Len(CStr(Day(FstIntDate))) = 1, "0", "")
                        strDate = strTemp2 & Day(FstIntDate) & "/" & strTemp & Month(NxtDate) & "/" & Year(NxtDate)
                    End If
                Else
                    strDate = MonthDays(Month(NxtDate)) & "/" & strTemp & Month(NxtDate) & "/" & Year(NxtDate)
                End If
                NxtDate = objCommon.DateFormat(strDate)
            Else
                NxtDate = DateAdd("M", 12 / Divisor, NxtIntDate)
            End If
            Return NxtDate
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SecurityNext_IntDate", "Error in SecurityNext_IntDate", "", ex)
            Throw ex
        End Try

    End Function


    Public Sub CalculateXIRRYield( _
                             ByVal SecurityId As Integer, _
                             ByVal SettementDate As String, _
                             ByVal Rate As Double, _
                             ByVal FrequencyOfInterest As Integer, _
                             ByVal ShowCashflow As Boolean, _
                             ByVal RateOrActual As String, _
                             Optional ByVal StepUp As String = "N", _
                             Optional ByVal YieldType As String = "M")

        Dim lstParam As New List(Of SqlParameter)()
        Dim ds As New DataSet
        Dim dtYTM As DataTable
        Dim dtYTC As DataTable
        Dim dtYTP As DataTable
        Dim I As Integer
        Dim strUrl As String
        Dim strAmount As String = ""
        Dim strDate As String = ""

        Try
            lstParam.Add(New SqlParameter("@SecurityId", SecurityId))
            lstParam.Add(New SqlParameter("@Dated", SettementDate))
            lstParam.Add(New SqlParameter("@PurchaseRate", Rate))
            lstParam.Add(New SqlParameter("@RateOrActual", RateOrActual))
            lstParam.Add(New SqlParameter("@StepUp", StepUp))
            lstParam.Add(New SqlParameter("@YieldType", YieldType))

            ds = objComm.FillDetails(lstParam, "WDM_Fill_SecurityYieldCashFlow")
            If ds.Tables.Count >= 3 Then
                I = 0
                dtYTM = ds.Tables(0)
                If dtYTM.Rows.Count > 0 Then
                    ReDim XirrDate(dtYTM.Rows.Count - 1)
                    ReDim XirrAmt(dtYTM.Rows.Count - 1)

                    For Each row As DataRow In dtYTM.Rows
                        XirrDate(I) = row("IPDate")
                        XirrAmt(I) = row("IPAmount")
                        strDate = strDate & CDate(row("IPDate")) & "!"
                        strAmount = strAmount & row("IPAmount") & "!"
                        I = I + 1
                    Next

                    dblYTMAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
                    dblYTMSemi = FinancialFunc.nominal(dblYTMAnn, 2)
                    dblYTMAnn = dblYTMAnn * 100
                    dblYTMSemi = dblYTMSemi * 100

                    If ShowCashflow Then
                        Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
                        strAmount = strAmount.Substring(0, strAmount.Length - 1)
                        strDate = strDate.Substring(0, strDate.Length - 1)
                        strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
                        ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
                    End If
                End If

                I = 0
                dtYTC = ds.Tables(1)
                If dtYTC.Rows.Count > 0 Then
                    ReDim XirrDate(dtYTC.Rows.Count - 1)
                    ReDim XirrAmt(dtYTC.Rows.Count - 1)
                    strAmount = ""
                    strDate = ""
                    For Each row As DataRow In dtYTC.Rows
                        XirrDate(I) = row("IPDate")
                        XirrAmt(I) = row("IPAmount")
                        strDate = strDate & CDate(row("IPDate")) & "!"
                        strAmount = strAmount & row("IPAmount") & "!"
                        I = I + 1
                    Next

                    dblYTCAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
                    dblYTCSemi = FinancialFunc.nominal(dblYTCAnn, 2)
                    dblYTCAnn = dblYTCAnn * 100
                    dblYTCSemi = dblYTCSemi * 100

                    If ShowCashflow And dtYTM.Rows.Count = 0 Then
                        Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
                        strAmount = strAmount.Substring(0, strAmount.Length - 1)
                        strDate = strDate.Substring(0, strDate.Length - 1)
                        strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
                        ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
                    End If
                End If

                I = 0
                dtYTP = ds.Tables(2)
                If dtYTP.Rows.Count > 0 Then
                    ReDim XirrDate(dtYTP.Rows.Count - 1)
                    ReDim XirrAmt(dtYTP.Rows.Count - 1)
                    strAmount = ""
                    strDate = ""
                    For Each row As DataRow In dtYTP.Rows
                        XirrDate(I) = row("IPDate")
                        XirrAmt(I) = row("IPAmount")
                        strDate = strDate & CDate(row("IPDate")) & "!"
                        strAmount = strAmount & row("IPAmount") & "!"
                        I = I + 1
                    Next

                    dblYTPAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
                    dblYTPSemi = FinancialFunc.nominal(dblYTPAnn, 2)
                    dblYTPAnn = dblYTPAnn * 100
                    dblYTPSemi = dblYTPSemi * 100

                    If ShowCashflow And dtYTM.Rows.Count = 0 And dtYTC.Rows.Count = 0 Then
                        Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
                        strAmount = strAmount.Substring(0, strAmount.Length - 1)
                        strDate = strDate.Substring(0, strDate.Length - 1)
                        strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
                        ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateXIRRYield", "Error in CalculateXIRRYield", "", ex)
            Throw ex
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Public Sub CalculateXIRRPrice( _
            ByVal SecurityId As Integer, _
            ByVal SettementDate As String, _
            ByVal FrequencyOfInterest As Integer, _
            ByVal ShowCashflow As Boolean, _
            ByVal SemiOrAnnual As String, _
            ByVal RateOrActual As String, _
            Optional ByVal StepUp As String = "N", _
            Optional ByVal PriceType As String = "M")

        Dim lstParam As New List(Of SqlParameter)()
        Dim ds As New DataSet
        Dim dt As DataTable
        Dim I As Integer
        Dim J As Integer
        Dim strUrl As String
        Dim strAmount As String = ""
        Dim strDate As String = ""
        Dim dbltmpYTM As Double
        Dim ParVal As Double

        Try
            lstParam.Add(New SqlParameter("@SecurityId", SecurityId))
            lstParam.Add(New SqlParameter("@Dated", SettementDate))
            lstParam.Add(New SqlParameter("@StepUp", StepUp))
            lstParam.Add(New SqlParameter("@PriceType", PriceType))

            ds = objComm.FillDetails(lstParam, "WDM_Fill_SecurityPriceCashFlow")
            If ds.Tables.Count >= 1 Then
                I = 0
                J = 0
                dt = ds.Tables(0)
                If dt.Rows.Count > 0 Then
                    ReDim XirrDate(dt.Rows.Count - 1)
                    ReDim XirrAmt(dt.Rows.Count - 1)
                    dblPTM = 0
                    dblPTC = 0
                    dblPTP = 0

                    For Each row As DataRow In dt.Rows
                        XirrDate(I) = row("IPDate")
                        XirrAmt(I) = row("IPAmount")
                        strDate = strDate & CDate(row("IPDate")) & "!"
                        strAmount = strAmount & row("IPAmount") & "!"
                        I = I + 1
                    Next

                    CntXirr = 0
                    ParVal = Val(dt.Rows(0).Item("FaceValue"))
                    If FrequencyOfInterest > 0 Then
                        If PriceType = "M" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTMAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTMSemi
                            End If
                            dblPTM = XirrAmt(0)
                            dblPTM = FinancialFunc.XNPV(dbltmpYTM, XirrAmt, XirrDate)
                            dblPTM = IIf(RateOrActual = "R", dblPTM * 100 / ParVal, dblPTM)
                        ElseIf PriceType = "C" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTCAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTCSemi
                            End If
                            dblPTC = XirrAmt(0)
                            dblPTC = FinancialFunc.XNPV(dbltmpYTM, XirrAmt, XirrDate)
                            dblPTC = IIf(RateOrActual = "R", dblPTC * 100 / ParVal, dblPTC)
                        ElseIf PriceType = "P" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTPAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTPSemi
                            End If
                            dblPTP = XirrAmt(0)
                            dblPTP = FinancialFunc.XNPV(dbltmpYTM, XirrAmt, XirrDate)
                            dblPTP = IIf(RateOrActual = "R", dblPTP * 100 / ParVal, dblPTP)
                        End If
                        'For J = 1 To CntXirr
                        '    MarketVal = MarketVal + (XirrAmt(J) * (1 / (((1 + (dbltmpYTM / 200)) ^ ((DateDiff(DateInterval.Day, YTMDate, XirrDate(J))) / (365 / 2))))))
                        'Next
                    Else
                        If PriceType = "M" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTMAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTMSemi
                            End If

                            'dblPTM = FinancialFunc.XNPV(dbltmpYTM, XirrAmt, XirrDate)
                            For J = J To UBound(XirrAmt)
                                dblPTM = dblPTM + (XirrAmt(J) * (1 / (((1 + (dbltmpYTM / 200)) ^ ((DateDiff(DateInterval.Day, objCommon.DateFormat(SettementDate), DateValue(XirrDate(J)))) / (365 / 2))))))
                            Next
                            dblPTM = IIf(RateOrActual = "R", dblPTM * 100 / ParVal, dblPTM)
                        ElseIf PriceType = "C" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTCAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTCSemi
                            End If
                            For J = J To UBound(XirrAmt)
                                dblPTC = dblPTC + (XirrAmt(J) * (1 / (((1 + (dbltmpYTM / 200)) ^ ((DateDiff(DateInterval.Day, objCommon.DateFormat(SettementDate), DateValue(XirrDate(J)))) / (365 / 2))))))
                            Next
                            dblPTC = IIf(RateOrActual = "R", dblPTC * 100 / ParVal, dblPTC)
                        ElseIf PriceType = "P" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTPAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTPSemi
                            End If
                            For J = J To UBound(XirrAmt)
                                dblPTP = dblPTP + (XirrAmt(J) * (1 / (((1 + (dbltmpYTM / 200)) ^ ((DateDiff(DateInterval.Day, objCommon.DateFormat(SettementDate), DateValue(XirrDate(J)))) / (365 / 2))))))
                            Next
                            dblPTP = IIf(RateOrActual = "R", dblPTP * 100 / ParVal, dblPTP)
                        End If
                    End If

                    If ShowCashflow Then
                        Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
                        strAmount = strAmount.Substring(0, strAmount.Length - 1)
                        strDate = strDate.Substring(0, strDate.Length - 1)
                        strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
                        ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateXIRRPrice", "Error in CalculateXIRRPrice", "", ex)
            Throw ex
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub


    Public Sub CalculateMMYYield( _
               ByVal SecurityId As Integer, _
               ByVal SettlementDate As String, _
               ByVal Rate As Double, _
               ByVal SemiOrAnnual As String, _
               ByVal RateOrActual As String, Optional ByVal StepUp As String = "N")

        Dim lstParam As New List(Of SqlParameter)()
        Dim ds As New DataSet
        Dim dtYTM As DataTable
        Dim strType As String = ""

        Try
            lstParam.Add(New SqlParameter("@SecurityId", SecurityId))
            lstParam.Add(New SqlParameter("@SettlementDate", objCommon.DateFormat(SettlementDate)))
            lstParam.Add(New SqlParameter("@PurchaseRate", Rate))

            lstParam.Add(New SqlParameter("@StepUp", StepUp))
            ds = objComm.FillDetails(lstParam, "WDM_Fill_SecurityMMYYield")
            If ds.Tables.Count >= 0 Then
                dtYTM = ds.Tables(0)
                If dtYTM.Rows.Count > 0 Then
                    strType = dtYTM.Rows(0).Item("Type").ToString()
                    If strType = "M" Then
                        dblYTMAnn = dtYTM.Rows(0).Item("Yield").ToString()
                        dblYTMSemi = FinancialFunc.nominal(dblYTMAnn / 100, 2)
                        dblYTMSemi = dblYTMSemi * 100
                    ElseIf strType = "C" Then
                        dblYTCAnn = dtYTM.Rows(0).Item("Yield").ToString()
                        dblYTCSemi = FinancialFunc.nominal(dblYTCAnn / 100, 2)
                        dblYTCSemi = dblYTCSemi * 100
                    End If
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateMMYYield", "Error in CalculateMMYYield", "", ex)
            Throw ex
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Public Sub CalculateMMYPrice( _
                ByVal SecurityId As Integer, _
                ByVal SettlementDate As String, _
                ByVal SemiOrAnnual As String, _
                ByVal RateOrActual As String, _
                Optional ByVal PriceType As String = "M", Optional ByVal StepUp As String = "N")

        Dim lstParam As New List(Of SqlParameter)()
        Dim ds As New DataSet
        Dim dt As DataTable
        Dim strType As String = ""

        Try
            lstParam.Add(New SqlParameter("@SecurityId", SecurityId))
            lstParam.Add(New SqlParameter("@SettlementDate", objCommon.DateFormat(SettlementDate)))
            lstParam.Add(New SqlParameter("@Yield", IIf(PriceType = "M", dblYTMAnn, dblYTCAnn)))
            lstParam.Add(New SqlParameter("@PriceType", PriceType))
            lstParam.Add(New SqlParameter("@StepUp", StepUp))
            ds = objComm.FillDetails(lstParam, "WDM_Fill_SecurityMMYPrice")
            If ds.Tables.Count >= 0 Then
                dt = ds.Tables(0)
                If dt.Rows.Count > 0 Then
                    strType = dt.Rows(0).Item("Type").ToString()
                    If strType = "M" Then
                        dblPTM = dt.Rows(0).Item("Price").ToString()
                    ElseIf strType = "C" Then
                        dblPTC = dt.Rows(0).Item("Price").ToString()
                    End If
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateMMYPrice", "Error in CalculateMMYPrice", "", ex)
            Throw ex
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub
    Public Sub CalculateXIRRYield_CashFlow(
                             ByVal SecurityId As Integer,
                             ByVal SettementDate As String,
                             ByVal Rate As Double,
                             ByVal FrequencyOfInterest As Integer,
                             ByVal ShowCashflow As Boolean,
                             ByVal RateOrActual As String,
                             Optional ByVal StepUp As String = "N",
                             Optional ByVal YieldType As String = "M")

        Dim lstParam As New List(Of SqlParameter)()
        Dim ds As New DataSet
        Dim dtYTM As DataTable
        Dim dtYTC As DataTable
        Dim dtYTP As DataTable
        Dim I As Integer
        Dim strUrl As String
        Dim strAmount As String = ""
        Dim strDate As String = ""

        Try
            lstParam.Add(New SqlParameter("@SecurityId", SecurityId))
            lstParam.Add(New SqlParameter("@Dated", SettementDate))
            lstParam.Add(New SqlParameter("@PurchaseRate", Rate))
            lstParam.Add(New SqlParameter("@RateOrActual", RateOrActual))
            lstParam.Add(New SqlParameter("@StepUp", StepUp))
            lstParam.Add(New SqlParameter("@YieldType", YieldType))

            ds = objComm.FillDetails(lstParam, "WDM_Fill_SecurityYieldCashFlow")
            If ds.Tables.Count >= 3 Then
                I = 0
                dtYTM = ds.Tables(0)
                If dtYTM.Rows.Count > 0 Then
                    ReDim XirrDate(dtYTM.Rows.Count - 1)
                    ReDim XirrAmt(dtYTM.Rows.Count - 1)

                    For Each row As DataRow In dtYTM.Rows
                        XirrDate(I) = row("IPDate")
                        XirrAmt(I) = row("IPAmount")
                        strDate = strDate & CDate(row("IPDate")) & "!"
                        strAmount = strAmount & row("IPAmount") & "!"
                        I = I + 1
                    Next

                    dblYTMAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
                    dblYTMSemi = FinancialFunc.nominal(dblYTMAnn, 2)
                    dblYTMAnn = dblYTMAnn * 100
                    dblYTMSemi = dblYTMSemi * 100

                    'If ShowCashflow Then
                    '    Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
                    '    strAmount = strAmount.Substring(0, strAmount.Length - 1)
                    '    strDate = strDate.Substring(0, strDate.Length - 1)
                    '    strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
                    '    ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
                    'End If

                    'If ShowCashflow Then
                    Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
                    If strDate <> "" Then
                        strAmount = strAmount.Substring(0, strAmount.Length - 1)
                        strDate = strDate.Substring(0, strDate.Length - 1)
                        strCashAmount = strAmount
                        strCashDate = strDate
                    End If

                    'strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
                    'ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
                    'End If
                End If

                I = 0
                dtYTC = ds.Tables(1)
                If dtYTC.Rows.Count > 0 Then
                    ReDim XirrDate(dtYTC.Rows.Count - 1)
                    ReDim XirrAmt(dtYTC.Rows.Count - 1)
                    strAmount = ""
                    strDate = ""
                    For Each row As DataRow In dtYTC.Rows
                        XirrDate(I) = row("IPDate")
                        XirrAmt(I) = row("IPAmount")
                        strDate = strDate & CDate(row("IPDate")) & "!"
                        strAmount = strAmount & row("IPAmount") & "!"
                        I = I + 1
                    Next

                    dblYTCAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
                    dblYTCSemi = FinancialFunc.nominal(dblYTCAnn, 2)
                    dblYTCAnn = dblYTCAnn * 100
                    dblYTCSemi = dblYTCSemi * 100

                    'If ShowCashflow And dtYTM.Rows.Count = 0 Then
                    '    Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
                    '    strAmount = strAmount.Substring(0, strAmount.Length - 1)
                    '    strDate = strDate.Substring(0, strDate.Length - 1)

                    '    strCashAmount = strAmount
                    '    strCashDate = strDate
                    '    'strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
                    '    'ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
                    'End If

                    Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
                    If strDate <> "" Then
                        strAmount = strAmount.Substring(0, strAmount.Length - 1)
                        strDate = strDate.Substring(0, strDate.Length - 1)
                        strCashAmount = strAmount
                        strCashDate = strDate
                    End If
                End If

                I = 0
                dtYTP = ds.Tables(2)
                If dtYTP.Rows.Count > 0 Then
                    ReDim XirrDate(dtYTP.Rows.Count - 1)
                    ReDim XirrAmt(dtYTP.Rows.Count - 1)
                    strAmount = ""
                    strDate = ""
                    For Each row As DataRow In dtYTP.Rows
                        XirrDate(I) = row("IPDate")
                        XirrAmt(I) = row("IPAmount")
                        strDate = strDate & CDate(row("IPDate")) & "!"
                        strAmount = strAmount & row("IPAmount") & "!"
                        I = I + 1
                    Next

                    dblYTPAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
                    dblYTPSemi = FinancialFunc.nominal(dblYTPAnn, 2)
                    dblYTPAnn = dblYTPAnn * 100
                    dblYTPSemi = dblYTPSemi * 100

                    'If ShowCashflow And dtYTM.Rows.Count = 0 And dtYTC.Rows.Count = 0 Then
                    '    Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
                    '    strAmount = strAmount.Substring(0, strAmount.Length - 1)
                    '    strDate = strDate.Substring(0, strDate.Length - 1)
                    '    strCashAmount = strAmount
                    '    strCashDate = strDate
                    '    'strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
                    '    'ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
                    'End If
                    If strDate <> "" Then
                        strAmount = strAmount.Substring(0, strAmount.Length - 1)
                        strDate = strDate.Substring(0, strDate.Length - 1)
                        strCashAmount = strAmount
                        strCashDate = strDate
                    End If
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateXIRRYield", "Error in CalculateXIRRYield", "", ex)
            Throw ex
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub
    Public Sub CalculateXIRRPrice_CashFlow(
            ByVal SecurityId As Integer,
            ByVal SettementDate As String,
            ByVal FrequencyOfInterest As Integer,
            ByVal ShowCashflow As Boolean,
            ByVal SemiOrAnnual As String,
            ByVal RateOrActual As String,
            Optional ByVal StepUp As String = "N",
            Optional ByVal PriceType As String = "M")

        Dim lstParam As New List(Of SqlParameter)()
        Dim ds As New DataSet
        Dim dt As DataTable
        Dim I As Integer
        Dim J As Integer
        Dim strUrl As String
        Dim strAmount As String = ""
        Dim strDate As String = ""
        Dim dbltmpYTM As Double
        Dim ParVal As Double

        Try
            lstParam.Add(New SqlParameter("@SecurityId", SecurityId))
            lstParam.Add(New SqlParameter("@Dated", SettementDate))
            lstParam.Add(New SqlParameter("@StepUp", StepUp))
            lstParam.Add(New SqlParameter("@PriceType", PriceType))

            ds = objComm.FillDetails(lstParam, "WDM_Fill_SecurityPriceCashFlow")
            If ds.Tables.Count >= 1 Then
                I = 0
                J = 0
                dt = ds.Tables(0)
                If dt.Rows.Count > 0 Then
                    ReDim XirrDate(dt.Rows.Count - 1)
                    ReDim XirrAmt(dt.Rows.Count - 1)
                    dblPTM = 0
                    dblPTC = 0
                    dblPTP = 0

                    For Each row As DataRow In dt.Rows
                        XirrDate(I) = row("IPDate")
                        XirrAmt(I) = row("IPAmount")
                        strDate = strDate & CDate(row("IPDate")) & "!"
                        strAmount = strAmount & row("IPAmount") & "!"
                        I = I + 1
                    Next

                    CntXirr = 0
                    ParVal = Val(dt.Rows(0).Item("FaceValue"))
                    If FrequencyOfInterest > 0 Then
                        If PriceType = "M" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTMAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTMSemi
                            End If
                            dblPTM = XirrAmt(0)
                            dblPTM = FinancialFunc.XNPV(dbltmpYTM, XirrAmt, XirrDate)
                            dblPTM = IIf(RateOrActual = "R", dblPTM * 100 / ParVal, dblPTM)
                        ElseIf PriceType = "C" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTCAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTCSemi
                            End If
                            dblPTC = XirrAmt(0)
                            dblPTC = FinancialFunc.XNPV(dbltmpYTM, XirrAmt, XirrDate)
                            dblPTC = IIf(RateOrActual = "R", dblPTC * 100 / ParVal, dblPTC)
                        ElseIf PriceType = "P" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTPAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTPSemi
                            End If
                            dblPTP = XirrAmt(0)
                            dblPTP = FinancialFunc.XNPV(dbltmpYTM, XirrAmt, XirrDate)
                            dblPTP = IIf(RateOrActual = "R", dblPTP * 100 / ParVal, dblPTP)
                        End If
                        'For J = 1 To CntXirr
                        '    MarketVal = MarketVal + (XirrAmt(J) * (1 / (((1 + (dbltmpYTM / 200)) ^ ((DateDiff(DateInterval.Day, YTMDate, XirrDate(J))) / (365 / 2))))))
                        'Next
                    Else
                        If PriceType = "M" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTMAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTMSemi
                            End If

                            'dblPTM = FinancialFunc.XNPV(dbltmpYTM, XirrAmt, XirrDate)
                            For J = J To UBound(XirrAmt)
                                dblPTM = dblPTM + (XirrAmt(J) * (1 / (((1 + (dbltmpYTM / 200)) ^ ((DateDiff(DateInterval.Day, objCommon.DateFormat(SettementDate), DateValue(XirrDate(J)))) / (365 / 2))))))
                            Next
                            dblPTM = IIf(RateOrActual = "R", dblPTM * 100 / ParVal, dblPTM)
                        ElseIf PriceType = "C" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTCAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTCSemi
                            End If
                            For J = J To UBound(XirrAmt)
                                dblPTC = dblPTC + (XirrAmt(J) * (1 / (((1 + (dbltmpYTM / 200)) ^ ((DateDiff(DateInterval.Day, objCommon.DateFormat(SettementDate), DateValue(XirrDate(J)))) / (365 / 2))))))
                            Next
                            dblPTC = IIf(RateOrActual = "R", dblPTC * 100 / ParVal, dblPTC)
                        ElseIf PriceType = "P" Then
                            If SemiOrAnnual = "A" Then
                                dbltmpYTM = FinancialFunc.nominal(dblYTPAnn / 100, 2) * 100
                            Else
                                dbltmpYTM = dblYTPSemi
                            End If
                            For J = J To UBound(XirrAmt)
                                dblPTP = dblPTP + (XirrAmt(J) * (1 / (((1 + (dbltmpYTM / 200)) ^ ((DateDiff(DateInterval.Day, objCommon.DateFormat(SettementDate), DateValue(XirrDate(J)))) / (365 / 2))))))
                            Next
                            dblPTP = IIf(RateOrActual = "R", dblPTP * 100 / ParVal, dblPTP)
                        End If
                    End If

                    'If ShowCashflow Then
                    Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
                    If strDate <> "" Then
                        strAmount = strAmount.Substring(0, strAmount.Length - 1)
                        strDate = strDate.Substring(0, strDate.Length - 1)
                        strCashAmount = strAmount
                        strCashDate = strDate
                    End If

                    'strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
                    'ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
                    'End If
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalculateXIRRPrice", "Error in CalculateXIRRPrice", "", ex)
            Throw ex
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub
End Module