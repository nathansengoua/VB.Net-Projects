Imports MigraDoc.DocumentObjectModel.Tables
Imports Spire.Doc.Interface
Imports Spire.DocViewer.Forms
Imports Syncfusion.DocIO
Imports Syncfusion.DocIO.DLS
Imports Syncfusion.DocToPDFConverter
Imports Syncfusion.Pdf
Imports System.IO
Imports System.Text.RegularExpressions
Imports Humanizer
Imports Humanizer.Localisation.Formatters
Imports System.Globalization
Public Class QuotationGeneration
    Private table As WTable
    Private appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Auto Forage Quotation App")
    Private doc As WordDocument
    Private ReadOnly templateBytes As Byte() = My.Resources.template
    Private wordDocumentPath As String ' Path to the current working Word document in AppData

    ' Constructor
    Public Sub New()
        ' Register Syncfusion license
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzUxODE2NUAzMjM3MmUzMDJlMzBvdVhKZ3hvRjNZeCsvSnRmbnBRa1hQbzJaaFhWUFNKRmsrUEFvZDUwblBBPQ==;Mgo+DSMBaFt4QHFqUU5rXVNbdV5dVGpAd0N3RGlcdlR1fUUmHVdTRHRbQ11jTn9WdEdiWXtbdHE=;Mgo+DSMBMAY9C3t2UlhhQlVMfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hTX9TdkJiXH1ccXRRTmVb")

        ' Create the directory if it does not exist
        If Not Directory.Exists(appDataPath) Then
            Directory.CreateDirectory(appDataPath)
        End If

        ' Initialize the Word document
        docinitialization()

        ' Load the first found table
        LocateTable()
        ' Load the second found table
        LocateTotalsTable()

        ' Initialize path for the document to be saved in AppData
        wordDocumentPath = Path.Combine(appDataPath, "CurrentQuotation.docx")
        SaveCurrentDocument() ' Save the document on initialization
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

        ' Log an error message if no table is found
        LogError("1st table not found in the document.")
        Return Nothing ' Return nothing if no table found
    End Function

    ' Method to log error messages
    Private Sub LogError(message As String)
        ' Here you can write to a log file, console, or display a message box
        ' For demonstration, we will use a simple MessageBox
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    ' Method to save the current Word document to the AppData folder
    Public Sub SaveCurrentDocument()
        Using fileStream As New FileStream(wordDocumentPath, FileMode.Create, FileAccess.Write)
            doc.Save(fileStream, FormatType.Docx)
        End Using
    End Sub

    ' Method to append data to the table (updates the live document)
    Public Sub UpdateTableRow(rowIndex As Integer, no As String, desc As String, quantity As String, unit As String, unitCost As String, totalCost As String)
        UpdateTableCell(rowIndex, 0, no)
        UpdateTableCell(rowIndex, 1, desc)
        UpdateTableCell(rowIndex, 2, quantity)
        UpdateTableCell(rowIndex, 3, unit)
        UpdateTableCell(rowIndex, 4, unitCost)
        UpdateTableCell(rowIndex, 5, totalCost)
    End Sub


    Public Sub UpdateTableCell(rowIndex As Integer, columnIndex As Integer, value As String)
        Dim table As WTable = LocateTable()
        If table Is Nothing Then
            LogError("No table found in the document.")
            Return
        End If

        If rowIndex < 0 OrElse rowIndex >= table.Rows.Count Then
            LogError("Row index is out of range.")
            Return
        End If

        Dim row As WTableRow = table.Rows(rowIndex)
        If columnIndex < 0 OrElse columnIndex >= row.Cells.Count Then
            LogError("Column index is out of range.")
            Return
        End If

        Try
            ' Directly update the cell content
            Dim paragraph As WParagraph = row.Cells(columnIndex).Paragraphs(0)
            paragraph.Text = value
        Catch ex As Exception
            LogError("An error occurred while updating the table cell: " & ex.Message)
        End Try

        ' Save the updated document after every change
        SaveCurrentDocument()
    End Sub





    Private Sub ReplacePlaceholder(cell As WTableCell, actualData As String)
        If cell.Paragraphs.Count > 0 Then
            Dim paragraph As WParagraph = cell.Paragraphs(0)
            If paragraph.Text.Contains("[Qté]") Then
                paragraph.Text = paragraph.Text.Replace("[Qté]", actualData)
            ElseIf paragraph.Text.Contains("[Unité]") Then
                paragraph.Text = paragraph.Text.Replace("[Unité]", actualData)
            ElseIf paragraph.Text.Contains("[Prix par unité]") Then
                paragraph.Text = paragraph.Text.Replace("[Prix par unité]", actualData)
            ElseIf paragraph.Text.Contains("[Total]") Then
                paragraph.Text = paragraph.Text.Replace("[Total]", actualData)
            End If
        Else
            LogError("No paragraph found in the cell to replace placeholder.")
        End If
    End Sub

    ' For culture settings

    Public Sub InsertTotalInWords(total As Double)
        ' Convert the total amount to a string with commas (currency format)
        Dim totalInFigure As String = total.ToString("N0", CultureInfo.CurrentCulture) ' Example: 1,234,567

        ' Convert the total amount to words in French
        Dim totalInWords As String = CInt(Math.Round(total)).ToWords(New CultureInfo("fr-FR")) ' Example: "mille deux cent trente-quatre"

        ' Define the sentence with the total in figures and words in French
        Dim finalSentence As String = $"Arrêté la présente facture à la somme de : {totalInWords} CFA"

        Try
            ' Replace placeholders in the Word document
            doc.Replace("[Total général en chiffre]", totalInWords, True, True)
            'doc.Replace("[Total général en lettres]", totalInWords, True, True)
            SaveCurrentDocument()
        Catch ex As Exception
            LogError("An error occurred while inserting total in words: " & ex.Message)
        End Try
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

        ' Save the updated document after replacing placeholders
        SaveCurrentDocument()
    End Sub

    ' Method to preview the current document
    Public Sub PreviewDocument(docviewer As DocDocumentViewer)
        ' Use the saved document path for preview (you can load this into a WebView or similar)
        docviewer.LoadFromFile(wordDocumentPath)
        docviewer.ZoomMode = ZoomMode.FitWidth
        docviewer.ZoomTo(81)
        docviewer.ZoomMode = ZoomMode.FitPage
    End Sub


    ' Method to save the document as PDF when the user decides to export it
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

    'method for second table

    Public Sub UpdateTotalsTable(rowIndex As Integer, columnIndex As Integer, value As String)
        Dim table As WTable = LocateTotalsTable() ' Assuming a different LocateTable method for this specific table
        If table Is Nothing Then
            LogError("Totals table not found in the document.")
            Return
        End If

        If rowIndex < 0 OrElse rowIndex >= table.Rows.Count Then
            LogError("Row index is out of range.")
            Return
        End If

        Dim row As WTableRow = table.Rows(rowIndex)
        If columnIndex < 0 OrElse columnIndex >= row.Cells.Count Then
            LogError("Column index is out of range.")
            Return
        End If

        Try
            ' Directly update the cell content by replacing the placeholder
            Dim paragraph As WParagraph = row.Cells(columnIndex).Paragraphs(0)
            paragraph.Text = value
        Catch ex As Exception
            LogError("An error occurred while updating the totals table cell: " & ex.Message)
        End Try

        ' Save the updated document after every change
        SaveCurrentDocument()
    End Sub

    Private Function LocateTotalsTable() As WTable
        Dim tableCount As Integer = 0

        For Each section As WSection In doc.Sections
            For Each table As WTable In section.Body.ChildEntities.OfType(Of WTable)()
                tableCount += 1
                If tableCount = 2 Then ' Return the second table found
                    Return table
                End If
            Next
        Next

        ' Log an error message if the second table is not found
        LogError("2nd table not found in the document.")
        Return Nothing ' Return nothing if no second table is found
    End Function


End Class
