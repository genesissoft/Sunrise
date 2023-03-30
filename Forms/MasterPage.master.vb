Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports Microsoft.Web.UI

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Dim oSqlCmd As SqlCommand
    Dim obj As New clsCommonFuns
    Dim objCommon As New clsCommonFuns
    Dim strHtml As String
    Dim Hid_Userid As New HiddenField
    Dim Hid_TodayDate As New HiddenField
    Dim sqlConn As New SqlConnection



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ScriptManager1.RegisterAsyncPostBackControl(DivEvents)
        Dim strConn As String = ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString()
        lit_CurrentDate.Text = DateTime.Now.ToString("D")
        ShowMessage()

        If Trim(Session("NameOfuser") & "") <> "" And Trim(Session("CompName") & "") <> "" And Trim(Session("YearText") & "") <> "" Then
            lbl_user.Text = "Welcome " & Trim(Session("NameOfuser") & "")
            CompanyYear.Text = "In " & Trim(Session("CompName") & "") + " - " + Trim(Session("YearText") & "")
        Else
            CompanyYear.Text = "In " & Trim(Session("NameOfuser") & "")
        End If
        If Trim(Request.QueryString("Flag") & "") = "C" Then
            If Session("PageNameCheckerView") = "" Then

                Dim btnUpdate As Button
                btnUpdate = Me.ContentPlaceHolder1.FindControl("btn_Update")
                If btnUpdate IsNot Nothing Then btnUpdate.Visible = False
            Else
                Dim btnUpdate As Button
                btnUpdate = Me.ContentPlaceHolder1.FindControl("btn_Update")
                If btnUpdate IsNot Nothing Then btnUpdate.Visible = True
            End If

            Dim btnCancel As Button
            btnCancel = Me.ContentPlaceHolder1.FindControl("btn_Cancel")
            If btnCancel IsNot Nothing Then btnCancel.Visible = False
            'row_Menu.Visible = False
        End If
        If (Request.QueryString("HideMenu") & "") <> "HideMenu" Then
            SetMenu()
        End If

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
        If sqlConn.State = ConnectionState.Open Then
            sqlConn.Close()
            sqlConn = Nothing
        End If
    End Sub
    Private Sub NewDealGenerated()
        Try
            Dim dt As DataTable

            Dim strHtml As String = ""
            OpenConn()

            dt = objCommon.FillDataTable(sqlConn, "ID_CHECK_DealSlipGenerated", Session("UserId"), "UserId")
            If (Session("UserTypeId") = "1" Or Session("UserTypeId") = "41" Or Session("UserTypeId") = "40" Or Session("UserTypeId") = "42" Or Session("UserTypeId") = "43" Or Session("UserTypeId") = "44") Then
                If dt.Rows.Count > 0 Then
                    DivNewDealSlip.InnerHtml = ""
                    strHtml = ""
                    strHtml = "<table  cellpadding='0' height='20' align='left' cellspacing='0' border='0' width='100%' >"
                    strHtml += "<tr>"
                    With dt
                        strHtml += "<td colspan='2' height='20' valign='middle' align='left'>"

                        strHtml += "<font style='cursor:hand' id='fntDealSlip' ><b>"

                        If (Session("UserTypeId") = "40" Or Session("UserTypeId") = "42" Or Session("UserTypeId") = "43" Or Session("UserTypeId") = "44") Then
                            strHtml += "<a id='lnkDealSlip'  style='color:Red;height:12px;color:Black;font-size:small;' title='Click For All New DealSlips' >New DealSlip Generated</a></b></font> "
                        Else
                            strHtml += "<a id='lnkDealSlip' href='CheckerView.aspx'  style='color:Red;height:12px;color:Black;font-size:small;' title='Click For All New DealSlips' >New DealSlip Generated</a></b></font> "
                        End If
                        strHtml += "</td>"

                    End With
                    strHtml += "</tr>"
                    strHtml += "</table>"
                    DivNewDealSlip.InnerHtml = strHtml
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub NewDealChecked()
        Try
            Dim dt As DataTable
            Dim strHtml As String = ""
            OpenConn()

            dt = objCommon.FillDataTable(sqlConn, "ID_CHECK_DealSlipChecked", Session("UserId"), "UserId")

            If (Session("UserTypeId") = "1" Or Session("UserTypeId") = "41" Or Session("UserTypeId") = "40" Or Session("UserTypeId") = "42" Or Session("UserTypeId") = "43" Or Session("UserTypeId") = "44") Then
                If dt.Rows.Count > 0 Then
                    DivNewDealSlipChecked.InnerHtml = ""
                    strHtml = ""
                    strHtml = "<table  cellpadding='0' height='20' align='left' cellspacing='0' border='0' width='100%' >"
                    strHtml += "<tr>"
                    With dt
                        strHtml += "<td colspan='2' height='20' valign='middle' align='left'>"

                        strHtml += "<font style='cursor:hand' id='fntDealSlip' ><b>"

                        If (Session("UserTypeId") = "40" Or Session("UserTypeId") = "42" Or Session("UserTypeId") = "43" Or Session("UserTypeId") = "44") Or Session("UserTypeId") = "1" Then
                            strHtml += "<a id='lnkDealSlip' href='PendingDealSlip.aspx'  style='color:Red;height:12px;color:Black;font-size:small;' title='Click For All New DealSlips' >New DealSlip Checked</a></b></font> "
                        End If
                        strHtml += "</td>"

                    End With
                    strHtml += "</tr>"
                    strHtml += "</table>"
                    DivNewDealSlipChecked.InnerHtml = strHtml
                End If
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub NewCRMGenerated()
        Try
            Dim dt As DataTable
            Dim userid As Integer
            Dim strHtml As String = ""
            OpenConn()

            dt = objCommon.FillDataTable(sqlConn, "ID_CHECK_CRMGenerated", Session("UserId"), "UserId")
            If dt.Rows.Count > 0 Then
                DivCRMGenertd.InnerHtml = ""
                strHtml = ""
                strHtml = "<table  cellpadding='0' height='20' align='left' cellspacing='0' border='0' width='100%' >"
                strHtml += "<tr>"
                With dt
                    strHtml += "<td colspan='2' height='20' valign='middle' align='left'>"
                    strHtml += "<font style='cursor:hand' id='fntDealSlip' ><b>"
                    strHtml += "<a id='lnkDealSlip' href='InvestorInteractionDetailNew.aspx?CRM=NewEntry' style='color:Red;height:12px;color:Black;font-size:small;' title='Click For CRM Entry' >NEW CRM ENTRY FOUND</a></b></font> "
                    strHtml += "</td>"

                End With
                strHtml += "</tr>"
                strHtml += "</table>"
                DivCRMGenertd.InnerHtml = strHtml
                DivCRMGenertd.Visible = True
                'Hid_CRMMsg.Value = "NewEntry"

                'Session("CRMEntry") = ""
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub


    Private Sub ShowMessage()
        Try
            Dim sqlDa As New SqlDataAdapter
            Dim dt As New DataTable
            OpenConn()
            oSqlCmd = New SqlCommand("ID_FILL_MessageBoardMaster", sqlConn)
            With oSqlCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Clear()
                obj.SetCommandParameters(oSqlCmd, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("userid")))
                obj.SetCommandParameters(oSqlCmd, "@Ret_Code", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
                If .Parameters.Item("@Ret_Code").Value <= 0 Then
                    row_Msg.Visible = False
                End If
                sqlDa.SelectCommand = oSqlCmd
                sqlDa.Fill(dt)
            End With
            DivEvents.InnerHtml = ""
            strHtml = ""
            strHtml = "<table  cellpadding='0' height='20' align='left' cellspacing='0' border='0' width='100%' >"
            strHtml += "<tr>"
            strHtml += "<td colspan='2' height='20' valign='middle' align='left'>"
            strHtml += "<marquee id='m1'name='m1'   scrolldelay='20' "
            strHtml += "trueSpeed scrollAmount='1'  vspace='0' scrollDelay='0' direction='left'"
            strHtml += "loop='infinite' width='100%'>"
            For I As Int16 = 0 To dt.Rows.Count - 1
                strHtml += "<font  style='cursor:hand'><b"
                strHtml += " <td class = 'MarqueeCSS' title='Messages'> "
                strHtml += dt.Rows(I).Item("Message") & " "
                strHtml += "From" & " "
                strHtml += dt.Rows(I).Item("NameOfUser") & " "
                strHtml += " of Branch " & dt.Rows(I).Item("BranchName") & "</b></font>&nbsp;&nbsp;&nbsp;&nbsp;"
            Next
            strHtml += "</marquee></td>"
            strHtml += "</tr>"
            strHtml += "</table>"
            DivEvents.InnerHtml = strHtml

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
            'If oSqlDr.IsClosed = False Then oSqlDr.Close()
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

    Private Sub SetMenu()

        If Session("UserTypeId") = 1 Or Session("UserTypeId") = 107 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\Admin.xml"
        ElseIf Session("UserTypeId") = 39 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\DEALER.xml"
        ElseIf Session("UserTypeId") = 40 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\BACKOFFICE.xml"
        ElseIf Session("UserTypeId") = 41 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\DEALERBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 42 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\DEALERMANAGER.xml"
        ElseIf Session("UserTypeId") = 43 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\BACKOFFICEBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 44 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\BACKOFFICEMANAGER.xml"
        ElseIf Session("UserTypeId") = 63 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\COMPLIANCE.xml"

            'New
        ElseIf Session("UserTypeId") = 69 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\CHECKER.xml"

        ElseIf Session("UserTypeId") = 66 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\ADVANCEDEALER.xml"

        ElseIf Session("UserTypeId") = 70 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\MBDEALER.xml"
        ElseIf Session("UserTypeId") = 71 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\MBBACKOFFICE.xml"
        ElseIf Session("UserTypeId") = 72 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\MBDEALERBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 73 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\MBDEALERMANAGER.xml"
        ElseIf Session("UserTypeId") = 74 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\MBBACKOFFICEBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 75 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\MBBACKOFFICEMANAGER.xml"
        ElseIf Session("UserTypeId") = 76 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\MBCOMPLIANCE.xml"

        ElseIf Session("UserTypeId") = 77 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\MBADVANCEDEALER.xml"
        ElseIf Session("UserTypeId") = 78 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\MBCHECKER.xml"



        ElseIf Session("UserTypeId") = 79 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\WDMDEALER.xml"
        ElseIf Session("UserTypeId") = 80 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\WDMBACKOFFICE.xml"
        ElseIf Session("UserTypeId") = 81 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\WDMDEALERBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 82 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\WDMDEALERMANAGER.xml"
        ElseIf Session("UserTypeId") = 83 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\WDMBACKOFFICEBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 84 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\WDMBACKOFFICEMANAGER.xml"
        ElseIf Session("UserTypeId") = 85 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\WDMCOMPLIANCE.xml"

        ElseIf Session("UserTypeId") = 86 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\WDMADVANCEDEALER.xml"
        ElseIf Session("UserTypeId") = 87 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\WDMCOMPLIANCE.xml"



        ElseIf Session("UserTypeId") = 88 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\CRMDEALER.xml"
        ElseIf Session("UserTypeId") = 89 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\CRMBACKOFFICE.xml"
        ElseIf Session("UserTypeId") = 90 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\CRMDEALERBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 91 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\CRMDEALERMANAGER.xml"
        ElseIf Session("UserTypeId") = 92 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\CRMBACKOFFICEBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 93 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\CRMBACKOFFICEMANAGER.xml"
        ElseIf Session("UserTypeId") = 94 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\CRMCOMPLIANCE.xml"

        ElseIf Session("UserTypeId") = 95 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\CRMADVANCEDEALER.xml"
        ElseIf Session("UserTypeId") = 96 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\CRMCHECKER.xml"


        ElseIf Session("UserTypeId") = 97 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\PMSDEALER.xml"
        ElseIf Session("UserTypeId") = 98 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\PMSBACKOFFICE.xml"
        ElseIf Session("UserTypeId") = 99 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\PMSDEALERBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 100 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\PMSDEALERMANAGER.xml"
        ElseIf Session("UserTypeId") = 101 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\PMSBACKOFFICEBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 102 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\PMSBACKOFFICEMANAGER.xml"
        ElseIf Session("UserTypeId") = 103 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\PMSCOMPLIANCE.xml"

        ElseIf Session("UserTypeId") = 104 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\PMSADVANCEDEALER.xml"
        ElseIf Session("UserTypeId") = 105 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\PMSCHECKER.xml"
        ElseIf Session("UserTypeId") = 67 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\USER.xml"

        ElseIf Session("UserTypeId") = 106 Then
            WebMenu1.MenuXMLPath = Server.MapPath(".") & "\OTHER.xml"

        End If

    End Sub



End Class