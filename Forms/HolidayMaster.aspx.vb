Imports System.Data
Imports System.Data.SqlClient

Partial Class Forms_HolidayMaster
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            Page.Form.DefaultButton = btn_Save.UniqueID


            If IsPostBack = False Then
                'CHANGE
                Hid_CompId.Value = Val(Session("CompId"))
                Hid_YearText.Value = Session("YearText")
                Hid_Year.value = Left(Session("YearText"), 4)
                FillYear()

                SetAttributes()
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("Id") & "")
                    FillFields()
                    btn_Save.Visible = False
                    'btn_Update.Visible = True
                    'Page.Form.DefaultButton = btn_Update.UniqueID
                Else
                    btn_Save.Visible = True
                    'btn_Update.Visible = False
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillYear()
        Try

            lit_April.Text = Left(Session("YearText"), 4)
            lit_May.Text = Left(Session("YearText"), 4)
            lit_June.Text = Left(Session("YearText"), 4)
            lit_July.Text = Left(Session("YearText"), 4)
            lit_Aug.Text = Left(Session("YearText"), 4)
            lit_Sept.Text = Left(Session("YearText"), 4)
            lit_Oct.Text = Left(Session("YearText"), 4)
            lit_Nov.Text = Left(Session("YearText"), 4)
            lit_Dec.Text = Left(Session("YearText"), 4)

            lit_Jan.Text = Right(Session("YearText"), 4)
            lit_feb.Text = Right(Session("YearText"), 4)
            lit_mar.Text = Right(Session("YearText"), 4)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetAttributes()

        txt_AprilDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_AprilDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_Maydate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Maydate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_Junedate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Junedate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_JulyDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_JulyDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_Augdate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Augdate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_Septdate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Septdate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_Octdate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Octdate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_NovDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_NovDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_Decdate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Decdate.Attributes.Add("onblur", "CheckDate(this,false);")

        'btn_Save.Attributes.Add("onclick", "return Validation();")
        'btn_Update.Attributes.Add("onclick", "return Validation();")
    End Sub
    Private Sub FillFields()
        'CHANGE 
        Dim dt As DataTable
        dt = objCommon.FillDataTable(clsCommonFuns.sqlConn, "ID_FILL_BankMaster", ViewState("Id"), "BankId")
        If dt.Rows.Count > 0 Then
            'txt_AccountNumber.Text = Trim(dt.Rows(0).Item("AccountNumber") & "")
            'txt_Bankalis.Text = Trim(dt.Rows(0).Item("Bankalis") & "")
            'txt_BankName.Text = Trim(dt.Rows(0).Item("BankName") & "")
            'txt_Branch.Text = Trim(dt.Rows(0).Item("Branch") & "")
            'txt_ContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
            'txt_FaxNo.Text = Trim(dt.Rows(0).Item("FaxNo") & "")
            'txt_PhoneNo.Text = Trim(dt.Rows(0).Item("PhoneNo") & "")
            'txt_RTGScode.Text = Trim(dt.Rows(0).Item("RTGScode") & "")
            'objCommon.FillSelectedListBox(list_Aprildate, "HolidayDate", ViewState("HolidayId"))
        End If
    End Sub
   
    Protected Sub btn_AddAprilDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddAprilDate.Click
        Try
            objCommon.FillListHoliday(list_Aprildate, txt_AprilDate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddAugdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddAugdate.Click
        Try
            objCommon.FillListHoliday(lst_augdate, txt_Augdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddDecdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddDecdate.Click
        Try
            objCommon.FillListHoliday(lst_decdate, txt_Decdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddFebdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddFebdate.Click
        Try
            objCommon.FillListHoliday(list_Febdate, txt_Febdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddJanDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddJanDate.Click
        Try
            objCommon.FillListHoliday(List_JanDate, txt_JanDate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddJulydate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddJulydate.Click
        Try
            objCommon.FillListHoliday(lst_Julydate, txt_JulyDate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddJunedate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddJunedate.Click
        Try
            objCommon.FillListHoliday(list_Junedate, txt_Junedate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddMarchdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddMarchdate.Click
        Try
            objCommon.FillListHoliday(lst_Marchdate, txt_marchdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddMayDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddMayDate.Click
        Try
            objCommon.FillListHoliday(lst_Maydate, txt_Maydate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddNovdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddNovdate.Click
        Try
            objCommon.FillListHoliday(lst_Novdate, txt_NovDate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddOctdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddOctdate.Click
        Try
            objCommon.FillListHoliday(lst_Octdate, txt_Octdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_AddSeptdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddSeptdate.Click
        Try
            objCommon.FillListHoliday(lst_septdate, txt_Septdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_Removeaprildate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Removeaprildate.Click
        Try
            objCommon.RemoveRecord(list_Aprildate, txt_AprilDate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_Removeaugdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Removeaugdate.Click
        Try
            objCommon.RemoveRecord(lst_augdate, txt_Augdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_removedecdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_removedecdate.Click
        Try
            objCommon.RemoveRecord(lst_decdate, txt_Decdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_RemoveFebDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_RemoveFebDate.Click
        Try
            objCommon.RemoveRecord(list_Febdate, txt_Febdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_RemoveJanDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_RemoveJanDate.Click
        Try
            objCommon.RemoveRecord(List_JanDate, txt_JanDate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_removejulydate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_removejulydate.Click
        Try
            objCommon.RemoveRecord(lst_Julydate, txt_JulyDate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_RemoveJunedate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_RemoveJunedate.Click
        Try
            objCommon.RemoveRecord(list_Junedate, txt_Junedate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_removeMarchdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_removeMarchdate.Click
        Try
            objCommon.RemoveRecord(lst_Marchdate, txt_marchdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_RemoveMaydate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_RemoveMaydate.Click
        Try
            objCommon.RemoveRecord(lst_Maydate, txt_Maydate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_removenovdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_removenovdate.Click
        Try
            objCommon.RemoveRecord(lst_Novdate, txt_NovDate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_Removeoctdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Removeoctdate.Click
        Try
            objCommon.RemoveRecord(lst_Octdate, txt_Octdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_removeseptdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_removeseptdate.Click
        Try
            objCommon.RemoveRecord(lst_septdate, txt_Septdate)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click

        SetSaveUpdate("ID_INSERT_HolidayMaster")
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            sqlTrans = clsCommonFuns.sqlConn.BeginTransaction
            If SaveHoliday(sqlTrans, strProc) = False Then Exit Sub
           

            sqlTrans.Commit()
            'Response.Redirect("BankDetail.aspx?Id=" & ViewState("Id"), False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Public Function SaveHoliday(ByRef sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim strdate As Date
            Dim I As Int16
            Dim lstbox As ListBox
            Dim LstItem As ListItem
            Dim arrdate() As ListBox = {list_Aprildate, lst_Maydate, list_Junedate, lst_Julydate, lst_augdate, lst_septdate, lst_Octdate, lst_Novdate, lst_decdate, List_JanDate, list_Febdate, lst_Marchdate}
            sqlComm.Connection = clsCommonFuns.sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandText = "ID_INSERT_HolidayMaster"
            For j As Integer = 0 To arrdate.Length - 1
                lstbox = arrdate(j)
                For I = 0 To lstbox.Items.Count - 1
                    LstItem = lstbox.Items(I)
                    strdate = Trim(LstItem.Value)
                    sqlComm.Parameters.Clear()
                    With sqlComm
                        objCommon.SetCommandParameters(sqlComm, "@HolidayDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(strdate))
                        objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , (Session("YearId")))
                        objCommon.SetCommandParameters(sqlComm, "@SatSunflag", SqlDbType.Bit, 4, "I", , , 0)
                        objCommon.SetCommandParameters(sqlComm, "@HolidayId", SqlDbType.BigInt, 8, "O")
                        objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")

                    End With
                    sqlComm.ExecuteNonQuery()
                    Hid_HolidayId.Value = Val(sqlComm.Parameters.Item("@HolidayId").Value & "")
                Next
            Next

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Response.Write(ex.Message)
            Return False
        End Try
    End Function

    

    
End Class
