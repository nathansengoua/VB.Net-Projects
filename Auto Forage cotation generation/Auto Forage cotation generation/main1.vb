Imports System.Data.SQLite
Imports System.IO
Imports System.Web.UI.WebControls
Imports iTextSharp.text.pdf

Public Class main1

    Private db As New DbManagement
    Private quotationGen As QuotationGeneration
    Private projectID As Integer

    ' Constructor that accepts projectID and initializes quotationGen
    Public Sub New(Optional ByVal passedProjectID As Integer = 0)

        ' This call is required by the designer.

        InitializeComponent()

        ' Initialize quotationGen
        quotationGen = New QuotationGeneration()

        ' Store the passed projectID in the form's variable
        projectID = passedProjectID
    End Sub

    Private Sub main1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim projectData As DataTable = db.GetProjectById(projectID)

            If projectData.Rows.Count > 0 Then
                Dim row As DataRow = projectData.Rows(0)
                descriptionTextBox.Text = row("Description").ToString()
                DateTextBox.Text = row("QuotationDate").ToString()
                RefTextBox.Text = row("reference_code").ToString()
                clientnameTextBox.Text = row("name").ToString()
                clientaddressTextBox.Text = row("address").ToString()
                clientphonetextbox.Text = row("phone").ToString()
                depthEtextbox.Text = row("depthE").ToString()

                ' Extract value from text fields
                Dim name As String = clientnameTextBox.Text.ToString()
                Dim address As String = clientaddressTextBox.Text.ToString()
                Dim tel As String = clientphonetextbox.Text.ToString()
                Dim dates As String = DateTextBox.Text.ToString()
                Dim ref As String = RefTextBox.Text.ToString()
                Dim descrip As String = descriptionTextBox.Text.ToString()
                Dim loc As String = projectlocation.Text.ToString()
                Dim depth As String = depthEtextbox.Text.ToString()

                ' Replace client and project details in the document
                quotationGen.ReplaceClientAndProjectDetails(dates, ref, name, address, tel, descrip, "depth", loc)

            Else
                MessageBox.Show("No project found with the specified ID.")
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading project details: " & ex.Message)
        End Try
    End Sub

    Private Sub SaveToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem1.Click

    End Sub

    Private Sub LoadPdfDocument(quotationReference As String)
        ' Construct the output PDF path using the quotationReference
        Dim outputPdfPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Auto Forage Quotation App", quotationReference & ".pdf")

        WebBrowser1.Navigate(outputPdfPath)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            ' Code to execute when the CheckBox is checked

            quotationGen.AppendDataToTable(4, "1", "", "-", "-", "40000 XFA", "")
            MessageBox.Show("CheckBox is checked!")
            ' Add your operation here
        Else
            ' Code to execute when the CheckBox is unchecked
            MessageBox.Show("CheckBox is unchecked!")
            ' Add your operation here
        End If
    End Sub

    Private Sub EXPORTERToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EXPORTERToolStripMenuItem.Click
        Try

            Using saveFileDialog1 As New SaveFileDialog()
                saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf"
                saveFileDialog1.Title = "Save Quotation as PDF"
                If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                    ' Export the document as a PDF
                    quotationGen.SaveDocumentAsPdf(saveFileDialog1.FileName)
                    MessageBox.Show("Document saved successfully!")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub
End Class
