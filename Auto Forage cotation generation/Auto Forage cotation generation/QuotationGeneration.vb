Imports Syncfusion.DocIO
Imports Syncfusion.DocIO.DLS
Imports Syncfusion.DocToPDFConverter
Imports Syncfusion.Pdf
Imports System.IO


Public Class QuotationGeneration
    Private table As WTable
    ' Private templateFilePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Template.docx")
    Private appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Auto Forage Quotation App")
    Private doc As WordDocument
    Private ReadOnly templateBytes As Byte() = My.Resources.quotationtemplate


    ' Constructor
    Public Sub New()
        If Not Directory.Exists(appDataPath) Then
            Directory.CreateDirectory(appDataPath)
        End If
        docinitialization()

        'load first found table
        LocateTable()

    End Sub
    Private Sub docinitialization()
        ' Load the Word document from the resource file
        Using stream As New MemoryStream(templateBytes)
            doc = New WordDocument(stream, FormatType.Docx)
        End Using
    End Sub

    Private Function LocateTable() As WTable
        For Each section As WSection In doc.Sections
            For Each table As WTable In section.Body.ChildEntities.OfType(Of WTable)()
                Return table ' Return the first table found
            Next
        Next
        Return Nothing ' Return nothing if no table found
    End Function

    ' Method to replace placeholders in the table rows and append to the description cell
    Public Sub AppendDataToTable(rowIndex As Integer, additionalDesc As String, quantity As String, unit As String, unitCost As String, totalCost As String)
        Dim table As WTable = LocateTable()
        If table IsNot Nothing AndAlso rowIndex >= 0 AndAlso rowIndex < table.Rows.Count Then
            Dim row As WTableRow = table.Rows(rowIndex)
            If row.Cells.Count >= 6 Then ' Assuming 6 cells: No, Description, Quantity, Unit, Unit Cost, Total
                ' Append additional description
                Dim descriptionCell As WTableCell = row.Cells(1) ' Assuming the description is in the second column
                If descriptionCell.Paragraphs.Count > 0 Then
                    Dim paragraph As WParagraph = descriptionCell.Paragraphs(0)
                    paragraph.AppendText(" " & additionalDesc) ' Append additional data
                End If

                ' Update other cells with quantities, units, and costs
                row.Cells(2).Paragraphs(0).Text = quantity ' Update quantity
                row.Cells(3).Paragraphs(0).Text = unit ' Update unit
                row.Cells(4).Paragraphs(0).Text = unitCost ' Update unit cost
                row.Cells(5).Paragraphs(0).Text = totalCost ' Update total cost
            End If
        Else
            Throw New IndexOutOfRangeException("Row index is out of range.")
        End If
    End Sub

    ' Method to replace client and project details
    Public Sub ReplaceClientAndProjectDetails(dateActuelle As String, referenceDevis As String, nomClient As String, adresseClient As String, telephoneClient As String, description As String, depth As String, lieu As String)
        doc.Replace("[Date actuelle]", dateActuelle, True, True)
        doc.Replace("[Numéro ou code de devis]", referenceDevis, True, True)
        doc.Replace("[Nom du client]", nomClient, True, True)
        doc.Replace("[Adresse du client]", adresseClient, True, True)
        doc.Replace("[Numéro de téléphone du client]", telephoneClient, True, True)
        doc.Replace("[description]", description, True, True)
        doc.Replace("[depth]", depth, True, True)
        doc.Replace("[lieu]", lieu, True, True)
    End Sub

    ' Method to replace totals and expiration details
    Public Sub ReplaceTotalsAndExpiration(total As String, expirationDate As String)
        doc.Replace("[Total général en chiffre]", total, True, True)
        doc.Replace("[date d’expiration]", expirationDate, True, True)
    End Sub

    ' Method to delete a specific row in the table
    Public Sub DeleteTableRow(tableIndex As Integer, rowIndex As Integer)
        Dim table As WTable = CType(doc.ChildEntities(tableIndex), WTable)
        If rowIndex < table.Rows.Count Then
            table.Rows.RemoveAt(rowIndex)
            ' Recalculate No column here if needed
            RecalculateNoColumn(table)
        End If
    End Sub

    ' Method to recalculate the "No" column after deleting a row
    Private Sub RecalculateNoColumn(table As WTable)
        For i As Integer = 0 To table.Rows.Count - 1
            table.Rows(i).Cells(0).Paragraphs(0).Text = (i + 1).ToString()
        Next
    End Sub

    ' Method to save the document as PDF
    Public Sub SaveDocumentAsPdf(outputPath As String)
        ' Load the saved document for conversion
        Using converter As New DocToPDFConverter()
            ' Convert Word document into PDF document
            Using pdfDocument As PdfDocument = converter.ConvertToPDF(doc)
                ' Save the PDF document
                Using fileStream As New FileStream(outputPath, FileMode.Create, FileAccess.Write)
                    pdfDocument.Save(fileStream)
                End Using
            End Using
        End Using

    End Sub

    ' Method to preview the document (for live preview implementation)
    Public Sub PreviewDocument(previewFileName As String)
        Dim tempDocPath As String = Path.Combine(appDataPath, previewFileName)
        Using tempStream As New MemoryStream()
            doc.Save(tempStream, FormatType.Docx)

            ' Save the document to a temporary location for preview
            File.WriteAllBytes(tempDocPath, tempStream.ToArray())
        End Using

        ' Code to preview in a WebView or other mechanism can go here
    End Sub
End Class

