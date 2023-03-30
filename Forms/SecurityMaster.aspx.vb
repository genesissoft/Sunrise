Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports System.Collections.Generic
Imports System.IO
Partial Class Forms_SecurityMaster
    Inherits System.Web.UI.Page
    Dim PgName As String = "$SecurityMaster$"
    Dim objCommon As New clsCommonFuns
    Dim arrSecurityIds() As String
    Dim IPDates As String
    Dim idate1 As Date
    Dim idate2 As Date
    Dim idate3 As Date
    Dim idate4 As Date
    Dim matDate As Date
    Dim callDate As Date
    Dim sqlConn As New SqlConnection
    Dim strUrlName As String
    Dim objUtil As New Util


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"

            If IsPostBack = False Then
                SetAttributes()
                SetControls()
                FillBlankGrids()
                FillBlankRatingGrids()
                FillCombo()
                If Request.QueryString("Id") <> "" Then
                    Hid_Id.Value = Request.QueryString("Id")
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    FillFields()
                    FillGrids()
                    FillRatingDetails()
                    btn_Save.Visible = False
                    btn_Update.Visible = True

                Else
                    btn_Save.Visible = True
                    btn_Update.Visible = False
                End If
                ' FillSecurityDocGrid()
            End If
            'Page.ClientScript.RegisterStartupScript(Me.GetType, "call", "Hidegrids();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "call", "Hidegrids();", True)
            'Page.ClientScript.RegisterStartupScript(Me.GetType, "call", "KeyCheck(e);", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    'Public Sub FillSecurityDocGrid()
    '    Try
    '        Dim dt As DataTable
    '        OpenConn()
    '        dt = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityDocDetail")
    '        Session("SecurityDocs") = dt
    '        Dim dr As DataRow = dt.NewRow
    '        dt.Rows.Add(dr)
    '        gv_docs.DataSource = dt
    '        gv_docs.DataBind()
    '        Session("SecurityDocs") = dt
    '        Call FillDocs()
    '        CloseConn()
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Private Sub FillDocs()
    '    Dim dt As DataTable
    '    Dim dt1 As DataTable
    '    Dim dr As DataRow
    '    Dim dr1 As DataRow
    '    dt = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityDocDetails", Val(ViewState("Id") & ""), "SecurityId")
    '    dt1 = CType(Session("SecurityDocs"), DataTable).Copy
    '    For i As Int16 = 0 To dt.Rows.Count - 1
    '        dr1 = dt1.NewRow
    '        dr = dt.Rows(i)
    '        dr1("SecurityDocumentId") = dr("SecurityDocumentId")
    '        dr1("SecurityId") = dr("SecurityId")
    '        dr1("UserId") = dr("UserId")
    '        dr1("FileType") = dr("FileType")
    '        dr1("FileName") = dr("FileName")
    '        dr1("FileData") = dr("FileData")
    '        dt1.Rows.Add(dr1)
    '    Next
    '    Session("SecurityDocs") = dt1
    '    DeleteBlankRow(dt1, "FileType")
    '    If (dt1.Rows.Count = 0) Then
    '        dr = dt1.NewRow
    '        dt1.Rows.Add(dr)
    '    End If
    '    gv_docs.DataSource = dt1
    '    gv_docs.DataBind()
    'End Sub
    'Protected Sub gv_docs_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_docs.RowDataBound
    '    Try
    '        objCommon.SetGridRows1(e)

    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            Dim dr As DataRow
    '            Dim imgBtnDelete As ImageButton
    '            'Dim imgBtnEdit As ImageButton
    '            Dim imgbtnedit As HtmlAnchor

    '            imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
    '            imgbtnedit = CType(e.Row.FindControl("imgBtn_Edit"), HtmlAnchor)
    '            imgbtnedit.InnerText = "Export"
    '            dr = TryCast(e.Row.DataItem, DataRowView).Row

    '            If e.Row.Cells(5).Text <> "&nbsp;" Then
    '                If e.Row.Cells(2).Text = "&nbsp;" Then
    '                    imgbtnedit.Visible = False
    '                Else
    '                    imgbtnedit.HRef = "ShowDocument.aspx" + "?Id=" + e.Row.Cells(2).Text + "&Type=SDD"
    '                    imgbtnedit.Visible = True
    '                End If
    '                imgBtnDelete.Visible = True

    '            Else
    '                imgbtnedit.Visible = False
    '                imgBtnDelete.Visible = False

    '            End If
    '        End If
    '    Catch ex As Exception

    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub

    'Protected Sub gv_docs_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_docs.RowCommand
    '    Try
    '        Dim imgBtn As ImageButton
    '        Dim gvRow As GridViewRow
    '        Dim dt As DataTable

    '        dt = ViewState("ContactTable")

    '        If e.CommandName = "EditRow" Then

    '            imgBtn = TryCast(e.CommandSource, ImageButton)
    '            gvRow = imgBtn.Parent.Parent
    '        ElseIf e.CommandName = "DeleteRow" Then
    '            imgBtn = TryCast(e.CommandSource, ImageButton)
    '            gvRow = imgBtn.Parent.Parent
    '            'ViewState("CustDelFlag") = True
    '            Dim t As String = Convert.ToString(gv_docs.Rows(gvRow.DataItemIndex).Cells(2).Text)
    '            If t <> "" And t <> "&nbsp;" Then
    '                Hid_DelSecurityDoc.Value = Hid_DelSecurityDoc.Value & t & "!"
    '            End If
    '            DeleteGridRows("SecurityDocs", gvRow.DataItemIndex, gv_docs)
    '        End If

    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHidShow", "ShowHidShow();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowDocs", "ShowDocs();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowDis", "ShowDistributors();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Showunderritten", "Showunderritten();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "solearrangerhideshow", "solearrangerhideshow();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowDS", "ShowDS();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getArrangerlist", "getArrangerlist();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getDistributorlist", "getDistributorlist();", True)
    '    Catch ex As Exception

    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub

    'Private Sub DeleteGridRows(ByVal strSessionName As String, ByVal intIndex As Int16, ByVal grdView As Object)
    '    Try
    '        Dim dt As DataTable
    '        Dim objSearch As New clsSearch
    '        Dim dr As DataRow
    '        dt = Session(strSessionName)
    '        dt.Rows.RemoveAt(intIndex)
    '        If strSessionName = "SecurityDocs" And dt.Rows.Count = 0 Then
    '            dr = dt.NewRow
    '            dt.Rows.Add(dr)
    '        End If
    '        Session(strSessionName) = dt
    '        grdView.DataSource = dt
    '        grdView.DataBind()
    '        ViewState("IssueDetailsEditFlag") = False
    '    Catch ex As Exception
    '        objUtil.WritErrorLog(PgName, "DeleteGridRows", "Error in DeleteGridRows", "", ex)

    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub
    'Protected Sub btnAttatchDocs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttatchDocs.Click
    '    Dim dt As New DataTable
    '    Dim filename As String = ""
    '    Dim ext As String = ""
    '    Try
    '        If FileUpload2.HasFile = True Then
    '            ' Dim dt As DataTable
    '            Dim dr As DataRow
    '            dt = CType(Session("SecurityDocs"), DataTable).Copy
    '            DeleteBlankRow(dt, "FileType")
    '            dr = dt.NewRow
    '            dr("userId") = Val(Session("UserId") & "")
    '            dr("FileType") = cboDocumentType1.SelectedItem.Text.ToString()
    '            dr("FileName") = FileUpload2.FileName
    '            Dim filebytes() As Byte = FileUpload2.FileBytes
    '            dr("FileData") = filebytes
    '            dt.Rows.Add(dr)
    '            Session("SecurityDocs") = dt
    '            gv_docs.DataSource = dt
    '            gv_docs.DataBind()
    '        End If

    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHidShow", "ShowHidShow();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowDocs", "ShowDocs();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowDis", "ShowDistributors();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Showunderritten", "Showunderritten();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "solearrangerhideshow", "solearrangerhideshow();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowDS", "ShowDS();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getArrangerlist", "getArrangerlist();", True)
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getDistributorlist", "getDistributorlist();", True)

    '    Catch ex As Exception

    '    End Try
    '    'gv_docs.DataSource = CType(Session("IssueDocs"), DataTable).Copy
    '    'gv_docs.DataBind()
    'End Sub

    'Private Sub DeleteBlankRow(ByRef dt As DataTable, ByVal strColName As String)
    '    Try
    '        For I As Int16 = 0 To dt.Rows.Count - 1
    '            If String.IsNullOrEmpty(dt.Rows(I).Item(strColName).ToString()) Then
    '                dt.Rows.RemoveAt(I)
    '                Exit Sub
    '            End If
    '        Next
    '    Catch ex As Exception
    '        objUtil.WritErrorLog(PgName, "DeleteBlankRow", "Error in DeleteBlankRow", "", ex)

    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
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

    Private Sub FillBlankGrids()
        Try
            Dim dtInfoGrid As New DataTable
            dtInfoGrid.Columns.Add("SecurityInfoDate", GetType(String))
            dtInfoGrid.Columns.Add("SecurityInfoAmt", GetType(String))
            dtInfoGrid.Columns.Add("SecurityInfoAmt1", GetType(String))
            Session("MaturityTable") = dtInfoGrid
            Session("CouponTable") = dtInfoGrid
            Session("CallTable") = dtInfoGrid
            Session("PutTable") = dtInfoGrid
            dg_Maturity.DataSource = dtInfoGrid
            dg_Coupon.DataSource = dtInfoGrid
            dg_Call.DataSource = dtInfoGrid
            dg_Put.DataSource = dtInfoGrid
            dg_Maturity.DataBind()
            dg_Coupon.DataBind()
            dg_Call.DataBind()
            dg_Put.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillBlankRatingGrids()
        Try
            Dim dtInfoGrid As New DataTable
            dtInfoGrid.Columns.Add("OrganizationName", GetType(String))
            dtInfoGrid.Columns.Add("Rating", GetType(String))
            dtInfoGrid.Columns.Add("RatingDate", GetType(String))
            dtInfoGrid.Columns.Add("SecurityId", GetType(String))
            dtInfoGrid.Columns.Add("RatingOrganizationId", GetType(String))
            dtInfoGrid.Columns.Add("RatingId", GetType(String))
            dtInfoGrid.Columns.Add("SecurityRatingId", GetType(String))
            Session("SecurityRatingTable") = dtInfoGrid
            dg_Rating.DataSource = dtInfoGrid
            dg_Rating.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetAttributes()
        txt_IssuerOfSecurrity.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_NameOfSecurity.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Granttedby.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Remark.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_IssuePrice.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_FaceValue.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_IssuePrice.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_NSDLFaceValue.Attributes.Add("onkeypress", "OnlyInteger();")
        txt_ISINNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_InterstDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Issuedate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Issuedate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_BookCloserDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_DMATBookCloserDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_InterstDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_BookCloserDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_DMATBookCloserDate.Attributes.Add("onblur", "CheckDate(this,false);")
        cbo_FrequencyOfInterest.Attributes.Add("onchange", "Hidegrids();")
        Cbo_CouponOn.Attributes.Add("onchange", "Hidegrids();")
        cbo_InstrumentName.Attributes.Add("onchange", "Hidegrids();")
        txt_RatingDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_RatingDate.Attributes.Add("onblur", "CheckDate(this,false);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
        Add_Rating.Attributes.Add("onclick", "return ValidateRating();")
        txt_Rating1.Attributes.Add("onkeypress", "OnlyCharacter();")
        txt_SecurityCode.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_SecurityTypeCode.Attributes.Add("onblur", "ConvertUCase(this);")
        'btnAttatchDocs.Attributes.Add("onclick", "return ValidateDocumentDetails();")
    End Sub
    Private Sub SetControls()
        Try
            OpenConn()
            'srh_IssuerOfSecurity.Columns.Add("IssuerName")
            'srh_IssuerOfSecurity.Columns.Add("RDMIssuerId")
            objCommon.FillControl(cbo_SecurityType, sqlConn, "ID_FILL_SecurityTypeMaster", "SecurityTypeName", "SecurityTypeId")
            objCommon.FillControl(Cbo_Exchange, sqlConn, "ID_FILL_ExchangeMaster", "ExchangeName", "ExchangeId")
            objCommon.FillControl(cbo_RatingOrg, sqlConn, "ID_FILL_RatingOrganization", "OrganizationName", "RatingOrganizationId")
            objCommon.FillControl(cbo_Rating, sqlConn, "ID_FILL_RatingOrganizationWiseRating", "Rating", "RatingId")
            txt_FaceValue.Text = "10"
            txt_IssuePrice.Text = "10"
            txt_NSDLFaceValue.Text = "10"
            cbo_Fv.SelectedValue = "100000"
            cbo_PriceMultiple.SelectedValue = "100000"
            Page.SetFocus(cbo_SecurityType)
            row_InterestDet.Visible = True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillCombo()
        Try
            OpenConn()
            Dim ds As New DataSet()
            ds = objComm.FillAllCombo("ID_Fill_SecurityDocumentsList")
            Hid_DocumentMaster.Value = objComm.Trim(ds.Tables(0).Rows(0)("Document").ToString())
            'cboDocumentType1.DataSource = ds.Tables(0)
            'cboDocumentType1.DataValueField = "Id"
            'cboDocumentType1.DataTextField = "Name"
            'cboDocumentType1.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function SaveKycDocuments_New(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim lstParam As List(Of SqlParameter) = New List(Of SqlParameter)
            Dim blnReturn As Boolean = False
            Dim dtDocumnet As DataTable = New DataTable
            Dim filename As String = ""
            Dim ext As String = ""
            Dim contenttype As String = ""
            If Val(ViewState("Id") & "") = 0 Then
                lstParam.Add(New SqlParameter("@SecurityId", 0))
            Else
                lstParam.Add(New SqlParameter("@SecurityId", ViewState("Id")))
            End If
            dtDocumnet.Columns.Add("Id", GetType(Int32))
            dtDocumnet.Columns.Add("DocumentId", GetType(Int32))
            dtDocumnet.Columns.Add("FileName", GetType(String))
            dtDocumnet.Columns.Add("FileData", GetType(System.Byte()))
            dtDocumnet.Columns.Add("FileType", GetType(String))
            dtDocumnet.Columns.Add("FileExtension", GetType(String))
            Dim Id() As String = Hid_SecurityDocumentId.Value.Split(Microsoft.VisualBasic.ChrW(44))
            Dim DocumentId() As String = Hid_DocumentId.Value.Split(Microsoft.VisualBasic.ChrW(44))
            If (Hid_SecurityDocumentId.Value <> "") Then
                Dim i As Integer = 0
                Do While (i < Id.Length)
                    Dim PostedFile As HttpPostedFile = Request.Files(i)
                    Dim datarow As DataRow = dtDocumnet.NewRow
                    datarow("Id") = objComm.Val(Id(i))
                    datarow("DocumentId") = objComm.Val(DocumentId(i))
                    If (PostedFile.ContentLength > 0) Then
                        filename = PostedFile.FileName
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
            If objComm.InsertUpdateDetails(lstParam, "ID_InsertUpdate_SecurityDocumentsDetails") > 0 Then
                blnReturn = True
            End If
            Return blnReturn
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveKycDocuments", "Error in SaveKycDocuments", "", ex)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try


    End Function

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            'If (cbo_SecurityType.SelectedValue = 54 Or cbo_SecurityType.SelectedValue = 59 Or cbo_SecurityType.SelectedValue = 66 Or cbo_SecurityType.SelectedValue = 61) Then
            If objCommon.CheckDuplicate(sqlConn, "SecurityMaster", "SecurityName", Trim(txt_NameOfSecurity.Text)) = False Then
                Dim msg As String = "Security Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            'Else
            If objCommon.CheckDuplicate(sqlConn, "SecurityMaster", "NSDLAcNumber", Trim(txt_ISINNo.Text)) = False Then
                If txt_ISINNo.Text <> "" Then
                    Dim msg As String = "Security Name With This ISIN No Already Exist"
                    Dim strHtml As String
                    strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                    Exit Sub
                End If
            End If
            'End If
            SetSaveUpdate("ID_INSERT_SecurityMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If (cbo_SecurityType.SelectedValue = 54 Or cbo_SecurityType.SelectedValue = 59 Or cbo_SecurityType.SelectedValue = 66 Or cbo_SecurityType.SelectedValue = 61) Then
                If objCommon.CheckDuplicate(sqlConn, "SecurityMaster", "SecurityName", Trim(txt_NameOfSecurity.Text), "Securityid", Val(ViewState("Id"))) = False Then
                    Dim msg As String = "Security Name Already Exist"
                    Dim strHtml As String
                    strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                    Exit Sub
                End If
            Else
                If objCommon.CheckDuplicate(sqlConn, "SecurityMaster", "NSDLAcNumber", Trim(txt_ISINNo.Text), "Securityid", Val(ViewState("Id"))) = False Then

                    If txt_ISINNo.Text <> "" Then
                        Dim msg As String = "Security Name With This ISIN No Already Exist"
                        Dim strHtml As String
                        strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                        Exit Sub
                    End If
                End If
            End If
            SetSaveUpdate("ID_UPDATE_SecurityMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            IPDate()
            DatesRate()
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            If DeleteDetails(sqlTrans) = False Then Exit Sub
            If SaveSecurityInfo(sqlTrans) = False Then Exit Sub
            If SaveRatings(sqlTrans) = False Then Exit Sub
            'If SaveDocs(sqlTrans) = False Then Exit Sub
            If SaveKycDocuments_New(sqlTrans) = False Then Exit Sub
            If Hid_ReCalculateIPDates.Value = "Y" Then If CalculateCashflow(sqlTrans) = False Then Exit Sub

            sqlTrans.Commit()
            Response.Redirect("SecurityMasterDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
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
            sqlComm.Parameters.Clear()
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@SecurityTypeId", SqlDbType.Int, 4, "I", , , Val(cbo_SecurityType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))

            objCommon.SetCommandParameters(sqlComm, "@SecurityIssuer", SqlDbType.VarChar, 250, "I", , , Trim(srh_IssuerOfSecurity.SearchTextBox.Text))
            objCommon.SetCommandParameters(sqlComm, "@SecurityName", SqlDbType.VarChar, 250, "I", , , Trim(txt_NameOfSecurity.Text))
            objCommon.SetCommandParameters(sqlComm, "@GuarantedBy", SqlDbType.VarChar, 250, "I", , , Trim(txt_Granttedby.Text))
            objCommon.SetCommandParameters(sqlComm, "@FaceValue", SqlDbType.Decimal, 9, "I", , , Val(txt_FaceValue.Text) * Val(cbo_Fv.SelectedValue) & "")
            objCommon.SetCommandParameters(sqlComm, "@FVMultiple", SqlDbType.BigInt, 9, "I", , , Val(cbo_Fv.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@IssuePrice", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(txt_IssuePrice.Text))
            objCommon.SetCommandParameters(sqlComm, "@PriceMultiple", SqlDbType.BigInt, 9, "I", , , Val(cbo_PriceMultiple.SelectedValue))
            If txt_Issuedate.Text <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@IssueDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_Issuedate.Text))
            End If
            objCommon.SetCommandParameters(sqlComm, "@FrequencyOfInterest", SqlDbType.Char, 1, "I", , , Trim(cbo_FrequencyOfInterest.SelectedValue))
            If txt_InterstDate.Text <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@FirstInterestDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_InterstDate.Text))

            End If
            If txt_SecurityCode.Text <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@SecurityCode", SqlDbType.VarChar, 50, "I", , , Trim(txt_SecurityCode.Text))
            Else
                objCommon.SetCommandParameters(sqlComm, "@SecurityCode", SqlDbType.VarChar, 50, "I", , , DBNull.Value)
            End If

            If txt_SecurityTypeCode.Text <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@SecurityTypeCode", SqlDbType.VarChar, 50, "I", , , Trim(txt_SecurityTypeCode.Text))
            Else
                objCommon.SetCommandParameters(sqlComm, "@SecurityTypeCode", SqlDbType.VarChar, 50, "I", , , DBNull.Value)
            End If
            If txt_BookCloserDate.Text <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@BookClosureDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_BookCloserDate.Text))
            End If
            If txt_DMATBookCloserDate.Text <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@DMATBookClosureDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_DMATBookCloserDate.Text))
            End If

            If matDate <> Date.MinValue Then
                objCommon.SetCommandParameters(sqlComm, "@MaturityDate", SqlDbType.SmallDateTime, 4, "I", , , matDate)
            End If
            If callDate <> Date.MinValue Then
                objCommon.SetCommandParameters(sqlComm, "@CallDate", SqlDbType.SmallDateTime, 4, "I", , , callDate)
            End If

            If Hid_CouponRate.Value <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@CouponRate", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Hid_CouponRate.Value))
            End If

            objCommon.SetCommandParameters(sqlComm, "@Remark", SqlDbType.VarChar, 500, "I", , , txt_Remark.Text)
            objCommon.SetCommandParameters(sqlComm, "@NSDLFaceValue", SqlDbType.Decimal, 9, "I", , , Val(txt_NSDLFaceValue.Text) * Val(cbo_PriceMultiple.SelectedValue) & "")
            objCommon.SetCommandParameters(sqlComm, "@ExchangeId", SqlDbType.Int, 4, "I", , , Val(Cbo_Exchange.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@CouponOn", SqlDbType.Char, 1, "I", , , Trim(Cbo_CouponOn.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Rating1", SqlDbType.VarChar, 50, "I", , , txt_Rating1.Text)
            objCommon.SetCommandParameters(sqlComm, "@Rating2", SqlDbType.VarChar, 50, "I", , , txt_Rating2.Text)
            objCommon.SetCommandParameters(sqlComm, "@Rating3", SqlDbType.VarChar, 50, "I", , , txt_Rating3.Text)
            objCommon.SetCommandParameters(sqlComm, "@ShortRating1", SqlDbType.VarChar, 50, "I", , , txt_ShortRating1.Text)
            objCommon.SetCommandParameters(sqlComm, "@ShortRating2", SqlDbType.VarChar, 50, "I", , , txt_ShortRating2.Text)
            objCommon.SetCommandParameters(sqlComm, "@ShortRating3", SqlDbType.VarChar, 50, "I", , , txt_ShortRating3.Text)
            'objCommon.SetCommandParameters(sqlComm, "@Company1", SqlDbType.VarChar, 50, "I", , , txt_Company1.Text)
            'objCommon.SetCommandParameters(sqlComm, "@Company2", SqlDbType.VarChar, 50, "I", , , txt_Company2.Text)
            'objCommon.SetCommandParameters(sqlComm, "@Company3", SqlDbType.VarChar, 50, "I", , , txt_Company3.Text)
            'If txt_Date1.Text <> "" Then
            '    objCommon.SetCommandParameters(sqlComm, "@RatingDate1", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_Date1.Text))
            'End If
            'If txt_Date2.Text <> "" Then
            '    objCommon.SetCommandParameters(sqlComm, "@RatingDate2", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_Date2.Text))
            'End If
            'If txt_Date3.Text <> "" Then
            '    objCommon.SetCommandParameters(sqlComm, "@RatingDate3", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_Date3.Text))
            'End If
            objCommon.SetCommandParameters(sqlComm, "@NSDLAcNumber", SqlDbType.VarChar, 20, "I", , , txt_ISINNo.Text)
            objCommon.SetCommandParameters(sqlComm, "@CSDLAcNumber", SqlDbType.VarChar, 20, "I", , , txt_RBILoanCode.Text)

            'objCommon.SetCommandParameters(sqlComm, "@SlrNonSlrFlag", SqlDbType.Char, 1, "I", , , Trim(rdoList_SlrNonSlr.SelectedValue))

            objCommon.SetCommandParameters(sqlComm, "@MaxActualFlag", SqlDbType.Char, 1, "I", , , Trim(rdo_MaxActual.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@IPDates", SqlDbType.VarChar, 50, "I", , , (Hid_IPDate.Value))
            'objCommon.SetCommandParameters(sqlComm, "@MaturityFlag", SqlDbType.Char, 1, "I", , , Trim(rdo_MatDate.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@AccIntDays", SqlDbType.VarChar, 5, "I", , , Trim(rdo_AccIntDays.SelectedValue))

            objCommon.SetCommandParameters(sqlComm, "@InterestOnHoliday", SqlDbType.Bit, 1, "I", , , Val(rdo_InterstHolidays.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@InterestOnSat", SqlDbType.Bit, 1, "I", , , Val(rdo_InterestSat.SelectedValue))

            objCommon.SetCommandParameters(sqlComm, "@MaturityOnHoliday", SqlDbType.Bit, 1, "I", , , Val(rdo_MaturityHolidays.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@MaturityOnSat", SqlDbType.Bit, 1, "I", , , Val(rdo_InterestSatMaturity.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@NormCompoundInt", SqlDbType.Char, 1, "I", , , Trim(rdo_NormCompound.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@NatureOFInstrument", SqlDbType.Char, 50, "I", , , Trim(cbo_InstrumentName.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@InterestDaysType", SqlDbType.SmallInt, 4, "I", , , Val(cboInterestDaysType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@IssuerIdRDM", SqlDbType.Int, 4, "I", , , Val(srh_IssuerOfSecurity.SelectedId))
            If Val(cbo_SecurityCategory.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@SecurityCatId", SqlDbType.Int, 4, "I", , , Val(cbo_SecurityCategory.SelectedValue))
            End If

            Dim dtMaturity As DataTable = Session("MaturityTable")
            Dim dblMaturity As Double = 0
            Dim intMaturity As Integer = 0
            For Each row As DataRow In dtMaturity.Rows
                dblMaturity += Val(row("SecurityInfoAmt"))
                intMaturity += 1
            Next
            If intMaturity > 1 Then
                objCommon.SetCommandParameters(sqlComm, "@IsStaggered", SqlDbType.Bit, 1, "I", , , 1)
            End If
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@SecurityId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    'Private Function SaveDocs(ByVal sqlTrans As SqlTransaction) As Boolean
    '    Try
    '        Dim sqlComm As New SqlCommand
    '        sqlComm.CommandText = "ID_INSERT_SecurityDocDetails"
    '        sqlComm.Transaction = sqlTrans
    '        sqlComm.CommandType = CommandType.StoredProcedure
    '        sqlComm.Connection = sqlConn

    '        Dim dt As DataTable
    '        Dim dr As DataRow
    '        dt = CType(Session("SecurityDocs"), DataTable).Copy
    '        DeleteBlankRow(dt, "FileType")
    '        For i As Int16 = 0 To dt.Rows.Count - 1
    '            dr = dt.Rows(i)
    '            'If (Val(dr("IssueDocId") & "") = 0) Then
    '            sqlComm.Parameters.Clear()
    '            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
    '            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Session("UserId"))
    '            objCommon.SetCommandParameters(sqlComm, "@SecurityDocumentId", SqlDbType.Int, 4, "O")
    '            objCommon.SetCommandParameters(sqlComm, "@FileName", SqlDbType.VarChar, 200, "I", , , dr("FileName"))
    '            objCommon.SetCommandParameters(sqlComm, "@FileData", SqlDbType.VarBinary, 0, "I", , , dr("FileData"))
    '            objCommon.SetCommandParameters(sqlComm, "@FileType", SqlDbType.VarChar, 50, "I", , , dr("FileType"))
    '            objCommon.SetCommandParameters(sqlComm, "@ret_code", SqlDbType.Int, 4, "O")
    '            sqlComm.ExecuteNonQuery()
    '            'End If
    '        Next
    '        Return True
    '    Catch ex As Exception
    '        sqlTrans.Rollback()
    '        objUtil.WritErrorLog(PgName, "SaveDocs", "Error in SaveDepositry", "", ex)

    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("SecurityMasterDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("SecurityMasterDetail.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillFields()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable1(sqlConn, "ID_FILL_SecurityMaster", ViewState("Id"), "SecurityId")
            If dt.Rows.Count > 0 Then
                cbo_SecurityType.SelectedValue = Val(dt.Rows(0).Item("SecurityTypeId") & "")
                objCommon.FillControl(cbo_SecurityCategory, sqlConn, "ID_FILL_SecurityCategoryMaster", "SecurityCategory", "SecurityCatId", Val(cbo_SecurityType.SelectedValue), "SecurityTypeId")
                cbo_SecurityCategory.SelectedValue = Val(dt.Rows(0).Item("SecurityCatId") & "")
                cbo_InstrumentName.SelectedValue = Trim(dt.Rows(0).Item("NatureOFInstrument") & "")
                txt_Granttedby.Text = Trim(dt.Rows(0).Item("GuarantedBy") & "")
                txt_IssuerOfSecurrity.Text = Trim(dt.Rows(0).Item("SecurityIssuer") & "")
                txt_NameOfSecurity.Text = Trim(dt.Rows(0).Item("SecurityName") & "")
                txt_FaceValue.Text = Val(dt.Rows(0).Item("FaceValue") & "") / Val(dt.Rows(0).Item("FVMultiple") & "")
                'txt_Issuedate.Text = Format(dt.Rows(0).Item("IssueDate"), "dd/MM/yyyy")
                txt_Issuedate.Text = Trim(dt.Rows(0).Item("IssueDate") & "")
                txt_IssuePrice.Text = Trim(dt.Rows(0).Item("IssuePrice") & "")
                cbo_PriceMultiple.SelectedValue = Val(dt.Rows(0).Item("PriceMultiple") & "")
                cbo_Fv.SelectedValue = Val(dt.Rows(0).Item("FVMultiple") & "")
                cbo_FrequencyOfInterest.SelectedValue = Trim(dt.Rows(0).Item("FrequencyOfInterest") & "")
                'txt_FaceValue.Text = Trim(dt.Rows(0).Item("FaceValue") & "")
                txt_InterstDate.Text = Trim(dt.Rows(0).Item("FirstInterestDate") & "")
                txt_BookCloserDate.Text = Trim(dt.Rows(0).Item("BookClosureDate") & "")
                txt_DMATBookCloserDate.Text = Trim(dt.Rows(0).Item("DMATBookClosureDate") & "")
                txt_Remark.Text = Trim(dt.Rows(0).Item("Remark") & "")
                txt_Rating1.Text = Trim(dt.Rows(0).Item("Rating1") & "")
                txt_Rating2.Text = Trim(dt.Rows(0).Item("Rating2") & "")
                txt_Rating3.Text = Trim(dt.Rows(0).Item("Rating3") & "")
                txt_ShortRating1.Text = Trim(dt.Rows(0).Item("ShortRating1") & "")
                txt_ShortRating2.Text = Trim(dt.Rows(0).Item("ShortRating2") & "")
                txt_ShortRating3.Text = Trim(dt.Rows(0).Item("ShortRating3") & "")
                'txt_Company1.Text = Trim(dt.Rows(0).Item("Company1") & "")
                'txt_Company2.Text = Trim(dt.Rows(0).Item("Company2") & "")
                'txt_Company3.Text = Trim(dt.Rows(0).Item("Company3") & "")
                'txt_Date1.Text = Trim(dt.Rows(0).Item("RatingDate1") & "")
                'txt_Date2.Text = Trim(dt.Rows(0).Item("RatingDate2") & "")
                'txt_Date3.Text = Trim(dt.Rows(0).Item("RatingDate3") & "")
                txt_ISINNo.Text = Trim(dt.Rows(0).Item("NSDLAcNumber") & "")
                txt_NSDLFaceValue.Text = Val(dt.Rows(0).Item("NSDLFaceValue") & "") / Val(dt.Rows(0).Item("PriceMultiple") & "")
                txt_RBILoanCode.Text = Trim(dt.Rows(0).Item("CSDLAcNumber") & "")
                Cbo_Exchange.SelectedValue = Trim(dt.Rows(0).Item("ExchangeId") & "")
                Cbo_CouponOn.SelectedValue = Trim(dt.Rows(0).Item("CouponOn") & "")
                'rdoList_SlrNonSlr.SelectedValue = Trim(dt.Rows(0).Item("SlrNonSlrFlag") & "")
                rdo_MaxActual.SelectedValue = Trim(dt.Rows(0).Item("MaxActualFlag") & "")
                'Hid_TypeFlag.value = Trim(dt.Rows(0).Item("TypeFlag") & "")
                txt_SecurityCode.Text = Trim(dt.Rows(0).Item("SecurityCode") & "")
                txt_SecurityTypeCode.Text = Trim(dt.Rows(0).Item("SecurityTypeCode") & "")
                cboInterestDaysType.SelectedValue = Trim(dt.Rows(0).Item("InterestDaysType") & "")
                rdo_AccIntDays.SelectedValue = Trim(dt.Rows(0).Item("AccIntDays") & "")
                rdo_NormCompound.SelectedValue = Trim(dt.Rows(0).Item("NormCompoundInt") & "")
                srh_IssuerOfSecurity.SelectedId = Val(dt.Rows(0).Item("IssuerIdRDM") & "")
                srh_IssuerOfSecurity.SearchTextBox.Text = dt.Rows(0).Item("IssuerName").ToString

                lbl_CreatedBy.Text = dt.Rows(0).Item("CreatedBy").ToString
                lbl_CreationDate.Text = dt.Rows(0).Item("lbl_CreationDate").ToString
                If Trim(dt.Rows(0).Item("IssueDateInclusive") & "") <> "" Then
                    If dt.Rows(0).Item("IssueDateInclusive") Then
                        rdo_IssueDateInclusive.SelectedValue = 1
                    Else
                        rdo_IssueDateInclusive.SelectedValue = 0
                    End If
                End If

                If Trim(dt.Rows(0).Item("MaturityDateInclusive") & "") <> "" Then
                    If dt.Rows(0).Item("MaturityDateInclusive") Then
                        rdo_MaturityDateInclusive.SelectedValue = 1
                    Else
                        rdo_MaturityDateInclusive.SelectedValue = 0
                    End If
                End If
                If cbo_FrequencyOfInterest.SelectedValue = "N" Then
                    row_InterestDet.Visible = False
                Else
                    row_InterestHoliday.Visible = True
                    row_MaturityInterest.Visible = True

                    If (dt.Rows(0).Item("InterestOnHoliday")) Then
                        row_Interest.Attributes.Add("style", "display:")
                        If (dt.Rows(0).Item("InterestOnHoliday")) Then
                            rdo_InterstHolidays.SelectedValue = 1
                        Else
                            rdo_InterstHolidays.SelectedValue = 0
                        End If
                        If (dt.Rows(0).Item("InterestOnSat")).ToString <> "" Then
                            If (dt.Rows(0).Item("InterestOnSat")) Then
                                rdo_InterestSat.SelectedValue = 1
                            Else
                                rdo_InterestSat.SelectedValue = 0
                            End If
                        Else
                            rdo_InterestSat.SelectedValue = 0
                        End If
                    Else
                        If (dt.Rows(0).Item("InterestOnHoliday")) Then
                            rdo_InterstHolidays.SelectedValue = 1
                        Else
                            rdo_InterstHolidays.SelectedValue = 0
                        End If
                        row_Interest.Attributes.Add("style", "display:none")
                    End If
                End If


                If (dt.Rows(0).Item("MaturityOnHoliday")) Then
                    row_MaturitySat.Attributes.Add("style", "display:")
                    If (dt.Rows(0).Item("MaturityOnHoliday")) Then
                        rdo_MaturityHolidays.SelectedValue = 1
                    Else
                        rdo_MaturityHolidays.SelectedValue = 0
                    End If
                    If (dt.Rows(0).Item("MaturityOnSat")).ToString <> "" Then
                        If (dt.Rows(0).Item("MaturityOnSat")) Then
                            rdo_InterestSatMaturity.SelectedValue = 1
                        Else
                            rdo_InterestSatMaturity.SelectedValue = 0
                        End If
                    Else
                        rdo_InterestSatMaturity.SelectedValue = 0
                    End If
                Else
                    If (dt.Rows(0).Item("MaturityOnHoliday")) Then
                        rdo_MaturityHolidays.SelectedValue = 1
                    Else
                        rdo_MaturityHolidays.SelectedValue = 0
                    End If
                    row_MaturitySat.Attributes.Add("style", "display:none")
                End If
                Hid_SecurityCashFlow.Value = Trim(dt.Rows(0).Item("SecurityCashFlow") & "")
                Hid_DocumentDetails.Value = Trim(dt.Rows(0).Item("DocumentDetails") & "")
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub dg_Maturity_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Maturity.ItemCommand
        SetInfoGrid(dg_Maturity, e, "MaturityTable")
    End Sub


    Protected Sub dg_Coupon_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Coupon.ItemCommand
        SetInfoGrid(dg_Coupon, e, "CouponTable")
    End Sub

    Protected Sub dg_Put_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Put.ItemCommand
        SetInfoGrid(dg_Put, e, "PutTable")
    End Sub

    Protected Sub dg_Call_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Call.ItemCommand
        SetInfoGrid(dg_Call, e, "CallTable")
    End Sub
    Private Sub SetInfoGrid(ByVal objGrid As DataGrid, ByVal e As DataGridCommandEventArgs, ByVal strSessionName As String)
        Try
            Dim dtGrid As DataTable
            dtGrid = TryCast(Session(strSessionName), DataTable).Copy
            If e.CommandName.ToUpper() = "ADD" Or e.CommandName.ToUpper() = "UPDATE" Then
                Dim dr As DataRow
                If e.CommandName.ToUpper() = "ADD" Then
                    dr = dtGrid.NewRow
                Else
                    dr = dtGrid.Rows(e.Item.ItemIndex)
                End If
                dr("SecurityInfoDate") = Trim(TryCast(e.Item.FindControl("txt_SecurityInfoDate"), TextBox).Text)
                dr("SecurityInfoAmt") = Val(TryCast(e.Item.FindControl("txt_SecurityInfoAmt"), TextBox).Text)
                If strSessionName = "CouponTable" Then
                    dr("SecurityInfoAmt1") = Val(TryCast(e.Item.FindControl("txt_SecurityInfoAmt1"), TextBox).Text)
                End If

                If e.CommandName.ToUpper() = "ADD" Then
                    dtGrid.Rows.Add(dr)
                End If
                objGrid.EditItemIndex = -1
            ElseIf e.CommandName.ToUpper() = "DELETE" Then
                dtGrid.Rows.RemoveAt(e.Item.ItemIndex)
            ElseIf e.CommandName.ToUpper() = "EDIT" Then
                objGrid.EditItemIndex = e.Item.ItemIndex
            ElseIf e.CommandName.ToUpper() = "CANCEL" Then
                objGrid.EditItemIndex = -1
            End If
            Session(strSessionName) = dtGrid
            objGrid.DataSource = dtGrid
            objGrid.DataBind()
            'Session("SecurityDetailsTable") = dtGrid
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function SaveSecurityInfo(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As New DataTable
            Dim arrColNames() As String = {"MaturityTable", "CouponTable", "CallTable", "PutTable"}
            Dim chrTypeFlag() As Char = {"M", "I", "C", "P"}

            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_SecurityInfo"
            sqlComm.Connection = sqlConn

            For J As Int16 = 0 To arrColNames.Length - 1
                dt = Session(arrColNames(J))
                If dt.Rows.Count > 0 Then
                    For I As Int16 = 0 To dt.Rows.Count - 1
                        If Val(dt.Rows(I).Item("SecurityInfoAmt").ToString) = 0 And
                            (cbo_InstrumentName.SelectedValue = "P" And chrTypeFlag(J) = "M") Or
                            (cbo_FrequencyOfInterest.SelectedValue = "N" And chrTypeFlag(J) = "I") Then
                        Else
                            sqlComm.Parameters.Clear()
                            If Trim(dt.Rows(I).Item("SecurityInfoDate").ToString) = "" Then
                                objCommon.SetCommandParameters(sqlComm, "@SecurityInfoDate", SqlDbType.DateTime, 8, "I", , , Date.MaxValue)
                            Else
                                objCommon.SetCommandParameters(sqlComm, "@SecurityInfoDate", SqlDbType.DateTime, 8, "I", , , objCommon.DateFormat(dt.Rows(I).Item("SecurityInfoDate")))
                            End If
                            objCommon.SetCommandParameters(sqlComm, "@SecurityInfoAmt", SqlDbType.Decimal, 9, "I", , , Val(dt.Rows(I).Item("SecurityInfoAmt")))
                            If Val(dt.Rows(I).Item("SecurityInfoAmt1") & "") <> 0 Then
                                objCommon.SetCommandParameters(sqlComm, "@SecurityInfoAmt1", SqlDbType.Decimal, 9, "I", , , Val(dt.Rows(I).Item("SecurityInfoAmt1")))
                            Else
                                objCommon.SetCommandParameters(sqlComm, "@SecurityInfoAmt1", SqlDbType.Decimal, 9, "I", , , 0.0)
                            End If
                            objCommon.SetCommandParameters(sqlComm, "@SecInfoUserId", SqlDbType.BigInt, 4, "I", , , Session("UserId"))
                            objCommon.SetCommandParameters(sqlComm, "@TypeFlag", SqlDbType.Char, 1, "I", , , chrTypeFlag(J))
                            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                            objCommon.SetCommandParameters(sqlComm, "@SecurityInfoId", SqlDbType.Int, 4, "O")
                            objCommon.SetCommandParameters(sqlComm, "@Intflag", SqlDbType.Int, 4, "O")
                            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 50, "O")
                            sqlComm.ExecuteNonQuery()
                        End If
                    Next
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function



    Private Function DeleteDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_SecurityInfo"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.VarChar, 100, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function DeleteCustomerApproval(ByVal sqltrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqltrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_CustomerApprovals"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.VarChar, 100, "O")
            sqlComm.ExecuteNonQuery()
            Return True

        Catch ex As Exception
            sqltrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function


    Private Sub IPDate()
        Dim dtTo As New DateTime(objCommon.DateFormat(txt_InterstDate.Text).Year, objCommon.DateFormat(txt_InterstDate.Text).Month, 1)

        dtTo = dtTo.AddMonths(1)

        dtTo = dtTo.AddDays(-(dtTo.Day))

        Dim FrequencyofInterest As String
        FrequencyofInterest = cbo_FrequencyOfInterest.SelectedValue
        If cbo_FrequencyOfInterest.SelectedValue = "Y" Then
            Hid_FreqOfInterest.Value = 1
        ElseIf cbo_FrequencyOfInterest.SelectedValue = "H" Then
            Hid_FreqOfInterest.Value = 2
        ElseIf cbo_FrequencyOfInterest.SelectedValue = "Q" Then
            Hid_FreqOfInterest.Value = 4
        ElseIf cbo_FrequencyOfInterest.SelectedValue = "M" Then
            Hid_FreqOfInterest.Value = 12
        Else
            Hid_FreqOfInterest.Value = 0
        End If
        Select Case FrequencyofInterest
            Case "N"
                IPDates = ""
            Case "Y"
                idate1 = CDate(objCommon.DateFormat(txt_InterstDate.Text))
                'IPDates = DatePart("D", idate1) & "/" & DatePart("M", idate1)
                IPDates = DatePart("D", idate1) & "-" & idate1.ToString("MMM")
            Case "H"
                idate1 = CDate(objCommon.DateFormat(txt_InterstDate.Text))
                idate2 = SecurityNext_IntDate(idate1, Val(Hid_FreqOfInterest.Value), rdo_MaxActual.SelectedValue, CDate(objCommon.DateFormat(txt_InterstDate.Text)))
                IPDates = IIf(Month(idate1) > Month(idate2),
                        Day(idate1) & "-" & idate1.ToString("MMM") & "," & Day(idate2) & "-" & idate2.ToString("MMM"),
                        Day(idate2) & "-" & idate2.ToString("MMM") & "," & Day(idate1) & "-" & idate1.ToString("MMM"))
            Case "Q"
                idate1 = CDate(objCommon.DateFormat(txt_InterstDate.Text))
                idate2 = SecurityNext_IntDate(idate1, Val(Hid_FreqOfInterest.Value), rdo_MaxActual.SelectedValue, CDate(objCommon.DateFormat(txt_InterstDate.Text)))
                idate2 = SecurityNext_IntDate(idate1, Val(Hid_FreqOfInterest.Value), rdo_MaxActual.SelectedValue, CDate(objCommon.DateFormat(txt_InterstDate.Text)))
                idate3 = SecurityNext_IntDate(idate2, Val(Hid_FreqOfInterest.Value), rdo_MaxActual.SelectedValue, CDate(objCommon.DateFormat(txt_InterstDate.Text)))
                'idate3 = DateAdd("M", 3, idate2)

                idate4 = SecurityNext_IntDate(idate3, Val(Hid_FreqOfInterest.Value), rdo_MaxActual.SelectedValue, CDate(objCommon.DateFormat(txt_InterstDate.Text)))
                ' Changed ip dates format
                'Select Case Month(idate1)
                '    Case 1, 2, 3
                '        IPDates = Day(idate1) & "/" & Month(idate1) & "," _
                '           & Day(idate2) & "/" & Month(idate2) & "," _
                '           & Day(idate3) & "/" & Month(idate3) & "," _
                '           & Day(idate4) & "/" & Month(idate4)
                '    Case 4, 5, 6
                '        IPDates = Day(idate4) & "/" & Month(idate4) & "," _
                '           & Day(idate1) & "/" & Month(idate1) & "," _
                '           & Day(idate2) & "/" & Month(idate2) & "," _
                '           & Day(idate3) & "/" & Month(idate3) & ","
                '    Case 7, 8, 9
                '        IPDates = Day(idate3) & "/" & Month(idate3) & "," _
                '           & Day(idate4) & "/" & Month(idate4) & "," _
                '           & Day(idate1) & "/" & Month(idate1) & "," _
                '           & Day(idate2) & "/" & Month(idate2)
                '    Case 10, 11, 12
                '        IPDates = Day(idate2) & "/" & Month(idate2) & "," _
                '           & Day(idate3) & "/" & Month(idate3) & "," _
                '           & Day(idate4) & "/" & Month(idate4) & "," _
                '           & Day(idate1) & "/" & Month(idate1)
                'End Select

                Select Case Month(idate1)
                    Case 1, 2, 3
                        IPDates = Day(idate1) & "-" & idate1.ToString("MMM") & "," _
                           & Day(idate2) & "-" & idate2.ToString("MMM") & "," _
                           & Day(idate3) & "-" & idate3.ToString("MMM") & "," _
                           & Day(idate4) & "-" & idate4.ToString("MMM")
                    Case 4, 5, 6
                        IPDates = Day(idate4) & "-" & idate4.ToString("MMM") & "," _
                           & Day(idate1) & "-" & idate1.ToString("MMM") & "," _
                           & Day(idate2) & "-" & idate2.ToString("MMM") & "," _
                           & Day(idate3) & "-" & idate3.ToString("MMM") & ","
                    Case 7, 8, 9
                        IPDates = Day(idate3) & "-" & idate3.ToString("MMM") & "," _
                           & Day(idate4) & "-" & idate4.ToString("MMM") & "," _
                           & Day(idate1) & "-" & idate1.ToString("MMM") & "," _
                           & Day(idate2) & "-" & idate2.ToString("MMM")
                    Case 10, 11, 12
                        IPDates = Day(idate2) & "-" & idate2.ToString("MMM") & "," _
                           & Day(idate3) & "-" & idate3.ToString("MMM") & "," _
                           & Day(idate4) & "-" & idate4.ToString("MMM") & "," _
                           & Day(idate1) & "-" & idate1.ToString("MMM")
                End Select
            Case "M"
                idate1 = CDate(objCommon.DateFormat(txt_InterstDate.Text))
                IPDates = DatePart("D", idate1)
                If dtTo = CDate(objCommon.DateFormat(txt_InterstDate.Text)) Then
                    IPDates = "Last Day of Every Month"
                ElseIf IPDates = 1 Or IPDates = 21 Or IPDates = 31 Then
                    IPDates = IPDates & "st of every month"
                ElseIf IPDates = 2 Or IPDates = 22 Then
                    IPDates = IPDates & "nd of every month"
                ElseIf IPDates = 3 Or IPDates = 23 Then
                    IPDates = IPDates & "rd of every month"
                Else
                    IPDates = IPDates & "th of every month"
                End If
        End Select
        Hid_IPDate.Value = IPDates
    End Sub
    Private Sub DatesRate()
        Try
            Dim i As Integer
            If dg_Maturity.Items.Count <> 0 Then
                For i = 0 To dg_Maturity.Items.Count - 1
                    matDate = objCommon.DateFormat(CType(dg_Maturity.Items(i).FindControl("lbl_SecurityInfoDate"), Label).Text)

                Next
            End If
            If (dg_Coupon.Items.Count <> 0) Then
                Hid_CouponRate.Value = Val(CType(dg_Coupon.Items(0).FindControl("lbl_SecurityInfoAmt"), Label).Text)
            End If
            If dg_Call.Items.Count <> 0 Then
                For i = 0 To dg_Call.Items.Count - 1
                    callDate = objCommon.DateFormat(CType(dg_Call.Items(i).FindControl("lbl_SecurityInfoDate"), Label).Text)
                Next
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillGrids()
        Try
            OpenConn()
            Dim dt As DataTable
            Dim lstItem As ListItem
            If cbo_InstrumentName.SelectedItem.Text <> "Perpetual" Then
                dt = objCommon.FillDetailsGrid(dg_Maturity, "ID_FILL_MatGrid", "SecurityId", Val(ViewState("Id") & ""), "TypeFlag", "M")
                Session("MaturityTable") = dt
            End If
            dt = objCommon.FillDetailsGrid(dg_Call, "ID_FILL_MatGrid", "SecurityId", Val(ViewState("Id") & ""), "TypeFlag", "C")
            Session("CallTable") = dt
            If cbo_FrequencyOfInterest.SelectedItem.Text <> "None" Then
                dt = objCommon.FillDetailsGrid(dg_Coupon, "ID_FILL_MatGrid", "SecurityId", Val(ViewState("Id") & ""), "TypeFlag", "I")
                Session("CouponTable") = dt
            End If
            dt = objCommon.FillDetailsGrid(dg_Put, "ID_FILL_MatGrid", "SecurityId", Val(ViewState("Id") & ""), "TypeFlag", "P")
            Session("PutTable") = dt
            dt = objCommon.FillDetailsGrid(dg_Rating, "ID_FILL_SecurityRatingDetails", "SecurityId", Val((ViewState("Id")) & ""))
            Session("SecurityRatingTable") = dt
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub cbo_SecurityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SecurityType.SelectedIndexChanged
        Try
            OpenConn()
            Dim dt As DataTable
            objCommon.FillControl(cbo_SecurityCategory, sqlConn, "ID_FILL_SecurityCategoryMaster", "SecurityCategory", "SecurityCatId", Val(cbo_SecurityType.SelectedValue), "SecurityTypeId")
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityTypeCode", Val(cbo_SecurityType.SelectedValue), "SecurityTypeid")
            If dt.Rows.Count > 0 Then
                txt_SecurityTypeCode.Text = Trim(dt.Rows(0).Item("Abbreviation") & "")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub rdo_InterstHolidays_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_InterstHolidays.SelectedIndexChanged
        Try
            If rdo_InterstHolidays.SelectedValue Then
                row_Interest.Visible = True
                rdo_InterestSat.SelectedValue = 0
            Else
                row_Interest.Visible = False
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub cbo_FrequencyOfInterest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FrequencyOfInterest.SelectedIndexChanged
        Try
            If cbo_FrequencyOfInterest.SelectedValue = "N" Then
                row_InterestDet.Visible = False
            Else
                row_InterestDet.Visible = True
                rdo_InterestSat.SelectedValue = 0
                rdo_InterestSatMaturity.SelectedValue = 0
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub rdo_MaturityHolidays_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_MaturityHolidays.SelectedIndexChanged
        Try
            If rdo_MaturityHolidays.SelectedValue Then
                row_MaturitySat.Visible = True
                rdo_InterestSatMaturity.SelectedValue = 0
            Else
                row_MaturitySat.Visible = False
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub cbo_RatingOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RatingOrg.SelectedIndexChanged
        Try
            OpenConn()
            Dim sqlcomm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dtfill As New DataTable
            Dim dvfill As New DataView

            With sqlcomm
                sqlcomm.Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_RatingOrganizationWiseRating"
                objCommon.SetCommandParameters(sqlcomm, "@RatingOrganizationId", SqlDbType.Int, 4, "I", , , Val(cbo_RatingOrg.SelectedValue))
                objCommon.SetCommandParameters(sqlcomm, "@RET_CODE", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
                sqlda.SelectCommand = sqlcomm
                sqlda.Fill(dtfill)
                dvfill = dtfill.DefaultView
                cbo_Rating.DataSource = dvfill
                cbo_Rating.DataBind()
            End With

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub Add_Rating_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Add_Rating.Click
        Try
            Dim dt As DataTable
            Dim dr As DataRow
            dt = Session("SecurityRatingTable")

            dr = dt.NewRow
            dr.Item("OrganizationName") = Trim(cbo_RatingOrg.SelectedItem.Text)
            dr.Item("Rating") = Trim(cbo_Rating.SelectedItem.Text)
            If txt_RatingDate.Text <> "" Then
                dr.Item("RatingDate") = Format(objCommon.DateFormat(txt_RatingDate.Text), "dd/MM/yyyy")
            End If
            dr.Item("RatingOrganizationId") = Val(cbo_RatingOrg.SelectedValue)
            dr.Item("RatingId") = Val(cbo_Rating.SelectedValue)
            dr.Item("SecurityRatingId") = 0
            dr.Item("SecurityId") = Val(ViewState("Id"))
            dt.Rows.Add(dr)
            Session("SecurityRatingTable") = dt
            cbo_RatingOrg.SelectedIndex = 0
            cbo_Rating.SelectedItem.Text = ""
            txt_RatingDate.Text = ""
            FillRatingDetails()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillRatingDetails()
        Try
            Dim dt As DataTable
            If TypeOf Session("SecurityRatingTable") Is DataTable Then
                dt = Session("SecurityRatingTable")
            Else
                dt = objCommon.FillDetailsGrid(dg_Rating, "ID_FILL_SecurityRatingDetails", "SecurityId", Val((ViewState("Id")) & ""))
            End If
            Session("SecurityRatingTable") = dt

            dg_Rating.DataSource = dt
            dg_Rating.DataBind()
        Catch ex As Exception

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Rating_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Rating.ItemCommand
        Try

            Dim dt As DataTable
            dt = CType(Session("SecurityRatingTable"), DataTable)
            If e.CommandName = "Edit" Then

                'If (dt.Rows(e.Item.ItemIndex).Item("RatingDate") & "") <> "" Then
                '    txt_RatingDate.Text = dt.Rows(e.Item.ItemIndex).Item("RatingDate")
                'End If

                'cbo_RatingOrg.SelectedValue = dt.Rows(e.Item.ItemIndex).Item("RatingOrganizationId")
                'cbo_Rating.SelectedValue = dt.Rows(e.Item.ItemIndex).Item("RatingId")

            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("SecurityRatingTable") = dt
            FillRatingDetails()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function SaveRatings(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            'Hid_ContactDetailId.Value = ""
            dt = Session("SecurityRatingTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_SecurityRatingDetails"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("OrganizationName").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@RatingId", SqlDbType.Int, 4, "I", , , Val(dt.Rows(I).Item("RatingId")))
                    objCommon.SetCommandParameters(sqlComm, "@RatingOrganizationId", SqlDbType.Int, 4, "I", , , Val(dt.Rows(I).Item("RatingOrganizationId")))
                    objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "I", , , Val(ViewState("Id")))
                    If dt.Rows(I).Item("RatingDate") & "" <> "" Then
                        objCommon.SetCommandParameters(sqlComm, "@RatingDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(dt.Rows(I).Item("RatingDate")))
                    End If

                    objCommon.SetCommandParameters(sqlComm, "@SecurityRatingId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                    Hid_RatingDetailId.Value += Val(sqlComm.Parameters.Item("@SecurityRatingId").Value & "") & "!"

                Else
                    Hid_RatingDetailId.Value += "!"
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function CalculateCashflow(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "WDM_Calculate_SecurityIPDates"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
End Class
