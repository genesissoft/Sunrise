Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.IO
Imports ICSharpCode.SharpZipLib.Zip

Public Class ZipUnZipFolder
    Public Shared Sub ZipFiles(ByVal inputFolderPath As String, ByVal outputPathAndFile As String, ByVal password As String)
        Dim ar As ArrayList = GenerateFileList(inputFolderPath)
        ' generate file list 
        Dim TrimLength As Integer = (Directory.GetParent(inputFolderPath)).ToString().Length
        ' find number of chars to remove // from orginal file path 
        TrimLength += 1
        'remove '\' 
        Dim ostream As FileStream
        Dim obuffer As Byte()
        Dim outPath As String = outputPathAndFile
        'Dim outPath As String = (inputFolderPath & "\") + outputPathAndFile
        'Dim oZipStream As New ZipOutputStream(File.Create(outPath))
        Dim oZipStream As New ZipOutputStream(File.Create(outPath))
        ' create zip stream 
        If password IsNot Nothing AndAlso password <> [String].Empty Then
            oZipStream.Password = password
        End If
        oZipStream.SetLevel(0)
        'oZipStream.SetLevel(9)
        ' maximum compression 
        Dim oZipEntry As ZipEntry
        For Each Fil As String In ar
            ' for each file, generate a zipentry 
            oZipEntry = New ZipEntry(Fil.Remove(0, TrimLength))
            oZipStream.PutNextEntry(oZipEntry)

            If Not Fil.EndsWith("/") Then
                ' if a file ends with '/' its a directory 
                ostream = File.OpenRead(Fil)
                obuffer = New Byte(ostream.Length - 1) {}
                ostream.Read(obuffer, 0, obuffer.Length)
                oZipStream.Write(obuffer, 0, obuffer.Length)
            End If
        Next
        oZipStream.Finish()
        oZipStream.Close()
    End Sub


    Private Shared Function GenerateFileList(ByVal Dir As String) As ArrayList
        Dim fils As New ArrayList()
        Dim Empty As Boolean = True
        For Each file As String In Directory.GetFiles(Dir)
            ' add each file in directory 
            fils.Add(file)
            Empty = False
        Next

        If Empty Then
            If Directory.GetDirectories(Dir).Length = 0 Then
                ' if directory is completely empty, add it 
                fils.Add(Dir & "/")
            End If
        End If

        For Each dirs As String In Directory.GetDirectories(Dir)
            ' recursive 
            For Each obj As Object In GenerateFileList(dirs)
                fils.Add(obj)
            Next
        Next
        Return fils
        ' return file list 
    End Function

    Public Shared Sub CreateZip(ByVal args As String())
        ' all Files in the given Folder will be compressed 
        Dim aFilenames As String() = Directory.GetFiles(args(0))

        ' the name of the Zip File is the second Parameter passed in calling 
        Dim s As New ZipOutputStream(File.Create(args(1)))

        ' Set compression level: 0 [none] - 9 [highest] 
        s.SetLevel(5)

        For i As Integer = 0 To aFilenames.Length - 1
            Dim fs As FileStream = File.OpenRead(aFilenames(i))

            ' normally, the Buffer is allocated once, 
            ' here we do it once per File for clarity's sake 
            Dim buffer As Byte() = New Byte(fs.Length - 1) {}
            fs.Read(buffer, 0, buffer.Length)

            ' and now we write a ZipEntry & the Data 
            Dim entry As New ZipEntry(aFilenames(i))
            s.PutNextEntry(entry)
            s.Write(buffer, 0, buffer.Length)
            fs.Close()
        Next

        s.Finish()
        s.Close()
        's.Flush()
    End Sub
End Class