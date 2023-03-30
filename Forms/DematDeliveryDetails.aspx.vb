Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Partial Class Forms_DematDeliveryDetails
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlComm As New SqlCommand
    Dim strValues() As String
    Dim sqlConn As SqlConnection
    Dim PgName As String = "$DematDeliveryDetails$"
    Dim objUtil As New Util
  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'objCommon.OpenConn()

            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            Dim filename As String = Me.Page.ToString().Substring(10, Me.Page.ToString().Substring(10).Length - 5) & ".aspx"
            Dim FaceVal As Integer
            If IsPostBack = False Then
                btn_Update.Visible = False
                SetAttributes()
                If (Request.QueryString("DealSlipId") & "") <> "" Then
                    Hid_DealSlipId.Value = Val(Request.QueryString("DealSlipId") & "")
                End If
                If (Request.QueryString("DematAccTo") & "") <> "" Then
                    Hid_DematAccTo.Value = Val(Request.QueryString("DematAccTo") & "")
                End If
                If (Request.QueryString("PayMode") & "") <> "" Then
                    Hid_PayMode.Value = (Request.QueryString("PayMode") & "")
                    If (Request.QueryString("PayMode") & "") = "B" Then
                        row_SettleNo.Visible = True
                    End If
                End If
                Hid_BalAmt.Value = Val(Request.QueryString("BalAmt") & "")
                If Trim(Request.QueryString("Values") & "") <> "" Then
                    strValues = Split(Trim(Request.QueryString("Values") & ""), "!")
                    'txt_FaceVal.Text = strValues(0)
                    FaceVal = strValues(0)
                    txt_NSDLFaceValue.Text = strValues(1)
                    txt_DmatSlipNo.Text = strValues(2)
                    txt_ChqNo.Text = Session("ClientName")
                    txt_Qty.Text = strValues(7)
                    Hid_bond.Value = txt_Qty.Text
                    txt_DelDate.Text = strValues(8)
                    txt_CustSlipNo.Text = strValues(12)
                    cbo_FaceVal.SelectedValue = strValues(14)
                    'cbo_FaceVal.SelectedValue = Val(Request.QueryString("FaceMultiple") & "")
                    cbo_DematAccTo.SelectedValue = strValues(9)
                    FaceVal = strValues(0)
                    If FaceVal > 1000 And FaceVal < 100000 Then
                        cbo_FaceVal.SelectedValue = 1000
                    ElseIf FaceVal > 100000 And FaceVal < 10000000 Then
                        cbo_FaceVal.SelectedValue = 100000
                    Else
                        cbo_FaceVal.SelectedValue = 10000000
                    End If
                    txt_FaceVal.Text = FaceVal / cbo_FaceVal.SelectedValue

                    'cbo_DPName.SelectedValue = strValues(7)
                    'txt_DPId.Text = strValues(5)
                    'txt_ClientId.Text = strValues(6)
                    'cbo_CustDPName.SelectedValue = strValues(13)  
                    If strValues(11) = "" Then
                        strValues(11) = 0
                    End If
                    If strValues(13) <> "" Then
                        FillDematDetails(strValues(13), strValues(11))
                    End If
                    If (Request.QueryString("PayMode") & "") = "B" Then
                        txt_SettleNo.Text = strValues(14)
                    End If
                    txt_Remark.Text = strValues(15)
                Else
                    FillFields()
                    txt_CustSlipNo.Text = ""
                    txt_DmatSlipNo.Text = ""

                End If
                If (Request.QueryString("url") & "") = "DeleteDMATDelivery.aspx" Then
                    btn_Save.Visible = False
                End If

            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
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

    Private Sub SetAttributes()
        ' btn_Save.Attributes.Add("onclick", "return Validation();")
        cbo_FaceVal.Attributes.Add("onchange", "TotalNoOfBond();")
        txt_FaceVal.Attributes.Add("onchange", "TotalNoOfBond();")
        txt_DelDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_DelDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_DelDate.Text = Format(Now, "dd/MM/yyyy")

    End Sub

    Private Sub FillFields()
        Try
            Dim dt As DataTable
            Dim dr As DataRow
            Dim balamt As Integer
            OpenConn()

            If Trim(Request.QueryString("Index") & "") = "" Then
                dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DematDelivery", Val(Hid_DealSlipId.Value), "DealSlipID")
                dr = dt.Rows(0)
            Else
                dt = TryCast(Session("DematInfoTable"), DataTable)
                dr = dt.Rows(Val(Request.QueryString("index") & ""))
            End If
            'dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DematDelivery", Val(Hid_DealSlipId.Value), "DealSlipID")
            If dt.Rows.Count > 0 Then
                txt_NSDLFaceValue.Text = Trim(dt.Rows(0).Item("NSDLFaceValue") & "")


                If Val(Request.QueryString("FaceMultiple") & "") <> 0 Then

                End If
                If Val(Request.QueryString("facevalue") & "") <> 0 Then

                End If
                If Val(Request.QueryString("BalAmt") & "") <> 0 Then
                    balamt = Val(Request.QueryString("BalAmt") & "")
                    If balamt > 1000 And balamt < 100000 Then
                        cbo_FaceVal.SelectedValue = 1000
                    ElseIf balamt > 100000 And balamt < 10000000 Then
                        cbo_FaceVal.SelectedValue = 100000
                    Else
                        cbo_FaceVal.SelectedValue = 10000000
                    End If
                    Dim intBalAmt As Decimal
                    intBalAmt = (Request.QueryString("BalAmt") & "")
                    txt_FaceVal.Text = (intBalAmt / cbo_FaceVal.SelectedValue)
                    'txt_FaceVal.Text = Val(Request.QueryString("BalAmt") & "") / cbo_FaceVal.SelectedValue
                    'cbo_FaceVal.SelectedValue = Val(Request.QueryString("FaceMultiple") & "")
                    Hid_FaceValue.Value = Val(txt_FaceVal.Text) * cbo_FaceVal.SelectedValue
                    txt_Qty.Text = Val(Hid_FaceValue.Value) / Val(txt_NSDLFaceValue.Text)
                    Hid_bond.Value = Val(txt_Qty.Text)
                Else
                    txt_FaceVal.Text = Val(dt.Rows(0).Item("FV") & "")
                    cbo_FaceVal.SelectedValue = Val(dt.Rows(0).Item("FaceValueMultiple") & "")
                    Hid_FaceValue.Value = Val(dt.Rows(0).Item("FaceValue") & "")
                    txt_Qty.Text = Val(dt.Rows(0).Item("NoOfBond") & "")
                    Hid_bond.Value = Val(txt_Qty.Text)
                End If


                If Trim(Request.QueryString("Index") & "") = "" Then
                    'txt_Qty.Text = Val(dt.Rows(0).Item("NoOfBond") & "")
                Else
                    'txt_Qty.Text = Val(dt.Rows(0).Item("Quantity") & "")
                    txt_CustSlipNo.Text = Trim(dt.Rows(0).Item("CustomerSlipNumber") & "")
                    txt_DmatSlipNo.Text = Trim(dt.Rows(0).Item("DMATslipNo") & "")
                End If
                Hid_CustomerId.Value = Val(dt.Rows(0).Item("CustomerID") & "")
                txt_ChqNo.Text = Trim(dt.Rows(0).Item("CustomerName").ToString)
                FillDematDetails(Val(dt.Rows(0).Item("DMATId") & ""), Val(dt.Rows(0).Item("CustDPId") & ""))
            End If


        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillFields", "Error in FillFields", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillDematDetails(ByVal intDematId As Int32, ByVal intCustDematId As Int32)
        Try
            Dim dtDmat As DataTable
            Dim dtCustName As DataTable
            Dim dtINDemat As DataTable
            OpenConn()
            objCommon.FillControl(cbo_DPName, sqlConn, "ID_FILL_DMATMaster", "DPName", "DMatId", Val(Session("CompId")), "CompId")
            objCommon.FillControl(cbo_CustDPName, sqlConn, "ID_FILL_CustdPDetails", "DpName", "CustDPId", Val(Hid_CustomerId.Value), "CustomerId")

            dtDmat = objCommon.FillDataTable(sqlConn, "ID_FILL_DMATMaster", intDematId, "DMatId")
            If dtDmat.Rows.Count > 0 Then
                cbo_DPName.SelectedValue = intDematId
                txt_DPId.Text = Trim(dtDmat.Rows(0).Item("DPId") & "")
                txt_ClientId.Text = Trim(dtDmat.Rows(0).Item("ClientId") & "")
                Hid_DmatId.Value = Val(dtDmat.Rows(0).Item("DMatId") & "")
            End If
            dtCustName = objCommon.FillDataTable(sqlConn, "ID_FILL_CustdPDetails", intCustDematId, "CustDPId")
            If dtCustName.Rows.Count > 0 Then
                cbo_CustDPName.SelectedValue = intCustDematId
                txt_CustDPId.Text = Trim(dtCustName.Rows(0).Item("DPId") & "")
                txt_CustClientId.Text = Trim(dtCustName.Rows(0).Item("ClientId") & "")
                Hid_CustDpId.Value = Trim(dtCustName.Rows(0).Item("CustDPId") & "")
            End If

            If (Request.QueryString("PayMode") & "") <> "" Then
                dtINDemat = objCommon.FillDataTable(sqlConn, "ID_FILL_DMATMaster", Val(Hid_DmatId.Value), "DMatId")
                If dtINDemat.Rows.Count > 0 Then
                    txt_DPId.Text = Trim(dtINDemat.Rows(0).Item("DPId") & "")
                    txt_ClientId.Text = Trim(dtINDemat.Rows(0).Item("ClientId") & "")
                    Hid_DmatId.Value = cbo_DPName.SelectedValue
                End If
                
            End If


        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillDematDetails", "Error in FillDematDetails", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
   

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "close", "RetValues();", True)

    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            'sqlConn.Dispose()
        Catch ex As Exception

        End Try

    End Sub
End Class
