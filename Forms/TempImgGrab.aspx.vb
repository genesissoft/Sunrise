Imports System.Data.SqlClient
Imports System.Data
Partial Class Forms_TempImgGrab
    Inherits System.Web.UI.Page
    Dim objComm As New clsCommonFuns
    Dim sqlComm As New SqlCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            'objCommon.OpenConn()

            Dim sqlDa As New SqlDataAdapter
            Dim ds As New DataSet
            Dim dr As DataRow
            Dim arrContent As Byte()

            sqlComm.Connection = clsCommonFuns.sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.CommandText = "ID_FILL_SHOWIMAGE"
            sqlComm.Parameters.Clear()
            objComm.SetCommandParameters(sqlComm, "@CustImageId", SqlDbType.BigInt, 8, "I", , , Val(Request.QueryString("ID")))
            objComm.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(ds)
            dr = ds.Tables(0).Rows(0)

            arrContent = CType(dr.Item("FooterData"), Byte())
            Response.ContentType = dr.Item("FooterType").ToString()
            Response.OutputStream.Write(arrContent, 0, dr.Item("FooterLength"))
            Response.End()
        Catch ex As Exception



        End Try
    End Sub
     
End Class
