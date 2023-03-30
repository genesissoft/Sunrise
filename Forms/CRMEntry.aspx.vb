Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Web
'Imports System.Linq
Imports System.IO
Imports log4net
'Imports System.Web.Routing

Partial Class Forms_CRMEntry
    Inherits System.Web.UI.Page
    Public ObjClass As New ClsCommonCRM
    Public objcommon As New clsCommonFuns
    Dim sqlConn As SqlConnection
    Dim InteractionId As Int64
    Dim strPath As String = ConfigurationManager.AppSettings("CRMPath").ToString()
    Dim PgName As String = "$CRMEntry$"
    Dim objUtil As New Util
    Dim SessionTimeOut As Exception

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Session("UserName") = "" Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If

            strPath = strPath & Convert.ToString(Session("UserName")) & "\"
            If Not Directory.Exists(strPath) Then
                Directory.CreateDirectory(Path.GetDirectoryName(strPath))
            End If
            Page.Form.Attributes.Add("enctype", "multipart/form-data")

            If Page.IsPostBack = False Then
                SetAttrbutes()
                FillCombo()
                'FillInteraction()

                InteractionId = Request.QueryString("Id")
                Hid_InteractionId.Value = InteractionId

                If (Val(Hid_InteractionId.Value) > 0) Then
                    Hid_PageCnt.Value = Convert.ToString(Request.QueryString("Pagecnt"))
                    FillFields()
                End If
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
            Response.Write(ex.Message)
        End Try

    End Sub

    Private Sub SetAttrbutes()
        btn_Save.Attributes.Add("onclick", "return Validation();")
        txt_EntryDate.Text = Date.Now.ToString("dd/MM/yyyy")
        txt_ContactPerson.Attributes.Add("onblur", "return Hide(this);")
    End Sub

    Private Sub FillCombo()
        Dim lstParam As New List(Of SqlParameter)()
        Dim ds As New DataSet()
        Try
            lstParam.Add(New SqlParameter("@UserId", Val(Session("UserId"))))
            ds = objComm.FillDetails(lstParam, "CRM_Fill_ClientInteractionCombo")

            cbo_City.DataValueField = "Id"
            cbo_City.DataTextField = "Name"
            cbo_City.DataSource = ds.Tables(0)
            cbo_City.DataBind()

            cbo_Purpose.DataValueField = "Id"
            cbo_Purpose.DataTextField = "Name"
            cbo_Purpose.DataSource = ds.Tables(1)
            cbo_Purpose.DataBind()

            cbo_ModeOfCnct.DataValueField = "Id"
            cbo_ModeOfCnct.DataTextField = "Name"
            cbo_ModeOfCnct.DataSource = ds.Tables(2)
            cbo_ModeOfCnct.DataBind()

            Lst_VerticalL.DataValueField = "Id"
            Lst_VerticalL.DataTextField = "Name"
            Lst_VerticalL.DataSource = ds.Tables(3)
            Lst_VerticalL.DataBind()

            Lst_AccompaniedbyL.DataValueField = "Id"
            Lst_AccompaniedbyL.DataTextField = "Name"
            Lst_AccompaniedbyL.DataSource = ds.Tables(4)
            Lst_AccompaniedbyL.DataBind()

            Lst_CustExpec.DataValueField = "Id"
            Lst_CustExpec.DataTextField = "Name"
            Lst_CustExpec.DataSource = ds.Tables(5)
            Lst_CustExpec.DataBind()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillFields()
        Dim lstParam As New List(Of SqlParameter)()
        Dim ds As New DataSet()

        Try
            Dim dt, DtAccompaniedBy, DtVertical As New DataTable
            Dim objClass As New ClsCommonCRM
            Dim dd1 As New DropDownList
            Dim Strdata As String = String.Empty
            Dim ProcName As String = String.Empty

            lstParam.Add(New SqlParameter("@InteractionId", Val(Hid_InteractionId.Value)))
            lstParam.Add(New SqlParameter("@UserId", Val(Session("UserId"))))
            ds = objComm.FillDetails(lstParam, "CRM_Fill_ClientInteractionDetails")

            dt = ds.Tables(0)

            cbo_InvContact.DataValueField = "Id"
            cbo_InvContact.DataTextField = "Name"
            cbo_InvContact.DataSource = ds.Tables(1)
            cbo_InvContact.DataBind()

            Lst_VerticalR.DataValueField = "Id"
            Lst_VerticalR.DataTextField = "Name"
            Lst_VerticalR.DataSource = ds.Tables(2)
            Lst_VerticalR.DataBind()

            Lst_AccompaniedbyR.DataValueField = "Id"
            Lst_AccompaniedbyR.DataTextField = "Name"
            Lst_AccompaniedbyR.DataSource = ds.Tables(3)
            Lst_AccompaniedbyR.DataBind()

            Lst_CustExpecR.DataValueField = "Id"
            Lst_CustExpecR.DataTextField = "Name"
            Lst_CustExpecR.DataSource = ds.Tables(4)
            Lst_CustExpecR.DataBind()

            If dt.Rows.Count > 0 Then
                Hid_Id.Value = Trim(dt.Rows(0).Item("ClientId") & "")
                txt_CustName.Text = Trim(dt.Rows(0).Item("CustomerName") & "")
                txt_EntryDate.Text = Trim(dt.Rows(0).Item("EntryDate") & "")
                cbo_City.SelectedValue = Val(dt.Rows(0).Item("CityId") & "")
                cbo_Purpose.SelectedValue = Val(dt.Rows(0).Item("PurposeId") & "")
                cbo_Station.SelectedValue = Trim(dt.Rows(0).Item("Station") & "")
                cbo_InvContact.SelectedValue = Trim(dt.Rows(0).Item("ContactDetailId") & "")

                cbo_ModeOfCnct.SelectedValue = Val(dt.Rows(0).Item("ModeOfCnctId") & "")
                txt_Remark.Text = Trim(dt.Rows(0).Item("Remark") & "")
                txt_Opportunity.Text = Trim(dt.Rows(0).Item("Opportunity") & "")
                txt_Parameters.Text = Trim(dt.Rows(0).Item("Parameters") & "")
                txt_TopicDiscussed.Text = Trim(dt.Rows(0).Item("TopicDiscussed") & "")
                Cbo_Status.SelectedValue = Trim(dt.Rows(0).Item("Status") & "")
                If Convert.ToString(dt.Rows(0).Item("FileName")) <> "" Then
                    tr_File.Style.Value = "display: '';"
                    txt_file.Text = Convert.ToString(dt.Rows(0).Item("FileName"))
                End If

                If Convert.ToString(dt.Rows(0).Item("CustBusinessType") & "") = "PF" Then
                    row_AdvComment.Style.Value = "display: '';"
                    row_AdvStatus.Style.Value = "display: '';"
                    txt_AdvComment.Text = Convert.ToString(dt.Rows(0).Item("AdvisoryComment"))
                    txt_Advstatus.Text = Convert.ToString(dt.Rows(0).Item("AdvisoryStatus"))
                Else
                    row_AdvComment.Style.Value = "display: none;"
                    row_AdvStatus.Style.Value = "display: none;"
                    txt_AdvComment.Text = ""
                    txt_Advstatus.Text = ""
                End If

                If Trim(dt.Rows(0).Item("NameOfUser") & "").ToLower() <> Session("NameOfUser").ToString().ToLower() Then
                    btn_Save.Visible = False
                Else
                    btn_Save.Text = "UPDATE"
                End If

                ViewState("FileName") = txt_file.Text
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillFields", "Error in FillFields", "", ex)

            Response.Write(ex.Message)
        End Try
    End Sub

    <WebMethod(EnableSession:=True)> _
    Public Shared Function BindContacts(ByVal Id As String) As String
        Try
            Dim lstParam As New List(Of SqlParameter)()
            Dim strData As String = ""
            Dim ds As New DataSet

            lstParam.Add(New SqlParameter("@CustomerId", Val(Id)))
            ds = objComm.FillDetails(lstParam, "CRM_Fill_CRMEntryContactDetails")

            If ds.Tables(0).Rows.Count > 0 Then
                strData = Trim(ds.Tables(0).Rows(0).Item("Data") & "")
            End If

            Return strData

        Catch ex As Exception

        End Try

    End Function

 

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            SetSaveUpdate("CRM_INSERT_ClientInteraction")
            ClearControls()
            Response.Redirect("CRMInteractionDetails.aspx?InteractionId=" + Convert.ToString(ViewState("InteractionId")) + "&Pagecnt=" + Convert.ToString(Hid_PageCnt.Value), False)
            ViewState("InteractionId") = Nothing
            Hid_PageCnt.Value = ""
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Save_Click", "Error in btn_Save_Click", "", ex)

            Response.Write(ex.Message)
        End Try
    End Sub

