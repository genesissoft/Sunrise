
Imports DocumentFormat.OpenXml
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports iTextSharp.text
'Imports iTextSharp.text.pdf
Imports iTextSharp.text.Element
Imports System.Net
Imports System.Net.Mail
Imports ClosedXML.Excel
Imports System.Drawing
Imports myApp.ns.pages
Imports log4net
Imports System.Xml
Imports System.Xml.Serialization

Imports System.Collections.Generic
'Imports System.Linq
Imports System.Text
Imports System.Object
'Imports DocumentFormat.OpenXml.Packaging.OpenXmlPartContainer
'Imports DocumentFormat.OpenXml.Packaging.OpenXmlPackage
'Imports DocumentFormat.OpenXml.Packaging.SpreadsheetPrinterSettingsPart


Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports DocumentFormat.OpenXml.Presentation
'Imports DocumentFormat.OpenXml.PresentationDocumentType
'Imports System.Windows.Forms
Imports iTextSharp.text.pdf
Imports System.FormatException
Imports DocumentFormat.OpenXml.Drawing.Spreadsheet
Imports DocumentFormat.OpenXml.Math
Imports Ionic.Zip

Partial Class Forms_FaxSelection
    Inherits System.Web.UI.Page
    Dim strFileName As String = "\Quote_Fax.doc"
    Dim sqlComm As New SqlCommand
    Dim objCommon As New clsCommonFuns
    Dim objcomm As New Common
    Dim dsmenu As DataSet
    Dim PgName As String = "$FaxSelection$"
    Dim dsDPDetails As DataSet
    Dim newTable As DataTable
    Dim DpDetailsTable As DataTable
    Dim trmenu As DataRow
    'Dim wrdApp As Word.Application
    'Dim oBook As Excel.Workbook
    'Dim oSheet As Excel.Worksheet
    Dim blnHeaderExist As Boolean = False
    Dim blnFooterExist As Boolean = False
    Dim strHeaderFile As String
    Dim strFooterFile As String
    Dim imgNew() As Byte
    Dim intTotWidth As Int32
    Dim intRowIndex As Int16
    Dim strFilePath As String = ConfigurationSettings.AppSettings("FaxFile").ToString()

    Dim intFaxQuoteId As Integer
    Dim intRateIndex As Int16
    Dim dt1 As DataTable
    Dim dt2 As DataTable
    Dim dt3 As DataTable
    Dim verdana As BaseFont
    Dim verdana1 As BaseFont
    Dim arial As BaseFont
    Dim arial1 As BaseFont
    Dim font As iTextSharp.text.Font
    Dim font2 As iTextSharp.text.Font
    Dim fontF As FontFactory
    Dim fontIn As iTextSharp.text.Font
    Dim fontAddr As iTextSharp.text.Font
    Dim fontTabPdf As iTextSharp.text.Font
    Dim sqlConn As SqlConnection
    Dim objUtil As New Util
    Dim OfferUser As String = (ConfigurationManager.AppSettings("OfferUser") & "")

    'Yield Calc Fields declaration
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
    Dim blnNonGovernment As Boolean
    Dim blnRateActual As Boolean
    Dim blnDMAT As Boolean
    Dim intBKDiff As Integer
    Dim dblPepCoupRate As Double
    Dim blnCompRate As Boolean
    Dim blnCloseButton As Boolean
    Dim BrokenInt As Boolean
    Dim InterestOnHoliday As Boolean
    Dim InterestOnSat As Boolean
    Dim MaturityOnHoliday As Boolean
    Dim MaturityOnSat As Boolean
    Dim SecurityId As Int64
    Dim SecurityName As String
    Dim ISINNo As String
    Dim SecurityTypeName As String
    Dim Quantity As String
    Dim RatingRemark As String
    Dim strSemiAnnFlag As String
    Dim RateActualFlag As String
    Dim PhysicalDematFlag As String
    Dim CombineIPMat As Boolean
    Dim EqualActualFlag As String
    Dim intDays As Int16
    Dim FirstAllYr As Char
    Dim ytmdt As Date
    Dim intSecurityTypeId As Integer
    Dim PerpetualFlag As String
    Public decYield As Decimal
    Public decYTC As Decimal
    Public decYTM As Decimal
    Public decYTCSem As Decimal
    Public decYTMSemi As Decimal
    Dim CCount As Integer = 0
    Dim lblCallDate As String
    Dim lblCategory As String
    Dim TaxFree As String
    Dim strUserName As String
    Dim strUserMobile As String
    Dim strUseremail As String
    Dim lblSubCategory As String
    Dim lblSecuredUnsec As String
    Dim lblNameOFPD As String
    Dim lblCallFlag As String
    Dim strUserBranchAddressExcel As String
    Dim strUserBranchAddressPDF As String
    Dim strBranchName As String
    Dim CalcDate As String
    Dim lblId As String
    Dim lblYieldPriceType As String
    Dim decbasispoint As Decimal = 0
    Dim OrderId As Integer = 0
    Dim Remark As String = ""


    Public Class SecIds
        Public Id As Decimal

        Public Sub SecIds()
        End Sub
        Public Sub SecIds(ByVal Id As Decimal)
            Id = Id
        End Sub
    End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If (Session("username") = "") Then
                Response.Redirect("Login.aspx")
                Exit Sub
            End If
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            If IsPostBack = False Then
                Dim dtDate As Date

                Dim strdate() As String = DateTime.Now.ToString("dd/MM/yyyy").Split(" ")
                'txt_Date.Text = String.Format(DateAndTime.Today, "dd/MM/yyyy")
                dtDate = objCommon.DateFormat(strdate(0))
                txt_Date.Text = dtDate.ToString("dd/MM/yyyy")

                'txt_CalcDate.Text = dtDate.AddDays(1).ToString("dd/MM/yyyy")
                SetAttributes()
                FillBlankSecurityGrids()
                FillBlankCustomerGrids()
                FillBlankBrokerGrids()
                FillSettDate()
                'CalcDate = ytmdt
                'txt_CalcDate.Text = ytmdt.ToString("dd/MM/yyyy")
                'FillOfferRemarkGrid()
                Session("FaxQuoteTable") = ""
                GetQuoteNames()
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("Id") & "")
                End If
                btn_UpdateStock.Visible = False
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "func", "getCustBrokerOption();", True)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    'Private Sub FillOfferRemarkGrid()
    '    Try
    '        OpenConn()
    '        Dim dt As New DataTable
    '        dt = objCommon.FillDataTable(sqlConn, "ID_FILL_OfferLetterRemark")
    '        If dt.Rows.Count > 0 Then
    '            Session("TempOfferRemarkGrid") = dt
    '            dg_OfferRemark.DataSource = dt
    '            dg_OfferRemark.DataBind()
    '        Else
    '            FillBlankOfferRemarkGrid()
    '        End If

    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    Finally
    '        CloseConn()
    '    End Try
    'End Sub

    Private Sub FillBlankOfferRemarkGrid()
        Try
            Dim dt As New DataTable
            Dim objSrh As New clsSearch
            dt.Columns.Add("RemarkId", GetType(Int32))
            dt.Columns.Add("RemarkHeading", GetType(String))


            Session("TempOfferRemarkGrid") = dt
            objSrh.SetGrid(dg_OfferRemark, dt)
            dg_OfferRemark.DataSource = dt
            dg_OfferRemark.DataBind()

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
    Private Sub FillBlankSecurityGrids()
        Try
            Dim dt As New DataTable
            Dim objSrh As New clsSearch
            dt.Columns.Add("FaxQuoteDetailId", GetType(Int32))
            dt.Columns.Add("SecurityId", GetType(String))
            dt.Columns.Add("TypeFlag", GetType(Char))
            dt.Columns.Add("SecurityName", GetType(String))
            dt.Columns.Add("ISINNo", GetType(String))
            dt.Columns.Add("CallDate", GetType(String))
            dt.Columns.Add("SellingRate", GetType(String))
            dt.Columns.Add("ShowNumber", GetType(String))
            dt.Columns.Add("Rating", GetType(String))
            dt.Columns.Add("Days", GetType(String))
            dt.Columns.Add("DaysOptions", GetType(String))
            dt.Columns.Add("PhysicalDMAT", GetType(String))
            dt.Columns.Add("IPCalc", GetType(String))
            dt.Columns.Add("RateActual", GetType(String))
            dt.Columns.Add("YXM", GetType(String))
            dt.Columns.Add("Yield", GetType(String))
            dt.Columns.Add("YTCAnn", GetType(String))
            dt.Columns.Add("YTPAnn", GetType(String))
            dt.Columns.Add("YTMSemi", GetType(String))
            dt.Columns.Add("YTCSemi", GetType(String))
            dt.Columns.Add("YTPSemi", GetType(String))
            dt.Columns.Add("SecurityTypeId", GetType(String))
            dt.Columns.Add("SecurityTypeName", GetType(String))
            dt.Columns.Add("SPriority", GetType(String))
            dt.Columns.Add("OrderId", GetType(String))
            dt.Columns.Add("CreditRating", GetType(String))
            dt.Columns.Add("Semi_Ann_Flag", GetType(String))
            dt.Columns.Add("CombineIPMat", GetType(String))
            dt.Columns.Add("Rate_Actual_Flag", GetType(String))
            dt.Columns.Add("Equal_Actual_Flag", GetType(String))
            dt.Columns.Add("IntDays", GetType(String))
            dt.Columns.Add("FirstYrAllYr", GetType(String))
            dt.Columns.Add("Margin")
            'dt.Columns.Add("IsMaster")
            'dt.Columns.Add("FaceValue")
            'dt.Columns.Add("TaxFree", GetType(String))
            dt.Columns.Add("OriginalSellingRate", GetType(String))
            dt.Columns.Add("NatureOFInstrument", GetType(String))
            dt.Columns.Add("Category", GetType(String))
            dt.Columns.Add("SubCategory", GetType(String))
            dt.Columns.Add("SecuredUnsec", GetType(String))
            dt.Columns.Add("Name of PD", GetType(String))
            dt.Columns.Add("CallFlag", GetType(String))
            dt.Columns.Add("Id", GetType(String))
            dt.Columns.Add("YieldPriceType", GetType(String))
            dt.Columns.Add("Remark", GetType(String))

            Session("SecurityMaster") = dt
            Session("TempSecurityTable") = dt
            objSrh.SetGrid(dg_Selected, dt)
            dg_Selected.DataSource = dt
            dg_Selected.DataBind()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillBlankCustomerGrids()
        Try
            Dim dtCustomerGrid As New DataTable
            Dim objSrh As New clsSearch
            dtCustomerGrid.Columns.Add("CustomerName", GetType(String))
            dtCustomerGrid.Columns.Add("ContactPerson", GetType(String))
            dtCustomerGrid.Columns.Add("CustomerId", GetType(String))
            dtCustomerGrid.Columns.Add("FieldId", GetType(String))
            dtCustomerGrid.Columns.Add("EmailId", GetType(String)) 'EmailId

            Session("CustomerContectTable") = dtCustomerGrid
            objSrh.SetGrid(dg_Customer, dtCustomerGrid)

            dg_Customer.DataSource = dtCustomerGrid
            dg_Customer.DataBind()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillBlankBrokerGrids()
        Try
            Dim dtBrokerGrid As New DataTable
            Dim objSrh As New clsSearch
            dtBrokerGrid.Columns.Add("BrokerName", GetType(String))
            dtBrokerGrid.Columns.Add("BrokerCode", GetType(String))
            dtBrokerGrid.Columns.Add("BrokerId", GetType(String))
            dtBrokerGrid.Columns.Add("FieldId", GetType(String))
            dtBrokerGrid.Columns.Add("EmailId", GetType(String))
            dtBrokerGrid.Columns.Add("BasisPoint", GetType(Decimal))

            Session("BrokerTable") = dtBrokerGrid
            objSrh.SetGrid(dg_Broker, dtBrokerGrid)

            dg_Broker.DataSource = dtBrokerGrid
            dg_Broker.DataBind()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Customer_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dg_Customer.RowCommand
        Try
            Dim imgBtn As ImageButton
            Dim btn As Button
            Dim gvRow As GridViewRow
            Dim dr As DataRow
            Dim dt As DataTable

            dt = Session("CustomerContectTable")

            If e.CommandName = "AddFormat" Then
                btn = TryCast(e.CommandSource, Button)
                gvRow = btn.Parent.Parent
                dr = dt.Rows(gvRow.DataItemIndex)
                dr("FieldId") = Replace(Hid_SelectedFields.Value, "!", ",")
                Session("CustomerContectTable") = dt
            ElseIf e.CommandName = "DeleteRow" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                gvRow = imgBtn.Parent.Parent
                DeleteGridRows("CustomerContectTable", gvRow.DataItemIndex, dg_Customer, 0)
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Private Function RemoveEmptyRowsFromDataTable(ByVal dt As DataTable) As DataTable
        Dim i As Integer
        For i = dt.Rows.Count - 1 To 0 Step i - 1
            If Convert.ToString(dt.Rows(i)("SecurityId")) = "" Then
                dt.Rows(i).Delete()
            End If
        Next
        dt.AcceptChanges()
        Return dt
    End Function

    Protected Sub dg_Selected_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg_Selected.PageIndexChanged

    End Sub


    Protected Sub dg_Selected_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dg_Selected.RowCommand
        Try
            Dim imgBtn As ImageButton
            Dim gvRow As GridViewRow
            Dim dr As DataRow
            Dim dt As DataTable
            Dim Id As Integer

            dt = Session("TempSecurityTable")
            'dt = RemoveEmptyRowsFromDataTable(dt)
            'dt.DefaultView.Sort = "SecurityTypeId Asc"
            'dt = dt.DefaultView.ToTable()

            If e.CommandName = "EditRow" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                gvRow = imgBtn.Parent.Parent
                ViewState("EditIndex") = gvRow.DataItemIndex
                ViewState("ContactEditFlag") = True
                dr = dt.Rows(gvRow.DataItemIndex)
                Hid_SecurityId.Value = dr.Item("SecurityId")
                Hid_FaxQuoteId.Value = dr.Item("FaxQuoteId")
                FillSecurityGrid()
            ElseIf e.CommandName = "DeleteRow" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                gvRow = imgBtn.Parent.Parent

                Dim IdS As String = ""
                IdS = e.CommandArgument
                If IdS <> "" Then
                    Id = Convert.ToInt32(e.CommandArgument)
                Else
                    Id = 0
                End If

                Dim MVFaxQuoteDetailId As String = CType(dg_Selected.Rows(gvRow.DataItemIndex).FindControl("lbl_FaxQuoteDetailId"), Label).Text
                If MVFaxQuoteDetailId <> "" Then
                    DeleteFromMaster(MVFaxQuoteDetailId)
                End If

                DeleteGridRows("TempSecurityTable", gvRow.DataItemIndex, dg_Selected, Id)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Sub DeleteFromMaster(ByVal intUserId As Integer)
        Dim sqlComm As New SqlCommand
        Try
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_FaxQuoteDetail"
            sqlComm.Parameters.Clear()
            sqlComm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = intUserId
            sqlComm.CommandTimeout = 0
            sqlComm.ExecuteNonQuery()
            sqlComm.Dispose()
        Catch ex As Exception
            Throw ex
        Finally
            'CloseConn()
        End Try
    End Sub
    Private Sub DeleteGridRows(ByVal strSessionName As String, ByVal intIndex As Int16, ByVal grdView As GridView, ByVal Id As Int16)
        Try
            'Dim dt As DataTable
            'Dim objSrh As New clsSearch
            'Dim i As Integer
            'dt = Session(strSessionName)
            'For i = 0 To dt.Rows.Count
            '    If strSessionName = "CustomerContectTable" Then
            '        If Convert.ToInt32(dt.Rows(i)("CustomerId")) = Id Then
            '            dt.Rows.RemoveAt(i)
            '            Exit For
            '        End If
            '    Else
            '        If Convert.ToInt32(dt.Rows(i)("SecurityId")) = Id Then
            '            dt.Rows.RemoveAt(i)
            '            Exit For
            '        End If
            '    End If

            'Next


            'If strSessionName.ToLower.IndexOf("cust") = -1 Then
            '    objSrh.SetGrid(grdView, dt)
            'Else
            '    If dt.Rows.Count = 0 Then
            '        Dim dr As DataRow = dt.NewRow()
            '        dt.Rows.Add(dr)
            '    End If
            'End If

            'Session(strSessionName) = dt
            'If strSessionName = "CustomerContectTable" Then
            '    Session("CustomerContectTable") = dt
            '    dt.DefaultView.Sort = "CustomerId Asc"
            'Else
            '    Session("SecurityMaster") = dt
            '    dt.DefaultView.Sort = "SecurityTypeId Asc"
            'End If

            'dt = dt.DefaultView.ToTable()
            'grdView.DataSource = dt
            'grdView.DataBind()
            Dim dt As DataTable
            Dim objSrh As New clsSearch
            dt = Session(strSessionName)
            dt.Rows.RemoveAt(intIndex)
            If strSessionName.ToLower.IndexOf("cust") = -1 Then
                objSrh.SetGrid(grdView, dt)
            Else
                If dt.Rows.Count = 0 Then
                    Dim dr As DataRow = dt.NewRow()
                    dt.Rows.Add(dr)
                End If
            End If


            For i As Integer = dt.Rows.Count - 1 To 0 Step -1
                Dim row As DataRow = dt.Rows(i)
                If row.Item(0) Is Nothing Then
                    dt.Rows.Remove(row)
                ElseIf row.Item(0).ToString = "" Then
                    dt.Rows.Remove(row)
                End If
            Next
            Session(strSessionName) = dt
            grdView.DataSource = dt
            grdView.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function DeleteFaxQuote(ByVal intId As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            '  sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_FaxQuoteDetail"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@FaxQuoteDetailId", SqlDbType.VarChar, 30, "I", , , intId)
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            'sqlTrans.Rollback()
            'objUtil.WritErrorLog(PgName, "DeleteMergeDeal", "Error in DeleteMergeDeal", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub DeleteDailyMarketValue(ByVal intId As Int64)
        Try
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandTimeout = "100"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_FaxQuoteDetail"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@FaxQuoteDetailId", SqlDbType.Int, 4, "I", , , intId)
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Private Sub SetAttributes()
        '  btn_AddSecurity.Attributes.Add("onclick", "return SecurityInformation();")
        'btn_AddCustomer.Attributes.Add("onclick", "return ShowCustomer();")
        btn_AddTempCustomer.Attributes.Add("onclick", "return ShowTempCust();")
        btn_Save.Attributes.Add("onclick", "return ValidateSave();")
        'btn_Open.Attributes.Add("onclick", "return OpenQuoteWindow();")
        btn_CreateExcelFax.Attributes.Add("onclick", "return ValidateCustomerBroker();")
        btn_CreatePDFFax.Attributes.Add("onclick", "return ValidateCustomerBroker();")
        'txt_Date.Attributes.Add("onblur", "CheckDate(this,false); CallServerSide();")
        'txt_Date.Attributes.Add("onblur", "CheckDate(this,false);")

        btn_UpdateStock.Attributes.Add("onclick", "return  UpdStock();")
        btn_UpdateGridQuote.Attributes.Add("onclick", "return  UpdStock();")
    End Sub

    Private Sub FillCustomerDetailsGrid1()
        Try
            Dim arrVals() As String
            OpenConn()

            Dim strCond As String
            arrVals = Split(Hid_RetValues.Value, "$")
            If arrVals(3) <> "" Then
                strCond = BuildCustCond1(arrVals(3))
                dt1 = objCommon.FillDataTable(sqlConn, "ID_FILL_CustomerContactPerson1", , , strCond)
                Session("CustomerContectTable1") = dt1
                dt2 = Session("CustomerContectTable")
                dt3 = mergeDTs(dt1, dt2)
            Else
                dt2 = Session("CustomerContectTable")
                dt3 = dt2
            End If

            Session("CustomerFormatTable") = dt3
            dg_Customer.DataSource = dt3
            dg_Customer.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function BuildCustCond1(ByVal strCustIds As String) As String
        Try
            Dim SB As New StringBuilder
            SB.AppendLine("WHERE CM.CustomerId IN (" & Left(strCustIds.Replace("!", ","), strCustIds.Length - 1) & ")")
            'SB.AppendLine("AND ( CC.ContactId IN (" & Left(strContactIds.Replace("!", ","), strContactIds.Length - 1) & ") OR CC.ContactId IS NULL) ")
            Return SB.ToString
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Sub FillCustomerDetailsGrid()
        Try
            Dim arrVals() As String
            Dim arrIds() As String
            Dim strarrIds() As String
            Dim dt1 As DataTable
            Dim dt2 As DataTable
            Dim dt3 As DataTable
            Dim strCustId As String = ""
            Dim strContactId As String = ""
            Dim strCond As String

            OpenConn()
            arrVals = Split(Hid_RetValues.Value, "#")
            If (arrVals.Length > 1 And arrVals(1) <> "") Then
                arrIds = Split(arrVals(1), "!")
                If (arrIds.Length > 1) Then
                    Dim I As Integer
                    For I = 0 To arrIds.Length - 1
                        'strarrIds = Split(arrIds(I), "/")
                        If (arrIds(I) <> "") Then
                            strCustId += arrIds(I) + "!"
                            ' strContactId += strarrIds(1) + "!"
                        End If
                    Next
                End If
            End If

            'strCond = BuildCustCond(arrVals(1), arrVals(2))
            strCond = BuildCustCond(strCustId, strContactId)
            dt1 = objCommon.FillDataTable(sqlConn, "ID_FILL_CustomerContactPerson", , , strCond)

            If TypeOf Session("CustomerContectTable") Is DataTable Then
                dt2 = TryCast(Session("CustomerContectTable"), DataTable)
                dt3 = mergeDTs(dt1, dt2)
            Else
                dt3 = dt1
            End If

            Session("CustomerContectTable") = dt3
            dg_Customer.DataSource = dt3
            dg_Customer.DataBind()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function BuildCustCond(ByVal strCustIds As String, ByVal strContactIds As String) As String
        Try
            Dim SB As New StringBuilder
            SB.AppendLine("WHERE CM.CustomerId IN (" & Left(strCustIds.Replace("!", ","), strCustIds.Length - 1) & ")")
            If strContactIds.Length > 0 Then
                SB.AppendLine("AND ( CC.ContactDetailId IN (" & Left(strContactIds.Replace("!", ","), strContactIds.Length - 1) & ") OR CC.ContactDetailId IS NULL) ")
            End If
            Return SB.ToString
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Protected Sub btn_AddCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddCustomer.Click
        lbl_Msg.Text = ""
        FillCustomerDetailsGrid()
    End Sub

    Protected Sub btn_AddFormat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddFormat_.Click
        Dim dt As DataTable
        Dim dr As DataRow
        dt = Session("CustomerContectTable")
        Dim FormatIndex As Integer = Convert.ToInt32(Hid_FormatIndex.Value)
        dr = dt.Rows(FormatIndex)
        dr("FieldId") = Replace(Hid_SelectedFields.Value, "!", ",")
        Session("CustomerContectTable") = dt
    End Sub
    Private Function FillSecurityGrid_OpenQuote()
        Try
            Dim dt As DataTable
            Dim objSrh As New clsSearch
            Dim I As Integer
            Dim Margin As Double = 0.0
            dt = CType(Session("TempSecurityTable"), DataTable)

            'dt.DefaultView.Sort = "OrderId Asc,CreditRating Asc"
            dt = dt.DefaultView.ToTable()
            objSrh.SetGrid1(dg_Selected, dt)
            'Margin = Val(txt_Margin.Text) / 100
            'For I = 0 To dt.Rows.Count - 1
            '    dt.Rows(I)("Rate") = Convert.ToDouble(dt.Rows(I)("Rate")) + Margin
            '    dt.Rows(I)("Margin") = Convert.ToDouble(txt_Margin.Text)
            'Next
            Session("SecurityMaster") = dt
            dg_Selected.DataSource = dt
            dg_Selected.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub dg_Selected_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dg_Selected.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim imgBtnEdit As ImageButton
                Dim imgBtnDelete As ImageButton
                Dim txtLotSize As TextBox
                Dim txtRate As TextBox
                Dim txtYTM As TextBox
                Dim txtYTC As TextBox
                Dim txtYTMSemi As TextBox
                Dim txtYTCSemi As TextBox


                'Dim rbl As RadioButtonList = CType(e.Row.FindControl("rdo_TaxFree"), RadioButtonList)
                'Dim selected As String = e.Row.DataItem("TaxFree").ToString
                'If Not IsNothing(selected) Then
                '    rbl.SelectedValue = selected
                'End If

                Dim lblPerpetualFlag As Label
                imgBtnEdit = CType(e.Row.FindControl("imgBtn_Edit"), ImageButton)
                imgBtnEdit.Attributes.Add("onclick", "return UpdateDetails('" & e.Row.RowIndex & "')")
                imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
                txtLotSize = CType(e.Row.FindControl("lbl_LotSize"), TextBox)
                txtRate = CType(e.Row.FindControl("lbl_Rate"), TextBox)
                txtYTM = CType(e.Row.FindControl("txt_Yield"), TextBox)
                txtYTC = CType(e.Row.FindControl("txt_YTC"), TextBox)
                txtYTMSemi = CType(e.Row.FindControl("txt_YTMSemi"), TextBox)
                txtYTCSemi = CType(e.Row.FindControl("txt_YTCSemi"), TextBox)
                lblPerpetualFlag = CType(e.Row.FindControl("lbl_PerpetualFlag"), Label)
                Dim lblSemiAnnFlag As Label = DirectCast(e.Row.FindControl("lbl_Semi_Ann_Flag"), Label)
                Dim lbl_CallDate As Label = DirectCast(e.Row.FindControl("lbl_CallDate"), Label)
                Dim lbl_CallFlag As Label = DirectCast(e.Row.FindControl("lbl_CallFlag"), Label)
                Dim lbl_SecurityTypeName As Label = DirectCast(e.Row.FindControl("lbl_SecurityTypeName"), Label)
                ''*************Mehul**********
                Dim lbl_TypeFlag As Label = DirectCast(e.Row.FindControl("lbl_TypeFlag"), Label)
                imgBtnDelete.Attributes.Add("onclick", "return CheckDelete('" & Me.ClientID & "');")

                If CType(e.Row.FindControl("txt_SecurityName"), Label).Text = "" Then
                    imgBtnDelete.Visible = False
                End If

                If lblSemiAnnFlag.Text = "Y" Or lblSemiAnnFlag.Text = "N" Or lblSemiAnnFlag.Text = "A" Then
                    'txtYTCSemi.Style.Add("display", "none")
                    'txtYTMSemi.Style.Add("display", "none")
                Else
                    If lbl_SecurityTypeName.Text = "PSU" Or lbl_SecurityTypeName.Text = "PVT" Then
                    Else
                        'txtYTM.Style.Add("display", "none")
                        'txtYTC.Style.Add("display", "none")
                    End If
                End If

                If lblPerpetualFlag.Text = "P" Then
                    txtYTM.Style.Add("display", "none")
                    txtYTMSemi.Style.Add("display", "none")
                End If
                If lbl_CallDate.Text <> "" And (lblSemiAnnFlag.Text = "Y" Or lblSemiAnnFlag.Text = "A") Then
                    txtYTC.Style.Add("display", "block")
                    txtYTCSemi.Style.Add("display", "none")
                Else
                    txtYTC.Style.Add("display", "block")
                    txtYTCSemi.Style.Add("display", "block")
                End If

                If (lbl_CallDate.Text <> "" And lblSemiAnnFlag.Text <> "Y") Then
                    txtYTC.Style.Add("display", "block")
                    txtYTCSemi.Style.Add("display", "block")
                End If
                If lbl_CallDate.Text = "" Then
                    txtYTC.Style.Add("display", "none")
                    txtYTCSemi.Style.Add("display", "none")
                End If
                If lblSemiAnnFlag.Text = "Y" Or lblSemiAnnFlag.Text = "A" Then
                    txtYTCSemi.Style.Add("display", "none")
                    txtYTMSemi.Style.Add("display", "none")
                End If
                If Val(Hid_FaxQuoteId.Value) = 0 Then
                End If
                e.Row.Cells(3).Width = 500


                ' ''***********Mehul*******
                'If Trim(lbl_TypeFlag.Text) = "G" Then
                '    If (lbl_CallDate.Text = "") Then
                '        txtYTCSemi.Style.Add("display", "none")
                '        txtYTMSemi.Style.Add("display", "block")
                '    Else
                '        txtYTCSemi.Style.Add("display", "normal")
                '        txtYTMSemi.Style.Add("display", "normal")
                '    End If

                'Else
                '    txtYTCSemi.Style.Add("display", "none")
                '    txtYTMSemi.Style.Add("display", "none")
                'End If

                If lbl_CallFlag.Text = "0" Then
                    txtYTC.Style.Add("display", "none")
                    txtYTCSemi.Style.Add("display", "none")
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub dg_Customer_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dg_Customer.RowDataBound
        Try
            Dim btnChangeFormat As HtmlInputButton
            Dim imgBtnDelete As ImageButton
            Dim strFieldIds As String
            If e.Row.RowType = DataControlRowType.DataRow Then
                imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
                btnChangeFormat = CType(e.Row.FindControl("btn_AddFormat"), HtmlInputButton)
                If e.Row.Cells(1).Text = "&nbsp;" Then
                    imgBtnDelete.Visible = False
                    btnChangeFormat.Visible = False
                End If

                If CType(e.Row.FindControl("txt_CustomerName"), TextBox).Text = "" Then
                    btnChangeFormat.Visible = False
                    imgBtnDelete.Visible = False
                End If

                'lnk_AddFormat = CType(e.Row.FindControl("lnk_AddFormat"), LinkButton)
                If (Hid_SelectedFields.Value <> "") Then
                    strFieldIds = Hid_SelectedFields.Value
                Else
                    strFieldIds = TryCast(e.Row.DataItem, DataRowView).Row.Item("FieldId").ToString
                    Hid_SelectedFields.Value = strFieldIds
                    Session("FileldIds") = strFieldIds
                End If

                btnChangeFormat.Attributes.Add("onclick", "return ShowSelection('" & strFieldIds & "', '" & e.Row.RowIndex & "' );")
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Broker_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dg_Broker.RowDataBound
        Try
            Dim btnChangeFormat As HtmlInputButton
            Dim imgBtnDelete As ImageButton
            Dim strFieldIds As String
            If e.Row.RowType = DataControlRowType.DataRow Then
                imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
                btnChangeFormat = CType(e.Row.FindControl("btn_AddFormat"), HtmlInputButton)
                If e.Row.Cells(1).Text = "&nbsp;" Then
                    imgBtnDelete.Visible = False
                    btnChangeFormat.Visible = False
                End If

                If CType(e.Row.FindControl("txt_BrokerName"), TextBox).Text = "" Then
                    btnChangeFormat.Visible = False
                    imgBtnDelete.Visible = False
                End If

                'lnk_AddFormat = CType(e.Row.FindControl("lnk_AddFormat"), LinkButton)
                If (Hid_SelectedFields.Value <> "") Then
                    strFieldIds = Hid_SelectedFields.Value
                Else
                    strFieldIds = TryCast(e.Row.DataItem, DataRowView).Row.Item("FieldId").ToString
                    Hid_SelectedFields.Value = strFieldIds
                    Session("FileldIds") = strFieldIds
                End If

                btnChangeFormat.Attributes.Add("onclick", "return ShowSelection('" & strFieldIds & "', '" & e.Row.RowIndex & "' );")
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Public Shared Function mergeDTs(ByVal dt1 As DataTable, ByVal dt2 As DataTable) As DataTable
        Dim dtResult As DataTable = dt1.Clone()
        For Each dr As DataRow In dt1.Rows
            dtResult.Rows.Add(dr.ItemArray)
        Next
        For Each dr As DataRow In dt2.Rows
            dtResult.Rows.Add(dr.ItemArray)
        Next
        Return dtResult
    End Function

    Protected Sub btn_AddSecurity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddSecurity.Click
        Try
            lbl_Msg.Text = ""
            FillSecurityGrid()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function FillSecurityGrid()
        Try
            Dim K As Integer
            Dim Margin As Double = 0.0
            Dim stock_updtflag As Boolean
            If Hid_SelVal.Value <> "" Then

                Dim arrSecId As String() = Hid_SelVal.Value.Split(",")
                Dim IsMasterArr() As String
                IsMasterArr = Hid_SelVal.Value.Split("|")
                If IsMasterArr(1) = "1" Then
                    stock_updtflag = False
                Else
                    stock_updtflag = True
                End If
                Dim list As New List(Of SecIds)

                For i As Int32 = 0 To arrSecId.Length - 2 Step +1
                    Dim secid As Decimal = Convert.ToDecimal(arrSecId(i))
                    Dim secid1 As New SecIds()
                    secid1.Id = secid
                    list.Add(secid1)
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
                sqlComm.CommandTimeout = "0"
                sqlComm.CommandType = CommandType.StoredProcedure
                sqlComm.CommandText = "ID_SEARCH_SecurityFieldsNew_StockUpdate"
                sqlComm.Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@Ids_Xml", SqlDbType.Xml, 0, "I", , , xmldoc.OuterXml)
                objCommon.SetCommandParameters(sqlComm, "@exist", SqlDbType.Bit, 0, "I", , , True)
                objCommon.SetCommandParameters(sqlComm, "@forDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_Date.Text))
                objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId")))
                objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
                sqlComm.ExecuteNonQuery()
                sqlda.SelectCommand = sqlComm
                sqlda.Fill(dtAdd)
                sqldv = dtAdd.DefaultView
                sqldv.Sort = "SecurityTypeID Desc,SecurityId"
                Dim today As New Date

                If stock_updtflag = False Then
                Else
                    today = DateAndTime.Today
                    Dim s_today As String
                    ''DateTime today = DateTime.Today;

                    s_today = today.ToString("dd/MM/yyyy")
                    sqldv.RowFilter = "StockDate='" + s_today + "'"
                    dtAdd = sqldv.ToTable
                End If

                'dtAdd.Columns.Add(New DataColumn("IsMaster", GetType(String)))
                'Dim dtAddR As Integer
                'If IsMasterArr(1).ToString() = "1" Then
                '    For dtAddR = 0 To dtAdd.Rows.Count - 1
                '        dtAdd.Rows(dtAddR)("IsMaster") = "1"
                '    Next
                'Else
                '    For dtAddR = 0 To dtAdd.Rows.Count - 1
                '        dtAdd.Rows(dtAddR)("IsMaster") = "0"
                '    Next
                'End If

                ' check for duplicate security
                Dim securityId As String = ""
                If dtAdd Is Nothing = False Then
                    If dtAdd.Rows.Count > 0 Then
                        securityId = dtAdd.Rows(0)("SecurityId").ToString()
                    End If
                End If


                Dim dtMerge As DataTable = New DataTable()
                Dim dtMergeNew As DataTable = New DataTable()
                Dim dvMerge As DataView

                Dim dtSec As DataTable = Session("SecurityMaster")

                Dim dtNewSecurity As DataTable = Session("TempSecurityTable")

                Dim dtSecAdd As DataTable = Session("SecAdd")
                If dtSecAdd Is Nothing Then
                    dtMerge = dtSec.Copy()
                    dtMerge.Merge(dtAdd, True, MissingSchemaAction.Ignore)
                    Session("SecurityMaster") = dtMerge

                End If

                'If dtNewSecurity Is Nothing Then
                dtMergeNew = dtNewSecurity.Copy()
                dtMergeNew.Merge(dtAdd, True, MissingSchemaAction.Ignore)
                Session("TempSecurityTable") = dtMergeNew
                'End If

                Dim H As Integer
                If dtMerge Is Nothing Then

                Else
                    If dtMerge.Rows.Count > 0 Then
                        For H = 0 To dtMerge.Rows.Count - 1
                            If dtMerge.Rows(H)("Yield").ToString() = "" Then
                                dtMerge.Rows(H)("Yield") = "0.0000"
                            End If

                            If dtMerge.Rows(H)("YTCAnn").ToString() = "" Then
                                dtMerge.Rows(H)("YTCAnn") = "0.0000"
                            End If

                            If dtMerge.Rows(H)("YTCSemi").ToString() = "" Then
                                dtMerge.Rows(H)("YTCSemi") = "0.0000"
                            End If

                            If dtMerge.Rows(H)("YTMSemi").ToString() = "" Then
                                dtMerge.Rows(H)("YTMSemi") = "0.0000"
                            End If
                        Next
                    End If
                End If

                '   dtMerge.Columns.Add(New DataColumn("IsMaster", GetType(String)))


                dtMerge = Session("TempSecurityTable")
                dtMerge = RemoveEmptyRowsFromDataTable(dtMerge)
                dtMerge.DefaultView.Sort = "SecurityTypeId Asc,SecurityId Asc"
                dtMerge = dtMerge.DefaultView.ToTable()
                dvMerge = dtMerge.DefaultView
                dg_Selected.DataSource = dvMerge
                dg_Selected.DataBind()

                If dtMerge.Rows.Count = 0 Then
                    '  btn_Save.Visible = False
                Else
                    ' btn_Save.Visible = True
                End If
            End If


        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function


    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction()
            If InsertFaxQuote(sqlTrans) = False Then Exit Sub
            If SaveFaxCustomer(sqlTrans) = False Then Exit Sub
            If SaveQuoteDetails(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            'If Val(Hid_FaxQuoteId.Value) <> 0 Then
            FillBlankSecurityGrids()
            'End If
            Hid_FaxQuoteId.Value = ""
            btn_Save.Text = "Save Quote"
            GetQuoteNames()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function SaveFaxCustomer(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim I As Int32
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_FaxCustomerDetail"
            sqlComm.Transaction = sqlTrans
            For I = 0 To dg_Customer.Rows.Count - 2
                sqlComm.Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@FaxQuoteId", SqlDbType.BigInt, 8, "I", , , Val(intFaxQuoteId))
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , Val(CType(dg_Customer.Rows(I).FindControl("lbl_CustomerId"), Label).Text))
                objCommon.SetCommandParameters(sqlComm, "@FaxSentUser", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
                objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
                objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, SqlDbType.Int, "O")
                objCommon.SetCommandParameters(sqlComm, "@FaxCustomerId", SqlDbType.Int, 4, "O")
                sqlComm.ExecuteNonQuery()
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Return False
        End Try
    End Function
    Private Function SaveQuoteDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Dim secId As Integer = 0
        Try
            Dim I As Int32
            Dim strQuantity As String
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_FaxQuoteDetails"
            sqlComm.Transaction = sqlTrans
            For I = 0 To dg_Selected.Rows.Count - 1
                sqlComm.Parameters.Clear()

                ''**************Mehul******************
                strQuantity = ""
                strQuantity = CType(dg_Selected.Rows(I).FindControl("lbl_LotSize"), TextBox).Text
                'If Trim(strQuantity) <> "" Then
                '    If InStr(strQuantity, "Cr.") > 0 Then
                '        strQuantity = strQuantity
                '    Else
                '        strQuantity = strQuantity & " Cr."
                '    End If
                'End If
                ''**************************************

                objCommon.SetCommandParameters(sqlComm, "@FaxQuoteId", SqlDbType.BigInt, 8, "I", , , intFaxQuoteId)
                objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_SecurityId"), Label).Text)

                objCommon.SetCommandParameters(sqlComm, "@SecurityName", SqlDbType.VarChar, 100, "I", , , CType(dg_Selected.Rows(I).FindControl("txt_SecurityName"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@Date", SqlDbType.DateTime, 4, "I", , , Now)
                'objCommon.SetCommandParameters(sqlComm, "@Rate", SqlDbType.Decimal, 18, "I", , , Convert.ToDouble(CType(dg_Selected.Rows(I).FindControl("lbl_Rate"), TextBox).Text))
                objCommon.SetCommandParameters(sqlComm, "@Rate", SqlDbType.Decimal, 18, "I", , , Convert.ToDouble(CType(dg_Selected.Rows(I).FindControl("hdnRate"), HiddenField).Value))
                '' '' ''objCommon.SetCommandParameters(sqlComm, "@Quantity", SqlDbType.VarChar, 100, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_LotSize"), TextBox).Text)
                objCommon.SetCommandParameters(sqlComm, "@Quantity", SqlDbType.VarChar, 100, "I", , , Trim(strQuantity))
                objCommon.SetCommandParameters(sqlComm, "@RatingRemarks", SqlDbType.VarChar, 100, "I", , , CType(dg_Selected.Rows(I).FindControl("txt_RatingRemark"), TextBox).Text)
                objCommon.SetCommandParameters(sqlComm, "@Rate_Actual_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_Rate_Actual_Flag"), Label).Text)
                objCommon.SetCommandParameters(sqlComm, "@Physical_DMAT_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_PhysicalDMAT"), Label).Text)
                objCommon.SetCommandParameters(sqlComm, "@YTM_XIRR_MMY_Flag", SqlDbType.Char, 1, "I", , , "")
                objCommon.SetCommandParameters(sqlComm, "@RateActual", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_RateActual"), Label).Text)
                objCommon.SetCommandParameters(sqlComm, "@AccIntDays", SqlDbType.Int, 4, "I", , , 0)
                objCommon.SetCommandParameters(sqlComm, "@Days_Flag", SqlDbType.Char, 1, "I", , , "A")
                objCommon.SetCommandParameters(sqlComm, "@IPCalc", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_IPCalc"), Label).Text)

                'objCommon.SetCommandParameters(sqlComm, "@IsMaster", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_IsMaster"), Label).Text)

                If CType(dg_Selected.Rows(I).FindControl("txt_Yield"), TextBox).Text = "" Then
                    CType(dg_Selected.Rows(I).FindControl("txt_Yield"), TextBox).Text = "0.0000"
                End If
                If CType(dg_Selected.Rows(I).FindControl("lbl_YTPAnn"), Label).Text = "" Then
                    CType(dg_Selected.Rows(I).FindControl("lbl_YTPAnn"), Label).Text = "0.0000"
                End If
                If CType(dg_Selected.Rows(I).FindControl("txt_YTC"), TextBox).Text = "" Then
                    CType(dg_Selected.Rows(I).FindControl("txt_YTC"), TextBox).Text = "0.0000"
                End If
                If CType(dg_Selected.Rows(I).FindControl("txt_YTMSemi"), TextBox).Text = "" Then
                    CType(dg_Selected.Rows(I).FindControl("txt_YTMSemi"), TextBox).Text = "0.0000"
                End If
                If CType(dg_Selected.Rows(I).FindControl("txt_YTCSemi"), TextBox).Text = "" Then
                    CType(dg_Selected.Rows(I).FindControl("txt_YTCSemi"), TextBox).Text = "0.0000"
                End If
                If CType(dg_Selected.Rows(I).FindControl("lbl_YTPSemi"), Label).Text = "" Then
                    CType(dg_Selected.Rows(I).FindControl("lbl_YTPSemi"), Label).Text = "0.0000"
                End If
                objCommon.SetCommandParameters(sqlComm, "@YTMAnn", SqlDbType.Decimal, 18, "I", , , Convert.ToDouble(CType(dg_Selected.Rows(I).FindControl("txt_Yield"), TextBox).Text))
                objCommon.SetCommandParameters(sqlComm, "@YTCAnn", SqlDbType.Decimal, 18, "I", , , Convert.ToDouble(CType(dg_Selected.Rows(I).FindControl("txt_YTC"), TextBox).Text))
                objCommon.SetCommandParameters(sqlComm, "@YTPAnn", SqlDbType.Decimal, 18, "I", , , Convert.ToDouble(CType(dg_Selected.Rows(I).FindControl("lbl_YTPAnn"), Label).Text))
                objCommon.SetCommandParameters(sqlComm, "@YTMSemi", SqlDbType.Decimal, 18, "I", , , Convert.ToDouble(CType(dg_Selected.Rows(I).FindControl("txt_YTMSemi"), TextBox).Text))
                objCommon.SetCommandParameters(sqlComm, "@YTCSemi", SqlDbType.Decimal, 18, "I", , , Convert.ToDouble(CType(dg_Selected.Rows(I).FindControl("txt_YTCSemi"), TextBox).Text))
                objCommon.SetCommandParameters(sqlComm, "@YTPSemi", SqlDbType.Decimal, 18, "I", , , Convert.ToDouble(CType(dg_Selected.Rows(I).FindControl("lbl_YTPSemi"), Label).Text))

                objCommon.SetCommandParameters(sqlComm, "@Semi_Ann_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_Semi_Ann_Flag"), Label).Text)
                objCommon.SetCommandParameters(sqlComm, "@CombineIPMat", SqlDbType.Bit, 1, "I", , , Val(CType(dg_Selected.Rows(I).FindControl("lbl_CombineIPMat"), Label).Text))
                'objCommon.SetCommandParameters(sqlComm, "@Rate_Actual_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_Rate_Actual_Flag"), Label).Text)
                objCommon.SetCommandParameters(sqlComm, "@Equal_Actual_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_Equal_Actual_Flag"), Label).Text)
                objCommon.SetCommandParameters(sqlComm, "@IntDays", SqlDbType.Int, 4, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_IntDays"), Label).Text)
                objCommon.SetCommandParameters(sqlComm, "@FirstYrAllYr", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_FirstYrAllYr"), Label).Text)

                objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
                objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, SqlDbType.Int, "O")
                objCommon.SetCommandParameters(sqlComm, "@FaxQuoteDetailId", SqlDbType.BigInt, 8, "O")


                'objCommon.SetCommandParameters(sqlComm, "@FaxQuoteId", SqlDbType.BigInt, 8, "I", , , intFaxQuoteId)
                'objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_SecurityId"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@SecurityName", SqlDbType.VarChar, 100, "I", , , CType(dg_Selected.Rows(I).FindControl("txt_SecurityName"), TextBox).Text)
                'objCommon.SetCommandParameters(sqlComm, "@Date", SqlDbType.DateTime, 4, "I", , , objCommon.DateFormat(CType(dg_Selected.Rows(I).FindControl("lbl_Date"), Label).Text))
                'objCommon.SetCommandParameters(sqlComm, "@Rate", SqlDbType.Decimal, 18, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_Rate"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@Quantity", SqlDbType.VarChar, 100, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_LotSize"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@RatingRemarks", SqlDbType.VarChar, 100, "I", , , CType(dg_Selected.Rows(I).FindControl("txt_RatingRemark"), TextBox).Text)
                ''objCommon.SetCommandParameters(sqlComm, "@Rate_Actual_Flag", SqlDbType.Char, 1, "I", , , Trim(dg_Selected.Rows(I).Cells(12).Text))
                'objCommon.SetCommandParameters(sqlComm, "@Physical_DMAT_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_PhysicalDMAT"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@YTM_XIRR_MMY_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_YXM"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@RateActual", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_RateActual"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@IPCalc", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_IPCalc"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@AccIntDays", SqlDbType.Int, 4, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_Days"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@Days_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_DaysOptions"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@YTMAnn", SqlDbType.Decimal, 18, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_YTMAnn"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@YTCAnn", SqlDbType.Decimal, 18, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_YTCAnn"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@YTPAnn", SqlDbType.Decimal, 18, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_YTPAnn"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@YTMSemi", SqlDbType.Decimal, 18, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_YTMSemi"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@YTCSemi", SqlDbType.Decimal, 18, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_YTCSemi"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@YTPSemi", SqlDbType.Decimal, 18, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_YTPSemi"), Label).Text)

                'objCommon.SetCommandParameters(sqlComm, "@Semi_Ann_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_Semi_Ann_Flag"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@CombineIPMat", SqlDbType.Bit, 1, "I", , , Val(CType(dg_Selected.Rows(I).FindControl("lbl_CombineIPMat"), Label).Text))
                'objCommon.SetCommandParameters(sqlComm, "@Rate_Actual_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_Rate_Actual_Flag"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@Equal_Actual_Flag", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_Equal_Actual_Flag"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@IntDays", SqlDbType.Int, 4, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_IntDays"), Label).Text)
                'objCommon.SetCommandParameters(sqlComm, "@FirstYrAllYr", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_FirstYrAllYr"), Label).Text)

                'objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
                'objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, SqlDbType.Int, "O")
                'objCommon.SetCommandParameters(sqlComm, "@FaxQuoteDetailId", SqlDbType.BigInt, 8, "O")

                objCommon.SetCommandParameters(sqlComm, "@YieldPriceType", SqlDbType.Char, 1, "I", , , CType(dg_Selected.Rows(I).FindControl("lbl_YieldPriceType"), Label).Text)
                secId = Convert.ToInt32(CType(dg_Selected.Rows(I).FindControl("lbl_SecurityId"), Label).Text)
                sqlComm.ExecuteNonQuery()
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            secId = secId
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Return False
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

    Private Sub GetQuoteNames()
        Try

            Dim I As Int32
            Dim dt As DataTable
            Dim strCond As String = " BM.BranchId=" & Trim(Session("BranchId"))
            Hid_AllQuoteNames.Value = ""
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxQuoteNames", Session("UserId"), "UserId")
            Session("FaxQuoteTable") = dt
            For I = 0 To dt.Rows.Count - 1
                With dt.Rows(I)
                    Hid_AllQuoteNames.Value += Trim(.Item("QuoteName") & "") & "!"
                End With
            Next
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Protected Sub btn_Open_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "ID_Fill_QuoteSecurity", Val(Hid_FaxQuoteId.Value), "FaxQuoteId")
            Session("TempSecurityTable") = dt
            Session("SavedFaxQuotes") = dt
            'FillSecurityGrid()
            FillSecurityGrid_OpenQuote()
            btn_Save.Text = "Save As Quote"
            btn_UpdateStock.Visible = False
            btn_UpdateGridQuote.Visible = True
            'btn_Save.Visible = False
            lbl_Msg.Text = ""
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function InsertFaxQuote(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand

            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_FaxQuotes"
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            With objCommon
                .SetCommandParameters(sqlComm, "@QuoteName", SqlDbType.VarChar, 50, "I", , , Hid_QuoteName.Value)
                .SetCommandParameters(sqlComm, "@SavedDate", SqlDbType.SmallDateTime, 4, "I", , , Today)
                .SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId") & ""))
                .SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
                .SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, SqlDbType.Int, "O")
                .SetCommandParameters(sqlComm, "@FaxQuoteId", SqlDbType.BigInt, 8, "O")
            End With
            sqlComm.ExecuteNonQuery()
            intFaxQuoteId = Val(sqlComm.Parameters("@FaxQuoteId").Value & "")
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Return False
        End Try
    End Function


    Private Sub SendMail(ByVal strFilePath As String, ByVal toAddress As String)
        Try



            Dim fromAddress As String = "debt@smcindiaonline.com"
            Dim bcc As String = "debt@smcindiaonline.com"
            'Dim fromAddress As String = "tssuvarna@gmail.com"

            ''Gmail Address from where you send the mail
            ''Dim toAddress = "poonam.sinha@genesissoftware.co.in"

            'toAddress = "suvarna.sreekant@genesissoftware.co.in"
            ' Dim toAddress As String = "genesismail123@gmail.com"

            ''Password of your gmail address
            Dim subject As String = "Offer File"

            Dim body As String = "Hi,"
            body += Environment.NewLine
            body += " <br/>I am sending you the Offer file attachment for your reference. <br/>"
            body += " <br/>Regards, <br/>" & Convert.ToString(Session("Username"))

            'strExcelFilePath = strExcelFilePath & "Report.xls"
            MailHelper.SendMailMessage(fromAddress, toAddress, "", "", subject, body, strFilePath, "")

            If File.Exists(strFilePath & "Report.xls") Then
                File.Delete(strFilePath & "Report.xls")
            End If

            'Response.Write("true")
            'Response.End()

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SendMail", "Error in SendMail", "", ex)
            Throw ex
        End Try
    End Sub

    Private Function FillUserContent(ByVal SR As StreamReader) As String
        Try
            Dim ds As DataTable
            Dim strOpt As String = ""
            Dim strLineContent As String
            OpenConn()

            ds = objCommon.FillDataTable(sqlConn, "ID_SELECT_UserMaster", Session("UserId"), "UserId")

            If ds.Rows(0).Item(0) > 0 Then
                'While SR.Peek >= 0
                '    strLineContent = SR.ReadLine
                '    If strLineContent.IndexOf("please contact") <> -1 Then
                With ds.Rows(0)
                    strLineContent = " " & Trim(.Item("NameOfUser") & "")
                    'If Trim(.Item("MobileNo") & "") <> "" Or Trim(.Item("PhoneNo") & "") <> "" Or Trim(.Item("FaxNo") & "") <> "" Or Trim(.Item("EmailId") & "") <> "" Then
                    '    strLineContent += " on "
                    'End If
                    strUserName = Trim(.Item("NameOfUser") & "")
                    If Trim(.Item("MobileNo") & "") <> "" Then
                        strLineContent += Environment.NewLine & " Mobile: " & Trim(.Item("MobileNo") & "")
                        '    strOpt = " / "
                        strUserMobile = "Mobile: " & Trim(.Item("MobileNo") & "")
                    End If
                    'strUserBranchAddressExcel = Trim(.Item("ExcelBranchAddress") & "")
                    'strUserBranchAddressPDF = Trim(.Item("PDFBranchAddress") & "")

                    strUserBranchAddressExcel = ""
                    strUserBranchAddressPDF = ""

                    'If Trim(.Item("PhoneNo") & "") <> "" Then
                    '    strLineContent += strOpt & Trim(.Item("PhoneNo") & "")
                    '    strOpt = ","
                    'Else
                    '    strOpt = ""
                    'End If
                    'If Trim(.Item("FaxNo") & "") <> "" Then
                    '    strLineContent += strOpt & " Fax: " & Trim(.Item("FaxNo") & "")
                    '    strOpt = " or "
                    'Else
                    '    strOpt = ""
                    'End If
                    strBranchName = Trim(.Item("BranchName") & "")
                    If Trim(.Item("EmailId") & "") <> "" Then
                        strLineContent += Environment.NewLine & " Email: " & Trim(.Item("EmailId") & "")
                        strUseremail = "Email: " & Trim(.Item("EmailId") & "")
                    End If
                End With
                Return strLineContent
            End If
            '    End While
            'End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function
    Private Sub GetImages()
        Try
            'to be changed 
            Dim ds As DataTable
            Hid_BranchId.Value = Val(Session("BranchId") & "")
            'ds = objCommon.GetDataSet(SqlDataSourceBranch)
            OpenConn()
            ds = objCommon.FillDataTable(sqlConn, "ID_SELECT_BranchMaster", Session("Branchid"), "Branchid")
            If ds.Rows(0).Item(0) > 0 Then
                With ds.Rows(0)
                    If Val(.Item("HeaderLength") & "") <> 0 Then
                        blnHeaderExist = True
                        strHeaderFile = SaveImage(.Item("HeaderData"), Trim(.Item("HeaderType")), Trim(.Item("HeaderFileName")))
                    End If
                    If Val(.Item("FooterLength") & "") <> 0 Then
                        blnFooterExist = True
                        strFooterFile = SaveImage(.Item("FooterData"), Trim(.Item("FooterType")), Trim(.Item("FooterFileName")))
                    End If
                End With
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            'CloseConn()
        End Try
    End Sub
    'Private Sub SetTableBorders(ByVal myRange As Excel.Range)
    '    Try
    '        'myRange.Borders(Excel.XlBordersIndex.xlEdgeBottom  
    '        SetTableStyle(myRange, Excel.XlBordersIndex.xlEdgeLeft)
    '        SetTableStyle(myRange, Excel.XlBordersIndex.xlEdgeTop)
    '        SetTableStyle(myRange, Excel.XlBordersIndex.xlEdgeBottom)
    '        SetTableStyle(myRange, Excel.XlBordersIndex.xlEdgeRight)
    '        SetTableStyle(myRange, Excel.XlBordersIndex.xlInsideHorizontal)
    '        SetTableStyle(myRange, Excel.XlBordersIndex.xlInsideVertical)
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub
    Private Sub SetTableStyle(ByRef myRange As ClosedXML.Excel.IXLCell, Optional ByVal str As String = "", Optional ByVal strFieldName As String = "")
        Try
            With myRange.Style.Border
                .BottomBorder = XLBorderStyleValues.Thin
                .LeftBorder = XLBorderStyleValues.Thin
                .RightBorder = XLBorderStyleValues.Thin
                .TopBorder = XLBorderStyleValues.Thin

            End With

            If str = "" Then
                With myRange.Style.Alignment
                    .WrapText = True
                    .Horizontal = XLAlignmentHorizontalValues.Center
                    .Vertical = XLAlignmentVerticalValues.Center
                End With

            End If
            If str = "M" Then
                With myRange.Style.Fill
                    '.BackgroundColor = XLColor.LightGray
                    .BackgroundColor = XLColor.FromArgb(79, 129, 189)
                End With
                With myRange.Style.Font
                    .Bold = True
                    .FontColor = XLColor.Black
                    .SetFontName("calibri")
                    .SetFontSize(10)
                End With
                With myRange.Style.Alignment
                    .WrapText = True
                    .Horizontal = XLAlignmentHorizontalValues.Center
                    .Vertical = XLAlignmentVerticalValues.Center
                End With

            End If
            If str = "SH" Then
                With myRange.Style.Fill
                    '.BackgroundColor = XLColor.LightGray
                    .BackgroundColor = XLColor.FromArgb(79, 129, 189)
                End With
                With myRange.Style.Font
                    .Bold = True
                    .FontColor = XLColor.Black
                    .SetFontName("calibri")
                    .SetFontSize(10)
                End With
                With myRange.Style.Alignment
                    .WrapText = True
                    .Horizontal = XLAlignmentHorizontalValues.Left
                    .Vertical = XLAlignmentVerticalValues.Center
                End With

            End If

            If str = "C" Then
                With myRange.Style.Border
                    .BottomBorder = XLBorderStyleValues.Thin
                    .LeftBorder = XLBorderStyleValues.Thin
                    .RightBorder = XLBorderStyleValues.Thin
                    .TopBorder = XLBorderStyleValues.Thin
                End With
                With myRange.Style.Alignment
                    .Horizontal = XLAlignmentHorizontalValues.Center
                    .Vertical = XLAlignmentVerticalValues.Center
                End With
                With myRange.Style.Font
                    .SetFontName("calibri")
                    .SetFontSize(10)
                End With
            End If
            If str = "N" Then
                With myRange.Style.Border
                    .BottomBorder = XLBorderStyleValues.None
                    .LeftBorder = XLBorderStyleValues.None
                    .RightBorder = XLBorderStyleValues.None
                    .TopBorder = XLBorderStyleValues.None
                End With
                With myRange.Style.Alignment
                    .Horizontal = XLAlignmentHorizontalValues.Left
                    .Vertical = XLAlignmentVerticalValues.Center
                End With
                With myRange.Style.Font
                    .SetFontName("calibri")
                    .SetFontSize(10)
                End With
            End If
            'If strFieldName = "SecurityName" Then
            '    myRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            'End If


        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    'Private Sub MergeCells(ByVal chrCell1 As Char, ByVal chrCell2 As Char)
    '    Try
    '        Dim myRange As Excel.Range
    '        myRange = oSheet.Range("A19:H19")
    '        '  myRange = oSheet.Range(chrCell1 & intRowIndex, chrCell2 & intRowIndex)
    '        With myRange
    '            .HorizontalAlignment = Excel.Constants.xlGeneral
    '            .VerticalAlignment = Excel.Constants.xlBottom
    '            .WrapText = False
    '            .Orientation = 0
    '            .AddIndent = False
    '            .IndentLevel = 0
    '            .ShrinkToFit = False
    '            .ReadingOrder = Excel.Constants.xlContext
    '            .MergeCells = True
    '        End With
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub


    Private Function WriteContents(ByVal strFilePath As String) As StreamReader
        Try
            Dim FS As FileStream
            Dim SR As StreamReader
            FS = New FileStream(strFilePath, FileMode.Open, FileAccess.Read)
            SR = New StreamReader(FS)
            Return SR
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function SaveImage(ByVal img() As Byte, ByVal strType As String, ByVal strFile As String) As String
        Try
            imgNew = img
            Dim strFileSave As String
            Dim MS As MemoryStream
            Dim newImage As System.Drawing.Image
            Dim strTemp() As String
            strFileSave = ConfigurationManager.AppSettings("ImagePath") & "\" & Trim(strFile)
            MS = New MemoryStream(img, 0, img.Length)


            MS.Write(img, 0, img.Length)
            newImage = System.Drawing.Image.FromStream(MS, True)
            strTemp = Split(strFileSave, ".")
            newImage.Save(strTemp(0) & ".bmp", System.Drawing.Imaging.ImageFormat.Bmp)

            strTemp = Split(strFile, ".")
            Return strTemp(0) & ".bmp"

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function IsInArray(ByVal FindValue As Object, ByVal arrSearch As Object) As Boolean
        Dim blnExist As Boolean = False
        If Not IsArray(arrSearch) Then Return blnExist
        If Array.IndexOf(arrSearch, FindValue) >= 0 Then
            Return True
        Else
            Return False
        End If
    End Function


    Private Function GetTable(ByVal strProcName As String, ByVal SecurityId As Integer, ByVal TaxFree As String) As System.Data.DataTable
        Try
            Dim datFrom As Date
            Dim datTo As Date
            Dim dtfill As New System.Data.DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            sqlComm.CommandTimeout = 0
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strProcName
                .Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , SecurityId)
                objCommon.SetCommandParameters(sqlComm, "@TaxFree", SqlDbType.Char, 1, "I", , , "N")
                objCommon.SetCommandParameters(sqlComm, "@SettmentDate", SqlDbType.SmallDateTime, 8, "I", , , objCommon.DateFormat(txt_Date.Text))
                objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)
            Return dtfill
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
        End Try
    End Function

    Private Function RemoveDuplicates(ByVal dt As DataTable) As DataTable
        dt = dt.DefaultView.ToTable(True, "SecurityId")
        dt.AcceptChanges()
        Return dt
    End Function

    Private Sub AddPDFTableRows(ByVal dt As DataTable, ByVal intIndex As Int32, ByRef table As iTextSharp.text.Table)
        Try
            Dim strFields() As String
            Dim strFieldName As String
            Dim strTableName As String
            Dim strColName As String
            Dim strFieldValue As String
            Dim I As Int32
            Dim J As Int32
            Dim dsFieldNames As New DataTable
            Dim bFlag As Boolean = False
            ''''
            Dim PdfPCell As iTextSharp.text.pdf.PdfPCell
            '''

            Dim dtSel As DataTable
            OpenConn()
            Dim DvSel As DataView = New DataView()

            'strFieldValue = Trim(CType(dg_Selected.(intIndex).FindControl("lbl_" & strFieldName), Label).Text & "")
            dtSel = TryCast(Session("TempSecurityTable"), DataTable)
            DvSel = New DataView(dtSel)

            DvSel.RowFilter = String.Empty
            DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value

            strFields = Split(Hid_SelectedFields.Value, ",")

            'strHTMLContent.Append("<tr>".ToString)
            'For I = 0 To strFields.Length - 2
            For I = 0 To strFields.Length - 1
                'strHTMLContent.Append("<td>".ToString)
                Hid_FieldId.Value = Val(strFields(I))
                dsFieldNames = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")

                With dsFieldNames.Rows(0)
                    strTableName = Trim(.Item("TableName") & "")
                    strFieldName = Trim(.Item("TableField") & "")
                    If strTableName <> "FromPage" Then

                        For J = 0 To dt.Columns.Count - 1
                            strColName = dt.Columns(J).ColumnName
                            If UCase(strFieldName) = UCase(strColName) Then
                                'strHTMLContent.Append(dt.Rows(0).Item(strColName).ToString)
                                'If LCase(strFieldName) = "calldatei" Then
                                'strFieldValue = IIf(Trim(dt.Rows(0).Item("CallDateI").ToString & "") = "", Trim(dt.Rows(0).Item("PutDate").ToString) & "", Trim(dt.Rows(0).Item("CallDateI").ToString & ""))
                                'Else
                                strFieldValue = IIf(Trim(dt.Rows(0).Item(strColName).ToString & "") = "", "  ", Trim(dt.Rows(0).Item(strColName).ToString & ""))
                                'End If

                                ''
                                'Dim font8 As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("ARIAL", 7)
                                'PdfPCell = New PdfPCell(New iTextSharp.text.Phrase(New iTextSharp.text.Chunk(strFieldValue)), font8)
                                'table.AddCell(PdfPCell)

                                ''
                                table.AddCell(strFieldValue)

                                bFlag = True
                                Exit For
                            End If
                        Next

                        If bFlag = False Then
                            table.AddCell("")
                        End If
                        bFlag = False

                    Else

                        'dtSel = TryCast(Session("TempSecurityTable"), DataTable)
                        'strFieldValue = Trim(dtSel.Rows(intIndex).Item(strFieldName) & "")
                        'strHTMLContent.Append(strFieldValue.ToString)

                        'Dim dtSel As DataTable
                        strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
                        'strFieldValue = Trim(dtSel.Rows(intIndex).Item(strFieldName) & "")

                        If IsNumeric(strFieldValue) = True Then
                            strFieldValue = String.Format(CDbl(strFieldValue), "#############0.00")
                        Else
                            strFieldValue = IIf(strFieldValue = "", " ", strFieldValue)
                        End If
                        'strHTMLContent.Append(strFieldValue.ToString)
                        table.AddCell(strFieldValue.ToString)
                    End If
                End With
                'strHTMLContent.Append("</td>".ToString)
            Next
            'strHTMLContent.Append("</tr>".ToString)
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            'CloseConn()
        End Try
    End Sub
    Private Function AddPDFTable() As iTextSharp.text.Table
        Try
            Dim ds As New DataTable
            Dim I As Int32
            Dim dsFill As New DataSet
            Dim dsFill1 As New DataSet
            Dim strFields() As String
            Dim table As iTextSharp.text.Table
            Dim RX As iTextSharp.text.Cell
            Dim dt As DataTable
            Dim PrevSecTypeId As Integer
            Dim dtNew As DataTable
            Dim j As Integer

            dt = CType(Session("TempSecurityTable"), DataTable)

            Dim view As DataView = New DataView(dt)
            Dim distinctValues As DataTable = view.ToTable(True, "SecurityTypeId")

            OpenConn()

            strFields = Split(Hid_SelectedFields.Value, ",")
            'table = New iTextSharp.text.Table(strFields.Length - 1, dg_Selected.Rows.Count + 1)
            'table = New iTextSharp.text.Table(strFields.Length, dg_Selected.Rows.Count + 1)

            table = New iTextSharp.text.Table(strFields.Length, dg_Selected.Rows.Count + distinctValues.Rows.Count + 1)

            table.WidthPercentage = 99
            table.Cellspacing = 1
            table.Cellpadding = 0


            Dim tbCell As iTextSharp.text.Cell
            'tbCell.Width = 200

            'For I = 0 To strFields.Length - 2
            For I = 0 To strFields.Length - 1
                Hid_FieldId.Value = Val(strFields(I))
                ds = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                tbCell = New iTextSharp.text.Cell(New iTextSharp.text.Paragraph(ds.Rows(0).Item("FaxField").ToString, font))
                tbCell.Width = 200
                table.AddCell(tbCell)
                'table.AddCell(ds.Rows(0).Item("FaxField").ToString)

            Next

            dtNew = dt.Copy()
            dtNew.DefaultView.Sort = "TypeFlag Asc, SecurityTypeId Asc,SecurityId Asc"
            dtNew = dtNew.DefaultView.ToTable()

            'For I = 0 To dg_Selected.Rows.Count - 1
            For I = 0 To dtNew.Rows.Count - 1
                'Hid_SecId.Value = Val(CType(dg_Selected.Rows(I).FindControl("lbl_SecurityId"), Label).Text)
                'Hid_SecTypeId.Value = dt.Rows(I)("SecurityTypeId").ToString()
                Hid_SecId.Value = dtNew.Rows(I)("SecurityId").ToString()
                Hid_SecTypeId.Value = dtNew.Rows(I)("SecurityTypeId").ToString()
                Dim strTaxFree As String = "N"
                'dsFill.Tables.Add(objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId"))

                dsFill.Tables.Add(GetTable("ID_FILL_SecurityInfo", Val(Hid_SecId.Value), strTaxFree))
                dsFill1.Tables.Add(objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Val(Hid_SecId.Value), "SecurityId"))

                Hid_RowNo.Value = Val(CType(dg_Selected.Rows(I).FindControl("lbl_RowNumber"), Label).Text)
                If PrevSecTypeId <> Val(Hid_SecTypeId.Value) Then
                    'add security type name
                    'iTextSharp.text.Table.CELL(RX)
                    RX = New iTextSharp.text.Cell()                    'RX = New iTextSharp.text.Paragraph(Trim(dsFill.Tables(I).Rows(0).Item("SecurityTypeName") & ""), font)

                    RX.Colspan = strFields.Length

                    table.AddCell(RX)
                    table.AddCell(New iTextSharp.text.Paragraph(Trim(dsFill.Tables(I).Rows(0).Item("SecurityTypeName") & ""), font))
                    'table.AddCell(RX)

                End If

                AddPDFTableRows(dsFill.Tables(I), I, table)
                PrevSecTypeId = Val(dtNew.Rows(I)("SecurityTypeId").ToString())
            Next

            'table.HorizontalAlignment = Element.ALIGN_LEFT;
            'table.VerticalAlignment = Element.ALIGN_MIDDLE;
            'table.AddCell("yyy", Element.ALIGN_MIDDLE)

            Return table

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Function



    Private Sub AddPDFPTableRows(ByVal dt As DataTable, ByVal intIndex As Int32, ByRef table As PdfPTable)
        Try
            Dim strFields() As String
            Dim strFieldName As String
            Dim strTableName As String
            Dim strColName As String
            Dim strFieldValue As String
            Dim I As Int32
            Dim J As Int32
            Dim dsFieldNames As DataTable
            Dim dtSel As DataTable
            Dim bFlag As Boolean = False
            OpenConn()
            Dim DvSel As DataView = New DataView()
            dtSel = TryCast(Session("TempSecurityTable"), DataTable)
            DvSel = New DataView(dtSel)

            DvSel.RowFilter = String.Empty
            DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value
            'DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value + " And Id=" + Hid_RowNo.Value
            Dim fontN As iTextSharp.text.Font = FontFactory.GetFont("calibri", 10, 0)
            strFields = Split(Hid_SelectedFields.Value, ",")
            For I = 0 To strFields.Length - 1
                'If Val(strFields(I)) <> 0 Then
                Hid_FieldId.Value = Val(strFields(I))
                dsFieldNames = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                With dsFieldNames.Rows(0)
                    strTableName = Trim(.Item("TableName") & "")
                    strFieldName = Trim(.Item("TableField") & "")
                    If strTableName <> "FromPage" Then
                        For J = 0 To dt.Columns.Count - 1
                            strColName = dt.Columns(J).ColumnName
                            If UCase(strFieldName) = UCase(strColName) Then
                                'strFieldValue = IIf(Trim(dt.Rows(0).Item(strColName) & "") = "", "  ", Trim(dt.Rows(0).Item(strColName) & ""))

                                If Trim(dt.Rows(0).Item(strColName) & "") = "" Then
                                    strFieldValue = "N/A"
                                Else
                                    strFieldValue = IIf(Trim(dt.Rows(0).Item(strColName) & "") = "", "  ", Trim(dt.Rows(0).Item(strColName) & ""))
                                End If
                                If IsNumeric(strFieldValue) = True Then
                                    If strColName = "YTMAnn" Or strColName = "YTCAnn" Or strColName = "YTPAnn" Or strColName = "YTMSemi" Or
                                              strColName = "YTCSemi" Or strColName = "YTPSemi" Or strColName = "CouponRate" Then
                                        If objCommon.DecimalFormat4((strFieldValue)) = 0.0 Then
                                            strFieldValue = "N/A"
                                        Else

                                            strFieldValue = (String.Format(objCommon.DecimalFormat4(strFieldValue), "#############0.0000")) & "%"
                                        End If

                                    Else
                                        strFieldValue = objCommon.DecimalFormat4((strFieldValue))
                                    End If
                                    'If strColName = "MaturityDate" Or strColName = "PutDate" Or strColName = "SecurityInfoDate" Or strColName = "IssueDate" Or strColName = "FirstInterestDate" Or strColName = "BookClosureDate" Or strColName = "DMATBookClosureDate" Or strColName = "FirstInterestDate" Or strColName = "CallDate" Then
                                    '    strFieldValue = (String.Format(strFieldValue)), "dd-mmm-yyyy"))
                                    'End If

                                End If
                                Dim pCell As PdfPCell
                                pCell = New PdfPCell(New iTextSharp.text.Paragraph(strFieldValue, fontIn))
                                table.AddCell(pCell)
                                bFlag = True
                                Exit For
                            End If
                        Next
                        If bFlag = False Then
                            table.AddCell("")
                        End If
                        bFlag = False
                    Else
                        'dtSel = TryCast(Session("TempSecurityTable"), DataTable)
                        If strFieldName = "ShowNumber" Then
                            If Hid_ShowNumber.Value = "" Or Hid_ShowNumber.Value = "0" Then
                                strFieldValue = "N/A"
                            Else
                                strFieldValue = Hid_ShowNumber.Value
                            End If
                        Else
                            If Trim(DvSel.Item(0)(strFieldName).ToString() & "") = "" Then
                                strFieldValue = "N/A"
                            Else
                                If strFieldName = "Rating" Then
                                    'strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
                                    'strFieldValue = strFieldValue.Replace("vbTab", vbCrLf)
                                    strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
                                    Dim strFN() As String
                                    Dim C As Integer
                                    Dim D As Integer
                                    If strFieldValue <> "" Then
                                        If strFieldValue.Contains("RATING NOT APPLICABLE") Then
                                            strFieldValue = strFieldValue.Replace("RATING NOT APPLICABLE", "")
                                        End If
                                        strFN = strFieldValue.Split(" ")
                                        If strFN.Length > 2 Then
                                            strFieldValue = ""
                                            For C = 0 To strFN.Length - 1
                                                strFieldValue += strFN(C) & " "
                                                'strFieldValue += System.Environment.NewLine
                                            Next
                                        Else
                                            strFieldValue = Replace(strFieldValue, "!", "")
                                        End If
                                    End If
                                ElseIf strFieldName = "SellingRate" Then
                                    If Trim(DvSel.Item(0)(strFieldName).ToString() & "") <> "" Then
                                        ' strFieldValue = (String.Format(Trim(DvSel.Item(0)(strFieldName).ToString() & ""), "#,##0.0000"))
                                        'strFieldValue = (String.Format(Trim(DvSel.Item(0)(strFieldName).ToString() & ""), "#############0.0000"))
                                        strFieldValue = String.Format("{0:N4}", Double.Parse(Trim(DvSel.Item(0)(strFieldName).ToString() & "")))
                                        '"#############0.00"
                                    End If
                                    'strFieldValue = String.Format ( Trim(DvSel.Item(0)(strFieldName).ToString() & "")
                                Else
                                    If strFieldName = "Id" Then
                                        strFieldValue = intIndex + 1
                                    Else
                                        strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
                                    End If

                                End If
                            End If
                        End If
                        'strHTMLContent.Append(strFieldValue.ToString)
                        If IsNumeric(strFieldValue) = True Then
                            If strFieldName = "YTMAnn" Or strFieldName = "YTCAnn" Or strFieldName = "YTPAnn" Or strFieldName = "YTMSemi" Or
                                          strFieldName = "YTCSemi" Or strFieldName = "YTPSemi" Or strFieldName = "Yield" Then
                                If objCommon.DecimalFormat4(strFieldValue) = 0.0 Then
                                    strFieldValue = "N/A"
                                Else
                                    strFieldValue = String.Format("{0:N4}", Double.Parse(Trim(DvSel.Item(0)(strFieldName).ToString() & ""))) & "%"
                                    'strFieldValue = (String.Format(objCommon.DecimalFormat4(strFieldValue), "#############0.0000"))
                                End If
                            Else
                                If strFieldName <> "ShowNumber" And strFieldName <> "Id" Then
                                    strFieldValue = String.Format("{0:N4}", Double.Parse(Trim(DvSel.Item(0)(strFieldName).ToString() & "")))
                                Else
                                End If

                            End If
                        Else
                            If IIf(strFieldValue = "", " ", strFieldValue) = "" Then
                                strFieldValue = "N/A"
                            ElseIf strFieldName = "Rating" Then
                                strFieldValue = IIf(strFieldValue = "", " ", strFieldValue)
                            Else
                                strFieldValue = IIf(strFieldValue = "", " ", strFieldValue)
                            End If

                        End If


                        Dim pCell As PdfPCell
                        pCell = New PdfPCell(New iTextSharp.text.Paragraph(strFieldValue, fontIn))
                        table.AddCell(pCell)
                    End If
                End With
                'strHTMLContent.Append("</td>".ToString)
                'End If
            Next


            'strHTMLContent.Append("</tr>".ToString)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            'CloseConn()
        End Try
    End Sub






    'Private Sub AddExcelQuotes(ByVal strFields() As String)
    '    Try
    '        Dim I As Int32
    '        Dim ds As DataTable
    '        Dim dtFill As DataTable
    '        Dim PrevSecTypeId As Integer
    '        Dim myRange As Excel.Range
    '        Dim str1 As String

    '        Dim dt As DataTable
    '        Dim dtNew As DataTable

    '        OpenConn()

    '        ' For I = 0 To strFields.Length - 2
    '        For I = 0 To strFields.Length - 1
    '            Hid_FieldId.Value = Val(strFields(I))
    '            ds = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")

    '            'ds = objCommon.GetDataSet(SqlDataSourceFieldId)
    '            intTotWidth = intTotWidth + Val(ds.Rows(0).Item("WordWidth") & "")
    '            'oSheet.Columns.  = Val(ds.Tables(0).Rows(0).Item("WordWidth") & "")
    '            oSheet.Cells(intRowIndex, I + 1) = Trim(ds.Rows(0).Item("FaxField") & "")

    '            myRange = oSheet.Range(Chr(I + 2 + 63) & CStr(intRowIndex))
    '            myRange.Font.Bold = True
    '        Next

    '        dt = CType(Session("TempSecurityTable"), DataTable)
    '        dtNew = dt.Copy()

    '        dtNew.DefaultView.Sort = "TypeFlag Asc, SecurityTypeId Asc"
    '        dtNew = dtNew.DefaultView.ToTable()

    '        For I = 0 To dtNew.Rows.Count - 1


    '            Hid_SecId.Value = dtNew.Rows(I)("SecurityId").ToString()
    '            Hid_SecTypeId.Value = dtNew.Rows(I)("SecurityTypeId").ToString()
    '            Dim strTaxFree As String = "N"
    '            'Hid_SecId.Value = Val(CType(dg_Selected.Rows(I).FindControl("lbl_SecurityId"), Label).Text)
    '            'Hid_SecTypeId.Value = Val(CType(dg_Selected.Rows(I).FindControl("lbl_SecurityTypeId"), Label).Text)
    '            If Val(Hid_SecId.Value) <> 0 Then
    '                intRowIndex = intRowIndex + 1
    '                ' dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")

    '                dtFill = GetTable("ID_FILL_SecurityInfo", Val(Hid_SecId.Value), strTaxFree)

    '                'dsFill.Tables.Add(GetTable("ID_FILL_SecurityInfo", Val(Hid_SecId.Value), strTaxFree))
    '                'dsFill = objCommon.GetDataSet(SqlDataSourceSecurity)

    '                ' If PrevSecTypeId <> Val(CType(dg_Selected.Rows(I).FindControl("lbl_SecurityTypeId"), Label).Text) Then
    '                If PrevSecTypeId <> Val(Hid_SecTypeId.Value) Then
    '                    str1 = CStr(Chr(strFields.Length + 1 + 63)) '& (intRowIndex + dg_Selected.Rows.Count))
    '                    myRange = oSheet.Range("A" & CStr(intRowIndex), str1 & CStr(intRowIndex))
    '                    myRange.MergeCells = True
    '                    myRange.Font.Bold = True
    '                    oSheet.Cells(intRowIndex, 1).Value = Trim(dtFill.Rows(0).Item("SecurityTypeName") & "")

    '                    intRowIndex = intRowIndex + 1
    '                End If
    '                AddExcelRows(dtFill, I, strFields)
    '                PrevSecTypeId = Val(dtNew.Rows(I)("SecurityTypeId").ToString())
    '            End If
    '        Next

    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    Finally
    '        CloseConn()
    '    End Try
    'End Sub
    'Private Sub AddExcelRows(ByVal dt As DataTable, ByVal intIndex As Int32, ByVal strFields() As String)
    '    Try

    '        Dim strFieldName As String
    '        Dim strTableName As String
    '        Dim strColName As String
    '        Dim strFieldValue As String
    '        Dim I As Int32
    '        Dim J As Int32
    '        Dim intColRows As Int16
    '        Dim intRowHeight As Int16
    '        Dim dsFieldNames As DataTable
    '        'Dim dt As DataTable
    '        Dim strTemp As String
    '        OpenConn()

    '        Dim K As Single
    '        Dim dtSel As DataTable
    '        Dim DvSel As DataView = New DataView()

    '        'strFieldValue = Trim(CType(dg_Selected.(intIndex).FindControl("lbl_" & strFieldName), Label).Text & "")
    '        dtSel = TryCast(Session("TempSecurityTable"), DataTable)
    '        DvSel = New DataView(dtSel)

    '        DvSel.RowFilter = String.Empty
    '        DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value

    '        For I = 0 To strFields.Length - 1
    '            Hid_FieldId.Value = Val(strFields(I))

    '            dsFieldNames = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
    '            'dsFieldNames = objCommon.GetDataSet(SqlDataSourceFieldId)
    '            With dsFieldNames.Rows(0)
    '                strTableName = Trim(.Item("TableName") & "")
    '                strFieldName = Trim(.Item("TableField") & "")
    '                If strFieldName = "Rate" Then intRateIndex = I
    '                If strTableName <> "FromPage" Then
    '                    For J = 0 To dt.Columns.Count - 1
    '                        strColName = dt.Columns(J).ColumnName
    '                        If UCase(strFieldName) = UCase(strColName) Then
    '                            'tbl.Cell(intIndex + 2, I + 1).Range.Text = Trim(dt.Rows(0).Item(strColName) & "")
    '                            strTemp = ""
    '                            If strColName = "IPDates" Then strTemp = " "
    '                            'If LCase(strFieldName) = "calldatei" Then
    '                            'strColName = IIf(Trim(dt.Rows(0).Item("CallDateI").ToString & "") = "", "PutDateI", "CallDateI")
    '                            'End If
    '                            oSheet.Cells(intRowIndex, I + 1).Value = strTemp & Trim(dt.Rows(0).Item(strColName) & "")
    '                            Exit For
    '                        End If
    '                    Next


    '                Else
    '                    intColRows = 0
    '                    strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
    '                    'strFieldValue = Trim(dtSel.Rows(intIndex).Item(strFieldName) & "")

    '                    If InStr(strFieldValue, "!") > 1 Then
    '                        For K = 1 To Len(strFieldValue)
    '                            If Mid$(strFieldValue, K, 1) = "!" Then
    '                                intColRows = intColRows + 1
    '                            End If
    '                        Next
    '                        strFieldValue = Replace(Left(strFieldValue, Len(strFieldValue) - 1), "!", Chr(10))
    '                        If intRowHeight < (13 * intColRows) Then intRowHeight = 13 * intColRows
    '                    Else
    '                        strFieldValue = Replace(strFieldValue, "!", "")
    '                    End If
    '                    'strFieldValue = Replace(strFieldValue, "!", Chr(10))
    '                    strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
    '                    oSheet.Cells(intRowIndex, I + 1).Value = strFieldValue
    '                End If
    '            End With
    '        Next
    '        If intRowHeight = 0 Then intRowHeight = 15
    '        oSheet.Rows(intRowIndex).RowHeight = intRowHeight
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    Finally
    '        'CloseConn()
    '    End Try

    'End Sub

    Protected Sub btn_AddTempCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddTempCustomer.Click
        lbl_Msg.Text = ""
        FillTempCustomer()
    End Sub

    Private Sub FillTempCustomer()
        Try
            Dim dt1 As DataTable
            Dim dt2 As DataTable
            Dim dt3 As DataTable
            dt2 = (Session("TempCustTable"))
            If TypeOf Session("CustomerContectTable") Is DataTable Then
                dt1 = TryCast(Session("CustomerContectTable"), DataTable)
                dt3 = mergeDTs(dt2, dt1)
            Else
                dt3 = dt2
            End If
            Session("CustomerContectTable") = dt3
            dg_Customer.DataSource = dt3
            dg_Customer.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btn_UpdateStock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_UpdateStock.Click
        lbl_Msg.Text = ""
        UpdateYield()
        txt_basispoint.Text = ""
        Hid_BasisPoint.Value = ""
    End Sub

    Protected Sub btn_UpdateGridQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_UpdateGridQuote.Click
        lbl_Msg.Text = ""
        UpdateGridYield()
        txt_basispoint.Text = ""
        Hid_BasisPoint.Value = ""
    End Sub

    Public Sub UpdateGridYield()
        Try
            Dim dt As DataTable
            Dim FaxQuoteId As Int32
            Dim Margin As String
            Dim SecurityTypeFlag As Char
            Dim IsMaster As String
            Dim CreditRating As String
            Dim FaceValue As String
            strSemiAnnFlag = Hid_AnnSemiFlag.Value
            FillSettDate()
            Dim strRate As String
            Dim decBrokerBasisPoint As Decimal = 0
            If (rbl_SelectCustomerBroker.SelectedValue = "B") Then
                Dim strBrokerbasisPoint As String = CType(dg_Broker.Rows(0).FindControl("lbl_BasisPoint"), Label).Text
                If strBrokerbasisPoint <> "" Then
                    decBrokerBasisPoint = Convert.ToDecimal(strBrokerbasisPoint)
                End If
            End If


            If decBrokerBasisPoint > 0 Then
                Hid_BasisPoint.Value = decBrokerBasisPoint / 100
            End If
            Dim strbasispoint As String = Hid_BasisPoint.Value
            If (strbasispoint <> "") Then
                decbasispoint = Convert.ToDecimal(strbasispoint)
            Else
                decbasispoint = 0
            End If

            Dim dtData As New DataTable()
            dtData.Columns.Add("FaxQuoteDetailId", GetType(Int32))
            dtData.Columns.Add("SecurityId", GetType(Int32))
            dtData.Columns.Add("YTMAnn", GetType(Decimal))
            dtData.Columns.Add("YTMSemi", GetType(Decimal))
            dtData.Columns.Add("YTCSemi", GetType(Decimal))
            dtData.Columns.Add("YTCAnn", GetType(Decimal))
            dtData.Columns.Add("YTPAnn", GetType(Decimal))
            dtData.Columns.Add("YTPSemi", GetType(Decimal))
            dtData.Columns.Add("Rate", GetType(Decimal))
            dtData.Columns.Add("OriginalSellingRate", GetType(Decimal))

            '''''''''''' For Adding New Security in Already added quotes

            Dim dtNewSecurity As New DataTable()
            dtNewSecurity.Columns.Add("FaxQuoteId", GetType(Int32))
            dtNewSecurity.Columns.Add("FaxQuoteDetailId", GetType(Int32))
            dtNewSecurity.Columns.Add("SecurityId", GetType(Int32))
            dtNewSecurity.Columns.Add("SecurityName", GetType(String))
            dtNewSecurity.Columns.Add("ISINNo", GetType(String))
            dtNewSecurity.Columns.Add("SecurityTypeName", GetType(String))
            dtNewSecurity.Columns.Add("SellingRate", GetType(String))
            dtNewSecurity.Columns.Add("ShowNumber", GetType(String))
            dtNewSecurity.Columns.Add("Rating", GetType(String))
            dtNewSecurity.Columns.Add("PhysicalDMAT", GetType(Char))
            dtNewSecurity.Columns.Add("Yield", GetType(String))
            dtNewSecurity.Columns.Add("YTMAnn", GetType(String))
            dtNewSecurity.Columns.Add("YTMSemi", GetType(String))
            dtNewSecurity.Columns.Add("YTCSemi", GetType(String))
            dtNewSecurity.Columns.Add("YTCAnn", GetType(String))
            dtNewSecurity.Columns.Add("YTPAnn", GetType(String))
            dtNewSecurity.Columns.Add("YTPSemi", GetType(String))
            dtNewSecurity.Columns.Add("Semi_Ann_Flag", GetType(Char))
            dtNewSecurity.Columns.Add("CombineIPMat", GetType(Boolean))
            dtNewSecurity.Columns.Add("Rate_Actual_Flag", GetType(Char))
            dtNewSecurity.Columns.Add("Equal_Actual_Flag", GetType(Char))
            dtNewSecurity.Columns.Add("IntDays", GetType(Int32))
            dtNewSecurity.Columns.Add("FirstYrAllYr", GetType(Char))
            dtNewSecurity.Columns.Add("TypeFlag", GetType(Char))
            dtNewSecurity.Columns.Add("SecurityTypeId", GetType(Int32))
            dtNewSecurity.Columns.Add("IPCalc", GetType(String))
            dtNewSecurity.Columns.Add("RateActual", GetType(String))
            dtNewSecurity.Columns.Add("OriginalSellingRate", GetType(Decimal))
            dtNewSecurity.Columns.Add("NatureOFInstrument", GetType(String))
            dtNewSecurity.Columns.Add("CallDate", GetType(String))
            dtNewSecurity.Columns.Add("CreditRating", GetType(String))
            dtNewSecurity.Columns.Add("Category", GetType(String))
            dtNewSecurity.Columns.Add("SubCategory", GetType(String))
            dtNewSecurity.Columns.Add("SecuredUnsec", GetType(String))
            dtNewSecurity.Columns.Add("Name of PD", GetType(String))
            dtNewSecurity.Columns.Add("CallFlag", GetType(String))
            dtNewSecurity.Columns.Add("Id", GetType(Integer))
            dtNewSecurity.Columns.Add("YieldPriceType", GetType(String))
            dtNewSecurity.Columns.Add("OrderId", GetType(Integer))
            dtNewSecurity.Columns.Add("Remark", GetType(String))

            OpenConn()
            dt = Session("SavedFaxQuotes")
            Dim stockcount As Integer
            stockcount = dg_Selected.Rows.Count
            If stockcount > 0 Then
                For K As Int32 = 0 To dg_Selected.Rows.Count - 1
                    lblYieldPriceType = ""
                    Dim strSecurityNature As String = ""
                    SecurityTypeFlag = CType(dg_Selected.Rows(K).FindControl("lbl_TypeFlag"), Label).Text
                    If CType(dg_Selected.Rows(K).FindControl("lbl_FaxQuoteDetailId"), Label).Text = "" Then
                        FaxQuoteId = 0
                    Else
                        FaxQuoteId = Convert.ToInt32(CType(dg_Selected.Rows(K).FindControl("lbl_FaxQuoteDetailId"), Label).Text)
                    End If

                    If CType(dg_Selected.Rows(K).FindControl("lbl_SecurityTypeId"), Label).Text = "" Then
                        intSecurityTypeId = 0
                    Else
                        intSecurityTypeId = Convert.ToInt32(CType(dg_Selected.Rows(K).FindControl("lbl_SecurityTypeId"), Label).Text)
                    End If
                    Dim strSemiAnn As String = CType(dg_Selected.Rows(K).FindControl("lbl_Semi_Ann_Flag"), Label).Text
                    strSemiAnnFlag = CType(dg_Selected.Rows(K).FindControl("lbl_Semi_Ann_Flag"), Label).Text
                    lblCallDate = CType(dg_Selected.Rows(K).FindControl("lbl_CallDate"), Label).Text
                    lblCategory = CType(dg_Selected.Rows(K).FindControl("lbl_Category"), Label).Text
                    lblSubCategory = CType(dg_Selected.Rows(K).FindControl("lbl_SubCategory"), Label).Text
                    lblSecuredUnsec = CType(dg_Selected.Rows(K).FindControl("lbl_SecuredUnsec"), Label).Text
                    lblNameOFPD = CType(dg_Selected.Rows(K).FindControl("lbl_NameOFPD"), Label).Text
                    lblCallFlag = CType(dg_Selected.Rows(K).FindControl("lbl_CallFlag"), Label).Text
                    lblId = CType(dg_Selected.Rows(K).FindControl("lbl_RowNumber"), Label).Text
                    Hid_Rate.Value = CType(dg_Selected.Rows(K).FindControl("hdnRate"), HiddenField).Value
                    strRate = Hid_Rate.Value
                    If Val(Hid_Rate.Value) > 0 Then
                        decRate = Convert.ToDecimal(Hid_Rate.Value) + decbasispoint
                        Hid_Rate.Value = decRate
                        strRate = Hid_Rate.Value
                        CType(dg_Selected.Rows(K).FindControl("lbl_Rate"), TextBox).Text = Convert.ToString(decRate)
                    End If
                    If decbasispoint > 0 Then
                        decYield = 0
                        decYTMSemi = 0
                        decYTC = 0
                        decYTCSem = 0
                    Else
                        decYield = Convert.ToDecimal(CType(dg_Selected.Rows(K).FindControl("txt_Yield"), TextBox).Text)
                        decYTC = Convert.ToDecimal(CType(dg_Selected.Rows(K).FindControl("txt_YTC"), TextBox).Text)
                        Dim ytcsemi As String = CType(dg_Selected.Rows(K).FindControl("txt_YTCSemi"), TextBox).Text
                        If ytcsemi = "" Or ytcsemi = "0.0000" Then
                            decYTCSem = 0.0
                        Else
                            decYTCSem = Convert.ToDecimal(CType(dg_Selected.Rows(K).FindControl("txt_YTCSemi"), TextBox).Text)
                        End If

                        Dim ytmsemi As String = CType(dg_Selected.Rows(K).FindControl("txt_YTMSemi"), TextBox).Text
                        If ytmsemi = "" Or ytmsemi = "0.0000" Then
                            decYTMSemi = 0.0
                        Else
                            decYTMSemi = Convert.ToDecimal(CType(dg_Selected.Rows(K).FindControl("txt_YTMSemi"), TextBox).Text)
                        End If
                    End If

                    Hid_YTMDate.Value = ytmdt
                    RateActualFlag = CType(dg_Selected.Rows(K).FindControl("lbl_Rate_Actual_Flag"), Label).Text
                    'TaxFree = CType(dg_Selected.Rows(K).FindControl("rdo_TaxFree"), RadioButtonList).SelectedValue
                    PhysicalDematFlag = "D"
                    SecurityName = CType(dg_Selected.Rows(K).FindControl("txt_SecurityName"), Label).Text
                    ISINNo = CType(dg_Selected.Rows(K).FindControl("txt_ISINNo"), Label).Text
                    Dim combIPMat As String = CType(dg_Selected.Rows(K).FindControl("lbl_CombineIPMat"), Label).Text
                    CombineIPMat = CType(dg_Selected.Rows(K).FindControl("lbl_CombineIPMat"), Label).Text
                    EqualActualFlag = CType(dg_Selected.Rows(K).FindControl("lbl_Equal_Actual_Flag"), Label).Text
                    If CType(dg_Selected.Rows(K).FindControl("lbl_IntDays"), Label).Text = "" Then
                        intDays = 0
                    Else
                        intDays = CType(dg_Selected.Rows(K).FindControl("lbl_IntDays"), Label).Text
                    End If
                    If CType(dg_Selected.Rows(K).FindControl("lbl_LotSize"), TextBox).Text = "" Then
                        Quantity = ""
                    Else
                        Quantity = Convert.ToString(CType(dg_Selected.Rows(K).FindControl("lbl_LotSize"), TextBox).Text)
                    End If 'PerpetualFlag
                    PerpetualFlag = CType(dg_Selected.Rows(K).FindControl("lbl_PerpetualFlag"), Label).Text
                    CreditRating = CType(dg_Selected.Rows(K).FindControl("lbl_CreditRating"), Label).Text
                    'FaceValue = CType(dg_Selected.Rows(K).FindControl("lbl_FaceValue"), Label).Text
                    SecurityTypeName = CType(dg_Selected.Rows(K).FindControl("lbl_SecurityTypeName"), Label).Text
                    RatingRemark = CType(dg_Selected.Rows(K).FindControl("txt_RatingRemark"), TextBox).Text
                    intDays = CType(dg_Selected.Rows(K).FindControl("lbl_IntDays"), Label).Text
                    FirstAllYr = CType(dg_Selected.Rows(K).FindControl("lbl_FirstYrAllYr"), Label).Text
                    lblYieldPriceType = CType(dg_Selected.Rows(K).FindControl("lbl_YieldPriceType"), Label).Text
                    Dim strOrderId As String = CType(dg_Selected.Rows(K).FindControl("lbl_OrderId"), Label).Text
                    If strOrderId <> "" Then
                        OrderId = Convert.ToInt32(strOrderId)
                    End If
                    Remark = CType(dg_Selected.Rows(K).FindControl("lbl_SecurityRemark"), Label).Text
                    Hid_MatDate.Value = ""
                    Hid_MatAmt.Value = ""
                    Hid_CoupDate.Value = ""
                    Hid_CoupRate.Value = ""
                    Hid_CallDate.Value = ""
                    Hid_CallAmt.Value = ""
                    Hid_PutDate.Value = ""
                    Hid_PutAmt.Value = ""
                    Hid_SecurityId.Value = CType(dg_Selected.Rows(K).FindControl("lbl_SecurityId"), Label).Text
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
                    ' Dim YieldType As Char ' = dt.Rows(K).Item("YTM_XIRR_MMY_Flag")

                    If Hid_RateAmtFlag.Value = "A" Then
                        For M As Int32 = 0 To CoupRate.Length - 1
                            CoupRate(M) = (CoupRate(M) / decFaceValue) * 100
                        Next
                    End If

                    Hid_YTMAnn.Value = 0
                    Hid_YTMSemi.Value = 0
                    Hid_YTCAnn.Value = 0
                    Hid_YTCSemi.Value = 0
                    Hid_YTPAnn.Value = 0
                    Hid_YTPSemi.Value = 0

                    Select Case SecurityTypeFlag
                        Case "G"
                            If CheckSecurity() = False Or Val(Hid_Frequency.Value) = 0 Then
                                FillXIRROptions_New()
                            Else
                                FillYieldOptions()
                            End If
                        Case "N"
                            FillXIRROptions_New()
                    End Select

                    'If strSemiAnnFlag = "A" Then
                    '    Hid_YTMSemi.Value = 0.0
                    '    Hid_YTCSemi.Value = 0.0
                    '    Hid_YTPSemi.Value = 0.0
                    'End If

                    If CCount = 1 Then
                        If decYield <> 0 Then
                            Hid_YTMAnn.Value = decYield
                        End If
                    End If
                    If CCount = 1 Then
                        If decYTC <> 0 Then
                            Hid_YTCAnn.Value = decYTC
                        End If
                    End If
                    If CCount = 1 Then
                        If decYTMSemi <> 0 Then
                            Hid_YTMSemi.Value = decYTMSemi
                        End If
                    End If
                    If CCount = 1 Then
                        If decYTCSem <> 0 Then
                            Hid_YTCSemi.Value = decYTCSem
                        End If
                    End If
                    If CCount = 1 Then
                        If decRate <> 0 Then
                            Hid_Rate.Value = strRate
                        End If
                    End If
                    Dim datarow As DataRow = dtNewSecurity.NewRow()
                    datarow("FaxQuoteId") = Val(Hid_FaxQuoteId.Value)
                    datarow("FaxQuoteDetailId") = 0
                    datarow("SecurityId") = SecurityId
                    datarow("SecurityTypeId") = intSecurityTypeId
                    datarow("SecurityName") = SecurityName
                    datarow("ISINNo") = ISINNo
                    'datarow("SellingRate") = Val(Hid_Rate.Value)
                    datarow("SellingRate") = Hid_Rate.Value
                    datarow("ShowNumber") = Quantity
                    datarow("Rating") = RatingRemark
                    datarow("PhysicalDMAT") = PhysicalDematFlag
                    'datarow("Yield") = Val(Hid_YTMAnn.Value)
                    'datarow("YTMAnn") = Val(Hid_YTMAnn.Value)
                    datarow("Yield") = Trim(Hid_YTMAnn.Value)
                    datarow("YTMAnn") = Trim(Hid_YTMAnn.Value)
                    datarow("YTMSemi") = Trim(Hid_YTMSemi.Value)
                    datarow("YTCSemi") = Trim(Hid_YTCSemi.Value)
                    datarow("YTCAnn") = Trim(Hid_YTCAnn.Value)
                    datarow("YTPAnn") = Trim(Hid_YTPAnn.Value)
                    datarow("YTPSemi") = Trim(Hid_YTPSemi.Value)
                    datarow("Semi_Ann_Flag") = strSemiAnn
                    datarow("CombineIPMat") = CombineIPMat
                    datarow("Rate_Actual_Flag") = RateActualFlag
                    datarow("Equal_Actual_Flag") = EqualActualFlag
                    datarow("IntDays") = intDays
                    datarow("FirstYrAllYr") = FirstAllYr
                    datarow("TypeFlag") = SecurityTypeFlag
                    datarow("SecurityTypeName") = SecurityTypeName
                    datarow("IPCalc") = SecurityTypeFlag
                    datarow("RateActual") = SecurityTypeFlag
                    datarow("CreditRating") = CreditRating
                    datarow("OriginalSellingRate") = Val(Hid_Rate.Value)
                    datarow("NatureOFInstrument") = PerpetualFlag
                    datarow("CallDate") = lblCallDate
                    datarow("Category") = lblCategory
                    datarow("SubCategory") = lblSubCategory
                    datarow("SecuredUnsec") = lblSecuredUnsec
                    datarow("Name of PD") = lblNameOFPD
                    datarow("CallFlag") = lblCallFlag
                    datarow("Id") = lblId
                    datarow("YieldPriceType") = lblYieldPriceType
                    datarow("OrderId") = OrderId
                    datarow("Remark") = Remark.Replace("'", "")
                    dtNewSecurity.Rows.Add(datarow)
                    CCount = 0

                Next

                Session("TempSecurityTable") = dtNewSecurity
                Session("SecurityMaster") = dtNewSecurity

                dg_Selected.DataSource = dtNewSecurity
                dg_Selected.DataBind()

                lbl_Msg.Text = "Stock Updated Successfully"
                '  BindOpenQuoteSecurity()

            Else
                lbl_Msg.Text = "No Stock To update!!!!"
            End If
        Catch ex As Exception
            Dim strError As String = ex.Message.ToString()
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function CheckSecurity() As Boolean
        Try
            Dim I As Int16
            Dim strTypes() As String
            'Dim dsInfo As DataSet
            Dim dt As DataTable
            strTypes = Split("M!I!C!P", "!")
            For I = 0 To strTypes.Length - 1
                Hid_TypeFlag.Value = strTypes(I)
                'dsInfo = objCommon.GetDataSet(SqlDataSourceSecurityInfo)
                OpenConn()
                dt = objCommon.FillDataTable(sqlConn, "ID_Check_SecurityInfo", SecurityId, "SecurityId", , Hid_TypeFlag.Value, "TypeFlag")
                'dsInfo = objCommon.FillDetailsGrid(dg_Maturity, "ID_Check_SecurityInfo", "SecurityId", Val(ViewState("Id") & ""), "M", "TypeFlag")

                If Val(dt.Rows(0).Item(0) & "") > 1 Then
                    Return False
                End If
            Next
            Return True
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CheckSecurity", "Error in CheckSecurity", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Function

    Public Sub UpdateYield()
        Try
            Dim dt As DataTable
            Dim FaxQuoteId As Int32
            Dim SecurityTypeFlag As Char
            Dim IsMaster As String
            Dim CreditRating As String
            Dim FaceValue As String

            FillSettDate()
            'ytmdt = ytmdt
            ''lblCalcDate.Text = "Calc Date: " + ytmdt
            'CalcDate = ytmdt
            'txt_CalcDate.Text = objCommon.DateFormat(ytmdt)
            Dim dtData As New DataTable()
            dtData.Columns.Add("FaxQuoteDetailId", GetType(Int32))
            dtData.Columns.Add("SecurityId", GetType(Int32))
            dtData.Columns.Add("YTMAnn", GetType(Decimal))
            dtData.Columns.Add("YTMSemi", GetType(Decimal))
            dtData.Columns.Add("YTCSemi", GetType(Decimal))
            dtData.Columns.Add("YTCAnn", GetType(Decimal))
            dtData.Columns.Add("YTPAnn", GetType(Decimal))
            dtData.Columns.Add("YTPSemi", GetType(Decimal))
            dtData.Columns.Add("Rate", GetType(Decimal))
            dtData.Columns.Add("Quantity", GetType(String))

            '''''''''''' For Adding New Security in Already added quotes

            Dim dtNewSecurity As New DataTable()
            dtNewSecurity.Columns.Add("FaxQuoteId", GetType(Int32))
            dtNewSecurity.Columns.Add("SecurityId", GetType(Int32))
            dtNewSecurity.Columns.Add("SecurityName", GetType(String))
            dtNewSecurity.Columns.Add("Rate", GetType(Decimal))
            dtNewSecurity.Columns.Add("Quantity", GetType(Int32))
            dtNewSecurity.Columns.Add("RatingRemark", GetType(String))

            dtNewSecurity.Columns.Add("Physical_DMAT_Flag", GetType(Char))
            'dtNewSecurity.Columns.Add("IPCalc", GetType(Char))
            'dtNewSecurity.Columns.Add("RateActual", GetType(Char))

            dtNewSecurity.Columns.Add("YTMAnn", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTMSemi", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTCSemi", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTCAnn", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTPAnn", GetType(Decimal))
            dtNewSecurity.Columns.Add("YTPSemi", GetType(Decimal))

            dtNewSecurity.Columns.Add("Semi_Ann_Flag", GetType(Char))
            dtNewSecurity.Columns.Add("CombineIPMat", GetType(Boolean))
            dtNewSecurity.Columns.Add("Rate_Actual_Flag", GetType(Char))
            dtNewSecurity.Columns.Add("Equal_Actual_Flag", GetType(Char))

            dtNewSecurity.Columns.Add("IntDays", GetType(Int32))
            dtNewSecurity.Columns.Add("FirstYrAllYr", GetType(Char))

            'dtNewSecurity.Columns.Add("IsMaster", GetType(String))
            dtNewSecurity.Columns.Add("CreditRating", GetType(String))

            Dim dtQty As New DataTable()
            dtQty.Columns.Add("FaxQuoteId", GetType(Int32))
            dtQty.Columns.Add("Qty", GetType(String))

            OpenConn()
            dt = Session("SavedFaxQuotes")
            Dim stockcount As Integer
            stockcount = dg_Selected.Rows.Count
            If stockcount > 0 Then
                For K As Int32 = 0 To dg_Selected.Rows.Count - 1
                    Dim strSecurityNature As String = ""
                    SecurityTypeFlag = CType(dg_Selected.Rows(K).FindControl("lbl_TypeFlag"), Label).Text
                    If CType(dg_Selected.Rows(K).FindControl("lbl_FaxQuoteDetailId"), Label).Text = "" Then
                        FaxQuoteId = 0
                    Else
                        FaxQuoteId = Convert.ToInt32(CType(dg_Selected.Rows(K).FindControl("lbl_FaxQuoteDetailId"), Label).Text)
                    End If
                    Dim strSemiAnn As String = CType(dg_Selected.Rows(K).FindControl("lbl_Semi_Ann_Flag"), Label).Text
                    strSemiAnnFlag = CType(dg_Selected.Rows(K).FindControl("lbl_Semi_Ann_Flag"), Label).Text
                    Hid_AnnSemiFlag.Value = strSemiAnnFlag
                    'Hid_Rate.Value = CType(dg_Selected.Rows(K).FindControl("lbl_Rate"), TextBox).Text
                    Hid_Rate.Value = CType(dg_Selected.Rows(K).FindControl("hdnRate"), HiddenField).Value
                    decYield = Convert.ToDecimal(CType(dg_Selected.Rows(K).FindControl("txt_Yield"), TextBox).Text)
                    decYTC = Convert.ToDecimal(CType(dg_Selected.Rows(K).FindControl("txt_YTC"), TextBox).Text)
                    decRate = Convert.ToDecimal(Hid_Rate.Value)
                    Dim ytcsemi As String = CType(dg_Selected.Rows(K).FindControl("txt_YTCSemi"), TextBox).Text
                    If ytcsemi = "" Or ytcsemi = "0.0000" Then
                        decYTCSem = 0.0
                    Else
                        decYTCSem = Convert.ToDecimal(CType(dg_Selected.Rows(K).FindControl("txt_YTCSemi"), TextBox).Text)
                    End If

                    Dim ytmsemi As String = CType(dg_Selected.Rows(K).FindControl("txt_YTMSemi"), TextBox).Text
                    If ytmsemi = "" Or ytmsemi = "0.0000" Then
                        decYTMSemi = 0.0
                    Else
                        decYTMSemi = Convert.ToDecimal(CType(dg_Selected.Rows(K).FindControl("txt_YTMSemi"), TextBox).Text)
                    End If
                    Hid_YTMDate.Value = ytmdt
                    RateActualFlag = CType(dg_Selected.Rows(K).FindControl("lbl_Rate_Actual_Flag"), Label).Text
                    'TaxFree = CType(dg_Selected.Rows(K).FindControl("rdo_TaxFree"), RadioButtonList).SelectedValue
                    PhysicalDematFlag = "D"
                    SecurityName = CType(dg_Selected.Rows(K).FindControl("txt_SecurityName"), Label).Text
                    CombineIPMat = CType(dg_Selected.Rows(K).FindControl("lbl_CombineIPMat"), Label).Text
                    EqualActualFlag = CType(dg_Selected.Rows(K).FindControl("lbl_Equal_Actual_Flag"), Label).Text
                    If CType(dg_Selected.Rows(K).FindControl("lbl_IntDays"), Label).Text = "" Then
                        intDays = 0
                    Else
                        intDays = CType(dg_Selected.Rows(K).FindControl("lbl_IntDays"), Label).Text
                    End If
                    If CType(dg_Selected.Rows(K).FindControl("lbl_LotSize"), TextBox).Text = "" Then
                        Quantity = "0"
                    Else
                        Quantity = Convert.ToString(CType(dg_Selected.Rows(K).FindControl("lbl_LotSize"), TextBox).Text)
                    End If
                    PerpetualFlag = CType(dg_Selected.Rows(K).FindControl("lbl_PerpetualFlag"), Label).Text
                    IsMaster = "1"
                    CreditRating = CType(dg_Selected.Rows(K).FindControl("lbl_CreditRating"), Label).Text
                    RatingRemark = CType(dg_Selected.Rows(K).FindControl("txt_RatingRemark"), TextBox).Text
                    intDays = CType(dg_Selected.Rows(K).FindControl("lbl_IntDays"), Label).Text
                    FirstAllYr = CType(dg_Selected.Rows(K).FindControl("lbl_FirstYrAllYr"), Label).Text
                    Hid_MatDate.Value = ""
                    Hid_MatAmt.Value = ""
                    Hid_CoupDate.Value = ""
                    Hid_CoupRate.Value = ""
                    Hid_CallDate.Value = ""
                    Hid_CallAmt.Value = ""
                    Hid_PutDate.Value = ""
                    Hid_PutAmt.Value = ""
                    Hid_SecurityId.Value = CType(dg_Selected.Rows(K).FindControl("lbl_SecurityId"), Label).Text
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
                            'FillXIRROptions()
                            FillXIRROptions_New()
                    End Select

                    'If strSemiAnnFlag = "A" Then
                    '    Hid_YTMSemi.Value = 0.0
                    '    Hid_YTCSemi.Value = 0.0
                    '    Hid_YTPSemi.Value = 0.0
                    'End If

                    If FaxQuoteId = 0 Then
                        Dim datarow As DataRow = dtNewSecurity.NewRow()
                        datarow("FaxQuoteId") = Val(Hid_FaxQuoteId.Value)
                        datarow("SecurityId") = SecurityId
                        datarow("SecurityName") = SecurityName
                        datarow("Rate") = Val(Hid_Rate.Value)
                        datarow("Quantity") = Quantity
                        datarow("RatingRemark") = RatingRemark
                        datarow("Physical_DMAT_Flag") = PhysicalDematFlag
                        datarow("YTMAnn") = Val(Hid_YTMAnn.Value)
                        datarow("YTMSemi") = Val(Hid_YTMSemi.Value)
                        datarow("YTCSemi") = Val(Hid_YTCSemi.Value)
                        datarow("YTCAnn") = Val(Hid_YTCAnn.Value)
                        datarow("YTPAnn") = Val(Hid_YTPAnn.Value)
                        datarow("YTPSemi") = Val(Hid_YTPSemi.Value)
                        datarow("Semi_Ann_Flag") = strSemiAnn
                        datarow("CombineIPMat") = CombineIPMat
                        datarow("Rate_Actual_Flag") = RateActualFlag
                        datarow("Equal_Actual_Flag") = EqualActualFlag
                        datarow("IntDays") = intDays
                        datarow("FirstYrAllYr") = FirstAllYr
                        'datarow("IsMaster") = IsMaster
                        datarow("CreditRating") = CreditRating
                        dtNewSecurity.Rows.Add(datarow)
                    Else
                        Dim datarow As DataRow = dtData.NewRow()
                        datarow("FaxQuoteDetailId") = FaxQuoteId
                        datarow("SecurityId") = Val(Hid_SecurityId.Value)
                        datarow("YTMAnn") = Val(Hid_YTMAnn.Value)
                        datarow("YTMSemi") = Val(Hid_YTMSemi.Value)
                        datarow("YTCSemi") = Val(Hid_YTCSemi.Value)
                        datarow("YTCAnn") = Val(Hid_YTCAnn.Value)
                        datarow("YTPAnn") = Val(Hid_YTPAnn.Value)
                        datarow("YTPSemi") = Val(Hid_YTPSemi.Value)
                        datarow("Rate") = Val(Hid_Rate.Value)
                        datarow("Quantity") = Quantity
                        dtData.Rows.Add(datarow)
                    End If
                    CCount = 0
                Next
                Dim sqlComm As New SqlCommand
                OpenConn()
                If dtData.Rows.Count > 0 Or dtNewSecurity.Rows.Count Or dtQty.Rows.Count > 0 Then
                    sqlComm.CommandType = CommandType.StoredProcedure
                    sqlComm.Connection = sqlConn
                    sqlComm.CommandText = "ID_UPDATE_MultiFaxQuoteUpdate"
                    sqlComm.Parameters.Clear()
                    'sqlComm.Parameters.Add("@UpdateQty", SqlDbType.Structured).Value = dtQty
                    sqlComm.Parameters.Add("@UpdateQuote", SqlDbType.Structured).Value = dtData
                    sqlComm.Parameters.Add("@InsertQuote", SqlDbType.Structured).Value = dtNewSecurity
                    sqlComm.ExecuteNonQuery()

                End If
                lbl_Msg.Text = "Stock Updated Successfully"
                BindOpenQuoteSecurity()

            Else
                lbl_Msg.Text = "No Stock To update!!!!"
            End If
        Catch ex As Exception
        Finally
            CloseConn()
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
                If lblYieldPriceType = "" Then
                    lblYieldPriceType = "R"
                Else
                    If lblYieldPriceType = "R" Then
                        CCount = CCount + 1
                    End If
                End If
            End If
            If decYield <> 0.0 Then
                If lblYieldPriceType = "" Then
                    lblYieldPriceType = "R"
                Else
                    If lblYieldPriceType = "R" Then
                        CCount = CCount + 1
                    End If
                End If
            End If
            If decRate <> 0.0 Then
                If lblYieldPriceType = "" Then
                    lblYieldPriceType = "Y"
                Else
                    If lblYieldPriceType = "Y" Then
                        CCount = CCount + 1
                    End If
                End If
            End If
            If decYTCSem <> 0.0 Then
                If lblYieldPriceType = "" Then
                    lblYieldPriceType = "R"
                Else
                    If lblYieldPriceType = "R" Then
                        CCount = CCount + 1
                    End If
                End If
            End If
            If decYTMSemi <> 0.0 Then
                If lblYieldPriceType = "" Then
                    lblYieldPriceType = "R"
                Else
                    If lblYieldPriceType = "R" Then
                        CCount = CCount + 1
                    End If
                End If
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
                If PerpetualFlag = "P" Then
                    If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                        Hid_YTCAnn.Value = decYTC
                        strSemiAnnFlag = "A"
                    Else
                        'txt_YTCSemi.Text = objCommon.DecimalFormat4(txt_YTCSemi.Text)
                        Hid_YTCSemi.Value = decYTCSem
                        strSemiAnnFlag = "S"
                    End If
                    dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTCAnn.Value), Val(Hid_YTCSemi.Value))
                    GlobalFuns.CalculateYield(txt_CalcDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                    datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "C", dblResult, strSemiAnnFlag)
                Else
                    dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTMAnn.Value), Val(Hid_YTMSemi.Value))
                    GlobalFuns.CalculateYield(txt_CalcDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                   datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")
                End If
            ElseIf decRate = 0.0 Then
                If (decYTC <> 0 Or decYTCSem <> 0) And decYield = 0 And decYTMSemi = 0 Then
                    If (decYTC = 0 And decYTCSem <> 0) Then
                        Hid_YTCSemi.Value = decYTCSem
                        strSemiAnnFlag = "S"
                    ElseIf (decYTC > 0 And decYTCSem = 0) Then
                        Hid_YTCAnn.Value = decYTC
                        strSemiAnnFlag = "A"
                    End If

                    dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTCAnn.Value), Val(Hid_YTCSemi.Value))
                    GlobalFuns.CalculateYield(txt_CalcDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                            datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "C", dblResult, strSemiAnnFlag)

                    Hid_Rate.Value = objCommon.DecimalFormat4(decMarketRate) + decbasispoint
                    GlobalFuns.CalculateYield(txt_CalcDate.Text, decFaceValue, decMarketRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                   datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")
                    'decYield = objCommon.DecimalFormat4(decYield)
                    'dblYTMAnn = objCommon.DecimalFormat4(decYield)
                    'dblYTMSemi = objCommon.DecimalFormat4(decYTMSemi)
                    'dblYTCSemi = objCommon.DecimalFormat4(decYTCSem)
                    'dblYTCAnn = objCommon.DecimalFormat4(decYTC)


                Else

                    If (decYield = 0 And decYTMSemi <> 0) Then
                        Hid_YTMSemi.Value = decYTMSemi
                        strSemiAnnFlag = "S"
                    ElseIf (decYield > 0 And decYTMSemi = 0) Then
                        Hid_YTMAnn.Value = decYield
                        strSemiAnnFlag = "A"
                    End If

                    dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTMAnn.Value), Val(Hid_YTMSemi.Value))
                    GlobalFuns.CalculateYield(txt_CalcDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                   datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "M", dblResult, strSemiAnnFlag)

                    Hid_Rate.Value = objCommon.DecimalFormat4(decMarketRate) + decbasispoint
                    If PerpetualFlag = "P" Then
                        If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                            Hid_YTCAnn.Value = decYTC
                            strSemiAnnFlag = "A"
                        Else
                            'txt_YTCSemi.Text = objCommon.DecimalFormat4(txt_YTCSemi.Text)
                            Hid_YTCSemi.Value = decYTCSem
                            strSemiAnnFlag = "S"
                        End If
                        dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTCAnn.Value), Val(Hid_YTCSemi.Value))
                        GlobalFuns.CalculateYield(txt_CalcDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                        datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "C", dblResult, strSemiAnnFlag)
                    Else
                        dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTMAnn.Value), Val(Hid_YTMSemi.Value))
                        GlobalFuns.CalculateYield(txt_CalcDate.Text, decFaceValue, decMarketRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                       datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")
                    End If
                    'decYield = objCommon.DecimalFormat4(decYield)
                    'dblYTMAnn = objCommon.DecimalFormat4(decYield)
                    'dblYTMSemi = objCommon.DecimalFormat4(decYTMSemi)
                    'dblYTCSemi = objCommon.DecimalFormat4(dblYTCSemi)

                End If
            ElseIf PerpetualFlag = "P" Then
                If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                    Hid_YTCAnn.Value = decYTC
                    strSemiAnnFlag = "A"
                Else
                    'txt_YTCSemi.Text = objCommon.DecimalFormat4(txt_YTCSemi.Text)
                    Hid_YTCSemi.Value = decYTCSem
                    strSemiAnnFlag = "S"
                End If
                dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTCAnn.Value), Val(Hid_YTCSemi.Value))
                GlobalFuns.CalculateYield(txt_CalcDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "C", dblResult, strSemiAnnFlag)
            Else
                dblResult = IIf(strSemiAnnFlag = "A", Val(Hid_YTMAnn.Value), Val(Hid_YTMSemi.Value))
                GlobalFuns.CalculateYield(txt_CalcDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                               datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")
            End If

            With objCommon
                Hid_YTMAnn.Value = .DecimalFormat4(dblYTMAnn)
                Hid_YTCAnn.Value = .DecimalFormat4(dblYTCAnn)
                Hid_YTPAnn.Value = .DecimalFormat4(dblYTPAnn)
                If Val(Hid_Frequency.Value & "") > 1 Then
                    Hid_YTMSemi.Value = .DecimalFormat4(dblYTMSemi)
                    Hid_YTCSemi.Value = .DecimalFormat4(dblYTCSemi)
                    Hid_YTPSemi.Value = .DecimalFormat4(dblYTPSemi)
                End If
            End With
        Catch ex As Exception
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillXIRROptions_New()
        Try
            If decYTC <> 0.0 Then
                If lblYieldPriceType = "" Then
                    lblYieldPriceType = "R"
                Else
                    If lblYieldPriceType = "R" Then
                        CCount = CCount + 1
                    End If
                End If
            End If
            If decYield <> 0.0 Then
                If lblYieldPriceType = "" Then
                    lblYieldPriceType = "R"
                Else
                    If lblYieldPriceType = "R" Then
                        CCount = CCount + 1
                    End If
                End If
            End If
            If decRate <> 0.0 Then
                If lblYieldPriceType = "" Then
                    lblYieldPriceType = "Y"
                Else
                    If lblYieldPriceType = "Y" Then
                        CCount = CCount + 1
                    End If
                End If
            End If
            If decYTCSem <> 0.0 Then
                If lblYieldPriceType = "" Then
                    lblYieldPriceType = "R"
                Else
                    If lblYieldPriceType = "R" Then
                        CCount = CCount + 1
                    End If
                End If
            End If
            If decYTMSemi <> 0.0 Then
                If lblYieldPriceType = "" Then
                    lblYieldPriceType = "R"
                Else
                    If lblYieldPriceType = "R" Then
                        CCount = CCount + 1
                    End If
                End If
            End If

            If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                strSemiAnnFlag = "A"
            Else
                strSemiAnnFlag = "S"
            End If

            If CCount > 1 Then
                CalculateXIRRYield(SecurityId, txt_CalcDate.Text, Val(decRate), Val(Hid_Frequency.Value), False, "R", "N")
            ElseIf decYield <> 0.0 Or decYTMSemi <> 0.0 Then
                dblYTMAnn = Val(decYield)
                dblYTMSemi = Val(decYTMSemi)
                strSemiAnnFlag = IIf(dblYTMAnn > 0, "A", "S")

                CalculateXIRRPrice(SecurityId, txt_CalcDate.Text, Val(Hid_Frequency.Value), False, strSemiAnnFlag, "R", "N", "M")
                Hid_Rate.Value = objCommon.DecimalFormat4(dblPTM) + decbasispoint
                CalculateXIRRYield(SecurityId, txt_CalcDate.Text, Val(objCommon.DecimalFormat4(dblPTM)), Val(Hid_Frequency.Value), False, "R", "N")

            ElseIf decYTC <> 0.0 Or decYTCSem <> 0.0 Then
                dblYTCAnn = Val(decYTC)
                dblYTCSemi = Val(decYTCSem)
                strSemiAnnFlag = IIf(dblYTCAnn > 0, "A", "S")

                CalculateXIRRPrice(SecurityId, txt_CalcDate.Text, Val(Hid_Frequency.Value), False, strSemiAnnFlag, "R", "N", "C")
                Hid_Rate.Value = objCommon.DecimalFormat4(dblPTC) + decbasispoint
                CalculateXIRRYield(SecurityId, txt_CalcDate.Text, Val(objCommon.DecimalFormat4(dblPTC)), Val(Hid_Frequency.Value), False, "R", "N")
            Else
                CalculateXIRRYield(SecurityId, txt_CalcDate.Text, Val(decRate), Val(Hid_Frequency.Value), False, "R", "N")

            End If

            With objCommon
                Hid_YTMAnn.Value = .DecimalFormat4(dblYTMAnn)
                Hid_YTCAnn.Value = .DecimalFormat4(dblYTCAnn)
                Hid_YTPAnn.Value = .DecimalFormat4(dblYTPAnn)
                If Val(Hid_Frequency.Value & "") > 1 Then
                    Hid_YTMSemi.Value = .DecimalFormat4(dblYTMSemi)
                    Hid_YTCSemi.Value = .DecimalFormat4(dblYTCSemi)
                    Hid_YTPSemi.Value = .DecimalFormat4(dblYTPSemi)
                End If
            End With
        Catch ex As Exception
            '   Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Private Sub FillXIRROptions()
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
                    dblMarketValue = Val(decRate) * Val(Hid_FaceValue.Value) / 100
                    'dblMarketValue = 23750
                    'dblMarketValue = txt_rate

                    XirrAmt(0) = -dblMarketValue
                    XirrDate(0) = objCommon.DateFormat(txt_Date.Text)
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
                        '  txt_YTMAnn.Text = objCommon.DecimalFormat4(GetDDBResult())
                        dblYTMAnn = objCommon.DecimalFormat4(GetDDBResult())
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
                        ' txt_YTCAnn.Text = objCommon.DecimalFormat4(GetDDBResult())
                        dblYTCAnn = objCommon.DecimalFormat4(GetDDBResult())
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
                    '    txt_YTPAnn.Text = objCommon.DecimalFormat4(GetDDBResult())
                    'Else
                    '    txt_YTPAnn.Text = "0.0"
                    'End If
                    ' ****************************************************************************************************

                Else
                    If CCount > 1 Then
                    ElseIf decYield <> 0.0 Or decYTMSemi <> 0.0 Then
                        If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                            decYield = objCommon.DecimalFormat4(decYield)
                            strSemiAnnFlag = "A"
                        Else
                            'txt_YTMSemi.Text = objCommon.DecimalFormat4(txt_YTMSemi.Text)
                            decYTMSemi = objCommon.DecimalFormat4(decYTMSemi)
                            strSemiAnnFlag = "S"
                        End If
                        dblYTMAnn = objCommon.DecimalFormat4(Val(decYield))
                        dblYTMSemi = objCommon.DecimalFormat4(Val(decYTMSemi))
                        dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual,
                               MatDate, MatAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue,
                                   Hid_Frequency.Value, strSemiAnnFlag, "M", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        Hid_Rate.Value = objCommon.DecimalFormat4(dblResult)
                    ElseIf decYTC <> 0.0 Or decYTCSem <> 0.0 Then
                        If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                            strSemiAnnFlag = "A"
                            decYTC = objCommon.DecimalFormat4(Val(decYTC))
                        Else
                            strSemiAnnFlag = "S"
                            decYTCSem = objCommon.DecimalFormat4(Val(decYTCSem))
                            'txt_YTCSemi.Text = objCommon.DecimalFormat4(Val(txt_YTCSemi.Text))
                        End If
                        dblYTCAnn = objCommon.DecimalFormat4(Val(decYTC))
                        dblYTCSemi = objCommon.DecimalFormat4(Val(decYTCSem))
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
                            dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual,
                                        CallDate, CallAmt, CoupDate, CoupRate, 31, datInterest, datIssue,
                                        Hid_Frequency.Value, Trim(strSemiAnnFlag), "C", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            'txt_rate.Text = objCommon.DecimalFormat4(dblResult)
                            Hid_Rate.Value = objCommon.DecimalFormat4(dblResult)
                        End If
                    Else
                        If MatDate.Length > 0 Then
                            CntXirr = 0
                            CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, MatDate, MatAmt,
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
                            CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, CallDate, CallAmt,
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
                            CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, PutDate, PutAmt,
                                          CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, "A", CombineIPMat, 365, "E", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                            GetXIRRResult(Val(Hid_Frequency.Value), dblYTPAnn, dblYTPSemi)
                        End If
                    End If
                End If
            Else
                If CCount > 1 Then

                ElseIf decYield <> 0.0 Or decYTMSemi <> 0.0 Then
                    If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                        decYield = objCommon.DecimalFormat4(decYield)
                        strSemiAnnFlag = "A"
                    Else
                        'txt_YTMSemi.Text = objCommon.DecimalFormat4(txt_YTMSemi.Text)
                        decYTMSemi = objCommon.DecimalFormat4(decYTMSemi)
                        strSemiAnnFlag = "S"
                    End If
                    dblYTMAnn = objCommon.DecimalFormat4(Val(decYield))
                    dblYTMSemi = objCommon.DecimalFormat4(Val(decYTMSemi))
                    dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual,
                           MatDate, MatAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue,
                               Hid_Frequency.Value, strSemiAnnFlag, "M", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                    Hid_Rate.Value = objCommon.DecimalFormat4(dblResult)
                ElseIf decYTC <> 0.0 Or decYTCSem <> 0.0 Then
                    If strSemiAnnFlag = "Y" Or strSemiAnnFlag = "A" Then
                        decYTC = objCommon.DecimalFormat4(Val(decYTC))
                        strSemiAnnFlag = "A"
                    Else
                        decYTCSem = objCommon.DecimalFormat4(Val(decYTCSem))
                        strSemiAnnFlag = "S"
                        'txt_YTCSemi.Text = objCommon.DecimalFormat4(Val(txt_YTCSemi.Text))
                    End If
                    dblYTCAnn = objCommon.DecimalFormat4(Val(decYTC))
                    dblYTCSemi = objCommon.DecimalFormat4(Val(decYTCSem))
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
                        dblResult = CalculateXIRRMarketRate(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual,
                                    CallDate, CallAmt, CoupDate, CoupRate, 31, datInterest, datIssue,
                                    Hid_Frequency.Value, Trim(strSemiAnnFlag), "C", "E", CombineIPMat, 365, "F", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        'txt_rate.Text = objCommon.DecimalFormat4(dblResult)
                        Hid_Rate.Value = objCommon.DecimalFormat4(dblResult)
                    End If
                Else
                    If MatDate.Length > 0 Then
                        CntXirr = 0
                        CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, MatDate, MatAmt,
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
                        CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, CallDate, CallAmt,
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
                        CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, PutDate, PutAmt,
                                      CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, "A", CombineIPMat, 365, "E", BrokenInt, InterestOnHoliday, InterestOnSat, MaturityOnHoliday, MaturityOnSat)
                        GetXIRRResult(Val(Hid_Frequency.Value), dblYTPAnn, dblYTPSemi)
                    End If
                End If
            End If

            With objCommon
                If decYield <> 0.0 And decRate <> 0.0 Then
                Else
                    Hid_YTMAnn.Value = .DecimalFormat4(dblYTMAnn)
                    Hid_YTCAnn.Value = .DecimalFormat4(dblYTCAnn)
                    Hid_YTPAnn.Value = .DecimalFormat4(dblYTPAnn)
                    If Val(Hid_Frequency.Value & "") > 1 Then
                        Hid_YTMSemi.Value = .DecimalFormat4(dblYTMSemi)
                        Hid_YTCSemi.Value = .DecimalFormat4(dblYTCSemi)
                        Hid_YTPSemi.Value = .DecimalFormat4(dblYTPSemi)
                    End If
                End If
            End With
            'CCount = 0
        Catch ex As Exception
            '   Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub



    Private Sub BindOpenQuoteSecurity()
        Dim dtNew As DataTable
        OpenConn()
        dtNew = objCommon.FillDataTable(sqlConn, "ID_Fill_QuoteSecurity", Val(Hid_FaxQuoteId.Value), "FaxQuoteId")
        Session("TempSecurityTable") = dtNew
        Session("SavedFaxQuotes") = dtNew
        FillSecurityGrid_OpenQuote()
        ' btn_Save.Visible = False
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
                    BrokenInt = 0
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
                'If Val(Hid_Frequency.Value) <> 0 Then
                '    If strMatDate(0) <> "" Then
                '        chk_CombineIPMat.Visible = CheckLastIPMaturity(Hid_InterestDate.Value, CDate(strMatDate(UBound(strMatDate) - 1)), Hid_Frequency.Value)
                '    End If

                'Else
                '    chk_CombineIPMat.Visible = False
                'End If
            End If
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
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


    Private Sub FillMMYOptions()
        Try
            datYTM = objCommon.DateFormat(txt_Date.Text)
            CalculateMMY(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, MatDate, MatAmt,
                                            CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Val(Hid_Frequency.Value), Val(Hid_MMYRate.Value) / 100, intDays, FirstAllYr)
            Hid_YTMAnn.Value = objCommon.DecimalFormat4(dblYTMAnn)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function FillDateArrays(ByVal strValue As String) As Date()
        Try
            Dim strDate() As String
            Dim arrDate() As Date
            Dim I As Int32

            strDate = Split(strValue, "!")
            ReDim arrDate(strDate.Length - 2)
            'arrDate(0) = Date.MinValue
            For I = 0 To strDate.Length - 2
                If strDate(I) <> "" Then arrDate(I) = CDate(strDate(I))
            Next
            Return arrDate
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Throw ex
        End Try
    End Function

    Private Function FillAmtArrays(ByVal strValue As String) As Double()
        Try
            Dim strDate() As String
            Dim arrDouble() As Double
            Dim I As Int32

            strDate = Split(strValue, "!")
            ReDim arrDouble(strDate.Length - 2)
            For I = 0 To strDate.Length - 2
                If strDate(I) <> "" Then arrDouble(I) = CDbl(strDate(I))
            Next
            Return arrDouble
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Throw ex
        End Try
    End Function
    Private Sub SetValues()
        Try
            Dim YTMDt As Date
            YTMDt = objCommon.DateFormat(txt_Date.Text)
            With objCommon
                datYTM = .DateFormat(txt_Date.Text)
                datInterest = Hid_InterestDate.Value
                datBookClosure = Hid_BookClosureDate.Value
                blnNonGovernment = IIf(Hid_GovernmentFlag.Value = "N", True, False)
                blnRateActual = IIf(RateActualFlag = "R", True, False)
                blnDMAT = IIf(PhysicalDematFlag = "P", False, True)
                decFaceValue = .DecimalFormat4(Val(Hid_FaceValue.Value))
                decRate = .DecimalFormat4(Val(Hid_Rate.Value))
                datIssue = Hid_Issue.Value
                datBookClosure = IIf(blnDMAT = True, Hid_DMATBkDate.Value, Hid_BookClosureDate.Value)
                intBKDiff = CalculateBookClosureDiff(datBookClosure, PhysicalDematFlag, datInterest, blnNonGovernment)
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

    Public Function MeasureTextHeight(ByVal text As String, ByVal width As Integer) As Double
        If String.IsNullOrEmpty(text) Then Return 0.0
        Dim bitmap = New Bitmap(1, 1)
        Dim graphics = System.Drawing.Graphics.FromImage(bitmap)
        Dim pixelWidth = Convert.ToInt32(width * 7.5)
        Dim drawingFont = New System.Drawing.Font("Calibri", 14)
        Dim size = graphics.MeasureString(text, drawingFont, pixelWidth)
        Return System.Math.Min(Convert.ToDouble(size.Height) * 72 / 96, 409)
    End Function

    'Protected Sub btn_CreateExcelFax_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CreateExcelFax.Click
    '    Try
    '        If dg_Customer.Rows.Count < 2 Then
    '            BlankExcelFax()
    '        Else
    '            Dim dt As DataTable
    '            Dim dtCust As DataTable
    '            Dim strDestination As String
    '            Dim strOldCustName As String = ""
    '            Dim intCnt As Int16 = -1
    '            Dim arrContact(0) As String
    '            Dim arrIndex(0) As Int16
    '            Dim strCustCont As String = ""
    '            Dim strFileOfferSave As String
    '            Dim K As Int16
    '            Dim J As Int16
    '            Dim srStart As StreamReader
    '            Dim srEnd As StreamReader = Nothing
    '            Dim strFields() As String
    '            Dim strFileSave As String
    '            Dim strUserContent As String
    '            Dim strEnd As String
    '            Dim strEnd2 As String
    '            Dim srEnd1 As StreamReader
    '            Dim CustomerName As String = ""
    '            Dim PrevSecTypeId As Integer
    '            Dim str1 As String
    '            Dim FooterText As String = ""
    '            Dim workbook As New XLWorkbook

    '            Dim dta As DataTable = TryCast(Session("CustomerContectTable"), DataTable)
    '            GetImages()

    '            strFileSave = ConfigurationManager.AppSettings("ImagePath") & "\"
    '            strFileOfferSave = ConfigurationManager.AppSettings("ImagePath") & "\blank.bmp"
    '            strDestination = strFileSave & Session("UserName")


    '            Dim DI As New DirectoryInfo(strDestination)
    '            If DI.Exists = True Then
    '                DI.Delete(True)
    '            End If
    '            DI.Create()
    '            Dim d1 As DataTable = objCommon.FillDataTable(sqlConn, "ID_SELECT_BranchMaster", Session("Branchid"), "Branchid")

    '            For b As Int32 = 0 To dta.Rows.Count - 2
    '                'Dim strppp As String = (Server.MapPath("").Replace("Forms", "Temp") & "\OfferExcel.xlsx")
    '                'Dim strppp As String = (Server.MapPath("").Replace("Forms", "Temp") & "\OfferExcel_Logo.xlsx")
    '                Dim strppp As String = (Server.MapPath("").Replace("Forms", "Temp") & "\OfferExcel.xlsx")

    '                'Dim worksheet = workbook.Add("Sample Sheet")
    '                Dim myRange As ClosedXML.Excel.IXLCell
    '                Dim strPath As String = ""
    '                Dim sImagePath As String = strFileOfferSave

    '                Dim strFileName As String = (Server.MapPath("").Replace("Forms", "Temp") & "\Report12.xlsx")
    '                Dim custname As String = ""

    '                dt = CType(Session("TempSecurityTable"), DataTable)
    '                dt.DefaultView.Sort = "SecurityTypeId Asc"
    '                dt = dt.DefaultView.ToTable()

    '                Dim view As DataView = New DataView(dt)

    '                Dim distinctValues As DataTable = view.ToTable(True, "SecurityTypeId")
    '                dtCust = TryCast(Session("CustomerContectTable"), DataTable)



    '                'For J = 0 To dg_Customer.Rows.Count - 2
    '                '    strFields = Split(Trim(dtCust.Rows(J).Item("FieldId") & ""), ",")
    '                'Next

    '                strFields = Split(Trim(dta.Rows(b).Item("FieldId") & ""), ",")
    '                intRowIndex = 0

    '                Dim C As Int32 = 1
    '                Dim B1 As Integer = 2
    '                Dim ds1 As New DataTable
    '                Dim dtFill1 As DataTable
    '                Dim dtCustHeaderData As New DataTable
    '                OpenConn()
    '                Dim strExport As New StringBuilder
    '                dtCustHeaderData = objCommon.FillDataTable(sqlConn, "ID_FILL_FaxHeaderFooter", dta.Rows(b).Item("CustomerId"), "CustomerId")
    '                'dtCustHeaderData = Nothing


    '                If dtCustHeaderData.Rows.Count > 0 Then
    '                    intRowIndex = intRowIndex + 1
    '                    If Convert.ToString(dtCustHeaderData.Rows(0).Item("HeaderText")) = "" Then
    '                        intRowIndex = intRowIndex + 3
    '                        workbook = New XLWorkbook(strppp)
    '                        Dim worksheet1 = workbook.Worksheet(1)

    '                        FooterText = Convert.ToString(dtCustHeaderData.Rows(0).Item("FooterText"))
    '                        worksheet1.Cell(intRowIndex, C).Style.Font.Bold = True
    '                        worksheet1.Cell(intRowIndex, C).WorksheetColumn.Width = "100"
    '                        worksheet1.Columns.AdjustToContents()
    '                        worksheet1.Range(intRowIndex, C, intRowIndex, 8).Merge()
    '                        worksheet1.Row(5).Height = MeasureTextHeight(worksheet1.Cell(intRowIndex, C).Value, 100)

    '                        intRowIndex = intRowIndex + 2
    '                        'AddExcelQuotes(strFields)
    '                        Dim R1 As Int32
    '                        Dim ds2 As DataTable
    '                        Dim dtFill2 As DataTable
    '                        OpenConn()
    '                        For R1 = 0 To strFields.Length - 1
    '                            Hid_FieldId.Value = Val(strFields(R1))
    '                            ds2 = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
    '                            intTotWidth = intTotWidth + Val(ds2.Rows(0).Item("WordWidth") & "")
    '                            worksheet1.Cell(intRowIndex, R1 + 1).Value = Trim(ds2.Rows(0).Item("FaxField") & "")
    '                            myRange = worksheet1.Cell(intRowIndex, R1 + 1)

    '                            SetTableStyle(myRange, "M")

    '                        Next

    '                        For R1 = 0 To dg_Selected.Rows.Count - 1
    '                            'intRowIndex = intRowIndex + 1
    '                            Hid_SecId.Value = Val(CType(dg_Selected.Rows(R1).FindControl("lbl_SecurityId"), Label).Text)
    '                            Hid_SecTypeId.Value = Val(CType(dg_Selected.Rows(R1).FindControl("lbl_SecurityTypeId"), Label).Text)
    '                            Hid_ShowNumber.Value = Trim(CType(dg_Selected.Rows(R1).FindControl("lbl_LotSize"), TextBox).Text)
    '                            dtFill2 = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")

    '                            If Val(Hid_SecId.Value) <> 0 Then
    '                                intRowIndex = intRowIndex + 1
    '                                dtFill2 = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")
    '                                Dim strFieldName As String
    '                                Dim strTableName As String
    '                                Dim strColName As String
    '                                Dim strFieldValue As String
    '                                Dim P As Int32
    '                                Dim Q As Int32
    '                                Dim intColRows As Int16
    '                                Dim intRowHeight As Int16
    '                                Dim dsFieldNames As DataTable
    '                                'Dim dt As DataTable
    '                                Dim strTemp As String
    '                                OpenConn()

    '                                Dim W As Single
    '                                Dim dtSel As DataTable
    '                                Dim DvSel As DataView = New DataView()

    '                                'strFieldValue = Trim(CType(dg_Selected.(intIndex).FindControl("lbl_" & strFieldName), Label).Text & "")
    '                                dtSel = TryCast(Session("TempSecurityTable"), DataTable)
    '                                dtSel.DefaultView.Sort = "SecurityTypeId Asc"
    '                                dtSel = dtSel.DefaultView.ToTable()
    '                                DvSel = New DataView(dtSel)

    '                                DvSel.RowFilter = String.Empty
    '                                DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value

    '                                For P = 0 To strFields.Length - 1
    '                                    Hid_FieldId.Value = Val(strFields(P))

    '                                    dsFieldNames = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
    '                                    'dsFieldNames = objCommon.GetDataSet(SqlDataSourceFieldId)
    '                                    With dsFieldNames.Rows(0)
    '                                        strTableName = Trim(.Item("TableName") & "")
    '                                        strFieldName = Trim(.Item("TableField") & "")
    '                                        If strFieldName = "Rate" Then intRateIndex = P
    '                                        If strFieldName = "SecurityName" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 20
    '                                            worksheet1.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                            worksheet1.Cell(intRowIndex, P + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
    '                                            worksheet1.Cell(intRowIndex, P + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
    '                                        ElseIf strFieldName = "NSDLAcNumber" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 13
    '                                        ElseIf strFieldName = "SecurityIssuer" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 20
    '                                        ElseIf strFieldName = "Category" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 9
    '                                        ElseIf strFieldName = "Issuedate" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                        ElseIf strFieldName = "SecuredUnsec" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                        ElseIf strFieldName = "NSDLFaceValue" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                        ElseIf strFieldName = "MaturityDate" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                        ElseIf strFieldName = "CallDate" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                        ElseIf strFieldName = "Rating" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                            worksheet1.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                        ElseIf strFieldName = "RatingOrg" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                            worksheet1.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                        ElseIf strFieldName = "CouponRate" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 8
    '                                        ElseIf strFieldName = "Rate" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                        ElseIf strFieldName = "PutDate" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                        ElseIf strFieldName = "IPDates" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                            worksheet1.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                        ElseIf strFieldName = "FrequencyOfInterest1" Then
    '                                            worksheet1.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                            worksheet1.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                        End If
    '                                        If strTableName <> "FromPage" Then
    '                                            For Q = 0 To dtFill2.Columns.Count - 1
    '                                                strColName = dtFill2.Columns(Q).ColumnName
    '                                                If UCase(strFieldName) = UCase(strColName) Then
    '                                                    strTemp = ""
    '                                                    If strColName = "IPDates" Then strTemp = " "

    '                                                    If strColName = "MaturityDate" Or strColName = "PutDate" Or strColName = "SecurityInfoDate" Or strColName = "IssueDate" Or strColName = "FirstInterestDate" Or strColName = "BookClosureDate" Or strColName = "DMATBookClosureDate" Or strColName = "FirstInterestDate" Or strColName = "CallDate" Or strColName = "Call Dates" Or strColName = "SecCallDate" Then
    '                                                        worksheet1.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-MMM-yyyy"
    '                                                        worksheet1.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill2.Rows(0).Item(strColName) & "")
    '                                                    ElseIf strColName = "IPDates" Then
    '                                                        worksheet1.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-MMM"
    '                                                        worksheet1.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill2.Rows(0).Item(strColName) & "")
    '                                                    Else
    '                                                        worksheet1.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill2.Rows(0).Item(strColName) & "")
    '                                                    End If

    '                                                    Exit For
    '                                                End If

    '                                                myRange = worksheet1.Cell(intRowIndex, P + 1)
    '                                                SetTableStyle(myRange, "", strColName)
    '                                            Next
    '                                        Else
    '                                            intColRows = 0
    '                                            If strFieldName = "ShowNumber" Then
    '                                                If Hid_ShowNumber.Value = "" Or Hid_ShowNumber.Value = "0" Then
    '                                                    strFieldValue = "N/A"
    '                                                Else
    '                                                    strFieldValue = Hid_ShowNumber.Value
    '                                                End If

    '                                            ElseIf Trim(DvSel.Item(0)(strFieldName).ToString() & "") = "" Then
    '                                                strFieldValue = "N/A"
    '                                            Else

    '                                                strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
    '                                            End If

    '                                            If InStr(strFieldValue, "!") > 1 Then
    '                                                For W = 1 To Len(strFieldValue)
    '                                                    If Mid$(strFieldValue, W, 1) = "!" Then
    '                                                        intColRows = intColRows + 1

    '                                                    End If
    '                                                Next
    '                                                strFieldValue = Replace(Left(strFieldValue, Len(strFieldValue) - 1), "!", Chr(10))
    '                                                If intRowHeight < (13 * intColRows) Then intRowHeight = 13 * intColRows
    '                                            Else
    '                                                strFieldValue = Replace(strFieldValue, "!", "")
    '                                            End If
    '                                            'strFieldValue = Replace(strFieldValue, "!", Chr(10))
    '                                            If IsNumeric(strFieldValue) = True Then
    '                                                If strFieldName = "YTMAnn" Or strFieldName = "YTCAnn" Or strFieldName = "YTPAnn" Or strFieldName = "YTMSemi" Or strFieldName = "YTCSemi" Or strFieldName = "YTPSemi" Or strFieldName = "CouponRate" Or strFieldName = "Yield" Then
    '                                                    If objCommon.DecimalFormat4((strFieldValue)) = 0.0 Then
    '                                                        strFieldValue = "N/A"
    '                                                    Else
    '                                                        strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
    '                                                    End If
    '                                                End If
    '                                            Else
    '                                                strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
    '                                            End If
    '                                            worksheet1.Cell(intRowIndex, P + 1).Value = strFieldValue
    '                                            If strFieldValue <> "" Then
    '                                                myRange = worksheet1.Cell(intRowIndex, P + 1)
    '                                                SetTableStyle(myRange)

    '                                            End If

    '                                        End If

    '                                    End With

    '                                Next
    '                                PrevSecTypeId = Val(CType(dg_Selected.Rows(R1).FindControl("lbl_SecurityTypeId"), Label).Text)
    '                            End If

    '                        Next
    '                        'footer code
    '                        Dim p1 As Integer = strFields.Length
    '                        If FooterText <> "" Then
    '                            intTotWidth = 0
    '                            intRowIndex = intRowIndex + 2
    '                            worksheet1.Cell(intRowIndex, C).Value = FooterText
    '                            worksheet1.Range(intRowIndex, C, intRowIndex, p1).Merge()
    '                            worksheet1.Range(intRowIndex, C, intRowIndex, p1).Style.Alignment.WrapText = True
    '                            worksheet1.Range(intRowIndex, C, intRowIndex, p1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
    '                            worksheet1.Row(intRowIndex).Height = MeasureTextHeight(worksheet1.Cell(intRowIndex, C).Value, 100)
    '                        Else
    '                            intRowIndex = intRowIndex + 2
    '                            worksheet1.Cell(intRowIndex, 1).Value = "Please note that the above offers are valid subject to market conditions and availability of stocks at the time of closing of a deal."
    '                            intRowIndex = intRowIndex + 1
    '                            worksheet1.Cell(intRowIndex, 1).Value = "Please feel free to contact the undersigned for any clarification"
    '                        End If
    '                        intRowIndex = intRowIndex + 2
    '                        worksheet1.Cell(intRowIndex, 1).Value = "Thanking You,"
    '                        intRowIndex = intRowIndex + 3
    '                        worksheet1.Cell(intRowIndex, 1).Value = "Yours Faithfully,"
    '                        strUserContent = FillUserContent(srEnd)
    '                        intRowIndex = intRowIndex + 2
    '                        worksheet1.Cell(intRowIndex, 1).Style.Font.Bold = True
    '                        worksheet1.Cell(intRowIndex, 1).Value = Session("NameOfUser")
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet1.Cell(intRowIndex, 1).Value = strUserMobile
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet1.Cell(intRowIndex, 1).Value = strUseremail

    '                        intRowIndex = intRowIndex + 2
    '                        worksheet1.Cell(intRowIndex, 1).Value = strUserBranchAddressExcel
    '                        'worksheet1.Cell(intRowIndex, 1).Value = "Mumbai Office :PNB House, Sir P.M.Road, Fort,Mumbai - 400 001. Tel : 022-22693314-17, 22614823, 22691812 . Fax : 022-22691811/ 2692248. Website : www.pnbgilts.com. E-mail : pnbgilts@pnbgilts.com "
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet1.Cell(intRowIndex, 1).Value = ""


    '                        workbook.SaveAs(strDestination & "\" & Regex.Replace(dta.Rows(b).Item("CustomerName"), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "") & "" & ".xlsx")
    '                    Else
    '                        intRowIndex = 5
    '                        workbook = New XLWorkbook(strppp)
    '                        Dim worksheet2 = workbook.Worksheet(1)
    '                        worksheet2.Cell(intRowIndex, C).Value = Convert.ToString(dtCustHeaderData.Rows(0).Item("HeaderText"))
    '                        worksheet2.Cell(intRowIndex, C).Style.Alignment.WrapText = True
    '                        worksheet2.Cell(intRowIndex, C).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
    '                        worksheet2.Cell(intRowIndex, C).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top

    '                        FooterText = Convert.ToString(dtCustHeaderData.Rows(0).Item("FooterText"))
    '                        worksheet2.Cell(intRowIndex, C).Style.Font.Bold = True
    '                        worksheet2.Cell(intRowIndex, C).WorksheetColumn.Width = "100"
    '                        worksheet2.Columns.AdjustToContents()
    '                        worksheet2.Range(intRowIndex, C, intRowIndex, 8).Merge()
    '                        worksheet2.Row(5).Height = MeasureTextHeight(worksheet2.Cell(intRowIndex, C).Value, 100)

    '                        intRowIndex = intRowIndex + 2
    '                        'AddExcelQuotes(strFields)
    '                        Dim R2 As Int32
    '                        Dim ds3 As DataTable
    '                        Dim dtFill3 As DataTable
    '                        OpenConn()
    '                        For R2 = 0 To strFields.Length - 1
    '                            Hid_FieldId.Value = Val(strFields(R2))
    '                            ds3 = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
    '                            intTotWidth = intTotWidth + Val(ds3.Rows(0).Item("WordWidth") & "")
    '                            worksheet2.Cell(intRowIndex, R2 + 1).Value = Trim(ds3.Rows(0).Item("FaxField") & "")
    '                            myRange = worksheet2.Cell(intRowIndex, R2 + 1)

    '                            SetTableStyle(myRange, "M")

    '                        Next

    '                        For R2 = 0 To dg_Selected.Rows.Count - 1
    '                            'intRowIndex = intRowIndex + 1
    '                            Hid_SecId.Value = Val(CType(dg_Selected.Rows(R2).FindControl("lbl_SecurityId"), Label).Text)
    '                            Hid_SecTypeId.Value = Val(CType(dg_Selected.Rows(R2).FindControl("lbl_SecurityTypeId"), Label).Text)
    '                            Hid_ShowNumber.Value = Trim(CType(dg_Selected.Rows(R2).FindControl("lbl_LotSize"), TextBox).Text)
    '                            dtFill3 = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")

    '                            If Val(Hid_SecId.Value) <> 0 Then
    '                                intRowIndex = intRowIndex + 1
    '                                dtFill3 = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")
    '                                Dim strFieldName As String
    '                                Dim strTableName As String
    '                                Dim strColName As String
    '                                Dim strFieldValue As String
    '                                Dim P As Int32
    '                                Dim Q As Int32
    '                                Dim intColRows As Int16
    '                                Dim intRowHeight As Int16
    '                                Dim dsFieldNames As DataTable
    '                                'Dim dt As DataTable
    '                                Dim strTemp As String
    '                                OpenConn()

    '                                Dim W As Single
    '                                Dim dtSel As DataTable
    '                                Dim DvSel As DataView = New DataView()

    '                                'strFieldValue = Trim(CType(dg_Selected.(intIndex).FindControl("lbl_" & strFieldName), Label).Text & "")
    '                                dtSel = TryCast(Session("TempSecurityTable"), DataTable)
    '                                dtSel.DefaultView.Sort = "SecurityTypeId Asc"
    '                                dtSel = dtSel.DefaultView.ToTable()
    '                                DvSel = New DataView(dtSel)

    '                                DvSel.RowFilter = String.Empty
    '                                DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value

    '                                For P = 0 To strFields.Length - 1
    '                                    Hid_FieldId.Value = Val(strFields(P))

    '                                    dsFieldNames = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
    '                                    'dsFieldNames = objCommon.GetDataSet(SqlDataSourceFieldId)
    '                                    With dsFieldNames.Rows(0)
    '                                        strTableName = Trim(.Item("TableName") & "")
    '                                        strFieldName = Trim(.Item("TableField") & "")
    '                                        If strFieldName = "Rate" Then intRateIndex = P
    '                                        If strFieldName = "SecurityName" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 20
    '                                            worksheet2.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                            worksheet2.Cell(intRowIndex, P + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
    '                                            worksheet2.Cell(intRowIndex, P + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
    '                                        ElseIf strFieldName = "NSDLAcNumber" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 13
    '                                        ElseIf strFieldName = "SecurityIssuer" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 20
    '                                        ElseIf strFieldName = "Category" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 9
    '                                        ElseIf strFieldName = "Issuedate" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                        ElseIf strFieldName = "SecuredUnsec" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                        ElseIf strFieldName = "NSDLFaceValue" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                        ElseIf strFieldName = "MaturityDate" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                        ElseIf strFieldName = "CallDate" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                        ElseIf strFieldName = "Rating" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                            worksheet2.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                        ElseIf strFieldName = "RatingOrg" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                            worksheet2.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                        ElseIf strFieldName = "CouponRate" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 8
    '                                        ElseIf strFieldName = "Rate" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                        ElseIf strFieldName = "PutDate" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                        ElseIf strFieldName = "IPDates" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                            worksheet2.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                        ElseIf strFieldName = "FrequencyOfInterest1" Then
    '                                            worksheet2.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                            worksheet2.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                        End If
    '                                        If strTableName <> "FromPage" Then
    '                                            For Q = 0 To dtFill3.Columns.Count - 1
    '                                                strColName = dtFill3.Columns(Q).ColumnName
    '                                                If UCase(strFieldName) = UCase(strColName) Then
    '                                                    strTemp = ""
    '                                                    If strColName = "IPDates" Then strTemp = " "

    '                                                    If strColName = "MaturityDate" Or strColName = "PutDate" Or strColName = "SecurityInfoDate" Or strColName = "IssueDate" Or strColName = "FirstInterestDate" Or strColName = "BookClosureDate" Or strColName = "DMATBookClosureDate" Or strColName = "FirstInterestDate" Or strColName = "CallDate" Or strColName = "Call Dates" Or strColName = "SecCallDate" Then
    '                                                        worksheet2.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-MMM-yyyy"
    '                                                        worksheet2.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill3.Rows(0).Item(strColName) & "")
    '                                                    ElseIf strColName = "IPDates" Then
    '                                                        worksheet2.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-MMM"
    '                                                        worksheet2.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill3.Rows(0).Item(strColName) & "")
    '                                                    ElseIf strColName = "CouponRate" Then
    '                                                        If Trim(dtFill3.Rows(0).Item(strColName) & "") <> "" Then
    '                                                            worksheet2.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill3.Rows(0).Item(strColName) & "") & "%"
    '                                                        Else
    '                                                            worksheet2.Cell(intRowIndex, P + 1).Value = strTemp & "N/A" '& Trim(dtFill3.Rows(0).Item(strColName) & "") & "%"
    '                                                        End If

    '                                                    Else
    '                                                        worksheet2.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill3.Rows(0).Item(strColName) & "")
    '                                                    End If

    '                                                    Exit For
    '                                                End If

    '                                                myRange = worksheet2.Cell(intRowIndex, P + 1)
    '                                                SetTableStyle(myRange, "", strColName)
    '                                            Next
    '                                        Else
    '                                            intColRows = 0
    '                                            If strFieldName = "ShowNumber" Then
    '                                                'strFieldValue = Hid_ShowNumber.Value
    '                                                If Hid_ShowNumber.Value = "" Or Hid_ShowNumber.Value = "0" Then
    '                                                    strFieldValue = "N/A"
    '                                                Else
    '                                                    strFieldValue = Hid_ShowNumber.Value
    '                                                End If
    '                                            ElseIf Trim(DvSel.Item(0)(strFieldName).ToString() & "") = "" Then
    '                                                strFieldValue = "N/A"
    '                                            Else
    '                                                strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
    '                                            End If

    '                                            If InStr(strFieldValue, "!") > 1 Then
    '                                                For W = 1 To Len(strFieldValue)
    '                                                    If Mid$(strFieldValue, W, 1) = "!" Then
    '                                                        intColRows = intColRows + 1

    '                                                    End If
    '                                                Next
    '                                                strFieldValue = Replace(Left(strFieldValue, Len(strFieldValue) - 1), "!", Chr(10))
    '                                                If intRowHeight < (13 * intColRows) Then intRowHeight = 13 * intColRows
    '                                            Else
    '                                                strFieldValue = Replace(strFieldValue, "!", "")
    '                                            End If
    '                                            'strFieldValue = Replace(strFieldValue, "!", Chr(10))

    '                                            'strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
    '                                            If IsNumeric(strFieldValue) = True Then
    '                                                If strFieldName = "YTMAnn" Or strFieldName = "YTCAnn" Or strFieldName = "YTPAnn" Or strFieldName = "YTMSemi" Or strFieldName = "YTCSemi" Or strFieldName = "YTPSemi" Or strFieldName = "CouponRate" Or strFieldName = "Yield" Then
    '                                                    If objCommon.DecimalFormat4((strFieldValue)) = 0.0 Then
    '                                                        strFieldValue = "N/A"
    '                                                    Else
    '                                                        strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
    '                                                    End If
    '                                                End If
    '                                            Else
    '                                                strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
    '                                            End If

    '                                            worksheet2.Cell(intRowIndex, P + 1).Value = strFieldValue
    '                                            If strFieldValue <> "" Then
    '                                                myRange = worksheet2.Cell(intRowIndex, P + 1)
    '                                                SetTableStyle(myRange)

    '                                            End If
    '                                            'If strFieldName = "SellingRate" Then
    '                                            '    If strFieldValue <> "" Then
    '                                            '        Dim dp As Integer = strFieldValue.ToString(System.Globalization.CultureInfo.InvariantCulture).Split("."c)(1).Length
    '                                            '        If dp = 2 Then
    '                                            '            worksheet2.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "#,##0.00"
    '                                            '        End If
    '                                            '    End If

    '                                            '    '  worksheet2.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "#,##0.00"
    '                                            'End If
    '                                            If strFieldName = "ShowNumber" Then
    '                                            Else
    '                                                worksheet2.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "#,##0.0000"
    '                                            End If

    '                                        End If

    '                                    End With

    '                                Next
    '                                PrevSecTypeId = Val(CType(dg_Selected.Rows(R2).FindControl("lbl_SecurityTypeId"), Label).Text)
    '                            End If

    '                        Next
    '                        'footer code
    '                        Dim p1 As Integer = strFields.Length
    '                        If FooterText <> "" Then
    '                            intTotWidth = 0
    '                            intRowIndex = intRowIndex + 2
    '                            worksheet2.Cell(intRowIndex, C).Value = FooterText
    '                            worksheet2.Range(intRowIndex, C, intRowIndex, p1).Merge()
    '                            worksheet2.Range(intRowIndex, C, intRowIndex, p1).Style.Alignment.WrapText = True
    '                            worksheet2.Range(intRowIndex, C, intRowIndex, p1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
    '                            worksheet2.Row(intRowIndex).Height = MeasureTextHeight(worksheet2.Cell(intRowIndex, C).Value, 100)
    '                        Else
    '                            intRowIndex = intRowIndex + 2
    '                            worksheet2.Cell(intRowIndex, 1).Value = "Please note that the above offers are valid subject to market conditions and availability of stocks at the time of closing of a deal."
    '                            intRowIndex = intRowIndex + 1
    '                            worksheet2.Cell(intRowIndex, 1).Value = "Please feel free to contact the undersigned for any clarification"
    '                        End If
    '                        intRowIndex = intRowIndex + 2
    '                        worksheet2.Cell(intRowIndex, 1).Value = "Thanking You,"
    '                        intRowIndex = intRowIndex + 3
    '                        worksheet2.Cell(intRowIndex, 1).Value = "Yours Faithfully,"
    '                        strUserContent = FillUserContent(srEnd)
    '                        intRowIndex = intRowIndex + 2
    '                        worksheet2.Cell(intRowIndex, 1).Style.Font.Bold = True
    '                        worksheet2.Cell(intRowIndex, 1).Value = Session("NameOfUser")
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet2.Cell(intRowIndex, 1).Value = strUserMobile
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet2.Cell(intRowIndex, 1).Value = strUseremail

    '                        intRowIndex = intRowIndex + 2
    '                        worksheet2.Cell(intRowIndex, 1).Value = strUserBranchAddressExcel
    '                        'worksheet2.Cell(intRowIndex, 1).Value = "Mumbai Office :PNB House, Sir P.M.Road, Fort,Mumbai - 400 001. Tel : 022-22693314-17, 22614823, 22691812 . Fax : 022-22691811/ 2692248. Website : www.pnbgilts.com. E-mail : pnbgilts@pnbgilts.com "
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet2.Cell(intRowIndex, 1).Value = ""


    '                        workbook.SaveAs(strDestination & "\" & Regex.Replace(dta.Rows(b).Item("CustomerName"), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "") & "" & ".xlsx")

    '                    End If
    '                Else
    '                    workbook = New XLWorkbook(strppp)
    '                    Dim worksheet = workbook.Worksheet(1)

    '                    'worksheet.Cell(1, 1).Value = objComm.Trim(dta.Rows(b).Item("CustomerName") & "")
    '                    'worksheet.Cell(1, 1).Style.Font.Bold = True
    '                    'worksheet.Cell(1, 1).Style.Font.FontSize = 12
    '                    'worksheet.Range(1, 1, 1, 6).Merge()
    '                    'intRowIndex = intRowIndex + 5

    '                    intRowIndex = 2
    '                    worksheet.Cell(intRowIndex, 2).Style.NumberFormat.Format = "dd-mmm-yyyy"
    '                    worksheet.Cell(intRowIndex, 1).Value = "Date : "
    '                    worksheet.Cell(intRowIndex, 2).Value = String.Format(Today, "dd/MM/yyyy")
    '                    worksheet.Cell(intRowIndex, 2).WorksheetColumn.Width = 12
    '                    intRowIndex = intRowIndex + 2

    '                    worksheet.Cell(intRowIndex, 1).Value = "To,"

    '                    If dta.Rows(b).Item("CustomerName") & "" <> "" Then
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerName") & ",")
    '                        custname = Trim(dta.Rows(b).Item("CustomerName") & "") & ","
    '                    End If

    '                    If dta.Rows(b).Item("CustomerAddress1") & "" <> "" Then
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerAddress1") & ",")
    '                    End If
    '                    If dta.Rows(b).Item("CustomerAddress2") & "" <> "" Then
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerAddress2") & ",")
    '                    End If
    '                    If dta.Rows(b).Item("CustomerCity") & "" <> "" Then
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerCity") & ",")
    '                    End If

    '                    intRowIndex = intRowIndex + 1
    '                    intRowIndex = intRowIndex + 1

    '                    worksheet.Cell(intRowIndex, 1).Value = "Kind Attention :- "
    '                    If dta.Rows(b).Item("ContactPerson") & "" <> "" Then
    '                        worksheet.Cell(intRowIndex, 2).Value = Trim(dta.Rows(b).Item("ContactPerson") & "")
    '                    End If

    '                    intRowIndex = intRowIndex + 1
    '                    srStart = WriteContents(strFileSave & "Start_Content.txt")
    '                    While srStart.Peek >= 0
    '                        intRowIndex = intRowIndex + 1
    '                        worksheet.Cell(intRowIndex, 1).Value = srStart.ReadLine
    '                        'MergeCells("A", "H")
    '                    End While

    '                    intRowIndex = intRowIndex + 1
    '                    intRowIndex = intRowIndex + 1
    '                    srStart.Close()

    '                    'AddExcelQuotes(strFields)
    '                    Dim R As Int32
    '                    Dim ds As DataTable
    '                    Dim dtFill As DataTable
    '                    OpenConn()
    '                    For R = 0 To strFields.Length - 1
    '                        Hid_FieldId.Value = Val(strFields(R))
    '                        ds = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
    '                        intTotWidth = intTotWidth + Val(ds.Rows(0).Item("WordWidth") & "")
    '                        worksheet.Cell(intRowIndex, R + 1).Value = Trim(ds.Rows(0).Item("FaxField") & "")
    '                        myRange = worksheet.Cell(intRowIndex, R + 1)

    '                        SetTableStyle(myRange, "M")

    '                    Next

    '                    For R = 0 To dg_Selected.Rows.Count - 1
    '                        'intRowIndex = intRowIndex + 1
    '                        Hid_SecId.Value = Val(CType(dg_Selected.Rows(R).FindControl("lbl_SecurityId"), Label).Text)
    '                        Hid_SecTypeId.Value = Val(CType(dg_Selected.Rows(R).FindControl("lbl_SecurityTypeId"), Label).Text)
    '                        Hid_ShowNumber.Value = Trim(CType(dg_Selected.Rows(R).FindControl("lbl_LotSize"), TextBox).Text)
    '                        dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")

    '                        If Val(Hid_SecId.Value) <> 0 Then
    '                            intRowIndex = intRowIndex + 1
    '                            dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")
    '                            Dim strFieldName As String
    '                            Dim strTableName As String
    '                            Dim strColName As String
    '                            Dim strFieldValue As String
    '                            Dim P As Int32
    '                            Dim Q As Int32
    '                            Dim intColRows As Int16
    '                            Dim intRowHeight As Int16
    '                            Dim dsFieldNames As DataTable
    '                            'Dim dt As DataTable
    '                            Dim strTemp As String
    '                            OpenConn()

    '                            Dim W As Single
    '                            Dim dtSel As DataTable
    '                            Dim DvSel As DataView = New DataView()

    '                            'strFieldValue = Trim(CType(dg_Selected.(intIndex).FindControl("lbl_" & strFieldName), Label).Text & "")
    '                            dtSel = TryCast(Session("TempSecurityTable"), DataTable)
    '                            dtSel.DefaultView.Sort = "SecurityTypeId Asc"
    '                            dtSel = dtSel.DefaultView.ToTable()
    '                            DvSel = New DataView(dtSel)

    '                            DvSel.RowFilter = String.Empty
    '                            DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value

    '                            For P = 0 To strFields.Length - 1
    '                                Hid_FieldId.Value = Val(strFields(P))

    '                                dsFieldNames = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
    '                                'dsFieldNames = objCommon.GetDataSet(SqlDataSourceFieldId)
    '                                With dsFieldNames.Rows(0)
    '                                    strTableName = Trim(.Item("TableName") & "")
    '                                    strFieldName = Trim(.Item("TableField") & "")
    '                                    If strFieldName = "Rate" Then intRateIndex = P
    '                                    If strFieldName = "SecurityName" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 20
    '                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
    '                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
    '                                    ElseIf strFieldName = "NSDLAcNumber" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 13
    '                                    ElseIf strFieldName = "SecurityIssuer" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 20
    '                                    ElseIf strFieldName = "Category" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 9
    '                                    ElseIf strFieldName = "Issuedate" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                    ElseIf strFieldName = "SecuredUnsec" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                    ElseIf strFieldName = "NSDLFaceValue" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                    ElseIf strFieldName = "MaturityDate" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                    ElseIf strFieldName = "CallDate" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                    ElseIf strFieldName = "Rating" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                    ElseIf strFieldName = "RatingOrg" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                    ElseIf strFieldName = "CouponRate" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 8
    '                                    ElseIf strFieldName = "Rate" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                    ElseIf strFieldName = "PutDate" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
    '                                    ElseIf strFieldName = "IPDates" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                    ElseIf strFieldName = "FrequencyOfInterest1" Then
    '                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
    '                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
    '                                    End If
    '                                    If strTableName <> "FromPage" Then
    '                                        For Q = 0 To dtFill.Columns.Count - 1
    '                                            strColName = dtFill.Columns(Q).ColumnName
    '                                            If UCase(strFieldName) = UCase(strColName) Then
    '                                                strTemp = ""
    '                                                If strColName = "IPDates" Then strTemp = " "

    '                                                If strColName = "MaturityDate" Or strColName = "PutDate" Or strColName = "SecurityInfoDate" Or strColName = "IssueDate" Or strColName = "FirstInterestDate" Or strColName = "BookClosureDate" Or strColName = "DMATBookClosureDate" Or strColName = "FirstInterestDate" Or strColName = "CallDate" Or strColName = "Call Dates" Or strColName = "SecCallDate" Then
    '                                                    worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-MMM-yyyy"
    '                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
    '                                                ElseIf strColName = "IPDates" Then
    '                                                    worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-MMM"
    '                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
    '                                                ElseIf strColName = "CouponRate" Then
    '                                                    If Trim(dtFill.Rows(0).Item(strColName) & "") <> "" Then
    '                                                        worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "") & "%"
    '                                                    Else
    '                                                        worksheet.Cell(intRowIndex, P + 1).Value = strTemp & "N/A" '& Trim(dtFill.Rows(0).Item(strColName) & "") & "%"
    '                                                    End If

    '                                                Else
    '                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
    '                                                End If

    '                                                Exit For
    '                                            End If

    '                                            myRange = worksheet.Cell(intRowIndex, P + 1)
    '                                            SetTableStyle(myRange, "", strColName)
    '                                        Next
    '                                    Else
    '                                        intColRows = 0
    '                                        If strFieldName = "ShowNumber" Then
    '                                            'strFieldValue = Hid_ShowNumber.Value
    '                                            If Hid_ShowNumber.Value = "" Or Hid_ShowNumber.Value = "0" Then
    '                                                strFieldValue = "N/A"
    '                                            Else
    '                                                strFieldValue = Hid_ShowNumber.Value
    '                                            End If
    '                                        ElseIf Trim(DvSel.Item(0)(strFieldName).ToString() & "") = "" Then
    '                                            strFieldValue = "N/A"
    '                                        Else
    '                                            strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
    '                                        End If

    '                                        If InStr(strFieldValue, "!") > 1 Then
    '                                            For W = 1 To Len(strFieldValue)
    '                                                If Mid$(strFieldValue, W, 1) = "!" Then
    '                                                    intColRows = intColRows + 1

    '                                                End If
    '                                            Next
    '                                            strFieldValue = Replace(Left(strFieldValue, Len(strFieldValue) - 1), "!", Chr(10))
    '                                            If intRowHeight < (13 * intColRows) Then intRowHeight = 13 * intColRows
    '                                        Else
    '                                            strFieldValue = Replace(strFieldValue, "!", "")
    '                                        End If
    '                                        'strFieldValue = Replace(strFieldValue, "!", Chr(10))
    '                                        'strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
    '                                        If IsNumeric(strFieldValue) = True Then
    '                                            If strFieldName = "YTMAnn" Or strFieldName = "YTCAnn" Or strFieldName = "YTPAnn" Or strFieldName = "YTMSemi" Or strFieldName = "YTCSemi" Or strFieldName = "YTPSemi" Or strFieldName = "CouponRate" Or strFieldName = "Yield" Then
    '                                                If objCommon.DecimalFormat4((strFieldValue)) = 0.0 Then
    '                                                    strFieldValue = "N/A"
    '                                                Else
    '                                                    strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
    '                                                End If
    '                                            End If
    '                                        Else
    '                                            strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
    '                                        End If
    '                                        worksheet.Cell(intRowIndex, P + 1).Value = strFieldValue
    '                                        If strFieldValue <> "" Then
    '                                            myRange = worksheet.Cell(intRowIndex, P + 1)
    '                                            SetTableStyle(myRange)

    '                                        End If
    '                                        'If strFieldName = "SellingRate" Then
    '                                        '    If strFieldValue <> "" Then
    '                                        '        Dim dp As Integer = strFieldValue.ToString(System.Globalization.CultureInfo.InvariantCulture).Split("."c)(1).Length
    '                                        '        If dp = 2 Then
    '                                        '            worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "#,##0.00"
    '                                        '        End If
    '                                        '    End If
    '                                        'End If
    '                                        If strFieldName = "ShowNumber" Then
    '                                        Else
    '                                            worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "#,##0.0000"
    '                                        End If

    '                                    End If
    '                                End With
    '                            Next
    '                            PrevSecTypeId = Val(CType(dg_Selected.Rows(R).FindControl("lbl_SecurityTypeId"), Label).Text)
    '                        End If

    '                    Next

    '                    intRowIndex = intRowIndex + 2
    '                    worksheet.Cell(intRowIndex, 1).Value = "Please note that the above offers are valid subject to market conditions and availability of stocks at the time of closing of a deal."
    '                    intRowIndex = intRowIndex + 1
    '                    worksheet.Cell(intRowIndex, 1).Value = "Please feel free to contact the undersigned for any clarification"

    '                    intRowIndex = intRowIndex + 2
    '                    worksheet.Cell(intRowIndex, 1).Value = "Thanking You,"
    '                    intRowIndex = intRowIndex + 3
    '                    worksheet.Cell(intRowIndex, 1).Value = "Yours Faithfully,"
    '                    strUserContent = FillUserContent(srEnd)
    '                    intRowIndex = intRowIndex + 2
    '                    worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
    '                    worksheet.Cell(intRowIndex, 1).Value = Session("NameOfUser")
    '                    intRowIndex = intRowIndex + 1
    '                    worksheet.Cell(intRowIndex, 1).Value = strUserMobile
    '                    intRowIndex = intRowIndex + 1
    '                    worksheet.Cell(intRowIndex, 1).Value = strUseremail

    '                    intRowIndex = intRowIndex + 2
    '                    worksheet.Cell(intRowIndex, 1).Value = strUserBranchAddressExcel
    '                    'worksheet.Cell(intRowIndex, 1).Value = "Mumbai Office :PNB House, Sir P.M.Road, Fort,Mumbai - 400 001. Tel : 022-22693314-17, 22614823, 22691812 . Fax : 022-22691811/ 2692248. Website : www.pnbgilts.com. E-mail : pnbgilts@pnbgilts.com "
    '                    intRowIndex = intRowIndex + 1
    '                    worksheet.Cell(intRowIndex, 1).Value = ""


    '                    workbook.SaveAs(strDestination & "\" & Regex.Replace(dta.Rows(b).Item("CustomerName"), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "") & "" & ".xlsx")


    '                End If


    '            Next

    '            Using zip As New ZipFile()
    '                zip.AlternateEncodingUsage = ZipOption.AsNecessary
    '                'zip.AddDirectoryByName("Admin")
    '                zip.AddDirectory(strDestination)

    '                Response.Clear()
    '                Response.BufferOutput = False
    '                'Dim zipName As String = [String].Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
    '                Dim zipName As String = "Offer.zip"
    '                Response.ContentType = "application/zip"
    '                Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
    '                zip.Save(Response.OutputStream)
    '                Response.End()
    '            End Using



    '            'Using zip = New ZipFile()
    '            '    ' CREATE A FILE USING A STRING. 
    '            '    ' THE FILE WILL BE STORED INSIDE THE ZIP FILE.
    '            '    'zip.AddEntry("Content.txt", "This Zip has 2 DOC files")

    '            '    ' ZIP THE FOLDER WITH THE FILES IN IT.
    '            '    zip.AddDirectory(strDestination)
    '            '    ' zip.AddFiles(Directory.GetFiles(Server.MapPath("~/Admin")), "packed")
    '            '    zip.Save(Server.MapPath("~/offer1.zip"))  ' SAVE THE ZIP FILE.
    '            'End Using

    '            ''Dim strParams(1) As String
    '            ''strParams(0) = strDestination
    '            ''strParams(1) = strDestination & "\Offer.zip"
    '            ''Dim ShlZip As New ShellZip
    '            ''ShlZip.Compress(strDestination & "\Offer.zip", strDestination)
    '            'System.Threading.Thread.Sleep(500)
    '            'With HttpContext.Current.Response
    '            '    .Clear()
    '            '    .Charset = ""
    '            '    .ClearHeaders()
    '            '    .ContentType = "application/vnd.ms-excel"
    '            '    '.ContentType = "application/x-zip-compressed"
    '            '    .AddHeader("content-disposition", "attachment;filename=offer1.zip")
    '            '    .WriteFile(Server.MapPath("~/offer1.zip"))
    '            '    .Flush()
    '            '    '.End()
    '            'End With
    '        End If
    '        'Page.ClientScript.RegisterStartupScript(Me.GetType, "select", "objExcel();", True)
    '    Catch ex As Exception
    '        objUtil.WritErrorLog(PgName, "btn_CreateExcelFax_Click", "Error in btn_CreateExcelFax_Click", "", ex)
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
    '    Finally
    '        CloseConn()
    '    End Try

    'End Sub

    Public Sub CreateExcel_Customer()
        Dim strMainPath As String = ""
        Try

            If dg_Customer.Rows.Count < 2 Then
                BlankExcelFax()
            Else
                Dim dt As DataTable
                Dim dtCust As DataTable
                Dim strDestination As String
                Dim strOldCustName As String = ""
                Dim intCnt As Int16 = -1
                Dim arrContact(0) As String
                Dim arrIndex(0) As Int16
                Dim strCustCont As String = ""
                Dim strFileOfferSave As String
                Dim K As Int16
                Dim J As Int16
                Dim srStart As StreamReader
                Dim srEnd As StreamReader = Nothing
                Dim strFields() As String
                Dim strFileSave As String
                Dim strUserContent As String
                Dim strEnd As String
                Dim strEnd2 As String
                Dim srEnd1 As StreamReader
                Dim CustomerName As String = ""
                Dim PrevSecTypeId As Integer
                Dim str1 As String
                Dim strCustPrefix As String = ""
                Dim ImgByte As Byte() = Nothing

                GetImages()
                Dim strExcelColumnName As String = ""
                strFileSave = ConfigurationManager.AppSettings("ImagePath") & "\"
                strFileOfferSave = ConfigurationManager.AppSettings("ImagePath") & "\blank.bmp"
                strDestination = strFileSave & Session("UserName")

                Dim dta As DataTable = TryCast(Session("CustomerContectTable"), DataTable)
                Dim DI As New DirectoryInfo(strDestination)
                If DI.Exists = True Then
                    DI.Delete(True)
                End If
                DI.Create()
                Dim d1 As DataTable = objCommon.FillDataTable(sqlConn, "ID_SELECT_BranchMaster", Session("Branchid"), "Branchid")
                For b As Int32 = 0 To dta.Rows.Count - 2
                    'Dim strppp_ As String = (Server.MapPath("").Replace("Forms", "Temp") & "\OfferExcel.xlsx")
                    ' Dim strppp As String = Hid_offerpath.Value.ToString()
                    Dim strppp As String = ConfigurationManager.AppSettings("offerPath") & "\OfferExcel.xlsx"
                    Dim workbook = New XLWorkbook(strppp)
                    Dim worksheet = workbook.Worksheet(1)


                    'Dim worksheet = workbook.Add("Sample Sheet")
                    Dim myRange As ClosedXML.Excel.IXLCell
                    Dim strPath As String = ""
                    Dim sImagePath As String = strFileOfferSave
                    Dim custname As String = ""

                    dt = CType(Session("TempSecurityTable"), DataTable)
                    dt.DefaultView.Sort = "OrderId Asc,SecurityTypeId, SecurityId Asc"
                    dt = dt.DefaultView.ToTable()

                    Dim view As DataView = New DataView(dt)

                    Dim distinctValues As DataTable = view.ToTable(True, "SecurityTypeId")
                    dtCust = TryCast(Session("CustomerContectTable"), DataTable)
                    strPath = strDestination & "\" & Regex.Replace(Trim(dtCust.Rows(J).Item("CustomerName") & ""), "[^0-9a-zA-Z .]+", "") & ".xlsx"
                    If IsInArray(J, arrIndex) Then
                        intRowIndex = 4
                    End If
                    intRowIndex = 2
                    myRange = worksheet.Cell(intRowIndex, 3)
                    myRange.Style.Font.Bold = True
                    myRange = worksheet.Cell(intRowIndex, 3)
                    myRange.Style.Font.Bold = True
                    'worksheet.Cell(intRowIndex - 4, 8).Value = ""
                    'worksheet.Cell(intRowIndex - 3, 8).Value = ""
                    'worksheet.Cell(intRowIndex - 2, 8).Value = ""
                    'worksheet.Cell(intRowIndex - 1, 8).Value = ""

                    'intRowIndex = intRowIndex + 1
                    'var Image = worksheet.A(imagePath).MoveTo(WS.Cell("A1")).Scale(1.0);
                    intRowIndex = 2
                    worksheet.Cell(intRowIndex, 2).Style.NumberFormat.Format = "dd-mmm-yyyy"
                    worksheet.Cell(intRowIndex, 1).Value = "Date : "
                    myRange = worksheet.Cell(intRowIndex, 1)
                    SetTableStyle(myRange, "N")
                    worksheet.Cell(intRowIndex, 2).Value = String.Format(Today, "dd/MM/yyyy")
                    myRange = worksheet.Cell(intRowIndex, 2)
                    SetTableStyle(myRange, "N")

                    worksheet.Cell(intRowIndex, 2).WorksheetColumn.Width = 18
                    intRowIndex = intRowIndex + 2

                    'worksheet.Cell(intRowIndex, 1).Value = "To,"
                    'myRange = worksheet.Cell(intRowIndex, 1)
                    'SetTableStyle(myRange, "N")

                    'If dta.Rows(b).Item("CustomerName") & "" <> "" Then
                    '    intRowIndex = intRowIndex + 1
                    '    'If Trim(dta.Rows(b).Item("CustPrefix") & "") <> "" Then
                    '    '    strCustPrefix = Trim(dta.Rows(b).Item("CustPrefix") & "")
                    '    '    If Not strCustPrefix.Contains(".") Then
                    '    '        strCustPrefix = strCustPrefix & ". "
                    '    '    End If
                    '    'End If
                    '    worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerName"))
                    '    custname = Trim(dta.Rows(b).Item("CustomerName") & "")
                    '    myRange = worksheet.Cell(intRowIndex, 1)
                    '    SetTableStyle(myRange, "N")
                    'End If

                    'Remove Client Address from All Quotes - 21AUG2020
                    'If dta.Rows(b).Item("CustomerAddress1") & "" <> "" Then
                    '    intRowIndex = intRowIndex + 1
                    '    worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerAddress1") & ",")
                    '    myRange = worksheet.Cell(intRowIndex, 1)
                    '    SetTableStyle(myRange, "N")
                    'End If
                    'If dta.Rows(b).Item("CustomerAddress2") & "" <> "" Then
                    '    intRowIndex = intRowIndex + 1
                    '    worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerAddress2") & ",")
                    '    myRange = worksheet.Cell(intRowIndex, 1)
                    '    SetTableStyle(myRange, "N")
                    'End If
                    'If dta.Rows(b).Item("CustomerCity") & "" <> "" Then
                    '    intRowIndex = intRowIndex + 1
                    '    worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerCity") & ",")
                    '    myRange = worksheet.Cell(intRowIndex, 1)
                    '    SetTableStyle(myRange, "N")
                    'End If 

                    'intRowIndex = intRowIndex + 1
                    'intRowIndex = intRowIndex + 1

                    worksheet.Cell(intRowIndex, 1).Value = "Kind Attention :- "
                    'If dta.Rows(b).Item("ContactPerson") & "" <> "" Then
                    '    worksheet.Cell(intRowIndex, 2).Value = Trim(dta.Rows(b).Item("ContactPerson") & "")
                    'End If

                    'intRowIndex = intRowIndex + 1
                    srStart = WriteContents(strFileSave & "Start_Content.txt")
                    While srStart.Peek >= 0
                        intRowIndex = intRowIndex + 1
                        worksheet.Cell(intRowIndex, 1).Value = srStart.ReadLine
                        myRange = worksheet.Cell(intRowIndex, 1)
                        SetTableStyle(myRange, "N")
                        'MergeCells("A", "H")
                    End While

                    intRowIndex = intRowIndex + 1
                    intRowIndex = intRowIndex + 1
                    srStart.Close()


                    strFields = Split(Trim(dta.Rows(b).Item("FieldId") & ""), ",")

                    'AddExcelQuotes(strFields)
                    Dim R As Int32
                    Dim S As Int32

                    Dim ds As DataTable
                    Dim dtFill As DataTable
                    OpenConn()
                    For R = 0 To strFields.Length - 1
                        Hid_FieldId.Value = Val(strFields(R))
                        ds = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                        intTotWidth = intTotWidth + Val(ds.Rows(0).Item("WordWidth") & "")
                        worksheet.Cell(intRowIndex, R + 1).Value = Trim(ds.Rows(0).Item("FaxField") & "")
                        myRange = worksheet.Cell(intRowIndex, R + 1)

                        SetTableStyle(myRange, "M")

                    Next

                    For R = 0 To dt.Rows.Count - 1
                        'intRowIndex = intRowIndex + 1
                        Hid_SecId.Value = Val(dt.Rows(R)("SecurityId").ToString())
                        Hid_SecTypeId.Value = Val(dt.Rows(R)("SecurityTypeId").ToString())
                        Hid_ShowNumber.Value = Trim(dt.Rows(R)("ShowNumber").ToString())
                        dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")
                        'Hid_RowNo.Value = Val(CType(dg_Selected.Rows(R).FindControl("lbl_RowNumber"), Label).Text)
                        If Val(Hid_SecId.Value) <> 0 Then
                            intRowIndex = intRowIndex + 1
                            dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")
                            If PrevSecTypeId <> Val(Hid_SecTypeId.Value) Then
                                For S = 0 To strFields.Length - 1
                                    myRange = worksheet.Cell(intRowIndex, S + 1)
                                    SetTableStyle(myRange, "SH")
                                Next
                                strExcelColumnName = ColumnName(S)
                                worksheet.Range("A" & intRowIndex & ":" & strExcelColumnName & intRowIndex).Merge()
                                str1 = CStr(Chr(strFields.Length + 1 + 63)) '& (intRowIndex + dg_Selected.Rows.Count))
                                myRange = worksheet.Cell(intRowIndex, strFields.Length)
                                myRange.Style.Font.Bold = True
                                'SetTableStyle(myRange, "M")
                                worksheet.Cell(intRowIndex, 1).Value = Trim(dtFill.Rows(0).Item("SecurityTypeName") & "")
                                worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
                                myRange = worksheet.Cell(intRowIndex, strFields.Length)
                                myRange.Style.Font.Bold = True
                                'SetTableStyle(myRange, "H")
                                intRowIndex = intRowIndex + 1

                            End If


                            Dim strFieldName As String
                            Dim strTableName As String
                            Dim strColName As String
                            Dim strFieldValue As String
                            Dim P As Int32
                            Dim Q As Int32
                            Dim intColRows As Int16
                            Dim intRowHeight As Int16
                            Dim dsFieldNames As DataTable
                            'Dim dt As DataTable
                            Dim strTemp As String
                            OpenConn()

                            Dim W As Single
                            Dim dtSel As DataTable
                            Dim DvSel As DataView = New DataView()

                            'strFieldValue = Trim(CType(dg_Selected.(intIndex).FindControl("lbl_" & strFieldName), Label).Text & "")
                            dtSel = TryCast(Session("TempSecurityTable"), DataTable)
                            dtSel.DefaultView.Sort = "OrderId Asc,SecurityId Asc"
                            dtSel = dtSel.DefaultView.ToTable()
                            DvSel = New DataView(dtSel)

                            DvSel.RowFilter = String.Empty
                            DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value
                            'DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value + " And Id=" + Hid_RowNo.Value
                            For P = 0 To strFields.Length - 1
                                Hid_FieldId.Value = Val(strFields(P))

                                dsFieldNames = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                                'dsFieldNames = objCommon.GetDataSet(SqlDataSourceFieldId)
                                With dsFieldNames.Rows(0)
                                    strTableName = Trim(.Item("TableName") & "")
                                    strFieldName = Trim(.Item("TableField") & "")
                                    If strFieldName = "Rate" Then intRateIndex = P
                                    If strFieldName = "SecurityName" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 45
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top

                                    ElseIf strFieldName = "NSDLFaceValue" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
                                    ElseIf strFieldName = "NSDLAcNumber" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 20
                                    ElseIf strFieldName = "MaturityDate" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 16.5
                                    ElseIf strFieldName = "CallDate" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
                                    ElseIf strFieldName = "Rating" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 18
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
                                    ElseIf strFieldName = "RatingOrg" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
                                    ElseIf strFieldName = "CouponRate" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
                                    ElseIf strFieldName = "Rate" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
                                    ElseIf strFieldName = "PutDate" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
                                    ElseIf strFieldName = "IPDates" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
                                    ElseIf strFieldName = "ShowNumber" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
                                    ElseIf strFieldName = "YTCAnn" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 13
                                    End If
                                    If strTableName <> "FromPage" Then
                                        For Q = 0 To dtFill.Columns.Count - 1
                                            strColName = dtFill.Columns(Q).ColumnName
                                            If UCase(strFieldName) = UCase(strColName) Then
                                                strTemp = ""
                                                If strColName = "IPDates" Then strTemp = " "

                                                If strColName = "MaturityDate" Or strColName = "PutDate" Or strColName = "SecurityInfoDate" Or strColName = "IssueDate" Or strColName = "FirstInterestDate" Or strColName = "BookClosureDate" Or strColName = "DMATBookClosureDate" Or strColName = "FirstInterestDate" Or strColName = "CallDate" Or strColName = "Call Dates" Or strColName = "SecCallDate" Then
                                                    worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-MMM-yyyy"
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                ElseIf strColName = "IPDates" Then
                                                    worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-MMM"
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")

                                                ElseIf strColName = "CouponRate" Then
                                                    If Trim(dtFill.Rows(0).Item(strColName) & "") <> "" Then
                                                        worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "") + "%"
                                                    End If

                                                ElseIf strColName = "SellingRate" Then
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "") + "%"
                                                Else
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                End If

                                                Exit For
                                            End If

                                            myRange = worksheet.Cell(intRowIndex, P + 1)
                                            SetTableStyle(myRange, "C", strColName)
                                        Next
                                    Else
                                        intColRows = 0
                                        If strFieldName = "ShowNumber" Then
                                            If Hid_ShowNumber.Value = "" Then
                                                strFieldValue = "N/A"
                                            Else
                                                strFieldValue = Hid_ShowNumber.Value
                                            End If

                                        Else
                                            If strFieldName = "Id" Then
                                                strFieldValue = R + 1
                                            Else
                                                strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
                                            End If

                                        End If

                                        If InStr(strFieldValue, "!") > 1 Then
                                            For W = 1 To Len(strFieldValue)
                                                If Mid$(strFieldValue, W, 1) = "!" Then
                                                    intColRows = intColRows + 1

                                                End If
                                            Next
                                            strFieldValue = Replace(Left(strFieldValue, Len(strFieldValue) - 1), "!", Chr(10))
                                            If intRowHeight < (13 * intColRows) Then intRowHeight = 13 * intColRows
                                        Else
                                            strFieldValue = Replace(strFieldValue, "!", "")
                                        End If
                                        'strFieldValue = Replace(strFieldValue, "!", Chr(10))
                                        strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
                                        If strFieldName = "SellingRate" Then
                                            worksheet.Cell(intRowIndex, P + 1).Value = strFieldValue '(String.Format(strFieldValue, "#############0.0000")) & ""
                                            worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "#,##0.0000"
                                        Else
                                            worksheet.Cell(intRowIndex, P + 1).Value = strFieldValue
                                        End If

                                        If strFieldName = "Yield" Or strFieldName = "YTCAnn" Or strFieldName = "YTPAnn" Or strFieldName = "YTMSemi" Or strFieldName = "YTPSemi" Or strFieldName = "YTCSemi" Or strFieldName = "CouponRate" Then
                                            If strFieldValue > 0 Then
                                                'worksheet.Cell(intRowIndex, P + 1).Value = strFieldValue + "%"

                                                worksheet.Cell(intRowIndex, P + 1).Value = (String.Format(objCommon.DecimalFormat4(strFieldValue), "#############0.0000")) & "%"


                                            Else
                                                worksheet.Cell(intRowIndex, P + 1).Value = "N/A"
                                            End If

                                        End If
                                        If strFieldValue <> "" Then
                                            myRange = worksheet.Cell(intRowIndex, P + 1)
                                            SetTableStyle(myRange, "C")

                                        End If

                                    End If

                                End With

                            Next
                            PrevSecTypeId = Val(dt.Rows(R)("SecurityTypeId").ToString())
                        End If

                    Next
                    'footer code
                    intRowIndex = intRowIndex + 2

                    worksheet.Cell(intRowIndex, 1).Value = "Kindly note that the above quotes are subject to availability of stock and prevailing market rates."
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = "Please confirm the stocks as early as possible to avoid stock availability and market volatility."
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = "For any further clarification please feel free to contact us at 09377417200"
                    worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = "For, SUNRISE GILTS & SECURITIES (P) LTD."
                    worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
                    intRowIndex = intRowIndex + 2
                    'worksheet.Cell(intRowIndex, 1).Value = "Corporate Office: 514,Pinnacle Business Park, Corporate Road, Prahlad Nagar,Ahmedabad - 380015. Gujarat"
                    'intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = "Registered Office: 514,Pinnacle Business Park, Corporate Road, Prahlad Nagar,Ahmedabad - 380015. Gujarat" & " Phone: +91 79 4032 7414 / 15,  4896 6870 ( 5 Line), Mobile : +91 9898658238,Fax:+ 91 79 4030 3249 " & vbCrLf & " Email : sunrisegilts@gmail.com  , info@sunrisegilts.com" & vbCrLf & "CIN No. : U67100GJ2013PTC077167  | SEBI Reg. No. : INZ0025734 | BSE Membership No, 4071 | NSE Membership No. 90076" & vbCrLf & "Branches : Kolkata ,Mumbai and New Delhi"
                    worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
                    worksheet.Cell(intRowIndex, 1).Style.Font.Italic = True
                    worksheet.Cell(intRowIndex, 1).Style.Alignment.WrapText = True
                    worksheet.Cell(intRowIndex, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    worksheet.Range("A" & intRowIndex & ":" & strExcelColumnName & intRowIndex).Merge()
                    worksheet.Row(intRowIndex).Height = 45
                    intRowIndex = intRowIndex + 2
                    intRowIndex = intRowIndex + 2

                    strUserContent = FillUserContent(srEnd)
                    'intRowIndex = intRowIndex + 2
                    worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
                    worksheet.Cell(intRowIndex, 1).Value = Session("NameOfUser")
                    myRange = worksheet.Cell(intRowIndex, 1)
                    SetTableStyle(myRange, "N")
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = strUserMobile
                    myRange = worksheet.Cell(intRowIndex, 1)
                    SetTableStyle(myRange, "N")
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = strUseremail
                    myRange = worksheet.Cell(intRowIndex, 1)
                    SetTableStyle(myRange, "N")

                    If strHeaderFile = "" Then
                        '  strHeaderFile = ConfigurationManager.AppSettings("BlankHeaderImage")
                        strHeaderFile = Hid_BlankHeaderImage.Value.ToString()
                    End If
                    If Convert.ToString(Session("LogoData")) <> "" Then
                        strMainPath = strFileSave + " HeaderPath1: " + strHeaderFile + " 1"
                        ImgByte = CType(Session("LogoData"), Byte())
                        Dim img As System.Drawing.Image = byteArrayToImage(ImgByte)
                        worksheet.AddPicture(img).MoveTo(worksheet.Cell(2, 8))
                    Else
                        worksheet.AddPicture(strFileSave & strHeaderFile).MoveTo(worksheet.Cell(2, 8))
                    End If



                    'If MyHandler.UserTypeId <> ConfigurationManager.AppSettings("DistributorId") Then
                    '    intRowIndex = intRowIndex + 2
                    '    worksheet.Cell(intRowIndex, 1).Value = "SMEST CAPITAL PRIVATE LIMITED" & vbCrLf & "Head Office: 5, Prabhat Kunj, 24th Road, Khar (W), Mumbai 400 052" & vbCrLf & "+91 22 26048050 | contact@smest.in | www.smest.in" & vbCrLf & "Corporate Identity Number: U74990MH2018PTC311416"
                    '    worksheet.Cell(intRowIndex, 1).RichText.Substring(0, 29).SetFontColor(XLColor.FromArgb(36, 64, 98))
                    '    worksheet.Cell(intRowIndex, 1).RichText.Substring(0, 29).SetFontName("calibri")
                    '    worksheet.Cell(intRowIndex, 1).RichText.Substring(0, 29).SetFontSize(10)
                    '    worksheet.Cell(intRowIndex, 1).RichText.Substring(0, 29).SetBold(True)

                    '    worksheet.Range("A" & intRowIndex & ":C" & intRowIndex).Merge()
                    '    worksheet.Row(intRowIndex).Height = 70
                    'End If



                    Dim I As Single
                    intTotWidth = 0
                    workbook.SaveAs(strDestination & "\" & "" & Regex.Replace(dta.Rows(b).Item("CustomerName"), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "") & "" & ".xlsx")
                    Dim ToEmailId As String
                    ToEmailId = Convert.ToString(dtCust.Rows(J).Item("EmailId"))

                    If chk_SendMail.Checked = True And ToEmailId <> "" Then
                        objcomm.SendOffer(Trim(dtCust.Rows(J).Item("CustomerName") & ""), strPath, ToEmailId, txt_CalcDate.Text, strUserName, strUserMobile)
                        ' SendMail(strPath, ToEmailId)
                    End If

                Next


                Using zip As New ZipFile()
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary
                    'zip.AddDirectoryByName("Admin")
                    zip.AddDirectory(strDestination)

                    Response.Clear()
                    Response.BufferOutput = False
                    'Dim zipName As String = [String].Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
                    Dim zipName As String = "Offer.zip"
                    Response.ContentType = "application/zip"
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
                    zip.Save(Response.OutputStream)
                    Response.End()
                End Using
            End If
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "select", "objExcel();", True)
        Catch ex As Exception
            'Dim file As System.IO.StreamWriter
            'file = My.Computer.FileSystem.OpenTextFileWriter("c:\test.txt", True)
            'file.WriteLine(ex.Message + strMainPath + "  " + System.DateTime.Now.ToString())
            'file.Close()
            objUtil.WritErrorLog(PgName, "btn_CreateExcelFax_Click", "Error in btn_CreateExcelFax_Click", "", ex)
        Finally
            CloseConn()
        End Try
    End Sub
    Public Sub CreatePDF_Customer()
        Try

            Dim imgHeader As iTextSharp.text.Image
            '   Dim imgFooter As iTextSharp.text.Image
            Dim strFileSave As String
            Dim srStart As StreamReader
            Dim srStart1 As StreamReader
            Dim srEnd As StreamReader = Nothing
            Dim srEnd1 As StreamReader
            Dim srEnd2 As StreamReader
            Dim strStart As String
            Dim strStart1 As String
            Dim strEnd As String
            Dim strEnd1 As String
            Dim strEnd2 As String
            Dim strEnd3 As String

            Dim strDate As String
            '  Dim objCommonFSO As Object
            Dim table1 As New iTextSharp.text.Table(2, 2)
            'Dim table2 As iTextSharp.text.Table
            Dim table3 As PdfPTable
            Dim para As iTextSharp.text.Paragraph
            Dim para1 As iTextSharp.text.Paragraph
            Dim para2 As iTextSharp.text.Paragraph
            Dim para3 As iTextSharp.text.Paragraph

            Dim strFontFilePath As String = Trim(ConfigurationManager.AppSettings("FontFilePath") & "")
            Dim strFontFilePath_CambriRegular As String = Trim(ConfigurationManager.AppSettings("FontFilePath_Cambria_Regular") & "")

            'Dim strFontFilePath As String = Hid_FontFIlePath.Value.ToString()
            'Dim strFontFilePath_CambriRegular As String = Hid_FontFIlePath_CambriaReg.Value.ToString()


            'Dim header As HeaderFooter
            '  Dim footer As HeaderFooter
            Dim strContPerson As String
            Dim strUserContent As String
            Dim strDestination As String
            Dim dtCust As DataTable
            Dim strOldCustName As String = ""
            Dim intCnt As Int16 = -1
            Dim arrContact(0) As String
            Dim arrIndex(0) As Int16
            Dim strCustCont As String = ""
            Dim blnSameCust As Boolean
            Dim intIndex As Int16
            Dim K As Int16
            Dim strCustPrefix As String = ""
            'Dim PDFFontSize As Integer = Convert.ToInt32(Hid_PDFFontSize.Value)
            Dim ImgByte As Byte() = Nothing

            'Dim bf As BaseFont = BaseFont.CreateFont(strFontFilePath & Hid_Calibri.Value, BaseFont.CP1252, BaseFont.EMBEDDED)
            'verdana = BaseFont.CreateFont(strFontFilePath & Hid_Cambrib.Value, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            'arial1 = BaseFont.CreateFont(strFontFilePath & Hid_Calibri.Value, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            'arial = BaseFont.CreateFont(strFontFilePath & Hid_Calibri.Value, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)

            'calibri = BaseFont.CreateFont(strFontFilePath & Hid_Calibri.Value, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            'calibrii = BaseFont.CreateFont(strFontFilePath & Hid_Calibrii.Value, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            'calibrib = BaseFont.CreateFont(strFontFilePath & Hid_Calibrib.Value, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            'calibribi = BaseFont.CreateFont(strFontFilePath & Hid_Calibribi.Value, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            'Dim FontColour = New iTextSharp.text.Color(System.Drawing.Color.White)
            'fontBold = New iTextSharp.text.Font(bf, PDFFontSize, iTextSharp.text.Font.BOLD, FontColour)
            'font = New iTextSharp.text.Font(arial1, PDFFontSize)
            'fontIn = New iTextSharp.text.Font(arial, PDFFontSize)
            'fontAddr = New iTextSharp.text.Font(arial, 8)
            'fontDealer = New iTextSharp.text.Font(bf, PDFFontSize)

            ''fontC = New iTextSharp.text.Font(calibri, PDFFontSize)
            ''fontCI = New iTextSharp.text.Font(calibrii, PDFFontSize)
            ''fontCB = New iTextSharp.text.Font(calibrib, PDFFontSize)
            ''fontCBI = New iTextSharp.text.Font(calibribi, PDFFontSize)

            'fontC = New iTextSharp.text.Font(calibri, PDFFontSize)
            'fontCI = New iTextSharp.text.Font(calibrii, PDFFontSize)
            'fontCB = New iTextSharp.text.Font(calibrib, PDFFontSize)
            'fontCBI = New iTextSharp.text.Font(calibribi, PDFFontSize)
            'fontLink = New iTextSharp.text.Font(calibri, PDFFontSize, 0, iTextSharp.text.Color.BLUE)
            verdana = BaseFont.CreateFont(strFontFilePath & "\verdanab.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            arial1 = BaseFont.CreateFont(strFontFilePath & "\verdanab.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            arial = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, False)
            '' '' '' ''font = New iTextSharp.text.Font(arial1, 7)
            '' '' '' ''fontIn = New iTextSharp.text.Font(arial, 8)
            '' '' '' ''fontAddr = New iTextSharp.text.Font(arial, 9)

            font = New iTextSharp.text.Font(arial1, 8)
            fontIn = New iTextSharp.text.Font(arial, 9)
            fontAddr = New iTextSharp.text.Font(arial, 9)
            strFileSave = ConfigurationManager.AppSettings("ImagePath") & "\"
            'strFileSave = Hid_pdfFilePath.Value.ToString()
            GetImages()
            strDestination = strFileSave & Session("UserName")

            Dim DI As New DirectoryInfo(strDestination)

            If DI.Exists = True Then
                DI.Delete(True)
            End If

            DI.Create()
            dtCust = TryCast(Session("CustomerContectTable"), DataTable)

            For K = 0 To dtCust.Rows.Count - 2
                If Trim(dtCust.Rows(K).Item("CustomerName") & "") = strOldCustName Then
                    strCustCont += Trim(dtCust.Rows(K).Item("ContactPerson") & "") & ","
                    arrContact(intCnt) = strCustCont
                    blnSameCust = True
                Else
                    strCustCont = Trim(dtCust.Rows(K).Item("ContactPerson") & "") & ","
                    intCnt = intCnt + 1
                    ReDim Preserve arrContact(intCnt)
                    ReDim Preserve arrIndex(intCnt)
                    arrContact(intCnt) = strCustCont
                    arrIndex(intCnt) = K
                    blnSameCust = False
                End If
                strOldCustName = Trim(dtCust.Rows(K).Item("CustomerName") & "")
            Next

            If blnSameCust = True Then
                arrContact(intCnt) = strCustCont
                arrIndex(intCnt) = K - 1
            End If

            For J As Int16 = 0 To dg_Customer.Rows.Count - 2

                If IsInArray(J, arrIndex) Then
                    ' Dim MyDocument As New iTextSharp.text.Document(PageSize.A4, 30, 30, 100, 80)
                    Dim MyDocument As New iTextSharp.text.Document(PageSize.A4.Rotate())
                    'MyDocument.SetMargins(20, 20, 110, 60)
                    MyDocument.SetMargins(20, 20, 30, 90)

                    'If Trim(dtCust.Rows(J).Item("CustPrefix") & "") <> "" Then
                    '    strCustPrefix = Trim(dtCust.Rows(J).Item("CustPrefix") & "")
                    '    If Not strCustPrefix.Contains(".") Then
                    '        strCustPrefix = strCustPrefix & ". "
                    '    End If
                    'End If

                    'Dim strPath = strDestination & "\" & Regex.Replace(strCustPrefix & Trim(dtCust.Rows(J).Item("CustomerName") & ""), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "") & ".pdf"
                    Dim strPath = strDestination & "\" & Regex.Replace(Trim(dtCust.Rows(J).Item("CustomerName") & ""), "[^0-9a-zA-Z .]+", "") & ".pdf"
                    Dim PdfWriter As PdfWriter = pdf.PdfWriter.GetInstance(MyDocument, New FileStream(strPath, FileMode.Create))
                    MyDocument.Open()

                    'Code For Header and Footer
                    'If blnHeaderExist = True Then
                    'khatija
                    strFooterFile = "Blank.bmp"
                    If strHeaderFile = "" Then
                        '  strHeaderFile = ConfigurationManager.AppSettings("BlankHeaderImage")
                        strHeaderFile = Hid_BlankHeaderImage.Value.ToString()
                    End If
                    'If Convert.ToString(Session("LogoData")) <> "" Then
                    '    ImgByte = CType(Session("LogoData"), Byte())
                    '    MyHandler.LogoByte = ImgByte
                    '    MyHandler.strHeaderPath = ""
                    'Else
                    MyHandler.strHeaderPath = strFileSave & strHeaderFile
                    'MyHandler.LogoByte = Nothing
                    'End If
                    MyHandler.strFooterPath = strFileSave & strFooterFile
                    Dim myEvents As MyHandler = New MyHandler
                    PdfWriter.PageEvent = myEvents
                    myEvents.onStartPage(PdfWriter, MyDocument)

                    'End If

                    Hid_SelectedFields.Value = Trim(dtCust.Rows(J).Item("FieldId") & "")
                    strContPerson = arrContact(J)

                    strDate = "Date: " & String.Format(Today, "dd/MM/yyyy")


                    'code for header and footer content

                    srStart = WriteContents(strFileSave & "Start_Content.txt")
                    srStart1 = WriteContents(strFileSave & "Start_ContentA.txt")
                    'If Session("CompId") = 1 Then
                    srEnd = WriteContents(strFileSave & "End_Content.txt")
                    'End If



                    strStart = srStart.ReadToEnd
                    strStart1 = srStart1.ReadToEnd
                    strUserContent = FillUserContent(srEnd)
                    'srEnd.Close()
                    srEnd = Nothing




                    strEnd = Replace(strEnd, "In case you need any clarification, please contact", strUserContent)
                    strEnd1 = strUserContent

                    Hid_SelectedFields.Value = Trim(dtCust.Rows(J).Item("FieldId") & "")
                    'table2 = AddPDFTable()

                    'code to create table and fill data
                    table3 = AddPDFPTable()

                    'code for date top right align
                    ' MyDocument.Add(New iTextSharp.text.Paragraph("                        "))
                    MyDocument.Add(New iTextSharp.text.Paragraph(""))
                    para = New iTextSharp.text.Paragraph(strDate, fontIn)
                    para.SetAlignment("left")
                    'MyDocument.Add(para)



                    MyDocument.Add(New iTextSharp.text.Paragraph("To,", fontIn))
                    MyDocument.Add(New iTextSharp.text.Paragraph("The Investment Department", fontIn))

                    'Remove Client Address & Kind Atten. from All Quotes - 21AUG2020
                    'If dtCust.Rows(J).Item("CustomerAddress1") & "" <> "" Then
                    '    MyDocument.Add(New iTextSharp.text.Paragraph(Trim(dtCust.Rows(J).Item("CustomerAddress1") & ""), fontIn))
                    'End If

                    'If dtCust.Rows(J).Item("CustomerAddress2") & "" <> "" Then
                    '    MyDocument.Add(New iTextSharp.text.Paragraph(Trim(dtCust.Rows(J).Item("CustomerAddress2") & ""), fontIn))
                    'End If

                    'If dtCust.Rows(J).Item("CustomerCity") & "" <> "" Then
                    '    MyDocument.Add(New iTextSharp.text.Paragraph(Trim(dtCust.Rows(J).Item("CustomerCity") & ""), fontIn))
                    'End If

                    MyDocument.Add(New iTextSharp.text.Paragraph("Kind Attn : - Mr. ", fontIn))
                    MyDocument.Add(New iTextSharp.text.Paragraph("We are pleased to Update you the following securities for your  Investment. ", fontIn))
                    'MyDocument.Add(New iTextSharp.text.Paragraph(Trim(dtCust.Rows(K).Item("CustomerName") & ""), fontIn))
                    ''MyDocument.Add(New iTextSharp.text.Paragraph("KIND ATTN :- " & arrContact(J), fontIn))
                    'MyDocument.Add(New iTextSharp.text.Paragraph("", fontIn))
                    'MyDocument.Add(New iTextSharp.text.Paragraph(strStart, fontIn))
                    'MyDocument.Add(New iTextSharp.text.Paragraph("    ", fontIn))
                    'MyDocument.Add(table2)

                    MyDocument.Add(New iTextSharp.text.Paragraph("    ", fontIn))
                    MyDocument.Add(New iTextSharp.text.Paragraph("    ", fontIn))
                    MyDocument.Add(table3)
                    MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontIn))

                    'MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontIn))
                    'MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontIn))
                    MyDocument.Add(New iTextSharp.text.Paragraph("Kindly note that the above quotes are subject to availability of stock and prevailing market rates.", fontIn))
                    MyDocument.Add(New iTextSharp.text.Paragraph("Please confirm the stocks as early as possible to avoid stock availability and market volatility.  ", fontIn))
                    'MyDocument.Add(New iTextSharp.text.Paragraph("The above mentioned offer(s) are indicative and subject to changes in market conditions. The above offers should not be construed to be solicitation to invest, buy or sell. The decision to invest would solely be the responsibility of the investor, after taking into account all relevant risk factors. We are not responsible for the independent investment decisions, made by the recipient of these offer(s) and investor(s). We would be obliged if you could process the offer at the earliest. Kindly revert to the undersigned for any further clarifications.", fontIn))

                    MyDocument.Add(New iTextSharp.text.Paragraph("For any further clarification please feel free to contact us at 09377417200  ", fontIn))
                    MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontIn))
                    MyDocument.Add(New iTextSharp.text.Paragraph("For, SUNRISE GILTS & SECURITIES (P) LTD.  ", fontIn))

                    'MyDocument.Add(New iTextSharp.text.Paragraph(strEnd1, fontIn))



                    'srStart.Close()
                    'srEnd.Close()
                    MyDocument.Close()
                    MyDocument = Nothing

                    Dim ToEmailId As String
                    ToEmailId = Convert.ToString(dtCust.Rows(J).Item("EmailId"))

                    If chk_SendMail.Checked = True And ToEmailId <> "" Then
                        objcomm.SendOffer(Trim(dtCust.Rows(J).Item("CustomerName") & ""), strPath, ToEmailId, txt_CalcDate.Text, strUserName, strUserMobile)
                        ' SendMail(strPath, ToEmailId)
                    End If
                End If
            Next


            Using zip As New ZipFile()
                zip.AlternateEncodingUsage = ZipOption.AsNecessary
                'zip.AddDirectoryByName("Admin")
                zip.AddDirectory(strDestination)

                Response.Clear()
                Response.BufferOutput = False
                'Dim zipName As String = [String].Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
                Dim zipName As String = "Offer.zip"
                Response.ContentType = "application/zip"
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
                zip.Save(Response.OutputStream)
                Response.End()
            End Using

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_CreatePDFFax_Click", "Error in btn_CreatePDFFax_Click", "", ex)
            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter("c:\test.txt", True)
            file.WriteLine(ex.Message)
            file.Close()
            ' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub
    Public Shared Function byteArrayToImage(ByVal byteArrayIn As Byte()) As System.Drawing.Image
        Dim ms As New MemoryStream(byteArrayIn)
        Dim returnImage As System.Drawing.Image = System.Drawing.Image.FromStream(ms)
        Return returnImage
    End Function
    Function ColumnName(ByVal index As Integer) As String
        Static chars() As Char = {"A"c, "B"c, "C"c, "D"c, "E"c, "F"c, "G"c, "H"c, "I"c, "J"c, "K"c, "L"c, "M"c, "N"c, "O"c, "P"c, "Q"c, "R"c, "S"c, "T"c, "U"c, "V"c, "W"c, "X"c, "Y"c, "Z"c}

        index -= 1 ''//adjust so it matches 0-indexed array rather than 1-indexed column

        Dim quotient As Integer = index \ 26 ''//normal / operator rounds. \ does integer division, which truncates
        If quotient > 0 Then
            ColumnName = ColumnName(quotient) & chars(index Mod 26)
        Else
            ColumnName = chars(index Mod 26)
        End If
    End Function
    Protected Sub TestButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TestButton.Click
        Try
            If dg_Customer.Rows.Count < 2 Then
                BlankExcelFax()
            Else
                Dim dt As DataTable
                Dim dtCust As DataTable
                Dim strDestination As String
                Dim strOldCustName As String = ""
                Dim intCnt As Int16 = -1
                Dim arrContact(0) As String
                Dim arrIndex(0) As Int16
                Dim strCustCont As String = ""
                Dim strFileOfferSave As String
                Dim K As Int16
                Dim J As Int16
                Dim srStart As StreamReader
                Dim srEnd As StreamReader = Nothing
                Dim strFields() As String
                Dim strFileSave As String
                Dim strUserContent As String
                Dim strEnd As String
                Dim strEnd2 As String
                Dim srEnd1 As StreamReader
                Dim CustomerName As String = ""
                Dim PrevSecTypeId As Integer
                Dim str1 As String
                Dim dta As DataTable = TryCast(Session("CustomerContectTable"), DataTable)
                GetImages()

                strFileSave = ConfigurationManager.AppSettings("ImagePath") & "\"
                strFileOfferSave = ConfigurationManager.AppSettings("ImagePath") & "\CentNewLogo.bmp"
                strDestination = strFileSave & Session("UserName")


                Dim DI As New DirectoryInfo(strDestination)
                If DI.Exists = True Then
                    DI.Delete(True)
                End If
                DI.Create()
                Dim d1 As DataTable = objCommon.FillDataTable(sqlConn, "ID_SELECT_BranchMaster", Session("Branchid"), "Branchid")

                For b As Int32 = 0 To dta.Rows.Count - 2
                    Dim strppp As String = (Server.MapPath("").Replace("Forms", "Temp") & "\OfferExcel.xlsx")
                    Dim workbook = New XLWorkbook(strppp)
                    Dim worksheet = workbook.Worksheet(1)

                    'Dim worksheet = workbook.Add("Sample Sheet")
                    Dim myRange As ClosedXML.Excel.IXLCell
                    Dim strPath As String = ""
                    Dim sImagePath As String = strFileOfferSave

                    Dim strFileName As String = (Server.MapPath("").Replace("Forms", "Temp") & "\Report12.xlsx")
                    Dim custname As String = ""

                    dt = CType(Session("TempSecurityTable"), DataTable)
                    Dim view As DataView = New DataView(dt)
                    Dim distinctValues As DataTable = view.ToTable(True, "SecurityTypeId")
                    dtCust = TryCast(Session("CustomerContectTable"), DataTable)

                    If IsInArray(J, arrIndex) Then
                        intRowIndex = 4
                    End If
                    intRowIndex = 7
                    myRange = worksheet.Cell(intRowIndex - 4, 8)
                    myRange.Style.Font.Bold = True
                    myRange = worksheet.Cell(intRowIndex - 1, 8)
                    myRange.Style.Font.Bold = True
                    worksheet.Cell(intRowIndex - 4, 8).Value = "Centrum Capital Limited"
                    worksheet.Cell(intRowIndex - 3, 8).Value = "Centrum House, 8th Floor, CST Road, "
                    worksheet.Cell(intRowIndex - 2, 8).Value = "Kalina, Santacruz (E), Mumbai - 400 098"
                    worksheet.Cell(intRowIndex - 1, 8).Value = "Ph: 42159066 / 42159060 Fax: 42159444"

                    'intRowIndex = intRowIndex + 1
                    intRowIndex = 2

                    worksheet.Cell(intRowIndex, 1).Value = "Date : " & String.Format(Today, "dd/MM/yyyy")
                    intRowIndex = intRowIndex + 2

                    worksheet.Cell(intRowIndex, 1).Value = "To,"

                    If dta.Rows(b).Item("CustomerName") & "" <> "" Then
                        intRowIndex = intRowIndex + 1
                        worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerName") & ",")
                        custname = Trim(dta.Rows(b).Item("CustomerName") & "") & ","
                    End If

                    If dta.Rows(b).Item("CustomerAddress1") & "" <> "" Then
                        intRowIndex = intRowIndex + 1
                        worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerAddress1") & ",")
                    End If
                    If dta.Rows(b).Item("CustomerAddress2") & "" <> "" Then
                        intRowIndex = intRowIndex + 1
                        worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerAddress2") & ",")
                    End If
                    If dta.Rows(b).Item("CustomerCity") & "" <> "" Then
                        intRowIndex = intRowIndex + 1
                        worksheet.Cell(intRowIndex, 1).Value = Trim(dta.Rows(b).Item("CustomerCity") & ",")
                    End If

                    intRowIndex = intRowIndex + 1
                    intRowIndex = intRowIndex + 1

                    worksheet.Cell(intRowIndex, 1).Value = "Kind Attention :- "
                    If dta.Rows(b).Item("ContactPerson") & "" <> "" Then
                        worksheet.Cell(intRowIndex, 2).Value = Trim(dta.Rows(b).Item("ContactPerson") & "")
                    End If

                    intRowIndex = intRowIndex + 1
                    srStart = WriteContents(strFileSave & "Start_Content.txt")
                    While srStart.Peek >= 0
                        intRowIndex = intRowIndex + 1
                        'worksheet.Cell(intRowIndex, 1).Value = srStart.ReadLine
                        'MergeCells("A", "H")
                    End While

                    intRowIndex = intRowIndex + 1
                    intRowIndex = intRowIndex + 1
                    srStart.Close()

                    For J = 0 To dg_Customer.Rows.Count - 2
                        strFields = Split(Trim(dtCust.Rows(J).Item("FieldId") & ""), ",")
                    Next

                    'AddExcelQuotes(strFields)
                    Dim R As Int32
                    Dim ds As DataTable
                    Dim dtFill As DataTable
                    OpenConn()
                    For R = 0 To strFields.Length - 1
                        Hid_FieldId.Value = Val(strFields(R))
                        ds = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                        intTotWidth = intTotWidth + Val(ds.Rows(0).Item("WordWidth") & "")
                        worksheet.Cell(intRowIndex, R + 1).Value = Trim(ds.Rows(0).Item("FaxField") & "")
                        myRange = worksheet.Cell(intRowIndex, R + 1)

                        SetTableStyle(myRange, "H")

                    Next

                    For R = 0 To dg_Selected.Rows.Count - 1
                        'intRowIndex = intRowIndex + 1
                        Hid_SecId.Value = Val(CType(dg_Selected.Rows(R).FindControl("lbl_SecurityId"), Label).Text)
                        Hid_SecTypeId.Value = Val(CType(dg_Selected.Rows(R).FindControl("lbl_SecurityTypeId"), Label).Text)
                        dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")

                        If Val(Hid_SecId.Value) <> 0 Then
                            intRowIndex = intRowIndex + 1
                            dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")
                            'If PrevSecTypeId <> Val(Hid_SecTypeId.Value) Then
                            '    str1 = CStr(Chr(strFields.Length + 1 + 63)) '& (intRowIndex + dg_Selected.Rows.Count))
                            '    myRange = worksheet.Cell(intRowIndex, strFields.Length)
                            '    myRange.Style.Font.Bold = True
                            '    SetTableStyle(myRange, "H")
                            '    worksheet.Cell(intRowIndex, 1).Value = Trim(dtFill.Rows(0).Item("SecurityTypeName") & "")
                            '    worksheet.Cell(intRowIndex, 1).style.font.bold = True
                            '    myRange = worksheet.Cell(intRowIndex, strFields.Length)
                            '    myRange.Style.Font.Bold = True
                            '    SetTableStyle(myRange, "H")
                            '    intRowIndex = intRowIndex + 1

                            'End If


                            Dim strFieldName As String
                            Dim strTableName As String
                            Dim strColName As String
                            Dim strFieldValue As String
                            Dim P As Int32
                            Dim Q As Int32
                            Dim intColRows As Int16
                            Dim intRowHeight As Int16
                            Dim dsFieldNames As DataTable
                            'Dim dt As DataTable
                            Dim strTemp As String
                            OpenConn()

                            Dim W As Single
                            Dim dtSel As DataTable
                            Dim DvSel As DataView = New DataView()

                            'strFieldValue = Trim(CType(dg_Selected.(intIndex).FindControl("lbl_" & strFieldName), Label).Text & "")
                            dtSel = TryCast(Session("TempSecurityTable"), DataTable)
                            DvSel = New DataView(dtSel)

                            DvSel.RowFilter = String.Empty
                            DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value

                            For P = 0 To strFields.Length - 1
                                Hid_FieldId.Value = Val(strFields(P))

                                dsFieldNames = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                                'dsFieldNames = objCommon.GetDataSet(SqlDataSourceFieldId)
                                With dsFieldNames.Rows(0)
                                    strTableName = Trim(.Item("TableName") & "")
                                    strFieldName = Trim(.Item("TableField") & "")
                                    If strFieldName = "Rate" Then intRateIndex = P
                                    If strTableName <> "FromPage" Then
                                        For Q = 0 To dtFill.Columns.Count - 1
                                            strColName = dtFill.Columns(Q).ColumnName
                                            If UCase(strFieldName) = UCase(strColName) Then
                                                strTemp = ""
                                                If strColName = "IPDates" Then strTemp = " "
                                                If strColName = "MaturityDate" Or strColName = "PutDate" Or strColName = "SecurityInfoDate" Or strColName = "IssueDate" Or strColName = "FirstInterestDate" Or strColName = "BookClosureDate" Or strColName = "DMATBookClosureDate" Or strColName = "FirstInterestDate" Or strColName = "CallDate" Or strColName = "Call Dates" Then
                                                    worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-mmm-yyyy"
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                ElseIf strColName = "IPDates" Then
                                                    worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-mmm"
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                    'If Trim(dtFill.Rows(0).Item(strColName) & "") <> "" Then
                                                    '    If Trim(dtFill.Rows(0).Item(strColName) & "").Contains(",") Then
                                                    '        worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                    '    Else
                                                    '        strFieldValue = Trim(dtFill.Rows(0).Item(strColName) & "") & "," & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                    '        worksheet.Cell(intRowIndex, P + 1).Value = strTemp & strFieldValue
                                                    '    End If
                                                    'End If
                                                Else
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                End If

                                                Exit For
                                            End If
                                            myRange = worksheet.Cell(intRowIndex, P + 1)
                                            SetTableStyle(myRange)

                                        Next
                                    Else
                                        intColRows = 0
                                        strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
                                        'strFieldValue = Trim(dtSel.Rows(intIndex).Item(strFieldName) & "")

                                        If InStr(strFieldValue, "!") > 1 Then
                                            For W = 1 To Len(strFieldValue)
                                                If Mid$(strFieldValue, W, 1) = "!" Then
                                                    intColRows = intColRows + 1

                                                End If
                                            Next
                                            strFieldValue = Replace(Left(strFieldValue, Len(strFieldValue) - 1), "!", Chr(10))
                                            If intRowHeight < (13 * intColRows) Then intRowHeight = 13 * intColRows
                                        Else
                                            strFieldValue = Replace(strFieldValue, "!", "")
                                        End If
                                        'strFieldValue = Replace(strFieldValue, "!", Chr(10))
                                        strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
                                        'oSheet.Cells(intRowIndex, P + 1).Value = strFieldValue
                                        'writer.WriteCell(intRowIndex, P + 1, strFieldValue)
                                        worksheet.Cell(intRowIndex, P + 1).Value = strFieldValue
                                        If strFieldValue <> "" Then
                                            myRange = worksheet.Cell(intRowIndex, P + 1)
                                            SetTableStyle(myRange)

                                        End If

                                    End If

                                End With

                            Next
                            PrevSecTypeId = Val(CType(dg_Selected.Rows(R).FindControl("lbl_SecurityTypeId"), Label).Text)
                        End If

                    Next
                    'footer code
                    intRowIndex = intRowIndex + 1
                    srEnd = WriteContents(strFileSave & "End_Content.txt")
                    If Val(Session("BranchId") & "") = 24 Then
                        srEnd1 = WriteContents(strFileSave & "End_ContentB.txt")

                    Else
                        srEnd1 = WriteContents(strFileSave & "End_ContentA.txt")

                    End If
                    strEnd = srEnd.ReadToEnd
                    strEnd2 = srEnd1.ReadToEnd
                    strUserContent = FillUserContent(srEnd)

                    srEnd.Close()
                    srEnd = Nothing

                    srEnd1.Close()
                    srEnd1 = Nothing
                    srEnd = WriteContents(strFileSave & "End_Content.txt")

                    If Val(Session("BranchId") & "") = 24 Then
                        srEnd1 = WriteContents(strFileSave & "End_ContentB.txt")
                    Else
                        srEnd1 = WriteContents(strFileSave & "End_ContentA.txt")
                    End If
                    While srEnd.Peek >= 0
                        intRowIndex = intRowIndex + 1
                        strEnd = srEnd.ReadLine
                        If strEnd.IndexOf("Centrum Capital") <> -1 Then

                            worksheet.Cell(intRowIndex, 1).Value = strEnd
                            myRange = worksheet.Cell(intRowIndex, 1)
                            myRange.Style.Font.Bold = True
                            'worksheet.Cell(intRowIndex, 1).Font.Bold = True
                        Else
                            worksheet.Cell(intRowIndex, 1).Value = strEnd
                        End If

                        'MergeCells("A", "H")
                    End While

                    intRowIndex = intRowIndex - 4
                    worksheet.Cell(intRowIndex, 1).Value = strUserContent
                    Dim I As Single
                    intTotWidth = 0
                    workbook.SaveAs(strDestination & "\" & dta.Rows(b).Item("CustomerName") & "" & ".xlsx")

                    HttpContext.Current.Response.Clear()
                    HttpContext.Current.Response.Buffer = True
                    HttpContext.Current.Response.Charset = ""
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" & dta.Rows(b).Item("CustomerName") & "" & ".xlsx")
                    HttpContext.Current.Response.Charset = ""
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Dim MyMemoryStream As MemoryStream = New MemoryStream()
                    'MemoryStream(MyMemoryStream = New MemoryStream())
                    workbook.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream)
                    HttpContext.Current.Response.Flush()
                    HttpContext.Current.Response.End()

                    'Dim strParams(1) As String
                    'strParams(0) = strDestination
                    'strParams(1) = strDestination & "\Offer.zip"
                    'Dim ShlZip As New ShellZip
                    'ShlZip.Compress(strDestination & "\Offer.zip", strDestination)
                    'System.Threading.Thread.Sleep(500)
                    'With HttpContext.Current.Response
                    '    .Clear()
                    '    .Charset = ""
                    '    .ClearHeaders()
                    '    '.ContentType = "application/vnd.ms-excel"
                    '    .ContentType = "application/x-zip-compressed"
                    '    .AddHeader("content-disposition", "attachment;filename=Offer.zip")
                    '    .WriteFile(strDestination & "\Offer.zip")
                    '    .Flush()
                    '    .End()
                    'End With
                Next
            End If
            'Page.ClientScript.RegisterStartupScript(Me.GetType, "select", "objExcel();", True)
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Sub




    Public Sub BlankExcelFax()
        Try

            Dim dt As DataTable
            Dim dtCust As DataTable
            Dim strDestination As String
            Dim strOldCustName As String = ""
            Dim intCnt As Int16 = -1
            Dim arrContact(0) As String
            Dim arrIndex(0) As Int16
            Dim strCustCont As String = ""
            Dim strFileOfferSave As String
            Dim K As Int16
            Dim J As Int16
            Dim srStart As StreamReader
            Dim srEnd As StreamReader = Nothing
            Dim strStartRemark As StreamReader
            Dim strFields() As String
            Dim strFileSave As String
            Dim strUserContent As String
            Dim strEnd As String
            Dim strEnd2 As String
            Dim srEnd1 As StreamReader
            Dim CustomerName As String = ""
            Dim PrevSecTypeId As Integer
            Dim str1 As String
            Dim dta As DataTable = TryCast(Session("CustomerContectTable"), DataTable)
            ' GetImages()
            Dim strDate_ As String = Date.Today.ToString("dd MMMM yyyy")
            Dim strDate1 As String

            strDate1 = Date.Today.AddDays(1).ToString("dd MMMM yyyy")


            strFileSave = ConfigurationManager.AppSettings("ImagePath") & "\"
            strFileOfferSave = ConfigurationManager.AppSettings("ImagePath") & "\CentLogo.bmp"
            strDestination = strFileSave & Session("UserName")
            Dim dtNew As DataTable

            Dim DI As New DirectoryInfo(strDestination)
            If DI.Exists = True Then
                DI.Delete(True)
            End If
            DI.Create()
            Dim d1 As DataTable = objCommon.FillDataTable(sqlConn, "ID_SELECT_BranchMaster", Session("Branchid"), "Branchid")

            'For b As Int32 = 0 To dta.Rows.Count - 2
            Dim strppp As String = (Server.MapPath("").Replace("Forms", "Temp") & "\OfferExcel.xlsx")
            Dim workbook = New XLWorkbook(strppp)
            Dim worksheet = workbook.Worksheet(1)

            'Dim worksheet = workbook.Add("Sample Sheet")
            Dim myRange As ClosedXML.Excel.IXLCell
            Dim strPath As String = ""
            Dim sImagePath As String = strFileOfferSave

            Dim strFileName As String = (Server.MapPath("").Replace("Forms", "Temp") & "\Report12.xlsx")
            Dim custname As String = ""

            dt = CType(Session("TempSecurityTable"), DataTable)
            Dim view As DataView = New DataView(dt)
            Dim distinctValues As DataTable = view.ToTable(True, "SecurityTypeId")

            'worksheet.AddPicture(ConfigurationManager.AppSettings("ImagePath") & "\AUMLogo.PNG")
            If IsInArray(J, arrIndex) Then
                intRowIndex = 6
            End If

            intRowIndex = 10
            myRange = worksheet.Cell(intRowIndex - 5, 3)
            myRange.Style.Font.Bold = True
            myRange = worksheet.Cell(intRowIndex - 1, 3)
            myRange.Style.Font.Bold = True

            worksheet.Cell(intRowIndex - 5, 8).Value = "Centrum Capital Limited"
            worksheet.Cell(intRowIndex - 4, 8).Value = "Centrum House, 7th Floor, CST Road, "
            worksheet.Cell(intRowIndex - 3, 8).Value = "Kalina, Santacruz (E), Mumbai - 400 098"
            worksheet.Cell(intRowIndex - 2, 8).Value = "Ph: 42159066 / 42159060 Fax: 42159444"


            intRowIndex = intRowIndex + 1
            intRowIndex = intRowIndex + 1

            intRowIndex = intRowIndex + 1
            worksheet.Cell(intRowIndex + 1, 11).Value = "Date : " & strDate_
            intRowIndex = intRowIndex + 2
            intRowIndex = intRowIndex + 1
            intRowIndex = intRowIndex + 1
            intRowIndex = intRowIndex + 1
            srStart = WriteContents(strFileSave & "Start_Content.txt")
            While srStart.Peek >= 0
                intRowIndex = intRowIndex + 1
                'worksheet.Cell(intRowIndex, 1).Value = srStart.ReadLine
                'MergeCells("A", "H")
                worksheet.Range("A" & intRowIndex & ":M" & intRowIndex).Merge()
            End While

            intRowIndex = intRowIndex + 1
            intRowIndex = intRowIndex + 1
            srStart.Close()

            'For J = 0 To dg_Customer.Rows.Count - 2
            '    strFields = Split("1,30,21,64,10,3,4,5,11,26", ",")
            'Next
            strFields = Split("1,30,21,64,10,3,4,5,11,2,26", ",")

            dtNew = dt.Copy()
            dtNew.DefaultView.Sort = "TypeFlag Asc, SecurityTypeId Asc, CreditRating Asc"
            dtNew = dtNew.DefaultView.ToTable()
            dtNew = RemoveEmptyRowsFromDataTable(dtNew)
            ' dtNew = RemoveDuplicates(dtNew)

            'AddExcelQuotes(strFields)
            Dim R As Int32
            Dim ds As DataTable
            Dim dtFill As DataTable
            OpenConn()
            For R = 0 To strFields.Length - 1
                Hid_FieldId.Value = Val(strFields(R))
                ds = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                intTotWidth = intTotWidth + Val(ds.Rows(0).Item("WordWidth") & "")
                worksheet.Cell(intRowIndex, R + 1).Value = Trim(ds.Rows(0).Item("FaxField") & "")
                myRange = worksheet.Cell(intRowIndex, R + 1)

                SetTableStyle(myRange, "H")

            Next

            For R = 0 To dtNew.Rows.Count - 1
                'intRowIndex = intRowIndex + 1
                Hid_SecId.Value = Val(dtNew.Rows(R)("SecurityId").ToString())
                Hid_SecTypeId.Value = Val(dtNew.Rows(R)("SecurityTypeId").ToString())
                Dim strTaxFree As String = "N"

                'dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")

                dtFill = GetTable("ID_FILL_SecurityInfo", Hid_SecId.Value, strTaxFree)
                If Val(Hid_SecId.Value) <> 0 Then
                    intRowIndex = intRowIndex + 1
                    'dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")

                    dtFill = GetTable("ID_FILL_SecurityInfo", Hid_SecId.Value, strTaxFree)
                    'If PrevSecTypeId <> Val(Hid_SecTypeId.Value) Then
                    '    str1 = CStr(Chr(strFields.Length + 1 + 63)) '& (intRowIndex + dg_Selected.Rows.Count))
                    '    'here
                    '    myRange = worksheet.Cell(intRowIndex, strFields.Length)
                    '    myRange.Style.Font.Bold = True
                    '    SetTableStyle(myRange, "H")
                    '    worksheet.Cell(intRowIndex, 1).Value = Trim(dtFill.Rows(0).Item("SecurityTypeName") & "")
                    '    myRange = worksheet.Cell(intRowIndex, strFields.Length)
                    '    myRange.Style.Font.Bold = True
                    '    worksheet.Range("A" & intRowIndex & ":J" & intRowIndex).Style.Font.Bold = True
                    '    worksheet.Range("A" & intRowIndex & ":J" & intRowIndex).Merge()
                    '    intRowIndex = intRowIndex + 1
                    'End If

                    Dim strFieldName As String
                    Dim strTableName As String
                    Dim strColName As String
                    Dim strFieldValue As String
                    Dim P As Int32
                    Dim Q As Int32
                    Dim intColRows As Int16
                    Dim intRowHeight As Int16
                    Dim dsFieldNames As DataTable
                    'Dim dt As DataTable
                    Dim strTemp As String
                    OpenConn()

                    Dim W As Single
                    Dim dtSel As DataTable
                    Dim DvSel As DataView = New DataView()

                    'strFieldValue = Trim(CType(dg_Selected.(intIndex).FindControl("lbl_" & strFieldName), Label).Text & "")
                    dtSel = TryCast(Session("TempSecurityTable"), DataTable)
                    DvSel = New DataView(dtSel)

                    DvSel.RowFilter = String.Empty
                    DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value

                    For P = 0 To strFields.Length - 1
                        Hid_FieldId.Value = Val(strFields(P))

                        dsFieldNames = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                        'dsFieldNames = objCommon.GetDataSet(SqlDataSourceFieldId)
                        With dsFieldNames.Rows(0)
                            strTableName = Trim(.Item("TableName") & "")
                            strFieldName = Trim(.Item("TableField") & "")
                            If strFieldName = "SellingRate" Then intRateIndex = P
                            If strTableName <> "FromPage" Then
                                For Q = 0 To dtFill.Columns.Count - 1
                                    strColName = dtFill.Columns(Q).ColumnName
                                    If UCase(strFieldName) = UCase(strColName) Then
                                        strTemp = ""
                                        If strColName = "IPDates" Then strTemp = " "
                                        ' worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                        ' Exit For

                                        If strColName = "CouponRate" Then
                                            If IsDBNull(dtFill.Rows(0).Item(strColName)) Then
                                                worksheet.Cell(intRowIndex, P + 1).Value = ""
                                            Else
                                                worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(String.Format(objCommon.DecimalFormat4(dtFill.Rows(0).Item(strColName) & ""), "#############0.00")) & "%"
                                            End If

                                        ElseIf strColName = "MaturityDate" Or strColName = "PutDate" Or strColName = "SecurityInfoDate" Or strColName = "IssueDate" Or strColName = "FirstInterestDate" Or strColName = "BookClosureDate" Or strColName = "DMATBookClosureDate" Or strColName = "FirstInterestDate" Or strColName = "CallDate" Or strColName = "PutDate" Or strColName = "Call Dates" Then
                                            worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-mmm-yyyy"
                                            worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                        ElseIf strColName = "IPDates" Then
                                            If Trim(dtFill.Rows(0).Item(strColName) & "") <> "" Then
                                                worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-mmm"
                                                worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                'If Trim(dtFill.Rows(0).Item(strColName) & "").Contains(",") Then
                                                '    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                'Else
                                                '    strFieldValue = Trim(dtFill.Rows(0).Item(strColName) & "") & "," & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                '    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & strFieldValue
                                                'End If
                                            End If
                                        Else
                                            worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                        End If
                                    End If
                                    myRange = worksheet.Cell(intRowIndex, P + 1)
                                    SetTableStyle(myRange)

                                Next
                            Else
                                intColRows = 0
                                strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
                                'strFieldValue = Trim(dtSel.Rows(intIndex).Item(strFieldName) & "")

                                If InStr(strFieldValue, "!") > 1 Then
                                    For W = 1 To Len(strFieldValue)
                                        If Mid$(strFieldValue, W, 1) = "!" Then
                                            intColRows = intColRows + 1

                                        End If
                                    Next
                                    strFieldValue = Replace(Left(strFieldValue, Len(strFieldValue) - 1), "!", Chr(10))
                                    If intRowHeight < (13 * intColRows) Then intRowHeight = 13 * intColRows
                                ElseIf strFieldName = "YTMAnn" Or strFieldName = "Yield" Or strFieldName = "YTCAnn" Or strFieldName = "YTPAnn" Or strFieldName = "YTMSemi" Or
                                          strFieldName = "YTCSemi" Or strFieldName = "YTPSemi" Or strFieldName = "CouponRate" Then
                                    strFieldValue = Replace(strFieldValue, "!", "") & "%"
                                    ' strFieldValue = (String.Format(objCommon.DecimalFormat4(strFieldValue), "#############0.00")) & "%"
                                ElseIf strFieldName = "SellingRate" Then
                                    'strFieldValue = "Rs." & String.Format(objCommon.DecimalFormat4(strFieldValue), "#############0.00") & " +A.I"
                                    strFieldValue = String.Format(objCommon.DecimalFormat4(strFieldValue), "#############0.0000") & ""
                                Else

                                    strFieldValue = Replace(strFieldValue, "!", "")
                                End If
                                'strFieldValue = Replace(strFieldValue, "!", Chr(10))
                                strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
                                If strFieldValue = "SOVEREIGN RATING NOT APPLICABLE" Then
                                    strFieldValue = strFieldValue.Replace("RATING NOT APPLICABLE", "")
                                End If
                                worksheet.Cell(intRowIndex, P + 1).Value = strFieldValue
                                If strFieldValue <> "" Then
                                    myRange = worksheet.Cell(intRowIndex, P + 1)
                                    SetTableStyle(myRange)
                                End If
                            End If
                        End With
                    Next
                    If dtNew.Rows(R)("SecurityTypeId").ToString() <> "" Then
                        PrevSecTypeId = Val(dtNew.Rows(R)("SecurityTypeId").ToString())
                    End If
                End If
            Next
            'footer code
            intRowIndex = intRowIndex + 1
            srEnd = WriteContents(strFileSave & "End_Content.txt")
            If Val(Session("BranchId") & "") = 24 Then
                srEnd1 = WriteContents(strFileSave & "End_ContentB.txt")

            Else
                srEnd1 = WriteContents(strFileSave & "End_ContentA.txt")

            End If
            strEnd = srEnd.ReadToEnd
            strEnd2 = srEnd1.ReadToEnd
            strUserContent = FillUserContent(srEnd)

            srEnd.Close()
            srEnd = Nothing

            srEnd1.Close()
            srEnd1 = Nothing
            srEnd = WriteContents(strFileSave & "End_Content.txt")

            If Val(Session("BranchId") & "") = 24 Then
                srEnd1 = WriteContents(strFileSave & "End_ContentB.txt")
            Else
                srEnd1 = WriteContents(strFileSave & "End_ContentA.txt")
            End If
            While srEnd.Peek >= 0
                intRowIndex = intRowIndex + 1
                strEnd = srEnd.ReadLine
                If strEnd.IndexOf("Centrum Capital") <> -1 Then

                    worksheet.Cell(intRowIndex, 1).Value = strEnd
                    myRange = worksheet.Cell(intRowIndex, 1)
                    myRange.Style.Font.Bold = True
                    'worksheet.Cell(intRowIndex, 1).Font.Bold = True
                Else
                    worksheet.Cell(intRowIndex, 1).Value = strEnd
                End If

                'MergeCells("A", "H")
            End While

            intRowIndex = intRowIndex - 4
            worksheet.Cell(intRowIndex, 1).Value = strUserContent


            Dim I As Single
            intTotWidth = 0
            workbook.SaveAs(strDestination & "\" & "Offer" & "" & ".xlsx")
            Dim strParams(1) As String
            strParams(0) = strDestination
            strParams(1) = strDestination & "\Offer.zip"
            Dim ShlZip As New ShellZip
            ShlZip.Compress(strDestination & "\Offer.zip", strDestination)
            System.Threading.Thread.Sleep(500)
            With HttpContext.Current.Response
                .Clear()
                .Charset = ""
                .ClearHeaders()
                '.ContentType = "application/vnd.ms-excel"
                .ContentType = "application/x-zip-compressed"
                .AddHeader("content-disposition", "attachment;filename=Offer.zip")
                .WriteFile(strDestination & "\Offer.zip")
                .Flush()
                .End()
            End With

            'Page.ClientScript.RegisterStartupScript(Me.GetType, "select", "objExcel();", True)
        Catch ex As Exception
        Finally

            CloseConn()
        End Try

    End Sub

    'Protected Sub btn_CreatePDFFax_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CreatePDFFax.Click
    '    Try

    '        Dim imgHeader As iTextSharp.text.Image
    '        '   Dim imgFooter As iTextSharp.text.Image
    '        Dim strFileSave As String
    '        Dim srStart As StreamReader
    '        Dim srStart1 As StreamReader
    '        Dim srEnd As StreamReader = Nothing
    '        Dim srEnd1 As StreamReader
    '        Dim srEnd2 As StreamReader
    '        Dim strStart As String
    '        Dim strStart1 As String
    '        Dim strEnd As String
    '        Dim strEnd1 As String
    '        Dim strEnd2 As String
    '        Dim strEnd3 As String

    '        Dim strDate As String
    '        '  Dim objCommonFSO As Object
    '        Dim table1 As New iTextSharp.text.Table(2, 2)
    '        'Dim table2 As iTextSharp.text.Table
    '        Dim table3 As PdfPTable
    '        Dim para As iTextSharp.text.Paragraph
    '        Dim para1 As iTextSharp.text.Paragraph
    '        Dim para2 As iTextSharp.text.Paragraph
    '        Dim para3 As iTextSharp.text.Paragraph

    '        Dim strFontFilePath As String = Trim(ConfigurationSettings.AppSettings("FontFilePath") & "")
    '        'Dim header As HeaderFooter
    '        '  Dim footer As HeaderFooter
    '        Dim strContPerson As String
    '        Dim strUserContent As String
    '        Dim strDestination As String
    '        Dim dtCust As DataTable
    '        Dim strOldCustName As String = ""
    '        Dim intCnt As Int16 = -1
    '        Dim arrContact(0) As String
    '        Dim arrIndex(0) As Int16
    '        Dim strCustCont As String = ""
    '        Dim blnSameCust As Boolean
    '        Dim intIndex As Int16
    '        Dim K As Int16
    '        Dim strPath As String = ""

    '        'verdana = BaseFont.CreateFont(strFontFilePath & "\verdanab.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
    '        'arial1 = BaseFont.CreateFont(strFontFilePath & "\verdanab.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
    '        'arial = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, False)
    '        ' '' '' ''font = New iTextSharp.text.Font(arial1, 7)
    '        ' '' '' ''fontIn = New iTextSharp.text.Font(arial, 8)
    '        ' '' '' ''fontAddr = New iTextSharp.text.Font(arial, 9)

    '        'font = New iTextSharp.text.Font(arial1, 6)
    '        'fontIn = New iTextSharp.text.Font(arial, 7)
    '        'fontAddr = New iTextSharp.text.Font(arial, 8)

    '        verdana = BaseFont.CreateFont(strFontFilePath & "\verdanab.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
    '        arial1 = BaseFont.CreateFont(strFontFilePath & "\verdanab.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
    '        arial = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, False)
    '        '' '' '' ''font = New iTextSharp.text.Font(arial1, 7)
    '        '' '' '' ''fontIn = New iTextSharp.text.Font(arial, 8)
    '        '' '' '' ''fontAddr = New iTextSharp.text.Font(arial, 9)

    '        font = New iTextSharp.text.Font(arial1, 6)
    '        fontIn = New iTextSharp.text.Font(arial, 7)
    '        fontAddr = New iTextSharp.text.Font(arial, 8)

    '        strFileSave = ConfigurationManager.AppSettings("ImagePath") & "\"
    '        GetImages()
    '        strDestination = strFileSave & Session("UserName")

    '        Dim DI As New DirectoryInfo(strDestination)
    '        If DI.Exists = True Then
    '            DI.Delete(True)
    '        End If

    '        DI.Create()
    '        dtCust = TryCast(Session("CustomerContectTable"), DataTable)

    '        For K = 0 To dtCust.Rows.Count - 2
    '            If Trim(dtCust.Rows(K).Item("CustomerName") & "") = strOldCustName Then
    '                strCustCont += Trim(dtCust.Rows(K).Item("ContactPerson") & "") & ","
    '                arrContact(intCnt) = strCustCont
    '                blnSameCust = True
    '            Else
    '                strCustCont = Trim(dtCust.Rows(K).Item("ContactPerson") & "") & ","
    '                intCnt = intCnt + 1
    '                ReDim Preserve arrContact(intCnt)
    '                ReDim Preserve arrIndex(intCnt)
    '                arrContact(intCnt) = strCustCont
    '                arrIndex(intCnt) = K
    '                blnSameCust = False
    '            End If
    '            strOldCustName = Trim(dtCust.Rows(K).Item("CustomerName") & "")
    '        Next

    '        If blnSameCust = True Then
    '            arrContact(intCnt) = strCustCont
    '            arrIndex(intCnt) = K - 1
    '        End If


    '        strUserContent = FillUserContent(srEnd)
    '        Session("strUserBranchAddressPDF") = ""
    '        Session("BranchName") = ""
    '        Session("strUserBranchAddressPDF") = strUserBranchAddressPDF
    '        Session("BranchName") = strBranchName
    '        For J As Int16 = 0 To dg_Customer.Rows.Count - 1

    '            If IsInArray(J, arrIndex) Then
    '                'Dim MyDocument As New iTextSharp.text.Document(PageSize.A4, 30, 30, 100, 80)
    '                'Dim strPath As String = strDestination & "\" & Regex.Replace(Trim(dtCust.Rows(J).Item("CustomerName") & ""), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "") & ".pdf"
    '                'Dim PdfWriter As PdfWriter = pdf.PdfWriter.GetInstance(MyDocument, New FileStream(strPath, FileMode.Create))


    '                Hid_SelectedFields.Value = Trim(dtCust.Rows(J).Item("FieldId") & "")
    '                'code to create table and fill data
    '                table3 = AddPDFPTable()

    '                'code for date top right align
    '                Dim C As Int32 = 1
    '                Dim B1 As Integer = 2
    '                Dim ds1 As New DataTable
    '                Dim dtFill1 As DataTable
    '                Dim strFooter As String = ""
    '                Dim dtCustHeaderData As New DataTable
    '                OpenConn()
    '                dtCustHeaderData = objCommon.FillDataTable(sqlConn, "ID_FILL_FaxHeaderFooter", dtCust.Rows(J).Item("CustomerId"), "CustomerId")
    '                If dtCustHeaderData.Rows.Count > 0 Then

    '                    If Convert.ToString(dtCustHeaderData.Rows(0).Item("HeaderText")) <> "" Then
    '                        'Dim MyDocument As New iTextSharp.text.Document(PageSize.A4.Rotate())
    '                        Dim MyDocument As New iTextSharp.text.Document(PageSize.A4.Rotate())
    '                        'Dim MyDocument As New iTextSharp.text.Document(PageSize.A4, 10, 10, 10, 10)
    '                        MyDocument.SetMargins(20, 20, 110, 60)
    '                        strPath = strDestination & "\" & Regex.Replace(Trim(dtCust.Rows(J).Item("CustomerName") & ""), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "") & ".pdf"
    '                        Dim PdfWriter As PdfWriter = pdf.PdfWriter.GetInstance(MyDocument, New FileStream(strPath, FileMode.Create))
    '                        MyDocument.Open()

    '                        ''Code For Header and Footer
    '                        'If blnHeaderExist = True Then
    '                        MyHandler.strHeaderPath = strFileSave & strHeaderFile
    '                        MyHandler.strFooterPath = strFileSave & strFooterFile
    '                        Dim myEvents As MyHandler = New MyHandler
    '                        PdfWriter.PageEvent = myEvents
    '                        myEvents.onStartPage(PdfWriter, MyDocument)

    '                        'End If
    '                        MyDocument.Add(New iTextSharp.text.Paragraph(Convert.ToString(dtCustHeaderData.Rows(0).Item("HeaderText")), font))
    '                        strFooter = Convert.ToString(dtCustHeaderData.Rows(0).Item("FooterText"))
    '                        CloseConn()
    '                        MyDocument.Add(New iTextSharp.text.Paragraph("      "))
    '                        MyDocument.Add(table3)
    '                        MyDocument.Add(New iTextSharp.text.Paragraph("     ", fontAddr))
    '                        MyDocument.Add(New iTextSharp.text.Paragraph("     "))
    '                        If strFooter <> "" Then
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(strFooter, font))

    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Thanking You,", font))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("", font))
    '                            MyDocument.Add(New iTextSharp.text.Phrase("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Yours Faithfully,", font))
    '                            strUserContent = FillUserContent(srEnd)
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(Session("NameOfUser"), font))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(strUserMobile, fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(strUseremail, fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("      "))
    '                        Else
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Please note that the above offers are valid subject to market conditions and availability of stocks at the time of closing of a deal.", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Please feel free to contact the undersigned for any clarification    ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Thanking You,", font))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("", font))
    '                            MyDocument.Add(New iTextSharp.text.Phrase("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Yours Faithfully,", font))
    '                            strUserContent = FillUserContent(srEnd)
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(Session("NameOfUser"), font))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(strUserMobile, fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(strUseremail, fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("      "))
    '                        End If
    '                        MyDocument.Close()
    '                        MyDocument = Nothing
    '                    Else
    '                        'Dim MyDocument As New iTextSharp.text.Document(PageSize.A4.Rotate())
    '                        Dim MyDocument As New iTextSharp.text.Document(PageSize.A4.Rotate())
    '                        'Dim MyDocument As New iTextSharp.text.Document(PageSize.A4, 10, 10, 10, 10)
    '                        MyDocument.SetMargins(20, 20, 110, 60)
    '                        strPath = strDestination & "\" & Regex.Replace(Trim(dtCust.Rows(J).Item("CustomerName") & ""), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "") & ".pdf"
    '                        Dim PdfWriter As PdfWriter = pdf.PdfWriter.GetInstance(MyDocument, New FileStream(strPath, FileMode.Create))
    '                        MyDocument.Open()

    '                        'Code For Header and Footer
    '                        'If blnHeaderExist = True Then
    '                        MyHandler.strHeaderPath = strFileSave & strHeaderFile
    '                        MyHandler.strFooterPath = strFileSave & strFooterFile
    '                        Dim myEvents As MyHandler = New MyHandler
    '                        PdfWriter.PageEvent = myEvents
    '                        myEvents.onStartPage(PdfWriter, MyDocument)

    '                        'End If
    '                        strFooter = Convert.ToString(dtCustHeaderData.Rows(0).Item("FooterText"))
    '                        CloseConn()
    '                        MyDocument.Add(New iTextSharp.text.Paragraph("      "))
    '                        MyDocument.Add(table3)
    '                        MyDocument.Add(New iTextSharp.text.Paragraph("     ", fontAddr))
    '                        If strFooter <> "" Then
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   "))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(strFooter, font))

    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Thanking You,", font))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("", font))
    '                            MyDocument.Add(New iTextSharp.text.Phrase("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Yours Faithfully,", font))
    '                            strUserContent = FillUserContent(srEnd)
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(Session("NameOfUser"), font))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(strUserMobile, fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(strUseremail, fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("      "))
    '                        Else
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Please note that the above offers are valid subject to market conditions and availability of stocks at the time of closing of a deal.", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Please feel free to contact the undersigned for any clarification    ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Thanking You,", font))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("", font))
    '                            MyDocument.Add(New iTextSharp.text.Phrase("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("Yours Faithfully,", font))
    '                            strUserContent = FillUserContent(srEnd)
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(Session("NameOfUser"), font))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(strUserMobile, fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph(strUseremail, fontAddr))
    '                            MyDocument.Add(New iTextSharp.text.Paragraph("      "))
    '                        End If
    '                        MyDocument.Close()
    '                        MyDocument = Nothing
    '                    End If
    '                Else
    '                    'Dim MyDocument As New iTextSharp.text.Document(PageSize.A4.Rotate())
    '                    Dim MyDocument As New iTextSharp.text.Document(PageSize.A4.Rotate())
    '                    'Dim MyDocument As New iTextSharp.text.Document(PageSize.A4, 10, 10, 10, 10)
    '                    MyDocument.SetMargins(20, 20, 110, 60)
    '                    strPath = strDestination & "\" & Regex.Replace(Trim(dtCust.Rows(J).Item("CustomerName") & ""), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "") & ".pdf"
    '                    Dim PdfWriter As PdfWriter = pdf.PdfWriter.GetInstance(MyDocument, New FileStream(strPath, FileMode.Create))
    '                    MyDocument.Open()
    '                    'Code For Header and Footer
    '                    If blnHeaderExist = True Then
    '                        MyHandler.strHeaderPath = strFileSave & strHeaderFile
    '                        MyHandler.strFooterPath = strFileSave & strFooterFile
    '                        Dim myEvents As MyHandler = New MyHandler
    '                        PdfWriter.PageEvent = myEvents
    '                        myEvents.onStartPage(PdfWriter, MyDocument)

    '                    End If

    '                    CloseConn()
    '                    Hid_SelectedFields.Value = Trim(dtCust.Rows(J).Item("FieldId") & "")
    '                    strContPerson = arrContact(J)
    '                    strDate = "Date: " & String.Format(Today, "dd/MM/yyyy")

    '                    'code for header and footer content

    '                    srStart = WriteContents(strFileSave & "Start_Content.txt")
    '                    srStart1 = WriteContents(strFileSave & "Start_ContentA.txt")
    '                    'If Session("CompId") = 1 Then
    '                    srEnd = WriteContents(strFileSave & "End_Content.txt")
    '                    'End If
    '                    If Val(Session("BranchId") & "") = 24 Then
    '                        srEnd1 = WriteContents(strFileSave & "End_ContentB.txt")
    '                        'srEnd2 = WriteContents(strFileSave & "End_ContentB1.txt")
    '                    Else
    '                        srEnd1 = WriteContents(strFileSave & "End_ContentA.txt")
    '                        'srEnd2 = WriteContents(strFileSave & "End_ContentA1.txt")
    '                    End If



    '                    strStart = srStart.ReadToEnd
    '                    strStart1 = srStart1.ReadToEnd
    '                    strUserContent = FillUserContent(srEnd)
    '                    srEnd.Close()
    '                    srEnd = Nothing


    '                    'If Session("CompId") = 1 Then
    '                    srEnd = WriteContents(strFileSave & "End_Content.txt")
    '                    'End If

    '                    If Val(Session("BranchId") & "") = 24 Then
    '                        srEnd1 = WriteContents(strFileSave & "End_ContentB.txt")
    '                        'srEnd2 = WriteContents(strFileSave & "End_ContentB1.txt")
    '                    Else
    '                        srEnd1 = WriteContents(strFileSave & "End_ContentA.txt")
    '                        'srEnd2 = WriteContents(strFileSave & "End_ContentA1.txt")
    '                    End If
    '                    strEnd = srEnd.ReadToEnd
    '                    strEnd2 = srEnd1.ReadToEnd
    '                    'strEnd3 = srEnd2.ReadToEnd

    '                    strEnd = Replace(strEnd, "In case you need any clarification, please contact", strUserContent)
    '                    strEnd1 = strUserContent

    '                    Hid_SelectedFields.Value = Trim(dtCust.Rows(J).Item("FieldId") & "")
    '                    'table2 = AddPDFTable()

    '                    'code to create table and fill data
    '                    table3 = AddPDFPTable()

    '                    'code for date top right align
    '                    para = New iTextSharp.text.Paragraph(strDate, fontIn)
    '                    para.SetAlignment("left")
    '                    MyDocument.Add(para)


    '                    MyDocument.Add(New iTextSharp.text.Paragraph("To,", font))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph(Trim(dtCust.Rows(J).Item("CustomerName") & ""), font))

    '                    If dtCust.Rows(J).Item("CustomerAddress1") & "" <> "" Then
    '                        MyDocument.Add(New iTextSharp.text.Paragraph(Trim(dtCust.Rows(J).Item("CustomerAddress1") & ""), font))
    '                    End If

    '                    If dtCust.Rows(J).Item("CustomerAddress2") & "" <> "" Then
    '                        MyDocument.Add(New iTextSharp.text.Paragraph(Trim(dtCust.Rows(J).Item("CustomerAddress2") & ""), font))
    '                    End If

    '                    If dtCust.Rows(J).Item("CustomerCity") & "" <> "" Then
    '                        MyDocument.Add(New iTextSharp.text.Paragraph(Trim(dtCust.Rows(J).Item("CustomerCity") & ""), font))
    '                    End If

    '                    MyDocument.Add(New iTextSharp.text.Paragraph("      "))

    '                    MyDocument.Add(New iTextSharp.text.Paragraph(Trim(dtCust.Rows(K).Item("CustomerName") & ""), fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("KIND ATTN :- " & arrContact(J), font))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph(strStart, fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("    ", fontAddr))
    '                    MyDocument.Add(table3)
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("     ", fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("   "))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("Please note that the above offers are valid subject to market conditions and availability of stocks at the time of closing of a deal.", fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("Please feel free to contact the undersigned for any clarification    ", fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("Thanking You,", font))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("", font))
    '                    MyDocument.Add(New iTextSharp.text.Phrase("   ", fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("Yours Faithfully,", font))
    '                    strUserContent = FillUserContent(srEnd)
    '                    MyDocument.Add(New iTextSharp.text.Paragraph(Session("NameOfUser"), font))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph(strUserMobile, fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph(strUseremail, fontAddr))
    '                    MyDocument.Add(New iTextSharp.text.Paragraph("      "))

    '                    MyDocument.Close()
    '                    MyDocument = Nothing
    '                End If

    '                Dim ToEmailId As String
    '                ToEmailId = Convert.ToString(dtCust.Rows(J).Item("EmailId"))

    '                If chk_SendMail.Checked = True And ToEmailId <> "" Then
    '                    SendMail(strPath, ToEmailId)
    '                End If
    '            End If
    '        Next

    '        Using zip As New ZipFile()
    '            zip.AlternateEncodingUsage = ZipOption.AsNecessary
    '            'zip.AddDirectoryByName("Admin")
    '            zip.AddDirectory(strDestination)

    '            Response.Clear()
    '            Response.BufferOutput = False
    '            'Dim zipName As String = [String].Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
    '            Dim zipName As String = "Offer.zip"
    '            Response.ContentType = "application/zip"
    '            Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
    '            zip.Save(Response.OutputStream)
    '            Response.End()
    '        End Using

    '        'Dim strParams(1) As String
    '        'strParams(0) = strDestination
    '        'strParams(1) = strDestination & "\Offer.zip"
    '        ''ZipUnZipFolder.CreateZip(strParams)
    '        ''---------------
    '        ''Dim FBDlg As New FolderBrowserDialog

    '        '' FBDlg.ShowDialog()

    '        'Dim ShlZip As New ShellZip
    '        ''ShlZip.Compress("C:\Inetpub\wwwroot\QuoteDemo\FaxImages\Admin\Offer.zip", strDestination)

    '        'ShlZip.Compress(strDestination & "\Offer.zip", strDestination)
    '        ''strDestination & "\Offer.zip"
    '        ''---------------
    '        'System.Threading.Thread.Sleep(4000)

    '        'With HttpContext.Current.Response
    '        '    .Clear()
    '        '    .Charset = ""
    '        '    .ClearHeaders()
    '        '    '.ContentType = "application/x-zip-compressed"
    '        '    .ContentType = "application/zip"
    '        '    .AddHeader("content-disposition", "attachment;filename=Offer.zip")
    '        '    .WriteFile(strDestination & "\Offer.zip")
    '        '    .Flush()
    '        '    '.End()
    '        'End With

    '        'Using zip = New ZipFile()
    '        '    ' CREATE A FILE USING A STRING. 
    '        '    ' THE FILE WILL BE STORED INSIDE THE ZIP FILE.
    '        '    'zip.AddEntry("Content.txt", "This Zip has 2 DOC files")

    '        '    ' ZIP THE FOLDER WITH THE FILES IN IT.
    '        '    zip.AddDirectory(strDestination)
    '        '    ' zip.AddFiles(Directory.GetFiles(Server.MapPath("~/Admin")), "packed")
    '        '    zip.Save(Server.MapPath("~/pdf.zip"))  ' SAVE THE ZIP FILE.
    '        'End Using

    '        'Dim strParams(1) As String
    '        'strParams(0) = strDestination
    '        'strParams(1) = strDestination & "\Offer.zip"
    '        'Dim ShlZip As New ShellZip
    '        'ShlZip.Compress(strDestination & "\Offer.zip", strDestination)
    '        'System.Threading.Thread.Sleep(4000)
    '        'With HttpContext.Current.Response
    '        '    .Clear()
    '        '    .Charset = ""
    '        '    .ClearHeaders()
    '        '    '.ContentType = "application/vnd.ms-excel"
    '        '    .ContentType = "application/x-zip-compressed"
    '        '    .AddHeader("content-disposition", "attachment;filename=pdf.zip")
    '        '    .WriteFile(Server.MapPath("~/pdf.zip"))
    '        '    .Flush()
    '        '    '.End()
    '        'End With
    '    Catch ex As Exception
    '        objUtil.WritErrorLog(PgName, "btn_CreatePDFFax_Click", "Error in btn_CreatePDFFax_Click", "", ex)
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
    '        ' Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub




    Private Function AddPDFPTable() As Object
        Try
            Dim ds As DataTable
            Dim I As Int32
            Dim dsFill As DataTable
            Dim dsFill1 As New DataSet
            Dim strFields() As String
            Dim table As PdfPTable

            'Dim ds As New DataTable
            'Dim I As Int32
            'Dim dsFill As New DataSet
            'Dim strFields() As String
            Dim table1 As iTextSharp.text.Table
            Dim RX As iTextSharp.text.Cell
            Dim dt As DataTable
            Dim PrevSecTypeId As Integer
            Dim PrevSecTypename As String
            Dim dtNew As DataTable
            Dim fontB As iTextSharp.text.Font = FontFactory.GetFont("calibri", 9, 1)
            'Dim j As Integer


            dt = CType(Session("TempSecurityTable"), DataTable)

            Dim view As DataView = New DataView(dt)
            Dim distinctValues As DataTable = view.ToTable(True, "SecurityTypeId")
            OpenConn()

            strFields = Split(Hid_SelectedFields.Value, ",")
            table1 = New iTextSharp.text.Table(strFields.Length, dg_Selected.Rows.Count + distinctValues.Rows.Count + 1)
            'Dim colWidth() As Single = {10, 10, 10, 10, 10, 10, 10, 10, 10, 10}
            table1.WidthPercentage = 99
            table1.Cellspacing = 1
            table1.Cellpadding = 0
            Dim colWidth(strFields.Length - 1) As Single


            table = New PdfPTable(strFields.Length)
            table.HeaderRows = 1
            table.WidthPercentage = 100

            dtNew = dt.Copy()
            dtNew.DefaultView.Sort = "OrderId Asc, SecurityTypeId Asc, SecurityId Asc"
            dtNew = dtNew.DefaultView.ToTable()
            dt = dtNew

            For I = 0 To strFields.Length - 1
                Hid_FieldId.Value = Val(strFields(I))
                ds = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                colWidth(I) = Convert.ToSingle(ds.Rows(0).Item("PDFWidth") & "")
                'table.AddCell(New PdfPCell(New iTextSharp.text.Paragraph(UCase(ds.Rows(0).Item("FaxField").ToString), fontBold)))

                Dim cell = New PdfPCell(New Phrase(UCase(ds.Rows(0).Item("FaxField").ToString), fontB))
                cell.BackgroundColor = New iTextSharp.text.Color(System.Drawing.Color.FromArgb(79, 129, 189))

                cell.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cell)
            Next

            table.SetWidths(colWidth)

            'code to add security type name
            For R = 0 To dt.Rows.Count - 1
                'intRowIndex = intRowIndex + 1
                Hid_SecId.Value = Val(dt.Rows(R)("SecurityId").ToString())
                Hid_SecTypeId.Value = Trim(dt.Rows(R)("SecurityTypeName").ToString())
                Hid_ShowNumber.Value = Trim(dt.Rows(R)("ShowNumber").ToString())
                dsFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Val(Hid_SecId.Value), "SecurityId")
                dsFill1.Tables.Add(objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Val(Hid_SecId.Value), "SecurityId"))
                ' Hid_RowNo.Value = Val(CType(dg_Selected.Rows(I).FindControl("lbl_RowNumber"), Label).Text)
                If PrevSecTypename <> Trim(Hid_SecTypeId.Value) Then
                    RX = New iTextSharp.text.Cell()
                    Dim cell = New PdfPCell(New Phrase(Trim(dt.Rows(R)("SecurityTypeName") & ""), fontB))
                    cell.Colspan = strFields.Length
                    cell.BackgroundColor = New iTextSharp.text.Color(System.Drawing.Color.FromArgb(79, 129, 189))
                    cell.HorizontalAlignment = Element.ALIGN_LEFT
                    table.AddCell(cell)

                    'RX = New iTextSharp.text.Cell()
                    'Dim cell = New PdfPCell(New Phrase("Category: " & Trim(dsFill1.Tables(I).Rows(0).Item("SecurityTypeName") & ""), fontBold))
                    'cell.Colspan = strFields.Length
                    'cell.BackgroundColor = New iTextSharp.text.Color(System.Drawing.Color.DodgerBlue)

                    'cell.HorizontalAlignment = Element.ALIGN_CENTER
                    'table.AddCell(cell)

                    'End If
                End If
                AddPDFPTableRows(dsFill, R, table)
                PrevSecTypename = Trim(dt.Rows(R)("SecurityTypeName").ToString())
            Next

            Return table

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function

    Private Sub createPDFBlank()
        Try
            Dim strDate1 As String
            Dim imgHeader As iTextSharp.text.Image
            '   Dim imgFooter As iTextSharp.text.Image
            Dim strFileSave As String
            Dim srStart As StreamReader
            Dim srStart1 As StreamReader
            Dim srEnd As StreamReader = Nothing
            Dim srEnd1 As StreamReader
            Dim srEnd2 As StreamReader
            Dim strStart As String
            Dim strStart1 As String
            Dim strEnd As String
            Dim strEnd1 As String
            Dim strEnd2 As String
            Dim strEnd3 As String

            Dim strDate As String
            '  Dim objCommonFSO As Object
            Dim table1 As New iTextSharp.text.Table(2, 2)
            'Dim table2 As iTextSharp.text.Table
            Dim table3 As PdfPTable
            Dim para As iTextSharp.text.Paragraph
            Dim para1 As iTextSharp.text.Paragraph
            Dim para2 As iTextSharp.text.Paragraph
            Dim para3 As iTextSharp.text.Paragraph

            Dim strFontFilePath As String = Trim(ConfigurationSettings.AppSettings("FontFilePath") & "")
            Dim strContPerson As String
            Dim strUserContent As String
            Dim strDestination As String
            Dim dtCust As DataTable
            Dim strOldCustName As String = ""
            Dim intCnt As Int16 = -1
            Dim arrContact(0) As String
            Dim arrIndex(0) As Int16
            Dim strCustCont As String = ""
            Dim blnSameCust As Boolean

            Dim K As Int16

            verdana = BaseFont.CreateFont(strFontFilePath & "\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            arial1 = BaseFont.CreateFont(strFontFilePath & "\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            arial = BaseFont.CreateFont(strFontFilePath & "\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            ' arial = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, False)
            font = New iTextSharp.text.Font(arial1, 11)
            font2 = New iTextSharp.text.Font(arial1, 8)
            fontIn = New iTextSharp.text.Font(arial, 9)
            fontAddr = New iTextSharp.text.Font(arial, 10)

            strFileSave = ConfigurationManager.AppSettings("ImagePath") & "\"
            GetImages()
            strDestination = strFileSave & Session("UserName")

            Dim DI As New DirectoryInfo(strDestination)
            If DI.Exists = True Then
                DI.Delete(True)
            End If

            DI.Create()
            If blnSameCust = True Then
                arrContact(intCnt) = strCustCont
                arrIndex(intCnt) = K - 1
            End If

            Dim MyDocument As New iTextSharp.text.Document(PageSize.A4, 30, 30, 100, 80)
            Dim strPath As String = strDestination & "\" & "Offer" & ".pdf"


            Dim PdfWriter As PdfWriter = pdf.PdfWriter.GetInstance(MyDocument, New FileStream(strPath, FileMode.Create))
            MyDocument.Open()

            'Code For Header and Footer
            If blnHeaderExist = True Then
                MyHandler.strHeaderPath = strFileSave & strHeaderFile
                MyHandler.strFooterPath = strFileSave & strFooterFile
                Dim myEvents As MyHandler = New MyHandler
                PdfWriter.PageEvent = myEvents
                myEvents.onStartPage(PdfWriter, MyDocument)

            End If

            Hid_SelectedFields.Value = "1,30,22,64,21,10,3,4,5,11,2,26"
            Dim strDate_ As String = Date.Today.ToString("dd MMMM yyyy")
            strDate = "Date: " & strDate_

            strDate1 = Date.Today.AddDays(1).ToString("dd MMMM yyyy")


            'code for header and footer content

            srStart = WriteContents(strFileSave & "Start_Content.txt")
            srStart1 = WriteContents(strFileSave & "Start_ContentA.txt")
            'If Session("CompId") = 1 Then
            srEnd = WriteContents(strFileSave & "End_Content.txt")
            'End If
            If Val(Session("BranchId") & "") = 24 Then
                srEnd1 = WriteContents(strFileSave & "End_ContentB.txt")
                'srEnd2 = WriteContents(strFileSave & "End_ContentB1.txt")
            Else
                srEnd1 = WriteContents(strFileSave & "End_ContentA.txt")
                'srEnd2 = WriteContents(strFileSave & "End_ContentA1.txt")
            End If



            strStart = srStart.ReadToEnd
            strStart1 = srStart1.ReadToEnd
            strUserContent = FillUserContent(srEnd)

            srEnd.Close()
            srEnd = Nothing


            'If Session("CompId") = 1 Then
            srEnd = WriteContents(strFileSave & "End_Content.txt")
            'End If

            If Val(Session("BranchId") & "") = 24 Then
                srEnd1 = WriteContents(strFileSave & "End_ContentB.txt")
                'srEnd2 = WriteContents(strFileSave & "End_ContentB1.txt")
            Else
                srEnd1 = WriteContents(strFileSave & "End_ContentA.txt")
                'srEnd2 = WriteContents(strFileSave & "End_ContentA1.txt")
            End If
            strEnd = srEnd.ReadToEnd
            strEnd2 = srEnd1.ReadToEnd
            'strEnd3 = srEnd2.ReadToEnd

            strEnd = Replace(strEnd, "In case you need any clarification, please contact", strUserContent)
            strEnd1 = strUserContent

            'Hid_SelectedFields.Value = Trim(dtCust.Rows(J).Item("FieldId") & "")
            'table2 = AddPDFTable()

            'code to create table and fill data
            table3 = AddPDFPTable()

            'code for date top right align
            para = New iTextSharp.text.Paragraph(strDate, font)
            para.SetAlignment("right")
            MyDocument.Add(para)

            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
            MyDocument.Add(New iTextSharp.text.Paragraph(strStart, font))
            MyDocument.Add(New iTextSharp.text.Paragraph("    ", fontAddr))
            'MyDocument.Add(table2)
            MyDocument.Add(table3)
            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))


            'Offer Letter Remark 
            Dim iCount As Integer = 0
            OpenConn()
            For Each gvr As GridViewRow In dg_OfferRemark.Rows
                If (CType(gvr.FindControl("chk_ItemChecked"), CheckBox)).Checked = True Then
                    Dim RemarkId As Integer = Convert.ToInt32(CType(dg_OfferRemark.Rows(iCount).FindControl("lbl_RemarkId"), Label).Text)
                    Dim dt As New DataTable
                    dt = objCommon.FillDataTable(sqlConn, "ID_FILL_OfferLetterRemark", RemarkId, "RemarkId")
                    If dt.Rows.Count > 0 Then
                        MyDocument.Add(New iTextSharp.text.Paragraph(Trim(dt.Rows(0)("RemarkDescription") & ""), fontAddr))
                        MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
                    End If
                End If
                iCount += 1
            Next gvr
            CloseConn()
            'MyDocument.Add(New iTextSharp.text.Paragraph(strEnd, fontAddr))

            MyDocument.Add(New iTextSharp.text.Paragraph(" This is an indicative offer based on value date " & strDate1 & " a firm offer will be made after your approval for the same. Thanking you ", font))
            MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontAddr))
            MyDocument.Add(New iTextSharp.text.Paragraph("For " & Trim(Session("UserName")), fontAddr))
            srStart.Close()
            srEnd.Close()
            MyDocument.Close()
            MyDocument = Nothing

            Dim strParams(1) As String
            strParams(0) = strDestination
            strParams(1) = strDestination & "\Offer.zip"
            'ZipUnZipFolder.CreateZip(strParams)
            '---------------
            'Dim FBDlg As New FolderBrowserDialog

            ' FBDlg.ShowDialog()

            Dim ShlZip As New ShellZip
            'ShlZip.Compress("C:\Inetpub\wwwroot\QuoteDemo\FaxImages\Admin\Offer.zip", strDestination)

            ShlZip.Compress(strDestination & "\Offer.zip", strDestination)
            'strDestination & "\Offer.zip"
            '---------------
            System.Threading.Thread.Sleep(4000)

            With HttpContext.Current.Response
                .Clear()
                .Charset = ""
                .ClearHeaders()
                '.ContentType = "application/x-zip-compressed"
                .ContentType = "application/zip"
                .AddHeader("content-disposition", "attachment;filename=Offer.zip")
                .WriteFile(strDestination & "\Offer.zip")
                .Flush()
                .End()
            End With


        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_CreatePDFFax_Click", "Error in btn_CreatePDFFax_Click", "", ex)
        Finally

            ' Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Public Sub New()

    End Sub

    'Protected Sub cbo_SettDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SettDay.SelectedIndexChanged
    '    Try
    '        FillSettDate()
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub

    Private Sub FillSettDate()
        Try
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "FillSettlementDate();", True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "checkdate", "Error in checkdate", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub FillSettDate_old()
        Try
            Dim intLoop As Int16 = 0
            Dim count As Integer = 0
            Dim incDate As Date

            incDate = objCommon.DateFormat(txt_Date.Text.ToString())
            While count < cbo_SettDay.SelectedValue
                incDate = DateAdd(DateInterval.Day, 1, incDate)
                If checkdate(incDate) = True Then
                    count = count - 1
                End If
                count = count + 1
            End While
            'txt_Date.Text = String.Format(incDate, "dd/MM/yyyy")
            txt_CalcDate.Text = incDate.ToString("dd/MM/yyyy")
            'ytmdt = objCommon.DateFormat(txt_CalcDate.Text)

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillSettDate", "Error in FillSettDate", "", ex)
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Function checkdate(ByVal incDate As Date) As Boolean
        Try

            Dim Sqlcomm As New SqlCommand
            OpenConn()
            With Sqlcomm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_HolidaysNew"
                .Parameters.Clear()
                objCommon.SetCommandParameters(Sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                objCommon.SetCommandParameters(Sqlcomm, "@Month", SqlDbType.Int, 4, "I", , , Val(incDate.Month))
                objCommon.SetCommandParameters(Sqlcomm, "@HolidayDate", SqlDbType.DateTime, 4, "I", , , incDate)
                objCommon.SetCommandParameters(Sqlcomm, "@RET_CODE", SqlDbType.Int, 4, "O")
            End With
            If Sqlcomm.ExecuteScalar > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "checkdate", "Error in checkdate", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Function

    Private Sub btn_CreateExcelFax_Click(sender As Object, e As EventArgs) Handles btn_CreateExcelFax.Click
        Try
            If rbl_SelectCustomerBroker.SelectedValue = "C" Then
                CreateExcel_Customer()
            Else
                CreateExcel_Broker()
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_CreateExcelFax_Click", "Error in btn_CreateExcelFax_Click", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()

        End Try
    End Sub

    Private Sub btn_CreatePDFFax_Click(sender As Object, e As EventArgs) Handles btn_CreatePDFFax.Click
        Try
            If rbl_SelectCustomerBroker.SelectedValue = "C" Then
                CreatePDF_Customer()
            Else
                CreatePDF_Broker()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btn_AddBroker_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddBroker.Click
        lbl_Msg.Text = ""
        FillBrokerDetailsGrid()
    End Sub

    Private Sub FillBrokerDetailsGrid()
        Try
            Dim arrVals() As String
            Dim arrIds() As String
            Dim strarrIds() As String
            Dim dt1 As DataTable
            Dim dt2 As DataTable
            Dim dt3 As DataTable
            Dim strBrokId As String = ""
            Dim strCond As String

            OpenConn()
            arrVals = Split(Hid_RetValues.Value, "#")
            If (arrVals.Length > 1 And arrVals(1) <> "") Then
                arrIds = Split(arrVals(1), "!")
                If (arrIds.Length > 1) Then
                    Dim I As Integer
                    For I = 0 To arrIds.Length - 1
                        'strarrIds = Split(arrIds(I), "/")
                        If (arrIds(I) <> "") Then
                            strBrokId += arrIds(I) + "!"
                            ' strContactId += strarrIds(1) + "!"
                        End If
                    Next
                End If
            End If

            'strCond = BuildCustCond(arrVals(1), arrVals(2))
            strCond = BuildBrokCond(strBrokId)
            dt1 = objCommon.FillDataTable(sqlConn, "ID_FILL_FaxBrokerDetails", , , strCond)

            If TypeOf Session("BrokerTable") Is DataTable Then
                dt2 = TryCast(Session("BrokerTable"), DataTable)
                dt3 = mergeDTs(dt1, dt2)
            Else
                dt3 = dt1
            End If
            Session("BrokerTable") = dt3
            dg_Broker.DataSource = dt3
            dg_Broker.DataBind()
            ClientScript.RegisterStartupScript(Me.GetType(), "getCustBrokerOption", "getCustBrokerOption();")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function BuildBrokCond(ByVal strBrokId As String) As String
        Try
            Dim SB As New StringBuilder
            SB.AppendLine("WHERE BrokerId IN (" & Left(strBrokId.Replace("!", ","), strBrokId.Length - 1) & ")")
            Return SB.ToString
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Public Sub CreateExcel_Broker()
        Dim strMainPath As String = ""
        Try

            If dg_Broker.Rows.Count < 2 Then
                BlankExcelFax()
            Else
                Dim dt As DataTable
                Dim dtBroker As DataTable
                Dim strDestination As String
                Dim strOldBrokerName As String = ""
                Dim intCnt As Int16 = -1
                Dim arrContact(0) As String
                Dim arrIndex(0) As Int16
                Dim strCustCont As String = ""
                Dim strFileOfferSave As String
                Dim K As Int16
                Dim J As Int16
                Dim srStart As StreamReader
                Dim srEnd As StreamReader = Nothing
                Dim strFields() As String
                Dim strFileSave As String
                Dim strUserContent As String
                Dim strEnd As String
                Dim strEnd2 As String
                Dim srEnd1 As StreamReader
                Dim BrokerName As String = ""
                Dim PrevSecTypeId As Integer
                Dim str1 As String
                Dim strCustPrefix As String = ""
                Dim ImgByte As Byte() = Nothing

                GetImages()
                Dim strExcelColumnName As String = ""
                strFileSave = ConfigurationManager.AppSettings("ImagePath") & "\"
                strFileOfferSave = ConfigurationManager.AppSettings("ImagePath") & "\blank.bmp"
                strDestination = strFileSave & Session("UserName")

                Dim dta As DataTable = TryCast(Session("BrokerTable"), DataTable)
                Dim DI As New DirectoryInfo(strDestination)
                If DI.Exists = True Then
                    DI.Delete(True)
                End If
                DI.Create()
                Dim d1 As DataTable = objCommon.FillDataTable(sqlConn, "ID_SELECT_BranchMaster", Session("Branchid"), "Branchid")
                For b As Int32 = 0 To dta.Rows.Count - 2
                    'Dim strppp_ As String = (Server.MapPath("").Replace("Forms", "Temp") & "\OfferExcel.xlsx")
                    ' Dim strppp As String = Hid_offerpath.Value.ToString()
                    Dim strppp As String = ConfigurationManager.AppSettings("offerPath") & "\OfferExcel.xlsx"
                    Dim workbook = New XLWorkbook(strppp)
                    Dim worksheet = workbook.Worksheet(1)


                    'Dim worksheet = workbook.Add("Sample Sheet")
                    Dim myRange As ClosedXML.Excel.IXLCell
                    Dim strPath As String = ""
                    Dim sImagePath As String = strFileOfferSave
                    Dim custname As String = ""

                    dt = CType(Session("TempSecurityTable"), DataTable)
                    dt.DefaultView.Sort = "OrderId Asc,SecurityTypeId, SecurityId Asc"
                    dt = dt.DefaultView.ToTable()

                    Dim view As DataView = New DataView(dt)

                    Dim distinctValues As DataTable = view.ToTable(True, "SecurityTypeId")
                    dtBroker = TryCast(Session("BrokerTable"), DataTable)
                    strPath = strDestination & "\" & Regex.Replace(Trim(dtBroker.Rows(J).Item("BrokerName") & ""), "[^0-9a-zA-Z .]+", "") & ".xlsx"
                    If IsInArray(J, arrIndex) Then
                        intRowIndex = 4
                    End If
                    intRowIndex = 2
                    myRange = worksheet.Cell(intRowIndex, 3)
                    myRange.Style.Font.Bold = True
                    myRange = worksheet.Cell(intRowIndex, 3)
                    myRange.Style.Font.Bold = True
                    'worksheet.Cell(intRowIndex - 4, 8).Value = ""
                    'worksheet.Cell(intRowIndex - 3, 8).Value = ""
                    'worksheet.Cell(intRowIndex - 2, 8).Value = ""
                    'worksheet.Cell(intRowIndex - 1, 8).Value = ""

                    'intRowIndex = intRowIndex + 1
                    'var Image = worksheet.A(imagePath).MoveTo(WS.Cell("A1")).Scale(1.0);
                    intRowIndex = 2
                    worksheet.Cell(intRowIndex, 2).Style.NumberFormat.Format = "dd-mmm-yyyy"
                    worksheet.Cell(intRowIndex, 1).Value = "Date : "
                    myRange = worksheet.Cell(intRowIndex, 1)
                    SetTableStyle(myRange, "N")
                    worksheet.Cell(intRowIndex, 2).Value = String.Format(Today, "dd/MM/yyyy")
                    myRange = worksheet.Cell(intRowIndex, 2)
                    SetTableStyle(myRange, "N")

                    worksheet.Cell(intRowIndex, 2).WorksheetColumn.Width = 18
                    intRowIndex = intRowIndex + 2

                    worksheet.Cell(intRowIndex, 1).Value = "Kind Attention :- "
                    srStart = WriteContents(strFileSave & "Start_Content.txt")
                    While srStart.Peek >= 0
                        intRowIndex = intRowIndex + 1
                        worksheet.Cell(intRowIndex, 1).Value = srStart.ReadLine
                        myRange = worksheet.Cell(intRowIndex, 1)
                        SetTableStyle(myRange, "N")
                        'MergeCells("A", "H")
                    End While

                    intRowIndex = intRowIndex + 1
                    intRowIndex = intRowIndex + 1
                    srStart.Close()


                    strFields = Split(Trim(dta.Rows(b).Item("FieldId") & ""), ",")

                    'AddExcelQuotes(strFields)
                    Dim R As Int32
                    Dim S As Int32

                    Dim ds As DataTable
                    Dim dtFill As DataTable
                    OpenConn()
                    For R = 0 To strFields.Length - 1
                        Hid_FieldId.Value = Val(strFields(R))
                        ds = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                        intTotWidth = intTotWidth + Val(ds.Rows(0).Item("WordWidth") & "")
                        worksheet.Cell(intRowIndex, R + 1).Value = Trim(ds.Rows(0).Item("FaxField") & "")
                        myRange = worksheet.Cell(intRowIndex, R + 1)

                        SetTableStyle(myRange, "M")

                    Next

                    For R = 0 To dt.Rows.Count - 1
                        'intRowIndex = intRowIndex + 1
                        Hid_SecId.Value = Val(dt.Rows(R)("SecurityId").ToString())
                        Hid_SecTypeId.Value = Val(dt.Rows(R)("SecurityTypeId").ToString())
                        Hid_ShowNumber.Value = Trim(dt.Rows(R)("ShowNumber").ToString())
                        'intRowIndex = intRowIndex + 1
                        dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")
                        'Hid_RowNo.Value = Val(CType(dg_Selected.Rows(R).FindControl("lbl_RowNumber"), Label).Text)
                        If Val(Hid_SecId.Value) <> 0 Then
                            intRowIndex = intRowIndex + 1
                            dtFill = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecId.Value, "SecurityId")
                            If PrevSecTypeId <> Val(Hid_SecTypeId.Value) Then
                                For S = 0 To strFields.Length - 1
                                    myRange = worksheet.Cell(intRowIndex, S + 1)
                                    SetTableStyle(myRange, "SH")
                                Next
                                strExcelColumnName = ColumnName(S)
                                worksheet.Range("A" & intRowIndex & ":" & strExcelColumnName & intRowIndex).Merge()
                                str1 = CStr(Chr(strFields.Length + 1 + 63)) '& (intRowIndex + dg_Selected.Rows.Count))
                                myRange = worksheet.Cell(intRowIndex, strFields.Length)
                                myRange.Style.Font.Bold = True
                                'SetTableStyle(myRange, "M")
                                worksheet.Cell(intRowIndex, 1).Value = Trim(dtFill.Rows(0).Item("SecurityTypeName") & "")
                                worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
                                myRange = worksheet.Cell(intRowIndex, strFields.Length)
                                myRange.Style.Font.Bold = True
                                'SetTableStyle(myRange, "H")
                                intRowIndex = intRowIndex + 1

                            End If


                            Dim strFieldName As String
                            Dim strTableName As String
                            Dim strColName As String
                            Dim strFieldValue As String
                            Dim P As Int32
                            Dim Q As Int32
                            Dim intColRows As Int16
                            Dim intRowHeight As Int16
                            Dim dsFieldNames As DataTable
                            'Dim dt As DataTable
                            Dim strTemp As String
                            OpenConn()

                            Dim W As Single
                            Dim dtSel As DataTable
                            Dim DvSel As DataView = New DataView()

                            'strFieldValue = Trim(CType(dg_Selected.(intIndex).FindControl("lbl_" & strFieldName), Label).Text & "")
                            dtSel = TryCast(Session("TempSecurityTable"), DataTable)
                            dtSel.DefaultView.Sort = "OrderId Asc,SecurityId Asc"
                            dtSel = dtSel.DefaultView.ToTable()
                            DvSel = New DataView(dtSel)

                            DvSel.RowFilter = String.Empty
                            DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value
                            'DvSel.RowFilter = "SecurityId =" + Hid_SecId.Value + " And Id=" + Hid_RowNo.Value
                            For P = 0 To strFields.Length - 1
                                Hid_FieldId.Value = Val(strFields(P))

                                dsFieldNames = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxFields", Hid_FieldId.Value, "FieldId")
                                'dsFieldNames = objCommon.GetDataSet(SqlDataSourceFieldId)
                                With dsFieldNames.Rows(0)
                                    strTableName = Trim(.Item("TableName") & "")
                                    strFieldName = Trim(.Item("TableField") & "")
                                    If strFieldName = "Rate" Then intRateIndex = P
                                    If strFieldName = "SecurityName" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 45
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top

                                    ElseIf strFieldName = "NSDLFaceValue" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
                                    ElseIf strFieldName = "NSDLAcNumber" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 20
                                    ElseIf strFieldName = "MaturityDate" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 16.5
                                    ElseIf strFieldName = "CallDate" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
                                    ElseIf strFieldName = "Rating" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 18
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
                                    ElseIf strFieldName = "RatingOrg" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
                                    ElseIf strFieldName = "CouponRate" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
                                    ElseIf strFieldName = "Rate" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 10
                                    ElseIf strFieldName = "PutDate" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
                                    ElseIf strFieldName = "IPDates" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
                                        worksheet.Cell(intRowIndex, P + 1).Style.Alignment.WrapText = True
                                    ElseIf strFieldName = "ShowNumber" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 12
                                    ElseIf strFieldName = "YTCAnn" Then
                                        worksheet.Cell(intRowIndex, P + 1).WorksheetColumn.Width = 13
                                    End If
                                    If strTableName <> "FromPage" Then
                                        For Q = 0 To dtFill.Columns.Count - 1
                                            strColName = dtFill.Columns(Q).ColumnName
                                            If UCase(strFieldName) = UCase(strColName) Then
                                                strTemp = ""
                                                If strColName = "IPDates" Then strTemp = " "

                                                If strColName = "MaturityDate" Or strColName = "PutDate" Or strColName = "SecurityInfoDate" Or strColName = "IssueDate" Or strColName = "FirstInterestDate" Or strColName = "BookClosureDate" Or strColName = "DMATBookClosureDate" Or strColName = "FirstInterestDate" Or strColName = "CallDate" Or strColName = "Call Dates" Or strColName = "SecCallDate" Then
                                                    worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-MMM-yyyy"
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                ElseIf strColName = "IPDates" Then
                                                    worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "dd-MMM"
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")

                                                ElseIf strColName = "CouponRate" Then
                                                    If Trim(dtFill.Rows(0).Item(strColName) & "") <> "" Then
                                                        worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "") + "%"
                                                    End If

                                                ElseIf strColName = "SellingRate" Then
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "") + "%"
                                                Else
                                                    worksheet.Cell(intRowIndex, P + 1).Value = strTemp & Trim(dtFill.Rows(0).Item(strColName) & "")
                                                End If

                                                Exit For
                                            End If

                                            myRange = worksheet.Cell(intRowIndex, P + 1)
                                            SetTableStyle(myRange, "C", strColName)
                                        Next
                                    Else
                                        intColRows = 0
                                        If strFieldName = "ShowNumber" Then
                                            If Hid_ShowNumber.Value = "" Then
                                                strFieldValue = "N/A"
                                            Else
                                                strFieldValue = Hid_ShowNumber.Value
                                            End If

                                        Else
                                            If strFieldName = "Id" Then
                                                strFieldValue = R + 1
                                            Else
                                                strFieldValue = Trim(DvSel.Item(0)(strFieldName).ToString() & "")
                                            End If

                                        End If

                                        If InStr(strFieldValue, "!") > 1 Then
                                            For W = 1 To Len(strFieldValue)
                                                If Mid$(strFieldValue, W, 1) = "!" Then
                                                    intColRows = intColRows + 1

                                                End If
                                            Next
                                            strFieldValue = Replace(Left(strFieldValue, Len(strFieldValue) - 1), "!", Chr(10))
                                            If intRowHeight < (13 * intColRows) Then intRowHeight = 13 * intColRows
                                        Else
                                            strFieldValue = Replace(strFieldValue, "!", "")
                                        End If
                                        'strFieldValue = Replace(strFieldValue, "!", Chr(10))
                                        strFieldValue = IIf(strFieldValue = Nothing, " ", strFieldValue)
                                        If strFieldName = "SellingRate" Then
                                            worksheet.Cell(intRowIndex, P + 1).Value = strFieldValue '(String.Format(strFieldValue, "#############0.0000")) & ""
                                            worksheet.Cell(intRowIndex, P + 1).Style.NumberFormat.Format = "#,##0.0000"
                                        Else
                                            worksheet.Cell(intRowIndex, P + 1).Value = strFieldValue
                                        End If

                                        If strFieldName = "Yield" Or strFieldName = "YTCAnn" Or strFieldName = "YTPAnn" Or strFieldName = "YTMSemi" Or strFieldName = "YTPSemi" Or strFieldName = "YTCSemi" Or strFieldName = "CouponRate" Then
                                            If strFieldValue > 0 Then
                                                'worksheet.Cell(intRowIndex, P + 1).Value = strFieldValue + "%"

                                                worksheet.Cell(intRowIndex, P + 1).Value = (String.Format(objCommon.DecimalFormat4(strFieldValue), "#############0.0000")) & "%"


                                            Else
                                                worksheet.Cell(intRowIndex, P + 1).Value = "N/A"
                                            End If

                                        End If
                                        If strFieldValue <> "" Then
                                            myRange = worksheet.Cell(intRowIndex, P + 1)
                                            SetTableStyle(myRange, "C")

                                        End If

                                    End If

                                End With

                            Next
                            PrevSecTypeId = Val(dt.Rows(R)("SecurityTypeId").ToString())
                        End If

                    Next
                    'footer code
                    intRowIndex = intRowIndex + 2

                    worksheet.Cell(intRowIndex, 1).Value = "Kindly note that the above quotes are subject to availability of stock and prevailing market rates."
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = "Please confirm the stocks as early as possible to avoid stock availability and market volatility."
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = "For any further clarification please feel free to contact us at 09377417200"
                    worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = "For, SUNRISE GILTS & SECURITIES (P) LTD."
                    worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
                    intRowIndex = intRowIndex + 2
                    'worksheet.Cell(intRowIndex, 1).Value = "Corporate Office: 514,Pinnacle Business Park, Corporate Road, Prahlad Nagar,Ahmedabad - 380015. Gujarat"
                    'intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = "Registered Office: 514,Pinnacle Business Park, Corporate Road, Prahlad Nagar,Ahmedabad - 380015. Gujarat" & " Phone: +91 79 4032 7414 / 15,  4896 6870 ( 5 Line), Mobile : +91 9898658238,Fax:+ 91 79 4030 3249 " & vbCrLf & " Email : sunrisegilts@gmail.com  , info@sunrisegilts.com" & vbCrLf & "CIN No. : U67100GJ2013PTC077167  | SEBI Reg. No. : INZ0025734 | BSE Membership No, 4071 | NSE Membership No. 90076" & vbCrLf & "Branches : Kolkata ,Mumbai and New Delhi"
                    worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
                    worksheet.Cell(intRowIndex, 1).Style.Font.Italic = True
                    worksheet.Cell(intRowIndex, 1).Style.Alignment.WrapText = True
                    worksheet.Cell(intRowIndex, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    worksheet.Range("A" & intRowIndex & ":" & strExcelColumnName & intRowIndex).Merge()
                    worksheet.Row(intRowIndex).Height = 45
                    intRowIndex = intRowIndex + 2
                    intRowIndex = intRowIndex + 2

                    strUserContent = FillUserContent(srEnd)
                    'intRowIndex = intRowIndex + 2
                    worksheet.Cell(intRowIndex, 1).Style.Font.Bold = True
                    worksheet.Cell(intRowIndex, 1).Value = Session("NameOfUser")
                    myRange = worksheet.Cell(intRowIndex, 1)
                    SetTableStyle(myRange, "N")
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = strUserMobile
                    myRange = worksheet.Cell(intRowIndex, 1)
                    SetTableStyle(myRange, "N")
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 1).Value = strUseremail
                    myRange = worksheet.Cell(intRowIndex, 1)
                    SetTableStyle(myRange, "N")

                    If strHeaderFile = "" Then
                        '  strHeaderFile = ConfigurationManager.AppSettings("BlankHeaderImage")
                        strHeaderFile = Hid_BlankHeaderImage.Value.ToString()
                    End If
                    If Convert.ToString(Session("LogoData")) <> "" Then
                        strMainPath = strFileSave + " HeaderPath1: " + strHeaderFile + " 1"
                        ImgByte = CType(Session("LogoData"), Byte())
                        Dim img As System.Drawing.Image = byteArrayToImage(ImgByte)
                        worksheet.AddPicture(img).MoveTo(worksheet.Cell(2, 8))
                    Else
                        worksheet.AddPicture(strFileSave & strHeaderFile).MoveTo(worksheet.Cell(2, 8))
                    End If

                    Dim I As Single
                    intTotWidth = 0
                    workbook.SaveAs(strDestination & "\" & "" & Regex.Replace(dta.Rows(b).Item("BrokerName"), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "") & "" & ".xlsx")
                    Dim ToEmailId As String
                    ToEmailId = Convert.ToString(dtBroker.Rows(J).Item("EmailId"))

                    If chk_SendMail.Checked = True And ToEmailId <> "" Then
                        objcomm.SendOffer(Trim(dtBroker.Rows(J).Item("BrokerName") & ""), strPath, ToEmailId, txt_CalcDate.Text, strUserName, strUserMobile)
                        ' SendMail(strPath, ToEmailId)
                    End If

                Next


                Using zip As New ZipFile()
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary
                    'zip.AddDirectoryByName("Admin")
                    zip.AddDirectory(strDestination)

                    Response.Clear()
                    Response.BufferOutput = False
                    'Dim zipName As String = [String].Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
                    Dim zipName As String = "Offer.zip"
                    Response.ContentType = "application/zip"
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
                    zip.Save(Response.OutputStream)
                    Response.End()
                End Using
            End If
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "select", "objExcel();", True)
        Catch ex As Exception
            'Dim file As System.IO.StreamWriter
            'file = My.Computer.FileSystem.OpenTextFileWriter("c:\test.txt", True)
            'file.WriteLine(ex.Message + strMainPath + "  " + System.DateTime.Now.ToString())
            'file.Close()
            objUtil.WritErrorLog(PgName, "btn_CreateExcelFax_Click", "Error in btn_CreateExcelFax_Click", "", ex)
        Finally
            CloseConn()
        End Try
    End Sub

    Public Sub CreatePDF_Broker()
        Try
            Dim strFileSave As String
            Dim srStart As StreamReader
            Dim srStart1 As StreamReader
            Dim srEnd As StreamReader = Nothing
            Dim strStart As String
            Dim strStart1 As String
            Dim strEnd As String
            Dim strEnd1 As String

            Dim strDate As String
            Dim table1 As New iTextSharp.text.Table(2, 2)
            Dim table3 As PdfPTable
            Dim para As iTextSharp.text.Paragraph
            Dim strFontFilePath As String = Trim(ConfigurationManager.AppSettings("FontFilePath") & "")
            Dim strFontFilePath_CambriRegular As String = Trim(ConfigurationManager.AppSettings("FontFilePath_Cambria_Regular") & "")
            Dim strUserContent As String
            Dim strDestination As String
            Dim dtBroker As DataTable
            Dim strOldCustName As String = ""
            Dim intCnt As Int16 = -1
            Dim arrContact(0) As String
            Dim arrIndex(0) As Int16
            Dim strCustCont As String = ""
            Dim strCustPrefix As String = ""
            Dim ImgByte As Byte() = Nothing

            verdana = BaseFont.CreateFont(strFontFilePath & "\verdanab.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            arial1 = BaseFont.CreateFont(strFontFilePath & "\verdanab.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            arial = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, False)

            font = New iTextSharp.text.Font(arial1, 8)
            fontIn = New iTextSharp.text.Font(arial, 9)
            fontAddr = New iTextSharp.text.Font(arial, 9)
            strFileSave = ConfigurationManager.AppSettings("ImagePath") & "\"
            GetImages()
            strDestination = strFileSave & Session("UserName")

            Dim DI As New DirectoryInfo(strDestination)

            If DI.Exists = True Then
                DI.Delete(True)
            End If

            DI.Create()
            dtBroker = TryCast(Session("BrokerTable"), DataTable)

            For J As Int16 = 0 To dg_Broker.Rows.Count - 2
                Dim MyDocument As New iTextSharp.text.Document(PageSize.A4.Rotate())
                MyDocument.SetMargins(20, 20, 30, 90)
                Dim strPath = strDestination & "\" & Regex.Replace(Trim(dtBroker.Rows(J).Item("BrokerName") & ""), "[^0-9a-zA-Z .]+", "") & ".pdf"
                Dim PdfWriter As PdfWriter = pdf.PdfWriter.GetInstance(MyDocument, New FileStream(strPath, FileMode.Create))
                MyDocument.Open()
                strFooterFile = "Blank.bmp"
                If strHeaderFile = "" Then
                    strHeaderFile = Hid_BlankHeaderImage.Value.ToString()
                End If
                MyHandler.strHeaderPath = strFileSave & strHeaderFile
                MyHandler.strFooterPath = strFileSave & strFooterFile
                Dim myEvents As MyHandler = New MyHandler
                PdfWriter.PageEvent = myEvents
                myEvents.onStartPage(PdfWriter, MyDocument)
                Hid_SelectedFields.Value = Trim(dtBroker.Rows(J).Item("FieldId") & "")
                strDate = "Date: " & String.Format(Today, "dd/MM/yyyy")

                'code for header and footer content

                srStart = WriteContents(strFileSave & "Start_Content.txt")
                srStart1 = WriteContents(strFileSave & "Start_ContentA.txt")
                srEnd = WriteContents(strFileSave & "End_Content.txt")
                strStart = srStart.ReadToEnd
                strStart1 = srStart1.ReadToEnd
                strUserContent = FillUserContent(srEnd)
                srEnd = Nothing
                strEnd = Replace(strEnd, "In case you need any clarification, please contact", strUserContent)
                strEnd1 = strUserContent

                Hid_SelectedFields.Value = Trim(dtBroker.Rows(J).Item("FieldId") & "")
                table3 = AddPDFPTable()
                MyDocument.Add(New iTextSharp.text.Paragraph(""))
                para = New iTextSharp.text.Paragraph(strDate, fontIn)
                para.SetAlignment("left")

                MyDocument.Add(New iTextSharp.text.Paragraph("To,", fontIn))
                MyDocument.Add(New iTextSharp.text.Paragraph("The Investment Department", fontIn))

                MyDocument.Add(New iTextSharp.text.Paragraph("Kind Attn : - Mr. ", fontIn))
                MyDocument.Add(New iTextSharp.text.Paragraph("We are pleased to Update you the following securities for your  Investment. ", fontIn))

                MyDocument.Add(New iTextSharp.text.Paragraph("    ", fontIn))
                MyDocument.Add(New iTextSharp.text.Paragraph("    ", fontIn))
                MyDocument.Add(table3)
                MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontIn))

                MyDocument.Add(New iTextSharp.text.Paragraph("Kindly note that the above quotes are subject to availability of stock and prevailing market rates.", fontIn))
                MyDocument.Add(New iTextSharp.text.Paragraph("Please confirm the stocks as early as possible to avoid stock availability and market volatility.  ", fontIn))

                MyDocument.Add(New iTextSharp.text.Paragraph("For any further clarification please feel free to contact us at 09377417200  ", fontIn))
                MyDocument.Add(New iTextSharp.text.Paragraph("   ", fontIn))
                MyDocument.Add(New iTextSharp.text.Paragraph("For, SUNRISE GILTS & SECURITIES (P) LTD.  ", fontIn))

                MyDocument.Close()
                MyDocument = Nothing

                Dim ToEmailId As String
                ToEmailId = Convert.ToString(dtBroker.Rows(J).Item("EmailId"))

                If chk_SendMail.Checked = True And ToEmailId <> "" Then
                    objcomm.SendOffer(Trim(dtBroker.Rows(J).Item("BrokerName") & ""), strPath, ToEmailId, txt_CalcDate.Text, strUserName, strUserMobile)
                    ' SendMail(strPath, ToEmailId)
                End If
                'End If
            Next

            Using zip As New ZipFile()
                zip.AlternateEncodingUsage = ZipOption.AsNecessary
                'zip.AddDirectoryByName("Admin")
                zip.AddDirectory(strDestination)

                Response.Clear()
                Response.BufferOutput = False
                'Dim zipName As String = [String].Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
                Dim zipName As String = "Offer.zip"
                Response.ContentType = "application/zip"
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
                zip.Save(Response.OutputStream)
                Response.End()
            End Using

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_CreatePDFFax_Click", "Error in btn_CreatePDFFax_Click", "", ex)
            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter("c:\test.txt", True)
            file.WriteLine(ex.Message)
            file.Close()
            ' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_Reset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Reset.Click
        Try
            For K As Int32 = 0 To dg_Selected.Rows.Count - 1
                CType(dg_Selected.Rows(K).FindControl("lbl_Rate"), TextBox).Text = "100.0000"
                Hid_Rate.Value = "100.0000"
                CType(dg_Selected.Rows(K).FindControl("txt_Yield"), TextBox).Text = "0.00"
                CType(dg_Selected.Rows(K).FindControl("txt_YTMSemi"), TextBox).Text = "0.00"
                CType(dg_Selected.Rows(K).FindControl("txt_YTC"), TextBox).Text = "0.00"
                CType(dg_Selected.Rows(K).FindControl("txt_YTCSemi"), TextBox).Text = "0.00"
                CType(dg_Selected.Rows(K).FindControl("hdnRate"), HiddenField).Value = "100.0000"
            Next
            Dim dt As New DataTable()
            dt = CType(Session("TempSecurityTable"), DataTable)
            For G As Int32 = 0 To dt.Rows.Count - 1
                dt.Rows(G).Item("Yield") = "0.00"
                dt.Rows(G).Item("YTMSemi") = "0.00"
                dt.Rows(G).Item("YTCAnn") = "0.00"
                dt.Rows(G).Item("YTCSemi") = "0.00"
                dt.Rows(G).Item("YTPAnn") = "0.00"
                dt.Rows(G).Item("YTPSemi") = "0.00"
                dt.Rows(G).Item("SellingRate") = "100.0000"
                dt.Rows(G).Item("OriginalSellingRate") = 100
            Next
            dg_Selected.DataSource = dt
            dg_Selected.DataBind()
            txt_basispoint.Text = ""
            Hid_BasisPoint.Value = ""
        Catch ex As Exception
            Dim strError As String = ex.Message
        End Try
    End Sub

    Protected Sub dg_Broker_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dg_Broker.RowCommand
        Try
            Dim imgBtn As ImageButton
            Dim gvRow As GridViewRow
            Dim dt As DataTable

            dt = Session("BrokerTable")

            If e.CommandName = "DeleteRow" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                gvRow = imgBtn.Parent.Parent
                DeleteGridRows("BrokerTable", gvRow.DataItemIndex, dg_Broker, 0)
            End If
            If dg_Broker.Rows.Count = 0 Then
                FillBlankBrokerGrids()
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

End Class