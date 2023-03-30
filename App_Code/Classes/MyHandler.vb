Imports Microsoft.VisualBasic
Imports iTextSharp.text.pdf
Imports iTextSharp.text

Public Class MyHandler
    Inherits iTextSharp.text.pdf.events.PdfPageEventForwarder
    Public Shared strHeaderPath As String
    Public Shared strFooterPath As String
    Public Shared LogoByte As Byte()
    Public Shared UserTypeId As Integer


    Public Overloads Overrides Sub onStartPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
        Dim ImgByte As Byte() = Nothing
        Dim strImagePath = ConfigurationManager.AppSettings("ImagePath") + "\\CompanyLogo.png"
        Dim png As iTextSharp.text.Image
        If Convert.ToString(HttpContext.Current.Session("LogoData")) <> "" Then
            ImgByte = CType(HttpContext.Current.Session("LogoData"), Byte())
            png = iTextSharp.text.Image.GetInstance(ImgByte)
        Else
            png = iTextSharp.text.Image.GetInstance(strImagePath)
        End If


        png.ScaleToFit(200.0F, 200.0F)
        png.SpacingBefore = 20.0F
        png.SpacingAfter = 50.0F
        'png.GetTop(50)
        png.GetBottom(80)
        png.Alignment = Element.ALIGN_RIGHT
        document.Add(png)
        document.Add(Chunk.NEWLINE)


    End Sub
    Public Overloads Overrides Sub onEndPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)


        Dim strFontFilePath_CalibriBold As String = Trim(ConfigurationManager.AppSettings("FontFilePath_CalibriBold") & "")
        Dim strFontFilePath_Calibri As String = Trim(ConfigurationManager.AppSettings("FontFilePath_Calibri") & "")
        Dim FontColour = New iTextSharp.text.Color(36, 64, 98)

        Dim CalibriCompany As BaseFont
        Dim CalibriCompanyBold As BaseFont
        Dim bf As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED)
        Dim cb As PdfContentByte = writer.DirectContent
        CalibriCompany = BaseFont.CreateFont(strFontFilePath_Calibri, BaseFont.CP1250, BaseFont.NOT_EMBEDDED)
        CalibriCompanyBold = BaseFont.CreateFont(strFontFilePath_CalibriBold, BaseFont.CP1250, BaseFont.NOT_EMBEDDED)

        cb.BeginText()
        cb.SetFontAndSize(CalibriCompanyBold, 12)
        cb.SetColorFill(FontColour)
        cb.SetTextMatrix(10, 30)
        cb.ShowText("SUNRISE GILTS & SECURITIES (P) LTD.")
        cb.EndText()

        cb.BeginText()
        cb.SetFontAndSize(CalibriCompany, 8)
        cb.SetColorFill(Color.BLACK)
        cb.SetTextMatrix(10, 20)
        cb.ShowText("Registered Office: 514,Pinnacle Business Park, Corporate Road, Prahlad Nagar,Ahmedabad - 380015. Gujarat")
        cb.EndText()

        'cb.BeginText()
        'cb.SetFontAndSize(CalibriCompany, 8)
        'cb.SetColorFill(Color.BLACK)
        'cb.SetTextMatrix(10, 20)
        'cb.ShowText("Registered Office : 317, Pratibha Plus, 3rd Floor, Narol Gam Char Rasta, N.H. No. 8, Narol Aslali Highway, Ahmedabad - 382 405")
        'cb.EndText()

        cb.BeginText()
        cb.SetFontAndSize(CalibriCompany, 8)
        cb.SetColorFill(Color.BLACK)
        cb.SetTextMatrix(10, 10)
        cb.ShowText("Phone: +91 79 4032 7414 / 15,  4896 6870 ( 5 Line), Mobile : +91 9898658238,Fax:+ 91 79 4030 3249.  CIN No. : U67100GJ2013PTC077167  | SEBI Reg. No. : INZ0025734 | BSE Membership No, 4071 | NSE Membership No. 90076 ")
        cb.EndText()

        'cb.BeginText()
        'cb.SetFontAndSize(CalibriCompany, 10)
        'cb.SetColorFill(Color.BLACK)
        'cb.SetTextMatrix(10, 10)
        'cb.ShowText(" CIN No. : U67100GJ2013PTC077167  | SEBI Reg. No. : INZ0025734 | BSE Membership No, 4071 | NSE Membership No. 90076")
        'cb.EndText()

        'cb.BeginText()
        'cb.SetFontAndSize(CalibriCompany, 10)
        'cb.SetColorFill(Color.BLACK)
        'cb.SetTextMatrix(10, 10)
        'cb.ShowText("Branches : Kolkata ,Mumbai and New Delhi")
        'cb.EndText()

    End Sub
End Class