#Region "Save Investor/Issuer/Customer!"
    Private Sub SetSaveUpdate(ByVal strProc As String)

        Try
            OpenConn()
            FileUpload()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction

            If Hid_LstbxTC.Value <> "" Then
                Hid_LstbxTC.Value = Hid_LstbxTC.Value.Remove(Hid_LstbxTC.Value.Length - 1)
            End If

            If Hid_LstbxPC.Value <> "" Then
                Hid_LstbxPC.Value = Hid_LstbxPC.Value.Remove(Hid_LstbxPC.Value.Length - 1)
            End If

            If SaveInvInteraction(sqlTrans, strProc) = False Then Exit Sub
            sqlTrans.Commit()

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetSaveUpdate", "Error in SetSaveUpdate", "", ex)

            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Sub
#End Region

#Region "File Upload!"
    Private Function FileUpload()
        If Not (fileUpld.PostedFile Is Nothing) Then


            Dim FileType As String = System.IO.Path.GetExtension(fileUpld.PostedFile.FileName)
            FileType = FileType.ToLower()
            Try

                If FileType = ".pdf" Or FileType = ".doc" Or FileType = ".docx" Or FileType = ".xls" Or FileType = ".xlsx" Or FileType = ".txt" Then
                    ' 10  MB size    
                    If fileUpld.PostedFile.ContentLength < 10485760 Then
                        Dim filename As String = System.IO.Path.GetFileName(fileUpld.PostedFile.FileName)
                        Dim strDate As String = ""
                        Dim time As DateTime = DateTime.Now
                        Dim format As String = "ddMMyyyy HH mm"
                        strDate = time.ToString(format)
                        Dim strNewFile() As String
                        strNewFile = filename.Split(".")
                        filename = strNewFile(0) & "_" & strDate & "." & strNewFile(1)
                        fileUpld.PostedFile.SaveAs(strPath + filename)
                        ViewState("FileName") = filename
                    Else
                        Dim msg As String = "Cant upload file,file Size greater than Specified Size"
                        Dim strHtml As String
                        strHtml = "alert('" + msg + "');"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "msg", strHtml, True)
                        Return True
                    End If
                Else
                    Dim msg As String = "Cant upload file,file Size greater than Specified Size"
                    Dim strHtml As String
                    strHtml = "alert('" + msg + "');"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "msg", strHtml, True)
                    Return True
                End If
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
        End If
    End Function
