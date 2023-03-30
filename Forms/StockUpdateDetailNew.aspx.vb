Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Net
Imports System.Net.Mail

Partial Class Forms_StockUpdateDetail

    Inherits System.Web.UI.Page
    Dim sqlComm As New SqlCommand
    Dim objCommon As New clsCommonFuns
    Dim dsmenu As DataSet
    Dim dsDPDetails As DataSet
    Dim newTable As DataTable
    Dim DpDetailsTable As DataTable
    Dim trmenu As DataRow
    Dim sqlConn As SqlConnection
    Dim intPageIndex As Int32
    Dim MatDate() As Date
    Dim MatAmt() As Double
    Dim CoupDate() As Date
    Dim CoupRate() As Double
    Dim CallDate() As Date
    Dim CallAmt() As Double
    Dim PutDate() As Date
    Dim PutAmt() As Double
    Dim strCoupFlag As String
    Dim datYTM As Date
    Dim decFaceValue As Decimal
    Dim datIssue As Date
    Dim datInterest As Date
    Dim datBookClosure As Date
    Dim decRate As Decimal
    Public decYield As Decimal
    Public decYTC As Decimal
    Public decYTM As Decimal
    Public decYTCSem As Decimal
    Public decYTMSemi As Decimal
    Dim blnNonGovernment As Boolean
    Dim blnRateActual As Boolean
    Dim blnDMAT As Boolean
    Dim intBKDiff As Integer
    Dim dblPepCoupRate As Double
    Dim blnCompRate As Boolean
    Dim blnCloseButton As Boolean
    Dim txt_rate As Decimal
    Dim SecurityId As Decimal
    Dim rbl_Days As Integer
    Dim BrokenInt As Boolean
    Dim InterestOnHoliday As Boolean
    Dim InterestOnSat As Boolean
    Dim MaturityOnHoliday As Boolean
    Dim MaturityOnSat As Boolean
    Dim strSemiAnnFlag As String = ""
    Dim RateActualFlag As String
    Dim PhysicalDematFlag As String
    Dim CombineIPMat As Boolean
    Dim EqualActualFlag As String
    Dim intDays As Int16
    Dim FirstAllYr As Char
    Dim ytmdt As Date
    Dim ShowNumber As String
    Dim PerpetualFlag As String
    Dim CCount As Integer = 0

    Public Class SecIds
        Public Id As Decimal

        Public Sub SecIds()
        End Sub
        Public Sub SecIds(ByVal Id As Decimal)
            Id = Id
        End Sub
    End Class


    Public Class DailyMarketData
        Public SecurityId As Int32
        Public MarketRate As Decimal
        Public SellingRate As Decimal

        Public Added As Boolean
        Public InStock As Boolean
        Public Yield As Decimal
        Public NoOfBonds As Int32
        Public ShowNumber As Int32

        Public Sub DailyMarketData()
        End Sub

        Public Sub DailyMarketData(ByVal SecurityID As Int32, ByVal MarketRate As Int32, _
        ByVal SellingRate As Int32, ByVal Added As Boolean, ByVal InStock As Boolean, _
        ByVal Yield As Decimal, ByVal NoOfBonds As Decimal, ByVal ShowNumber As Decimal)

            SecurityID = SecurityID
            MarketRate = MarketRate
            SellingRate = SellingRate
            Added = Added
            InStock = InStock
            Yield = Yield
            NoOfBonds = NoOfBonds
            ShowNumber = ShowNumber
        End Sub

    End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If

            If Request.Params("secids") = "" Then
                'objCommon.OpenConn()
                Response.Buffer = True
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
                Response.Expires = -1500

                Response.CacheControl = "no-cache"
                Response.AddHeader("Pragma", "no-cache")
                Response.AddHeader("Cache-Control", "no-cache")
                Response.AddHeader("Cache-Control", "no-store")

                If IsPostBack = False Then
                    txt_ForDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
                    txt_ForDate.Attributes.Add("onkeypress", "OnlyDate();")
                    txt_ForDate.Attributes.Add("onblur", "CheckDate(this,false);")
                    txt_ForDate.Attributes.Add("onblur", "CheckDate(this,false);")
                    btn_Save.Attributes.Add("onclick", "return ValidateDate();")

                    'FillBlankSecurityGrids()
                    FillDailyMarketValueGridsecurity("SecurityTypeId ASC,SecurityName ASC", 0)

                End If

                Dim FormWidth As Int32 = 350
                Dim FormHeight As Int32 = 475

                Dim SourceType As String = "StoredProcedure"
                Dim TableName As String = Nothing
                Dim ProcName As String = "ID_SEARCH_SecurityMaster"

                Dim SelectedFieldName As String = "SecurityName"
                Dim SelectedValueName As String = "SecurityId"
                Dim ConditionalFieldName As String = ""

                Dim ConditionExist As String = "false"
                Dim ShowAll As String = "false"
                Dim CheckYearCompany As String = "false"

                Dim ConditionalFieldName1 As String = Nothing
                Dim ConditionalFieldName2 As String = Nothing
            Else
                Hid_SecIdsSend.Value = Request.Params("secids")
                Session("SecIds") = Request.Params("secids")
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillBlankSecurityGrids()
        Try
            Dim dt As New DataTable
            Dim objSrh As New clsSearch

            dt.Columns.Add("SecurityId", GetType(Int32))
            dt.Columns.Add("SecurityEvalId", GetType(Int32))
            dt.Columns.Add("StockUpdtId", GetType(Int32))
            dt.Columns.Add("TypeFlag", GetType(String))
            dt.Columns.Add("SecurityName", GetType(String))
            dt.Columns.Add("SecurityTypeName", GetType(String))
            dt.Columns.Add("ISIN", GetType(String))
            dt.Columns.Add("MaturityDate", GetType(String))
            dt.Columns.Add("CallDate", GetType(String))
            dt.Columns.Add("QTM", GetType(String))
            dt.Columns.Add("BookStock", GetType(String))
            dt.Columns.Add("Rating", GetType(String))
            dt.Columns.Add("SellingRate", GetType(String))
            dt.Columns.Add("Yield", GetType(String))
            dt.Columns.Add("ShowNumber", GetType(Int32))
            dt.Columns.Add("StockFaceValue", GetType(Double))
            dt.Columns.Add("YTCAnn", GetType(String))
            dt.Columns.Add("YTPAnn", GetType(String))
            dt.Columns.Add("YTMSemi", GetType(String))
            dt.Columns.Add("YTCSemi", GetType(String))
            dt.Columns.Add("YTPSemi", GetType(String))
            dt.Columns.Add("Semi_Ann_Flag", GetType(String))
            dt.Columns.Add("CombineIPMat", GetType(String))
            dt.Columns.Add("Rate_Actual_Flag", GetType(String))
            dt.Columns.Add("Equal_Actual_Flag", GetType(String))
            dt.Columns.Add("IntDays", GetType(Int32))
            dt.Columns.Add("FirstYrAllYr", GetType(String))
            dt.Columns.Add("NatureOFInstrument", GetType(String))

            Dim datarow As DataRow = dt.NewRow()
            datarow("SecurityId") = 0
            datarow("StockUpdtId") = 0
            datarow("TypeFlag") = ""
            datarow("SecurityName") = ""
            datarow("SecurityTypeName") = ""
            datarow("NatureOFInstrument") = ""
            datarow("ISIN") = ""
            datarow("MaturityDate") = ""
            datarow("CallDate") = ""
            datarow("QTM") = ""
            datarow("BookStock") = 0.0
            datarow("Rating") = 0.0
            datarow("SellingRate") = 0.0
            datarow("Yield") = ""
            datarow("ShowNumber") = 0
            datarow("Rating") = ""
            datarow("SellingRate") = 0
            datarow("FaceValue") = 0.0
            datarow("BookStock") = ""
            datarow("Semi_Ann_Flag") = 0.0
            datarow("CombineIPMat") = 0.0
            datarow("Rate_Actual_Flag") = ""
            datarow("Equal_Actual_Flag") = ""
            datarow("IntDays") = 0
            datarow("FirstYrAllYr") = ""
            dt.Rows.Add(datarow)
            Session("TempSecurityTable") = dt

            objSrh.SetGrid(dg_dme, dt)
            dg_dme.DataSource = dt
            dg_dme.DataBind()

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

    Private Sub FillDailyMarketValueGridsecurity(ByVal strSort As String, Optional ByVal intPageIndex As Int16 = 0)
        Try
            ytmdt = objCommon.DateFormat(txt_ForDate.Text)
            Dim strcon As String = ""
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldt1 As New DataTable
            Dim sqldv As New DataView
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandTimeout = "100"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_FILL_ViewUpdatedStock"
            sqlComm.Parameters.Clear()
            If Trim(txt_ForDate.Text) <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ForDate.Text))
            End If
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId")))
            objCommon.SetCommandParameters(sqlComm, "@compid", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)
            sqldv = sqldt.DefaultView

            'sqldv.RowFilter = "YTMDate='" + objCommon.DateFormat(txt_ForDate.Text) + "'"
            sqldv.RowFilter = ""

            If sqldv.Count = 0 Then
                Dim sqlda1 As New SqlDataAdapter


                sqlComm.Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
                objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId")))
                objCommon.SetCommandParameters(sqlComm, "@compid", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
                objCommon.SetCommandParameters(sqlComm, "@newflag", SqlDbType.Int, 4, "I", , , 1)
                objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
                sqlComm.ExecuteNonQuery()
                sqlda.SelectCommand = sqlComm
                sqlda.Fill(sqldt)

                If sqldt.Rows.Count > 0 Then
                    sqldv.RowFilter = ""
                    sqldv = sqldt.DefaultView
                End If
            End If

            dg_dme.DataSource = sqldv
            dg_dme.DataBind()

            Session("SecurityMaster") = sqldt
            If sqldt.Rows.Count = 0 Then
                btn_Save.Visible = False
            Else
                btn_Save.Visible = True
                'FillBlankSecurityGrids()
            End If
            'FillBlankSecurityGrids()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_View_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        FillDailyMarketValueGridsecurity("SecurityTypeId ASC,SecurityName ASC", 0)
    End Sub

    Private Sub FillDailyMarketValueAdditioanl()
        Try
            lblMsg.Visible = False
            lblMsg.Text = ""
            If Hid_SelVal.Value <> "" Then

                Dim arrSecId As String() = Hid_SelVal.Value.Split(",")
                Dim list As New List(Of SecIds)

                For i As Int32 = 0 To arrSecId.Length - 2 Step +1
                    Dim secid_ As String = arrSecId(i)
                    If secid_ <> "" Then
                        Dim secid As Decimal = Convert.ToDecimal(arrSecId(i))
                        Dim secid1 As New SecIds()
                        secid1.Id = secid
                        list.Add(secid1)
                    End If
                Next

                Dim xmldoc As New XmlDocument
                Dim xs As New XmlSerializer(GetType(List(Of SecIds)))

                Using stream As New MemoryStream()
                    xs.Serialize(stream, list)
                    stream.Flush()
                    stream.Seek(0, SeekOrigin.Begin)
                    xmldoc.Load(stream)
                End Using

                Dim sqlda As New SqlDataAdapter
                Dim dtAdd As New DataTable
                Dim sqldv As New DataView
                OpenConn()
                sqlComm.Connection = sqlConn
                sqlComm.CommandTimeout = "100"
                sqlComm.CommandType = CommandType.StoredProcedure
                sqlComm.CommandText = "ID_SEARCH_SecurityFieldsNew_StockUpdate"
                sqlComm.Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@Ids_Xml", SqlDbType.Xml, 0, "I", , , xmldoc.OuterXml)
                objCommon.SetCommandParameters(sqlComm, "@exist", SqlDbType.Bit, 0, "I", , , True)
                objCommon.SetCommandParameters(sqlComm, "@forDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ForDate.Text))
                objCommon.SetCommandParameters(sqlComm, "@stock_updtflag", SqlDbType.Bit, 0, "I", , , False)
                objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
                sqlComm.ExecuteNonQuery()
                sqlda.SelectCommand = sqlComm
                sqlda.Fill(dtAdd)
                sqldv = dtAdd.DefaultView

                Dim dtMerge As DataTable = New DataTable()
                Dim dvMerge As DataView
                Dim dtSec As DataTable = Session("SecurityMaster")
                Dim dtSecAdd As DataTable = Session("SecAdd")

                If dtSecAdd Is Nothing Then
                    dtMerge = dtSec.Copy()
                    dtMerge.Merge(dtAdd, True, MissingSchemaAction.Ignore)
                    Session("SecurityMaster") = dtMerge
                End If

                dvMerge = dtMerge.DefaultView
                dvMerge.Sort = "SecurityName ASC"

                dg_dme.DataSource = dvMerge
                dg_dme.DataBind()

                If dtMerge.Rows.Count = 0 Then
                    btn_Save.Visible = False
                Else
                    btn_Save.Visible = True
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillGridWithCondition(ByVal bShowAll As Boolean, Optional ByVal strSort As String = "")
        Try
            Dim strcon As String = ""
            Dim objSearch As New clsSearch()
            Dim dtGrid As New DataTable()
            Dim dtMerge As DataTable

            dtGrid = DirectCast(Session("SecurityMaster"), DataTable)
            Dim DvMerg As New DataView(dtGrid)

            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandTimeout = "100"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_SEARCH_StockUpdateMaster_UpdatedStock"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId")))
            objCommon.SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 1000, "I", 0, 0, strcon)
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)
            If strSort <> "" Then
                DvMerg.Sort = strSort
            End If
            dg_dme.DataSource = sqldt
            dg_dme.DataBind()

        Catch ex As Exception
            'Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "');", true);
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Function GetFrequency(ByVal strFrequency As String) As Int16
        Try
            Select Case UCase(strFrequency)
                Case "Y"
                    Return 1
                Case "H"
                    Return 2
                Case "Q"
                    Return 4
                Case "M"
                    Return 12
                Case "N"
                    Return 0
            End Select
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Sub SetValues()
        Try

            With objCommon
                ' datYTM = .DateFormat(txt_ForDate.Text)
                datYTM = .DateFormat(txt_ForDate.Text)
                datInterest = Hid_InterestDate.Value
                datBookClosure = Hid_BookClosureDate.Value
                blnNonGovernment = IIf(Hid_GovernmentFlag.Value = "N", True, False)

                ' blnRateActual = IIf(rdo_RateActual.SelectedValue = "R", True, False)
                blnRateActual = True

                '  blnDMAT = IIf(rdo_PhysicalDMAT.SelectedValue = "P", False, True)
                blnDMAT = True

                decFaceValue = .DecimalFormat(Val(Hid_FaceValue.Value))
                'decRate = .DecimalFormat(Val(txt_rate))

                datIssue = Hid_Issue.Value
                datBookClosure = IIf(blnDMAT = True, Hid_DMATBkDate.Value, Hid_BookClosureDate.Value)
                intBKDiff = CalculateBookClosureDiff(datBookClosure, "D", datInterest, blnNonGovernment)
                'blnCompRate = chk_CompRate.Checked
                dblYTMAnn = 0
                dblYTCAnn = 0
                dblYTPAnn = 0
                dblYTMSemi = 0
                dblYTCSemi = 0
                dblYTPSemi = 0
            End With

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Private Sub FillSecurityInfoDetails(ByVal InfoDate As Date, ByVal InfoAmt As Decimal, ByVal TypeFlag As String)
        Try
            Select Case TypeFlag
                Case "M"
                    Hid_MatDate.Value += InfoDate & "!"
                    Hid_MatAmt.Value += InfoAmt & "!"
                Case "I"
                    If InfoDate <> Date.MinValue Then
                        Hid_CoupDate.Value += InfoDate & "!"
                        Hid_CoupRate.Value += InfoAmt & "!"
                    Else
                        dblPepCoupRate = InfoAmt
                    End If
                Case "C"
                    Hid_CallDate.Value += InfoDate & "!"
                    Hid_CallAmt.Value += InfoAmt & "!"
                Case "P"
                    Hid_PutDate.Value += InfoDate & "!"
                    Hid_PutAmt.Value += InfoAmt & "!"
            End Select
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub



    Private Function FillDateArrays(ByVal strValue As String) As Date()
        Dim arrDate() As Date
        Dim strDate() As String
        Dim I As Int32

        strDate = Split(strValue, "!")
        ReDim arrDate(strDate.Length - 2)

        Try
            'arrDate(0) = Date.MinValue
            For I = 0 To strDate.Length - 2
                If strDate(I) <> "" Then arrDate(I) = CDate(strDate(I))
            Next

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

        Return arrDate

    End Function

    Private Function FillAmtArrays(ByVal strValue As String) As Double()
        Dim arrDouble() As Double = Nothing
        Try
            Dim strDate() As String
            Dim I As Int32

            strDate = Split(strValue, "!")
            ReDim arrDouble(strDate.Length - 2)

            For I = 0 To strDate.Length - 2
                If strDate(I) <> "" Then arrDouble(I) = CDbl(strDate(I))
            Next

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

        Return arrDouble

    End Function

    Protected Sub DeleteDailyMarketValue()
        Try
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandTimeout = "100"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_DailyMarketValueEntry"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 4, "I", , , Now)
            objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        lblMsg.Visible = False
        lblMsg.Text = ""
        UpdateYield()
        FillDailyMarketValueGridsecurity("SecurityTypeId ASC,SecurityName ASC", 0)
        lblMsg.Visible = True
        lblMsg.Text = "Stock Updated Successfully."
        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "$('#ctl00_ContentPlaceHolder1_btn_Send').removeAttr('disabled'); alert('Stock Updated Successfuly')", True)
    End Sub
    Public Sub UpdateYield()
        Try
            Dim dt As DataTable
            Dim StockUpdtId As Int32
            Dim SecurityTypeFlag As Char

            ytmdt = objCommon.DateFormat(txt_ForDate.Text)
            Dim dtData As New DataTable()
            dtData.Columns.Add("SecurityEvalId", GetType(Int32))
            dtData.Columns.Add("YTMDate", GetType(Date))
            dtData.Columns.Add("Rate", GetType(Decimal))
            dtData.Columns.Add("YTMAnn", GetType(Decimal))
            dtData.Columns.Add("YTMSemi", GetType(Decimal))
            dtData.Columns.Add("YTCAnn", GetType(Decimal))
            dtData.Columns.Add("YTCSemi", GetType(Decimal))
            dtData.Columns.Add("YTPAnn", GetType(Decimal))
            dtData.Columns.Add("YTPSemi", GetType(Decimal))
            dtData.Columns.Add("UserId", GetType(Int32))

            Dim dtNewSecurity As New DataTable()
            dtNewSecurity.Columns.Add("SecurityId", GetType(Int32))
            dtNewSecurity.Columns.Add("YTMDate", GetType(Date))
            dtNewSecurity.Columns.Add("Rate", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTMAnn", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTMSemi", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTCAnn", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTCSemi", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTPAnn", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTPSemi", GetType(Decimal))
            dtNewSecurity.Columns.Add("UserId", GetType(Int32))

            OpenConn()
            'txt_ShowNumber 
            For K As Int32 = 0 To dg_dme.Rows.Count - 1
                SecurityTypeFlag = CType(dg_dme.Rows(K).FindControl("lbl_TypeFlag"), Label).Text
                Dim strSecurityNature As String = ""
                StockUpdtId = CType(dg_dme.Rows(K).FindControl("lbl_StockUpdtId"), Label).Text
                '  StockUpdtId = Convert.ToInt32(CType(dg_dme.Rows(K).FindControl("lbl_StockUpdtId"), Label).Text)
                'StockUpdtId = 0

                'If CType(dg_dme.Rows(K).FindControl("txt_ShowNumber"), TextBox).Text = "" Then
                '    ShowNumber = "0"
                'Else
                '    ShowNumber = Convert.ToString(CType(dg_dme.Rows(K).FindControl("txt_ShowNumber"), TextBox).Text)
                'End If
                Hid_CFlag.Value = Hid_CFlag.Value

                PerpetualFlag = CType(dg_dme.Rows(K).FindControl("lbl_PerpetualFlag"), Label).Text
                strSemiAnnFlag = CType(dg_dme.Rows(K).FindControl("lbl_Semi_Ann_Flag"), Label).Text
                Hid_Rate.Value = CType(dg_dme.Rows(K).FindControl("txt_SellingRate"), TextBox).Text

                'If Hid_Rate.Value <> "0.0000" Then
                '    decYield = 0.0
                'Else
                '    decYield = Convert.ToDecimal(CType(dg_dme.Rows(K).FindControl("txt_Yield"), TextBox).Text)
                'End If

                decYield = Convert.ToDecimal(CType(dg_dme.Rows(K).FindControl("txt_Yield"), TextBox).Text)
                Dim ytc As String = CType(dg_dme.Rows(K).FindControl("txt_YTC"), TextBox).Text
                If ytc = "" Then
                    CType(dg_dme.Rows(K).FindControl("txt_YTC"), TextBox).Text = "0.0000"
                End If
                decYTC = Convert.ToDecimal(CType(dg_dme.Rows(K).FindControl("txt_YTC"), TextBox).Text)
                'Dim ytcsemi As String = CType(dg_dme.Rows(K).FindControl("txt_YTCSemi"), TextBox).Text
                'If ytcsemi = "" Or ytcsemi = "0.0000" Then
                '    decYTCSem = 0.0
                'Else
                '    decYTCSem = Convert.ToDecimal(CType(dg_dme.Rows(K).FindControl("txt_YTCSemi"), TextBox).Text)
                'End If

                'Dim ytmsemi As String = CType(dg_dme.Rows(K).FindControl("txt_YTMSemi"), TextBox).Text
                'If ytmsemi = "" Or ytmsemi = "0.0000" Then
                '    decYTMSemi = 0.0
                'Else
                '    decYTMSemi = Convert.ToDecimal(CType(dg_dme.Rows(K).FindControl("txt_YTMSemi"), TextBox).Text)
                'End If


                'decYTMSemi = Convert.ToDecimal(CType(dg_dme.Rows(K).FindControl("txt_YTMSemi"), TextBox).Text)
                decRate = Convert.ToDecimal(Hid_Rate.Value)
                Hid_YTMDate.Value = ytmdt
                RateActualFlag = CType(dg_dme.Rows(K).FindControl("lbl_Rate_Actual_Flag"), Label).Text
                PhysicalDematFlag = "D"
                CombineIPMat = CType(dg_dme.Rows(K).FindControl("lbl_CombineIPMat"), Label).Text
                EqualActualFlag = CType(dg_dme.Rows(K).FindControl("lbl_Equal_Actual_Flag"), Label).Text
                If CType(dg_dme.Rows(K).FindControl("lbl_IntDays"), Label).Text = "" Then
                    intDays = 0
                Else
                    intDays = CType(dg_dme.Rows(K).FindControl("lbl_IntDays"), Label).Text
                End If

                FirstAllYr = CType(dg_dme.Rows(K).FindControl("lbl_FirstYrAllYr"), Label).Text
                Hid_MatDate.Value = ""
                Hid_MatAmt.Value = ""
                Hid_CoupDate.Value = ""
                Hid_CoupRate.Value = ""
                Hid_CallDate.Value = ""
                Hid_CallAmt.Value = ""
                Hid_PutDate.Value = ""
                Hid_PutAmt.Value = ""
                Hid_SecurityId.Value = CType(dg_dme.Rows(K).FindControl("lbl_Security"), Label).Text

                'Fill Options() is common to calculate yield --- no change in this function.
                FillOptions()
                If SecurityId = 0 Then Exit Sub

                MatDate = FillDateArrays(Hid_MatDate.Value)
                MatAmt = FillAmtArrays(Hid_MatAmt.Value)
                CallDate = FillDateArrays(Hid_CallDate.Value)
                CallAmt = FillAmtArrays(Hid_CallAmt.Value)


                CoupDate = FillDateArrays(Hid_CoupDate.Value)
                CoupRate = FillAmtArrays(Hid_CoupRate.Value)
                PutDate = FillDateArrays(Hid_PutDate.Value)
                PutAmt = FillAmtArrays(Hid_PutAmt.Value)
                SetValues()
                GetNextIntDate(Val(Hid_SecurityId.Value), ytmdt)
                Dim YieldType As Char ' = dt.Rows(K).Item("YTM_XIRR_MMY_Flag")

                If Hid_RateAmtFlag.Value = "A" Then
                    For M As Int32 = 0 To CoupRate.Length - 1
                        CoupRate(M) = (CoupRate(M) / decFaceValue) * 100
                    Next
                End If

                Select Case SecurityTypeFlag
                    Case "G"
                        FillYieldOptions()
                    Case "N"
                        'FillXIRROptionsOld()
                        FillXIRROptions()
                End Select

                If strSemiAnnFlag = "A" Then
                    'Hid_YTMSemi.Value = 0.0
                    'Hid_YTCSemi.Value = 0.0
                    'Hid_YTPSemi.Value = 0.0
                End If

                If StockUpdtId <> 0 Then
                    If CCount > 1 Then
                    Else
                        Dim datarow As DataRow = dtData.NewRow()
                        datarow("SecurityEvalId") = StockUpdtId
                        datarow("YTMDate") = objCommon.DateFormat(txt_ForDate.Text)
                        datarow("Rate") = Val(Hid_Rate.Value)
                        datarow("YTMAnn") = Val(Hid_YTMAnn.Value)
                        datarow("YTMSemi") = Val(Hid_YTMSemi.Value)
                        datarow("YTCAnn") = Val(Hid_YTCAnn.Value)
                        datarow("YTCSemi") = Val(Hid_YTCSemi.Value)
                        datarow("YTPAnn") = Val(Hid_YTPAnn.Value)
                        datarow("YTPSemi") = Val(Hid_YTCSemi.Value)
                        datarow("UserId") = Val(Session("UserId"))
                        dtData.Rows.Add(datarow)
                    End If
                Else
                    If CCount > 1 Then
                    Else
                        Dim datarow As DataRow = dtNewSecurity.NewRow()
                        datarow("SecurityId") = SecurityId
                        datarow("YTMDate") = objCommon.DateFormat(txt_ForDate.Text)
                        datarow("Rate") = Val(Hid_Rate.Value)
                        datarow("YTMAnn") = Val(Hid_YTMAnn.Value)
                        datarow("YTMSemi") = Val(Hid_YTMSemi.Value)
                        datarow("YTCAnn") = Val(Hid_YTCAnn.Value)
                        datarow("YTCSemi") = Val(Hid_YTCSemi.Value)
                        datarow("YTPAnn") = Val(Hid_YTPAnn.Value)
                        datarow("YTPSemi") = Val(Hid_YTCSemi.Value)
                        datarow("UserId") = Val(Session("UserId"))
                        dtNewSecurity.Rows.Add(datarow)
                    End If
                End If

                CCount = 0
            Next
            Dim sqlComm As New SqlCommand
            OpenConn()
            If dtData.Rows.Count > 0 Or dtNewSecurity.Rows.Count Then
                sqlComm.CommandType = CommandType.StoredProcedure
                sqlComm.Connection = sqlConn
                sqlComm.CommandText = "ID_UPDATE_MultiStockQuoteUpdate"
                sqlComm.Parameters.Clear()
                sqlComm.Parameters.Add("@UpdateQuote", SqlDbType.Structured).Value = dtData
                sqlComm.Parameters.Add("@InsertQuote", SqlDbType.Structured).Value = dtNewSecurity
                sqlComm.ExecuteNonQuery()
            End If
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub dg_dme_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        dg_dme.PageIndex = e.NewPageIndex
        dg_dme.DataSource = Session("SecurityMaster")
        dg_dme.DataBind()
    End Sub

    Protected Sub dg_dme_RowCommand(ByVal sender As Object, ByVal e As WebControls.GridViewCommandEventArgs) Handles dg_dme.RowCommand
        Try
            Dim gvRow As GridViewRow
            Dim imgBtn As ImageButton
            Dim strId As String
            Dim strUrl As String
            Dim strPage As String
            Dim Id As Integer

            If e.CommandName = "DeleteRow" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                gvRow = imgBtn.Parent.Parent
                OpenConn()

                Id = Convert.ToInt32(e.CommandArgument)
                If DeleteStockQuote(Id) = True Then FillDailyMarketValueGridsecurity("SecurityTypeId ASC,SecurityName ASC", 0)


            End If
        Catch ex As Exception
            'objUtil.WritErrorLog(PgName, "gv_Details_RowCommand", "Error in gv_Details_RowCommand", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function DeleteStockQuote(ByVal intId As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_StockUpdateQuote"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@StockUpdtId", SqlDbType.VarChar, 30, "I", , , intId)
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            'sqlTrans.Rollback()
            'objUtil.WritErrorLog(PgName, "DeleteMergeDeal", "Error in DeleteMergeDeal", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Sub GetNextIntDate(ByVal SecurityId As Int64, ByVal YtmDate As DateTime)
        Try
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView

            Dim sqlComm As New SqlCommand
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_FILL_NextIntDate"
            sqlComm.Parameters.Clear()

            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "I", , , SecurityId)
            objCommon.SetCommandParameters(sqlComm, "@YTMDate", SqlDbType.DateTime, 4, "I", , , objCommon.DateFormat(YtmDate))
            sqlComm.ExecuteNonQuery()

            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)

            If sqldt.Rows.Count > 0 Then
                Hid_NextIntDate.Value = sqldt.Rows(0).Item("NextIntDate")
            End If
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillOptions()
        Try
            Dim dtSecurity As DataTable
            Dim strSecurityNature As String = ""
            Dim datInfo As Date
            OpenConn()

            dtSecurity = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Val(Hid_SecurityId.Value), "SecurityId")
            If dtSecurity IsNot Nothing Then

            End If
            For x As Int32 = 0 To dtSecurity.Rows.Count - 1
                With dtSecurity.Rows(x)
                    SecurityId = Val(.Item("SecurityId") & "")
                    chrMaxActFlag = Trim(.Item("MaxActualFlag") & "")
                    strSecurityNature = Trim(.Item("NatureOfInstrument") & "")
                    Hid_Issuer.Value = Trim(.Item("SecurityIssuer") & "")
                    Hid_Security.Value = Trim(.Item("SecurityName") & "")
                    Hid_SecurityMatDate.Value = IIf(Trim(.Item("MaturityDate") & "") = "", Date.MinValue, .Item("MaturityDate"))
                    Hid_BookClosureDate.Value = IIf(Trim(.Item("BookClosureDate") & "") = "", Date.MinValue, .Item("BookClosureDate"))
                    Hid_InterestDate.Value = IIf(Trim(.Item("FirstInterestDate") & "") = "", Date.MinValue, .Item("FirstInterestDate"))
                    Hid_DMATBkDate.Value = IIf(Trim(.Item("DMATBookClosureDate") & "") = "", Date.MinValue, .Item("DMATBookClosureDate"))
                    Hid_Issue.Value = IIf(Trim(.Item("IssueDate") & "") = "", Date.MinValue, .Item("IssueDate"))
                    datInfo = IIf(Trim(.Item("SecurityInfoDate") & "") = "", Date.MinValue, .Item("SecurityInfoDate"))
                    Hid_GovernmentFlag.Value = Trim(.Item("GovernmentFlag") & "")
                    Hid_Frequency.Value = GetFrequency(Trim(.Item("FrequencyOfInterest") & ""))
                    Hid_FaceValue.Value = Val(.Item("FaceValue") & "")
                    Hid_RateAmtFlag.Value = Trim(.Item("CouponOn") & "")
                    'BrokenInt = (.Item("BrokenInterest"))
                    InterestOnHoliday = (.Item("InterestOnHoliday"))
                    InterestOnSat = (.Item("InterestOnSat"))
                    MaturityOnHoliday = (.Item("MaturityOnHoliday"))
                    MaturityOnSat = (.Item("MaturityOnSat"))
                    FillSecurityInfoDetails(datInfo, Val(.Item("SecurityInfoAmt") & ""), Trim(.Item("TypeFlag") & ""))
                End With
            Next
            Dim strMatDate() As String
            If strSecurityNature = "P" Then
                Hid_CoupDate.Value += CStr(#12/31/9999#) & "!"
                Hid_CoupRate.Value += dblPepCoupRate & "!"
            Else
                strMatDate = Split(Hid_MatDate.Value, "!")
            End If
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub FillYieldOptions()
        Try
            Dim datMaturity As Date
            Dim datCoupon As Date
            Dim datCall As Date
            Dim datPut As Date
            Dim decMaturityAmt As Decimal
            Dim decCouponRate As Decimal
            Dim decCallAmt As Decimal
            Dim decPutAmt As Decimal
            Dim dblResult As Double
            'Dim strSemiAnnFlag As String
            If decYTC <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYield <> 0.0 Then
                CCount = CCount + 1
            End If
            If decRate <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYTCSem <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYTMSemi <> 0.0 Then
                CCount = CCount + 1
            End If
            With objCommon
                If MatDate.Length <= 0 Then
                    datMaturity = Date.MinValue
                Else
                    datMaturity = MatDate(0)
                End If
                If CallDate.Length <= 0 Then
                    datCall = Date.MinValue
                Else
                    datCall = CallDate(0)
                End If
                If PutDate.Length <= 0 Then
                    datPut = Date.MinValue
                Else
                    datPut = PutDate(0)
                End If
                If CoupDate.Length <= 0 Then
                    datCoupon = Date.MinValue
                Else
                    datCoupon = CoupDate(0)
                End If
                If MatAmt.Length <= 0 Then
                    decMaturityAmt = 0
                Else
                    decMaturityAmt = MatAmt(0)
                End If
                If CallAmt.Length <= 0 Then
                    decCallAmt = 0
                Else
                    decCallAmt = CallAmt(0)
                End If
                If PutAmt.Length <= 0 Then
                    decPutAmt = 0
                Else
                    decPutAmt = PutAmt(0)
                End If
                If CoupRate.Length <= 0 Then
                    decCouponRate = 0
                Else
                    decCouponRate = CoupRate(0)
                End If
            End With

            If CCount > 1 Then
            ElseIf decRate = 0.0 Then
                If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                    Hid_YTMAnn.Value = decYield
                    strSemiAnnFlag = "A"
                Else
                    Hid_YTMSemi.Value = decYTMSemi
                    strSemiAnnFlag = "S"
                End If
                dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTMAnn.Value), Val(Hid_YTMSemi.Value))
                GlobalFuns.CalculateYield(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt, _
                               datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "M", dblResult, strSemiAnnFlag)

                Hid_Rate.Value = decMarketRate
                decYield = objCommon.DecimalFormat(decYield)
                dblYTMAnn = objCommon.DecimalFormat(decYield)
                dblYTMSemi = objCommon.DecimalFormat(decYTMSemi)
                dblYTCSemi = objCommon.DecimalFormat(dblYTCSemi)
            ElseIf PerpetualFlag = "P" Then
                If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                    Hid_YTCAnn.Value = decYTC
                    strSemiAnnFlag = "A"
                Else
                    Hid_YTCSemi.Value = decYTCSem
                    strSemiAnnFlag = "S"
                End If
                dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTCAnn.Value), Val(Hid_YTCSemi.Value))
                GlobalFuns.CalculateYield(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt, _
                                datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "C", dblResult, strSemiAnnFlag)
            Else
                dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTMAnn.Value), Val(Hid_YTMSemi.Value))
                GlobalFuns.CalculateYield(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt, _
                               datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")
            End If

            With objCommon
                Hid_YTMAnn.Value = .DecimalFormat(dblYTMAnn)
                Hid_YTCAnn.Value = .DecimalFormat(dblYTCAnn)
                Hid_YTPAnn.Value = .DecimalFormat(dblYTPAnn)
                If Val(Hid_Frequency.Value & "") > 1 Then
                    Hid_YTMSemi.Value = .DecimalFormat(dblYTMSemi)
                    Hid_YTCSemi.Value = .DecimalFormat(dblYTCSemi)
                    Hid_YTPSemi.Value = .DecimalFormat(dblYTPSemi)
                End If
            End With
        Catch ex As Exception
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillXIRROptionsOld_02102017()
        Try
            Dim dblMarketValue As Double
            Dim J As Int32
            Dim K As Int32 = 1
            Dim blnShow As Boolean = False
            Dim dblResult As Double

            If decYTC <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYield <> 0.0 Then
                CCount = CCount + 1
            End If
            If decRate <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYTCSem <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYTMSemi <> 0.0 Then
                CCount = CCount + 1
            End If
            If PerpetualFlag = "NP" Or PerpetualFlag = "N" Then
                If Val(Hid_Frequency.Value) = 0 Then
                    ReDim XirrDate(23)
                    ReDim XirrAmt(23)
                    dblMarketValue = Val(txt_rate) * Val(Hid_FaceValue.Value) / 100
                    XirrAmt(0) = -dblMarketValue
                    XirrDate(0) = objCommon.DateFormat(txt_ForDate.Text)
                    For J = 0 To UBound(MatAmt)
                        If MatDate(J) > XirrDate(0) Then
                            blnShow = True
                            Exit For
                        End If
                    Next

                    For J = J To UBound(MatAmt)
                        XirrAmt(K) = MatAmt(J)
                        XirrDate(K) = MatDate(J)
                        K = K + 1
                    Next

                    If blnShow = True Then
                        dblYTMAnn = objCommon.DecimalFormat(GetDDBResult())
                    Else
                        dblYTMAnn = "0.0"
                    End If
                    'If chk_CashFlow.Checked = True Then ShowCashFlow()

                    K = 1
                    blnShow = False
                    ' ****************************************************************************************************
                    ' Code for YTC(Call XIRR) calculation
                    For J = 0 To UBound(CallAmt)
                        If CallDate(J) > XirrDate(0) Then
                            blnShow = True
                            Exit For
                        End If
                    Next
                    For J = J To UBound(CallAmt)
                        XirrAmt(K) = CallAmt(J)
                        XirrDate(K) = CallDate(J)
                        K = K + 1
                    Next
                    If blnShow = True Then
                        ' txt_YTCAnn.Text = objCommon.DecimalFormat(GetDDBResult())
                        dblYTCAnn = objCommon.DecimalFormat(GetDDBResult())
                    Else
                        'txt_YTCAnn.Text = "0.0"
                        dblYTCAnn = "0.0"
                    End If

                    ' ****************************************************************************************************
                    K = 1
                    blnShow = False
                    ' ****************************************************************************************************
                    ' Code for YTP(Put XIRR) calculation
                    For J = 0 To UBound(PutAmt)
                        If PutDate(J) > XirrDate(0) Then
                            blnShow = True
                            Exit For
                        End If
                    Next
                    For J = J To UBound(PutAmt)
                        XirrAmt(K) = PutAmt(J)
                        XirrDate(K) = PutDate(J)
                        K = K + 1
                    Next
                    'If blnShow = True Then
                    '    txt_YTPAnn.Text = objCommon.DecimalFormat(GetDDBResult())
                    'Else
                    '    txt_YTPAnn.Text = "0.0"
                    'End If
                    ' ****************************************************************************************************

                Else
                    If CCount > 1 Then
                    ElseIf decYield <> 0.0 Or decYTMSemi <> 0.0 Then
                        If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                            decYield = objCommon.DecimalFormat(decYield)
                            strSemiAnnFlag = "A"
                        Else
                            'txt_YTMSemi.Text = objCommon.DecimalFormat(txt_YTMSemi.Text)
                            decYTMSemi = objCommon.DecimalFormat(decYTMSemi)
                            strSemiAnnFlag = "S"
                        End If
                        dblYTMAnn = objCommon.DecimalFormat(Val(decYield))
                        dblYTMSemi = objCommon.DecimalFormat(Val(decYTMSemi))
                        dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                               MatDate, MatAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, _
                                   Hid_Frequency.Value, strSemiAnnFlag, "M", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        Hid_Rate.Value = objCommon.DecimalFormat(dblResult)
                    ElseIf decYTC <> 0.0 Or decYTCSem <> 0.0 Then
                        If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                            strSemiAnnFlag = "A"
                            decYTC = objCommon.DecimalFormat(Val(decYTC))
                        Else
                            strSemiAnnFlag = "S"
                            decYTCSem = objCommon.DecimalFormat(Val(decYTCSem))
                            'txt_YTCSemi.Text = objCommon.DecimalFormat(Val(txt_YTCSemi.Text))
                        End If
                        dblYTCAnn = objCommon.DecimalFormat(Val(decYTC))
                        dblYTCSemi = objCommon.DecimalFormat(Val(decYTCSem))
                        For J = 0 To UBound(CallAmt)
                            If CallDate(J) > datYTM Then Exit For
                        Next
                        If J <> UBound(CallDate) + 1 Then
                            CallDate(0) = CallDate(J)
                            CallAmt(0) = CallAmt(J)
                            ReDim Preserve CallDate(0)
                            ReDim Preserve CallAmt(0)
                            'dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                            '            CallDate, CallAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, _
                            '            Hid_Frequency.Value, Trim(strSemiAnnFlag), "C", "E", CombineIPMat, intDays, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                                        CallDate, CallAmt, CoupDate, CoupRate, 31, datInterest, datIssue, _
                                        Hid_Frequency.Value, Trim(strSemiAnnFlag), "C", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            'txt_rate.Text = objCommon.DecimalFormat(dblResult)
                            Hid_Rate.Value = objCommon.DecimalFormat(dblResult)
                        End If
                    Else
                        If MatDate.Length > 0 Then
                            CntXirr = 0
                            CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, MatDate, MatAmt, _
                                          CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Val(Hid_Frequency.Value & ""), blnCompRate, "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            GetXIRRResult(Val(Hid_Frequency.Value), dblYTMAnn, dblYTMSemi)

                        End If
                        ' ****************************************************************************************************
                        ' Code for YTC(Call XIRR) calculation
                        For J = 0 To UBound(CallAmt)
                            If CallDate(J) > datYTM Then Exit For
                        Next
                        If J <> UBound(CallDate) + 1 Then
                            CallDate(0) = CallDate(J)
                            CallAmt(0) = CallAmt(J)
                            ReDim Preserve CallDate(0)
                            ReDim Preserve CallAmt(0)
                            CntXirr = 0
                            CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, CallDate, CallAmt, _
                                          CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, "A", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            GetXIRRResult(Val(Hid_Frequency.Value), dblYTCAnn, dblYTCSemi)
                            If MatDate.Length = 0 Then

                            End If
                        End If
                        ' ****************************************************************************************************

                        ' ****************************************************************************************************
                        ' Code for YTP(Put XIRR) calculation
                        For J = 0 To UBound(PutAmt)
                            If PutDate(J) > datYTM Then Exit For
                        Next
                        If J <> UBound(PutDate) + 1 Then
                            PutDate(0) = PutDate(J)
                            PutAmt(0) = PutAmt(J)
                            ReDim Preserve PutDate(0)
                            ReDim Preserve PutAmt(0)
                            CntXirr = 0
                            CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, PutDate, PutAmt, _
                                          CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, "A", CombineIPMat, 365, "E", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            GetXIRRResult(Val(Hid_Frequency.Value), dblYTPAnn, dblYTPSemi)
                        End If
                    End If
                End If
            Else
                If CCount > 1 Then

                ElseIf decYield <> 0.0 Or decYTMSemi <> 0.0 Then
                    If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                        decYield = objCommon.DecimalFormat(decYield)
                        strSemiAnnFlag = "A"
                    Else
                        'txt_YTMSemi.Text = objCommon.DecimalFormat(txt_YTMSemi.Text)
                        decYTMSemi = objCommon.DecimalFormat(decYTMSemi)
                        strSemiAnnFlag = "S"
                    End If
                    dblYTMAnn = objCommon.DecimalFormat(Val(decYield))
                    dblYTMSemi = objCommon.DecimalFormat(Val(decYTMSemi))
                    dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                           MatDate, MatAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, _
                               Hid_Frequency.Value, strSemiAnnFlag, "M", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                    Hid_Rate.Value = objCommon.DecimalFormat(dblResult)
                ElseIf decYTC <> 0.0 Or decYTCSem <> 0.0 Then
                    If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                        decYTC = objCommon.DecimalFormat(Val(decYTC))
                        strSemiAnnFlag = "A"
                    Else
                        decYTCSem = objCommon.DecimalFormat(Val(decYTCSem))
                        strSemiAnnFlag = "S"
                        'txt_YTCSemi.Text = objCommon.DecimalFormat(Val(txt_YTCSemi.Text))
                    End If
                    dblYTCAnn = objCommon.DecimalFormat(Val(decYTC))
                    dblYTCSemi = objCommon.DecimalFormat(Val(decYTCSem))
                    For J = 0 To UBound(CallAmt)
                        If CallDate(J) > datYTM Then Exit For
                    Next
                    If J <> UBound(CallDate) + 1 Then
                        CallDate(0) = CallDate(J)
                        CallAmt(0) = CallAmt(J)
                        ReDim Preserve CallDate(0)
                        ReDim Preserve CallAmt(0)
                        'dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                        '            CallDate, CallAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, _
                        '            Hid_Frequency.Value, Trim(strSemiAnnFlag), "C", "E", CombineIPMat, intDays, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                                    CallDate, CallAmt, CoupDate, CoupRate, 31, datInterest, datIssue, _
                                    Hid_Frequency.Value, Trim(strSemiAnnFlag), "C", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        'txt_rate.Text = objCommon.DecimalFormat(dblResult)
                        Hid_Rate.Value = objCommon.DecimalFormat(dblResult)
                    End If
                Else
                    If MatDate.Length > 0 Then
                        CntXirr = 0
                        CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, MatDate, MatAmt, _
                                      CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Val(Hid_Frequency.Value & ""), blnCompRate, "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        GetXIRRResult(Val(Hid_Frequency.Value), dblYTMAnn, dblYTMSemi)

                    End If
                    ' ****************************************************************************************************
                    ' Code for YTC(Call XIRR) calculation
                    For J = 0 To UBound(CallAmt)
                        If CallDate(J) > datYTM Then Exit For
                    Next
                    If J <> UBound(CallDate) + 1 Then
                        CallDate(0) = CallDate(J)
                        CallAmt(0) = CallAmt(J)
                        ReDim Preserve CallDate(0)
                        ReDim Preserve CallAmt(0)
                        CntXirr = 0
                        CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, CallDate, CallAmt, _
                                      CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, "A", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        GetXIRRResult(Val(Hid_Frequency.Value), dblYTCAnn, dblYTCSemi)
                        If MatDate.Length = 0 Then

                        End If
                    End If
                    ' ****************************************************************************************************

                    ' ****************************************************************************************************
                    ' Code for YTP(Put XIRR) calculation
                    For J = 0 To UBound(PutAmt)
                        If PutDate(J) > datYTM Then Exit For
                    Next
                    If J <> UBound(PutDate) + 1 Then
                        PutDate(0) = PutDate(J)
                        PutAmt(0) = PutAmt(J)
                        ReDim Preserve PutDate(0)
                        ReDim Preserve PutAmt(0)
                        CntXirr = 0
                        CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, PutDate, PutAmt, _
                                      CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, "A", CombineIPMat, 365, "E", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        GetXIRRResult(Val(Hid_Frequency.Value), dblYTPAnn, dblYTPSemi)
                    End If
                End If
            End If

            With objCommon
                If decYield <> 0.0 And decRate <> 0.0 Then
                Else
                    Hid_YTMAnn.Value = .DecimalFormat(dblYTMAnn)
                    Hid_YTCAnn.Value = .DecimalFormat(dblYTCAnn)
                    Hid_YTPAnn.Value = .DecimalFormat(dblYTPAnn)
                    If Val(Hid_Frequency.Value & "") > 1 Then
                        Hid_YTMSemi.Value = .DecimalFormat(dblYTMSemi)
                        Hid_YTCSemi.Value = .DecimalFormat(dblYTCSemi)
                        Hid_YTPSemi.Value = .DecimalFormat(dblYTPSemi)
                    End If
                End If
            End With
            'CCount = 0
        Catch ex As Exception
            '   Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub dg_dme_OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btn As ImageButton = DirectCast(e.Row.FindControl("imgBtn_Delete"), ImageButton)
            Dim cchk_AllItems As CheckBox = DirectCast(e.Row.FindControl("chk_AllItems"), CheckBox)
            Dim cchk_ItemChecked As CheckBox = DirectCast(e.Row.FindControl("chk_ItemChecked"), CheckBox)
            Dim lblPerpetualFlag As Label = DirectCast(e.Row.FindControl("lbl_PerpetualFlag"), Label)
            Dim lbl_CallDate As Label = DirectCast(e.Row.FindControl("lbl_CallDate"), Label)
            Dim txtYTMAnn As TextBox = DirectCast(e.Row.FindControl("txt_Yield"), TextBox)
            Dim txtYTCAnn As TextBox = DirectCast(e.Row.FindControl("txt_YTC"), TextBox)
            Dim txtYTMSemi As TextBox = DirectCast(e.Row.FindControl("txt_YTMSemi"), TextBox)
            Dim txtYTCSemi As TextBox = DirectCast(e.Row.FindControl("txt_YTCSemi"), TextBox)
            Dim lblSemiAnnFlag As Label = DirectCast(e.Row.FindControl("lbl_Semi_Ann_Flag"), Label)
            If Val(Session("UserTypeId") & "") <> 1 Then
                cchk_ItemChecked.Enabled = False
                btn.Visible = False
            End If
            If lbl_CallDate.Text <> "" Then
                txtYTCAnn.Enabled = True
                '    txtYTCSemi.Enabled = True
            Else
                txtYTCAnn.Style.Add("display", "none")
                '    txtYTCSemi.Style.Add("display", "none")
            End If
            If lblSemiAnnFlag.Text = "Y" Or lblSemiAnnFlag.Text = "N" Or lblSemiAnnFlag.Text = "A" Then
                'txtYTCSemi.Style.Add("display", "none")
                'txtYTMSemi.Style.Add("display", "none")
            Else
                txtYTCAnn.Style.Add("display", "none")
                txtYTMAnn.Style.Add("display", "none")
            End If
            If lblPerpetualFlag.Text = "P" Then
                txtYTMAnn.Style.Add("display", "none")
                'txtYTMSemi.Style.Add("display", "none")
            End If
        End If
    End Sub
    Private Sub FillXIRROptionsOld()
        Try
            Dim dblMarketValue As Double
            Dim J As Int32
            Dim K As Int32 = 1
            Dim blnShow As Boolean = False
            Dim dblResult As Double

            If decYTC <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYield <> 0.0 Then
                CCount = CCount + 1
            End If
            If decRate <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYTCSem <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYTMSemi <> 0.0 Then
                CCount = CCount + 1
            End If
            ' If rdo_Yield.Checked = True Then

            If PerpetualFlag = "NP" Or PerpetualFlag = "N" Then
                If Val(Hid_Frequency.Value) = 0 Then
                    ReDim XirrDate(23)
                    ReDim XirrAmt(23)

                    'dblMarketValue = IIf(rdo_RateActual.SelectedValue = "R", Val(txt_rate) * Val(Hid_FaceValue.Value) / 100, Val(txt_rate))
                    dblMarketValue = Val(txt_rate) * Val(Hid_FaceValue.Value) / 100
                    'dblMarketValue = 23750
                    'dblMarketValue = txt_rate

                    XirrAmt(0) = -dblMarketValue
                    XirrDate(0) = objCommon.DateFormat(txt_ForDate.Text)
                    For J = 0 To UBound(MatAmt)
                        If MatDate(J) > XirrDate(0) Then
                            blnShow = True
                            Exit For
                        End If
                    Next

                    For J = J To UBound(MatAmt)
                        XirrAmt(K) = MatAmt(J)
                        XirrDate(K) = MatDate(J)
                        K = K + 1
                    Next

                    If blnShow = True Then
                        '  txt_YTMAnn.Text = objCommon.DecimalFormat(GetDDBResult())
                        dblYTMAnn = objCommon.DecimalFormat(GetDDBResult())
                    Else
                        ' txt_YTMAnn.Text = "0.0"
                        dblYTMAnn = "0.0"
                    End If
                    'If chk_CashFlow.Checked = True Then ShowCashFlow()

                    K = 1
                    blnShow = False
                    ' ****************************************************************************************************
                    ' Code for YTC(Call XIRR) calculation
                    For J = 0 To UBound(CallAmt)
                        If CallDate(J) > XirrDate(0) Then
                            blnShow = True
                            Exit For
                        End If
                    Next
                    For J = J To UBound(CallAmt)
                        XirrAmt(K) = CallAmt(J)
                        XirrDate(K) = CallDate(J)
                        K = K + 1
                    Next
                    If blnShow = True Then
                        ' txt_YTCAnn.Text = objCommon.DecimalFormat(GetDDBResult())
                        dblYTCAnn = objCommon.DecimalFormat(GetDDBResult())
                    Else
                        'txt_YTCAnn.Text = "0.0"
                        dblYTCAnn = "0.0"
                    End If

                    ' ****************************************************************************************************
                    K = 1
                    blnShow = False
                    ' ****************************************************************************************************
                    ' Code for YTP(Put XIRR) calculation
                    For J = 0 To UBound(PutAmt)
                        If PutDate(J) > XirrDate(0) Then
                            blnShow = True
                            Exit For
                        End If
                    Next
                    For J = J To UBound(PutAmt)
                        XirrAmt(K) = PutAmt(J)
                        XirrDate(K) = PutDate(J)
                        K = K + 1
                    Next
                    'If blnShow = True Then
                    '    txt_YTPAnn.Text = objCommon.DecimalFormat(GetDDBResult())
                    'Else
                    '    txt_YTPAnn.Text = "0.0"
                    'End If
                    ' ****************************************************************************************************

                Else
                    If CCount > 1 Then
                    ElseIf decYield <> 0.0 Or decYTMSemi <> 0.0 Then
                        If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                            decYield = objCommon.DecimalFormat(decYield)
                            strSemiAnnFlag = "A"
                        Else
                            'txt_YTMSemi.Text = objCommon.DecimalFormat(txt_YTMSemi.Text)
                            decYTMSemi = objCommon.DecimalFormat(decYTMSemi)
                            strSemiAnnFlag = "S"
                        End If
                        dblYTMAnn = objCommon.DecimalFormat(Val(decYield))
                        dblYTMSemi = objCommon.DecimalFormat(Val(decYTMSemi))
                        dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                               MatDate, MatAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, _
                                   Hid_Frequency.Value, strSemiAnnFlag, "M", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        Hid_Rate.Value = objCommon.DecimalFormat(dblResult)
                    ElseIf decYTC <> 0.0 Or decYTCSem <> 0.0 Then
                        If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                            strSemiAnnFlag = "A"
                            decYTC = objCommon.DecimalFormat(Val(decYTC))
                        Else
                            strSemiAnnFlag = "S"
                            decYTCSem = objCommon.DecimalFormat(Val(decYTCSem))
                            'txt_YTCSemi.Text = objCommon.DecimalFormat(Val(txt_YTCSemi.Text))
                        End If
                        dblYTCAnn = objCommon.DecimalFormat(Val(decYTC))
                        dblYTCSemi = objCommon.DecimalFormat(Val(decYTCSem))
                        For J = 0 To UBound(CallAmt)
                            If CallDate(J) > datYTM Then Exit For
                        Next
                        If J <> UBound(CallDate) + 1 Then
                            CallDate(0) = CallDate(J)
                            CallAmt(0) = CallAmt(J)
                            ReDim Preserve CallDate(0)
                            ReDim Preserve CallAmt(0)
                            'dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                            '            CallDate, CallAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, _
                            '            Hid_Frequency.Value, Trim(strSemiAnnFlag), "C", "E", CombineIPMat, intDays, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                                        CallDate, CallAmt, CoupDate, CoupRate, 31, datInterest, datIssue, _
                                        Hid_Frequency.Value, Trim(strSemiAnnFlag), "C", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            'txt_rate.Text = objCommon.DecimalFormat(dblResult)
                            Hid_Rate.Value = objCommon.DecimalFormat(dblResult)
                        End If
                    Else
                        If MatDate.Length > 0 Then
                            CntXirr = 0
                            CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, MatDate, MatAmt, _
                                          CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Val(Hid_Frequency.Value & ""), blnCompRate, "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            GetXIRRResult(Val(Hid_Frequency.Value), dblYTMAnn, dblYTMSemi)

                        End If
                        ' ****************************************************************************************************
                        ' Code for YTC(Call XIRR) calculation
                        For J = 0 To UBound(CallAmt)
                            If CallDate(J) > datYTM Then Exit For
                        Next
                        If J <> UBound(CallDate) + 1 Then
                            CallDate(0) = CallDate(J)
                            CallAmt(0) = CallAmt(J)
                            ReDim Preserve CallDate(0)
                            ReDim Preserve CallAmt(0)
                            CntXirr = 0
                            CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, CallDate, CallAmt, _
                                          CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, "A", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            GetXIRRResult(Val(Hid_Frequency.Value), dblYTCAnn, dblYTCSemi)
                            If MatDate.Length = 0 Then

                            End If
                        End If
                        ' ****************************************************************************************************

                        ' ****************************************************************************************************
                        ' Code for YTP(Put XIRR) calculation
                        For J = 0 To UBound(PutAmt)
                            If PutDate(J) > datYTM Then Exit For
                        Next
                        If J <> UBound(PutDate) + 1 Then
                            PutDate(0) = PutDate(J)
                            PutAmt(0) = PutAmt(J)
                            ReDim Preserve PutDate(0)
                            ReDim Preserve PutAmt(0)
                            CntXirr = 0
                            CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, PutDate, PutAmt, _
                                          CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, "A", CombineIPMat, 365, "E", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            GetXIRRResult(Val(Hid_Frequency.Value), dblYTPAnn, dblYTPSemi)
                        End If
                    End If
                End If
            Else
                If CCount > 1 Then

                ElseIf decYield <> 0.0 Or decYTMSemi <> 0.0 Then
                    If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                        decYield = objCommon.DecimalFormat(decYield)
                        strSemiAnnFlag = "A"
                    Else
                        'txt_YTMSemi.Text = objCommon.DecimalFormat(txt_YTMSemi.Text)
                        decYTMSemi = objCommon.DecimalFormat(decYTMSemi)
                        strSemiAnnFlag = "S"
                    End If
                    dblYTMAnn = objCommon.DecimalFormat(Val(decYield))
                    dblYTMSemi = objCommon.DecimalFormat(Val(decYTMSemi))
                    dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                           MatDate, MatAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, _
                               Hid_Frequency.Value, strSemiAnnFlag, "M", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                    Hid_Rate.Value = objCommon.DecimalFormat(dblResult)
                ElseIf decYTC <> 0.0 Or decYTCSem <> 0.0 Then
                    If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                        decYTC = objCommon.DecimalFormat(Val(decYTC))
                        strSemiAnnFlag = "A"
                    Else
                        decYTCSem = objCommon.DecimalFormat(Val(decYTCSem))
                        strSemiAnnFlag = "S"
                        'txt_YTCSemi.Text = objCommon.DecimalFormat(Val(txt_YTCSemi.Text))
                    End If
                    dblYTCAnn = objCommon.DecimalFormat(Val(decYTC))
                    dblYTCSemi = objCommon.DecimalFormat(Val(decYTCSem))
                    For J = 0 To UBound(CallAmt)
                        If CallDate(J) > datYTM Then Exit For
                    Next
                    If J <> UBound(CallDate) + 1 Then
                        CallDate(0) = CallDate(J)
                        CallAmt(0) = CallAmt(J)
                        ReDim Preserve CallDate(0)
                        ReDim Preserve CallAmt(0)
                        'dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                        '            CallDate, CallAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, _
                        '            Hid_Frequency.Value, Trim(strSemiAnnFlag), "C", "E", CombineIPMat, intDays, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                                    CallDate, CallAmt, CoupDate, CoupRate, 31, datInterest, datIssue, _
                                    Hid_Frequency.Value, Trim(strSemiAnnFlag), "C", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        'txt_rate.Text = objCommon.DecimalFormat(dblResult)
                        Hid_Rate.Value = objCommon.DecimalFormat(dblResult)
                    End If
                Else
                    If MatDate.Length > 0 Then
                        CntXirr = 0
                        CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, MatDate, MatAmt, _
                                      CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Val(Hid_Frequency.Value & ""), blnCompRate, "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        GetXIRRResult(Val(Hid_Frequency.Value), dblYTMAnn, dblYTMSemi)

                    End If
                    ' ****************************************************************************************************
                    ' Code for YTC(Call XIRR) calculation
                    For J = 0 To UBound(CallAmt)
                        If CallDate(J) > datYTM Then Exit For
                    Next
                    If J <> UBound(CallDate) + 1 Then
                        CallDate(0) = CallDate(J)
                        CallAmt(0) = CallAmt(J)
                        ReDim Preserve CallDate(0)
                        ReDim Preserve CallAmt(0)
                        CntXirr = 0
                        CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, CallDate, CallAmt, _
                                      CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, "A", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        GetXIRRResult(Val(Hid_Frequency.Value), dblYTCAnn, dblYTCSemi)
                        If MatDate.Length = 0 Then

                        End If
                    End If
                    ' ****************************************************************************************************

                    ' ****************************************************************************************************
                    ' Code for YTP(Put XIRR) calculation
                    For J = 0 To UBound(PutAmt)
                        If PutDate(J) > datYTM Then Exit For
                    Next
                    If J <> UBound(PutDate) + 1 Then
                        PutDate(0) = PutDate(J)
                        PutAmt(0) = PutAmt(J)
                        ReDim Preserve PutDate(0)
                        ReDim Preserve PutAmt(0)
                        CntXirr = 0
                        CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, PutDate, PutAmt, _
                                      CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, "A", CombineIPMat, 365, "E", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        GetXIRRResult(Val(Hid_Frequency.Value), dblYTPAnn, dblYTPSemi)
                    End If
                End If
            End If

            With objCommon
                If decYield <> 0.0 And decRate <> 0.0 Then
                Else
                    Hid_YTMAnn.Value = .DecimalFormat(dblYTMAnn)
                    Hid_YTCAnn.Value = .DecimalFormat(dblYTCAnn)
                    Hid_YTPAnn.Value = .DecimalFormat(dblYTPAnn)
                    If Val(Hid_Frequency.Value & "") > 1 Then
                        Hid_YTMSemi.Value = .DecimalFormat(dblYTMSemi)
                        Hid_YTCSemi.Value = .DecimalFormat(dblYTCSemi)
                        Hid_YTPSemi.Value = .DecimalFormat(dblYTPSemi)
                    End If
                End If
            End With
            'CCount = 0
        Catch ex As Exception
            '   Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillXIRROptions()
        Try
            If decYTC <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYield <> 0.0 Then
                CCount = CCount + 1
            End If
            If decRate <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYTCSem <> 0.0 Then
                CCount = CCount + 1
            End If
            If decYTMSemi <> 0.0 Then
                CCount = CCount + 1
            End If

            If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                strSemiAnnFlag = "A"
            Else
                strSemiAnnFlag = "S"
            End If

            If CCount > 1 Then
            ElseIf decYield <> 0.0 Or decYTMSemi <> 0.0 Then
                dblYTMAnn = Val(decYield)
                dblYTMSemi = Val(decYTMSemi)

                CalculateXIRRPrice(SecurityId, txt_ForDate.Text, Val(Hid_Frequency.Value), False, strSemiAnnFlag, "R", "M")
                Hid_Rate.Value = dblPTM
            ElseIf decYTC <> 0.0 Or decYTCSem <> 0.0 Then
                dblYTCAnn = Val(decYTC)
                dblYTCSemi = Val(decYTCSem)

                CalculateXIRRPrice(SecurityId, txt_ForDate.Text, Val(Hid_Frequency.Value), False, strSemiAnnFlag, "R", "C")
                Hid_Rate.Value = dblPTC
            Else
                CalculateXIRRYield(SecurityId, txt_ForDate.Text, Val(decRate), Val(Hid_Frequency.Value), False, "R")
            End If

            With objCommon
                If decYield <> 0.0 And decRate <> 0.0 Then
                Else
                    Hid_YTMAnn.Value = .DecimalFormat(dblYTMAnn)
                    Hid_YTCAnn.Value = .DecimalFormat(dblYTCAnn)
                    Hid_YTPAnn.Value = .DecimalFormat(dblYTPAnn)
                    If Val(Hid_Frequency.Value & "") > 1 Then
                        Hid_YTMSemi.Value = .DecimalFormat(dblYTMSemi)
                        Hid_YTCSemi.Value = .DecimalFormat(dblYTCSemi)
                        Hid_YTPSemi.Value = .DecimalFormat(dblYTPSemi)
                    End If
                End If
            End With
        Catch ex As Exception
            '   Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub



    Private Sub FillXIRROptions_New()
        Try
            Dim dblMarketValue As Double
            Dim J As Int32
            Dim K As Int32 = 1
            Dim blnShow As Boolean = False
            Dim dblResult As Double

            If decYTC <> 0.0 Then
                CCount = CCount + 1
            End If

            If decYield <> 0.0 Then
                CCount = CCount + 1
            End If

            If decRate <> 0.0 Then
                CCount = CCount + 1
            End If

            If CCount > 1 Then
            ElseIf decYield <> 0.0 Then
                dblYTMAnn = Val(decYield)
                CalculateXIRRPrice(SecurityId, txt_ForDate.Text, Val(Hid_Frequency.Value), False, "R", "M")
                Hid_Rate.Value = dblPTM
            ElseIf decYTC = 0.0 Then
                CalculateXIRRYield(SecurityId, txt_ForDate.Text, Val(txt_rate), Val(Hid_Frequency.Value), False, "R")
            Else
                dblYTCAnn = Val(decYTC)
                CalculateXIRRPrice(SecurityId, txt_ForDate.Text, Val(Hid_Frequency.Value), False, "R", "C")
                Hid_Rate.Value = dblPTC
            End If

            With objCommon
                If decYield <> 0.0 And decRate <> 0.0 Then
                Else
                    Hid_YTMAnn.Value = .DecimalFormat(dblYTMAnn)
                    Hid_YTCAnn.Value = .DecimalFormat(dblYTCAnn)
                    Hid_YTPAnn.Value = .DecimalFormat(dblYTPAnn)
                    If Val(Hid_Frequency.Value & "") > 1 Then
                        Hid_YTMSemi.Value = .DecimalFormat(dblYTMSemi)
                        Hid_YTCSemi.Value = .DecimalFormat(dblYTCSemi)
                        Hid_YTPSemi.Value = .DecimalFormat(dblYTPSemi)
                    End If
                End If
            End With
            'CCount = 0
        Catch ex As Exception
            '   Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

End Class
