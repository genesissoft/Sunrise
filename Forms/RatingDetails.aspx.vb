Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_RatingDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Val(Session("UserId") & "") = 0 Then
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.AddHeader("Cache-Control", "no-cache")
        Response.AddHeader("Cache-Control", "no-store")
        Try
            Dim strValues() As String

            If (IsPostBack = False) Then
                SetAttributes()
                If (Request.QueryString("CustomerId") & "") <> "" Then
                    Hid_CustomerId.Value = Val(Request.QueryString("CustomerId").ToString())
                End If
                If Trim(Request.QueryString("CustomerId") & "") <> "" Then

                    Hid_CustomerId.Value = Val(Request.QueryString("CustomerId").ToString())
                    strValues = Split(Trim(Request.QueryString("Values") & ""), "!")
                    cbo_InstrumentType.SelectedValue = strValues(0)
                    txt_Rating1.Text = Request.QueryString("Rating1").ToString()
                    txt_Rating2.Text = Request.QueryString("Rating2").ToString()
                    txt_Rating3.Text = Request.QueryString("Rating3").ToString()
                    txt_Rating4.Text = Request.QueryString("Rating4").ToString()

                    txt_Agency1.Text = Request.QueryString("Agency1").ToString()
                    txt_Agency2.Text = Request.QueryString("Agency2").ToString()
                    txt_Agency3.Text = Request.QueryString("Agency3").ToString()
                    txt_Agency4.Text = Request.QueryString("Agency4").ToString()


                    btn_Cancel.Visible = True
                    btn_Update.Visible = False
                End If
                btn_Update.Visible = False

            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub SetAttributes()
        'btn_Save.Attributes.Add("onclick", "return Validation();")
        'txt_RATINGUPTO.Attributes.Add("onblur", "ConvertUCase(this);")
    End Sub


    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "close", "RetValues();", True)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        btn_Save_Click(sender, e)
    End Sub

End Class
