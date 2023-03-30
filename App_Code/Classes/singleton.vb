Imports Microsoft.VisualBasic
Public Class Singleton
    Private Shared objexcel As Excel.Application
    Private Sub New()
        System.Console.WriteLine("Instance of Singleton class created at : " & Now())
    End Sub
    Public Shared Function GetInstance() As Excel.Application

        If objexcel Is Nothing Then
            objexcel = New Excel.Application

            If objexcel.Version = 15.0 Or objexcel.Version = 12.0 Then
                objexcel = CreateObject("Excel.Application")
            Else
                objexcel.RegisterXLL(objexcel.LibraryPath & "\Analysis\ANALYS32.XLL")
            End If
        End If

        Return objexcel
    End Function
End Class











