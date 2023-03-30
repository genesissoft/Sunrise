Imports Microsoft.VisualBasic

Imports System
Imports System.Web
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Namespace myApp.ns.pages
    Public Class pdfPage1
        Inherits iTextSharp.text.pdf.PdfPageEventHelper
        'I create a font object to use within my footer
        Protected ReadOnly Property footer() As Font
            Get
                ' create a basecolor to use for the footer font, if needed.
                'Dim grey As New ConsoleColor(128, 128, 128)
                Dim font__1 As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL)
                Return font__1
            End Get
        End Property
        'override the OnStartPage event handler to add our header
        Public Overrides Sub OnStartPage(ByVal writer As PdfWriter, ByVal doc As Document)
            'I use a PdfPtable with 1 column to position my header where I want it
            Dim headerTbl As New PdfPTable(1)

            'set the width of the table to be the same as the document
            headerTbl.TotalWidth = doc.PageSize.Width

            'I use an image logo in the header so I need to get an instance of the image to be able to insert it. I believe this is something you couldn't do with older versions of iTextSharp
            Dim logo As Image = Image.GetInstance("C:\INETPUB\WWWROOT\EINSTADEAL_SMC\eInstadeal_SMC" & "\Images\SMCLogo.gif")

            'I used a large version of the logo to maintain the quality when the size was reduced. I guess you could reduce the size manually and use a smaller version, but I used iTextSharp to reduce the scale. As you can see, I reduced it down to 7% of original size.
            logo.ScalePercent(7)

            ''create instance of a table cell to contain the logo
            Dim cell As New PdfPCell(logo)


            'align the logo to the right of the cell
            cell.HorizontalAlignment = Element.ALIGN_RIGHT

            'add a bit of padding to bring it away from the right edge
            cell.PaddingRight = 20

            'remove the border
            cell.Border = 0

            'Add the cell to the table
            headerTbl.AddCell(cell)

            'write the rows out to the PDF output stream. I use the height of the document to position the table. Positioning seems quite strange in iTextSharp and caused me the biggest headache.. It almost seems like it starts from the bottom of the page and works up to the top, so you may ned to play around with this.
            headerTbl.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height - 10), writer.DirectContent)
        End Sub

        'override the OnPageEnd event handler to add our footer
        Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal doc As Document)
            'I use a PdfPtable with 2 columns to position my footer where I want it
            Dim footerTbl As New PdfPTable(2)

            'set the width of the table to be the same as the document
            footerTbl.TotalWidth = doc.PageSize.Width

            'Center the table on the page
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER

            'Create a paragraph that contains the footer text
            Dim para As New Paragraph("Some footer text", footer)

            'add a carriage return
            para.Add(Environment.NewLine)
            para.Add("Some more footer text")

            'create a cell instance to hold the text
            Dim cell As New PdfPCell(para)

            'set cell border to 0
            cell.Border = 0

            'add some padding to bring away from the edge
            cell.PaddingLeft = 10

            'add cell to table
            footerTbl.AddCell(cell)

            'create new instance of Paragraph for 2nd cell text
            para = New Paragraph("Some text for the second cell", footer)

            'create new instance of cell to hold the text
            cell = New PdfPCell(para)

            'align the text to the right of the cell
            cell.HorizontalAlignment = Element.ALIGN_RIGHT
            'set border to 0
            cell.Border = 0

            ' add some padding to take away from the edge of the page
            cell.PaddingRight = 10

            'add the cell to the table
            footerTbl.AddCell(cell)

            'write the rows out to the PDF output stream.
            footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 10), writer.DirectContent)
        End Sub
    End Class
End Namespace