#End Region

    Private Sub ClearControls()
        lst_InvContact.Items.Clear()
        ' txt_Investor.Text = ""
        txt_Remark.Text = ""
        Hid_LstbxPC.Value = ""
        Hid_LstbxTC.Value = ""
        Hid_Id.Value = ""
        ViewState("FileName") = Nothing
        Hid_SelectionId.Value = ""
    End Sub

    Private Function SaveInvInteraction(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Dim sqlComm As New SqlCommand
        Try
            If Val(Hid_InteractionId.Value) = 0 Then
                sqlComm.Parameters.Add("@InteractionId", SqlDbType.BigInt, 8).Value = DBNull.Value
                sqlComm.Parameters("@InteractionId").Direction = ParameterDirection.Output
            Else
                sqlComm.Parameters.Add("@InteractionId", SqlDbType.BigInt, 8).Value = Val(Hid_InteractionId.Value)
            End If
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            If txt_EntryDate.Text.ToString() <> "" Then
                sqlComm.Parameters.Add("@EntryDate", SqlDbType.SmallDateTime, 4).Value = objcommon.DateFormat(txt_EntryDate.Text.ToString())
            Else
                sqlComm.Parameters.Add("@EntryDate", SqlDbType.SmallDateTime, 4).Value = DBNull.Value
            End If

            sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Val(Session("UserId"))
            If Val(Hid_InvContact.Value) > 0 Then
                sqlComm.Parameters.Add("@ContactDetailId", SqlDbType.Int, 4).Value = Val(Hid_InvContact.Value)
            End If

            If Val(cbo_ModeOfCnct.SelectedValue) <> 0 Then
                sqlComm.Parameters.Add("@ModeOfCnctId", SqlDbType.Int, 4).Value = Val(cbo_ModeOfCnct.SelectedValue)
            End If
            If Trim(Hid_LstCustExpecR.Value) <> "" Then
                sqlComm.Parameters.Add("@ExpectationIds", SqlDbType.VarChar, 200).Value = Trim(Hid_LstCustExpecR.Value)
            End If
            If Trim(Hid_LstAccompaniedbyR.Value) <> "" Then
                sqlComm.Parameters.Add("@AccompaniedByIds", SqlDbType.VarChar, 200).Value = Trim(Hid_LstAccompaniedbyR.Value)
            End If

            sqlComm.Parameters.Add("@Remark", SqlDbType.VarChar, 8000).Value = Trim(txt_Remark.Text)
            sqlComm.Parameters.Add("@TopicDiscussed", SqlDbType.VarChar, 8000).Value = Trim(txt_TopicDiscussed.Text)
            sqlComm.Parameters.Add("@Parameters", SqlDbType.VarChar, 8000).Value = Trim(txt_Parameters.Text)
            sqlComm.Parameters.Add("@Opportunity", SqlDbType.VarChar, 8000).Value = Trim(txt_Opportunity.Text)
            If Convert.ToString(ViewState("FileName")) <> "" Then
                sqlComm.Parameters.Add("@FileName", SqlDbType.VarChar, 200).Value = ViewState("FileName")
            End If
            If Convert.ToString(Cbo_Status.SelectedValue) <> "" Then
                sqlComm.Parameters.Add("@Status", SqlDbType.Char, 1).Value = Convert.ToString(Cbo_Status.SelectedValue)
            End If

            If Val(Hid_Id.Value) <> 0 Then
                sqlComm.Parameters.Add("@ClientId", SqlDbType.Int, 4).Value = Val(Hid_Id.Value)
            Else
                sqlComm.Parameters.Add("@ClientId", SqlDbType.Int, 4).Value = DBNull.Value
            End If

            sqlComm.Parameters.Add("@TempClient", SqlDbType.VarChar, 500).Value = Trim(txt_CustName.Text)
            sqlComm.Parameters.Add("@AddTemporary", SqlDbType.Char, 1).Value = "N"

            sqlComm.Parameters.Add("@AdvisoryComment", SqlDbType.VarChar, 8000).Value = Trim(txt_AdvComment.Text)
            sqlComm.Parameters.Add("@AdvisoryStatus", SqlDbType.VarChar, 8000).Value = Trim(txt_Advstatus.Text)
            If Hid_CustBusinessType.Value = "PF" Then
                sqlComm.Parameters.Add("@CustBusinessType", SqlDbType.VarChar, 10).Value = Hid_CustBusinessType.Value
            Else
                sqlComm.Parameters.Add("@CustBusinessType", SqlDbType.VarChar, 10).Value = DBNull.Value
            End If
            sqlComm.Parameters.Add("@CityId", SqlDbType.Int, 4).Value = Val(cbo_City.SelectedValue)
            sqlComm.Parameters.Add("@PurposeId", SqlDbType.Int, 4).Value = Val(cbo_Purpose.SelectedValue)
            If Trim(Hid_LstVerticalR.Value) <> "" Then
                sqlComm.Parameters.Add("@VerticalPurposeIds", SqlDbType.VarChar, 200).Value = Trim(Hid_LstVerticalR.Value)
            End If
            sqlComm.Parameters.Add("@Station", SqlDbType.VarChar, 50).Value = Trim(cbo_Station.SelectedValue)
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4)
            sqlComm.Parameters("@RET_CODE").Direction = ParameterDirection.Output
            sqlComm.ExecuteNonQuery()
            ViewState("InteractionId") = sqlComm.Parameters("@InteractionId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveInvInteraction", "Error in SaveInvInteraction", "", ex)

            Response.Write(ex.Message)
        Finally
            sqlComm.Dispose()
        End Try
    End Function

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

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        CloseConn()
        If sqlConn IsNot Nothing Then
            sqlConn.Dispose()
            System.GC.Collect()
        End If
    End Sub

    Protected Sub Btn_Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Delete.Click
        Try
            If ViewState("FileName") <> "" Then
                Dim f As System.IO.FileInfo = New System.IO.FileInfo(strPath + ViewState("FileName"))
                If f.Exists = True Then
                    f.Delete()
                End If
                ViewState("FileName") = ""
                txt_file.Text = ""
                tr_File.Style.Value = "display: none;"
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Btn_Delete_Click", "Error in Btn_Delete_Click", "", ex)

            Throw ex
        End Try

    End Sub

End Class