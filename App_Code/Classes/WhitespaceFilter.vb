Imports System.IO
Imports System.Text.RegularExpressions

' This filter gets rid of all unnecessary whitespace in the output.

Public Class WhitespaceFilter
    Inherits Stream

    Private _sink As Stream
    Private _position As Long

    Public Sub New(ByVal sink As Stream)
        _sink = sink
    End Sub 'New

#Region " Code that will most likely never change from filter to filter. "
    ' The following members of Stream must be overridden.
    Public Overrides ReadOnly Property CanRead() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property CanSeek() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property CanWrite() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides Property Position() As Long
        Get
            Return _position
        End Get
        Set(ByVal Value As Long)
            _position = Value
        End Set
    End Property

    Public Overrides Function Seek(ByVal offset As Long, ByVal direction As System.IO.SeekOrigin) As Long
        Return _sink.Seek(offset, direction)
    End Function 'Seek

    Public Overrides Sub SetLength(ByVal length As Long)
        _sink.SetLength(length)
    End Sub 'SetLength

    Public Overrides Sub Close()
        _sink.Close()
    End Sub 'Close

    Public Overrides Sub Flush()
        _sink.Flush()
    End Sub 'Flush

    Public Overrides Function Read(ByVal MyBuffer() As Byte, ByVal offset As Integer, ByVal count As Integer) As Integer
        _sink.Read(MyBuffer, offset, count)
    End Function

#End Region

    ' Write is the method that actually does the filtering.

    Public Overrides Sub Write(ByVal MyBuffer() As Byte, ByVal offset As Integer, ByVal count As Integer)
        Dim data(count) As Byte
        Buffer.BlockCopy(MyBuffer, offset, data, 0, count)

        ' Don't use ASCII encoding here.  The .NET IDE replaces some characters, such as &reg;
        ' with a UTF-8 entity.  If you use ASCII encoding, you'll get B. instead of the registered
        ' trademark symbol.
        Dim s As String = System.Text.Encoding.UTF8.GetString(data)

        ' Replace control characters with either spaces or nothing

        ' The funky semi-colon handling is there because of a JavaScript comment in a component.
        ' This way, we keep the carriage returns that actually matter.
        s = s.Replace(ControlChars.Cr, Chr(255)).Replace(ControlChars.Lf, "").Replace(ControlChars.Tab, "")
        s = s.Replace(";" & Chr(255), ";" & ControlChars.Cr)
        s = s.Replace(Chr(255), " ")

        ' Eliminate excess whitespace.
        Do
            s = s.Replace("  ", " ")
        Loop Until s.IndexOf("  ") = -1

        ' Eliminate known comments.

        ' We use three comments in our template.  These comments go on every single page on the site.
        ' Obviously, we can kill them when they are going out.  This way, the comments stay in for
        ' maintenance, but are trimmed before release.
        s = s.Replace("<!-- Page Content Goes Above Here -->", "")
        s = s.Replace("<!-- Page Content Goes Below Here -->", "")
        s = s.Replace("<!-- Do not get rid of this &nbsp; on data pages -->", "")

        ' Eliminate some additional whitespace we can kill

        ' For some reason, a single space gets emitted before each of our DOCTYPE directives.
        s = s.Replace(" <!DOCTYPE", "<!DOCTYPE")

        ' These are the most common excess whitespace items we can remove.
        s = s.Replace("<li> ", "<li>").Replace("</td> ", "</td>").Replace("</tr> ", "</tr>").Replace("</ul> ", "</ul>").Replace("</table> ", "</table>").Replace("</li> ", "</li>")
        s = s.Replace("<LI> ", "<LI>").Replace("</TD> ", "</TD>").Replace("</TR> ", "</TR>").Replace("</UL> ", "</UL>").Replace("</TABLE> ", "</TABLE>").Replace("</LI> ", "</LI>")
        s = s.Replace("<td> ", "<td>").Replace("<tr> ", "<tr>")
        s = s.Replace("<TD> ", "<TD>").Replace("<TR> ", "<TR>")
        s = s.Replace("<P> ", "<P>").Replace("<p> ", "<p>")
        s = s.Replace("</P> ", "</P>").Replace("</p> ", "</p>")
        s = s.Replace("style=""display:inline""> ", "style=""display:inline"">")
        s = s.Replace(" <H", "<H").Replace(" <h", "<h").Replace(" </H", "</H").Replace(" </h", "</h")
        s = s.Replace("<UL> ", "<UL>").Replace("<ul> ", "<ul>")
        s = s.Replace(" <TABLE", "<TABLE").Replace(" <table", "<table")
        s = s.Replace(" <li>", "<li>").Replace(" <LI>", "<LI>")
        s = s.Replace(" <br>", "<br>").Replace(" <BR>", "<BR>").Replace("<br> ", "<br>").Replace("<BR> ", "<BR>")
        s = s.Replace(" <ul>", "<ul>").Replace(" <UL>", "<UL>")

        ' Replace long tags with short ones
        s = s.Replace("<STRONG>", "<B>").Replace("<strong>", "<b>")
        s = s.Replace("</STRONG>", "</B>").Replace("</strong>", "</b>")

        ' Replace some HTML entities with true character codes
        s = s.Replace("&brkbar;", "|")
        s = s.Replace("&brvbar;", "|")
        s = s.Replace("&shy;", "-")
        s = s.Replace("&nbsp;", Chr(160))
        s = s.Replace("&lsquor;", "'")
        s = s.Replace("&ldquor;", """")
        s = s.Replace("&lsquo;", "'")
        s = s.Replace("&rsquor;", "'")
        s = s.Replace("&rsquo;", "'")
        s = s.Replace("&ldquo;", """")
        s = s.Replace("&rdquor;", """")
        s = s.Replace("&rdquo;", """")
        s = s.Replace("&ndash;", "-")
        s = s.Replace("&endash;", "-")

        ' If we don't do this, JavaScript horks on the site
        s = s.Replace("<!--", "<!--" & ControlChars.Cr)
        s = s.Replace("}", "}" & ControlChars.Cr)

        ' Last chance to eliminate excess whitespace
        Do
            s = s.Replace("  ", " ")
        Loop Until s.IndexOf("  ") = -1

        ' Finally, we spit out what we have done.
        Dim outdata() As Byte = System.Text.Encoding.UTF8.GetBytes(s)
        _sink.Write(outdata, 0, outdata.GetLength(0))

    End Sub 'Write 

End Class

