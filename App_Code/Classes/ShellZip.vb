Imports Microsoft.VisualBasic
Imports System.IO

Imports Shell32 'Reference Microsoft Shell Controls And Automation on the COM tab.

Public Class ShellZip
    Sub Compress(ByVal CompressedFileName As String, ByVal FileOrFolderToCompress As String)

        Dim B(21) As Byte

        B(0) = 80 : B(1) = 75 : B(2) = 5 : B(3) = 6

        File.WriteAllBytes(CompressedFileName, B) 'Make an empty PKZip file.

        Dim SH As New Shell

        Dim SF As Folder = SH.NameSpace(CompressedFileName)

        Dim DF As Folder = SH.NameSpace(FileOrFolderToCompress)

        SF.CopyHere(DF)
    End Sub

    Sub Expand(ByVal CompressedFileName As String, ByVal ExpandedFolder As String)
        Dim Sh As New Shell
        Dim SF As Folder = Sh.NameSpace(CompressedFileName)
        Dim DF As Folder = Sh.NameSpace(ExpandedFolder)

        For Each F As FolderItem In SF.Items
            DF.CopyHere(F)
        Next
    End Sub
End Class
